
var ChatStuff = {
    checkStateDuration: 8000,
    TimeidleAgent: 10000,
    checkStateTimer: 0,
    AgentToken: '',
    AgentId: 0,
    HdlUrl: "post.action", //"AjaxServers/Handler.ashx"
    i: 0,
    UserId: 0,
    //    mc: "经销商code",
    lgid: "loginid",
    CsId: 0,
    mn: "网友名称",
    cst: "开始时间",
    //    adr: "地址",
    //    cgn: "城市群",
    //    lct: "最后消息时间",
    //    dst: "晨曦时间",
    ctn: "Contract", //网友姓名
    //    ctj: "联系人职务",
    newMsgDuration: 3000,
    newMsgTimer: 3000,   //计时表示
    //    uft: "跳转页面",
    ctp: "",
    vid: -1,   ///访问ID
    BQPop: null,
    isLeaveNotice: false, //记录是否已经弹出重新登录提示    
    tagId: 0,
    tagName: "",
    busiTypeId: 0
};
var defaultChannel = null;


ChatStuff.initEvent = function () {


    //初始化常用服务语
    $('#' + ChatStuff.ulCM + ' > li').click(function (eve) {
        var tTarget$ = $(eve.target);
        if (tTarget$.hasClass('item')) {
            //$('#DialogM' + ChatStuff.CsId).find('.ask_t').text(tTarget$.text());
            var tContent$ = $('#DialogM' + ChatStuff.CsId).find('.ask_t');
            if (tContent$.length > 0) {
                tContent$.html(tContent$.html() + ' ' + tTarget$.text());
                moveEnd(tContent$);
            }
            tTarget$.closest('ul').hide().siblings().hide();
            eve.stopPropagation();
            eve.preventDefault();
            return false;
        }
        var this$ = $(this);
        var cd$ = this$.children();
        if (cd$.is(':visible')) {
            cd$.hide();
            return false;
        }
        this$.siblings().children().hide();

        //        if (this$.prev().length > 0) {
        //            var topT = this$.position().top * -1 + 2;
        //            this$.find('ul').css('top', topT);
        //        }
        if (cd$.find('li').length > 0) {
            cd$.show();
        }

        return false;
    });

};

ChatStuff.UpdateConvsCountTimer = function () {
    setInterval(function () {
        var tabLeft$ = $('#tabLeft');
        $('#totalConvsNum').text(tabLeft$.find('div').length - 2);
        $('#totalUnReadConvsNum').text(tabLeft$.find('span.newbox').length);
    }, 1000);
};

ChatStuff.ChangeAgentState = function (txtState) {

    var target$ = $('#liState' + txtState);
    target$.hide().siblings().show();
    $('#StateTop').text($.trim(target$.text()));
    if (txtState == "2") {
        DisableAllWY();
    }
    return;
};

function StartTimer() {
    StopTimer();
    // ChatStuff.newMsgTimer = setInterval(CheckFlickMsg, ChatStuff.newMsgDuration);
    ChatStuff.checkStateTimer = setTimeout(CheckDialogState, ChatStuff.checkStateDuration);
}

function StopTimer() {
    //clearInterval(ChatStuff.newMsgTimer);
    clearTimeout(ChatStuff.checkStateTimer);
}


//开始长连接
function StartLongConnect() {
    if (defaultChannel == null) {
        defaultChannel = new AspNetComet("/poll", ChatStuff.AgentToken, "defaultChannel");

        defaultChannel.addTimeoutHandler(TimeoutHandler);
        defaultChannel.addFailureHandler(FailureHandler);
        defaultChannel.addSuccessHandler(SuccessHandler);
        //分配坐席
        defaultChannel.addAllocAgentForUserHandler(AllocAgentForUserHandler);
        //网友离开
        defaultChannel.addAgentLeaveHandler(AgentLeaveHandler);
        defaultChannel.addSatisfactionHandLer(SatisfactionHandLer);
        defaultChannel.addSendFileHandLer(SendFileHandLer);
        defaultChannel.addQueueSortHandLer(QueueSortHandLer);
        defaultChannel.addTransferHandLers(TransferAgentHandLers);
        //发送长连接请求
        defaultChannel.subscribe();
    }
}

function TransferAgentHandLers(privateToken, alias, message) {
    var userinfo = GetUserInfoFromMsg(message);
    AddNewChat(userinfo);
}

function SatisfactionHandLer(privateToken, alias, message) {
    var csidT = message.cs;
    $('#DialogL' + csidT).attr('isSatis', "1");
    var chatinfo = {
        uid: message.f, //网友ID
        //AgentID: username, //坐席ID
        cs: csidT,
        name: $('#DialogL' + csidT).attr('ctn'), //别名.text()
        rectime: ChatStuff.parseLongDate(message.t), //收到消息时间
        content: "网友已完成满意度评价." //消息内容
    };

    //显示聊天记录
    CreateMyChatNew(chatinfo);

}

function QueueSortHandLer(privateToken, alias, message) {
    alert(message);
}

function SendFileHandLer(privateToken, alias, message) {
    //SuccessHandler(privateToken, alias, message);

    var chatinfo = {
        uid: message.f, //网友ID,
        cs: message.cs, //网友ID,
        AgentID: '', //坐席ID
        name: $('#DialogL' + message.cs).attr('ctn'), //网友别名 .text()       
        sendtime: ChatStuff.parseLongDate(message.t),
        //rectime: getHoursMinute2(message.ct),
        content: message.m //消息内容
    };

    chatinfo.rectime = chatinfo.sendtime;
    NewMessageFlicker();
    //显示聊天记录
    CreateMyChatNew(chatinfo);
    UpdateUnReadCount(message.cs);
    //    NewMessageFlicker();
}

//分配坐席
function AllocAgentForUserHandler(privateToken, alias, message) {
    //给分配消息记录日志
    //    var parameters = {
    //        action: 'recordlog',
    //        FromPrivateToken: escape(privateToken),
    //        SendToPrivateToken: escape(message.uid),
    //        AllocID: escape(message.cs)
    //    };
    //    $.ajax({
    //        //async: false,
    //        type: "POST",
    //        url: ChatStuff.HdlUrl, // "AjaxServers/Handler.ashx",
    //        data: parameters,
    //        success: function (msg) { }
    //    });
    //
    //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮
    if (!isExistChatLayer(message.cs)) {
        var userinfo = GetUserInfoFromMsg(message);
        //OperateChatLayer(userinfo);
        AddNewChat(userinfo);
        NewMessageFlicker();
    }
}

