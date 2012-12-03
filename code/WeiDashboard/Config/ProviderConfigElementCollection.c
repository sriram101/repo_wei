using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class ProviderConfigElementCollection : ConfigurationElementCollection
    {
        public ProviderConfigElementCollection()
        {
            base.AddElementName = "Provider";
        }
        public ProviderConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ProviderConfigElement;
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
            return new ProviderConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderConfigElement)element).Name;
        }

    }
}