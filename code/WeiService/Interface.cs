/*
**  File Name:		Interface.cs
**
**  Functional Description:
**
**      This class represent all the configured interfaces that will be loaded by wei service.
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
    public class Interface
    {
        private readonly int _id;
        private readonly string _name;
        private readonly IHandler _handler;
        private readonly IDriver _driver;
        private readonly string[] _fileFormats;

        public Interface(int id, string name, IHandler handler, IDriver driver, String fileFormat)
        {
            this._id = id;
            this._name = name;
            this._driver = driver;
            this._handler = handler;
            _fileFormats = fileFormat.Split(',');
            for (int i = 0; i < _fileFormats.Length; i++)
            {
                _fileFormats[i] = _fileFormats[i].Trim();
            }
        }
        public int InterfaceId
        {
            get
            {
                return _id;
            }
        }
        public string InterfaceName
        {
            get
            {
                return _name;
            }
        }
        public IHandler Handler
        {
            get
            {
                return _handler;
            }
        }
        public IDriver Driver
        {
            get
            {
                return _driver;
            }
        }

        public String[] FileFormats
        {
            get
            {
                return _fileFormats;
            }
        }
    }
}
