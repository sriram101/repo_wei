/*
**  File Name:		RequestHeader.cs
**
**  Functional Description:
**
**      This class is used to store and retrive some of the MQ header information that needs to be restored 
**      when sending the response to wei client
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
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Telavance.AdvantageSuite.Wei.MQDriver
{
    [Serializable]
    [XmlRoot("header")]
    public class RequestHeader
    {
        [XmlElement(ElementName = "correlationId")]
        public String correlationId;

        public override string ToString()
        {
            var xs = new XmlSerializer(typeof(RequestHeader));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            xs.Serialize(sw, this);
            return sb.ToString();
        }

        public static RequestHeader getRequestHeader(String config)
        {
            var xs = new XmlSerializer(typeof(RequestHeader));

            using (var stringReader = new StringReader(config))
            {
                XmlReader reader = new XmlTextReader(stringReader);
                var requestHeader = (RequestHeader)xs.Deserialize(reader);
                return requestHeader;
            }
        }
    }

}
