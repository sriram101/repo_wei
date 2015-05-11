using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;

[assembly: CLSCompliant(true)]
namespace Telavance.AdvantageSuite.Wei.MQDriver
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

            FileInfo info = new FileInfo(dir);
            if (info.Exists == false)
            {
                throw new InstallException("File " + dir + " does not exist");
            }
            
            dir = dir.Substring(0, dir.Length - 1).Replace('\\', '/');

            StreamWriter writer = new StreamWriter(dir + "/sql/MQDriver.sql");
            writer.WriteLine("INSERT INTO Drivers (name, dll, type)	VALUES ('MQ Driver', '" + dir + "bin/MQDriver.dll', 'Telavance.AdvantageSuite.Wei.MQDriver.MQDriver')");
            writer.Close();

        }

        public override void Uninstall(IDictionary stateSaver)
        {

            base.Uninstall(stateSaver);
            try
            {
                string dir = this.Context.Parameters["installdir"].ToString();
                dir = dir.Substring(0, dir.Length - 1).Replace('\\', '/'); ;
                if (File.Exists(dir + "/sql/MQDriver.sql"))
                    File.Delete(dir + "/sql/MQDriver.sql");
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }
    }
}
