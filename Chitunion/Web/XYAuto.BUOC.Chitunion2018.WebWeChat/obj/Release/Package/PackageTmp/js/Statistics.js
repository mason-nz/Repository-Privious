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

// 移动端css
var  M_css  =  M_css  ||  [];
(function()  {
    var  hm  =  document.createElement("link");
    hm.rel="stylesheet";
    hm.href  =  "/css/MSite.css?r="+Math.random();
    var  s  =  document.getElementsByTagName("link")[0];
    s.parentNode.insertBefore(hm,  s);
})();

// wx
var  _weixin =  _weixin  ||  [];
(function()  {
    var  weixin  =  document.createElement("script");
    weixin.src  =  "http://res.wx.qq.com/open/js/jweixin-1.2.0.js";
    var  s  =  document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(weixin,  s);
})();


// 移动端js文件
var  _msile =  _msile  ||  [];
(function()  {
    var  msile  =  document.createElement("script");
    msile.src  =  "/js/MSile.js?v="+Math.random();
    var  s  =  document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(msile,  s);
})();

// footer js文件
var  footer =  footer  ||  [];
(function()  {
    var  msile  =  document.createElement("script");
    msile.src  =  "/js/footer.js?v="+Math.random();
    var  s  =  document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(msile,  s);
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


$(document).ready(function () {
    //绑定百度统计trackEvent事件逻辑
    $('[baidu_track_name][baidu_track_action][baidu_track_label]').on('click', function () {
        var name = $(this).attr('baidu_track_name');
        var action = $(this).attr('baidu_track_action');
        var label = $(this).attr('baidu_track_label');
        var val = $(this).attr('baidu_track_val');
        if (val == null) {
            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label]);
        }
        else {
            window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label, val]);
        }
    });
});

/**
 * Written by:      zhengxh
 * function:        jq仿rem适配
 * Created Date:    2018-01-06
 */
function setImgSize() {
    var originWidth = 375,
        ratio = $(window).width() / originWidth;
    $('.img-size').each(function () {
        var self = $(this);
        $.each(['height', 'width', 'left', 'fontSize', 'right', 'bottom', 'top', 'paddingTop', 'lineHeight', 'paddingLeft', 'paddingRight', 'paddingBottom', 'marginTop', 'marginLeft', 'marginRight', 'marginBottom','padding','borderRadius'], function (i, str) {
            var num = self.attr('data-' + str);
            if (num) {
                num = num * ratio / 2 + 'px';
                self.css(str, num)
            }
        })
    });
}
setImgSize();

