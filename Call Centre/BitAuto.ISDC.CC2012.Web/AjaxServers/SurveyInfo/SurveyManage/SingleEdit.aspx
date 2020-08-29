<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage.SingleEdit" %>

<script type="text/javascript">


    $(document).ready(function () {

        InitControl();

        if ($("#ckbPF").attr("checked") == false) { //是否按评分
            $("[name='txtOptionScore']").hide();
        }
        $("#ckbPF").change(function () { if ($(this).attr("checked") == true) { $("[name='txtOptionScore']").show(); SetOptionScore(); } else { $("[name='txtOptionScore']").hide(); } });

        $("[name='rdoOptionCol']").unbind("change");
        $("[name='rdoOptionCol']").change(changeType);

        $("#btnQuestionSumit").bind("click", QuestionSumit);

        $("[name='txtOptionScore']").unbind("keyup");
        $("[name='txtOptionScore']").live("keyup", function (event) {

            if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                $(this).attr("title", "分数值为 " + $(this).val());
                if ($(this).data('tooltipsy') != null) {
                    $(this).data('tooltipsy').destroy();
                }
                $(this).tooltipsy();
                $(this).data('tooltipsy').show();

            } else {
                $(this).attr("value", "");
            }
        });
    });

    //提交
    function QuestionSumit() {

        if (CheckInfo()) {
            var jsonObj = GetQuestionJsonFormWeb();
            var str = JSON.stringify(jsonObj);
            $('#popupLayer_' + 'QuestionPopup').data('SingleJson', str);
            $.closePopupLayer('QuestionPopup', true);
        }
    }

    function CheckInfo() {
        var msg = "";
        var isTrue = true;
        if ($.trim($("#txtOptionName").val()) == "") {
            isTrue = false;
            msg += "题干内容不能为空！<br/>";
        }
        if ($('input:radio[name="rdoOptionCol"]:checked').length == 0) {
            isTrue = false;

            msg += "请选择样式！<br/>";
        }
        $('input:text[name="txtOptionName"]').each(function () {
            if ($.trim($(this).val()) == "") {
                isTrue = false;

                msg += "选项内容不能为空！<br/>";
                return false;
            }
        });
        if (msg != "") {
            $.jAlert(msg);
        }

        return isTrue;
    }


    //从页面上获取信息（Json格式）
    function GetQuestionJsonFormWeb() {

        var optionListObj = new Array();

        //选项
        $("#ulOptionList li").each(function (i, v) {

            var isblank = "0";

            if ($(this).find("input:checkbox").attr("checked") == true) {
                isblank = "1";
            }
            var score = $($(this).find("[name='txtOptionScore']")[0]).val();
            var oneOption = {
                ordernum: i,
                score: score,
                isblank: isblank,
                optionname: escape($(this).find("[name='txtOptionName']").val()),
                sqid: $(this).find("[name='hidSqid']").val(),
                siid: $(this).find("[name='hidSiid']").val(),
                soid: $(this).find("[name='hidSoid']").val(),
                linkid: $(this).find("[name='hidlinkid']").val()
            };
            optionListObj.push(oneOption);
        });


        var sqid = '<%=SQID %>';
        if (sqid == "") {
            sqid = "-1"; //新增的，暂时为-1
        }

        var showcolumnnum = "1";
        if ($("#rdosingleCol1").attr("checked") == true) {
            showcolumnnum = "1";
        }
        else if ($("#rdosingleCol2").attr("checked") == true) {
            showcolumnnum = "2";
        }

        var IsMustAnswer = "0"; //是否必答
        if ($("#ckbBD").attr("checked") == true) {
            IsMustAnswer = "1";
        }
        var IsStatByScore = "0"; //是否按评分
        if ($("#ckbPF").attr("checked") == true) {
            IsStatByScore = "1";
        }

        var QuestionLinkId = $("#hidQuestionLinkId").val();

        //总的问题Json
        var dataObj = {
            mintextlen: "0", //单选临时当是否按评分用
            maxtextlen: "0",
            showcolumnnum: showcolumnnum,
            askcategory: "1",
            ask: escape($("#txtOptionName").val()),
            siid: '<%=SIID %>',
            sqid: sqid,
            option: optionListObj, //选项
            matrix: null, //矩阵标题
            ordernum: "0",
            IsMustAnswer: IsMustAnswer, //是否必答
            IsStatByScore: IsStatByScore, //是否按评分
            QuestionLinkId: QuestionLinkId//序号
        };

        return dataObj;
    }


    //初始化
    function InitControl() {
        var jsonstr = '<%=JsonStr %>';
        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var action = '<%=action %>';
        if (action == "add") {
            //新增
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项

            $("#rdosingleCol1").attr("checked", true);
            $("#txType1").show();
            $("#txType2").hide();

        }
        else {
            //编辑

            //获取JSon对象
            var jsonObj = $.evalJSON(unescape(jsonstr));

            $("#txtOptionName").val(jsonObj.ask);

            if (jsonObj.IsMustAnswer == "1") { //必填题
                $("#ckbBD").attr("checked", true);
            }
            if (jsonObj.IsStatByScore == "1") { //是否按评分
                $("#ckbPF").attr("checked", true);
            }

            //类型
            $($('[name="rdoOptionCol"][value="' + jsonObj.showcolumnnum + '"]')[0]).attr("checked", true);
            if (jsonObj.showcolumnnum == "1") {
                $("#rdosingleCol1").attr("checked", true);
                $("#txType1").show();
                $("#txType2").hide();
            }
            else if (jsonObj.showcolumnnum == "2") {
                $("#rdosingleCol2").attr("checked", true);
                $("#txType2").show();
                $("#txType1").hide();
            }

            //序号
            $("#hidQuestionLinkId").val(jsonObj.QuestionLinkId);

            //生成选项

            $(jsonObj.option).each(function (i, v) {
                var lihtml = " <li>";
                lihtml += "<input type='hidden' name='hidSoid' value='" + v.soid + "' />";
                lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                lihtml += "<input type='hidden' name='hidlinkid' value='" + v.linkid + "' />";
                lihtml += "<span><input name='txtOptionName' type='text' class='w220' value=" + v.optionname + " /></span>";
                lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
                lihtml += "<em> <input name='' type='checkbox' value='' ";

                if (v.isblank == 1) {
                    lihtml += " checked='checked'";
                }
                lihtml += " />填空</em>"

                lihtml += "<span><input name='txtOptionScore' title='分数值为 " + v.score + "' type='text' class='w50 score' value='" + v.score + "' style=' margin-left: 10px; ' /></span>";
                lihtml += " </li>";

                $("#ulOptionList").append(lihtml);
            });

        }

    }

    //修改竖向还是横向
    function changeType() {

        var isShow = $('input:radio[name="rdoOptionCol"]:checked').val();
        if (isShow == "1") {
            $("#txType1").show();
            $("#txType2").hide();
        }
        else if (isShow == "2") {
            $("#txType2").show();
            $("#txType1").hide();
        }

    }

    //新加一个选项，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddOption(obj) {

        //取最大值
        var maxScore = 0;
        $("[name='txtOptionScore']").each(function (i, v) {
            if (!isNaN($(this).val())) {
                if (Number($(this).val()) > maxScore) {
                    maxScore = Number($(this).val());
                }
            }
        });
        maxScore = Number(maxScore) + 1;

        optionIDNum = optionIDNum - 1;

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSoid' value='" + optionIDNum + "' />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<input type='hidden' name='hidlinkid' value='0' />";
        lihtml += "<span><input name='txtOptionName' type='text' class='w220' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
        lihtml += "<em> <input name='' type='checkbox' value='' ";
        lihtml += " />填空</em> ";
        lihtml += "<span><input name='txtOptionScore' type='text' id='" + maxScore + "'  title='' class='w50 score' value=''  style=' margin-left: 10px; ";

        if (!$("#ckbPF").attr("checked")) {
            lihtml += "display: none;";
        }
        lihtml += "' /></span> </li>";
        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulOptionList").append(lihtml);
        }

        SetOptionScore();
    }

    //删除选项
    function DelOption(obj) {
        if ($("#ulOptionList li").length > 1) {
            if ($($(obj).parent().parent().find("input:text[name='txtOptionName']")[0]).val() != "") {
                $.jConfirm("确定删除选项吗？", function (r) {
                    if (r) {
                        $(obj).parent().parent().remove();
                    }
                });
            }
            else {
                $(obj).parent().parent().remove();

            }
        }
        else {
            $.jAlert("至少要有一个选项");
        }
    }

    ///给选项赋值分数
    function SetOptionScore() {

        $("input[name='txtOptionScore']").each(function (i, v) {
            $(this).val(Number(i + 1));
            $(this).attr("title", "分数值是 " + (Number(i + 1)));

            $(this).tooltipsy();

        });
    }
