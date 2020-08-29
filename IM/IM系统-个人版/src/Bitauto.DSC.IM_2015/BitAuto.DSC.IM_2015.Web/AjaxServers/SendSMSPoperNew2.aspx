<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSMSPoperNew2.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.SendSMSPoperNew2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    var MaxCount = 700;
    $(document).ready(function () {
        getUserGroup();

        AddContext();
        GetNum();
        $("#txtSendContent").keyup(GetNum);

        var tels = '<%=Tel%>'.split(',');
        $("#txtTelPop1").val("");
        $("#txtTelPop2").val("");
        for (var i = 0; i < tels.length; i++) {
            //   alert(i + ",  " + tels[i])
            $("#txtTelPop" + (i + 1)).val(tels[i]);
        }
    });

    //加载登陆人业务组
    function getUserGroup() {
        AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
            $("#<%=selGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
            var selval;
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                selval = jsonData[0].BGID;
                for (var i = 0; i < jsonData.length; i++) {
                    $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }

                $("#<%=selGroup.ClientID %>").val(selval);
                selGroupChange();
                selCategoryChange();
            }
        });
    }


    //根据选择的分组绑定对应的分类
    function selGroupChange() {
        $("#<%=selCategory.ClientID %>").children().remove();
        $("#<%=selCategory.ClientID %>").append("<option value='-1'>请选择分类</option>");
        if ($("#<%=selGroup.ClientID %>").val() != "-1") {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selGroup.ClientID %>").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#<%=selCategory.ClientID %>").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }

    //根据选择的分类绑定对应的短信模板
    function selCategoryChange() {
        $("#selSMSTemplate").children().remove();
        $("#selSMSTemplate").append("<option value='-1'>请选择模板</option>");
        if ($("#<%=selCategory.ClientID %>").val() != "-1") {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSMSTemplate", SCID: $("#<%=selCategory.ClientID %>").val(), r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selSMSTemplate").append("<option value='" + jsonData[i].RecID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }

    function SetSendContent() {
        if ($("#selSMSTemplate").val() != "-1") {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "getsmstemplatebyrecid", TemplateID: $("#selSMSTemplate").val(), r: Math.random() }, null, function (data) {
                if (data != "") {
                    $("#txtSendContent").val(data);
                    GetNum();
                }
                else {
                    $.jAlert("模板内容为空!");
                }
            });
        }
    }

    //根据传过来的值填写内容
    function AddContext() {
        var pageType = '<%=PageType %>';
        if (pageType == 1) {
            //CRM客户联系人调用
            //$("input[name='telPop']").attr('disabled', 'true');
            $("input[name='telPop']").val('<%=Tel %>');
        }
        else if (pageType == 2) {
            //其他任务，电话手动输入
        }

    }

    function SendSMS() {
        var crmCustID = '<%=CRMCustID %>';
        var custID = '<%=CustID %>';
        var taskType = '<%=TaskType %>';
        var taskID = '<%=TaskID %>';
        var templateID = $("#selSMSTemplate").val();


        var msg = "";
        var tels = $("input[name='telPop']").map(function () {
            var thisval = $.trim($(this).val())
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
            msg += "电话不能为空 \n</br>";
        }

        if (tels.charAt(tels.length - 1) == ',') {
            tels = tels.substring(0, tels.length - 1);
        }

        if (Len(content) == 0) {
            msg += "请输入发送内容 \n</br>";
        }
        else if (Len(content) > MaxCount) {
            msg += "短信内容不能超过" + parseInt(num / 2) + "个字 \n</br>";
        }
        if (Len(msg) == 0) {
            $('#hdsubhave').val("1");
            $('#smms').Mask();
            $.post("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "sendsms2", CRMCustID: crmCustID, CustID: custID, TaskType: taskType, TaskID: taskID, TemplateID: templateID, Tels: tels, SendContent: encodeURIComponent(content) }, function (data) {
                $('#smms').UnMask();
                $('#hdsubhave').val("");
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "true") {
                    $.jAlert("短信发送成功！");
                    $.closePopupLayer('SendSMSPopup22', true, { Tels: tels })
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
<div class="popup openwindow" id="smms">
    <div class="title ft14">
        <h2>
            发送短信</h2>
        <span><a href="#" class="right" onclick="javascript:$.closePopupLayer('SendSMSPopup22',false,{Tels:''});">
            <img src="../images/c_btn.png" border="0" /></a></span></div>
    <div class="content">
        <ul>
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w125"
                    style="border: 1px solid #ccc; width: 100px; height: 25px;">
                </select>
                <select id="selCategory" onchange="javascript:selCategoryChange()" runat="server"
                    class="w125" style="border: 1px solid #ccc; width: 100px; height: 25px;">
                </select>
                <select id="selSMSTemplate" onchange="javascript:SetSendContent()" class="w125" style="border: 1px solid #ccc;
                    width: 100px; height: 25px;">
                </select>
            </li>
            <li>
                <label>
                    发送号码：</label>
                <input name="telPop" id="txtTelPop1" type="text" class="w125" style="border: 1px solid #ccc;
                    height: 25px;  font-size:14px" />
                <a href="javascript:void(0)" onclick="RemoveTextControlEvent(this)" name="removeTel">
                    <img width="12" height="10" src="../../images/collapsed_no.gif" />
                </a>
                <input name="telPop" id="txtTelPop2" type="text" class="w125" style="border: 1px solid #ccc;
                    height: 25px;font-size:14px" />
                <a href="javascript:void(0)" onclick="RemoveTextControlEvent(this)" name="removeTel">
                    <img width="12" height="10" src="../../images/collapsed_no.gif" />
                </a></li>
            <li>
                <label>
                    发送内容：</label><textarea onkeypress="GetNum()" id="txtSendContent" rows="7" cols="10"
                        style="border: 1px solid #ccc; width: 350px;"></textarea>
                <span id="spanNum" style="clear: both; float: left; font-size:12px; color: Red; padding-left:120px;"></span>
            </li>
        </ul>
        <div class="btn">
            <input type="button" onclick="SendSMS()" type="button" value="发 送" id="smssendbt"
                class="save w60" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" value="关闭" onclick="javascript:$.closePopupLayer('SendSMSPopup22',false,{Tels:''});"
                    class="cancel w60 gray" /></div>
    </div>
