<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyPassword.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ModifyPassword" %>
<div class="pop pb15 popuser openwindow">
    <div class="title bold">
        <h2>
            修改密码</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('UpdatePasswordPoper',false);">
        </a></span>
    </div>
    <div class="moveC clearfix" style="margin-top: 15px;">
        <ul class="clearfix">
            <li>
                <label>
                    <span class="redColor">*</span>帐号名称：</label><span class="gh"><input type="text" id="txtUserName"
                        value='' class="w190" /></span></li>
            <li>
                <label>
                    <span class="redColor">*</span>原密码：</label><span class="gh"><input type="password"
                        id="txtOldPassword" value='' class="w190" /></span></li>
            <li>
                <label>
                    <span class="redColor">*</span>新密码：</label><span class="gh"><input type="password"
                        id="txtNewPassword" value='' class="w190" /></span><span  class="redColor">&nbsp;&nbsp;6-10位字符</span></li>
            <li>
                <label>
                    <span class="redColor">*</span>确认新密码：</label><span class="gh"><input type="password"
                        id="txtConfirmNewPassword" value='' class="w190" /></span></li>
        </ul>
    </div>
    <div class="btn mt20">
        <input name="" id="btnSave" type="button" onclick="Submit()" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('UpdatePasswordPoper',false);" /></div>
    <input id="hidUserID" type="hidden" value='' />
</div>
<script type="text/javascript">
    function Submit() {
        var url = "../AjaxServers/LoginManager.ashx";

        var userName = $.trim($("#txtUserName").val());
        var oldPassword = $.trim($("#txtOldPassword").val());
        var newPassword = $.trim($("#txtNewPassword").val());
        var confirmPassword = $.trim($("#txtConfirmNewPassword").val());

        var postBody = "isVal=updatepwd&username=" + URLencode(userName) + "&oldpwd=" + URLencode(oldPassword) + "&pwd=" + URLencode(newPassword);
        var pody = {
            UserName: userName,
            OldPwd: oldPassword,
            Pwd: newPassword,
            ConfirmPassword: confirmPassword
        };

        var msg = Verify(pody);
        if (msg == "") {
            AjaxPost(url, postBody, function (data) {
                if (data == "success") {
                    $.jPopMsgLayer("修改成功", function () {
                        $.closePopupLayer('UpdatePasswordPoper', false);
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
        else {
            $.jAlert(msg);
        }
    }
    function Verify(pody) {
        var msg = "";
        if (pody.UserName == "") {
            msg = "帐号名称不能为空";
        }
        if (pody.Pwd == "") {
            msg = "新密码不能为空";
        }
        else if (pody.Pwd.length < 6 || pody.Pwd.length > 10) {
            msg = "新密码字符必须在6-10个字符之间";
        }
        if (pody.ConfirmPassword != pody.Pwd) {
            msg = "您两次输入的新密码不一致，请确认";
        }

        return msg;
    }
</script>
