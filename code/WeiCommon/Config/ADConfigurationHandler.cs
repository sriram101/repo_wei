using System;
using System.Web;
using System.Configuration;
using System.Xml;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    class ADConfigurationHandler : IConfigurationSectionHandler
    {

        public object Create(object parent, object configContext, XmlNode section)
        {
            ADConfiguration config = new ADConfiguration();
            config.LoadValuesFromXml(section);

            return config;
        }
    }
}
