/**
 * Written by:     zhengxh
 * Created Date:   2018/1/30
 */

// 百度统计
var  _hmt  =  _hmt  ||  [];
(function()  {
    var  hm  =  document.createElement("script");
    hm.src  =  "https://hm.baidu.com/hm.js?19e63f5b1be125b0393ea7e0ec049716";
    var  s  =  document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm,  s);
})();

// QusetMobile统计
var _maq = _maq || [];
_maq.push(['_setAccount', 'xyauto']);
(function() {
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