function GetUserInfoFromMsg(message) {

    return {
        UserId: message.uid,
        cs: message.cs,

        ctp: message.ctp,
        vid: message.vid,
        //            mn: message.mn,
        cst: ChatStuff.parseLongDate(message.cst),
        //            ast: ChatStuff.parseLongDate(message.ast),
        //            adr: message.adr,
        //            cgn: message.cgn,
        //            lct: ChatStuff.parseLongDate(message.lct),
        //            dst: message.dst,
        ctn: message.ctn,
        //            ctj: message.ctj,
        lgid: message.lgid
        //            ,uft: message.uft,
        //            lmt: ChatStuff.parseLongDate(message.lmt)
    };
}



function SuccessHandler(privateToken, alias, message) {
    if (message.c == null || message.c.cs == null) {
        return false;
    }
    if (message.c.f == "System") {
        return false;
    }

    //如果网友已掉线，接受消息到消息时重新激活网友Tab
    if ($('#DialogL' + message.c.cs).hasClass('offline')) {
        ReSuriveWY(message.c.cs);
    }


    //网友ID
    var uid = message.c.f;
    if (!isExistChatLayer(message.c.cs) && uid != "System") {
        //网友不存在时重建网友。
        //            AllocAgentForUserHandler(privateToken, alias, message);
        var userinfo = {
            UserId: message.c.f,
            cs: message.c.cs,
            cst: ChatStuff.parseLongDate(message.c.t),
            ctn: message.c.ctn,
            ctp: message.c.ctp,
            lgid: 0
        };
        AddNewChat(userinfo);
        NewMessageFlicker();
        //        return false;
    }

    //有网友新消息，更新临控信息行状态图标
    if (uid != "System") {
        //doFlashTitle();

    }
    NewMessageFlicker();
    //收到消息内容            
    var chatinfo = {
        UserId: uid, //网友ID,
        AgentID: '', //坐席ID
        name: $('#DialogL' + message.c.cs).attr('ctn'), //网友别名.text()
        rectime: message.ct, //收到消息时间
        sendtime: ChatStuff.parseLongDate(message.c.t),
        //rectime: getHoursMinute2(message.ct),
        cs: message.c.cs,
        content: message.c.m //消息内容
    };

    //显示聊天记录
    CreateMyChatNew(chatinfo);
    //更新未读数目
    UpdateUnReadCount(message.c.cs);
    return false;

}


function FailureHandler(privateToken, alias, error) {
    //alert("FailureHandler");
    if (error == "CometClient does not exist.") {
        ChatStuff.ChangeAgentState(2);
    }
    else {
        // alert(error);
        //$.jAlert(error);
    }
    // Quit();
    //    NewMessageFlicker();
}

function TimeoutHandler(privateToken, alias, message) {
    GetServerTime(new Date(parseFloat(message.ct)));
    //alert("timeout");
    //console.log('timeout');
    //document.getElementById("messages").innerHTML += "timeout<br/>";
}

//网友离开通知
function AgentLeaveHandler(privateToken, alias, message) {

    var csidT = message.cs;

    var chatinfo = {
        uid: message.f, //网友ID        
        cs: csidT,
        name: $('#DialogL' + csidT).attr('ctn'), //别名
        rectime: GetServerTime(), // ChatStuff.parseLongDate(message.t), //收到消息时间
        content: "会话已结束或对方已离线." //消息内容
    };
    RemoveSingleWY(csidT);
    //显示聊天记录
    CreateMyChatNew(chatinfo);

    //    NewMessageFlicker();
}


//添加新聊天用户
function AddNewChat(userinfo) {

    GenerateChatLayer(userinfo);

    //如果页面中没有其他的客户时，显示此第一个客户,
    if ($('#DialogMain .dialogue:visible').length == 0) {
        ChatStuff.CsId = userinfo.cs;
        ChatStuff.UserId = userinfo.UserId;
        ChatStuff.vid = userinfo.vid;
        ShowChatLayer(userinfo.cs);
    }
    setTab('one', 1, 4);
}

function UpdateUnReadCount(csid) {
    if (ChatStuff.CsId == csid) {
        return false;
    }
    var dialog$ = $('#DialogL' + csid);
    var a$ = dialog$.find('span');
    if (a$.length == 0) {
        dialog$.append(CreateCircleStatus(1));
    } else {
        var num = parseInt($.trim(a$.text()));
        a$.text(num + 1).closest('span').removeClass('close').addClass('newbox');
    }
    return false;
}

function CreateCircleStatus(status) {
    if (!status) {
        return "";
    }
    if (status < 0) {
        return '<span class="close"><a href="#"></a></span>';
    } else {
        return '<span class="newbox">' + status + '</span>';
    }
}

//uid:网参ID，UserReferURL:访客来源,AllocID:会话标识
function GenerateChatLayer(userinfo) {

    var tCsId = userinfo.cs;

    //    var DialogLeft = '<div id="DialogL' + tCsId + '" did="' + userinfo.UserId + '" CsId="' + tCsId + '"  ctp="' + userinfo.ctp + '"  ctn="' + userinfo.ctn + '"  mc="' + userinfo.mc + '" lgid="' + userinfo.lgid + '" isopen="0" class="queue"><i></i><h4>' + userinfo.mn + '</h4>' + CreateCircleStatus() + '</div>';
    var dialogLeft = '<div id="DialogL' + tCsId + '" did="' + userinfo.UserId + '" cs="' + tCsId + '"  ctp="' + userinfo.ctp + '" vid="' + userinfo.vid + '" ctn="' + userinfo.ctn + '"  uid="' + userinfo.UserId + '" isopen="0" class="queue"><i></i><h4>' + userinfo.ctn + '</h4></div>';
    var DialogMid = CreateDialogDiv(userinfo);
    //    var DialogRight = CreateUserTable(userinfo);

    //默认按网友分配时间降序排列，如有新消息网友排在最前面
    $('#tabLeftTotalInfo').after($(dialogLeft));
    $('#DialogMain').append(DialogMid);
    //    $('#con_one_2').append(DialogRight);
}

//显示指定消息聊天层
function ShowChatLayer(cs) {
    //显示左边
    $('#tabLeft .online').removeClass('online'); //.addClass('queue');
    var dialogL$ = $('#DialogL' + cs);
    dialogL$.removeClass("new_msg").addClass('online');
    var span$ = dialogL$.find('span');
    //如果含有未读对话时，移除该未读对话
    if (span$.length > 0 && span$.find('a').length == 0) {
        span$.remove();
    }


    $('#DialogMain .dialogue').hide();
    $('#DialogM' + cs).show();

    setTab('one', 1, 4);
    //  滚动置底部、
    var divP$ = $('#DialogM' + cs).find('.scroll_gd');
    if (divP$.length > 0) {
        divP$.scrollTop(divP$[0].scrollHeight);
    }

    //滚动条默认在最下边
    //document.getElementById("mychat" + uid).scrollTop = document.getElementById("mychat" + uid).scrollHeight;
}


