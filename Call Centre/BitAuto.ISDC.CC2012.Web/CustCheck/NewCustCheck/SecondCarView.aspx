<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondCarView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.NewCustCheck.SecondCarView" %>

<%@ Register Src="~/CustInfo/DetailV/UCSecondCarView.ascx" TagName="UCSecondCarView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>新增非4S客户信息核实</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Js/GMapTool.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <script type="text/javascript">
        function Audit(AuditType) {
            var url = "/AjaxServers/CustAudit/CustAuditManager.ashx";
            var postBody = 'Audit=yes&TID=<%= this.TID %>&AuditType=' + AuditType;

            AjaxPost(url, postBody, null, SuccessPost);
            function SuccessPost(data) {
                var s = $.evalJSON(data);
                if (s.Update == 'yes') {
                    $.jPopMsgLayer('操作成功！', function () {
                        closePage();
                    });
                }
                else {
                    var msg = unescape(s.Update).replace('VerifyLogic,', '');
                    $.jAlert(msg);
                }
            }
        }
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="w980">
        <div class="taskT">
            客户信息<input type="hidden" id="hdnCallRecordID" /><span></span></div>
        <%--<uc:Top ID="Top" runat="server" />--%>
        <div class="baseInfo clearfix">
            <uc1:UCSecondCarView ID="UCSecondCarView1" runat="server" />
        </div>
        <div class="btn" style="clear: both;">
            <input type="button" onclick="javascript:closePage();"
                class="button" value="关 闭" />
        </div>
    </div>
    </form>
</body>
</html>
