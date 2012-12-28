<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/WEIDashboard.Master" AutoEventWireup="true" CodeBehind="CustomErrorPage.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Pages.CustomErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<br />
<br />
<asp:Label ID="lblError" runat="server" CssClass="LabelCaption"></asp:Label>
<br />
<br />
<asp:Label ID="lblBack" runat="server" CssClass="Label">Click on the following button to go to home page:</asp:Label>
<br />
<br />
<asp:button ID="btnBack" runat="server" CssClass="Button" Text="Back" 
        onclick="btnBack_Click" />

</asp:Content>
