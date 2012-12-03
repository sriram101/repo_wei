<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMessageViewer.ascx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.UserControls.ucMessageViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">

    function ExpandCollapse() {
        var collPanel = $find("CollapsiblePanelExtender1");
        if (collPanel.get_Collapsed())
            collPanel.set_Collapsed(false);
        else
            collPanel.set_Collapsed(true);
    }

    function checkDatefrom(sender, args) {

        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day later than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format));
        }
    }
    function checkDateTo(sender, args) {

        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day later than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format));
        }
    }

        
</script>
<style type="text/css">
    .SearchTable td
    {
        padding: 5px 7px 5px 7px;
    }
    .style1
    {
        width: 104px;
    }
    .style2
    {
        width: 287px;
    }
    .caldisplay tr td {
         padding:0;margin:0; 
         background-color:White;
    } 


</style>
<asp:Panel ID="SearchWrap" runat="server" Style="border: solid 1px blue; border-collapse: collapse;
    margin-bottom: 10px;">
    <asp:Panel ID="searchHeader" runat="server" CssClass="collapsePanelHeader">
        <asp:Image ID="ImgHidden" runat="server" ImageUrl="~/images/collapse.jpg" CssClass="collapsebleImage" />
        <asp:Label ID="lblsearchhdr" runat="server" CssClass="LabelCaption"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlSearch" runat="server" CssClass="collapsePanelContent">
        <table width="100%" class="SearchTable">
            <tr>
                <td class="style1">
                    <asp:Label ID="lblFromDate" Text="From Date:" runat="server" CssClass="Label"></asp:Label>
                </td>
                <td class="caldisplay">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="200px" CssClass="TextBox"></asp:TextBox>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Calendar.png" ImageAlign="Middle" />
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" EnableViewState="false"
                        TargetControlID="txtFromDate" OnClientDateSelectionChanged="checkDatefrom" PopupPosition="Right"
                        PopupButtonID="Image1"></cc1:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblToDate" Text="To Date:" runat="server" CssClass="Label"></asp:Label>
                </td>
                <td class="caldisplay">
                    <asp:TextBox ID="txtToDate" runat="server" Width="200px" CssClass="TextBox"></asp:TextBox>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Calendar.png" ImageAlign="Middle" />
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                        OnClientDateSelectionChanged="checkDateTo" PopupPosition="Right" PopupButtonID="Image2" ></cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" CssClass="Label" 
                            Text="Message Text:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;   
                        <asp:CheckBox ID="chkShowCTC" runat="server" Text="Messages with CTC" checked="true" CssClass="Checkbox" OnCheckedChanged="chkShowCTC_CheckChanged"/>
                        <asp:CheckBox ID="chkShowErrors" runat="server" Text="Messages with Errors" checked="false" CssClass="Checkbox" OnCheckedChanged="chkShowErrors_CheckChanged"/>
                       </td>
                       </tr>
            <tr>
                <td class="style1">
                    <asp:Label ID="lblSearchText" runat="server" CssClass="Label" Text="Search Text:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearchText" runat="server" Width="200px" CssClass="TextBox" ToolTip="Enter Search Text Here"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnSearch" runat="server" CssClass="rbutton" Width="100px" Text="Search" OnClick="btnSearch_Click"
                        UseSubmitBehavior="true" ToolTip="Search" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnReset" runat="server" CssClass="rbutton" Width="100px" Text="Reset" UseSubmitBehavior="true"
                        OnClick="btnReset_Click" ToolTip="Reset" />
                    &nbsp;&nbsp;
                    <asp:Label ID="MsgDisplay" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <cc1:CollapsiblePanelExtender ID="cpe2" runat="server" TargetControlID="pnlSearch"
        CollapseControlID="searchHeader" ExpandControlID="searchHeader" Collapsed="false"
        ImageControlID="ImgHidden" CollapsedSize="0" CollapsedImage="~/images/collapse.jpg"
        ExpandedImage="~/images/expand.jpg" CollapsedText="Search" ExpandedText="Search"
        TextLabelID="lblserachhdr"></cc1:CollapsiblePanelExtender>
