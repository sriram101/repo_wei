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
using Telavance.AdvantageSuite.Wei.WeiCommon;
using System.Reflection;

namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    public partial class NewForm : Form
    {
        private string serviceName;
        public NewForm()
        {
            InitializeComponent();
            PopulateGridView();
            WeiMonitorConfigurationSection config = config = (WeiMonitorConfigurationSection)ConfigurationManager.GetSection("WeiServiceManager");
            if (config != null)
            {
                serviceName = config.ServiceName;

            }
        }
        private void PopulateGridView()
        {

            
            refresh();
        }


        private void refresh()
        {
            frmInterfaceGrid.Rows.Clear();
            WeiMonitoringClient client = new WeiMonitoringClient();
            try
            {
                
                if (client != null)
                {
                    InterfaceStatus[] interfaceStatus = client.getInterfaces();

                    for (int i = 0; i < interfaceStatus.Length; i++)
                    {
                        string[] row = new string[] { "" + interfaceStatus[i].InterfaceId, interfaceStatus[i].InterfaceName, interfaceStatus[i].Status.ToString() };
                        frmInterfaceGrid.Rows.Add(row);
                    }
                }

            }
            catch (Exception ex)
            {

                frmInterfaceGrid.Rows.Clear();
                StatusLabel.Text = "WEI Service is not currently running";
                //LogUtil.log("", ex.InnerException);

            }
            finally
            {
                
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
                            if (client != null)
                            {
                                client.forceShutdown();

                                // Use the 'client' variable to call operations on the service.

                                // Always close the client.
                                client.Close();
                                StatusLabel.Text = "WEI Service was forecefully shutdown.";
                            }
                        }
                        break;
                    default:
                        StatusLabel.Text = "WEI Service stopped.";
                        break;
                }

            }
            catch (Exception ex)
            {
                frmInterfaceGrid.Rows.Clear();
                //MessageBox.Show("Service is not running now!");
                StatusLabel.Text = "WEI Service is not currently running";
                //LogUtil.log("", ex.InnerException);
            }
        }

        private void btnGracefulShutdown_Click(object sender, EventArgs e)
        {
            try
            {
                WeiMonitoringClient client = new WeiMonitoringClient();
                if (client != null)
                {
                    client.ShutdownGracefully();

                    // Use the 'client' variable to call operations on the service.

                    // Always close the client.
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                frmInterfaceGrid.Rows.Clear();
                //MessageBox.Show("Service is not running now!");
                StatusLabel.Text = "WEI Service is not running.";
                //LogUtil.log("", ex.InnerException);
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
                        StatusLabel.Text = "WEI Service started successfully";
                    }
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        StatusLabel.Text = "WEI Service stopped";
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "WEI Service cannot be started. Please check the event viewer for any errors.";
                    

                }
            }
            else
            {
                //MessageBox.Show("Service is not yet stopped");
                StatusLabel.Text = "WEI Service is already running. ";
            }
        }



        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            refresh();
        }

      
        private void frmInterfaceGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (frmInterfaceGrid.RowCount > 0)
                {
                    if (e.ColumnIndex == 3)
                    {
                        string message = "Unable to get message statistics";
                        object value = frmInterfaceGrid.Rows[e.RowIndex].Cells[0].Value;

                        WeiMonitoringClient client = new WeiMonitoringClient();
                        if (client != null)
                        {
                            message = client.getStatistics(int.Parse(value.ToString()));
                            // Use the 'client' variable to call operations on the service.

                            // Always close the client.
                            client.Close();
                            MessageBox.Show(message);
                        }


                    }
                    if (e.ColumnIndex == 4)
                    {
                        //Application.Run(new ShowRequestsWithErrors());
                        //ShowRequestsWithErrors.ActiveForm.ShowDialog();
                        int _interfaceID;
                        _interfaceID = Int32.Parse(frmInterfaceGrid.Rows[e.RowIndex].Cells[0].Value.ToString());
                        ShowRequestsWithErrors newForm = new ShowRequestsWithErrors(_interfaceID);
                        newForm.ShowDialog();

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
               
            }
        }
        }
        


}
