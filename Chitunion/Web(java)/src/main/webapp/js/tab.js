//tab切换
$(function () {
    $('.tab ul.menu li').click(function(){
        //获得当前被点击的元素索引值
        var Index = $(this).index();
        //给菜单添加选择样式
        $(this).addClass('active').siblings().removeClass('active');
        //显示对应的div
        $('.tab').children('div').eq(Index).show().siblings('div').hide();

    });
});
$(function () {
    $('.tab2 ul.menu li').click(function(){
        //获得当前被点击的元素索引值
        var Index = $(this).index();
        //给菜单添加选择样式
        $(this).addClass('active').siblings().removeClass('active');
        //显示对应的div
        $('.tab2').children('div').eq(Index).show().siblings('div').hide();

    });
});
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
})

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