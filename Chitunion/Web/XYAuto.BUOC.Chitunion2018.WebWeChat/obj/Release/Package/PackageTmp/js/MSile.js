// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
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
// 用于跳转
function url() {
    // 判断微信域名跳转pc端
    if ((window.location + '').indexOf('wxs.chitunion.com/news') != -1 && $('html').width() >= 992) {
        window.location = window.location.href.replace("wxs.chitunion.com/news", "newscdn.qichedaquan.com/ct_pc");
        return false
    }

    if ((window.location + '').indexOf('wxtest-ct.qichedaquan.com/news') != -1 && $('html').width() >= 992) {
        window.location = window.location.href.replace("wxtest-ct.qichedaquan.com/news", "newscdn.qichedaquan.com/ct_pc");
        return false
    }
    if ((window.location + '').indexOf('wx-ct.qichedaquan.com/news') != -1 && $('html').width() >= 992) {
        window.location = window.location.href.replace("wx-ct.qichedaquan.com/news", "newscdn.qichedaquan.com/ct_pc");
        return false
    }

    if (GetRequest().Operate != 'yes' && $('html').width() >= 992 && (window.location + '').indexOf('qcdq_m') == -1) {
        window.location = window.location.href.replace("ct_m", "ct_pc");
        return false
    }
}
url()


