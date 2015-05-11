/*
**  File Name:		Request.cs
**
**  Functional Description:
**
**      This class represent each request handled by the wei service.
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

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class Request
    {
        private int _requestId;
        private int _interfaceId;
        private String _name;
        private String _header;
        private String _messageBody;
        private Status _status;
        private bool _isError = false;
        private bool _isCTCApproved = false;
        private OfacStatus _ofacStatus = OfacStatus.None;
        private String _translatedMessage = null;
        private String _responseMessage = null;
        private bool _hasCTC = false;
        private DateTime _dCreateDate;
        private DateTime _dModifiedDate;
        private String _user;

        public int RequestId
        {
            get
            {
                return _requestId;
            }
            set
            {
                _requestId = value;
            }
        }
        
        public int InterfaceId
        {
            get
            {
                return _interfaceId;
            }
            set
            {
                _interfaceId = value;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public String Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
            }
        }

        public String MessageBody
        {
            get
            {
                return _messageBody;
            }
            set
            {
                _messageBody = value;
            }
        }

        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public DateTime CreateDate
        {
            get
            {
                return _dCreateDate;
            }
            set
            {
                _dCreateDate = value;
            }
        }

        public DateTime ModifiedDate
        {
            get
            {
                return _dModifiedDate;
            }
            set
            {
                _dModifiedDate = value;
            }
        }

        public bool IsError
        {
            get
            {
                return _isError;
            }
            set
            {
                _isError = value;
            }
        }

        public bool hasCTCApproved
        {
            get
            {
                return _isCTCApproved;
            }
            set
            {
                _isCTCApproved = value;
            }
        }

        public OfacStatus OfacStatus
        {
            get
            {
                return _ofacStatus;
            }
            set
            {
                _ofacStatus = value;
            }
        }

        public bool HasCTC
        {
            get
            {
                return _hasCTC;
            }
            set
            {
                _hasCTC = value;
            }
        }
        public String TranslatedMessage
        {
            get
            {
                return _translatedMessage;
            }
            set
            {
                _translatedMessage = value;
            }
        }

        public String ResponseMessage
        {
            get
            {
                return _responseMessage;
            }
            set
            {
                _responseMessage = value;
            }
        }

        public String CreateOper
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
    }
}
