/*
**  File Name:		WeiConfiguration.cs
**
**  Functional Description:
**
**      This class is used to read the configuration from app.config specifically <wei> 
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
    public class WeiConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("threadPoolSize", IsRequired = true)]
        public string ThreadPoolSize
        {
            get
            {
                return (string)this["threadPoolSize"];
            }
            set
            {
                this["threadPoolSize"] = value;
            }
        }

        [ConfigurationProperty("applyCustomName", IsRequired = true)]
        public bool applyCustomName
        {
            get
            {
                return (bool)this["applyCustomName"];
            }
            set
            {
                this["applyCustomName"] = value;
            }
        }

        [ConfigurationProperty("customName", IsRequired = true)]
        public string customName
        {
            get
            {
                return (string)this["customName"];
            }
            set
            {
                this["customName"] = value;
            }
        }

        [ConfigurationProperty("requiresReview", IsRequired = true)]
        public bool requiresReview
        {
            get
            {
                return (bool)this["requiresReview"];
            }
            set
            {
                this["requiresReview"] = value;
            }
        }

        [ConfigurationProperty("searchString", IsRequired = true)]
        public string searchString
        {
            get
            {
                return (string) this["searchString"];
            }
            set
            {
                this["searchString"] = value;
            }
        }

        [ConfigurationProperty("replaceString", IsRequired = true)]
        public string replaceString
        {
            get
            {
                return (string)this["replaceString"];
            }
            set
            {
                this["replaceString"] = value;
            }
        }

        [ConfigurationProperty("Translator")]
        public TranslateConfigElement TranslatorSetting
        {
            get
            {
                return (TranslateConfigElement)this["Translator"];
            }
            set
            {
                this["Translator"] = value;
            }
        }

        [ConfigurationProperty("Swift")]
        public SwiftConfigElement SwiftSetting
        {
            get
            {
                return (SwiftConfigElement)this["Swift"];
            }
            set
            {
                this["Swift"] = value;
            }
        }

        [ConfigurationProperty("Parsers", IsDefaultCollection = false, IsRequired=false)]
        public ParserConfigElementCollection Parsers
        {
            get
            {
                return (ParserConfigElementCollection)this["Parsers"];
            }
            set
            {
                this["Parsers"] = value;
            }
        }

        [ConfigurationProperty("Proxy", IsRequired = false)]
        public ProxyConfigElement Proxy
        {
            get
            {
                return (ProxyConfigElement)this["Proxy"];
            }
            set
            {
                this["Proxy"] = value;
            }
        }
    }
}
