var domainCookie = 'ncc.sys1.bitauto.com';
var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间

function validateEdit(ids) {
    var b = false;
    var ar = document.getElementsByName(ids);
    for (var ii = 0; ii < ar.length; ii++) {
        if (ar[ii].type == "checkbox") {
            if (ar[ii].checked) {
                b = true;
            }
        }
    }
    if (b) {
        return b;

    }
    else {
        $.jAlert('请至少选择一项！');

        return false;
    }
}
//全选反选取消
function selectCheckBoxDelAll(objName, showType) {
    var delAllObj = document.getElementsByName(objName);
    if (showType == 1) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else if (showType == 2) {
        //反选
        for (var i = 0; i < delAllObj.length; i++) {
            if (delAllObj[i].checked) {
                delAllObj[i].checked = false;
            }
            else {
                delAllObj[i].checked = true;
            }
        }
    }
    else if (showType == 3) {
        //取消
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}

function selectCheckBoxDelAllCheck(obj, objName) {
    var delAllObj = document.getElementsByName(objName);
    if (obj.checked == true) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
            delAllObj[i].enabled = false;
        }
    }
    else {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}

//选择框绑定
function CommonCheckBoxBindForRow(allcbx, trcbx) {
    var all = $("input[type='checkbox'][name='" + allcbx + "']");
    var item = $("input[type='checkbox'][name='" + trcbx + "']:enabled");
    all.click(function () {
        for (var i = 0; i < item.length; i++) {
            item[i].checked = all[0].checked;
        }
    });
    item.click(function () {
        var checkeditem = $("input[type='checkbox'][name='" + trcbx + "']:checked:enabled");
        all[0].checked = (item.length == checkeditem.length);
    });
}
//获取选择框的值
function CommonGetCheckBoxValues(name) {
    var item = $("input[type='checkbox'][name='" + name + "']:checked:enabled");
    var values = "";
    for (var i = 0; i < item.length; i++) {
        values += item[i].value + ",";
    }
    if (values.length > 0) {
        values = values.substr(0, values.length - 1);
    }
    return values;
}

//验证浮点数
function isFloat(val) {
    var re = /^[0-9]+.?[0-9]*$/;
    if (!re.test(val)) {
        return true;
    }
    else {
        return false;
    }
}

//页面调转
function redirect(url) {
    window.location.href = url;
}

//将时间格式化年-月-日形式
// function getDate(datetime){ 
//    datetime = datetime.toLocaleDateString();
//    datetime = datetime.replace(/年/,"-");
//    datetime = datetime.replace(/月/,"-");
//    datetime = datetime.replace(/日/,"");
//    return datetime;
//}

function getDate(datetime) {
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1;
    var date = datetime.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (date < 10) {
        date = "0" + date;
    }
    var time = year + "-" + month + "-" + date; //2009-06-12 17:18:05
    return time;
}

//判断日期格式是否合法
String.prototype.isDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false; var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}
//验证长日期(2007-06-05 10:57:10)
String.prototype.isDateTime = function () {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    var r = this.match(reg);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
}

//计算字符串长度，汉字算2
function Len(str) {
    var i, sum;
    sum = 0;
    for (i = 0; i < str.length; i++) {
        if ((str.charCodeAt(i) >= 0) && (str.charCodeAt(i) <= 255))
            sum = sum + 1;
        else
            sum = sum + 2;
    }
    return sum;
}

//检查是否含有汉字
function checkChars(s) {
    if (/[^\x00-\xff]/g.test(s)) {
        return true; //含有汉字
    }
    else {
        return false; //全是字符
    }
}


//状态标签点击切换函数
function Show_TabADSMenu(tabadid_num, tabadnum) {
    for (var i = 0; i < 2; i++) { document.getElementById("tabadcontent_" + tabadid_num + i).style.display = "none"; }
    for (var i = 0; i < 2; i++) { document.getElementById("tabadmenu_" + tabadid_num + i).className = ""; }
    document.getElementById("tabadmenu_" + tabadid_num + tabadnum).className = "linknow";
    document.getElementById("tabadcontent_" + tabadid_num + tabadnum).style.display = "block";
}


