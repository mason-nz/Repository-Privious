<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_new.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.Login_new" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="description" content="描述文字，字数200内？">
    <meta name="keywords" content="网站相关关键字，字数200内？">
	<title>用户登录-赤兔联盟广告在线交易平台</title>
	<link rel="stylesheet" type="text/css" href="css/resetNew.css"/>
    <link rel="stylesheet" type="text/css" href="css/layoutNew.css"/>
    <script type="text/javascript" src="js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="js/jquery.browser.js"></script>
    <!--<script language="javascript" src="/api/check.ashx?NotCheckModule=true&NotRedirectURL=true" type="text/javascript"></script>-->
    <script type="text/javascript" src="js/login_new.js"></script>
    <script type="text/javascript" src="js/tab.js"></script>
</head>
<body onkeydown="LoginHelper.KeyDown(event);" style="height:100%;">
    <form id="form1" runat="server" style="height:100%;">
    <div class="login_box">
	<div class="login_left">
		<div class="left_logo">
            <a href="/" style="display:inline-block">
                <img src="./images/loginLogo.png" alt="">
            </a>
        </div>
	</div>
    <div class="login_right">
        <div class="box">
            <div class="title">登录</div>
            <!-- 提示信息 -->
            <h2 id="H2ErrorNBYHMsg"></h2>
            <div id="userName">
                <input id="txtNBYHUserName" name="txtNBYHUserName" type="text"   placeholder="请输入手机号或账号" autocomplete="off">
            </div>
            <div id="passWord">
                <input id="txtNBYHPwd" name="txtNBYHPwd" type="password"  placeholder="请输入密码" autocomplete="off">
            </div>
            <div id="checkCode">
                <input id="txtNBYHUCheckCode" name="txtNBYHUCheckCode" type="text"  style="width: 120px;margin-bottom: 0" placeholder="请输入验证码" autocomplete="off">
                <img id='imgNBYH' src="CheckCode.aspx" style="cursor: pointer"/>
            </div>
            <div>
                <a href="javascript:void(0);"  class="button" style="width: 205px;height:28px;line-height: 28px;" onclick="javascript:LoginHelper.Login();">登录</a>
            </div>
            <div class="loginBottom">
                <p style="float: right"><a href="http://j.chitunion.com/userInfo/toSaleRecoverPWD">忘记密码?</a></p>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</div>
    </form>
</body>
</html>
