using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class MapFileConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("language", IsRequired = true)]
        public string Language
        {
            get
            {
                return (string)this["language"];
            }
            set
            {
                this["language"] = value;
            }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }


        [ConfigurationProperty("keyindex", IsRequired = true)]
        public string Keyindex
        {
            get
            {
                return (string)this["keyindex"];
            }
            set
            {
                this["keyindex"] = value;
            }
        }

        [ConfigurationProperty("keyindex", IsRequired = true)]
        public int KeyIndex
        {
            get
            {
                return Convert.ToInt32(this["keyindex"]);
            }
            set
            {
                this["keyindex"] = value;
            }
        }

        [ConfigurationProperty("valueindex", IsRequired = true)]
        public int ValueIndex
        {
            get
            {
                return Convert.ToInt32(this["valueindex"]);
            }
            set
            {
                this["valueindex"] = value;
            }
        }
    }
}