function RemoveSingleWY(csid) {
    var dl$ = $('#DialogL' + csid);
    if (dl$.length == 0) return false;

    if (!dl$.hasClass('offline')) {
        dl$.removeClass('queue').addClass('offline').find('span').remove().end().append(CreateCircleStatus(-1));
    }
    $('#DialogM' + csid).find('.ask_t').attr("contenteditable", "false").end().find('.style_1').empty();
    $('#DialogM' + csid).find('.endmsg input[name="btnSend"]').remove();
    return false;
}

function ReSuriveWY(csid) {
    var dl$ = $('#DialogL' + csid);
    if (dl$.length == 0) return false;

    if (dl$.hasClass('offline')) {
        dl$.removeClass('offline').addClass('queue').find('span').remove(); //.end().append(CreateCircleStatus(-1));
    }
    $('#DialogM' + csid).find('.ask_t').attr("contenteditable", "true").end().find('.style_1').append('<span class="kind_detail"><a href="#" title="表情" class="fw_bq"></a><a href="#" title="历史记录" class="fw_jl"></a><a href="#" title="上传文件" class="fw_sc"></a><a href="#" title="截屏" class="fw_jp"></a><a href="#" title="满意度" class="fw_my"></a><a href="#" title="用户转移" class="fw_zy"></a><a href="#" title="发送短信" class="fw_news"></a></span>');
    $('#DialogM' + csid).find('.endmsg span').append('<input type="button" value="发送" name="btnSend" title="发送" class="w60">');
    return false;
}


function DisableAllWY() {
    var csidT = -1;
    $('#tabLeft > div').each(function (i, domE) {
        csidT = $(domE).attr("cs");
        if (csidT) {
            RemoveSingleWY(csidT);
        }
    });
}


function CreateMyChatNew(chatinfo, self) {
    self = (self == null) ? "" : "2";
    var user$ = $('#DialogM' + chatinfo.cs);
    if (user$.length == 0) return;
    var rectime = getHoursMinute2(chatinfo.rectime);
    if (chatinfo.utcNow == null) {
        //chatinfo.utcNow = Date.now();
        chatinfo.utcNow = (new Date()).valueOf();
    }
    var html = "<div class='dh1' id='" + chatinfo.utcNow + "'>";


    html += "<div class='title" + self + "'>" + chatinfo.name + "  ：  " + rectime;
    if (self == "2") {
        html += "<em class='embg'></em>";
    }

    //html += "<div class='title'>" + chatinfo.name + tuid + " " + chatinfo.rectime + "</div>";
    html += "</div><div class='dhc" + self + "'>" + unescape(chatinfo.content) + "</div>";
    html += "</div>";


    user$.find('.endmsg em').text(chatinfo.rectime);
    var DivP$ = user$.find('.scroll_gd');
    DivP$.append(html);

    if (ChatStuff.CsId == chatinfo.cs) {
        //DivP$.scrollTop(DivP$[0].scrollHeight);
        DivP$[0].scrollTop = DivP$[0].scrollHeight;
    } else {
        $('#DialogL' + chatinfo.cs).addClass("new_msg");
    }

    //if (!$('#DialogL' + chatinfo.uid).hasClass("online")) {
    //    $('#DialogL' + chatinfo.uid).addClass("new_msg");
    //}
}


//消息接收人：recipient
//消息内容：message
function SendMessage() {

    //判断是否已经离线
    var DialogL$ = $('#DialogL' + ChatStuff.CsId);
    if (DialogL$.length == 0) {
        return false;
    }
    if (DialogL$.hasClass('offline')) {
        RemoveSingleWY(DialogL$.attr("cs"));
        return false;
    }
    //会话标识，每分配一次生成新标识
    var allocId = DialogL$.attr('cs');

    //消息内容

    //把超链接设置成新窗口打开

    var MsgDiv$ = $("#DialogM" + ChatStuff.CsId).find('.ask_t');
    MsgDiv$.find("a").attr('target', '_blank');

    var message = HtmlReplacehaveImgA(MsgDiv$.html());
    if (Len(message) > 480) {
        alert("最大数量不能超过500..");
        return false;
    }
    if (ChatStuff.UserId == "") {
        alert("消息接收人不能为空!");
        return false;
    }
    if (message == "") {
        alert("消息内容不能为空!");
        return false;
    }


    message = WrapHttpWithATag(message);


    //格式化特殊字符
    //message = FormatSpecialCharacters(message);
    //清空发送消息文本框                         
    //$("#mymsg" + uid).val('');

    //消息发送人

    var parameters = {
        action: 'sendmessage',
        FromPrivateToken: ChatStuff.AgentToken,
        SendToPrivateToken: ChatStuff.UserId,
        AllocID: escape(allocId),
        usertype: 1,
        message: message,
        encry: 1

    };

    SendMessageByPara(parameters);
    return false;
}

//发送满意度消息
function SendSatisMsg() {

    var allocId = $('#DialogL' + ChatStuff.CsId).attr('cs');
    if (ChatStuff.UserId == "") {
        alert("消息接收人不能为空!");
        return;
    }
    var parameters = {
        action: 'sendmessage',
        FromPrivateToken: ChatStuff.AgentToken,
        SendToPrivateToken: ChatStuff.UserId,
        AllocID: escape(allocId),
        message: '<span style="color:#333333;">非常感谢您的使用，请</span><span style="text-decoration:underline;color:#3399FF; cursor:pointer;" onclick="addSatisfaction()">点击这里</span><span style="color:#333333;">对我的服务做出评价。</span>',
        usertype: 1,
        MessageType: 6,
        encry: 1
    };
    SendMessageByPara(parameters);
    //非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！
}

function GetSecondIds() {
}

function SendMessageByPara(parameters) {

    $('#DialogM' + ChatStuff.CsId).find('.ask_t').text('');


    //当前系统时间                                
    var dtime = getSysTime();
    //var dtime = getHoursMinute2(r2.rectime);
    //收到消息内容            
    var chatinfo = {
        uid: parameters.SendToPrivateToken, //网友ID
        //AgentID: username, //坐席ID
        cs: parameters.AllocID,
        name: '您', //别名
        rectime: getSysTime(), //收到消息时间
        content: unescape(parameters.message), //消息内容
        //utcNow: Date.now()
        utcNow: (new Date()).valueOf()
    };

    //显示聊天记录
    CreateMyChatNew(chatinfo, 1);
    if (parameters.encry == 1) {
        parameters.message = escape(parameters.message);
    }
    $.ajax({
        //async: false,
        type: "POST",
        url: ChatStuff.HdlUrl, // "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            if (msg != "") {
                var r = $.evalJSON(msg); //ClientNotExists 
                if (r != null && r.result == 'SendToLeave') {
                    alert("发送消息失败，网友已离线!");
                    return;
                }
                if (r != null && r.result == 'ClientNotExists') {
                    alert("发送消息失败，您已离线!");
                    return;
                }
                var r2 = $.evalJSON(r.result);
                if (r && r2.result == 'sendok') {//登录成功之后
                    $('#' + chatinfo.utcNow + ' .title2 em').removeClass('embg').html("");
                }
                else {
                    // alert('发送消息失败：' + r.result + '/n消息内容：' + message);
                    //alert('发送消息失败：');
                    //$('#' + chatinfo.utcNow + ' .title2').append('<em>发送消息失败...</em>');
                    $('#' + chatinfo.utcNow + ' .title2 em').removeClass('embg').html("发送消息失败...");
                }
            }
            else {
                alert("您的网络出现异常，请检查网络后重新发送！/n消息内容：");

            }
        }
    });
}

