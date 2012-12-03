/*
**  File Name:		AbstractWebUrlTranslator.cs
**
**  Functional Description:
**
**      This File is provides default funtionality for both google and bing translation. For that matter this abstract 
**      implementation could be used for any HTTP GET based translation
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

using System.IO;
using System.Net;
using System.Web;

//using Telavance;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{
    public abstract class AbstractWebUrlTranslator : ITranslationProvider
    {
        protected string key;
        protected Int32 maxLength;

        protected WebProxy proxy = null;

        /* batching translator
        public string translate(String message, string fromLanguage, string toLanguage)
        {
            //this piece of code is specifically not in a try block

            StringBuilder sb = new StringBuilder();

            int payLoadSize = getPayLoadSize(fromLanguage, toLanguage);

            message = message.Trim();

            while (message.Length > 0)
            {
                string output;
                Stream stream;
                WebRequest httpWebRequest;
                WebResponse response;
                StreamReader reader;

                string currrentMessage = message;

                if (payLoadSize == -1 || payLoadSize > message.Length)
                {
                    currrentMessage = message;
                    message = "";
                }
                else
                {
                    currrentMessage = message.Substring(0, payLoadSize);
                    message = message.Substring(payLoadSize);
                }

                currrentMessage = HttpUtility.UrlEncode(currrentMessage);

                string translateUri = getURL(currrentMessage, fromLanguage, toLanguage);

                httpWebRequest = (HttpWebRequest)WebRequest.Create(translateUri);


                httpWebRequest.Method = "GET";

                response = httpWebRequest.GetResponse();

                stream = response.GetResponseStream();
                reader = new StreamReader(stream);
                output = reader.ReadToEnd();

                LogUtil.logDebug("Raw translated response:" + output);
                output = processResponse(output, currrentMessage, fromLanguage, toLanguage);

                sb.Append(output);
            }

            return sb.ToString();


        }*/

        public string translate(String message, string fromLanguage, string toLanguage)
        {
            string output;
            Stream stream;
            WebRequest httpWebRequest;
            WebResponse response;
            StreamReader reader;


            string messageBody = getMessage(HttpUtility.UrlEncode(message), fromLanguage, toLanguage);
            //messageBody = HttpUtility.UrlEncode(messageBody);

            if (maxLength != -1 && messageBody.Length>maxLength)
            {
                throw new AbortTranslationException("Max allowed translation string size is " + maxLength + ". But the current message size is " + messageBody.Length);
            }

            string translateUri = getURL(message, fromLanguage, toLanguage);
            translateUri = translateUri + "?" + messageBody;

            httpWebRequest = (HttpWebRequest)WebRequest.Create(translateUri);
            if(proxy!=null)
                httpWebRequest.Proxy = proxy;



            httpWebRequest.Method = "GET";

            response = httpWebRequest.GetResponse();

            stream = response.GetResponseStream();
            reader = new StreamReader(stream);
            output = reader.ReadToEnd();

            LogUtil.logDebug("Raw translated response:" + output);
            output = processResponse(output, message, fromLanguage, toLanguage);

            return output;
        }

        public void initialize(string key, Int32 maxLength, WeiConfiguration config)
        {
            this.key = key;
            this.maxLength = maxLength;

            if (config.Proxy != null && config.Proxy.Uri != null && config.Proxy.Uri.Trim().Length>0)
            {
                proxy = new WebProxy();
                proxy.Address = new Uri(config.Proxy.Uri);
                proxy.BypassProxyOnLocal = Convert.ToBoolean(config.Proxy.BypassLocal);

                foreach (BypassConfigElement bypass in config.Proxy.Bypass)
                {
                    proxy.BypassArrayList.Add(bypass.Name);
                }

                if (config.Proxy.Credential.Count > 0)
                {
                    String username = null;
                    String password = null;

                    foreach (CredentialConfigElement cred in config.Proxy.Credential)
                    {
                        if (cred.Key.Equals("username"))
                            username = cred.Value;
                        if (cred.Key.Equals("password"))
                            password = cred.Value;
                    }
                    if (username != null && password != null)
                    {
                        ICredentials credential = new NetworkCredential(username, password);
                        proxy.Credentials = credential;
                    }
                    else
                    {
                        LogUtil.logInfo("Ignoring credential as either username/password is not configured. Username:" + username + " password:" + password);
                    }
                }
            }
        }

        public abstract string getURL(String message, string fromLanguage, string toLanguage);
        public abstract string getMessage(String message, string fromLanguage, string toLanguage);
        public abstract string processResponse(String output, String message, string fromLanguage, string toLanguage);
    }
}
