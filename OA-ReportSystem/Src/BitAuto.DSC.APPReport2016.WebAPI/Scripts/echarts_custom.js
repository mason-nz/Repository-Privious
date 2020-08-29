
///*-----通用方法----------------------*/
//Object.prototype.APclone = function () {
//    var copy = (this instanceof Array) ? [] : {};
//    for (attr in this) {
//        if (!this.hasOwnProperty(attr)) continue;
//        copy[attr] = (typeof this[attr] == "object") ? this[attr].APclone() : this[attr];
//    }
//    return copy;
//};
//小数转换成百分数
Number.prototype.toPercent = function (digit) {
    return (Math.round(this * 10000) / 100).toFixed((digit ? digit : 2)) + '%';
}
//实数3位加逗号
Number.prototype.addComma = function (tofixed) {
    var n = this;
    if (tofixed >= 0) {
        n = this.toFixed(tofixed);
    }
    var all = Math.abs(parseFloat(n)).toString();
    var pos = all.indexOf(".");
    var b = "";
    var end = "";
    if (pos > 0) {
        b = all.substr(0, pos);
        end = all.substr(pos);
    }
    else {
        b = all;
        end = "";
    }
    var len = b.length;
    if (len <= 3) {
        return b;
    }
    var r = len % 3;
    var result = r > 0 ? b.slice(0, r) + "," + b.slice(r, len).match(/\d{3}/g).join(",") : b.slice(r, len).match(/\d{3}/g).join(",");
    return parseInt(n) >= 0 ? result + end : "-" + result + end;
}
//数字转金额 type=亿 or 万
Number.prototype.toMoney = function (type, tofixed) {
    var n = parseFloat(this);
    if (tofixed == null || tofixed == undefined || tofixed < 0) {
        tofixed = 2;
    }
    if (!isNaN(n)) {
        if (type == "亿") {
            n = n / 100000000;
        }
        else if (type = "万") {
            n = n / 10000;
        }
        return n.toFixed(tofixed);
    }
    return 0;
}
//y轴最大值向上取整
Number.prototype.toMaxRound2 = function () {
    var value = Number(this);
    var first = value;
    if (value <= 10 && value >= 0) {
        first = 10;
    }
    else if (value > 10) {
        first = Number(value.toString().substr(0, 2)) + 1;
        first = first * Math.pow(10, (value.toString().length - 2));
    }

    return first;
}

//y轴最大值向上取整
Number.prototype.toMaxRound = function () {
    var value = Number(this);
    //倍数
    var beishu = 1;
    //大于等于10的转个位数
    if (value >= 10) {
        while (value >= 10) {
            value = value / 10;
            beishu = beishu * 10;
        }
    }
    //小于1的转个位数
    else if (value < 1 && value != 0) {
        while (value < 1) {
            value = value * 10;
            beishu = beishu / 10;
        }
    }
    else if (value == 0) {
        return value;
    }
    //取整，取第一位数，其余补0
    var first = Number(value.toString().substr(0, 1)) + 1;
    first = first * beishu;
    return first;
}
//取最大值
function GetMaxNumber(array) {
    var maxbarvalue = 0;
    for (var i = 0; i < array.length; i++) {
        var key = Number(array[i]);
        if (isNaN(key)) {
            continue;
        }
        else {
            if (maxbarvalue < key) {
                maxbarvalue = key;
            }
        }
    }
    return maxbarvalue;
}

