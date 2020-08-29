<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderViewDealerCRM.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.WorkOrderViewDealerCRM" %>

<%@ Register Src="UControl/OperInfoControl.ascx" TagName="OperInfoControl" TagPrefix="uc1" %>
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
    <%--<script src="/WorkOrder/UControl/ucCommon.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980 gdcl">
        <div class="taskT">
            工单查看</div>
        <div class="content">
            <div class="gd_title">
                <%=title%><span>（来源：<%=dataSource%>）</span></div>
            <!--切换标签开始-->
            <div class="Menubox8">
                <ul>
                    <li id="one1" onclick="setTab('one',1,2)" class=""></li>
                    <li id="one2" onclick="setTab('one',2,2)"></li>
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
                        <li id="liDemandCC" runat="server">
                            <label id="lbRelateDemands">
                                关联需求：</label><span>
                                    <asp:HyperLink ID="hlRelateDemand" runat="server"></asp:HyperLink>
                                </span></li>
                        <li>
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
