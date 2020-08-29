<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WOrderProcess.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.WOrderProcess" %>

<%@ Register Src="UserControl/CustInfoView.ascx" TagName="CustInfoView" TagPrefix="uc1" %>
<%@ Register Src="UserControl/WOrderBasicInfo.ascx" TagName="WOrderBasicInfo" TagPrefix="uc2" %>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=TitleName %></title>
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
    <script type="text/javascript">
        $(document).ready(function () {
            //设置电话插件
            HollyPhoneControl.Init("工单", "<%=OrderID %>", "", "", "<%=CRMCustID %>");
            //外呼事件
            $("img[name='TelImg']").click(function (d, e) {
                var img = $(this);
                HollyPhoneControl.SetInfoFunc(function () {
                    //客户类别
                    return "<%=CustTypeId %>";
                }, function () {
                    //姓名
                    return $.trim(img.attr("cbname"));
                }, function () { return ""; }, function () { return ""; }, function () { return ""; });
                //外呼
                HollyPhoneControl.CallOut($.trim($(this).attr("tel")));
            });
            //注册接通事件
            HollyPhoneControl.SetEstablishedEvent(function (jsondata) {
                //设置按钮不可用
                SetBtnEnable(false);
            });
            //挂断事件
            HollyPhoneControl.SetReleaseEvent(function (jsondata) {
                //设置按钮可用
                SetBtnEnable(true);
                //保存话务数据
                SaveIDToHidden("#hid_callids", jsondata.CallData.CallID);
                //保存成功//
                $.jPopMsgLayer("保存个人用户信息成功！");
            });
        });
        //设置按钮可用
        function SetBtnEnable(enab) {
            if (enab) {
                $("input[name='submit']").attr("disabled", false);
                $("input[name='submit']").attr("style", "");
            }
            else {
                $("input[name='submit']").attr("disabled", true);
                $("input[name='submit']").css("background", "url(/Images/workorder/search_btn_gray.png) no-repeat");
            }
        }
        //设置电话号码
        function SaveIDToHidden(id, dataid) {
            var val = $.trim($(id).val());
            if (val == "") {
                $(id).val(dataid);
            }
            else {
                var newval = "," + val + ",";
                var pos = newval.indexOf("," + dataid + ",");
                if (pos == -1) {
                    $(id).val(val + "," + dataid);
                }
            }
        };        
    </script>
</head>
<body>
    <div class="w980">
        <div class="title">
            <%=TitleName %></div>
        <div class="clearfix" style="height: 15px;">
        </div>
        <uc1:CustInfoView ID="ucCustInfoView" runat="server" />
         <div class="clearfix">
        </div>
        <uc2:WOrderBasicInfo ID="ucWOrderBasicInfo" runat="server" />
         <div class="clearfix">
        </div>
        <asp:PlaceHolder ID="ucPlaceHolder" runat="server"></asp:PlaceHolder>
        <input type="hidden" id="hid_callids" des="通话ID(只要在页面打过电话，均记录在此)" value="" />
    </div>
</body>
</html>
