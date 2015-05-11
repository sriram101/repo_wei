var mLabelValueMTV = null;
var mMessagesMTV = null;
var mstrUserType = null;
var mRadGrid1MTV = null;
var marrSelectedMessages;
var ClientID = "";

var objUserType = { "Reviewer": "Reviewer", "Approver": "Approver" }

//Sys.Application.add_init(application_init);

//function application_init() {
//    var prm = Sys.WebForms.PageRequestManager.getInstance();
//    prm.add_endRequest(prm_endRequest);
//}

//function prm_endRequest() {
////    if (null != mMessagesMTV && null != mRadGrid1MTV) {
////        mRadGrid1MTV.rebind();
////    }
//}

function pageLoad() {
    //$find("FromDatePicker").get_dateInput().focus();

    if (document.getElementById("hfUserType").value == "Reviewer") {
        mstrUserType = objUserType.Reviewer;
    }

    if (document.getElementById("hfUserType").value == "Approver") {
        mstrUserType = objUserType.Approver;
    }

    ClientID = document.getElementById("strPanelClientID").value;
    //prm_endRequest();
}

function ListButton_Click() {

    if (null != mMessagesMTV) {

        var FromDate;
        var ToDate;
        var Status;

        if (null == $find("FromDatePicker").get_selectedDate()) {
            alert(document.getElementById("MSG_FromDateNotEmpty").value);
            $find("FromDatePicker").get_dateInput().focus();
            return;
        }

        //if (null == $find("ToDatePicker").get_selectedDate()) {
        if (null == $find(ClientID).get_selectedDate()) {
            alert(document.getElementById("MSG_ToDateNotEmpty").value);
            //$find("ToDatePicker").get_dateInput().focus();
            $find(ClientID).get_dateInput().focus();
            return;
        }

        if (0 == $find("rcbStatus").get_selectedIndex()) {
            alert(document.getElementById("MSG_StatusNotEmpty").value);
            $find("rcbStatus").get_inputDomElement().focus();
            return;
        }

        FromDate = $find("FromDatePicker").get_selectedDate().format("MMM dd, yyyy");
        //ToDate = $find("ToDatePicker").get_selectedDate().format("MMM dd, yyyy");
        ToDate = $find(ClientID).get_selectedDate().format("MMM dd, yyyy");
        Status = $find("rcbStatus").get_text();

        mMessagesMTV.rebind();

        document.getElementById("DisplayMessagesLabel").innerHTML = (document.getElementById("hfDisplayMessagesLabel").value).replace("%1", FromDate).replace("%2", ToDate).replace("%3", Status).replace("%4", "OFF");
    }
}

function MessagesGrid_OnGridCreated(sender, eventArgs) {
    mMessagesMTV = sender.get_masterTableView();
//    if (null != mMessagesMTV.get_selectedItems()) {
//        if (null != mMessagesMTV && null != mRadGrid1MTV) {
//            mRadGrid1MTV.rebind();
//        }
//    }
}

function LabelValueGrid_OnGridCreated(sender, eventArgs) {
    mLabelValueMTV = sender.get_masterTableView();

    if (mstrUserType == objUserType.Reviewer) {
        if (null != mLabelValueMTV) {
            //if (mLabelValueMTV.get_dataItems().length > 0) {
            for (i = 0; i < mLabelValueMTV.get_dataItems().length; i++) {
                var row = mLabelValueMTV.get_dataItems()[i];
                var Chk = row.findElement("ApprovedCheckBox");
                Chk.disabled = true;
            }
        }
    }
}

function RadGrid1_OnGridCreated(sender, eventArgs) {
    mRadGrid1MTV = sender.get_masterTableView();

    if (mstrUserType == objUserType.Reviewer) {
        if (null != mRadGrid1MTV) {
            //if (mRadGrid1MTV.get_dataItems().length > 0) {
            for (i = 0; i < mRadGrid1MTV.get_dataItems().length; i++) {
                var row = mRadGrid1MTV.get_dataItems()[i];
                var Chk = row.findElement("ApprovedCheckBox");
                Chk.disabled = true;
            }
        }
    }
}

function RefreshMessagesButton_Click() {
    if (null != mMessagesMTV) { mMessagesMTV.rebind(); }
}

function MessagesGrid_OnRowSelected(sender, eventArgs) {
    document.getElementById("hfRequestsID").value = eventArgs.getDataKeyValue("id");

    //if (null != mLabelValueMTV) { mLabelValueMTV.rebind(); }
    if (null != mRadGrid1MTV) { mRadGrid1MTV.rebind(); }

    if ($find(document.getElementById("strPanelClientID").value)) {
        $find(document.getElementById("strPanelClientID").value).ajaxRequest(document.getElementById("hfRequestsID").value);
    }
}



