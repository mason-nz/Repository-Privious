<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispose.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.Dispose" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardEditForState.ascx"
    TagName="UCScoreTableDispose" TagPrefix="uc1" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardView.ascx"
    TagName="UCScoreTableView" TagPrefix="uc2" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCCallRecordView.ascx" TagName="UCCallRecordView"
    TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Title %></title>
    <link type="text/css" href="../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../Css/style.css" rel="stylesheet" />
    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="../Js/common.js"></script>
    <script type="text/javascript" src="../Js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../CTI/CTITool.js"></script>
    <script type="text/javascript" src="../Js/json2.js"></script>
    <script type="text/javascript">
        function OpenAppealPopup() {
            $.openPopupLayer({
                name: "AppealPopup",
                url: "/AjaxServers/QualityStandard/AppealPopup.aspx",
                parameters: { QS_RID: '<%=RequestQS_RID %>' },
                afterClose: function (e) {
                    if (e) {
                        $("#btnAppeal").css("display", "none");
                        loadApprovalHistory();
                    }
                }
            });
        }
        function Dispose(action, isReject) {
            var remark = encodeURIComponent($.trim($("#txtRemark").val()));
            if (remark.length == 0) {
                $.jAlert("请填写审核建议", function () {
                });
                return;
            }
            if (action == "auditagain") {
                if (isReject != "yes") {
                    var submitResult = SubQualityStandar();
                    if (!submitResult) {
                        $.jAlert("表单提交失败");
                        return;
                    }
                }
            }
            $.post("/AjaxServers/QualityStandard/Handler.ashx", { Action: action, QS_RID: '<%=RequestQS_RID %>',
                IsReject: isReject, Remark: remark
            }, function (data) {
                if (data == "success") {
                    $.jAlert("操作成功！", function () {
                        closePageExecOpenerSearch();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
        function loadApprovalHistory() {
            $("#divApprovalHistory").load("/AjaxServers/QualityStandard/ApprovalHistoryList.aspx",
            { QS_RID: '<%=RequestQS_RID %>' }, function () {
            });
        }
        $(document).ready(function () {
            loadApprovalHistory();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980 zj">
        <div class="taskT">
            <%=TableName%></div>
        <!--录音基本信息-->
        <uc3:UCCallRecordView ID="ucCallRecordView" runat="server" />
        <!--录音基本信息-->
        <!--服务规范-->
        <uc1:UCScoreTableDispose ID="ucTableDispose" runat="server" />
        <uc2:UCScoreTableView ID="UCScoreTableView" runat="server" />
        <div id="divApprovalHistory" class="lybase czjl">
        </div>
        <%if ((FirstTrialButton && Status == "20003") || (RecheckButton && Status == "20004"))
          { %>
        <div class="lybase fwgf">
            <div class="title">
                审核建议</div>
            <div class="pj">
                <textarea name="" id="txtRemark" cols="" rows=""></textarea></div>
        </div>
        <%} %>
        <!--质检评价-->
        <div class="btn" style="margin: 20px auto">
            <%if (AppealButton && Status == "20002")
              { %>
            <input type="button" id="btnAppeal" name="" value="申诉" onclick="OpenAppealPopup()" />
            <%} %>
            <%if (FirstTrialButton && Status == "20003")
              { %>
            <input type="button" name="" value="通过" onclick="Dispose('firstaudit','no')" />&nbsp;&nbsp;<input
                type="button" name="" value="拒绝" onclick="Dispose('firstaudit','yes')" />
            <%} %>
            <%if (RecheckButton && Status == "20004")
              { %>
            <input type="button" name="" value="通过" onclick="Dispose('auditagain','no')" />&nbsp;&nbsp;<input
                type="button" name="" value="拒绝" onclick="Dispose('auditagain','yes')" />
            <%} %>
        </div>
    </div>
    </form>
</body>
</html>