/*
function GetAgentState() {

var parameters = {
action: 'getagentsatetbyid',
FromPrivateToken: ChatStuff.AgentToken
};

$.ajax({
type: "POST",
url: "AjaxServers/Handler.ashx",
data: parameters,
success: function (msg) {
var r = $.evalJSON(msg);
if (r != null && r.result != '') {
var result = $.evalJSON(r.result);
//ChatStuff.ChangeAgentState(result);
}
else {
alert('获取坐席状态失败');
}
}
});


}

*/

//更改坐席状态  1:在线， 2：离线
function SetAgentState(state, isOnLine) {
    var parameters = {
        action: 'setagentstate',
        FromPrivateToken: ChatStuff.AgentToken,
        AgentState: state
    };
    MaskPage();
    $.ajax({
        type: "POST",
        url: ChatStuff.HdlUrl, //"AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            UnMaskPage();
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {
                if (isOnLine) {
                    $.jAlert("已重新上线.", null, 5);
                } else {
                    $.jAlert("更改状态成功!", null, 5);
                }

                ChatStuff.ChangeAgentState(state);
            }
            else {
                $.jAlert('更改状态失败：' + r.result);

            }
        }
    });
}


//根据网友ID判断网友聊天层是否存在
function isExistChatLayer(cs) {
    return $('#DialogM' + cs).length > 0;
}


//获取系统时间
function getSysTime() {
    //    var now = new Date();
    var now = GetServerTime();
    var year = now.getYear();
    var month = now.getMonth();
    var day = now.getDate();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    if (parseInt(year + 1900) > 3000) {
        return parseInt(year) + "/" + parseInt(month + 1) + "/" + day + " " + hours + ":" + minutes + ":" + seconds + "";
    }
    else {
        return parseInt(year + 1900) + "/" + parseInt(month + 1) + "/" + day + " " + hours + ":" + minutes + ":" + seconds + "";
    }
}


//初始化方法
function Init() {

    InitUploadify();
    SetBeforeunload(true, onbeforeunload_handler);
    GetServerTime(); //获取服务器时间

    //绑定主聊天窗口事件
    BindDiaglogMainEvent();

    //绑定左侧用户列表事件。
    BindUserListClick();


    Login();

    //GetAgentState();
    ChatStuff.initEvent();

    //隐藏所有常用语
    $('#' + ChatStuff.ulCM + ' > li').children().hide();
    $('#onlineStatus').show();
    //StartTimer();
    ChatStuff.UpdateConvsCountTimer();

    $(document).keydown(function (eve) {
        if (eve.keyCode == 8) {

            var obj = eve.target;
            var vReadOnly = obj.getAttribute('readonly');
            var vEnabled = obj.getAttribute('enabled');
            //处理null值情况 
            vReadOnly = (vReadOnly == null) ? false : vReadOnly;
            vEnabled = (vEnabled == null) ? true : vEnabled;
            var t = obj.tagName.toLowerCase();

            //当时编辑窗口时，返回true
            var this$ = $(obj);
            if (t == "div" && this$.hasClass('ask_t') && this$.attr('contenteditable') == "true") {
                return true;
            }

            //当敲Backspace键时，事件源类型为密码或单行、多行文本的， 
            //并且readonly属性为true或enabled属性为false的，则退格键失效 
            var flag1 = ((t == "password" || t == "input" || t == "textarea") && (vReadOnly == true || vEnabled != true)) ? true : false;
            if (flag1) {
                return false;
            }
            //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效 
            var flag2 = (t != "password" && t != "input" && t != "textarea") ? true : false;
            if (flag2) {
                return false;
            }

        }
    });

    $('#stateUL').click(function (e) {
        var target$ = $(e.target);
        //        var this$ = $(this);
        //        target$.closest('li').hide().siblings().show();
        //           $('#StateTop').text($.trim( target$.text()));
        SetAgentState(target$.attr('data'));

        //           target$.closest('span').removeClass('nav_class_hover').addClass('nav_class');
    });

}

function onbeforeunload_handler() {
    var parameters = {
        action: 'closechat',
        FromPrivateToken: ChatStuff.AgentToken
    };
    MaskPage();
    $.ajax({
        type: "POST",
        url: ChatStuff.HdlUrl, //"AjaxServers/Handler.ashx",
        data: parameters,
        async: false,
        complete: function () {
            UnMaskPage();
        },
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {//登录成功之后                                           
            }
        }
    });
}


function CreateDialogDiv(userinfo) {
    var t = [];

    t.push('<div class="dialogue" style="display:none;" id="DialogM' + userinfo.cs + '" >');
    t.push('<p>对话开始于: ' + userinfo.cst + '</p>');

    t.push('<div class="scroll_gd"></div>');
    t.push('<div class="editC">');
    t.push('<div class="style_1">');
    t.push('<span class="kind_detail"><a href="#" title="表情" class="fw_bq"></a><a href="#" title="历史记录"class="fw_jl"></a>');
    t.push('<a href="#" title="上传文件" class="fw_sc"></a><a href="#" title="截屏"class="fw_jp"></a><a href="#" title="满意度" class="fw_my"></a><a href="#" title="用户转移" class="fw_zy"></a><a href="#" title="发送短信" class="fw_news"></a></span>');
    t.push('</div>');
    t.push('<div class="ask_c"><div class="ask_t"  contenteditable="true" ></div></div>');
    t.push('</div>');
    t.push('<div class="endmsg">(发送快捷键：Enter)最后收到消息时间：<em></em><span class="right btn" style="*margin-top: -20px;"><input type="button" value="结束对话" title="结束对话" class="w80 gray endBtn" /><input type="button" value="对话标签" style="margin:0 2px;"  title="对话标签" class="w80 gray" /><input type="button" value="发送" name="btnSend" title="发送" class="w60" /></span></div>');
    t.push('</div>');
    return t.join('');
}



//浏览器标题新消息闪烁
function NewMessageFlicker() {
    //    messageFlicker.clear();
    messageFlicker.show();
}


