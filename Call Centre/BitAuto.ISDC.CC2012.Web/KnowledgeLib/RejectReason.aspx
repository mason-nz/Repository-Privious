<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RejectReason.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.RejectReason" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        //驳回（批量、单个）
        function LibReject(klid) {
            var KLIDs = klid;
            var rejectReason = encodeURI($.trim($("#txtRejectReason").val()));
            if (rejectReason == "") {
                $.jAlert("驳回理由不能为空！");
                return false;
            }
            $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'RejectKnowledgeLib', 'KLID': KLIDs, 'RejectReason': rejectReason, 'random': Math.random() }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    if (jsonData.msg.indexOf("成功") != -1) {
                        $.jPopMsgLayer(jsonData.msg, function () {
                            if ('<%=RequestAction %>' == "auditpage") {
                                window.close();
                            }
                            else {
                                window.location.reload();
                            }
                        });
                    }
                    else {
                        $.jAlert(jsonData.msg, function () {
                            if ('<%=RequestAction %>' == "auditpage") {
                                window.close();
                            }
                            else {
                                window.location.reload();
                            }
                        });
                    }
                }
                else {
                    $.jAlert("操作失败");
                    return false;
                }
            });
        }
    </script>
    <style type="text/css">
        .pop .bh
        {
            margin: 5px 10px 10px;
        }
        .pop .bh textarea
        {
            width: 575px;
            height: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15">
        <div class="title bold">
            驳回 <a onclick="javascript:$.closePopupLayer('RejectReasonPopup',false);" href="javascript:void(0)">
            </a>
        </div>
        <div class="popT bold">
            <span class="redColor">*</span>驳回理由</div>
        <div class="bh">
            <textarea name="" cols="" rows="" id="txtRejectReason" style="display: block; clear: both;
                width: 520px; height: 80px; margin: 5px 0;"></textarea>
        </div>
        <div class="btn">
            <input type="button" onclick="javascript:LibReject('<%=RequestKLID %>');" class="btnSave bold"
                value="确定" />
            <input type="button" onclick="javascript:$.closePopupLayer('RejectReasonPopup',false);"
                class="btnCannel bold" value="取消" />
        </div>
    </div>
    </form>
</body>
</html>
