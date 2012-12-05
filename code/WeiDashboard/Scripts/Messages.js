var mLabelValueMTV = null;
var mMessagesMTV = null;

function pageLoad() {
    $find("FromDatePicker").get_dateInput().focus();
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

function RefreshMessagesButton_Click() {
    if (null != mMessagesMTV) {
        mMessagesMTV.rebind();
    }
}

function MessagesGrid_OnGridCreated(sender, eventArgs) {
    mMessagesMTV = sender.get_masterTableView();
}

function LabelValueGrid_OnGridCreated(sender, eventArgs) {
    mLabelValueMTV = sender.get_masterTableView();
}

function ChildGrid1_OnRowSelected() {
    $find("hfSelectedLabelValueId")
    document.getElementById("hfSelectedLabelValueId").value = "1234";
    if (null != mLabelValueMTV) {
        mLabelValueMTV.rebind();
    }
}