//设置表格样式
function SetTableStyle(tableid) {
    //$('#'+tableid+' tr:even').addClass('color_hui');//设置列表行样式
    $('#' + tableid + ' tr').removeData('currentcolor');
    $('#' + tableid + ' tr').mouseover(function () {
        if (!($(this).data('currentcolor')))
            $(this).data('currentcolor', $(this).css('backgroundColor'));
        $(this).css('backgroundColor', '#e5edf1').css('fontWeight', '');
    }).mouseout(function () {
        $(this).css('backgroundColor', $(this).data('currentcolor')).css('fontWeight', '');
    });

}
//手机验证
function isMobile(mobile) {
    //alert("isMobile000003:"+ mobile);
    return (/^(?:13\d|15\d|17\d|18\d|19\d|14\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
}
//电话验证
function isTel(tel) {
    return (/^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$/.test(tel));
}
//电话或者手机验证
function isTelOrMobile(s) {
    //alert("isTelOrMobile000002:"+ s);
    return (isMobile(s) || isTel(s));
}
//邮件验证
function isEmail(s) {
    return (/^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$/.test(s));
}
//营业执照验证
//目前只验证长度和是否都为数字
function isLicense(s) {
    return (!isNaN(s) && s.length == 13);
}

//验证是否为数字
function checkNum(obj) {
    var re = /^-?[1-9]+(\.\d+)?$|^-?0(\.\d+)?$|^-?[1-9]+[0-9]*(\.\d+)?$/;
    if (!re.test(obj)) {
        return false;
    }
    return true;
}


//验证是否为数字
function isNum(s) {
    var pattern = /^[0-9]*$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
//验证是否为正整数
function isPositiveNum(s) {
    var pattern = /^[1-9]\d*$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
//身份证验证
function checkIdcard(idcard) {
    idcard = idcard.replace('x', 'X');
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var idcard, Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.split("");
    //地区检验 
    if (area[parseInt(idcard.substr(0, 2))] == null) return false;
    //身份号码位数及格式检验 
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            if (ereg.test(idcard)) return true;
            else return false;
            break;
        case 18:
            //18位身份号码检测 
            //出生日期的合法性检查  
            //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
            //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
            } else {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
            }
            if (ereg.test(idcard)) {//测试出生日期的合法性 
                //计算校验位 
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
    + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
    + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
    + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
    + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
    + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
    + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
    + parseInt(idcard_array[7]) * 1
    + parseInt(idcard_array[8]) * 6
    + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1); //判断校验位 
                if (M == idcard_array[17]) return true; //检测ID的校验位 
                else return false;
            }
            else return false;
            break;
        default:
            return false;
            break;
    }
}
//兼容性的日历控件
function L_calendar() { }
L_calendar.prototype = {
    _VersionInfo: "Version:1.0&#13",
    Moveable: true,
    NewName: "",
    insertId: "",
    ClickObject: null,
    InputObject: null,
    InputDate: null,
    IsOpen: false,
    MouseX: 0,
    MouseY: 0,
    GetDateLayer: function () {
        if (window.parent) {
            return window.parent.L_DateLayer;
        }
        else { return window.L_DateLayer; }
    },
    L_TheYear: new Date().getFullYear(), //定义年的变量的初始值
    L_TheMonth: new Date().getMonth() + 1, //定义月的变量的初始值
    L_WDay: new Array(39), //定义写日期的数组
    MonHead: new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31),    		   //定义阳历中每个月的最大天数
    GetY: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];
        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetTop;
            while (obj = obj.offsetParent) y += obj.offsetTop;
            return y;
        }
        else { return 0; }
    },
    GetX: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];

        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetLeft;
            while (obj = obj.offsetParent) y += obj.offsetLeft;
            return y;
        }
        else { return 0; }
    },
    CreateHTML: function () {
        var htmlstr = "";
        htmlstr += "<div id=\"L_calendar\">\r\n";
        htmlstr += "<span id=\"SelectYearLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 19;display: none\"></span>\r\n";
        htmlstr += "<span id=\"SelectMonthLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 78;display: none\"></span>\r\n";
        htmlstr += "<div id=\"L_calendar-year-month\"><div id=\"L_calendar-PrevM\" onclick=\"parent." + this.NewName + ".PrevM()\" title=\"前一月\"><b>&lt;</b></div><div id=\"L_calendar-year\" onclick=\"parent." + this.NewName + ".SelectYearInnerHTML('" + this.L_TheYear + "')\"></div><div id=\"L_calendar-month\"   onclick=\"parent." + this.NewName + ".SelectMonthInnerHTML('" + this.L_TheMonth + "')\"></div><div id=\"L_calendar-NextM\" onclick=\"parent." + this.NewName + ".NextM()\" title=\"后一月\"><b>&gt;</b></div></div>\r\n";
        htmlstr += "<div id=\"L_calendar-week\"><ul  onmouseup=\"StopMove()\"><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ul></div>\r\n";
        htmlstr += "<div id=\"L_calendar-day\">\r\n";
        htmlstr += "<ul>\r\n";
        for (var i = 0; i < this.L_WDay.length; i++) {
            htmlstr += "<li id=\"L_calendar-day_" + i + "\" style=\"background:#EFEFEF\" onmouseover=\"this.style.background='#ffffff'\"  onmouseout=\"this.style.background='#e0e0e0'\"></li>\r\n";
        }
        htmlstr += "</ul>\r\n";
        //htmlstr+="<span id=\"L_calendar-today\" onclick=\"parent."+this.NewName+".Today()\"><b>Today</b></span>\r\n";
        htmlstr += "</div>\r\n";
        //htmlstr+="<div id=\"L_calendar-control\"></div>\r\n";
        htmlstr += "</div>\r\n";
        htmlstr += "<scr" + "ipt type=\"text/javas" + "cript\">\r\n";
        htmlstr += "var MouseX,MouseY;";
        htmlstr += "var Moveable=" + this.Moveable + ";\r\n";
        htmlstr += "var MoveaStart=false;\r\n";
        htmlstr += "document.onmousemove=function(e)\r\n";
        htmlstr += "{\r\n";
        htmlstr += "var DateLayer=parent.document.getElementById(\"L_DateLayer\");\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "var DateLayerLeft=DateLayer.style.posLeft || parseInt(DateLayer.style.left.replace(\"px\",\"\"));\r\n";
        htmlstr += "var DateLayerTop=DateLayer.style.posTop || parseInt(DateLayer.style.top.replace(\"px\",\"\"));\r\n";
        htmlstr += "if(MoveaStart){DateLayer.style.left=(DateLayerLeft+e.clientX-MouseX)+\"px\";DateLayer.style.top=(DateLayerTop+e.clientY-MouseY)+\"px\"}\r\n";
        htmlstr += ";\r\n";
        htmlstr += "}\r\n";

        htmlstr += "document.getElementById(\"L_calendar-week\").onmousedown=function(e){\r\n";
        htmlstr += "if(Moveable){MoveaStart=true;}\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "  MouseX = e.clientX;\r\n";
        htmlstr += "  MouseY = e.clientY;\r\n";
        htmlstr += "	}\r\n";

        htmlstr += "function StopMove(){\r\n";
        htmlstr += "MoveaStart=false;\r\n";
        htmlstr += "	}\r\n";
        htmlstr += "</scr" + "ipt>\r\n";
        var stylestr = "";
        stylestr += "<style type=\"text/css\">";
        stylestr += "body{background:transparent;font-size:12px;margin:0px;padding:0px;text-align:left;font-family:宋体,Arial, Helvetica, sans-serif;line-height:1.6em;color:#666666;}\r\n";
        stylestr += "#L_calendar{background:#fff;border:1px solid #7AB8DF;width:158px;padding:1px;height:180px;z-index:100099;text-align:center}\r\n";
        stylestr += "#L_calendar-year-month{height:23px;line-height:23px;z-index:100099;background-color:#2670C1;color:#fff}\r\n";
        stylestr += "#L_calendar-year{line-height:23px;width:60px;float:left;z-index:100099;position: absolute;top: 3; left: 19;cursor:default}\r\n";
        stylestr += "#L_calendar-month{line-height:23px;width:48px;float:left;z-index:100099;position: absolute;top: 3; left: 78;cursor:default}\r\n";
        stylestr += "#L_calendar-PrevM{position: absolute;top: 3; left: 5;cursor:pointer}"
        stylestr += "#L_calendar-NextM{position: absolute;top: 3; left: 145;cursor:pointer}"
        stylestr += "#L_calendar-week{height:23px;line-height:23px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-day{height:136px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-week ul{cursor:move;list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-week li{width:20px;height:20px;float:left;;margin:1px;padding:0px;text-align:center;}\r\n";
        stylestr += "#L_calendar-day ul{list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-day li{cursor:pointer;width:20px;height:20px;float:left;;margin:1px;padding:0px;}\r\n";
        stylestr += "#L_calendar-control{height:25px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-today{cursor:pointer;float:left;width:63px;height:20px;line-height:20px;margin:1px;text-align:center;background:#3B80CB}"
        stylestr += "</style>";
        var TempLateContent = "<html>\r\n";
        TempLateContent += "<head>\r\n";
        TempLateContent += "<title></title>\r\n";
        TempLateContent += stylestr;
        TempLateContent += "</head>\r\n";
        TempLateContent += "<body>\r\n";
        TempLateContent += htmlstr;
        TempLateContent += "</body>\r\n";
        TempLateContent += "</html>\r\n";
        this.GetDateLayer().document.writeln(TempLateContent);
        this.GetDateLayer().document.close();
    },
    InsertHTML: function (id, htmlstr) {
        var L_DateLayer = this.GetDateLayer();
        if (L_DateLayer) { L_DateLayer.document.getElementById(id).innerHTML = htmlstr; }
    },
    WriteHead: function (yy, mm)  //往 head 中写入当前的年与月
    {
        this.InsertHTML("L_calendar-year", yy + " 年");
        this.InsertHTML("L_calendar-month", mm + " 月");
    },
    IsPinYear: function (year)            //判断是否闰平年
    {
        if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0))) return true; else return false;
    },
    GetMonthCount: function (year, month)  //闰年二月为29天
    {
        var c = this.MonHead[month - 1]; if ((month == 2) && this.IsPinYear(year)) c++; return c;
    },
    GetDOW: function (day, month, year)     //求某天的星期几
    {
        var dt = new Date(year, month - 1, day).getDay() / 7; return dt;
    },
    GetText: function (obj) {
        if (obj.innerText) { return obj.innerText }
        else { return obj.textContent }
    },
    PrevM: function ()  //往前翻月份
    {
        if (this.L_TheMonth > 1) { this.L_TheMonth-- } else { this.L_TheYear--; this.L_TheMonth = 12; }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    NextM: function ()  //往后翻月份
    {
        if (this.L_TheMonth == 12) { this.L_TheYear++; this.L_TheMonth = 1 } else { this.L_TheMonth++ }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    Today: function ()  //Today Button
    {
        var today;
        this.L_TheYear = new Date().getFullYear();
        this.L_TheMonth = new Date().getMonth() + 1;
        today = new Date().getDate();
        if (this.InputObject) {
            this.InputObject.value = this.L_TheYear + "-" + this.L_TheMonth + "-" + today;
        }
        this.CloseLayer();
    },
    SetDay: function (yy, mm)   //主要的写程序**********
    {
        this.WriteHead(yy, mm);
        //设置当前年月的公共变量为传入值
        this.L_TheYear = yy;
        this.L_TheMonth = mm;
        for (var i = 0; i < 39; i++) { this.L_WDay[i] = "" };  //将显示框的内容全部清空
        var day1 = 1, day2 = 1, firstday = new Date(yy, mm - 1, 1).getDay();  //某月第一天的星期几

        for (i = 0; i < firstday; i++) this.L_WDay[i] = this.GetMonthCount(mm == 1 ? yy - 1 : yy, mm == 1 ? 12 : mm - 1) - firstday + i + 1	//上个月的最后几天
        for (i = firstday; day1 < this.GetMonthCount(yy, mm) + 1; i++) { this.L_WDay[i] = day1; day1++; }
        for (i = firstday + this.GetMonthCount(yy, mm); i < 39; i++) { this.L_WDay[i] = day2; day2++ }
        for (i = 0; i < 39; i++) {
            var da = this.GetDateLayer().document.getElementById("L_calendar-day_" + i + "");
            var month, day;
            if (this.L_WDay[i] != "") {
                if (i < firstday) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm-1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    }
                    else {
                        da.setAttribute("onclick", "null");
                    }
                }
                else if (i >= firstday + this.GetMonthCount(yy, mm)) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm+1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    }
                    else {
                        da.setAttribute("onclick", "null");
                    }
                }
                else {
                    da.innerHTML = "<b style=\"color:#000\">" + this.L_WDay[i] + "</b>";
                    //month=(mm==1?12:mm);
                    month = mm;
                    day = this.L_WDay[i];
                    if (document.all) {
                        da.onclick = Function("parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    else {
                        da.setAttribute("onclick", "parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    da.title = month + "月" + day + "日";
                    da.style.background = (yy == new Date().getFullYear() && month == new Date().getMonth() + 1 && day == new Date().getDate()) ? "#3B80CB" : "#EFEFEF";
                    if (this.InputDate != null) {
                        if (yy == this.InputDate.getFullYear() && month == this.InputDate.getMonth() + 1 && day == this.InputDate.getDate()) {
                            da.style.background = "#FF0000";
                        }
                    }
                }


            }
        }
    },
    SelectYearInnerHTML: function (strYear) //年份的下拉框
    {
        if (strYear.match(/\D/) != null) { $.jAlert("年份输入参数不是数字！"); return; }
        var m = (strYear) ? strYear : new Date().getFullYear();
        if (m < 1000 || m > 9999) { $.jAlert("年份值不在 1000 到 9999 之间！"); return; }
        var n = m - 50;
        if (n < 1000) n = 1000;
        if (n + 56 > 9999) n = 9974;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectYear\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectYearLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectYearLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheYear = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = n; i < n + 56; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "年" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "年" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectYearLayer").style.display = "";
        DateLayer.document.getElementById("SelectYearLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectYear").focus();
    },
    SelectMonthInnerHTML: function (strMonth) //月份的下拉框
    {
        if (strMonth.match(/\D/) != null) { $.jAlert("月份输入参数不是数字！"); return; }
        var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectMonth\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectMonthLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectMonthLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheMonth = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = 1; i < 13; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "月" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "月" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectMonthLayer").style.display = "";
        DateLayer.document.getElementById("SelectMonthLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectMonth").focus();
    },
    DayClick: function (mm, dd)  //点击显示框选取日期，主输入函数*************
    {
        var yy = this.L_TheYear;
        //判断月份，并进行对应的处理
        if (mm < 1) { yy--; mm = 12 + mm; }
        else if (mm > 12) { yy++; mm = mm - 12; }
        if (mm < 10) { mm = "0" + mm; }
        if (this.ClickObject) {
            if (!dd) { return; }
            if (dd < 10) { dd = "0" + dd; }
            this.InputObject.value = yy + "-" + mm + "-" + dd; //注：在这里你可以输出改成你想要的格式
            this.CloseLayer();
        }
        else { this.CloseLayer(); alert("您所要输出的控件对象并不存在！"); }
    },
    SetDate: function () {
        if (arguments.length < 1) { alert("对不起！传入参数太少！"); return; }
        else if (arguments.length > 3) { alert("对不起！传入参数太多！"); return; }
        this.InputObject = (arguments.length == 1) ? arguments[0] : arguments[1];
        this.ClickObject = arguments[0];
        if (typeof (arguments[arguments.length - 1]) == 'function') {//如果最后一个参数是函数的话，为关闭时的响应方法
            this.OnClose = arguments[arguments.length - 1];
        }
        var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
        var r = this.InputObject.value.match(reg);
        if (r != null) {
            r[2] = r[2] - 1;
            var d = new Date(r[1], r[2], r[3]);
            if (d.getFullYear() == r[1] && d.getMonth() == r[2] && d.getDate() == r[3]) {
                this.InputDate = d; 	//保存外部传入的日期
            }
            else this.InputDate = "";
            this.L_TheYear = r[1];
            this.L_TheMonth = r[2] + 1;
        }
        else {
            this.L_TheYear = new Date().getFullYear();
            this.L_TheMonth = new Date().getMonth() + 1
        }
        this.CreateHTML();
        var top = this.GetY();
        var left = this.GetX();
        var DateLayer = document.getElementById("L_DateLayer");

        //判断如果浏览器是ie 7.0，且是查询列表中的时间控件 则将离左边宽度-240px;高度加上3px
        if ($.browser.msie) {
            if ($.browser.version == "7.0" && $("#sidebar").length > 0) {
                left = left - 240;
                top = top + 3;
            }
        }

        DateLayer.style.top = top + this.ClickObject.clientHeight + 5 + "px";
        DateLayer.style.left = left + "px";

        DateLayer.style.display = "block";
        if (document.all) {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "160px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "180px"
        }
        else {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "154px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "180px"
            DateLayer.style.width = "158px";
            DateLayer.style.height = "250px";
        }
        //alert(DateLayer.style.display)
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    CloseLayer: function () {
        try {
            var DateLayer = document.getElementById("L_DateLayer");
            if ((DateLayer.style.display == "" || DateLayer.style.display == "block") && arguments[0] != this.ClickObject && arguments[0] != this.InputObject) {
                DateLayer.style.display = "none";
                if (this.OnClose) { this.OnClose(this.InputObject); }
            }
        }
        catch (e) { }
    }
}

document.writeln('<iframe id="L_DateLayer" name="L_DateLayer" frameborder="0" style="position:absolute;width:160px; height:190px;overflow:hidden;z-index:100099;display:none;backgorund-color:transparent;"></iframe>');
var MyCalendar = new L_calendar();
MyCalendar.NewName = "MyCalendar";
document.onclick = function (e) {
    e = window.event || e;
    var srcElement = e.srcElement || e.target;
    MyCalendar.CloseLayer(srcElement);
}




//编辑数据
function toggles(obj_id) {
    var target = document.getElementById(obj_id);

    var bgObj = document.getElementById("bgDiv");
    bgObj.style.width = document.body.offsetWidth + "px";
    bgObj.style.height = screen.height + "px";

    var bgOifm = document.getElementById("iframe_top");
    bgOifm.style.width = document.body.offsetWidth + "px";
    bgOifm.style.height = screen.height + "px";

    if (target.style.display == "none") {
        target.style.display = "block";
        target.style.top = (document.documentElement.scrollTop + document.documentElement.clientHeight / 2);
        bgObj.style.display = "block";
        bgOifm.style.display = "block";
    }
    else {
        target.style.display = "none";
        bgObj.style.display = "none";
        bgOifm.style.display = "none";
    }
}

//站点异步统计加载耗时-JS方法 Add=Masj，Date=2016-03-15
function StatAjaxPageTime(startTime, url) {
    var t = new Date().getTime() - startTime;
    $.post("/AjaxServers/LoginManager.ashx", { Action: "StatAjaxPageTime", LoadTime: t, RequestURL: url }, function (data) {

    });
}



/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091207
*/
function AjaxPost(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            AjaxErrorLogic(XMLHttpRequest, textStatus);
        }
    });
}

