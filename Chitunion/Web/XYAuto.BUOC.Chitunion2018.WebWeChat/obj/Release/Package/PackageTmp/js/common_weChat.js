/**
 * Written by:     zhengxh
 * Created Date:   2018/1/16
 */


//var public_url = 'http://wxs.chitunion.com';//正式环境

var public_url = 'http://wxtest-ct.qichedaquan.com', //开发环境
    public_pre =  public_url + '/api/OAuth2/Index?returnUrl=' + public_url,

    invite_url = 'http://hd.wxtest-ct.qichedaquan.com';

/**
 * Written by:      wangc
 * function:        wx.conig配置
 * Created Date:    2018-01-18
 * Modified Date:
 */

$.ajax({
    url: public_url + '/api/WeixinJSSDK/GetInfo?url='+encodeURIComponent(window.location.href),
    type:'get',
    xhrFields: {
        withCredentials: true
    },
    crossDomain: true,
    //data:{
    //    url: encodeURIComponent(window.location.href)
    //},
    success:function(data){
        var configInfo = data.Result;
        console.log(configInfo,'configInfo');
        wx.config({
            debug: false,
            appId: configInfo.AppId,
            nonceStr: configInfo.NonceStr,
            timestamp: configInfo.Timestamp,
            signature: configInfo.Signature,
            jsApiList: [
                'checkJsApi',
                'onMenuShareTimeline',
                'onMenuShareAppMessage',
                'onMenuShareQQ',
                'onMenuShareWeibo',
                'onMenuShareQZone',
                'hideMenuItems',
                'showMenuItems',
                'hideAllNonBaseMenuItem',
                'showAllNonBaseMenuItem',
                'translateVoice',
                'startRecord',
                'stopRecord',
                'onVoiceRecordEnd',
                'playVoice',
                'onVoicePlayEnd',
                'pauseVoice',
                'stopVoice',
                'uploadVoice',
                'downloadVoice',
                'chooseImage',
                'previewImage',
                'uploadImage',
                'downloadImage',
                'getNetworkType',
                'openLocation',
                'getLocation',
                'hideOptionMenu',
                'showOptionMenu',
                'closeWindow',
                'scanQRCode',
                'chooseWXPay',
                'openProductSpecificView',
                'addCard',
                'chooseCard',
                'openCard'
            ]
        });
    }
})


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


/**
 * Written by:      zhengxh
 * function:        获取URL参数
 * Created Date:    2018-01-06
 */
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

//判断文本是否为手机格式
String.prototype.isMobile = function () {
    //var phone = document.getElementById('phone').value;
    if (!(/^1[34578]\d{9}$/.test(this))) {
        //alert("手机号码有误，请重填");
        return false;
    }
    return true;
}


/**
  * Written by:     zhengxh
  * function:       对Date的扩展，将 Date 转化为指定格式的String
                    月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q)
                    可以用 1-2 个占位符，年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)
  * Created Date:   2018-01-06
  * eg:             (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
                    (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18
*/
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

//百度统计
var _hmt = _hmt || [];
(function() {
  var hm = document.createElement("script");
  hm.src = "https://hm.baidu.com/hm.js?d751b51d2c5894f498a8281997cc154f";
  var s = document.getElementsByTagName("script")[0];
  s.parentNode.insertBefore(hm, s);
})();

// QusetMobile统计
var _maq = _maq || [];
_maq.push(['_setAccount', 'xyauto']);
(function () {
    var ma = document.createElement('script');
    ma.type = 'text/javascript';
    ma.async = true;
    ma.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'www.qchannel03.cn/m.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ma, s);
})();

// 行圆统计
(function () {
    var s = document.createElement('script');
    s.type = 'text/javascript';
    s.async = true;
    s.src = "http://static.qcdqcdn.com/js/xy.js?ver=1.0";
    var firstScript = document.getElementsByTagName('script')[0];
    firstScript.parentNode.insertBefore(s, firstScript);
})();

/**
 * Written by:      fengbo
 * function:        拓展localStorage
 * Created Date:    2018-03-20
 */

(function(window, localStorage, undefined) {
    var LS = {
        set: function(key, value) {
            //在iPhone/iPad上有时设置setItem()时会出现诡异的QUOTA_EXCEEDED_ERR错误
            //这时一般在setItem之前，先removeItem()就ok了
            if (this.get(key) !== null) this.remove(key);
            localStorage.setItem(key, value);
        },
        //查询不存在的key时，有的浏览器返回undefined，这里统一返回null
        get: function(key) {
            var v = localStorage.getItem(key);
            return v === undefined ? null : v;
        },
        remove: function(key) {
            localStorage.removeItem(key);
        },
        clear: function() {
            localStorage.clear();
        },
        each: function(fn) {
            var n = localStorage.length,
                i = 0,
                fn = fn ||
            function() {}, key;
            for (; i < n; i++) {
                key = localStorage.key(i);
                if (fn.call(this, key, this.get(key)) === false) break;
                //如果内容被删除，则总长度和索引都同步减少
                if (localStorage.length < n) {
                    n--;
                    i--;
                }
            }
        }
    },
        j = window.jQuery,
        c = window.Core;
    //扩展到相应的对象上
    window.LS = window.LS || LS;
    //扩展到其他主要对象上
    if (j) j.LS = j.LS || LS;
    if (c) c.LS = c.LS || LS;
})(window, window.localStorage);
