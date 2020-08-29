<%@ Page Title="发送短信" Language="C#" AutoEventWireup="true" CodeBehind="SendMessageLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.SendMessageLayer" %>

<script type="text/javascript">
    var MaxCount = 700;
    $(document).ready(function () {
        if ("<%=JsonData.SMSData.PageType_Out %>" == "2") {
            GetUserGroup();
        }
        else {
            $("#smsLiNotDealer").css("display", "none");
        }
        AddContext();
        $("#smsAreaContent").keyup(GetNum);
    });
    //加载登陆人业务组
    function GetUserGroup() {
        AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx",
        {
            Action: "GetUserGroupByLoginUserID",
            ShowSelfGroup: "true",
            r: Math.random()
        }, null, function (data) {
            $("#smsSelGroup").append("<option value='-1'>请选所属分组</option>");
            var selval;
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                selval = jsonData[0].BGID;
                for (var i = 0; i < jsonData.length; i++) {
                    $("#smsSelGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }

                $("#smsSelGroup").val(selval);
                SelGroupChange();
                SelCategoryChange();
            }
        });
    }
    //根据传过来的值填写内容
    function AddContext() {
        //电话号码 
        var phonenum = $.trim('<%=JsonData.SMSData.Phone_Out %>');
        if (phonenum == "") {
            $.jAlert("手机号码不能为空!");
            return;
        }
        else {
            $("#smsTextPhoneNum").attr('disabled', 'disabled');
            $("#smsTextPhoneNum").val(phonenum);
        }

        //短信内容
        if ("<%=JsonData.SMSData.PageType_Out%>" == "1") {
            var dmsName = "<%=JsonData.SMSData.MemberName_Out%>";
            var dMSAddress = "<%=JsonData.SMSData.MemberAddress_Out%>";
            var dMSTel = "<%=JsonData.SMSData.MemberTel_Out%>";
            var custName = '<%=JsonData.PageData.CBName_Out %>';
            var custSex = "<%=JsonData.PageData.CBSex_Out%>";
            var str = "";
            if (dmsName != undefined && dMSAddress != undefined && dMSTel != undefined) {
                if (custName != "" && custSex != "") {
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
                    str += "地址:" + dMSAddress + "，";
                }
                if (dMSTel != '') {
                    str += "电话:" + dMSTel + "，";
                }
                if (str != "") {
                    str += "如果您需要其他经销商联系方式，可以拨打易车购车咨询电话：4000-168-168（工作日 9:00-21:00）感谢您对易车网的关注！";
                }
                $("#smsAreaContent").val(str);
                GetNum();
            }
        }
    }
    //根据选择的分组绑定对应的分类
    function SelGroupChange() {
        $("#smsSelCategory").children().remove();
        $("#smsSelCategory").append("<option value='-1'>请选择分类</option>");
        if ($("#smsSelGroup").val() != "-1") {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#smsSelGroup").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#smsSelCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }
    //根据选择的分类绑定对应的短信模板
    function SelCategoryChange() {
        $("#smsSelSMSTemplate").children().remove();
        $("#smsSelSMSTemplate").append("<option value='-1'>请选择模板</option>");
        if ($("#smsSelCategory").val() != "-1") {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSMSTemplate", SCID: $("#smsSelCategory").val(), r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#smsSelSMSTemplate").append("<option value='" + jsonData[i].RecID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }
    //绑定短信模板信息
    function SetSendContent() {
        if ($("#smsSelSMSTemplate").val() != "-1") {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "getsmstemplatebyrecid", TemplateID: $("#smsSelSMSTemplate").val(), r: Math.random() }, null, function (data) {
                if (data != "") {
                    $("#smsAreaContent").val(data);
                    GetNum();
                }
                else {
                    $.jAlert("模板内容为空!");
                }
            });
        }
    }
    //计算可输入字数
    function GetNum() {
        var num = "";
        var msg = "";
        num = MaxCount - Len($("#smsAreaContent").val());
        num = parseInt(num / 2);
        if (num > 0) {
            msg = "还可输入" + num + "个字";
        }
        else if (num < 0) {
            msg = "已超出" + Math.abs(num) + "个字";
        }
        $("#pWarmingMsg").text(msg);
    }
    //发送短信
    function SendSMSForCC() {
        //选择的模板
        var templateID = $("#smsSelSMSTemplate").val();
        //电话号码
        var phone = $("#smsTextPhoneNum").val();
        //发送内容
        var content = $.trim($("#smsAreaContent").val());
        //校验
        if ($.trim(phone) == "") {
            $.jAlert("用户号码不能为空！", function () { $("#smsTextPhoneNum").focus(); });
            return;
        }
        if (!isMobile(phone)) {
            $.jAlert("用户号码格式不正确！", function () { $("#smsTextPhoneNum").focus(); });
            return;
        }
        if (Len(content) == 0) {
            $.jAlert("请填写短信内容！", function () { $("#smsAreaContent").focus(); });
            return;
        }
        else if (Len(content) > MaxCount) {
            $.jAlert("短信内容不能超过" + parseInt(Len(content) / 2) + "个字！", function () { $("#smsAreaContent").focus(); });
            return;
        }
        //url对象
        var jsondata = $.evalJSON('<%=JsonStr %>');
        jsondata.SMSData.TemplateID = templateID;
        jsondata.SMSData.Content = content;
        jsondata.Phone = phone;
        //发送短信
        AjaxPost("/AjaxServers/CommonCallHandler.ashx",
        {
            Action: "SMSSaveEvent",
            JsonData: escape(JSON.stringify(jsondata)),
            R: Math.random()
        },
        null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result) {
                $.jPopMsgLayer("短信发送成功！", function () { $.closePopupLayer('SendSMSPopup', true, { RecID: $.trim(jsonData.data) }); });
            }
            else {
                $.jAlert("短信发送失败：" + jsonData.message + "");
            }
        });
    }
