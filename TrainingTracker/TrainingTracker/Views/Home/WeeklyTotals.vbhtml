﻿@ModelType TrainingTracker.WeeklyOverviewModel

@Code
    ViewData("Title") = "WeeklyTotals"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Weekly Totals</h2>
<br />
<h3>@Model.table.title</h3>
<br />
<span>End Date</span><br />
@Html.TextBoxFor(Function(x) x.endDate, New With {.readOnly = "readOnly", .class = "datepicker datefield", .id = "dateOfTraining", .Value = Model.endDate.ToShortDateString})

@Html.Partial("TableSorter", Model.table)

@If Model.displayFeedbackLink Then
    @<br />
    @<a href=# onclick="SendEmail()">Send Feeback For This Week</a>
End If


<script>
    $(function () { // will trigger when the document is ready
        $('.datepicker').datepicker(); //Initialise any date pickers
    });
    $("#dateOfTraining").change(function () {
        window.location.href = "/Home/WeeklyOverview?targetEmail=@Model.email" + "&endDate="+$("#dateOfTraining").val();
    });

    function SendEmail() {
        outputTable = createTable("@Model.plainTextTableInput");
        window.location.href = "mailto:@Model.email?subject=Training%20Feedback%20" + "@Model.endDate.ToShortDateString" + "&body=" + "The%20following%20is%20a%20table%20containing%20the%20training%20for%20the%20week%20commented%20on.%20To%20make%20it%20format%20correctly%20,%20change%20its%20font%20to%20Courier%20New.%0D%0A" + outputTable;
    }

    /*http://www.sensefulsolutions.com/2010/10/format-text-as-table.html is the owner of the following js. It has been modified to remove various choices providing one static style of table and to allow passing of the initial string.*/

    /* Format text as table */

    function createTable(inputString) {
        // set up the style
        var cTL, cTM, cTR;
        var cML, cMM, cMR;
        var cBL, cBM, cBR;
        var cH, cV;

        var headerStyle = "top";
        var autoFormat = true;
        var hasHeaders = headerStyle == "top";
        var spreadSheetStyle = headerStyle == "ssheet";
        var input = inputString;//add string for table here

        var rows = input.split(/[\r\n]+/);
        if (rows[rows.length - 1] == "") {
            // extraneous last row, so delete it
            rows.pop();
        }

        if (spreadSheetStyle) {
            hasHeaders = true;
            // add the row numbers
            for (var i = 0; i < rows.length; i++) {
                rows[i] = (i+1) + "\t" + rows[i];
            }
        }

        // calculate the max size of each column
        var colLengths = [];
        var isNumberCol = [];
        for (var i = 0; i < rows.length; i++) {
            var cols = rows[i].split(/\t/);
            for (var j = 0; j < cols.length; j++) {
                var data = cols[j];
                var isNewCol = colLengths[j] == undefined;
                if (isNewCol) {
                    isNumberCol[j] = true;
                }
                // keep track of which columns are numbers only
                if (autoFormat) {
                    if (hasHeaders && i == 0 && !spreadSheetStyle) {
                        ; // a header is allowed to not be a number (exclude spreadsheet because the header hasn't been added yet
                    } else if (isNumberCol[j] && !data.match(/^(\s*-?\d+\s*|\s*)$/)) {
                        isNumberCol[j] = false;
                    }
                }
                if (isNewCol || colLengths[j] < data.length) {
                    colLengths[j] = data.length;
                }
            }
        }

        if (spreadSheetStyle) {
            // now that we have the number of columns, add the letters
            var colCount = colLengths.length;
            var letterRow = " "; // initial column will have a space
            for (var i = 0; i < colCount; i++) {
                var asciiVal = (65 + i);
                if (90 < asciiVal) {
                    asciiVal = 90; // Z is the max column
                }
                letterRow += "\t" + String.fromCharCode(asciiVal);
            }
            rows.splice(0, 0, letterRow); // add as first row
        }

        var style = "0";
        switch (style) {
            case "0":
                // mysql
                cTL = "+";
                cTM = "+";
                cTR = "+";
                cML = "+";
                cMM = "+";
                cMR = "+";
                cBL = "+";
                cBM = "+";
                cBR = "+";
                cH = "-";
                cV = "|";
                break;
            case "1":
                // unicode
                cTL = "\u2554";
                cTM = "\u2566";
                cTR = "\u2557";
                cML = "\u2560";
                cMM = "\u256C";
                cMR = "\u2563";
                cBL = "\u255A";
                cBM = "\u2569";
                cBR = "\u255D";
                cH = "\u2550";
                cV = "\u2551";
                break;
            case "html":
                outputAsNormalTable(rows, hasHeaders, colLengths);
                return;
            default:
                break;
        }

        // output the text
        var output = "";
        for (var i = 0; i < rows.length; i++) {
            // output the top most row
            // Ex: +---+---+
            if (i == 0) {
                output += cTL;
                for (var j = 0; j < colLengths.length; j++) {
                    output += sfu.repeat(cH, colLengths[j] + 2);
                    if (j < colLengths.length - 1) {
                        output += cTM;
                    }
                    else output += cTR;
                }
                output += "%0D%0A";
            }

            // output the header separator row
            // Ex: +---+---+
            if (hasHeaders && i == 1) {
                output += cML;
                for (var j = 0; j < colLengths.length; j++) {
                    output += sfu.repeat(cH, colLengths[j] + 2);
                    if (j < colLengths.length - 1) {
                        output += cMM;
                    }
                    else {
                        output += cMR;
                    }
                }
                output += "%0D%0A";
            }

            // output the data
            output += cV;
            var cols = rows[i].split(/\t/);
            for (var j = 0; j < colLengths.length; j++) {
                var data = cols[j] || "";
                var align = "l";
                if (autoFormat) {
                    if (hasHeaders && i == 0) {
                        align = "c";
                    } else if (isNumberCol[j]) {
                        align = "r";
                    }
                }
                data = sfu.pad(data, colLengths[j], "%20", align);
                output += "%20" + data + "%20" + cV;
            }
            output += "%0D%0A";

            // output the bottom row
            // Ex: +---+---+
            if (i == rows.length - 1) {
                output += cBL;
                for (var j = 0; j < colLengths.length; j++) {
                    output += sfu.repeat(cH, colLengths[j] + 2);
                    if (j < colLengths.length - 1) output += cBM;
                    else output += cBR;
                }
            }
        }

        //$('#output').val(output);
        return output//actual value I want
        //$('#outputText').show();
        //$('#outputTbl').hide();
    }

        function outputAsNormalTable(rows, hasHeaders, colLengths) {
            var output = "";

            var $outputTable = $('<table>');
            for (var i = 0; i < rows.length; i++) {
                var cols = rows[i].split(/\t/);
                var tag = (hasHeaders && i == 0) ? "th" : "td";
                var $row = $('<tr>').appendTo($outputTable);
                for (var j = 0; j < colLengths.length; j++) {
                    var data = cols[j] || " ";
                    var $cell = $('<' + tag + '>').text(data);
                    $row.append($cell);
                }
            }
            var $outputDiv = $('#outputTbl');
            $outputDiv.empty();
            $outputDiv.append($outputTable);
            //add custom handling for output if going to use html table
            $('#output').val('<table>' + $outputTable.html() + '</table>');
            $('#outputText').show();
            $('#outputTbl').show();
        }

        var sfu = new function () {
            // public functions
            this.assert = assert;
            this.defValue = defValue;
            this.error = error;
            this.getAllRegexMatches = getAllRegexMatches;
            this.mdaCreate = mdaCreate;
            this.mdaGet = mdaGet;
            this.mdaIndexOf = mdaIndexOf;
            this.mdaSet = mdaSet;
            this.pad = pad;
            this.toBase64 = toBase64;
            this.trim = trim;
            this.repeat = repeat;
            this.zeroFill = zeroFill;

            // public namespaces
            this.clr = new function () {
                // public functions
                this.getVaryingBrightnesses = getVaryingBrightnesses;
                this.getVaryingHues = getVaryingHues;
                this.getVaryingSaturations = getVaryingSaturations;
                this.hsvToRgb = hsvToRgb;
                this.newHsv = newHsv;
                this.newRgb = newRgb;
                this.rgbToHex = rgbToHex;
                this.rgbToHsv = rgbToHsv;
                this.rgbToRgbStr = rgbToRgbStr;

                // function declarations
                function getVaryingBrightnesses(rgb, colorCount) {
                    // rgb: array[0..2] of values from 0 to 255 
                    var increment = 1 / colorCount;
                    var hsv = rgbToHsv(rgb);
                    hsv[2] = 0;
                    var result = [];
                    for (var i = 0; i < colorCount; i++) {
                        rgb = hsvToRgb(hsv);
                        result.push(rgb);

                        hsv[2] += increment;
                    }
                    return result;
                }
                function getVaryingHues(rgb, colorCount) {
                    // rgb: array[0..2] of values from 0 to 255 
                    var increment = 1 / colorCount;
                    var hsv = rgbToHsv(rgb);
                    var result = [];
                    for (var i = 0; i < colorCount; i++) {
                        rgb = hsvToRgb(hsv);
                        result.push(rgb);
                        hsv[0] += increment;
                        if (1 < hsv[0]) hsv[0] = hsv[0] - 1;
                    }
                    return result;
                }
                function getVaryingSaturations(rgb, colorCount) {
                    // rgb: array[0..2] of values from 0 to 255 
                    var increment = 1 / colorCount;
                    var hsv = rgbToHsv(rgb);
                    hsv[1] = 0;
                    var result = [];
                    for (var i = 0; i < colorCount; i++) {
                        rgb = hsvToRgb(hsv);
                        result.push(rgb);
                        hsv[1] += increment;
                    }
                    return result;
                }
                function hsvToRgb(hsv) {
                    /**
                     * Converts an HSV color value to RGB. Conversion formula
                     * adapted from http://en.wikipedia.org/wiki/HSV_color_space.
                     * Assumes h, s, and v are contained in the set [0, 1] and
                     * returns r, g, and b in the set [0, 255].
                     *
                     // hsv: array[0..2] of values from 0 to 1 
                     * param   Number  h       The hue
                     * param   Number  s       The saturation
                     * param   Number  v       The value
                     * return  Array           The RGB representation
                     */

                    var h = hsv[0];
                    var s = hsv[1];
                    var v = hsv[2];

                    var r, g, b;

                    var i = Math.floor(h * 6);
                    var f = h * 6 - i;
                    var p = v * (1 - s);
                    var q = v * (1 - f * s);
                    var t = v * (1 - (1 - f) * s);

                    switch (i % 6) {
                        case 0:
                            r = v, g = t, b = p;
                            break;
                        case 1:
                            r = q, g = v, b = p;
                            break;
                        case 2:
                            r = p, g = v, b = t;
                            break;
                        case 3:
                            r = p, g = q, b = v;
                            break;
                        case 4:
                            r = t, g = p, b = v;
                            break;
                        case 5:
                            r = v, g = p, b = q;
                            break;
                    }

                    return [Math.round(r * 255), Math.round(g * 255), Math.round(b * 255)];
                }
                function newHsv(h, s, v) {
                    return [h, s, v];
                }
                function newRgb(r, g, b) {
                    return [r, g, b];
                }
                function rgbToHex(rgb) {
                    // rgb: array[0..2] of values from 0 to 255 
                    return ('#' + zeroFill(rgb[0].toString(16), 2) + zeroFill(rgb[1].toString(16), 2) + zeroFill(rgb[2].toString(16), 2)).toUpperCase();
                }
                function rgbToHsv(rgb) {
                    /**
                     * Converts an RGB color value to HSV. Conversion formula
                     * adapted from http://en.wikipedia.org/wiki/HSV_color_space.
                     * Assumes r, g, and b are contained in the set [0, 255] and
                     * returns h, s, and v in the set [0, 1].
                     *
                     // rgb: array[0..2] of values from 0 to 255 
                     * param   Number  r       The red color value
                     * param   Number  g       The green color value
                     * param   Number  b       The blue color value
                     * return  Array           The HSV representation
                     */

                    var r = rgb[0];
                    var g = rgb[1];
                    var b = rgb[2];
                    r = r / 255, g = g / 255, b = b / 255;
                    var max = Math.max(r, g, b),
                    min = Math.min(r, g, b);
                    var h, s, v = max;

                    var d = max - min;
                    s = max == 0 ? 0 : d / max;

                    if (max == min) {
                        h = 0; // achromatic
                    } else {
                        switch (max) {
                            case r:
                                h = (g - b) / d + (g < b ? 6 : 0);
                                break;
                            case g:
                                h = (b - r) / d + 2;
                                break;
                            case b:
                                h = (r - g) / d + 4;
                                break;
                        }
                        h /= 6;
                    }

                    return [h, s, v];
                }
                function rgbToRgbStr(rgb) {
                    // rgb: array[0..2] of values from 0 to 255 
                    return rgb[0] + ', ' + rgb[1] + ', ' + rgb[2];
                }
            };
            this.jq = new function () {
                // public functions
                this.selectorsAreEqual = selectorsAreEqual;

                // private members
                var $ = jQuery;

                // function declarations
                function selectorsAreEqual(a, b) {
                    if (!a || !b || !a.length || a.length !== b.length) {
                        return false;
                    }
                    var length = a.length;
                    for (var i = 0; i < length; i++) {
                        if (a[i] !== b[i])
                            return false;
                    }
                    return true;
                }
            }

            // function declarations
            function assert(val, msg) {
                if (msg !== "")
                    msg = ": " + msg;
                if (val) {
                    error("Assertion failure" + msg);
                }
            }
            function defValue(value, defaultValue) {
                return (typeof value === "undefined") ? defaultValue : value;
            }
            function error(msg) {
                alert("Error: " + msg);
            }
            function getAllRegexMatches(regex, str) {
                var result = [];
                var match;
                while ((match = regex.exec(str)) != null) {
                    result.push(match);
                }
                return result;
            }
            function mdaCreate(arrayLengths) {
                // mda: Multi-Dimension Array
                // mdaCreate([1, 2, 3]) creates a 1 x 2 x 3 dimensioned array.
                var curLength = arrayLengths[0];
                var a = new Array(curLength);

                if (arrayLengths.length == 1) {
                    ; // do nothing
                } else {
                    arrayLengths = arrayLengths.slice(1);
                    for (var i = 0; i < curLength; i++)
                        a[i] = mdaCreate(arrayLengths);
                }
                return a;
            }
            function mdaGet(array, indexes) {
                // mdaGet(myArray, [1, 2, 3]);
                //   is the same as reading myArray[1, 2, 3]
                var result = array;
                for (var i = 0; i < indexes.length; i++) {
                    result = result[indexes[i]];
                }
                return result;
            }
            function mdaIndexOf(array, element) {
                return mdaIndexOfI(array, element, []);

                function mdaIndexOfI(array, element, indexesSoFar) {
                    for (var i = 0; i < array.length; i++) {
                        if (array[i] === element) {
                            indexesSoFar.push(i);
                            return indexesSoFar;
                        }
                        if (Array.isArray(array[i])) {
                            var a = indexesSoFar.slice(0);
                            a.push(i);
                            var j = mdaIndexOfI(array[i], element, a);
                            if (typeof j !== "undefined")
                                return j;
                        }
                    }
                    return undefined;
                }
            }
            function mdaSet(array, indexes, value) {
                // mdaSet(myArray, [1, 2, 3], "foo");
                //   is the same as myArray[1, 2, 3] = "foo"

                var currentArray = array;
                for (var i = 0; i < indexes.length - 1; i++) {
                    currentArray = currentArray[indexes[i]];
                }
                currentArray[indexes[indexes.length - 1]] = value;
            }
            function pad(text, length, char, align) {
                // align: r l or c
                char = defValue(char, "%20");
                align = defValue(align, "l");
                var additionalChars = length - text.length;
                var result = "";
                switch (align) {
                    case "r":
                        result = repeat(char, additionalChars) + text;
                        break;
                    case "l":
                        result = text + repeat(char, additionalChars);
                        break;
                    case "c":
                        var leftSpaces = Math.floor(additionalChars / 2);
                        var rightSpaces = additionalChars - leftSpaces;
                        result = repeat(char, leftSpaces) + text + repeat(char, rightSpaces);
                        break;
                    default:
                        assert(false);
                        break;
                }
                return result;
            }
            function repeat(str, num) {
                return new Array(num + 1).join(str);
            }
            function toBase64(input) {
                // http://en.wikipedia.org/wiki/Base64#Example
                var remainingBits;
                var result = "";
                var additionalCharsNeeded = 0;

                var charIndex = -1;
                var charAsciiValue;
                var advanceToNextChar = function () {
                    charIndex++;
                    charAsciiValue = input.charCodeAt(charIndex);
                    return charIndex < input.length;
                };

                while (true) {
                    var base64Char;

                    // handle 1st char
                    if (!advanceToNextChar()) break;
                    base64Char = charAsciiValue >>> 2;
                    remainingBits = charAsciiValue & 3; // 0000 0011
                    result += getBase64Char(base64Char); // 1st char
                    additionalCharsNeeded = 3;

                    // handle 2nd char
                    if (!advanceToNextChar()) break;
                    base64Char = (remainingBits << 4) | (charAsciiValue >>> 4);
                    remainingBits = charAsciiValue & 15; // 0000 1111
                    result += getBase64Char(base64Char); // 2nd char
                    additionalCharsNeeded = 2;

                    // handle 3rd char
                    if (!advanceToNextChar()) break;
                    base64Char = (remainingBits << 2) | (charAsciiValue >>> 6);
                    result += getBase64Char(base64Char); // 3rd char
                    remainingBits = charAsciiValue & 63; // 0011 1111
                    result += getBase64Char(remainingBits); // 4th char
                    additionalCharsNeeded = 0;
                }

                // there may be an additional 2-3 chars that need to be added
                if (additionalCharsNeeded == 2) {
                    remainingBits = remainingBits << 2; // 4 extra bits
                    result += getBase64Char(remainingBits) + "=";
                } else if (additionalCharsNeeded == 3) {
                    remainingBits = remainingBits << 4; // 2 extra bits
                    result += getBase64Char(remainingBits) + "==";
                } else if (additionalCharsNeeded != 0) {
                    throw "Unhandled number of additional chars needed: " + additionalCharsNeeded;
                }

                return result;
            }
            function trim(str) {
                var rgx = /^\s*(.*?)\s*$/;
                var result = str.match(rgx);
                return result[1];
            }
            function zeroFill(s, width) {
                return pad(s, width, "0", "r");
            }

            // private functions
            function getBase64Char(base64Value) { // TODO: rename?
                if (base64Value < 0) {
                    throw "Invalid number: " + base64Value;
                } else if (base64Value <= 25) {
                    // A-Z
                    return String.fromCharCode(base64Value + "A".charCodeAt(0));
                } else if (base64Value <= 51) {
                    // a-z
                    base64Value -= 26; // a
                    return String.fromCharCode(base64Value + "a".charCodeAt(0));
                } else if (base64Value <= 61) {
                    // 0-9
                    base64Value -= 52; // 0
                    return String.fromCharCode(base64Value + "0".charCodeAt(0));
                } else if (base64Value <= 62) {
                    return '+';
                } else if (base64Value <= 63) {
                    return '/';
                } else {
                    throw "Invalid number: " + base64Value;
                }
            }
        };

</script>