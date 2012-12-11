using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.Wei.DBUtils;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace WEI_Dashboard.Pages
{

    public enum UserType
    {
        Reviewer = 1,
        Approver = 2
    }

    public partial class Messages : System.Web.UI.Page
    {
        private DBUtil mdbUtils;
        private DataTable mdtMSG;
        private int mintKey;
        private UserType meUserType;
        private string mstrUserName = "";
        //private string mstrCTCCodes = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Telavance WEI Dashboard";

            mdbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();

            mstrUserName = "SRI";
            meUserType = UserType.Reviewer;
            SetUserCredentials();

            if (!IsPostBack)
            {
                PopulateDropDownlist();                

                hfDisplayMessagesLabel.Value = Resources.locStrings.LBL_Messages_DisplayMessages;
                DisplayMessagesLabel.Text = "";

                //for test
                FromDatePicker.SelectedDate = new DateTime(2011, 1, 1);
                ToDatePicker.SelectedDate = new DateTime(2012, 1, 1);
            }
        }

        protected void DataSourceSelecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["filterExpression"] = MessagesGrid.MasterTableView.FilterExpression;

            //rcbStatus.SelectedIndex = 2;
        }

        protected void MessagesGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                GridDataItem parentItem = e.Item as GridDataItem;


                string strMsg1 = (e.Item as GridDataItem).GetDataKeyValue("OriginalMessage").ToString();
                string strMsg2 = (e.Item as GridDataItem).GetDataKeyValue("TransilatedMessage").ToString();
                string strMsg3 = (e.Item as GridDataItem).GetDataKeyValue("ModifiedMessage").ToString();

                mintKey = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("id"));

                CreateChildTable1(strMsg1, strMsg2, strMsg3);

                RadGrid rg = parentItem.ChildItem.FindControl("OriginalMessageGrid") as RadGrid;
                rg.Rebind();

                rg = parentItem.ChildItem.FindControl("AuditGrid") as RadGrid;
                rg.Rebind();

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
                {
                    ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible = !e.Item.Expanded;
                }
            }
        }

        protected void OriginalMessageGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((source as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            (source as RadGrid).DataSource = mdtMSG;

            //(source as RadGrid).Height = 38 + dtTable.Rows.Count * 22;
            (source as RadGrid).Height = 112;
        }

        protected void AuditGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((source as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

            DataSet dsDataset = mdbUtils.getAuditMessagesByRequest(mintKey);
            (source as RadGrid).DataSource = dsDataset.Tables[0];

            //(source as RadGrid).Height = 38 + dtTable.Rows.Count * 22;
            //(source as RadGrid).Height = 38 + 5 * 22;
            (source as RadGrid).Height = 120;
        }

        protected void RadGrid1_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {            
            DataSet ds = mdbUtils.getTranslations(2998);

            if (hfRequestsID.Value != string.Empty)
            {
                ds = mdbUtils.getTranslations(Convert.ToInt32(hfRequestsID.Value));
                if (ds.Tables.Count > 0)
                {
                    RadGrid1.DataSource = ds.Tables[0];
                }
            }
            else
            {
                RadGrid1.DataSource =  ds.Tables[0];
            }
        }

        protected void OriginalMessageGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                item["col1"].Text = Server.HtmlEncode(item["col1"].Text);
                item["col2"].Text = Server.HtmlEncode(item["col2"].Text);
                item["col3"].Text = Server.HtmlEncode(item["col3"].Text);
            }

        }

        private void CreateChildTable1(string strMsg1, string strMsg2, string strMsg3)
        {
            mdtMSG = new DataTable();

            mdtMSG.Columns.Add("id", typeof(int));
            mdtMSG.Columns.Add("col1", typeof(string));
            mdtMSG.Columns.Add("col2", typeof(string));
            mdtMSG.Columns.Add("col3", typeof(string));

            //dtTest.Rows.Add(1, "MasterTableView", "table");
            mdtMSG.Rows.Add(1, strMsg1, strMsg2, strMsg3);

        }

        private void PopulateDropDownlist()
        {

            try
            {
                DataSet _dataset = mdbUtils.MessageStatus();
                rcbStatus.DataSource = _dataset;
                rcbStatus.DataValueField = _dataset.Tables[0].Columns[0].ColumnName;
                rcbStatus.DataTextField = _dataset.Tables[0].Columns[1].ColumnName;
                rcbStatus.DataBind();
                rcbStatus.Items.Insert(0,  new RadComboBoxItem("---Select----", String.Empty));
            }

            catch (Exception ex)
            {
                throw (ex);
            }

        }

        private void SetUserCredentials()
        {

            if (meUserType == UserType.Approver)
            {
                hfUserType.Value = UserType.Approver.ToString();                
            }

            if (meUserType == UserType.Reviewer)
            {
                hfUserType.Value = UserType.Reviewer.ToString();
                //LabelValueGrid.Columns[5].
            }
            
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            string strReviewMode = "Review";

            //if (meUserType == UserType.Reviewer)
            //{
            //    strReviewMode = "Review";
            //}

            if (meUserType == UserType.Approver)
            {
                strReviewMode = "Approve";
            }

            //foreach (GridDataItem item in LabelValueGrid.MasterTableView.Items)
            foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
            {
                if (((CheckBox)item.FindControl("UpdatedCheckBox")).Checked)
                {
                    Boolean bReviewed = ((CheckBox)item.FindControl("ReviewedCheckBox")).Checked;
                    Boolean bApproved = ((CheckBox)item.FindControl("ApprovedCheckBox")).Checked;

                    mdbUtils.UpdateTranslations(Convert.ToInt32(item["id"].Text), ((RadTextBox)item.FindControl("NewTransTextBox")).Text.Trim(), bReviewed, bApproved, mstrUserName, strReviewMode);
                }
            }
        }

        protected void LabelValueGrid_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //if (hfRequestsID.Value != string.Empty && mstrCTCCodes != null)
            //{

            //    //DataSet ds = mdbUtils.getTranslations(mstrCTCCodes);
            //    //if (ds.Tables.Count > 0)
            //    //{
            //    //    LabelValueGrid.DataSource = ds.Tables[0];
            //    //}
            //}
            //else
            //{
            //    LabelValueGrid.DataSource = "";
            //}
        }

        //private string  GetCTCCodes(string strMessage)
        //{

        //    string pattern = @"\d{4}";
        //    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(pattern);
        //    MatchCollection mc = r.Matches(strMessage);
        //    foreach (Match match in mc)
        //    {
        //        //match
        //    }

        //    return("'3527 5646 7456 2609 2139 0030', '3527 5646 7456 2609 2139 0031'");
        //}

        //protected void MessagesGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        //{
        //    //if (hfSelectedAttribValueIds.Value != "")
        //    //{
        //    //MessagesGrid.DataSource = GetMasterTable();
        //    //}
        //    //else
        //    //{
        //    //    RadGridMaster.DataSource = "";
        //    //}

        //}
    }
}