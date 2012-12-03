<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Messages1.aspx.cs" Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Messages1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

           
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">  

   .highlight  

    {  

         background-Color:Yellow;  

    }  
    
 
    
</style>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"  EnablePartialRendering="false"/>
     
     <br />
    
    <div>
            <asp:Timer ID="Timer1"  OnTick="Timer1_Tick" runat="server"></asp:Timer>
    </div>
    <asp:Panel ID="Panel1" runat="server"  CssClass="Panel">
  <script type="text/javascript">
      function checkDate(sender, args) {
          if (sender._selectedDate > new Date()) {
              alert("You cannot select a day later than today!");
              sender._selectedDate = new Date();
              // set the date back to the current date
              sender._textbox.set_Value(sender._selectedDate.format(sender._format))

          }

      }
     
    </script>
   


        <table id="table1" runat="server">
        <tr>
            <td>
                <asp:Label ID="lblSearchParameters" Text="Search Criteria" runat="server" CssClass="LabelCaption"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
        <td>
       
       

           <asp:Label ID="lblFromDate" Text="From Create Date:" runat="server" CssClass="Label"></asp:Label>
                &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="txtFromDate" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;
                <asp:Image ID="Image1" runat="server" ImageUrl="images/Calendar.png" ImageAlign="Middle"/>&nbsp
            <asp:CalendarExtender ID="CalendarExtender1" runat="server"  EnableViewState="false" TargetControlID="txtFromDate" OnClientDateSelectionChanged="checkDate" PopupPosition="Right" PopupButtonID="Image1">
            </asp:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblToDate" Text="To Create Date:" runat="server" CssClass="Label"></asp:Label>
                &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="txtToDate" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;
                <asp:Image ID="Image2" runat="server" ImageUrl="images/Calendar.png" ImageAlign="Middle" />&nbsp
            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" OnClientDateSelectionChanged="checkDate" PopupPosition="Right" PopupButtonID="Image2">
            </asp:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <asp:Label ID="lblMessageStatus" runat="server" CssClass="Label" 
                            Text="Status:"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlMessageStatus" runat="server" CssClass="DropDownList" 
                            Width="200px" >
                        </asp:DropDownList>
                
            </td>
         
        </tr>
            <caption>
                &quot;
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSearchText" runat="server" CssClass="Label" 
                            Text="Message Text:"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                           <asp:TextBox ID="txtSearchText" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;   
                        <asp:CheckBox ID="chkShowCTC" runat="server" Text="Messages with CTC" checked="true" CssClass="Checkbox" OnCheckedChanged="chkShowCTC_CheckChanged"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:CheckBox ID="chkShowErrors" runat="server" Text="Messages with Errors" checked="false" CssClass="Checkbox" OnCheckedChanged="chkShowErrors_CheckChanged"/>
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Button ID="btnSearch" runat="server" CssClass="Button" Text="List" 
                            onclick="btnSearch_Click"  />
                       &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnRefresh" runat="server" CssClass="Button" Text="Refresh Messages" 
                            onclick="btnRefresh_Click"  />
                    </td>  
                    <td>
                    
                    </td>
                </tr>
                
            </caption>
        
        </table>
        <br />
    </asp:Panel>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage" runat="server" CssClass="LabelValue"></asp:Label>
    <br />
    <br />

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"/>
         </Triggers>
        <ContentTemplate>
            <table width="100%">
                <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID ="lblInfo" runat="server" CssClass="LabelItalics" Text = "" ></asp:Label> 
                </td>
                
                </tr>
            </table>
            <br /> 
            <asp:Panel ID="Panel2" runat="server" CssClass="Panel"  ScrollBars="Auto">
                <asp:GridView ID="grdShowMessages" AutoGenerateColumns="False" GridLines="None" 
                            AllowSorting="true"  Width="100%" 
                    CssClass="GridViewStyle" Visible="true"  
                    OnRowCommand="grdShowMessages_RowCommand"  
                    OnRowCreated="grdShowMessages_RowCreated"  runat="server" 
                    OnSorting="grdShowMessages_Sorting" >
                
                                 <HeaderStyle CssClass="HeaderStyle" />
                                <PagerStyle CssClass="PagerStyle"/>
                                <AlternatingRowStyle  CssClass="AltRowStyle" />
                                <EditRowStyle CssClass="EditRowStyle" /> 
                                <RowStyle CssClass="RowStyle" /> 
                            
                                <Columns>
                                   <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Message ID" DataField="Name" SortExpression="Name" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Interface Name" DataField="InterfaceName" SortExpression="InterfaceName" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Status" DataField="OFACStatus" SortExpression="OFACStatus"  ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Error" DataField="IsErrors" Visible="false" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="OFAC Violation" DataField="Description" SortExpression="Description" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Create Date" DataField="CreatedDateTime" SortExpression="CreatedDateTime" ItemStyle-Wrap="false"/>
                                    <asp:BoundField HeaderText="Modified Date" DataField="ModifiedDateTime" SortExpression="ModifiedDateTime" ItemStyle-Wrap="false"/>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnShowDetails" Text="Show Details" CssClass="Button" runat="server"  OnClick="btnShowDetails_Click" 
                                            CommandName="ClickMe"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                               
                                </Columns>
                                                    
                            
             </asp:GridView>
                <asp:UpdatePanel ID="upnlGrdPaging" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" >
                           <tr style="height:20px;">
                                <td width="20%">&nbsp;&nbsp;&nbsp;</td>
                                <td >
                                    <asp:Label ID="LblRows" runat="server" Text = "Rows Per Page:" CssClass="Label"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlRows_SelectedIndexChanged" CssClass="DropDownList1">
                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnFirst" runat="server" Text="First" CommandName="First" CssClass="GridPageFirstInactive"  OnCommand="GetPageIndex" ToolTip="First"/>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnPrevious" runat="server" Text="Previous" CommandName="Previous" CssClass="GridPagePreviousInactive" ToolTip="Previous"  OnCommand="GetPageIndex" />
                                    
                                </td>
                                <td>
                                        &nbsp;&nbsp;&nbsp;<asp:Label ID = "LblCurrPage" runat="server" Text="Page" CssClass="Label"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlPage" CssClass="DropDownList1" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPage_SelectedIndexChanged"></asp:DropDownList>
                                    
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblOf" Text="of" runat="server" CssClass="Label"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalPages" runat="server" CssClass="Label"></asp:Label>

                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnNext" runat="server" Text="Next" CommandName="Next" OnCommand="GetPageIndex" CssClass="GridPageNextInactive" ToolTip="Next"/>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnLast" runat="server" Text="Last" CommandName="Last"  OnCommand="GetPageIndex" CssClass="GridPageLastInactive" ToolTip="Last"/>
                                </td>
                                <td width="15%">
                                &nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalRecords" runat="server" CssClass="Label"></asp:Label>
                                </td>
                                            
                            </tr>
                        </table>
                                
                        </ContentTemplate>
                        </asp:UpdatePanel>
         </asp:Panel>
        <br />

        <div  id="divPopUp" class="popUpStyle" runat="server">
            <asp:Panel runat="Server" ID="panelDragHandle"  CssClass="drag">
                <asp:Button runat="server" ID="btnShowModalPopup" style="display:none"/>

                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowModalPopup" PopupControlID="divPopUp" 
                BackgroundCssClass="modalBackground" DropShadow="false"  />
            
                <br />
                <table width="100%">
                    <tr>
                    <td align="center">
                        <asp:Button ID="btnClose" runat="server"  Text="Close" CssClass="Button"   OnClick="btnClose_Click" />
                    </td>
                
                    </tr>
                
                </table>
                
                &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessageDetails" runat="server" CssClass="LabelCaption" Text="Message Details:"></asp:Label>

                <br />
                <br />
               <asp:Panel ID="pnlSummary" runat="server" CssClass="Panel"  BackColor="white" Width="90%" Height="50px">
                    <asp:label ID="lblRequestID" runat="server" Text="Request ID :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblRequestValue" runat="server" CssClass="LabelValue"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:label ID="lblMessageID" runat="server" Text="Message ID :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblMessageValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblInterface" runat="server" Text="Interface Name" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblInterfaceValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblDescription" runat="server" Text="Current Status :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblDescValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblCreateTime" runat="server" Text="Create Date :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblCreateTimeValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblModifiedTime" runat="server" Text="Modified Date :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                    <asp:label ID="lblModifiedTimeValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
          
                   
	            </asp:Panel>
                <br />
                <asp:Panel ID="Panel3" runat="server" CssClass="Panel" Width="90%" ScrollBars="Vertical">
	                <asp:GridView ID="grdShowDetails" runat="server" AutoGenerateColumns="False" AllowSorting="false" GridLines="None"
	                     CssClass="GridViewStyle" Visible="true" Width="100%" >
	                       
                            
	                        <HeaderStyle CssClass="HeaderStyle"  HorizontalAlign="Center"/>
	                                   <PagerStyle CssClass="PagerStyle" />
	                                   <AlternatingRowStyle  CssClass="AltRowStyle" />
	                                   <EditRowStyle CssClass="EditRowStyle" /> 
	                                   <RowStyle CssClass="RowStyle" HorizontalAlign="Left" /> 
                            <Columns>
                             <asp:TemplateField HeaderText="Original Message"  HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                               <%# HighlightText(_sSearchString, (string) Eval("messagebody").ToString()).Replace("\r\n", "<br/>").Trim() %>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Translated Message" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                               <%# HighlightText(_sSearchString, (string)Eval("translatedmessage").ToString()).Replace("\r\n", "<br/>").Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Response Message" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                               <%# HighlightText(_sSearchString, (string)Eval("responsemessage").ToString()).Replace("\r\n", "<br/>").Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                           
	                   </asp:GridView>                          
	            </asp:Panel>
           
             
                 &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="lblRecCount" runat="server" Text='' CssClass="Label"></asp:Label>
                    <br />   <br />
                    &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;<asp:Label ID="lblAuditMessage" runat="server" Text='Audit Details:' CssClass="LabelCaption"></asp:Label>
                <br />
                <br />
                <asp:Panel ID="Panel4" runat="server" CssClass="Panel" Width="90%" ScrollBars="Vertical">  
                    <asp:GridView ID="grdShowAudit" runat="server" AutoGenerateColumns="False" GridLines="None"
                                    Width="100%" 
                        CssClass="GridViewStyle" Visible="true" >
                
                         <HeaderStyle  HorizontalAlign="Center" CssClass="HeaderStyle" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <AlternatingRowStyle  CssClass="AltRowStyle" />
                                    <EditRowStyle CssClass="EditRowStyle" /> 
                                    <RowStyle CssClass="RowStyle" /> 
                        <Columns>
                             <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# Eval("ID").ToString().Trim() %>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Audit Status" HeaderStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# Eval("description").ToString().Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Level" HeaderStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# Eval("Level").ToString().Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Create Date" HeaderStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# Eval("CreatedDateTime").ToString().Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Audit Message" HeaderStyle-HorizontalAlign="Center" >
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                               <%# Eval("Message").ToString().Trim()%>
                            </ItemTemplate>
                            </asp:TemplateField>

                            </Columns>
                    </asp:GridView>                          
                </asp:Panel>
           </asp:Panel>
        </div>
      </ContentTemplate>

   </asp:UpdatePanel>     
        
        
        
</asp:Content>

