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
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1">
                </telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
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
<asp:HiddenField runat="server" ClientIDMode="Static" EnableViewState="true" Value="" ID="hfUserType" />
<asp:HiddenField runat="server" ClientIDMode="Static" EnableViewState="true" Value="" ID="hfRequestsID" />

<table cellpadding="0px" cellspacing="0px">
    <tr>
        <td  colspan="2">
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
                        <telerik:RadDatePicker runat="server" AutoPostBack="false" CssClass="Label" ID="FromDatePicker" ClientIDMode="Static" Skin="Office2007" TabIndex="1" ></telerik:RadDatePicker>
                    </td>
                    <td colspan="2" style="padding-left:90px">
                        <asp:Label runat="server" ID="Label1" CssClass="Label" Text="<%$ Resources:locStrings, LBL_Messages_MeaasgeText %>" ></asp:Label>
                        <telerik:RadTextBox ID="MeaasgeTextBox" AutoPostBack="false" runat="server" CssClass="Label" Width="240px" Skin="Windows7"  TabIndex="3"></telerik:RadTextBox>
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
                        <telerik:RadDatePicker runat="server" AutoPostBack="false" CssClass="Label" ID="ToDatePicker" ClientIDMode="Static" Skin="Office2007"  TabIndex="2" ></telerik:RadDatePicker>
                    </td>
                    <td style="padding-left:90px">
                        <asp:CheckBox runat="server" AutoPostBack="false" ID="MessagesWithCTCCheckBox" CssClass="Label"  TabIndex="4" Text="<%$ Resources:locStrings, CHK_Messages_MessagesWithCTC %>" />
                    </td>
                    <td style="padding-left:40px">
                        <asp:CheckBox runat="server" AutoPostBack="false" ID="MessagesWithErrorsCheckBox" CssClass="Label"  TabIndex="5" Text="<%$ Resources:locStrings, CHK_Messages_MessagesWithErrors %>" />
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
        <td style="padding: 10px 0px 10px; float:right">
            <telerik:RadButton ID="ReleaseButton" runat="server" Skin="Windows7" ClientIDMode="Static" AutoPostBack="false" OnClientClicked="ReleaseButton_Click" CssClass="Label"  Text="<%$ Resources:locStrings, CMD_Messages_Release %>" > </telerik:RadButton>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <telerik:RadAjaxPanel ID="MasterPanel" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" >
            <%--OnNeedDataSource="MessagesGrid_NeedDataSource"--%>
            <telerik:RadGrid ID="MessagesGrid" runat="server" AllowPaging="True" PageSize="15" Height="305px" Width="100%" Skin="Office2007"
                    AllowMultiRowSelection="false"  DataSourceID="objDsMessagesList"                     
                    OnItemCommand="MessagesGrid_ItemCommand"  >

                <ClientSettings EnablePostBackOnRowClick="true" >
                    <Selecting AllowRowSelect="true" />
                    <ClientEvents OnGridCreated="MessagesGrid_OnGridCreated" OnRowSelected="MessagesGrid_OnRowSelected" /> 
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                </ClientSettings>
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle CssClass="RowCSS" />
                <AlternatingItemStyle CssClass="RowCSS" />

                <MasterTableView DataKeyNames="id,OriginalMessage,TransilatedMessage,ModifiedMessage" ClientDataKeyNames="id" AutoGenerateColumns="false" Width="100%">
                                    
                    <Columns>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ID" DataField="ID" UniqueName="ID" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ID %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="status" DataField="status" UniqueName="status" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Status %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="OFACViolation" DataField="OFACViolation" UniqueName="OFACViolation" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_OFACViolation %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CreateDate" DataField="CreateDate" UniqueName="CreateDate" ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_CreateDate %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ModifiedDate" DataField="ModifiedDate" UniqueName="ModifiedDate" ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ModifiedDate %>"> </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Select %>" AllowFiltering="false" >
                            <HeaderStyle Width="40px" />
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="MessagesCheckBox" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn DataField="OriginalMessage" UniqueName="OriginalMessage" Visible="false" > </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="TransilatedMessage" UniqueName="TransilatedMessage" Visible="false" > </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ModifiedMessage" UniqueName="ModifiedMessage" Visible="false" > </telerik:GridBoundColumn>
                    </Columns>                                    
                                              
                    <NestedViewTemplate>

                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                        <asp:Panel runat="server" ID="InnerContainer" CssClass="viewWrap" Visible="false">

                        <asp:Label runat="server" ID="OriginalDetailsLabel" Text="<%$ Resources:locStrings, LBL_Messages_OriginalDetails %>" ></asp:Label>
                        <br />

                        <telerik:RadGrid ID="OriginalMessageGrid" runat="server" AllowMultiRowSelection="false" Skin="Office2007"
                            OnNeedDataSource="OriginalMessageGrid_NeedDataSource" 
                            onitemdatabound="OriginalMessageGrid_ItemDataBound" >
                                            
                            <ClientSettings>
                                <Scrolling AllowScroll="true" UseStaticHeaders="true"  />
                                <%--<ClientEvents OnRowSelected="OriginalMessageGrid_OnRowSelected" />--%>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>                                
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle CssClass="RowCSS" />
                            <AlternatingItemStyle CssClass="RowCSS" />

                            <MasterTableView DataKeyNames="id" AutoGenerateColumns="false" >
                                <Columns>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col1" DataField="col1" UniqueName="col1" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_OriginalMessage %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col2" DataField="col2" UniqueName="col2" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_TranslatedMessage %>"> </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="col3" DataField="col3" UniqueName="col3" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ResponseMessage %>"> </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="id" DataField="id" UniqueName="id" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_ID %>" Visible="false"> </telerik:GridBoundColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>

                        <asp:Label runat="server" ID="AuditDetailsLabel" Text="<%$ Resources:locStrings, LBL_Messages_AuditDetails %>" ></asp:Label>
                        <br />

                        <telerik:RadGrid ID="AuditGrid" runat="server" AllowPaging="true" PageSize="6" Skin="Office2007"
                            AllowMultiRowSelection="false"
                            OnNeedDataSource="AuditGrid_NeedDataSource" >
                        
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

    <%--<tr>
        <td colspan="2" style="padding: 10px 0px 0px">
            <telerik:RadAjaxPanel ID="LabelValueGridPanel" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" >

            <telerik:RadGrid runat="server" ID="LabelValueGrid" AllowMultiRowEdit="false"  AllowSorting="false" Height="105px" 
                Width="100%" Skin="Office2007" AllowMultiRowSelection="false" OnNeedDataSource="LabelValueGrid_OnNeedDataSource" >
                          
                <ClientSettings AllowKeyboardNavigation="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <ClientEvents OnGridCreated="LabelValueGrid_OnGridCreated" />
                </ClientSettings>
                            
                <HeaderStyle HorizontalAlign="Center" /> 
                            
                <MasterTableView AutoGenerateColumns="false" >
                    <Columns>                        
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CTCCode" DataField="CTCCode" UniqueName="CTCCode" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_CTCCode %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ChineseChar" DataField="ChineseChar" UniqueName="ChineseChar" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_ChineseChar %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="OldTrans" DataField="OldTrans" UniqueName="OldTrans" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_OldTrans %>"> </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:locStrings, GV_COl_Messages_NewTrans %>" UniqueName="value">
                            <ItemTemplate>
                                <telerik:RadTextBox runat="server" Text='<%# Eval("NewTrans") %>' ID="NewTransTextBox" Width="99%" ClientEvents-OnValueChanged="NewTransTextBox_OnValueChanged" > </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ReviewColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Review %>" AllowFiltering="false" >
                            <HeaderStyle Width="55px" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ReviewedCheckBox" runat="server" onClick="ReviewedCheckBox_onClick(this)"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ApproveColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Approve %>" AllowFiltering="false" >
                            <HeaderStyle Width="60px" />
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ApprovedCheckBox" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn DataField="id" UniqueName="id" Display="false">  </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NewTrans" UniqueName="NewTrans1" Display="false" > </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>

            </telerik:RadGrid> 
            </telerik:RadAjaxPanel>
        </td>
    </tr>--%>

    <tr>
        <td  colspan="2" style="padding: 10px 0px 0px">
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1" Width="100%" >

            <%--<telerik:RadGrid ID="RadGrid1" Width="700px" Height="200px" runat="server" OnNeedDataSource="RadGrid1_OnNeedDataSource" >

                <ClientSettings AllowKeyboardNavigation="true">
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <ClientEvents OnGridCreated="RadGrid1_OnGridCreated" />
                </ClientSettings>
                            
                <HeaderStyle HorizontalAlign="Center" /> 

                    <MasterTableView Width="700px" AutoGenerateColumns="false">

                    <Columns>                        
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CTCCode" DataField="CTCCode" UniqueName="CTCCode" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_CTCCode %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ChineseChar" DataField="ChineseChar" UniqueName="ChineseChar" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_ChineseChar %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="OldTrans" DataField="OldTrans" UniqueName="OldTrans" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_OldTrans %>"> </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:locStrings, GV_COl_Messages_NewTrans %>" UniqueName="value">
                            <ItemTemplate>
                                <telerik:RadTextBox runat="server" Text='<%# Eval("NewTrans") %>' ID="NewTransTextBox" Width="99%" ClientEvents-OnValueChanged="NewTransTextBox_OnValueChanged" > </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ReviewColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Review %>" AllowFiltering="false" >
                            <HeaderStyle Width="55px" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ReviewedCheckBox" runat="server" onClick="ReviewedCheckBox_onClick(this)"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ApproveColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Approve %>" AllowFiltering="false" >
                            <HeaderStyle Width="60px" />
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ApprovedCheckBox" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn DataField="id" UniqueName="id" Display="false">  </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NewTrans" UniqueName="NewTrans1" Display="false" > </telerik:GridBoundColumn>
                    </Columns>

                    </MasterTableView>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                </telerik:RadGrid>--%>

            <%--<telerik:RadGrid ID="" Width="700px" Height="200px" runat="server" OnNeedDataSource="" >--%>

            <telerik:RadGrid ID="RadGrid1" runat="server" Skin="Office2007" AllowMultiRowSelection="false" Height="105px"
                            OnNeedDataSource="RadGrid1_OnNeedDataSource" >

                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <ClientEvents OnGridCreated="RadGrid1_OnGridCreated" />
                </ClientSettings>
                            
                <HeaderStyle HorizontalAlign="Center" /> 

                <MasterTableView Width="100%" AutoGenerateColumns="false">
                <Columns>                        
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="CTCCode" DataField="CTCCode" UniqueName="CTCCode" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_CTCCode %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="ChineseChar" DataField="ChineseChar" UniqueName="ChineseChar" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_ChineseChar %>"> </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderStyle-Width="20%" HeaderButtonType="TextButton" SortExpression="OldTrans" DataField="OldTrans" UniqueName="OldTrans" HeaderText="<%$ Resources:locStrings, GV_COl_Messages_OldTrans %>"> </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="<%$ Resources:locStrings, GV_COl_Messages_NewTrans %>" UniqueName="value">
                            <ItemTemplate>
                                <telerik:RadTextBox runat="server" Text='<%# Eval("NewTrans") %>' ID="NewTransTextBox" Width="99%" ClientEvents-OnValueChanged="NewTransTextBox_OnValueChanged" > </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ReviewColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Review %>" AllowFiltering="false" >
                            <HeaderStyle Width="55px" />
                            <ItemStyle HorizontalAlign="Center" Width="55px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ReviewedCheckBox" runat="server" Checked='<%# Eval("Reviewed") %>' onClick="ReviewedCheckBox_onClick(this)"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="ApproveColumn" HeaderText="<%$ Resources:locStrings, GV_COL_Messages_Approve %>" AllowFiltering="false" >
                            <HeaderStyle Width="60px" />
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="ApprovedCheckBox" runat="server" Checked='<%# Eval("Approved") %>' onClick="ApprovedCheckBox_onClick(this)"></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="UpdatedColumn" AllowFiltering="false" Display="false" >
                            <ItemTemplate>
                                <asp:CheckBox ID="UpdatedCheckBox" runat="server" ></asp:CheckBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn DataField="id" UniqueName="id" Display="false">  </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NewTrans" UniqueName="NewTrans1" Display="false" > </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>              
            </telerik:RadGrid>

            </telerik:RadAjaxPanel>
        </td>
    </tr>

    <tr>
        <td colspan="2" style="float:right; padding-top:4px">
            <telerik:RadButton ID="UpdateButton" runat="server" Width="75px" Skin="Windows7" ClientIDMode="Static" OnClick="UpdateButton_OnClick" CssClass="Label"   Text="<%$ Resources:locStrings, CMD_Messages_Update %>" > </telerik:RadButton>
        </td>
    </tr>

