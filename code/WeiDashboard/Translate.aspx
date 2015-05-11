<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="Translate.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Translate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head runat="server">
    <title>WEI Translate</title>
      <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel ID="AccelPanel" runat="server">
    <asp:Label ID="LblError" runat="server" CssClass="LabelValue" Visible="false"></asp:Label><br /><br />
    <asp:Label ID="lblCTC" runat="server" Text="CTC:" CssClass="Label"></asp:Label> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblCTCValue" runat="server" CssClass="LabelValue"></asp:Label><br /><br />
    <asp:Label ID="lblChinese" runat="server" Text="Chinese:" CssClass="Label"></asp:Label> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblChineseValue" runat="server" CssClass="LabelValue"></asp:Label><br /><br />
    <asp:Label ID="lblOldTrans" runat="server" Text="Old Translation:" CssClass="Label"></asp:Label> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblOldTransValue" runat="server" CssClass="LabelValue"></asp:Label><br /><br />
    <asp:Label ID="lblNewTrans" runat="server" Text="New Translation:" CssClass="Label"></asp:Label> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblNewTransValue" runat="server" CssClass="LabelValue"></asp:Label><br /><br />
    
    </asp:Panel>
    </div>
    </form>
</body>
</html>
