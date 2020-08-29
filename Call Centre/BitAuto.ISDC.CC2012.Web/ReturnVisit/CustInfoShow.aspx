<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustInfoShow.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.CustInfoShow" %>

<%@ Register Src="../CustInfo/DetailVWithCalling/UCCust.ascx" TagName="UCCust" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CRM客户回访</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("ucCommon");
        loadJS("TemplateFiledData");
        loadJS("controlParams");
        loadJS("UserControl");
    </script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            //初始化短信控件
            SendMessageControl.Init("无任务");
            //注册回调事件
            SendMessageControl.SetSendMessageCompleteEvent(function (recid, jsondata) {
                SaveCustBasicInfoPoP(jsondata);
            });
        });
        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            客户信息<span></span></div>
        <div class="baseInfo clearfix">
            <uc1:UCCust ID="UCCust1" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
