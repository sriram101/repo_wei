using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WeiDashboard.Config
{
    public class MapFileConfigElementCollection : ConfigurationElementCollection
    {
        public MapFileConfigElementCollection()
        {
            base.AddElementName = "MapFile";
        }
        public MapFileConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as MapFileConfigElement;
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
            return new MapFileConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((MapFileConfigElement)element).Language;
        }
    }
}