/*
* 同步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=chybin,Date=20120801
*/
function AjaxPostAsync(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        async: false,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            AjaxErrorLogic(XMLHttpRequest, textStatus);
        }
    });
}


/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091215
*/
function AjaxGet(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "GET",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            AjaxErrorLogic(XMLHttpRequest, textStatus);
        }
    });
}

/*
* 异步调用出错后提示消息
* Add=Masj, Date: 2016-07-11
*/
function AjaxErrorLogic(XMLHttpRequest, textStatus) {
    $.unblockUI();
    alert('网络超时，提交失败，请重试！');
    return;
}

/*
* 绑定省份，需要引用Area.js文件
* Area.js文件是生成的
* Add=Masj, Date: 2009-12-07 
*/
function BindProvince(SelectID) {
    if (JSonData && JSonData.masterArea.length > 0) {
        var masterObj = document.getElementById(SelectID);
        if (masterObj && masterObj.options) {
            masterObj.options.length = 0;
            masterObj.options[0] = new Option("省/直辖市", -1);
            for (var i = 0; i < JSonData.masterArea.length; i++) {
                masterObj.options[masterObj.options.length] = new Option(JSonData.masterArea[i].name, JSonData.masterArea[i].id);
            }
        }
    }
}

/*
* 绑定城市，需要引用Area.js文件
* Area.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID
* Add=Masj, Date: 2009-12-07 
*/
function BindCity(provinceSelectID, citySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var masterObjid = temp.value;
    if (masterObjid && masterObjid > 0) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == masterObjid) {
                for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                    subAreaObj.options[subAreaObj.options.length] = new Option(JSonData.masterArea[i].subArea[j].name, JSonData.masterArea[i].subArea[j].id);
                }
            }
        }
    }
    else if (masterObjid && masterObjid == -1) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
    }
}

/*
* 绑定区县，需要引用Area2.js文件
* Area2.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID, countyID为区县ID
*/
function BindCounty(provinceSelectID, citySelectID, countySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var provinceId = temp.value;

    var temp = document.getElementById(citySelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(citySelectID).selectedIndex]; if (!temp) { return; }
    var cityId = temp.value;
    if (provinceId && provinceId > 0 && cityId && cityId > 0) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceId) {
                var t1 = JSonData.masterArea[i];
                for (var j = 0; j < t1.subArea.length; j++) {
                    if (t1.subArea[j].id == cityId) {
                        var t2 = t1.subArea[j];
                        for (var k = 0; k < t2.subArea2.length; k++) {
                            subAreaObj.options[subAreaObj.options.length] =
                                new Option(t2.subArea2[k].name, t2.subArea2[k].id);
                        }
                        return;
                    }
                }
            }
        }
    }
    else if ((provinceId && provinceId == -1) || (cityId && cityId == -1)) {
        var subAreaObj = document.getElementById(countySelectID);
        if (subAreaObj != null) {
            subAreaObj.options.length = 0;
            subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        }
    }
}


/*
* 绑定枚举列表，需要引用ShowEnum.js文件和类库JQuery
* ShowEnum.js文件是生成的
* 参数arrayObject为数组对象
* Add=Masj, Date: 2009-12-07 
*/
function BindArrayToSelect(arrayObject, selectID, str) {
    var selectObject = document.getElementById(selectID);
    selectObject.options.length = 0;
    if (str) {
        selectObject.options[0] = new Option(str, -1);
    }
    else {
        selectObject.options[0] = new Option("请选择", -1);
    }
    $.each(arrayObject, function (name, value) {
        selectObject.options[selectObject.options.length] = new Option(value[0], value[1]);
    });
}

/**
* @desc   escape字符串,escape不编码字符有69个：*，-，.，@，_，0-9，a-z，A-Z
* @param  字符串
* @return 返回string等对象
* @Add=Masj, Date: 2009-12-16
*/
function escapeStr(str) {
    return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
}

