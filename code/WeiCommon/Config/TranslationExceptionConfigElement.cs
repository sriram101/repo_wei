using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class TranslationExceptionConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get
            {
                return (string)this["id"];
            }
            set
            {
                this["id"] = value;
            }
        }

        [ConfigurationProperty("expression", IsRequired = true)]
        public string Expression
        {
            get
            {
                return (string)this["expression"];
            }
            set
            {
                this["expression"] = value;
            }
        }
    }
}
