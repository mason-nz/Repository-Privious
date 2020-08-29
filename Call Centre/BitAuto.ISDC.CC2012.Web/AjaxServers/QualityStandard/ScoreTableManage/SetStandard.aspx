<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetStandard.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetStandard" %>

<script type="text/javascript">

    $("#divstandardPopup").ready(function () {

        //绑定
          EditScoreT.BindStandardHTML();

          //保存
          $("#btnSSave").click(function () { EditScoreT.SaveStandard(); });

//          //提交
//          $("#btnSSubmit").click(function () { EditScoreT.SubmitStandard(); });

    });
</script>
<div class="pop pb15 w700 zjpf openwindow" id='divstandardPopup'>
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">质检标准</h2><span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('StandardPopup');"></a></span></div>
    <ul class="clearfix ft14">
        <li>
            <label>
                所属类别：</label><span><select name="" id='standardSelCatagory' style="width: 188px;">
                </select>&nbsp;<select name="" id='standardSelItem' style="width: 188px;"></select>
                </span>
            <ul style="margin-left: 105px; padding: 0;">
                <li style="line-height: 25px;"><span class="pfbz StandardTitle" style="width: 390px;">质检标准</span>
                    <span class="fz StandardTitle typeFlog2" style="width: 80px;">分值</span>
                    <span class="fz StandardTitle typeFlog" style="width: 80px;">致命</span>
                </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input name="" id="btnSSave" type="button" value="保存" class="btnSave bold" />
        <%--<input name="" id="btnSSubmit" type="button" value="提 交" class="btnSave bold" />--%>
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('StandardPopup');" /></div>
</div>
