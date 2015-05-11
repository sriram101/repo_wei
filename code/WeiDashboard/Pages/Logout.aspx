<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<script language="javascript" type="text/javascript">

    function pageLoad() {
    }

    function SupportLinkButton_OnClientClick() {
    }

    function AboutLinkButton_OnClientClick() {
    }

        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div style="text-align:center">              
        <asp:Label ID="lblsignout" runat="server" Text="Label" CssClass="LabelCaption"></asp:Label>&nbsp;
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Size="Small" 
                ForeColor="#0033CC">[HyperLink1]</asp:HyperLink>
    </div>


</asp:Content>
