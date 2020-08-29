
//****************************************************  经销商查询控件  ****************************************************
//此控件至少需要初始化四个参数：thisTxtDistributorId（经销商名称显示文本框id）、
//                              thisBtnDistributorId(经销商查询按钮id)
//
//可选初始化参数：thisBtnYaChaoId(亚超登录按钮id)、
//                thisEnableInput (经销商名称显示文本框是否可以手动输入)
//                thisCustId（客户id，用以查询指定客户下的经销商）
//返回的其他信息放在thisTxtDistributorId的三个属性中（attrMemberCode， attrCustName， attrCustID）
//**************************************************************************************************************************
DealerSearchControl = (function () {
    var jsonParams = { "thisTxtDistributorId": "",
        "thisBtnDistributorId": "",
        "thisBtnYaChaoId": "",
        "thisEnableInput": true,
        "thisCustId": "",
        "thisMemberName": "",
        "thisMemberCode": ""
    };
    var InitialEventNoParams = function () {  //按钮绑定事件
        if (CheckParams()) {
            var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
            DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode', DealerSearchControl.JsonParams.thisMemberCode);
            $("#" + DealerSearchControl.JsonParams.thisBtnDistributorId).click(BtnDistributorClick);
            $("#" + DealerSearchControl.JsonParams.thisBtnYaChaoId).click(BtnYaChaoClick);
            //输入框是否可以手动输入
            if (!DealerSearchControl.JsonParams.thisEnableInput) {
                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('disabled', true);
                DealerSearchControl.JsonParams.thisEnableInput = "0";
            }
            else {
                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('disabled', false);
                DealerSearchControl.JsonParams.thisEnableInput = "1";
            }
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).bind('blur', function () {
                ClearAttrValue(this);
            });
        }
    };
    var InitialEvent = function (txtDistributorId, btnDistributorId, btnYaChaoId, enableInput, custId) {  //按钮绑定事件
        var argLength = arguments.length;
        if (argLength != 5) {
            $.jAlert("初始化参数个数不对");
        }
        else if (typeof (enableInput) != "boolean") {
            $.jAlert("enableInput参数类型因为boolean");
        }
        else {
            //公共变量赋值
            DealerSearchControl.JsonParams.thisTxtDistributorId = txtDistributorId;
            DealerSearchControl.JsonParams.thisBtnDistributorId = btnDistributorId;
            if (CheckParams()) {
                DealerSearchControl.JsonParams.thisBtnYaChaoId = btnYaChaoId;
                DealerSearchControl.JsonParams.thisCustId = custId;
                var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
                DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
                //按钮绑定事件
                $("#" + btnDistributorId).click(BtnDistributorClick);
                if (btnYaChaoId != "") {
                    $("#" + btnYaChaoId).click(BtnYaChaoClick);
                }
                //输入框是否可以手动输入
                if (!enableInput) {
                    $("#" + txtDistributorId).attr('disabled', true);
                    DealerSearchControl.JsonParams.thisEnableInput = "0";
                }
                else {
                    $("#" + txtDistributorId).attr('disabled', false);
                    DealerSearchControl.JsonParams.thisEnableInput = "1";
                }

                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).bind('blur', function () {
                    ClearAttrValue(this);
                });
            }
        }
    };

    var ClearAttrValue = function (obj) {
        var new_value = $.trim($(obj).val());
        var old_value = $.trim($(obj).attr("attrMemberName"));
        if (new_value == old_value) {
            //无操作            
        }
        else {
            //清空四个属性
            $(obj).attr('attrMemberCode', "");
            $(obj).attr('attrMemberName', "");
            $(obj).attr('attrCustName', "");
            $(obj).attr('attrCustID', "");

        }
        //去掉空格
        $(obj).val(new_value);
        return;
    };
    var SetMemberCode = function (memberCode) {  //设置MemberCode属性
        DealerSearchControl.JsonParams.thisMemberCode = memberCode;
        SetDealerInfoByMemberCode();
    };
    var SetMemberName = function (memberName) {  //设置MemberName属性
        DealerSearchControl.JsonParams.thisMemberName = memberName;
        if (DealerSearchControl.JsonParams.thisTxtDistributorId != "") {
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val(memberName);
        }
    };
    var SetDealerInfoByMemberCode = function () {
        if (DealerSearchControl.JsonParams.thisTxtDistributorId != "") {
            AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx", {
                Action: "GetDealerInfo",
                MemberCode: DealerSearchControl.JsonParams.thisMemberCode,
                r: Math.random()
            }, null, function (returnValue) {
                if (returnValue) {
                    var backData = $.evalJSON(returnValue);
                    if (backData.result) {
                        var txtDistributorObj = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId);
                        txtDistributorObj.val(backData.data.name);
                        txtDistributorObj.attr('attrCustName', backData.data.CustName);
                        txtDistributorObj.attr('attrCustID', backData.data.CustID);
                        txtDistributorObj.attr('attrMemberCode', DealerSearchControl.JsonParams.thisMemberCode);
                        txtDistributorObj.attr('attrMemberName', backData.data.name);
                        DealerSearchControl.JsonParams.thisMemberName = backData.data.name;
                    }
                    else {
                        $.jAlert(backData.message);
                    }
                }
            });
        }
    };
    var CheckParams = function () {
        if (DealerSearchControl.JsonParams.thisTxtDistributorId == "" ||
            DealerSearchControl.JsonParams.thisBtnDistributorId == ""
           ) {
            $.jAlert("DealerSearchControl必要的参数未初始化");
            return false;
        }
        else {
            return true;
        }
    };
    var BtnDistributorClick = function () { //经销商查询按钮事件 
        var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
        DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
        OpenPopupAndBind('/WOrderV2/PopLayer/DealerSelectLayer.aspx', 'DealerSelectPopup');
    };
    var BtnYaChaoClick = function () {  //亚超登录按钮点击事件
        var attrMemberCode = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode');
        if (typeof (attrMemberCode) == "undefined") {  //没有选择过经销商
            $.jAlert("请选择一个经销商");
        }
        else if (attrMemberCode == "") {  //选择了经销商，但没有经销商id值
            if ($.trim(attrMemberCode) == "")  //没有经销商名称
            {
                $.jAlert("所属经销商不能为空");
            }
            else {
                $.jAlert("该经销商不是易湃会员，无法登录");
            }
        }
        else {
            try {
                var pagehost = window.location.host;
                window.external.MethodScript('/browsercontrol/newpage?url=' + escape('/AjaxServers/RYP.aspx?tid=' + attrMemberCode));
            }
            catch (e) {
                window.open("/AjaxServers/RYP.aspx?tid=" + attrMemberCode);
            }
        }
    };
    var OpenPopupAndBind = function (url, PopupName) {//打开弹出层并绑定数据（通用)
        $.openPopupLayer({
            name: PopupName,
            parameters: { CustId: DealerSearchControl.JsonParams.thisCustId,
                MemberName: DealerSearchControl.JsonParams.thisMemberName,
                MemberCode: $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode')
            },
            url: url + "?r=" + Math.random(),
            popupMethod: "POST",
            beforeClose: function (e, data) {
                if (e) {
                    BindHTMLbyOjb(data);
                }
            }
        });
    };
    var ClosePopupAndBind = function () {//关闭对话框时回传的数据的处理
        $.closePopupLayer('DealerSelectPopup', true);
    };
    var BindHTMLbyOjb = function (data) {
        if (data != "") {
            var txtDistributorObj = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId);
            txtDistributorObj.val(data.MemberName);
            txtDistributorObj.bindTextDefaultMsg("所属经销商");
            txtDistributorObj.attr('attrMemberCode', data.MemberCode);
            txtDistributorObj.attr('attrMemberName', data.MemberName);
            txtDistributorObj.attr('attrCustName', data.CustName);
            txtDistributorObj.attr('attrCustID', data.CustID);
        }
    };

    //获取经销商id
    var GetCRMMemberCode = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrMemberCode"));
    };
    //获取经销商名称
    var GetCRMMemberName = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).val());
    };
    //获取客户id
    var GetCRMCustID = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrCustID"));
    };
    //获取客户名称
    var GetCRMCustName = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrCustName"));
    };
    return {
        InitialEvent: InitialEvent,
        InitialEventNoParams: InitialEventNoParams,
        //设置初始数据
        SetMemberCode: SetMemberCode,
        SetMemberName: SetMemberName,
        //获取数据接口
        GetCRMMemberCode: GetCRMMemberCode,
        GetCRMMemberName: GetCRMMemberName,
        GetCRMCustID: GetCRMCustID,
        GetCRMCustName: GetCRMCustName,
        JsonParams: jsonParams
    }
})();