/**
* @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
* @param   json字符串
* @return 返回object,array,string等对象
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
 {
     evalJSON: function (strJson) {
         try {
             if ($.trim(strJson) == '')
                 return '';
             else
                 return eval("(" + strJson + ")");
         } catch (e) {
             return '';
         }
     }
 });
/**
* @desc  JQuery扩展，将javascript数据类型转换为json字符串，需要引用类库JQuery
* @param 待转换对象,支持object,array,string,function,number,boolean,regexp
* @return 返回json字符串
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
{
    toJSONstring: function (object) {
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor)
                type = 'array';
            else if (RegExp == object.constructor)
                type = 'regexp';
            else
                type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g,
                      function () {
                          var a = arguments[0];
                          return (a == '\n') ? '\\n' :
                                   (a == '\r') ? '\\r' :
                                   (a == '\t') ? '\\t' : ""
                      }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSONstring(object[property]);
                    if (typeof (value) != "undefined")
                        results.push(jQuery.toJSONstring(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSONstring(object[i]);
                    if (typeof (value) != "undefined") results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    }
});

//获得客户浏览器类型
function GetBrowserName() {
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

    //以下进行测试
    if (Sys.ie) {
        //alert('IE: ' + Sys.ie);
        return 'IE';
    }
    else if (!!window.ActiveXObject || "ActiveXObject" in window) {
        return 'IE';
    }
    else if (Sys.firefox) {
        //alert('Firefox: ' + Sys.firefox);
        return 'FF';
    }
    else
        return '';
    //        if (Sys.chrome) document.write('Chrome: ' + Sys.chrome);
    //        if (Sys.opera) document.write('Opera: ' + Sys.opera);
    //        if (Sys.safari) document.write('Safari: ' + Sys.safari);
}

//重置
function resetForm(id) {
    jQuery('#' + id).each(function () {
        this.reset();
    });
}

//关闭弹出层
/**
@name 弹出层的名字
@, isCancel 是否是取消之类的操作，默认为true
*/
function Close(name, effectiveAction) {
    $.closePopupLayer(name, effectiveAction);
}

/**
* @desc  返回字符串长度
* @param 字符串
* @return 返回字符串长度
* @Add=Masj, Date: 2009-12-11
*/
function GetStringRealLength(str) {
    var bytesCount = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charAt(i);
        if (/^[\u0000-\u00ff]$/.test(c))   //匹配双字节
        {
            bytesCount += 1;
        }
        else {
            bytesCount += 2;
        }
    }
    return bytesCount;
}

/*头部鼠标划过出现下拉层star*/
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
}

function addClass(element, value) {
    if (!element.className) {
        element.className = value;
    } else {
        newClassName = element.className;
        newClassName += " ";
        newClassName += value;
        element.className = newClassName;
    }
}

function removeClass(element, value) {
    var removedClass = element.className;
    var pattern = new RegExp("(^| )" + value + "( |$)");
    removedClass = removedClass.replace(pattern, "$1");
    removedClass = removedClass.replace(/ $/, "");
    element.className = removedClass;
    return true;
}

function bt_login_more(overID, boxID, add_Class) {
    if (!document.getElementById(overID)) return false;
    var btli = document.getElementById(overID);
    var btpop = document.getElementById(boxID);
    btli.onmouseover = function () {
        addClass(btpop, add_Class)
    }
    btli.onmouseout = function () {
        removeClass(btpop, add_Class)
    }
}
function all_login_box() {
    bt_login_more('goOther', 'goOtherContent', 'pop_block');
}
addLoadEvent(all_login_box);
/*头部鼠标划过出现下拉层end*/





/*节选自jQueryString v2.0.2*/
(function ($) {
    $.unserialise = function (Data) {
        var Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };
})(jQuery);


/*设置透明度，兼容IE和FF*/
; (function ($) {
    $.freeOpacity = {
        main: function (opacity) {
            this.each(function (i) {
                var _this = $(this);
                if ($.browser.msie) { _this.css('filter', 'alpha(opacity=' + opacity * 100 + ')'); }
                else { _this.css('opacity', opacity); this.style.Opacity = 0.5; }
            });
            return this;
        }
    }; $.fn.opacity = $.freeOpacity.main;
})(jQuery);

/*
* jqDnR - Minimalistic Drag'n'Resize for jQuery.
*
* Copyright (c) 2007 Brice Burgess <bhb@iceburg.net>, http://www.iceburg.net
* Licensed under the MIT License:
* http://www.opensource.org/licenses/mit-license.php
* 
* $Version: 2007.08.19 +r2
* Drag and Resize, 我觉得比jQuery.ui中写的还要好...
* 我将opacity注释掉，因为IE中会有BUG；添加了setCapture，解决IE中在浏览器外不响应mouseup事件的问题。
* Modify Date=2014-11-18 弹出层范围限制（仅显示top）
*/
(function ($) {
    $.fn.jqDrag = function (h) { return i(this, h, 'd'); };
    $.fn.jqResize = function (h) { return i(this, h, 'r'); };
    $.jqDnR = { dnr: {}, e: 0,
        drag: function (v) {
            //var scrollWidth = Math.max(document.documentElement.scrollWidth, document.body.scrollWidth);
            //var clientWidth = document.documentElement.clientWidth || document.body.clientWidth;
            var scrollHeight = Math.max(document.documentElement.clientHeight, document.body.scrollHeight);
            //var clientHeight = document.documentElement.clientHeight || document.body.clientHeight;
            //if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: M.Y + v.pageY - M.pY });
            if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: (M.Y + v.pageY - M.pY) < 0 || (scrollHeight - M.H) < 0 ? 0 : (M.Y + v.pageY - M.pY) < scrollHeight - M.H ? (M.Y + v.pageY - M.pY) : scrollHeight - M.H });
            else E.css({ width: Math.max(v.pageX - M.pX + M.W, 0), height: Math.max(v.pageY - M.pY + M.H, 0) });
            return false;
        },
        stop: function (h) {/*E.opacity(M.o);*/
            if (h[0].releaseCapture) { h[0].releaseCapture(); } //取消捕获范围
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).unbind('mousemove', J.drag).unbind('mouseup', J.stop);
        }
    };
    var J = $.jqDnR, M = J.dnr, E = J.e,
i = function (e, h, k) {
    return e.each(function () {
        h = (h) ? $(h, e) : e;
        h.bind('mouseover', function () { $(this).css('cursor', 'move'); })
 .bind('mouseout', function () { $(this).css('cursor', 'auto'); });
        h.bind('mousedown', { e: e, k: k }, function (v) {
            var d = v.data, p = {}; E = d.e;
            // attempt utilization of dimensions plugin to fix IE issues
            if (E.css('position') != 'relative') { try { E.position(p); } catch (e) { } }
            M = { X: p.left || f('left') || 0, Y: p.top || f('top') || 0, W: f('width') || E[0].scrollWidth || 0, H: f('height') || E[0].scrollHeight || 0, pX: v.pageX, pY: v.pageY, k: d.k, o: E.css('opacity') };
            /*E.opacity(0.8);*/
            //设置捕获范围
            if (h[0].setCapture) { h[0].setCapture(); }
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).mousemove($.jqDnR.drag).mouseup(function () {
                $.jqDnR.stop(h);
            });
            return false;
        });
    });
},
f = function (k) { return parseInt(E.css(k)) || false; };
})(jQuery);


