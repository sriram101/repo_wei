<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/WEIDashboard.Master" AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="WEI_Dashboard.Pages.Messages" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/Generic.css" rel="Stylesheet" type="text/css" />
    <link href="../Styles/Messages.css" rel="Stylesheet" type="text/css" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<telerik:RadScriptManager runat="server" ID="MainRadScriptManager" EnablePartialRendering="true" EnableViewState="true">
    <Scripts>
        <asp:ScriptReference Path="~/Scripts/Messages.js" />
    </Scripts>
</telerik:RadScriptManager>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="LabelValueGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="LabelValueGrid" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="LabelValueGrid"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="MessagesGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="MessagesGrid" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="MessagesGrid"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="RadContextMenu1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadContextMenu1" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="RadContextMenu1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>

<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" BackColor="#E0E0E0" Transparency="20">
<table style="height: 100%; width: 100%" border="0">
        <tr>
            <td width="100%" align="center" valign="middle">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/48.gif" AlternateText="loading"></asp:Image>
            </td>
        </tr>
    </table>
</telerik:RadAjaxLoadingPanel>

<asp:HiddenField runat="server" ClientIDMode="Static" EnableViewState="true" Value="" ID="hfSelectedLabelValueId" />
<asp:HiddenField runat="server" ClientIDMode="Static" EnableViewState="true" Value="" ID="hfDisplayMessagesLabel" />


