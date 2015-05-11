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
using Telavance.AdvantageSuite.Wei.WeiService;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace TranslationTester
{
    public partial class Form2 : Form
    {


        public Form2()
        {
            InitializeComponent();

            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DBUtil _dbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();
            Request request =  _dbUtils.getRequest(310);

            this.input.Text = request.TranslatedMessage;
        }


    }
}