//载入时的动画. eleId为容器ID
function LoadingAnimation(eleId, classname) {
    classname = $.trim(classname);
    if (classname == "") {
        classname = "div_loading";
    }
    jQuery('#' + eleId).html('<div class="' + classname + '" style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
}

function MaskPage() {
    jQuery('body').append('<div name="divMaskPage" style="position: fixed; left: 0; top: 0; width: 100%; height: 100%; z-index: 20000;background-color: #666; opacity: 0.3; filter: alpha(opacity=30);"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
}
function unMaskPage() {
    $(document.getElementsByName("divMaskPage")).remove();
}

function fullChar2halfChar(str) {
    var result = '';
    for (i = 0; i < str.length; i++) {
        code = str.charCodeAt(i);             //获取当前字符的unicode编码
        if (code >= 65281 && code <= 65373)   //unicode编码范围是所有的英文字母以及各种字符
        {
            result += String.fromCharCode(str.charCodeAt(i) - 65248);    //把全角字符的unicode编码转换为对应半角字符的unicode码
        }
        else if (code == 12288)                                      //空格
        {
            result += String.fromCharCode(str.charCodeAt(i) - 12288 + 32); //半角空格
        } else {
            result += str.charAt(i);                                     //原字符返回
        }
    }
    return result;
}

/*! Copyright (c) 2010 Brandon Aaron (http://brandonaaron.net)
* Licensed under the MIT License (LICENSE.txt).
* Version 2.1.3-pre
*/
(function ($) {
    $.fn.bgiframe = ($.browser.msie && /msie 6\.0/i.test(navigator.userAgent) ? function (s) {
        s = $.extend({
            top: 'auto', // auto == .currentStyle.borderTopWidth
            left: 'auto', // auto == .currentStyle.borderLeftWidth
            width: 'auto', // auto == offsetWidth
            height: 'auto', // auto == offsetHeight
            opacity: true,
            src: 'javascript:false;'
        }, s);
        var html = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="' + s.src + '"' +
                   'style="display:block;position:absolute;z-index:-1;' +
                       (s.opacity !== false ? 'filter:Alpha(Opacity=\'0\');' : '') +
                       'top:' + (s.top == 'auto' ? 'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')' : prop(s.top)) + ';' +
                       'left:' + (s.left == 'auto' ? 'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')' : prop(s.left)) + ';' +
                       'width:' + (s.width == 'auto' ? 'expression(this.parentNode.offsetWidth+\'px\')' : prop(s.width)) + ';' +
                       'height:' + (s.height == 'auto' ? 'expression(this.parentNode.offsetHeight+\'px\')' : prop(s.height)) + ';' +
                '"/>';
        return this.each(function () {
            if ($(this).children('iframe.bgiframe').length === 0)
                this.insertBefore(document.createElement(html), this.firstChild);
        });
    } : function () { return this; });
    // old alias
    $.fn.bgIframe = $.fn.bgiframe;
    function prop(n) {
        return n && n.constructor === Number ? n + 'px' : n;
    }
})(jQuery);

(function ($) {
    $.fn.centerScreen = function (ratioW, ratioH) {
        if (!ratioW) { ratioW = 0.5; }
        if (!ratioH) { ratioH = 0.5; }
        var top = ($(window).height() - this.height()) * ratioH;
        var left = ($(window).width() - this.width()) * ratioW;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();
        return this.css({ position: 'absolute', 'top': top + scrollTop, left: left + scrollLeft }).show();
    }
})(jQuery)

//将一块区域覆盖
function CoverArea(ele, coverId, txt) {
    var source = jQuery(ele);
    if (source.size() <= 0) { return; }
    if (!coverId) { coverId = '_coverAreaID' }
    if (!txt) { txt = ''; }
    var z = source.css('z-index'); //"z-index"
    if (z == 'auto') { z = 90000; }
    var c = jQuery("<div/>").attr('id', coverId).appendTo(source).css({
        'z-index': z + 3,
        background: 'gray',
        position: 'absolute',
        left: source.offset().left,
        top: source.offset().top,
        height: source.height(),
        width: source.width()
    }).opacity(0.5);
    var textDiv = $('<div class="locking" style="">' + txt + '</div>');
    textDiv.centerScreen(0.2);
    var closeSpan = $('<span onclick="javascript:$(\'#' + coverId + '\').remove();">close</span>');
    c.append(textDiv).append(closeSpan).bgiframe();
    return c;
}

function OpenWaitting() {
    $.openPopupLayer({
        name: "WaittingPopup",
        parameters: {},
        url: "../AjaxServers/RequestWaittingPoper.aspx",
        beforeClose: function (e) {
        }
    });
}
function CloseWaitting() {
    $.closePopupLayer('WaittingPopup', false);
}
//日期增加函数
function dateAdd(strInterval, NumDay, dtDate) {
    var dtTmp = new Date(dtDate);
    if (isNaN(dtTmp)) dtTmp = new Date();
    switch (strInterval) {
        case "s": return new Date(Date.parse(dtTmp) + (1000 * NumDay));
        case "n": return new Date(Date.parse(dtTmp) + (60000 * NumDay));
        case "h": return new Date(Date.parse(dtTmp) + (3600000 * NumDay));
        case "d": return new Date(Date.parse(dtTmp) + (86400000 * NumDay));
        case "w": return new Date(Date.parse(dtTmp) + ((86400000 * 7) * NumDay));
        case "m": return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + NumDay, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case "y": return new Date((dtTmp.getFullYear() + NumDay), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}


//写cookies函数
function SetCookie(name, value)//两个参数，一个是cookie的名子，一个是值
{
    var Days = 1;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    // document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=oa.bitauto.com;path=/;';
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=' + domainCookie + ';path=/;';
}
function GetCookie(name)//取cookies函数        
{
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;

}
function DelCookie(name)//删除cookie
{
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = GetCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";domain=" + domainCookie + ";path=/;";
}

String.prototype.rExp = function (a1, a2) {
    var reg = new RegExp(a1, "g");
    return this.replace(reg, a2);
}
String.prototype.returnRegExp = function () {
    var str = this;

    str = str.rExp("\\+", "%2B");
    str = str.rExp("%F7", escape("&divide;"));
    str = str.rExp("%B1", escape("&plusmn;"));
    str = str.rExp("%D7", escape("&times;"));
    str = str.rExp("%A9", escape("&copy;"));
    str = str.rExp("%AE", escape("&reg;"));
    str = str.rExp("%B7", escape("&middot;"));
    str = str.rExp("%A3", escape("&pound;"));
    str = str.rExp("%u2122", escape("&#8482;"));
    str = str.rExp("%u2022", escape("&bull;"));


    return str;
}
//得到radio的value值
function getRadioVal(name) {
    if ($("input[name='isMakeUp']:checked").length > 0) {
        return $("input[name='isMakeUp']:checked").val();
    }
    else {
        return "";
    }
}
//得到checkbox的value值
function getCheckBoxVal(name) {
    var val = "";
    var d = $(":checkbox[name='" + name + "'][checked=true]");
    if (d.length != 0) {
        for (var i = 0; i < d.length; i++) {
            val += d.eq(i).val() + ",";
        }
        val = val.substring(0, val.length - 1);
    }
    return val;
}

//点击文字，选中复选框、单选框
function emChkIsChoose(othis) {
    var $ChkRdo = $(othis).prev();
    //控件未被禁用时点击生效
    if (!$ChkRdo.is(":disabled")) {
        //判断 单选OR复选
        if ($ChkRdo.attr("type") == "checkbox") {
            if ($ChkRdo.is(":checked")) {
                $ChkRdo.removeAttr("checked");
            }
            else {
                $ChkRdo.attr("checked", "checked");
            }
        }
        else if ($ChkRdo.attr("type") == "radio") {
            if (!$ChkRdo.is(":checked")) {
                $ChkRdo.attr("checked", "checked");
            }
        }
    }
}

//把Json对象转换成 & 分割的串形式
function JsonObjToParStr(json) {
    var tmps = [];
    for (var key in json) {
        tmps.push(key + '=' + escape(json[key]));
    }
    return tmps.join('&');
}

///根据枚举绑定下拉列表
///id  下拉列表的ID
/// enumName 枚举名称
function BindByEnum(id, enumName) {
    AjaxPostAsync('/AjaxServers/Common/GetFromEnum.ashx', { Action: 'GetListByEnum', EnumName: enumName }, null, function (data) {
        var jsonData = $.evalJSON(data);
        $("[id$='" + id + "']").html("");
        $("[id$='" + id + "']").append("<option value='-1'>请选择</option>");
        $(jsonData.root).each(function (i, v) {
            $("[id$='" + id + "']").append("<option value=" + v.value + ">" + v.name + "</option>");
        });
    });
}

//页面敲回车键，执行的方法，funName-方法名
//注：如果列表页面有弹出层也需要使用该方法，则需要在列表页面调用完弹出层方法后的回调函数重新绑定该方法，否则列表页因为document的keydown事件被解除而不可用
enterSearch = function (funName) {
    $(document).unbind("keydown");
    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            funName();
        }
    });
}

//初始化时间
//type=1：只有一个时间输入框用以下脚本初始化；
//type=2:有两个时间输入框，前面的日期不能大于后面的日期
//type=3:有两个时间输入框，且需要精确到时分秒
//arryTimeID:时间控件ID数组。例：InitWdatePicker(2, ["tfBeginTime", "tfEndTime"]);
function InitWdatePicker(type, arryTimeID) {

    switch (type) {
        case 1: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker(); });
            break;
        case 2: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker({ maxDate: "#F{$dp.$D(" + arryTimeID[1] + ")}", onpicked: function () { document.getElementById(arryTimeID[1]).focus(); } }); });
            $('#' + arryTimeID[1]).bind('click focus', function () { WdatePicker({ minDate: "#F{$dp.$D(" + arryTimeID[0] + ")}" }); });
            break;
        case 3: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 00:00:00", maxDate: "#F{$dp.$D(" + arryTimeID[1] + ")}", onpicked: function () { document.getElementById(arryTimeID[1]).focus(); } }); });
            $('#' + arryTimeID[1]).bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", minDate: "#F{$dp.$D(" + arryTimeID[0] + ")}" }); });
            break;
    }
}


//搜索-智能提示
function getContactTips(id, telID, showWidth) {
    if ($.trim($("#" + telID).val()) == "") {
        return false;
    }
    $("#" + id).autocomplete("/AjaxServers/ContactTips.ashx", {
        minChars: 1,
        width: showWidth,
        scrollHeight: 300,
        autoFill: false,
        delay: 100,
        matchContains: "word",
        extraParams: { keyWord: function () { return $("#" + id).val(); }, Tel: function () { return $.trim($("#" + telID).val()) }, r: Math.random() },
        parse: function (data) {
            if (data != "") {
                return $.map(eval(data), function (row) {
                    return {
                        data: row,
                        value: row.CustID,    //此处无需把全部列列出来，只是两个关键列
                        result: data.CustName
                    }
                });
            }
        },
        formatItem: function (data, i, n, value) {
            return data.CustName;
        },
        formatResult: function (data, value) {
            return data.CustID;
        }
    });

    //选中时填写到输入框中
    $("#" + id).result(function (event, data, formatted) {
        if (data && data != "")
            $(this).val(data.CustName);
    });

}

