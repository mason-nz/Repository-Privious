<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPwd.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCategory.ResetPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <script type="text/javascript">
        $(function () {
            if ("<%=Email %>" != "") {
                $("#txtEmail").val("<%=Email %>");
            }
        });

        //修改email
        function updateEmail() {
            var newEmail = $.trim($("#txtEmail").val());
            var validateStr = validateEmail(newEmail);
            var memberCode = "<%=MemberCode %>";
            if (validateStr != "") {
                $.jAlert(validateStr);
                return false;
            }
            if (memberCode == "") {
                $.jAlert("会员编码不能为空！无法操作");
                return false;
            }

            if ($.jConfirm("是否确认修改？", function (r) {
                if (r) {
                    AjaxPostAsync("/AjaxServers/CustCategory/DealerInfo.ashx", { Action: "UpdateEmail", NewEmail: newEmail, OldEmail: '<%=Email %>', MemberCode: memberCode, r: Math.random() }, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            $("#popupLayer_" + "ResetPwdPopup").data("email", newEmail);
                        }

                        $.jAlert(jsonData.msg);

                    });
                }

            }));

        }

        //发送修改密码的email
        function sentEmail() {
            var memberCode = "<%=MemberCode %>";
            if (memberCode == "") {
                $.jAlert("会员编码不能为空！无法操作");
                return false;
            }

            if ($.jConfirm("是否确认发送修改密码邮件？", function (r) {

                if (r) {
                    AjaxPostAsync("/AjaxServers/CustCategory/DealerInfo.ashx", { Action: "SendEmail", MemberCode: memberCode, r: Math.random() }, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            $.jAlert(jsonData.msg, function () {
                                $.closePopupLayer('ResetPwdPopup', false);
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg);
                        }

                    });
                }

            }));
        }

        //验证email
        function validateEmail(email) {
            var msg = "";

            if (email == "") {
                msg = "Email不能为空！";
            }
            else if (!isEmail(email)) {
                msg = "Email格式不正确！";
            }

            return msg;
        }
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15" style="width: 400px">
        <div class="title bold">
            重置密码 <a onclick="javascript:$.closePopupLayer('ResetPwdPopup',false);" href="javascript:void(0)">
            </a>
        </div>
        <div class="popT bold" style="font-size: 14px">
            &nbsp;&nbsp;&nbsp; <span class="redColor">*</span>修改账号：
            <input type="text" id="txtEmail" />
        </div>
        <div class="btn" style="width: 400px; margin-top: 15px;">
            <input type="button" value="修改" id="btnUpdateEmail" class="btnSave bold" onclick="updateEmail()" />
            &nbsp;&nbsp;&nbsp;
            <input type="button" value="发送邮件" id="btnSentEmail" class="btnSave bold" onclick="sentEmail()" />
        </div>
    </div>
    </form>
</body>
</html>
