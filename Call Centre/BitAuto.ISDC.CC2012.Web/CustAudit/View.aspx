<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustAudit.View" %>

<!--功能废弃 强斐 2016-8-3-->
<%@ Register Src="../CustInfo/DetailV/UCCust.ascx" TagName="UCCust" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CRM客户信息核实</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../Css/cc_checkStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="http://www.google.cn/jsapi"></script>

    <script type="text/javascript" src="http://maps.google.cn/maps/api/js?sensor=false"></script>--%>
    <script type="text/javascript" src="../Js/GMapTool.js"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <%--<script src="../ADT/ADTTool.js" type="text/javascript"></script>--%>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Audit(AuditType) {
            var url = "/AjaxServers/CustAudit/CustAuditManager.ashx";
            var postBody = 'Audit=yes&TID=<%= this.TID %>&AuditType=' + AuditType;

            AjaxPost(url, postBody, null, SuccessPost);
            function SuccessPost(data) {
                var s = $.evalJSON(data);
                if (s.Update == 'yes') {
                    $.jPopMsgLayer('操作成功！', function () {
                        closePageExecOpenerSearch();
                    });
                }
                else {
                    var msg = unescape(s.Update).replace('VerifyLogic,', '');
                    $.jAlert(msg);
                }
            }
        }

        $(function () {
            GMapService.loadMapJs(); //加载google地图相关脚本，完成后回调相应方法队列
        });
    </script>
    <style>
        /* zhaoxinxin 20120302 用于更改上方会员基本信息的显示 */
        .cstText
        {
            border: 1px solid #d9d9d9;
            background: #eeeeee;
            text-align: center;
            padding: 3px;
        }
        .cstValue
        {
            border: 1px solid #d9d9d9;
            text-align: center;
            padding: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            客户信息<input type="hidden" id="hdnCallRecordID" /><span></span></div>
        <%--<uc:Top ID="Top" runat="server" />--%>
        <div class="baseInfo clearfix">
            <uc1:UCCust ID="UCCust1" runat="server" />
        </div>
        <div class="btn">
            <input type="button" id="btnConfirm" onclick="javascript:if(confirm('你确定要审核通过该客户及会员吗？审核成功后将会进入CRM系统中！'))Audit('AuditPass');"
                class="button" value="通过" runat="server" visible="false" />
            <input type="button" id="btnRefuse" onclick="javascript:if(confirm('你确定要审核拒绝该客户及会员吗？拒绝后此任务不能再编辑！'))Audit('AuditRefuse');"
                class="button" value="拒绝" runat="server" visible="false" />
            <input type="button" id="btnReject" onclick="javascript:if(confirm('您确定要将该信息驳回给相关座席吗？'))Audit('AuditReject');"
                class="button" value="驳回" runat="server" visible="false" />
            <input type="button" id="btnCallBack" runat="server" onclick="javascript:if(confirm('您确定要将该信息打回给相关座席吗？'))Audit('CallBack');"
                class="button" value="打回" visible="false" />
        </div>
    </div>
    </form>
</body>
</html>
