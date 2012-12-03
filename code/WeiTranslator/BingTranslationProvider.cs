/*
**  File Name:		BingTranslationProvider.cs
**
**  Functional Description:
**
**      This File is used for bing translation
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


using System.Net;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{
    class BingTranslationProvider : AbstractWebUrlTranslator
    {
        private string uri = "http://api.microsofttranslator.com/V2/Http.svc/Translate";

        private String messageFormat = "appId={0}&text={1}&from={2}&to={3}";


        public override string getMessage(String message, string fromLanguage, string toLanguage)
        {
            return string.Format(messageFormat, key, message, fromLanguage, toLanguage);
        }

        public override string getURL(String message, string fromLanguage, string toLanguage)
        {
            return uri;
        }

        public override string processResponse(String output, String message, string fromLanguage, string toLanguage)
        {
            int index = output.IndexOf('>');
            output = output.Substring(index + 1);
            output = output.Replace("</string>", "");
            return output;
        }

    }
}
