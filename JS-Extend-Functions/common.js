﻿function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function() {
            oldonload();
            func();
        }
    }
}

/*=======================tab=============================*/
function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }
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

function tabsRemove(index, head, divs, div2s) {
    if (!document.getElementById(head)) return false;
    var tab_heads = document.getElementById(head);
    if (tab_heads) {
        var alis = tab_heads.getElementsByTagName("li");
        for (var i = 0; i < alis.length; i++) {
            removeClass(alis[i], "current");

            hide(divs + "_" + i);
            if (div2s) { hide(div2s + "_" + i) };

            if (i == index) {
                addClass(alis[i], "current");
            }
        }

        show(divs + "_" + index);
        if (div2s) { show(div2s + "_" + index) };
    }
}

function tabs(head, divs, div2s, over) {
    if (!document.getElementById(head)) return false;
    var tab_heads = document.getElementById(head);

    if (tab_heads) {
        var alis = tab_heads.getElementsByTagName("li");
        for (var i = 0; i < alis.length; i++) {
            alis[i].num = i;


            if (over) {
                alis[i].onmouseover = function() {
                    changetab(this);
                }

            }
            else {
                alis[i].onclick = function() {
                    if (this.className == "current" || this.className == "last current") {
                        changetab(this);
                        return true;
                    }
                    else {
                        changetab(this);
                        return false;
                    }

                }
            }

            function changetab(thebox) {
                tabsRemove(thebox.num, head, divs, div2s);
            }

        }
    }
}


function all_func() {
    //breakul('break_shangjia',5);
    tabs("IDtab1", "IDbox1", null, true);
    tabs("IDtab2", "IDbox2", null, true);
    tabs("IDtab3", "IDbox3", null, true);
    tabs("IDtab4", "IDbox4", null, true);
    tabs("IDtab5", "IDbox5", null, true);
}

addLoadEvent(all_func)




/*====================================================*/

function validateDel(ids) {
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
        return confirm("确定要删除吗?");

    }
    else {
        alert("请至少选择一项!");
        return false;
    }
}

