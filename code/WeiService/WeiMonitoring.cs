using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class WeiMonitoring : IWeiMonitoring
    {
        public bool forceShutdown()
        {
            Console.WriteLine("invoking forceShutdown");
            return shutdown(true);
        }

        public bool ShutdownGracefully()
        {
            LogUtil.logInfo("invoking ShutdownGracefully");
            return shutdown(false);
        }

        private bool shutdown(bool force)
        {
            WeiService.stopInterfaces();
            for (int i = 0; i < 10; i++)
            {
                if (WeiService.isAnyInterfaceRunning())
                {
                    Thread.Sleep(5000);
                }
                else
                {
                    WeiService.instance.Stop();
                    return true;
                }
            }

            if (force)
            {
                WeiService.instance.Stop();
                return true;
            }

            return false;
        }

        

        public List<InterfaceStatus> getInterfaces()
        {
            List<InterfaceStatus> interfaceStatus = new List<InterfaceStatus>();
            foreach (Interface weiInterface in InterfaceManager.interfaces.Values)
            {
                InterfaceStatus status = new InterfaceStatus();
                status.InterfaceId = weiInterface.InterfaceId;
                status.InterfaceName = weiInterface.InterfaceName;
                status.Status = weiInterface.Driver.getStatus();
                interfaceStatus.Add(status);

            }
            return interfaceStatus;
        }

        public string getStatistics(int interfaceId)
        {
            return InterfaceManager.interfaces[interfaceId].Driver.getStatistics();
        }

        public List<Request> getAllErrors(int interfaceId)
        {
            return WeiService.instance.DBUtil.getRequestWithErrors(interfaceId);
        }
       

        public List<AuditMessages> getAuditMessages(int requestId)
        {
            return WeiService.instance.DBUtil.getAuditMessages(requestId);
        }

        public void reprocess(int requestId)
        {
            Request request = WeiService.instance.DBUtil.getRequest(requestId);
            WeiService.instance.RequestManager.process(request);
        }
    }
}
