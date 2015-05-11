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
                    if (Session["WEIReviewer"].ToString() == "N")
                    {
                        lblsignout.Text = "User : " + Session["User"].ToString() + " does not belong to a valid role.";
                        HyperLink1.Visible = false;
                    }
                    else
                    {
                        lblsignout.Text = "Your session has timed out. Please click here to";
                        HyperLink1.Text = "sign in";
                        HyperLink1.NavigateUrl = "Login.aspx";
                    }
                }
                else
                {
                    //force logout
                    Response.Cookies["WEI"].Expires = DateTime.Now.AddDays(-1);

                    //SMU: Feb 01, 2013 Added the following block
                    //ref: http://msdn.microsoft.com/en-us/library/ms178195%28v=vs.100%29.aspx
                    HttpCookie weiCookie = new HttpCookie("WEI");
                    weiCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(weiCookie);


                    lblsignout.Text = "Sign out successful. Click here to";
                    HyperLink1.Text = "sign in";
                    HyperLink1.NavigateUrl = "Login.aspx";
                }

                //SMU: Feb 01, 2013 Added the following blocks
                if (Request.Cookies["WEIRole"] != null)
                {
                    HttpCookie weiCookie = new HttpCookie("WEIRole");
                    weiCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(weiCookie);
                }
                if (Request.Cookies["starttime"] != null)
                {
                    HttpCookie weiCookie = new HttpCookie("starttime");
                    weiCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(weiCookie);
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