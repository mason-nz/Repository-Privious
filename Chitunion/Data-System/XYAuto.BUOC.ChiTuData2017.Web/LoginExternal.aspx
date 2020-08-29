<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginExternal.aspx.cs" Inherits="XYAuto.BUOC.ChiTuData2017.Web.LoginExternal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>行圆汽车—赤兔平台—登录</title>
    <link rel="stylesheet" type="text/css" href="css/resetNew_login.css" />
    <link rel="stylesheet" type="text/css" href="css/layoutNew_login.css" />
    <script language="JavaScript" type="text/javascript" src="Js/jquery.1.11.3.min.js"></script>
</head>
<body onkeydown="KeyDown(event);" style="height: 100%;">
    <%--<div class="page950">
        <form runat="server" id="loginform" method="post" onkeydown="KeyDown(event)">
            <div class="logo">
                <h2>行圆汽车—流量变现业务管理平台</h2>
            </div>
            <div class="logo_info">
                <h2>用户登录</h2>
                <em id="liError">&nbsp;</em>
                <ul class="list">
                    <li><i>帐号：</i><input onblur="ChangeText(this);" onclick="RemoveStyleAndText(this);"
                        type="text" id="username" name="username" class="inputborder k205 hui" value="请使用域帐号登录" />
                    </li>
                    <li><i>密码：</i><input name="password" id="password" type="password" class="inputborder k205" /></li>
                    <li class="two">
                        <input name="ckxRememberMe" id="ckxRememberMe" checked="checked" type="checkbox"
                            value="" class="fxk" />记住域帐号 <em id="capStatus" style="visibility: hidden; display: inline;"
                                class="error">大写锁打开</em> </li>
                </ul>
                <div class="sumbit">
                    <a onclick="javascript:return checkloginShowDiv();" class="bt_denglu" href="#">登录</a>
                </div>
            </div>
            <div class="clear">
            </div>
        </form>
        <div class="footer" style="padding-top: 60px">
            <p>
                流量变现平台事业部 任何建议和意见，请发邮件至：<a href="mailto:masj@xingyuanauto.com">masj@xingyuanauto.com</a>
            </p>
            <p>
                CopyRight © 2000-<%=DateTime.Now.Year %>
                XYAuto,All Rights Reserved 版权所有 北京行圆汽车信息技术有限公司
            </p>
        </div>
    </div>--%>

    <form id="form1" runat="server" style="height: 100%;">
        <div class="login_box">
            <div class="login_left">
                <div class="left_logo">
                    <%--<a href="/" style="display: inline-block">--%>
                        <img src="/ImagesNew/loginLogoExternal.png" alt="" />
                    <%--</a>--%>
                </div>
            </div>
            <div class="login_right">
                <div class="box">
                    <div class="title">登录</div>
                    <!-- 提示信息 -->
                    <h2 id="H2ErrorNBYHMsg"></h2>
                    <div>
                        <input id="username" name="username" type="text" autocomplete="off" placeholder="请输入手机号" />
                    </div>
                    <div>
                        <input id="password" name="password" type="password" placeholder="请输入密码" autocomplete="off" />
                    </div>
                    <div>
                        <input id="txtCheckCode" name="txtCheckCode" type="text" style="width: 120px; margin-bottom: 0" autocomplete="off" placeholder="请输入验证码" />
                        <img id='imgcheckCode' src="CheckCode.aspx" style="cursor: pointer" />
                    </div>
                    <div>
                        <a href="javascript:void(0);" class="button" style="width: 205px; height: 28px; line-height: 28px;" onclick="javascript:return checkloginShowDiv();">登录</a>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function RemoveStyleAndText(obj) {
            if (obj.style.color != "black") {
                obj.value = "";
                obj.style.color = "black";
            }
        }
        function ChangeText(obj, txtContent) {
            if (obj.value.length == 0) {
                obj.value = txtContent;
                obj.style.color = "#999999";
            }
            else {
                obj.style.color = "black";
            }
        }
    </script>
    <script type="text/javascript" language="JavaScript">
        /**
        * Read the JavaScript cookies tutorial at:
        *   http://www.netspade.com/articles/javascript/cookies.xml
        */

        /**
        * Sets a Cookie with the given name and value.
        *
        * name       Name of the cookie
        * value      Value of the cookie
        * [expires]  Expiration date of the cookie (default: end of current session)
        * [path]     Path where the cookie is valid (default: path of calling document)
        * [domain]   Domain where the cookie is valid
        *              (default: domain of calling document)
        * [secure]   Boolean value indicating if the cookie transmission requires a
        *              secure transmission
        */
        function setCookie(name, value, expires, path, domain, secure) {
            document.cookie = name + "=" + escape(value) +
        ((expires) ? "; expires=" + expires.toGMTString() : "") +
        ((path) ? "; path=" + path : "") +
        ((domain) ? "; domain=" + domain : "") +
        ((secure) ? "; secure" : "");
        }
        //window.addEvent('domready', function () {
        //    function passCapsOn() {
        //        document.getElementById('capStatus').style.visibility = 'visible';
        //    }
        //    function passCapsOff() {
        //        document.getElementById('capStatus').style.visibility = 'hidden';
        //    }
        //    document.id('password').addEvents({
        //        'capsLockOn': passCapsOn,
        //        'capsLockOff': passCapsOff,
        //        'blur': passCapsOff,
        //        'focus': function (event) {
        //            if (event.hasCapsLock()) { passCapsOn(); }
        //        }
        //    });
        //});
        /**
        * Gets the value of the specified cookie.
        *
        * name  Name of the desired cookie.
        *
        * Returns a string containing value of specified cookie,
        *   or null if cookie does not exist.
        */
        function getCookie(name) {
            var dc = document.cookie;
            var prefix = name + "=";
            var begin = dc.indexOf("; " + prefix);
            if (begin == -1) {
                begin = dc.indexOf(prefix);
                if (begin != 0) return null;
            }
            else {
                begin += 2;
            }
            var end = document.cookie.indexOf(";", begin);
            if (end == -1) {
                end = dc.length;
            }
            return unescape(dc.substring(begin + prefix.length, end));
        }

        /**
        * Deletes the specified cookie.
        *
        * name      name of the cookie
        * [path]    path of the cookie (must be same as path used to create cookie)
        * [domain]  domain of the cookie (must be same as domain used to create cookie)
        */
        function deleteCookie(name, path, domain) {
            if (getCookie(name)) {
                document.cookie = name + "=" +
            ((path) ? "; path=" + path : "") +
            ((domain) ? "; domain=" + domain : "") +
            "; expires=Thu, 01-Jan-70 00:00:01 GMT";
            }
        }

    </script>
    <script type="text/javascript" language="JavaScript">
        function URLencode(sStr) {
            return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
        }
        function AjaxPost(url, postBody, PostPalyCallbackName) {
            $.post(url, postBody, PostPalyCallbackName);
        }
        $('#imgcheckCode').unbind('click').bind('click', function () {
            $(this).attr('src', 'CheckCode.aspx?r=' + Math.random());
        });
        //function AjaxPost(url, postBody, PostPalyCallbackName) {
        //    var myAjax = new Ajax.Request(url, { method: 'post', asynchronous: false, postBody: postBody, onComplete: PostPalyCallbackName });
        //}
        //输入回车时产生的事件，兼容IE和火狐
        function KeyDown(evt) {
            //这行代码用于兼容IE和火狐
            evt = evt = (evt) ? evt : ((window.event) ? window.event : "");
            if (evt.keyCode == 13) {
                evt.returnValue = false;
                evt.cancel = true;
                checkloginShowDiv();
            }
        }


        //绑定数据
        function checkloginShowDiv() {
            var name = $('#username');
            var pwd = $('#password');
            var checkCode = $('#txtCheckCode');
            var errorMsg = $('#H2ErrorNBYHMsg');
            var imgCheck = $('#imgcheckCode');

            if (name.val() == '') {
                errorMsg.text('请输入手机号'); name.focus(); return;
            }
            else if (pwd.val() == '') {
                errorMsg.text('请输入密码'); pwd.focus(); return;
            }
            else if (checkCode.val() == '') {
                errorMsg.text('请输入验证码'); checkCode.focus(); return;
            }


            var url = "/AjaxServers/LoginManager.ashx";

            var agentNum = '';
            var postBody = "action=login&username=" + URLencode(name.val()) + "&pwd=" + URLencode(pwd.val()) +
                "&checkCode=" + URLencode(checkCode.val()) +
                "&gourl=" + URLencode("<%=Request.QueryString["gourl"] %>"); //构造要携带的数据 
                AjaxPost(url, postBody, PostPalyCallbackLoad);

                function PostPalyCallbackLoad(data, textStatus, xhr) {
                    if (textStatus == 'success') {
                        //var jsonval = eval('(' + data + ')');
                        //var s = jsonval.ret;
                        var s = data.split(',')[0];

                        switch (s) {
                            case '-6':
                                errorMsg.text("验证码不正确"); checkCode.focus();
                                imgCheck.triggerHandler('click');
                                break;
                            case '-7'://库存经销商登陆接口，验证失败时
                                errorMsg.text(unescape(data.split(',')[1]));
                                imgCheck.triggerHandler('click');
                                break;
                            case '-8'://库存经销商登陆接口，登陆成功后，但账号状态为停用时
                                window.location = unescape(data.split(',')[1]);
                                break;
                            case '-3':
                                errorMsg.text("帐号已停用"); imgCheck.triggerHandler('click');
                                break;
                            case '-2':
                                errorMsg.text("帐户不存在"); imgCheck.triggerHandler('click');
                                break;
                            case '1'://登陆验证成功
                                window.location = data.split(',')[1];
                                break;
                            default:
                                errorMsg.text("用户名密码不匹配"); imgCheck.triggerHandler('click');
                                break;
                        }
                    }
                    else {
                        alert('请求错误');
                    }
                }
                return false;
            <%--var name = $('username');
            var pwd = $('password');
            var checkCode = $('txtCheckCode');
            if (name.value == "") {
                var error = $('H2ErrorNBYHMsg');
                error.innerHTML = "请输入域帐号";
                error.className = "error";
                return false;
            }
            if (pwd.value == "") {
                var error = $('H2ErrorNBYHMsg');
                error.innerHTML = "请输入密码";
                error.className = "error";
                return false;
            }
            if (checkCode.value == "") {
                var error = $('H2ErrorNBYHMsg');
                error.innerHTML = "请输入验证码";
                error.className = "error";
                return false;
            }
            //if ($('ckxRememberMe').checked) {
            //    RememberMe();
            //}
            var url = "../AjaxServers/LoginManager.aspx?gourl=<%=Request.QueryString["gourl"] %>";

            var postBody = "isVal=yes&username=" + URLencode(name.value) + "&pwd=" + URLencode(pwd.value) + "&checkCode=" + URLencode(checkCode.value); //构造要携带的数据 
            AjaxPost(url, postBody, PostPalyCallbackLoad);

            function PostPalyCallbackLoad(req) {
                if (req.status == 200) {
                    var jsonval = eval('(' + req.responseText + ')');
                    var s = jsonval.ret;
                    if (s == '-3') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "帐号已停用";
                        error.className = "error";

                    }
                    else if (s == '-2') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "帐户不存在，请联系销售管理中心";
                        error.className = "error";

                    }
                    else if (s == '-6') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "帐户被禁用,请联系网管";
                        error.className = "error";

                    }
                    else if (s == '-7') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "用户不存在,请联系网管";
                        error.className = "error";
                    }
                    else if (s == '-8') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "域帐号或密码错误";
                        error.className = "error";
                    }
                    else if (s == '-9') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "帐号已锁定，半小时后自动解锁。紧急情况请联系（masj@xingyuanauto.com）";
                        error.className = "error";
                    } else if (s == '-11') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "帐号已锁定，请联系（masj@xingyuanauto.com）";
                        error.className = "error";
                    } else if (s == '-10') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "账号长期未使用已休眠，请联系（masj@xingyuanauto.com）";
                        error.className = "error";
                    }
                    else if (s == '-6') {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "验证码不正确";
                        error.className = "error";
                        checkCode.focus();
                        imgCheck.triggerHandler('click');
                    }
                    else if (s == '1') {
                        window.location = jsonval.gurl;
                    }
                    else {
                        var error = $('H2ErrorNBYHMsg');
                        error.innerHTML = "域帐号或密码错误";
                        error.className = "error";
                    }
                }
                else {
                    alert(req.status);
                    alert(req.responseText);
                }
            }
            return false;--%>
        }
        function hidDiv() {
            var err = $('liError');
            err.className = "hid";
        }
        function checklogin(form) {
            if (form.username.value == "") {
                alert("请输入域帐号");
                return false;
            }
            if (form.password.value == "") {
                alert("请输入密码");
                return false;
            }
            return true;
        }
        function RememberMe() {
            setCookie('sysloginusername', $('username').value, new Date('2100/01/01'));
        }
        function GetLoginUser() {
            if (getCookie('sysloginusername')) {
                $('username').value = getCookie('sysloginusername');
                $('username').style.color = "black";
            }
        }
        GetLoginUser();
    </script>
</body>
</html>
