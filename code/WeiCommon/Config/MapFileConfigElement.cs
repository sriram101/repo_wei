/*
**  File Name:		MapFileConfigElement.cs
**
**  Functional Description:
**
**      This class is used to read the configuration from app.config specifically <MapFile> 
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
    public class MapFileConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get
            {
                return (string)this["description"];
            }
            set
            {
                this["description"] = value;
            }
        }

        [ConfigurationProperty("language", IsRequired = true)]
        public string Language
        {
            get
            {
                return (string)this["language"];
            }
            set
            {
                this["language"] = value;
            }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }


        [ConfigurationProperty("keyindex", IsRequired = true)]
        public string Keyindex
        {
            get
            {
                return (string)this["keyindex"];
            }
            set
            {
                this["keyindex"] = value;
            }
        }

        [ConfigurationProperty("keyindex", IsRequired = true)]
        public int KeyIndex
        {
            get
            {
                return Convert.ToInt32(this["keyindex"]);
            }
            set
            {
                this["keyindex"] = value;
            }
        }

        [ConfigurationProperty("valueindex", IsRequired = true)]
        public int ValueIndex
        {
            get
            {
                return Convert.ToInt32(this["valueindex"]);
            }
            set
            {
                this["valueindex"] = value;
            }
        }
    }
}
