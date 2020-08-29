<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamPaperStorage.ExamPaperEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http：//www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加试卷</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/Js/common.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            //添加大题按钮
            $("#btnAddBigQ").click(AddBigQuestion);

            //总分输入事件，只能输入数字
            $("#txtTotalScore").keyup(function (event) {
                if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                    $("#spanmaxscore").html("&nbsp;&nbsp;目前可用分数：" + GetMaxScore()); //可用总分

                } else {
                    $('#txtTotalScore').attr("value", "");
                }
            });

            //选择小题窗口中的分类下拉列表
            $("#selKCID1").change(function () {
                $("#selKCID3").hide();
                BindSelect(2, $("#selKCID1").val());
            });
            $("#selKCID2").change(function () {
                $("#selKCID3").hide();
                BindSelect(3, $("#selKCID2").val());
            });

            ///绑定编辑大题、编辑小题、删除小题按钮
            $("input[name='btnEditBig']").live('click', EditBigQ);
            $("input[name='btnEditSmall']").live('click', EditSmallQ);
            $("input[name='btnDelBig']").live('click', DelBigQ);

            $("#btnPreview").click(Preview);
            //保存按钮
            $("#btnSave").click(Save);
            //提交按钮
            $("#btnSumbit").click(Submit);

            //初始化可用分
            $("#hidBigQMaxScore").val(GetMaxScore());
            $("#spanmaxscore").html("&nbsp;&nbsp;目前可用分数：" + GetMaxScore()); //可用总分  


            //小题的删除按钮
            $("a[name='DelSelect']").live('click', function (e) { e.preventDefault(); DelSelectQuestion($(this)); });

        });
        
    
    </script>
    <script type="text/javascript">

        //添加大题
        function AddBigQuestion() {

            var msg = CheckPar();
            if (msg != "") {
                $.jAlert(msg);
                return;
            }
            else {

                //清空隐藏域
                $("#hidBigQIndex").val(""); //index
                $("#hidSmallQIDs").val("");
                $("#hidBigQBQID").val("");
                $("#hidBigQName").val(""); //大题名称
                $("#hidBigQDesc").val(""); //大题说明
                $("#hidBigQType").val(""); //题型
                $("#hidBigQScore").val(""); //每题分值
                $("#hidBigQTotolCount").val(""); //试题总量
                $("#hidBigQRealTotolCount").val(""); //实际试题总量

                $("#hidIsOk").val("");
                $.openPopupLayer({
                    name: "DisposeOnePoper",
                    parameters: {},
                    url: "BigQuestion.aspx",
                    beforeClose: function (e, data) {
                        //关闭后，执行
                        if ($("#hidIsOk").val() == "1") {

                            //最大索引
                            var maxindex = 1;
                            if ($("td[name='tdindex']").length > 0) {
                                maxindex = Number($($("td[name='tdindex']").last()).text()) + 1;
                            }
                            $("#hidBigQIndex").val(maxindex);

                            var html = GetNewBigQHtml();
                            html = "<tr class='bigItem'>" + html + "</tr>";
                            $("#TbbigList").append(html);
                            $("#spanmaxscore").html("&nbsp;&nbsp;目前可用分数：" + GetMaxScore()); //可用总分

                        }
                    }
                });
            }
        }

        //添加大题时的判断
        function CheckPar() {

            var msg = "";

            $("#hidBigQMaxScore").val(GetMaxScore()); //新增大题可用总分
            if ($("#hidBigQMaxScore").val() <= 0) {
                msg = msg + "已有大题分数之和已经达到试卷总分数，不能再添加大题";
            }

            return msg;
        }

        ///获取新增大题的可用最大分数值
        function GetMaxScore() {

            var totolScore = $("#txtTotalScore").val(); //总分数
            var usedScore = 0; //大题列表已有分数

            if (!isNaN(totolScore)) {

                $("td[name='bigitemTotol']").each(function () {
                    usedScore = Number(usedScore) + Number($(this).text());
                });
                return totolScore - usedScore;
            }
            else {
                return 0;
            }
        }

        ///获取新增大题的列表HTMl
        function GetNewBigQHtml() {
            var itemIndex = $("#hidBigQIndex").val(); //Index
            var smallIDs = $("#hidSmallQIDs").val(); //小题IDs
            var bqid = $("#hidBigQBQID").val(); //大题ID
            var name = $("#hidBigQName").val(); //大题名称
            var des = $("#hidBigQDesc").val(); //大题说明
            var type = $("#hidBigQType").val(); //题型
            var score = $("#hidBigQScore").val(); //每题分值
            var totolcount = $("#hidBigQTotolCount").val(); //试题总量
            var realTCount = $("#hidBigQRealTotolCount").val(); //实际试题总量
            var realTScore = Number(score) * Number(realTCount); //实际试题总分

            var totolscore = Number(score) * Number(totolcount);
            var typeName = "";
            switch (type) {
                case "1": typeName = "单选题"; break;
                case "2": typeName = "复选题"; break;
                case "3": typeName = "主观题"; break;
                case "4": typeName = "判断题"; break;
            }

            var html = "<td name='tdindex'>" + itemIndex + "</td>"
                        + "<td>" + name + "</td>"
                        + "<td>" + typeName + "</td>"
                        + "<td>" + totolcount + "/" + realTCount + "</td>"
                        + "<td>" + score + "</td>"
                        + "<td name='bigitemTotol'>" + totolscore + "</td>"
                        + "<td>" + realTScore + "</td>"
                        + "<td>手动</td>"
                        + " <td>"
                            + " <span><input type='button' name='btnEditBig' value='编辑大题' class='btnattach' style='border: none;'></span>"
                            + " <span><input type='button' name='btnEditSmall'  value='编辑小题' class='btnattach' style='border: none;'></span>"
                             + "<span><input type='button' name='btnDelBig' value='删除' class='btnattach' style='border: none;'></span>"
                        + " </td>"
                        + "   <input type='hidden' name='bigIndex' value='" + itemIndex + "' />"
                        + "   <input type='hidden' name='bigSmallQIDs' value='" + smallIDs + "' />"
                        + "   <input type='hidden' name='bigQBQID' value='" + bqid + "' />"
                        + "   <input type='hidden' name='bigQName' value='" + name + "' />"
                        + "   <input type='hidden' name='bigQDes' value='" + des + "' />"
                        + "   <input type='hidden' name='bigQType' value='" + type + "' />"
                        + "   <input type='hidden' name='bigQCount' value='" + totolcount + "' />"
                        + "   <input type='hidden' name='bigQScore' value='" + score + "' />"
                        + "   <input type='hidden' name='bigQRealTCount' value='" + realTCount + "' />"


            return html;
        }

    </script>
    <script type="text/javascript">
        var ddlbgid = '<%=ddlBGIDs.ClientID %>';
        //编辑大题
        function EditBigQ() {
            var curTr = $(this).parent().parent().parent();

            $("#hidBigQIndex").val($(curTr).find("input[name='bigIndex']").val());
            $("#hidBigQBQID").val($(curTr).find("input[name='bigQBQID']").val()); //大题ID
            $("#hidBigQBQID").val($(curTr).find("input[name='bigQBQID']").val()); //大题ID
            $("#hidBigQName").val($(curTr).find("input[name='bigQName']").val()); //大题名称
            $("#hidBigQDesc").val($(curTr).find("input[name='bigQDes']").val()); //大题说明
            $("#hidBigQType").val($(curTr).find("input[name='bigQType']").val()); //题型
            $("#hidBigQScore").val($(curTr).find("input[name='bigQScore']").val()); //每题分值
            $("#hidBigQTotolCount").val($(curTr).find("input[name='bigQCount']").val()); //试题总量
            $("#hidBigQRealTotolCount").val($(curTr).find("input[name='bigQRealTCount']").val()); //实际试题总量

            var curTotol = Number($("#hidBigQScore").val()) * Number($("#hidBigQTotolCount").val());
            $("#hidBigQMaxScore").val(GetMaxScore() + curTotol);

            $("#hidIsOk").val("");
            $.openPopupLayer({
                name: "DisposeOnePoper",
                parameters: {},
                url: "BigQuestion.aspx",
                beforeClose: function (e, data) {
                    //关闭后，执行
                    if ($("#hidIsOk").val() == "1") {

                        var html = GetNewBigQHtml();
                        $(curTr).html(html);
                        $("#hidBigQMaxScore").val(GetMaxScore());
                        $("#spanmaxscore").html("&nbsp;&nbsp;目前可用分数：" + GetMaxScore()); //可用总分                      
                    }
                }
            });
        }

        ///编辑小题
        function EditSmallQ() {
            var curTr = $(this).parent().parent().parent();

            var smallQIDs = $(curTr).find("input[name='bigSmallQIDs']").val();
            var bigQType = $(curTr).find("input[name='bigQType']").val();
            var bigQTotolCount = $(curTr).find("input[name='bigQCount']").val(); //最大题量

            $("#hidIsSmallOk").val("");

            $.openPopupLayer({
                name: "QustionSelectAjaxPopup",
                parameters: { SmallQIDs: escape(smallQIDs), QustionType: escape(bigQType), MaxCount: escape(bigQTotolCount) },
                url: "/ExamOnline/ExamPaperStorage/QuestionSelect.aspx",
                beforeClose: function () {
                    if ($("#hidIsSmallOk").val() == "1") {

                        var ids = $("[id$='QustionSelectAjaxPopup']").data('selectIDs');
                        $("#hidSmallQIDs").val(ids); //大题名称

                        var idlen = ids.split(',').length; //选择的个数
                        $("#hidBigQIndex").val($(curTr).find("input[name='bigIndex']").val()); //大题Index
                        $("#hidBigQBQID").val($(curTr).find("input[name='bigQBQID']").val()); //大题ID
                        $("#hidBigQName").val($(curTr).find("input[name='bigQName']").val()); //大题名称
                        $("#hidBigQDesc").val($(curTr).find("input[name='bigQDes']").val()); //大题说明
                        $("#hidBigQType").val($(curTr).find("input[name='bigQType']").val()); //题型
                        $("#hidBigQScore").val($(curTr).find("input[name='bigQScore']").val()); //每题分值
                        $("#hidBigQTotolCount").val($(curTr).find("input[name='bigQCount']").val()); //试题总量

                        var totolCount = Number(idlen);
                        $("#hidBigQRealTotolCount").val(totolCount); //实际试题总量

                        var html = GetNewBigQHtml();
                        $(curTr).html(html);
                    }
                }
            });
        }

        //删除大题
        function DelBigQ() {
            var curTr = $(this).parent().parent().parent();
            $.jConfirm("确定要删除此大题吗？", function (isOk) {
                if (isOk) {

                    var bqID = $(curTr).find("input[name='bigQBQID']").val()
                    var thisIndex = $(curTr).find("input[name='bigIndex']").val(); //当前IDNex

                    if (bqID != "") {
                        $("#hidDelBigIDs").val($("#hidDelBigIDs").val() + "," + bqID); //删除的IDs
                    }
                    curTr.remove();
                    $("#hidBigQMaxScore").val(GetMaxScore());
                    $("#spanmaxscore").html("&nbsp;&nbsp;目前可用分数：" + GetMaxScore()); //可用总分  

                    //修改index
                    $("td[name='tdindex']").each(function () {
                        if (Number($(this).text()) > thisIndex) {
                            var itemTr = $(this).parent().parent().parent();

                            $(itemTr).find("input[name='bigIndex']").val(Number($(this).text()) - 1)
                            $(this).text(Number($(this).text()) - 1);

                        }
                    });


                }
            });
        }
    </script>
    <script type="text/javascript">

        //点击预览按钮
        function Preview() {
            var ExamPaperInfo = GetJsonData();

            $("#ExamPaperInfoStr").val(escape(JSON.stringify(ExamPaperInfo)));

            $("#previewForm").submit();

        }

        //点击保存按钮
        function Save() {
            $("#inputSaveOrSub").val("save");
            var msg = checkInfo();
            if (msg != "") {
                $.jAlert(msg);
            }
            else {
                SaveInfo();
            }
        }

        //点击提交按钮
        function Submit() {
            $("#inputSaveOrSub").val("sub");
            var msg = checkSubmitInfo();
            if (msg != "") {
                $.jAlert(msg);
            }
            else {
                SaveInfo();
            }
        }

        //保存试卷信息
        function SaveInfo() {

            var ExamPaperInfo = GetJsonData();

            var pody = {
                ExamPaperInfoStr: escape(JSON.stringify(ExamPaperInfo)),
                Action: escape($("#inputSaveOrSub").val())
            };

            AjaxPostAsync('/AjaxServers/ExamOnline/ExamPaperStorage/ExamPaperSave.ashx', pody,
            function () {
                $("#btnSave").attr("disabled", "disabled");
                $("#btnSumbit").attr("disabled", "disabled");
                $("#imgLoadingPop").css("display", "");
            }
            ,
             function (data) {

                 var jsonData = $.evalJSON(data);
                 if (jsonData.result == "success") {
                     var saveOrSub = $("#inputSaveOrSub").val();

                     if (saveOrSub == "save") {
//                         $.jAlert("保存成功！", function () {
//                             var epid = jsonData.epid;
//                             window.location = "ExamPaperEdit.aspx?epid=" + jsonData.epid;
                         //                         });
                         $.jPopMsgLayer("保存成功！", function () {
                             var epid = jsonData.epid;
                             window.location = "ExamPaperEdit.aspx?epid=" + jsonData.epid;
                         });
                         
                     }
                     else if (saveOrSub == "sub") {
//                         $.jAlert("提交成功！", function () {
//                             closePage();
                         //                         });
                         $.jPopMsgLayer("提交成功！", function () {
                             closePage();
                         });
                         
                     }
                 }
                 else {
                     $.jAlert("操作出错！<br/>" + jsonData.epid);
                 }
                 $("#btnSave").attr("disabled", "");
                 $("#btnSumbit").attr("disabled", "");
                 $("#imgLoadingPop").css("display", "none");
             });

        }

        //保存验证
        function checkInfo() {
            var msg = "";
            if ($.trim($("#txtName").val()) == "") {
                msg += "试卷名称不能为空<br/>";
            }
            if ($('#' + ddlbgid).val() == null) {
                msg += "试卷所属分组不能为空<br/>";
            }

            if ($.trim($('input:radio[name$="paperCatage"]:checked').val()) == "") {
                msg += "请选择分类<br/>";
            }
            if ($.trim($("#txtDes").val()) == "") {
                msg += "考试说明不能为空<br/>";
            }
            if ($.trim($("#txtTotalScore").val()) == "") {
                msg += "总分不能为空<br/>";
            }
            var totolScore = $.trim($("#txtTotalScore").val());
            if (isNaN(totolScore)) {
                msg += "总分应该为数字<br/>";
            }
            if (Number(totolScore) < 0) {
                msg += "总分不能小于0<br/>";
            }

            return msg;
        }

        //提交验证
        function checkSubmitInfo() {

            var msg = checkInfo();
            if ($("tr[class='bigItem']").length == 0) {
                msg += "没有添加大题，不能提交<br/>";
            }
            var maxScore = GetMaxScore();
            if (Number(maxScore) != 0) {
                msg += "所有大题总分之和应该等于试卷总分<br/>";
            }
            $("tr[class='bigItem']").each(function (i, v) {
                var BigQName = $(this).find("input[name='bigQName']").val(); //大题名称
                var BigQTotolCount = $(this).find("input[name='bigQCount']").val(); //试题总量
                var BigQReadCount = $(this).find("input[name='bigQRealTCount']").val(); //试题实际总量

                if (Number(BigQReadCount) != Number(BigQTotolCount)) {
                    msg += "大题【" + BigQName + "】的小题个数不够" + BigQTotolCount + "个<br/>";
                }
            });
            return msg;
        }

        //获取页面上的试卷信息
        function GetJsonData() {


            //获取试卷信息
            var paperInfo = {
                EPID: escape($.trim($("#hidEPID").val())),
                Name: escape($.trim($("#txtName").val())),
                ECID: escape($.trim($('input:radio[name$="paperCatage"]:checked').val())),
                ExamDesc: escape($.trim($("#txtDes").val())),
                TotalScore: escape($.trim($("#txtTotalScore").val())),
                BGID: escape($('#' + ddlbgid).val())
            };

            //获取大题信息

            var bigQList = new Array();

            $("tr[class='bigItem']").each(function (i, v) {
                var BQID = $(this).find("input[name='bigQBQID']").val(); //大题名称
                var BigQName = $(this).find("input[name='bigQName']").val(); //大题名称
                var BigQDesc = $(this).find("input[name='bigQDes']").val(); //大题说明
                var BigQType = $(this).find("input[name='bigQType']").val(); //题型
                var BigQScore = $(this).find("input[name='bigQScore']").val(); //每题分值
                var BigQTotolCount = $(this).find("input[name='bigQCount']").val(); //试题总量
                var smallQIDs = $(this).find("input[name='bigSmallQIDs']").val();

                //大题
                var bigQModel = {
                    BQID: escape(BQID),
                    EPID: escape($("#hidEPID").val()),
                    Name: escape(BigQName),
                    BQDesc: escape(BigQDesc),
                    AskCategory: escape(BigQType),
                    EachQuestionScore: escape(BigQScore),
                    QuestionCount: escape(BigQTotolCount)
                };


                //大小题关系
                var shipList = new Array();
                if (smallQIDs != "") {
                    shipList = new Array();

                    $(smallQIDs.split(',')).each(function (i, v) {
                        var shipModel = {
                            BQID: escape(BQID),
                            KLQID: escape(v)
                        };

                        shipList.push(shipModel);
                    });
                }

                //总计一个大题
                var oneBigQ = {
                    bigqpageinfo: bigQModel,
                    shipList: shipList
                };

                bigQList.push(oneBigQ);

            });

            var delbigIDs = $("#hidDelBigIDs").val(); //删除的大题IDs
            delbigIDs = delbigIDs.substr(1);

            var ExamPaperInfo = {
                ExamPaper: paperInfo,
                ExamBigQuestioninfoList: bigQList,
                DelBigQIDs: delbigIDs
            };

            return ExamPaperInfo;
        }
    
    </script>
    <style type="text/css">
        .ddlT {
            *width: 264px;
            width: 262px;            
        }
    </style>