//选择推荐活动弹出层 add lxw 14.1.14
function fnSelectActivityPop(opts, afterFn) {
    var popObj = { ids: "", values: "", msg: "" };
    if (typeof opts != "object") {
        popObj.msg = "传入的参数格式不正确！";
        return popObj;
    }
    var params = opts;
    //验证
    if (!params.pid || !params.cid) {
        popObj.msg = "省份城市传入参数有错误！";
        return popObj;
    }
    if (params.pid == "-1" && params.cid == "-1") {
        popObj.msg = "请选择省份或城市！";
        return popObj;
    }
    var brandID = "";
    if (!params.carid || !params.bid) {
        popObj.msg = "品牌车型传入参数有错误！";
        return popObj;
    }
    if ((params.bid == "0" || params.bid == "-1") && (params.carid == "0" || params.carid == "-1")) {
        popObj.msg = "请选择品牌车型！";
        return popObj;
    }
    if (params.carid != "0" && params.carid != "-1") {
        brandID = params.carid;
    }
    else {
        brandID = params.bid;
    }

    $.openPopupLayer({
        name: "selectActivityAjaxPopup",
        parameters: {},
        url: "/TemplateManagement/SelectActivity.aspx?ActivityIDs=" + params.selectids + "&ProvinceID=" + params.pid + "&CityID=" + params.cid + "&BrandID=" + brandID,
        beforeClose: function (b, cData) {
            if (b) {
                popObj.ids = cData.ActivityIDs;
                popObj.values = cData.ActivityNames;
                if (typeof afterFn == "function") {
                    afterFn(popObj);
                }
            }
        },
        afterClose: function () {
            //敲回车键执行方法
            //enterSearch(search);
        }
    });

    return popObj;
}

//添加多个绑定事件 add lxw 14.1.14
//调用：AttachEvent("id", "change", function () { alert(0); });
function AttachEvent(id, eventName, fn) {
    if (window.attachEvent)//IE
        document.getElementById(id).attachEvent("on" + eventName, fn);
    else//FF
        document.getElementById(id).addEventListener(eventName, fn, false);
}

//其他任务统一登录处理逻辑 add=masj,Date=2016-03-10
function OtherBusinessLogin(obj) {
    if (typeof obj == "object" && obj) {
        AjaxPostAsync("/AjaxServers/OtherBusinessLogin.ashx", obj, null, function (data) {
            if (data) {
                if (data != "" && data != "Error") {
                    try {
                        var dd = '/browsercontrol/newpage?url=' + data;
                        if (obj.callbackurl && obj.callbackurl != '') {
                            dd += ("&callback=" + encodeURIComponent(obj.callbackurl));
                        }
                        window.external.MethodScript(dd);
                    }
                    catch (e) {
                        var strUrl = decodeURIComponent(data);
                        if (obj.callbackurl && obj.callbackurl != '') {
                            strUrl += ("&callback=" + encodeURIComponent(obj.callbackurl));
                        }
                        window.open(strUrl);
                    }
                }
            }
        });
    }
}


//其他任务统一登录处理逻辑 add=masj,Date=2016-08-16
function OtherBusinessLoginByIframe(obj, divObj, loadUrl) {
    if (typeof obj == "object" && obj) {
        AjaxPostAsync("/AjaxServers/OtherBusinessLogin.ashx", obj, null, function (data) {
            if (data) {
                if (data != "" && data != "Error") {
                    var strUrl = decodeURIComponent(data);
                    if (obj.callbackurl && obj.callbackurl != '') {
                        strUrl += ("&callback=" + encodeURIComponent(obj.callbackurl));
                    }
                    //console.log(strUrl);
                    //                    var iframeObj = $('<iframe>').attr({
                    //                        id: 'iframeSubPage',
                    //                        name: 'iframeSubPage',
                    //                        src: strUrl,
                    //                        width: '1100px',
                    //                        height: '790px',
                    //                        scrolling: 'yes'
                    //                    });
                    divObj.empty().load(loadUrl + encodeURIComponent(strUrl) + "&r=" + Math.random());
                }
            }
        });
    }
}

//质检列表，个人客户业务记录，话务记录，话务总表，去掉记录 易湃惠买车页面查看
function GoToEpURL(acontrl, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID) {
    var OrgUrl = $(acontrl).attr("urlstr");
    var obj = new Object();
    obj.businessType = 'huimaiche';
    obj.YPFanXianURL = YPFanXianHBuyCarURL;
    obj.TaskURL = OrgUrl;
    obj.EPEmbedCC_APPID = EPEmbedCCHBuyCar_APPID;
    OtherBusinessLogin(obj);
}

////母版页面易车车贷任务页登录
//function CarFinancialLogin() {
//    AjaxPostAsync("/AjaxServers/CarFinancial.ashx", null, null, function (data) {
//        //alert("data1=" + data);
//        if (data) {
//            if (data != "" && data != "Error") {
//                try {

//                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
//                }
//                catch (e) {
//                    window.open(decodeURIComponent(data));
//                }
//            }
//        }
//    });
//}

//function CarFinancialLoginToUrl(url, EncryType) {
//    AjaxPostAsync("/AjaxServers/CarFinancial.ashx", null, null, function (data) {
//        if (data) {
//            if (data != "" && data != "Error") {
//                try {
//                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data + "&callback=" + encodeURIComponent(url));
//                }
//                catch (e) {
//                    window.open(decodeURIComponent(data) + "&callback=" + encodeURIComponent(url));
//                }
//            }
//        }
//    });
//}



////母版页面精准广告任务页登录
//function EasySetOffLogin() {
//    AjaxPostAsync("/AjaxServers/EasySetOff.ashx", null, null, function (data) {
//        //alert("data2="+data);
//        if (data) {
//            if (data != "" && data != "Error") {
//                try {

//                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
//                }
//                catch (e) {
//                    window.open(decodeURIComponent(data));
//                }
//            }
//        }
//    });
//}

//质检列表，个人客户业务记录，话务记录，话务总表，去掉记录 精准广告页面查看
function GoToURLEasySetOff(acontrl) {
    AjaxPostAsync("/AjaxServers/EasySetOff.ashx", { GoToEPURL: acontrl }, null, function (data) {
        //alert("data2="+data);
        if (data) {
            if (data != "" && data != "Error") {
                try {

                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
                }
                catch (e) {
                    window.open(decodeURIComponent(data));
                }
            }
        }
    });
}
//母版页面-帮买业务登录
function BangMaiLogin(url) {
    if (url != "") {
        try {
            var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + url);
        }
        catch (e) {
            window.open(decodeURIComponent(url));
        }
    }
}
//商城业务登录逻辑
function ShangChengLogin() {
    AjaxPostAsync("/AjaxServers/SCLogin.ashx", { action: 'VerifyLogin' }, null, function (data) {
        if (data) {
            if (data != "" && data != "Error") {
                try {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.State) {
                        try {
                            var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + jsonData.Content);
                        }
                        catch (e) {
                            window.open(decodeURIComponent(jsonData.Content));
                        }
                    }
                    else
                    { $.jAlert(jsonData.Msg); }
                }
                catch (e) {
                    alert('商城业务接口返回参数错误！');
                }
            }
        }
    });
}


/*--统计代码(开始)
var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
document.write(unescape("%3Cspan id='cnzz_stat_icon_1253597472' style='display:none;' %3E%3Cscript  src='" + cnzz_protocol + "s23.cnzz.com/z_stat.php%3Fid%3D1253597472' type='text/javascript'%3E%3C/script%3E  %3C/span%3E"));
--统计代码(结束)*/


jQuery.fn.ChooseWorkTag = function (opts, callback) {
    if (opts == null) {
        opts = {};
    }
    opts = jQuery.extend({ //default
        url: '../AjaxServers/TemplateManagement/TagEdit.ashx',
        OpenDialog: function (obj) {
            var this$ = $(this);
            var txt$ = this$.closest('b').siblings();
            $.openPopupLayer({
                name: "TagSelectPopNew",
                parameters: { "val": txt$.attr("did"), "name": $.trim(txt$.val()) },
                url: "../TemplateManagement/TagSelectPopNew.aspx",
                //                url: "TagEdit.aspx",
                beforeClose: function (e, data) {
                    if (e) {

                        txt$.val(data.name);
                        txt$.attr("did", data.val);
                    } else {

                    }
                }
            });
        }

    }, opts);

    return this.each(function () {
        var this$ = $(this);
        this$.autocomplete(opts.url, {
            minChars: 0,
            matchContains: true,
            autoFill: false,
            //dataType: "json",
            extraParams: { action: "GetSimulerWorkOrder" },
            formatItem: function (row, i, max) {
                return row[0];
            },
            formatMatch: function (row, i, max) {
                return row[0];
            },
            formatResult: function (row) {
                return row[0];
            }
        }).result(function (event, row) {
            //$("#txtSelect").attr("did", row[1]);
            this$.attr("did", row[1]);
        });
        this$.siblings().find('a').click(opts.OpenDialog);
    });
};
//左trim
function LTrimChar(s, chars) {
    var i = 0;
    var length = s.length;
    var retval = "";
    for (i = 0; i < length; i++) {
        if (retval == "") {
            if (s.charAt(i) != chars) {
                retval = retval + s.charAt(i);
            }
        }
        else {
            retval = retval + s.charAt(i);
        }
    }
    return retval;
}
//右trim
function RTrimChar(s, chars) {
    var i = 0;
    var length = s.length;
    var retval = "";
    for (i = 0; i < length; i++) {
        if (retval == "") {
            if (s.charAt(length - i - 1) != chars) {
                retval = s.charAt(length - i - 1) + retval;
            }
        }
        else {
            retval = s.charAt(length - i - 1) + retval;
        }
    }
    return retval;
}
//左右trim
function TrimChar(s, chars) {
    s = LTrimChar(s, chars);
    s = RTrimChar(s, chars);
    return s;
}

