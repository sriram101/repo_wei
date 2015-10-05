/*
**  File Name:		Messages.aspx
**
**  Functional Description:
**
**      
**	
**
**	Author:	Rama Pappu
**  Facility	    WEI Dashboard
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

12/30/2010       RP        Initial Version
02/13/2011       RP        Modified the code to include the logic for filtering 
                           error messages
04/23/2011       RP        changed the namespace
05/01/2011       RP        Introduced custom paging
12/05/2012                 Code to display the translations for the message
12/07/2012                 Code to process error messages 
12/09/2012                 Code to review and approve translations and release the message for OFAC check
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using System.Configuration;
using System.Text.RegularExpressions;
using Telavance.AdvantageSuite.Wei.DBUtils;
using Telavance.AdvantageSuite.Wei.DataAccess;

using System.Text;
using System.Xml;
using System.IO;



namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Messages : System.Web.UI.Page
    {

        private DateTime _dfromdate;
        private DateTime _dtodate;
        private string _sdropdownvalue;
        private DataSet _dDataset;
        private DataTable _dDataTable;
        private int _gridRefresh;
        private bool _hasCTC;
        private bool _hasError;
        private int _pageSize;
        private double _recCount;
        private String _strSortExp = "ModifiedDateTime";
        private String _strAuditSortExp = "CreatedDateTime";
        private SortDirection _SortDirection = SortDirection.Descending;
        protected string _sSearchString = String.Empty;
        protected int _currentPageNumber = 1;
        protected int _totalPages = 1;
        private DbUtil _dataAccess;

        public enum UserAction
        {
            Review = 1,
            Approve = 2
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                _dataAccess = EnterpriseLibraryContainer.Current.GetInstance<DbUtil>();
                _gridRefresh = Convert.ToInt32(ConfigurationManager.AppSettings["GridRefresh"]);
                _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPaging"));

                
                ViewState["GridRowHighlight"] = true;
                ViewState["RowHighlightLabel"] = null;
                ViewState["SortDirection"] = SortDirection.Ascending;
               


                txtFromDate.Attributes.Add("ReadOnly", "true"); // Make the text box readonly.
                txtFromDate.Attributes.Add("ReadOnly", "true"); // Make the text box readonly.
                lblInfo.Attributes.Add("ReadOnly", "true");
                //lblMsgTranslations.Attributes.Add("ReadOnly", "true");
                pnlSummary.Visible = false;

                //grdShowMessages.PagerSettings.Position = PagerPosition.Bottom;
                if (!Page.IsPostBack)
                {
                    UserRole();

                    //Fix for start date and enddate when page is loaded firsttime
                    //shashank - 07/11/2012
                    //set the value of these textboxes only to date value - removed time part of value

                    /*-------------- NEW CODE -----*/


                    txtFromDate.Text = System.DateTime.Now.ToShortDateString();
                    txtToDate.Text = System.DateTime.Now.ToShortDateString();
                    _dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
                    _dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);
                    ViewState["StartDate"] = _dfromdate;
                    ViewState["EndDate"] = _dtodate;
                    ViewState["Status"] = "";
                    ViewState["hasCTC"] = "true";
                    ViewState["hasError"] = "false";
                    ViewState["EditRow"] = String.Empty;
                    ViewState["CurrentPageNumber"] = 1;
                   

                    _sdropdownvalue = string.Empty;
                    _hasCTC = true;
                    _hasError = false;


                    labelText(); // Assign the label text based on the Grid Refresh or Select Criteria
                    populateGridView();
                    populateDropDownlist(); // to populate the dropdown list with Message Status

                }
                else
                {
                    if (null != ViewState["SortExp"])
                    {
                        _strSortExp = ViewState["SortExp"] as String;
                    }

                    if (null != ViewState["SortDirection"])
                    {
                        _SortDirection = (SortDirection)ViewState["SortDirection"];
                    }
                    else
                    {
                        ViewState["SortDirection"] = SortDirection.Ascending;
                    }

                   // ViewState["MessageId"] = null;
                   // ViewState["GridRowIndex"] = null;

                }

            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error in Page Load method."+ ex.Message);
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }

        int GetSortColumnIndex(String strCol, GridView grdView)
        {
            foreach (DataControlField field in grdView.Columns)
            {
                if (field.SortExpression == strCol)
                {
                    return grdView.Columns.IndexOf(field);
                }
            }

            return -1;
        }

        void AddSortImage(GridViewRow headerRow, GridView grdView)
        {
            Int32 iCol = GetSortColumnIndex(_strSortExp, grdView);
            if (-1 == iCol)
            {
                return;
            }
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();

            sortImage.Style[HtmlTextWriterStyle.PaddingTop] = "3px";

            if (null != ViewState["SortDirection"].ToString())
            {

                if (SortDirection.Ascending.ToString() == ViewState["SortDirection"].ToString())
                {
                    sortImage.ImageUrl = "../Images/sort_asc.gif";
                    sortImage.AlternateText = "Ascending Order";

                }
                else
                {
                    sortImage.ImageUrl = "../Images/sort_desc.gif";
                    sortImage.AlternateText = "Descending Order";

                }
            }
            else
            {
                sortImage.ImageUrl = "../Images/sort_asc.gif";
                sortImage.AlternateText = "Ascending Order";

            }
            if (headerRow.Cells[iCol].Controls.Count > 0)
            {
                ((LinkButton)headerRow.Cells[iCol].Controls[0]).Style["float"] = "left";
            }
            headerRow.Cells[iCol].Controls.AddAt(0, sortImage);

        }

        void labelText()
        {
            string _sLabelText;
            if (Timer1.Enabled == false)
            {
                if ((_sdropdownvalue == null || _sdropdownvalue == ""))
                {
                    _sLabelText = "Display messages created between <b>" + txtFromDate.Text + "</b> and <b>" + txtToDate.Text + "</b> and status is <b> -ALL- </b>. Auto Refresh is turned <b>OFF.</b>";
                }
                else
                {
                    _sLabelText = "Display messages created between <b>" + txtFromDate.Text + "</b> and <b>" + txtToDate.Text + "</b> and status is <b> " + ddlMessageStatus.SelectedItem.Text + "</b>. Auto Refresh is turned <b>OFF.</b>";
                }
            }
            else
            {
                _sLabelText = "Display messages created between <b>" + txtFromDate.Text + "</b> and <b>" + txtToDate.Text + "</b>. Auto Refresh is turned <b>ON</b>. Messages will be refreshed automatically every " + (_gridRefresh) / 1000 + " seconds.";
            }
            lblMessage.Text = _sLabelText;


        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
            resetParams();
            labelText();
            populateGridView();
            PopulateMessageDetail();

            
        }
        void resetParams()
        {

            txtFromDate.Text = System.DateTime.Now.ToShortDateString();
            txtToDate.Text = System.DateTime.Now.ToShortDateString();
            ddlMessageStatus.SelectedIndex = 0; //reset the value from the dropdownlist
            _dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
            _dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);
            ViewState["StartDate"] = _dfromdate;
            ViewState["EndDate"] = _dtodate;
            _sdropdownvalue = string.Empty;
            ViewState["Status"] = "";
            //ViewState["MessageId"] = null;
            //ViewState["GridRowIndex"] = null;



        }
        protected void chkShowCTC_CheckChanged(object sender, EventArgs e)
        {
            _hasCTC = (_hasCTC == chkShowCTC.Checked) ? false : true;
            ViewState["hasCTC"] = _hasCTC;
        }

        protected void chkShowErrors_CheckChanged(object sender, EventArgs e)
        {

            _hasError = (_hasError == chkShowErrors.Checked) ? false : true;
            ViewState["hasError"] = _hasError;
        }

        protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPageNumber = 1;
            grdShowMessages.PageSize = Int32.Parse(ddlRows.SelectedValue);
            _pageSize = Int32.Parse(ddlRows.SelectedValue);
            setParams();
            populateGridView();
        }

        protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPageNumber = Int32.Parse(ddlPage.SelectedValue);
            setParams();
            populateGridView();
        }

        protected void GetPageIndex(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    _currentPageNumber = 1;
                    
                    break;
                case "Previous":
                    _currentPageNumber = Int32.Parse(ddlPage.SelectedValue) - 1;
                    break;
                case "Next":
                    _currentPageNumber = Int32.Parse(ddlPage.SelectedValue) + 1;
                    break;
                case "Last":
                    _currentPageNumber = Int32.Parse(lblTotalPages.Text);
                    break;

            }
            ViewState["CurrentPageNumber"] = _currentPageNumber;
            setParams();
            populateGridView();
        }

        private int GetTotalPages(double totalRows)
        {
            int totalPages = (int)Math.Ceiling(totalRows / _pageSize);

            return totalPages;
        }

        void populateGridView()
        {
            String strSort = String.Empty;

            
            

            // Refresh the grid 
            if (grdShowMessages.PageSize > 0)
            {
                _pageSize = grdShowMessages.PageSize;
                if (grdShowMessages.Rows.Count ==1)
                {
                    ViewState["CurrentPageNumber"] = 1;
                }
            }
            //string _sorting;
            strSort = String.Format("{0} {1}", _strSortExp, (_SortDirection == SortDirection.Descending) ? "DESC" : "ASC");

            _dDataset = _dataAccess.getMessages(Convert.ToDateTime(ViewState["StartDate"]), Convert.ToDateTime(ViewState["EndDate"]), ViewState["Status"].ToString(),
                         Convert.ToBoolean(ViewState["hasCTC"]), Convert.ToBoolean(ViewState["hasError"]), txtSearchText.Text, strSort,Int32.Parse(ViewState["CurrentPageNumber"].ToString()), _pageSize, out _recCount);
            if (_dDataset != null)
            {
                _dDataTable = _dDataset.Tables[0];

                Session["DataTable"] = _dDataTable;
            }



            ddlPage.Items.Clear();
            if (_recCount > 0)
            {
                //ViewState["RecCount"] = _recCount;
                _totalPages = GetTotalPages(_recCount);

                lblTotalPages.Text = GetTotalPages(_recCount).ToString();
                for (int i = 1; i < Convert.ToInt32(lblTotalPages.Text) + 1; i++)
                {
                    ddlPage.Items.Add(new ListItem(i.ToString()));
                }
                ddlPage.SelectedValue = _currentPageNumber.ToString();
            }

            navigateButtons(_currentPageNumber);



            if (null != _strSortExp &&
                String.Empty != _strSortExp)
            {
                strSort = String.Format("{0} {1}", _strSortExp, (_SortDirection == SortDirection.Descending) ? "DESC" : "ASC");
            }

            //SMU: Jan 31, 2013
            //DataView dv = new DataView(_dDataTable, String.Empty, strSort, DataViewRowState.CurrentRows);
            //grdShowMessages.DataSource = dv;
            grdShowMessages.DataSource = _dDataTable;
            //EU: Jan 31, 2013

            grdShowMessages.DataBind();

            if (grdShowMessages.Rows.Count == 0)
            {
                btnProcessError.Enabled = false;
                lblInfo.Text = "";
                Panel5.Visible = false;
                //Panel3.Visible = false;
               // pnlTranslations.Visible = false;
                pnlSummary.Visible = false;
                insertGridRow(grdShowMessages, _dDataTable);
                
            }
            else
            {
                Panel5.Visible = true;
                //pnlSummary.Visible = true;
                //Panel3.Visible = true;
                //pnlTranslations.Visible = true;
                //if (ddlMessageStatus.SelectedItem.Value != "")
                //{
                //    if (Convert.ToInt32(ddlMessageStatus.SelectedItem.Value) == Convert.ToInt32(Status.Review))
                //    {
                //        btnProcessReview.Enabled = true;
                //    }

                //}
            }
            if (ViewState["RowHighlightLabel"] == null)
            {
                lblInfo.Visible = false;
            }
            else
            {
                lblInfo.Visible = true;
                lblInfo.Text = "(Highlighted row indicates error in processing the message)";
                //enable the button when the gridview shows error records
                btnProcessError.Enabled = true;
                btnProcessReview.Enabled = false;
            }
            if (_recCount == 0)
            {
                lblTotalRecords.Visible = false;
            }
            else
            {
                lblTotalRecords.Visible = true;
                lblTotalRecords.Text = "Total Number of Records: " + _recCount.ToString();
            }


        }

        void insertGridRow(GridView grdView, DataTable _dtTable)
        {
            _dtTable.Rows.Add(_dtTable.NewRow());
            grdView.DataSource = _dtTable;
            grdView.DataBind();
            int TotalColumns = grdView.Rows[0].Cells.Count;
            grdView.Rows[0].Cells.Clear();
            grdView.Rows[0].Cells.Add(new TableCell());
            grdView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            grdView.Rows[0].Cells[0].Text = "No matching records found.";
            btnProcessReview.Enabled = false;
        }


        void navigateButtons(int currentPageNumber)
        {
            if (currentPageNumber == 1)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                if (lblTotalPages.Text != "")
                {
                    if (Int32.Parse(lblTotalPages.Text) > 1)
                    {
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    else
                    {
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;

                    }
                }
            }

            else
            {
                btnPrevious.Enabled = true;
                btnFirst.Enabled = true;

                if (currentPageNumber == Int32.Parse(lblTotalPages.Text))
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;

                }
                else
                {
                    btnNext.Enabled = true;
                    btnLast.Enabled = true;

                }
            }

        }
        void setParams()
        {
            _dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
            _dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);
            _sdropdownvalue = ddlMessageStatus.SelectedItem.Value;
            if (chkShowCTC.Checked)
            {
                _hasCTC = true;
                
            }
            else
            {
                _hasCTC = false;

            }

            if (chkShowErrors.Checked)
            {
                _hasError = true;
            }
            else
            {
                _hasError = false;
            }
            ViewState["StartDate"] = _dfromdate;
            ViewState["EndDate"] = _dtodate;
            ViewState["Status"] = _sdropdownvalue;
            ViewState["hasCTC"] = _hasCTC;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Timer1.Enabled = false; //disable the timer when search criteria is selected.
                _sSearchString = txtSearchText.Text;
                setParams();
                labelText();
                populateGridView();

            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in search button click - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }

        private void UserRole()
        {
            try
            {
                //if (Request.Cookies["WEIRole"] == null || Request.Cookies["WEIRole"] != null)
                // {
                HttpCookie WEIRole = new HttpCookie("WEIRole");

                bool bValid = false;
                int rolecount = 0;

                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principle = new WindowsPrincipal(identity);

              //  LogUtil.logInfo("User Identity: " + identity.Name);

                Session["User"] = identity.Name;

                string _reviewer = ADConfiguration.GetConfig().Reviewer;
                string[] _arrReviewerRoles = null;
                if (_reviewer != "")
                {
                    _arrReviewerRoles = _reviewer.Split(',');
                }

                rolecount = 0;
                for (int i = 0; i < _arrReviewerRoles.Length; i++)
                {
                    bValid = principle.IsInRole(_arrReviewerRoles[i].ToString());

//#if DEBUG
//                    bValid = true;
//#endif
                    if (bValid)
                    {
                       // LogUtil.logInfo("User is member of:--" + _arrReviewerRoles[i].ToString());
                        rolecount = rolecount + 1;
                    }
                }
                WEIRole["WEIReviewer"] = (rolecount > 0) ? "Y" : "N";

                //string _approver = ADConfiguration.GetConfig().Approver;
                //string[] _arrApproverRoles = null;
                //if (_approver != "")
                //{
                //    _arrApproverRoles = _approver.Split(',');
                //}

                //rolecount = 0;
                //for (int i = 0; i < _arrApproverRoles.Length; i++)
                //{
                //    bValid = principle.IsInRole(_arrApproverRoles[i].ToString());
                //    if (bValid)
                //    {
                //        LogUtil.logInfo("User is member of:--" + _arrApproverRoles[i].ToString());
                //        rolecount = rolecount + 1;
                //    }
                //}
                //WEIRole["WEIApprover"] = (rolecount > 0) ? "Y" : "N";

                //Session["WEIApprover"] = WEIRole["WEIApprover"];
                Session["WEIReviewer"] = WEIRole["WEIReviewer"];

                //TRSRole.Expires = DateTime.Now.AddMinutes(20); //removed expiration date

                //SMU: Feb 05, 2013 for testing
                WEIRole.Expires = DateTime.Now.AddMinutes(20);

                Response.Cookies.Add(WEIRole);

                var httpCookie = Request.Cookies["WEIRole"];
                if (httpCookie != null)
                {
                    if ((httpCookie["WEIReviewer"] == "N"))// && httpCookie["WEIApprover"] == "N"))
                    {
                        LogUtil.logInfo("Login failed. User does not belong to a valid role.");
                        Response.Redirect("Logout.aspx", false);
                    }
                }
                else
                {
                    LogUtil.logInfo("Login failed. User does not belong to a valid role.");
                    Response.Redirect("Logout.aspx", false);
                }
                //}
            }
            catch (Exception ex)
            {
                LogUtil.logError("Method: UserRole - " + ex.InnerException.ToString());
                Response.Redirect("Logout.aspx", false);
            }
        }


        void populateDropDownlist()
        {
                DataSet _dataset = _dataAccess.MessageStatus();
                if (_dataset != null)
                {
                    ddlMessageStatus.DataSource = _dataset;
                    ddlMessageStatus.DataValueField = _dataset.Tables[0].Columns[0].ColumnName;
                    ddlMessageStatus.DataTextField = _dataset.Tables[0].Columns[1].ColumnName;
                    ddlMessageStatus.DataBind();
                    ddlMessageStatus.Items.Insert(0, new ListItem("---All----", String.Empty));
                }
           
        }


        protected void grdShowMessages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int _iRowIndex = 0;
                int _igrdRowIndex;
                GridViewRow _grdViewRow;

                _sSearchString = txtSearchText.Text;
                ViewState["EditRow"] = String.Empty;

                if (e.CommandName == "ShowTranslations")
                {

                    _grdViewRow = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                    if (_grdViewRow.RowType == DataControlRowType.DataRow)
                    {
                        _igrdRowIndex = _grdViewRow.RowIndex;

                        ViewState["GridRowIndex"] = _igrdRowIndex;
                        _iRowIndex = Convert.ToInt32(_grdViewRow.Cells[1].Text);

                        ViewState["MessageId"] = _iRowIndex;

                        if (grdShowMessages.Rows.Count > 0)
                        {
                            //Assign the message details to the labels
                            if (ViewState["GridRowIndex"] != null)
                            {

                                //_rowindex = Convert.ToInt32(ViewState["GridRowIndex"]);
                                pnlSummary.Visible = true;
                                if (grdShowMessages.Rows[_igrdRowIndex].Cells.Count > 1)
                                {
                                    lblRequestValue.Text = grdShowMessages.Rows[_igrdRowIndex].Cells[1].Text;
                                    lblInterfaceValue.Text = grdShowMessages.Rows[_igrdRowIndex].Cells[2].Text;
                                    lblDescValue.Text = grdShowMessages.Rows[_igrdRowIndex].Cells[3].Text;
                                    lblCreateTimeValue.Text = grdShowMessages.Rows[_igrdRowIndex].Cells[7].Text;
                                    lblModifiedTimeValue.Text = grdShowMessages.Rows[_igrdRowIndex].Cells[8].Text;
                                    //ViewState["Description"] = lblDescValue.Text;
                                    //ViewState["OriginalMessage"] = grdShowMessages.Rows[_igrdRowIndex].Cells[10].Text;
                                    //ViewState["TranslatedMessage"] = grdShowMessages.Rows[_igrdRowIndex].Cells[11].Text;
                                    //ViewState["HasCTCApproved"] = grdShowMessages.Rows[_igrdRowIndex].Cells[13].Text;

                                   // lblNote.Visible = false;
                                   // lblNoteValue.Visible = false;
                                }

                            }
               }
                    }

                    PopulateMessageDetail();
                    ModalPopupExtender1.Show();
                }
            }
            catch (Exception ex)
             {
                 LogUtil.logError("Unexpected error occured in grdShowMessages RowCommand: - " + ex.InnerException.ToString());
                 Response.Redirect("CustomErrorPage.aspx",false);
            }
       }

        void highlightGridRowEx(GridViewRow _row)
        {
            //Check if the row created is a datarow and the row dataitem is not null
            if (_row.RowType == DataControlRowType.DataRow && _row.DataItem != null)
            {

                //highlight the row that has errors in red.

                if (DataBinder.Eval(_row.DataItem, "IsError").ToString().Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    _row.Style["color"] = "red";
                    //set the viewstate value if the gridview row has an error
                    if (ViewState["RowHighlightLabel"] == null)
                    {
                        ViewState["RowHighlightLabel"] = true;
                    }
                }
                
            }
            
        }

        void highlightGridRow(GridViewRow _row)
        {
            //Check if the row created is a datarow and the row dataitem is not null
            if (_row.RowType == DataControlRowType.DataRow && _row.DataItem != null)
            {

                //highlight the row that has errors in red.

                if (DataBinder.Eval(_row.DataItem, "IsError").ToString().Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    _row.Style["color"] = "red";
                    //set the viewstate value if the gridview row has an error
                    if (ViewState["RowHighlightLabel"] == null)
                    {
                        ViewState["RowHighlightLabel"] = true;
                    }
                }

            }
        }
        protected void grdShowMessages_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (String.Empty != _strSortExp)
                    {
                        AddSortImage(e.Row, grdShowMessages);
                    }
                }

                highlightGridRow(e.Row);
            }
            catch (Exception ex)
            {
                LogUtil.logError("Method: grdShowMessages_RowCreated - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }

        protected void grdShowMessages_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Timer1.Enabled = false;

                if (String.Empty != _strSortExp)
                {
                    if (String.Compare(e.SortExpression, _strSortExp, true) == 0)
                    {
                        _SortDirection = (_SortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
                    }
                }

                ViewState["SortDirection"] = _SortDirection;
                ViewState["SortExp"] = _strSortExp = e.SortExpression;

                setParams();
                labelText();
                populateGridView();

            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in grdShowMessages Sorting - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }
        
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                _sSearchString = txtSearchText.Text;
                Timer1.Enabled = true; //Enable the timer when the refresh button is clicked.
                resetParams();
                labelText();
                populateGridView();
            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in btnRefresh Click - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
        }


        protected void btnProcessReview_Click(object sender, EventArgs e)
        {
            WeiMonitoringClient client = new WeiMonitoringClient();
            try
            {
                string _sUser;
                GridViewRow drRow;
                int iRowIndex;
                bool bReviewed = false;
                //bool bApproved = false; ;
                //lblMsgTranslations.Visible = false;
                lblNoteValue.Visible = false;
                lblNote.Visible = false;
                _sUser = WindowsIdentity.GetCurrent().Name;
                bool bSuccess = false;
                string _sMessage = string.Empty;

                for (iRowIndex = 0; iRowIndex < grdShowTranslations.Rows.Count; iRowIndex++)
                {
                    drRow = grdShowTranslations.Rows[iRowIndex];
                    //if (ViewState["EditRow"].ToString() == "")
                    //if (drRow.RowState == DataControlRowState.Edit)
                    //{
                    //    drRow.RowState = DataControlRowState.Normal;
                    //    populateTranslationsGrid(Convert.ToInt32(ViewState["MessageId"]), false);
                    //}

                    if ((Label)drRow.FindControl("lblReviewed") != null)
                    {
                        bReviewed = Convert.ToBoolean(((Label)drRow.FindControl("lblReviewed")).Text);
                        if (!bReviewed)
                        {
                            break;
                        }
                        //  bApproved = Convert.ToBoolean(((Label)drRow.FindControl("lblApproved")).Text);
                    }
                    //else
                    // {
                    //bReviewed = ((CheckBox)drRow.FindControl("chkReviewMessage")).Checked;
                    //bApproved = ((CheckBox)drRow.FindControl("chkApproveMessage")).Checked;
                    //}
                }

                if (bReviewed)
                {
                    if (grdShowMessages.Rows.Count > 0)
                    {
                        //client = new WeiMonitoringClient();
                        if (client != null)
                        {
                            if (grdShowTranslations.Rows.Count > 0)
                            {
                                if (_dataAccess.AddAuditForReview(Int32.Parse(ViewState["MessageId"].ToString()), _sUser))
                                {

                                    //AuditUtil.getInstance().audit(Int32.Parse(ViewState["RequestID"].ToString()), AuditLevel.Info, "Message has been released for OFAC Check by user: " + _sUser);
                                    
                                    bSuccess = client.processMessageForOFACCheck(Int32.Parse(ViewState["MessageId"].ToString()), false);
                                    if (bSuccess)
                                    {
                                        _sMessage = "Message successfully released for OFAC check";
                                    }
                                    else
                                    {
                                        _sMessage = "There was an error sending the message for OFAC check";
                                    }
 
                                }
                                lblNote.Visible = true;
                                lblNoteValue.Text = _sMessage;
                                lblNoteValue.Visible = true;

                            }

                        }
                    }

                    //populateGridView();
                }
                else
                {
                    lblNote.Visible = true;
                    lblNoteValue.Text = "Please review the translation before sending the message for OFAC check";
                    lblNoteValue.Visible = true;

                }
                PopulateMessageDetail();
            }
            catch (TimeoutException ex)
            {
                LogUtil.logError("Timeout while waiting for the service" +  ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);;
            }
            catch (CommunicationException ex)
            {
                LogUtil.logError("Error while establishing connection with the service:" + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);;
            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in Process Review Method - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);;

            }
            finally
            {
                client.Close();
            }
        }
        protected void lnkShowTranslations_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    int _iRowIndex;
            //    int _igrdRowIndex;
            //    GridViewRow _grdViewRow;

            //    _sSearchString = txtSearchText.Text;
            //    ViewState["EditRow"] = String.Empty;
            //    _grdViewRow = ((LinkButton)sender).Parent.Parent as GridViewRow;
            //    if (_grdViewRow.RowType == DataControlRowType.DataRow)
            //    {

            //        _igrdRowIndex = _grdViewRow.RowIndex;

            //        ViewState["GridRowIndex"] = _igrdRowIndex;
            //        _iRowIndex = Convert.ToInt32(_grdViewRow.Cells[1].Text);
            //        //ViewState["RowIndex"] = _iRowIndex;
            //        populateTranslationsGrid(_iRowIndex, false);
            //        //if (ViewState["EditRow"].ToString() != "")
            //        //{
            //        //    setTranslations(_iRowIndex);
            //        //}
            //        tbContainer.ActiveTabIndex = 0;
            //        tbTranslations.Visible = true;
            //    }


            //catch (Exception)
            //{
            //    throw;
            //}
        }
        protected void tbContainer_TabChanged(object sender, EventArgs e)
        {

                if (tbContainer.ActiveTabIndex == 0)
                {
                    tbContainer.ActiveTabIndex = 0;
                    tbContainer.Visible = true;
                }
                else if (tbContainer.ActiveTabIndex ==1)
                {
                    if (grdShowMessages.Rows.Count > 0)
                    {

                         tbContainer.ActiveTabIndex = 1;
                         tbContainer.Visible = true;
                   }
                }
                else 
                {
                    tbContainer.ActiveTabIndex = 2;
                    tbContainer.Visible = true;
                   
                }
                pnlSummary.Visible = true;
            }

        protected void tbContainer_TabChangedEx(object sender, EventArgs e)
            {
            //int _iRowIndex = 0;
            //int _igrdRowIndex = 0;

            //try
            //{
            //    _iRowIndex = Convert.ToInt32(ViewState["RowIndex"]);
            //    _igrdRowIndex = Convert.ToInt32(ViewState["GridRowIndex"]);

            //    if (tbContainer.ActiveTabIndex == 0)
            //    {
            //        populateTranslationsGrid(Int32.Parse(ViewState["RequestID"].ToString()), false);
            //        tbContainer.ActiveTabIndex = 0;
            //        tbContainer.Visible = true;
            //    }
            //    else if (tbContainer.ActiveTabIndex == 1)
            //    {
            //        if (grdShowMessages.Rows.Count > 0)
            //        {

            //            if (Session["DataTable"] != null)
            //            {

            //                _dDataTable = (DataTable)Session["DataTable"];
            //                _dDataTable.DefaultView.RowFilter = "ID=" + Int32.Parse(ViewState["RequestID"].ToString());

            //                grdShowDetails.DataSource = _dDataTable;
            //                grdShowDetails.DataBind();

            //                string strTest = FormatXml(_dDataTable.Rows[0]["translatedmessage"].ToString());

            //                //GridView2.DataSource = _dDataTable;
            //                //GridView2.DataBind();
            //            }
            //            tbContainer.ActiveTabIndex = 1;
            //            tbContainer.Visible = true;
            //        }
            //    }
            //    else
            //    {
            //        populateAuditGrid(_igrdRowIndex, Int32.Parse(ViewState["RequestID"].ToString()));
            //        tbContainer.ActiveTabIndex = 2;
            //        tbContainer.Visible = true;

            //    }
            //    pnlSummary.Visible = true;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        protected void btnProcessError_Click(object sender, EventArgs e)
        {
            string requestID;
            string _sUser;
            _sUser = WindowsIdentity.GetCurrent().Name;
            WeiMonitoringClient client = new WeiMonitoringClient();
            try
            {
                if (grdShowMessages.Rows.Count > 0)
                {

                    if (client != null)
                    {
                        for (int iRow = 0; iRow < grdShowMessages.Rows.Count; iRow++)
                        {
                            GridViewRow row = grdShowMessages.Rows[iRow];
                            if (row != null)
                            {
                                bool isChecked = ((CheckBox)row.FindControl("chkSelectRecord")).Checked;
                                if (isChecked)
                                {
                                    requestID = row.Cells[1].Text;
                                    if (_dataAccess.AddAuditForErrorProcess(Int32.Parse(requestID), _sUser))
                                    {
                                        //AuditUtil.getInstance().audit(Int32.Parse(requestID), WeiCommon.AuditLevel.Info, "Error message has been processed by user: " + _sUser);
                                        client.reprocess(Int32.Parse(requestID));
                                    }

                                }
                            }
                        }

                    }
                }

                populateGridView();
            }
            catch (TimeoutException ex)
            {
                LogUtil.log("Timeout while waiting for the service", ex.InnerException);
                Response.Redirect("CustomErrorPage.aspx", false);;
            }
            catch (CommunicationException ex)
            {
                LogUtil.logError("Error while establishing connection with the service:" + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);;
            }

            catch (Exception ex)
            {
                LogUtil.logError("Error while releasing error messages:" + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
            finally
            {
                client.Close();
            }
        }
        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            try
            {

                int _iRowIndex;
                int _igrdRowIndex;
                GridViewRow _grdViewRow;

                _sSearchString = txtSearchText.Text;
                _grdViewRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                if (_grdViewRow.RowType == DataControlRowType.DataRow)
                {
                    _igrdRowIndex = _grdViewRow.RowIndex;
                    ViewState["GridRowIndex"] = _igrdRowIndex;
                    _iRowIndex = Convert.ToInt32(_grdViewRow.Cells[2].Text);
                    ViewState["RowIndex"] = _iRowIndex;

                }
            }

            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured while showing the message details - " + ex.InnerException.ToString());
                  Response.Redirect("CustomErrorPage.aspx", false);
            }
        }

        protected void lnkShowTranslations_ClickEx(object sender, EventArgs e)
        {
            try
            {

                // Fetch the customer id
                LinkButton lb = sender as LinkButton;
                string custID = lb.Text;

                DataTable dt = new DataTable();


                // Bind the reader to the GridView
                // You can also use a lighter control 
                // like the Repeater to display data

                int _iRowIndex;
                int _igrdRowIndex;
                GridViewRow _grdViewRow;
                
                _sSearchString = txtSearchText.Text;
                ViewState["EditRow"] = String.Empty;
                _grdViewRow = ((LinkButton)sender).Parent.Parent as GridViewRow;
                if (_grdViewRow.RowType == DataControlRowType.DataRow)
                {
                    
                    _igrdRowIndex = _grdViewRow.RowIndex;
                   
                    ViewState["GridRowIndex"] = _igrdRowIndex;
                    _iRowIndex = Convert.ToInt32(_grdViewRow.Cells[1].Text);
                    //ViewState["RowIndex"] = _iRowIndex;
                    //populateTranslationsGrid(_iRowIndex, false);

                    DataSet _dataset;
                    _dataset = _dataAccess.getTranslations(_iRowIndex);

                    //GridView2.DataSource = _dataset.Tables[0];
                    //GridView2.DataBind();


                    //if (ViewState["EditRow"].ToString() != "")
                    //{
                    //    setTranslations(_iRowIndex);
                    //}
                    tbContainer.ActiveTabIndex = 0;
                    tbTranslations.Visible = true;
                }



                // Show the modalpopupextender
                ModalPopupExtender1.Show();
            }

            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in lnkShowTranslations method :" + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
        }

        void populateTranslationsGrid(int _key, bool _bEditMode)
        {
            DataSet _dataset;
            int _rowindex;
            try
            {
                if (grdShowMessages.Rows.Count > 1)
                {
                    //Assign the message details to the labels
                    if (ViewState["GridRowIndex"] != null)
                    {

                        _rowindex = Convert.ToInt32(ViewState["GridRowIndex"]);
                        pnlSummary.Visible = true;
                        if (grdShowMessages.Rows[_rowindex].Cells.Count > 1)
                        {
                            lblRequestValue.Text = grdShowMessages.Rows[_rowindex].Cells[1].Text;
                            lblInterfaceValue.Text = grdShowMessages.Rows[_rowindex].Cells[2].Text;
                            lblDescValue.Text = grdShowMessages.Rows[_rowindex].Cells[3].Text;
                            lblCreateTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[7].Text;
                            lblModifiedTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[8].Text;
                            // lblNote.Visible = false;
                            // lblNoteValue.Visible = false;
                        }

                    }
                }

               _dataset = _dataAccess.getTranslations(_key);
               if (_dataset != null)
                {
                    DataView dv = new DataView(_dataset.Tables[0]);
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        grdShowTranslations.DataSource = dv;
                        grdShowTranslations.DataBind();
                        //ViewState["RequestID"] = _key;

                        // Panel3.Visible = true;
                        pnlSummary.Visible = true;
                        //pnlTranslations.Visible = true;
                       // btnProcessReview.Enabled = true;
                        //lblMsgTranslations.Text = "Chinese Telegraphic Codes translations for message : " + _key;
                    }
                    else
                    {
                        //Insert a blank row 
                        insertGridRow(grdShowTranslations, _dataset.Tables[0]);
                        //pnlTranslations.Visible = true;
                        //lblMsgTranslations.Text = "No Chinese Telegraphic Codes for message: " + _key;
                        //btnProcessReview.Enabled = false;
                        //Panel3.Visible = false;
                    }



                    if (_bEditMode)
                    {
                        gridRowEditCheck();
                    }
                }
            }

            catch (Exception ex)
            {
                LogUtil.logError("Method: populateTranslationsGrid - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
        }

        
        //populate the translations grid
        //void populateTranslationsGridEx(int _key, bool _bEditMode)
        //{
        //    DataSet _dataset;
        //    int _rowindex;
        //    try
        //    {
        //        if (grdShowMessages.Rows.Count > 0) 
        //        {
        //        //Assign the message details to the labels
        //            if (ViewState["GridRowIndex"] != null)
        //            {

        //                _rowindex = Convert.ToInt32(ViewState["GridRowIndex"]);
        //                pnlSummary.Visible = true;
        //                if (grdShowMessages.Rows[_rowindex].Cells.Count > 0)
        //                {
        //                    lblRequestValue.Text = grdShowMessages.Rows[_rowindex].Cells[1].Text;
        //                    lblInterfaceValue.Text = grdShowMessages.Rows[_rowindex].Cells[2].Text;
        //                    lblDescValue.Text = grdShowMessages.Rows[_rowindex].Cells[3].Text;
        //                    lblCreateTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[7].Text;
        //                    lblModifiedTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[8].Text;
        //                }

        //            }
        //        }
                
        //        _dataset = _dataAccess.getTranslations(_key);
                
        //        DataView dv = new DataView(_dataset.Tables[0]);
        //        if (_dataset.Tables[0].Rows.Count > 0)
        //        {
        //            grdShowTranslations.DataSource = dv;
        //            grdShowTranslations.DataBind();
        //            ViewState["RequestID"] = _key;

        //            //Panel3.Visible = true;
        //            pnlSummary.Visible = true;
        //            //pnlTranslations.Visible = true;
        //            btnProcessReview.Enabled = true;
        //            //lblMsgTranslations.Text = "Chinese Telegraphic Codes translations for message : " + _key;
        //        }
        //        else
        //        {
        //            //Insert a blank row 
        //            insertGridRow(grdShowTranslations, _dataset.Tables[0]);
        //            //pnlTranslations.Visible = true;
        //            //lblMsgTranslations.Text = "No Chinese Telegraphic Codes for message: " + _key;
        //            btnProcessReview.Enabled = false;
        //            //Panel3.Visible = false;
        //        }
        //        if (_bEditMode)
        //        {
        //            gridRowEditCheck();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.logError("Method: populateTranslationsGridEx - " + ex.InnerException.ToString());
        //        Response.Redirect("CustomErrorPage.aspx", false);
        //    }
        //}

        void populateAuditGrid(int _rowindex, int _key)
        {
            String strSort = String.Empty;
            DataSet _dataset;
            try
            {
                // check if the master grid has any records.
                if (grdShowMessages.Rows.Count > 0)
                {

                    _dataset = _dataAccess.getAuditMessagesByRequest(_key);

                    if (null != _strAuditSortExp && String.Empty != _strAuditSortExp)
                    {
                        strSort = String.Format("{0} {1}", _strAuditSortExp, (_SortDirection == SortDirection.Descending) ? "DESC" : "ASC");
                    }
                    DataView dv = new DataView(_dataset.Tables[0], String.Empty, strSort, DataViewRowState.CurrentRows);

                    grdShowAudit.DataSource = dv;
                    grdShowAudit.DataBind();


                }
                
            }
            catch (Exception ex)
            {
                LogUtil.logError("Method: PopulateAuditGrid - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
        }

        protected string HighlightText(string searchWord, string inputText)
        {

            // Replace spaces by | for Regular Expressions  

            Regex expression = new Regex(_sSearchString, RegexOptions.IgnoreCase);

            return expression.Replace(inputText, new MatchEvaluator(ReplaceKeywords));

        }

        public string ReplaceKeywords(Match m)
        {
            return "<span class='highlight'>" + m.Value + "</span>";
        }

      

        protected void grdShowTranslations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdShowTranslations.EditIndex = -1;
                populateTranslationsGrid(Convert.ToInt32(ViewState["MessageId"]),false);
            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in grdShowTranslations_RowCancelingEdit method: " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }

        protected void grdShowTranslations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string sReviewMode = "Review";
            string sUser = "";
            int iRowIndex = 0;
            bool bSuccess;
            bool bReviewed =false;
            bool bApproved = false;
            string sReviewedBy = String.Empty;
            string sApprovedBy = String.Empty;
            string sNewTrans;
            try
            {

                ViewState["EditRow"] = "true";
                GridViewRow drRow;
                //Session["WEIReviewer"] = "Y";
                //Session["WEIApprover"] = String.Empty;
                Session["User"] = WindowsIdentity.GetCurrent().Name;

                if (null != Session["User"])
                {
                    sUser = Session["User"].ToString();
                }
                if (Session["WeiReviewer"] != null)
                {
                    if (Session["WEIReviewer"].ToString() == "Y")
                    {
                        sReviewMode = "Review";
                        sReviewedBy = sUser;
                    }
                }
                //if (Session["WEIApprover"].ToString() == "Y") { sReviewMode = "Approve"; sApprovedBy = sUser; }
                //if ((Session["WEIApprover"].ToString() == "Y") && (Session["WEIReviewer"].ToString() == "Y"))
                //{
                //    sReviewMode = "Approve";
                //    sReviewedBy = sUser;
                //    sApprovedBy = sUser;

                //}
                bSuccess = false;


                if (grdShowTranslations.Rows.Count > 0)
                {
                    for (iRowIndex = 0; iRowIndex < grdShowTranslations.Rows.Count; iRowIndex++)
                    {
                        drRow = grdShowTranslations.Rows[iRowIndex];

                        if (drRow.RowState == DataControlRowState.Edit || drRow.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))

                        {
                            if (grdShowTranslations.EditIndex != -1)
                            {

                                int iTranID = Convert.ToInt32(((Label)drRow.FindControl("lblID")).Text.Trim());
                                string sCTCCode = ((Label)drRow.FindControl("lblCTCCodes")).Text.Trim();
                                string sChinesChar = ((Label)drRow.FindControl("lblChineseChar")).Text.Trim();
                                string sOldTrans = ((Label)drRow.FindControl("lblOldTrans")).Text.Trim();
                                //if (((TextBox)drRow.FindControl("txtNewTranslations")).Text.Trim() != null)
                                //{
                                sNewTrans = ((TextBox)drRow.FindControl("txtNewTranslations")).Text.Trim();
                                bReviewed = ((CheckBox)drRow.FindControl("chkReviewMessage")).Checked;
                                sReviewedBy = WindowsIdentity.GetCurrent().Name;
                                //}
                                //else
                                //{
                                //  sNewTrans = ((Label)drRow.FindControl("lblNewTrans")).Text.Trim();
                                // bReviewed = Convert.ToBoolean(((Label)drRow.FindControl("chkReviewMessage")).Text);
                                //}


                                //if (sReviewMode == "Approve")
                                //{
                                //    sReviewedBy = ((Label)drRow.FindControl("lblReviewedBy")).Text;
                                //    bApproved = ((CheckBox)drRow.FindControl("chkApproveMessage")).Checked;
                                //}
                                //else
                                //{
                                //    ((CheckBox)drRow.FindControl("chkApproveMessage")).Checked = false;
                                //    ((CheckBox)drRow.FindControl("chkReviewMessage")).Checked = true;
                                //    bReviewed = ((CheckBox)drRow.FindControl("chkReviewMessage")).Checked;

                                //}
                                //if (sReviewedBy == "" && !bReviewed)
                                //{
                                //    lblMsgTranslations.Text = "Approver cannot approve the translations that are not reviewed.";
                                //}
                                //else if (sReviewedBy == sApprovedBy && Session["WEIApprover"].ToString() == "N")
                                //{
                                //    lblMsgTranslations.Text = "Reviewer does not have the priveleges to approve the translations";
                                //}
                                //else
                                //{
                                //lblMsgTranslations.Text = "";
                                
                                bSuccess = _dataAccess.UpdateTranslations(iTranID, Convert.ToInt32(ViewState["MessageId"]), sNewTrans,
                                     sReviewMode, sReviewedBy, bReviewed, sApprovedBy, bApproved, sCTCCode);

                                //SMU: Feb 01, 2013
                                grdShowTranslations.EditIndex = -1;
                            }
                            //EU: Feb 01, 2013
                            //}
                        }

                    }

                }
                PopulateMessageDetail();
                PopulateOriginalMessage();
                ViewState["EditRow"] = "true";
                
            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in grdShowTranslations_RowUpdating method: " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }

        }
        protected void grdShowTranslations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdShowTranslations.EditIndex = e.NewEditIndex;
                ViewState["EditRow"] = "true";
                populateTranslationsGrid(Convert.ToInt32(ViewState["MessageId"]),true);
                checkMode(e.NewEditIndex);
            }
            catch (Exception ex)
            {
                LogUtil.logError("Method: grdShowTranslations_RowEditing - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx",false);
            }
        }
        
        private void gridRowEditCheck()
        {
            GridViewRow drRow;
            int iRowIndex =0;
            bool bReviewed;
            //bool bApproved;

            
            if ((grdShowTranslations.Rows.Count > 0) && (ViewState["EditRow"].ToString() != ""))
            {
                for (iRowIndex = 0; iRowIndex < grdShowTranslations.Rows.Count; iRowIndex++)
                {
                    drRow = grdShowTranslations.Rows[iRowIndex];
                    if (drRow.RowState == DataControlRowState.Edit)
                    {
                        bReviewed = ((CheckBox)drRow.FindControl("chkReviewMessage")).Checked;
              //          bApproved = ((CheckBox)drRow.FindControl("chkApproveMessage")).Checked;

                        //if (bReviewed && bApproved)
                        //{
                        //    ((LinkButton)drRow.FindControl("btnUpdate")).Visible = false;
                        //}
                        //else
                        //{
                        //    ((LinkButton)drRow.FindControl("btnUpdate")).Visible = true;
                        //}
                    }
                }
            }

        }
        //disable the approve checkbox if the user has review priveleges
        private void checkMode(int iRowIndex)
        {
            GridViewRow drRow;
            string sReviewMode = string.Empty;

            if (grdShowTranslations.Rows.Count > 0)
            {
                drRow = grdShowTranslations.Rows[iRowIndex];
                if ((DataControlRowState.Edit) > 0)
                {
                    if (Session["WEIReviewer"] != null)
                    {
                        if (Session["WEIReviewer"].ToString() == "Y")
                        {

                            //((CheckBox)drRow.FindControl("chkApproveMessage")).Enabled = false;
                           // ((Label)drRow.FindControl("lblApproved")).Visible = true;

                        }
                    }
                }

            }

        }


        private DataTable GetMessageTable(string strMsg1, string strMsg2, string strMsg3)
        {
            DataTable dtMSG = new DataTable();

            dtMSG.Columns.Add("id", typeof(int));
            dtMSG.Columns.Add("messagebody", typeof(string));
            dtMSG.Columns.Add("translatedmessage", typeof(string));
            dtMSG.Columns.Add("responsemessage", typeof(string));

            dtMSG.Rows.Add(1, strMsg1, strMsg2, strMsg3);

            return (dtMSG);
        }

        private string FormatXml(string strUnformattedXml)
        {
            XmlDocument xmlDoc;
            XmlTextWriter xtw = null;                                   //does the formatting
            StringBuilder sbFormattedXml;
            string strReturn;

            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strUnformattedXml);                          //load unformatted xml into a dom
                sbFormattedXml = new StringBuilder();                       //will hold formatted xml
                StringWriter sw = new StringWriter(sbFormattedXml);         //pumps the formatted xml into the StringBuilder above

                xtw = new XmlTextWriter(sw);                //point the xtw at the StringWriter
                xtw.Formatting = Formatting.Indented;       //we want the output formatted
                xmlDoc.WriteTo(xtw);                        //get the dom to dump its contents into the xtw 
                strReturn = sbFormattedXml.ToString();                           //return the formatted xml
            }
            catch (Exception)
            {
                strReturn = strUnformattedXml;


            }
            finally
            {
                if (xtw != null) xtw.Close();               //clean up even if error
            }
            return strReturn;
        }

        private void PopulateMessageDetail()
        {
            int _iRowIndex = 0;
            string strMessage = string.Empty;
            if (ViewState["MessageId"] != null)
            {
                _iRowIndex = Convert.ToInt32(ViewState["MessageId"]);
            }
            //ViewState["RowIndex"] = _iRowIndex;
            populateTranslationsGrid(_iRowIndex, false);
            //if (ViewState["EditRow"].ToString() != "")
            //{
            //    setTranslations(_iRowIndex);
            //}
            tbContainer.ActiveTabIndex = 0;
            tbTranslations.Visible = true;

            //Tab Original Message
            //====================

            if (grdShowMessages.Rows.Count > 1)
            {
                if (Session["DataTable"] != null)
                {
                    _dDataTable = (DataTable)Session["DataTable"];
                    if (ViewState["MessageId"] != null)
                    {
                        DataRow[] drSelectedRow = _dDataTable.Select("id = " + ViewState["MessageId"].ToString());

                        if (drSelectedRow.Length > 0)
                        {
                            strMessage = drSelectedRow[0]["messagebody"].ToString();
                            if (!string.IsNullOrEmpty(strMessage)) txtMessagebody.Text = FormatXml(strMessage);

                            //if (ViewState["TranslatedMessage"] != null)
                            //{
                            //    strMessage = ViewState["TranslatedMessage"].ToString();
                            //}
                            strMessage = drSelectedRow[0]["translatedmessage"].ToString();
                            if (!string.IsNullOrEmpty(strMessage)) txtTranslatedmessage.Text = FormatXml(strMessage);

                            //strMessage = drSelectedRow[0]["responsemessage"].ToString();
                            //if (!string.IsNullOrEmpty(strMessage)) txtResponsemessage.Text = FormatXml(strMessage);
                        }
                    }

                }
            

            //Tab Audit Details
            //====================
            int _igrdRowIndex;
            _igrdRowIndex = Convert.ToInt32(ViewState["GridRowIndex"]);
            populateAuditGrid(_igrdRowIndex, _iRowIndex);
            }
        }

        protected void NavigateMessage(object sender, CommandEventArgs e)
        {
            int _currentRowNumber = -1;

            if (Session["DataTable"] != null)
            {
                _dDataTable = (DataTable)Session["DataTable"];

                switch (e.CommandName)
                {
                    case "First":
                        _currentRowNumber = 0;
                        break;
                    case "Previous":
                        _currentRowNumber = Int32.Parse(ViewState["GridRowIndex"].ToString()) - 1;
                        break;
                    case "Next":
                        _currentRowNumber = Int32.Parse(ViewState["GridRowIndex"].ToString()) + 1;
                        break;
                    case "Last":
                        _currentRowNumber = _dDataTable.Rows.Count - 1;
                        break;
                }

                if (_currentRowNumber < 0) _currentRowNumber = 0;
                if (_currentRowNumber > _dDataTable.Rows.Count - 1) _currentRowNumber = _dDataTable.Rows.Count - 1;

                if (_currentRowNumber >= 0 && _currentRowNumber <= _dDataTable.Rows.Count - 1)
                {
                    ViewState["GridRowIndex"] = _currentRowNumber;
                    ViewState["MessageId"] = Int32.Parse(_dDataTable.Rows[_currentRowNumber]["id"].ToString());
                    //Session["MessageId"] = Int32.Parse(_dDataTable.Rows[_currentRowNumber]["id"].ToString());
                    PopulateMessageDetail();
                    grdShowTranslations.EditIndex = -1;

                    ResetPopupControls();
                    ModalPopupExtender1.Show();
                }
            }
        }

        private void ResetPopupControls()
        {
            int intCurrentRowNumber =  Int32.Parse(ViewState["GridRowIndex"].ToString());
            _dDataTable = (DataTable)Session["DataTable"];

            if (intCurrentRowNumber > -1 && null != _dDataTable)
            {
                if (0 == intCurrentRowNumber)
                {
                    btnFirstRequest.Enabled = false;
                    btnPreviousRequest.Enabled = false;
                    btnNextRequest.Enabled = true;
                    btnLastRequest.Enabled = true;
                }
                else if (Convert.ToInt32(_dDataTable.Rows.Count) - 1 == intCurrentRowNumber)
                {
                    btnFirstRequest.Enabled = true;
                    btnPreviousRequest.Enabled = true;
                    btnNextRequest.Enabled = false;
                    btnLastRequest.Enabled = false;
                }
                else
                {
                    btnFirstRequest.Enabled = true;
                    btnPreviousRequest.Enabled = true;
                    btnNextRequest.Enabled = true;
                    btnLastRequest.Enabled = true;
                }
            }
        }

        protected void grdShowTranslations_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string strDescription = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");

                    if (null != Session["DataTable"] && null != ViewState["MessageId"])
                    {
                        _dDataTable = (DataTable)Session["DataTable"];
                        if (_dDataTable != null)
                        {
                    //        //_dDataTable.DefaultView.RowFilter = "ID=" + Int32.Parse(ViewState["MessageId"].ToString());
                            if (ViewState["MessageId"].ToString() != null)
                            {
                                DataRow[] drSelectedRow = _dDataTable.Select("id = " + ViewState["MessageId"].ToString());
                                if (drSelectedRow.Length > 0)
                                {
                                    //string strHasCTC = _dDataTable.Rows[0]["HasCTC"].ToString();
                                    //string strDescription = _dDataTable.Rows[0]["Description"].ToString().Trim();

                                    string strHasCTC = drSelectedRow[0]["HasCTC"].ToString();
                                    strDescription = drSelectedRow[0]["Description"].ToString().Trim();
                                    string strHasCTCApproved = drSelectedRow[0]["HasCTCApproved"].ToString();

                                    int intStatus = 0;
                                    switch (strDescription)
                                    {
                                        case "UnProcessed":
                                            intStatus = 1;
                                            break;

                                        case "Translated":
                                            intStatus = 2;
                                            break;

                                        case "Review":
                                            intStatus = 3;
                                            break;

                                        case "SentForOFACCheck":
                                            intStatus = 4;
                                            break;

                                        case "OFACResponseReceived":
                                            intStatus = 5;
                                            break;

                                        case "Processed":
                                            intStatus = 6;
                                            break;
                                    }

                                    if (null != btnEdit)
                                    {

                                        //if (strHasCTCApproved.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                                        //{
                                        //    lblNoteValue.Visible = true;
                                        //    lblNote.Visible = true;
                                        //    lblNoteValue.Text = "Translations have been already approved";

                                        //    btnEdit.Enabled = false;
                                        //    //btnEdit.Text = "";
                                        //    if (intStatus == 3)
                                        //    {
                                        //        btnProcessReview.Enabled = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        btnProcessReview.Enabled = false;
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (strHasCTC.Equals("No", StringComparison.InvariantCultureIgnoreCase))
                                        //    {
                                        //        lblNoteValue.Visible = true;
                                        //        lblNote.Visible = true;
                                        //        lblNoteValue.Text = "No CTC Code";

                                        //        btnEdit.Enabled = false;
                                        //        //btnEdit.Text = "";
                                        //        btnProcessReview.Enabled = false;
                                        //    }
                                        //    else
                                        //    {
                                        if (intStatus > 3)
                                        {
                                            lblNoteValue.Visible = true;
                                            lblNote.Visible = true;
                                            if (intStatus == 4)
                                            {
                                                lblNoteValue.Text = "Message has been sent for OFAC check";
                                                btnEdit.Enabled = false;
                                                //btnEdit.Text = "";
                                                btnProcessReview.Enabled = false;
                                            }
                                            
                                            else if (intStatus == 5)
                                            {
                                                lblNoteValue.Text = "Message was OFAC checked successfully. ";
                                            }
                                            btnEdit.Enabled = false;
                                            //btnEdit.Text = "";
                                            btnProcessReview.Enabled = false;
                                        }
                                        else
                                        {
                                            if ( intStatus == 3)
                                            {
                                                lblNoteValue.Text = "Translations need to be reviewed before releasing the message for OFAC check ";
                                            }
                                            lblNoteValue.Visible = true;
                                            lblNote.Visible = true;
                                            //lblNoteValue.Text = "";

                                            btnEdit.Enabled = true;
                                            //btnEdit.Text = "Edit";
                                            btnProcessReview.Enabled = true;
                                        }
                                        //}
                                        //}
                                        //    }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.logError("unexpected error occured in grdShowTranslations_OnRowDataBound method - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);
            }
        }

        private void PopulateOriginalMessage()
        {
            DataSet _dataset;
            try
            {
                _dataset = _dataAccess.getOriginalMessages(Convert.ToInt32(ViewState["MessageId"]));

                if (null != _dataset && null != _dataset.Tables[0])
                {
                    string strMessage = _dataset.Tables[0].Rows[0]["messagebody"].ToString();
                    if (!string.IsNullOrEmpty(strMessage)) txtMessagebody.Text = FormatXml(strMessage);

                    strMessage = _dataset.Tables[0].Rows[0]["translatedmessage"].ToString();
                    if (!string.IsNullOrEmpty(strMessage)) txtTranslatedmessage.Text = FormatXml(strMessage);

                    //strMessage = _dataset.Tables[0].Rows[0]["responsemessage"].ToString();
                    //if (!string.IsNullOrEmpty(strMessage)) txtResponsemessage.Text = FormatXml(strMessage);
                }
            }
            catch (Exception ex)
            {
                LogUtil.logError("Unexpected error occured in PopulateOriginalMessage method - " + ex.InnerException.ToString());
                Response.Redirect("CustomErrorPage.aspx", false);

            }

        }
       

    }

}