</script>
<!--发送短信-->
<div class="pop_new w600 openwindow">
    <div class="title">
        <h2 style="cursor: auto;">
            发送短信</h2>
        <span><a hidefocus="true" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SendSMSPopup',false);">
        </a></span>
    </div>
    <div class="set_edit">
        <ul>
            <li style="width: 550px;" id="smsLiNotDealer">
                <label>
                    短信模板：</label>
                <span>
                    <select id="smsSelGroup" onchange="javascript:SelGroupChange();" class="w136" style="width: 133px;">
                    </select>
                    <select id="smsSelCategory" onchange="javascript:SelCategoryChange();" class="w136"
                        style="width: 133px;">
                    </select>
                    <select id="smsSelSMSTemplate" onchange="javascript:SetSendContent();" class="w136"
                        style="width: 133px;">
                    </select>
                </span></li>
            <li style="width: 550px;">
                <label style="*padding-top: 3px;">
                    <span class="redcolor">*</span>用户号码：</label>
                <span>
                    <input style="padding-bottom: 5px; padding-top: 3px" id="smsTextPhoneNum" name=""
                        type="text" class="w300 w200" /></span></li>
            <li style="width: 550px;">
                <label>
                    <span class="redcolor">*</span>发送内容：</label>
                <span>
                    <textarea id="smsAreaContent" name="" cols="" rows="" style="height: 80px;"></textarea>
                    <p id="pWarmingMsg">
                        还可以输入350字</p>
                </span></li>
        </ul>
    </div>
    <div class="clearfix">
    </div>
    <div class="option_button btn">
        <input name="" type="button" value="发送" onclick="SendSMSForCC();" />
        <input name="" type="button" value="取消" onclick="javascript:$.closePopupLayer('SendSMSPopup',false);" />
    </div>
    <div class="clearfix">
    </div>
</div>