//全选反选取消
function selectCheckBoxDelAll(objName, showType) {
    var delAllObj = document.getElementsByName(objName);
    if (showType == 1) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            if (!delAllObj[i].disabled) {
                delAllObj[i].checked = true;
            }
        }
    }
    else if (showType == 2) {
        //反选
        for (var i = 0; i < delAllObj.length; i++) {
            if (!delAllObj[i].disabled) {
                if (delAllObj[i].checked) {
                    delAllObj[i].checked = false;
                }
                else {
                    delAllObj[i].checked = true;
                }
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
String.prototype.isDate = function() {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false; var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
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
//trim
function trim(s) {
    return s.replace(/(^[\s\u3000]*)|([\s\u3000]*$)/g, "");
}
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

//状态标签点击切换函数
function Show_TabADSMenu(tabadid_num, tabadnum) {
    for (var i = 0; i < 2; i++) { document.getElementById("tabadcontent_" + tabadid_num + i).style.display = "none"; }
    for (var i = 0; i < 2; i++) { document.getElementById("tabadmenu_" + tabadid_num + i).className = ""; }
    document.getElementById("tabadmenu_" + tabadid_num + tabadnum).className = "linknow";
    document.getElementById("tabadcontent_" + tabadid_num + tabadnum).style.display = "block";
}

//手机验证
function isMobile(mobile) {
    return (/^(?:13\d|15\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
}
//电话验证
function isTel(tel) {
    return (/^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/.test(tel));
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
    GetDateLayer: function() {
        if (window.parent) {
            return window.parent.L_DateLayer;
        }
        else { return window.L_DateLayer; }
    },
    L_TheYear: new Date().getFullYear(), //定义年的变量的初始值
    L_TheMonth: new Date().getMonth() + 1, //定义月的变量的初始值
    L_WDay: new Array(39), //定义写日期的数组
    MonHead: new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31),    		   //定义阳历中每个月的最大天数
    GetY: function() {
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
    GetX: function() {
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
    CreateHTML: function() {
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
    InsertHTML: function(id, htmlstr) {
        var L_DateLayer = this.GetDateLayer();
        if (L_DateLayer) { L_DateLayer.document.getElementById(id).innerHTML = htmlstr; }
    },
    WriteHead: function(yy, mm)  //往 head 中写入当前的年与月
    {
        this.InsertHTML("L_calendar-year", yy + " 年");
        this.InsertHTML("L_calendar-month", mm + " 月");
    },
    IsPinYear: function(year)            //判断是否闰平年
    {
        if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0))) return true; else return false;
    },
    GetMonthCount: function(year, month)  //闰年二月为29天
    {
        var c = this.MonHead[month - 1]; if ((month == 2) && this.IsPinYear(year)) c++; return c;
    },
    GetDOW: function(day, month, year)     //求某天的星期几
    {
        var dt = new Date(year, month - 1, day).getDay() / 7; return dt;
    },
    GetText: function(obj) {
        if (obj.innerText) { return obj.innerText }
        else { return obj.textContent }
    },
    PrevM: function()  //往前翻月份
    {
        if (this.L_TheMonth > 1) { this.L_TheMonth-- } else { this.L_TheYear--; this.L_TheMonth = 12; }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    NextM: function()  //往后翻月份
    {
        if (this.L_TheMonth == 12) { this.L_TheYear++; this.L_TheMonth = 1 } else { this.L_TheMonth++ }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    Today: function()  //Today Button
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
    SetDay: function(yy, mm)   //主要的写程序**********
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
    SelectYearInnerHTML: function(strYear) //年份的下拉框
    {
        if (strYear.match(/\D/) != null) { alert("年份输入参数不是数字！"); return; }
        var m = (strYear) ? strYear : new Date().getFullYear();
        if (m < 1000 || m > 9999) { alert("年份值不在 1000 到 9999 之间！"); return; }
        var n = m - 10;
        if (n < 1000) n = 1000;
        if (n + 26 > 9999) n = 9974;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectYear\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectYearLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectYearLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheYear = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = n; i < n + 26; i++) {
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
    SelectMonthInnerHTML: function(strMonth) //月份的下拉框
    {
        if (strMonth.match(/\D/) != null) { alert("月份输入参数不是数字！"); return; }
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
    DayClick: function(mm, dd)  //点击显示框选取日期，主输入函数*************
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
            if (MyCalendar && MyCalendar.onDayClick) { MyCalendar.onDayClick(this.InputObject); }
            this.CloseLayer();
        }
        else { this.CloseLayer(); alert("您所要输出的控件对象并不存在！"); }
    },
    SetDate: function() {
        if (arguments.length < 1) { alert("对不起！传入参数太少！"); return; }
        else if (arguments.length > 2) { alert("对不起！传入参数太多！"); return; }
        this.InputObject = (arguments.length == 1) ? arguments[0] : arguments[1];
        this.ClickObject = arguments[0];
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
    CloseLayer: function() {
        try {
            var DateLayer = document.getElementById("L_DateLayer");
            if ((DateLayer.style.display == "" || DateLayer.style.display == "block") && arguments[0] != this.ClickObject && arguments[0] != this.InputObject) {
                DateLayer.style.display = "none";
            }
        }
        catch (e) { }
    }
}

document.writeln('<iframe id="L_DateLayer" name="L_DateLayer" frameborder="0" style="position:absolute;width:160px; height:190px;overflow:hidden;z-index:100099;display:none;backgorund-color:transparent;"></iframe>');
var MyCalendar = new L_calendar();
MyCalendar.NewName = "MyCalendar";
document.onclick = function(e) {
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
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
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
        error: function(XMLHttpRequest, textStatus, errorThrown) {
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
        masterObj.options.length = 0;
        masterObj.options[0] = new Option("省/直辖市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            masterObj.options[masterObj.options.length] = new Option(JSonData.masterArea[i].name, JSonData.masterArea[i].id);
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
    var masterObjid = document.getElementById(provinceSelectID).options[document.getElementById(provinceSelectID).selectedIndex].value;
    if (masterObjid && masterObjid > 0) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("市/县", -1);
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
        subAreaObj.options[subAreaObj.options.length] = new Option("市/县", -1);
    }
}

//绑定区域值
function BindProvinceAndCity(selectProvinceId, selectCityId, provinceId, cityId) {
    jQuery('#' + selectProvinceId).val(provinceId);
    BindCity(selectProvinceId, selectCityId);
    jQuery('#' + selectCityId).val(cityId);
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
    $.each(arrayObject, function(name, value) {
        selectObject.options[selectObject.options.length] = new Option(value[0], value[1]);
    });
}

//将select的值写入隐藏域（用于ajax的select刷新时保留值）
function _BindSelectValue(selectId, hiddenId, clearId) {
    var selectObj = jQuery('#' + selectId);
    if (selectObj) {
        var hidden = selectObj.next('#' + hiddenId);
        if (hidden.length > 0) {
            hidden.val(selectObj.val());
            if (clearId) { jQuery('#' + clearId).val(''); }
        }
    }
}
//将隐藏域的值绑定到select（用于ajax的select刷新时保留值）
function _RebindSelectValue(selectId, hiddenId) {
    var selectObj = jQuery('#' + selectId);
    if (selectObj) {
        var hidden = selectObj.next('#' + hiddenId);
        if (hidden.length > 0) { selectObj.val(hidden.val()); }
    }
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
     evalJSON: function(strJson) {
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
    toJSONstring: function(object) {
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
                      function() {
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

//弹出伙伴代理品牌DIV层
function openFriendAgentBrandAjaxPopup(hidAgentBrandIDs, txtAgentBrandNames) {
    $.openPopupLayer({
        name: 'FriendAgentBrandAjaxPopup',
        url: "../BaseDataManager/SelectAgentBrand.aspx?page=1",
        beforeClose: function() {
            var brandids = $('#popupLayer_' + 'FriendAgentBrandAjaxPopup').data('brandids');
            var brandnames = $('#popupLayer_' + 'FriendAgentBrandAjaxPopup').data('brandnames');
            if (typeof (brandids) != "undefined") {
                //alert(brandids);
                $('#' + hidAgentBrandIDs).val(brandids);
                $('#' + txtAgentBrandNames).val(brandnames);
            }
        }
    });
}
//弹出选择单位DIV层(伙伴)
function openComanyListAjaxPopup(hidCodes, txtNames) {
    $.openPopupLayer({
        name: 'SelectCompanyListAjaxPopup',
        parameters: { editcode: $('#' + hidCodes).val() },
        url: "../AjaxServer/BaseDataManager/ComanyList.aspx?page=1",
        beforeClose: function() {
            var code = $('#popupLayer_' + 'SelectCompanyListAjaxPopup').data('code');
            var name = $('#popupLayer_' + 'SelectCompanyListAjaxPopup').data('name');
            if (typeof (code) != "undefined") {
                $('#' + hidCodes).val(code);
                $('#' + txtNames).val(name);
            }
        }
    });
}
//弹出选择单位DIV层(供应商)
function openSupplierAjaxPopup(hidCodes, txtNames) {
    $.openPopupLayer({
        name: 'SelectCompanyListAjaxPopup',
        parameters: { editcode: $('#' + hidCodes).val() },
        url: "../AjaxServer/BaseDataManager/SupplierList.aspx?page=1",
        beforeClose: function() {
            var code = $('#popupLayer_' + 'SelectCompanyListAjaxPopup').data('code');
            var name = $('#popupLayer_' + 'SelectCompanyListAjaxPopup').data('name');
            if (typeof (code) != "undefined") {
                $('#' + hidCodes).val(code);
                $('#' + txtNames).val(name);
            }
        }
    });
}
//弹出伙伴上级单位DIV层
function openSelectSuperiorFriendAjaxPopup(hidIDs, txtNames, friendid) {
    $.openPopupLayer({
        name: 'SuperiorFriendAjaxPopup',
        parameters: { selffriendid: friendid },
        url: "../AjaxServers/FriendManager/SelectSuperiorFriend.aspx?page=1",
        beforeClose: function() {
            var friendid = $('#popupLayer_' + 'SuperiorFriendAjaxPopup').data('friendid');
            var friendname = $('#popupLayer_' + 'SuperiorFriendAjaxPopup').data('friendname');
            if (typeof (friendid) != "undefined") {
                //alert(brandids);
                $('#' + hidIDs).val(friendid);
                $('#' + txtNames).val(friendname);
            }
        }
    });
}
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

//设置表格样式
function SetTableStyle(tableid) {
    //$('#'+tableid+' tr:even').addClass('color_hui');//设置列表行样式
    $('#' + tableid + ' tr').removeData('currentcolor');
    $('#' + tableid + ' tr').mouseover(function() {
        if (!($(this).data('currentcolor')))
            $(this).data('currentcolor', $(this).css('backgroundColor'));
        $(this).css('backgroundColor', '#e5edf1').css('fontWeight', '');
    }).mouseout(function() {
        $(this).css('backgroundColor', $(this).data('currentcolor')).css('fontWeight', '');
    });

}
//判断当前季度
function GetQuarter(ddl) {
    var myDate = new Date();
    var m = myDate.getMonth() + 1;
    $('#' + ddl).val(Math.ceil(m / 3));
}
//重置
function resetForm(id) {
    jQuery('#' + id).each(function() {
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

/**
* @desc  选择厂商大品牌联动销售网络和主营品牌
* @param 字符串
* @return 返回字符串长度
* @Add=fangxc, Date: 2009-12-16
*/
function BindSaleNetwork(ddlProducerID, ddlSaleNetworkID, ddlBrandID, SaleNetworkID, BrandID) {
    var ddlProducer = document.getElementById(ddlProducerID);
    var ddlSaleNetwork = document.getElementById(ddlSaleNetworkID);
    var ddlBrand = document.getElementById(ddlBrandID);
    if (ddlProducer && ddlSaleNetwork) {
        ddlSaleNetwork.options.length = 0;
        ddlSaleNetwork.options[0] = new Option("请选择", -1);
        if (ddlBrand) {
            ddlBrand.options.length = 0;
            ddlBrand.options[0] = new Option("请选择", -1);
        }
        if (ddlProducer.value != "-1") {
            var pody = 'BindSaleNetwork=yes&producerID=' + ddlProducer.value;
            AjaxPost('/AjaxServers/BaseDataManager/BaseDataBind.ashx', pody, null, function(data) {
                var jsonData = $.evalJSON(data);
                if ($.trim(jsonData.text) != "") {
                    var textList = jsonData.text.split('|');
                    var valueList = jsonData.value.split('|');
                    for (var i = 0; i < textList.length; i++) {
                        ddlSaleNetwork.options[i + 1] = new Option(textList[i], valueList[i]);
                    }
                    if (SaleNetworkID && SaleNetworkID != '-1') {
                        ddlSaleNetwork.value = SaleNetworkID;
                        if (ddlBrandID) {
                            BindBrand(ddlSaleNetworkID, ddlBrandID, BrandID)
                        }
                    }
                }
            });
        }
    }
}
/**
* @desc  选择销售网络联动主营品牌
* @param 字符串
* @return 返回字符串长度
* @Add=fangxc, Date: 2009-12-16
*/
function BindBrand(ddlSaleNetworkID, ddlBrandID, BrandID) {
    var ddlSaleNetwork = document.getElementById(ddlSaleNetworkID);
    var ddlBrand = document.getElementById(ddlBrandID);
    if (ddlSaleNetwork && ddlBrand) {

        ddlBrand.options.length = 0;
        ddlBrand.options[0] = new Option("请选择", -1);
        if (ddlSaleNetwork.value != "-1") {
            var pody = 'BindBrand=yes&snid=' + ddlSaleNetwork.value;
            AjaxPost('/AjaxServers/BaseDataManager/BaseDataBind.ashx', pody, null, function(data) {
                var jsonData = $.evalJSON(data);
                if ($.trim(jsonData.text) != "") {
                    var textList = jsonData.text.split('|');
                    var valueList = jsonData.value.split('|');
                    for (var i = 0; i < textList.length; i++) {
                        ddlBrand.options[i + 1] = new Option(textList[i], valueList[i]);
                    }
                    if (BrandID && BrandID != '-1') {
                        ddlBrand.value = BrandID;
                    }
                }
            });
        }
    }
}
function testasdf() {
    alert("ok");
}
function RefreshPage() {
    window.location.reload();
}
/*头部鼠标划过出现下拉层star*/
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function() {
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
    btli.onmouseover = function() {
        addClass(btpop, add_Class)
    }
    btli.onmouseout = function() {
        removeClass(btpop, add_Class)
    }
}
function all_login_box() {
    bt_login_more('goOther', 'goOtherContent', 'pop_block');
}
addLoadEvent(all_login_box);
/*头部鼠标划过出现下拉层end*/


/*点击更多显示，点击收起隐藏star*/
function showMore() {
    document.getElementById("more").style.display = "block";
}
function hideMore() {
    document.getElementById("more").style.display = "none";
}
/*点击更多显示，点击收起隐藏end*/



/*节选自jQueryString v2.0.2，将查询串转换成查询对象*/
(function($) {
    $.unserialise = function(Data) {
        var Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function() {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };
})(jQuery);


/*设置透明度，兼容IE和FF*/
; (function($) {
    $.freeOpacity = {
        main: function(opacity) {
            this.each(function(i) {
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
(function($) {
    $.fn.jqDrag = function(h) { return i(this, h, 'd'); };
    $.fn.jqResize = function(h) { return i(this, h, 'r'); };
    $.jqDnR = { dnr: {}, e: 0,
        drag: function(v) {
            if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: M.Y + v.pageY - M.pY });
            else E.css({ width: Math.max(v.pageX - M.pX + M.W, 0), height: Math.max(v.pageY - M.pY + M.H, 0) });
            return false;
        },
        stop: function(h) {/*E.opacity(M.o);*/
            if (h[0].releaseCapture) { h[0].releaseCapture(); } //取消捕获范围
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $().unbind('mousemove', J.drag).unbind('mouseup', J.stop);
        }
    };
    var J = $.jqDnR, M = J.dnr, E = J.e,
i = function(e, h, k) {
    return e.each(function() {
        h = (h) ? $(h, e) : e;
        h.bind('mouseover', function() { $(this).css('cursor', 'move'); })
 .bind('mouseout', function() { $(this).css('cursor', 'auto'); });
        h.bind('mousedown', { e: e, k: k }, function(v) {
            var d = v.data, p = {}; E = d.e;
            // attempt utilization of dimensions plugin to fix IE issues
            if (E.css('position') != 'relative') { try { E.position(p); } catch (e) { } }
            M = { X: p.left || f('left') || 0, Y: p.top || f('top') || 0, W: f('width') || E[0].scrollWidth || 0, H: f('height') || E[0].scrollHeight || 0, pX: v.pageX, pY: v.pageY, k: d.k, o: E.css('opacity') };
            /*E.opacity(0.8);*/
            //设置捕获范围
            if (h[0].setCapture) { h[0].setCapture(); }
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $().mousemove($.jqDnR.drag).mouseup(function() {
                $.jqDnR.stop(h);
            });
            return false;
        });
    });
},
f = function(k) { return parseInt(E.css(k)) || false; };
})(jQuery);


//载入时的动画. eleId为容器ID
function LoadingAnimation(eleId) {
    jQuery('#' + eleId).html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
}
//载入时的动画. eleId为容器ID 自动拆分中使用
function LoadingAnimationSplit(eleId) {
    jQuery('#' + eleId).html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在拆分中...</div></div>');
}
//将服务器端返回的表示Table的JSON串转换成JSON对象
function TableJson2Json(tableJson) {
    if (tableJson.result) {
        var result = JSON.parse(tableJson.result);
        if (result.Data) {
            result.Data = JSON.parse(result.Data);
            tableJson.result = result;
            return tableJson;
        }
        else { return false; }
    }
    else { return false; }
}


//验证是否为数字，且小数点后最多保留2位小数，可以为负数
function isNum(s) {
    var pattern = /^([+-]{0,1})\d+(\.[0-9]{0,2})?$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
function Getdecimal(str, len) {
    if (len < 0) {
        alert(str);
    }
    str = str + '';
    var index = (str + '').indexOf(".");
    if (index == -1) {
        if (len !== 0) {
            str = str + ".";
            for (var i = 0; i < len; i++) {
                str = str + "0";
            }
        }
        else {
        }
    }
    else {
        var length = str.length;
        if (length - index < len + 2) {
            for (var j = 0; j < (len + 1 - length + index); j++) {
                str = str + "0";
            }
        }
        else {
            if (str.substr(index + len + 1, 1) > 4) {
                var additional = 1;
                for (var k = 0; k < len; k++) {
                    additional = additional * 10;
                }
                str = (Number(str.substr(0, index + len + 1)) * additional + 1) / additional;
                str = str + "";
                index = str.indexOf(".");
                if (index == -1) {
                    if (len != 0) {
                        str = str + ".";
                        for (i = 0; i < len; i++) {
                            str = str + "0";
                        }
                    }
                }
                else {
                    length = str.length;
                    if (length - index < len + 2) {
                        for (j = 0; j < (len + 1 - length + index); j++) {
                            str = str + "0";
                        }
                    }
                    else {
                        str = str.substr(0, index + len + 1);
                    }
                }

            }
            else {
                str = str.substr(0, index + len + 1);
            }
        }
    }

    return TrimChar(str, '.');
}

//将浮点型数字，转换为货币格式
function ToMoney(s) {
    if (!/^([+-]{0,1})[0-9\.]/.test(s)) return "invalid value";
    s = s.replace(/^(\d*)$/, "$1.");
    s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    s = s.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(s))
        s = s.replace(re, "$1,$2");
    s = s.replace(/,(\d\d)$/, ".$1");
    return "￥" + s.replace(/^\./, "0.")
}

//将货币类型转换为数字类型
function ToDecimal(str) {
    return str.replace('。', '.').replace('￥', '').replace('，', '').replace(/,/g, '');
}

//将一块区域覆盖
function CoverArea(ele, coverId, txt) {
    var source = jQuery(ele);
    if (source.size() <= 0) { return; }
    if (!coverId) { coverId = '_coverAreaID' }
    if (!txt) { txt = ''; }
    var z = source.css('z-index'); //"z-index"
    if (z == 'auto') { z = 10000; }
    var c = jQuery("<div/>").attr('id', coverId).appendTo(source).css({
        'z-index': z + 1,
        background: 'gray',
        position: 'absolute',
        left: source.offset().left,
        top: source.offset().top,
        height: source.height(),
        width: source.width()
    }).opacity(0.5).html('<div class="blue-loading" style="width:100%;text-align:center;background-position:right;">' + txt + '</div>');
    return c;
}

/**
* @desc  根据字典类型绑定下拉列表
* @Add=fangxc, Date: 2010-04-08
*/
function BindDict(type, ddlControlID) {
    var ddlControl = document.getElementById(ddlControlID);
    if (ddlControl) {
        ddlControl.options.length = 0;
        ddlControl.options[0] = new Option("请选择", -1);
        var pody = 'type=' + type;
        AjaxPost('/AjaxServers/BaseDataManager/dictinfo.ashx', pody, null, function(data) {
            var jsonData = $.evalJSON(data);
            if ($.trim(jsonData.text) != "") {
                var textList = jsonData.text.split('|');
                var valueList = jsonData.value.split('|');
                for (var i = 0; i < textList.length; i++) {
                    ddlControl.options[i + 1] = new Option(textList[i], valueList[i]);
                }
            }
        });
    }
}
/**
* 根据字典生成数组
* @Add=xings, Date: 2010-04-09
*/
function BindDictData(type, ckxName, divID, inputType) {
    var pody = 'type=' + type;
    AjaxPost('/AjaxServers/BaseDataManager/dictinfo.ashx', pody, null, function(data) {
        var jsonData = $.evalJSON(data);
        if ($.trim(jsonData.text) != "") {
            var textList = jsonData.text.split('|');
            var valueList = jsonData.value.split('|');
            var myArray = new Array();
            for (var i = 0; i < textList.length; i++) {
                myArray[i] = new Array(textList[i], valueList[i]);
            }

            $.each(myArray, function(i, array) {
                var ckxID = ckxName + (i + 1);
                if (inputType == 'checkbox') {
                    $('#' + divID).append('<span><label for="' + ckxID + '" style="width:auto;"><input class="noborder" id="' + ckxID + '" name="' + ckxID + '" type="' + inputType + '" value="' + array[1] + '" /><em style="color:black;">' + array[0] + '</em></label></span>');
                }
                else if (inputType == 'radio') {
                    $('#' + divID).append('<span><input name="' + ckxName + '" type="' + inputType + '" value="' + array[1] + '"' + (i == 0 ? ' checked="checked" ' : '') + ' /><em style="color:black;">' + array[0] + '</em></span>');
                }
            });
        }
    });
}
//验证URL 正则表达式
function IsURL(str_url) {
    var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
    + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@ 
          + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184 
          + "|" // 允许IP和DOMAIN（域名）
          + "([0-9a-z_!~*'()-]+\.)*" // 域名- www. 
          + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名 
          + "[a-z]{2,6})" // first level domain- .com or .museum 
          + "(:[0-9]{1,4})?" // 端口- :80 
          + "((/?)|" // a slash isn't required if there is no file name 
          + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}

//除法函数，用来得到精确的除法结果
//说明：javascript的除法结果会有误差，在两个浮点数相除的时候会比较明显。这个函数返回较为 精确的除法结果。
//返回值：arg1除以arg2的精确结果
function AccDivide(arg1, arg2) {
    var t1 = 0, t2 = 0, r1, r2;
    try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
    try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
    with (Math) {
        r1 = Number(arg1.toString().replace(".", ""))
        r2 = Number(arg2.toString().replace(".", ""))
        return (r1 / r2) * pow(10, t2 - t1);
    }
}

//给Number类型增加一个div方法，调用起来更加 方便。
Number.prototype.AccDivide = function(arg) {
    return AccDivide(this, arg);
}

//乘法函数，用来得到精确的乘法结果
//说明：javascript的乘法结果会有误差，在两个浮点数 相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
//调用：accMul(arg1,arg2)
//返回值：arg1乘以 arg2的精确结果
function AccMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try { m += s1.split(".")[1].length } catch (e) { }
    try { m += s2.split(".")[1].length } catch (e) { }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m)
}

// 给Number类型增加一个mul方法，调用起来更加方便。
Number.prototype.AccMul = function(arg) {
    return AccMul(arg, this);
}

//加法函数，用来得到精确的加法结果
//说明：javascript的加法 结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
//调用：AccAdd(arg1,arg2)
// 返回值：arg1加上arg2的精确结果
function AccAdd(arg1, arg2) {
    var r1, r2, m;
    try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
    try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
    m = Math.pow(10, Math.max(r1, r2))
    return (arg1 * m + arg2 * m) / m
}

//给Number类型增加一个add方法，调用起来更加方便。
Number.prototype.AccAdd = function(arg) {
    return AccAdd(arg, this);
}

//合算成英文字符长度
function lenOfStr(s) {
    var l = 0;
    var a = s.split("");
    for (var i = 0; i < a.length; i++) {
        if (a[i].charCodeAt(0) < 299) {
            l++;
        } else {
            l += 2;
        }
    }
    return l;
}

function subStr(sourse, len) {
    var l = 0;
    var a = sourse.split("");
    for (var i = 0; i < a.length; i++) {
        if (a[i].charCodeAt(0) < 299) {
            l++;
        } else {
            l += 2;
        }
        if (l > len) {
            if (i - 1 < 0) { return ''; }
            else return sourse.substr(0, (i - 1));
        }
    }
}

//截取补...
function cutString(sourse, len) {
    if (!len) { return ''; }
    if (typeof (sourse) == 'string' && lenOfStr(sourse) > len) {
        sourse = sourse.substr(0, len - 3) + '...';
    }
    else if (typeof (sourse) == 'object') {
        var s = $(sourse);
        var t = $.trim(s.text());
        if (lenOfStr(t) > len) {
            s.attr('title', t);
            s.text(subStr(t, len - 3) + '...');
        }
    }
}