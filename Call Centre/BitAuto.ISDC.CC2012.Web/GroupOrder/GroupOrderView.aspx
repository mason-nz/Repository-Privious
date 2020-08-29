<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupOrderView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupOrder.GroupOrderView" %>

<%@ Register Src="UControl/ViewCustBaseInfo.ascx" TagName="ViewCustBaseInfo" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>团购订单查看</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            //判断权限
            //            var isEditTask = '<%=IsEditTask %>';
            //            var IsBelong = '<%=IsBelong %>';
            //            if (IsBelong == "0" && isEditTask == "0") {
            //                $.jAlert("此订单不是您处理的且无管理员查看权限，无法查看！", function () {
            //                    closePage();
            //                });
            //            }

            var isR = '<%=IsReturnVisit%>';
            if (isR != "未知") {
                $("#liFailReason").hide();
            }

            bindHistory();
        });

        //绑定历史记录
        function bindHistory() {
            LoadingAnimation("divHandlerHistory");
            $("#divHandlerHistory").load("../../AjaxServers/GroupOrder/GroupOrderTaskOperLogList.aspx", { TaskID: '<%=RequestTaskID %>', r: Math.random() });
        }

        function divShowHideEvent(id, othis) {
            if ($("#" + id).is(":visible")) {
                $("#" + id).hide("slow");
                $(othis).addClass("toggle hide");
            }
            else {
                $("#" + id).show("slow");
                $(othis).removeClass("hide");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            订单信息<span></span></div>
        <div class="baseInfo">
            <div class="title ft16">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle"></a></div>
            <div id="divBaseInfo">
                <uc2:ViewCustBaseInfo ID="ViewCustBaseInfo1" runat="server" />
            </div>
            <div class="title contact">
                订单信息<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle"></a></div>
            <div id="divContactRecord">
                <ul class="clearfix">
                    <li>
                        <label>
                            订单编号：</label><span><%=OrderCode%></span></li>
                    <li>
                        <label>
                            下单经销商：</label><span><a href="../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                                target="_blank"><%=DealerName%></a></span></li>
                    <li>
                        <label>
                            订购车款：</label><span><%=CarSerialName%></span></li>
                    <li>
                        <label>
                            下单时间：</label><span><%=CreateTime%></span></li>
                    <li>
                        <label>
                            价格：</label><span><%=OrderPrice %></span></li>
                    <li>
                        <label>
                            意向车型：</label><span><%=wantcarname%></span></li>
                    <li>
                        <label>
                            预计购车时间：</label><span><%=planbuycartime%></span></li>
                    <li>
                        <label>
                            处理状态：</label><span><%=IsReturnVisit%></span></li>
                    <li id="liFailReason">
                        <label>
                            失败理由：</label><span><%=FailReason%></span></li>
                    <li>
                        <label>
                            备注：</label><span><%=CallRecord%></span></li>
                </ul>
            </div>
            <div class="title contact">
                历史记录<a href="javascript:void(0)" onclick="divShowHideEvent('divHandlerHistory',this)"
                    class="toggle"></a></div>
            <div id="divHandlerHistory" class="tableList clearfix tableList2">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
