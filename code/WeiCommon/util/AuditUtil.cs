/*
**  File Name:		AuditUtil.cs
**
**  Functional Description:
**
**      This class is used audit vaious events of importance to the used in respect to a particular request
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public enum AuditLevel : int
    {
        Debug,
        Info,
        Error
    }
    public class AuditUtil
    {
        private static AuditUtil _instance = null;
        private Database _weidb;

        public AuditUtil(/*[Dependency("WeiDB")]*/Database db)
        {
            _weidb = db;
        }

        public void audit(int requestId, AuditLevel auditLevel, String message)
        {
            if (auditLevel == AuditLevel.Debug)                
            {
                LogUtil.logDebug("RequestId:" + requestId + ". " + message);
            }else if (auditLevel == AuditLevel.Info){
                LogUtil.logInfo("RequestId:" + requestId + ". " + message);
            }

            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddAudit");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, requestId);
            _weidb.AddInParameter(cmd, "@auditLevel", DbType.Int32, auditLevel);
            _weidb.AddInParameter(cmd, "@message", DbType.String, message);

            _weidb.ExecuteScalar(cmd);

        }

        public void audit(AuditLevel auditLevel, String message)
        {
            
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_AddAudit");

            _weidb.AddInParameter(cmd, "@requestid", DbType.Int32, null);
            _weidb.AddInParameter(cmd, "@auditLevel", DbType.Int32, auditLevel);
            _weidb.AddInParameter(cmd, "@message", DbType.String, message);

            _weidb.ExecuteScalar(cmd);

        }
        public static AuditUtil getInstance()
        {
            if(_instance==null)
                _instance = EnterpriseLibraryContainer.Current.GetInstance<AuditUtil>();

            return _instance;
        }
    }
}