function BindUserListClick() {
    $('#tabLeft').click(function (eve) {
        if (ChatStuff.BQPop != null) {
            ChatStuff.BQPop.hide();
        }
        if (eve.target.nodeName == "EM") {
            return false;
        }
        var t = $(eve.target).closest('div');
        var idT = t[0].id;
        if (idT == "tabLeft") return false;
        //idT = idT.substr(7);
        var csidT = $(t).attr('cs');


        if (eve.target.nodeName == "A") {
            eve.stopPropagation();
            eve.preventDefault();

            if (csidT == ChatStuff.CsId) {
                ClearUserTabInfo();
                return false;
            } else {
                $('#DialogL' + csidT).remove();
                $('#DialogM' + csidT).remove();
                ChatStuff.CsId = -1;
                ChatStuff.UserId = -1;
            }

            return false;
        }
        ChatStuff.CsId = csidT;
        ChatStuff.UserId = $.trim($(t).attr('did'));
        ChatStuff.ctn = $.trim($(t).attr('ctn'));
        ChatStuff.ctp = $.trim($(t).attr('ctp'));
        ChatStuff.vid = $.trim($(t).attr('vid'));
        $('#txt_Telephone').val(ChatStuff.ctp);
        ShowChatLayer(ChatStuff.CsId);

        return true;
    });
}


function BindDiaglogMainEvent() {
    $('#DialogMain').bind('click', function (eve) {
        //if (eve.target.title == "") return false;
        switch (eve.target.title) {
            case "表情":
                BiaoqingClick(eve);
                break;
            case "历史记录":
                HistoryClick();
                break;
            case "上传文件":
                FileUpLoadClick(); break;
            case "截屏":
                CaptureScreenClick(); break;
            case "满意度":
                SatisClick(eve); break;
            case "结束对话":
                BtnEndDialogClick(); break;
            case "对话标签":
                BtnAddWorkClick(); break;
            case "发送":
                BtnSendClick(); break;
            case "用户转移":
                TransferAgent();
                break;
            case "发送短信":
                SendSimpleMesssage();
                break;
            default:
                break;

        }
    });



    $("#DialogMain").bind("keydown", function (eve) {
        //SendMessage();
        if (eve.ctrlKey && eve.keyCode == 13) {
            var MsgDiv$ = $("#DialogM" + ChatStuff.CsId).find('.ask_t');
            if (MsgDiv$.length > 0) {
                if ($.browser.msie) {
                    MsgDiv$[0].innerHTML += "<div></div>";
                } else {
                    MsgDiv$[0].innerHTML += "<div><br/></div>";
                }
                moveEnd(MsgDiv$);
                moveEnd(MsgDiv$);
                return false;
            }
        }
        else if (!eve.ctrlKey && eve.keyCode == 13) {
            eve.stopPropagation();
            eve.preventDefault();
            SendMessage();
            return false;
        }

    });


}

function BiaoqingClick(eve) {
    eve.stopPropagation();
    eve.preventDefault();

    if (ChatStuff.BQPop == null) {
        $('#DialogMain').BQPop({ left: 10, top: 80 });
        ChatStuff.BQPop = $('#BQMain').show();
    } else {
        ChatStuff.BQPop.toggle();
    }
}

function BQClick(url, e) {
    var tContent$ = $('#DialogM' + ChatStuff.CsId).find('.ask_t');
    if (tContent$.length > 0) {
        tContent$.append('<img src="' + url + '"/>');
        tContent$.focus();
        moveEnd(tContent$);
        ChatStuff.BQPop.hide();
        //        tContent$.text(tContent$.text() + ' ' + tTarget$.text());
    }
    return false;
}

function TransferAgent() {
    var name = ChatStuff.ctn;
    var csidT = ChatStuff.CsId;
    $.openPopupLayer({
        name: "SelectAgents",
        url: "AjaxServers/PopPages/SelectAgents.aspx?TrueName=&BGID=",
        afterClose: function (result, data) {
            if (result && data) {
                tAgentHandler(data.agentid, data.number);
            }
        }
    });

    function tAgentHandler(targetAgentId, agentNumber) {
        if (targetAgentId == null || agentNumber == null) {
            return false;
        }

        MaskPage();
        $.post(ChatStuff.HdlUrl, { action: "transferagent", FromPrivateToken: ChatStuff.AgentToken, targetagent: targetAgentId, wyid: ChatStuff.UserId }, function (data) {
            UnMaskPage();
            data = $.parseJSON(data);
            if (data.result == "0") {

                var chatinfo = {
                    cs: csidT, //网友ID,        
                    name: name, //$('#DialogL' + val.csid + ' h4').text(), //网友别名        
                    rectime: GetServerTime(),
                    content: "---访客已成功转移至客服" + agentNumber + "，当前对话已中断---" //消息内容
                };
                //添加一条消息
                CreateMyChatNew(chatinfo);
                //移除网友
                //                RemoveSingleWY(ChatStuff.CsId);

            } else {
                alert(data.result);
            }
        });
    }

}

//发送短信
function SendSimpleMesssage() {
    //    if (tel1 == undefined) {
    //        tel1 = "";
    //    }
    //    if (tel2 == undefined) {
    //        tel2 = "";
    //    }

    var Telephonetemp = ChatStuff.ctp;
    //    if (tel1 != "") {
    //        Telephonetemp = tel1;
    //        if (tel2 != "") {
    //            Telephonetemp += "," + tel2;
    //        }
    //    } else if (tel2 != "") {
    //        Telephonetemp = tel2;
    //    }

    $.openPopupLayer({
        name: "SendSMSPopup22",
        parameters: { PageType: escape("2"), Tel: Telephonetemp },
        url: "/AjaxServers/SendSMSPoperNew2.aspx",
        beforeClose: function (e, data) {
            //            loaded = false;
            if (data != '') {
                $("#hidSendSmSPersonal").val(data.Tels);
            }
        }
    });
}

function HistoryClick() {
    var curr$ = $('#DialogL' + ChatStuff.CsId);
    var urlT = "MessageHistoryForAgent.aspx?vid=" + curr$.attr("vid");
    window.open(urlT);
}
function FileUpLoadClick() {

    //$('#uploadify').uploadify('upload', '*')
}
function CaptureScreenClick() {
    f_capture();
}
function SatisClick(eve) {
    eve.preventDefault();
    eve.stopPropagation();
    if ($('#DialogL' + ChatStuff.CsId).attr('isSatis') == "1") {
        alert("网友已完成满意度评价,不允许多次发送评价请求.");
        return;
    }

    SendSatisMsg();
    //$('#DialogL' + ChatStuff.CsId).attr('isSatis', "1");
}

//结束对话
function BtnEndDialogClick() {

    if ($('#DialogL' + ChatStuff.CsId).hasClass('offline')) {
        ClearUserTabInfo();
        //        ChatStuff.CsId = -1;
        return;
    }

    if ($.jConfirm("您确定要结束对话么？", function (r) {
        if (r) {
            CloseSingleUser();
        }

    }));

}

