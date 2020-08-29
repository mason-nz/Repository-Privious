<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderViewDealer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.WorkOrderViewDealer" %>

<%@ Register Src="UControl/OperInfoControl.ascx" TagName="OperInfoControl" TagPrefix="uc1" %>
<%@ Register Src="UControl/CustSalesControl.ascx" TagName="CustSalesControl" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工单查看</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script src="../CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        loadJS("ucCommon");
    </script>
<%--    <script src="/WorkOrder/UControl/ucCommon.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript">
        // JavaScript Document
        /*第一种形式 第二种形式 更换显示样式*/
        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
        }
        function GotoConversation778(a, url, userid, csid, orderid) {

            url = decodeURIComponent(url);
            var href = "";
            var paras = "{'CSID':'" + $.trim(csid) + "','OrderID':'" + $.trim(orderid) + "','AgentID':'" + userid + "','TimeStamp':'" + new Date().getTime() + "'}";
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx",
             { Action: "EncryptString", EncryptInfo: paras, r: Math.random() }, null, function (data) {
                href = url + "?data=" + data;
             });
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(href));

            } catch (e) {
                window.open(href);
            }
            //window.external.MethodScript('/browsercontrol/newpage?url=http://im.sys1.bitauto.com');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980 gdcl">
        <div class="taskT">
            工单查看</div>
        <div class="content">
            <div class="gd_title">
                <%=title%><span>（来源：<%=dataSource%>）</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><%=CustID%></span>
                  &nbsp;&nbsp;&nbsp;&nbsp;<span><%=CHUrl%>
            </div>
            <!--切换标签开始-->
            <div class="Menubox8" style="*margin-right: 20px">
                <ul>
                    <li id="one1" onclick="setTab('one',1,2)" class="hover">回复工单</li>
                    <li id="one2" onclick="setTab('one',2,2)">操作记录</li>
                </ul>
            </div>
            <!--切换标签结束-->
            <!--内容开始-->
            <div class="Contentbox8">
                <!--内容1开始-->
                <div class="hover">
                    <ul class="clearfix Info">
                        <li class="w260" style="width: 320px">
                            <label>
                                关联客户：</label>
                            <%=CRMCustURL%>
                        </li>
                        <li style="width: 300px">
                            <label>
                                客户地区：</label>
                            <%=custArea%></li>
                        <li style="width: 160px">
                            <label>
                                联系人：</label><span><%=contact%></span></li>
                        <li style="position: relative; top: 0px; width: 160px;">
                            <label>
                                联系电话：</label><span><%=tel%></span></li>
                    </ul>
                    <ul class="clearfix Info2" id="ulReturn">
                        <li>
                            <label>
                                工单分类：</label><span><asp:Label ID="lblCategory" runat="server" Text="" /></span></li>
                        <li>
                            <label>
                                工单类型：</label><span><asp:Label ID="lblComplaintType" runat="server" Text="" /></span></li>
                        <li>
                            <label>
                                工单状态：</label><span><asp:Label ID="lblStatus" runat="server" Text="" /></span></li>
                        <li>
                            <label>
                                接收人：</label><span><asp:Label ID="lblReceiverPerson" runat="server" Text="" /></span></li>
                        <li>
                            <label>
                                优先级：</label><span><asp:Label ID="lblPriorityLevel" runat="server" Text="" /></span></li>
                        <li id="liDemandDealer" runat="server">
                            <label>
                                关联需求：</label><span>
                                    <asp:HyperLink ID="hlRelateDemand"  runat="server"></asp:HyperLink>
                                </span></li>
                    </ul>
                </div>
                <div id="con_one_1" class="hover">
                    <!--回复区-->
                    <uc1:OperInfoControl ID="OperInfoControl1" runat="server" />
                    <!--回复区-->
                </div>
                <!--内容1结束-->
                <!--内容2开始-->
                <div id="con_one_2" style="display: none;">
                    <!--回复区-->
                    <uc1:OperInfoControl ID="OperInfoControl2" runat="server" />
                    <!--回复区-->
                </div>
                <!--内容2结束-->
            </div>
            <!--内容结束-->
        </div>
    </div>
    </form>
</body>
</html>
