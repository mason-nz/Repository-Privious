<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQuestionAdd.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCQuestionAdd" %>
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
                 $("#ulQuestion" + num).remove();
                 GetQuestionCountAndResetOrder();
             },
              //删除选项
             deleteOption = function (obj, num) {
                 var objParent = $(obj).parent().parent().parent();

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
                 var category = $("#ulQuestion" + num).find("[name='hdnAskCategory']").val();
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
                 htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=1>";
                 htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>";
                 htmlStr += "<ul>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                 htmlStr += "<li><input name='liOption" + num + "' type='radio' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=2>&nbsp;";
                htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;";
                htmlStr += "<ul>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")' /><em class='dx'>A、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>B、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>C、</em><span><input name='txtOption' type='text'  class='w550'/></span></li>";
                htmlStr += "<li><input name='liOption" + num + "' type='checkbox' value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>D、</em><span><input name='txtOption' type='text'  class='w550'/></span>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value=3>&nbsp;";
                htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>";
                htmlStr += "<div name='divAnswer' class='title bold conrect'>正确答案：<span><textarea name='' id='AnswerOptionText" + num + "' cols='' rows=''  class='w500'></textarea></span></div>";
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
                htmlStr += "<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea name='questionAsk' type='text' class='w600' style='width:590px; height:50px;'></textarea></span><input type='hidden' name='hdnAskCategory' value='4' />&nbsp;";
                htmlStr += "<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")' ></a></span>&nbsp;";
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
              var txtAsk = $.trim($("[name='questionAsk']", $(area)).val());
              var askCategory = $.trim($("[name='hdnAskCategory']", $(area)).val());
              var answerOptions = $.trim($("[name='hdnAnswerOptionIndex']", $(area)).val());
              var questionOptions = new Array();
              //如果是主观题
              if (askCategory == "3") {
                  var optionTitle = $("[id^='AnswerOptionText']").val();
                  var option = {
                      Answer:escapeStr($.trim(optionTitle)),
                      KLQID: ''
                  }
                  questionOptions.push(option);
              }
              else {
                  $(area).find("ul").children("li").each(function (idx, item) {
                      var optionTitle = "";
                      if (askCategory == "4") {
                          //如果是判断题,获取“错”、“对”
                          optionTitle = $.trim($(item).find("em").html());
                      }
                      else {
                          optionTitle = $.trim($(item).find("[name='txtOption']").val());
                      }
                      var option = {
                          Answer: escapeStr(optionTitle),
                          KLQID: ''
                      }
                      questionOptions.push(option);
                  });
              }
              var Question = {
                  KLQID: '',
                  AskCategory: askCategory,
                  Ask: escapeStr(txtAsk),
                  AnswerOptions: answerOptions,
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
                         msg += "第" + (i + 1) + "道试题【" + questions[i].Ask + "】类别转换成整型时失败</br>";
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