function ClearUserTabInfo() {
    $('#DialogL' + ChatStuff.CsId).remove();
    $('#DialogM' + ChatStuff.CsId).remove();
    $('#con_one_2').empty();
    $('#txt_Telephone').val('');
    $('#ajaxOrderInfoList').empty();
    ChatStuff.CsId = '-1';
}

//创建工单
function BtnAddWorkClick() {
    //    CheckOrderComplete();
    $.openPopupLayer({
        name: "TagSelectPopNew",
        parameters: { "tagid": ChatStuff.tagId, "busitypeid": ChatStuff.busiTypeId },
        url: "PopPage/SelectTagLayer.aspx",
        beforeClose: function (e, data) {
            if (e) {
                ChatStuff.tagId = data.tagid;
                ChatStuff.tagName = data.tagname;
                ChatStuff.busiTypeId = data.busiTypeId;
                $.post(ChatStuff.HdlUrl, { action: "udpt", tgd: ChatStuff.tagId, tgn: escape(ChatStuff.tagName), csid: ChatStuff.CsId }, function (msg) {
                    if (msg == "ok") {
                        $.jConfirm("是否要添加工单", function (r) {
                            if (r) {
                                CheckOrderComplete();
                            }
                        });
                    } else {
                        $.jAlert(msg);
                    }
                });
            } else {

            }
        },
        success: function () {
            if (ChatStuff.tagId != 0) {
                var choose$ = $('.choose');
                choose$.empty().append('<li class="lichoose" name="imgDelTag"  did="' + ChatStuff.tagId + '">' + ChatStuff.tagName + '&nbsp;<img src="/images/xz_close.png"></li>');
            }
        }
    });
    return false;
}

//发送消息
function BtnSendClick() {
    SendMessage();
}

function CloseSingleUser() {
    var parameters = {
        action: 'closesinglechat',
        FromPrivateToken: ChatStuff.AgentToken,
        SendToPrivateToken: ChatStuff.UserId
    };

    $.ajax({
        type: "POST",
        url: ChatStuff.HdlUrl, //"AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {//登录成功之后                            
                $.jAlert("对话已结束", function () {
                    ClearUserTabInfo();

                    //                    $('#DialogR' + ChatStuff.CsId).remove();
                });

            }
            else {
                $.jAlert('请求失败，请稍后重试：' + r.result);

            }
        }
    });
}


//系统启动
$(function () {

    if (ChatStuff.IsDuplicateLogin == "1") {
        alert("您已经登录不允许重复登录，您可以30秒后重试..");
        //        $.jAlert('您已经登录不允许重复登录，您可以30秒后重试..');
        CloseWindow();
        return false;
    }


    Init();
    $('#btnTest').click(function () {

        return;
        ChatStuff.i++;
        var userinfo = {
            UserId: ChatStuff.i,
            M: ChatStuff.m + ChatStuff.i,
            cs: ChatStuff.i,
            mn: ChatStuff.mn + ChatStuff.i,
            cst: ChatStuff.cst + ChatStuff.i,
            adr: ChatStuff.adr + ChatStuff.i,
            cgn: ChatStuff.cgn + ChatStuff.i,
            lct: ChatStuff.lct + ChatStuff.i,
            dst: ChatStuff.dst + ChatStuff.i,
            ctn: ChatStuff.ctn + ChatStuff.i,
            ctj: ChatStuff.ctj + ChatStuff.i,
            ctp: ChatStuff.ctp + ChatStuff.i,
            mc: ChatStuff.mc + ChatStuff.i,
            uft: ChatStuff.uft + ChatStuff.i,
            lgid: ChatStuff.lgid + ChatStuff.i
        };

        AddNewChat(userinfo);
    });

    $('#btnAgentLive').click(function () {
        var id = $('#idtest').val();
        $('#DialogL' + id).removeClass('queue').addClass('offline').append('<span class="close"><a href="#">关闭</a></span>');
    });

    $('#ReceiveMsg').click(function () {
        //        var id = $('#idtest').val();
        //        if (!$('#DialogL' + id).hasClass("online")) {
        //            $('#DialogL' + id).addClass("new_msg");
        //        }
        //CloseWindow();
        // CheckDialogState();
        //clearInterval(ChatStuff.checkStateTimer);
        //$(this).toggle(function () {
        //    setEmotionTab('tabEmotion', 2, 3);
        //    $(".bq_list").css("display", "block");
        //}, function () {
        //    $(".bq_list").css("display", "none");
        //});

        //Quit();
        $('#spanOrder').click();

    });

    $(document).click(function () {
        messageFlicker.clear("对话管理");
        $('#' + ChatStuff.ulCM + ' > li').children(':visible').hide();
    });


});

//登录系统
function Login(isOnLine) {
    var parameters = {
        action: 'init',
        FromPrivateToken: ChatStuff.AgentToken,
        usertype: '1',
        isReInit: (isOnLine ? '1' : '0')
    };
    MaskPage();
    $.ajax({
        type: "POST",
        url: ChatStuff.HdlUrl, // "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            ChatStuff.isLeaveNotice = false;
            if (msg == null || msg == "") {
                alert('登录失败：请检测您的网络连接或者重新登录.');
                return;
            }
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'loginok') {//登录成功之后
                if (r.fm) {
                    ChatStuff.AgentToken = r.fm;
                }
                defaultChannel = null;
                StartLongConnect();
                StartTimer();
                //掉线登录后直接设置为在线
                //                if (isOnLine) {
                //                    SetAgentState(1, true);
                //                }
            }
            else {
                $.jAlert('登录消息：' + r.result, function () {
                    //window.location = "Login.aspx";
                    //alert("登录失败");
                    if (r.result == '您已经登录，不能重复登录！') {
                        CloseWindow();
                    }
                });

            }

        }, complete: function () {
            UnMaskPage();
        }
    });
}

function CloseWindow(parameters) {
    window.onbeforeunload = null;
    //SetBeforeunload(false, onbeforeunload_handler);
    if ($.browser.msie) {
        window.opener = null;
        window.open('', '_self');
        window.close();
    } else if ($.browser.webkit) {

        //        $(document).empty();
        window.opener = null;
        window.open('', '_self');
        window.close();
    } else {
        window.opener = null;
        window.open('', '_parent', '');
        window.close();
    }

    $(document.body).empty();
}



///针对类型///Date(-2209017600000+0800)/
ChatStuff.parseLongDate = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    } else {
        var myDate = new Date(parseInt(strDate.substring(strDate.indexOf("(") + 1, strDate.indexOf(")"))));
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        if (month < 10) {
            month = "0" + month;
        }
        var day = myDate.getDate();
        if (day < 10) {
            day = "0" + day;
        }
        var h = myDate.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = myDate.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = myDate.getSeconds("ss");
        if (s < 10) {
            s = "0" + s;
        }
        return year + "/" + month + "/" + day + " " + h + ":" + m + ":" + s;
    }
};

