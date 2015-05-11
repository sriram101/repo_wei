/*
**  File Name:		Messages.aspx
**
**  Functional Description:
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	    DBUtils.cs
**  Creation Date:  01/09/2011
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
**                                                                           **
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

01/05/2011       RP        Initial Version
04/24/2011       RP        changed the namespace
05/01/2011       RP        introduced new parameters to public method GetMessages
                            to introduce custom paging.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.DBUtils
{
    public class DBUtil
    {
        private Database _weidb;
        
        public DBUtil([Dependency("WeiDB")] Database dbase)
        {
            _weidb = dbase;
        }


        public DataSet getMessages(DateTime fromDate, DateTime toDate, string status, bool hasCTC, bool hasErrors, string searchText, string sorting, int currentPage, int pageSize, out double totalRecords)
        {
            //, int currentPage, int pageSize, out int totalRecords
             string sStoreProcName = "Wei_GetMessages";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                _weidb.AddInParameter(dbCommand, "@StartDate", DbType.DateTime, fromDate);
                _weidb.AddInParameter(dbCommand, "@EndDate", DbType.DateTime, toDate);
                _weidb.AddInParameter(dbCommand, "@Status", DbType.String, status);
                _weidb.AddInParameter(dbCommand, "@HasCTC", DbType.Boolean, hasCTC);
                _weidb.AddInParameter(dbCommand, "@IsError", DbType.Boolean, hasErrors);
                _weidb.AddInParameter(dbCommand, "@SearchText", DbType.String, searchText);
                _weidb.AddInParameter(dbCommand, "@Sorting", DbType.String, sorting);
                _weidb.AddInParameter(dbCommand, "@CurrentPage", DbType.Int32, currentPage);
                _weidb.AddInParameter(dbCommand, "@PageSize", DbType.Int32, pageSize);
                _weidb.AddOutParameter(dbCommand, "@TotalRecords",DbType.Int32,4);

                DataSet _dDataSet =  _weidb.ExecuteDataSet(dbCommand);
                totalRecords = Convert.ToInt32(_weidb.GetParameterValue(dbCommand, "@TotalRecords"));

                return _dDataSet;
                
            
        }
        public DataSet MessageStatus()
        {

            try
            {
                string sStoreProcName = "Wei_GetMessageStatus";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (Exception)
            {
                throw; 
            }
            
        }
        public Boolean AddAuditForErrorProcess(Int32 requestID,String userID)
        {
            try
            {
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddAuditForErrorProcess");

                _weidb.AddInParameter(cmd, "@Requestid", DbType.Int32, requestID);
                _weidb.AddInParameter(cmd, "@CreatedOper", DbType.String, userID);

                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

            }
                
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public Boolean AddAuditForReview(Int32 requestID, String userID)
        {
            try
            {
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddAuditForReview");

                _weidb.AddInParameter(cmd, "@Requestid", DbType.Int32, requestID);
                _weidb.AddInParameter(cmd, "@CreatedOper", DbType.String, userID);

                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

            }

            catch (Exception)
            {
                throw;
            }
            return false;
        }
        public Boolean UpdateTranslations(Int32 iTranID, Int32 iRequestID, String sNewTrans, String sReviewMode, 
                String sReviewOper, bool bReviewed, String sApproveOper, bool bApproved)
        {
            try
            {
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_UpdateTranslations");

                _weidb.AddInParameter(cmd, "@ID", DbType.Int32, iTranID);
                _weidb.AddInParameter(cmd, "@Requestid", DbType.Int32, iRequestID);
                _weidb.AddInParameter(cmd, "@NewTrans", DbType.String, sNewTrans);
                _weidb.AddInParameter(cmd, "@ReviewMode", DbType.String, sReviewMode);
                _weidb.AddInParameter(cmd, "@ReviewOper", DbType.String, sReviewOper);
                _weidb.AddInParameter(cmd, "@Reviewed", DbType.Boolean, bReviewed);
                _weidb.AddInParameter(cmd, "@ApproveOper", DbType.String, sApproveOper);
                _weidb.AddInParameter(cmd, "@Approved", DbType.Boolean, bApproved);

                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

            }

                
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        public DataSet getAuditMessagesByRequest(Int32 requestID)
        {
            try
            {
                string sStoreProcName = "Wei_GetAuditMessagesByRequest";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                _weidb.AddInParameter(dbCommand, "@RequestID", DbType.Int32, requestID);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (Exception)
            {
                throw;
            }

        }
        public DataSet getTranslations(Int32 requestID)
        {
            try
            {
                string sStoreProcName = "Wei_GetTranslations";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                _weidb.AddInParameter(dbCommand, "@RequestsID", DbType.Int32, requestID);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}