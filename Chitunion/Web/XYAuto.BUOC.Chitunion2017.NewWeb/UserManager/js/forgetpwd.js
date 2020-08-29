


var ForgetpwdHelper = window.ForgetpwdHelper = (function () {



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

    //是否满足用户名格式（字母、数字、-、_两种及以上组合的4-20个字符）
    function isUserName(strVal) {
        if (!(/(?!^[a-zA-Z]+$)(?!^[\d]+$)(?!^[^a-zA-Z-_\d]+$)^.{4,20}$/.test(strVal))) {
            //alert("手机号码有误，请重填");
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

    var Forgetpwd = function (categoryid) {
        if (categoryid == 29001) {//广告主
            //var name = $('#txtGGZUserName');
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
            //var name = $('#txtMTZUserName');
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

        if (Verify(categoryid)) {
            var url = "/AjaxServers/LoginManager.ashx";

            var agentNum = '';
            var postBody = {
                action: "forgetpwd",
                category: categoryid,
                //username: URLencode(name.val()),
                pwd: URLencode(pwd.val()),
                pwdConfirm: URLencode(pwdConfirm.val()),
                mobile: URLencode(mobile.val()),
                //checkCode: URLencode(checkCode.val()),
                mobileCheckCode: URLencode(mobileCheckCode.val()),
                gourl: URLencode(request('gourl')),
                r: Math.random()
            };
            AjaxPost(url, postBody, function ForgetpwdCallbackLoad(data, textStatus, xhr) {
                if (textStatus == 'success') {
                    var jsonData = $.evalJSON(data);

                    switch (jsonData.result) {
                        case -1:
                            errorMsg.prev('img').css('visibility', 'visible');
                            errorMsg.text(jsonData.msg); //sendMobileMsg.focus();
                            //imgCheck.triggerHandler('click');
                            break;
                        case 0://注册成功
                            //if (jsonData.msg != '') {
                            //    window.location = jsonData.msg;
                            //}
                            //alert('重置密码成功');
                            layer.msg('成功', { 'time': 2000 }, function () {
                                window.location = '/login.aspx' + (jsonData.msg != '' ? '?gourl=' + URLencode(jsonData.msg) : '');
                            });
                            break;
                        default:
                            errorMsg.prev('img').css('visibility', 'visible');
                            errorMsg.text("重置密码失败"); //imgCheck.triggerHandler('click');
                            break;
                    }
                }
                else {
                    alert('请求错误');
                }
            });
            return false;
        }
    };

    var Verify = function (categoryid) {
        var flag = false;
        if (categoryid == 29001) {//广告主
            //var name = $('#txtGGZUserName');
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
            //var name = $('#txtMTZUserName');
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

        if (mobile.val() == '') {
            errorMsg.text('请填写手机号'); mobile.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
        else if (!mobile.val().isMobile()) {
            errorMsg.text('请填写正确的手机号'); mobile.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
            //else if (checkCode.val() == '') {
            //    errorMsg.text('请输入验证码'); checkCode.focus();
            //    errorMsg.prev('img').css('visibility', 'visible');
            //}
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
        else if (mobileCheckCode.val() == '') {
            errorMsg.text('请输入手机验证码'); mobileCheckCode.focus();
            errorMsg.prev('img').css('visibility', 'visible');
        }
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
            Forgetpwd($('ul.nav li.selected:first').attr('category'));
        }
    }

    var SendMobileMsg = function () {
        var category = $('ul.nav li.selected:first').attr('category');
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
        //    errorMsg.text('请输入验证码'); checkCode.focus();
        //    errorMsg.prev('img').css('visibility', 'visible');
        //    return;
        //}

        var url = "/AjaxServers/LoginManager.ashx";
        var postBody = {
            action: "sendmobilemsg_forgetpwd",
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
                        errorMsg.text("验证码不正确"); //checkCode.focus();
                        //imgCheck.triggerHandler('click');
                        break;
                    case '-10':
                        errorMsg.text("验证码超时时间未到，不能获取"); //checkCode.focus();
                        //imgCheck.triggerHandler('click');
                        break;
                    case '-11':
                        errorMsg.text("发送短信失败"); //checkCode.focus();
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
                    ForgetpwdHelper.SendMobileMsg();
                });
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

    return {
        Forgetpwd: Forgetpwd,
        SendMobileMsg: SendMobileMsg,
        KeyDown: KeyDown,
        SetTimeOutIndex: SetTimeOutIndex,
        Verify: Verify
    }
})();

$(document).ready(function () {
    $('#btnGGZSendMobileMsg,#btnMTZSendMobileMsg').unbind('click').bind('click', function () {
        ForgetpwdHelper.SendMobileMsg();
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