//跳转会话
function GotoConversation(a, url, userid, csid, orderid) {
    url = decodeURIComponent(url);
    var paras = "{'CSID':'" + $.trim(csid) + "','OrderID':'" + $.trim(orderid) + "','AgentID':'" + userid + "','TimeStamp':'" + new Date().getTime() + "'}";
    AjaxPostAsync("/AjaxServers/CommonHandler.ashx",
        { Action: "EncryptString", EncryptInfo: paras, r: Math.random() },
        null,
        function (data) {
            var href = url + "?data=" + data;
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(href));
            } catch (e) {
                window.open(href);
            }
        });
}

//打开新页面
function OpenNewPageForURL(url) {
    try {
        var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(url));
    }
    catch (e) {
        window.open(url);
    }
}

//话务相关查询逻辑校验
function CheckForSelectCallRecordORIG(beginid, endid) {
    var begintime = $.trim($("#" + beginid).val());
    var endtime = $.trim($("#" + endid).val());
    if (!begintime || begintime == "" || begintime == undefined) {
        $.jAlert("起始时间不能为空", function () { document.getElementById(beginid).focus(); });
        return false;
    }
    else if (!endtime || endtime == "" || endtime == undefined) {
        $.jAlert("终止时间不能为空", function () { document.getElementById(endid).focus(); });
        return false;
    }

    var r = false;
    //同步执行
    AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "CheckForSelectCallRecordORIG", BeginTime: begintime, EndTime: endtime }, null, function (data) {
        var jsonData = $.evalJSON(data);
        if (jsonData && jsonData.success) {
            r = true;
        }
        else if (jsonData) {
            $.jAlert(jsonData.msg, function () { document.getElementById(beginid).focus(); });
            r = false;
        }
    });
    return r;
}

var TelNumManag = CreateTelNumManag();

//热线号码管理类
function CreateTelNumManag() {
    var obj = new Object();
    var skillgroup = CreateSkillGroups();

    obj.TelNumList = new Array();

    //tel, outnum, name, no, skill, datasource, MutilID, AreaCode
    obj.TelNumList.push(CreateTelNum("87237700", "88", "130购车", 9, skillgroup.skill9, 14, 128, "029"));
    obj.TelNumList.push(CreateTelNum("87237799", "94", "167二手车", 7, skillgroup.skill7, 12, 32, "029"));
    obj.TelNumList.push(CreateTelNum("87237756", "98", "168个人", 2, skillgroup.skill2, 2, 2, "029"));
    obj.TelNumList.push(CreateTelNum("87237676", "96", "169金融", 5, skillgroup.skill5, 11, 8, "029"));
    obj.TelNumList.push(CreateTelNum("87237711", "87", "588乐车宝", 10, skillgroup.skill10, 15, 256, "029"));
    obj.TelNumList.push(CreateTelNum("87237788", "97", "591惠买车", 4, skillgroup.skill4, 10, 4, "029"));
    obj.TelNumList.push(CreateTelNum("87237878", "89", "598易鑫", 8, skillgroup.skill8, 13, 64, "029"));
    obj.TelNumList.push(CreateTelNum("87237888", "82", "599惠买车", 13, skillgroup.skill13, 18, 2048, "029"));
    obj.TelNumList.push(CreateTelNum("87237802", "83", "605帮买", 12, skillgroup.skill12, 17, 1024, "029"));
    obj.TelNumList.push(CreateTelNum("87237766", "95", "658易车惠", 6, skillgroup.skill6, 7, 16, "029"));
    obj.TelNumList.push(CreateTelNum("87237777", "99", "719企业", 1, skillgroup.skill1, 1, 1, "029"));
    obj.TelNumList.push(CreateTelNum("87237801", "84", "920帮买", 11, skillgroup.skill11, 16, 512, "029"));

    //测试热线
    obj.TelNumList.push(CreateTelNum("87237722", "86", "7722测试", 100, skillgroup.skill100, 100, 1048576, "029"));
    obj.TelNumList.push(CreateTelNum("68492026", "90", "2026测试", 101, skillgroup.skill101, 101, 2097152, "010"));

    //2015双11临时切换热线（Genesys）
    //    obj.TelNumList.push(CreateTelNum("68210222", "0", "西安个人（Genesys）", -1, skillgroup.skill98, 2, null));
    //    obj.TelNumList.push(CreateTelNum("68211101", "7", "西安惠买车（Genesys）", -1, skillgroup.skill97, 10, null));
    //    obj.TelNumList.push(CreateTelNum("68211168", "5", "西安易车商城（Genesys）", -1, skillgroup.skill96, 7, null));

    //获取option字符串
    obj.GetOptions = function () {
        var str = "";
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (obj.TelNumList[i].No > 0) {
                var selected = "";
                //if (i == 0) {
                //    selected = "selected='selected'";
                //}
                str += "<option style='width:168px' value='" + obj.TelNumList[i].AreaCode + obj.TelNumList[i].Tel + "' MutilID='" + obj.TelNumList[i].MutilID + "'" + selected + ">" + obj.TelNumList[i].Name + "</option>";
            }
        }
        return str;
    }
    //获取checkbox字符串
    obj.GetCheckboxes = function () {
        var str = "";
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (obj.TelNumList[i].No > 0) {
                str += "<span style='width:150px;display:block;float:left;'><input type='checkbox' value='" + obj.TelNumList[i].MutilID + "' name='cb_BussinessTyoe' /><em onclick='emChkIsChoose(this);'>" + obj.TelNumList[i].Name + "</em></span>";
            }
        }
        return str;
    }
    obj.GetNameAndMultyidArr = function () {
        var businessTypeList = new Array();
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (obj.TelNumList[i].No > 0) {
                var businessType = {
                    BusinessName: obj.TelNumList[i].Name, BusinessRightValue: obj.TelNumList[i].MutilID
                };

                businessTypeList.push(businessType);
            }
        }
        return businessTypeList;
    }
    obj.GetOptions_HotLine = function () {
        var str = "";
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (obj.TelNumList[i].No > 0) {
                var selected = "";
                //if (obj.TelNumList[i].No == selectedno) {
                //  selected = "selected='selected'";
                //}
                str += "<option value='" + obj.TelNumList[i].No + "' " + selected + ">" + obj.TelNumList[i].Name + "</option>";
            }
        }
        return str;
    }
    //是否是热线号码
    obj.CheckTelNum = function (tel) {
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (tel == obj.TelNumList[i].AreaCode + obj.TelNumList[i].Tel) {
                return true;
            }
        }
        return false;
    }
    //获取技能组
    obj.GetSkillGroup = function (tel) {
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (tel == obj.TelNumList[i].AreaCode + obj.TelNumList[i].Tel) {
                return obj.TelNumList[i].Skill;
            }
        }
        return null;
    }
    //获取数据来源
    obj.GetDataSource = function (tel) {
        for (var i = 0; i < obj.TelNumList.length; i++) {
            if (tel == obj.TelNumList[i].AreaCode + obj.TelNumList[i].Tel) {
                return obj.TelNumList[i].DataSource;
            }
        }
        return "";
    }
    return obj;
}

