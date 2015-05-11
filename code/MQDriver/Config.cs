/*
**  File Name:		Config.cs
**
**  Functional Description:
**
**      This class is used to serialize and deserialize config for MQ driver
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
    [XmlRoot("config")]
    public class Config
    {
        [XmlElement(ElementName = "remote")]
        public String remote = "false";

        [XmlElement(ElementName = "queueManager")]
        public String queueManager;

        //ip address
        [XmlElement(ElementName = "channelName")]
        public String channelName;

        
        [XmlElement(ElementName = "connectionName")]
        public String connectionName;

        [XmlElement(ElementName = "inputQueue")]
        public String inputQueue;

        [XmlElement(ElementName = "okQueue")]
        public String okQueue;

        [XmlElement(ElementName = "confirmQueue")]
        public String confirmQueue;

        [XmlElement(ElementName = "errorQueue")]
        public String errorQueue;

        [XmlElement(ElementName = "reviewQueue")]
        public String reviewQueue;

        [XmlElement(ElementName = "ofacInputQueue")]
        public String ofacInputQueue;

        [XmlElement(ElementName = "ofacOkQueue")]
        public String ofacOkQueue;

        [XmlElement(ElementName = "ofacConfirmQueue")]
        public String ofacConfirmQueue;

        [XmlElement(ElementName = "shouldMoveToError")]
        public bool shouldMoveToError = true;

        [XmlElement(ElementName = "shouldMoveToReview")]
        public bool shouldMoveToReview = true;

        public bool isRemote()
        {
            return Convert.ToBoolean(remote);
        }

        public bool isValid()
        {
            if(isRemote())
                return queueManager != null && connectionName != null && channelName != null && inputQueue != null && okQueue != null && confirmQueue != null && errorQueue != null && ofacInputQueue != null && ofacOkQueue != null && ofacConfirmQueue != null;
            else
                return queueManager != null && inputQueue != null && okQueue != null && confirmQueue != null && errorQueue != null && ofacInputQueue != null && ofacOkQueue != null && ofacConfirmQueue != null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("remote:");
            sb.Append(isRemote());

            sb.Append(";queueManager:");
            sb.Append(queueManager);

            sb.Append(";connectionName:");
            sb.Append(connectionName);

            sb.Append(";channelName:");
            sb.Append(channelName);

            sb.Append(";inputQueue:");
            sb.Append(inputQueue);

            sb.Append(";okQueue:");
            sb.Append(okQueue);

            sb.Append(";confirmQueue:");
            sb.Append(confirmQueue);

            sb.Append(";errorQueue:");
            sb.Append(errorQueue);

            sb.Append(";reviewQueue:");
            sb.Append(reviewQueue);

            sb.Append(";ofacInputQueue:");
            sb.Append(ofacInputQueue);

            sb.Append(";ofacOkQueue:");
            sb.Append(ofacOkQueue);

            sb.Append(";ofacConfirmQueue:");
            sb.Append(ofacConfirmQueue);


            
            return sb.ToString();
        }

        public static Config getConfig(String config)
        {
            var xs = new XmlSerializer(typeof(Config));

            using (var stringReader = new StringReader(config))
            {
                XmlReader reader = new XmlTextReader(stringReader);
                var newConfig = (Config)xs.Deserialize(reader);
                return newConfig;
            }
        }

    }
}
