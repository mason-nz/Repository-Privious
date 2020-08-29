<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login_old.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.NewWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录-赤兔联盟</title>
    <meta http-equiv="keywords" content="赤免联盟,登录" />
    <meta http-equiv="description" content="赤兔联盟,登录,微播易,新榜,蜂传,疯传,Robin8" />
    <meta name="keywords" content="赤免联盟,登录" />
    <meta name="description" content="赤兔联盟,登录,微播易,新榜,蜂传,疯传,Robin8" />

    <link rel="stylesheet" href="/css/common.css" />
    <script type="text/javascript" src="/js/jquery.1.11.3.min.js"></script>
    <script type="text/javascript" src="/js/jquery.browser.js"></script>
    <script type="text/javascript" src="/js/tab.js"></script>
    <script type="text/javascript" src="/js/login.js"></script>

    <script>
        //百度统计代码
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?25a4fec22e854c0ec742ae03e3036fc7";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>
</head>
<body style='background: #fff;' onkeydown="LoginHelper.KeyDown(event);">
    <!-- 头部 -->
    <div class="topBar" style="margin-top: 0">
        <div class="topBox" style="width: 1200px;">
            <a href="/index.html" class="topLogo">
                </a>
            <span class="fl come" style='margin-left: 60px;'>欢迎登录</span>
            <div class="clear"></div>
        </div>
    </div>
    <!--中间内容-->
    <div class="register login">
        <ul class="menu">
            <li class="active" category='29001'>广告主</li>
            <li category='29002'>媒体主</li>
        </ul>
        <h2 class="line1"></h2>
        <div class="advertiser item_register">
            <ul>
                <li class="hra">用户名：</li>
                <li class="hr_right">
                    <input id="txtGGZUserName" name="userName" type="text" onblur="javascript:if(this.value != '') LoginHelper.Verify(29001);" placeholder="请输入用户名" autocomplete="off" />
                    <img src='../images/registered_01.png' style='visibility: hidden;' />
                </li>
                <div class="clear"></div>
            </ul>
            <div class="notes">&nbsp;</div>
            <ul>
                <li class="hra">密码：</li>
                <li class="hr_right">
                    <input id="txtGGZPwd" type="password" onblur="javascript:if(this.value != '') LoginHelper.Verify(29001);" placeholder="请输入密码（密码长度应为6〜20位字符）" autocomplete="off" />
                    <img src='../images/registered_01.png' style='visibility: hidden;' />
                </li>
                <div class="clear"></div>
            </ul>
            <div class="notes">&nbsp;</div>
            <ul>
                <li class="hra">验证码：</li>
                <li class="hr_right">
                    <input id="txtGGZCheckCode" type="text" placeholder="请输入验证码" autocomplete="off" class="two_code" />

                </li>
                <li style='margin-left: 34px;'>
                    <img id="imgGGZ" src='CheckCode.aspx' /></li>
                <div class="clear"></div>
            </ul>
            <div id="divErrorGGZMsg" class="notes">&nbsp;</div>

            <div class="bottom_btn">
                <a href="javascript:void(0);" onclick="javascript:LoginHelper.Login(29001);" class="but_register sure_register">登录</a>
                <div style="margin-top: 10px; padding: 0 54px 0 50px;">
                    <p class="fl">
                        <a href="/usermanager/ForgetPwd.html?Type=1">忘记密码</a>
                    </p>
                    <p class="fr">
                        还没注册？ <a href="/usermanager/Register.html?Type=1">立即注册</a>
                    </p>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear">&nbsp;</div>
        </div>

        <div class="media_owner item_register" style='display: none'>
            <ul>
                <li class="hra">用户名：</li>
                <li class="hr_right">
                    <input id="txtMTZUserName" name="userName" type="text" onblur="javascript:if(this.value != '') LoginHelper.Verify(29002);" placeholder="请输入用户名" autocomplete="off" />
                    <img src='../images/registered_01.png' style='visibility: hidden;' />
                </li>
                <div class="clear"></div>
            </ul>
            <div class="notes">&nbsp;</div>
            <ul>
                <li class="hra">密码：</li>
                <li class="hr_right">
                    <input id="txtMTZPwd" type="password" onblur="javascript:if(this.value != '') LoginHelper.Verify(29002);" placeholder="请输入密码（密码长度应为6〜20位字符）" autocomplete="off" />
                    <img src='../images/registered_01.png' style='visibility: hidden;' />
                </li>
                <div class="clear"></div>
            </ul>
            <div class="notes">&nbsp;</div>
            <ul>
                <li class="hra">验证码：</li>
                <li class="hr_right">
                    <input id="txtMTZCheckCode" type="text" placeholder="请输入验证码" autocomplete="off" class="two_code" />

                </li>
                <li style='margin-left: 34px;'>
                    <img id="imgMTZ" src='CheckCode.aspx' /></li>
                <div class="clear"></div>
            </ul>
            <div id="divErrorMTZMsg" class="notes">&nbsp;</div>

            <div class="bottom_btn">
                <a href="javascript:void(0);" onclick="javascript:LoginHelper.Login(29002);" class="but_register sure_register">登录</a>
                <div style="margin-top: 10px; padding: 0 54px 0 50px;">
                    <p class="fl">
                        <a href="/usermanager/ForgetPwd.html?Type=2">忘记密码</a>
                    </p>
                    <p class="fr">
                        还没注册？ <a href="/usermanager/Register.html?Type=2">立即注册</a>
                    </p>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="line2">&nbsp;</div>

    <!--底部-->
    <!--#include file="/base/footer.html" -->
</body>
</html>
