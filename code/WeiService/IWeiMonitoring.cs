using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    [ServiceContract()]
    interface IWeiMonitoring
    {
        [OperationContract]
        bool forceShutdown();

        [OperationContract]
        bool ShutdownGracefully();

        [OperationContract]
        List<InterfaceStatus> getInterfaces();

        [OperationContract]
        string getStatistics(int interfaceId);

        [OperationContract]
        List<Request> getAllErrors(int interfaceId);

        [OperationContract]
        List<AuditMessages> getAuditMessages(int requestId);

        [OperationContract]
        void reprocess(int requestId);

    }
}
