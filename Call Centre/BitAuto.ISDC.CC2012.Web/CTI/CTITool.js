//检测是否是IE6，是-则关闭页面
$(function () {
    if ($.browser.msie && $.browser.version == 6) {
        alert("请升级IE6版本！");
        try {
            ADTTool.MethodScript('colsewindow');
        } catch (e) {
            window.opener = null; window.open('', '_self'); window.close();
        }
        return false;
    }
    $(document).bind("contextmenu", function (e) {
        return false;
    });
});

///需引用jquery.js
var callRecordID = "";
var callType = 1;
var taskTypeID = "";
var isDisconnected = 0;
//延迟时间，以毫秒为单位
var delayTime = 3000;
var telIsAvailable = false; //标记拨打电话是否能通话，true-能；false-不能 add lxw 11.22
var callidtonodisturb = ""; //免打扰使用

document.onkeydown = function () {
    if ($.browser.msie) {
        if (event.keyCode == 116) { event.keyCode = 0; event.cancelBubble = true; return false; }
    }
}

//响应事件
function MethodScript(message) {
    if (ADTTool) {
        ADTTool.adtHandler(ADTTool.unserialise(message));
    }
}
//电话控件
var ADTTool = window.ADTTool = (function () {
    //参数
    var params_url = '',
    //打开新页面
    newPage = function (url) {
        try {
            var flag = ADTTool.MethodScript('/browsercontrol/newpage?url=' + url);
            if (flag == 'failed')
            { return -1; }
        }
        catch (e) {
            return -1;
        }
    },

    //废弃 强斐 2016-8-18
    callOut = function (TargetDN, ShowANI, clickObj, outnum) {//外呼
        if (typeof $("#hidCallPhone").val() != "undefined") {
            $("#hidCallPhone").val(TargetDN);
        }
        if (TargetDN) {
            if (typeof (clickObj) == 'string')
                clickObj = $('#' + clickObj);
            if (ADTTool.beforeCallOut) {
                //外呼之前事件
                ADTTool.beforeCallOut(clickObj.attr('MemberCode'), clickObj.attr('ContactInfoUserID'), clickObj.attr('cTel'), clickObj.attr('cName'), clickObj.attr('cSex'));
            }
            TargetDN = fullChar2halfChar(TargetDN);
            ShowANI = ShowANI || '';
            try {
                ADTTool.MethodScript('/CallControl/MakeCall?targetdn=' + TargetDN + '&OutShowTag=' + outnum);
                isTelAvailable = true;
            }
            catch (e) {
                alert('通话功能不可用！');
            }
        }
        else { alert('未初始化参数'); }
    },
    
    //废弃 强斐 2016-8-18
    openCallOutPopup = function (clickObj, phoneNumText, ShowANI, outNum) {//若有多个呼出号码，则弹出层         
        if (window.ADTToolPopupContent) {
            window.ADTToolPopupContent.remove();
            $(document).unbind('mousemove', f);
            window.ADTToolPopupContent = null;
        }

        phoneNumText = $.trim(phoneNumText.replace(/\-/g, ""));
        if (phoneNumText.length == 0) { alert('电话号码为空'); return; }

        phoneNumText = $.trim(phoneNumText.replace(/\$/g, ',')); //把$符号替换为逗号

        if (phoneNumText.indexOf(',') < 0) {
            callOut(phoneNumText, ShowANI, $(clickObj), outNum);
        }
        else {
            var co = $(clickObj); if (co.length == 0) { return; }
            var content = $('<div class="open_tell" stype="position:absolute;"></div>');
            var ul = $('<ul class="list"></ul>');
            var array = phoneNumText.split(',');
            var i;
            for (i = 0; i < array.length; i++) {
                var v = array[i];
                var ulObj = $('<li>').append($('<span>').html(v));
                var aObj = $('<a>').addClass('tell').attr('id', 'calltell' + getRandomStr(20) + v)
                .attr('ctel', v);
                aObj.attr('href', 'javascript:ADTTool.callOut("' + v + '", "","' + aObj.attr('id') + '","' + outNum + '");');

                if ($(clickObj).attr('ContactInfoUserID') != undefined)
                    aObj.attr('ContactInfoUserID', $(clickObj).attr('ContactInfoUserID'));
                else if ($(clickObj).attr('MemberCode') != undefined)
                    aObj.attr('MemberCode', $(clickObj).attr('MemberCode'));
                ul.append(ulObj.append(aObj));
            }
            content.append(ul).append('<em></em>').appendTo($('body'));
            height = 25 * (i + 1) + 29;
            top = (co.offset().top - height) + 100;
            left = co.offset().left - 21;
            content.css({ left: left, top: top });
            window.ADTToolPopupContent = content;
            $(document).bind('mousemove', f);
        }
    },

    top = 0, left = 0, height = 0,
    f = function (e) {
        if (e.pageX < left - 20 || e.pageX > left + 180 || e.pageY < top - 20 || e.pageY > top + height + 20) {
            $(document).unbind('mousemove', f);
            window.ADTToolPopupContent.remove();
        }
    },

    //url参数解析
    unserialise = function (Data) {
        params_url = "";
        Data = Data.substr(Data.indexOf('?') + 1);
        Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
            params_url += "'" + Properties[0] + "':'" + Properties[1] + "',";
        });
        if (params_url.length > 1)
            params_url = params_url.substr(0, params_url.length - 1);
        params_url = "{" + params_url + "}";
        return Serialised;
    };

    //事件入口
    adtHandler = function (data) {
        //-----------数据解析说明------------------------
        //UserEvent: data.UserEvent,
        //UserName: data.UserName,
        //CalledNum: data.CalledNum, //对方
        //CallerNum: data.CallerNum,  //本机
        //CallID: data.CallID,
        //UserChoice: data.UserChoice,
        //RecordID: data.RecordID,
        //RecordIDURL: data.RecordIDURL,
        //AgentState: data.AgentState,
        //AgentAuxState: data.AgentAuxState,
        //MediaType: data.MediaType,
        //CallType: data.CallType
        //-----------数据解析说明------------------------
        callidtonodisturb = data.CallID;
        //接通
        if (data.UserEvent == 'Established') {
            ADTTool.connectOPRequesting = true;
            //alert(data.CallType);
            //如果是呼入
            if (data.CallType == "1" || data.CallType == "33" || data.CallType == "34") {
                data.CallType = "1";
                if (ADTTool.EstablishedForCallComing) {
                    //【=========================================呼入接通事件，需外层页面实现=========================================】
                    ADTTool.EstablishedForCallComing(data);
                }
            }
            //如果是呼出
            else if (data.CallType == "2") {
                //************************处理号码*************************//
                var pos = data.CallerNum.indexOf("0");
                var len = data.CallerNum.length;
                //手机号判断规则：大于等于11位，倒数第11位是1，倒数第10位不是0
                if (len >= 11 && data.CallerNum.charAt(len - 11) == '1' && data.CallerNum.charAt(len - 11 + 1) != '0') {
                    data.CallerNum = data.CallerNum.substr(len - 11, 11);
                    //alert("手机号：" + data.CallerNum);
                }
                //区号座机判断规则：存在0，且第一个0之后（含0）的长度大于等于10
                else if (pos >= 0 && len - pos >= 10) {
                    data.CallerNum = data.CallerNum.substr(pos);
                    //alert("区号座机：" + data.CallerNum);
                }
                //非区号座机：不是手机号，不带区号的情况下，大于8位
                else if (len > 8) {
                    data.CallerNum = data.CallerNum.substr(len - 8, 8);
                    //alert("座机：" + data.CallerNum);
                }
                else {
                    //alert("无处理：" + data.CallerNum);
                }
                if (data.CallerNum.substr(0, 2) == "00") {
                    //00开头 去掉一个0
                    data.CallerNum = data.CallerNum.substr(1);
                }
                //************************处理号码*************************//
                if (ADTTool.Established) {
                    //【=========================================呼出接通事件，需外层页面实现=========================================】
                    ADTTool.Established(data);
                }
            }
            ADTTool.connectOPRequesting = false;
        }
        //挂断
        else if (data.UserEvent == 'Released' ||
                   data.UserEvent == 'CA_CALL_EVENT_OP_DISCONNECT' || data.UserEvent == 'CA_CALL_EVENT_THIRD_PARTY_DISCONNECT' || data.UserEvent == 'CA_CALL_EVENT_FOURTH_PARTY_DISCONNECT') {
            ADTTool.disconnectRequesting = true;
            if (ADTTool.onDisconnected) {
                //【=========================================挂断事件，需外层页面实现=========================================】
                ADTTool.onDisconnected(data);
            }
            ADTTool.disconnectRequesting = false;
        }
        //呼入振铃
        else if (data.UserEvent == 'Transferred') {
            //无实现
        }
        //呼出初始化
        else if (data.UserEvent == 'Initiated') {
            if (ADTTool.onInitiated) {
                //【=========================================呼出初始化事件，需外层页面实现=========================================】
                ADTTool.onInitiated(data);
            }
        }
        //呼出到达事件
        else if (data.UserEvent == 'NetworkReached') {
            if (ADTTool.onNetworkReached) {
                //【=========================================呼出到达事件事件，需外层页面实现=========================================】
                ADTTool.onNetworkReached(data);
            }
        }
        //自动外呼
        else if (data.UserEvent == "AutoCall") {
            var url = "";
            switch (data.TaskType) {
                case "1":
                    url = "/OtherTask/OtherTaskDeal.aspx?OtherTaskID=" + data.TaskID;
                    break;
            }
            if (url != "") {
                url += "&isautocall=true&r=" + Math.random() + "&autocalldata=" + escape(params_url);
                ADTTool.MethodScript('/browsercontrol/newpageinbound?url=' + escape(url));
            }
        }
        //通知事件
        else if (data.UserEvent == "Notice") {
            if (data.Action == "SaveWindowsName") {
                window.name = data.Data;
            }
            else if (data.Action == "SendMsgToWindows") {
                if (ADTTool.SendMsgToWindows && typeof ADTTool.SendMsgToWindows == "function") {
                    ADTTool.SendMsgToWindows(unescape(data.Data));
                }
            }
        }
    }

    var getDate = function () {
        var s, d = new Date();
        s += d.getYear() + "-";
        s += (d.getMonth() + 1) + "-";
        s += d.getDate() + " ";
        s += d.getHours() + ':';
        s += d.getMinutes() + ':';
        s += d.getSeconds();
        return s;
    };

    return {
        callOut: callOut,
        openCallOutPopup: openCallOutPopup,
        unserialise: unserialise,
        adtHandler: adtHandler,
        f: f,
        newPage: newPage
    }
})();
//自动外呼初始话，自动触发3个事件
ADTTool.AutoCallInit = function (data) {
    if (typeof $("#hidCallPhone").val() != "undefined") {
        $("#hidCallPhone").val(data.CallerNum);
    }
    if (ADTTool.onInitiated) {
        ADTTool.onInitiated(data); //外呼初始化
    }
    if (ADTTool.onNetworkReached) {
        ADTTool.onNetworkReached(data); //外呼到达
    }
    if (ADTTool.Established) {
        ADTTool.Established(data); //外呼接通
    }
}
//通知浏览器
ADTTool.MethodScript = function (msg) {
    var pos = msg.indexOf('?');
    if (pos > 0) {
        msg += "&WebPageID=" + window.name;
    }
    else {
        msg += "?WebPageID=" + window.name;
    }
    return window.external.MethodScript(msg);
}
//播放录音（弹出层）
ADTTool.PlayRecord = function (playurl, pageurl) {

    if (!pageurl) {
        pageurl = "/CTI/PlayRecord.aspx";
    }
    if (playurl) {
        $.openPopupLayer({
            name: 'PlayRecordLayer',
            url: pageurl,
            parameters: { 'RecordURL': playurl },
            popupMethod: 'Post'
        });
    }
}
//计时
ADTTool.LogonTime = function (type, msg) {
    ADTTool.MethodScript('/time/' + type + "?msg=" + encodeURIComponent(msg));
}
//全角转半角
function fullChar2halfChar(str) {
    var result = '';
    for (i = 0; i < str.length; i++) {
        code = str.charCodeAt(i);             //获取当前字符的unicode编码
        if (code >= 65281 && code <= 65373)   //unicode编码范围是所有的英文字母以及各种字符
        {
            result += String.fromCharCode(str.charCodeAt(i) - 65248);    //把全角字符的unicode编码转换为对应半角字符的unicode码
        }
        else if (code == 12288)                                      //空格
        {
            result += String.fromCharCode(str.charCodeAt(i) - 12288 + 32); //半角空格
        } else {
            result += str.charAt(i);                                     //原字符返回
        }
    }
    return result;
}

