/*
**  File Name:		MQDriver.cs
**
**  Functional Description:
**
**      This class is the implementor of IDriver interface and this class provides support 
**      for handling ofac requests through MQ queues. This class support Queues in local and remote machine.
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
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Principal;
using Telavance.AdvantageSuite.Wei.WeiService;
using Telavance.AdvantageSuite.Wei.WeiCommon;

using IBM.WMQ;


namespace Telavance.AdvantageSuite.Wei.MQDriver
{
    public delegate void HandleMessage(MQMessage message, OfacStatus status);

    class OfacResponse
    {
        public string message;
        public string name;
        public int requestId;
    }
    public class MQDriver :IDriver
    {
        private int _interfaceId;
        private RequestManager _manager;
        private Config config;
        private DBUtil _dbUtils;


        private MQQueueManager _mqManagerInQueue = null;
        private MQQueueManager _mqManagerOkQueue = null;
        private MQQueueManager _mqManagerConfirmQueue = null;
        private MQQueueManager _mqManagerErrorQueue = null;
        //Added a new Queue for Review Queue
        private MQQueueManager _mqManagerReviewQueue = null;

        private MQQueue _inQueue;
        private MQQueue _okQueue;
        private MQQueue _confirmQueue;
        private MQQueue _errorQueue;
        private MQQueue _reviewQueue;

        private MQQueueManager _mqManagerOfacInQueue = null;
        private MQQueueManager _mqManagerOfacOkQueue = null;
        private MQQueueManager _mqManagerOfacConfirmQueue = null;

        private MQQueue _ofacInQueue;
        private MQQueue _ofacOkQueue;
        private MQQueue _ofacConfirmQueue;

        private readonly object syncOfacInQueueLock = new object();
        private readonly object syncOkQueueLock = new object();
        private readonly object syncConfirmQueueLock = new object();
        private readonly object syncErrorQueueLock = new object();
        private readonly object syncReviewQueueLock = new object();


        private int maxNoOfMessagesToProcess;
        private int currentMessagesToBeProcessed = 0;

        private readonly object syncLock = new object();
        private DriverStatus status = DriverStatus.NotRunning;
        private Boolean pendingStop = false;
        private int totalMessagesProcessed = 0;
        private DateTime lastProcessedMessageTime = DateTime.MinValue;

        private Boolean isInputThreadRunning = false;
        private Boolean isthreadOkOfacRunning = false;
        private Boolean isthreadConfirmOfacRunning = false;
        

        public void initialize(int interfaceId, String configStr, RequestManager manager, DBUtil dbUtils)
        {
            _interfaceId = interfaceId;

            _manager = manager;
            _dbUtils = dbUtils;
            config = Config.getConfig(configStr);
        }
        public void start()
        {
            LogUtil.logInfo("config:" + config.ToString());

            if (!config.isValid())
                throw new Exception("Configuration for the driver is incomplete");
            lock (syncLock)
            {

                try
                {

                    ThreadPool.GetMaxThreads(out maxNoOfMessagesToProcess, out maxNoOfMessagesToProcess);

                    maxNoOfMessagesToProcess = maxNoOfMessagesToProcess * 2;

                    if (config.isRemote())
                    {
                        _mqManagerInQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerOkQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerConfirmQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerErrorQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerReviewQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerOfacInQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerOfacOkQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                        _mqManagerOfacConfirmQueue = new MQQueueManager(config.queueManager, config.channelName, config.connectionName);
                    }
                    else
                    {
                        _mqManagerInQueue = new MQQueueManager(config.queueManager);
                        _mqManagerOkQueue = new MQQueueManager(config.queueManager);
                        _mqManagerConfirmQueue = new MQQueueManager(config.queueManager);
                        _mqManagerErrorQueue = new MQQueueManager(config.queueManager);
                        _mqManagerReviewQueue = new MQQueueManager(config.queueManager);
                        _mqManagerOfacInQueue = new MQQueueManager(config.queueManager);
                        _mqManagerOfacOkQueue = new MQQueueManager(config.queueManager);
                        _mqManagerOfacConfirmQueue = new MQQueueManager(config.queueManager);
                    }
                    LogUtil.logInfo("Successfully connected to the queue manager:" + config.queueManager);

                    int openInputOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_INQUIRE;
                    int openOutputOptions = MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_OUTPUT | MQC.MQOO_INPUT_SHARED;
                    int openInputOutputOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_FAIL_IF_QUIESCING  |
                                                 MQC.MQOO_OUTPUT ;

                    _inQueue = _mqManagerInQueue.AccessQueue(config.inputQueue, openInputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.inputQueue);
                    _okQueue = _mqManagerOkQueue.AccessQueue(config.okQueue, openOutputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.okQueue);
                    _confirmQueue = _mqManagerConfirmQueue.AccessQueue(config.confirmQueue, openOutputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.confirmQueue);
                    _errorQueue = _mqManagerErrorQueue.AccessQueue(config.errorQueue, openInputOutputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.errorQueue);
                   // _reviewQueue = _mqManagerReviewQueue.AccessQueue(config.reviewQueue, openInputOptions);
                    _reviewQueue = _mqManagerReviewQueue.AccessQueue(config.reviewQueue, openInputOutputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.reviewQueue);
                    _ofacInQueue = _mqManagerOfacInQueue.AccessQueue(config.ofacInputQueue, openOutputOptions);
                    
                    LogUtil.logInfo("Successfully connected to the queue:" + config.ofacInputQueue);
                    _ofacOkQueue = _mqManagerOfacOkQueue.AccessQueue(config.ofacOkQueue, openInputOptions);
                    LogUtil.logInfo("Successfully connected to the queue:" + config.ofacOkQueue);
                    _ofacConfirmQueue = _mqManagerOfacConfirmQueue.AccessQueue(config.ofacConfirmQueue, openInputOptions);
                    
                    LogUtil.logInfo("Successfully connected to the queue:" + config.ofacConfirmQueue);

                    Thread threadInputQueue = new Thread(listenInputQueue);
                    Thread threadOfacOkQueue = new Thread(listenOfacOkQueue);
                    Thread threadOfacConfirmQueue = new Thread(listenOfacConfirmQueue);

                    threadInputQueue.Start();
                    threadOfacOkQueue.Start();
                    threadOfacConfirmQueue.Start();

                    status = DriverStatus.Running;
                }
                catch (Exception e)
                {
                    pendingStop = true;

                    if (_inQueue != null && _inQueue.OpenStatus)
                        _inQueue.Close();
                    if (_okQueue != null && _okQueue.OpenStatus)
                        _okQueue.Close();
                    if (_confirmQueue != null && _confirmQueue.OpenStatus)
                        _confirmQueue.Close();
                    if (_errorQueue != null && _errorQueue.OpenStatus)
                        _errorQueue.Close();
                    if (_reviewQueue != null && _reviewQueue.OpenStatus)
                        _reviewQueue.Close();

                    if (_ofacInQueue != null && _ofacInQueue.OpenStatus)
                        _ofacInQueue.Close();
                    if (_ofacOkQueue != null && _ofacOkQueue.OpenStatus)
                        _ofacOkQueue.Close();
                    if (_ofacConfirmQueue != null && _ofacConfirmQueue.OpenStatus)
                        _ofacConfirmQueue.Close();

                    if (_mqManagerInQueue != null && _mqManagerInQueue.OpenStatus)
                        _mqManagerInQueue.Close();
                    if (_mqManagerOkQueue != null && _mqManagerOkQueue.OpenStatus)
                        _mqManagerOkQueue.Close();
                    if (_mqManagerConfirmQueue != null && _mqManagerConfirmQueue.OpenStatus)
                        _mqManagerConfirmQueue.Close();
                    if (_mqManagerErrorQueue != null && _mqManagerErrorQueue.OpenStatus)
                        _mqManagerErrorQueue.Close();
                    if (_mqManagerReviewQueue != null && _mqManagerReviewQueue.OpenStatus)
                        _mqManagerReviewQueue.Close();

                    if (_mqManagerOfacInQueue != null && _mqManagerOfacInQueue.OpenStatus)
                        _mqManagerOfacInQueue.Close();
                    if (_mqManagerOfacOkQueue != null && _mqManagerOfacOkQueue.OpenStatus)
                        _mqManagerOfacOkQueue.Close();
                    if (_mqManagerOfacConfirmQueue != null && _mqManagerOfacConfirmQueue.OpenStatus)
                        _mqManagerOfacConfirmQueue.Close();

                    status = DriverStatus.NotRunning;
                    throw e;
                }
            }
        }

        public void stop()
        {
            if (status == DriverStatus.Running || status == DriverStatus.Error)
            {
                pendingStop = true;
                status = DriverStatus.Shuttingdown;
                Thread threadStop = new Thread(stopThread);
                threadStop.Start();
            }
        }

        public void stopThread()
        {
            for (int counter = 0; counter < 10; counter++)
            {
                lock (syncLock)
                {
                    if (currentMessagesToBeProcessed <= 0 && !isInputThreadRunning && !isthreadOkOfacRunning && !isthreadConfirmOfacRunning)
                    {
                        status = DriverStatus.NotRunning;
                        return;
                    }
                }

                Thread.Sleep(5000);
            }

            status = DriverStatus.Error;

        }

        public DriverStatus getStatus()
        {
            return status;
        }

        private MQMessage getMessage(MQQueue queue)
        {
            MQGetMessageOptions gmo = new MQGetMessageOptions();
            gmo.Options = MQC.MQGMO_FAIL_IF_QUIESCING | MQC.MQGMO_WAIT | MQC.MQGMO_SYNCPOINT;
            gmo.WaitInterval = 5000;
            MQMessage message = new MQMessage();

            try
            {
                //wait for message
                queue.Get(message, gmo); ;
            }
            catch (MQException ex)
            {
                message = null;
                if (ex.CompletionCode == MQC.MQCC_FAILED && ex.ReasonCode == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
            return message;
        }

        private MQMessage getMessagebyCorelationId(MQQueue queue, string correlationId)
        {
            MQGetMessageOptions gmo = new MQGetMessageOptions();
            gmo.Options = MQC.MQGMO_FAIL_IF_QUIESCING | MQC.MQGMO_WAIT | MQC.MQGMO_SYNCPOINT | MQC.MQMO_MATCH_CORREL_ID;
;
            gmo.WaitInterval = 5000;
            MQMessage message = new MQMessage();
            message.CorrelationId = System.Text.ASCIIEncoding.ASCII.GetBytes(correlationId);
            MQQueueManager manager = _mqManagerReviewQueue;
            try
            {
                //wait for message
                queue.Get(message, gmo); ;
                manager.Commit();
            }
            catch (MQException ex)
            {
                message = null;
                if (ex.CompletionCode == MQC.MQCC_FAILED && ex.ReasonCode == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
            return message;
        }

        private void listen(MQQueue queue, MQQueueManager manager, string queueName, HandleMessage handler, OfacStatus status)
        {            
            while (!pendingStop)
            {
                while (!pendingStop && currentMessagesToBeProcessed >= maxNoOfMessagesToProcess)
                {
                    Thread.Sleep(5000);
                }

                if (pendingStop)
                    return;
                try
                {
                    MQMessage message = getMessage(queue);
                    if (message == null)
                        continue;

                    handler(message, status);

                    manager.Commit();
                }
                catch (MQException e)
                {
                    LogUtil.log("Error when getting message from the " +queueName+" queue", e);
                    manager.Backout();
                    Thread.Sleep(30000);
                    throw;
                }
            }


        }

        private void listenInputQueue()
        {
            isInputThreadRunning = true;
            listen(_inQueue, _mqManagerInQueue, config.inputQueue, new HandleMessage(handleInputMessage), OfacStatus.None);
            LogUtil.logInfo("Shutting input queue thread for interface:" + _interfaceId);
            isInputThreadRunning = false;
        }

        private void handleInputMessage(MQMessage message, OfacStatus status)
        {
            Request request = new Request();
            request.Name = System.Text.ASCIIEncoding.ASCII.GetString(message.MessageId);
            request.MessageBody = message.ReadString(message.DataLength);
            
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null) request.CreateOper = windowsIdentity.Name;
            RequestHeader header = new RequestHeader();
            
            
            header.correlationId = System.Text.ASCIIEncoding.ASCII.GetString(message.CorrelationId);
            

            request.Header = header.ToString();
            request.InterfaceId = _interfaceId;
            _dbUtils.addRequest(request);
            //queue the work with the thread pool

            lock (syncLock)
            {
                currentMessagesToBeProcessed++;
            }
            

            ThreadPool.QueueUserWorkItem(new WaitCallback(startTranslation), request);
        }

        public void startTranslation(object request)
        {


            _manager.process((Request)request);
            updateStatistics();
            lock (syncLock)
            {
                currentMessagesToBeProcessed--;
            }
        }

        private void updateStatistics()
        {
            lock (syncLock)
            {
                totalMessagesProcessed++;
                lastProcessedMessageTime = DateTime.Now;
            }
        }

        private void handleResponseMessage(MQMessage message, OfacStatus status)
        {
            try
            {
                String requestId = System.Text.ASCIIEncoding.ASCII.GetString(message.CorrelationId);
                String name = System.Text.ASCIIEncoding.ASCII.GetString(message.MessageId);
                String messageText = message.ReadString(message.DataLength);

            
                OfacResponse response = new OfacResponse();
                response.message = messageText;
                response.requestId = Convert.ToInt32(requestId);
                response.name = name;
                

                _dbUtils.addOfacResponse(response.requestId, name, messageText, status);

                lock (syncLock)
                {
                    currentMessagesToBeProcessed++;
                }
                
                //queue the work with the thread pool
                if (status == OfacStatus.Confirm)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(startConfirmResponse), response);
                else
                    ThreadPool.QueueUserWorkItem(new WaitCallback(startOkResponse), response);
                
            }
            catch (MQException e)
            {
                LogUtil.log("Error while adding response after watchlist filtering check:", e);
            }
        }

        public void startOkResponse(object objMessage)
        {
            startResponse(objMessage, OfacStatus.OK);
        }

        public void startConfirmResponse(object objMessage)
        {
            startResponse(objMessage, OfacStatus.Confirm);
        }

        public void startResponse(object objMessage, OfacStatus ofacStatus)
        {
            try
            {
                OfacResponse response = (OfacResponse)objMessage;
                
                _manager.processResponse(response.requestId, _interfaceId, response.name, response.message, ofacStatus);
                updateStatistics();
                lock (syncLock)
                {
                    currentMessagesToBeProcessed--;
                }
                
            }
            catch (Exception e)
            {
                LogUtil.log("startReponse", e);
            }
        }

        private void listenOfacOkQueue()
        {
            isthreadOkOfacRunning = true;
            listen(_ofacOkQueue, _mqManagerOfacOkQueue , config.ofacOkQueue, new HandleMessage(handleResponseMessage), OfacStatus.OK);
            LogUtil.logInfo("Shutting ofacOk queue thread for interface:" + _interfaceId);
            isthreadOkOfacRunning = false;
        }

        private void listenOfacConfirmQueue()
        {
            try
            {
                isthreadConfirmOfacRunning = true;
                listen(_ofacConfirmQueue, _mqManagerOfacConfirmQueue, config.ofacConfirmQueue, new HandleMessage(handleResponseMessage), OfacStatus.Confirm);
                LogUtil.logInfo("Shutting ofacConfirm queue thread for interface:" + _interfaceId);
                isthreadConfirmOfacRunning = false;
            }
            catch (Exception e)
            {
                LogUtil.log("Error in listening to OFAC Confirm Queue:" , e);
            }
        }

        public bool sendForOfacCheck(Request request)
        {
            if (request.Status == Status.Review)
            {
                getMessagebyCorelationId(_reviewQueue, request.RequestId.ToString());
                
            }
            
            return sendMessage(config.ofacInputQueue, request.RequestId, _ofacInQueue, _mqManagerOfacInQueue, 
                    request.Name, request.TranslatedMessage, Convert.ToString(request.RequestId), syncOfacInQueueLock);
            
        }
        public bool sendResponse(Request request, string ofacResponseIdentifier)
        {
            MQQueue queue = _okQueue;
            Object lockObject = syncOkQueueLock;
            string queueName = config.okQueue;
            MQQueueManager manager = _mqManagerOkQueue;
            

            if (request.OfacStatus == OfacStatus.Confirm)
            {
                queue = _confirmQueue;
                lockObject = syncConfirmQueueLock;
                queueName = config.confirmQueue;
                manager = _mqManagerConfirmQueue;
            }
            
            RequestHeader header = RequestHeader.getRequestHeader(request.Header);
            
            
            return sendMessage(queueName, request.RequestId, queue, manager, request.Name, request.ResponseMessage, request.RequestId.ToString(), lockObject);

        }
        public bool moveToError(Request request)
        {
            RequestHeader header = RequestHeader.getRequestHeader(request.Header);
            return sendMessage(config.errorQueue, request.RequestId, _errorQueue, _mqManagerErrorQueue, request.Name, request.MessageBody, request.RequestId.ToString(), syncErrorQueueLock);

        }

        public bool moveToReview(Request request)
        {
            RequestHeader header = RequestHeader.getRequestHeader(request.Header);
            header = RequestHeader.getRequestHeader(request.Header);
            
            return sendMessage(config.reviewQueue, request.RequestId, _reviewQueue, _mqManagerReviewQueue, request.Name, request.MessageBody, request.RequestId.ToString(), syncReviewQueueLock);
            
            
        }

        
        private bool sendMessage(string queueName, int requestId, MQQueue queue, MQQueueManager manager, string messageId, string message, string correlationId, Object lockObject)
        {
            lock (lockObject)
            {
                bool sentMessage = false;
                try
                {
                 

                    MQPutMessageOptions pmo = new MQPutMessageOptions();
                    pmo.Options = MQC.MQOO_INQUIRE| MQC.MQPMO_FAIL_IF_QUIESCING | MQC.MQPMO_SYNCPOINT;
                    MQMessage mqMessage = new MQMessage();
                    mqMessage.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(message));
                    mqMessage.Format = MQC.MQFMT_STRING;
                    mqMessage.MessageId = System.Text.ASCIIEncoding.ASCII.GetBytes(messageId);
                    mqMessage.CorrelationId = System.Text.ASCIIEncoding.ASCII.GetBytes(correlationId);
                    queue.Put(mqMessage, pmo);
                    

                    manager.Commit();
                    sentMessage = true;
                    LogUtil.logInfo("Sent message " +requestId+" to " + queueName);
                    LogUtil.logInfo("End SendMesage:");
                }
                catch (MQException e)
                {
                    sentMessage = false;
                    manager.Backout();
                    LogUtil.log("Error sending the message to  " + queue.Name + " queue", e);
                }
                return sentMessage;
            }
        }

        public string getStatistics()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" Status:");
            builder.Append(status.ToString());
            builder.Append("\n Total messages processed:");
            builder.Append(totalMessagesProcessed);
            builder.Append("\n Last processed message time:");
            if(lastProcessedMessageTime == DateTime.MinValue)
                builder.Append("None");
            else
                builder.Append(lastProcessedMessageTime);
            builder.Append("\n current messages in process:");
            builder.Append(currentMessagesToBeProcessed);

            return builder.ToString();
        }

        public bool shouldMoveToError()
        {
            return config.shouldMoveToError;
        }
        public bool shouldMoveToReview()
        {
            return config.shouldMoveToReview;
        }
    }
}
