<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyInfoEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyInfoEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加调查问卷</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        .tooltipsy
        {
            padding: 10px;
            max-width: 200px;
            color: #303030;
            background-color: #f5f5b5;
            border: 1px solid #deca7e;
        }
    </style>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="/Js/json2.js"></script>
    <script type="text/javascript" src="/Js/jquery.blockUI.js"></script>
    <script src="../Js/tooltipsy.min.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../Js/popup.js" type="text/javascript"></script>
    <%--预览保存和提交--%>
    <script type="text/javascript">

        //从网页上获取问卷信息
        function GetSurveyInfoObj() {

            //问题列表
            var questList = new Array(); //问题列表
            $("#divQuestionList [name='JsonStr']").each(function (i, v) {
                var h = $(this).val();
                var questObj = $.evalJSON(unescape($(this).val()));
                questObj.ordernum = i;

                questList.push(questObj);
            });

            //问卷信息
            var SurveyInfoObj = {
                siid: $("#hidSiid").val(),
                name: escape($.trim($("[id$=txtName]").val())),
                bgid: $("[id$='selGroup']").val(),
                scid: $("[id$='selCategory']").val(),
                desc: escape($.trim($("[id$='txtDes']").val())),
                questList: questList
            };

            return SurveyInfoObj
        }

        //预览
        function Preview() {
            var msg = Check();
            if (msg == "") {
                $("#hidSaveOrSumit").val("preview");
                SaveInfo();
            }
            else {
                $.jAlert(msg);
            }
        }

        //保存
        function Save() {
            var msg = Check();
            if (msg == "") {
                $("#hidSaveOrSumit").val("save");
                SaveInfo();
            }
            else {
                $.jAlert(msg);
            }
        }
        
        //提交
        function Sumit() {
            $.jConfirm("确认要提交吗？", function (r) {
                if (r) {
                    var msg = Check();
                    if (msg == "") {
                        $("#hidSaveOrSumit").val("sub");
                        SaveInfo();
                    }
                    else {
                        $.jAlert(msg);
                    }
                }
            });
        }

        function SaveInfo() {

         
            var json = {
                siid: $("#hidSiid").val(),
                action: $("#hidSaveOrSumit").val(),
                data: JSON.stringify(GetSurveyInfoObj())
            };

            AjaxPost("/AjaxServers/SurveyInfo/SurveyManage/SurveySave.ashx", json,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data.split('_')[0] == "success") {

                    $("[id$='hidSiid']").val(data.split('_')[1]);

                    if ($("#hidSaveOrSumit").val() == "save") {
                        $.jPopMsgLayer("保存成功！", function () {
                            window.location = "SurveyInfoEdit.aspx?siid=" + data.split('_')[1];
                        });
                    }
                    else if ($("#hidSaveOrSumit").val() == "preview") {
                        try {
                            window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/SurveyInfo/SurveyInfoView.aspx?SIID=' + data.split('_')[1]));
                        }
                        catch (e) {
                            window.open("/SurveyInfo/SurveyInfoView.aspx?SIID=" + data.split('_')[1]);
                        }
                    }
                    else if ($("#hidSaveOrSumit").val() == "sub") {
                        $.jPopMsgLayer("提交成功！", function () {
                            closePage();
                        });
                    }
                }
                else {
                    $.jAlert(data);
                }
            });
        }

        function Check() {
            var msg = "";

            if ($.trim($("[id$='txtName']").val()) == "") {
                msg += "请输入问卷名称<br/>";
            }
            if ($.trim($("[id$='selGroup']").val()) == "-1") {
                msg += "请选择所属分组<br/>";
            }
            if ($.trim($("[id$='selCategory']").val()) == "-1") {
                msg += "请选择分类<br/>";
            }

            return msg;
        }

    </script>
    <%--页面初始化控件--%>
    <script type="text/javascript">

        $(document).ready(function () {

            var IsHaveRole = '<%=IsHaveRole %>';
            if (IsHaveRole == "0") {
                $.jAlert("您没有添加和编辑问卷的权限");
            }
            else {

                //判断状态
                var status = '<%=status %>';
                if (status != "") {
                    if (status != "0" && status != "1") {
                        $.jAlert("当前状态不允许编辑");
                        return;
                    }
                    else {
                        if (status == "1") {
                            $("#btnPreview").remove();
                            $("#btnSave").remove();
                        }
                    }
                }

                //生成问题列表
                GenerateQustionListHtml();

                $("a[name='editItem']").live("click", function (e) { e.preventDefault(); editQuestion($(this), $(this).attr("askcategory")); });
                $("a[name='delItem']").live("click", function (e) { e.preventDefault(); delQuestion($(this)); });

                $("#btnAddSingle").live("click", function (e) { e.preventDefault(); editQuestion(null, "1"); });
                $("#btnAddCheckbox").live("click", function (e) { e.preventDefault(); editQuestion(null, "2"); });
                $("#btnAddText").live("click", function (e) { e.preventDefault(); editQuestion(null, "3"); });
                $("#btnAddSingleTable").live("click", function (e) { e.preventDefault(); editQuestion(null, "4"); });
                $("#btnAddCheckboxTable").live("click", function (e) { e.preventDefault(); editQuestion(null, "5"); });

                //预览、保存、提交
                $("#btnPreview").click(Preview);
                $("#btnSave").click(Save);
                $("#btnSumit").click(Sumit);

                //分类，分组
                getUserGroup();
                $("[id$='selGroup']").val('<%=BGID %>');
                //  UserGroupChanged();
                selGroupChange();

                //上移、下移

                $("[name='upItem']").live("click", function (e) { e.preventDefault(); UpLi($(this)); ShowText(); });
                $("[name='downItem']").live("click", function (e) { e.preventDefault(); DownLi($(this)); ShowText(); });

            }
        });

        //生成问题列表
        function GenerateQustionListHtml() {

            //替换单选题
            $("#divQuestionList div[name='qitem']").each(function (i, v) {
                var dataObj = GetDataObj($(this));

                var askcategory = dataObj.askcategory;
                var html = "";
                switch (askcategory) {
                    case "1": html = GetSingleHtml(dataObj); break; // 1，单选
                    case "2": html = GetCheckBoxHtml(dataObj); break; // 2，复选
                    case "3": html = GetTextHtml(dataObj); break; // 3，文本
                    case "4": html = GetSingleTableHtml(dataObj); break; // 4，矩形单选
                    case "5": html = GetCheckBoxTableHtml(dataObj); break; // 4，矩形下拉选
                }
                $(this).after(html);
                $(this).remove();
            });

            $("#divQuestionList .addst1").show();
            ShowText();

        }

        /// 
        function ShowText() {

            $("input:text[name='optionShowTxt']").each(function () {
                var isblank = $(this).attr("isblank");
                if (isblank == "0") {
                    $(this).remove();
                }
            });

            //单选，是1列还是2列
            $(".OptionUL").each(function () {
                var showcol = $(this).attr("showcol");
                if (showcol == "1") {
                    $(this).removeClass("s2");
                    $(this).find("li").removeClass("s2");
                }
                else if (showcol == "2") {
                    $(this).addClass("s2");
                    $(this).find("li").addClass("s2");
                }
            });

            //添加标号

            //##rowIndex##

            $("#divQuestionList .addst1").each(function (i, v) {

                var index = $("#divQuestionList .addst1").index(this) + 1;
                $($(this).find("span[name='no']")[0]).text(index);
                var html = $(this).html();
                //html = html.replace(/\##rowIndex##/g, index);
                $(this).html(html);
            });

            $("input[name*='hidItemSQID']").remove();

        }

        //根据绑定的项，获取Json对象
        function GetDataObj(obj) {

            var optionListObj = new Array();
            var matrixTListObj = new Array();

            //选项
            var ordernum = "";
            var score = "";
            var isblank = "";
            var optionname = "";
            var soid = "";

            //矩阵标题
            var smtid = "";
            var titlename = "";
            var type = "";

            //选项
            $(obj).find("[name='oItem']").each(function () {

                var oneOption = {
                    ordernum: $(this).attr("ordernum"),
                    score: $(this).attr("score"),
                    isblank: $(this).attr("isblank"),
                    optionname: escape($(this).attr("optionname")),
                    sqid: $(this).attr("sqid"),
                    siid: $(this).attr("siid"),
                    soid: $(this).attr("soid"),
                    linkid: $(this).attr("linkid")
                };
                optionListObj.push(oneOption);
            });

            //矩阵标题  smtid='' siid='' sqid='' titlename=''type=''
            $(obj).find("[name='mItem']").each(function () {

                var oneMatrix = {
                    smtid: $(this).attr("smtid"),
                    siid: $(this).attr("siid"),
                    sqid: $(this).attr("sqid"),
                    type: $(this).attr("type"),
                    titlename: escape($(this).attr("titlename"))
                };
                matrixTListObj.push(oneMatrix);
            });

            var IsMustAnswer = $(obj).attr("IsMustAnswer");
            if (IsMustAnswer == "") {
                IsMustAnswer = "0";
            }
            var IsStatByScore = $(obj).attr("IsStatByScore");
            if (IsStatByScore == "") {
                IsStatByScore = "0";
            }

            //总的问题Json
            var dataObj = {
                mintextlen: $(obj).attr("mintextlen"),
                maxtextlen: $(obj).attr("maxtextlen"),
                showcolumnnum: $(obj).attr("showcolumnnum"),
                askcategory: $(obj).attr("askcategory"),
                ask: escape($(obj).attr("ask")),
                siid: $(obj).attr("siid"),
                sqid: $(obj).attr("sqid"),
                option: optionListObj, //选项
                matrix: matrixTListObj, //矩阵标题
                ordernum: "0",
                IsMustAnswer: IsMustAnswer,
                IsStatByScore: IsStatByScore,
                QuestionLinkId: $(obj).attr("QuestionLinkId")
            };

            return dataObj;
        }

        //加载登陆人分组
        function getUserGroup() {

            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }


        //所属业务组改变时，重新加载分类
        function selGroupChange() {
            $("[id$='selCategory']").find("option").remove();
            var bgId = $("[id$='selGroup']").val();
            $("[id$='selCategory']").append("<option value='-1'>请选择</option>");
            if (bgId != "-1") {

                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: bgId,IsFilterStop:"1" }, null, function (data) {
                    if (data) {
                        var jsonData = $.evalJSON(data);
                        $.each(jsonData, function (i, item) {
                            $("[id$='selCategory']").append("<option value='" + item.SCID + "'>" + item.Name + "</option>");
                        });
                        $("[id$='selCategory']").val('<%=CategoryID %>');
                    }
                });
            }
        }

    </script>
    <%--根据JSon数据获取Html(五种类型)--%>
    <script type="text/javascript">

        //根据数据，用模板生成单选题HTML
        function GetSingleHtml(dataObj) {

            //从题干模板里取HTMl
            var modelHtml = $("#singleModel").html();

            //替换生成当前问题的HTML
            modelHtml = modelHtml.replace(/\##ask##/g, unescape(dataObj.ask))
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##mintextlen##/g, dataObj.mintextlen)
                                .replace(/\##maxtextlen##/g, dataObj.maxtextlen)
                                .replace(/\##showcolumnnum##/g, dataObj.showcolumnnum)
                                .replace(/\##askcategory##/g, dataObj.askcategory)
                                .replace(/\##siid##/g, dataObj.siid)
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##JsonStr##/g, escape(JSON.stringify(dataObj)))
                                .replace(/\##IsMustAnswer##/g, dataObj.IsMustAnswer)
                                .replace(/\##IsStatByScore##/g, dataObj.IsStatByScore)
                                .replace(/\##QuestionLinkId##/g, dataObj.QuestionLinkId)
                                ;

            if (dataObj.option != null) {   //有选项

                var optionListHtml = "";
                $(dataObj.option).each(function (i, v) {
                    var optionHtml = $("#singleOptionModel").html(); //取模板                     
                    optionHtml = optionHtml.replace(/\##optionname##/g, unescape(v.optionname))
                                .replace(/\##soid##/g, v.soid)
                                .replace(/\##ordernum##/g, v.ordernum)
                                .replace(/\##score##/g, v.score)
                                .replace(/\##isblank##/g, v.isblank)
                                .replace(/\##sqid##/g, v.sqid)
                                .replace(/\##siid##/g, v.siid)
                                .replace(/\##linkid##/g, v.linkid)
                                ;

                    if (v.linkid != "0") {
                        optionHtml = optionHtml.replace(/\无跳题/g, "跳到第<span class='spanlinkIndexText'>" + v.linkid + "</span>题")
                    }

                    optionListHtml += optionHtml;
                });

                var temp = $("<p>").append($(modelHtml).clone());

                var l = $(temp).find("[name='optionList']").length;
                if (l > 0) {
                    $(temp).find("[name='optionList']").html(optionListHtml);

                    modelHtml = $(temp).html();
                }
            }

            return modelHtml;
        }

        
        //根据数据，用模板生成复选题HTML
        function GetCheckBoxHtml(dataObj) {
 
            //从题干模板里取HTMl
            var modelHtml = $("#CheckboxModel").html();

            //替换生成当前问题的HTML
            modelHtml = modelHtml.replace(/\##ask##/g, unescape(dataObj.ask))
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##mintextlen##/g, dataObj.mintextlen)
                                .replace(/\##maxtextlen##/g, dataObj.maxtextlen)
                                .replace(/\##showcolumnnum##/g, dataObj.showcolumnnum)
                                .replace(/\##askcategory##/g, dataObj.askcategory)
                                .replace(/\##siid##/g, dataObj.siid)
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##JsonStr##/g, escape(JSON.stringify(dataObj)))
                                .replace(/\##IsMustAnswer##/g, dataObj.IsMustAnswer)
                                .replace(/\##IsStatByScore##/g, dataObj.IsStatByScore)
                                 .replace(/\##QuestionLinkId##/g, dataObj.QuestionLinkId)
                                ;

            if (dataObj.option != null) {   //有选项

                var optionListHtml = "";
                $(dataObj.option).each(function (i, v) {
                    var optionHtml = $("#CheckOptionModel").html(); //取模板
                    optionHtml = optionHtml.replace(/\##optionname##/g, unescape(v.optionname))
                                .replace(/\##isblank##/g, v.isblank)
                                .replace(/\##soid##/g, v.soid)
                                ;
                    optionListHtml += optionHtml;
                });

                var temp = $("<p>").append($(modelHtml).clone());

                var l = $(temp).find("[name='optionList']").length;
                if (l > 0) {
                    $(temp).find("[name='optionList']").html(optionListHtml);
                    modelHtml = $(temp).html();
                }
            }

            return modelHtml;

        }

        //根据数据，用模板生成文本题HTML
        function GetTextHtml(dataObj) {

            //从题干模板里取HTMl
            var modelHtml = $("#TextModel").html();
            
            //替换生成当前问题的HTML
            modelHtml = modelHtml.replace(/\##ask##/g, unescape(dataObj.ask))
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##mintextlen##/g, dataObj.mintextlen)
                                .replace(/\##maxtextlen##/g, dataObj.maxtextlen)
                                .replace(/\##showcolumnnum##/g, dataObj.showcolumnnum)
                                .replace(/\##askcategory##/g, dataObj.askcategory)
                                .replace(/\##siid##/g, dataObj.siid)
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##JsonStr##/g, escape(JSON.stringify(dataObj)))
                                .replace(/\##IsMustAnswer##/g, dataObj.IsMustAnswer)
                                .replace(/\##IsStatByScore##/g, dataObj.IsStatByScore)
                                .replace(/\##QuestionLinkId##/g, dataObj.QuestionLinkId)
                                ;

            return modelHtml;

        }

        //根据数据，用模板生成HTML (矩阵单选题)
        function GetSingleTableHtml(dataObj) {

            //从题干模板里取HTMl
            var modelHtml = $("#SingleTableModel").html();

            //替换生成当前问题的HTML
            modelHtml = modelHtml.replace(/\##ask##/g, unescape(dataObj.ask))
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##mintextlen##/g, dataObj.mintextlen)
                                .replace(/\##maxtextlen##/g, dataObj.maxtextlen)
                                .replace(/\##showcolumnnum##/g, dataObj.showcolumnnum)
                                .replace(/\##askcategory##/g, dataObj.askcategory)
                                .replace(/\##siid##/g, dataObj.siid)
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##JsonStr##/g, escape(JSON.stringify(dataObj)))
                                .replace(/\##IsMustAnswer##/g, dataObj.IsMustAnswer)
                                .replace(/\##IsStatByScore##/g, dataObj.IsStatByScore)
                                .replace(/\##QuestionLinkId##/g, dataObj.QuestionLinkId)
                                ;

            if (dataObj.option != null) {   //有选项

                var optionNum = dataObj.option.length; //选项个数
                var temp1 = $("#SingleOptionModel").clone();

                var colWidth = Number(95) / Number(Number($(dataObj.option).length) + 1); //每一列的宽度

                //生成选项列
                $(dataObj.option).each(function (i, v) {
                    $(temp1).find("table thead tr").append("<td style='width:" + colWidth + "%;text-align:center;'>" + unescape(v.optionname) + " </td>");
                });

                //生成行
                $(dataObj.matrix).each(function (i, v) {
                    if (v.type == "1") {
                        if (i % 2 == 0) {
                            strHtml = "<tr  style = 'background: #ffffff;'><th  style='background: #ffffff;'>";
                        }
                        else {
                            strHtml = "<tr  style='background: #F2F2F2;'><th  style='background: #F2F2F2;'>";
                        }
                        strHtml += unescape(v.titlename) + "</th>";

                        for (var i = 0; i < optionNum; i++) {
                            strHtml += "<td  style='text-align:center;'><input type='radio'></td>";
                        }
                        strHtml += "</tr>";
                        $(temp1).find("table tbody").append(strHtml);
                    }
                });
                $(temp1).show();
            }
            var temp = $("<p>").append($(modelHtml).clone());

            var l = $(temp).find("[name='optionList']").length;
            if (l > 0) {
                $(temp).find("[name='optionList']").html($(temp1).html());
                modelHtml = $(temp).html();
            }

            return modelHtml;
        }

        //根据数据，用模板生成HTML (矩阵复选题)
        function GetCheckBoxTableHtml(dataObj) {

            //从题干模板里取HTMl
            var modelHtml = $("#CheckboxTableModel").html();

            //替换生成当前问题的HTML
            modelHtml = modelHtml.replace(/\##ask##/g, unescape(dataObj.ask))
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##mintextlen##/g, dataObj.mintextlen)
                                .replace(/\##maxtextlen##/g, dataObj.maxtextlen)
                                .replace(/\##showcolumnnum##/g, dataObj.showcolumnnum)
                                .replace(/\##askcategory##/g, dataObj.askcategory)
                                .replace(/\##siid##/g, dataObj.siid)
                                .replace(/\##sqid##/g, dataObj.sqid)
                                .replace(/\##JsonStr##/g, escape(JSON.stringify(dataObj)))
                                .replace(/\##IsMustAnswer##/g, dataObj.IsMustAnswer)
                                .replace(/\##IsStatByScore##/g, dataObj.IsStatByScore)
                                .replace(/\##QuestionLinkId##/g, dataObj.QuestionLinkId)
                                ;

            //生成选项下拉列表
            var dropDownListHtml = "<select><option value='-1'>请选择</option>";
            $(dataObj.option).each(function (i, v) {
                dropDownListHtml += "<option value=" + v.soid + ">" + unescape(v.optionname) + "</option>";
                $(temp1).find("table thead tr").append("<td width='168'>" + unescape(v.optionname) + " </td>");
            });
            dropDownListHtml += "</select>";

            if (dataObj.matrix != null) {   //有矩阵标题

                var matrixColNum = 0; //列矩阵标题个数
                $(dataObj.matrix).each(function (i, v) {
                    if (v.type == 2) {
                        matrixColNum = matrixColNum + 1;
                    }
                });
                var temp1 = $("#CheckBoxOptionModel").clone();

                var colWidth = Number(95) / Number(matrixColNum + 1); //每一列的宽度

                //生成矩阵列标题
                $(dataObj.matrix).each(function (i, v) {
                    if (v.type == 2) {
                        $(temp1).find("table thead tr").append("<td  style='width:" + colWidth + "%;text-align:center;'>" + unescape(v.titlename) + " </td>");
                    }
                });

                //生成行
                $(dataObj.matrix).each(function (i, v) {
                    if (v.type == 1) {
                        var strHtml = "";
                        if (i % 2 == 0) {
                            strHtml = "<tr  style = 'background: #ffffff;'><th  style='background: #ffffff;'>";
                        }
                        else {
                            strHtml = "<tr  style='background: #F2F2F2;'><th  style='background: #F2F2F2;'>";
                        }
                        strHtml += unescape(v.titlename) + "</th>";

                        for (var i = 0; i < matrixColNum; i++) {
                            strHtml += "<td style='text-align:center;'>" + dropDownListHtml + "</td>";
                        }
                        strHtml += "</tr>";
                        $(temp1).find("table tbody").append(strHtml);
                    }
                });
                $(temp1).show();
            }
            var temp = $("<p>").append($(modelHtml).clone());

            var l = $(temp).find("[name='optionList']").length;
            if (l > 0) {
                $(temp).find("[name='optionList']").html($(temp1).html());
                modelHtml = $(temp).html();
            }

            return modelHtml;
        }
         
    </script>
    <%--编辑和删除操作--%>
    <script type="text/javascript">

        var questIndex = '<%= QuestionLinkId %>'; //试题序号，不跟着上移、下移改变

        var indexNum = 0;
        var optionIDNum = 0;

        //编辑/添加问题框  (obj 为null，就是添加，不为空就是 a 本身)
        function editQuestion(obj, askcategory) {

            var action = "";
            var url = "";
            var jsonStr = "";
            var sqid = "";
            var siid = $("#hidSiid").val();

            indexNum = indexNum + 1;

            if (obj == null) {
                action = "add";
            }
            else {
                action = "edit";
                sqid = $(obj).attr("sqid");
                siid = $(obj).attr("siid");

                //获取存放的JSon串
                var jsonStrItem = $($(obj).parents(".addst1")[0]).find("[name='JsonStr']")[0];
                if (jsonStrItem != null && jsonStrItem != undefined) {
                    jsonStr = unescape($(jsonStrItem).val());

                }
            }

            switch (askcategory) {
                case "1": url = "/AjaxServers/SurveyInfo/SurveyManage/SingleEdit.aspx"; break;
                case "2": url = "/AjaxServers/SurveyInfo/SurveyManage/CheckBoxEdit.aspx"; break;
                case "3": url = "/AjaxServers/SurveyInfo/SurveyManage/TextEdit.aspx"; break;
                case "4": url = "/AjaxServers/SurveyInfo/SurveyManage/SingleTableEdit.aspx"; break;
                case "5": url = "/AjaxServers/SurveyInfo/SurveyManage/CheckTableEdit.aspx"; break;
            }
            $.openPopupLayer({
                name: "QuestionPopup",
                parameters: { sqid: sqid, siid: siid, JsonStr: jsonStr, action: action, indexNum: indexNum },
                url: url,
                popupMethod: "POST",
                beforeClose: function (e) {
                     
                    if (e) {
                        var returnJsonStr = $('#popupLayer_' + 'QuestionPopup').data('SingleJson');
                        var returnJsonObj = $.evalJSON((returnJsonStr));

                        if (action == "add") { //新增的，就序号加一
                            returnJsonObj.QuestionLinkId = questIndex;
                            questIndex = Number(questIndex) + Number(1);
                        }

                         var html = "";
                        switch (askcategory) {
                            case "1": html = GetSingleHtml(returnJsonObj); break; // 1，单选
                            case "2": html = GetCheckBoxHtml(returnJsonObj); break; // 2，复选
                            case "3": html = GetTextHtml(returnJsonObj); break; // 3，文本
                            case "4": html = GetSingleTableHtml(returnJsonObj); break; // 4，矩形单选
                            case "5": html = GetCheckBoxTableHtml(returnJsonObj); break; // 4，矩形下拉选
                        }

                        if (obj == null) {

                            // 添加
                            $("#divQuestionList").append(html);
                        }
                        else {

                            //编辑
                            var currItem = $(obj).parents(".addst1")[0]; //找到当前项
                            $(currItem).before(html); //在前面插入新生成的
                            $(currItem).remove(); //删除原来的
                        }
                    }
                },
                afterClose: function (e) {

                    ShowText();
                }
            });
        }

        //删除
        function delQuestion(obj) {

            $.jConfirm("确定删除问题吗？", function (r) {
                if (r) {
                    var sqid = $(obj).attr("sqid");
                    var delSqids = $("#hidDelSQIDs").val();
                    delSqids = delSqids + "," + sqid;
                    $("#hidDelSQIDs").val(delSqids);
                    $($(obj).parents(".addst1")[0]).remove();
                    ShowText();
                }
            });


        }

        
    </script>
    <%--上移、下移--%>
    <script type="text/javascript">
        //上移
        function UpLi(obj) {
            var getup = $($(obj).parents(".addst1")[0]).prev();
            if ($(getup).length != 0) {
                var onthis = $(obj).parents(".addst1")[0];
                $(onthis).hide(500);

                $(getup).before(onthis);
                $(onthis).show(300, GetIndexByLinkID);
            }
        }
        //下移
        function DownLi(obj) {
            var getdown = $($(obj).parents(".addst1")[0]).next();

            if ($(getdown).length != 0) {
                var onthis = $(obj).parents(".addst1")[0];
                $(onthis).hide(500);
                $(getdown).after(onthis);
                $(onthis).show(300, GetIndexByLinkID);
                
            }
        }
        

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT" id="Pagetitle" runat="server">
            添加调查问卷</div>
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        
                        <span class="redColor">*</span>问卷名称：</label><span><input id="txtName" runat="server"
                            type="text" value="" class="w260" /></span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>所属分组：</label><span>
                            <select class="w125" id="selGroup" onchange="selGroupChange()" runat="server">
                            </select></span> </li>
                <li>
                    <label>
                        <span class="redColor">*</span>分类：</label><span>
                            <select class="w125" id="selCategory">
                            </select></span> </li>
                <li>
                    <label>
                        问卷说明：</label><span>
                            <textarea name="" id="txtDes" runat="server" cols="1" rows="2"></textarea>
                        </span></li>
            </ul>
        </div>
        <div class="addexam sjList">
            <ul>
                <li>
                    <label>
                        添加问题：</label>
                    <span>
                        <input type="button" name="" value="添加单选题" class="btn98" style="border: none;" id="btnAddSingle" /></span>
                    <span>
                        <input type="button" name="" value="添加复选题" class="btn98" style="border: none;" id="btnAddCheckbox" /></span>
                    <span>
                        <input type="button" name="" value="添加文本题" class="btn98" style="border: none;" id="btnAddText" /></span>
                    <span>
                        <input type="button" name="" value="矩阵单选题" class="btn98" style="border: none;" id="btnAddSingleTable" /></span>
                    <span>
                        <input type="button" name="" value="矩阵评分题" class="btn98" style="border: none;" id="btnAddCheckboxTable" /></span>
                </li>
            </ul>
            <div id="divQuestionList" class="tableList clearfix tableList2">
                <asp:Repeater runat="server" ID="rpQuestionList" OnItemDataBound="rpQuestionList_ItemDataBound">
                    <ItemTemplate>
                        <div style="display: none" name="qitem" sqid='<%#Eval("SQID")%>' siid='<%#Eval("SIID")%>'
                            ask='<%#Eval("Ask")%>' askcategory='<%#Eval("AskCategory")%>' showcolumnnum='<%#Eval("ShowColumnNum")%>'
                            maxtextlen='<%#Eval("MaxTextLen")%>' mintextlen='<%#Eval("MinTextLen")%>' status='<%#Eval("Status")%>'
                            ordernum='<%#Eval("OrderNum")%>' ismustanswer='<%#Eval("IsMustAnswer")%>' isstatbyscore='<%#Eval("IsStatByScore")%>'
                            questionlinkid='<%# Container.ItemIndex + 1%>'>
                            <asp:Repeater runat="server" ID="rpOptionList">
                                <ItemTemplate>
                                    <div name='oItem' soid='<%#Eval("SOID")%>' siid='<%#Eval("SIID")%>' sqid='<%#Eval("SQID")%>'
                                        optionname='<%#Eval("OptionName")%>' isblank='<%#Eval("IsBlank")%>' score='<%#Eval("Score")%>'
                                        ordernum='<%#Eval("OrderNum")%>' linkid='<%#Eval("linkid")%>'>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater runat="server" ID="rpMatrixTitleList">
                                <ItemTemplate>
                                    <div name='mItem' smtid='<%#Eval("SMTID")%>' siid='<%#Eval("SIID")%>' sqid='<%#Eval("SQID")%>'
                                        titlename='<%#Eval("TitleName")%>' type='<%#Eval("Type")%>'>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <input type="hidden" id="hidItemSQID" name="hidItemSQID" value='<%#Eval("SQID")%>'
                            runat="server" />
                    </ItemTemplate>
                </asp:Repeater>
                <div id="panel" class="tip" style="display: none;">
                    <div class="tip1">
                        请选择要跳转的题目
                        <img id="closePanel" style="float: right; cursor: pointer; margin-right: 20px;" src="/Images/close.png" />
                    </div>
                    <ul class="tip2" id="PanelquestList">
                    </ul>
                    <ul class="tip2">
                        <li>
                            <input type="button" id="cancelSelectQuestion" value="取消跳题" class="addbtn" />
                        </li>
                    </ul>
                    <div class="tip3">
                    </div>
                </div>
            </div>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="预 览" id="btnPreview" />&nbsp;&nbsp;
            <input type="button" name="" value="保 存" id="btnSave" />&nbsp;&nbsp;
            <input type="button" name="" value="提 交" id="btnSumit" />&nbsp;&nbsp;
        </div>
        <%--问题模板--%>
        <div id="singleModel" style="display: none">
            <!--单选题-->
            <div class="addst1 questitem">
                <input type="hidden" name="JsonStr" value="##JsonStr##" />
                <input type="hidden" name="IsMustAnswer" value="##IsMustAnswer##" />
                <input type="hidden" name="IsStatByScore" value="##IsStatByScore##" />
                <input type="hidden" name="QuestionLinkId" value="##QuestionLinkId##" />
                <ul class="clearfix">
                    <li class="s1">
                        <label style="width: 20px; float: left;">
                        </label>
                        <span class="qtitle" QuestionLinkId='##QuestionLinkId##' sqid='##sqid##'><span name="no">1</span><span>.</span>##ask##</span><span
                            class="bold"></span><em style="float: right; *margin-top: -22px;"> <a href="#" name="upItem">
                                上移</a> | <a href="#" name="downItem">下移</a> | <a href="#" name="editItem" sqid='##sqid##'
                                    siid='##siid##' askcategory='##askcategory##'>编辑</a> | <a href="#" name="delItem"
                                        sqid='##sqid##'>删除</a></em>
                        <ul class="clearfix s2  ml20 OptionUL" name="optionList" showcol='##showcolumnnum##'>
                        </ul>
                    </li>
                </ul>
            </div>
            <!--单选题-->
        </div>
        <div id="singleOptionModel" style="display: none">
            <%--单选题选项--%>
            <li>
                <input type="radio" bqid="44" valuea="A" value="565" class="dt" /><span linkid='##linkid##'
                    name="sigleTitleSpan">##optionname##
                    <input type="text" value="" name='optionShowTxt' isblank='##isblank##' style="border: none;
                        border-bottom: #CCC 1px solid;" /></span><span class="tipSpan" sqid='##sqid##'   linkid='##linkid##'>无跳题</span>
            </li>
            <%--单选题选项--%>
        </div>
        <div id="CheckboxModel" style="display: none">
            <!--多选题-->
            <div class="addst1">
                <input type="hidden" name="JsonStr" value="##JsonStr##" />
                <input type="hidden" name="IsMustAnswer" value="##IsMustAnswer##" />
                <input type="hidden" name="QuestionLinkId" value="##QuestionLinkId##" />
                <ul class="clearfix">
                    <li class="s1">
                        <label style="width: 20px; float: left;">
                        </label>
                        <span class="qtitle" QuestionLinkId='##QuestionLinkId##'  sqid='##sqid##'><span name="no">1</span><span>.</span>##ask## <span
                            class="bold"></span></span><em style="float: right; *margin-top: -22px;"><a href="#"
                                name="upItem">上移</a> | <a href="#" name="downItem">下移</a> | <a href="#" name="editItem"
                                    sqid='##sqid##' askcategory='##askcategory##'>编辑</a> | <a href="#" name="delItem"
                                        sqid='##sqid##'>删除</a></em>
                        <ul class="clearfix  ml20 OptionUL" name="optionList" showcol='##showcolumnnum##'>
                        </ul>
                    </li>
                </ul>
            </div>
            <!--多选题-->
        </div>
        <div id="CheckOptionModel" style="display: none">
            <%--复选题选项--%>
            <li>
                <input name="185" type="checkbox" valuea="A" bqid="45" value="553" class="dt" />
                <span>##optionname##
                    <input type="text" value="" name='optionShowTxt' isblank='##isblank##' style="border: none;
                        border-bottom: #CCC 1px solid;" />
                </span></li>
            <%--复选题选项--%>
        </div>
        <div id="TextModel" style="display: none">
            <!--文本题-->
            <div class="addst1">
                <input type="hidden" name="JsonStr" value="##JsonStr##" />
                <input type="hidden" name="IsMustAnswer" value="##IsMustAnswer##" />
                <input type="hidden" name="QuestionLinkId" value="##QuestionLinkId##" />
                <ul class="clearfix">
                    <li class="s1">
                        <label style="width: 20px; float: left;">
                        </label>
                        <span class="qtitle" QuestionLinkId='##QuestionLinkId##'  sqid='##sqid##'><span name="no">1</span><span>.</span>##ask## <span
                            class="bold"></span></span><em style="float: right; *margin-top: -22px;"><a href="#"
                                name="upItem">上移</a> | <a href="#" name="downItem">下移</a> | <a href="#" name="editItem"
                                    sqid='##sqid##' askcategory='##askcategory##'>编辑</a> | <a href="#" name="delItem"
                                        sqid='##sqid##'>删除</a></em>
                        <ul class="clearfix  ml20">
                            <li>
                                <textarea></textarea></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <!--文本题-->
        </div>
        <div id="SingleTableModel" style="display: none">
            <!--矩阵单选题-->
            <div class="addst1">
                <input type="hidden" name="JsonStr" value="##JsonStr##" />
                <input type="hidden" name="IsMustAnswer" value="##IsMustAnswer##" />
                <input type="hidden" name="QuestionLinkId" value="##QuestionLinkId##" />
                <ul class="clearfix">
                    <li class="s1">
                        <label style="width: 20px; float: left;">
                        </label>
                        <span class="qtitle" QuestionLinkId='##QuestionLinkId##'  sqid='##sqid##'><span name="no">1</span><span>.</span>##ask## <span
                            class="bold"></span></span><em style="float: right; *margin-top: -22px;"><a href="#"
                                name="upItem">上移</a> | <a href="#" name="downItem">下移</a> | <a href="#" name="editItem"
                                    sqid='##sqid##' askcategory='##askcategory##'>编辑</a> | <a href="#" name="delItem"
                                        sqid='##sqid##'>删除</a></em>
                        <ul class="clearfix" name="optionList">
                        </ul>
                    </li>
                </ul>
            </div>
            <!--矩阵单选题-->
        </div>
        <div id="SingleOptionModel" style="display: none">
            <li>
                <table border="0" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr style="background: #F2F2F2;">
                            <th width="115" style="border-bottom: 1px solid #efefef; background: #F2F2F2;">
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr style="background: #ffffff;">
                        </tr>
                    </tbody>
                </table>
            </li>
        </div>
        <div id="CheckboxTableModel" style="display: none">
            <!--矩阵评分题-->
            <div class="addst1">
                <input type="hidden" name="JsonStr" value="##JsonStr##" />
                <input type="hidden" name="IsMustAnswer" value="##IsMustAnswer##" />
                <input type="hidden" name="QuestionLinkId" value="##QuestionLinkId##" />
                <ul class="clearfix">
                    <li class="s1">
                        <label style="width: 20px; float: left;">
                        </label>
                        <span class="qtitle" QuestionLinkId='##QuestionLinkId##'  sqid='##sqid##'><span name="no">1</span><span>.</span>##ask## <span
                            class="bold"></span></span><em style="float: right; *margin-top: -22px;"><a href="#"
                                name="upItem">上移</a> | <a href="#" name="downItem">下移</a> | <a href="#" name="editItem"
                                    sqid='##sqid##' askcategory='##askcategory##'>编辑</a> | <a href="#" name="delItem"
                                        sqid='##sqid##'>删除</a></em>
                        <ul class="clearfix" name="optionList">
                        </ul>
                    </li>
                </ul>
            </div>
            <!--矩阵评分题-->
        </div>
        <div id="CheckBoxOptionModel" style="display: none">
            <li>
                <table border="0" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr style="background: #F2F2F2;">
                            <th width="125" style="background: #F2F2F2;">
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr style="background: #ffffff;">
                        </tr>
                    </tbody>
                </table>
            </li>
        </div>
    </div>
   <input type="hidden" id="hidSiid" value="" runat="server" />
    <input type="hidden" id="hidDelSQIDs" value="" />
    <input type="hidden" id="hidSaveOrSumit" value="" />
    </form>
</body>
</html>
