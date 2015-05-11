<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard._About" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>About Telavance.</title>
    <link href="~/Styles/Style.css" rel="stylesheet" type="text/css" />
          
     <style type="text/css">
         .style1
         {
             width: 214px;
         }
         .style2
         {
             width: 214px;
             height: 74px;
         }
         .style3
         {
             height: 74px;
         }
     </style>
        <script language="javascript" type="text/javascript">
            function popup_close() {
             
                window.close();
                return false;
            }


        </script>
     </head>
<body>
    <form id="form1" runat="server">
       
         <asp:Panel ID="abtPanel" runat="server">
         <div>        
            <table id="table1" runat="server">
            <tr>
            <td colspan="2">
            <img alt="Telavance" src="Images/Telavance.JPEG" 
                        style="height: 35px; width: 239px" />
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="lblVersion" text="Version:" CssClass="Label" runat="server"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblVersionValue" runat="server" CssClass="LabelValue"></asp:Label>&nbsp;&nbsp;
            </td>
            </tr>
            <tr>
            <td class="style1">
            <asp:Label ID="lblCopyRight" text="Copyright:" CssClass="Label" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblCopyRightValue" runat="server" CssClass="LabelValue">(C) 2011</asp:Label>
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="lblCompanyName" text="Company:" CssClass="Label" runat="server"></asp:Label>&nbsp;&nbsp;           
                &nbsp;
                <asp:Label ID="lblName" runat="server" CssClass="LabelValue">Telavance, Inc.</asp:Label>
                </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="Label1" text="Contact:" CssClass="Label" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;<blockquote id="Blockquote1" 
                    runat="server" class="BlockQuote"> 517, RT 1 South Ste 5400<br /> Iselin, NJ 08830<br /> 
                    Ph: 732-744-0066<br /> Website: <a href="http://www.telavance.com" runat="server" target="_blank">www.telavance.com</a><br />
                </blockquote>
                &nbsp;</td>
            </tr>
            

            <tr>
            <td>
            <asp:Label ID="lblWarning" text="Warning:" CssClass="Label" runat="server"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="lblValue" text="Warning:" CssClass="LabelValue" runat="server">Usage of this program is subject to the terms of the Telavance Standard License Agreement.
                <br />
                Any unauthorized use is subject to civil and criminal penalties.</asp:Label>
            </td>
            </tr>
            <tr>
            <td>
            &nbsp;
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label id="lblText" CssClass="LabelItalics" runat="server" Text=""></asp:Label>
            <asp:Label id="lblTextValue" CssClass="LabelValue" runat="server"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>
            &nbsp;
            </td>
            </tr>
            <tr>
            <td align="center">
            <asp:Button ID="btnClose" CssClass="Button" Text="Close" runat="server" />
            </td>
            </tr>
            
            </table>
         </div>    
         </asp:Panel>    
            
    
    </form>
</body>
</html>

