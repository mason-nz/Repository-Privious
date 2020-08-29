<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BitAuto.ISDC.CC2012.WebAPI.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>易车客户呼叫中心系统-登录</title>
    <link href="http://review.bitauto.com/bitauto.ico" rel="SHORTCUT ICON" />
    <link href="Content/Css/login.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/HighChart/jquery1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var browerVersion = 0;
        $(document).ready(function () {
            var Sys = {};
            var ua = navigator.userAgent.toLowerCase();
            var s;
            (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] : 0;
            var brower = 'other';
            if ($.browser.msie) {
                //检测是否是IE6，是-则关闭页面
                if ($.browser.version == 6) {
                    browerVersion = 6;
                    alert("请升级IE6版本！");
                    try {
                        window.external.MethodScript('/browsercontrol/closepage');
                    } catch (e) {
                        window.opener = null; window.open('', '_self'); window.close();
                    }
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

            $('#spanBrower').html('当前浏览器：' + brower);
        });
    </script>
    <%--<script src="ADT/ADTTool.js" type="text/javascript"></script>--%>
    <%--<style type="text/css">
        .login .hid
        {
            visibility: hidden;
            height: 22px;
            margin: 12px 0 0 20px;
            padding-left: 30px;
        }
        .show
        {
            display: inline;
        }
    </style>--%>
</head>
<body>
    <div class="page950">
        <form runat="server" id="loginform" method="post" onkeydown="KeyDown(event)">
        <div class="logo">
            <h2>
                易车销售管理平台</h2>
        </div>
        <div class="logo_info">
            <h2>
                用户登录</h2>
            <%--<a href="/cti/CTI_Test.aspx">CallTest</a>--%>
            <span id="spanBrower"></span><em id="liError">&nbsp;</em>
            <ul class="list">
                <li><i>帐号：</i><input onblur="ChangeText(this);" onclick="RemoveStyleAndText(this);"
                    type="text" id="username" name="username" class="inputborder k205 hui" value="请使用域帐号登录" />
                </li>
                <li><i>密码：</i><input name="password" id="password" type="password" class="inputborder k205" /></li>
                <li class="two">
                    <input name="ckxRememberMe" id="ckxRememberMe" checked="checked" type="checkbox"
                        value="" class="fxk" />记住域帐号</li>
            </ul>
            <div class="sumbit">
                <a onclick="javascript:return checkloginShowDiv();" id="btnLogin" class="bt_denglu"
                    href="javascript:void(0);">登录</a></div>
            <div class="sumbit" style="text-align: center;">
                <%--<a href="loginform.aspx" style=" color:Blue;">点击进入模拟登陆</a>--%>
            </div>
        </div>
        <div class="clear">
        </div>
        </form>
        <div class="footer" style="padding-top: 120px">
            <p style="color: #999; text-align: center; font-family: 宋体">
                数据系统中心 任何建议和意见，请进入 <a style="" target="_blank" href="http://sys.bitauto.com/systemManager/AddFeedback.aspx?SysID=SYS011">
                    销售管理系统-反馈中心</a>
            </p>
            <p style="color: #999; text-align: center; line-height: 24px; vertical-align: bottom;
                font-family: 宋体">
                <span style="font-family: Times New Roman">CopyRight &copy; 2000-2014 Bitauto,All Rights
                    Reserved</span> 版权所有 北京易车互联信息技术有限公司</p>
        </div>
    </div>
</body>
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
    function RemoveStyleAndText(obj) {
        if (obj.style.color != "black") {
            obj.value = "";
            obj.style.color = "black";
        }
    }
    function ChangeText(obj) {
        if (obj.value.length == 0) {
            obj.value = "请使用域帐号登录";
            obj.style.color = "#cccccc";
        }
        else {
            obj.style.color = "black";
        }
    }
    function URLencode(sStr) {
        return escape(sStr).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    }
    function AjaxPost(url, postBody, PostPalyCallbackName) {
        //        var myAjax = new Ajax.Request(url, { method: 'post', asynchronous: false, postBody: postBody, onComplete: PostPalyCallbackName });
        $.post(url, postBody, PostPalyCallbackName);
    }

    //绑定数据
    function checkloginShowDiv() {
        //检测是否是IE6，是-则关闭页面
        if (browerVersion == 6) {
            alert("请升级IE6版本！");
            try {
                window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            }
            return false;
        }

        var name = $('#username');
        var pwd = $('#password');
        if ($('#ckxRememberMe').is(':checked')) {
            RememberMe();
        }
        var url = "../api/APILogin/CommonLogin";

        var agentNum = '';
        //        if (ADTTool && ADTTool.UserName) {
        //            agentNum = ADTTool.UserName;
        //        }
        //        var postBody = "isVal=yes&username=" + URLencode(name.val()) + "&pwd=" + URLencode(pwd.val()) + "&AgentNum=" + agentNum + '&gourl=' + URLencode(request('gourl')); //构造要携带的数据 
        var postBody = "u=" + URLencode(name.val()) + "&p=" + URLencode(pwd.val()); //构造要携带的数据 
        //AjaxPost(url, postBody, PostPalyCallbackLoad);
        $.getJSON(url + "?" + postBody, null, PostPalyCallbackLoad);


        function PostPalyCallbackLoad(data, textStatus, xhr) {
            if (textStatus == 'success') {
                var s = data.ErrorNumber;
                if (s == '-3') {
                    var error = $('#liError');
                    error.html("帐号已停用");
                    error.attr('class', "error");

                }
                else if (s == '-2') {
                    var error = $('#liError');
                    error.html("帐户未关联或者不存在，请联系管理员");
                    error.attr('class', "error");

                }
                else if (s == '-6') {
                    var error = $('#liError');
                    error.html("帐户被禁用,请联系管理域帐号的网管");
                    error.attr('class', "error");

                }
                else if (s == '-7') {
                    var error = $('#liError');
                    error.html("用户不存在,请联系管理域帐号的网管");
                    error.attr('class', "error");
                }
                else if (s == '-8') {
                    var error = $('#liError');
                    error.html("密码错误,请联系管理域帐号的网管");
                    error.attr('class', "error");
                }
                else if (s == '-9') {
                    var error = $('#liError');
                    error.html("工号与用户帐户不对应,请联系管理域帐号的网管");
                    error.attr('class', "error");
                }
                else if (s == '0') {
                    //window.location = ""; //data.split(',')[1];
                    alert("登录成功");
                }
                else {
                    var error = $('#liError');
                    error.html("用户名密码不匹配");
                    error.attr('class', "error");
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
    function hidDiv() {
        var err = $('#liError');
        err.attr('class', "hid");
    }
    function checklogin(form) {
        if (form.username.val() == "") {
            alert("请输入用户登录名");
            return false;
        }
        if (form.password.val() == "") {
            alert("请输入用户密码");
            return false;
        }
        return true;
    }
    function RememberMe() {
        setCookie('mmploginusername', $('#username').val(), new Date('2100/01/01'));
    }
    function GetLoginUser() {
        if (getCookie('mmploginusername')) {
            $('#username').val(getCookie('mmploginusername'));
        }
    }
    GetLoginUser();
</script>
</html>
