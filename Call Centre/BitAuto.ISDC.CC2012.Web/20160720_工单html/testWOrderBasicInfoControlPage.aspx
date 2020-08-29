<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testWOrderBasicInfoControlPage.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web._20160720_工单html.testWOrderBasicInfoControlPage" %>

<%@ Register Src="~/WOrderV2/UserControl/CustInfoView.ascx" TagName="CustInfoView"
    TagPrefix="uc" %>
<%@ Register Src="~/WOrderV2/UserControl/WOrderBasicInfo.ascx" TagName="WOrderBasicInfo"
    TagPrefix="uc1" %>
<%@ Register Src="~/WOrderV2/UserControl/WOrderDealControl/WOrderDealView.ascx" TagName="WOrderDealView"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工单回访--大页面</title>
    <link href="../Css/workorder/wo-base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/workorder/wo-style.css" rel="stylesheet" type="text/css" />
    <link href="../../Js/SliderImg/Slider.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/SliderImg/Slider.js" type="text/javascript"></script> 
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool"); 
    </script>
  
</head>
<body>
    <div class="w980">
        <div class="title">
            工单处理</div>
        <div class="clearfix" style="height: 15px;">
        </div>
        <uc:CustInfoView runat="server" ID="custInfoView" />
        <div class="clearfix">
        </div>
        <!--工单信息Start-->
        <uc1:WOrderBasicInfo runat="server" ID="worderBasicInfo" />
        <!--工单信息End-->
        <div class="clearfix">
        </div>
        <uc2:WOrderDealView runat="server" ID="dealView" />
    </div>
</body>
</html>
