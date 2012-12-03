using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class TranslationExceptionConfigCollection: ConfigurationElementCollection
    {
        public TranslationExceptionConfigCollection()
        {
            base.AddElementName = "TranslationException";
        }
        public TranslationExceptionConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as TranslationExceptionConfigElement;
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
            return new TranslationExceptionConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((TranslationExceptionConfigElement)element).Id;
        }


    }
}
