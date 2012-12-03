using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class ProviderConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("className", IsRequired = true)]
        public string ClassName
        {
            get
            {
                return (string)this["className"];
            }
            set
            {
                this["className"] = value;
            }
        }


        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("maxLength", IsRequired = false, DefaultValue = -1)]
        public int MaxLength
        {
            get
            {
                return Convert.ToInt32(this["maxLength"]);
            }
            set
            {
                this["maxLength"] = value;
            }
        }
        [ConfigurationProperty("textDescription", IsRequired = false)]
        public string textDescription
        {
            get
            {
                return (string)this["textDescription"];
            }
            set
            {
                this["textDescription"] = value;
            }
        }

        
        [ConfigurationProperty("versionAPI", IsRequired = false)]
        public string versionAPI
        {
            get
            {
                return (string)this["versionAPI"];
            }
            set
            {
                this["versionAPI"] = value;
            }
        }

      
    }
}