//****************************************************  发送短信控件  ****************************************************
//此控件至少需要初始化四个参数：thisPageType（1：发送经销商信息（不需要显示短信模板）；2：其他（需要选择短信模板））、
//                              thisTxtUserPhoneId(获取电话号码的文本框ID)、
//                              thisBtnSendMessageId(触发发送短信事件的控件ID)、
//                              thisBackValueSaveControlId或thisBackValueSaveToBtnAttr (返回的短信记录id存放的位置：控件id或触发发送短信事件的控件的属性的名称)
//************************************************************************************************************************
SendMessage = (function () {
    var jsonParams = { "thisPageType": "",
        "thisTxtUserPhoneId": "",
        "thisBtnSendMessageId": "",
        "thisBackValueSaveControlId": "",
        "thisBackValueSaveToBtnAttr": ""
    },
    InitialEventNoParams = function () {  //按钮绑定事件
        if (CheckParams()) {
            $("#" + SendMessage.JsonParams.thisBtnSendMessageId).click(BtnSendMessageClick);
        }
    },
    InitialEvent = function (pageType, txtUserPhoneId, btnSendMessageId, backValueSaveControlId, backValueSaveToBtnAttr) {  //按钮绑定事件
        //公共变量赋值
        SendMessage.JsonParams.thisPageType = pageType;
        SendMessage.JsonParams.thisTxtUserPhoneId = txtUserPhoneId;
        SendMessage.JsonParams.thisBtnSendMessageId = btnSendMessageId;
        SendMessage.JsonParams.thisBackValueSaveControlId = backValueSaveControlId;
        SendMessage.JsonParams.thisBackValueSaveToBtnAttr = backValueSaveToBtnAttr;
        if (CheckParams()) {
            $("#" + SendMessage.JsonParams.thisBtnSendMessageId).click(BtnSendMessageClick);
        }
    },
    CheckParams = function () {
        if (SendMessage.JsonParams.thisPageType == "" ||
            SendMessage.JsonParams.thisTxtUserPhoneId == "" ||
            SendMessage.JsonParams.thisBtnSendMessageId == "" ||
            (SendMessage.JsonParams.thisBackValueSaveControlId == "" && SendMessage.JsonParams.thisBackValueSaveToBtnAttr == "")
           ) {
            $.jAlert("SendMessage必要的参数未初始化");
            return false;
        }
        else {
            return true;
        }
    },
    SendMessageCompleteEvent = null,
    BtnSendMessageClick = function () {
        $.openPopupLayer({
            name: "SendSMSPopup",
            parameters: { PageType: escape(SendMessage.JsonParams.thisPageType), Phone: $.trim($("#" + SendMessage.JsonParams.thisTxtUserPhoneId).val()) },
            url: "/WOrderV2/PopLayer/SendMessageLayer.aspx",
            beforeClose: function (e, data) {
                if (e) {
                    //data.RecID 是否返回值
                    if (SendMessageCompleteEvent && typeof SendMessageCompleteEvent == "function") {
                        SendMessageCompleteEvent(data.RecID);
                    }
                    if (SendMessage.JsonParams.thisBackValueSaveControlId != "") {
                        $("#" + SendMessage.JsonParams.thisBackValueSaveControlId).val(data.RecID);
                    }
                    if (SendMessage.JsonParams.thisBackValueSaveToBtnAttr_attrRecId != "") {
                        $("#" + SendMessage.JsonParams.thisBtnSendMessageId).attr(SendMessage.JsonParams.thisBackValueSaveToBtnAttr, data.RecID);
                    }
                    //console.log(data.RecID); //返回插入发送邮件记录表（SMSSendHistory）的数据的RecID
                } else {
                    if (SendMessage.JsonParams.thisBackValueSaveControlId != "") {
                        $("#" + SendMessage.JsonParams.thisBackValueSaveControlId).val("");
                    }
                    if (SendMessage.JsonParams.thisBackValueSaveToBtnAttr_attrRecId != "") {
                        $("#" + SendMessage.JsonParams.thisBtnSendMessageId).attr(SendMessage.JsonParams.thisBackValueSaveToBtnAttr, "");
                    }
                    //console.log("没有返回信息");
                }
            }
        });
    };
    return {
        InitialEvent: InitialEvent,
        InitialEventNoParams: InitialEventNoParams,
        JsonParams: jsonParams
    }
})();

