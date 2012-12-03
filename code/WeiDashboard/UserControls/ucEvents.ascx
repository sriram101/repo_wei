<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEvents.ascx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.UserControls.ucEvents" %>
<asp:UpdatePanel ID="eventPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pHeader" runat="server">
            <asp:Label ID="lblMessageHdr" runat="server" CssClass="LabelCaption"></asp:Label>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblAction" runat="server" CssClass="LabelCaption"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pmessage" runat="server">
            <br />
            <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                Width="100%" EmptyDataText="No matching records found." HorizontalAlign="Center"
                AllowSorting="True" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-Font-Bold="true"
                EmptyDataRowStyle-BackColor="#d1dbe0" CssClass="GridViewStyle"
                OnRowCreated="grdView_RowCreated" 
                OnSorting="grdView_Sorting" >
                <EmptyDataRowStyle BackColor="#D1DBE0" Font-Bold="True" 
                    HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" />
                <PagerStyle CssClass="PagerStyle" />
                <AlternatingRowStyle CssClass="AltRowStyle" />
                <EditRowStyle CssClass="EditRowStyle" />
                <RowStyle CssClass="RowStyle" />
                <Columns>
                    <asp:BoundField DataField="LogDate" HeaderText="Event Date" SortExpression="LogDate" />
                    <%--<asp:BoundField DataField="Details" HeaderText="Details" SortExpression="Details" />--%>
                    <asp:TemplateField HeaderText="Details" SortExpression="Details">
                        <ItemTemplate>
                            <asp:Label ID="lblDetails" runat="server" Text='<%# Eval("Details").ToString()+" ..." %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Objectid1" HeaderText="Object ID" SortExpression="Objectid1" />--%>
                    <asp:TemplateField HeaderText="Event Type" SortExpression="EventType">
                        <ItemTemplate>
                            <asp:Label ID="lblEventType" runat="server" Text='<%# Bind("EventType.Code") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Oper" HeaderText="User" SortExpression="Oper" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
       
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
    </Triggers>
</asp:UpdatePanel>
