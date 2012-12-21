using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Telavance.AdvantageSuite.Wei.DBUtils;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Telavance.AdvantageSuite.Wei.WeiDashboard.Pages
{

    public enum UserType
    {
        Reviewer = 1,
        Approver = 2
    }

    public partial class Messages1 : System.Web.UI.Page
    {
        private DBUtil mdbUtils;
        private DataTable mdtMSG;
        private int mintKey;
        private UserType meUserType;
        private string mstrUserName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Telavance WEI Dashboard";

            MSG_DateCannotLaterThenToday.Value = Resources.locStrings.MSG_DateCannotLaterThenToday;
            MSG_DateCannotLaterThenToDate.Value = Resources.locStrings.MSG_DateCannotLaterThenToDate;
            MSG_DateCannotBeforeThenFromDate.Value = Resources.locStrings.MSG_DateCannotBeforeThenFromDate;
            MSG_FromDateNotEmpty.Value = Resources.locStrings.MSG_FromDateNotEmpty;
            MSG_ToDateNotEmpty.Value = Resources.locStrings.MSG_ToDateNotEmpty;
            MSG_StatusNotEmpty.Value = Resources.locStrings.MSG_StatusNotEmpty;

            mdbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();

            mstrUserName = "SRI";
            meUserType = UserType.Approver;
            SetUserCredentials();

            strPanelClientID.Value = ToDatePicker.ClientID;

            if (!IsPostBack)
            {
                PopulateDropDownlist();

                hfDisplayMessagesLabel.Value = Resources.locStrings.LBL_Messages_DisplayMessages;
                DisplayMessagesLabel.Text = "";

                //for test
                //FromDatePicker.SelectedDate = new DateTime(2011, 1, 1);
                //ToDatePicker.SelectedDate = new DateTime(2012, 1, 1);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedIndexes.Count == 0)
                MessagesGrid.SelectedIndexes.Add(0);
            if (RadGrid2.SelectedIndexes.Count == 0)
            {
                RadGrid2.Rebind();
                RadGrid2.SelectedIndexes.Add(0);
            }
        }

        protected void DataSourceSelecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["filterExpression"] = MessagesGrid.MasterTableView.FilterExpression;

            //rcbStatus.SelectedIndex = 2;
        }

        protected void MessagesGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            RadGrid2.SelectedIndexes.Clear();
            if (null != (e.Item as GridDataItem))
            {
                hfRequestsID.Value = (e.Item as GridDataItem).GetDataKeyValue("ID").ToString();
            }

            if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
            {
                GridDataItem parentItem = e.Item as GridDataItem;

                string strMsg1 = (e.Item as GridDataItem).GetDataKeyValue("OriginalMessage").ToString();
                string strMsg2 = (e.Item as GridDataItem).GetDataKeyValue("TransilatedMessage").ToString();
                string strMsg3 = (e.Item as GridDataItem).GetDataKeyValue("ModifiedMessage").ToString();

                mintKey = Convert.ToInt32((e.Item as GridDataItem).GetDataKeyValue("ID"));

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

        private void CreateChildTable1(string strMsg1, string strMsg2, string strMsg3)
        {
            mdtMSG = new DataTable();

            mdtMSG.Columns.Add("id", typeof(int));
            mdtMSG.Columns.Add("col1", typeof(string));
            mdtMSG.Columns.Add("col2", typeof(string));
            mdtMSG.Columns.Add("col3", typeof(string));

            mdtMSG.Rows.Add(1, strMsg1, strMsg2, strMsg3);
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

        protected void SqlDSTranslations_OnSelecting(object sender, System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Connection.ConnectionString = Generic.cGeneric.GetConnectionString();

            if (!string.IsNullOrWhiteSpace(hfRequestsID.Value))
            {
                e.Command.Parameters["@RequestsID"].Value = int.Parse(hfRequestsID.Value);
            }
            else
            {
                e.Command.Parameters["@RequestsID"].Value = 0;
            }
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
                rcbStatus.Items.Insert(0, new RadComboBoxItem("---Select----", String.Empty));
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected void rapTranslations_OnAjaxRequest(object sender, EventArgs e)
        {
            RadGrid2.Rebind();
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
            string ReviewMode = "Review";

            //if (meUserType == UserType.Reviewer)
            //{
            //    strReviewMode = "Review";
            //}

            if (meUserType == UserType.Approver)
            {
                ReviewMode = "Approve";
            }

            //foreach (GridDataItem item in LabelValueGrid.MasterTableView.Items)
            foreach (GridDataItem row in RadGrid2.MasterTableView.Items)
            {
                if (((CheckBox)row.FindControl("UpdatedCheckBox")).Checked)
                {
                    //Boolean bReviewed = ((CheckBox)item.FindControl("ReviewedCheckBox")).Checked;
                    //Boolean bApproved = ((CheckBox)item.FindControl("ApprovedCheckBox")).Checked;

                    int RequestID = Convert.ToInt32(((GridDataItem)MessagesGrid.SelectedItems[0]).GetDataKeyValue("ID").ToString());
                    string CTCCode = row.GetDataKeyValue("CTCCode").ToString();
                    string ChineseChar = row.GetDataKeyValue("ChineseChar").ToString();
                    string PinYin = "NEW";
                    string NewTrans = ((RadTextBox)row.FindControl("NewTransTextBox")).Text.Trim();
                    string ReviewOper = mstrUserName;
                    bool Reviewed = ((CheckBox)row.FindControl("ReviewedCheckBox")).Checked;
                    string ApproveOper = mstrUserName;
                    bool Approved = ((CheckBox)row.FindControl("ApprovedCheckBox")).Checked;

                    //mdbUtils.UpdateTranslations(Convert.ToInt32(item["id"].Text), ((RadTextBox)item.FindControl("NewTransTextBox")).Text.Trim(), bReviewed, bApproved, mstrUserName, strReviewMode);
                    mdbUtils.UpdateTranslations(RequestID, CTCCode, ChineseChar, PinYin, ReviewMode, NewTrans, ReviewOper, Reviewed, ApproveOper, Approved);
                }
            }
        }

        protected void ReleaseButton_OnClick(object sender, EventArgs e)
        {

            foreach (GridDataItem row in MessagesGrid.MasterTableView.Items)
            {
                if (((CheckBox)row.FindControl("MessagesCheckBox")).Checked)
                {
                    int RequestID = Convert.ToInt32(row.GetDataKeyValue("ID").ToString());
                    string ReviewOper = mstrUserName;

                    mdbUtils.AddAudit(RequestID, 1, "User released request to OFAC check", ReviewOper);
                }
            }
        }
    }
}