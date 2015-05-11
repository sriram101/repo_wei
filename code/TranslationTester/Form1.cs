using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Configuration;

using Telavance.AdvantageSuite.Wei.WeiCommon;
using Telavance.AdvantageSuite.Wei.WeiTranslator;

namespace TranslationTester
{
    public partial class Form1 : Form
    {

        Translator translator;

        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WeiConfiguration weiConfig = (WeiConfiguration)ConfigurationManager.GetSection("Wei");

            translator = new Translator(weiConfig);

            foreach (MapFileConfigElement mapFileConfig in weiConfig.TranslatorSetting.MapFiles)
            {
                srcLanguage.Items.Add(mapFileConfig.Language);
            }

            


            providers.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (srcLanguage.SelectedIndex ==  - 1)
            {
                MessageBox.Show("Select a source language before converting" );
                return;
            }

            

            string language = srcLanguage.Items[srcLanguage.SelectedIndex].ToString();
            //string translationProvider = srcLanguage.Items[srcLanguage.SelectedIndex].ToString();

            String convertedString = translator.convert(language, input.Text, false, null);

            converted.Text = convertedString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (srcLanguage.SelectedIndex == -1)
            {
                MessageBox.Show("Select a source language before converting");
                return;
            }

            if (providers.SelectedIndex ==  - 1)
            {
                MessageBox.Show("Select a translation provider before converting" );
                return;
            }

            string language = srcLanguage.Items[srcLanguage.SelectedIndex].ToString();
            string translationProvider = providers.Items[providers.SelectedIndex].ToString();
            try
            {
                String translatedString = translator.translate(language, converted.Text, translationProvider);

                translated.Text = translatedString;
            }
            catch (Exception ex)
            {
                translated.Text = ex.Message;
            }

        }

    }
}