//热线号码信息
function CreateTelNum(tel, outnum, name, no, skill, datasource, MutilID, AreaCode) {
    var obj = new Object();
    obj.Tel = tel;
    obj.Out = outnum;
    obj.Name = name;
    obj.No = no;
    //技能组
    obj.Skill = skill;
    obj.DataSource = datasource;
    //多选id
    obj.MutilID = MutilID;
    //区号
    obj.AreaCode = AreaCode;
    return obj;
}
//技能组
function CreateSkillGroups() {
    var obj = new Object();
    obj.skill1 = new Array();
    obj.skill2 = new Array();
    obj.skill3 = new Array();
    obj.skill4 = new Array();
    obj.skill5 = new Array();
    obj.skill6 = new Array();
    obj.skill7 = new Array();
    obj.skill8 = new Array();
    obj.skill9 = new Array();
    obj.skill10 = new Array();
    obj.skill11 = new Array();
    obj.skill12 = new Array();
    obj.skill13 = new Array();

    obj.skill99 = new Array();

    //测试热线
    obj.skill100 = new Array();
    obj.skill101 = new Array();

    //    obj.skill98 = new Array();
    //    obj.skill97 = new Array();
    //    obj.skill96 = new Array();

    //企业
    obj.skill1.push(CreateSkill("QY01", "车易通/微信通会员"));
    obj.skill1.push(CreateSkill("QY02", "淘车通会员"));
    obj.skill1.push(CreateSkill("QY03", "汽车知道顾问"));
    obj.skill1.push(CreateSkill("QY04", "汽车经纪人"));
    obj.skill1.push(CreateSkill("QY05", "商务合作咨询"));
    obj.skill1.push(CreateSkill("QY06", "投诉及建议"));
    obj.skill1.push(CreateSkill("QY07", "IPAD销售助手"));
    obj.skill1.push(CreateSkill("QY08", "奔驰星翼积分计划"));

    //个人
    obj.skill2.push(CreateSkill("GR11", "底价购车咨询"));
    obj.skill2.push(CreateSkill("GR12", "限时抢购"));
    obj.skill2.push(CreateSkill("GR13", "厂商优惠咨询"));
    obj.skill2.push(CreateSkill("GR14", "车型及报价咨询"));
    obj.skill2.push(CreateSkill("GR15", "经销商信息咨询"));
    obj.skill2.push(CreateSkill("GR02", "二手车买卖"));
    obj.skill2.push(CreateSkill("GR03", "汽车贷款"));
    obj.skill2.push(CreateSkill("GR41", "活动咨询"));
    obj.skill2.push(CreateSkill("GR42", "礼品发放咨询"));
    obj.skill2.push(CreateSkill("GR43", "退款咨询"));
    obj.skill2.push(CreateSkill("GR05", "售后保养咨询"));
    obj.skill2.push(CreateSkill("GR06", "商务合作咨询"));
    obj.skill2.push(CreateSkill("GR07", "投诉及建议"));
    obj.skill2.push(CreateSkill("GR08", "其他咨询"));

    //惠买车
    obj.skill4.push(CreateSkill("HMC01", "购车咨询"));
    obj.skill4.push(CreateSkill("HMC02", "礼品发放咨询"));
    obj.skill4.push(CreateSkill("HMC03", "支付退款问题"));
    obj.skill4.push(CreateSkill("HMC04", "账号密码服务"));
    obj.skill4.push(CreateSkill("HMC05", "惠买车合作商家"));
    obj.skill4.push(CreateSkill("HMC06", "商务合作咨询"));
    obj.skill4.push(CreateSkill("HMC07", "投诉及建议"));

    //汽车金融
    obj.skill5.push(CreateSkill("JR01", "贷款购车咨询"));
    obj.skill5.push(CreateSkill("JR02", "抵押贷款咨询"));
    obj.skill5.push(CreateSkill("JR03", "汽车保险咨询"));
    obj.skill5.push(CreateSkill("JR04", "商务合同咨询"));
    obj.skill5.push(CreateSkill("JR05", "投诉及建议"));

    //易车惠
    obj.skill6.push(CreateSkill("SC01", "易车惠车型及活动咨询"));
    obj.skill6.push(CreateSkill("SC02", "礼品发放咨询"));
    obj.skill6.push(CreateSkill("SC03", "支付退款问题"));
    obj.skill6.push(CreateSkill("SC04", "账号密码服务"));
    obj.skill6.push(CreateSkill("SC05", "商务合作咨询"));
    obj.skill6.push(CreateSkill("SC06", "投诉及建议"));
    obj.skill6.push(CreateSkill("SC07", "其他咨询"));

    //易车二手车
    obj.skill7.push(CreateSkill("ESC01", "二手车买卖"));
    obj.skill7.push(CreateSkill("ESC02", "车辆评估"));
    obj.skill7.push(CreateSkill("ESC03", "购车贷款问题"));
    obj.skill7.push(CreateSkill("ESC04", "商务合作"));
    obj.skill7.push(CreateSkill("ESC05", "其他咨询"));
    obj.skill7.push(CreateSkill("ESC06", "投诉及建议"));

    //598易鑫
    obj.skill8.push(CreateSkill("YX01", "提前还款"));
    obj.skill8.push(CreateSkill("YX02", "资料变更"));
    obj.skill8.push(CreateSkill("YX03", "新用户申请"));
    obj.skill8.push(CreateSkill("YX04", "申请进度查询"));
    obj.skill8.push(CreateSkill("YX05", "投诉及建议"));

    //130购车
    obj.skill9.push(CreateSkill("GC01", "乐车购业务咨询"));
    obj.skill9.push(CreateSkill("GC02", "易团购活动咨询"));
    obj.skill9.push(CreateSkill("GC03", "其他活动咨询"));

    //588乐车宝
    obj.skill10.push(CreateSkill("LC01", "售前咨询"));
    obj.skill10.push(CreateSkill("LC02", "发货问题"));
    obj.skill10.push(CreateSkill("LC03", "售后问题"));
    obj.skill10.push(CreateSkill("LC04", "商务合作"));
    obj.skill10.push(CreateSkill("LC05", "投诉建议"));

    //920帮买热线
    obj.skill11.push(CreateSkill("BM101", "人工咨询"));

    //605帮买热线
    obj.skill12.push(CreateSkill("BM201", "人工咨询"));

    //599线索收集
    obj.skill13.push(CreateSkill("LD01", "人工咨询"));

    //合力外呼热线
    obj.skill99.push(CreateSkill("None1", "无技能组"));


    //西安测试热线
    obj.skill100.push(CreateSkill("CS01", "技能1"));
    obj.skill100.push(CreateSkill("CS02", "技能2"));
    obj.skill100.push(CreateSkill("CS03", "技能3"));
    obj.skill100.push(CreateSkill("CS04", "技能4"));
    obj.skill100.push(CreateSkill("CS05", "技能5"));
    //北京测试热线
    obj.skill101.push(CreateSkill("BJ01", "技能1"));
    obj.skill101.push(CreateSkill("BJ02", "技能2"));
    obj.skill101.push(CreateSkill("BJ03", "技能3"));
    obj.skill101.push(CreateSkill("BJ04", "技能4"));

    //2015双11迁移Genesys热线（个人）
    //    obj.skill98.push(CreateSkill("GR41", "活动咨询"));
    //    obj.skill98.push(CreateSkill("GR03", "汽车贷款"));
    //    obj.skill98.push(CreateSkill("GR12", "网上直销咨询"));
    //    obj.skill98.push(CreateSkill("GR11", "底价购车咨询"));
    //    obj.skill98.push(CreateSkill("GR02", "二手车买卖"));
    //    obj.skill98.push(CreateSkill("GR08", "其他咨询"));

    //2015双11迁移Genesys热线（惠买车）
    //    obj.skill97.push(CreateSkill("HMC01", "购车咨询"));
    //    obj.skill97.push(CreateSkill("HMC02", "礼品发放咨询"));
    //    obj.skill97.push(CreateSkill("HMC03", "支付退款问题"));
    //    obj.skill97.push(CreateSkill("HMC04", "账号密码服务"));
    //    obj.skill97.push(CreateSkill("HMC05", "惠买车合作商家"));
    //    obj.skill97.push(CreateSkill("HMC06", "商务合作咨询"));
    //    obj.skill97.push(CreateSkill("HMC07", "投诉及建议"));

    //2015双11迁移Genesys热线（商城）
    //    obj.skill96.push(CreateSkill("SC01", "商城车型及活动咨询"));
    //    obj.skill96.push(CreateSkill("SC02", "易车惠活动咨询"));
    //    obj.skill96.push(CreateSkill("SC03", "礼品发放咨询"));
    //    obj.skill96.push(CreateSkill("SC04", "支付退款问题"));
    //    obj.skill96.push(CreateSkill("SC05", "账号密码服务"));
    //    obj.skill96.push(CreateSkill("SC06", "商务合作咨询"));
    //    obj.skill96.push(CreateSkill("SC07", "投诉及建议"));

    return obj;
}
//技能
function CreateSkill(id, name) {
    var obj = new Object();
    obj.id = id;
    obj.name = name;
    return obj;
}


/*
扩展JQuery方法，bindTextDefaultMsg初始化文本默认值功能
若要给文本框赋值时，需要先赋值，后执行初始化默认值的脚本
Add=masj,Date=2016-07-21    
*/
jQuery.fn.extend({
    bindTextDefaultMsg: function (msg) {
        var b = getBrowser();
        return this.each(function () {
            if (b.ie && (
                    b.ie == '7.0' ||
            		b.ie == '8.0' ||
            		b.ie == '9.0' ||
                    b.ie == '10.0' ||
                    b.ie == '11.0')) {
                var labelObj = $("<label class='u-label f_dn'>" + msg + "</label>").show();
                var item = $(this);
                labelObj.bind('click', function () {
                    item.focus().trigger('click');
                });
                if ($(this).prev('label.u-label.f_dn').size() > 0) {
                    $(this).prev('label.u-label.f_dn').remove();
                }
                $(this).before(labelObj)
                    .unbind('input propertychange focus blur')
                    .bind('input propertychange focus blur', function () {
                        if ($(this).val() != '') {
                            $(this).prev('label').hide();
                        }
                        else {
                            $(this).prev('label').show();
                        }
                    });
            }
            else {
                $(this).attr('placeholder', msg);
            }
            if ($(this).val() != '') {
                $(this).prev('label').hide();
            }
        });


        //获得当前客户端浏览器及版本
        function getBrowser() {
            var Sys = {};
            var ua = navigator.userAgent.toLowerCase();
            var s;
            (s = ua.match(/rv:([\d.]+)\) like gecko/)) ? Sys.ie = s[1] :
                (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
                (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
                (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
                (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
                (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
            return Sys;
        }
    }
});


//文本框长度限制
function InitInputMaxLength(textboxid, len) {
    var textbox = $("#" + textboxid);
    if (textbox.length > 0) {
        textbox.keyup(function () {
            var val = textbox.val();
            if (val.length > len) {
                textbox.val(val.substr(0, len));
            }
        });
    }
}
//文本框限制长度和类型
function InitInputMaxLengthForDigit(textboxid, len) {
    var textbox = $("#" + textboxid);
    if (textbox.length > 0) {
        textbox.keyup(function () {
            var val = textbox.val();
            var newval = "";
            $(val.split("")).each(function (i, n) {
                if (!isNaN(n) && n != " ") {
                    newval += n;
                }
            });
            if (newval.length > len) {
                textbox.val(newval.substr(0, len));
            }
            else if (val != newval) {
                textbox.val(newval);
            }
        });
    }
}