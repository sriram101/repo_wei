/*
**  File Name:		JsonResponse.cs
**
**  Functional Description:
**
**      This class is used for deserializing the response from google
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
11/21/2011       RP        Changed the code to incorporate Google Translate V2
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{

    public class JsonResponse
    {
        public static object Deserialize(string Text)
        {
            Response myResult;
            MemoryStream ms = null;
            try
            {
                
                myResult = new Response();
                ms = new MemoryStream(Encoding.Unicode.GetBytes(Text));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(myResult.GetType());
                myResult = serializer.ReadObject(ms) as Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                    ms.Close();
            }
            return myResult;
        }
        public static string HtmlEncode(string text)
        {
            return System.Web.HttpUtility.HtmlEncode(text);
        }
        
        public static string HtmlDecode(string text)
        {
            return System.Web.HttpUtility.HtmlDecode(text);
        }
    }

}
