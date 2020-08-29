<%@ Page Title="用户信息" Language="C#" AutoEventWireup="true" CodeBehind="CustInformation.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.CustInformation" %>

<%@ Register Src="../WOrderV2/UserControl/CustInfoView.ascx" TagName="CustInfoView"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Css/workorder/wo-base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/workorder/wo-style.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("UserControl");
    </script>
    <script type="text/javascript">
        $(function () {
            setTab("one", 1, 5);
        });
        //获取电话
        function GetTels() {
            var tels = $.trim($("#td_CustPhones").text());
            return tels;
        }

        //加载订单
        var keyid = Math.ceil(Math.random() * 100000);
        function loadCallOrderHtml() {
            var tels = GetTels();
            LoadingAnimation("con_one_1");
            $("#con_one_1").load("/WOrderV2/AjaxServers/CustInfoTab/OrderInfoListForHB.aspx?tel=" + tels + "&keyid=" + keyid + "&Notext=true");
        }
        //根据加载历史记录页面
        function loadHistoryRecordHtml() {
            var tels = GetTels();
            LoadingAnimation("con_one_2");
            $("#con_one_2").load("/WOrderV2/AjaxServers/CustInfoTab/TaskHistoryRecord.aspx?PhoneNums=" + tels + "&r=" + Math.random());
        }
        //根据加载话务记录页面
        function loadCallRecordORIGHtml() {
            var tels = GetTels();
            LoadingAnimation("con_one_3");
            $("#con_one_3").load("/WOrderV2/AjaxServers/CustInfoTab/CallRecordORIG.aspx?PhoneNums=" + tels + "&r=" + Math.random());
        }
        //根据加载短信页面
        function loadCallSMSSHtml() {
            var tels = GetTels();
            LoadingAnimation("con_one_4");
            $("#con_one_4").load("/WOrderV2/AjaxServers/CustInfoTab/SMSSendHistoryList.aspx?PhoneNums=" + tels + "&r=" + Math.random());
        }
        //加载黑白名单操作日志列表
        function loadBlackWhiteListOperLogHtml() {
            var tels = GetTels();
            LoadingAnimation("con_one_5");
            $("#con_one_5").load("/WOrderV2/AjaxServers/CustInfoTab/BlackWhiteListOperLogList.aspx?PhoneNums=" + tels + "&r=" + Math.random());
        }
        //选择标签
        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
            if (cursel == 1) {
                loadCallOrderHtml();
            }
            else if (cursel == 2) {
                loadHistoryRecordHtml();
            }
            else if (cursel == 3) {
                loadCallRecordORIGHtml();
            }
            else if (cursel == 4) {
                loadCallSMSSHtml();
            }
            else if (cursel == 5) {
                loadBlackWhiteListOperLogHtml();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="title">
            用户信息</div>
        <div class="clearfix" style="height: 15px;">
        </div>
        <uc2:CustInfoView ID="CustInfoViewControl" runat="server" />
        <div class="content" style="padding-top: 0px;">
            <!--滑动门开始-->
            <div class="Menubox">
                <ul>
                    <li id="one1" onclick="setTab('one',1,5)" class="hover">订单记录</li>
                    <li id="one2" onclick="setTab('one',2,5)">业务记录</li>
                    <li id="one3" onclick="setTab('one',3,5)">话务记录</li>
                    <li id="one4" onclick="setTab('one',4,5)">短信记录</li>
                    <li id="one5" onclick="setTab('one',5,5)">免打扰记录</li>
                </ul>
            </div>
            <!--滑动门结束-->
            <!--滑动内容开始-->
            <div class="Contentbox">
                <!--内容1开始-->
                <div id="con_one_1" style="display: none;">
                </div>
                <!--内容1结束-->
                <!--内容2开始-->
                <div id="con_one_2" style="display: none;">
                </div>
                <!--内容2结束-->
                <!--内容3开始-->
                <div id="con_one_3" style="display: none;">
                </div>
                <!--内容3结束-->
                <!--内容4开始-->
                <div id="con_one_4" style="display: none;">
                </div>
                <!--内容4结束-->
                <!--内容5开始-->
                <div id="con_one_5" style="display: none;">
                </div>
                <!--内容5结束-->
            </div>
            <!--滑动内容结束-->
        </div>
    </form>
</body>
</html>