//日期格式处理
DateFormat = (function () {
    //日期转字符串
    var DateToString = function (date, format) {
        if (!format || format == "") {
            format = "yyyy-MM-dd HH:mm:ss";
        }

        format = Replace(format, "yyyy", date.getFullYear());
        format = Replace(format, "MM", GetFullMonth(date));
        format = Replace(format, "dd", GetFullDate(date));
        format = Replace(format, "yy", GetHarfYear(date));
        format = Replace(format, "M", date.getMonth() + 1);
        format = Replace(format, "d", date.getDate());

        format = Replace(format, "HH", GetFullHours(date));
        format = Replace(format, "mm", GetFullMinutes(date));
        format = Replace(format, "ss", GetFullSeconds(date));
        format = Replace(format, "H", date.getHours());
        format = Replace(format, "m", date.getMinutes());
        format = Replace(format, "s", date.getSeconds());

        return format;
    };
    // 返回月份（修正为两位数） 
    var GetFullMonth = function (date) {
        var v = date.getMonth() + 1;
        if (v > 9) return v.toString();
        return "0" + v;
    };
    // 返回日 （修正为两位数） 
    var GetFullDate = function (date) {
        var v = date.getDate();
        if (v > 9) return v.toString();
        return "0" + v;
    }
    // 返回两位数的年份
    var GetHarfYear = function (date) {
        var v = date.getFullYear().toString();
        return v.substr(2, 2);
    }
    //返回小时
    var GetFullHours = function (date) {
        var v = date.getHours();
        if (v > 9) return v.toString();
        return "0" + v;
    };
    //返回分钟
    var GetFullMinutes = function (date) {
        var v = date.getMinutes();
        if (v > 9) return v.toString();
        return "0" + v;
    };
    //返回秒
    var GetFullSeconds = function (date) {
        var v = date.getSeconds();
        if (v > 9) return v.toString();
        return "0" + v;
    };
    // 替换字符串
    var Replace = function (str, from, to) {
        return str.split(from).join(to);
    };

    //字符串转日期（标准格式：yyyy,yy,MM,dd,HH,mm,ss）
    var StringToDate = function (str, format) {
        if (!format || format == "") {
            format = "yyyy-MM-dd HH:mm:ss";
        }
        var yyyy = Find(str, format, "yyyy", "yy");
        var mth = Find(str, format, "MM");
        var dd = Find(str, format, "dd");
        var hh = Find(str, format, "HH");
        var mm = Find(str, format, "mm");
        var ss = Find(str, format, "ss");

        return new Date(yyyy == 0 ? 1900 : yyyy, mth == 0 ? 0 : mth - 1, dd == 0 ? 1 : dd, hh, mm, ss);
    };
    //查找元素
    var Find = function (str, format, firstkey, secondkey) {
        var pos1 = format.indexOf(firstkey);
        var pos2 = secondkey ? format.indexOf(secondkey) : -1;
        var value = "";
        if (pos1 >= 0) {
            value = str.substr(pos1, firstkey.length);
        }
        else if (pos2 >= 0) {
            value = str.substr(pos2, secondkey.length);
        }
        else {
            value = "0";
        }
        return isNaN(parseInt(value)) ? 0 : parseInt(value);
    };

    // 增加天 
    var AddDays = function (date, value) {
        var yyyy = date.getFullYear();
        var mth = date.getMonth();
        var dd = date.getDate() + value;
        var hh = date.getHours();
        var mm = date.getMinutes();
        var ss = date.getSeconds();
        return new Date(yyyy, mth, dd, hh, mm, ss);
    };
    // 增加月
    var AddMonths = function (date, value) {
        var yyyy = date.getFullYear();
        var mth = date.getMonth() + value;
        var dd = date.getDate();
        var hh = date.getHours();
        var mm = date.getMinutes();
        var ss = date.getSeconds();
        return new Date(yyyy, mth, dd, hh, mm, ss);
    };
    // 增加年 
    var AddYears = function (date, value) {
        var yyyy = date.getFullYear() + value;
        var mth = date.getMonth();
        var dd = date.getDate();
        var hh = date.getHours();
        var mm = date.getMinutes();
        var ss = date.getSeconds();
        return new Date(yyyy, mth, dd, hh, mm, ss);
    };

    return {
        /*
        用法例子：
        var date = DateFormat.StringToDate(array[i], "yyyy-MM");
        item = DateFormat.DateToString(date, "yy年M月");
        item = DateFormat.DateToString(date, "M月");
        
        var date = DateFormat.StringToDate(array[i], "yyyy");
        var item = DateFormat.DateToString(date, "yyyy年");
        */

        //日期转字符串（支持格式：yyyy,MM,dd,HH,mm,ss,yy,M,d,H,m,s），format默认值：yyyy-MM-dd HH:mm:ss
        DateToString: DateToString,
        //字符串转日期（仅支持格式：yyyy,yy,MM,dd,HH,mm,ss），format默认值：yyyy-MM-dd HH:mm:ss
        StringToDate: StringToDate,

        AddDays: AddDays,
        AddMonths: AddMonths,
        AddYears: AddYears
    }
})();

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
    //    if (XMLHttpRequest.responseText) {
    //        var obj = eval(eval("(" + XMLHttpRequest.responseText + ")"));
    //        alert(obj.Status + "：" + obj.Message);
    //    }
    //    else {
    //        alert('网络超时，提交失败，请重试！');
    //    }
}

