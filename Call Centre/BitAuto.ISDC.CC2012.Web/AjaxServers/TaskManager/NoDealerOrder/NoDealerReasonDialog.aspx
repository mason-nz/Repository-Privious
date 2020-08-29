<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoDealerReasonDialog.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder.NoDealerReasonDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">

    function GetInfo() {

        if ($("#dllReason").val() == undefined) {
            $.jAlert("请选择理由", function () { return; });
        }
        else if ($.trim($("#txtContent").val()) == "") {
            $.jAlert("请填写备注", function () { return; });
        }
        else if (Len($.trim($("#txtContent").val())) > 500) {
            $.jAlert("备注字数不能多于500字", function () { return; });
        }
        else {
            $.closePopupLayer('ReasonPopup', true);
        }
     }

</script>

<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            操作提示</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ReasonPopup',false);"
            class="right"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                理由<span class="redColor">*</span>:</label>
            <select id="dllReason" runat="server">
            </select>
        </li>
        <li>
            <label>
                备注<span class="redColor">*</span>:</label><textarea id="txtContent" rows="5" cols="10"></textarea>
        </li>
        <li style="height: 20px; line-height: 20px;">
            <label>
                &nbsp;</label>
            <span class="redColor" id="spanNum"></span></li>
    </ul>
    <div class="btn">
        <input name="" onclick="GetInfo()" type="button" value="确 认" class="btnSave bold" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('ReasonPopup',false);"
            class="btnCannel bold" /></div>
</div>