</head>
<body>
    <form runat="server">
    <div class="w980">
        <div class="taskT" runat="server" id="Pagetitle">
            添加试卷</div>
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <input type="hidden" runat="server" id="hidEPID" value="" />
                        <span class="redColor">*</span>试卷名称：</label>
                    <span>
                        <input type="text" id="txtName" runat="server" value="" class="w260" />
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>所属分组：</label><span>
                            <asp:DropDownList runat="server" ID="ddlBGIDs" CssClass="ddlT" Style="border: #CCC 1px solid;">
                            </asp:DropDownList>
                        </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>分类：</label><span>
                            <asp:Repeater ID="RptCatage" runat="server">
                                <ItemTemplate>
                                    <input type="radio" id="ecID" runat="server" name="paperCatage" value='<%#Eval("ECID") %>' /><em><%#Eval("Name") %></em>&nbsp;
                                </ItemTemplate>
                            </asp:Repeater>
                        </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试说明：</label><span>
                            <textarea name="" id="txtDes" runat="server" cols="1" rows="2"></textarea>
                        </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>总分：</label><span>
                            <input type="text" id="txtTotalScore" runat="server" value="" class="w260" /><span
                                style="color: Gray;" id="spanmaxscore"></span></span></li>
            </ul>
        </div>
        <div class="addexam sjList">
            <ul>
                <li>
                    <label>
                        试卷大题：</label><span>
                            <input type="button" id="btnAddBigQ" name="" value="添加大题" class="btn98" style="border: none;">
                        </span></li>
            </ul>
            <div class="tableList clearfix tableList2">
                <table border="1" cellpadding="0" cellspacing="0" id="TbbigList">
                    <tr>
                        <th>
                            编号
                        </th>
                        <th>
                            大题名称
                        </th>
                        <th>
                            题型
                        </th>
                        <th>
                            题量/实际题量
                        </th>
                        <th>
                            每题分值
                        </th>
                        <th>
                            设置的总分
                        </th>
                        <th>
                            实际总分
                        </th>
                        <th>
                            出题方式
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="RptBigQuestion" runat="server">
                        <ItemTemplate>
                            <tr class="bigItem">
                                <td name="tdindex">
                                    <%# Container.ItemIndex+1 %>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <%# GetTypeName(Eval("AskCategory").ToString())%>
                                </td>
                                <td>
                                    <%#Eval("QuestionCount")%>/<%#Eval("realQCount")%>
                                </td>
                                <td>
                                    <%#Eval("EachQuestionScore")%>
                                </td>
                                <td name="bigitemTotol">
                                    <%# Convert.ToInt32(Eval("EachQuestionScore").ToString()) * Convert.ToInt32(Eval("QuestionCount").ToString())%>
                                </td>
                                <td>
                                    <%# Convert.ToInt32(Eval("EachQuestionScore").ToString()) * Convert.ToInt32(Eval("realQCount").ToString())%>
                                </td>
                                <td>
                                    手动
                                </td>
                                <td>
                                    <span>
                                        <input type="button" style="border: none;" class="btnattach" value="编辑大题" name="btnEditBig">
                                    </span><span>
                                        <input type="button" style="border: none;" class="btnattach" value="编辑小题" name="btnEditSmall">
                                    </span><span>
                                        <input type="button" style="border: none;" class="btnattach" value="删除" name="btnDelBig">
                                    </span>
                                </td>
                                <input type='hidden' name='bigIndex' value="<%# Container.ItemIndex+1 %>" />
                                <input type="hidden" value='<%#Eval("BQID")%>' name="bigQBQID">
                                <input type="hidden" value='<%#Eval("SmallQIDs")%>' name="bigSmallQIDs">
                                <input type="hidden" value='<%#Eval("Name")%>' name="bigQName">
                                <input type="hidden" value='<%#Eval("BQDesc")%>' name="bigQDes">
                                <input type="hidden" value='<%#Eval("AskCategory")%>' name="bigQType">
                                <input type="hidden" value='<%#Eval("QuestionCount")%>' name="bigQCount">
                                <input type="hidden" value='<%#Eval("EachQuestionScore")%>' name="bigQScore">
                                <input type="hidden" value='<%#Eval("realQCount")%>' name="bigQRealTCount">
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <img id="imgLoadingPop" src="/Images/blue-loading.gif" style="display: none" />
            <input type="button" id="btnPreview" name="" value="预 览" />&nbsp;&nbsp;
            <input type="button" id="btnSave" runat="server" name="" value="保 存" />&nbsp;&nbsp;
            <input type="button" id="btnSumbit" name="" value="提 交" />&nbsp;&nbsp;
        </div>
    </div>
    </form>
    <input type="hidden" id="hidBigQIndex" value="" />
    <input type="hidden" id="hidSmallQIDs" value="" />
    <input type="hidden" id="hidBigQBQID" value="" />
    <input type="hidden" id="hidBigQName" value="" />
    <input type="hidden" id="hidBigQDesc" value="" />
    <input type="hidden" id="hidBigQType" value="" />
    <input type="hidden" id="hidBigQScore" value="" />
    <input type="hidden" id="hidBigQTotolCount" value="" />
    <input type="hidden" id="hidBigQRealTotolCount" value="" />
    <%--可用最大分数--%>
    <input type="hidden" id="hidBigQMaxScore" value="" />
    <%--是否是点击确定关闭的大题窗口--%>
    <input type="hidden" id="hidIsOk" value="" />
    <%--是否是点击确定关闭的小题窗口--%>
    <input type="hidden" id="hidIsSmallOk" value="" />
    <%--保存还是提交(保存：save  提交: sub)--%>
    <input type="hidden" id="inputSaveOrSub" value="" />
    <%--删除的大题IDs--%>
    <input type="hidden" id="hidDelBigIDs" value="" />
    <%--预览的Form--%>
    <form id="previewForm" action="<%=ExitAddress %>/ExamOnline/ExamPaperStorage/ExamPaperPreview.aspx"
    target="_blank" method="post">
    <input type="hidden" id="ExamPaperInfoStr" name="ExamPaperInfoStr" value="" />
    </form>
</body>
</html>
