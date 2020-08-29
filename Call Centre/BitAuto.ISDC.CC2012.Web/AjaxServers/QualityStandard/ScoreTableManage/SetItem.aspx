<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetItem.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetItem" %>

<script type="text/javascript">

    $("#divItemPopup").ready(function () {

        ///绑定
        EditScoreT.BindItemHTML();

        //提交
        $("#btnISubmit").click(function () { EditScoreT.SaveItem(); });

    });
</script>
<div class="pop pb15 w700 zjpf openwindow" id='divItemPopup'>
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">
            质检项目</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ItemPopup');">
        </a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                所属类别：</label><span><select name="" id='itemSelCatagory' style="width: 200px;">
                </select>
                </span>
            <ul style="margin-left: 105px; padding: 0;">
                <li style="line-height: 25px;"><span class="pfbz ItemTitle" style="width: 390px;">项目名称</span>
                    <span class="fz ItemTitle typeFlog" style="width: 80px;">分值</span> </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input name="" id="btnISubmit" type="button" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('ItemPopup');" /></div>
</div>
