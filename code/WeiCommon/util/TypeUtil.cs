/*
**  File Name:		TypeUtil.cs
**
**  Functional Description:
**
**      This class is used to create types dynamically using reflection
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
using System.Reflection;

namespace Telavance.AdvantageSuite.Wei.WeiCommon
{
    public class TypeUtil
    {
        public static Type getType(String dll, String typeName, Type expected)
        {
            try
            {
                Assembly assembly =null;
                if(dll != null)
                    assembly = Assembly.LoadFrom(dll);
                else
                    assembly = Assembly.GetCallingAssembly();

                Type returnType = null;
                if (typeName == null || typeName.Trim().Length == 0)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type t in types)
                    {
                        if (expected.IsAssignableFrom(t))
                        {
                            returnType = t;
                            break;
                        }

                    }
                }
                else
                {
                    returnType = assembly.GetType(typeName.Trim(), false, false);

                }

                return returnType;
            }
            catch (Exception e)
            {
                LogUtil.log("Error getting type:" + typeName + " from dll " + dll, e);
            }
            return null;
        }
    }
}
