//浏览器版本号
var browerVersion = 0;

//tab切换
$(function () {
    GetBrowerVer();
    $('#imgGGZ,#imgMTZ,#imgNBYH').unbind('click').bind('click', function () {
        $(this).attr('src', 'CheckCode.aspx?r=' + Math.random());
    });
    //登陆页面，媒体主/广告主，切换样式
    $('.tab ul.menu li').unbind('click').bind('click', function () {
        //获得当前被点击的元素索引值
        var Index = $(this).index();
        //给菜单添加选择样式
        $(this).addClass('active').siblings().removeClass('active');

        var divObj = $('.tab').children('div').eq(Index);
        divObj.find(':input[type=text],:input[type=password]').val('');
        divObj.find('h2').html('&nbsp;');

        var imgObj = divObj.find('img');
        imgObj.removeAttr('src').triggerHandler('click');

        //显示对应的div
        divObj.show().siblings('div').hide();
    });

    //首页登陆区域，媒体主/广告主，切换样式
    $('.logon_sy_nr .box ul.menu li').unbind('click').bind('click', function () {
        //获得当前被点击的元素索引值
        var Index = $(this).index();
        //给菜单添加选择样式
        $(this).addClass('active').siblings().removeClass('active');

        var divObj = $('.logon_sy_nr .box').children('div').eq(Index);
        divObj.find(':input[type=text],:input[type=password]').val('');
        divObj.find('h2').html('&nbsp;');

        var imgObj = divObj.find('img');
        imgObj.removeAttr('src').triggerHandler('click');

        //显示对应的div
        divObj.show().siblings('div').hide();
    });
});
//$(function () {
//    $('.tab2 ul.menu li').unbind('click').click(function () {
//        //获得当前被点击的元素索引值
//        var Index = $(this).index();
//        //给菜单添加选择样式
//        $(this).addClass('active').siblings().removeClass('active');
//        //显示对应的div
//        $('.tab2').children('div').eq(Index).show().siblings('div').hide();
//        //$('#imgGGZ').attr('src', 'CheckCode.aspx?r='+Math.random());
//        $('#imgGGZ').trigger('click');
//    });
//});
$(document).ready(function(){
    var $tab_li = $('#tab ul li');
    $tab_li.click(function(){
        $(this).addClass('selected').siblings().removeClass('selected');
        var index = $tab_li.index(this);
        $('div.tab_box > .box').eq(index).show().siblings().hide();
    });
});
$(document).ready(function(){
    var $tab_li = $('#tab_list ul li');
    $tab_li.click(function(){
        $(this).addClass('selected').siblings().removeClass('selected');
        var index = $tab_li.index(this);
        $('div.tab_box > .box').eq(index).show().siblings().hide();
    });
});


//菜单隐藏展开
$(function(){
    //菜单隐藏展开
    var tabs_i=0
    $('.vtitle').on('click',function(){
        tabs_i = parseInt($(this).attr('data-num'));
        $(this).siblings('.v_current').attr('class','vtitle');
        $(this).attr('class','v_current');
        $('.vcon').css('display','none');
        $(this).next('.vcon ').css('display','block');
        // var _self = $(this);
        // var j = $('.vtitle').index(_self);
        // if( tabs_i == j ) return false; tabs_i = j;
        // $('.vtitle em').each(function(e){
        //     if(e==tabs_i){
        //         $('em',_self).removeClass('v01').addClass('v02');
        //     }else{
        //         $(this).removeClass('v02').addClass('v01');
        //     }
        // });
        // // $('.v_current em').removeClass('v01').addClass('v02');
        // // var j = $('.v_current').index(_self);
        // if( tabs_i == j ) return false; tabs_i = j;
        // $('.v_current em').each(function(e){
        //     if(e==tabs_i){
        //         $('em',_self).removeClass('v01').addClass('v02');
        //     }else{
        //         $(this).removeClass('v02').addClass('v01');
        //     }
        // });
        console.log(tabs_i)
        $('.vcon').eq(tabs_i).slideDown().siblings('.vcon').slideUp();
    });
    $('.vtitle').eq(0).click();
});

// $(function(){
//     //菜单隐藏展开
//     var tabs_i=0
//     $('.vtitle').click(function(){
//         console.log($(this))
//         // $(this).siblings('.v_current').attr('class','vtitle');
//         // $(this).attr('class','v_current');
//         $('.vcon').css('display','none');
//
//         $(this).next('.vcon ').css('display','block');
//     });
//     $('.vtitle').eq(0).click();
// })


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