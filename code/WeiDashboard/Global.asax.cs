/*
**  File Name:		Messages.aspx
**
**  Functional Description:
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	   Global.asax
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

04/25/2011       RP        changed the namespace
05/01/2011       RP        Introduced application level custom exception handling

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using System.Web.SessionState;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = HttpContext.Current.Server.GetLastError().GetBaseException();
            if (ex.GetType() == typeof(HttpUnhandledException) && ex.InnerException != null)
            {
                ex = ex.InnerException;
                               
                LogUtil.log("Please Check the Event Viewer or Log File for errors:", ex);
                
            }

            Server.Transfer("CustomErrorPage.aspx");

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