/*
* 异步调用出错后提示消息
* Add=Masj, Date: 2016-12-13
*/
$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    var obj = eval("(" + jqxhr.responseText + ")");
    if (thrownError == "Unauthorized") {
        if (obj.Message == '身份验证失败') {
            //alert('您当前身份授权失败！');
            //window.close();
            //授权失败后，在APP中跳转到指定的url进行重新授权
            window.location = 'app.oa.bitauto.com/control/loginauthorizefailed';
        }
        else if (obj.Message == '功能权限验证失败') {
            alert('您当前没有权限访问！');
            window.close();
        }
    }
    else {
        alert('请求出错——' + obj.Status + "：" + obj.Message);
    }
});

/*-----图标默认配置---------(开始)----*/
var defaultEChartsLoadingStyle = {
    maskColor: '#1b1d33'
};

var defaultEChartsTextStyle = {
    color: '#B0BEE3',
    fontWeight: 'normal',
    fontFamily: '微软雅黑',
    fontSize: 12
};

//var defaultEChartsTextStyleFontBold = defaultEChartsTextStyle.APclone();
//defaultEChartsTextStyleFontBold.fontWeight = 'bold';



/*
用于绘制Pie图，中心圆形的对象
Add=masj,Date=2016-11-10    
*/
function GetInnerCircleByPieSeries(radiusValue, top, left) {
    var v = ((radiusValue) ? radiusValue : 4);
    var vTop = ((top) ? top : 35);
    var vLeft = ((left) ? left : 50);
    var seriesObj = {
        name: '',
        type: 'pie',
        selectedOffset: 0,
        hoverAnimation: false,
        silent: true,
        z: 3,
        radius: [0, v + '%'],
        center: [vTop + '%', vLeft + '%'],
        label: {
            show: false
        },
        labelLine: {
            normal: {
                show: false
            }
        },
        data: [
                    { value: 0, name: '', itemStyle: { normal: { color: '#CCCCCC'}} }
                ]
    };
    return seriesObj;
}


/*-----图标默认配置---------(结束)----*/



/*
* 弹出提示框逻辑,需要引用JQuery和jquery.jmpopups-0.5.1.pack.js
* Add=Masj,Date=2016-12-06
*/
function PopTitle(content) {
    $.openPopupLayer({
        name: "popTitle",
        popupMethod: 'post',
        parameters: { Content: encodeURIComponent(content), r: Math.random() },
        url: "/Html/PopTitle.aspx"
    });
}

/*
* 统一处理调用接口请求后的Json数据
* Add=Masj,Date=2016-12-06
*/
function ProcessRequestJsonData(json) {
    if (json) {
        if (json.Status == 0 && json.IsOverdue) {
            //alert('当前访问接口，登录授权失败');
            window.location = 'app.oa.bitauto.com/control/loginauthorizefailed';
            return;
        }
        else if (json.Status == 500 || (json.Status == 0 && json.Result.Success == false)) {
            if (json.Message) {
                alert('调用接口失败，报错信息为：\n' + json.Message);
            }
            else if (json.Result.ErrorMsg) {
                alert('调用接口失败，报错信息为：\n' + json.Result.ErrorMsg);
            }
            else {
                alert('调用接口失败');
            }
            return;
        }
        else if (json.Status == 0) {
            return json.Result;
        }
    }
    return;
}


////数字动画效果 add=masj,Date=2016-11-24
jQuery.fn.extend({
    MagicNumber: function (value, duration) {
        var dur = (duration) ? duration : 1500;
        if (!isNaN(value)) { //判断是否为数字
            var comma_separator_number_step = $.animateNumber.numberStepFactories.separator(',');
            $(this).prop('number', 0).animateNumber(
              {
                  number: value,
                  numberStep: comma_separator_number_step
              },
              dur
            );
        }
    }
});

////浮点数字动画效果 add=masj,Date=2016-12-14
jQuery.fn.extend({
    MagicFloat: function (value, decimal_places, unit, duration) {
        var dur = (duration) ? duration : 1000;
        var dp = (decimal_places) ? decimal_places : 2;
        if (!isNaN(value)) { //判断是否为数字
            //var decimal_places = 2;
            var decimal_factor = dp === 0 ? 1 : Math.pow(10, dp);
            $(this).animateNumber(
                {
                    number: value * decimal_factor,

                    numberStep: function (now, tween) {
                        var floored_number = Math.round(now) / decimal_factor, target = $(tween.elem);

                        if (dp > 0) {
                            // force decimal places even if they are 0
                            floored_number = floored_number.toFixed(dp);

                            // replace '.' separator with ','
                            //floored_number = floored_number.toString().replace('.', ',');
                        }

                        target.text(floored_number + (unit ? unit : ''));
                    }
                },
                dur
            );
        }
    }
});