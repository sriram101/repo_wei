using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    public class PaymentTypeTagConfigElement : ConfigurationElement
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
    }
}
