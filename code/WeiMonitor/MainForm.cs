using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration;
using Telavance.AdvantageSuite.Wei.WeiService;

namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    public partial class MainForm : Form
    {
        private string serviceName;
        public MainForm()
        {
            InitializeComponent();

            frmInterfaceGrid.ColumnCount = 3;
            frmInterfaceGrid.Columns[0].Name = "Interface ID";
            frmInterfaceGrid.Columns[0].ReadOnly = true;
            frmInterfaceGrid.Columns[1].Name = "Interface Name";
            frmInterfaceGrid.Columns[1].ReadOnly = true;
            frmInterfaceGrid.Columns[2].Name = "Status";
            frmInterfaceGrid.Columns[2].ReadOnly = true;

            frmInterfaceGrid.RowCount = 20;


            //frmInterfaceGrid.AutoSize = true;

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.FlatStyle = FlatStyle.Flat;
            frmInterfaceGrid.Columns.Add(btn);
            btn.HeaderText = "Message Statistics";
            btn.Text = "Message Stats";
            btn.Name = "Statistics";
            btn.UseColumnTextForButtonValue = true;
            refresh();

            WeiMonitorConfigurationSection config = config = (WeiMonitorConfigurationSection)ConfigurationManager.GetSection("WeiServiceManager");

            serviceName = config.ServiceName;
            
        }

        private void frmInterfaceGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (frmInterfaceGrid.RowCount > 0)
            {
                if (e.ColumnIndex == 3)
                {
                    string message = "Unable to get message statistics";
                    object value = frmInterfaceGrid.Rows[e.RowIndex].Cells[0].Value;

                    WeiMonitoringClient client = new WeiMonitoringClient();
                    message = client.getStatistics(int.Parse(value.ToString()));

                    // Use the 'client' variable to call operations on the service.

                    // Always close the client.
                    client.Close();
                     MessageBox.Show(message);
                    //StatusLabel.Text = message;

                }
            }
        }

        private void refresh()
        {
            frmInterfaceGrid.Rows.Clear();
            try
            {
                WeiMonitoringClient client = new WeiMonitoringClient();
                InterfaceStatus[] interfaceStatus = client.getInterfaces();

                for (int i = 0; i < interfaceStatus.Length; i++)
                {
                    string[] row = new string[] { "" + interfaceStatus[i].InterfaceId, interfaceStatus[i].InterfaceName, interfaceStatus[i].Status.ToString() };
                    frmInterfaceGrid.Rows.Add(row);
                }
            }
            catch (CommunicationException ex)
            {
                frmInterfaceGrid.Rows.Clear();
                //MessageBox.Show("Service is not running now!");
                StatusLabel.Text = "Service is not currently running";
            }
        }

        private void btnForceShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                switch (service.Status)
                {
                    case ServiceControllerStatus.Running:
                        string message = null;
                        DialogResult dresult;
                        message = "Do you want to forcefully shutdown the service?. Any messages in the queues that are currently being processed may be lost.";
                        dresult = MessageBox.Show(message, "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        if (dresult == DialogResult.OK)
                        {
                            WeiMonitoringClient client = new WeiMonitoringClient();
                            client.forceShutdown();

                            // Use the 'client' variable to call operations on the service.

                            // Always close the client.
                            client.Close();
                            StatusLabel.Text = "Service stopped.";
                        }
                        break;
                        default:
                        StatusLabel.Text = "Service stopped.";
                        break;
                }
                
            }
            catch (CommunicationException ex)
            {
                frmInterfaceGrid.Rows.Clear();
                //MessageBox.Show("Service is not running now!");
                StatusLabel.Text = "Service is not currently running";
            }
        }

        private void btnGracefulShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                WeiMonitoringClient client = new WeiMonitoringClient();
                client.ShutdownGracefully();

                // Use the 'client' variable to call operations on the service.

                // Always close the client.
                client.Close();
            }
            catch (CommunicationException ex)
            {
                frmInterfaceGrid.Rows.Clear();
                //MessageBox.Show("Service is not running now!");
                StatusLabel.Text = "Service is not running.";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(6000);

                    service.Start();
                   service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        // MessageBox.Show("Service started successfully");
                        StatusLabel.Text = "Service started successfully";
                    }
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        StatusLabel.Text = "Service stopped";
                    }
                }
                catch(Exception ex)
                {
                    //MessageBox.Show("Cannot start the service!");
                    StatusLabel.Text = "Service cannot be started. Please check the event viewer for any errors.";
                }
            }
            else
            {
                //MessageBox.Show("Service is not yet stopped");
                StatusLabel.Text = "Service is already running. ";
            }
        }

       
      

        
    }
}
