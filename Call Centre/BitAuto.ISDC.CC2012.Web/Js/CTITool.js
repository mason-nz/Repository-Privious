///需引用jquery.js

document.oncontextmenu = function () { self.event.returnValue = false; }; //屏蔽右键
document.onkeydown = function () {
    if (event.keyCode == 116) { event.keyCode = 0; event.cancelBubble = true; return false; }
}
//function document.onkeydown() {
//    if (event.keyCode == 116) { event.keyCode = 0; event.cancelBubble = true; return false; } 
//}     //屏蔽F5按键

var ADTTool = window.ADTTool = (function () {
    var businessNature = '', relationID = '', cRMCustID = '',

    //    init = function (bn, rID, ccID) {
    //        businessNature = bn || ''; //1:核对信息,2:回访
    //        relationID = rID || ''; //关联ID（任务ID或者回访ID）
    //        cRMCustID = ccID || ''; //CRM系统客户ID
    //    },

    newPage = function (url) {
        try {
            var flag = window.external.MethodScript('/browsercontrol/newpage?url=' + url);
            if (flag == 'failed')
            { return -1; }
        }
        catch (e) {
            return -1;
        }
    },

    callOut = function (TargetDN, ShowANI, clickObj) {//外呼
        //alert(businessNature + '|' + relationID + '|' + cRMCustID + "|" + TargetDN);
        //        if (businessNature == '' ||
        //            (businessNature == '1' && relationID == '') ||
        //            (businessNature == '2' && cRMCustID == ''))
        //        { alert('绑定录音参数无效，不能呼出电话'); return; }
        if (TargetDN) {
            if (ADTTool.beforeCallOut) {//外呼之前事件
                //ADTTool.beforeCallOut(clickObj.attr('MemberCode'), clickObj.attr('ContactInfoUserID')); 
                ADTTool.beforeCallOut();
            }
            TargetDN = fullChar2halfChar(TargetDN);
            ShowANI = ShowANI || '';
            try {
                //window.external.Method4Script('/CallControl/MakeCallEX?TargetDN=TEL:' + TargetDN + '&MakeCallType=2&ShowANI=' + ShowANI);
                //alert(TargetDN);
                window.external.MethodScript('/CallControl/MakeCall?targetdn=' + TargetDN)
            }
            catch (e) {
                if (ADTTool.onDisconnected) { ADTTool.onDisconnected(); }
                alert('通话功能不可用');
            }
        }
        else { alert('未初始化参数'); }
    },

    openCallOutPopup = function (clickObj, phoneNumText, ShowANI) {//若有多个呼出号码，则弹出层
        if (window.ADTToolPopupContent) {
            window.ADTToolPopupContent.remove();
            $(document).unbind('mousemove', f);
            window.ADTToolPopupContent = null;
        }

        phoneNumText = $.trim(phoneNumText.replace('-', ''));
        if (phoneNumText.length == 0) { alert('电话号码为空'); return; }
        phoneNumText = $.trim(phoneNumText.replace('$', ',')); //把$符号替换为逗号
        if (phoneNumText.indexOf(',') < 0) {
            callOut(phoneNumText, ShowANI, $(clickObj));
        }
        else {
            var co = $(clickObj); if (co.length == 0) { return; }

            var content = $('<div class="open_tell" stype="position:absolute;"></div>');
            //var content = $('<div class="open_tell" stype="position:absolute;"></div>');
            var ul = $('<ul class="list"></ul>');
            var array = phoneNumText.split(',');
            var i;
            for (i = 0; i < array.length; i++) {
                var v = array[i]; //alert(v);
                var ulObj = $('<li>').append($('<span>').html(v));
                var aObj = $('<a>').addClass('tell')
                                   .html('点击电话')
                                   .attr('href', 'javascript:ADTTool.callOut("' + v + '", "", this);');
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

    unserialise = function (Data) {
        Data = Data.substr(Data.indexOf('?') + 1);

        Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };

    var handlerURL = '/CTI/CTIHelper.ashx?callback=?',

    adtHandler = function (data) {//处理
        //alert(data.UserEvent);
        if (data.UserEvent == 'Established') {//呼出接通//alert(data.UserEvent + ' ' + data.SessionID + ' ' + data.DNIS);
            //alert('呼出接通' + businessNature + '|' + relationID + '|' + cRMCustID);
            ADTTool.connectOPRequesting = true;
            $.getJSON(handlerURL, {
                Action: 'Established',
                UserEvent: data.UserEvent,
                UserName: data.UserName,
                CalledNum: data.CalledNum, //对方
                CallerNum: data.CallerNum,  //本机
                CallID: data.CallID,
                UserChoice: data.UserChoice,
                RecordID: data.RecordID,
                RecordIDURL: data.RecordIDURL,
                AgentState: data.AgentState,
                AgentAuxState: data.AgentAuxState,
                MediaType: data.MediaType
            }, function (jd, textStatus, xhr) {
                ADTTool.connectOPRequesting = false;
                $.closePopupLayer('ADTToolConverPopup');
                alert(jd.message);
            });

            //ADTTool.CallStartTime = getDate();
            //ADTTool.CallEndTime = null;
        }
        else if (data.UserEvent == 'Released' ||
                 data.UserEvent == 'CA_CALL_EVENT_OP_DISCONNECT' ||
                 data.UserEvent == 'CA_CALL_EVENT_THIRD_PARTY_DISCONNECT' ||
                 data.UserEvent == 'CA_CALL_EVENT_FOURTH_PARTY_DISCONNECT') {//坐席挂断，客户挂断//alert(data.UserEvent + ' ' + data.SessionID);
            ADTTool.disconnectRequesting = true;
            if (ADTTool.onDisconnectRequesting) { ADTTool.onDisconnectRequesting(); }
            $.closePopupLayer('ADTToolConverPopup');
            $.getJSON(handlerURL, {
                Action: 'Released',
                UserEvent: data.UserEvent,
                UserName: data.UserName,
                CalledNum: data.CalledNum, //对方
                CallerNum: data.CallerNum,  //本机
                CallID: data.CallID,
                UserChoice: data.UserChoice,
                RecordID: data.RecordID,
                RecordIDURL: data.RecordIDURL,
                AgentState: data.AgentState,
                AgentAuxState: data.AgentAuxState,
                MediaType: data.MediaType
            }, function (jd, textStatus, xhr) {
                if (ADTTool.onDisconnected) { ADTTool.onDisconnected(); }
                ADTTool.disconnectRequesting = false;
                alert(jd.message);
            });

            //ADTTool.CallEndTime = getDate();
        }
        else if (data.UserEvent == 'Ringing' ||
                 data.UserEvent == 'Initiated' ||
                 data.UserEvent == 'Transferred') {//响铃
            //ADTTool.SessionID = data.SessionID;
            //alert(ADTTool.onAlerting);
            if (ADTTool.onAlerting) { ADTTool.onAlerting(data); } //显示遮盖层，不能操作页面数据

            //else { alert('没有弹屏'); }
        }
    };

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
        //init: init,
        callOut: callOut,
        openCallOutPopup: openCallOutPopup,
        unserialise: unserialise,
        adtHandler: adtHandler,
        f: f,
        newPage: newPage
    }
})();


//响应事件
function MethodScript(message) {
    if (ADTTool) {
        ADTTool.adtHandler(ADTTool.unserialise(message));
    }
}

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