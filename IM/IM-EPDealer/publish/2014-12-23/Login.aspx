﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IM系统登录</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="http://review.bitauto.com/bitauto.ico" rel="SHORTCUT ICON" />
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" language="javascript" type="text/javascript"></script>
</head>
<body>
    <div class="page950">
    <form runat="server" id="loginform" method="post" onkeydown="KeyDown(event)">
    <div class="logo"><h2>IM系统管理平台</h2></div>
	<div class="logo_info">
	    <h2>用户登录</h2>
        <em id="liError">&nbsp;</em>
		<ul class="list">
		    <li><i>帐号：</i><input onblur="ChangeText(this);" onclick="RemoveStyleAndText(this);"
                    type="text" id="username" name="username" class="inputborder k205 hui" value="请使用域帐号登录" />
		    </li>
		    <li><i>密码：</i><input name="password" id="password" type="password" class="inputborder k205" /></li>
		    <li class="two"><input name="ckxRememberMe" id="ckxRememberMe" checked="checked" type="checkbox"
                        value="" class="fxk" />记住域帐号</li>
		</ul>
        <div class="sumbit">
            <a onclick="javascript:return checkloginShowDiv();" id="btnLogin" class="bt_denglu"
                href="javascript:void(0);">登录</a></div>        
	</div>
	<div class="clear"></div>
    </form>
	<div class="footer">
	    <p>信息系统研发中心  任何建议和意见，请发邮件至：<a href="ISDC@bitauto.com">ISDC@bitauto.com</a></p>
	    <p>CopyRight   © Bitauto,All Rights Reserved 版权所有 北京易车互联信息技术有限公司</p>
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

        var name = $('#username');
        var pwd = $('#password');
        if ($('#ckxRememberMe').is(':checked')) {
            RememberMe();
        }
        var url = "../AjaxServers/LoginManager.ashx";
        
        var postBody = "isVal=yes&username=" + URLencode(name.val()) + "&pwd=" + URLencode(pwd.val()) + '&gourl=' + URLencode(request('gourl')); //构造要携带的数据 
        AjaxPost(url, postBody, PostPalyCallbackLoad);

        function PostPalyCallbackLoad(data, textStatus, xhr) {
            if (textStatus == 'success') {
                var s = data.split(',')[0];
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
                else if (s == '-10') {
                    var error = $('#liError');
                    error.html("IM系统帐号初始化失败,请联系管理IM系统帐号的网管");
                    error.attr('class', "error");
                }
                else if (s == '-11') {
                    var error = $('#liError');
                    error.html("该帐号在IM系统已登录");
                    error.attr('class', "error");
                }
                else if (s == '1') {
                    window.location = data.split(',')[1];
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
