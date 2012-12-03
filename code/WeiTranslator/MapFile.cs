/*
**  File Name:		MapFile.cs
**
**  Functional Description:
**
**      This class is used reading the map file for ctc code translation
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

using System.Collections;
using System.IO;

using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{

    public class MapFile
    {
        private string _mapFileName;
        private string _language;
        private int _keyIndex;
        private int _valueIndex;
        private Hashtable _dictionary;


        public Hashtable Dictionary
        {
            get { return _dictionary; }
        }

        public MapFile(string language, string filename, int keyIndex, int valueIndex)
        {
            _language = language;
            _mapFileName = filename;
            this._keyIndex = keyIndex;
            this._valueIndex = valueIndex;
            initiliaze();
        }

        public String MapFileName
        {
            get
            {
                return _mapFileName;
            }
        }

        public String Language
        {
            get
            {
                return _language;
            }
        }
        public void initiliaze()
        {
            string[] line;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(_mapFileName);
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split('\t');
                    if (_dictionary == null)
                        _dictionary = new Hashtable();
                    try
                    {
                        _dictionary.Add(line[_keyIndex], line[_valueIndex]);
                    }
                    catch (ArgumentException ex)
                    {
                        if (line != null)
                            LogUtil.logInfo("Error reading mapfile:" + _mapFileName + " for line:" + line+".Exception Message:" + ex.Message);

                    }
                }
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}
