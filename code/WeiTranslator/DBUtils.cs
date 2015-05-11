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
using System.Data.SqlClient;
using System.Data.Common;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{
    public class DBUtils
    {
        private Database _weidb;
        String _sTrans = null;
        
        public DBUtils(/*[Dependency("WeiDB")]*/Database db)
        {
            _weidb = db;
        }

        public String getCTCTranslations(string sCTCCode)
        {
            try
            {
                _sTrans = null;
                DbCommand cmd = _weidb.GetStoredProcCommand("Wei_GetTranslationByCTCCode");
                _weidb.AddInParameter(cmd, "@CTCCode", DbType.String, sCTCCode);

                using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
                {
                    while (dataReader.Read())
                    {
                        _sTrans = dataReader["NewTrans"].ToString();
                    }
                }
                return _sTrans;
            }
            catch (SqlException ex)
            {
                LogUtil.logError("Error getting translations for telegraphic codes: " + ex.InnerException.ToString());
                throw new Exception("Error getting translations for telegraphic codes: " + ex.InnerException.ToString());
            }
        }

        

        public void addTranslation(string sCTCCode, string sChineseChar, string sPinYin, string sEnglishTrans)
        {
            try
            {

                DbCommand cmd = _weidb.GetStoredProcCommand("WEI_AddTranslation");

                _weidb.AddInParameter(cmd, "@CTCCode", DbType.String, sCTCCode);
                _weidb.AddInParameter(cmd, "@ChineseChar", DbType.String, sChineseChar);
                _weidb.AddInParameter(cmd, "@PinYin", DbType.String, sPinYin);
                _weidb.AddInParameter(cmd, "@EnglishTrans", DbType.String, sEnglishTrans);
                _weidb.ExecuteScalar(cmd);

                LogUtil.logInfo("Created new translation for Telegraphic Code : " + sCTCCode);
                AuditUtil.getInstance().audit( AuditLevel.Info, "Added new translations for CTC codes -" + sCTCCode);
            }
            catch (SqlException ex)
            {
                LogUtil.logError("Error adding new translation: " + ex.ToString());
                throw new Exception("Error adding new translation: " + ex.ToString());
            }
            
        }
        public void addCTCRequest(int requestId,string sCTCCode)
        {
            try
            {

                DbCommand cmd = _weidb.GetStoredProcCommand("WEI_InsertCTCRequests");

                _weidb.AddInParameter(cmd, "@RequestID", DbType.Int32, requestId);
                _weidb.AddInParameter(cmd, "@CTCCode", DbType.String, sCTCCode); 
                _weidb.ExecuteScalar(cmd);

                LogUtil.logInfo("Created Telegraphic Code for request: " + requestId);
                AuditUtil.getInstance().audit((int)requestId, AuditLevel.Info, "Created Telegraphic Code for request -" + requestId);
            }
             
            catch (SqlException ex)
            {
                LogUtil.logError("Error adding  telegraphic codes to request: " + ex.InnerException.ToString());
                throw new Exception("Error adding new telegraphic codes to request: " + ex.InnerException.ToString());
            }
            

        }
    }

}
