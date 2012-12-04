using System;
using System.Data;
using Telerik.Web.UI;
using Telavance.AdvantageSuite.Wei.DBUtils;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace WEI_Dashboard.Pages
{
    public partial class Messages : System.Web.UI.Page
    {
        private DBUtil mdbUtils;
        private DataTable mdtMSG;
        private int mintKey;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Telavance WEI Dashboard";

            mdbUtils = EnterpriseLibraryContainer.Current.GetInstance<DBUtil>();

            PopulateDropDownlist();

            hfDisplayMessagesLabel.Value = Resources.locStrings.LBL_Messages_DisplayMessages;
            DisplayMessagesLabel.Text = "";
        }

        protected void DataSourceSelecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["filterExpression"] = MessagesGrid.MasterTableView.FilterExpression;
        }

        protected void MessagesGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //if (hfSelectedAttribValueIds.Value != "")
            //{
            MessagesGrid.DataSource = GetMasterTable();
            //}
            //else
            //{
            //    RadGridMaster.DataSource = "";
            //}
            
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

                RadGrid rg = parentItem.ChildItem.FindControl("ChildGrid1") as RadGrid;
                rg.Rebind();

                rg = parentItem.ChildItem.FindControl("ChildGrid2") as RadGrid;
                rg.Rebind();

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
                {
                    ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible = !e.Item.Expanded;
                }
            }
        }

        protected void ChildGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //int intTabIndex = radMultiPage.SelectedIndex;
            //bool bExclusive = chkExclusiveResultSet.Checked;

            GridDataItem parentItem = ((source as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

            //DataTable dtTable = IQR.DataSources.Inventory.Attributes.cDS_Attributes.GetDetailList1(parentItem.GetDataKeyValue("attribvalueids").ToString(), intTabIndex, bExclusive);
            //(source as RadGrid).DataSource = dtTable;

            //DataTable dtTable = GetChildTable1();
            (source as RadGrid).DataSource = mdtMSG;

            //(source as RadGrid).Height = 38 + dtTable.Rows.Count * 22;
            (source as RadGrid).Height = 112;
        }

        protected void ChildGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                item["col1"].Text = Server.HtmlEncode(item["col1"].Text);
                item["col2"].Text = Server.HtmlEncode(item["col2"].Text);
                item["col3"].Text = Server.HtmlEncode(item["col3"].Text);
            }

        }

        protected void ChildGrid2_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            //int intTabIndex = radMultiPage.SelectedIndex;
            //bool bExclusive = chkExclusiveResultSet.Checked;

            GridDataItem parentItem = ((source as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;

            //DataTable dtTable = IQR.DataSources.Inventory.Attributes.cDS_Attributes.GetDetailList1(parentItem.GetDataKeyValue("attribvalueids").ToString(), intTabIndex, bExclusive);
            //(source as RadGrid).DataSource = dtTable;

            //DataTable dtTable = GetChildTable2();
            //(source as RadGrid).DataSource = dtTable;

            DataSet dsDataset = mdbUtils.getAuditMessagesByRequest(mintKey);
            (source as RadGrid).DataSource = dsDataset.Tables[0];                       

            //(source as RadGrid).Height = 38 + dtTable.Rows.Count * 22;
            //(source as RadGrid).Height = 38 + 5 * 22;
            (source as RadGrid).Height = 120;
        }

        protected void LabelValueGrid_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            if (hfSelectedLabelValueId.Value != "")
            {
                LabelValueGrid.DataSource = GetLabelValueTable();
            }
            else
            {
                LabelValueGrid.DataSource = "";
            }
        }

        private DataTable GetMasterTable()
        {
            DataTable dtTest = new DataTable();

            //dtTest.Columns.Add("ID", typeof(int));
            //dtTest.Columns.Add("status", typeof(string));
            //dtTest.Columns.Add("OFACViolation", typeof(string));
            //dtTest.Columns.Add("CreateDate", typeof(DateTime));
            //dtTest.Columns.Add("ModifiedDate", typeof(DateTime));

            //dtTest.Rows.Add(1, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(2, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(3, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(4, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(5, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(6, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(7, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(8, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(9, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(10, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(11, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(12, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(13, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(14, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(15, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(16, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(17, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(18, "OK", "Processed", DateTime.Now, DateTime.Now);
            //dtTest.Rows.Add(19, "OK", "Processed", DateTime.Now, DateTime.Now);

            return (dtTest);
        }

        private DataTable GetChildTable1()
        {
            DataTable dtTest = new DataTable();

            dtTest.Columns.Add("id", typeof(int));
            dtTest.Columns.Add("col1", typeof(string));
            dtTest.Columns.Add("col2", typeof(string));

            //dtTest.Rows.Add(1, "MasterTableView", "table");
            dtTest.Rows.Add(1, "<!DOCTYPE html PUBLIC -//W3C//DTD XHTML 1.0 Transitional//EN http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd><html xmlns=http://www.w3.org/1999/xhtml dir=ltr lang=en class=nojs> </script><link rel=shortcut icon type=image/x-icon href=/shared/templates/master/o365Page/images/favicon.ico /><meta http-equiv=imagetoolbar content=false /><script type=text/javascript src=/en-in/office365/shared/core/2/js/js.ashx?s=Csp;shared></script></script><noscript>", 
                " <label> Label1 </label> <value> Value1 <value> <label> Label2 </label> <value> Value2 <value> <label> Label3 </label> <value> Value3 <value> <label> Label4 </label> <value> Value4 <value> <label> Label5 </label> <value> Value5 <value> <label> Label6 </label> <value> Value6 <value> <label> Label7 </label> <value> Value7 <value> <label> Label8 </label> <value> Value8 <value> <label> Label9 </label> <value> Value9 <value> <label> Label10 </label> <value> Value10 <value>");

            return (dtTest);
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

        private DataTable GetChildTable2()
        {
            DataTable dtTest = new DataTable();

            dtTest.Columns.Add("id", typeof(int));
            dtTest.Columns.Add("AuditStatus", typeof(string));
            dtTest.Columns.Add("Level", typeof(string));
            dtTest.Columns.Add("CreateDate", typeof(DateTime));
            dtTest.Columns.Add("AuditMessage", typeof(string));

            dtTest.Rows.Add(1, "OK2", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(2, "OK3", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(3, "OK4", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(4, "OK5", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(5, "OK2", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(6, "OK3", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(7, "OK4", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(8, "OK5", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(9, "OK2", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(10, "OK3", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(11, "OK4", "Level", DateTime.Now, "Message");
            dtTest.Rows.Add(12, "OK5", "Level", DateTime.Now, "Message");

            return (dtTest);
        }

        private DataTable GetLabelValueTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("label", typeof(string));
            dt.Columns.Add("value", typeof(string));

            dt.Rows.Add("Label1", "Value1");
            dt.Rows.Add("Label2", "Value2");
            dt.Rows.Add("Label3", "Value3");
            dt.Rows.Add("Label4", "Value4");
            dt.Rows.Add("Label5", "Value5");
            dt.Rows.Add("Label6", "Value6");
            dt.Rows.Add("Label7", "Value7");
            dt.Rows.Add("Label8", "Value8");
            dt.Rows.Add("Label9", "Value9");
            dt.Rows.Add("Label10", "Value10");

            return (dt);
        }

        void PopulateDropDownlist()
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
    }
}