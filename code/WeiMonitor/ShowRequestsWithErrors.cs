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
using Telavance.AdvantageSuite.Wei.WeiService;

namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    public partial class ShowRequestsWithErrors : Form
    {
        private int _pageCount = 1;
        private int _maxRec = 0;
        private int _pageSize  = 10;
        private int _currentPage = 1;
        private int _recNo;
        private int _intefaceID;

        public ShowRequestsWithErrors(int InterfaceID)
        {
            InitializeComponent();
            _intefaceID = InterfaceID;
            PopulateGridWithErrorMessages();
        }


        void PopulateGridWithErrorMessages()
        {
            try
            {
                WeiMonitoringClient client = new WeiMonitoringClient();
                if (client != null)
                {
                    Telavance.AdvantageSuite.Wei.WeiService.Request[] values = client.getAllErrors(_intefaceID);
                    
                    

                    if (values != null)
                    {
                        if (frmErrorMessages.Rows.Count > 0)
                        {
                            frmErrorMessages.DataSource = null;
                        }
                        
                        BindingSource bindingSource = new BindingSource();
                        bindingSource.DataSource = values;
                        frmErrorMessages.AutoGenerateColumns = false;
                        frmErrorMessages.DataSource = bindingSource;
                        frmErrorMessages.AutoSize = true;
                        
                     
                    }
                    
                }
            }
            catch (FaultException e)      
            {
                MessageBox.Show(e.ToString());
            }
        }
       
        private void frmErrorMessages_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (frmErrorMessages.RowCount > 0)
            {

                
                int value = Int32.Parse(frmErrorMessages.Rows[e.RowIndex].Cells[0].Value.ToString());
                //if (value != null)
                //{
                    //lblAuditMessages.Text = "";
                
                    lblAuditMessages.Text =  "Audit messages for Request: " + value.ToString();
                    WeiMonitoringClient client = new WeiMonitoringClient();
                    if (client != null)
                    {
                        Telavance.AdvantageSuite.Wei.WeiService.AuditMessages[] values = client.getAuditMessages(value);

                        if (values != null)
                        {


                            if (frmAuditMessages.Rows.Count > 0)
                            {
                                frmAuditMessages.DataSource = null;
                            }
                            


                            frmAuditMessages.AutoGenerateColumns = false;
                         
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource = values;
                            frmAuditMessages.DataSource = bindingSource;
                        }
                    }
                //}
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                int requestID;
                if (frmErrorMessages.RowCount > 0)
                {
                    WeiMonitoringClient client = new WeiMonitoringClient();
                    if (client != null)
                    {
                        for (int iRow = 0; iRow < frmErrorMessages.Rows.Count; iRow++)
                        {
                            requestID = Convert.ToInt32(frmErrorMessages.Rows[iRow].Cells[0].Value.ToString());
                            client.reprocess(requestID);

                        }
                        //Refresh the grid after the messages were reprocessed. Only the messages that failed the reprocessing
                        // will be populated.

                        PopulateGridWithErrorMessages();

                    }
                }
            }
            catch (FaultException ex)
            {
                throw (ex);
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_pageSize == 0)
            {
                return;
            }

            _currentPage = _currentPage+1;

            if (_currentPage > _pageCount)
            {
                _currentPage = _pageCount;
            }
            //Check if you are already at the last page
            if (_recNo == _maxRec)
            {
                return;
            }
            PopulateGridWithErrorMessages();

        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {

            if (_currentPage == _pageCount)
            {
                _recNo = _pageSize * (_currentPage-2);
            }

            _currentPage = _currentPage-1;

            //Check if you are already at the first page.
            if (_currentPage < 1 )
            {
                _currentPage = 1;
                return;
            }
            else
            {
    
                _recNo = _pageSize * (_currentPage - 1);
            }

            PopulateGridWithErrorMessages();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            //Check if you are already at the first page.
            if  (_currentPage == 1)
            {
                return;
            }
                _currentPage = 1;
                _recNo = 0;
                PopulateGridWithErrorMessages();

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            //Check if you are already at the last page.
            if (_recNo == _maxRec)
            {
                MessageBox.Show("You are at the Last Page!");
                return;
            }
            _currentPage = _pageCount;
            _recNo = _pageSize * (_currentPage - 1);
            PopulateGridWithErrorMessages();



        }

       
       
    }
}
