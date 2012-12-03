/*
**  File Name:		InterfaceManager.cs
**
**  Functional Description:
**
**      This class created and manages  all the interfaces that are configured in the db.
**	
**
**	Author:	Lakshman Ramakrishnan
**  Facility	    WEI
**  Creation Date:  12/30/2010
**
*******************************************************************************
**                                                                           **
**      COPYRIGHT                                                            **
**                                                                           **
** (C) Copyright 2010                                                        **
** Telavance, inc                                                            **
**                                                                           **
** This software is furnished under a license for use only on a single       **
** computer system and may be copied only with the inclusion of the  above   **
** copyright notice. This software or any other copies thereof, may not be   **
** provided or otherwise made available to any other person  except for use  **
** on such system and to one who agrees to these license terms. title and    **
** ownership of the software shall at all times remain in Telavance,inc      **
** Inc.                                                                      **
**                                                                           **
** The information in this software is subject to change without notice and  **
** should not be construed as a commitment by Telavance, Inc.	             **
**									     									 **
*******************************************************************************
 									    
*******************************************************************************
 		                    Maintenance History				    
-------------|----------|------------------------------------------------------
    Date     |	Person  |  Description of Modification			    
-------------|----------|------------------------------------------------------

12/30/2010       RL        Inital version
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Telavance.AdvantageSuite.Wei.WeiCommon;


namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class InterfaceManager
    {
        private Database _weidb;
        private IDictionary<String, IHandler> handlers = new Dictionary<String, IHandler>();
        private IDictionary<String, Type> drivers = new Dictionary<String, Type>();
        public static IDictionary<int, Interface> interfaces = new Dictionary<int, Interface>();

        public InterfaceManager(/*[Dependency("WeiDB")]*/Database db)
        {
            _weidb = db;
        }

        private IHandler createHandlerIfReqd(String handlerName, String handlerDll, String typeName)
        {
            try
            {
                if (!handlers.ContainsKey(handlerName))
                {

                    Type handlerType = TypeUtil.getType(handlerDll, typeName, typeof(IHandler));

                    if (handlerType == null)
                    {
                        LogUtil.logInfo("Cannot load typeName=" + typeName + " in dll=" + handlerDll);
                        return null;
                    }

                    IHandler handler = (IHandler)Activator.CreateInstance(handlerType);
                    handlers.Add(handlerName, handler);
                }

                return handlers[handlerName];
            }
            catch (Exception e)
            {
                LogUtil.log("Error creating handler type:" + typeName + " from dll " + handlerDll, e);
            }
            return null;
        }

        private IDriver createDriver(String driverName, String driverDll, String typeName)
        {
            try
            {
                if (!drivers.ContainsKey(driverName))
                {
                    Type type = TypeUtil.getType(driverDll, typeName, typeof(IDriver));

                    if (type == null)
                    {
                        LogUtil.logInfo("Cannot load typeName=" + typeName + " in dll=" + driverDll);
                        return null;
                    }

                    drivers.Add(driverName, type);
                }

                Type driverType = drivers[driverName];
                IDriver driver = (IDriver)Activator.CreateInstance(driverType);
                return driver;
            }
            catch (Exception e)
            {
                LogUtil.log("Error creating handler type:" + typeName + " from dll " + driverDll, e);
            }

            return null;
        }



        public void initailize(WeiService service)
        {
            LogUtil.logDebug("Starting initailize in InterfaceManager");
            DbCommand cmd = _weidb.GetStoredProcCommand("Wei_getInterfaces");
            using (IDataReader dataReader = _weidb.ExecuteReader(cmd))
            {
                LogUtil.logDebug("Executed getInterfaces");
                while (dataReader.Read())
                {
                    LogUtil.logDebug("Has data from getInterfaces");
                    try
                    {
                        int interfaceId = (int)dataReader["id"];

                        LogUtil.logInfo("Starting Interface:" + dataReader["name"] + " with interface id " + interfaceId + " DriverType:" + dataReader["drivername"].ToString() + " HandlerType: " + dataReader["handlername"].ToString());

                        IHandler handler = createHandlerIfReqd(dataReader["handlername"].ToString(), dataReader["handlerdll"].ToString(), dataReader["handlertype"].ToString());
                        IDriver driver = createDriver(dataReader["drivername"].ToString(), dataReader["driverdll"].ToString(), dataReader["drivertype"].ToString());

                        if (handler == null || driver == null)
                        {
                            LogUtil.logInfo("Skipping Interface:" + dataReader["name"] + " with interface id " + interfaceId + " DriverType:" + dataReader["drivername"].ToString() + " HandlerType: " + dataReader["handlername"].ToString() + ". Handler=" + handler +" and driver=" + driver);
                            continue;
                        }
                        driver.initialize(interfaceId, dataReader["config"].ToString(), service.RequestManager, service.DBUtil);

                        interfaces.Add(interfaceId, new Interface(interfaceId, (string)dataReader["name"], handler, driver, dataReader["fileformat"].ToString()));

                        driver.start();

                        LogUtil.logInfo("Started Interface:" + dataReader["name"]);

                        
                    }
                    catch (Exception e)
                    {
                        LogUtil.log("Error starting the interface", e);
                    }
                }
                LogUtil.logDebug("Exiting initialize in InterfaceManager");
            }

        }

        public static Interface getInterface(int interfaceid){
            return interfaces[interfaceid];
        }
    }
}
