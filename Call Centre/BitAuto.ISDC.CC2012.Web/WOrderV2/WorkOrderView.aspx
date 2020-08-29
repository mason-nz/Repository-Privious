<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.WorkOrderView" %>

<%@ Register Src="~/WOrderV2/UserControl/CustInfoView.ascx" TagName="CustInfoView"
    TagPrefix="uc" %>
<%@ Register Src="~/WOrderV2/UserControl/WOrderBasicInfo.ascx" TagName="WOrderBasicInfo"
    TagPrefix="uc1" %>
<%@ Register Src="~/WOrderV2/UserControl/WOrderDealControl/WOrderDealView.ascx" TagName="WOrderDealView"
    TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工单查看</title>
   <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="/Css/workorder/wo-base.css" rel="stylesheet" type="text/css" />
    <link href="/Css/workorder/wo-style.css?r=1.1" rel="stylesheet" type="text/css" />
     <link href="/Js/SliderImg/Slider.css" rel="stylesheet" type="text/css" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Js/SliderImg/Slider.js" type="text/javascript"></script>
    <!-----------------------------CC自定义js-------------------------------------------->
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("UserControl");
    </script>
</head>
<body>
    <div class="w980">
        <div class="title">
            工单查看</div>
        <div class="clearfix" style="height: 15px;">
        </div>
        <uc:CustInfoView runat="server" ID="ucCustInfoView" />
        <div class="clearfix">
        </div>
        <!--工单信息Start-->
        <uc1:WOrderBasicInfo runat="server" ID="ucWOrderBasicInfo" />
        <!--工单信息End-->
        <div class="clearfix">
        </div>
        <uc2:WOrderDealView runat="server" ID="ucWOrderDealView" />
    </div>
</body>
</html>
