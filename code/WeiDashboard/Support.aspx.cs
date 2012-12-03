/*
**  File Name:		Support.aspx
**
**  Functional Description: 
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	    WEI Dashboard
**  Creation Date:  01/14/2011
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

01/14/2011       RP        Initial Version
04/25/2011       RP        Changed the namespace
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Support : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string _stext = ConfigurationManager.AppSettings["support"];
                lblText.Text = _stext;
                lblEmailAddKey.Text = ConfigurationManager.AppSettings["DefaultSupportEmailAddressKey"];
                lnkEmailAddress.InnerText =  ConfigurationManager.AppSettings["DefaultSupportEmailAddress"];
                lblSupportTelKey.Text = ConfigurationManager.AppSettings["DefaultSupportTelephoneNumberKey"];
                lblSupportKeyValue.Text = ConfigurationManager.AppSettings["DefaultSupportTelephoneNumber"];

                btnClose.Attributes.Add("onclick", "popup_close();return false;"); 
            }
        }
    }
}