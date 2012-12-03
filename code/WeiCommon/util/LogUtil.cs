/*
**  File Name:		LogUtil.cs
**
**  Functional Description:
**
**      This class is used to log various thing using enterprise lib.
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
using System.Diagnostics;
using System.Threading;

using System.Reflection; 

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class       LogUtil
    {
        private static readonly object syncLock = new object();

        public static void logDebug(String message)
        {
            lock (syncLock)
            {
                log(message, TraceEventType.Verbose);
            }
        }

        public static void logInfo(String message)
        {
            lock (syncLock)
            {
                log(message, TraceEventType.Information);
            }
        }

        public static void logInfo(String message, Exception e)
        {
            lock (syncLock)
            {
                logInfo(message + ". ExceptionMessage:" + e.Message);
            }
        }

        public static void log(String message, Exception e)
        {
            lock (syncLock)
            {

                if (e is ThreadAbortException)
                {
                    message = "Thread aborted for Project: " + Thread.CurrentThread.Name;
                }
                else
                {
                    MethodBase method = e.TargetSite; 
                    Module module = method.Module;
                    ParameterInfo[] param = method.GetParameters();
                    StringBuilder strParam = new StringBuilder();
                    if (param.Length > 0)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            strParam.Append(param[i].ToString());
                            strParam.Append(",");
                        }
                    }
                    string msg = module.Name + "." + method.Name + "Parameter " + strParam.ToString() + " - " + e.Message + Environment.NewLine + "Stack Trace - " + e.StackTrace;

                    message = msg;
                }

                logError(message);
                log(e.StackTrace, TraceEventType.Error);
            }
        }

        public static void logError(String message)
        {
            lock (syncLock)
            {
                log(message, TraceEventType.Error);
            }
        }

        public static void logFatal(String message)
        {
            lock (syncLock)
            {
                log(message, TraceEventType.Critical);
            }
        }
        //public static void logFatal(Exception message)
        //{
        //    lock (syncLock)
        //    {
        //        log(message, TraceEventType.Critical);
        //    }
        //}
        private static void log(String message, TraceEventType severity)
        {

            LogEntry logEntry = new LogEntry();
            logEntry.Message = message;
            logEntry.Severity = severity;

            Logger.Write(logEntry);
        }


        //private static void log(Exception message, TraceEventType severity)
        //{

        //    LogEntry logEntry = new LogEntry();
        //    logEntry.Message = message.InnerException.ToString();
        //    logEntry.Severity = severity;

        //    Logger.Write(logEntry);
        //}
    }
}
