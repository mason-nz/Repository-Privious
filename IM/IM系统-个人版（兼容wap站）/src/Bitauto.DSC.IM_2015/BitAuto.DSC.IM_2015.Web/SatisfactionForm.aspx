<%@ Page Title="客户满意度" Language="C#" AutoEventWireup="true" CodeBehind="SatisfactionForm.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.SatisfactionForm" %>

<link href="css/css.css" rel="stylesheet" type="text/css" />
<link href="css/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">

    function saveSatisfaction() {
        var rdpersatisfaction = 0;
        $("input[name$='rdpersatisfaction']").each(function () {
            if ($(this).attr("checked")) {
                rdpersatisfaction = $(this).val();
            }
        });
        var rdprosatisfaction = 0;
        $("input[name$='rdprosatisfaction']").each(function () {
            if ($(this).attr("checked")) {
                rdprosatisfaction = $(this).val();
            }
        });
        var satisfactiondetail = $("#trsatisfactiondetail").val();

        var msg = "";
        if (rdpersatisfaction == 0) {
            msg += "请选择对客服人员服务的满意度<br/>";
        }
        if (rdprosatisfaction == 0) {
            msg += "请选择对易车产品的满意度<br/>";
        }
        if (satisfactiondetail.length > 500) {
            msg += "填写内容不能超过500字<br/>";
        }
        if (msg != "") {
            $.jAlert(msg, function () {
                return false;
            });
        }
        else {

            $.post("/AjaxServers/LayerDataHandlerBefore.ashx", { Action: 'addsatisfaction', CSID: encodeURIComponent('<%=CSID%>'), PerSatisfaction: encodeURIComponent(rdpersatisfaction), ProSatisfaction: encodeURIComponent(rdprosatisfaction), DSatisfaction: encodeURIComponent(satisfactiondetail), r: Math.random() }, function (data) {
                if (data == "success") {
                    $.jAlert("提交成功！");
                    $.closePopupLayer('AddSatisfactionAjaxPopup', true);
                }
                else if (data == "您的评价我们已经记录，谢谢您对我们的支持！") {
                    $.jAlert(data);
                    $.closePopupLayer('AddSatisfactionAjaxPopup', true);
                }
                else {
                    $.jAlert(data);
                }
            });
        }
    }
    function closeLay() {
        $.closePopupLayer('AddSatisfactionAjaxPopup');
    }
</script>
<!--满意度-->
<div class="online_kf online_kf2" style="background-color:#fff; width: 600px;">
    <div class="title_kf">
        满意度<span><a href="#" onclick="javascript:$.closePopupLayer('AddSatisfactionAjaxPopup',false,{Tels:''});"><img
            src="/Images/c_btn.png" border="0" alt="关闭" /></a></span></div>
    <div class="content">
        <table cellspacing="0" cellpadding="0" class="myd_pop">
            <tr>
                <td colspan="5" style="font-size:14px; font-weight:bold; padding-top:20px;">
                    <span class="red">*</span>您对当前客服人员的服务是否满意？
                </td>
            </tr>
            <tr>
                <td width="20%">
                    <label>
                        <input name="rdpersatisfaction" type="radio" value="5" />
                        非常满意</label>
                </td>
                <td width="20%">
                    <label>
                        <input name="rdpersatisfaction" type="radio" value="4" />
                        满意</label>
                </td>
                <td width="20%">
                    <label>
                        <input name="rdpersatisfaction" type="radio" value="3" />
                        一般</label>
                </td>
                <td width="20%">
                    <label>
                        <input name="rdpersatisfaction" type="radio" value="2" />
                        不满意</label>
                </td>
                <td width="20%">
                    <label>
                        <input name="rdpersatisfaction" type="radio" value="1" />
                        非常不满意</label>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="font-size:14px; font-weight:bold; padding-top:20px;">
                    <span class="red">*</span>您对易车的产品是否满意？
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <input name="rdprosatisfaction" type="radio" value="5" />
                        非常满意</label>
                </td>
                <td>
                    <label>
                        <input name="rdprosatisfaction" type="radio" value="4" />
                        满意</label>
                </td>
                <td>
                    <label>
                        <input name="rdprosatisfaction" type="radio" value="3" />
                        一般</label>
                </td>
                <td>
                    <label>
                        <input name="rdprosatisfaction" type="radio" value="2" />
                        不满意</label>
                </td>
                <td>
                    <label>
                        <input name="rdprosatisfaction" type="radio" value="1" />
                        非常不满意</label>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <textarea id="trsatisfactiondetail" cols="" rows="" style=" width:547px;"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="5" style="text-align: right;">
                    <div class="btn" style="text-align: center; margin-top: 0;">
                        <input type="button" value="提交" onclick="saveSatisfaction()" class="save w60" /></div>
                </td>
            </tr>
        </table>
    </div>
</div>
<!--满意度-->
