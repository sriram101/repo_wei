using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;

namespace Telavance.AdvantageSuite.Wei.FileDriver
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

            string dir = this.Context.Parameters["installdir"].ToString();
            dir = dir.Substring(0, dir.Length - 1).Replace('\\', '/');

            StreamWriter writer = new StreamWriter(dir + "/sql/FileDriver.sql");
            writer.WriteLine("INSERT INTO Drivers (name, dll, type)	VALUES ('File Driver', '" + dir + "bin/FileDriver.dll', 'Telavance.AdvantageSuite.Wei.FileDriver.FileDriver')");
            writer.Close();

        }

        public override void Uninstall(IDictionary stateSaver)
        {

            base.Uninstall(stateSaver);
            try
            {
                string dir = this.Context.Parameters["installdir"].ToString();
                dir = dir.Substring(0, dir.Length - 1).Replace('\\', '/'); 

                if (File.Exists(dir + "/sql/FileDriver.sql"))
                    File.Delete(dir + "/sql/FileDriver.sql");
            }
            catch (Exception e)
            {
                //swallow the error
            }
        }
    }
}
