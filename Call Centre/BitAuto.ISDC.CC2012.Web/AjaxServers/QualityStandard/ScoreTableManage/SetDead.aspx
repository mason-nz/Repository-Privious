<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetDead.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage.SetDead" %>


<script type="text/javascript">

    $("#divDeadPopup").ready(function () {
        
        //绑定
        EditScoreT.BindDeadHTML();

         //提交
        $("#btnDSubmit").click(function () { EditScoreT.SaveDead(); });
    });
</script>
<div class="pop pb15 w700 zjpf openwindow" id="divDeadPopup">
    <div class="title pt5 ft16 bold">
        <h2 style="cursor: auto;">致命项</h2><span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('DeadPopup');"></a></span></div>
    <ul class="clearfix ft14">
        <li>
            <ul style="margin-left: 105px; padding: 0;" >
                <li style="line-height: 25px;"  >
                <span class="pfbz Deadtitle" style="width: 390px;"  >
                    致命项</span> 
                    <span class="fz Deadtitle" style="width: 80px;"  ></span>
                     
                </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input name="" type="button" id="btnDSubmit" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('DeadPopup');" /></div>
</div>
