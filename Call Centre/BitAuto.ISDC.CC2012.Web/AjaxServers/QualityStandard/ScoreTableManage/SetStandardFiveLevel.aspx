<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetStandardFiveLevel.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetStandardFiveLevel" %>

<script type="text/javascript">

    $("#divstandardPopup").ready(function () {
        //绑定
        EditScoreT.BindStandardHTMLFiveLevel();

        //保存
        $("#btnSSave").click(function () {
            if (checkRequiredStandards()) {
                EditScoreT.SaveStandard();
            }
        });

        changestandardtileindexscore($("#divstandardPopup .fivestandardRow:visible"));
    });
    //水平按钮点击事件：根据选择的水平按钮，显示/隐藏对应的质检项目水平项
    function changestatus(thisObj, val) {
        if (val == "1" || val == "5") {
            return;
        }
        var $thisObjParent = $(thisObj).parent().parent().parent();
        if ($(thisObj).hasClass("current")) {
            $(thisObj).removeClass("current");
            //$("#divstandardPopup .clearfix .zjpf_df .standard" + val).hide();
            $thisObjParent.find(" .standard" + val).hide();
            changestandardtileindexscore($thisObjParent)
        }
        else {
            $(thisObj).addClass("current");
            //$("#divstandardPopup .clearfix .zjpf_df .standard" + val).show();
            $thisObjParent.find(" .standard" + val).show();
            changestandardtileindexscore($thisObjParent)
        }

    }
    //排序质检标标准名称前的汉字，计算对应的分数
    function changestandardtileindexscore($thisObjParent) {
        var fullscore = GetSelectOptionScore($("#divstandardPopup #standardSelItem").find("option:selected").text());
        //console.log("text = " + $("#divstandardPopup #standardSelItem").find("option:selected").text() +" ;fullscore : " + fullscore);
        //$("#divstandardPopup .clearfix .zjpf_df .standardli:visible").each(function (i) {
        $thisObjParent.find(" .standardli:visible").each(function (i) {
            var strclassname = $(this).attr("class");
            $(this).find(".count").text("质检标准" + (i + 1) + "：");
            if (strclassname.indexOf("standard1") >= 0) {
                $(this).find(" input:[name='Sore']").val(fullscore);
            }
            else if (strclassname.indexOf("standard2") >= 0) {
                $(this).find(" input:[name='Sore']").val(fullscore*100 * 0.8/100);
            }
            else if (strclassname.indexOf("standard3") >= 0) {
                $(this).find(" input:[name='Sore']").val(fullscore * 100 * 0.6 / 100);
            }
            else if (strclassname.indexOf("standard4") >= 0) {
                $(this).find(" input:[name='Sore']").val(fullscore * 100 * 0.4 / 100);
            }
        });
    }
    function GetSelectOptionScore(strOptionText) {
        var startIndex = strOptionText.indexOf('(');
        var endIndex = strOptionText.lastIndexOf('分');
        if (startIndex >= 0 && endIndex > 0 && endIndex - startIndex > 1) {
            return parseFloat($.trim(strOptionText.substring(startIndex + 1, endIndex)));
        }
        else {
            return 0;
        }
    }

    function checkRequiredStandards(scoreVal) {
        var hasNotMatchInfo = false;
        $("#divstandardPopup .fivestandardRow:visible .standardli:visible").each(function (i) {
            var standardName = $.trim($(this).find(" input:[name='SName']").val());
            var $snameObj = $(this).find(" span.count");
            if (standardName == "") {
                $.jAlert("[" + $snameObj.text().substring(0, $snameObj.text().lastIndexOf("：")) + "]内容不能为空！");
                hasNotMatchInfo = true;
                return false;
            }
        });

        if (hasNotMatchInfo) {
            return false;
        }
        else {
            return true;
        }
    }
</script>
<div class="pop pb15 w700 zjpf zjpf_bz" id='divstandardPopup' style=" width:680px;">
    <div class="title pt5 ft16 bold">
        质检标准<span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('StandardPopup');"></a></span>
    </div>
    <ul class="clearfix ft14 zjpf_sp">
        <li id="selectorsli">
            <label>
                所属类别：</label>
            <span>
                <select name="" id='standardSelCatagory' style="width: 188px;">
                </select>&nbsp;
                <select name="" id='standardSelItem' style="width: 188px;">
                </select>
            </span>
        </li>
 
    </ul>
    <div class="btn">
        <input name="" id="btnSSave" type="button" value="保存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('StandardPopup');" />
    </div>
</div>
 