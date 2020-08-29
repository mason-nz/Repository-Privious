<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreTableReview.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage.ScoreTableReview" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCScoreTableView.ascx" TagName="UCScoreTableView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评分表审核</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="../../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../../Css/style.css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript" />
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script type="text/javascript">
        function Approved() {
            var json = {
                action: "scoretablerereview",
                QS_RTID: '<%=QS_RTID %>'
            };
            //评分表通过
            AjaxPost("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx", json,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data == "success") {
                    $("#btnApproved").hide();
                    $("#btnReJect").hide();
                    $.jPopMsgLayer("提交成功！", function () {
                        closePageReloadOpener();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
        function Rejected() {
            var json = {
                action: "scoretablereject",
                QS_RTID: '<%=QS_RTID %>'
            };
            //复审驳回
            AjaxPost("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx", json,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data == "success") {
                    $("#btnApproved").hide();
                    $("#btnReJect").hide();
                    $.jPopMsgLayer("提交成功！", function () {
                        closePageReloadOpener();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <%=TableName%></div>
        <uc1:UCScoreTableView ID="ScoreTableViewID" runat="server" />
        <div class="btn" style="margin: 20px auto">
            <input type="button" value="审核通过" onclick="Approved()" id="btnApproved">&nbsp;&nbsp;&nbsp;
            <input type="button" value="驳回" onclick="Rejected()" id="btnReJect">
        </div>
    </div>
    </form>
</body>
</html>