///针对类型///Date(-2209017600000+0800)/
ChatStuff.parseShortDate = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    } else {
        var myDate = new Date(parseInt(strDate.substring(strDate.indexOf("(") + 1, strDate.indexOf(")"))));
        var h = myDate.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = myDate.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = myDate.getSeconds("ss");
        if (s < 10) {
            s = "0" + s;
        }
        return h + ":" + m + ":" + s;
    }
};

//针对 2014/11/5 14:15:52
ChatStuff.parseTime = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    } else {
        var myDate = new Date(strDate);

        var h = myDate.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = myDate.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = myDate.getSeconds("ss");
        if (s < 10) {
            s = "0" + s;
        }
        return h + ":" + m + ":" + s;
    }
};

//截屏相关

function f_onUploadCompleted(responseText) {
    f_log("图片上传完成.");
    var jsonData = $.evalJSON(responseText);
    if (jsonData.result == "noFiles") {
        $.jAlert("截屏出错!");
    }
    else if (jsonData.result == "failure") {
        $.jAlert("截屏出错!");
    }
    else if (jsonData.result != "succeed") {
        $.jAlert("截屏出错!");
    }
    else {
        //上传成功
        var filepath = jsonData.FilePath;
        filepath = filepath.replace(new RegExp(/(--)/g), '/');
        var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + filepath;
        var imgstr = "<img src='" + hostT + "'/>";
        var tContent$ = $('#DialogM' + ChatStuff.CsId).find('.ask_t');
        if (tContent$.length > 0) {
            tContent$.append(imgstr);
        }
    }
}



function InitUploadify() {
    var uploadSuccess = true;
    $("#uploadify").uploadify({
        'buttonText': '',
        'buttonImg': 'css/img/shangchuan.png',
        'hideButton': false,
        'queueID': 'fileQueue',
        'auto': true,
        'swf': 'Scripts/uploadify.swf',
        'uploader': '/AjaxServers/FileLoad.ashx?v=' + Math.random(),
        'multi': false,
        'fileSizeLimit': '5MB',
        'queueSizeLimit': 1,
        'method': 'post',
        'removeTimeout': 1,
        'fileTypeDesc': '*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.gif;*.png',
        'fileTypeExts': '*.doc;*.docx;*.xls;*.xlsx;*.jpg;*.gif;*.png',
        'width': 20,
        'height': 24,
        'onUploadSuccess': function (file, data, response) {
            if (response == false) {
                uploadSuccess = false;
                $.jAlert("上传失败!");
            }
            else {
                //    alert('The file ' + file.name + ' was successfully uploaded with a response of ' + response + ':' + data);
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "noFiles") {
                    uploadSuccess = false;
                    $.jAlert("请选择文件!");
                }
                else if (jsonData.result == "failure") {
                    uploadSuccess = false;
                    $.jAlert("上传文件出错!");
                }
                else if (jsonData.result != "succeed") {
                    uploadSuccess = false;
                }
                else {
                    //上传成功
                    uploadSuccess = true;
                    var filepath = jsonData.FilePath;
                    filepath = filepath.replace(new RegExp(/(--)/g), '/');
                    var filename = unescape(jsonData.FileName);
                    var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + filepath;
                    var tMsg = "<a class='upfile' target='_blank' href=" + hostT + ">下载文件：" + filename + "</a>";

                    //var tContent$ = $('#DialogM' + ChatStuff.UserId).find('.ask_t');
                    //if (tContent$.length > 0) {
                    //    tContent$.append(tMsg);
                    //}
                    //SendMessage("7", filename, jsonData.ExtendName, jsonData.FileSize, filepath);
                    //MessageType, FileName, FileType, FileSize, FilePath
                    var paras = {
                        action: 'sendmessage', FromPrivateToken: ChatStuff.AgentToken,
                        usertype: 1, message: escape(tMsg),
                        SendToPrivateToken: ChatStuff.UserId, AllocID: $('#DialogL' + ChatStuff.CsId).attr("cs"),
                        MessageType: 7, FileName: escape(filename), FileType: escape(jsonData.ExtendName),
                        FileSize: escape(jsonData.FileSize), FilePath: escape(filepath)
                    };
                    SendMessageByPara(paras);
                }
            }
        },
        'onQueueComplete': function (queueData) {
            // alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
            if (uploadSuccess) { //上传都成功
            }
        }
    });
}



//满意度弹层
function addSatisfaction() {

}


function setTab(name, cursel, n) {
    //return false;
    for (i = 1; i <= n; i++) {
        var menu = document.getElementById(name + i);
        var con = document.getElementById("con_" + name + "_" + i);
        menu.className = i == cursel ? "hover" : "";
        con.style.display = i == cursel ? "block" : "none";
    }
    if (name == "one" && cursel == 1) {
        $('#' + ChatStuff.ulCM + ' ul[name=ulLiItem]').hide().siblings().hide();
        return false;
    }
    if (name == "one" && cursel == 2) {
        DialogR2Click();
        return false;
    }
    if (name == "one" && cursel == 3) {
        //getEmotionInfo(cursel);
        DialogR3Click();
        return false;
    }
}

function DialogR2Click() {
    $('#con_one_2').empty();
    if (ChatStuff.CsId == -1) {
        return;
    }
    $('#rightTabDiv').Mask();
    $(".blue-loading").css("width", "55%");
    $.ajax({
        url: "CustInfo/VisitCustInfo.aspx?loginid=" + ChatStuff.UserId + '&VisitID=' + ChatStuff.vid + '&CSID=' + ChatStuff.CsId + '&r=' + Math.random(),
        cache: false,
        success: function (html) {
            $('#rightTabDiv').UnMask();
            $('#con_one_2').html(html).show();
        }
    });

}
function DialogR3Click() {

    return false;
    var dialogR3$ = $('#DialogR3' + ChatStuff.CsId);
    if (dialogR3$.length == 0) {
        $.ajax({
            url: "",
            cache: false,
            success: function (html) {
                $('#con_one_3').append(html);
            }
        });
        dialogR3$ = $('#DialogR3' + ChatStuff.CsId);
        dialogR3$.siblings().hide().end().show();
    }

}

