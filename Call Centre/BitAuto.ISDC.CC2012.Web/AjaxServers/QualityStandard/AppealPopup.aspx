<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppealPopup.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.AppealPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    function submit() {
        var remark = encodeURIComponent($.trim($("#txtRemarkPop").val()));
        if (remark.length == 0) {
            $.jAlert("请填写申诉理由！");
            return;
        }
        $.blockUI({ message: "正在提交中，请等待..." });

        var url = "/AjaxServers/QualityStandard/Handler.ashx";
        var type = '<%=RequestType %>';
        if (type == 'IM') {
            url = "/AjaxServers/QualityStandard/IMHandler.ashx";
        }

        $.post(url, { Action: "appeal", QS_RID: '<%=RequestQS_RID %>', Remark: remark }, function (data) {
            $.unblockUI();
            if (data == "success") {
//                $.jAlert("申诉成功！", function () {
//                    $.closePopupLayer('AppealPopup', true);
//                });
                $.jPopMsgLayer("申诉成功！", function () {
                    $.closePopupLayer('AppealPopup', true);
                });
            }
            else {
                $.jAlert(data);
            }
        });
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            申诉</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AppealPopup',false);"
            class="right"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                申诉理由<span class="redColor">*</span>:</label><textarea id="txtRemarkPop" rows="5"
                    cols="10"></textarea></li>
    </ul>
    <div class="btn">
        <input name="" onclick="submit()" type="button" value="确 定" class="btnSave bold" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('AppealPopup',false);"
            class="btnCannel bold" /></div>
</div>
