using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    public class ChipsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("translateAllTags", IsRequired = true)]
        public string TranslateAllTags
        {
            get
            {
                return (string)this["translateAllTags"];
            }
            set
            {
                this["translateAllTags"] = value;
            }
        }

        [ConfigurationProperty("Tags", IsDefaultCollection = false, IsRequired = false)]
        public TagConfigElementCollection Tags
        {
            get
            {
                return (TagConfigElementCollection)this["Tags"];
            }
            set
            {
                this["Tags"] = value;
            }
        }


       
    }
}