//****************************************************  免打扰控件  ****************************************************
//此控件至少需要初始化四个参数：thisTxtUserPhoneId（电话号码所在的控件id）、
//                              thissBtnOpenLayerId(触发打开免打扰层事件的控件ID)、
//                              thisCallId(打完电话的CallID)
//************************************************************************************************************************
NoDisturbLayerControl = (function () {
    var jsonParams = { "thisTxtUserPhoneId": "",
        "thissBtnOpenLayerId": "",
        "thisCallId": ""
    },
    InitialEventNoParams = function () {  //按钮绑定事件 
        CheckParamsAndBindEvent();
    },
    InitialEvent = function (txtUserPhoneId, btnOpenLayerId, callId) {  //按钮绑定事件
        NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId = txtUserPhoneId;
        NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId = btnOpenLayerId;
        NoDisturbLayerControl.JsonParams.thisCallId = callId;
        CheckParamsAndBindEvent();
    },
    CheckParamsAndBindEvent = function () {  //检验必填项
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId == "" ||
            NoDisturbLayerControl.JsonParams.thisCallId == "" ||
            NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId == ""
           ) {
            $.jAlert("NoDisturbLayerControl必要的参数未初始化");
        }
        else {
            $("#" + NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId).click(BtnOpenLayerClick);
        }
    },
    BtnOpenLayerClick = function () {
        var phone = $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val());
        if (phone == "") {
            $.jAlert("电话号码为空！", function () { $("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).focus(); });
            return;
        }
        else if (!isTelOrMobile(phone)) {
            $.jAlert("电话号码格式不正确！", function () { $("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).focus(); });
            return;
        }
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId != "") {
            AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx", { Action: "phonenumisnodisturb", Phone: $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val()) }, null, function (returnValue) {
                /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
                switch (returnValue) {
                    case "0":
                        $.jAlert("当前号码已被设置为免打扰，请勿重复添加！");
                        break;
                    case "1":
                        $.jConfirm("当前号码已被设置为免打扰，且有效期已失效,点击“确定”按钮，更新号码信息？", function (r) {
                            if (r) {
                                OpenPopupAndBind();
                            }
                        });
                        break;
                    default:
                        OpenPopupAndBind();
                        break;
                }
            });
        }
    },
    OpenPopupAndBind = function () {
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId != "") {
            $.openPopupLayer({
                name: "UpdateBlackDataAjaxPopup",
                parameters: { CallId: NoDisturbLayerControl.JsonParams.thisCallId,
                    PhoneNumber: $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val()),
                    ResponseFrom: "plugin",
                    r: Math.random()
                },
                url: "/WOrderV2/PopLayer/NoDisturbLayer.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        if (NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId != "") {
                            $("#" + NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId).attr("disabled", true);
                        }
                    }
                }
            });
        }
        else {
            $.jAlert("NoDisturbLayerControl必要的参数thisTxtUserPhoneId未初始化");
        }
    };
    return {
        InitialEvent: InitialEvent,
        InitialEventNoParams: InitialEventNoParams,
        JsonParams: jsonParams
    }
})();

