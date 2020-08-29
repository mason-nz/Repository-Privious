<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteTask.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.DeleteTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">

        function Delete() {
            if ($.jConfirm("确认对该任务进行删除操作？", function (r) {
                if (r) {
                    var TaskID = "<%=TaskID %>";
                    var Reason = encodeURI($.trim($("#txtReason").val()));
                    if (Reason == "") {
                        $.jAlert("删除原因不能为空！");
                        return false;
                    }
                    $.post("Handler.ashx", { 'Action': 'DeleteTask', 'TaskID': TaskID, 'Reason': Reason, 'random': Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "false") {
                            $.jAlert(jsonData.msg);
                        }
                        else if (jsonData.result == "true") {
                            $.jAlert("操作成功", function () {
                                closePageExecOpenerSearch("btnsearch");
                            });
                        }
                    });
                }
            }));
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
            删除任务 <a onclick="javascript:$.closePopupLayer('ReasonPopup',false);" href="javascript:void(0)">
            </a>
        </div>
        <div class="popT bold">
            <span class="redColor">*</span>删除原因</div>
        <div class="bh">
            <textarea name="" cols="" rows="" id="txtReason" style="display: block; clear: both;
                width: 520px; height: 80px; margin: 5px 0;"></textarea>
        </div>
        <div class="btn">
            <input type="button" onclick="javascript:Delete();" class="btnSave bold"
                value="确定" />
            <input type="button" onclick="javascript:$.closePopupLayer('ReasonPopup',false);"
                class="btnCannel bold" value="取消" />
        </div>
    </div>
    </form>
</body>
</html>
