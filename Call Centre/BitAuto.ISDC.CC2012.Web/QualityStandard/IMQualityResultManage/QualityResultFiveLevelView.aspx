<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityResultFiveLevelView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.IMQualityResultManage.QualityResultFiveLevelView" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardFiveLevelView.ascx" TagName="UCQualityStandardFiveLevelView" TagPrefix="uc1" %>
<%@ Register Src="~/QualityStandard/UCQualityStandard/UCConversationsView.ascx" TagName="UCConversationsView" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>对话质检成绩查看</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="../../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../../Css/style.css?v=201633123" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script type="text/javascript">
        function loadApprovalHistory() {
            $("#divApprovalHistory").load("/AjaxServers/QualityStandard/ApprovalHistoryList.aspx",
            { QS_RID: '<%=QS_RID %>' }, function () {
            });
        }
        function Closeform() {
            closePage();
        }
        $(document).ready(function () {
            loadApprovalHistory();
        });

        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <%=TableName%>
        </div>
        <uc2:UCConversationsView ID="UCConversationsViewID" runat="server" />
        <uc1:UCQualityStandardFiveLevelView ID="QualityStandardViewID" runat="server" />
        <div id="divApprovalHistory" class="lybase czjl">
        </div>
        <div class="btn" style="margin: 20px auto">
            <input type="button" value="关  闭" onclick="Closeform()">
        </div>
    </div>
    </form>
</body>
</html>
