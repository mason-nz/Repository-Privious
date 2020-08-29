<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreTableView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage.ScoreTableView" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCScoreTableView.ascx" TagName="UCScoreTableView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>质检表查看</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/CTI/CTITool.js" type="text/javascript" />
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script type="text/javascript">
        function Closeform() {
            closePage();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <%=TableName%></div>
        <uc1:UCScoreTableView ID="ScoreTableViewID" runat="server" />
    </div>
    <div class="btn" style="margin: 20px auto">
        <input type="button" value="关  闭" onclick="Closeform()">
    </div>
    </form>
</body>
</html>