<table cellpadding="0px" cellspacing="0px">
    <tr>
        <td>
            <div style="border: 1px solid; padding-left:80px; " >
            
            <table class="Label">
                <tr>
                    <td>                           
                        <asp:Label runat="server" ID="SearchCriteriaLabel" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_SearchCriteria %>" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="FromDateLabel" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_FromCreateDate %>" ></asp:Label>
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" CssClass="Label" ID="FromDatePicker" ClientIDMode="Static" Skin="Office2007" TabIndex="1" ></telerik:RadDatePicker>
                    </td>
                    <td colspan="2" style="padding-left:90px">
                        <asp:Label runat="server" ID="Label1" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_MeaasgeText %>" ></asp:Label>
                        <telerik:RadTextBox ID="MeaasgeTextBox" runat="server" CssClass="Label" Width="240px" Skin="Windows7"  TabIndex="3"></telerik:RadTextBox>
                    </td>
                    <td  style="padding-left:90px">
                        <asp:Label runat="server" ID="StatusLabel" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_Status %>" ></asp:Label>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="rcbStatus" Runat="server" DropDownWidth="157px" Width="157px" Skin="Windows7" 
                                HighlightTemplatedItems="True" MaxHeight="135px" AutoPostBack="false" ClientIDMode="Static"  TabIndex="6" > </telerik:RadComboBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label runat="server" ID="ToDateLabel" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_ToCreateDate %>" ></asp:Label>
                    </td>
                    <td>
                        <telerik:RadDatePicker runat="server" CssClass="Label" ID="ToDatePicker" ClientIDMode="Static" Skin="Office2007"  TabIndex="2" ></telerik:RadDatePicker>
                    </td>
                    <td style="padding-left:90px">
                        <asp:CheckBox runat="server" ID="MessagesWithCTCCheckBox" CssClass="Label"  TabIndex="4" Text="<%$ Resources:locStrings, CHK_Messages_MessagesWithCTC %>" />
                    </td>
                    <td style="padding-left:40px">
                        <asp:CheckBox runat="server" ID="MessagesWithErrorsCheckBox" CssClass="Label"  TabIndex="5" Text="<%$ Resources:locStrings, CHK_Messages_MessagesWithErrors %>" />
                    </td>
                    <td  style="padding-left:90px" colspan="2">
                        <telerik:RadButton ID="ListButton" runat="server" Width="75px" Skin="Windows7" ClientIDMode="Static" AutoPostBack="false" OnClientClicked="ListButton_Click" CssClass="Label"  TabIndex="7" Text="<%$ Resources:locStrings, CMD_Messages_List %>" > </telerik:RadButton>
                        <telerik:RadButton ID="RadButton1" runat="server" Skin="Windows7" ClientIDMode="Static" AutoPostBack="false" OnClientClicked="RefreshMessagesButton_Click" CssClass="Label"  TabIndex="8" Text="<%$ Resources:locStrings, CMD_Messages_RefreshMessages %>" > </telerik:RadButton>
                    </td>
                </tr>
            </table>

            </div>
        </td>
    </tr>

    <tr>
        <td style="padding: 10px 0px 10px">
            <asp:Label runat="server" ID="DisplayMessagesLabel" CssClass="Label" ClientIDMode="Static" Text="<%$ Resources:locStrings, LBL_Messages_DisplayMessages %>" ></asp:Label>
        </td>
    </tr>

    <tr>
        <td>
            <telerik:RadAjaxPanel ID="MasterPanel" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" >
            <%--OnNeedDataSource="MessagesGrid_NeedDataSource"--%>
            <telerik:RadGrid ID="MessagesGrid" runat="server" AllowPaging="True" PageSize="15" Height="305px" Width="100%" Skin="Office2007"
                    AllowMultiRowSelection="false"  DataSourceID="objDsMessagesList"                     
                    OnItemCommand="MessagesGrid_ItemCommand">

                <ClientSettings >
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnGridCreated="MessagesGrid_OnGridCreated" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle CssClass="RowCSS" />
                <AlternatingItemStyle CssClass="RowCSS" />

                <MasterTableView DataKeyNames="id,OriginalMessage,TransilatedMessage,ModifiedMessage" AutoGenerateColumns="false" Width="100%">
                                    
                    <Columns>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ID" DataField="ID" UniqueName="ID" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ID %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="status" DataField="status" UniqueName="status" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Status %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="OFACViolation" DataField="OFACViolation" UniqueName="OFACViolation" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_OFACViolation %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CreateDate" DataField="CreateDate" UniqueName="CreateDate" ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_CreateDate %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ModifiedDate" DataField="ModifiedDate" UniqueName="ModifiedDate" ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ModifiedDate %>"> </telerik:GridBoundColumn>

                        <telerik:GridBoundColumn DataField="OriginalMessage" UniqueName="OriginalMessage" Visible="false" > </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TransilatedMessage" UniqueName="TransilatedMessage" Visible="false" > </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ModifiedMessage" UniqueName="ModifiedMessage" Visible="false" > </telerik:GridBoundColumn>
                    </Columns>                                    
                                              
                    <NestedViewTemplate>

                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                        <asp:Panel runat="server" ID="InnerContainer" CssClass="viewWrap" Visible="false">

                        <asp:Label runat="server" ID="OriginalDetailsLabel" Text="<%$ Resources:locStrings, LBL_Messages_OriginalDetails %>" ></asp:Label>
                        <br />

                        <telerik:RadGrid ID="ChildGrid1" runat="server" AllowMultiRowSelection="false" Skin="Office2007"
                            OnNeedDataSource="ChildGrid1_NeedDataSource" 
                            onitemdatabound="ChildGrid1_ItemDataBound" >
                                            
                            <ClientSettings>
                                <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                                <ClientEvents OnRowSelected="ChildGrid1_OnRowSelected" />
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>                                
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle CssClass="RowCSS" />
                            <AlternatingItemStyle CssClass="RowCSS" />

                            <MasterTableView DataKeyNames="id" AutoGenerateColumns="false" >
                                <Columns>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col1" DataField="col1" UniqueName="col1" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Col1 %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col2" DataField="col2" UniqueName="col2" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Col2 %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col3" DataField="col3" UniqueName="col3" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Col3 %>"> </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="id" DataField="id" UniqueName="id" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ID %>" Visible="false"> </telerik:GridBoundColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>

                        <asp:Label runat="server" ID="AuditDetailsLabel" Text="<%$ Resources:locStrings, LBL_Messages_AuditDetails %>" ></asp:Label>
                        <br />

                        <telerik:RadGrid ID="ChildGrid2" runat="server" AllowPaging="true" PageSize="6" Skin="Office2007"
                            AllowMultiRowSelection="false"
                            OnNeedDataSource="ChildGrid2_NeedDataSource" >
                        
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                            </ClientSettings>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle CssClass="RowCSS" />
                            <AlternatingItemStyle CssClass="RowCSS" />

                            <MasterTableView DataKeyNames="id" AutoGenerateColumns="false">
                                <Columns>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ID" DataField="ID" UniqueName="ID" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ID %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="Description" DataField="Description" UniqueName="Description" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_AuditStatus %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="Level" DataField="Level" UniqueName="Level" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Level %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CreatedDateTime" DataField="CreatedDateTime" ItemStyle-HorizontalAlign="Center" UniqueName="CreatedDateTime" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_CreateDate %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="message" DataField="message" UniqueName="message" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_AuditMessage %>"> </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>

                        </telerik:RadGrid>

                        </asp:Panel>
                    </NestedViewTemplate>                    
                </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </td>
    </tr>

    <tr>
        <td style="padding: 10px 0px 0px">
            <telerik:RadAjaxPanel ID="LabelValueGridPanel" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" >

            <telerik:RadGrid runat="server" ID="LabelValueGrid" AllowMultiRowEdit="false"  AllowSorting="false" AllowPaging="true" PageSize="5" 
                Height="150px" Width="100%" Skin="Office2007" AllowMultiRowSelection="false" 
                OnNeedDataSource="LabelValueGrid_OnNeedDataSource" >
                            
                <ClientSettings AllowKeyboardNavigation="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <ClientEvents OnGridCreated="LabelValueGrid_OnGridCreated" />
                </ClientSettings>
                            
                <HeaderStyle HorizontalAlign="Center" /> 
                            
                <MasterTableView AutoGenerateColumns="false" >
                    <Columns>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="label" DataField="label" UniqueName="label" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_Label %>"> </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="value" DataField="value" UniqueName="value" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_Value %>"> </telerik:GridBoundColumn>--%>

                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:locStrings, GV_COl_Messages_Value %>" UniqueName="value">
                            <ItemTemplate>
                                <telerik:RadTextBox Text='<%# Eval("value") %>' ID="ValueTextBox" Width="95%" runat="server" > </telerik:RadTextBox>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <telerik:RadTextBox Text='<%# Bind("TextData") %>' ID="EditTextBox" runat="server">
                                </telerik:RadTextBox>
                            </EditItemTemplate>--%>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>

            </telerik:RadGrid> 
            </telerik:RadAjaxPanel>
        </td>
    </tr>

