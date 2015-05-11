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
*/

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using System.Configuration;
using System.Text.RegularExpressions;
using Telavance.AdvantageSuite.Wei.DBUtils;



namespace Telavance.AdvantageSuite.Wei.WeiDashboard
{
    public partial class Messages : System.Web.UI.Page
    {
       private DBUtil _dbUtils;
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
       private SortDirection _SortDirection = SortDirection.Descending;
       protected string _sSearchString = String.Empty;
       protected int _currentPageNumber = 1;
       protected int _totalPages=1;
       
       
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
               _dbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();
               _gridRefresh = Convert.ToInt32(ConfigurationManager.AppSettings["GridRefresh"]);
               _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPaging"));
               

               ViewState["GridRowHighlight"] = true;
               ViewState["RowHighlightLabel"] = null;
               ViewState["ShowModal"] = null;

               txtFromDate.Attributes.Add("ReadOnly", "true"); // Make the text box readonly.
               txtFromDate.Attributes.Add("ReadOnly", "true"); // Make the text box readonly.
               lblInfo.Attributes.Add("ReadOnly", "true");
               divPopUp.Visible = false;

               //grdShowMessages.PagerSettings.Position = PagerPosition.Bottom;
               if (!Page.IsPostBack)
               {

                   //Fix for start date and enddate when page is loaded firsttime
                   //shashank - 07/11/2012
                   //set the value of these textboxes only to date value - removed time part of value

                   /*-------------- NEW CODE -----*/
                   txtFromDate.Text = System.DateTime.Now.ToShortDateString();
                   txtToDate.Text = System.DateTime.Now.ToShortDateString();                   
                   _dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
                   _dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);

            
                   LogUtil.logInfo("Method=Page Load, Startdate=" + _dfromdate + " EndDate=" + _dtodate);

                   /* ---------- OLD CODE ------------- */
                   //txtFromDate.Text = System.DateTime.Now.ToString();
                   //txtToDate.Text = System.DateTime.Now.ToString();
                 
