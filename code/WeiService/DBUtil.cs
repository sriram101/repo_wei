/*
**  File Name:		DBUtil.cs
**
**  Functional Description:
**
**      All DB related functions are in this util class. This class is instanciated by unity service resolver and 
**      the db information will be injected
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

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class DBUtil
    {
        private Database _weidb;
        private int _maxLockRetryCount = 10;
        private int _waitTimeBetweenRetries = 10000; //10 sec

        public DBUtil(/*[Dependency("WeiDB")]*/Database db)
        {
            _weidb = db;
        }

        public void addRequest(Request request)
        {

            request.Status = Status.Unprocessed;
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddRequest");

            _weidb.AddInParameter(cmd, "@name", DbType.String, request.Name);
            _weidb.AddInParameter(cmd, "@messagebody", DbType.String, request.MessageBody);
            _weidb.AddInParameter(cmd, "@interfaceid", DbType.Int32, request.InterfaceId);
            _weidb.AddInParameter(cmd, "@requestheader", DbType.String, request.Header);
            _weidb.AddInParameter(cmd, "@status", DbType.Int32, request.Status);
            _weidb.AddInParameter(cmd, "@iserror", DbType.Int32, request.IsError);
            decimal requestId = (decimal)_weidb.ExecuteScalar(cmd);

            request.RequestId = (int)requestId;

            LogUtil.logInfo("Created new request");
            AuditUtil.getInstance().audit((int)requestId, AuditLevel.Info, "Created the request");

        }

        public Boolean acquireLock(int requestId)
        {
            int count = 0;
            while (count < _maxLockRetryCount)
            {
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetLock");

                _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

                System.Threading.Thread.Sleep(_waitTimeBetweenRetries);
                count++;
            }
            return false;
        }

        public Boolean releaseLock(int requestId)
        {
            int count = 0;
            while (count < _maxLockRetryCount)
            {
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_ReleaseLock");

                _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

                System.Threading.Thread.Sleep(_waitTimeBetweenRetries);
            }
            return false;
        }

        public void changeStatus(Request request)
        {
            changeStatus(request.RequestId, request.Status, request.IsError, request.OfacStatus);
        }

        public void changeStatus(int requestId, Status status, bool isError, OfacStatus ofacStatus)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_ChangeStatus");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
            _weidb.AddInParameter(cmd, "@status", DbType.Int32, status);
            _weidb.AddInParameter(cmd, "@iserror", DbType.Int32, isError);
            _weidb.AddInParameter(cmd, "@ofacStatus", DbType.Int32, ofacStatus);

            _weidb.ExecuteScalar(cmd);
        }

        public void addTranslatedMessage(Request request)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddTranslatedMessage");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, request.RequestId);
            _weidb.AddInParameter(cmd, "@status", DbType.Int32, request.Status);
            _weidb.AddInParameter(cmd, "@translatedmessage", DbType.String, request.TranslatedMessage);
            _weidb.AddInParameter(cmd, "@iserror", DbType.Int32, request.IsError);
            _weidb.AddInParameter(cmd, "@hasCtc", DbType.Int32, request.HasCTC?1:0);
            
            _weidb.ExecuteScalar(cmd);
        }

        public Request getRequest(int requestId)
        {
            
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetRequest");
            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);

            using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
            {
                if (dataReader.Read())
                {
                    Request request = new Request();
                    request.RequestId = requestId;
                    request.InterfaceId = Int32.Parse(dataReader["interfaceid"].ToString());
                    request.Name = dataReader["name"].ToString();
                    request.MessageBody = dataReader["messagebody"].ToString();
                    request.TranslatedMessage = dataReader["translatedmessage"].ToString();
                    request.ResponseMessage = dataReader["responsemessage"].ToString();
                    request.IsError = Convert.ToBoolean(dataReader["iserror"].ToString());
                    request.Status = (Status)Enum.Parse(typeof(Status), dataReader["status"].ToString());
                    request.OfacStatus = (OfacStatus)Enum.Parse(typeof(OfacStatus), dataReader["ofacstatus"].ToString());
                    request.Header = dataReader["requestheaders"].ToString();
                    return request;
                }
            }

            return null;
        }

        public void addOfacResponse(int requestId, String identifier, String responseBody, OfacStatus ofacStatus)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddOfacResponse");
            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
            _weidb.AddInParameter(cmd, "@responsebody", DbType.String, responseBody);
            _weidb.AddInParameter(cmd, "@identifier", DbType.String, identifier);

            _weidb.ExecuteScalar(cmd);


        }

        public void addResponseMessage(Request request)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddResponseMessage");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, request.RequestId);
            _weidb.AddInParameter(cmd, "@status", DbType.Int32, request.Status);
            _weidb.AddInParameter(cmd, "@responsemessage", DbType.String, request.ResponseMessage);
            _weidb.AddInParameter(cmd, "@iserror", DbType.Int32, request.IsError);

            _weidb.ExecuteScalar(cmd);
        }

        public void markRequestError(int requestId)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_MarkRequestError");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
 
            _weidb.ExecuteScalar(cmd);
        }

        public List<Request> getRequestWithErrors(int interfaceId)
        {

            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetRequestWithError");
            _weidb.AddInParameter(cmd, "@interfaceId", DbType.Int32, interfaceId);

            List<Request> retValue = new List<Request>();

            using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
            {
                while (dataReader.Read())
                {
                    Request request = new Request();
                    request.RequestId = Int32.Parse(dataReader["id"].ToString()); ;
                    request.InterfaceId = Int32.Parse(dataReader["interfaceid"].ToString());
                    request.Name = dataReader["name"].ToString();
                    request.HasCTC = Convert.ToBoolean(dataReader["hasCTC"].ToString());
                    request.IsError = Convert.ToBoolean(dataReader["iserror"].ToString());
                    request.Status = (Status)Enum.Parse(typeof(Status), dataReader["status"].ToString());
                    request.OfacStatus = (OfacStatus)Enum.Parse(typeof(OfacStatus), dataReader["ofacstatus"].ToString());
                    request.Header = dataReader["requestheaders"].ToString();
                    request.CreateDate = Convert.ToDateTime(dataReader["createddatetime"].ToString());
                    request.ModifiedDate = Convert.ToDateTime(dataReader["ModifiedDatetime"].ToString());
                    request.MessageBody = dataReader["messagebody"].ToString();
                    request.TranslatedMessage = dataReader["translatedmessage"].ToString();
                    request.ResponseMessage = dataReader["responsemessage"].ToString();
                    
                    retValue.Add(request);
                }
            }

            return retValue;
        }

        public List<AuditMessages> getAuditMessages(int requestId)
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetAuditMessages");
            _weidb.AddInParameter(cmd, "@requestId", DbType.Int32, requestId);

            List<AuditMessages> retValue = new List<AuditMessages>();

            using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
            {
                int idxCreatedDate = dataReader.GetOrdinal("createddatetime");
                while (dataReader.Read())
                {
                    AuditMessages auditMessages = new AuditMessages();
                    auditMessages.auditLevel = (AuditLevel)Enum.Parse(typeof(Status), dataReader["level"].ToString());;
                    auditMessages.status = (Status)Enum.Parse(typeof(Status), dataReader["status"].ToString());
                    auditMessages.createdDate = dataReader.GetDateTime(idxCreatedDate);
                    auditMessages.message = dataReader["message"].ToString();
                    retValue.Add(auditMessages);
                }
            }

            return retValue;
        }
        public List<Interfaces> getInterfaces()
        {
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetInterfaces");
            List<Interfaces> retValue = new List<Interfaces>();
            using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
            {

                while (dataReader.Read())
                {
                    Interfaces interfaces = new Interfaces();
                    interfaces.interfaceID = Int32.Parse(dataReader["ID"].ToString());
                    interfaces.interfaceName = dataReader["Name"].ToString();

                    retValue.Add(interfaces);
                }
            }

            return retValue;

        }
    }
}
