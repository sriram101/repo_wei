/*
**  File Name:		GoogleTranslationProvider.cs
**
**  Functional Description:
**
**      This File is used for google translation
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
11/21/2011       RP        Changed the Google Translate API URI to Version 2
                           Changed the code on JSON Decode
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{
    class GoogleTranslationProvider : AbstractWebUrlTranslator
    {
        //private string uri = "http://ajax.googleapis.com/ajax/services/language/translate";
        private string uri = "https://www.googleapis.com/language/translate/v2";

        //private String messageFormat = "v=1.0&key={0}&q={1}&langpair={2}%7C{3}";

        private String messageFormat = "key={0}&q={1}&source={2}&target={3}";

        public override string getMessage(String message, string fromLanguage, string toLanguage)
        {
           
            return string.Format(messageFormat, key, message, fromLanguage, toLanguage);
        }

        public override string getURL(String message, string fromLanguage, string toLanguage)
        {
            return uri;//string.Format(uri, key, message, fromLanguage, toLanguage);
        }

        public override string processResponse(String output, String message, string fromLanguage, string toLanguage)
        {
            object result = JsonResponse.Deserialize(output);
            
            //return JsonResponse.HtmlDecode(((Response<TranslatedText>)result).responseData.translatedText);

            return JsonResponse.HtmlDecode(((Response)result).Data.Translations[0].TranslatedText.ToString());
            
            

        }

    }
}
