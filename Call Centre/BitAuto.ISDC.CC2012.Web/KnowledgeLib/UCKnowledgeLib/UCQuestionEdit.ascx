<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQuestionEdit.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCQuestionEdit" %>
     <script type="text/javascript">
         var uCEditQuestionHelper = (function () {
             var LetterArry = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"],
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
                 $.jConfirm("确定删除该试题吗？", function (r) {
                     if (r) {
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
                     }
                 });
             },
             //删除选项
             deleteOption = function (obj, num) {
                 var objParent = $(obj).parent().parent().parent();
                 var optionId = $.trim($(objParent).find("[name='hdnOptionID']").val());
                 if (optionId.length > 0) {
                     if ($.trim($("#hdnDeleteOptionIDs").val()).length > 0) {
                         $("#hdnDeleteOptionIDs").val($("#hdnDeleteOptionIDs").val() + "," + optionId);
                     }
                     else {
                         $("#hdnDeleteOptionIDs").val(optionId);
                     }
                 }

                 if ($(objParent).parent().children().size() > 3) {
                     $(objParent).prev().append("<span class='addst3'><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span>");
                 }
                 else {
                     $(objParent).prev().append("<span class='addst3'><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span></span>");
                 }
                 $(objParent).remove();
                 selectOption(num);
             },
             //新增选项
             addOption = function (obj, num) {
                 var objParent = $(obj).parent().parent().parent();
                 var type = $(objParent).find("[name^='liOption']").attr("type");

                 var count = $(objParent).parent().children().size();
                 if (count < 10) {
                     $(objParent).find(".addst3").remove();
                     $(objParent).after("<li><input name='liOption" + num + "' type='" + type + "' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'><span class='addst3'><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span></li>");
                     $(objParent).next().find('.dx').html(LetterArry[count] + "、");
                 }
                 else {
                     $.jAlert("选项不能超过10个！");
                 }
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
                 $("[name='hdnAnswerOptionIndex']", $("#ulQuestion" + num)).val(answerOptionIndex);
                 $("#AnswerOptionText" + num).val(answerOptionText);
             },
             addRadioQustion = function (num) {
                 var htmlStr = "<ul name='ulQuestion' id='ulQuestion" + num + "'>";
                 htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=1><input type='hidden' name='hdnQuestionID'>";
                 htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'></span>";
                 htmlStr += "<ul>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'>";
                 htmlStr += "<span class='addst3'><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span></li>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=2><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span class='addst2' ><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'></span>";
                htmlStr += "<ul>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")' /><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span><input name='hdnOptionID' type='hidden'>";
                htmlStr += "<span class='addst3'><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this," + num + ")'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span></li>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=3><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;";
                htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><textarea id='AnswerOptionText" + num + "'  name='' cols='' rows=''  class='w500'></textarea></span><input name='hdnOptionID' type='hidden' value=''></div>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value='4' /><input type='hidden' name='hdnQuestionID'>&nbsp;";
                htmlStr += "<span class='addst2' ><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;";
                htmlStr += "<ul>";
                htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>错</em><input name='hdnOptionID' type='hidden' value=''></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>对</em><input name='hdnOptionID' type='hidden' value=''></li>";
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
             //如果是主观题
             if (askCategory == "3") {
                 var klaoId = $(area).find("[name='hdnOptionID']").val();
                 var optionTitle = $("[id^='AnswerOptionText']", $(area)).val();
                 var option = {
                     Answer: escapeStr($.trim(optionTitle)),
                     KLAOID: escapeStr(klaoId)
                 }
                 questionOptions.push(option);
             }
             else {
                 $(area).find("ul").children("li").each(function (idx, item) {
                     var optionTitle = "";
                     if (askCategory == "4") {
                         //如果是判断题,获取“错”、“对”
                         optionTitle = $.trim($(item).find("em").html().substring(0,1));
                     }
                     else {
                         optionTitle = $.trim($(item).find("[name='txtOption']").val());
                     }
                     var klaoId = $(item).find("[name='hdnOptionID']").val();
                     var option = {
                         Answer: escapeStr(optionTitle),
                         KLAOID: escapeStr(klaoId)
                     }
                     questionOptions.push(option);
                 });
             }
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
             var deleteQuestionIds = $("#hdnDeleteQuestionIDs").val();
             var deleteOptionIds = $("#hdnDeleteOptionIDs").val();
             var questionAllData = {
                 Questions: QuestionArry,
                 DeleteQuestionIDs: deleteQuestionIds,
                 DeleteOptionIDs: deleteOptionIds
             }
             return questionAllData;
         }

         uCEditQuestionHelper.ValidateData = function (questions) {
             var msg = '';
             if (questions) {
                 for (var i = 0; i < questions.length; i++) {
                     if (!isNum(questions[i].AskCategory)) {
                         msg += "第" + (i+1) + "道试题【" + questions[i].Ask + "】类别转换成整型时失败</br>";
                     }
                     if (questions[i].Ask.length == 0) {
                         msg += "第" + (i + 1) + "道试题问题不能为空</br>";
                     }
                     if (questions[i].AskCategory != 3 && questions[i].AnswerOptions.length == 0) {
                         msg += "第" + (i + 1) + "道试题请选择答案</br>";
                     }
                     for (var j = 0; j < questions[i].KLAnswerOptions.length; j++) {
                         if (questions[i].KLAnswerOptions[j].Answer.length == 0) {
                             msg += "第" + (i + 1) + "道试题第" + (j + 1) + "个选项不能为空</br>";
                         }
                     }

                 }
             }
             return msg;
         }
</script>
<input id="hdnDeleteQuestionIDs" type="hidden"/><input id="hdnDeleteOptionIDs" type="hidden"/>
<asp:Repeater id="rptQuestion" OnItemDataBound="rptQuestion_ItemDataBind" runat="server">
<ItemTemplate>
<asp:Literal ID="LOption" runat="server"></asp:Literal>
</ItemTemplate>
</asp:Repeater>