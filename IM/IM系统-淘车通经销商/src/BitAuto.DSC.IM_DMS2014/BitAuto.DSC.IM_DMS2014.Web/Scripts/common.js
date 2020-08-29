var domainCookie = 'im.sys.bitauto.com';
//消息闪烁相关
// 使用message对象封装消息
var messageFlicker = {
    time: 0,
    title: document.title,
    timer: null,

    // 显示新消息提示   
    show: function () {
        var title = messageFlicker.title.replace("【　　　】", "").replace("【新消息】", "");
        // 定时器，设置消息切换频率闪烁效果就此产生   
        messageFlicker.timer = setTimeout(
                function () {
                    messageFlicker.time++;
                    messageFlicker.show();

                    if (messageFlicker.time % 2 == 0) {
                        document.title = "【新消息】" + title
                    }
                    else {
                        document.title = "【　　　】" + title
                    };
                },
                600 // 闪烁时间差  
            );
        return [messageFlicker.timer, messageFlicker.title];
    },

    // 取消新消息提示   
    clear: function (name) {
        clearTimeout(messageFlicker.timer);
        document.title = (name == null ? "在线客服" : name);
    }
};
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
    return (/^(?:13\d|15\d|18\d|19\d|14\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
}
//电话验证
function isTel(tel) {
    return (/^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$/.test(tel));
}
//电话或者手机验证
function isTelOrMobile(s) {
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
            alert(XMLHttpRequest.responseText);
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
            //alert(XMLHttpRequest.responseText);
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
            alert(XMLHttpRequest.responseText);
        }
    });
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
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
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
         if ($.trim(strJson) == '')
             return '';
         else
             return eval("(" + strJson + ")");
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
//function addLoadEvent(func) {
//    var oldonload = window.onload;
//    if (typeof window.onload != 'function') {
//        window.onload = func;
//    } else {
//        window.onload = function () {
//            oldonload();
//            func();
//        }
//    }
//}

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
//addLoadEvent(all_login_box);
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
*/
(function ($) {
    $.fn.jqDrag = function (h) { return i(this, h, 'd'); };
    $.fn.jqResize = function (h) { return i(this, h, 'r'); };
    $.jqDnR = { dnr: {}, e: 0,
        drag: function (v) {
            if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: M.Y + v.pageY - M.pY });
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
function LoadingAnimation(eleId) {
    jQuery('#' + eleId).html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
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
    //document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=;path=/;';
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
    //if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";domain=;path=/;";
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

function getHoursMinute() {
    var date = "";
    var jsDate = new Date();
    if (jsDate.getHours() < 10) {
        date = date + "0" + jsDate.getHours() + ":";
    } else {
        date = date + jsDate.getHours() + ":";
    }
    if (jsDate.getMinutes() < 10) {
        date = date + "0" + jsDate.getMinutes() + ":";
    } else {
        date = date + jsDate.getMinutes() + ":";
    }
    if (jsDate.getSeconds() < 10) {
        date = date + "0" + jsDate.getSeconds() + "";
    } else {
        date = date + jsDate.getSeconds() + "";
    } //
    return date;
}

function getHoursMinute2(ttDate) {


    var date = "";
    var jsDate = null;

    if ((typeof ttDate) == "object") {
        jsDate = ttDate;
    } else if ((typeof ttDate) == "string") {
        jsDate = new Date(ttDate.replace(/-/g, "/"));
    }

    if (jsDate.getHours() < 10) {
        date = date + "0" + jsDate.getHours() + ":";
    } else {
        date = date + jsDate.getHours() + ":";
    }
    if (jsDate.getMinutes() < 10) {
        date = date + "0" + jsDate.getMinutes() + ":";
    } else {
        date = date + jsDate.getMinutes() + ":";
    }
    if (jsDate.getSeconds() < 10) {
        date = date + "0" + jsDate.getSeconds() + "";
    } else {
        date = date + jsDate.getSeconds() + "";
    } //
    return date;
}
//比较后以取到时间
//返回值毫秒：ttDate2-ttDate1
function CompareDateTime(ttDate1, ttDate2) {
    ttDate1 = ttDate1.replace(/-/g, "/");
    ttDate2 = ttDate2.replace(/-/g, "/");
    var jsDate1 = new Date(ttDate1);
    var jsDate2 = new Date(ttDate2);
    var millsecs = 0;
    millsecs = Date.parse(jsDate2) - Date.parse(jsDate1);

    return millsecs;
}

//格式化html中的特殊字符
function FormatSpecialCharacters(msgstr) {
    msgstr = msgstr.replace(/</g, "&lt;");
    msgstr = msgstr.replace(/>/g, "&gt;");
    msgstr = msgstr.replace(/\r/g, "<br>");
    msgstr = msgstr.replace(/\n/g, "<br>");
    //msgstr = msgstr.replace(/ /g, "&nbsp;");
    //msgstr = msgstr.replace(/\&/g, "&amp;");
    msgstr = msgstr.replace(/\+/g, "&#43;");

    return msgstr;
}

//正则表达式替换文本中的url为链接
function replaceRegUrl(str) {
    var reg = /http:\/\/[\w-]*(\.[\w-]*)+/ig;
    return str.replace(reg, function (m) { return '<a href="' + m + '" target="blank">' + m + '</a>'; })
}

//分页控件中跳转使用方法

//校验文本域页码是否正确
function CheckPageNum(txt, max) {
    var v = parseInt($.trim(txt.value));
    if (isNaN(v)) {
        txt.value = "";
    }
    else {
        if (v < 1)
            v = 1;
        if (v > max)
            v = max;
        txt.value = v;
    }
}
//跳转
function GoToPageForInput(txtid, gofun, para) {
    var txt = document.getElementById(txtid);
    var v = parseInt($.trim(txt.value));
    if (isNaN(v)) {
        return;
    }
    else {
        gofun(para + "page=" + v);
    }
}
//回车跳转
function EnterPressGoTo(txtid, gofun, para, max) {
    var e = window.event;
    if (e.keyCode == 13) {
        CheckPageNum(document.getElementById(txtid), max);
        GoToPageForInput(txtid, gofun, para);
    }
}

//去掉html标签，保留img，a 标签
function HtmlReplacehaveImgA(description) {
    description = description.replace(/(\n)/g, "");
    description = description.replace(/(\t)/g, "");
    description = description.replace(/<(?!\/?a|\/?IMG)[^<>]*>/ig, "");
    return description;
}


/*--统计代码(开始)
var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
document.write(unescape("%3Cspan id='cnzz_stat_icon_1254803534' style='display:none;' %3E%3Cscript  src='" + cnzz_protocol + "s23.cnzz.com/z_stat.php%3Fid%3D1254803534' type='text/javascript'%3E%3C/script%3E  %3C/span%3E"));
--统计代码(结束)*/