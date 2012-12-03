/*
**  File Name:		SwiftConfigElement.cs
**
**  Functional Description:
**
**      This class is used to read the configuration from app.config specifically <Swift> 
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
    public class SwiftConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("Tags", IsDefaultCollection = false)]
        public SwiftTagConfigElementColl Tags
        {
            get
            {
                return (SwiftTagConfigElementColl)this["Tags"];
            }
            set
            {
                this["Tags"] = value;
            }
        }

        [ConfigurationProperty("translateAllTags", IsRequired = false, DefaultValue = "false")]
        public string TranslateAllTags
        {
            get
            {
                return (string)this["translateAllTags"];
            }
            set
            {
                this["translateAllTags"] = value;
            }
        }
    }
}
