/*
**  File Name:		Messages.aspx
**
**  Functional Description:
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	    Translate
**  Creation Date:  01/09/2011
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
**                                                                           **
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

01/09/2011       RP        Initial Version
04/23/2011       RP        changed the namespace

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Telavance.AdvantageSuite.Wei.WeiTranslator;
using System.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Translate : System.Web.UI.Page
    {
        Translator translator;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _translate;
            string _CTCode;
            string _sEnglish;
            string _sChinese = "";
            int _istart;
            int _iend;
            
                if (!Page.IsPostBack)
                {
                    WeiConfiguration weiConfig = (WeiConfiguration)ConfigurationManager.GetSection("Wei");

                    if (weiConfig != null)
                    {

                        translator = new Translator(weiConfig);
                        if (translator != null)
                        {

                            string from = Request.QueryString["from"];
                            string text = Request.QueryString["text"];
                            from = "CTC";
                            text = "1159 5646 7456 2609 2139 4098";
                            _translate = translator.convertAndTranslate(text, true);


                            if (_translate != null)
                            {
                                _istart = _translate.IndexOf("[");
                                _iend = _translate.IndexOf("]");
                                if (_istart > 0)
                                {
                                    _CTCode = _translate.Substring(0, _istart);
                                    //Check if this is a valid CTC Code


                                    lblCTCValue.Text = _CTCode.ToString();

                                    _sEnglish = _translate.Substring(_istart + 1, _iend - (_istart + 1));
                                    lblEnglishValue.Text = _sEnglish.Trim().ToString();

                                    _sChinese = translator.convert(text, false,null);

                                    if (_sChinese != null)
                                    {

                                        lblChineseValue.Text = _sChinese.Trim().ToString();
                                    }
                                    LblError.Visible = false;
                                    lblChinese.Visible = true;
                                    lblCTC.Visible = true;
                                    lblEnglish.Visible = true;
                                }
                                else
                                {
                                    LblError.Text = "The text is not valid for translation.";
                                    LblError.Visible = true;
                                    lblChinese.Visible = false;
                                    lblCTC.Visible = false;
                                    lblEnglish.Visible = false;
                                }

                            }
                        }
                    }
                }
        }
    }
}