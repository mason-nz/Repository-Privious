<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalSurveyInfoView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.PersonalSurveyInfoView" %>

<%@ Register Src="~/SurveyInfo/UCSurveyInfo/UCSurveyInfoEdit.ascx" TagName="UCSurveyInfoEdit"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>调查详情</title>
    <script src="../Js/jquery-1.4.4.min.js" language="javascript" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/common.js" language="javascript" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../Css/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <div class="w980">
        <div class="taskT">
            调查问卷--<%=SurveyName%></div>
        <div class="examBt1">
            <b>
                <%=SurveyName%></b>
        </div>
        <div class="examBt2">
            调查对象：<%=UserName %>&nbsp;&nbsp;&nbsp;&nbsp; 填写时间：<%= SurveyTime%>
        </div>
        <div class="addzs">
            <uc1:UCSurveyInfoEdit ID="UCSurveyInfoEditID" runat="server" />
        </div>
    </div>
    <div class="btn" style="margin: 20px auto;">
        <input type="button" name="" value="关 闭" onclick="javascript:window.close();">
    </div>
</body>
</html>
