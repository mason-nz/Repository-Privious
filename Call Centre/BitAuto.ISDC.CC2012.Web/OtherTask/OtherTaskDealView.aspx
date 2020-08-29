<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskDealView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.OtherTaskDealView" %>

<%@ Register Src="UCOtherTask/OtherTaskView.ascx" TagName="OtherTaskEdit" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>其他任务处理查看</title>
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/Enum/ProvinceCityCountry.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/GooCalendar.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/css/GooCalendar.css" />
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="/Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="/Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script src="http://img1.bitauto.com/bt/Price/js/autodata/brand.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("TemplateFiledData");
        loadJS("controlParams");
        loadJS("bitdropdownlist");
    </script>
    <script type="text/javascript">
        $(function () {
            //加载用户控件
            LoadOtherTask();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div>
            <uc1:OtherTaskEdit ID="OtherTaskEdit1" runat="server" />
            <div class="btn" style="clear: both;">
                <input type="button" id="btnConfirm" onclick="javascript:closePage();" class="button"
                    value="关 闭" />
            </div>
        </div>
    </form>
</body>
</html>