function getEmotionInfo(ecategory) {
    $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getemotioninfobyecategory', ECategory: ecategory }, function (data) {
        if (data != "") {
            var jsonData = $.evalJSON(data);

            if (jsonData != "") {
                var temphtml = "<tr>";
                $.each(jsonData.root, function (idx, item) {
                    temphtml += '<td><img class="emotionImgs" src="' + item.EUrl + '" width="16" height="16"/></td>';
                    if ((idx + 1) % 10 == 0) {
                        temphtml += "</tr><tr>";
                    }
                });
                temphtml = temphtml.substr(0, temphtml.length - 5);
                switch (ecategory) {
                    case 1: $("#con_one_1 table").html(temphtml); break;
                    case 2: $("#con_one_2 table").html(temphtml); break;
                    case 3: $("#con_one_3 table").html(temphtml); break;
                }

            }
        }
    });
}

function LeaveNetOverTimeNotice() {
    //StopTimer();
    ChatStuff.ChangeAgentState(2);

    if (!ChatStuff.isLeaveNotice) {
        ChatStuff.isLeaveNotice = true;
        $.jConfirm("您已经离线，是否要重新连接?", function (blR) {

            if (blR) {
                Login(true);
            } else {
                ChatStuff.isLeaveNotice = false;
            }
        });

    }
}


//定时监测对话状态
function CheckDialogState() {

    clearTimeout(ChatStuff.checkStateTimer);

    var cslst = [];
    $('#tabLeft > div[cs]').each(function (i, e) {
        if (!$(e).hasClass('offline')) {
            cslst.push($(e).attr('cs') + "_" + $(e).attr('did'));
        }
    });


    cslst = cslst.length == 0 ? "" : cslst.join(',');

    $.ajax({
        url: ChatStuff.HdlUrl, // 'AjaxServers/Handler.ashx',
        type: "post",
        data: { action: "checkstate", csids: cslst, FromPrivateToken: ChatStuff.AgentToken },
        timeout: 5000,
        cache: false,
        dataType: "json",
        complete: function (xhr, TS) {
            ////启动计时
            ChatStuff.checkStateTimer = setTimeout(CheckDialogState, ChatStuff.checkStateDuration);

            ////断网时计时判断是否超过指定时间
            try {
                if (TS != 'timeout' && (xhr.status == 0 || xhr.status == 12029 || xhr.status == 12030 || xhr.status == 12031 || xhr.status == 12152 || xhr.status == 12159)) {
                    CheckTimeOut();
                }
                else {
                    ChatStuff.timeOutStart = new Date();
                }
            } catch (e) {
                CheckTimeOut();
            }

        },
        success: function (dataT, textStatusT) {

            if (dataT && dataT.astatus == "0") {
                ////状态设置为离线                
                LeaveNetOverTimeNotice();
            }
            if (dataT && dataT.result == "sendok") {
                $.each(dataT.data, function (n, val) {

                    if (val != null && val.state == 0 && !$('#DialogL' + val.csid).hasClass('offline')) {

                        var chatinfo = {
                            cs: val.csid, //网友ID,        
                            name: $('#DialogL' + val.csid + ' h4').text(), //网友别名        
                            rectime: GetServerTime(),
                            content: "已离线" //消息内容
                        };
                        CreateMyChatNew(chatinfo);
                        RemoveSingleWY(val.csid);
                        /*
                        if (!$('#DialogL' + val.csid).hasClass('offline')) {
                        $('#DialogL' + val.csid).removeClass("online queue").addClass('offline').find('span').remove().end().append(CreateCircleStatus(-1));
                        }
                        */
                        //                        $('#DialogL' + val.csid).removeClass('queue ').removeClass('online').addClass('offline').append('<span class="close"><a href="#">关闭</a></span>');
                    }
                });
            }

        },
        error: function (xT, textStatusT, errorThrownT) {
            //if (textStatusT == "timeout") {
            //    LeaveNetOverTimeNotice();
            //}

        }
    });

    //记录是否已经离线    

    function CheckTimeOut() {

        if ((new Date()) - ChatStuff.timeOutStart > ChatStuff.TimeidleAgent) {
            LeaveNetOverTimeNotice();
        }
    }
}

function CheckOrderComplete() {
    var csidT = ChatStuff.CsId;
    var tagId = ChatStuff.tagId;
    var tagName = ChatStuff.tagName;
    var businesstypeid = ChatStuff.busiTypeId
    var curr$ = $('#DialogL' + csidT);
    if (curr$.length == 0) return;
    $('#MainContentDiv').Mask();
    $.post(ChatStuff.HdlUrl, { action: "checkorder", csid: csidT, tgd: tagId, businesstypeid: businesstypeid, CustName: escape(curr$.attr("ctn")), CalledNum: ChatStuff.ctp }, function (a, b, c) {
        $('#MainContentDiv').UnMask();
        var s;
        if (a != "") {
            s = $.parseJSON(a);
            if (s.Result == "error") {
                $.jAlert(s.Data);
                return false;
            }
        } else {
            $.jAlert("参数错误");
            return false;
        }
        var OrderID = s.OrderID;
        if (OrderID) {
            $.jAlert("已经添加过工单,不能重复添加.");
            return false;
        }
        window.open(s.OrderURL);
        $('#aNewOrder').attr('href', s.OrderURL);
        $('#aNewOrder').attr('click', "javascript:void(0);").trigger('click');
        return false;
    });
}


function ChangeWYTabName(csid, newName) {
    var tab$ = $('#DialogL' + csid);
    if (tab$.length > 0 && newName) {
        tab$.attr('ctn', newName).find('h4').html(newName);
    }
}


function WrapHttpWithATag(str) {
    var re = /\s(http:\/\/)?([A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*)/g;
    var re2 = /^(http:\/\/)?([A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*)/g;
    str = str.replace(re, function (a, b, c) { return '<a href="http://' + c + '" target="_blank">' + a + '</a>'; });
    str = str.replace(re2, function (a, b, c) { return '<a href="http://' + c + '" target="_blank">' + a + '</a>'; });
    return str;
}

/*
function ttt(parameters) {
$.openPopupLayer({
name: "TagSelectPopNew",
//        parameters: { "val": txt$.attr("did"), "name": $.trim(txt$.val()) },
url: "PopPage/TagSelectPopNew.aspx",
beforeClose: function (e, data) {
if (e) {
ChatStuff.tagId = data.val;
ChatStuff.tagName = data.name;

$.post(ChatStuff.HdlUrl, { action: "udpt", tgd: ChatStuff.tagId, tgn: ChatStuff.tagName, csid: ChatStuff.CsId }, function (msg) {
if (msg == "ok") {
$.jConfirm("是否要添加工单", function () {
CheckOrderComplete();
});
} else {
$.jAlert(msg);
}
});
} else {

}
},
success: function () {
if (ChatStuff.tagId != 0) {
var choose$ = $('.choose');
choose$.empty().append('<li class="lichoose" name="imgDelTag"  did="' + ChatStuff.tagId + '">' + ChatStuff.tagName + '&nbsp;<img src="/images/xz_close.png"></li>');
}
}
});
return false;
}
*/