using System;
using System.Web;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.Pages
{
    public partial class CustomErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _sEmail = ConfigurationManager.AppSettings["DefaultSupportEmailAddress"];
            string _sTelNo = ConfigurationManager.AppSettings["DefaultSupportTelephoneNumber"];
            lblError.Text = "An unexpected error occured in the application. Please email Telavance support at: " + _sEmail + " or contact at: " + _sTelNo;
            Exception ex = HttpContext.Current.Server.GetLastError();
            if (ex != null)
            {
                if (ex.GetType() == typeof(HttpUnhandledException) && ex.InnerException != null)
                {


                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("Messages1.aspx");
        }
    }
}