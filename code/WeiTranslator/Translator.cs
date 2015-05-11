/*
**  File Name:		Translator.cs
**
**  Functional Description:
**
**      This class is used for ctc code conversion and translation. Wei uses this class for all its translation needs
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
09/23/2011       RL         Fixed the issue with plain text format where the user 
 *                          defined string is not getting appended to the translation
 *                         
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using Telavance.AdvantageSuite.Wei.WeiCommon;
//using Telavance;
//using Telavance.AdvantageSuite.Wei.Wei
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

[assembly:CLSCompliant(true)]
namespace Telavance.AdvantageSuite.Wei.WeiTranslator
{

    public delegate string Preprocess(string message);
     


    public class Word
    {
        public StringBuilder precedingChars = new StringBuilder();
        public String ctcCode = null;
        public StringBuilder succeedingChars = new StringBuilder();
    }

    public class Translator
    {
        private String _currentLanguage;
        private String _currentTranslationProvider;
        private DBUtils _dbutils;
        
        private IDictionary<String, MapFile> mapFiles = new Dictionary<String, MapFile>();

        private IDictionary<String, ITranslationProvider> translationProviders = new Dictionary<String, ITranslationProvider>();

        private IList<Regex> translationExceptions = new List<Regex>();

        private String hasCTCpattern = ".*\\s+([%allowedchars%]*\\d{4}[%allowedchars%]*\\s+){%count%,}.*";
        private String singleCTCpattern = "^[%allowedchars%]*(\\d{4})[%allowedchars%]*$";
        private Regex singleCTCpatternRegex;

        private int _determiningCTCCount;
        private bool _applyCustomName;
        private string _sCustomName;
        private bool _requiresReview;

        
        public Translator(WeiConfiguration weiConfig)
        {

            TranslateConfigElement config = weiConfig.TranslatorSetting;
            _applyCustomName = weiConfig.applyCustomName;
            _sCustomName = weiConfig.customName;
            _requiresReview = weiConfig.requiresReview;

            this._dbutils = EnterpriseLibraryContainer.Current.GetInstance<DBUtils>();

            foreach (ProviderConfigElement provider in config.Providers)
            {
                Type type = TypeUtil.getType(null, provider.ClassName, typeof(ITranslationProvider));
                ITranslationProvider translationProvider = (ITranslationProvider)Activator.CreateInstance(type);

                int maxLength = -1;
                try
                {
                    maxLength = Convert.ToInt32(provider.MaxLength);
                }
                catch (Exception)
                {
                    LogUtil.logInfo("Invalid maxLength for provider:" + provider.Name + ". So defaulting to -1.");
                    maxLength = -1;
                }
                translationProvider.initialize(provider.Key, maxLength, weiConfig);
                translationProviders[provider.Name] = translationProvider;
            }

            this._currentLanguage = config.CurrentLanguage;
            this._currentTranslationProvider = config.CurrentTranslationProvider;

            try
            {
                _determiningCTCCount = Convert.ToInt32(config.CtcDeterminingCount);
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot parse CtcDeterminingCount. CtcDeterminingCount is configured as:" + config.CtcDeterminingCount, e);
                throw new Exception("Invalid CtcDeterminingCount in the config file");
            }

            hasCTCpattern = hasCTCpattern.Replace("%count%", config.CtcDeterminingCount);
            hasCTCpattern = hasCTCpattern.Replace("%allowedchars%", config.CtcAllowedChars);
            singleCTCpattern = singleCTCpattern.Replace("%allowedchars%", config.CtcAllowedChars);

            

            singleCTCpatternRegex = new Regex(singleCTCpattern);

            foreach (MapFileConfigElement mapFileConfig in config.MapFiles)
            {
                try
                {
                    MapFile mapFile = new MapFile(mapFileConfig.Language, mapFileConfig.Path, mapFileConfig.KeyIndex, mapFileConfig.ValueIndex);
                    mapFiles[mapFileConfig.Language] = mapFile;
                }
                catch (Exception e)
                {
                    LogUtil.log("Error loading the map file:" + mapFileConfig.Path + " for language:" + mapFileConfig.Language, e);
                    throw new Exception("Error loading the map file:" + mapFileConfig.Path + " for language:" + mapFileConfig.Language);
                }
            }

            //validate the current language and current translation provider

            if (!mapFiles.ContainsKey(this._currentLanguage))
            {
                LogUtil.logError("Missing mapfile for the current language:" + this._currentLanguage);
                throw new Exception("Missing mapfile for the current language:" + this._currentLanguage);
            }

            if (!translationProviders.ContainsKey(this._currentTranslationProvider))
            {
                LogUtil.logError("Missing translationProvider. current translationProvider is " + this._currentTranslationProvider);
                throw new Exception("Missing translationProvider. current translationProvider is " + this._currentTranslationProvider);
            }

            foreach (TranslationExceptionConfigElement translationExceptionConfig in config.TranslationExceptions)
            {
                Regex regEx = new Regex(translationExceptionConfig.Expression);
                translationExceptions.Add(regEx);
            }
        }

        public String convertAndTranslate(String message, bool translateOnlyCTC)
        {
            return convertAndTranslate(message, translateOnlyCTC, null);
        }

        public String convertAndTranslate(String message, bool translateOnlyCTC, Preprocess handler)
        {
            return convertAndTranslate(_currentLanguage, message, translateOnlyCTC, handler);
        }

        public String convertAndTranslate(String srcLanguage, String message, bool translateOnlyCTC, Preprocess handler)
        {
            return convertAndTranslate(srcLanguage, message, _currentTranslationProvider, translateOnlyCTC, handler);
        }
        public String convertAndTranslate(String srcLanguage, String message, String translationProvider, bool translateOnlyCTC, Preprocess handler)
        {
           
            String converted = convert(srcLanguage, message, translateOnlyCTC, handler);
            if (converted != null)
            {
                if (translateOnlyCTC)
                    //as it is already translated
                    return converted;
                String translated = translate(srcLanguage, converted);
                if (_applyCustomName == true)
                {
                    return message + "[ " + translated + " ]" + _sCustomName;
                }
                else
                {
                    return message + "[ " + translated + " ]";
                }
            }
            else
            {
                return message;
            }
        }

        public String convert(String message, bool translateOnlyCTC, Preprocess handler)
        {
            return convert(_currentLanguage, message, translateOnlyCTC, handler);
        }

        public String convert(String srcLanguage, String message, bool translateOnlyCTC, Preprocess handler)
        {
            if(handler!=null)
                message = handler(message);

            MapFile mapFile = mapFiles[srcLanguage];
            if (mapFile != null)
            {
                string originalMessage = message;
                message = " " + message + " ";
                if (Regex.IsMatch(message, hasCTCpattern))
                {

                    foreach (Regex regEx in translationExceptions)
                    {
                        if (regEx.IsMatch(originalMessage))
                        {
                            //Skipping translation because of the exception rule
                            LogUtil.logInfo("Skipping translation of message:" + originalMessage + ". Because of the exception rule:" + regEx.ToString());
                            return null;
                        }
                    }

                    message = message.Substring(1, message.Length-2);
                    message = convert(srcLanguage, message, mapFile, translateOnlyCTC);
                }
                else

                    message = null;
            }
            return message;
        }

        public String translate(String message)
        {
            return translate(_currentLanguage, message);
        }

        public String translate(String srcLanguage, String message)
        {
            return translate(srcLanguage, message, _currentTranslationProvider);
        }

        public String translate(String srcLanguage, String message, String translationProvider)
        {
            if (!translationProviders.ContainsKey(translationProvider))
            {
                LogUtil.logError("Cannot find the translation provider:" + translationProvider);
                throw new Exception("Cannot find the translation provider:" + translationProvider);
            }



            String translated = translationProviders[translationProvider].translate(message, srcLanguage, "en");

            LogUtil.logDebug("convertedString:" + message + ". Translated:" + translated);

            return translated;
        }

        private String convert(String srcLanguage, String message, MapFile mapFile, bool translateOnlyCTC)
        {

            StringBuilder sb = new StringBuilder();
            StringBuilder currentWord = new StringBuilder();
            List<Word> possibleCtcs = new List<Word>();
            try
            {
                int chrint = 0, lastint;
                char chr;
                StreamReader sr = new StreamReader(new MemoryStream(Encoding.Default.GetBytes(message)));
                while (!sr.EndOfStream)
                {
                    lastint = chrint;
                    chrint = sr.Read();
                    chr = Convert.ToChar(chrint);
                    if (Char.IsWhiteSpace(chr))
                    {
                        if(currentWord.Length>0)
                            processWord(srcLanguage, mapFile, sb, possibleCtcs, currentWord, false, translateOnlyCTC);

                        if (possibleCtcs.Count > 0)
                        {
                            possibleCtcs[possibleCtcs.Count-1].succeedingChars.Append(chr);
                        }
                        else
                        {
                            sb.Append(chr);
                        }
                    }
                    else
                    {
                        currentWord.Append(chr);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            //process any remaining characters
            processWord(srcLanguage, mapFile, sb, possibleCtcs, currentWord, true, translateOnlyCTC);
           
            return sb.ToString();
            
        }

        private void processWord(String srcLanguage, MapFile mapFile, StringBuilder sb, List<Word> possibleCtcs, StringBuilder currentWord, bool bFinish, bool translateOnlyCTC)
        {
            bool bProcessed = false;
            string sCTCCode = "";

            if (currentWord.Length >= 4)
            {
                //could be a ctc. Let us check

                String strWord = currentWord.ToString();
                Match match = singleCTCpatternRegex.Match(strWord);
                if (match.Success && match.Groups.Count == 2)
                {
                    Word word = new Word();
                    word.ctcCode = match.Groups[1].Value;
                    if (currentWord.Length > 4)
                    {
                        int index = strWord.IndexOf(word.ctcCode);
                        if (index > 0)
                        {
                            word.precedingChars.Append(strWord.Substring(0, index));
                        }
                        if ((index + 4) < strWord.Length)
                        {
                            word.succeedingChars.Append(strWord.Substring(index + 4));
                        }
                    }

                    possibleCtcs.Add(word);
                    bProcessed = true;
                }



                //it is a 4 digit integer. So lets us convert
                //sb.Append(getTranslatedCTCCode(mapFile, currentWord.ToString()));

            }


            if (!bProcessed || bFinish)
            {
                if (possibleCtcs.Count > 0)
                {
                    bool bConvert = possibleCtcs.Count >= _determiningCTCCount;
                    StringBuilder currentConvertedWord = new StringBuilder();
                    StringBuilder OriginalWord = new StringBuilder();
                    foreach (Word word in possibleCtcs)
                    {
                        currentConvertedWord.Append(word.precedingChars.ToString());
                        currentConvertedWord.Append(bConvert ? getTranslatedCTCCode(mapFile, word.ctcCode) : word.ctcCode);
                        currentConvertedWord.Append(word.succeedingChars.ToString());

                        OriginalWord.Append(word.precedingChars.ToString());
                        OriginalWord.Append(word.ctcCode);
                        OriginalWord.Append(word.succeedingChars.ToString());
                        sCTCCode = sCTCCode + " " + word.ctcCode;
                    }
                    possibleCtcs.Clear();

                    if (bConvert && translateOnlyCTC)
                    {
                        string translated;
                        translated = "";
                        
                        //Check if the translation exists in the translations table
                        translated = _dbutils.getCTCTranslations(sCTCCode.Trim());
                        if (translated == null)
                        {
                            //Add the translation to the translations table if it does not exist
                            translated = translate(srcLanguage, currentConvertedWord.ToString());
                            _dbutils.addTranslation(sCTCCode.Trim(), currentConvertedWord.ToString(), "", translated);
                            LogUtil.logInfo("New translation added to Translations table for the telegraphic code: " + sCTCCode.Trim());

                        }
                        LogUtil.logInfo("Translation already exists for telegraphic Code:" + sCTCCode.Trim());
                        sb.Append(formattedTranslatedString(OriginalWord.ToString(), translated));
                    }
                    else
                    {
                        sb.Append(currentConvertedWord.ToString());
                    }
                }
                if(!bProcessed)
                    sb.Append(currentWord);
            }
            currentWord.Clear();

        }
        private String getTranslatedCTCCode(MapFile mapFile, String ctcCode)
        {
            if (mapFile.Dictionary[ctcCode] == null)
            {
                LogUtil.logInfo("Cannot find the value for " + ctcCode + " in the mapfile:" + mapFile.MapFileName);
                return ctcCode;
            }
            return mapFile.Dictionary[ctcCode].ToString();
        }

        private String formattedTranslatedString(string message, string translated)
        {
            if (_applyCustomName == true)
            {
                return message + "[ " + translated + " ]" + _sCustomName;
            }
            else
            {
                return message + "[ " + translated + " ]";
            }
        }
    }
}
