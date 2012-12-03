/*
**  File Name:		WeiMonitorConfigurationSection.cs
**
**  Functional Description:
**
**      This module get the service name from the configuration file
**	
**
**	Author:	Lakshman Ramakrishnan
**  Facility	    WEI
**  Creation Date:  07/30/2011
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

07/30/2011       RL        Inital version
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    public class WeiMonitorConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("serviceName", IsRequired = true)]
        public string ServiceName
        {
            get
            {
                return (string)this["serviceName"];
            }
            set
            {
                this["serviceName"] = value;
            }
        }
    }
}