</asp:Panel>
<br />
<asp:Timer runat="server" ID="UpdateTimer" Enabled="false" OnTick="btnSearch_Click" />
<asp:UpdatePanel ID="messagePanel" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <table style="border: solid 1px #91a7b4; border-collapse: collapse; width: 100%;">
            <tr>
                <td>
                    <asp:Panel ID="pHeader" runat="server" CssClass="collapsePanelHeader">
                        <asp:Image ID="ImgHiddenmsg" runat="server" ImageUrl="~/images/collapse.jpg" CssClass="collapsebleImage" />
                        <asp:Label ID="lblMessageHdr" runat="server" CssClass="LabelCaption"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pmessage" runat="server"  CssClass="collapsePanelContent" Style="padding-top: 0px;
                        padding-bottom: 0px;">
                        <asp:GridView ID="grdReviewMessages" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            EmptyDataText="No matching records found" HorizontalAlign="Center" AllowSorting="True"
                            EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-Font-Bold="true"
                            EmptyDataRowStyle-BackColor="#d1dbe0" Width="100%" CssClass="GridViewStyle" Style="border-width: 0px;"
                            OnRowCommand="grdReviewMessages_RowCommand" OnRowCreated="grdReviewMessages_RowCreated"
                            OnSorting="grdReviewMessages_Sorting" OnRowDataBound="grdReviewMessages_RowDataBound"
                            OnPageIndexChanging="grdReviewMessages_PageIndexChanging">
                            <EmptyDataRowStyle BackColor="#D1DBE0" Font-Bold="True" HorizontalAlign="Center" />
                            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
                            <PagerStyle CssClass="PagerStyle" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                            <EditRowStyle CssClass="EditRowStyle" />
                            <RowStyle CssClass="RowStyle" />
                            <Columns>
                                <asp:TemplateField SortExpression="ID" HeaderText="ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnView" runat="server" Text='<%# Eval("RequestId")%>' OnClick="btnEntry_Click"
                                            CommandArgument='<%# Eval("RequestId")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle />
                                </asp:TemplateField>
                                    <asp:BoundField HeaderText="Message ID" DataField="Name" SortExpression="Name" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Interface Name" DataField="InterfaceName" SortExpression="InterfaceName" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Status" DataField="OFACStatus" SortExpression="OFACStatus"  ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Error" DataField="IsErrors" Visible="false" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="OFAC Violation" DataField="Description" SortExpression="Description" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Create Date" DataField="CreatedDateTime" SortExpression="CreatedDateTime" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Modified Date" DataField="ModifiedDateTime" SortExpression="ModifiedDateTime" ItemStyle-Wrap="false"/>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
            <table>
                <tr style="height: 10px;">
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="LblRows" runat="server" Text="Rows Per Page:" CssClass="Label"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged"
                            CssClass="selectboxPaging">
                            <asp:ListItem Selected="True">10</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnFirst" runat="server" Text="|<" CommandName="First" OnCommand="GetPageIndex"
                            ToolTip="First" CssClass="Label"/>
                    </td>
                    <td>
                        <asp:Button ID="btnPrevious" runat="server" Text="<<" CommandName="Previous" ToolTip="Previous"
                            OnCommand="GetPageIndex" CssClass="Label"/>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="LblCurrPage" runat="server" Text="Page" CssClass="Label"
                            ></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlPage" CssClass="selectboxPaging"
                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged"
                            > </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblOf" Text="of" runat="server" CssClass="Label"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalPages" runat="server" CssClass="Label"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnNext" runat="server" Text=">>" CommandName="Next" OnCommand="GetPageIndex"
                            ToolTip="Next" />
                    </td>
                    <td>
                        <asp:Button ID="btnLast" runat="server" Text=">|" CommandName="Last" OnCommand="GetPageIndex"
                            ToolTip="Last" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalRecords" runat="server" CssClass="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pmessage"
            CollapseControlID="pHeader" ExpandControlID="pHeader" Collapsed="false" ImageControlID="ImgHiddenmsg"
            CollapsedSize="0" CollapsedImage="~/images/collapse.jpg" ExpandedImage="~/images/expand.jpg"
            SuppressPostBack="true" CollapsedText="Message Queue" ExpandedText="Message Queue"
            TextLabelID="lblMessageHdr">
        </cc1:CollapsiblePanelExtender>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        <asp:AsyncPostBackTrigger ControlID="btnReset" />
        <asp:PostBackTrigger ControlID="UpdateTimer" />
    </Triggers>
</asp:UpdatePanel>