/*
**  File Name:		Status.cs
**
**  Functional Description:
**
**      This class contains all the status used by wei service
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
using System.Runtime.Serialization;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    [DataContract]
    public enum DriverStatus : int
    {
        [EnumMember]
        NotRunning=1,
        [EnumMember]
        Starting=2,
        [EnumMember]
        Running=3,
        [EnumMember]
        Shuttingdown=4,
        [EnumMember]
        Error=5
    }

    public enum Status : int
    {
        Unprocessed=1,
        Translated=2,
        SentForOfacCheck=3,
        OfacResponseReceived=4,
        Processed=5
    }

    public enum OfacStatus : int

    {
        None=1,
        OK=2,
        Confirm=3
    }

}
