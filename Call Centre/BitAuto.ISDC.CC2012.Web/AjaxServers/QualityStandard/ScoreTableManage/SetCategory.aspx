<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetCategory.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetCategory" %>

<script type="text/javascript">

    $("#divCategoryPopup").ready(function () {

        EditScoreT.BindCategoryHTML(); //绑定

        //提交
        $("#btnCSubmit").click(function () { EditScoreT.SaveCategory(); });

    });
     
   

</script>
<div class="pop pb15 w700 zjpf openwindow" id="divCategoryPopup">
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">
            质检类别</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('CategoryPopup');">
        </a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <ul style="margin-left: 105px; padding: 0;">
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
