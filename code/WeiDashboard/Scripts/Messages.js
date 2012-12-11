var mLabelValueMTV = null;
var mMessagesMTV = null;
var mstrUserType = null;
var mRadGrid1MTV = null;

var objUserType = { "Reviewer": "Reviewer", "Approver": "Approver" }

function pageLoad() {
    $find("FromDatePicker").get_dateInput().focus();

    if (document.getElementById("hfUserType").value == "Reviewer") {
        mstrUserType = objUserType.Reviewer;
    }

    if (document.getElementById("hfUserType").value == "Approver") {
        mstrUserType = objUserType.Approver;
    }
}

function ListButton_Click() {

    if (null != mMessagesMTV) {

        var FromDate;
        var ToDate;
        var Status;

        if (null == $find("FromDatePicker").get_selectedDate()) {
            alert("From date should not be empty.");
            $find("FromDatePicker").get_dateInput().focus();
            return;
        }

        if (null == $find("ToDatePicker").get_selectedDate()) {
            alert("To date should not be empty.");
            $find("ToDatePicker").get_dateInput().focus();
            return;
        }

        if (0 == $find("rcbStatus").get_selectedIndex()) {
            alert("Status should not be empty.");
            $find("rcbStatus").get_inputDomElement().focus();
            return;
        }

        FromDate = $find("FromDatePicker").get_selectedDate().format("MMM dd, yyyy");
        ToDate = $find("ToDatePicker").get_selectedDate().format("MMM dd, yyyy");
        Status = $find("rcbStatus").get_text();

        mMessagesMTV.rebind();

        document.getElementById("DisplayMessagesLabel").innerHTML = (document.getElementById("hfDisplayMessagesLabel").value).replace("%1", FromDate).replace("%2", ToDate).replace("%3", Status).replace("%4", "OFF");
    }
}

function MessagesGrid_OnGridCreated(sender, eventArgs) {
    mMessagesMTV = sender.get_masterTableView();
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

    //    if (null != mLabelValueMTV) { mLabelValueMTV.rebind(); }
    if (null != mRadGrid1MTV) { mRadGrid1MTV.rebind(); }
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

}

function NewTransTextBox_OnValueChanged(sender, eventArgs) {
    var row = sender.get_element().parentNode.parentNode.parentNode;

    if (mstrUserType == objUserType.Reviewer) {
        row.cells[4].all[0].checked = true;
    }

    if (mstrUserType == objUserType.Approver) {
        row.cells[4].all[0].checked = true;
        row.cells[5].all[0].checked = true;
    }
}

function ReviewedCheckBox_onClick(chkMark) {
    var row = chkMark.parentNode.parentNode;
    row.cells[6].all[0].checked = true;

    if (!chkMark.checked) {        
        row.cells[3].all[1].value = row.cells[7].innerText;
    }
}

function ApprovedCheckBox_onClick(chkMark) {
    var row = chkMark.parentNode.parentNode;
    row.cells[6].all[0].checked = true; 
}