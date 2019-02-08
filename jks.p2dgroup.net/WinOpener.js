
function OpenWindow(sURL, title, w, h) {
    var winl = (screen.width - w) / 2;
    var wint = (screen.height - h) / 2;
    winprops = 'height=' + h + ',width=' + w + ',top=' + wint + ',left=' + winl + 'resizable=0'
    window.open(sURL + "&" + title + "", "", winprops)
} 
