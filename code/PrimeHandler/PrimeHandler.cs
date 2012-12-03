/*
**  File Name:		PrimeHandler.cs
**
**  Functional Description:
**
**      This class implements IHandler and provides Prime specific message handling
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

using Telavance.AdvantageSuite.Wei.WeiService;


namespace Telavance.AdvantageSuite.Wei.PrimeHandler
{
    public class PrimeHandler : IHandler
    {
        public String getMessageForTranslation(String message)
        {
            try
            {
                //return data if it is a valid xml
                //OFACRequest request = OFACRequest.getOFACRequest(message);
                //return request.data;

                int startIndex = message.IndexOf("<data>", StringComparison.OrdinalIgnoreCase);
                if (startIndex != -1)
                {
                    startIndex += "<data>".Length;
                    int endIndex = message.IndexOf("</data>", StringComparison.OrdinalIgnoreCase);
                    if (endIndex != -1)
                    {
                        return message.Substring(startIndex, (endIndex - startIndex));
                    }
                }
            }
            catch (Exception e)
            {
                //dont log
            }

            //else return the full message
            return message;
        }

        public String getRepackagedTranslatedString(Request request, String translateInput)
        {
            if (translateInput.Length == request.MessageBody.Length)
                return request.TranslatedMessage;
            int startIndex = request.MessageBody.IndexOf("<data>",StringComparison.OrdinalIgnoreCase);
            startIndex += "<data>".Length;
            int endIndex = request.MessageBody.IndexOf("</data>", StringComparison.OrdinalIgnoreCase);
            String retString = request.MessageBody;
            retString = retString.Remove(startIndex, (endIndex - startIndex));
            retString = retString.Insert(startIndex, request.TranslatedMessage);
            request.TranslatedMessage = retString;
            return retString;
        }

        public String getRepackagedResponseString(String request, String response)
        {
            int startIndex = response.IndexOf("</OfacHeader>", StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1)
                return request;
            startIndex += "</OfacHeader>".Length;

            int endIndex = response.IndexOf("</OfacRequestAndResponse>", StringComparison.OrdinalIgnoreCase);

            String retString="";
            if (endIndex == -1)
            {
                retString = response.Remove(startIndex);
                retString += request;
            }
            else
            {
                retString = response.Remove(startIndex, (endIndex - startIndex));
                retString = retString.Insert(startIndex, request);
            }

            return retString;

        }
    }

}
