using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public class WeiDashboardConfiguration:ConfigurationSection
    {
        [ConfigurationProperty("serviceName", IsRequired = true)]
            public string ServiceName
            {
                get
                {
                    return (string)this["serviceName"];
                }
                set
                {
                    this["serviceName"] = value;
                }
            }
        
    }
}