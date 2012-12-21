using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.DataSources
{
    public class cDS_Messages_List
    {

        private int _mintID;
        private string _mstrStatus;
        private string _mstrOFACViolation;
        private DateTime _mdtCreateDate;
        private DateTime _mdtModifiedDate;

        private string _mstrOriginalMessage;
        private string _mstrTransilatedMessage;
        private string _mstrModifiedMessage;

        public cDS_Messages_List(int intPOID, string strStatus, string strstrOFACViolation, DateTime dtCreateDate, DateTime dtModifiedDate,
                                    object objOriginalMessage, object objTransilatedMessage, object objModifiedMessage)
        {
            _mintID = intPOID;
            _mstrStatus = strStatus;
            _mstrOFACViolation = strstrOFACViolation;
            _mdtCreateDate = dtCreateDate;
            _mdtModifiedDate = dtModifiedDate;
            _mstrOriginalMessage = (objOriginalMessage == System.DBNull.Value ? "" : (string)objOriginalMessage);
            _mstrTransilatedMessage = (objTransilatedMessage == System.DBNull.Value ? "" : (string)objTransilatedMessage);
            _mstrModifiedMessage = (objModifiedMessage == System.DBNull.Value ? "" : (string)objModifiedMessage);
        }

        public int ID
        {
            get { return _mintID; }
            set { _mintID = value; }
        }

        public string Status
        {
            get { return _mstrStatus; }
            set { _mstrStatus = value; }
        }

        public string OFACViolation
        {
            get { return _mstrOFACViolation; }
            set { _mstrOFACViolation = value; }
        }

        public DateTime CreateDate
        {
            get { return _mdtCreateDate; }
            set { _mdtCreateDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _mdtModifiedDate; }
            set { _mdtModifiedDate = value; }
        }

        public string OriginalMessage
        {
            get { return _mstrOriginalMessage; }
            set { _mstrOriginalMessage = value; }
        }

        public string TransilatedMessage
        {
            get { return _mstrTransilatedMessage; }
            set { _mstrTransilatedMessage = value; }
        }

        public string ModifiedMessage
        {
            get { return _mstrModifiedMessage; }
            set { _mstrModifiedMessage = value; }
        }
    }

    public class cDS_Messages_ListDataSource
    {

        private static int _mintTotalRecords;

        public static int GetTotalRecords(string strStartDate, string strEndDate, int intStatus, bool bHasCTC, bool bIsError,
                                                        string strSearchText, string filterExpression)
        {
            return (_mintTotalRecords);
        }

        public static List<cDS_Messages_List> GetList(string strStartDate, string strEndDate, int intStatus, bool bHasCTC, bool bIsError,
                                                        string strSearchText, string sortExpression, int startIndex, int pageSize,
                                                        string filterExpression)
        {
            int intTotalRecords = 0;
            int intCurrentPage = startIndex / pageSize + 1;
            List<cDS_Messages_List> lsReturn = GetListInner(strStartDate, strEndDate, intStatus, bHasCTC, bIsError,
                                                        strSearchText, sortExpression, intCurrentPage, pageSize, ref intTotalRecords);
            _mintTotalRecords = intTotalRecords;
            return (lsReturn);
        }

        public static List<cDS_Messages_List> GetListInner(string strStartDate, string strEndDate, int intStatus, bool bHasCTC, bool bIsError,
                                                        string strSearchText, string strSorting, int intCurrentPage, int intPageSize, 
                                                        ref int intTotalRecords)
        {
            SqlConnection objConnection = null;
            SqlCommand objCommand = null;

            List<cDS_Messages_List> lsMessages = new List<cDS_Messages_List>();
            DataSet dsMasterItemList = new DataSet();

            SqlDataAdapter objDataAdapter = new SqlDataAdapter();

            try
            {

                string strConnectionString = Generic.cGeneric.GetConnectionString();

                using (objConnection = new SqlConnection(strConnectionString))
                {
                    using (objCommand = new SqlCommand())
                    {
                        objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        objCommand.CommandText = Generic.cStoredProcedureConstants.SP_Messages_List;
                        objCommand.Connection = objConnection;

                        if (strSearchText == null) { strSearchText = ""; }

                        objCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = ConvertToDateTime(strStartDate);
                        objCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = ConvertToDateTime(strEndDate);
                        objCommand.Parameters.Add("@Status", SqlDbType.Int).Value = intStatus;
                        objCommand.Parameters.Add("@HasCTC", SqlDbType.Bit).Value = bHasCTC;
                        objCommand.Parameters.Add("@IsError", SqlDbType.Bit).Value = bIsError;
                        objCommand.Parameters.Add("@SearchText", SqlDbType.NVarChar).Value = strSearchText;
                        objCommand.Parameters.Add("@Sorting", SqlDbType.NVarChar).Value = strSorting;
                        objCommand.Parameters.Add("@CurrentPage", SqlDbType.Int).Value = intCurrentPage;

                        objCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = (intPageSize >= 500 ? 50 : intPageSize); 

                        SqlParameter sqlReturnParameter = objCommand.Parameters.Add("@TotalRecords", SqlDbType.Int);

                        sqlReturnParameter.Direction = ParameterDirection.Output;

                        if (objConnection.State != ConnectionState.Open) objConnection.Open();

                        if (objConnection.State == ConnectionState.Open)
                        {
                            objDataAdapter.SelectCommand = objCommand;

                            objDataAdapter.Fill(dsMasterItemList);

                            if (sqlReturnParameter.Value != DBNull.Value)
                                intTotalRecords = Convert.ToInt32(sqlReturnParameter.Value);

                            objDataAdapter.Dispose();

                            objConnection.Close();

                            if (dsMasterItemList.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsMasterItemList.Tables[0].Rows)
                                {
                                    lsMessages.Add(new cDS_Messages_List((int)dr[0], (string)dr[4], (string)dr[5], (DateTime)dr[3], (DateTime)dr[10],
                                                                            (object)dr[7], (object)dr[8], (object)dr[9]));
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //TODO: Provide error log
            }
            finally
            {
                if (objConnection.State != ConnectionState.Closed) objConnection.Close();
            }

            return (lsMessages);
        }




        private static DateTime ConvertToDateTime(string strValue)
        {
            if (null != strValue && strValue.Length > 0)
            {
                //return (Convert.ToDateTime(strValue, CultureInfo.CreateSpecificCulture("en-US")));
                return (Convert.ToDateTime(strValue));
            }
            else
            {
                return (DateTime.Now);
            }
        }
    }
}