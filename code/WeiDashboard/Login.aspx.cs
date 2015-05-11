using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.Cookies["WEI"] == null)
            {


                HttpCookie WEICookie = new HttpCookie("WEI");
                WEICookie["user"] = WindowsIdentity.GetCurrent().Name;
                WEICookie.Expires = DateTime.Now.AddMinutes(20);
                Response.Cookies.Add(WEICookie); 
                Response.Redirect("Messages.aspx");

            }
        }
    }
}