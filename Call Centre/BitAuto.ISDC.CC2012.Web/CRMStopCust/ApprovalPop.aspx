<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.ApprovalPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        $(function () {
            if ("<%=Type %>" == "1") {
                $("#ucDivReject").hide();
            } else {
                $("#ucDivApprovalOK").hide();
            }
        })


        //驳回、审核操作；type-1：审核；type-2：驳回
        function LibReject(type) {
            $("#btnApproval").attr("disabled", "disabled");
            $("#btnReject").attr("disabled", "disabled");

            var rejectReason = type == 1 ? $.trim($("#txtPend").val()) : $.trim($("#txtRejectReason").val());


            if (rejectReason == "") {
                $("#btnApproval").attr("disabled", "");
                $("#btnReject").attr("disabled", "");
                $.jAlert("<%=desc %>不能为空！");
                return false;
            }
            if (Len(rejectReason) > 300) {
                $("#btnApproval").attr("disabled", "");
                $("#btnReject").attr("disabled", "");
                $.jAlert("<%=desc %>长度不能超过150个字！");
                return false;
            }

            rejectReason = encodeURI(rejectReason);

            $.post("OperHandler.ashx", { 'Action': 'OperDeal', 'Type': type, 'CRMStopCustApplyID': "<%=CRMStopCustApplyID %>", 'TaskID': "<%=TaskID %>", 'Reason': rejectReason, 'random': Math.random() }, function (data) {
                $("#btnApproval").attr("disabled", "");
                $("#btnReject").attr("disabled", "");
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "true") {
                    $.jPopMsgLayer("操作成功", function () {
                        $("#hidOk").data("result", "true");
                        $.closePopupLayer('RejectReasonPopup', false);
                    });
                } else {
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
    <div class="pop pb15 openwindow">
        <div class="title bold">
           <h2><%=title %></h2> 
            <span><a onclick="javascript:$.closePopupLayer('RejectReasonPopup',false);" href="javascript:void(0)">
            </a></span>
        </div>
        <div class="popT bold">
            <span class="redColor">*</span><%=desc %></div>
        <div id="ucDivApprovalOK">
            <div class="bh">
                <textarea name="" cols="" rows="" id="txtPend" style="display: block; clear: both;
                    width: 520px; height: 80px; margin: 5px 0;"></textarea>
            </div>
            <div class="btn">
                <input type="button" onclick="javascript:LibReject(1);" class="btnSave bold" value="确定" id="btnApproval" />
                <input type="button" onclick="javascript:$.closePopupLayer('RejectReasonPopup',false);"
                    class="btnCannel bold" value="取消" />
            </div>
        </div>
        <div id="ucDivReject">
            <div class="bh">
                <textarea name="" cols="" rows="" id="txtRejectReason" style="display: block; clear: both;
                    width: 520px; height: 80px; margin: 5px 0;"></textarea>
            </div>
            <div class="btn">
                <input type="button" onclick="javascript:LibReject(2);" class="btnSave bold" value="确定" id="btnReject" />
                <input type="button" onclick="javascript:$.closePopupLayer('RejectReasonPopup',false);"
                    class="btnCannel bold" value="取消" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
