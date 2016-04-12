using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;



namespace Telavance.AdvantageSuite.Wei.DataAccess
{
    public class DbUtil
    {
        private Database _weidb;
        
        public DbUtil([Dependency("WeiDB")] Database dbase)
        {
            _weidb = dbase;
        }

         public DataSet getMessages(DateTime fromDate, DateTime toDate, string status, bool hasCTC, bool hasErrors, string searchText, string sorting, int currentPage, int pageSize, out double totalRecords)
        {
            try
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
                _weidb.AddOutParameter(dbCommand, "@TotalRecords", DbType.Int32, 4);

                DataSet _dDataSet = _weidb.ExecuteDataSet(dbCommand);
                totalRecords = Convert.ToInt32(_weidb.GetParameterValue(dbCommand, "@TotalRecords"));

                return _dDataSet;

            }
            catch (SqlException ex)
            {
                LogUtil.logError("Error getting messages: " + ex.ToString());
                throw new Exception("Error getting messages: " , ex);
            }
        }
        public DataSet MessageStatus()
        {

            try
            {
                string sStoreProcName = "Wei_GetMessageStatus";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting message status: " + ex.ToString());
                throw new Exception("Error getting message status: " , ex);
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

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting audit information for message : " + requestID + " " + ex.ToString());
                throw new Exception("Error getting audit information for message : " + requestID + " " , ex);
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

            catch (SqlException ex)
            {
                LogUtil.logError("Error adding  audit information for message that was reviewed: " + requestID + " " + ex.ToString());
                throw new Exception("Error adding  audit information for message that was reviewed : " + requestID + " " , ex);
            }
            return false;
        }
        public Boolean UpdateTranslations(Int32 iTranID, Int32 iRequestID, String sNewTrans, String sReviewMode, 
                String sReviewOper, bool bReviewed, String sApproveOper, bool bApproved, string sCTCCode)
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
                _weidb.AddInParameter(cmd, "@CTCCode", DbType.String, sCTCCode);

                int retRowCount = _weidb.ExecuteNonQuery(cmd);
                if (retRowCount > 0)
                    return true;

            }


            catch (SqlException ex)
            {
                LogUtil.logError("Error updating translations for message : " + iRequestID + " " + ex.ToString());
                throw new Exception("Error updating translations for message : " + iRequestID + " " , ex);
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

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting audit information for message : " + requestID + " " + ex.ToString());
                throw new Exception("Error getting audit information for message : " + requestID + " " , ex);
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

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting translations for message : " + requestID + " " + ex.ToString());
                throw new Exception("Error getting translations for message : " + requestID + " " , ex);
            }
        }

        public DataSet getTranslationsByCTC(String sCTCCode)
        {
            try
            {
                string sStoreProcName = "WEI_GetTranslationByCTCCode";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                _weidb.AddInParameter(dbCommand, "@CTCCode", DbType.String, sCTCCode);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting translations for Telegraphic Codes : " + sCTCCode + " " + ex.ToString());
                throw new Exception("Error getting translations for Telegraphic Codes : " + sCTCCode + " " , ex);
            }
        }

        public DataSet getOriginalMessages(Int32 requestID)
        {
            try
            {
                string sStoreProcName = "Wei_GetOriginalMessages";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                _weidb.AddInParameter(dbCommand, "@RequestID", DbType.Int32, requestID);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (SqlException ex)
            {
                LogUtil.logError("Error getting original messages for request : " + requestID + " " + ex.ToString());
                throw new Exception("Error getting original messages for request : " + requestID + " " , ex);
            }
        }
    }


}
