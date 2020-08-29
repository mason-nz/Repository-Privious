<%@ Page Title="客户留言" Language="C#" AutoEventWireup="true" CodeBehind="OnLineMessageForm.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.OnLineMessageForm" %>

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
            $.jAlert(msg);
            return false;
        }

        var rdmessageType = 0;
        $("input[name$='rdmessageType']").each(function () {
            if ($(this).attr("checked")) {
                rdmessageType = $(this).val();
            }
        });
        var tamessagecontents = encodeURIComponent($("#tamessagecontents").val());
        var txtmessagename = encodeURIComponent($("#txtmessagename").val());
        var txtmessagephone = encodeURIComponent($("#txtmessagephone").val());
        var txtmessageemail = encodeURIComponent($("#txtmessageemail").val());

        $.post("/AjaxServers/LayerDataHandler.ashx", { Action: 'addonlinemessage', VisitID: encodeURIComponent("<%=VisitID %>"), Meesage_Type: encodeURIComponent(rdmessageType), Meesage_Contents: tamessagecontents, Meesage_Name: txtmessagename, Meesage_Email: txtmessageemail, Meesage_Phone: txtmessagephone, r: Math.random() }, function (data) {
            if (data == "success") {
                $.jAlert("提问成功！");
                $.closePopupLayer('AddOnlineMessageAjaxPopup');
            }
            else {
                $.jAlert(data);
            }
        });
    }
    //验证数据格式
    function judgeIsSuccess() {
        var msg = "";
        var rdmessageType = 0;
        $("input[name$='rdmessageType']").each(function () {
            if ($(this).attr("checked")) {
                rdmessageType = $(this).val();
            }
        });
        if (rdmessageType == 0) {
            msg = "请选择留言类型<br/>";
        }
        var tamessagecontents = $("#tamessagecontents").val();
        if (tamessagecontents == "") {
            msg = "请输入留言内容<br/>";
        }
        if (tamessagecontents.length > 1000) {
            msg = "留言内容长度不能超过1000个字符<br/>";
        }
        var txtmessagename = $("#txtmessagename").val();
        if (txtmessagename == "") {
            msg = "请填写留言人的姓名<br/>";
        }
        if (txtmessagename.length > 10) {
            msg = "留言人的姓名长度不能超过10个字符<br/>";
        }
        var txtmessagephone = $("#txtmessagephone").val();
        if (txtmessagephone == "") {
            msg = "请填写留言人的手机号<br/>";
        }
        else if (!isTelOrMobile(txtmessagephone)) {
            msg += "手机号码 " + txtmessagephone + " 格式不正确<br/>";
        }

        var txtmessageemail = $("#txtmessageemail").val();

        if ($.trim(txtmessageemail) != "") {
            if (!isEmail($.trim(txtmessageemail))) {
                msg = "邮箱格式不正确<br/>";
            }
        }
        return msg;
    }
    //-----------------------------------   添加留言End-------------------------------------------------    
</script>
<script type="text/javascript">
    function closeLayer() {
        $.closePopupLayer('AddOnlineMessageAjaxPopup');
    }
</script>
<div class="online_kf online_kf2" style="background-color: #E4E4E4;">
    <div class="title_kf">
        在线留言<span><a href="#" onclick="closeLayer()"><img src="/Images/c_btn.png" border="0"
            alt="关闭" /></a></span></div>
    <div class="content_kf content_kf_ms">
        <!--左开始-->
        <div class="left_c">
            <div class="answer answer2">
                <p class="hs">
                    请填写您的信息及咨询问题，我们会尽快给您回复</p>
                <table border="0" cellspacing="0" cellpadding="0" class="msg_lv" width="100%">
                    <tr>
                        <th width="10%">
                            <span class="red">*</span>类型：
                        </th>
                        <td width="80%">
                            <label>
                                <input name="rdmessageType" type="radio" value="1" /><span>购车咨询</span></label>
                            <label>
                                <input name="rdmessageType" type="radio" value="2" /><span>卖车咨询</span></label>
                            <label>
                                <input name="rdmessageType" type="radio" value="3" /><span>活动咨询</span></label>
                            <label>
                                <input name="rdmessageType" type="radio" value="4" /><span>网站建设</span></label>
                            <label>
                                <input name="rdmessageType" type="radio" value="5" /><span>其它</span></label>
                        </td>
                    </tr>
                    <tr>
                        <th style="vertical-align: top;">
                            <span class="red">*</span>内容：
                        </th>
                        <td>
                            <textarea name="" id="tamessagecontents" cols="" rows=""></textarea>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            邮箱：
                        </th>
                        <td>
                            <input type="text" value="" class="w180" id="txtmessageemail" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <span class="red">*</span>姓名：
                        </th>
                        <td>
                            <input type="text" value="" id="txtmessagename" class="w180" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <span class="red">*</span>手机：
                        </th>
                        <td>
                            <input type="text" value="" id="txtmessagephone" class="w180" />
                        </td>
                    </tr>
                </table>
                <div class="btn submit">
                    <input type="button" value="提交" onclick="saveOnLineMessage()" class="w80" />
                </div>
            </div>
        </div>
        <!--左结束-->
    </div>
    <div class="clearfix">
    </div>
</div>
