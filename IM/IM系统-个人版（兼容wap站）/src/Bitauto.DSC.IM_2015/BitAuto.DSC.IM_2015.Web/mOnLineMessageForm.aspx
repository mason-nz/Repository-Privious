<%@ Page Title="客户留言" Language="C#" AutoEventWireup="true" CodeBehind="mOnLineMessageForm.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.mOnLineMessageForm" %>

<link href="Mcss/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    //------------------------------   添加留言start-----------------------------------------------
    //    function addonlinemessage() {
    //        $.openPopupLayer({
    //            name: "AddOnlineMessageAjaxPopup",
    //            parameters: {},
    //            url: "/OnLineMessageForm.aspx?VisitID=1&r=" + Math.random()
    //        });

    //    };
    function saveOnLineMessage() {
        var msg = judgeIsSuccess();
        if (msg != "") {
            alert(msg);
            return false;
        }

        var rdmessageType = 1;
        var tamessagecontents = encodeURIComponent($("#Content").val());
        var txtmessagename = encodeURIComponent($("#Name").val());
        var Identity = encodeURIComponent($("#Identity").val());
        var txtmessagephone = "";
        var txtmessageemail = "";
        if (isEmail($.trim(Identity))) {
            txtmessageemail = Identity;
        }
        else if (isTelOrMobile(Identity)) {
            txtmessagephone = Identity;
        }
        else {

        }

        $.post("/AjaxServers/LayerDataHandlerBefore.ashx", { Action: 'addonlinemessage', VisitID: encodeURIComponent("<%=VisitID %>"), Meesage_Type: encodeURIComponent(rdmessageType), Meesage_Contents: tamessagecontents, Meesage_Name: txtmessagename, Meesage_Email: txtmessageemail, Meesage_Phone: txtmessagephone, r: Math.random() }, function (data) {
            if (data == "success") {
                alert("提问成功！");
                $.closePopupLayer('AddOnlineMessageAjaxPopup');
            }
            else {
                alert(data);
            }
        });
    }
    //验证数据格式
    function judgeIsSuccess() {
        var msg = "";

        var Identity = $("#Identity").val();
        if ($.trim(Identity) == "") {
            msg = "请填写联系方式<br/>";
        }
        var Name = $("#Name").val();
        if (Name == "") {
            msg = "请填写留言人的姓名<br/>";
        }
        if (Name.length > 10) {
            msg = "留言人的姓名长度不能超过10个字符<br/>";
        }

        var Content = $("#Content").val();
        if (Content == "") {
            msg = "请输入留言内容<br/>";
        }
        if (Content.length > 1000) {
            msg = "留言内容长度不能超过1000个字符<br/>";
        }

        return msg;
    }
    //-----------------------------------   添加留言End-------------------------------------------------    


    function closeLayer() {
        $.closePopupLayer('AddOnlineMessageAjaxPopup');
    }
</script>
<div class="layer">
    <div class="close" onclick="closeLayer()">
        &nbsp;</div>
    <ul>
        <li>
            <div class="field_c">
                <input type="text" class="txt" id="Identity" placeholder="手机/邮箱/用户名">
            </div>
        </li>
        <li>
            <div class="field_c">
                <input class="txt" id="Name" placeholder="姓名">
            </div>
        </li>
        <li>
            <div class="field_c">
                <textarea name=" " class="txt" id="Content" placeholder="咨询内容"></textarea>
            </div>
        </li>
    </ul>
    <div class="btn_box">
        <a href="javascript:void(0)" class="btn" onclick="saveOnLineMessage()">提交</a></div>
</div>
