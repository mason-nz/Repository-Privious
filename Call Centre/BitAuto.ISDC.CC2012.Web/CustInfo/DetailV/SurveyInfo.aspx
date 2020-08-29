<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyInfo.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.SurveyInfo" %>

<%@ Register Src="~/CustInfo/DetailV/UCSurveyInfoEdit.ascx" TagName="UCSurveyInfoEdit"
    TagPrefix="uc1" %>
<%@ Register Src="~/CustInfo/DetailV/UCSurveyInfoView.ascx" TagName="UCSurveyInfoView"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>问卷调查</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        //提交
        function surveySubmit() {
            if (CheckDataForSurvey()) {
                var jsonSurveyAnswer = GetData();
                var pody = {
                    Action: "SurveyAnswerSubmit",
                    SPIID: '',
                    SIID: '<%=RequestSIID %>',
                    ProjectID: '<%=RequestProjectID%>',
                    PTID: '<%=RequestTaskID%>',
                    JsonSurveyAnswer: encodeURIComponent(jsonSurveyAnswer),
                    r: Math.random()
                };


                $.post("../../AjaxServers/CustCheck/TakingAnSurveyHandler.ashx", pody, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.msg == 'success') {
                        alert("操作成功");
                    }
                    else {
                        alert(jsonData.msg);
                    }
                    closePage();
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            问卷调查</div>
        <div class="examBt1">
            <b style="font-family: 宋体; font-size: 20px"><span id="spanProjectName" runat="server">
            </span></b>
        </div>
        <div class="examBt2">
            <span id="spanProjectDesc" runat="server"></span>
        </div>
        <div class="addzs">
            <uc1:UCSurveyInfoEdit ID="UCSurveyInfoEdit1" runat="server" />
            <uc2:UCSurveyInfoView ID="UCSurveyInfoView1" runat="server" />
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="保 存" onclick="surveySubmit()" runat="server"
                id="btnsave" />&nbsp;&nbsp;<input type="button" name="" value="关 闭" onclick="javascript:closePage();" runat="server"
                id="Button1" />
        </div>
    </div>
    </form>
</body>
</html>
