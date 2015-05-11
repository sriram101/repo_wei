<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Messages.aspx.cs"  
    Inherits="Telavance.AdvantageSuite.Wei.WeiDashboard.Messages" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

           
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">  

   .highlight  

    {  
         background-Color:Yellow;  
    } 
    /*SMU: Jan 28, 2013*/
    
        .popUpStyle
    {
        font: normal 11px auto "Trebuchet MS", Verdana;    
        background-color: Gray;
        color: #4f6b72;  
        padding:6px;        
        opacity: 0.8;
        filter: alpha(opacity=80);        
    }
    
    .drag
    {
         background-color:Blue;
         cursor: move;
         border-width: 0px;
         height:22px;
    }
    
        .Background 

    {
        background-color:Gray;
        filter:alpha(opacity=40);
        opacity:0.4;
    }    
    
    .modalPopupEx
    {
        position:static;
        top:10%;
        left:10px;
        width:1000px;
        height:535px;
        text-align:center;
        background-color:White;
        border:solid 2px black;
    }
    
    .modalPopup 
    {
        position:absolute;
        top:auto;
        left:auto;
        width:1000px;
        height:540px;
        text-align:center;
        background-color:White;
        border:solid 2px black;
    }
    
    .PanelStyle
    {    	
    	 border: 1px solid Gray;
    	 height:373px;
    }
    
    .hiddencol
    {
        display: none;
    }
    
    /*EU: Jan 28, 2013*/
    
 
    
</style>

 <script type="text/javascript">
     function checkDate(sender, args) {
         if (sender._selectedDate > new Date()) {
             alert("You cannot select a day later than today!");
             sender._selectedDate = new Date();
             // set the date back to the current date
             sender._textbox.set_Value(sender._selectedDate.format(sender._format))

         }

     }

     function pageLoad(sender, args) {
         if (!args.get_isPartialLoad()) {
             //  add our handler to the document's
             //  keydown event
             $addHandler(document, "keydown", onKeyDown);
         }
     }

     function onKeyDown(e) {
         if (e && e.keyCode == Sys.UI.Key.esc) {
             // if the key pressed is the escape key, dismiss the dialog
             Form1.submit(); 
             $find('ModalPopupExtender1').hide();
         }
     }
      

    </script>
    
<br />

<asp:ScriptManager runat="server" ID="objScriptManager" EnablePartialRendering="true" AsyncPostBackTimeOut="36000">
</asp:ScriptManager>
    <script type="text/javascript" language="javascript">
//        var divElem = 'AlertDiv';
//        var messageElem = 'AlertMessage';
//        var bodyTag = 'bodytag';
//        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//        function EndRequestHandler(sender, e) {
//            if (e.get_error() != undefined) {
//                var errorMessage = e.get_error().message;
//                e.set_errorHandled(true);
//                alert(errorMessage)
//            }