<asp:ObjectDataSource runat="server" ID="objDsMessagesList" TypeName="Telavance.AdvantageSuite.Wei.WeiDashboard.DataSources.cDS_Messages_ListDataSource"
                    SelectCountMethod="GetTotalRecords" SelectMethod="GetList" EnablePaging="true"
                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortExpression"
                    OnSelecting="DataSourceSelecting" >

<SelectParameters>
    <asp:ControlParameter Name="strStartDate" ControlID="FromDatePicker" PropertyName="SelectedDate" Type="String" DefaultValue="" />
    <asp:ControlParameter Name="strEndDate" ControlID="ToDatePicker" PropertyName="SelectedDate" Type="String" DefaultValue="" />
    <asp:ControlParameter Name="intStatus" ControlID="rcbStatus" PropertyName="SelectedValue" Type="Int32" DefaultValue="1" />
    <asp:ControlParameter Name="bHasCTC" ControlID="MessagesWithCTCCheckBox" PropertyName="Checked" Type="Boolean" DefaultValue="0" />
    <asp:ControlParameter Name="bIsError" ControlID="MessagesWithErrorsCheckBox" PropertyName="Checked" Type="Boolean" DefaultValue="0" />
    <asp:ControlParameter Name="strSearchText" ControlID="MeaasgeTextBox" PropertyName="Text" Type="String" DefaultValue="" />
</SelectParameters>
</asp:ObjectDataSource>
    
</table>

</asp:Content>