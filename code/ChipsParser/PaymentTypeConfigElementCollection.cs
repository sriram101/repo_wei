using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    public class PaymentTypeConfigElementCollection: ConfigurationElementCollection
    {
        public PaymentTypeConfigElementCollection()
        {
            base.AddElementName = "PaymentType";
        }
        public PaymentTypeConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as PaymentTypeConfigElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PaymentTypeConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PaymentTypeConfigElement)element).Id;
        }

    }
}