</script>
<div class="pop pb15 w700 openwindow">
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">
            单选题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('QuestionPopup');">
        </a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                题干：</label><span class="tgJr"><input name="" type="text" class="w220" id="txtOptionName" /></span>
            <span>
                <input type="checkbox" value="" id="ckbBD" /><em onclick="emChkIsChoose(this)">必答题</em></span>
            <span>
                <input type="checkbox" value="" id="ckbPF" /><em onclick="emChkIsChoose(this)">按评分统计</em></span>
        </li>
        <li>
            <label>
                选项：</label><span><input id="rdosingleCol1" name="rdoOptionCol" type="radio" value="1" />
                    <em onclick="emChkIsChoose(this)" class="dx">竖向排列</em> &nbsp;&nbsp;<input id="rdosingleCol2"
                        name="rdoOptionCol" type="radio" value="2" />
                    <em onclick="emChkIsChoose(this)" class="dx">每行两列</em></span>
            <ul style="margin-left: 125px; padding: 0;" id="ulOptionList">
            </ul>
            <li class="ckTx" id="txType1" style="display: none;">
                <label>
                    参考样式：</label><span>请问您每天使用数字销售助手接待多少批次请问您每天使用数字销售助手接待多少批次的顾客的顾客？</span>
                <ul class="txType">
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                </ul>
            </li>
            <li id="txType2" style="display: none;">
                <label>
                    参考样式：</label><span>请问您每天使用数字销售助手接待多少批次的顾客？</span>
                <ul class="txType2">
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                    <li>
                        <input name="" type="radio" value="" />3个批次以下 </li>
                </ul>
            </li>
        </li>
    </ul>
    <div class="btn">
        <input type='hidden' id='hidQuestionLinkId' value='' />
        <input name="" type="button" value="提 交" class="btnSave bold" id="btnQuestionSumit" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('QuestionPopup');" /></div>
</div>