//           }
    </script>

    <asp:Panel ID="Panel1" runat="server" CssClass="Panel">
        <table id="table1" runat="server" width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblSearchParameters" Text="Search Criteria:" runat="server" CssClass="LabelCaption"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFromDate" Text="From Date:" runat="server" CssClass="Label"></asp:Label>
                        &nbsp;&nbsp; &nbsp;
                        <asp:TextBox ID="txtFromDate" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;
                        <asp:Image ID="Image1" runat="server" ImageUrl="images/Calendar.png" ImageAlign="Middle"/>&nbsp
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server"  EnableViewState="false" TargetControlID="txtFromDate" 
                            OnClientDateSelectionChanged="checkDate" PopupPosition="Right" PopupButtonID="Image1">
                    </asp:CalendarExtender>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblSearchText" runat="server" CssClass="Label" 
                        Text="Message Text:"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                        <asp:TextBox ID="txtSearchText" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessageStatus" runat="server" CssClass="Label" 
                        Text="Status:"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlMessageStatus" runat="server" CssClass="DropDownList" 
                        Width="200px" >
                    </asp:DropDownList>
                
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblToDate" Text="To Date:" runat="server" CssClass="Label"></asp:Label>
                        &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                        <asp:TextBox ID="txtToDate" runat="server" Width="200px" CssClass="TextBox" ></asp:TextBox>&nbsp;&nbsp;
                        <asp:Image ID="Image2" runat="server" ImageUrl="images/Calendar.png" ImageAlign="Middle" />&nbsp
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" 
                                OnClientDateSelectionChanged="checkDate" PopupPosition="Right" PopupButtonID="Image2">
                    </asp:CalendarExtender>
                    &nbsp;&nbsp;   
                    <asp:CheckBox ID="chkShowCTC" runat="server" Text="Messages with CTC" checked="true" CssClass="Checkbox" OnCheckedChanged="chkShowCTC_CheckChanged"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="chkShowErrors" runat="server" Text="Messages with Errors" checked="false" CssClass="Checkbox" OnCheckedChanged="chkShowErrors_CheckChanged"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSearch" runat="server" CssClass="Button" Text="List" 
                        onclick="btnSearch_Click"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnRefresh" runat="server" CssClass="Button" Text="Refresh Messages" 
                        onclick="btnRefresh_Click"  />
                </td>  
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMessage" runat="server" CssClass="LabelValue"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="Panel2" runat="server" CssClass="Panel">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID ="lblInfo" runat="server" CssClass="LabelItalics" Text = "" ></asp:Label> 
                        </td>
                        <td align="right">
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button id="btnProcessError" text="Process Error Messages" runat="server" Enabled="false" OnClick="btnProcessError_Click"/>
                        </td>
                
                    </tr>
                </table>
            </ContentTemplate>
        
        </asp:UpdatePanel>
    </asp:Panel>
    <br /> 
    <asp:Timer ID="Timer1"  OnTick="Timer1_Tick" runat="server"></asp:Timer>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="ParentPanel">
         <Triggers>
              <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"/>
         </Triggers>
        <ContentTemplate>
            <asp:Panel ID= "grdPanel" CssClass = "Panel" runat="server">
                <asp:GridView ID="grdShowMessages" AutoGenerateColumns="False" GridLines="None" 
                    HorizontalAlign="Center" AllowSorting="true"
                    EmptyDataRowStyle-HorizontalAlign="Center" 
                    EmptyDataRowStyle-Font-Bold="true" ShowFooter="true"
                    EmptyDataRowStyle-BackColor="#d1dbe0" Width="100%" 
                    CssClass="GridViewStyle" Visible="true"  
                    OnRowCommand="grdShowMessages_RowCommand"  
                    OnRowCreated="grdShowMessages_RowCreated"  runat="server" 
                    OnSorting="grdShowMessages_Sorting" 
                    ShowHeaderWhenEmpty="true" >
                
                    <HeaderStyle CssClass="HeaderStyle" />
                    <PagerStyle CssClass="PagerStyle"/>
                    <AlternatingRowStyle  CssClass="AltRowStyle" />
                    <EditRowStyle CssClass="EditRowStyle" /> 
                    <RowStyle CssClass="RowStyle" /> 
               
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID= "chkSelectRecord" runat="server"  />                                        
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" ItemStyle-Wrap="false" />
                        <asp:BoundField HeaderText="Interface Name" DataField="InterfaceName" SortExpression="InterfaceName" ItemStyle-Wrap="false" />
                        <asp:BoundField HeaderText="Status" DataField="Description" SortExpression="Description"  ItemStyle-Wrap="false"/>
                        <asp:BoundField HeaderText="Has CTC" DataField="HasCTC" SortExpression="HasCTC"  ItemStyle-Wrap="false"/>
                        <asp:BoundField HeaderText="Error" DataField="IsErrors" Visible="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField HeaderText="OFAC Violation" DataField="OFACStatus" SortExpression="OFACStatus" ItemStyle-Wrap="false"/>
                        <asp:BoundField HeaderText="Create Date" DataField="CreatedDateTime" SortExpression="CreatedDateTime" ItemStyle-Wrap="false"/>
                        <asp:BoundField HeaderText="Modified Date" DataField="ModifiedDateTime" SortExpression="ModifiedDateTime" ItemStyle-Wrap="false"/>

                        <asp:TemplateField>
                           <ItemTemplate>
                               <asp:LinkButton ID="LnkButton"  Text="Show Translation" CommandName="ShowTranslations"
                                   runat="server" OnClick="lnkShowTranslations_Click"/>
                           </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="messagebody" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" HtmlEncode="false"  />
                        <asp:BoundField DataField="translatedmessage" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"  HtmlEncode="false" />
                        <asp:BoundField DataField="responsemessage" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"  HtmlEncode="false" />
                        <asp:BoundField DataField="hasCTCApproved" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                         <%--<asp:BoundField DataField="messagebody" />
                        <asp:BoundField DataField="translatedmessage" />
                        <asp:BoundField DataField="responsemessage" />--%>

                    </Columns>
                </asp:GridView>                
              </asp:Panel>
              <asp:Panel ID="Panel5" runat="server" CssClass="Panel">
                    <table>
                        <tr style="height: 10px;">
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Rows Per Page:" CssClass="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlRows_SelectedIndexChanged" CssClass="DropDownList1">
                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnFirst" runat="server" Text="First" CommandName="First" CssClass="Button"  OnCommand="GetPageIndex" ToolTip="First"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPrevious" runat="server" Text="Previous" CommandName="Previous" CssClass="Button" ToolTip="Previous"  OnCommand="GetPageIndex" />
                                   
                            </td>
                            <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID = "LblCurrPage" runat="server" Text="Page" CssClass="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlPage" CssClass="DropDownList1" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPage_SelectedIndexChanged"></asp:DropDownList>
                                    
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblOf" Text="of" runat="server" CssClass="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalPages" runat="server" CssClass="Label"></asp:Label>

                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnNext" runat="server" Text="Next" CommandName="Next" OnCommand="GetPageIndex" CssClass="Button" ToolTip="Next"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnLast" runat="server" Text="Last" CommandName="Last"  OnCommand="GetPageIndex" CssClass="Button" ToolTip="Last"/>
                            </td>
                            <td width="15%">
                            &nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTotalRecords" runat="server" CssClass="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                                             
                </asp:Panel>

           </ContentTemplate>   
        </asp:UpdatePanel>      
        
        
        <br />
        <br />


        <%--SMU: Jan 28, 2013--%>

        <asp:Button runat="server" ID="btnShowModalPopup" style="display:none"/>

        <asp:ModalPopupExtender runat="server" 
            ID="ModalPopupExtender1" 
            BehaviorID="ModalPopupExtender1"
            TargetControlID="btnShowModalPopup"
            PopupControlID="divPopup" 
            BackgroundCssClass="Background"
            PopupDragHandleControlID="panelDragHandle"
            />
        <br />

        <div class="modalPopup" runat="server" id="divPopup" style="display: none; padding:0px">

        <table width="100%" cellpadding="0px" cellspacing="0px">
           <tr>
                <td style="padding:2px 2px 0px 2px">
                    <asp:UpdatePanel ID="upPopUp" runat="server" >
                        <ContentTemplate>
                            <asp:Panel ID="pnlSummary" runat="server" BackColor="white" Height="50px" BorderColor="Gray" BorderWidth="1px" >                                
                                <asp:label ID="lblRequestID" runat="server" Text="Request ID :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblRequestValue" runat="server" CssClass="LabelValue"></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:label ID="lblInterface" runat="server" Text="Interface Name" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblInterfaceValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblDescription" runat="server" Text="Current Status :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblDescValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblCreateTime" runat="server" Text="Create Date :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblCreateTimeValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblModifiedTime" runat="server" Text="Modified Date :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblModifiedTimeValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblNote" runat="server" Text="Note :" CssClass="Label"></asp:label> &nbsp;&nbsp;
                                <asp:label ID="lblNoteValue" runat="server" CssClass="LabelValue"></asp:label> &nbsp;&nbsp;
          
                                <br />
	                            <br />
                                <br />
                                <asp:TabContainer ID="tbContainer" runat="server" OnActiveTabChanged="tbContainer_TabChanged" ActiveTabIndex="0" AutoPostBack="true">
                                    <asp:TabPanel ID="tbTranslations" runat="server" Height="400px">
                                        <HeaderTemplate>Chinese Telegraphic Codes Translations</HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel6" runat="server"  ScrollBars="Auto" CssClass="PanelStyle" > 
                                                    <asp:GridView ID="grdShowTranslations" AutoGenerateColumns="False" GridLines="None" ShowFooter="True" Width="100%" 
                                                        CssClass="GridViewStyle" runat="server" 
                                                        OnRowEditing="grdShowTranslations_RowEditing"
                                                        OnRowCancelingEdit="grdShowTranslations_RowCancelingEdit"
                                                        OnRowDataBound="grdShowTranslations_OnRowDataBound"
                                                        OnRowUpdating = "grdShowTranslations_RowUpdating" >
                
                                                        <EmptyDataRowStyle BackColor="#D1DBE0" Font-Bold="True" HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="HeaderStyle"  Font-Bold="False"/>
                                                        <PagerStyle CssClass="PagerStyle"/>
                                                        <AlternatingRowStyle  CssClass="AltRowStyle" />
                                                        <EditRowStyle CssClass="EditRowStyle" /> 
                                                        <RowStyle CssClass="RowStyle" /> 
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Telegraphic Codes">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblCTCCodes" runat="server" Text='<%# Eval("CTCCode") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Chinese Characters">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblChineseChar" runat="server" Text='<%# Eval("ChineseChar") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Old Translation">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblOldTrans" runat="server" Text='<%# Eval("OldTrans") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="New Translation">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblNewTrans" runat="server" Text='<%# Eval("NewTrans") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtNewTranslations" TextMode="MultiLine" runat="server" Visible="true" Text='<%# Eval("NewTrans") %>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reviewed">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblReviewed" runat="server" Text='<%# Eval("Reviewed") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkReviewMessage" runat="server" Visible="true" Checked='<%# Eval("Reviewed") %>'></asp:CheckBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reviewed By">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReviewedBy" runat="server" Text='<%# Eval("ReviewOper") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved" Visible="False">
                                                                <ItemTemplate >
                                                                    <asp:Label ID="lblApproved" runat="server" Text='<%# Eval("Approved") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:CheckBox ID="chkApproveMessage" runat="server" Visible="true"  Checked='<%# Eval("Approved") %>'></asp:CheckBox>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approved By" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovedBy" runat="server" Text='<%# Eval("ApprovedOper") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField >
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" ></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" ></asp:LinkButton>
                                                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                        </ContentTemplate>
                                    </asp:TabPanel>                        
                                    <asp:TabPanel ID="tbOriginalMessage" runat="server">
                                        <HeaderTemplate>Original Message</HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" CssClass="PanelStyle" > 
                                                <table>
                                                    <tr>
                                                        <td> <asp:Label ID="lblOriginalMessage" runat="server" CssClass="Label" Text="Original Message"> </asp:Label> </td>
                                                        <td> <asp:Label ID="lblTranslatedMessage" runat="server" CssClass="Label" Text="Translated Message"></asp:Label> </td>
                                                        <%--<td> <asp:Label ID="lblResponseMessage" runat="server" CssClass="Label" Text="Response Message"></asp:Label></td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtMessagebody" Height="342px" Width="468px" TextMode="MultiLine" Wrap="false" ReadOnly="true" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTranslatedmessage" Height="342px" Width="468px" TextMode="MultiLine" Wrap="false" ReadOnly="true" ></asp:TextBox>
                                                        </td>
                                                        <%--<td>
                                                            <asp:TextBox runat="server" ID="txtResponsemessage" Height="342px" Width="308px" TextMode="MultiLine" Wrap="false" ReadOnly="true" ></asp:TextBox>
                                                        </td>--%>
                                                    </tr>
                                                </table>    
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbAudit" runat="server">
                                        <HeaderTemplate>Audit Details</HeaderTemplate>
                                        <ContentTemplate>
                                             <%--<asp:UpdatePanel runat="server" ID="ChildControl2" UpdateMode="Conditional"> 
                                                <ContentTemplate> --%>
                                            <asp:Panel ID="pnlAudit" runat="server" ScrollBars="Auto" CssClass="PanelStyle"> <%--CssClass="Panel"--%>
                                                <asp:GridView ID="grdShowAudit" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%" CssClass="GridViewStyle" Visible="true" >
                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                    <PagerStyle CssClass="PagerStyle"/>
                                                    <AlternatingRowStyle  CssClass="AltRowStyle" />
                                                    <EditRowStyle CssClass="EditRowStyle" /> 
                                                    <RowStyle CssClass="RowStyle" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate><%# Eval("ID").ToString().Trim() %></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Status" HeaderStyle-HorizontalAlign="Left" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate><%# Server.HtmlEncode(Eval("description").ToString().Trim())%></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Level" HeaderStyle-HorizontalAlign="Left" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate><%# Server.HtmlEncode(Eval("Level").ToString().Trim())%></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Create Date" HeaderStyle-HorizontalAlign="Left" >
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <ItemTemplate><%# Server.HtmlEncode(Eval("CreatedDateTime").ToString().Trim())%></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Audit Message" HeaderStyle-HorizontalAlign="Left" >
                                                            <ItemStyle HorizontalAlign="Left" /><ItemTemplate>
                                                            <%-- <%# Eval("Message").ToString().Replace("\r\n", "<br/>").Trim()%> --%>
                                                                <%# Server.HtmlEncode(Eval("Message").ToString().Replace(";", "<br/>").Replace("\r", "<br/>").Trim())%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                         <%--</ContentTemplate>
                                        </asp:UpdatePanel> --%>
                                        </ContentTemplate>
                                    </asp:TabPanel>                        
                                </asp:TabContainer>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="panEmpty" Height="410px" ></asp:Panel>
                                <table width="100%" style="padding-top:12px" >
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnFirstRequest" runat="server" Text="First" UseSubmitBehavior="false" CommandName="First" CssClass="Button"  ToolTip="First" OnCommand="NavigateMessage"/>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnPreviousRequest" runat="server" Text="Previous" UseSubmitBehavior="false" CommandName="Previous" CssClass="Button" ToolTip="Previous"  OnCommand="NavigateMessage" />
                                   
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnNextRequest" runat="server" Text="Next" CommandName="Next" CssClass="Button" ToolTip="Next" OnCommand="NavigateMessage"/>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnLastRequest" runat="server" Text="Last" CommandName="Last"  CssClass="Button" ToolTip="Last" OnCommand="NavigateMessage"/>
                                        </td>
                                        <td>
                                            <asp:Label id="lblMsgTranslations" runat="server" CssClass="Label"  Text="" ></asp:Label>
                                        </td>
                                        <td style="float:right">
                                            <asp:Button id="btnProcessReview" text="Release Message for OFAC Check" runat="server" CssClass="Button" OnClick="btnProcessReview_Click"/>
                                        </td>
                                    <%--<td style="float:right" >
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="Button" />
                                    </td>--%>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                 
                <td style="float:right; padding-right:5px">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="Button" />
                </td>
            </tr>
        </table>

        </div>

        
        
</asp:Content>

