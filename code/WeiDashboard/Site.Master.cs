/*
**  File Name:		Site.Master
**
**  Functional Description:
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	    WEI Dashboard
**  Creation Date:  01/07/2011
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

01/07/2011       RP        Initial Version
04/23/2011       RP        Added Close link button to close the webpage
11/30/2012                 Added a SignOut Link button. Label to display the user name and the role
*/


using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private string _strUser;

        public string Username
        {

            get
            {
                return _strUser;
            }
            set
            {
                _strUser = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkAbout.Attributes.Add("onClick","open_win('About.aspx'); return false;");
            lnkSupport.Attributes.Add("onClick","open_win('Support.aspx'); return false;");
           //lnkSignOut.Attributes.Add("onClick", "NavigateURL(); return false");
            
            _strUser = WindowsIdentity.GetCurrent().Name;

            //SMU: Feb 05, 2013
            //setUser(_strUser);

            if (null != Request.Cookies["WEIRole"] && Request.Cookies["WEIRole"]["WEIReviewer"] == "Y") //SMU: Feb 12, 2013
            {
                setUser(_strUser);
            }

            else
            {
                setUser("");
            }


        }

        protected void LinkButton_Click(object sender, EventArgs e)
        {
            Request.Cookies["WEIRole"]["WEIReviewer"] = "N"; //SMU: Feb 12, 2013
            lblUserRole.Text = "";
            lblUserRole.Visible = false;
            lblUser.Text = "";
            lblUser.Visible = false;
            Server.Transfer("LogOut.aspx",false);
        }
        public void setUser(string strUserName)
        {

            if (strUserName != string.Empty)
            {
            lblUser.Text = strUserName;
                lblWelcome.Visible = true;
                lblRoleName.Visible = true;
            lblUser.Visible = true;
            lblUserRole.Visible = true;
            //lblRole.Text =
            var httpCookie = Request.Cookies["WEIRole"];
            if (httpCookie != null)
            {
                if (httpCookie["WEIReviewer"] == "Y")
                {
                    lblUserRole.Text = "Reviewer";
                }
                if (httpCookie["WEIApprover"] == "Y")
                {
                    lblUserRole.Text = "Approver";
                }
             }
         }
            else
            {
                lblWelcome.Visible = false;
                lblUser.Visible = false;
                lblUserRole.Visible = false;
                lblRoleName.Visible = false;
            }
         }     

     
    }
}