using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

using System.Configuration;

using System.IO;
using System.Text.RegularExpressions;


namespace Telavance.AdvantageSuite.Wei.WeiService
{
    [RunInstaller(true)]
    public partial class CustomInstallAction : System.Configuration.Install.Installer
    {
        public CustomInstallAction()
        {
            InitializeComponent();
        }


        public override void Install(IDictionary stateSaver)
        {

            base.Install(stateSaver);



            /*Directory.Delete("c://laks2", true);
            Directory.CreateDirectory("c://laks2");

            StreamWriter sw = File.CreateText("c://laks2//dump.txt");

            foreach (string key in this.Context.Parameters.Keys)
            {
                sw.WriteLine(key + ":" + this.Context.Parameters[key].ToString());
            }

            sw.Flush();
            sw.Close();*/

            var assemblyPath = this.Context.Parameters["assemblypath"];


            StreamReader reader = new StreamReader(assemblyPath+".config");
            string content = reader.ReadToEnd();
            reader.Close();
            string dir = this.Context.Parameters["installdir"].ToString();
            dir = dir.Substring(0, dir.Length - 1).Replace('\\','/');
            
            string searchText = "C:/wei/Code/WeiService/bin/Release/";
            content = Regex.Replace(content, searchText, dir);
            content = Regex.Replace(content, "key=\".*\"", "key=\"Enter Key\"");

            StreamWriter writer = new StreamWriter(assemblyPath + ".config");
            writer.Write(content);
            writer.Close();

            //couldnt use the below code because configelements are in a differnt dll. Ahhhhhh

            /*var config = ConfigurationManager.OpenExeConfiguration(assemblyPath);

            WeiConfiguration weiConfig = (WeiConfiguration)config.GetSection("Wei");
            string dir = this.Context.Parameters["installdir"].ToString();
            dir = dir.Substring(0, dir.Length - 1).Replace('\\','/');

            foreach (MapFileConfigElement mapFileConfig in weiConfig.TranslatorSetting.MapFiles)
            {
                String path = mapFileConfig.Path;
                int index = path.LastIndexOf("/");
                mapFileConfig.Path = dir + path.Substring(index);
            }

            weiConfig.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);*/


        }
    }
}
