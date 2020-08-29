<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakingAnSurvey.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.QuestionnaireSurvey.TakingAnSurvey" %>

<%@ Register Src="../UCSurveyInfo/UCSurveyInfoShow.ascx" TagName="UCSurveyInfoShow"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>问卷调查</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            judgeIsCorrect();
        });

        //判断能否作答问卷
        function judgeIsCorrect() {
            $.post("../../AjaxServers/SurveyInfo/TakingAnSurveyHandler.ashx", { Action: "JudgeIsCorrect", SPIID: '<%=RequestSPIID %>', r: Math.random() }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg != 'success') {
                    //window.opener.location.href = window.opener.location.href;
                    alert(jsonData.msg);
                    closePage();
                }
            });
        }

        //提交
        function surveySubmit() {
            if (CheckDataForSurvey()) {
                var jsonSurveyAnswer = GetData();
                var pody = {
                    Action: "SurveyAnswerSubmit",
                    SPIID: '<%=RequestSPIID %>',
                    SIID: '<%=RequestSIID %>',
                    JsonSurveyAnswer: encodeURIComponent(jsonSurveyAnswer),
                    r: Math.random()
                };

                if ($.jConfirm("是否确定提交该问卷？", function (r) {
                    if (r) {
                        $.post("../../AjaxServers/SurveyInfo/TakingAnSurveyHandler.ashx", pody, function (data) {
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
                }));
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
            <b style="font-family: 宋体; font-size: 20px"><span id="spanProjectName" runat="server"></span></b>
        </div>
    <div class="examBt2">
        <span id="spanProjectDesc" runat="server"></span>
    </div>
    <div class="addzs">
        <uc1:UCSurveyInfoShow ID="UCSurveyInfoShow1" runat="server" />
    </div>
    <div class="btn" style="margin: 20px auto;">
        <input type="button" name="" value="提 交" onclick="surveySubmit()" />&nbsp;&nbsp;
    </div>
    </div>
    </form>
</body>
</html>
