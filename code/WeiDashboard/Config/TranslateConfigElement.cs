using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class TranslateConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("noOfRetries", IsRequired = true)]
        public string NoOfRetries
        {
            get
            {
                return (string)this["noOfRetries"];
            }
            set
            {
                this["noOfRetries"] = value;
            }
        }

        [ConfigurationProperty("currentLanguage", IsRequired = true)]
        public string CurrentLanguage
        {
            get
            {
                return (string)this["currentLanguage"];
            }
            set
            {
                this["currentLanguage"] = value;
            }
        }

        [ConfigurationProperty("currentTranslationProvider", IsRequired = true)]
        public string CurrentTranslationProvider
        {
            get
            {
                return (string)this["currentTranslationProvider"];
            }
            set
            {
                this["currentTranslationProvider"] = value;
            }
        }

        [ConfigurationProperty("ctcDeterminingCount", IsRequired = true)]
        public string CtcDeterminingCount
        {
            get
            {
                return (string)this["ctcDeterminingCount"];
            }
            set
            {
                this["ctcDeterminingCount"] = value;
            }
        }

        [ConfigurationProperty("ctcAllowedChars", IsRequired = true)]
        public string CtcAllowedChars
        {
            get
            {
                return (string)this["ctcAllowedChars"];
            }
            set
            {
                this["ctcAllowedChars"] = value;
            }
        }

        
        [ConfigurationProperty("Providers", IsDefaultCollection = false)]
        public ProviderConfigElementCollection Providers
        {
            get
            {
                return (ProviderConfigElementCollection)this["Providers"];
            }
            set
            {
                this["Providers"] = value;
            }
        }
       
        


    }


}