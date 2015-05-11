using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.IO;
//using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using System.Threading;
using Telavance.AdvantageSuite.Wei.WeiCommon;

[assembly:CLSCompliant(true)]

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public partial class WeiService : ServiceBase
    {
        private InterfaceManager _interfaceManager = null;
        private RequestManager _requestManager;
        public static WeiService instance;

        private DBUtil _dbUtils;

        public ServiceHost serviceHost = null;

        public RequestManager RequestManager
        {
            get
            {
                return _requestManager;
            }
        }

        public DBUtil DBUtil
        {
            get
            {
                return _dbUtils;
            }
        }
        public WeiService()
        {
            InitializeComponent();
        }

#if DEBUG
        public void Start()
        {
            OnStart(null);
            System.Threading.Thread.Sleep(50000000);
        }
#endif

        protected override void OnStart(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            String fileName = assembly.Location + ".config";

            if (!File.Exists(fileName))
            {
                throw new Exception("Missing configuration file. Expecting file @" + fileName);
            }

            LogUtil.logInfo("Starting Wei Service");
            try
            {
                _dbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();
                _interfaceManager = EnterpriseLibraryContainer.Current.GetInstance<InterfaceManager>();
                _requestManager = new RequestManager(_dbUtils);
                _interfaceManager.initailize(this);

                if (serviceHost != null)
                {
                    serviceHost.Close();
                }

                // Create a ServiceHost for the CalculatorService type and 
                // provide the base address.
                serviceHost = new ServiceHost(typeof(WeiMonitoring));

                // Open the ServiceHostBase to create listeners and start 
                // listening for messages.

                serviceHost.Open();
                instance = this;
                LogUtil.logInfo("Started Wei Service");
            }
            catch (AddressAlreadyInUseException e)
            {
                LogUtil.log("Service address is already in use", e);
            }
            catch (Exception e)
            {
                LogUtil.log("Error Starting Wei Service", e);
                throw (e);
            }
           
        }

        protected override void OnStop()
        {
            Console.WriteLine("in stop");
            stopInterfaces();
            for (int i = 0; i < 10; i++)
            {
                if (WeiService.isAnyInterfaceRunning())
                {
                    Thread.Sleep(5000);
                }
            }
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

        public static void stopInterfaces()
        {
            foreach (Interface weiInterface in InterfaceManager.interfaces.Values)
            {
                weiInterface.Driver.stop();
            }
        }

        public static bool isAnyInterfaceRunning()
        {
            foreach (Interface weiInterface in InterfaceManager.interfaces.Values)
            {
                if (weiInterface.Driver.getStatus() != DriverStatus.NotRunning)
                    return true;
            }
            return false;
        }
    }
}