                   //_dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
                   //_dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);

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
               }
             
             }
            catch (Exception ex)
            {
                throw (ex);
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

        void AddSortImage(GridViewRow headerRow,GridView grdView)
        {
            Int32 iCol = GetSortColumnIndex(_strSortExp,grdView);
            if (-1 == iCol)
            {
                return;
            }
            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();

            sortImage.Style[HtmlTextWriterStyle.PaddingTop] = "3px";
            
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
           try
           {

               Timer1.Enabled = true;
               resetParams();
               labelText();
               populateGridView();

           }
           catch (Exception ex)
           {
               throw (ex);
           }        
           
       }
       void resetParams()
       {

           txtFromDate.Text = System.DateTime.Now.ToShortDateString();
           txtToDate.Text = System.DateTime.Now.ToLongDateString(); 
           ddlMessageStatus.SelectedIndex = 0; //reset the value from the dropdownlist
           _dfromdate = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.InvariantCulture);
           _dtodate = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.InvariantCulture);
           _hasCTC = true;
           _hasError = false;
           _sdropdownvalue = string.Empty;
           
          
       }
       protected void chkShowCTC_CheckChanged(object sender, EventArgs e)
       {
           _hasCTC = (_hasCTC == chkShowCTC.Checked) ? false : true;
           ViewState["HasCTC"] = _hasCTC;
       }

       protected void chkShowErrors_CheckChanged(object sender, EventArgs e)
       {
           
          _hasError =  (_hasError == chkShowErrors.Checked) ? false : true;
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
           }
           //string _sorting;
           strSort = String.Format("{0} {1}", _strSortExp, (_SortDirection == SortDirection.Descending) ? "DESC" : "ASC");

           if (ViewState["ProcessError"] != null)
           {
               _dDataset = _dbUtils.getMessages(Convert.ToDateTime(ViewState["StartDate"]), Convert.ToDateTime(ViewState["EndDate"]), _sdropdownvalue,
                        Convert.ToBoolean(ViewState["HasCTC"]), Convert.ToBoolean(ViewState["hasError"] ), txtSearchText.Text, strSort, _currentPageNumber, _pageSize, out _recCount);
           }
           else
           {

               _dDataset = _dbUtils.getMessages(_dfromdate, _dtodate, _sdropdownvalue, _hasCTC, _hasError, txtSearchText.Text, strSort, _currentPageNumber, _pageSize, out _recCount);
           }
            //,_currentPageNumber,_pageSize, out _recCount
            _dDataTable = _dDataset.Tables[0];
           
            Session["DataTable"] = _dDataTable;

         

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
           DataView dv = new DataView(_dDataTable, String.Empty, strSort, DataViewRowState.CurrentRows);
           
           grdShowMessages.DataSource = dv;
           grdShowMessages.DataBind();
           
              
           // Display the modal window on grid refresh, if the modal window was already displayed
           if (ViewState["ShowModal"] != null && Timer1.Enabled == true)
           {
               divPopUp.Visible = true;
               panelDragHandle.Visible = true;
               ModalPopupExtender1.Show();
           }
           else
           {
               divPopUp.Visible = false;
               panelDragHandle.Visible = false;
               ModalPopupExtender1.Hide();
           }

           if (grdShowMessages.Rows.Count == 0)
           {
               Panel2.Visible = false;
               btnProcessError.Enabled = false;
               grdShowMessages.EmptyDataText = "No matching records found";
               lblInfo.Text = "";
                
           }
           else
           {
               Panel2.Visible = true;
              
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

       }
       protected void btnClose_Click(object sender, EventArgs e)
       {
           ViewState["ShowModal"] = null;
           panelDragHandle.Visible = false;
           ModalPopupExtender1.Hide();
       }

       protected void btnSearch_Click(object sender, EventArgs e)
       {
           try
           {
               Timer1.Enabled = false; //disable the timer when search criteria is selected.
               _sSearchString = txtSearchText.Text;
               ViewState["ShowModal"] = null; //Reset the viewstate to null to prevent the display of the modal popup
               setParams();
               labelText();
               populateGridView();

           }
           catch (Exception ex)
           {
               throw (ex);
           }
       }

       void populateDropDownlist()
       {

           try
           {
               DataSet _dataset = _dbUtils.MessageStatus();
               ddlMessageStatus.DataSource = _dataset;
               ddlMessageStatus.DataValueField = _dataset.Tables[0].Columns[0].ColumnName;
               ddlMessageStatus.DataTextField = _dataset.Tables[0].Columns[1].ColumnName;
               ddlMessageStatus.DataBind();
               ddlMessageStatus.Items.Insert(0, new ListItem("---Select----", String.Empty));
           }

           catch (Exception ex)
           {
               throw (ex);
           }
          
       }

       
       protected void grdShowMessages_RowCommand(object sender, GridViewCommandEventArgs e)
       {
           try
           {
               int _iRowIndex;
               int _igrdRowIndex;
               
               ViewState["SortExpr"] = null; // to reset the sorting on the grid

               switch (e.CommandName)
               {
                   case "ClickMe":
                       {
                           ViewState["ShowModal"] = true; //Set the viewstate to display the modal popup
                           _iRowIndex = Convert.ToInt32(ViewState["RowIndex"]);
                           _igrdRowIndex = Convert.ToInt32(ViewState["GridRowIndex"]);

                           ViewState["GridRowHighlight"] = null;

                           populateAuditGrid(_igrdRowIndex, _iRowIndex);
                           break;
                       }
                   
               }

           }
           catch (Exception ex)
           {
               throw (ex);
           }
       }

       

       
       void highlightGridRow(GridViewRow _row)
       {
            //Check if the row created is a datarow and the row dataitem is not null
           if (_row.RowType == DataControlRowType.DataRow && _row.DataItem !=null)
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
           

           if (e.Row.RowType == DataControlRowType.Header)
           {
               if (String.Empty != _strSortExp)
               {
                   //AddSortImage(e.Row,grdShowMessages);
               }
           }
          
           highlightGridRow(e.Row);
       }
       protected void grdShowMessages_Sorting(object sender, GridViewSortEventArgs e)
       {
           try
           {
               Timer1.Enabled = false;
               ViewState["ShowModal"] = null; // Set the viewstate to null to prevent the modal window to showup on sorting

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
               throw (ex);
           }        
       }
       
       protected void btnRefresh_Click(object sender, EventArgs e)
       {
           try
           {
               ViewState["ShowModal"] = null; //Reset the viewstate to null to prevent the display of the modal popup
               _sSearchString = txtSearchText.Text;
               Timer1.Enabled = true; //Enable the timer when the refresh button is clicked.
               resetParams();
               labelText();
               populateGridView();
           }
           catch (Exception ex)
           {
               //LogUtil.log("Please check the event viewer or the log file for error message:", ex.InnerException);
               throw (ex);
           }        
       }

       protected void btnProcessError_Click(object sender, EventArgs e)
       {
           string requestID;
           try
           {
               if (grdShowMessages.Rows.Count > 0)
               {
                   WeiMonitoringClient client = new WeiMonitoringClient();
                   if (client != null)
                   {
                       for (int iRow = 0; iRow < grdShowMessages.Rows.Count; iRow++)
                       {
                           GridViewRow row = grdShowMessages.Rows[iRow];
                           bool isChecked = ((CheckBox)row.FindControl("chkSelectRecord")).Checked;
                           if (isChecked)
                           {
                               requestID = row.Cells[1].Text;
                               if (_dbUtils.AddAuditForErrorProcess(Int32.Parse(requestID),"Test"))
                               {
                                    client.reprocess(Int32.Parse(requestID));
                               }
                               ViewState["ProcessError"] = "false";
                           }
                       }
                  
                   }
               }
            
            populateGridView();
           }
            catch (Exception ex)
            {

                throw(ex);
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
               _grdViewRow = ((Button)sender).Parent.Parent as GridViewRow;
               if (_grdViewRow.RowType == DataControlRowType.DataRow)
               {
                   _igrdRowIndex = _grdViewRow.RowIndex;
                   ViewState["GridRowIndex"] = _igrdRowIndex;
                   _iRowIndex = Convert.ToInt32(_grdViewRow.Cells[0].Text);
                   ViewState["RowIndex"] = _iRowIndex;

               }
                   }

           catch (Exception ex)
           {
               throw (ex);
           }        
       }

      
       
       void populateAuditGrid(int _rowindex, int _key)
       {
           String strSort = String.Empty;
           DataSet _dataset;
           try
           {
               // check if the master grid has any records.
               if (grdShowMessages.Rows.Count > 0)
               {

                   if (Session["DataTable"] != null)
                   {

                       _dDataTable = (DataTable)Session["DataTable"];
                       _dDataTable.DefaultView.RowFilter = "ID=" + _key;


                       grdShowDetails.DataSource = _dDataTable;
                       grdShowDetails.DataBind();


                   }
                   //Assign the message details to the labels

                   lblRequestValue.Text = grdShowMessages.Rows[_rowindex].Cells[0].Text;
                   lblMessageValue.Text = grdShowMessages.Rows[_rowindex].Cells[1].Text;
                   lblInterfaceValue.Text = grdShowMessages.Rows[_rowindex].Cells[2].Text;
                   lblDescValue.Text = grdShowMessages.Rows[_rowindex].Cells[3].Text;
                   lblCreateTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[6].Text;
                   lblModifiedTimeValue.Text = grdShowMessages.Rows[_rowindex].Cells[7].Text;

                   _dataset = _dbUtils.getAuditMessagesByRequest(_key);

                   //if (null != _strSortExp && String.Empty != _strSortExp)
                   //{
                   //    strSort = String.Format("{0} {1}", _strSortExp, (_SortDirection == SortDirection.Descending) ? "DESC" : "ASC");
                   //}
                   DataView dv = new DataView(_dataset.Tables[0], String.Empty, strSort, DataViewRowState.CurrentRows);

                   grdShowAudit.DataSource = dv;
                   grdShowAudit.DataBind();

                   if (dv.Table.Rows.Count ==0)
                       
                   {
                       lblRecCount.Text = "No Audit details for Request: " + _key.ToString();
                       lblAuditMessage.Visible = false;
                       Panel4.Visible = false;
                   }
                   else
                   {
                       lblRecCount.Text = "";
                       lblAuditMessage.Visible = true;
                       Panel4.Visible = true;
                   }
                   divPopUp.Visible = true;
                   panelDragHandle.Visible = true;
                   ModalPopupExtender1.Show();
               }
               else
               {
                   divPopUp.Visible = false;
                   panelDragHandle.Visible = false;
                   ModalPopupExtender1.Hide();
                   ViewState["ShowModal"] = null;
               }
           }
           catch (Exception ex)
           {
               throw (ex);
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

       protected void grdShowMessages_SelectedIndexChanged(object sender, EventArgs e)
       {

       }

       
       

    }
    


}