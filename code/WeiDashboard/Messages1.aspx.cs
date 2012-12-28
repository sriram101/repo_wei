using System;
using System.Web;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using Telavance.AdvantageSuite.Wei.DBUtils;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

//using Telavance.AdvantageSuite.Security;
//using System.DirectoryServices;
//using Telavance.AdvantageSuite.Common;
//using TravelRule.BusinessEntity.Common;
//using TravelRule.BusinessProcess.Common;

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
        //private UserType meUserType;
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
            
            //SetUserCredentials();

            strPanelClientID.Value = ToDatePicker.ClientID;

            if (!IsPostBack)
            {
                UserRole();

                PopulateDropDownlist();

                hfDisplayMessagesLabel.Value = Resources.locStrings.LBL_Messages_DisplayMessages;
                DisplayMessagesLabel.Text = "";                
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

        protected void RadGrid2_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                if (Session["WEIReviewer"].ToString() == "Y")
                {
                    GridDataItem item = e.Item as GridDataItem;

                    ((CheckBox)item.FindControl("ApprovedCheckBox")).Enabled = false;
                }
            }
        }

        protected void MessagesGrid_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                if (Session["WEIReviewer"].ToString() == "Y")
                {
                    GridDataItem item = e.Item as GridDataItem;

                    ((CheckBox)item.FindControl("MessagesCheckBox")).Enabled = false;
                }
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

        //private void SetUserCredentials()
        //{
        //    if (meUserType == UserType.Approver)
        //    {
        //        hfUserType.Value = UserType.Approver.ToString();
        //    }

        //    if (meUserType == UserType.Reviewer)
        //    {
        //        hfUserType.Value = UserType.Reviewer.ToString();
        //        //LabelValueGrid.Columns[5].
        //    }
        //}

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            string ReviewMode = "Review";
            string User = "";

            if (Session["WEIReviewer"].ToString() == "Y") { ReviewMode = "Review"; }
            if (Session["WEIApprover"].ToString() == "Y") { ReviewMode = "Approve"; }            
            if (null != Session["User"]) { User = Session["User"].ToString(); }

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
                    string ReviewOper = User;
                    bool Reviewed = ((CheckBox)row.FindControl("ReviewedCheckBox")).Checked;
                    string ApproveOper = User;
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

        private void UserRoleEx()
        {
            try
            {
                //if (Request.Cookies["WEIRole"] == null || Request.Cookies["WEIRole"] != null)
                // {

                bool bValid = false;
                int rolecount = 0;

                LogUtil.logInfo("User Identity: " + User.Identity.Name);

                Session["User"] = User.Identity.Name;

                string _reviewer = ADConfiguration.GetConfig().Reviewer;
                string[] _arrReviewerRoles = null;
                if (_reviewer != "")
                {
                    _arrReviewerRoles = _reviewer.Split(',');
                }

                rolecount = 0;
                for (int i = 0; i < _arrReviewerRoles.Length; i++)
                {
                    bValid = User.IsInRole(_arrReviewerRoles[i].ToString());
                    if (bValid)
                    {
                        LogUtil.logInfo("User is member of:--" + _arrReviewerRoles[i].ToString());
                        rolecount = rolecount + 1;
                    }
                }


                //string _admin = ADConfiguration.GetConfig().Admin;
                //string[] _arrAdminRoles = null;
                //if (_admin != "")
                //{
                //    _arrAdminRoles = _admin.Split(',');
                //}

                //rolecount = 0;
                //for (int i = 0; i < _arrAdminRoles.Length; i++)
                //{
                //    bValid = principle.IsInRole(_arrAdminRoles[i].ToString());
                //    if (bValid)
                //    {
                //        LogUtil.logInfo("User is member of:--" + _arrAdminRoles[i].ToString());
                //        rolecount = rolecount + 1;
                //    }
                //}
                //if (rolecount > 0)
                //{
                //    WEIRole["WEIAdmin"] = "Y";
                //}
                //else
                //{
                //    WEIRole["WEIAdmin"] = "N";
                //}

                //temporatry setting for testing Jyoti
                //WEIRole["WEIAdmin"] = "N";
                //WEIRole["WEIApprover"] = "Y";
                //}
            }
            catch (Exception ex)
            {
                LogUtil.log("Login Failed", ex);
                //Server.Transfer("/pages/Logout.aspx");
                //Server.Execute("/pages/Logout.aspx");
                Response.Redirect("/pages/Logout.aspx", false);
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

                LogUtil.logInfo("User Identity: " + identity.Name);

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
                    if (bValid)
                    {
                        LogUtil.logInfo("User is member of:--" + _arrReviewerRoles[i].ToString());
                        rolecount = rolecount + 1;
                    }
                }
                WEIRole["WEIReviewer"] = (rolecount > 0) ? "Y" : "N";

                string _approver = ADConfiguration.GetConfig().Approver;
                string[] _arrApproverRoles = null;
                if (_approver != "")
                {
                    _arrApproverRoles = _approver.Split(',');
                }

                rolecount = 0;
                for (int i = 0; i < _arrApproverRoles.Length; i++)
                {
                    bValid = principle.IsInRole(_arrApproverRoles[i].ToString());
                    if (bValid)
                    {
                        LogUtil.logInfo("User is member of:--" + _arrApproverRoles[i].ToString());
                        rolecount = rolecount + 1;
                    }
                }
                WEIRole["WEIApprover"] = (rolecount > 0) ? "Y" : "N";

                //string _admin = ADConfiguration.GetConfig().Admin;
                //string[] _arrAdminRoles = null;
                //if (_admin != "")
                //{
                //    _arrAdminRoles = _admin.Split(',');
                //}

                //rolecount = 0;
                //for (int i = 0; i < _arrAdminRoles.Length; i++)
                //{
                //    bValid = principle.IsInRole(_arrAdminRoles[i].ToString());
                //    if (bValid)
                //    {
                //        LogUtil.logInfo("User is member of:--" + _arrAdminRoles[i].ToString());
                //        rolecount = rolecount + 1;
                //    }
                //}
                //if (rolecount > 0)
                //{
                //    WEIRole["WEIAdmin"] = "Y";
                //}
                //else
                //{
                //    WEIRole["WEIAdmin"] = "N";
                //}

                //temporatry setting for testing
                //WEIRole["WEIAdmin"] = "N";
                //WEIRole["WEIApprover"] = "Y";
                //WEIRole["WEIReviewer"] = "Y";

                //Session["WEIAdmin"] = WEIRole["WEIAdmin"];
                Session["WEIApprover"] = WEIRole["WEIApprover"];
                Session["WEIReviewer"] = WEIRole["WEIReviewer"];                

                //TRSRole.Expires = DateTime.Now.AddMinutes(20); //removed expiration date
                Response.Cookies.Add(WEIRole);

                var httpCookie = Request.Cookies["WEIRole"];
                if (httpCookie != null)
                {
                    //if ((httpCookie["WEIReviewer"] == "N" && httpCookie["WEIApprover"] == "N" && httpCookie["WEIAdmin"] == "N"))
                    if ((httpCookie["WEIReviewer"] == "N" && httpCookie["WEIApprover"] == "N"))
                    {
                        LogUtil.logInfo("Login Failed as No TRSRole Found");
                        //Server.Transfer("/pages/Logout.aspx");
                        //Server.Execute("/pages/Logout.aspx");
                        
                        Response.Redirect("/pages/Logout.aspx", false);
                    }
                }
                else
                {
                    LogUtil.logInfo("Login Failed as No TRSRole Found");
                    //Server.Transfer("/pages/Logout.aspx");
                    //Server.Execute("/pages/Logout.aspx");
                    Response.Redirect("/pages/Logout.aspx", false);
                }
                //}
            }
            catch (Exception ex)
            {
                LogUtil.log("Login Failed", ex);
                //Server.Transfer("/pages/Logout.aspx");
                //Server.Execute("/pages/Logout.aspx");
                Response.Redirect("/pages/Logout.aspx", false);
            }
        }
    }
}