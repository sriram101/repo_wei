
function SupportLinkButton_OnClientClick() {
    var Page = "Support.aspx";
    var Settings = "title.text:'Support';titlebar:0;center:yes;resizable:no;dialogWidth:345px;dialogHeight:210px;status:no;scroll:no;";
    var objReturn = window.showModalDialog(Page, "", Settings);
}

function AboutLinkButton_OnClientClick() {
    var Page = "About.aspx";
    var Settings = "title.text:'About';titlebar:0;center:yes;resizable:no;dialogWidth:575px;dialogHeight:350px;status:no;scroll:no;";
    var objReturn = window.showModalDialog(Page, "", Settings);
}