using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["WEI"] == null)
                {
                    lblsignout.Text = "Your session has timed out. Please click here to";
                    HyperLink1.Text = "sign in";
                    HyperLink1.NavigateUrl = "Login.aspx";
                }
                else
                {
                    //force logout
                    Response.Cookies["WEI"].Expires = DateTime.Now.AddDays(-1);
                    lblsignout.Text = "Sign out successful. Click here to";
                    HyperLink1.Text = "sign in";
                    HyperLink1.NavigateUrl = "Login.aspx";
                }
            }
            catch (Exception ex)
            {
                LogUtil.log("Error while logging out.", ex);
                throw ;
            }
        }
    }
}