$(function () {
    // 获取文章id
    var MaterialID = $('#M_ArticleID').attr('articleID');
    // 获取物料id
    var MaterialID1 = (window.location+'').split('/')[(window.location+'').split('/').length-1].split('.')[0];

 var public_url = 'http://op1.chitunion.com';
 var public_wxurl = 'http://wx-ct.qichedaquan.com';
    // 计算腰部高度
    if ($('.M_Waist img').attr('mimg') == 1) {
        //    图片宽度
        var img_w = $('.M_Waist img').width();
        $('.M_Waist img').each(function () {
            $(this).css('height', (img_w * 2 / 3) + 'px')
            $(this).next().css('height', (img_w * 2 / 3) + 'px')
        })
    };
    window.onload=function () {
        if ($('.M_content').height() <= 2499) {
            $('.M_book').hide()
        }
        // 查看全部
        $('.M_book').off('click').on('click', function () {
            $(this).css('display', 'none');
            $('.M_content').css('max-height', '100%')
        })
        // 隐藏分享
        if($('html').width() >= 800){
            $('.M_RelatedReading_title').parent().hide();
            $('.M_share_imgQR img').eq(0).css('width','300px')
            $('.M_Close').css('width','18px')
            $('.M_Close').css('right','18px')
            $('.M_Waist a img').css('width','33%')
            // 隐藏相关阅读
            $('.M_RelatedReading').hide();
            // 查看更多
            $('.M_book').click()
        }
        if(document.referrer.split('/')[document.referrer.split('/').length-1]=='make_money.html'){
            // 隐藏分享
            $('.M_RelatedReading_title').parent().hide();
            // 隐藏头部
            $('.M_header').hide();
            // 隐藏相关阅读
            $('.M_RelatedReading').hide();
        }else {
            // 显示头部
            $('.M_header').show();
            // 显示分享和相关阅读
            $('.triggerHide').show()
        }
    }
    // 分享图片
    $('#M_share_img').off('click').on('click',function () {
        $('.M_share_imgQR_Mask').css({
            width:'100%',
            height:'100%',
            position:'fixed',
            top:0,
            left:0,
            backgroundColor:'rgba(0,0,0,.5)',
            zIndex:2
        }).show().off('click').on('click',function () {
            $(this).hide()
            $('.M_share_imgQR').hide()
        })
        $('.M_Close').off('click').on('click',function () {
            $('.M_share_imgQR_Mask').click()
        })
        $('.M_share_imgQR').css({
            position:'fixed',
            top:'50%',
            left:'50%',
            zIndex:3,
            transform: 'translate(-50%, -50%)'
        }).show()
    })

    $.ajax({
        url: public_url + '/api/ApiNL/GetLikeCount',
        type: 'get',
        dataType: 'json',
        data: {
            MaterieUrl: window.location.href
        },
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (data) {
            if (data.Status == 0) {
                // 点赞渲染
                $('.M_Fabulous_img div').eq(0).find('span').text(data.Result.LikeCount)
                $('.M_Fabulous_img div').eq(1).find('span').text(data.Result.NoLikeCount)
                // 点赞功能
                if (data.Result.ClickType == 1) {
                    $('.M_Fabulous .M_Fabulous_img div').eq(0).css('background-image', 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEoAAABKCAYAAAFr1/LnAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjhCQTJGNjM4RDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjhCQTJGNjM5RDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OEJBMkY2MzZEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OEJBMkY2MzdEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4k6iz1AAAZWUlEQVR42mJUiTjE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)');
                } else if (data.Result.ClickType == 2) {
                    $('.M_Fabulous .M_Fabulous_img div').eq(1).css('background-image', 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEoAAABKCAYAAAFr1/LnAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjhCMzQ5MTdERDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjhCMzQ5MTdFRDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OEIzNDkxN0JEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OEIzNDkxN0NEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6OspuOAAAZKElEQVR42mJUiTjE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)');
                }
                var judge = true;
                if (data.Result.ClickType != 0) {
                    judge = false;
                }
                $('.M_Fabulous .M_Fabulous_img div').off('click').on('click', function () {
                    if (judge) {
                        if ($(this).find('span').attr('i') == 1) {
                            if (M_operation($(this).find('span').attr('i'))) {
                                var getnub = $(this).find('span').html() - 0 + 1;
                                $(this).find('span').html(getnub);
                                $(this).css('background-image', 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEoAAABKCAYAAAFr1/LnAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjhCQTJGNjM4RDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjhCQTJGNjM5RDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OEJBMkY2MzZEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OEJBMkY2MzdEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4k6iz1AAAZWUlEQVR42mJUiTjE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)');
                            }
                        } else {
                            if (M_operation($(this).find('span').attr('i'))) {
                                var getnub = $(this).find('span').html() - 0 + 1;
                                $(this).find('span').html(getnub);
                                $(this).css('background-image', 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEoAAABKCAYAAAFr1/LnAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjhCMzQ5MTdERDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjhCMzQ5MTdFRDg5RTExRTdCRDQ3RTRBNjM3RUQ0OTlGIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OEIzNDkxN0JEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OEIzNDkxN0NEODlFMTFFN0JENDdFNEE2MzdFRDQ5OUYiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6OspuOAAAZKElEQVR42mJUiTjE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)');
                            }
                        };
                        judge = false;
                    }else {
                        if($('.M_Fabulous .M_Fabulous_img div').eq(0).attr('style')){
                            wu.showToast({
                                title: '已赞',
                                mask: false,                //是否可以操作dom
                                icon: 'icon-success',   // icon-success | icon-error | icon-info
                                duration: 3000
                            });
                        } else {
                            wu.showToast({
                                title: '已踩',
                                mask: false,                //是否可以操作dom
                                icon: 'icon-success',   // icon-success | icon-error | icon-info
                                duration: 3000
                            });
                        }
                    }
                })
            } else {
                alert(data.Message)
            }
        }

    })

    // 物料静态页点赞点踩接口
    function M_operation(i) {
        var that = true;
        $.ajax({
            url: public_url + '/api/ApiNL/SetCustomerInfo',
            type: 'post',
            asycn: false,
            dataType: 'json',
            data: {
                MaterieUrl: window.location.href,
                LikeType: i
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                if (data.Status == 0) {

                } else {
                    alert(data.Message);
                    that = false
                }
            }
        });
        return that
    }

    // 控制M_content下的标签
    $('.M_content img').css('max-width', '100%');

    // 相关阅读渲染
    $.ajax({
        url:'http://op.chitunion.com/api/ApiNL/GetRelatedReading',
        type:'get',
        dataType:'json',
        data:{
            MaterielID:MaterialID
        },
        async:false,
        success:function (data) {
            if(data.Status==0){
                if(data.Result.length!=0){
                    $('.M_RelatedReading').html(ejs.render($('#M_RelatedReading').html(), data));
                }else {
                    $('.M_RelatedReading').hide()
                }
            }else {
                // alert(data.Message)
            }

        }
    })
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
    setImgSize()
    // 判断是否已经生成分享参数
    if(GetRequest().utm_term){
        // 判断是否在微信访问
        if(is_weixn()){
            // 分享
            share('Generated');
        }
        return false
    }else {
        // 判断是否在微信访问
        if(is_weixn()){
            // 分享
            share();
        }
    }

    function is_weixn() {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    }

    function share(Generated) {
        // 微信配置
        $.ajax({
            url: public_wxurl+'/api/WeixinJSSDK/GetInfo?url=' + encodeURIComponent(window.location.href),
            type: 'get',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            asycn: false,
            success: function (data) {
                var configInfo = data.Result;
                console.log(configInfo, 'configInfo');
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
                        'hideMenuItems'
                    ]
                });

            }
        })



        $.ajax({
            url: public_wxurl+'/api/Task/GetOrderUrl',
            type: 'get',
            dataType: 'json',
            data: {
                // TaskId: data.TaskId
                MaterialID: MaterialID1
            },
            xhrFields: {
                withCredentials: true
            },
            asycn: false,
            crossDomain: true,
            success: function (data) {
                if (data.Status == 0) {
                    // _this.MaterialUrl='http://newscdn.chitunion.com/ct_m/20180123/74744.html?utm_source=chitu&utm_term=bgL0IP0mHg'
                    var option
                    if(Generated){
                        option = {
                            title: data.Result.TaskName, // 分享标题
                            desc: data.Result.Synopsis, // 分享描述
                            link: window.location+'', // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
                            // link:'http://newscdn.chitunion.com/ct_m/20180123/74744.html',
                            imgUrl: data.Result.ImgUrl, // 分享图标
                            type: '', // 分享类型,music、video或link，不填默认为link
                            dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                            success: function () {

                            },
							trigger:function () {
								//分享动作保存日志
								$.ajax({
									url:public_wxurl+'/api/WeixinJSSDK/ShareLog',
									type:'get',
									xhrFields: {
										withCredentials: true
									},
									crossDomain: true,
									data:{
										url:window.location.href
									},
									success:function(){
										//保存成功
									}
								})
							},
                            cancel: function () {
                                // 用户取消分享后执行的回调函数
                            }
                        }
                    }else {
                        option = {
                            title: data.Result.TaskName, // 分享标题
                            desc: data.Result.Synopsis, // 分享描述
                            link: data.Result.OrderUrl, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
                            // link:'http://newscdn.chitunion.com/ct_m/20180123/74744.html',
                            imgUrl: data.Result.ImgUrl, // 分享图标
                            type: '', // 分享类型,music、video或link，不填默认为link
                            dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                            success: function () {
                                // 用户确认分享后执行的回调函数
                                shareOK(data.Result.TaskId, data.Result.OrderUrl)
                            },
							trigger:function () {
								//分享动作保存日志
								$.ajax({
									url:public_wxurl+'/api/WeixinJSSDK/ShareLog',
									type:'get',
									xhrFields: {
										withCredentials: true
									},
									crossDomain: true,
									data:{
										url:window.location.href
									},
									success:function(){
										//保存成功
									}
								})
							},
                            cancel: function () {
                                // 用户取消分享后执行的回调函数
                            }							
                        }
                    }
                    wx.ready(function () {
                        wx.onMenuShareTimeline(option);
                        wx.onMenuShareAppMessage(option);
                        wx.onMenuShareQQ(option);
                        wx.onMenuShareWeibo(option);
                        wx.onMenuShareQZone(option);
                        wx.hideMenuItems({
                            menuList: ['menuItem:copyUrl']
                        });
                    })
                } else {
                    // alert(data.Message);
                }
            }
        })

    }

    function shareOK(TaskId, OrderUrl) {
        var channel=GetRequest().channel?GetRequest().channel:0;
        $.ajax({
            url: public_wxurl+'/api/Task/SubmitOrderUrl',
            type: 'post',
            dataType: 'json',
            data: {
                TaskId: TaskId,
                OrderUrl: OrderUrl,
                PromotionChannelID:channel
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                if (data.Status == 0) {

                } else {
                    // alert(data.Message);
                }
            }
        })
    }


})