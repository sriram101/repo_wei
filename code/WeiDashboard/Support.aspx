<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Support.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Support" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact</title>
    <link href="~/Styles/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function popup_close() {

            window.close();
            return false;
        }


        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Panel ID="abtPanel" runat="server">
         <div>        
            <table id="table1" runat="server">
            <caption>
            <tr>
            <td colspan="2">
            <img alt="Telavance" src="Images/Telavance.JPEG" 
                        style="height: 35px; width: 239px" />
            </td>
            </tr>
            <tr>
            <td>
            &nbsp;
            </td>
            </tr>
            <tr>
            <td>
            &nbsp;
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="lblText" CssClass="LabelValue" runat="server" ></asp:Label>
            </td>
            </tr>
            <tr>
            <td>
            <br />
            <asp:Label ID="lblEmailAddKey" CssClass="Label" runat="server"></asp:Label>
            &nbsp;<a href="mailto:support@telavance.com" id="lnkEmailAddress" runat="server" class="HyperLink"></a>
            </td>
            </tr>
            <tr>
            <td>
            <br />
            <asp:Label ID="lblSupportTelKey" CssClass="Label" runat="server"></asp:Label>
            &nbsp;<asp:Label ID="lblSupportKeyValue" CssClass="LabelValue" runat="server"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>
                &nbsp;
            </td>
            </tr>
            <tr>
                 <td colspan="2" align="center">
                            <asp:Button ID="btnClose" runat="server" CssClass="Button" Text="Close" />
                        </td>
                    </tr>
                </caption>
            </table>
           </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
