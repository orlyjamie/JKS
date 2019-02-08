function GetCookie(sName) {
    var aCookie = document.cookie.split("; ");
    for (var i = 0; i < aCookie.length; i++) {
        var aCrumb = aCookie[i].split("=");
        if (sName == aCrumb[0])
            return unescape(aCrumb[1]);
    }
    return null;
}
function SetCookie(sName, sValue) {
    var date = new Date()
    date.setFullYear(date.getFullYear() + 1);
    document.cookie = sName + "=" + escape(sValue) + "; expires=" + date.toUTCString();
}

function DelCookie(sName) {
    document.cookie = sName + "=" + escape(sValue) + "; expires=Fri, 31 Dec 1999 23:59:59 GMT;";
}

function getRadioValue(radioName) {
    var collection;
    collection = document.all[radioName];
    for (var i = 0; i < collection.length; i++) {
        if (collection[i].checked)
            return (collection[i].value);
    }
    return "0";
}
function setRadioValue(radioName, val) {
    var collection;
    collection = document.all[radioName];
    for (var i = 0; i < collection.length; i++) {
        if (collection[i].value == val)
            collection[i].checked = true;
    }
}
function centreWindow() {
    var sh = parseInt(window.screen.availHeight);
    var sw = parseInt(window.screen.availWidth);
    var dh = 0;
    var dw = 0;
    window.alert("centreWindow \navailHeight = " + sh + "\navailWidth = " + sw + "\ndlg Height =" + dh + "\ndlg Width = " + dw);


}
function centreDialog(dlg) {
    dlg.dialogTop = Math.round((parseInt(window.screen.availHeight) - parseInt(dlg.dialogHeight)) / 2);
    dlg.dialogLeft = Math.round((parseInt(window.screen.availWidth) - parseInt(dialogWidth)) / 2);
}


