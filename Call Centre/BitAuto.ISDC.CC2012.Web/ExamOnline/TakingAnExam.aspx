<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakingAnExam.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.TakingAnExam" %>

<%@ Register Src="UCExamOnline/ExamPaperView.ascx" TagName="ExamPaperView" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开始考试</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            //如果errorMsg不为空，则表示不该进入页面，关闭  
            var msg = "<%=errorMsg %>";
            if (msg != "" && msg != "undefined") {
                $.jAlert("<%=errorMsg %>", function () { closePage() });
            }
            else {
                exam();
            }
        });

        function exam() {

            //绑定右上角的提交按钮事件
            $(".tj_btn").click(function () {
                examOnlineSubmit(0);
                return false;
            });

            //倒计时
            if ($("#spanCountDown").html() != "00:00:00") {
                var spanTime = $("#spanCountDown").html().split(':');
                var hour = spanTime[0];
                var minute = spanTime[1];
                var second = spanTime[2];
                var countSnd = 1;
                var countMin = 0;
                $("#spanCountDown").html((hour < 10 ? "0" + hour : hour) + ":" + (minute < 10 ? "0" + minute : minute) + ":" + (second < 10 ? "0" + second : second));
                timer = setInterval(function () {
                    if (parseInt(second) == 0) {
                        second = 59;

                        //如果不是第一次进入
                        if (countSnd != 1) {
                            if (parseInt(minute) == 0) {
                                if (hour == 0) {
                                    //考试时间到 
                                    clearInterval(timer);
                                    examOnlineSubmit(1);
                                    return;
                                }
                                else {
                                    minute = 59;
                                    hour = parseInt(hour) - 1;
                                }
                            }
                            else {
                                minute = parseInt(minute) - 1;
                            }
                            ++countMin;
                        }
                        else {  //如果是第一次进入，分钟数-1
                            if (parseInt(minute) == 0 && parseInt(hour) != 0) {
                                minute = 59;
                                hour = parseInt(hour) - 1;
                            }
                            if (parseInt(minute) == 0 && parseInt(hour) == 0) {
                                //考试时间到
                                clearInterval(timer);
                                examOnlineSubmit(1);
                                return;
                            }
                        }

                        //每隔10分钟做保存操作
                        if (countMin % 10 == 0) {
                            examOnlineSave();
                        }

                    }
                    else {
                        --second;
                    }
                    countSnd = 0;
                    $("#spanCountDown").html((hour < 10 ? "0" + hour : hour) + ":" + (minute < 10 ? "0" + minute : minute) + ":" + (second < 10 ? "0" + second : second));
                }, 1000);
            }
        }

        //保存试卷
        function examOnlineSave() {
            //获取参数
            var paramsJson = $.evalJSON(_params());
            AjaxPostAsync("../AjaxServers/ExamOnline/ExamSubmitHandler.ashx",
                    { 'Action': 'ExamOnlineSave', 'EIID': '<%=RequestEIID %>', 'ExamType': '<%=RequestType %>',
                        'ExamPaperID': paramsJson.ExamPaperID, 'NoSubjectQestion': paramsJson.NoSubjectQestion,
                        'SubjectQuestion': paramsJson.SubjectQuestion, 'ExamStartTime': paramsJson.ExamStartTime, 'r': Math.random()
                    }, null,
                    function (data) { });
        }
        //交卷按钮 提交;type:0-正常个人提交；1-系统自动提交
        function examOnlineSubmit(type) {
            //获取参数
            var paramsJson = $.evalJSON(_params());

            if (paramsJson._AnswerLength == 1 && type == 0) {
                $.jAlert("主观题答案超过限定500字长度，无法提交");
                return false;
            }

            if (type == 1) {
                AjaxPostAsync("../AjaxServers/ExamOnline/ExamSubmitHandler.ashx",
                { 'Action': 'ExamOnlineSubmit', 'EIID': '<%=RequestEIID %>', 'ExamType': '<%=RequestType %>',
                    'ExamPaperID': paramsJson.ExamPaperID, 'NoSubjectQestion': paramsJson.NoSubjectQestion,
                    'SubjectQuestion': paramsJson.SubjectQuestion, 'ExamStartTime': paramsJson.ExamStartTime, 'r': Math.random()
                }, null,
                function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.msg == "true") {
                        $.jAlert("考试时间到！系统已自动帮您成功提交试卷！若没有主观题，考试结束时间后即可查看成绩，否则等阅卷后才能查看成绩。", function () {
                            $("#spanCountDown").html("00:00:00");
                            closePageReloadOpener();
                        });
                    }
                    else {
                        $.jAlert(jsonData.msg, null);
                    }
                });
            }
            else if (type == 0) {
                //获取未做的题目数量；
                var strAlert = "确定交卷吗？";
                var strNoAnswer = "";
                var IsHaveNoAnswer = false;
                var countNoSubject = $.evalJSON(countNoAnswer());
                if (countNoSubject.RadioNoAnswerCount != 0) {
                    IsHaveNoAnswer = true;
                    strNoAnswer += "单选、判断题：" + countNoSubject.RadioNoAnswerCount + "道；";
                }
                if (countNoSubject.CheckNoAnswerCount != 0) {
                    IsHaveNoAnswer = true;
                    strNoAnswer += "多选题：" + countNoSubject.CheckNoAnswerCount + "道；";
                }
                if (countNoSubject.SubjectNoAnswerCount != 0) {
                    IsHaveNoAnswer = true;
                    strNoAnswer += "主观题：" + countNoSubject.SubjectNoAnswerCount + "道；";
                }
                if (IsHaveNoAnswer) {
                    strAlert += "您还剩下：<br/> " + strNoAnswer.substring(0, strNoAnswer.lastIndexOf('；')) + " 未做";
                }

                if ($.jConfirm(strAlert, function (r) {
                    if (r) {
                        AjaxPostAsync("../AjaxServers/ExamOnline/ExamSubmitHandler.ashx",
                        { 'Action': 'ExamOnlineSubmit', 'EIID': '<%=RequestEIID %>', 'ExamType': '<%=RequestType %>',
                            'ExamPaperID': paramsJson.ExamPaperID, 'NoSubjectQestion': paramsJson.NoSubjectQestion,
                            'SubjectQuestion': paramsJson.SubjectQuestion, 'ExamStartTime': paramsJson.ExamStartTime, 'r': Math.random()
                        }, null,
                        function (data) {
                            var jsonData = $.evalJSON(data);
                            if (jsonData.msg == "true") {
                                $.jAlert("试卷提交成功，已交卷！若没有主观题，考试结束时间后即可查看成绩，否则等阅卷后才能查看成绩。", function () {
                                    closePageReloadOpener();
                                });
                            }
                            else {
                                $.jAlert(jsonData.msg, null);
                            }
                        });
                    }
                }));
            }
        }

        //计算没有完成的单选题、多选题、主观题
        function countNoAnswer() {
            var radioNoAnswerCount = 0;
            var checkNoAnswerCount = 0;
            var subjectNoAnswerCount = 0;
            $(".st").each(function () {
                if ($(this).find(":radio").length > 0) {
                    var radioValue = $(this).find(":radio:checked").val();
                    if (radioValue == undefined) {
                        radioNoAnswerCount += 1;
                    }
                }
                if ($(this).find(":checkbox").length > 0) {
                    var checkValue = $(this).find(":checkbox:checked").val();
                    if (checkValue == undefined) {
                        checkNoAnswerCount += 1;
                    }
                }
            });
            $(".faq").each(function () {
                if ($(this).find("span textarea").length > 0) {
                    var subjectValue = $.trim($(this).find("span textarea").val());
                    if (subjectValue == "") {
                        subjectNoAnswerCount += 1;
                    }
                }
            });
            return "{RadioNoAnswerCount:'" + radioNoAnswerCount + "',CheckNoAnswerCount:'" +
                    checkNoAnswerCount + "',SubjectNoAnswerCount:'" + subjectNoAnswerCount + "'}";
        }

        //参数
        function _params() {
            //取试卷ID
            var examPaperID = $.trim($("#ExamPaperID").val());

            //1、非主观题，形式 '大题ID:小题ID:答案,大题ID:小题ID:答案' 

            var radioQuestion = $(":radio:checked").map(function () {
                return $(this).attr("BQID") + ":" + $(this).attr("name") + ":" + $(this).attr("value");
            }).get().join(',');     //单选题答案

            var checkQuestion = $(":checkbox:checked").map(function () {
                return $(this).attr("BQID") + ":" + $(this).attr("name") + ":" + $(this).attr("value");
            }).get().join(',');     //复选题答案

            var checkStrQuestion = checkQuestion.split(",");
            var checkStr = "";
            var checkJudgeStr = "";
            var checkS = "";

            //将多选题答案连成一个串，最终形式 '大题ID:小题ID:单选题答案,大题ID:小题ID:复选题答案1;复选题答案2;复选题答案3,大题ID:小题ID:单选题答案'
            //38:71:189,38:71:190,38:71:191,38:101:284,38:101:285,38:101:286
            for (var i = 0; i < checkStrQuestion.length; i++) {
                var eachCheck = checkStrQuestion[i].split(":");
                if (checkJudgeStr != "" && eachCheck[1] != checkJudgeStr) {
                    checkStr += eachCheck[0] + ":" + checkStrQuestion[i - 1].split(":")[1] + ":" + checkS.substring(0, checkS.lastIndexOf(";")) + ",";
                    checkS = eachCheck[2] + ";";
                }
                else {
                    checkS += eachCheck[2] + ";";
                }
                if (i == checkStrQuestion.length - 1) {
                    checkStr += eachCheck[0] + ":" + eachCheck[1] + ":" + checkS.substring(0, checkS.lastIndexOf(";")) + ",";
                    checkS = "";
                }
                checkJudgeStr = eachCheck[1];
            }
            //非主观题 集合 形式 '大题ID:小题ID:单选题答案,大题ID:小题ID:复选题答案1;复选题答案2;复选题答案3'
            var noSubjectQuestion = "";
            if (radioQuestion != "" && checkStr != "") {
                noSubjectQuestion = radioQuestion + "," + checkStr.substring(0, checkStr.lastIndexOf(","));
            }
            else if (radioQuestion == "" && checkStr != "") {
                noSubjectQuestion = checkStr.substring(0, checkStr.lastIndexOf(","));
            }
            else if (radioQuestion != "" && checkStr == "") {
                noSubjectQuestion = radioQuestion;
            }

            //2、主观题，形式 '大题ID^^小题ID^^答案$$大题ID^^小题ID^^答案'
            var subjectQuestion = "";
            var answerLength = 0;
            $(".faq").each(function () {
                //答案(为防止跟传值冲突，将答案中的^^、$$字符替换成.)
                var answer = encodeURI($.trim($(this).find("span textarea").val()).replace("^^", ".").replace("$$", "."));
                //小题ID
                var subjectSmallID = $.trim($(this).find("span textarea").attr("name"));
                //大题ID 
                var subjectBigID = $.trim($(this).find("span textarea").attr("BQID"));
                if ($.trim($(this).find("span textarea").val()).replace("^^", ".").replace("$$", ".").length > 500) {
                    answerLength = 1;   //如果有主观题答案长度超过500，标记为1，提交时不允许提交
                }
                subjectQuestion += subjectBigID + "^^" + subjectSmallID + "^^" + answer + "$$";
            });
            subjectQuestion = subjectQuestion.substring(0, subjectQuestion.lastIndexOf("$$"));

            paramsJson = "{_AnswerLength:'" + answerLength + "',ExamPaperID:'" + examPaperID + "',NoSubjectQestion:'" + noSubjectQuestion +
                     "',SubjectQuestion:'" + subjectQuestion + "',ExamStartTime:'" + $("#hidStartTime").val() + "'}";
            return paramsJson;
        }
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            开始考试</div>
        <div class="examBt1">
            <b><span id="spanExamPaperName" runat="server"></span></b>
        </div>
        <div class="examBt2">
            <span id="spanExamPaperDesc" runat="server"></span>
        </div>
        <div class="top_yy">
            <p class="time">
                <span id="spanCountDown" runat="server"></span>
            </p>
            <a style="cursor: pointer" class="tj_btn"></a>
        </div>
        <div class="addzs">
            <uc1:ExamPaperView ID="ExamPaperView1" runat="server" />
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="交 卷" onclick="examOnlineSubmit(0)" />&nbsp;&nbsp;
            <%--<input type="button" name="" value="保存" onclick="examOnlineSave()" />&nbsp;&nbsp;--%>
            <input type="hidden" id="hidStartTime" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
