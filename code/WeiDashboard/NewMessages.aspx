<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewMessages.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.NewMessages" %>
<%@ Register Src="~/UserControls/ucMessageViewer.ascx" TagName="ucMessageViewer"
    TagPrefix="uc1" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="upMessages">
        <ContentTemplate>
            <cc1:TabContainer ID="uiTabContainer" runat="server" ActiveTabIndex="0">
                <cc1:TabPanel runat="server" ID="tbGrid" TabIndex="0" HeaderText="Messages">
                    <ContentTemplate>
                        <uc1:ucMessageViewer ID="ucMessageViewer" runat="server" />
                    </ContentTemplate>
                </cc1:TabPanel>
                
                
            </cc1:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
