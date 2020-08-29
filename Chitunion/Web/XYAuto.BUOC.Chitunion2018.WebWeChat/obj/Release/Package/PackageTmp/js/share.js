/**
 * Written by:     zhengxh
 * Created Date:   2018/2/7
 */


    var nativeShare = new NativeShare()
    var shareData = {
        title: document.title,
        desc: document.title,
        // 如果是微信该link的域名必须要在微信后台配置的安全域名之内的。
        link: window.location+'',
        icon: $('#M_img_pic').attr('imgPic')?$('#M_img_pic').attr('imgPic'):'',
        // 不要过于依赖以下两个回调，很多浏览器是不支持的
        success: function () {
            wu.showToast({
                title: '分享成功',
                mask: false,                //是否可以操作dom
                icon: 'icon-success',   // icon-success | icon-error | icon-info
                duration: 3000
            });
        },
        fail: function () {
            wu.showToast({
                title: '取消分享',
                mask: false,                //是否可以操作dom
                icon: 'icon-success',   // icon-success | icon-error | icon-info
                duration: 3000
            });
        }
    }
    nativeShare.setShareData(shareData)

    function setTitle(title) {
        nativeShare.setShareData({
            title: title,
        })
    }
    // 用于获取是否是Safari浏览器
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
            (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
                (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
                    (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
    // 判断是哪个浏览器
    var browser={
        versions:function(){
            var u = navigator.userAgent, app = navigator.appVersion;
            var ua = navigator.userAgent.toLowerCase();
            return {
                mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
                ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                android: u.indexOf('Android') > -1 || u.indexOf('Adr') > -1, //android终端
                iPhone: u.indexOf('iPhone') > -1 , //是否为iPhone或者QQHD浏览器
                iPad: u.indexOf('iPad') > -1, //是否iPad
                webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
                weixin:u.indexOf('MicroMessenger') > -1, //是否微信 （2015-01-22新增）
                qq: u.match(/\sQQ/i) == "qq", //是否QQ
                isweixin:ua.match(/MicroMessenger/i)=="micromessenger",
                safari:u.toLowerCase().indexOf("safari") > 0,
                qqbrowser:u.indexOf('QQBrowser')>0,
                ucbrowser:u.indexOf('UCBrowser')>0,
                baidubrowser:u.indexOf('baidubrowser')>0,//baidu
                qhbrowser:u.indexOf('QHBrowser')>0//360
            };
        }()
    }

    // 判断当前是否是移动端打开
    if(browser.versions.mobile) {
        if (browser.versions.isweixin) {//微信
            $('#M_qzone').attr('onclick','')
            $('.M_weiboHide').addClass('M_weiboHide -mob-share-weibo').attr('onclick','')
            // 用于显示微信遮罩层
            $('.M_wechatFriend').on('click',function () {
                $('.share_mark').show()
                    .on('click',function () {
                    $('.share_mark').hide()
                })
            })
            $('.M_wechatTimeline').on('click',function () {
                $('.M_wechatFriend').click()
            })
            $('.M_qqFriend').on('click',function () {
                $('.M_wechatFriend').click()
            })

            $('.M_wechatFriend').attr('onclick','')
            $('.M_wechatTimeline').attr('onclick','')
            $('.M_qqFriend').attr('onclick','')

        } else if(browser.versions.qqbrowser||browser.versions.ucbrowser){//qq浏览器和uc浏览器
            var isAndroid = navigator.userAgent.indexOf('Android') > -1 || navigator.userAgent.indexOf('Adr') > -1; //android终端
            var isiOS = !!navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
            if(isAndroid){
                // 在其他浏览器中隐藏微信、朋友圈、qq
                $('.M_wechatFriend').hide();
                $('.M_wechatTimeline').hide();
                $('.M_qqFriend').hide();
                $('#M_qzone').attr('onclick','')
                $('.M_weiboHide').attr('onclick','')
            }else if(isiOS){
                $('#M_qzone').attr('class','')
                $('.M_weiboHide').attr('class','M_weiboHide')
                function call(command) {
                    try {
                        nativeShare.call(command)
                    } catch (err) {
                        // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                    }
                }
            }else {
                $('#M_qzone').attr('class','')
                function call(command) {
                    try {
                        nativeShare.call(command)
                    } catch (err) {
                        // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                    }
                }
            }
        }else if(navigator.userAgent.indexOf('QQ') > -1){//qq
            // 在qq内置浏览器中隐藏微信、朋友圈
            $('.M_wechatFriend').hide();
            $('.M_wechatTimeline').hide();
            $('.M_weiboHide').addClass('M_weiboHide -mob-share-weibo').attr('onclick','')
            $('#M_qzone').attr('class','')
            function call(command) {
                try {
                    nativeShare.call(command)
                } catch (err) {
                    // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                }
            }
        }else if(Sys.safari){//safari
            $('.M_qqFriend').hide();
            $('.M_wechatFriend').on('click',function () {
                $('.safari_mask').show().off('click').on('click',function () {
                    $('.safari_mask').hide()
                    $('.wx_fixed_tip').hide()
                })
                $('.wx_fixed_tip').show().off('click').on('click',function () {
                    $('.safari_mask').hide()
                    $('.wx_fixed_tip').hide()
                })
            })
            $('.M_wechatTimeline').on('click',function () {
                $('.M_wechatFriend').click()
            })
            $('#M_qzone').attr('onclick','')
            $('.M_weiboHide').attr('class','M_weiboHide')
            function call(command) {
                try {
                    nativeShare.call(command)
                } catch (err) {
                    // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                }
            }
        }else {//其他浏览器
            // 在其他浏览器中隐藏微信、朋友圈、qq
            $('.M_wechatFriend').hide();
            $('.M_wechatTimeline').hide();
            $('.M_qqFriend').hide();
            $('#M_qzone').attr('onclick','')
            $('.M_weiboHide').attr('class','M_weiboHide')
            function call(command) {
                try {
                    nativeShare.call(command)
                } catch (err) {
                    // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                }
            }
        }
    }else { //pc端
        if(Sys.safari){//safari
            $('.M_wechatFriend').hide();
            $('.M_wechatTimeline').hide();
            $('#M_qzone').attr('onclick','')
            function call(command) {
                try {
                    nativeShare.call(command)
                } catch (err) {
                    // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                }
            }
        }else {
// 在其他浏览器中隐藏微信、朋友圈、qq
            $('.M_wechatFriend').hide();
            $('.M_wechatTimeline').hide();
            $('.M_qqFriend').hide();
            $('#M_qzone').attr('onclick','')
            function call(command) {
                try {
                    nativeShare.call(command)
                } catch (err) {
                    // 如果不支持，你可以在这里做降级处理
//            alert(err.message,1)
                }
            }
        }
    }



    //    mob分享
    mobShare.config( {

        debug: true, // 开启调试，将在浏览器的控制台输出调试信息

        appkey: '23ef9117a062a', // appkey

        params: {
            url: window.location+'', // 分享链接
            title: document.title, // 分享标题
            description: document.title, // 分享内容
            pic: $('#M_img_pic').attr('imgPic')?$('#M_img_pic').attr('imgPic'):'', // 分享图片，使用逗号,隔开
            reason:'',//自定义评论内容，只应用与QQ,QZone与朋友网
        },

        /**
         * 分享时触发的回调函数
         * 分享是否成功，目前第三方平台并没有相关接口，因此无法知道分享结果
         * 所以此函数只会提供分享时的相关信息
         *
         * @param {String} plat 平台名称
         * @param {Object} params 实际分享的参数 { url: 链接, title: 标题, description: 内容, pic: 图片连接 }
         */
        callback: function( plat, params ) {
        }

    } );
