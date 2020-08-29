<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.Login" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="description" content="">
    <meta name="keywords" content="">
    <title>用户登录-赤兔联盟广告在线交易平台</title>
    <link rel="stylesheet" type="text/css" href="/css/reset.css" />
    <link rel="stylesheet" type="text/css" href="/css/layout.css" />
    <%--<link rel="stylesheet" type="text/css" href="/css/Videojs/video-js.css" />--%>
    <script type="text/javascript" src="/js/jquery.1.11.3.min.js"></script>
    <%--<script type="text/javascript" src="/js/vmc.slider.full.min.js"></script>--%>
    <script type="text/javascript" src="/js/jquery.browser.js"></script>
    <script type="text/javascript" src="/js/tab.js"></script>
    <%--<script type="text/javascript" src="/js/laydate.js"></script>
<script type="text/javascript" src="/js/common.js"></script>--%>
    <%--<script type="text/javascript" src="/js/Common_chitu.js"></script>--%>
    <script type="text/javascript" src="/js/login.js"></script>
    <%--<script type="text/javascript" src="/js/Videojs/videojs-ie8.min.js"></script>
<script type="text/javascript" src="/js/Videojs/video.min.js"></script>
<script type="text/javascript">
    videojs.options.flash.swf = "/js/Videojs/video-js.swf";  
</script>--%>
    <!-- 百度统计代码head变量声明 -->
    <script>
        var _hmt = _hmt || [];
    </script>
</head>

<body onkeydown="LoginHelper.KeyDown(event);">
    <!--顶部logo-->

    <div class="topBar">
        <div class="topBox">
            <a href="/Login.aspx" class="topLogo"></a>
            <div class="fl ml20 come">欢迎登陆</div>
            <div class="clear"></div>
        </div>
    </div>

    <!--中间内容-->
    <div class="logon_box">
        <div class="pw">
            <div class="logon_box_r">
                <div class="logon_box_nr">
                    <div class="tab">
                        <ul class="menu">
                            <li class="active" category='29001'>广告主</li>
                            <li category='29002'>媒体主</li>
                        </ul>
                        <div class="con1">
                            <h2 id='H2ErrorGGZMsg' class="">&nbsp;</h2>
                            <div><span class="hre">登录名:</span><input id="txtGGZUserName" name="txtUserName" type="text" class="tel" style="width: 225px;" placeholder="请输入手机号或用户名" autocomplete="off"></div>
                            <div><span class="hre">密 &nbsp; 码:</span><input id="txtGGZPwd" name="txtPwd" type="password" class="password" style="width: 225px;" placeholder="请输入密码" autocomplete="off"></div>
                            <div><span class="hre">验证码:</span><input id="txtGGZCheckCode" name="txtCheckCode" type="text" class="code" style="width: 130px;" placeholder="验证码" autocomplete="off"><span><img id='imgGGZ' src="CheckCode.aspx"></span></div>
                            <div><a href="javascript:void(0);" onclick="javascript:LoginHelper.Login(29001);" class="button" style="width: 300px; margin-top: 20px">登录</a></div>
                            <div class="loginBottom">
                                <p class="fl"><a href="http://j.chitunion.com/userInfo/toRecoverPWD">忘记密码</a></p>
                                <p class="fr">还没注册？ <a id="btnRegister" href="http://j.chitunion.com/userInfo/toRegister">立即注册</a></p>
                                <div class="clear"></div>
                            </div>
                        </div>
                        <div class="con2">
                            <h2 id="H2ErrorMTZMsg">&nbsp;</h2>
                            <div><span class="hre">登录名:</span><input id="txtMTZUserName" name="Username" type="text" class="tel" style="width: 225px;" placeholder="请输入手机号或用户名" autocomplete="off"></div>
                            <div><span class="hre">密 &nbsp; 码:</span><input id="txtMTZPwd" name="Username" type="password" class="password" style="width: 225px;" placeholder="请输入密码" autocomplete="off"></div>
                            <div><span class="hre">验证码:</span><input id="txtMTZCheckCode" name="Username" type="text" class="code" style="width: 130px;" placeholder="验证码" autocomplete="off"><span><img id='imgMTZ' src="CheckCode.aspx"></span></div>
                            <div><a href="javascript:void(0);" onclick="javascript:LoginHelper.Login(29002);" class="button" style="width: 300px; margin-top: 20px">登录</a></div>
                            <div class="loginBottom">
                                <p class="fl"><a href="http://j.chitunion.com/userInfo/toRecoverPWD">忘记密码</a></p>
                                <p class="fr">还没注册？ <a href="http://j.chitunion.com/userInfo/toRegister">立即注册</a></p>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>

    <%-- <video class="video-js vjs-default-skin vjs-big-play-centered" controls preload="auto" width="240" height="240" poster="http://vjs.zencdn.net/v/oceans.png" data-setup="{}">
    <source src="/oceans.mp4" type="video/mp4">
    <source src="http://vjs.zencdn.net/v/oceans.webm" type="video/webm">
    <source src="http://vjs.zencdn.net/v/oceans.ogv" type="video/ogg">
    <p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a></p>
  </video>

  <video class="video-js vjs-default-skin vjs-big-play-centered" controls preload="auto" width="240" height="240" poster="http://vjs.zencdn.net/v/oceans.png" data-setup="{}">
    <source src="/oceans.mp4" type="video/mp4">
    <source src="http://vjs.zencdn.net/v/oceans.webm" type="video/webm">
    <source src="http://vjs.zencdn.net/v/oceans.ogv" type="video/ogg">
    <p class="vjs-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a></p>
  </video>--%>

    <%--<div class="line2">&nbsp;</div>--%>
    <!--底部-->
    <div id="footer2">
        <div class="main">
            <%--<ul>
            <li><a href="javascript:void(0);" target="_self">关于我们</a></li>
            <li>|</li>
            <li><a href="javascript:void(0);" target="_self">联系我们</a></li>
            <li>|</li>
            <li><a href="/help.html" target="_blank">帮助中心</a></li>
            <div class="clear"></div>
        </ul>--%>
            <p>@2016-2017 www.chitunion.com All Rights Reserved. 北京行圆汽车信息技术有限公司所有 京ICP备17011360号-3</p>
        </div>
    </div>
    <script type="text/javascript">
        //百度统计代码
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?21b9d859e6d7d81f4199ab4414b5b18e";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>
</body>
</html>
