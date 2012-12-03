using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    [DataContract]
    public class InterfaceStatus
    {
        private int interfaceId;
        private string interfaceName;
        private DriverStatus status;

        [DataMember]
        public int InterfaceId
        {
            get { return interfaceId; }
            set { interfaceId = value; }
        }

        [DataMember]
        public string InterfaceName
        {
            get { return interfaceName; }
            set { interfaceName = value; }
        }

        [DataMember]
        public DriverStatus Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