function OriginalMessageGrid_OnRowSelected() {
//    $find("hfSelectedLabelValueId")
//    document.getElementById("hfSelectedLabelValueId").value = "1234";
//    if (null != mLabelValueMTV) {
//        mLabelValueMTV.rebind();
//    }
}

function UpdateButton_Click(sender, eventArgs) {

}

function ReleaseButton_Click(sender, eventArgs) {
    if (null != marrSelectedMessages && marrSelectedMessages.length > 0) {
        alert("Please write codes here for moving messages to OFAC queue.");
        //TODO: Sent the array marrSelectedMessages to OFAC release.
    }
    else {
        alert("Please select a record!");
    }
}

function NewTransTextBox_OnValueChanged(sender, eventArgs) {
    var row = sender.get_element().parentNode.parentNode.parentNode;
    row.cells[6].all[0].checked = true;
//    var row = sender.get_element().parentNode.parentNode.parentNode;

//    if (mstrUserType == objUserType.Reviewer) {
//        row.cells[4].all[0].checked = true;
//    }

//    if (mstrUserType == objUserType.Approver) {
//        row.cells[4].all[0].checked = true;
//        row.cells[5].all[0].checked = true;
//    }
}

function ReviewedCheckBox_onClick(chkMark) {
    var row = chkMark.parentNode.parentNode;
    row.cells[6].all[0].checked = true;

//    if (!chkMark.checked) {        
//        row.cells[3].all[1].value = row.cells[7].innerText;
//    }
}

function ApprovedCheckBox_onClick(chkMark) {
    var row = chkMark.parentNode.parentNode;
    row.cells[6].all[0].checked = true;
}

function MessagesCheckBox_onClick(chkBox) {
    if (null == marrSelectedMessages) {
        marrSelectedMessages = new Array();
    }

    var intId = parseInt(chkBox.parentNode.parentNode.cells[1].innerText, 10);
    if (chkBox.checked) {
        marrSelectedMessages.push(intId);
    }
    else {
        var index = marrSelectedMessages.indexOf(intId);
        marrSelectedMessages.splice(index, 1);
    }

    if (null != marrSelectedMessages && marrSelectedMessages.length > 0) {
        $find("ReleaseButton").set_enabled(true);
    }
    else {
        $find("ReleaseButton").set_enabled(false);
    }
}

function FromDatePicker_OnDateSelected(sender, eventArgs) {
    if (eventArgs.get_newDate() != null) {

        //if (null == $find("ToDatePicker").get_selectedDate()) {        
        if (null == $find(ClientID).get_selectedDate()) {
            if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) > (new Date(new Date().format("MMM dd, yyyy")))) {
                alert(document.getElementById("MSG_DateCannotLaterThenToday").value);
                sender.set_selectedDate(new Date());
            }
        }
        else {
            //if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) > new Date($find("ToDatePicker").get_selectedDate().format("MMM dd, yyyy"))) {
            if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) > new Date($find(ClientID).get_selectedDate().format("MMM dd, yyyy"))) {
                alert(document.getElementById("MSG_DateCannotLaterThenToDate").value);
                //sender.set_selectedDate($find("ToDatePicker").get_selectedDate());
                sender.set_selectedDate($find(ClientID).get_selectedDate());
            }
            else
                if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) > (new Date(new Date().format("MMM dd, yyyy")))) {
                    alert(document.getElementById("MSG_DateCannotLaterThenToday").value);
                    sender.set_selectedDate(new Date());
                }
        }
    }
}

function ToDatePicker_OnDateSelected(sender, eventArgs) {
    if (eventArgs.get_newDate() != null) {

        if (null == $find("FromDatePicker").get_selectedDate()) {
            if (new Date( eventArgs.get_newDate().format("MMM dd, yyyy")) > new Date(new Date().format("MMM dd, yyyy"))) {
                alert(document.getElementById("MSG_DateCannotLaterThenToday").value);
                sender.set_selectedDate(new Date());
            }
        }
        else {
            if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) < new Date($find("FromDatePicker").get_selectedDate().format("MMM dd, yyyy"))) {
                alert(document.getElementById("MSG_DateCannotBeforeThenFromDate").value);
                sender.set_selectedDate($find("FromDatePicker").get_selectedDate());
            }
            else if (new Date(eventArgs.get_newDate().format("MMM dd, yyyy")) > new Date(new Date().format("MMM dd, yyyy"))) {
                alert(document.getElementById("MSG_DateCannotLaterThenToday").value);
                sender.set_selectedDate(new Date());
            }
        }
    }
}

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (id /*, from*/) {
        var len = this.length;
        //var index = from;
        var index = 0;

        for (; index < len; index++) {
            if (index in this && this[index].ARInvoiceID === id)
                return index;
        }
        return -1;
    };

    function onFailed(Error) {

    }
}