//**************************************************** 接收/抄送人控件  //****************************************************
//此控件需要初始化1个参数：thisTxtReceiverCopyUserId（选择接收/抄送人的textbox的Id） 
//返回信息存放在Id为thisTxtReceiverCopyUserId的textbox的userids属性中
//****************************************************************************************************************************
ReceiverCopyUserSelectControl = function () {
    var jsonParams = { "thisTxtReceiverCopyUserId": "" },
    InitialEventNoParams = function () {  //按钮绑定事件 
        CheckParamsAndBindEvent();
    },
    InitialEvent = function (txtReceiverCopyUserId) {  //按钮绑定事件
        jsonParams.thisTxtReceiverCopyUserId = txtReceiverCopyUserId;
        CheckParamsAndBindEvent();
    },
    CheckParamsAndBindEvent = function () {  //检验必填项
        if (jsonParams.thisTxtReceiverCopyUserId == "") {
            $.jAlert("ReceiverCopyUserSelectControl必要的参数未初始化");
        }
        else {
            $("#" + jsonParams.thisTxtReceiverCopyUserId).click(BtnOpenLayerClick);
        }
    },
    BtnOpenLayerClick = function () {
        if (jsonParams.thisTxtReceiverCopyUserId != "") {
            var thisObj = $("#" + jsonParams.thisTxtReceiverCopyUserId);
            var userids = "";
            var attruserids = thisObj.attr("userids")
            if (typeof (attruserids) == "undefined") {  //判断属性是否存在
                thisObj.attr("userids", "");  //新增属性
            }
            $.openPopupLayer({
                name: "SelReceiveCopyUserAjaxPopup",
                parameters: { UserIDs: userids },
                url: "/WOrderV2/PopLayer/ReceiveCopyLayer.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        if (data != "") {
                            thisObj.attr("userids", data.Ids);
                            thisObj.val(data.Names);
                        }
                    }
                }
            });
        }
        else {
            $.jAlert("ReceiverCopyUserSelectControl必要的参数thisTxtReceiverCopyUserId未初始化");
        }
    };
    return {
        InitialEvent: InitialEvent,
        //InitialEventNoParams: InitialEventNoParams,
        JsonParams: jsonParams
    }
};

   

 