
/**
* Written by:      lixh
* Created Date:    2017-3-1
* @param setting   ajax请求的参数，可设置url,type(可选，默认get),data（可选，默认为空）
* @param fn1    	异步请求的成功回调函数（参数data为请求回的数据）
* @param fn2    	异步请求的失败回调函数（可选）
* @param fn3    	请求发送之前的回调函数（可选）
*/

var public_url = '';
function setAjax(setting, fn1, fn2) {
    var r = setting.url.split('&r=')[1] || setting.data ? setting.data.r : '';
    if (!r && setting.data) {
        setting.data.r = Math.random();
    } else {
    }
    $.ajax({
        url: setting.url,
        type: setting.type,
        data: setting.data,
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: fn1,
        error: fn2,
        beforeSend: function () {
            $(setting.selector).html('<div style="margin:70px auto; width:100%;"><img src="/images/loading.gif"></div>');
        }
    });
}

// 价格数字格式化
/**
* Written by:      liushuai
* function:        将数字转换成货币类型,一般使用传入number和places即可
* Created Date:    2017-2-23
* Modified Date:   2017-2-26
* @param number    传入你想改变的数字
* @param places    保留小数位数,默认保留两位小数位
* @param symbol    以什么符号开头,默认是'¥ '
* @returns{string} 返回值为转换后的货币字符串
*/
function formatMoney(number, places, symbol) {
    number = number || 0;
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "¥";
    thousand = ",";
    decimal = ".";
    var negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
}
/**
* Written by:      wangc
* function:        判断小数点后是否是00或者n0
* Created Date:    2017-12-28
* Modified Date: 
* @param number    传入你要操作的千位分隔的数  
* @param n         传入你保留的小数位数
* @returns{string}   返回值为：正确的显示方式
*/
function removePoint0(number,n){
    var count0 = '0';
    for(var i = 1;i<n;i++){
        count0 += '0';
    }
    if(number.substr(number.length - n,n) == count0){
        return number.substr(0,number.length-n-1);
    }else{
        return number;
    }
}

function GetRequest() {// 获取url？后面的参数
    var url = location.search;
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}

//判断文本是否为手机格式
String.prototype.isMobile = function () {
    //var phone = document.getElementById('phone').value;
    if (!(/^1[34578]\d{9}$/.test(this))) {
        //alert("手机号码有误，请重填");
        return false;
    }
    return true;
}

//判断当前字符串是否以str结束
if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}


//判断日期格式是否合法
String.prototype.isDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false; var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}

//判断日期格式是否合法
String.prototype.parseDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) {
        return null;
    }
    var d = new Date(r[1], r[3] - 1, r[4]);
    if (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]) {
        return d;
    }
    return null;
}

//日期类型，添加天数函数
Date.prototype.addDate = function (days) {
    var a = this;
    a = a.valueOf();
    a = a + days * 24 * 60 * 60 * 1000
    a = new Date(a);
    return a;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}


// 只能输入数字 Start
/**
* Written by:      fengb
* function:        输入框智只能输入正整数
* Created Date:    2017-12-19
/*调用    '/限制的正则表达式/g'   必须以/开头，/g结尾
 *  onkeyup="replaceAndSetPos(this,/[^0-9]/g,'')" oninput ="replaceAndSetPos(this,/[^0-9]/g,'')"
 */
