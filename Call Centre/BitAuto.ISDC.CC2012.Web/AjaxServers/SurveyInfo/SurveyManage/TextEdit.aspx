<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage.TextEdit" %>

<script type="text/javascript">
    $(document).ready(function () {

        InitControl();

        $("#btnQuestionSumit").bind("click", QuestionSumit);
    });

    //提交
    function QuestionSumit() {

        if (CheckInfo()) {
            var jsonObj = GetQuestionJsonFormWeb();
            //alert(JSON.stringify(jsonObj));
            var str = JSON.stringify(jsonObj);

            $('#popupLayer_' + 'QuestionPopup').data('SingleJson', str);
            $.closePopupLayer('QuestionPopup', true);
        }
    }

    //从页面上获取信息（Json格式）
    function GetQuestionJsonFormWeb() {

        var sqid = '<%=SQID %>';
        if (sqid == "") {
            sqid = "-1"; //新增的，暂时为-1
        }
        var showcolumnnum = "1";

        var IsMustAnswer = "0"; //是否必答
        if ($("#ckbBD").attr("checked") == true) {
            IsMustAnswer = "1";
        }

        var QuestionLinkId = $("#hidQuestionLinkId").val();

        //总的问题Json
        var dataObj = {
            mintextlen: $.trim($("#txtMinLen").val()),
            maxtextlen: $.trim($("#txtMaxLen").val()),
            showcolumnnum: showcolumnnum,
            askcategory: "3",
            ask: escape($("#txtOptionName").val()),
            siid: '<%=SIID %>',
            sqid: sqid,
            option: null, //选项
            matrix: null, //矩阵标题
            ordernum: "0",
            IsMustAnswer: IsMustAnswer,
            IsStatByScore: "0",
            QuestionLinkId: QuestionLinkId//序号
        };

        return dataObj;
    }

    function CheckInfo() {
        var msg = "";
        var isTrue = true;
        if ($.trim($("#txtOptionName").val()) == "") {
            isTrue = false;
            msg += "请输入题干内容！<br/>";
        }

        if ($.trim($("#txtMaxLen").val()) == "") {
            isTrue = false;

            msg += "请输入最大字数！<br/>";
        }
        if ($.trim($("#txtMinLen").val()) == "") {
            isTrue = false;
            msg += "请输入最小字数！<br/>";
        }

        if (isNaN($.trim($("#txtMaxLen").val()))) {
            isTrue = false;
            msg += "最大字数应该为数字！<br/>";
        }
        if (isNaN($.trim($("#txtMinLen").val()))) {
            isTrue = false;

            msg += "最小字数应该为数字！<br/>";
        }
        if (Number($.trim($("#txtMinLen").val())) >= Number($.trim($("#txtMaxLen").val()))) {
            isTrue = false;
            msg += "最小字数应该大于最大字数！<br/>";
        }

        if (msg != "") {
            $.jAlert(msg);
        }

        return isTrue;
    }

    //初始化
    function InitControl() {
        var jsonstr = '<%=JsonStr %>';
        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var action = '<%=action %>';
        if (action == "add") {

        }
        else {
            //编辑

            //获取JSon对象
            var jsonObj = $.evalJSON(unescape(jsonstr));

            $("#txtOptionName").val(jsonObj.ask);
            $("#txtMaxLen").val(jsonObj.maxtextlen);
            $("#txtMinLen").val(jsonObj.mintextlen);

            if (jsonObj.IsMustAnswer == "1") { //必填题
                $("#ckbBD").attr("checked", true);
            }
            //序号
            $("#hidQuestionLinkId").val(jsonObj.QuestionLinkId);
        }

    }

</script>
<div class="pop pb15 w700 openwindow">
    <div class="title pt5 ft16 bold">
       <h2 style="cursor: auto;">文本题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('QuestionPopup');"></a></span></div>
    <ul class="clearfix ft14">
        <li>
            <label>
                题干：</label><span class="tgJr"><input name="" type="text" class="w220" id="txtOptionName" /></span>
                <span><input type="checkbox" value="" id="ckbBD" />必答题</span>
                </li>
        <li>
            <label>
                设置：</label><span>最大字数:
                    <input id="txtMaxLen" name="" type="text" value="200" class="w50" />
                    &nbsp;&nbsp;&nbsp;&nbsp;最小字数:
                    <input id="txtMinLen" name="" type="text" value="0" class="w50" />
                </span></li>
        <li class="ckTx">
            <label>
                参考样式：</label><span>请问您每天使用数字销售助手接待多少批次请问您每天使用数字销售助手接待多少批次的顾客的顾客？</span>
            <ul class="txType">
                <li>
                    <textarea></textarea>
                </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input type='hidden' id='hidQuestionLinkId' value='' />
        <input name="" type="button" value="提 交" class="btnSave bold" id="btnQuestionSumit" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('QuestionPopup');" /></div>
</div>
