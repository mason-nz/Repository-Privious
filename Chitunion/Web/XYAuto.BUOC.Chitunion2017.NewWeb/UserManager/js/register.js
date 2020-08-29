




var RegisterHelper = window.RegisterHelper = (function () {



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
    function URLencode(sStr) {
        return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    }

    //是否满足用户名格式（字母、数字、-、_这些任意组合的4-20个字符，且字母开头）
    function isUserName(strVal) {
        //（字母、数字、-、_两种及以上组合的4-20个字符）
        //if (!(/(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$/.test(strVal))) {
        if (!(/^[a-zA-Z][\w-]{3,19}$/.test(strVal))) {
            return false;
        }
        if ((/[\u4e00-\u9fa5]/).test(strVal)) {
            return false;
        }
        return true;
    }
    function AjaxPost(url, postBody, PostPalyCallbackName) {
        $.post(url, postBody, PostPalyCallbackName);
    }
    var browerObj = jQuery.browser;
    //检测是否是IE6，是-则关闭页面
    if (browerObj.msie && browerObj.version < 8) {
        alert("请升级IE版本！");
        window.opener = null; window.open('', '_self'); window.close();
        return false;
    }

    var Register = function (categoryid, result) {
        if (categoryid == 29001) {//广告主
            var name = $('#txtGGZUserName');
            var pwd = $('#txtGGZPwd');
            var pwdConfirm = $('#txtGGZPwdConfirm');
            var mobile = $('#txtGGZMobile');
            //var checkCode = $('#txtGGZCheckCode');
            var mobileCheckCode = $('#txtGGZMobileCheckCode');
            //var imgCheck = $('#imgGGZ');
            var sendMobileMsg = $('#btnGGZSendMobileMsg');
            //var chx_Readme = $('#chx_GGZReadme');
            var errorMsg = $('#divErrorGGZMsg');
        }
        else if (categoryid == 29002) {//媒体主
            var name = $('#txtMTZUserName');
            var pwd = $('#txtMTZPwd');
            var pwdConfirm = $('#txtMTZPwdConfirm');
            var mobile = $('#txtMTZMobile');
            //var checkCode = $('#txtMTZCheckCode');
            var mobileCheckCode = $('#txtMTZMobileCheckCode');
            //var imgCheck = $('#imgMTZ');
            var sendMobileMsg = $('#btnMTZSendMobileMsg');
            //var chx_Readme = $('#chx_MTZReadme');
            var errorMsg = $('#divErrorMTZMsg');
        }

        if (Verify(categoryid)) {
            var url = "/AjaxServers/LoginManager.ashx";
            var postBody = {
                action: "register",
                category: categoryid,
                username: URLencode(name.val()),
                pwd: URLencode(pwd.val()),
                pwdConfirm: URLencode(pwdConfirm.val()),
                mobile: URLencode(mobile.val()),
                //checkCode: URLencode(checkCode.val()),
                mobileCheckCode: URLencode(mobileCheckCode.val()),
                gourl: URLencode(request('gourl')),
                r: Math.random(),
                geetest_challenge: result ? result.geetest_challenge : '',
                geetest_validate: result ? result.geetest_validate : '',
                geetest_seccode: result ? result.geetest_seccode : ''
            };
            AjaxPost(url, postBody, function (data, textStatus, xhr) {
                if (textStatus == 'success') {
                    var jsonData = $.evalJSON(data);

                    switch (jsonData.result) {
                        case -1:
                            errorMsg.prev('img').css('visibility', 'visible');
                            errorMsg.text(jsonData.msg); mobileCheckCode.focus();
                            //imgCheck.triggerHandler('click');
                            break;
                        case 0://注册成功
                            if (jsonData.msg != '') {
                                window.location = jsonData.msg;
                            }
                            //alert('注册成功');
                            break;
                        default:
                            errorMsg.text("用户注册失败"); //imgCheck.triggerHandler('click');
                            break;
                    }
                }
                else {
                    //                alert(req.status);
                    //                alert(req.responseText);
                    alert('请求错误');
                }
            });
            return false;
        }


    };

    var Verify = function (categoryid) {
        var flag = false;
        if (categoryid == 29001) {//广告主
            var name = $('#txtGGZUserName');
            var pwd = $('#txtGGZPwd');
            var pwdConfirm = $('#txtGGZPwdConfirm');
            var mobile = $('#txtGGZMobile');
            //var checkCode = $('#txtGGZCheckCode');
            var mobileCheckCode = $('#txtGGZMobileCheckCode');
            //var imgCheck = $('#imgGGZ');
            var sendMobileMsg = $('#btnGGZSendMobileMsg');
            //var chx_Readme = $('#chx_GGZReadme');
            var errorMsg = $('#divErrorGGZMsg');
        }
        else if (categoryid == 29002) {//媒体主
            var name = $('#txtMTZUserName');
            var pwd = $('#txtMTZPwd');
            var pwdConfirm = $('#txtMTZPwdConfirm');
            var mobile = $('#txtMTZMobile');
            //var checkCode = $('#txtMTZCheckCode');
            var mobileCheckCode = $('#txtMTZMobileCheckCode');
            //var imgCheck = $('#imgMTZ');
            var sendMobileMsg = $('#btnMTZSendMobileMsg');
            //var chx_Readme = $('#chx_MTZReadme');
            var errorMsg = $('#divErrorMTZMsg');
        }

        var divContent = $('div.two_rules.registered div.account ul.all_input li:last img');
        divContent.css('visibility', 'hidden');
        errorMsg.html('&nbsp;');

        if (name.val() == '') {
            errorMsg.text('请输入用户名'); name.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (!isUserName(name.val())) {
            errorMsg.text('可使用字母、数字、"_"、"-"需以字母开头!'); name.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (pwd.val() == '') {
            errorMsg.text('请输入密码'); pwd.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (pwd.val().length < 6 || pwd.val().length > 20) {
            errorMsg.text('密码长度应为6~20位字符！'); pwd.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (pwd.val() != pwdConfirm.val()) {
            errorMsg.text('确认密码与密码不一致，请重新输入！'); pwdConfirm.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (mobile.val() == '') {
            errorMsg.text('请填写手机号'); mobile.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (!mobile.val().isMobile()) {
            errorMsg.text('请填写正确的手机号'); mobile.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
            //else if (checkCode.val() == '') {
            //    checkCode.parent().parent().next('div.notes').text('请输入验证码'); checkCode.focus();
            //    checkCode.parent().find('img').css('visibility', 'visible');
            //}
        else if (mobileCheckCode.val() == '') {
            errorMsg.text('请输入手机验证码'); mobileCheckCode.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
            //else if (!chx_Readme.is(':checked')) {
            //    mobileCheckCode.parent().parent().next('div.notes').text('请阅读并同意服务条款'); checkCode.focus();
            //}
        else {
            flag = true;
        }
        return flag;
    };

    //输入回车时产生的事件，兼容IE和火狐
    var KeyDown = function (evt) {
        //这行代码用于兼容IE和火狐
        evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
        if (evt.keyCode == 13) {
            evt.returnValue = false;
            evt.cancel = true;
            //Register($('ul.menu li.active:first').attr('category'));
            var categoryid = $('ul.nav li.selected:first').attr('category');
            $('div.two_rules.registered div.account div.btn a.login_btn[category=' + categoryid + ']').triggerHandler('click');
        }
    }

    var SendMobileMsg = function () {
        var category = $('ul.nav li.selected').attr('category');
        if (category == '29001') {
            var mobile = $('#txtGGZMobile');
            //var checkCode = $('#txtGGZCheckCode');
            var sendMobileMsg = $('#btnGGZSendMobileMsg');
            var errorMsg = $('#divErrorGGZMsg');
            //var imgCheck = $('#imgGGZ');
        }
        else if (category == '29002') {
            var mobile = $('#txtMTZMobile');
            //var checkCode = $('#txtMTZCheckCode');
            var sendMobileMsg = $('#btnMTZSendMobileMsg');
            var errorMsg = $('#divErrorMTZMsg');
            //var imgCheck = $('#imgMTZ');
        }
        var divContent = $('div.two_rules.registered div.account ul.all_input li:last img');
        divContent.css('visibility', 'hidden');
        errorMsg.html('&nbsp;');
        if (mobile.val() == '') {
            errorMsg.text('请输入手机号'); mobile.focus();
            errorMsg.prev('img').css('visibility', 'visible');
            return;
        }
        //if (checkCode.val() == '') {
        //    checkCode.parent().parent().next('div.notes').text('请输入验证码'); checkCode.focus();
        //    checkCode.parent().find('img').css('visibility', 'visible');
        //    return;
        //}

        var url = "/AjaxServers/LoginManager.ashx";
        var postBody = {
            action: "sendmobilemsg_register",
            mobile: URLencode(mobile.val()),
            //checkCode: URLencode(checkCode.val()),
            r: Math.random()
        };
        //InvokeSettime(sendMobileMsg);
        AjaxPost(url, postBody, SendMobileMsgCallbackLoad);

        function SendMobileMsgCallbackLoad(data, textStatus, xhr) {
            if (textStatus == 'success') {
                var s = data.split(',')[0];
                errorMsg.prev('img').css('visibility', 'visible');
                switch (s) {
                    case '-9':
                        errorMsg.text("验证码不正确"); sendMobileMsg.focus();
                        //imgCheck.triggerHandler('click');
                        break;
                    case '-10':
                        errorMsg.text("验证码超时时间未到，不能获取"); sendMobileMsg.focus();
                        //imgCheck.triggerHandler('click');
                        break;
                    case '-11':
                        errorMsg.text("发送短信失败"); sendMobileMsg.focus();
                        //imgCheck.triggerHandler('click');
                        break;
                    case '0':
                        errorMsg.text("短信已发送"); //imgCheck.triggerHandler('click');
                        //sendMobileMsg.removeClass('but_register');
                        sendMobileMsg.attr("disabled", true);
                        sendMobileMsg.removeAttr('href').unbind('click').css('cursor', 'default');

                        //倒计时
                        InvokeSettime(sendMobileMsg);
                        break;
                }
            }
            else {
                //                alert(req.status);
                //                alert(req.responseText);
                alert('请求错误');
            }
        }
        return false;

    }
    var timeOutIndex;
    var SetTimeOutIndex = function () {
        return timeOutIndex;
    }
    var InvokeSettime = function (obj) {
        var countdown = 60;
        settime(obj);
        function settime(obj) {
            if (countdown == 0) {
                $(obj).text('获取验证码').attr("disabled", false);
                $(obj).attr('href', 'javascript:void(0);').css('cursor', 'pointer');
                $(obj).unbind('click').bind('click', function () {
                    RegisterHelper.SendMobileMsg();
                });
                //$(obj).removeAttr('href').unbind('click');//.next('span.repeatSecondMsg').remove();
                countdown = 60;
                return;
            } else {
                $(obj).attr("disabled", true);
                $(obj).removeAttr('href').unbind('click').css('cursor', 'default');
                var msg = "(" + countdown + ")s重新发送";
                $(obj).text(msg);
                //if ($(obj).next('span.repeatSecondMsg').size() > 0) {
                //    $(obj).next('span:first').text(msg);
                //}
                //else {
                //    $(obj).after($('<span class="repeatSecondMsg">').text(msg));
                //}
                countdown--;
            }
            timeOutIndex = setTimeout(function () {
                settime(obj)
            }
                    , 1000);
        }
    }
    var InitVerifyCode = function (initVerifyCodecallback) {
        if (!initVerifyCodecallback) { initVerifyCodecallback = function () { }; }
        var setting = {
            url: '/ajaxservers/geetest/getcaptcha.ashx',
            type: 'post',
        };
        setAjax(setting, function (data) {
            // 使用initGeetest接口
            // 参数1：配置参数
            // 参数2：回调，回调的第一个参数验证码对象，之后可以使用它做appendTo之类的事件
            initGeetest({
                gt: data.gt,
                challenge: data.challenge,
                product: "bind", // 产品形式，包括：float，embed，popup。注意只对PC版验证码有效
                offline: !data.success, // 表示用户后台检测极验服务器是否宕机，一般不需要关注
                new_captcha: data.new_captcha
            }, function (captchaObj) {

                captchaObj.onReady(function () {
                    //$("#wait").hide();
                    initVerifyCodecallback(captchaObj);//执行回调函数
                }).onSuccess(function () {
                    var result = captchaObj.getValidate();
                    if (!result) {
                        return alert('请完成验证');
                    }
                    var categoryid = $('ul.nav li.selected[category]').attr('category');
                    RegisterHelper.Register(categoryid, result);
                });

                $('div.two_rules.registered div.account div.btn a.login_btn').unbind('click').bind('click', function () {
                    // 调用之前先通过前端表单校验
                    var categoryid = $('ul.nav li.selected[category]').attr('category');
                    if (RegisterHelper.Verify(categoryid)) {
                        captchaObj.verify();
                    }
                });
            });
        },
            function () { alert('error'); }
        );
    }

    return {
        Register: Register,
        SendMobileMsg: SendMobileMsg,
        KeyDown: KeyDown,
        SetTimeOutIndex: SetTimeOutIndex,
        Verify: Verify,
        InitVerifyCode: InitVerifyCode
    }



})();

//RegisterHelper.InitVerifyCode();

$(document).ready(function () {
    $('#btnGGZSendMobileMsg,#btnMTZSendMobileMsg').unbind('click').bind('click', function () {
        RegisterHelper.SendMobileMsg();
    });


    //绑定注册点击事件
    $('div.two_rules.registered div.account div.btn a.login_btn').unbind('click').bind('click', function () {
        var categoryid = $(this).attr('category');
        if (RegisterHelper.Verify(categoryid)) {

            var settingVerify = {
                url: '/ajaxservers/LoginManager.ashx',
                type: 'post',
                data: {
                    action: "islaunchverify",
                    r: Math.random()
                }
            };
            setAjax(settingVerify, function (data) {
                if (data && data == 1) {
                    RegisterHelper.InitVerifyCode(function (captchaObj) {
                        $('div.two_rules.registered div.account div.btn a.login_btn[category=' + categoryid + ']').triggerHandler('click');
                    });
                }
                else {
                    RegisterHelper.Register(categoryid);

                }
            }, function () { alert('error'); }
            );
        }
    });

    //绑定微信登陆逻辑（媒体主）
    $('a[name="aWxLogin"]').unbind('click').bind('click', function () {
        var url = '/login.aspx';
        var category = $('ul.nav li.selected').attr('category');
        //var gourl = request('gourl');
        if (category && category == '29002') {
            url = url + '?type=2&s=wx';
        }
        //if (gourl != '') {
        //    url = url + (url.indexOf('?') >= 0 ? '&' : '?') + 'gourl=' + gourl;
        //}
        window.location = url;
    });
});