//获取光标位置
function getCursorPos(obj) {
    var CaretPos = 0;
    // IE Support
    if (document.selection) {
        obj.focus (); //获取光标位置函数
        var Sel = document.selection.createRange ();
        Sel.moveStart ('character', -obj.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox/Safari/Chrome/Opera support
    else if (obj.selectionStart || obj.selectionStart == '0')
        CaretPos = obj.selectionEnd;
    return (CaretPos);
};
//定位光标
function setCursorPos(obj,pos){
    if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
        obj.focus(); //
        obj.setSelectionRange(pos,pos);
    } else if (obj.createTextRange) { // IE
        var range = obj.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
};
//替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
function replaceAndSetPos(obj,pattern,text){
    if ($(obj).val() == "" || $(obj).val() == null) {
        return;
    }
    var pos=getCursorPos(obj);//保存原始光标位置
    var temp=$(obj).val(); //保存原始值
    obj.value=temp.replace(pattern,text);//替换掉非法值
    //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
    var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
    if( obj.value.length > max_length){
        var str1 = obj.value.substring( 0,pos-1 );
        var str2 = obj.value.substring( pos,max_length+1 );
        obj.value = str1 + str2;
    }
    pos=pos-(temp.length-obj.value.length);//当前光标位置
    setCursorPos(obj,pos);//设置光标
    //el.onkeydown = null;
};
//-----------------只能输入数字 end


/**
* Written by:      fengb
* Created Date:    2017-12-12
* function:  ES5中新增了写数组方法，如下：
    forEach (js v1.6)
    map (js v1.6)
    filter (js v1.6)
    some (js v1.6)
    every (js v1.6)
    indexOf (js v1.6)
    lastIndexOf (js v1.6)
    reduce (js v1.8)
    reduceRight (js v1.8)

    浏览器支持
    Opera 11+
    Firefox 3.6+
    Safari 5+
    Chrome 8+
    Internet Explorer 9+
    所以对于IE6-IE8浏览器，Array原型扩展可以实现以上全部功能
*/

if (typeof Array.prototype.forEach != "function") {
    Array.prototype.forEach = function (fn, context) {
        for (var k = 0, length = this.length; k < length; k++) {
            if (typeof fn === "function" && Object.prototype.hasOwnProperty.call(this, k)) {
                fn.call(context, this[k], k, this);
            }
        }
    };
}

if (typeof Array.prototype.map != "function") {
    Array.prototype.map = function (fn, context) {
        var arr = [];
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
                arr.push(fn.call(context, this[k], k, this));
          }
        }
        return arr;
  };
}

if (typeof Array.prototype.filter != "function") {
    Array.prototype.filter = function (fn, context) {
        var arr = [];
        if (typeof fn === "function") {
          for (var k = 0, length = this.length; k < length; k++) {
              fn.call(context, this[k], k, this) && arr.push(this[k]);
          }
        }
        return arr;
  };
}

if (typeof Array.prototype.some != "function") {
    Array.prototype.some = function (fn, context) {
        var passed = false;
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
              if (passed === true) break;
              passed = !!fn.call(context, this[k], k, this);
            }
        }
        return passed;
  };
}

if (typeof Array.prototype.every != "function") {
    Array.prototype.every = function (fn, context) {
        var passed = true;
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
                if (passed === false) break;
                passed = !!fn.call(context, this[k], k, this);
            }
        }
        return passed;
  };
}

if (typeof Array.prototype.indexOf != "function") {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var index = -1;
        fromIndex = fromIndex * 1 || 0;

        for (var k = 0, length = this.length; k < length; k++) {
          if (k >= fromIndex && this[k] === searchElement) {
              index = k;
              break;
          }
        }
        return index;
  };
}

if (typeof Array.prototype.lastIndexOf != "function") {
    Array.prototype.lastIndexOf = function (searchElement, fromIndex) {
        var index = -1, length = this.length;
        fromIndex = fromIndex * 1 || length - 1;

        for (var k = length - 1; k > -1; k-=1) {
            if (k <= fromIndex && this[k] === searchElement) {
                index = k;
                break;
            }
        }
        return index;
  };
}

if (typeof Array.prototype.reduce != "function") {
    Array.prototype.reduce = function (callback, initialValue ) {
        var previous = initialValue, k = 0, length = this.length;
        if (typeof initialValue === "undefined") {
            previous = this[0];
            k = 1;
        }
         
        if (typeof callback === "function") {
          for (k; k < length; k++) {
            this.hasOwnProperty(k) && (previous = callback(previous, this[k], k, this));
          }
        }
        return previous;
    };
}

