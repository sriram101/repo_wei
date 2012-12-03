/*
**  File Name:		TranslateConfigElement.cs
**
**  Functional Description:
**
**      This class is used to read the configuration from app.config specifically <Translator> 
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
using System.Xml;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class TranslateConfigElement : ConfigurationElement
    {

        [ConfigurationProperty("noOfRetries", IsRequired = true)]
        public string NoOfRetries
        {
            get
            {
                return (string)this["noOfRetries"];
            }
            set
            {
                this["noOfRetries"] = value;
            }
        }

        [ConfigurationProperty("currentLanguage", IsRequired = true)]
        public string CurrentLanguage
        {
            get
            {
                return (string)this["currentLanguage"];
            }
            set
            {
                this["currentLanguage"] = value;
            }
        }

        [ConfigurationProperty("currentTranslationProvider", IsRequired = true)]
        public string CurrentTranslationProvider
        {
            get
            {
                return (string)this["currentTranslationProvider"];
            }
            set
            {
                this["currentTranslationProvider"] = value;
            }
        }

        [ConfigurationProperty("ctcDeterminingCount", IsRequired = true)]
        public string CtcDeterminingCount
        {
            get
            {
                return (string)this["ctcDeterminingCount"];
            }
            set
            {
                this["ctcDeterminingCount"] = value;
            }
        }

        [ConfigurationProperty("ctcAllowedChars", IsRequired = true)]
        public string CtcAllowedChars
        {
            get
            {
                return (string)this["ctcAllowedChars"];
            }
            set
            {
                this["ctcAllowedChars"] = value;
            }
        }
        
        [ConfigurationProperty("MapFiles", IsDefaultCollection = false)]
        public MapFileConfigElementCollection MapFiles
        {
            get
            {
                return (MapFileConfigElementCollection)this["MapFiles"];
            }
            set
            {
                this["MapFiles"] = value;
            }
        }

        [ConfigurationProperty("Providers", IsDefaultCollection = false)]
        public ProviderConfigElementColl Providers
        {
            get
            {
                return (ProviderConfigElementColl)this["Providers"];
            }
            set
            {
                this["Providers"] = value;
            }
        }

        [ConfigurationProperty("TranslationExceptions", IsDefaultCollection = false, IsRequired=true)]
        public TranslationExceptionConfigCollection TranslationExceptions
        {
            get
            {
                return (TranslationExceptionConfigCollection)this["TranslationExceptions"];
            }
            set
            {
                this["TranslationExceptions"] = value;
            }
        }

        
        
    }
}
