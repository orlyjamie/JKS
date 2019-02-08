
var negSymbol = "-";
var groupSep = ",";
var decimalSep = ".";
var groupSize = 3;

function TestNum() {
    var x = new Number("£2,123.45");
    var y = new Number("GBP(1345865234)");
    window.alert("Number Test Results:\ny=" + y.text + " \nx=" + x.text + "\nx=" + x + "  y=" + y);
}

function Number(number) {
    //window.alert("Number(" + number + ")");
    var ok = true;
    this.number = number;
    this.prefix = "";
    this.suffix = "";

    switch (typeof (number)) {
        case "number":
            number = number.toString();
            break;
        case "string":
            break;
        default:
            ok = false;
    }

    if (ok) {
        var decimalFound = false;
        var negFound = false;
        var openingBracket = false;
        var clossingBracket = false;
        var num = "";
        var i = 0;
        while (ok && i < number.length) {
            if (number.charAt(i) >= '0' && number.charAt(i) <= '9') break;
            this.prefix += number.charAt(i);
            if (number.charAt(i) == '(')
                if (!openingBracket) openingBracket = true; else ok = false;
            else if (number.charAt(i) == negSymbol)
                if (!negFound) negFound = true; else ok = false;
            i++;
        }
        while (ok && i < number.length) {
            if (number.charAt(i) >= "0" && number.charAt(i) <= "9")
                num += number.charAt(i);
            else if (number.charAt(i) == decimalSep)
                if (!decimalFound) {
                    decimalFound = true;
                    num += number.charAt(i);
                }
                else
                    ok = false;
            else if (number.charAt(i) != groupSep) {
                break;
            }
            i++;
        }
        while (ok && i < number.length) {
            this.suffix += number.charAt(i);
            if (number.charAt(i) == ')')
                if (!clossingBracket)
                    if (openingBracket)
                        clossingBracket = true;
                    else
                        ok = false;
                else
                    ok = false;
            else if (number.charAt(i) == negSymbol)
                if (!negFound) negFound = true; else ok = false;
            i++;
        }
        if ((openingBracket && !clossingBracket) || (openingBracket && negFound))
            ok = false;
        //window.alert( "Past Suffix i = " + i   + "\nthis.prefix = '" + this.prefix + "'\nnum = '" + num + "'\nthis.suffix = '" + this.suffix + "'\nok = " + ok );
        if (ok) {
            this.text = this.prefix + formatValue(num) + this.suffix;
            if (negFound || openingBracket)
                num = "-" + num;
            this.value = parseFloat(num);
        }
        else {
            this.prefix = "";
            this.value = NaN;
            this.suffix = "";
            this.text = number;
        }
    }
}

function toFloat(number) {
    n = new Number(number);
    return (n.value);
}

function formatValue(number) {
    switch (typeof (number)) {
        case "number":
            number = number.toString();
            break;
        case "string":
            break;
        default:
            return (0);
    }
    if (number == "" || number == null)
        return ("");
    try {
        var x = Math.round(parseFloat(number) * 100);
        var neg = false;
        if (x < 0) {
            neg = true;
            x = Math.abs(n);
        }
        var n = x.toString();
        if (n.length < 3)
            n = "000".substring(0, (3 - n.length)) + n;
        var result = decimalSep + n.substr((n.length - 2), 2);

        var j = 0;
        for (var i = (n.length - 3); i >= 0; i--) {
            if (j >= groupSize) {
                result = groupSep + result;
                j = 0;
            }
            result = number.charAt(i) + result;
            j++;
        }
        if (neg)
            result = negSymbol + result;
        return (result);
    }
    catch (e) {
        window.alert("formatValue Catch!! " + e.toString());
        return (number);
    }
}
