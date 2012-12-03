/*
**  File Name:		SwiftParser.cs
**
**  Functional Description:
**
**      This is the default parser for swift messages.
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

using Telavance.AdvantageSuite.Wei.WeiTranslator;
using System.IO;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Telavance.AdvantageSuite.Wei.WeiService;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class SwiftParser:IMessageParser
    {
        private readonly Translator _translator;
        private readonly List<string> _tagsToTranslate = new List<string>();
        private readonly bool translateAllTags;
        
        public SwiftParser(Translator translator, SwiftConfigElement config)
        {
            this._translator = translator;

            translateAllTags = Convert.ToBoolean(config.TranslateAllTags);

            foreach (SwiftTagConfigElement tag in config.Tags)
            {
                _tagsToTranslate.Add(tag.Name);
            }
        }

        public bool translate(Request request, String message)
        {
            message = message.Trim();
            if (isSwiftMessage(message))
            {
                parseMessage(request, message);
                return true;
            }
            return false;
        }

        private bool isSwiftMessage(String message)
        {
            return message.StartsWith("{1:") && message.Length > 28 && "}".Equals(message.Substring(28, 1));
        }


        internal void parseMessage(Request request, string text)
        {
            bool hasCTC = false;
            StringBuilder translatedText = new StringBuilder();
            StringBuilder currentTag = new StringBuilder();

            //stringreader is used to crawl through the text
            var strReader = new StringReader(text); //initialize a StringReader
            while (true) //enless loop
            {
                string line = strReader.ReadLine();
                if (line == null) //if the line is empty then EOstring
                    break; //so end iterating
                String firstChar = "";
                if(line.Length>0)
                {
                    firstChar = line.Substring(0, 1);
                }
                switch (firstChar)
                {
                    case "{":
                        processHeader(translatedText, currentTag, line);
                        break;
                    case "-":
                        processFooter(translatedText,currentTag,line);
                        break;
                    case ":":
                        bool retValue = processTag(translatedText,currentTag);
                        if (!hasCTC)
                            hasCTC = retValue;
                        currentTag.Append(line);
                        break;
                    default:
                        currentTag.Append("\n\r");;
                        currentTag.Append(line);
                        break;

                }
            }

            request.TranslatedMessage = translatedText.ToString();
            request.HasCTC = hasCTC;
        }

        void processHeader(StringBuilder translatedText, StringBuilder currentTag, String currentLine)
        {

            processTag(translatedText, currentTag);
            translatedText.Append(currentLine);
            translatedText.Append("\r\n");
        }

        void processFooter(StringBuilder translatedText, StringBuilder currentTag, String currentLine)
        {

            processTag(translatedText, currentTag);
            translatedText.Append(currentLine);
            translatedText.Append("\r\n");
        }

        bool processTag(StringBuilder translatedText, StringBuilder currentTag)
        {
            bool bTranslate = false;
            if (currentTag.Length > 0)
            {
                String tagText = currentTag.ToString();
                int tagIndexEnd = tagText.IndexOf(':', 1);
                if (tagIndexEnd == -1)
                {
                    translatedText.Append(tagText);
                }
                else
                {
                    String tag = tagText.Substring(0, tagIndexEnd + 1);
                    String tagContents = tagText.Substring(tagIndexEnd + 1);
                    translatedText.Append(tag);
                    tag = tagText.Substring(1, tagIndexEnd - 1);
                    if (translateAllTags || _tagsToTranslate.Contains(tag))
                    {
                        string translatedTagContents = _translator.convertAndTranslate(tagContents, false);
                        translatedText.Append(translatedTagContents);
                        bTranslate = !tagContents.Equals(translatedTagContents);
                    }
                    else
                        translatedText.Append(tagContents);
                }
                translatedText.Append("\r\n");
                currentTag.Clear();                
            }
            return bTranslate;
        }
    }
}
