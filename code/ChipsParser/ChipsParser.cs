using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telavance.AdvantageSuite.Wei.WeiService;
using Telavance.AdvantageSuite.Wei.WeiTranslator;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    public class ChipsParser : IConfigurableMessageParser
    {
        private bool isIntialized = false;
        private static string pattern = "^\\s*\\{[0-9]{1,}\\}";
        private Regex regex = new Regex(pattern);
        private Translator _translator;
        private string _searchString;
        private string _replaceString;

        private static List<string> tags = new List<string>();
        
        private bool translateAlltags = false;

        WeiConfiguration weiConfig = (WeiConfiguration)ConfigurationManager.GetSection("Wei");
            
            

        public bool translate(Request request, String message)
        {
            if (isIntialized)
            {
                Match match = regex.Match(message);
                if (match.Success)
                {
                    parseMessage(request, message);
                    return true;
                }
            }
            return false;
        }
        
        private void parseMessage(Request request, string text)
        {
            string originalText = text;
            request.HasCTC = false;
            StringBuilder translatedText = new StringBuilder();
            _searchString = weiConfig.searchString;
            _replaceString = weiConfig.replaceString;

            while (text!=null) //enless loop
            {
                int indexEnd = text.IndexOf("{", 1);
                string currentTag = (indexEnd!=-1) ? text.Substring(0, indexEnd): text;
                text = (indexEnd!=-1) ? text.Substring(indexEnd):null;
                if (processTag(request.RequestId,translatedText, currentTag, translateAlltags ? null : tags))
                        request.HasCTC = true;
            }
            
            if (_searchString != "")
            {
                request.TranslatedMessage = translatedText.ToString().Replace(_searchString, " " + _replaceString + " ");
            }
            

            return;
        }

       private string getText(string tag, string startDelim, string endDelim)
        {
            int indexStart = tag.IndexOf(startDelim, 0);
            if (indexStart != -1)
            {
                int indexEnd = tag.IndexOf(endDelim, indexStart);
                if (indexEnd != -1)
                {
                    return (indexStart != -1 && indexEnd != -1) ? tag.Substring(indexStart + 1, indexEnd - indexStart - 1).Trim() : "";
                }
            }

            return "";
            
        }

        

       private bool processTag(int requestId,StringBuilder translatedText, string currentTag, List<string> currentTagsToTranslate)
        {
            Console.WriteLine("Current Tag:" + currentTag);
            string tag = getText(currentTag, "{", "}");
            Console.WriteLine("Current  tag:" + tag);

            bool translateReqd = true;
            if (currentTagsToTranslate != null)
            {
                if (!currentTagsToTranslate.Contains(tag))
                    translateReqd = false;
            }

            if (translateReqd)
            {
                int index = currentTag.IndexOf("}");
                if (index != -1)
                {
                    string tagText = currentTag.Substring(0, index + 1);
                    string tagContents = currentTag.Substring(index + 1);

                    //string translatedTagContents = _translator.convertAndTranslate(tagContents, false, new Preprocess(preProcess));
                    string translatedTagContents = translate(requestId,tagContents);

                    translatedText.Append(tagText);
                    translatedText.Append(translatedTagContents);
                    return !tagContents.Equals(translatedTagContents);
                }
                else
                {
                    translatedText.Append(currentTag);
                }
            }
            else
            {
                translatedText.Append(currentTag);
            }

            return false;
        }


        private string translate(int requestId,string text)
        {
            StringBuilder output = new StringBuilder();
            
            while (text != null)
            {
                int index = text.IndexOf("*");
                string tag = index == -1 ? text : text.Substring(0, index);
                text = index == -1 ? null : text.Substring(index + 1);
                _translator.RequestId = requestId;
                string translatedTagContents = _translator.convertAndTranslate(tag, false, null);
                output.Append(translatedTagContents);
                if (text != null)
                {
                    output.Append("*");
                }                

            }

            return output.ToString();
        }

        private string preProcess(string message)
        {
            return message.Replace('*', ' ');
        }

        public void configure(Translator translator, string param1, string param2, string param3, string param4, string param5)
        {
            try
            {
                _translator = translator;
                ChipsConfigurationSection config = (ChipsConfigurationSection)ConfigurationManager.GetSection("Chips");

                translateAlltags = Boolean.Parse(config.TranslateAllTags);

                if (translateAlltags)
                {
                    isIntialized = true;
                    return;
                }

                foreach (TagConfigElement tag in config.Tags)
                {
                    tags.Add(tag.Id.Trim());
                }

                isIntialized = true;
            }
            catch (Exception e)
            {
                isIntialized = false;
                LogUtil.log("Error loading config for ChipsParser", e);
                throw;
            }
        }
    }
}
