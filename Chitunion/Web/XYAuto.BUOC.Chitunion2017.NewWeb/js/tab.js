//浏览器版本号
var browerVersion = 0;

//tab切换
$(function () {

    //服务条款
    $('.register .TermsService').off('click').on('click', function () {
        $.openPopupLayer({
            name: "TermsService",
            url: "TermsService.html",
            success: function () {
                $('.offLayer').off('click').on('click', function () {
                    $.closePopupLayer('TermsService');
                })
            }
        })
    })

    GetBrowerVer();
    $('#imgGGZ,#imgMTZ').unbind('click').bind('click', function () {
        $(this).attr('src', '/CheckCode.aspx?r=' + Math.random());
    });

    //添加密码后面图标点击事件
    $('div.two_rules a.close.pwdTag').unbind('click').bind('click', function () {
        var pwdObj = $(this).prev('input');
        if (pwdObj != null) {
            if (pwdObj.attr('type') == 'password') {
                pwdObj.attr('type', 'text');
                $(this).find('img').attr('src', '../images/login/open.png');
            }
            else {
                pwdObj.attr('type', 'password');
                $(this).find('img').attr('src', '../images/login/close.png');
            }
        }
    });

    //登陆页面，媒体主/广告主，切换样式
    $('ul.nav li').unbind('click').bind('click', function () {
        var oldIndex = $('ul.nav li.selected').index();//当前选中之前的索引值
        //获得当前被点击的元素索引值
        var Index = $(this).index();

        var isCurrentTab = $(this).is('.selected');
        //给菜单添加选择样式
        $(this).addClass('selected').siblings().removeClass('selected');

        var divObj = $('div.two_rules').children('div').eq(Index);
        divObj.find(':input[type=text],:input[type=password]').val('');
        divObj.find('label.notes').html('&nbsp;');
        divObj.find('ul.all_input img:not([id])').css('visibility', 'hidden');

        //if (typeof (RegisterHelper) != "undefined" && RegisterHelper.SetTimeOutIndex()) {
        //    clearTimeout(RegisterHelper.SetTimeOutIndex());
        //    divObj.find('span.repeatSecondMsg').remove();
        //    divObj.find('button').removeAttr('disabled').addClass('but_register');
        //}

        if (!isCurrentTab) {
            var imgObj = divObj.find('img[id]');
            imgObj.removeAttr('src').triggerHandler('click');
        }

        if (Index != oldIndex) {
            if ($('div.two_rules').children('div').eq(oldIndex).find('div.we_code').is(':visible')) {
                if (WXLoginHelper.SetTimeOutIndex()) {
                    clearTimeout(WXLoginHelper.SetTimeOutIndex());
                }
                //显示对应的div
                //WXLoginHelper.Load_QRCode($(this).attr('category'), divObj.find('div.we_code img.code'));
                divObj.find('div[name="qr_timeoutTag"]').remove();
                divObj.show();
                if (divObj.find('div.we_code').size() > 0) {
                    divObj.show().find('div.we_code').show();
                    WXLoginHelper.Load_QRCode($(this).attr('category'));
                    //divObj.find('a.close.pwdTag img').css('visibility', 'visible');
                    divObj.find('div.account').hide();
                    divObj.siblings('div').hide();
                }
                else {
                    divObj.show().find('div.account').show();
                    divObj.find('a.close.pwdTag img').css('visibility', 'visible');
                    divObj.find('div.we_code').hide();
                    divObj.siblings('div').hide();
                }
            }
            else {
                //显示对应的div
                divObj.show().find('div.account').show();
                divObj.find('a.close.pwdTag img').css('visibility', 'visible');
                divObj.find('div.we_code').hide();
                divObj.siblings('div').hide();
            }
        }



    });

    //首页登陆区域，媒体主/广告主，切换样式
    //$('.logon_sy_nr .box ul.menu li').unbind('click').bind('click', function () {
    //    //获得当前被点击的元素索引值
    //    var Index = $(this).index();
    //    //给菜单添加选择样式
    //    $(this).addClass('active').siblings().removeClass('active');

    //    var divObj = $('.logon_sy_nr .box').children('div').eq(Index);
    //    divObj.find(':input[type=text],:input[type=password]').val('');
    //    divObj.find('h2').html('&nbsp;');

    //    var imgObj = divObj.find('img');
    //    imgObj.removeAttr('src').triggerHandler('click');

    //    //显示对应的div
    //    divObj.show().siblings('div').hide();
    //});

    var type = request('type');
    var source = request('s');
    if (type == '2') {
        $('ul.nav li[category="29002"]').triggerHandler('click');
        var index = $('ul.nav li.selected').index();//当前选中之前的索引值

        if (source == 'wx') {
            $('div.two_rules div.role_media div.account ul.bottom li a[name="aWxLogin"]').triggerHandler('click');
        }
    }
});


//解析参数
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}
//$(document).ready(function(){
//    var $tab_li = $('#tab ul li');
//    $tab_li.click(function(){
//        $(this).addClass('selected').siblings().removeClass('selected');
//        var index = $tab_li.index(this);
//        $('div.tab_box > .box').eq(index).show().siblings().hide();
//    });
//});
//$(document).ready(function(){
//    var $tab_li = $('#tab_list ul li');
//    $tab_li.click(function(){
//        $(this).addClass('selected').siblings().removeClass('selected');
//        var index = $tab_li.index(this);
//        $('div.tab_box > .box').eq(index).show().siblings().hide();
//    });
//});


////菜单隐藏展开
//$(function(){
//    //菜单隐藏展开
//    var tabs_i=0
//    $('.vtitle').on('click',function(){
//        tabs_i = parseInt($(this).attr('data-num'));
//        $(this).siblings('.v_current').attr('class','vtitle');
//        $(this).attr('class','v_current');
//        $('.vcon').css('display','none');
//        $(this).next('.vcon ').css('display','block');
//        console.log(tabs_i)
//        $('.vcon').eq(tabs_i).slideDown().siblings('.vcon').slideUp();
//    });
//    $('.vtitle').eq(0).click();
//});

function GetBrowerVer() {
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] : 0;
    var brower = 'other';
    if ($.browser.msie) {
        //检测是否是IE6，是-则关闭页面
        if ($.browser.version == 7) {
            browerVersion = 7;
            alert("请升级IE7版本！");
            window.opener = null; window.open('', '_self'); window.close();
            return false;
        }
        brower = 'IE ' + $.browser.version;
    }
    else if ($.browser.safari) {
        // 火狐浏览器
        if (Sys.firefox) {
            brower = 'firefox ' + $.browser.version;
        }
            // 谷歌浏览器
        else if (Sys.chrome) {
            brower = 'chrome ' + Sys.chrome;
        }
    }
    else if ($.browser.mozilla)
    { brower = 'mozilla ' + $.browser.version; }
    else if ($.browser.opera)
    { brower = 'opera ' + $.browser.version; }

    //console.log(brower);
}