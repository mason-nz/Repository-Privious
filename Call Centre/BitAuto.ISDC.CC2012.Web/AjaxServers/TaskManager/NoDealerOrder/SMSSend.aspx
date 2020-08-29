<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSend.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder.SMSSend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    var MaxCount = 700;

    $(document).ready(function () {

        $("#txtTelPop1").val($("input[id$='hidTel1']").val())
        $("#txtTelPop2").val($("input[id$='hidTel2']").val())

        AddContext();
        GetNum();
        $("#txtSendContent").keyup(GetNum);

    });

    //根据传过来的值填写内容
    function AddContext() {

        var dmsName = $("input[id$='hidDMSName']").val();
        var dMSAddress = $("input[id$='hidDMSAddress']").val();
        var dMSTel = $("input[id$='hidDMSTel']").val();
        var dMSCity = $("input[id$='hidDMSCity']").val();
        var dMSLevel = $("input[id$='hidDMSLevel']").val();
        var custName = $("input[id$='hidCustName']").val();
        var custSex = $("input[id$='hidCustSex']").val();

        var str = "";
        if (dmsName != "" && dMSAddress != "") {
            switch (custSex) {
                case "1": str += custName + "先生您好，";
                    break;
                case "2": str += custName + "女士您好，";
                    break;
            }
        }
        if (dmsName != "") {
            str += "已帮您查询" + dmsName + "经销商，";
        }
        if (dMSAddress != "") {
            str += "地址:" + dMSCity + dMSAddress + "，";
        }
        //        if (dMSLevel != "") {
        //            if (dMSLevel == "1") {
        //                str += "经销商类型:4S";
        //            }
        //            else if (dMSLevel == "0") {
        //                str += "经销商类型:综合";
        //            }
        //        }
        if (dMSTel != "") {
            str += "电话:" + dMSTel + "，";
        }

        if (str != "") {
            str += "如果您需要其他经销商联系方式，可以拨打易车购车咨询电话：4000-168-168（工作日 9:00-21:00）感谢您对易车网的关注！";
        }
        $("#txtSendContent").val(str)
    }

    function RemoveTextControlEvent(obj) {
        if ($("input[name='telPop']").size() > 1) {
            $(obj).prev().remove();
            $(obj).remove();
            if ($("input[name='telPop']").size() == 1) {
                $("a[name='removeTel']").remove();
            }
        }
        else {
            $(obj).remove();
        }
    }

    function SendSMS() {
        var tels = $("input[name='telPop']").map(function () {
            if ($.trim($(this).val()) != "") {
                return $(this).val();
            }
        }).get().join(',');

        var custId = $.trim($("#hdnCustID").val());
        var content = $.trim($("#txtSendContent").val());
        var msg = "";

        if (Len(tels) == 0) {
            msg += "电话不能为空 \n";
        }

        if (Len(content) == 0) {
            msg += "请输入发送内容 \n";
        }
        else if (Len(content) > MaxCount) {
            msg += "短信内容不能超过" + parseInt(num / 2) + "个字 \n";
        }
        var body = {
            Tels: tels,
            SendContent: escape(content)

        };
        if (Len(msg) == 0) {
            $.post("/AjaxServers/TaskManager/NoDealerOrder/SMSSend.ashx", body, function (data) {
                if (data == "success") {
                    $.jPopMsgLayer("短信发送成功！");
                    $.closePopupLayer('SendSMSPopup', false);
                }
                else {
                    $.jAlert("发送失败【" + data + "】");
                }
            });
        }
        else {
            $.jAlert(msg);
        }
    }


    function GetNum() {

        var num = "";
        var msg = "";
        num = MaxCount - Len($("#txtSendContent").val());
        num = parseInt(num / 2);
        if (num > 0) {
            msg = "还可输入" + num + "个字";
        }
        else {
            msg = "已超出" + parseInt(MaxCount / 2) + "个字";
        }
        $("#spanNum").text(msg);
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            发送短信</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SendSMSPopup',false);"
            class="right"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>
                发送号码<span class="redColor">*</span>:</label><input name="telPop" id="txtTelPop1"
                    type="text" class="w125" /><a href="javascript:void(0)" onclick="RemoveTextControlEvent(this)"
                        name="removeTel"><img width="12" height="10" src="../../images/collapsed_no.gif" /></a>
            <input name="telPop" id="txtTelPop2" type="text" class="w125" /><a href="javascript:void(0)"
                onclick="RemoveTextControlEvent(this)" name="removeTel"><img width="12" height="10"
                    src="../../images/collapsed_no.gif" /></a></li>
        <li>
            <label>
                发送内容<span class="redColor">*</span>:</label><textarea id="txtSendContent" rows="5"
                    cols="10"></textarea>
        </li>
        <li style="height: 20px; line-height: 20px;">
            <label>
                &nbsp;</label>
            <span class="redColor" id="spanNum"></span></li>
    </ul>
    <div class="btn">
        <input name="" onclick="SendSMS()" type="button" value="发 送" class="btnSave bold" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('SendSMSPopup',false);"
            class="btnCannel bold" /></div>
    <input type="hidden" id="hidTel1" value='<%=Tel1 %>' />
    <input type="hidden" id="hidTel2" value='<%=Tel2 %>' />
    <input type="hidden" id="hidDMSCode" value='<%=DMSCode %>' />
    <input type="hidden" id="hidDMSName" value='<%=DMSName %>' />
    <input type="hidden" id="hidDMSAddress" value='<%=DMSAddress %>' />
    <input type="hidden" id="hidDMSTel" value='<%=DMSTel %>' />
    <input type="hidden" id="hidDMSLevel" value='<%=DMSLevel %>' />
    <input type="hidden" id="hidDMSCity" value='<%=DMSCity %>' />
    <input type="hidden" id="hidCustName" value='<%=CustName %>' />
    <input type="hidden" id="hidCustSex" value='<%=CustSex %>' />
</div>
