<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionSelect.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage.QuestionSelect" %>

<link href="../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式

    $(document).ready(function () {

        BindSelect(1, 0);

        //小题选择窗口的选择类别
        $("#selKCID1").change(function () {
            $("#selKCID3").hide();
            BindSelect(2, $("#selKCID1").val());
        });
        $("#selKCID2").change(function () {
            $("#selKCID3").hide();
            BindSelect(3, $("#selKCID2").val());
        });

        //计算还有多少题
        var MaxCount = '<%=MaxCount %>'; //最大个数
        var SelCount = '<%=SelCount %>'; //当前选择个数\
        var c = Number(MaxCount) - Number(SelCount);
        $("#pSel").text("您当前已选择" + SelCount + "题，还需要添加" + c + "题。");
    });

    // n 要绑定控件所在级别   pid 上一个ID
    function BindSelect(n, pid) {

        AjaxPostAsync("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid ,regionid:<%=RegionID %>}, null, function (data) {

            $("#selKCID" + n).html("");
            $("#selKCID" + n).append("<option value='-1'>请选择</option>");

            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {

                    $.each(jsonData.root, function (idx, item) {
                        $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                    });
                    if (n == 3) {
                        $("#selKCID3").show();
                    }
                    else {
                        $("#selKCID3").hide();
                    }
                }
            }
        });
    }

    //查询操作
    function QustionSearch() {
        var kcid = "";
        var kcid;
        if ($("#selKCID3").val() != "-1") {
            kcid = $("#selKCID3").val();
        }
        else if ($("#selKCID2").val() != "-1") {
            kcid = $("#selKCID2").val();
        }
        else if ($("#selKCID1").val() != "-1") {
            kcid = $("#selKCID1").val();
        }
        else {
            kcid = "";
        }

        var QustionType = '<%=QustionType %>';


        var pody = 'KCID=' + escape(kcid) + '&QustionName=' + escape($.trim($('#txtQuestionName').val())) + '&QustionType=' + QustionType + '&page=1' + '&random=' + Math.random();
        $('#divMemberList').load('/ExamOnline/ExamPaperStorage/QuestionSelect.aspx #divMemberList > *', pody, LoadDivSuccess);
    }

    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式
    }

    //删除选择的
    function DelSelectQuestion(obj) {
        var curTr = $(obj).parent().parent();
        $.jConfirm("确定要删除此题吗？", function (isOk) {
            if (isOk) {
                curTr.remove();

                //计算还有多少题
                var MaxCount = '<%=MaxCount %>'; //最大个数
                var currCount = $("a[name='DelSelect']").length; //当前个数
                var SelCount = currCount; //当前选择个数\
                var c = Number(MaxCount) - Number(SelCount);
                $("#pSel").text("您当前已选择" + SelCount + "题，还需要添加" + c + "题。");
            }
        });

    }

    //选择操作ask, type, askCategory, createtime
    function SelectQustion(qid) {
        var tdObj=$('#tableMemberList a[id="' + qid + '"]').parent().parent().children('td');
        var ask = $.trim($(tdObj[1]).html());
        var type = $.trim($(tdObj[2]).text());
        var askCategory =  $.trim($(tdObj[3]).text());
        var createtime =  $.trim($(tdObj[4]).text());
        var MaxCount = '<%=MaxCount %>'; //最大个数
        var currCount = $("a[name='DelSelect']").length; //当前个数
        if (currCount >= MaxCount) {
            $.jAlert("不能继续添加试题，最多可以选择个" + MaxCount + "个试题");
        }
        else {
            var havCount = $("a[name='DelSelect'][qid='" + qid + "']").length;
            if (havCount > 0) {
                $.jAlert("已选择此题，不能重复选择");
            }
            else {
                var html = "<tr><td><a href='#'  name='DelSelect' qid='" + qid + "'   ><img src='/Images/close.gif' title='删除'/></a></td>"
                + "<td class='l'>" + ask + "&nbsp;</td>"
                + "<td class='l'>" + type + "&nbsp;</td>"
                + "<td class='l'>" + askCategory + "&nbsp;</td>"
                + "<td cls='1'>" + createtime + "&nbsp;</td>"
                + "</tr>";
                $("#tableIDs").append(html);
               
                //计算还有多少题
                var MaxCount = '<%=MaxCount %>'; //最大个数
                var SelCount = currCount + 1; //当前选择个数\
                var c = Number(MaxCount) - Number(SelCount);
                $("#pSel").text("您当前已选择" + SelCount + "题，还需要添加" + c + "题。");

            }
        }



    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divMemberList').load('/ExamOnline/ExamPaperStorage/QuestionSelect.aspx #divMemberList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }

    //保存选择的问题
    function SaveSelectQuestion() {

        var IDs = "";
        $("a[name='DelSelect']").each(function () {
            IDs = IDs + $(this).attr("qid") + ",";
        });
        IDs = IDs.substr(0, IDs.length - 1);

        $("[id$='QustionSelectAjaxPopup']").data('selectIDs', IDs);

        $("#hidIsSmallOk").val("1");
        $.closePopupLayer('QustionSelectAjaxPopup');
    }


</script>
<div class="pop pb15 openwindow" style="width: 770px;">
    <div class="title bold">
        <h2>
            选择试题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('QustionSelectAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1" style="width: 360px;">所属分类：
            <select id="selKCID1" class="w90">
                <option value='-1'>请选择</option>
            </select>
            <select id="selKCID2" class="w90">
                <option value='-1'>请选择</option>
            </select>
            <select id="selKCID3" class="w90">
                <option value='-1'>请选择</option>
            </select>
        </li>
        <li class="name1" style="width: 260px;">试题：
            <input type="text" name="txtQuestionName" id="txtQuestionName" runat="server" class="w190" />
        </li>
        <li class="btn">
            <input name="" type="button" value="查 询" onclick="javascript:QustionSearch();" class="btnSave bold" />
        </li>
    </ul>
    <div class="Table2" id="divMemberList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableMemberList">
            <tbody>
                <tr class="bold">
                    <th width="10%">
                        操作
                    </th>
                    <th width="50%">
                        试题
                    </th>
                    <th width="10%">
                        题型
                    </th>
                    <th width="15%">
                        所属分类
                    </th>
                    <th width="15%">
                        创建日期
                    </th>
                </tr>
                <asp:repeater id="rptQuestionList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td> 
                                <a href="javascript:SelectQustion('<%# Eval("KLQID") %>');" name='<%# Eval("KLQID")%>' id='<%# Eval("KLQID") %>'>选择</a>
                            </td>
                            <td class="l" style="text-align:left;">
                                <%# Eval("Ask")%>&nbsp;
                            </td>
                            <td class="l">
                                <%# GetTypeNameByID(Eval("AskCategory").ToString())%>&nbsp;
                            </td> 
                            <td class="l">
                                <%# Eval("Name")%>&nbsp;
                             </td> 
                            <td class="1">
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                             </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
    </div>
    <p id="pSel" style="padding-left: 30px; color: Red;">
    </p>
    <div class="Table2" id="div1">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List">
            <tbody id="tableIDs">
                <tr class="bold">
                    <th width="10%">
                        操作
                    </th>
                    <th width="40%">
                        试题
                    </th>
                    <th width="15%">
                        题型
                    </th>
                    <th width="20%">
                        所属分类
                    </th>
                    <th width="15%">
                        创建日期
                    </th>
                </tr>
                <asp:repeater id="rptIDsList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='#' name='DelSelect' qid='<%# Eval("KLQID") %>'>
                                    <img src='/Images/close.gif' title='删除' /></a>
                            </td>
                            <td class='l'>
                                <%# Eval("Ask")%>&nbsp;
                            </td>
                            <td class='l'>
                                <%# GetTypeNameByID(Eval("AskCategory").ToString())%>&nbsp;
                            </td>
                            <td class='l'>
                                <%# Eval("Name")%>&nbsp;
                            </td>
                            <td class='1'>
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown2"></asp:literal>
            </p>
        </div>
    </div>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" name="" value="提 交" onclick="javascript:SaveSelectQuestion();"
            class="btnSave bold" />&nbsp;&nbsp;
        <input type="button" name="" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('QustionSelectAjaxPopup');">
    </div>
</div>
