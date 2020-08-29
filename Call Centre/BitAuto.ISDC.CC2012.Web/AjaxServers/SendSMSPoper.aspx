<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSMSPoper.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SendSMSPoper" %>

<!--旧版短信，废弃，强斐 2016-8-18-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    var MaxCount = 700;
    $(document).ready(function () {
        $("#txtTelPop1").val($("input[id$='txtTel1']").val())
        $("#txtTelPop2").val($("input[id$='txtTel2']").val())
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
        var custName = '<%=CustName %>';
        var custSex = $(":radio[name$='sex']:checked").val();
        var str = "";
        var TelFour = '<%=DMSTEL%>';
        if (dmsName != undefined && dMSAddress != undefined && TelFour != undefined) {
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
            if (TelFour == '') {
                if (dMSTel != "") {
                    str += "电话:" + dMSTel + "，";
                }
            }
            else {
                str += "电话:" + TelFour + "，";
            }
            if (str != "") {
                str += "如果您需要其他经销商联系方式，可以拨打易车购车咨询电话：4000-168-168（工作日 9:00-21:00）感谢您对易车网的关注！";
            }
            $("#txtSendContent").val(str);
        }
    }

    function SendSMSPoper_Send() {
        if ($('#hdsubhave').val() != "") {
            $.jAlert("正在发送请等待！");
        }
        else {
            var msg = "";
            var tels = $("input[name='telPop']").map(function () {
                var thisval = $(this).val()
                if (thisval != '' && !isMobile(thisval)) {
                    msg += "“" + thisval + "”这个电话号码格式不正确 \n</br>";
                }
                else {
                    return thisval;
                }
            }).get().join(',');
            var custId = $.trim($("#hdnCustID").val());
            var content = $.trim($("#txtSendContent").val());

            if ($.trim(tels) == ",") {
                msg += "电话不能为空 \n";
            }
            if (tels.charAt(tels.length - 1) == ',') {
                tels = tels.substring(0, tels.length - 1);
            }

            if (Len(custId) == 0) {
                custId = "新增工单";
            }
            if (Len(content) == 0) {
                msg += "请输入发送内容 \n";
            }
            else if (Len(content) > MaxCount) {
                msg += "短信内容不能超过" + parseInt(num / 2) + "个字 \n";
            }
            if (Len(msg) == 0) {
                $('#hdsubhave').val("1");
                $.post("../AjaxServers/CustBaseInfo/Handler.ashx",
                {
                    Action: "SendSMS",
                    CustID: custId,
                    Tels: tels,
                    SendContent: encodeURIComponent(content)
                }, function (data) {
                    $('#hdsubhave').val("");
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "true") {
                        $.jPopMsgLayer("短信发送成功！", function () {
                            $.closePopupLayer('SendSMSPopup', true, { Tels: tels })
                        });
                        if ($("#HiddenSMSSendHistoryRecID").length == 1) {
                            var old = $.trim($("#HiddenSMSSendHistoryRecID").val());
                            var recid = $.trim(jsonData.SMSSendHistoryRecID);
                            if (old == "") {
                                $("#HiddenSMSSendHistoryRecID").val(recid);
                            }
                            else {
                                $("#HiddenSMSSendHistoryRecID").val(old + "," + recid);
                            }
                        }
                    }
                    else {
                        $.jAlert("发送失败（" + jsonData.msg + "）");
                    }
                });
            }
            else {
                $.jAlert(msg);
            }
        }
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
        <input type="hidden" id="hdsubhave" value="" />
        <h2>
            发送短信</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SendSMSPopup',false,{Tels:''});"
            class="right"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <!--edit by wangtonghai 2016/4/26 for 经销商短信两个input电话disable-->
            <label>
                发送号码<span class="redColor">*</span>:</label><input name="telPop" id="txtTelPop1"
                    type="text" class="w125" disabled="disabled" /><a href="javascript:void(0)" onclick="RemoveTextControlEvent(this)"
                        name="removeTel"><img width="12" height="10" src="../../images/collapsed_no.gif" /></a>
            <input name="telPop" id="txtTelPop2" type="text" class="w125" disabled="disabled" /><a
                href="javascript:void(0)" onclick="RemoveTextControlEvent(this)" name="removeTel"><img
                    width="12" height="10" src="../../images/collapsed_no.gif" /></a></li>
        <li>
            <label>
                发送内容<span class="redColor">*</span>:</label><textarea id="txtSendContent" rows="5"
                    cols="10"></textarea></li>
        <li style="height: 20px; line-height: 20px;">
            <label>
                &nbsp;</label>
            <span class="redColor" id="spanNum"></span></li>
    </ul>
    <div class="btn">
        <input name="" onclick="SendSMSPoper_Send()" type="button" value="发 送" class="btnSave bold" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('SendSMSPopup',false,{Tels:''});"
            class="btnCannel bold" /></div>
</div>
