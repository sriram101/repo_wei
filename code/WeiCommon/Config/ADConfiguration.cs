using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class ADConfiguration
    {
        private string _LDAPConnectionString;
        private string _approver;
        private string _reviewer;
        //private string _admin;

        public string LDAPConnectionString { get { return _LDAPConnectionString; } }
        public string Approver { get { return _approver; } }
        public string Reviewer { get { return _reviewer; } }
        //public string Admin { get { return _admin; } }

        internal void LoadValuesFromXml(XmlNode section)
        {
            XmlAttributeCollection attrs = section.Attributes;

            if (attrs["LDAPConnectionString"] != null)
            {
                _LDAPConnectionString = attrs["LDAPConnectionString"].Value;
                attrs.RemoveNamedItem("LDAPConnectionString");
            }

            if (attrs["Approver"] != null)
            {
                _approver = attrs["Approver"].Value;
                attrs.RemoveNamedItem("Approver");
            }
            if (attrs["Reviewer"] != null)
            {
                _reviewer = attrs["Reviewer"].Value;
                attrs.RemoveNamedItem("Reviewer");
            }
            //if (attrs["Admin"] != null)
            //{
            //    _admin = attrs["Admin"].Value;
            //    attrs.RemoveNamedItem("Admin");
            //}


            // If there are any further attributes, there's an error!
            if (attrs.Count > 0)
                throw new System.Configuration.ConfigurationErrorsException("There are illegal attributes provided in the section");
        }

        public static ADConfiguration GetConfig()
        {
            return System.Configuration.ConfigurationManager.GetSection("ADConfiguration") as ADConfiguration;
        } 
    }
}
