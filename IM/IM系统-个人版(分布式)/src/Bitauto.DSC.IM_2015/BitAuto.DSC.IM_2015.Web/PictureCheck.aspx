<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureCheck.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.PictureCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    $(document).ready(function () {
        GetImage();
    });
    function GetImage() {
        var guidstr = '<%=GuidStr%>';
        var pody = { action: 'getyanzhengimage', WYGUID: escape(guidstr) };
        AjaxPostAsync(ONS.HdlUrl, pody, null,
                    function (msg) {
                        document.getElementById("im_yanzheng").src = msg;
                    });
    }
    function SubAnswer() {
        $("#sperror").hide();
        var guidstr = '<%=GuidStr%>';
        var danan = $.trim($("#txtYanZheng").val());
        if (danan == "") {

            $("#sperror").show();
            $("#txtYanZheng").focus().select();
        }
        else if (!isNum(danan)) {

            $("#sperror").show();
            $("#txtYanZheng").focus().select();
        }
        else if (Len(danan) > 2) {

            $("#sperror").show();
            $("#txtYanZheng").focus().select();
        }
        else {
            var pody = { action: 'yanzhengdanan', WYGUID: escape(guidstr), DaAn: escape(danan) };
            AjaxPostAsync(ONS.HdlUrl, pody, null,
                    function (msg) {
                        if (msg == "yes") {
                            ischeck = "1";
                            $.closePopupLayer('TuPianYanZhengPopup');
                        }
                        else {
                            document.getElementById("im_yanzheng").src = msg;
                            $("#sperror").show();
                            $("#txtYanZheng").focus().select();
                        }
                    });
        }

    }
    function subdaan(e) {
        if (e.which)
            keyCode = e.which;
        else if (e.keyCode)
            keyCode = e.keyCode;
        if (keyCode == 13) {
            SubAnswer();
            return false;
        }
    }
</script>
<div class="online_kf online_kf2" style="background-color: #fff; width: 364px; height: 210px;
    margin: 0; padding-bottom: 0px;">
    <div class="title_kf">
    </div>
    <div class="content_kf content_kf_ms" style="height: 80%">
        <!--左开始-->
        <div class="left_c">
            <div class="answer answer2" style="padding-left: 15px; margin-top: -25px; border: 0px;
                padding: 0px;">
                <table border="0" cellspacing="0" cellpadding="0" class="msg_lv" style="padding-top: 5px;"
                    width="100%">
                    <tr>
                        <td style="padding-bottom: 5px; padding-top: 5px;">
                            <img src="Images/u321.png" style="width: 20px; height: 20px" />
                            请填写图片中问题的正确答案，开始在线聊天
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 0px;">
                            <img id="im_yanzheng" />
                            <a href="#" onclick="GetImage()" style="font-size: 12px">看不清楚？换张图片</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 50px; padding-top: 4px; padding-bottom: 0px;">
                            答案
                            <input type="text" id="txtYanZheng" class="w180" onkeypress="return subdaan(event)" /><br />
                            <span id="sperror" style="color: Red; display: none; padding-left: 32px; font-size:12px">您的答案不正确，请重新输入</span>
                        </td>
                    </tr>
                </table>
                <div class="btn submit" style="margin-top: 13px; margin-bottom: 10px">
                    <input type="button" value="提交" onclick="SubAnswer()" class="save w60" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                    <input type="button" value="关闭" onclick="javascript:SetBeforeunload(false, onbeforeunload_handler);
                    window.opener = null; window.open('', '_self'); window.close();" class="cancel w60 gray" />
                </div>
            </div>
        </div>
        <!--左结束-->
    </div>
    <div class="clearfix">
    </div>
</div>