if (typeof Array.prototype.reduceRight != "function") {
    Array.prototype.reduceRight = function (callback, initialValue ) {
        var length = this.length, k = length - 1, previous = initialValue;
        if (typeof initialValue === "undefined") {
            previous = this[length - 1];
            k--;
        }
        if (typeof callback === "function") {
           for (k; k > -1; k-=1) {          
              this.hasOwnProperty(k) && (previous = callback(previous, this[k], k, this));
           }
        }
        return previous;
    };
}
/*---------------------end-----------------*/


//解决IE10以下不支持Function.bind
if (!Function.prototype.bind) {
    Function.prototype.bind = function(oThis) {
        if (typeof this !== "function") {
            throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
        }
        var aArgs = Array.prototype.slice.call(arguments, 1),
            fToBind = this,
            fNOP = function() {},
            fBound = function() {
                return fToBind.apply(this instanceof fNOP && oThis ? this : oThis,
                    aArgs.concat(Array.prototype.slice.call(arguments)));
            };
        fNOP.prototype = this.prototype;
        fBound.prototype = new fNOP();
        return fBound;
    };
}



/**
* forEach遍历数组
* @param callback [function] 回调函数；
* @param context [object] 上下文；
*/
Array.prototype.myForEach = function myForEach(callback,context){
    context = context || window;
    if('forEach' in Array.prototye) {
        this.forEach(callback,context);
        return;
    }
    //IE6-8下自己编写回调函数执行的逻辑
    for(var i = 0,len = this.length; i < len;i++) {
        callback && callback.call(context,this[i],i,this);
    }
};



/**
* map遍历数组
* @param callback [function] 回调函数；
* @param context [object] 上下文；
*/
Array.prototype.myMap = function myMap(callback,context){
    context = context || window;
    if('map' in Array.prototye) {
        return this.map(callback,context);
    }
    //IE6-8下自己编写回调函数执行的逻辑
    var newAry = [];
    for(var i = 0,len = this.length; i < len;i++) {
        if(typeof  callback === 'function') {
            var val = callback.call(context,this[i],i,this);
            newAry[newAry.length] = val;
        }
    }
    return newAry;
};

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
if (!String.prototype.endsWith) {
    (function() {
        'use strict'; // needed to support `apply`/`call` with `undefined`/`null`
        var defineProperty = (function() {
            // IE 8 only supports `Object.defineProperty` on DOM elements
            try {
                var object = {};
                var $defineProperty = Object.defineProperty;
                var result = $defineProperty(object, object, object) && $defineProperty;
            } catch(error) {}
            return result;
        }());
        var toString = {}.toString;
        var endsWith = function(search) {
            if (this == null) {
                throw TypeError();
            }
            var string = String(this);
            if (search && toString.call(search) == '[object RegExp]') {
                throw TypeError();
            }
            var stringLength = string.length;
            var searchString = String(search);
            var searchLength = searchString.length;
            var pos = stringLength;
            if (arguments.length > 1) {
                var position = arguments[1];
                if (position !== undefined) {
                    // `ToInteger`
                    pos = position ? Number(position) : 0;
                    if (pos != pos) { // better `isNaN`
                        pos = 0;
                    }
                }
            }
            var end = Math.min(Math.max(pos, 0), stringLength);
            var start = end - searchLength;
            if (start < 0) {
                return false;
            }
            var index = -1;
            while (++index < searchLength) {
                if (string.charCodeAt(start + index) != searchString.charCodeAt(index)) {
                    return false;
                }
            }
            return true;
        };
        if (defineProperty) {
            defineProperty(String.prototype, 'endsWith', {
                'value': endsWith,
                'configurable': true,
                'writable': true
            });
        } else {
            String.prototype.endsWith = endsWith;
        }
    }());
}