//关闭页面
function closePage() {
    try {
        ADTTool.MethodScript('/browsercontrol/closepage');
    } catch (e) {
        window.opener = null; window.open('', '_self'); window.close();
    }
}
//关闭页面，并刷新上一个页面
function closePageReloadOpener() {
    try {
        ADTTool.MethodScript('/browsercontrol/closepagereloadppage');
    }
    catch (e) {
        try {
            window.opener.document.location.reload();
            window.close();
        }
        catch (ex) {
            window.opener = null; window.open('', '_self'); window.close();
        }
    }
}
//关闭页面，并刷新上一个页面
function closePageExecOpenerSearch(controlid) {
    var id = "btnSearch";
    if (controlid != undefined) {
        id = controlid;
    }
    try {
        ADTTool.MethodScript('/browsercontrol/closepagecallparentpagefun?parentpagecontrolid=' + id);
    }
    catch (e) {
        try {
            window.opener.document.getElementById(id).click();
            window.opener = null; window.open('', '_self'); window.close();
        }
        catch (ex) {
            try {
                window.opener.document.location.reload();
                window.close();
            }
            catch (ex2) {
                window.opener = null; window.open('', '_self'); window.close();
            }
        }
    }
}
//当前页面刷新Opener页面。注：对用Window.external.MethodScript(url)方式打开的页面无效
function execOpenerSearch(controlid) {
    var id = "btnSearch";
    if (controlid != undefined) {
        id = controlid;
    }
    try {
        window.opener.document.getElementById(id).click();
    }
    catch (ex) {
        window.opener = null;
    }
}
//随机字符串
function getRandomStr(len) {
    var x = "123456789poiuytrewqasdfghjklmnbvcxzQWERTYUIPLKJHGFDSAZXCVBNM";
    var str = "";
    for (var i = 0; i < len; i++) {
        str += x.charAt(Math.ceil(Math.random() * 100000000) % x.length);
    }
    return str;
}

