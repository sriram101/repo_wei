using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.Generic
{
    public class cGeneric
    {
        public static string GetConnectionString()
        {
            string strReturn = System.Configuration.ConfigurationManager.ConnectionStrings["WeiDB"].ConnectionString;
            return (strReturn);
        }
    }
}