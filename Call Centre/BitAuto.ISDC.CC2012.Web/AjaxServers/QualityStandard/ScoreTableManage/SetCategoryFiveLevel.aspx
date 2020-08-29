<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetCategoryFiveLevel.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetCategoryFiveLevel" %>

<script type="text/javascript">

    $("#divCategoryPopup").ready(function () {

        EditScoreT.BindCategoryHTML(); //绑定
        $("#spanPinFenBiaoClass").text(EditScoreT.scoreObj.ScoreType);
        $("#divCategoryPopup ul.categoryRow:visible [name='cScore']").live("keyup", function () {
            var thisVal = $.trim($(this).val());
            if (isNum(thisVal) && $.trim(thisVal) != "") {
                if (parseInt(thisVal) > 100) {
                    $("#spanCanUseScore").css("color", "red").text("请输入不大于100的正整数");
                    $(this).val(thisVal.substring(0, thisVal.length - 1));
                }
                else {
                    $(this).val(parseInt(thisVal));
                    GetCanUseScore();
                }
            }
            else {
                if (thisVal.charAt(thisVal.length - 1) == ".") {
                    $(this).val(thisVal.substring(0, thisVal.length - 1));
                }
                $("#spanCanUseScore").css("color", "red").text("请输入不大于100的正整数");
                $(document.activeElement).css({ "border-color": "#FF0000", "color": "red" });
                $("#btnCSubmit").attr("disabled", true).css("color", "#666"); ;
            }
        });
        //提交
        $("#btnCSubmit").click(function () {
            GetCanUseScore();
            if ($("#spanCanUseScore").text() == "0") {
                EditScoreT.SaveCategory();
            }
        });
        GetCanUseScore();
    });
    //计算可用分值
    function GetCanUseScore() {
        var totalscore = 0;
        var hasBadData = false;
        $("#divCategoryPopup ul.categoryRow:visible").each(function (i, v) {
            var $scoreobj = $(this).find(" input[name='cScore']");
            var score = $scoreobj.val();
            if ($.trim(score) != "") {
                if (isNum(score)) {
                    $scoreobj.css({ "border-color": "#CCC", "color": "#666" })
                    totalscore += parseInt(score);
                }
                else {
                    hasBadData = true;
                }

            }
        });
        if (!hasBadData) {
            totalscore = parseInt($("#spanTotalScore").text()) - totalscore;
            if (totalscore < 0) {
                if ($(document.activeElement).attr("id") != "btnCategory") {
                    $(document.activeElement).css({ "border-color": "#FF0000", "color": "red" });
                }
                $("#spanCanUseScore").css("color", "red").text(totalscore + "（类别分值合计已超过总分值）");
                $("#btnCSubmit").attr("disabled", true).css("color", "#666"); ;
            }
            else {
                if ($(document.activeElement).attr("id") != "btnCategory") {
                    $(document.activeElement).css({ "border-color": "#CCC", "color": "#666" });
                }
                if (totalscore == 0) {
                    $("#spanCanUseScore").css("color", "black").text(totalscore);
                    $("#btnCSubmit").attr("disabled", false).css("color", "#fff");
                }
                else {
                    $("#spanCanUseScore").css("color", "red").text(totalscore + "（可用分值为0才能保存）");
                    $("#btnCSubmit").attr("disabled", true).css("color", "#666");
                }
            }
        }
    }

</script>
<div class="pop pb15 w700 zjpf openwindow" id="divCategoryPopup">
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">
            质检类别</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('CategoryPopup');">
        </a></span>
    </div>
    <div style="margin: 15px 25px; font-size: 14px; font-weight: bold; border-bottom: 1px solid #CCC;
        padding-bottom: 5px;">
        <label style="float: left; margin-left: 20px;">
            总分值：<span id="spanTotalScore">100</span></label>
        &nbsp;&nbsp;
        <label style="float: right; margin-right: 20px; *margin-top: -28px;
            padding-top: 0px \9; *padding-top: 0px;">
            可用分值：<span id="spanCanUseScore">100</span></label>
    </div>
    <ul class="clearfix ft14" style="clear: both;">
        <li>
            <ul style="margin-left: 72px; padding: 0;">
                <li style="line-height: 25px;"><span class="pfbz Categorytitle" style="width: 390px;">
                    质检类别</span> <span class="fz Categorytitle typeFlog" style="width: 80px;">分值</span>
                </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input name="" type="button" id="btnCSubmit" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('CategoryPopup');" /></div>
</div>
