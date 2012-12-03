/*
**  File Name:		ParserConfigElement.cs
**
**  Functional Description:
**
**      This class is used to read the configuration from app.config specifically <Parser> in <Wei> tag 
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
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class ParserConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {

            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("className", IsRequired = true)]
        public string ClassName
        {

            get
            {
                return (string)this["className"];
            }
            set
            {
                this["className"] = value;
            }
        }

        [ConfigurationProperty("param1", IsRequired = false)]
        public string Param1
        {

            get
            {
                return (string)this["param1"];
            }
            set
            {
                this["param1"] = value;
            }
        }

        [ConfigurationProperty("param2", IsRequired = false)]
        public string Param2
        {

            get
            {
                return (string)this["param2"];
            }
            set
            {
                this["param2"] = value;
            }
        }

        [ConfigurationProperty("param3", IsRequired = false)]
        public string Param3
        {

            get
            {
                return (string)this["param3"];
            }
            set
            {
                this["param3"] = value;
            }
        }


        [ConfigurationProperty("param4", IsRequired = false)]
        public string Param4
        {

            get
            {
                return (string)this["param4"];
            }
            set
            {
                this["param4"] = value;
            }
        }

        [ConfigurationProperty("param5", IsRequired = false)]
        public string Param5
        {

            get
            {
                return (string)this["param5"];
            }
            set
            {
                this["param5"] = value;
            }
        }

        [ConfigurationProperty("dllName", IsRequired = false)]
        public string DllName
        {

            get
            {
                return (string)this["dllName"];
            }
            set
            {
                this["dllName"] = value;
            }
        }


    }
}
