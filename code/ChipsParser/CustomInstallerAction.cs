using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Telavance.AdvantageSuite.Wei.ChipsParser
{
    [RunInstaller(true)]
    public partial class CustomInstallerAction : System.Configuration.Install.Installer
    {
        public CustomInstallerAction()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {

            base.Install(stateSaver);


            //var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyPath = this.Context.Parameters["installdir"].ToString() + "bin/" +this.Context.Parameters["wei"].ToString(); ;
            

            StreamReader reader = new StreamReader(assemblyPath + ".config");
            string content = reader.ReadToEnd();
            reader.Close();
            string dir = this.Context.Parameters["installdir"].ToString();
            dir = dir.Substring(0, dir.Length - 1).Replace('\\', '/');
            dir = dir +  "bin/" + "ChipsParser.dll";
            string searchText = "ChipsParser.dll";
            content = Regex.Replace(content, searchText, dir );

            StreamWriter writer = new StreamWriter(assemblyPath + ".config");
            writer.Write(content);
            writer.Close();




        }
    }
}
