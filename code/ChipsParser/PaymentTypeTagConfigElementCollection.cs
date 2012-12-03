using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    public class PaymentTypeTagConfigElementCollection: ConfigurationElementCollection
    {
        public PaymentTypeTagConfigElementCollection()
        {
            base.AddElementName = "Tag";
        }
        public PaymentTypeTagConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as PaymentTypeTagConfigElement;
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
            return new PaymentTypeTagConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((PaymentTypeTagConfigElement)element).Id;
        }


    }
}
