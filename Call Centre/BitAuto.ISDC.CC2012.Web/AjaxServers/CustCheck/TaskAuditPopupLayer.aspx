<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskAuditPopupLayer.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck.TaskAuditPopupLayer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    function ShowResion() {
        var auditVal = $("input[name='audit']:checked").val();
        if (auditVal != "1") {
            $("#liResion").show();
        }
        else {
            $("#liResion").hide();
        }
    }
    function Submit() {
        $.openPopupLayer({
            name: "WaittingPoper",
            parameters: { FunctionName: "AuditTask" },
            url: "/AjaxServers/RequestWaittingPoper.aspx",
            beforeClose: function (e, data) {
                if (e) {
                }
            }
        });
    }
    function AuditTask() {
        var auditVal = $("input[name='audit']:checked").val();
        var audittype = "";
        if (auditVal == "1") {
            audittype = "AuditPass";
        }
        else if (auditVal == "2") {
            audittype = "AuditRefuse";
        }
        else if (auditVal == "3") {
            audittype = "CallBack";
        }
        var description = escape($("#txtResion").val());
        $.post("/AjaxServers/CustAudit/CustAuditManager.ashx", { Audit: "yes", AuditType: audittype, TID: '<%=PTIDS %>', Description: description }, function (data) {
            var s = $.evalJSON(data);
            if (s.Update == 'yes') {
                $.jPopMsgLayer('操作成功！', function () {
                    search();
                    $.closePopupLayer('AuditTaskPopup', true);
                });
            }
            else {
                var msg = unescape(s.Update).replace('VerifyLogic,', '');
                $.jAlert(msg);
            }
            $.closePopupLayer("WaittingPoper");
        });
    }
    $(document).ready(function () {
        $("input[name='audit']").eq(0).attr("checked", true);
    });
</script>
<div class="pop pb15 openwindow" id="popfowwardtask">
    <div class="title bold">
        <h2>批量审核</h2>
        <span><a href="javascript:void(0);" onclick="javascript:$.closePopupLayer('AuditTaskPopup',true);"
            class="right"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>审核操作<span class="redColor">*</span>：</label>
                <label style="float:none;cursor:pointer;font-weight:normal;"><input type="radio" name="audit" value="1" onclick="ShowResion()" />审核通过</label>
                <label style="float:none;cursor:pointer;font-weight:normal;"><input type="radio" name="audit" value="2" onclick="ShowResion()"/>审核拒绝</label>
                <label style="float:none;cursor:pointer;font-weight:normal;"><input type="radio" name="audit" value="3" onclick="ShowResion()"/>打回</label>
                </li>
        <li id="liResion" style=" display:none">
        <label>原因<span class="redColor"></span>：</label>
        <textarea id="txtResion" cols="5" rows="6">
        </textarea>
        </li>
    </ul>
    <div id="btnForwardTask" class="btn">
        <input name="" type="button" value="确 定" class="btnSave bold" onclick="Submit()" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('AuditTaskPopup',true);"
            class="btnCannel bold" />
        <img id="imgLoadingPop" src="../../Images/blue-loading.gif" style="display: none"
            alt='' />
    </div>
</div>
