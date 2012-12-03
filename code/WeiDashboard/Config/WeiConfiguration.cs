using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class WeiConfiguration : ConfigurationSection
    {

            [ConfigurationProperty("threadPoolSize", IsRequired = true)]
            public string ThreadPoolSize
            {
                get
                {
                    return (string)this["threadPoolSize"];
                }
                set
                {
                    this["threadPoolSize"] = value;
                }
            }

            [ConfigurationProperty("Translator")]
            public TranslateConfigElement TranslatorSetting
            {
                get
                {
                    return (TranslateConfigElement)this["Translator"];
                }
                set
                {
                    this["Translator"] = value;
                }
            }

          
        }


        
    }
