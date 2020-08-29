<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyInfoView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyInfoView" %>

<%@ Register Src="~/SurveyInfo/UCSurveyInfo/UCSurveyInfoShow.ascx" TagName="SurveyInfoView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>调查问卷预览</title>
    <link href="../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../Js/json2.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
        function getinfo() {
            alert(GetData());
        }
    </script>--%>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            调查问卷--<%=SurveyName%></div>
        <div class="examBt1">
            <b style="font-family: 宋体; font-size: 20px">
                <%=SurveyName%></b>
        </div>
        <div class="addzs">
            <uc1:SurveyInfoView ID="SurveyInfoViewID" runat="server" />
        </div>
    </div>
<%--    <input type="button" value="验证" onclick="CheckDataForSurvey()"/>
    <input type="button" value="取数据" onclick="getinfo()"/>--%>
    <div class="btn" style="margin: 20px auto;">
        <input type="button" name="" value="关 闭" onclick="javascript:closePage();">
    </div>
</body>
</html>