<asp:ObjectDataSource runat="server" ID="objDsMessagesList" TypeName="Telavance.AdvantageSuite.Wei.WeiDashboard.DataSources.cDS_Messages_ListDataSource"
                    SelectCountMethod="GetTotalRecords" SelectMethod="GetList" EnablePaging="true"
                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortExpression"
                    OnSelecting="DataSourceSelecting" >

<SelectParameters>
    <asp:ControlParameter Name="strStartDate" ControlID="FromDatePicker" PropertyName="SelectedDate" Type="String" DefaultValue="" />
    <asp:ControlParameter Name="strEndDate" ControlID="ToDatePicker" PropertyName="SelectedDate" Type="String" DefaultValue="" />
    <asp:ControlParameter Name="intStatus" ControlID="rcbStatus" PropertyName="SelectedIndex" Type="Int32" DefaultValue="1" />
    <asp:ControlParameter Name="bHasCTC" ControlID="MessagesWithCTCCheckBox" PropertyName="Checked" Type="Boolean" DefaultValue="0" />
    <asp:ControlParameter Name="bIsError" ControlID="MessagesWithErrorsCheckBox" PropertyName="Checked" Type="Boolean" DefaultValue="0" />
    <asp:ControlParameter Name="strSearchText" ControlID="MeaasgeTextBox" PropertyName="Text" Type="String" DefaultValue="" />
</SelectParameters>
</asp:ObjectDataSource>
    
</table>

</asp:Content>