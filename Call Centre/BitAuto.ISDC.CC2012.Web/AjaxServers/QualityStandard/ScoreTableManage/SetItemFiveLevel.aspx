<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetItemFiveLevel.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetItemFiveLevel" %>

<script type="text/javascript">

    $("#divItemPopup").ready(function () {
        ///绑定
        EditScoreT.BindItemHTML();
        // $("#spanPinFenBiaoClass").text(EditScoreT.scoreObj.ScoreType);
        $("#divItemPopup ul.itemRow:visible [name='IScore']").live("keyup", function () {
            var pattern = /^[0-9]+(\.[05])?$/;
            var thisVal = $.trim($(this).val());
            if (pattern.test(thisVal) && thisVal % 0.5 == 0) {
                if (parseFloat(GetSelectOptionScore($("#itemSelCatagory").find("option:selected").text())) < parseFloat(thisVal)) {
                    $("#spanCanUseScore").css("color", "red").text("输入分值不能大于该类别总分值");
                    $(this).val(thisVal.substring(0, thisVal.length - 1));
                    GetCanUseScore();
                    return false;
                }
                else if (thisVal.charAt(thisVal.length - 1) != ".") {
                    $(this).val(parseFloat(thisVal));
                }
                GetCanUseScore();

            }
            else {
                if (thisVal.charAt(thisVal.length - 1) != ".") {
                    $(this).val(thisVal.substring(0, thisVal.length - 1));
                }
                $("#spanCanUseScore").css("color", "red").text("请输入数字必须是0.5的倍数");
                $(document.activeElement).css({ "border-color": "#FF0000", "color": "red" });
                $("#btnISubmit").attr("disabled", true).css("color", "#666");
            }
        });
        //提交
        $("#btnISubmit").click(function () {
            GetCanUseScore();
            if ($("#spanCanUseScore").text() == "0") {
                EditScoreT.SaveItem();
            }
        });
        GetCanUseScore();
    });

    //计算可用分值
    function GetCanUseScore() {
        var totalscore = 0;
        var pattern = /(?=.*[^0.])(0|[1-9]\d*)(\.\d+)?/;
        $("#divItemPopup ul.itemRow:visible").each(function (i, v) {
            var $scoreObj = $(this).find(" input[name='IScore']");
            var score = $.trim($scoreObj.val());
            if (pattern.test(score) && score % 0.5 == 0) {
                $scoreObj.css({ "border-color": "#CCC", "color": "#666" })
            }
            //console.log("score" + i + " = " + score);
            if (score != "") {
                totalscore += parseFloat(score);
            }
        });
        //console.log("itemSelCatagory = " + $("#itemSelCatagory").find("option:selected").text());
        //console.log("totalscore1 = " + totalscore);
        totalscore = GetSelectOptionScore($("#divItemPopup #itemSelCatagory").find("option:selected").text()) - totalscore;
        //console.log("totalscore2 = " + totalscore);
        if (totalscore < 0) {
            if ($(document.activeElement).attr("type") == "text") {
                $(document.activeElement).css({ "border-color": "#FF0000", "color": "red" });
            }
            $("#spanCanUseScore").css("color", "red").text(totalscore + "（项目分值合计已超过类别总分值）");
            $("#btnISubmit").attr("disabled", true).css("color", "#666");
        }
        else {
            if ($(document.activeElement).attr("type") == "text") {
                $(document.activeElement).css({ "border-color": "#CCC", "color": "#666" });
            }
            if (totalscore == 0) {
                $("#spanCanUseScore").css("color", "black").text(totalscore);
                $("#btnISubmit").attr("disabled", false).css("color", "#fff");
            }
            else {
                $("#spanCanUseScore").css("color", "red").text(totalscore + "（可用分值为0才能保存）");
                $("#btnISubmit").attr("disabled", true).css("color", "#666");
            }

        }
    }

    function GetSelectOptionScore(strOptionText) {

        var startIndex = strOptionText.indexOf('(');
        var endIndex = strOptionText.lastIndexOf('分');
        //console.log("strOptionText = " + strOptionText + " ;startIndex = " + startIndex + "  ;endIndex = " + endIndex);
        if (startIndex >= 0 && endIndex > 0 && endIndex - startIndex > 1) {
            return parseFloat($.trim(strOptionText.substring(startIndex + 1, endIndex)));
        }
        else {
            return 0;
        }
    }
</script>
<div class="pop pb15 w700 zjpf openwindow" id='divItemPopup'>
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">
            质检项目</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ItemPopup');">
        </a></span>
    </div>
    <div style="margin: 15px 25px; font-size: 14px; font-weight: bold; border-bottom: 1px solid #CCC;
        padding-bottom: 5px;">
        &nbsp;
        <label style="float: right; margin-right: 20px; *margin-top:-28px;padding-top:0px \9; *padding-top:0px;">
            可用分值：<span id="spanCanUseScore">100</span>
        </label>
        <%-- <label>评分表类型：</label><span id="spanPinFenBiaoClass"></span>--%>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                所属类别：</label><span><select name="" id='itemSelCatagory' style="width: 200px;">
                </select>
                </span>
            <ul style="margin-left: 105px; padding: 0;">
                <li style="line-height: 25px;"><span class="pfbz ItemTitle" style="width: 390px;">项目名称</span>
                    <span class="fz ItemTitle typeFlog" style="width: 80px;">满分分值</span> </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input name="" id="btnISubmit" type="button" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('ItemPopup');" /></div>
</div>
