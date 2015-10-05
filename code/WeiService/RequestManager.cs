/*
**  File Name:		RequestManager.cs
**
**  Functional Description:
**
**      This class manages the lifecycle of the request. All the drivers call this 
**      manager to perform various activites on the request.
**	
**
**	Author:	Lakshman Ramakrishnan
**  Facility	    WEI
**  Creation Date:  12/30/2010
**
*******************************************************************************
**                                                                           **
**      COPYRIGHT                                                            **
**                                                                           **
** (C) Copyright 2010                                                        **
** Telavance, inc                                                            **
**                                                                           **
** This software is furnished under a license for use only on a single       **
** computer system and may be copied only with the inclusion of the  above   **
** copyright notice. This software or any other copies thereof, may not be   **
** provided or otherwise made available to any other person  except for use  **
** on such system and to one who agrees to these license terms. title and    **
** ownership of the software shall at all times remain in Telavance,inc      **
** Inc.                                                                      **
**                                                                           **
** The information in this software is subject to change without notice and  **
** should not be construed as a commitment by Telavance, Inc.	             **
**									     									 **
*******************************************************************************
 									    
*******************************************************************************
 		                    Maintenance History				    
-------------|----------|------------------------------------------------------
    Date     |	Person  |  Description of Modification			    
-------------|----------|------------------------------------------------------

12/30/2010       RL        Inital version
12/10/2012                 Code Changes for Review of Messages
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Telavance.AdvantageSuite.Wei.WeiTranslator;


namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class RequestManager
    {
        private readonly DBUtil _dbUtils;
        private IDictionary<String, IMessageParser> _parsers = new Dictionary<String, IMessageParser>();
        private IMessageParser _fallBackParser;
        private int _maxTranslateRetryCount;
        private int _waitTimeBetweenRetries = 10000; //10 sec
        bool _bSentForOFACCheck = false;
        private bool _brequiresReview;
        private string _searchString;
        private string _replaceString;

        private Translator translator;


        public RequestManager(DBUtil dbUtils)
        {
            this._dbUtils = dbUtils;

            WeiConfiguration weiConfig = (WeiConfiguration)ConfigurationManager.GetSection("Wei");
            _brequiresReview = weiConfig.requiresReview;
            _searchString = weiConfig.searchString;
            _replaceString = weiConfig.replaceString;

            int threadPoolSize = 0;
            try
            {
                threadPoolSize = Convert.ToInt32(weiConfig.ThreadPoolSize);
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot parse ThreadPoolSize attribute. ThreadPoolSize is configured as:" + weiConfig.ThreadPoolSize, e);
                throw new Exception("Invalid ThreadPoolSize attribute specified in the configuration file");
            }

            System.Threading.ThreadPool.SetMaxThreads(threadPoolSize, threadPoolSize);

            try
            {
                _maxTranslateRetryCount = Int32.Parse(weiConfig.TranslatorSetting.NoOfRetries);
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot parse NoOfRetries attribute. NoOfRetries is configured as:" + weiConfig.TranslatorSetting.NoOfRetries, e);
                throw new Exception("Invalid NoOfRetries attribute in the configuration file");
            }
            translator = new Translator(weiConfig);

            _fallBackParser = new DefaultParser(translator);
            _parsers.Add("SWIFT", new SwiftParser(translator, weiConfig.SwiftSetting));
            foreach (ParserConfigElement parserConfig in weiConfig.Parsers)
            {
                Type type;
                if (parserConfig.DllName != null && parserConfig.DllName.Trim().Length>0)
                    type = TypeUtil.getType(parserConfig.DllName, parserConfig.ClassName, typeof(IConfigurableMessageParser));
                else    
                    type = TypeUtil.getType(null, parserConfig.ClassName, typeof(IConfigurableMessageParser));
                IConfigurableMessageParser configurableParser = (IConfigurableMessageParser)Activator.CreateInstance(type);
                configurableParser.configure(translator, parserConfig.Param1, parserConfig.Param2, parserConfig.Param3, parserConfig.Param4, parserConfig.Param5);
                _parsers.Add(parserConfig.Name, configurableParser);
            }
        }

        public bool processMessageForOFACCheck(Request request, bool fromErrorQueue)
        {
            //AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Started processing the message");
            bool bLocked = false;
            int iStatus =0;
            bool bSuccess = false;
            Interface i = InterfaceManager.getInterface(request.InterfaceId);

            try
            {
                bLocked = _dbUtils.acquireLock(request.RequestId);
                if (bLocked)
                {
                    iStatus = _dbUtils.getStatusByRequest(request.RequestId);
                    if (iStatus == Convert.ToInt32(Status.Review))
                    {
                        bSuccess = i.Driver.sendForOfacCheck(request);
                        if (bSuccess)
                        {
                            request.Status = Status.SentForOfacCheck;
                            request.IsError = false;
                            _dbUtils.changeStatus(request);
                            AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Message sent for watchlist filtering check");
                        }
                    }
                }
                else
                {
                    LogUtil.logError("Cannot acquire lock for message :" + request.RequestId + ". Translation incomplete");
                    AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Abort processing. Cannot acquire lock");
                }

            }
            catch (Exception e)
            {
                LogUtil.log("Error while sending the message for OFAC check:" + request.RequestId, e);
                AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Error while sending the message for OFAC check");
            }
            finally
            {
                if (bLocked)
                {
                    _dbUtils.releaseLock(request.RequestId);
                }
            }
            return bSuccess;
        }
        public void process(Request request)
        {
            AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Started processing the message");
            bool bLocked = false;
            bool bSuccess = false;

            

            Interface i = InterfaceManager.getInterface(request.InterfaceId);

            try
            {
                bLocked = _dbUtils.acquireLock(request.RequestId);
                if (bLocked)
                {
                    //if (!_bSentForOFACCheck)
                    //{
                        bSuccess = translateRequest(request, i);
                    //If translation was success and the review flag is false
                    //if (bSuccess && (_brequiresReview == false) && (_bSentForOFACCheck == false))
                    //{
                    //    bSuccess = i.Driver.sendForOfacCheck(request);
                        if (!bSuccess )
                        //{
                        //    request.Status = Status.SentForOfacCheck;
                        //    request.IsError = false;
                        //    _dbUtils.changeStatus(request);
                        //    AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Translation review flag is not set. Message directly sent for watchlist filtering check");
                        //}
                        //else
                        {
                            request.Status = Status.Translated;
                            request.IsError = true;
                            _dbUtils.changeStatus(request);
                            LogUtil.logError("Error sending the message - " + request.RequestId + " for watchlist filtering check");
                            AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Error sending the message for watchlist filtering check");
                        }
                    //}
                }
               
                else
                {
                    LogUtil.logError("Cannot acquire lock for message :" + request.RequestId + ". Translation incomplete");
                    AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Abort processing. Cannot acquire lock");
                }

            }
            catch (Exception e)
            {
                LogUtil.log("Error processing the message:" + request.RequestId, e);
                AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Error processing the message");
            }
            finally
            {
                if (!bSuccess)
                {
                    _dbUtils.markRequestError(request.RequestId);
                    if(i.Driver.shouldMoveToError())
                    //send it to error 
                    i.Driver.moveToError(request);

                }
                if (bLocked)
                {
                    _dbUtils.releaseLock(request.RequestId);
                }
            }
        }

        private bool translateRequest(Request request, Interface i)
        {
            bool bTranslate = false;
            bool bSuccess = false;
            String message = "";
            try
            {
                AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Translation started for the message");
                translator.RequestId = request.RequestId;
                message = i.Handler.getMessageForTranslation(request.MessageBody);
                //if (_searchString != "")
                //{
                //    message = message.Replace(_searchString, " " + _replaceString + " ");
                //}
                IList<IMessageParser> currentParsers = new List<IMessageParser>();

                foreach (string fileFormat in i.FileFormats)
                {
                    if (_parsers.ContainsKey(fileFormat))
                    {
                        currentParsers.Add(_parsers[fileFormat]);
                    }
                }

                int counter = 0;
                bool bAbort = false;
                while (counter < _maxTranslateRetryCount && !bTranslate &&!bAbort)
                {
                    counter++;
                    bTranslate = false;
                    try
                    {
                        int currentParserCount = 0;

                        while (!bTranslate && currentParserCount < currentParsers.Count)
                        {
                            bTranslate = currentParsers[currentParserCount].translate(request, message);

                            if (bTranslate)
                            {
                                LogUtil.logInfo("Message:" + request.RequestId + " is processed by an instance of " + currentParsers[currentParserCount].GetType().FullName);
                            }
                            currentParserCount++;
                        }

                        if (!bTranslate)
                        {
                            bTranslate = _fallBackParser.translate(request, message);
                            if (bTranslate)
                            {
                                LogUtil.logInfo("Message:" + request.RequestId + " is processed by an instance of " + _fallBackParser.GetType().FullName);
                            }


                        }

                        if (!bTranslate)
                        {
                            //couldnt translate. Wait and retry
                            System.Threading.Thread.Sleep(_waitTimeBetweenRetries);
                        }
                        counter++;


                    }
                    catch (AbortTranslationException e)
                    {
                        bAbort = true;
                        LogUtil.log("Error translating the message:" + request.RequestId, e);
                    }
                    catch (Exception e)
                    {
                        LogUtil.log("Error translating the message:" + request.RequestId, e);
                    }
                }

                 if (bTranslate)
                {
                    request.Status = Status.Translated;
                    request.IsError = false;
                    i.Handler.getRepackagedTranslatedString(request, message);
                    
                    _dbUtils.addTranslatedMessage(request);
                    if(request.HasCTC)
                        AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Message has telegraphic codes");
                    else
                        AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Message doesn't have telegraphic codes");

                    AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Translation complete");
                    //if review of translations flag is set to false, move the message to OFAC Queue                    
                    if (_brequiresReview == false)
                    {
                        bSuccess = i.Driver.sendForOfacCheck(request);
                        if (bSuccess)
                        {
                            _bSentForOFACCheck = true;
                            request.Status = Status.SentForOfacCheck;
                            request.IsError = false;
                            _dbUtils.changeStatus(request);
                            AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Translation review flag is not set. Message directly sent for watchlist filtering check");
                        }
                    }
                    // if the message has set of CTC codes that have already been reviewed and approved, move the message
                    // directly for OFAC Check

                    else if (_dbUtils.getApprovedTranslationsByRequest(request.RequestId))
                    {
                        i.Driver.sendForOfacCheck(request);
                        //Set the boolean variable to true to indicate that the message has already been sent for OFAC check
                        _bSentForOFACCheck = true;
                        //change status and log and audit and return
                        request.Status = Status.SentForOfacCheck;
                        request.IsError = false;
                        _dbUtils.changeStatus(request);
                        AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Message sent for watchlist filtering check");
                    }
                    //If Requires Review flag is set to true and the message as CTC codes then move the message to Review Queue
                    else if (_brequiresReview && request.HasCTC)
                    {
                        if (i.Driver.shouldMoveToReview())
                        {
                            //send it to review Queue
                            i.Driver.moveToReview(request);
                            _bSentForOFACCheck = false;
                            //_bReview = true;
                            //change status and log and audit and return
                            request.Status = Status.Review;
                            request.IsError = false;
                            _dbUtils.changeStatus(request);
                            AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Info, "Message moved to Review Queue for verification of translation");
                        }
                    }

                }
                else
                {
                    //change status and log and audit and return
                    request.Status = Status.Unprocessed;
                    request.IsError = true;
                    _dbUtils.changeStatus(request);
                    LogUtil.logError("Cannot translate message:" + request.RequestId);
                    AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Error translating the message");
                }
            }
            catch (Exception e)
            {
                bTranslate = false;
                LogUtil.log("Error when translating message:" + request.RequestId, e);
                AuditUtil.getInstance().audit(request.RequestId, AuditLevel.Error, "Error translating the message");
            }

            return bTranslate;
        }

        public void processResponse(int requestId, int interfaceId, String identifier, String responseBody, OfacStatus ofacStatus)
        {
            AuditUtil.getInstance().audit(requestId, AuditLevel.Info, "Received " + ofacStatus.ToString() + " response");

            bool bLocked = false;
            bool bSuccess = false;
            Request request = null;
            Interface i = InterfaceManager.getInterface(interfaceId);
            try
            {

                bLocked = _dbUtils.acquireLock(requestId);
                if (bLocked)
                {
                    request = _dbUtils.getRequest(requestId);

                    if (request == null)
                    {
                        LogUtil.logError("Cannot retrieve the message:" + requestId);
                        return;
                    }

                    if (request.Status != Status.SentForOfacCheck)
                    {
                        LogUtil.logError("Cannot process response for the message :" + requestId + " . Current status is " + request.Status + ". Expecting " + Status.SentForOfacCheck);
                        return;
                    }

                    request.Status = Status.OfacResponseReceived;
                    request.IsError = false;
                    request.OfacStatus = ofacStatus;

                    _dbUtils.changeStatus(request);


                    request.ResponseMessage = i.Handler.getRepackagedResponseString(request.MessageBody, responseBody);

                    if (i.Driver.sendResponse(request, identifier))
                    {

                        request.Status = Status.Processed;
                        request.IsError = false;

                        _dbUtils.addResponseMessage(request);
                        bSuccess = true;
                        AuditUtil.getInstance().audit(requestId, AuditLevel.Info, "Sent response for the message ");
                    }


                }
            }
            catch (Exception e)
            {
                bSuccess = false;
                LogUtil.log("Cannot process response for the message :" + requestId, e);
                AuditUtil.getInstance().audit(requestId, AuditLevel.Error, "Error processing response for the message ");
            }
            finally
            {
                try
                {
                    if (!bSuccess)
                    {
                        _dbUtils.markRequestError(requestId);
                        //send it to error 
                        if (request != null && i.Driver.shouldMoveToError())
                            i.Driver.moveToError(request);
                    }
                    if (bLocked)
                    {
                        _dbUtils.releaseLock(request.RequestId);
                    }
                }
                catch (Exception e)
                {
                    LogUtil.log("Exception when trying to backout of another exception for message:" + requestId, e);
                }
            }
        }
    }
}
