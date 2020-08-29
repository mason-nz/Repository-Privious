﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BitAuto.DSC.APPReport2016.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width,user-scalable=0" />    
    <title>APP报表页面——测试导航页面</title>
    
    <link type="text/css" href="Css/reset.css" rel="Stylesheet">
    <link type="text/css" href="Css/default.css" rel="Stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
            <input type="button" value="1、运营日报" onclick="window.open('运营日报/yyrb.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="2、平台覆盖用户" onclick="window.open('平台覆盖用户/ptfgyh.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="3、商机监控" onclick="window.open('商机监控/sjjk1.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="4、交易量" onclick="window.open('交易量/jyl.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="5、渠道分析（待定）" onclick="window.open('渠道分析/qdfx.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="6、人员情况" onclick="window.open('人员情况/ryqk1.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="7、业务收入" onclick="window.open('业务收入/ywsr.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="8、新车会员合作" onclick="window.open('新车会员合作/xchyhz1.htm');" />
        </div>
        <br/>
        <div style="margin-left:auto;margin-right:auto;text-align: center;"> 
        <input type="button" value="9、品牌广告合作" onclick="window.open('品牌广告合作/ppgghz.htm');" />
        </div>
        <br/>
    </form>
</body>
</html>