/********************************  免打扰插件  **************************************/
var NoDisturbTool = {
    top: 0,
    left: 0,
    height: 0,
    //top = 0,left = 0, height = 0;
    //打开增加/修改免打扰信息的层（层逻辑中会进行判断：如果已经是免打扰号码，则显示修改；如果不是免打扰号码，则显示新增）
    openNoDisturbInfoLayer: function (clickObj, phoneNum, callID) {
        AjaxPostAsync("/BlackWhite/BlackWhiteHandler.ashx", { Action: "checkphoneNumisnodisturb", PhoneNum: phoneNum }, null, function (returnValue) {
            /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
            switch (returnValue) {
                case "0":
                    $.jAlert("当前号码已被设置为免打扰，请勿重复添加！");
                    break;
                case "1":
                    // $.jAlert("当前号码已被设置为免打扰，且有效期已失效,点击“确定”按钮，更新号码信息");
                    $.jConfirm("当前号码已被设置为免打扰，且有效期已失效,点击“确定”按钮，更新号码信息？", function (r) {
                        if (r) {
                            $.openPopupLayer({
                                name: "UpdateBlackDataAjaxPopup",
                                parameters: { CallId: callID, PhoneNumber: phoneNum, ResponseFrom: "plugin", r: Math.random() },
                                url: "/BlackWhite/NoDisturbLayer.aspx",
                                beforeClose: function (e, data) {
                                    if (e) {
                                        $(clickObj).attr("disabled", true);
                                    }
                                }
                            });
                        }
                    });
                    break;
                default:
                    $.openPopupLayer({
                        name: "UpdateBlackDataAjaxPopup",
                        parameters: { CallId: callID, PhoneNumber: phoneNum, ResponseFrom: "plugin", r: Math.random() },
                        url: "/BlackWhite/NoDisturbLayer.aspx",
                        beforeClose: function (e, data) {
                            if (e) {
                                $(clickObj).attr("disabled", true);
                            }
                        }
                    });
                    break;
            }
        });
    },
    //打开免打扰电话选择层（若有多个号码，则弹出层  ）
    openNoDisturbPopup: function (clickObj, phoneNumsText, callID) {
        if ($(clickObj).attr("src").indexOf("nodisturbgray.png") > 0) {
            alert("此状态不允许添加免打扰信息");
            return false;
        }
        callID = callidtonodisturb;

        //判断层是否存在，存在则移除
        if (window.NoDisturbPopupContent) {
            window.NoDisturbPopupContent.remove();
            $(document).unbind('mousemove', this.f);
            window.NoDisturbPopupContent = null;
        }
        //去掉电话号码中的短线
        phoneNumsText = $.trim(phoneNumsText.replace(/\-/g, ""));
        if (phoneNumsText.length == 0) {
            alert('电话号码为空'); return;
        }
        //把$符号替换为逗号
        phoneNumsText = $.trim(phoneNumsText.replace(/\$/g, ','));
        //如果只有一个电话号码，则直接打开免打扰信息的层
        if (phoneNumsText.indexOf(',') < 0) {
            this.openNoDisturbInfoLayer(clickObj, phoneNumsText, callID);
        }
        else {
            var co = $(clickObj);
            if (co.length == 0) {
                return;
            }
            var content = $('<div class="open_nodisturbtell" stype="position:absolute;"></div>');
            var ul = $('<ul class="list"></ul>');
            var array = phoneNumsText.split(',');
            var i;
            for (i = 0; i < array.length; i++) {
                var v = array[i];
                var ulObj = $('<li>').append($('<span>').html(v));
                var aObj = $('<a>').addClass('tell').attr('id', 'nodisturbtell' + getRandomStr(20) + v).attr('ctel', v);
                aObj.attr('href', 'javascript:NoDisturbTool.openNoDisturbInfoLayer($(".open_nodisturbtell"),"' + v + '","' + callID + '");');
                ul.append(ulObj.append(aObj));
            }
            content = content.append(ul).append('<em></em>');
            content.appendTo($('body'));
            this.height = 25 * (i + 1) + 29;
            this.top = (co.offset().top - this.height) + 100;
            this.left = co.offset().left - 21;
            $(".open_nodisturbtell").css({ left: this.left, top: this.top });
            window.NoDisturbPopupContent = content;
            $(".open_nodisturbtell").bind('mouseleave', this.f);
        }
    },
    f: function (e) {
        window.NoDisturbPopupContent.remove();
    }
};
/********************************  免打扰插件  **************************************/

/*************************************************  废弃  *******************************************************/
//插入录音 废弃
function InsertCallRecord(data, callerNum, sessionId, url, timespan, establishBeginTime, establishEndTime) {
   
}
//响应客户消息 废弃
function Response2CC(message) {
    if (message == "CMDTransfer") {
        //保存客户信息
        if ($("#hdAddCustBaseInfo").val() != undefined) {
            if (SubmitData(false)) {
                $.jAlert("保存成功！");
            }
        }
    }
}
//发送邮件 废弃
function SendEmailForRecordError(message, url, line) {
    var errorMsg = errorMsg += "执行" + url + "文件中的第" + line + "行代码出错，错误信息：" + message;
    window.onerror = null;
    $.post("/AjaxServers/ErrorHandler.ashx", { Action: "SendEmailForRecordError", ErrorMsg: errorMsg }, function () {

    });
    return true;
}
//插入日志 废弃
function InsertCallRecordEventLog(eventName, sessionId, logMsg) {
}
/*************************************************  废弃  *******************************************************/

 