using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

using Telavance.AdvantageSuite.Wei.WeiService;
using Telavance.AdvantageSuite.Wei.WeiTranslator;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public class XmlParser : IConfigurableMessageParser
    {
        private IList<string> xmlTags = new List<string>();
        private string delimiter;
        private  Translator _translator;
        private string tagsToCheck = null;
        private string rootNodeToAppend = null;
        private Regex startTagToCheck = null;
        private Regex endTagToCheck = null;

        public void configure(Translator translator, string param1, string param2, string param3, string param4, string param5)
        {
            this._translator = translator;
            string[] tags = param1.Split(',');
            foreach (string tag in tags)
            {
                xmlTags.Add(tag);
            }

            delimiter = param2;
            if (param3 != null && param3.Trim().Length > 0)
            {
                tagsToCheck = param3.Trim();
                startTagToCheck = new Regex("<\\s*" + tagsToCheck + "\\s*>");
                endTagToCheck = new Regex("<\\s*/\\s*" + tagsToCheck + "\\s*>");
                
            }

            if (param4 != null && param4.Trim().Length > 0)
            {
                rootNodeToAppend = param4.Trim();
            }
        }

        private XmlDocument canHandle(String message)
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(message);
                //first check if the message is plain text

                XmlNode root = document.DocumentElement;
                if (root.ChildNodes.Count == 1 && root.FirstChild.FirstChild == null)
                {
                    //this is plain text
                    return null;
                }

                if (tagsToCheck != null)
                {
                    if (!(startTagToCheck.IsMatch(message) && endTagToCheck.IsMatch(message)))
                    {
                        document = null;
                    }
                }
            }
            catch (XmlException e)
            {
                //may be this is not a valid xml
                document = null;
            }
            return document;
        }

        public bool translate(Request request, String message)
        {
            if (rootNodeToAppend != null)
            {
                message = "<" + rootNodeToAppend + ">" + message + "</" + rootNodeToAppend + ">";
            }
            
            try
            {
                XmlDocument document = canHandle(message);
                if (document == null)
                {
                    return false;
                }
                document.LoadXml(message);
                XmlNode root = document.DocumentElement;
                IEnumerator ienum = root.GetEnumerator();
                XmlNode node;
                bool hasCTC = false;
                while (ienum.MoveNext())
                {
                    node = (XmlNode)ienum.Current;
                    if (process(node, null))
                    {
                        hasCTC = true;
                    }
                }

                string translatedText = document.OuterXml;

                if(rootNodeToAppend!=null)
                {
                    int index = translatedText.IndexOf("<" + rootNodeToAppend + ">");
                    translatedText = translatedText.Remove(index, ("<" + rootNodeToAppend + ">").Length);
                    index = translatedText.LastIndexOf("</" + rootNodeToAppend + ">");
                    translatedText = translatedText.Remove(index, ("</" + rootNodeToAppend + ">").Length);
                }

                request.TranslatedMessage = translatedText;

                request.HasCTC = hasCTC;

                return true;
            }
            catch (XmlException e)
            {
                //may be this is not a valid xml
            }

            return false;
        }

        public bool process(XmlNode node, string tag)
        {
            bool bHasCTC = false;
            string currentTag = tag == null ? node.Name : tag + delimiter + node.Name;

            if (node.HasChildNodes)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].HasChildNodes)
                    {
                        if (process(node.ChildNodes[i], currentTag))
                        {
                            bHasCTC = true;
                        }
                    }else{
                        if (node.ChildNodes[i].NodeType == XmlNodeType.Text && xmlTags.Contains(currentTag))
                        {
                            string translatedText = _translator.convertAndTranslate(node.ChildNodes[i].InnerText, false);
                            if (!bHasCTC)
                            {
                                bHasCTC = !translatedText.Equals(node.ChildNodes[i].InnerText);
                            }
                            node.ChildNodes[i].InnerText = translatedText;
                        }
                    }
                }
            }
            return bHasCTC;
        }
    }
}
