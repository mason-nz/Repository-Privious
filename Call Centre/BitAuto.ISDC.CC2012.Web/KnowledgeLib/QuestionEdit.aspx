<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.QuestionEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<head id="Head1" runat="server">
    <title></title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/Js/common.js"></script>
     <script type="text/javascript">
         var uCEditQuestionHelper = (function () {
             var LetterArry = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"],
             //新增试题
             addQuestion = function (ulname) {
                 var num = GetQuestionCountAndResetOrder();
                 switch (ulname) {
                     case "radio":
                         addRadioQustion(num);
                         break;
                     case "checkbox":
                         AddCheckQuestion(num);
                         break;
                     case "text":
                         AddSubjectiveQuestion(num);
                         break;
                     case "select":
                         AddJudgeQuestion(num);
                         break;
                 }
             },
             addQuestionSelectType = function (obj) {
                 var objParent = $(obj).parent().parent();
                 var selectValue = $(objParent).children().find("select").val();
                 addQuestion(selectValue);
             },
             //删除试题
             deleteQuestion = function (num) {
                 var questionId = $.trim($("#ulQuestion" + num).find("[name='hdnQuestionID']").val());
                 if (questionId.length > 0) {
                     if ($.trim($("#hdnDeleteQuestionIDs").val()).length > 0) {
                         $("#hdnDeleteQuestionIDs").val($("#hdnDeleteQuestionIDs").val() + "," + questionId);
                     }
                     else {
                         $("#hdnDeleteQuestionIDs").val(questionId);
                     }
                 }
                 $("#ulQuestion" + num).remove();
                 GetQuestionCountAndResetOrder();
             },
             //删除选项
             deleteOption = function (obj,num) {
                 var objParent = $(obj).parent().parent();


                 var optionId = $.trim($(objParent).find("[name='hdnOptionID']").val());
                 if (optionId.length > 0) {
                     if ($.trim($("#hdnDeleteOptionIDs").val()).length > 0) {
                         $("#hdnDeleteOptionIDs").val($("#hdnDeleteOptionIDs").val() + "," + optionId);
                     }
                     else {
                         $("#hdnDeleteOptionIDs").val(optionId);
                     }
                 }


                 if ($(objParent).parent().children().size() > 2) {
                     $(objParent).prev().prepend("<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteOption(this,"+num+")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddOption(this)'></a></span>");
                 }
                 $(objParent).remove();
                 selectOption(num);
             },
             //新增选项
             addOption = function (obj) {
                 var objParent = $(obj).parent().parent();
                 var count = $(objParent).parent().children().size();
                 var cloneObj = $(objParent).clone();
                 $(objParent).find(".delete").remove();
                 $(objParent).find(".add").remove();
                 $(objParent).after(cloneObj);
                 $(objParent).next().find('.dx').html(LetterArry[count] + "、");
             },
             selectOption = function (num) {
                 var answerOptionIndex = "";
                 var answerOptionText = "";
                 $("#ulQuestion" + num).find("[name='liOption" + num + "']").each(function (idx, item) {
                     if ($(item).attr("checked")) {
                         answerOptionIndex += idx + ",";
                         answerOptionText += $(item).next().html();
                     }
                 });
                 var category = $("#ulQuestion" + num).find("[name='hdnAskCategory']").val()
                 if (answerOptionIndex.length > 0) {
                     answerOptionIndex = answerOptionIndex.substring(0, answerOptionIndex.length - 1);
                     if (category != 4) {
                         answerOptionText = answerOptionText.substring(0, answerOptionText.length - 1)
                     }
                     else {
                         answerOptionText = answerOptionText.substring(0, answerOptionText.length)
                     }
                 }

                 $("name=['hdnAnswerOptionIndex']", $("#ulQuestion" + num)).val(answerOptionIndex);
                 $("#AnswerOptionText" + num).val(answerOptionText);
             },
             addRadioQustion = function (num) {
                 var htmlStr = "<ul name='ulQuestion' id='ulQuestion" + num + "'>";
                 htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><input name='questionAsk' type='text'  class='w600'/></span><input type='hidden' name='hdnAskCategory' value=1><input type='hidden' name='hdnQuestionID'>";
                 htmlStr += "<span><select  name='sltQuestionType' class='w100' ><option value='radio'>单选题</option><option value='checkbox'>复选题</option><option value='text'>主观题</option><option value='select'>判断题</option></select></span>";
                 htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddQuestionSelectType(this)'></a></span>";
                 htmlStr += "<ul>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'>";
                 htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteOption(this)'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddOption(this,"+num+")'></a></span></li>";
                 htmlStr += "</ul>";
                 htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><input id='AnswerOptionText" + num + "' type='text' disabled='disabled'   class='w90'/></span></div><input type='hidden' name='hdnAnswerOptionIndex'></li>";
                 htmlStr += "</ul>";
                 var ulSize = $("ul[name='ulQuestion']").size();
                 if (ulSize > 0) {
                     $("ul[name='ulQuestion']").last().after(htmlStr);
                 }
                 else {
                     $("#divAddQuestion").after(htmlStr);
                 }
             },
            AddCheckQuestion = function (num) {
                var htmlStr = "<ul name='ulQuestion' id='ulQuestion" + num + "'>";
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><input name='questionAsk' type='text'  class='w600'/></span><input type='hidden' name='hdnAskCategory' value=2><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span><select  name='sltQuestionType' class='w100' ><option value='radio'>单选题</option><option value='checkbox'>复选题</option><option  value='text'>主观题</option><option value='select'>判断题</option></select></span>";
                htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddQuestionSelectType(this)'></a></span>";
                htmlStr += "<ul>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")' /><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'>";
                htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteOption(this)'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span></li>";
                htmlStr += "</ul>";
                htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><input name='' id='AnswerOptionText" + num + "' disabled='disabled' type='text'  class='w90'/></span><input type='hidden' name='hdnAnswerOptionIndex'></div>";
                htmlStr += "</li></ul>";
                var ulSize = $("ul[name='ulQuestion']").size();
                if (ulSize > 0) {
                    $("ul[name='ulQuestion']").last().after(htmlStr);
                }
                else {
                    $("#divAddQuestion").after(htmlStr);
                }
            },
            AddSubjectiveQuestion = function (num) {
                var htmlStr = "<ul name='ulQuestion' id='ulQuestion" + num + "'>";
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><input name='questionAsk' type='text'  class='w600'/></span><input type='hidden' name='hdnAskCategory' value=3><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span><select  name='sltQuestionType' class='w100'><option value='radio'>单选题</option><option value='checkbox'>复选题</option><option  value='text' >主观题</option><option value='select'>判断题</option></select></span>";
                htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddQuestionSelectType(this)'></a></span>";
                htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><textarea name='' cols='' rows=''  class='w500'></textarea></span></div>";
                htmlStr += "</li></ul>";
                var ulSize = $("ul[name='ulQuestion']").size();
                if (ulSize > 0) {
                    $("ul[name='ulQuestion']").last().after(htmlStr);
                }
                else {
                    $("#divAddQuestion").after(htmlStr);
                }
            },
            AddJudgeQuestion = function (num) {
                var htmlStr = "<ul name='ulQuestion' id='ulQuestion" + num + "'>";
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><input name='questionAsk' type='text'  class='w600'/></span><input type='hidden' name='hdnAskCategory' value='4' /><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span><select  name='sltQuestionType' class='w100'><option value='radio'>单选题</option><option value='checkbox'>复选题</option><option  value='text'>主观题</option><option value='select'>判断题</option></select></span>";
                htmlStr += "<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddQuestionSelectType(this)'></a></span>";
                htmlStr += "<ul>";
                htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>错</em></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>对</em></li>";
                htmlStr += "</ul>";
                htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><input name='' id='AnswerOptionText" + num + "'  disabled='disabled' type='text'  class='w90'/></span><input type='hidden' name='hdnAnswerOptionIndex'></div>";
                htmlStr += "</li></ul>";
                var ulSize = $("ul[name='ulQuestion']").size();
                if (ulSize > 0) {
                    $("ul[name='ulQuestion']").last().after(htmlStr);
                }
                else {
                    $("#divAddQuestion").after(htmlStr);
                }
            },
            GetQuestionCountAndResetOrder = function () {
                var count = $("label[name='QuestionOrder']").size();
                $("label[name='QuestionOrder']").each(function (idx, item) {
                    $(item).html((idx + 1) + "、");
                    $("input[name$='liOption']", $(item)).each(function () {
                        $(this).attr("name", "liOption" + (idx + 1))
                    });
                });
                count = count + 1;

                return count;
            }

             return {
                 AddQuestion: addQuestion,
                 AddQuestionSelectType: addQuestionSelectType,
                 DeleteQuestion: deleteQuestion,
                 DeleteOption: deleteOption,
                 AddOption: addOption,
                 SelectOption: selectOption
             }
         })();

         uCEditQuestionHelper.GetQuestionData = function (area) {
             var klqId = $("[name='hdnQuestionID']", $(area)).val();
             var txtAsk = $.trim($("[name='questionAsk']", $(area)).val());
             var askCategory = $.trim($("[name='hdnAskCategory']", $(area)).val());
             var answerOptions = $.trim($("[name='hdnAnswerOptionIndex']", $(area)).val());

             var questionOptions = new Array();
             $(area).find("ul").children("li").each(function (idx, item) {
                 var optionTitle = $.trim($(item).find("[name='txtOption']").val());
                 var klaoId = $(item).find("[name='hdnOptionID']").val();
                 var option = {
                     Answer: escapeStr(optionTitle),
                     KLAOID: escapeStr(klaoId)
                 }
                 questionOptions.push(option);
             });
             var Question = {
                 KLQID: escapeStr(klqId),
                 AskCategory: escapeStr(askCategory),
                 Ask: escapeStr(txtAsk),
                 AnswerOptions: escapeStr(answerOptions),
                 KLAnswerOptions: questionOptions
             }
             return Question;
         }

         uCEditQuestionHelper.GetAllPageData = function () {
             var QuestionArry = new Array();
             $("ul[name='ulQuestion']").each(function (idx, item) {
                 QuestionArry.push(uCEditQuestionHelper.GetQuestionData(item));
             });
             var questionAllData = {
                 Questions: QuestionArry,
                 DeleteQuestionIDs: "",
                 DeleteOptionIDs: ""
             }
             return questionAllData;
         }

         uCEditQuestionHelper.ValidateData = function (questions) {
             var msg = '';
             if (questions) {
                 for (var i = 0; i < questions.length; i++) {
                     if (!isNum(questions[i].AskCategory)) {
                         msg += "第" + i + "道试题【" + questions[i].Ask + "】类别转换成整型时失败</br>";
                     }
                     if (questions[i].Ask.length == 0) {
                         msg += "第" + i + "道试题问题不能为空</br>";
                     }
                     if (questions[i].AskCategory != 3 && questions[i].AnswerOptions.length == 0) {
                         msg += "第" + i + "道试题请选择答案</br>";
                     }
                     for (var j = 0; j < questions[i].KLAnswerOptions.length; j++) {
                         if (questions[i].KLAnswerOptions[j].Answer.length == 0) {
                             msg += "第" + i + "道试题第" + j + "个选项不能为空</br>";
                         }
                     }

                 }
             }
             return msg;
         }
         function a() {
             alert(uCEditQuestionHelper.ValidateData(uCEditQuestionHelper.GetAllPageData().Questions));
         }
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="addzs">

   <div id="divAddQuestion" class="title bold">添加试题：<span><input name="" type="button" value="添加单选题" onclick="javascript:uCEditQuestionHelper.AddQuestion('radio');" class="newBtn2 addbtn"/></span>&nbsp;
                <span><input name="" type="button" value="添加复选题" class="newBtn2 addbtn" onclick="javascript:uCEditQuestionHelper.AddQuestion('checkbox');"/></span>&nbsp;<span><input name="" type="button" value="添加主观题" onclick="javascript:uCEditQuestionHelper.AddQuestion('text');" class="newBtn2 addbtn"/></span>&nbsp;
                <span><input name="" type="button" value="添加判断题" onclick="javascript:uCEditQuestionHelper.AddQuestion('select');" class="newBtn2 addbtn"/></span>
                <span><input name="" type="button" value="添加" onclick="javascript:a();" class="newBtn2 addbtn"/></span>
            </div>
<!--添加单选题开始-->
<input id="hdnDeleteQuestionIDs" type="hidden"/><input id="hdnDeleteOptionIDs" type="hidden"/>
<asp:Repeater id="rptQuestion" OnItemDataBound="rptQuestion_ItemDataBind" runat="server">
<ItemTemplate>
<ul>
<asp:Literal ID="LOption" runat="server"></asp:Literal>
</ul>
</ItemTemplate>
</asp:Repeater>
<!--添加判断题结束-->
    </div>
    </form>
</body>
</html>

