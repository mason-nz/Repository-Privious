


var ChatStuff = {
    checkStateDuration: 5000,
    TimeidleAgent: 10000,
    checkStateTimer: 0,
    AgentId: 0,
    i: 0,
    UserId: 0,
    mc: "经销商code",
    lgid: "loginid",
    CsId: 0,
    mn: "经销商名称",
    cst: "开始时间",
    adr: "地址",
    cgn: "城市群",
    lct: "最后消息时间",
    dst: "晨曦时间",
    ctn: "Contract",
    ctj: "联系人职务",
    newMsgDuration: 3000,
    newMsgTimer: 3000,   //计时表示
    uft: "跳转页面",
    ctp: "13913913691",
    isLeaveNotice: false, //记录是否已经弹出重新登录提示
    ChangeAgentState: function (txtState) {
        //1,在线；2：离线
        if (txtState == "1") {
            $('#StateTop').text("在线");
            $('#StateBotm').text("离线");
            // StartTimer();
        } else {
            $('#StateBotm').text("在线");
            $('#StateTop').text("离线");
            //移除所有网友

            //            ChatStuff.UserId = -1;
            //            ChatStuff.CsId = -1;
            //            $('#tabLeft').empty();
            //            $('#DialogMain').empty();
            //            $('#con_one_2').empty();
            DisableAllWY();
            //StopTimer();
        }
    }
};
var defaultChannel = null;

function CheckFlickMsg() {
    if ($('#tabLeft > div.new_msg').length > 0) {
        if (messageFlicker.working != 1) {
            messageFlicker.show();
            messageFlicker.working = 1;
        }
    } else {
        if (messageFlicker.working == 1) {
            messageFlicker.clear();
            messageFlicker.working = 0;
        }
    }
}

ChatStuff.initEvent = function () {

    $('#StateBotm').click(function () {

        SetAgentState(($(this).text() == "在线") ? "1" : "2");
    });

    //初始化常用服务语
    $('#' + ChatStuff.ulCM + ' > li').click(function (eve) {
        var tTarget$ = $(eve.target);
        if (tTarget$.hasClass('item')) {
            //$('#DialogM' + ChatStuff.CsId).find('.ask_t').text(tTarget$.text());
            var tContent$ = $('#DialogM' + ChatStuff.CsId).find('.ask_t');
            if (tContent$.length > 0) {
                tContent$.text(tContent$.text() + ' ' + tTarget$.text());
            }
            tTarget$.closest('ul').hide().siblings().hide();
            eve.stopPropagation();
            eve.preventDefault();
            return false;
        }
        var this$ = $(this);

        this$.siblings().children().hide();

        if (this$.prev().length > 0) {
            var topT = this$.position().top * -1;
            this$.find('ul').css('top', topT);
        }
        this$.children().show();
    });

}

function StartTimer() {
    StopTimer();
    ChatStuff.newMsgTimer = setInterval(CheckFlickMsg, ChatStuff.newMsgDuration);
    ChatStuff.checkStateTimer = setTimeout(CheckDialogState, ChatStuff.checkStateDuration);
}

function StopTimer() {
    clearInterval(ChatStuff.newMsgTimer);
    clearTimeout(ChatStuff.checkStateTimer);
}


//开始长连接
function StartLongConnect() {
    if (defaultChannel == null) {
        defaultChannel = new AspNetComet("/DefaultChannel.ashx", ChatStuff.AgentId, "defaultChannel");
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
        //发送长连接请求
        defaultChannel.subscribe();
    }
}

function SatisfactionHandLer(privateToken, alias, message) {
    var csidT = message.cs;
    $('#DialogL' + csidT).attr('isSatis', "1");
    var chatinfo = {
        uid: message.f, //网友ID
        //AgentID: username, //坐席ID
        CsId: csidT,
        name: $('#DialogL' + csidT).text(), //别名
        rectime: ChatStuff.parseLongDate(message.t), //收到消息时间
        content: "网友已完成满意度评价." //消息内容
    }

    //显示聊天记录
    CreateMyChatNew(chatinfo);

}

function QueueSortHandLer(privateToken, alias, message) {
    alert(message)
}

function SendFileHandLer(privateToken, alias, message) {
    //SuccessHandler(privateToken, alias, message);

    var chatinfo = {
        uid: message.f, //网友ID,
        CsId: message.cs, //网友ID,
        AgentID: '', //坐席ID
        name: $('#DialogL' + message.cs).text(), //网友别名        
        sendtime: ChatStuff.parseLongDate(message.t),
        //rectime: getHoursMinute2(message.ct),
        content: message.m //消息内容
    }

    chatinfo.rectime = chatinfo.sendtime;

    //显示聊天记录
    CreateMyChatNew(chatinfo);
}

//分配坐席
function AllocAgentForUserHandler(privateToken, alias, message) {
    //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮

    if (!isExistChatLayer(message.CsId)) {
        var userinfo = {
            UserId: message.uid,
            M: message.m,
            CsId: message.cs,
            mc: message.mc,
            ctp: message.ctp,
            mn: message.mn,
            cst: ChatStuff.parseLongDate(message.cst),
            adr: message.adr,
            cgn: message.cgn,
            lct: ChatStuff.parseLongDate(message.lct),
            dst: message.dst,
            ctn: message.ctn,
            ctj: message.ctj,
            lgid: message.lgid,
            uft: message.uft,
            lmt: ChatStuff.parseLongDate(message.lmt)
        };
        //OperateChatLayer(userinfo);
        AddNewChat(userinfo);
        //NewMessageFlicker();
    }
}

function SuccessHandler(privateToken, alias, message) {
    if (message.c.f == "System") {
        return false;
    }
    //如果网友已掉线，不接受消息
    if ($('#DialogL' + message.c.cs).hasClass('offline')) {
        return false;
    }


    //网友ID
    var uid = message.c.f;
    if (!isExistChatLayer(message.c.cs) && uid != "System") {
        //网友不存在时重建网友。
        AllocAgentForUserHandler(privateToken, alias, message);
        return false;
    }

    //有网友新消息，更新临控信息行状态图标
    if (uid != "System") {
        //doFlashTitle();
        //NewMessageFlicker();
    }

    //收到消息内容            
    var chatinfo = {
        UserId: uid, //网友ID,
        AgentID: '', //坐席ID
        name: $('#DialogL' + message.c.cs).text(), //网友别名
        rectime: message.ct, //收到消息时间
        sendtime: ChatStuff.parseLongDate(message.c.t),
        //rectime: getHoursMinute2(message.ct),
        CsId: message.c.cs,
        content: message.c.m //消息内容
    }

    //显示聊天记录
    CreateMyChatNew(chatinfo);

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
}

function TimeoutHandler(privateToken, alias) {
    //alert("timeout");
    //console.log('timeout');
    //document.getElementById("messages").innerHTML += "timeout<br/>";
}

//网友离开通知
function AgentLeaveHandler(privateToken, alias, message) {

    var csidT = message.cs;

    var chatinfo = {
        uid: message.f, //网友ID        
        CsId: csidT,
        name: $('#DialogL' + csidT).text(), //别名
        rectime: ChatStuff.parseLongDate(message.t), //收到消息时间
        content: "会话已结束或对方已离线." //消息内容
    }
    RemoveSingleWY(csidT);
    //显示聊天记录
    CreateMyChatNew(chatinfo);


}


//添加新聊天用户
function AddNewChat(userinfo) {

    GenerateChatLayer(userinfo);

    //如果页面中没有其他的客户时，显示此第一个客户,
    if ($('#DialogMain .dialogue:visible').length == 0) {
        ChatStuff.CsId = userinfo.CsId;
        ChatStuff.UserId = userinfo.UserId;
        ShowChatLayer(userinfo.CsId);
    }

}

//uid:网参ID，UserReferURL:访客来源,AllocID:会话标识
function GenerateChatLayer(userinfo) {

    var tCsId = userinfo.CsId;

    var DialogLeft = '<div id="DialogL' + tCsId + '" did="' + userinfo.UserId + '" CsId="' + tCsId + '"  ctp="' + userinfo.ctp + '"  ctn="' + userinfo.ctn + '"  mc="' + userinfo.mc + '" lgid="' + userinfo.lgid + '" isopen="0" class="queue"><i></i><h4>' + userinfo.mn + '</h4></div>';
    var DialogMid = CreateDialogDiv(userinfo);
    var DialogRight = CreateUserTable(userinfo);

    //默认按网友分配时间降序排列，如有新消息网友排在最前面
    $('#tabLeft').append($(DialogLeft));
    $('#DialogMain').append(DialogMid);
    $('#con_one_2').append(DialogRight);
}

//显示指定消息聊天层
function ShowChatLayer(CsId) {
    //显示左边
    $('#tabLeft .online').removeClass('online').addClass('queue');
    $('#DialogL' + CsId).removeClass('queue').removeClass("new_msg").addClass('online').show();
    if ($('#DialogL' + CsId + ' span.close').length == 0) {
        $('#DialogL' + CsId).removeClass('offline');
    } else {
        $('#DialogL' + CsId).addClass('offline');
    }
    /*
    if (!$('#DialogL' + CsId).hasClass('offline')) {
    $('#DialogL' + CsId).removeClass('queue').removeClass("new_msg").addClass('online').show();
    } else {
    $('#DialogL' + CsId).removeClass('queue').removeClass("new_msg").show();
    }
    */
    $('#DialogMain .dialogue').hide();
    $('#DialogM' + CsId).show();

    //显示常用用
    setTab('one', 2, 2);

    //  滚动置底部、
    var DivP$ = $('#DialogM' + CsId).find('.scroll_gd');
    if (DivP$.length > 0) {
        DivP$.scrollTop(DivP$[0].scrollHeight);
    }


    //滚动条默认在最下边
    //document.getElementById("mychat" + uid).scrollTop = document.getElementById("mychat" + uid).scrollHeight;
}


function RemoveSingleWY(csid) {
    var DL$ = $('#DialogL' + csid);
    if (DL$.length == 0) return false;

    if (!DL$.hasClass('offline')) {
        DL$.removeClass('queue').addClass('offline').append('<span class="close"><a href="#">关闭</a></span>');
    }
    $('#DialogM' + csid).find('.ask_t').attr("contenteditable", "false").end().find('.style_1').empty();
    $('#DialogM' + csid).find('.endmsg input:last-child').remove();
    return false;
}

function DisableAllWY() {
    var csidT = -1;
    $('#tabLeft > div').each(function (i, domE) {
        csidT = $(domE).attr("CsId");
        if (csidT) {
            RemoveSingleWY(csidT);
        }
    });


}


function CreateMyChatNew(chatinfo, self) {
    self = (self == null) ? "" : "2";
    var user$ = $('#DialogM' + chatinfo.CsId);
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
    html += "</div><div class='dhc" + self + "'>" + chatinfo.content + "</div>";
    html += "</div>";


    user$.find('.endmsg em').text(chatinfo.rectime);
    var DivP$ = user$.find('.scroll_gd');
    DivP$.append(html);

    if (ChatStuff.CsId == chatinfo.CsId) {
        //DivP$.scrollTop(DivP$[0].scrollHeight);
        DivP$[0].scrollTop = DivP$[0].scrollHeight;
    } else {
        $('#DialogL' + chatinfo.CsId).addClass("new_msg");
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
        RemoveSingleWY(DialogL$.attr("CsId"));
        return false;
    }
    //会话标识，每分配一次生成新标识
    var allocId = DialogL$.attr('CsId');

    //消息内容
    var message = HtmlReplacehaveImgA($("#DialogM" + ChatStuff.CsId).find('.ask_t').html());
    if (message.length > 480) {
        alert("最大数量不能超过500..");
        return false;
    }
    if (ChatStuff.UserId == "") {
        alert("消息接收人不能为空!");
        return;
    }
    if (message == "") {
        alert("消息内容不能为空!");
        return;
    }

    //格式化特殊字符
    //message = FormatSpecialCharacters(message);
    //清空发送消息文本框                         
    //$("#mymsg" + uid).val('');

    //消息发送人

    var parameters = {
        action: 'sendmessage',
        FromPrivateToken: ChatStuff.AgentId,
        SendToPrivateToken: ChatStuff.UserId,
        AllocID: escape(allocId),
        usertype: 1,
        message: message

    };

    SendMessageByPara(parameters);

}

//发送满意度消息
function SendSatisMsg() {

    var allocId = $('#DialogL' + ChatStuff.CsId).attr('CsId');
    if (ChatStuff.UserId == "") {
        alert("消息接收人不能为空!");
        return;
    }
    var parameters = {
        action: 'sendmessage',
        FromPrivateToken: ChatStuff.AgentId,
        SendToPrivateToken: ChatStuff.UserId,
        AllocID: escape(allocId),
        message: '<span style="color:#333333;">非常感谢您的使用，请</span><span style="text-decoration:underline;color:#3399FF; cursor:pointer;" onclick="addSatisfaction()">点击这里</span><span style="color:#333333;">对我的服务做出评价。</span>',
        usertype: 1,
        MessageType: 6
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
        CsId: parameters.AllocID,
        name: '您', //别名
        rectime: getSysTime(), //收到消息时间
        content: unescape(parameters.message), //消息内容
        //utcNow: Date.now()
        utcNow: (new Date()).valueOf()
    }

    //显示聊天记录
    CreateMyChatNew(chatinfo, 1);

    $.ajax({
        //async: false,
        type: "POST",
        url: "AjaxServers/Handler.ashx",
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
                if (r != null && r2.result == 'sendok') {//登录成功之后
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


function GetAgentState() {

    var parameters = {
        action: 'getagentsatetbyid',
        FromPrivateToken: ChatStuff.AgentId
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



//更改坐席状态  1:在线， 2：离线
function SetAgentState(state) {
    var parameters = {
        action: 'setagentstate',
        FromPrivateToken: ChatStuff.AgentId,
        AgentState: state
    };

    $.ajax({
        type: "POST",
        url: "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {
                $.jAlert("更改状态成功!");
                ChatStuff.ChangeAgentState(state);
            }
            else {
                $.jAlert('更改状态失败：' + r.result);

            }
        }
    });
}


//根据网友ID判断网友聊天层是否存在
function isExistChatLayer(CsId) {
    return $('#DialogM' + CsId).length > 0;
}


//获取系统时间
function getSysTime() {
    var now = new Date();
    var year = now.getYear();
    var month = now.getMonth();
    var day = now.getDate();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    return parseInt(year + 1900) + "/" + parseInt(month + 1) + "/" + day + " " + hours + ":" + minutes + ":" + seconds + "";
}


//初始化方法
function Init() {

    InitUploadify();
    SetBeforeunload(true, onbeforeunload_handler);


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

}

function onbeforeunload_handler() {
    var parameters = {
        action: 'closechat',
        FromPrivateToken: ChatStuff.AgentId
    };

    $.ajax({
        type: "POST",
        url: "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {//登录成功之后                            
                //$.jAlert("退出成功");
                SetBeforeunload(false, onbeforeunload_handler);
                //window.location="Login.aspx";
            }
            else {
                $.jAlert('退出失败：' + r.result);
                SetBeforeunload(false, onbeforeunload_handler);
                //window.location = "Login.aspx";
            }
        }
    });
}


function CreateDialogDiv(userinfo) {
    var t = [];

    t.push('<div class="dialogue" style="display:none;" id="DialogM' + userinfo.CsId + '" >');
    t.push('<p>对话开始于: ' + userinfo.cst + '</p>');

    t.push('<div class="scroll_gd"></div>');
    t.push('<div class="editC">');
    t.push('<div class="style_1">');
    t.push('<span class="kind_detail"><a href="#" title="表情" class="fw_bq"></a><a href="#" title="历史记录"class="fw_jl"></a>');
    t.push('<a href="#" title="上传文件" class="fw_sc"></a><a href="#" title="截屏"class="fw_jp"></a><a href="#" title="满意度" class="fw_my"></a></span>');
    t.push('</div>');
    t.push('<div class="ask_c"><div class="ask_t"  contenteditable="true" ></div></div>');
    t.push('</div>');
    t.push('<div class="endmsg">(发送快捷键：Enter)最后收到消息时间：<em>10:00:09</em><span class="right btn" style="*margin-top: -20px;"><input type="button" value="结束对话" title="结束对话" class="w80 gray endBtn" /><input type="button" value="新增工单" style="margin:0 2px;"  title="新增工单" class="w80 gray" /><input type="button" value="发送"  title="发送" class="w60" /></span></div>');
    t.push('</div>');
    return t.join('');
}

function CreateUserTable(userinfo) {
    var t = [];
    if (userinfo.lmt == "1900/01/01 00:00:00") { userinfo.lmt = ""; }
    if (userinfo.lct == "1900/01/01 00:00:00") { userinfo.lct = ""; }
    t.push('<table id="DialogR' + userinfo.CsId + '"  border="0" cellspacing="0" cellpadding="0" style="display:none;">');
    t.push('<tr><th width="48%">经销商名称：</th><td width="50%">' + userinfo.mn + '</td></tr>');
    t.push('<tr><th>地理位置：</th><td>' + userinfo.adr + '</td></tr>');
    t.push('<tr><th>所属城市群：</th><td>' + userinfo.cgn + '</td></tr>');
    t.push('<tr><th>姓名：</th><td>' + userinfo.ctn + '</td></tr>');
    t.push('<tr><th>职务：</th><td>' + userinfo.ctj + '</td></tr>');
    t.push('<tr><th>连接时间：</th><td class="ct">' + userinfo.cst + '</td></tr>');
    t.push('<tr><th>上次最后消息时间：</th><td>' + userinfo.lmt + '</td></tr>');
    t.push('<tr><th>上次访问时间：</th><td>' + userinfo.lct + '</td></tr>');
    t.push('<tr><th>连接次数：</th><td>' + userinfo.dst + '</td></tr>');
    t.push('<tr><th>最近访问页面：</th><td>' + userinfo.uft + '</td></tr>');
    t.push('</table>');
    return t.join(' ');
}

//浏览器标题新消息闪烁
function NewMessageFlicker() {
    messageFlicker.clear();
    messageFlicker.show();
}


function BindUserListClick() {
    $('#tabLeft').click(function (eve) {

        var t = $(eve.target).closest('div');
        var idT = t[0].id;
        if (idT == "tabLeft") return false;
        //idT = idT.substr(7);
        var csidT = $(t).attr('CsId');


        if (eve.target.nodeName == "A") {
            eve.stopPropagation();
            eve.preventDefault();
            $('#DialogL' + csidT).remove();
            $('#DialogM' + csidT).remove();
            $('#DialogR' + csidT).remove();

            ChatStuff.CsId = -1;
            ChatStuff.UserId = -1;
            return false;
        }
        ChatStuff.CsId = csidT;
        ChatStuff.UserId = $(t).attr('did');
        ShowChatLayer(ChatStuff.CsId);

        return true;
    });
}


function BindDiaglogMainEvent() {
    $('#DialogMain').bind('click', function (eve) {
        //if (eve.target.title == "") return false;
        switch (eve.target.title) {
            case "表情":
                BiaoqingClick();
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
            case "新增工单":
                BtnAddWorkClick(); break;
            case "发送":
                BtnSendClick(); break;
            default:
                break;

        }
    });



    $("#DialogMain").bind("keydown", function (eve) {
        //SendMessage();
        if (eve.keyCode == 13) {
            eve.stopPropagation();
            eve.preventDefault();
            SendMessage();
            return false;
        }
        return true;
    });

}

function BiaoqingClick() {
    //$(this).toggle(function () {
    //    setEmotionTab('tabEmotion', 2, 3);
    //    $(".bq_list").css("display", "block");
    //}, function () {
    //    $(".bq_list").css("display", "none");
    //});
    BtnBiaoQing();
}



function HistoryClick() {
    var curr$ = $('#DialogL' + ChatStuff.CsId);
    var urlT = "ConversationHistory.aspx?LoginID=" + curr$.attr("lgid") + "&UserID=" + ChatStuff.AgentId;
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
        $('#DialogL' + ChatStuff.CsId).remove();
        $('#DialogM' + ChatStuff.CsId).remove();
        $('#DialogR' + ChatStuff.CsId).remove();
        $.jAlert("已结束");
        ChatStuff.CsId = -1;
        return;
    }

    if ($.jConfirm("您确定要结束对话么？", function (r) {
        if (r) {
            CloseSingleUser();
        }

    }));

}


//创建工单
function BtnAddWorkClick() {
    CheckOrderComplete();
}

//发送消息
function BtnSendClick() {
    SendMessage();
}

function CloseSingleUser() {
    var parameters = {
        action: 'closesinglechat',
        FromPrivateToken: ChatStuff.AgentId,
        SendToPrivateToken: ChatStuff.UserId
    };

    $.ajax({
        type: "POST",
        url: "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {//登录成功之后                            
                $.jAlert("已结束");
                $('#DialogL' + ChatStuff.CsId).remove();
                $('#DialogM' + ChatStuff.CsId).remove();
                $('#DialogR' + ChatStuff.CsId).remove();
            }
            else {
                $.jAlert('请求失败，请稍后重试：' + r.result);

            }
        }
    });
}


//系统启动
$(function () {
    Init();
    $('#btnTest').click(function () {

        ChatStuff.i++;
        var userinfo = {
            UserId: ChatStuff.i,
            M: ChatStuff.m + ChatStuff.i,
            CsId: ChatStuff.i,
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

});

//登录系统
function Login(isOnLine) {
    var parameters = {
        action: 'init',
        FromPrivateToken: ChatStuff.AgentId,
        usertype: '1'
    };

    $.ajax({
        type: "POST",
        url: "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            ChatStuff.isLeaveNotice = false;
            if (msg == null || msg == "") {
                alert('登录失败：请检测您的网络连接或者重新登录.');
                return;
            }
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'loginok') {//登录成功之后
                defaultChannel = null;
                StartLongConnect();
                StartTimer();
                //掉线登录后直接设置为在线
                if (isOnLine) {
                    SetAgentState(1);
                }
            }
            else {
                $.jAlert('登录消息：' + r.result, function () {
                    SetBeforeunload(false, onbeforeunload_handler);
                    //window.location = "Login.aspx";
                    //alert("登录失败");
                    //CloseWindow();


                });

            }

        }
    });
}

function CloseWindow(parameters) {

    //SetBeforeunload(false, onbeforeunload_handler);
    if ($.browser.msie) {
        window.opener = null;
        window.open('', '_self');
        window.close();
    } else if ($.browser.webkit) {
        //open("http://www.baidu.com/", '_self').close();
        //open(location, '_self').close();
        $(document).empty();
        window.opener = null;
        window.open('', '_self');
        window.close();
    }


}

//退出系统
function Quit() {
    var parameters = {
        action: 'closechat',
        FromPrivateToken: ChatStuff.AgentId
    };

    $.ajax({
        type: "POST",
        url: "AjaxServers/Handler.ashx",
        data: parameters,
        success: function (msg) {
            var r = $.evalJSON(msg);
            if (r != null && r.result == 'sendok') {//登录成功之后                            
                $.jAlert("已经退出登录");
                SetBeforeunload(false, onbeforeunload_handler);
                //ChatStuff.ChangeAgentState(2);
                //window.location = "Login.aspx";
                // alert("Login");
            }
            else {
                $.jAlert('退出失败：' + r.result);
                SetBeforeunload(false, onbeforeunload_handler);
                //window.location = "Login.aspx";

            }
        }
    });


    //更改所有网友状态
    DisableAllWY();
    StopTimer();
}

///针对类型///Date(-2209017600000+0800)/
ChatStuff.parseLongDate = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    }
    else {
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
}

///针对类型///Date(-2209017600000+0800)/
ChatStuff.parseShortDate = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    }
    else {
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
}

//针对 2014/11/5 14:15:52
ChatStuff.parseTime = function (strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    }
    else {
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
}

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
        var imgstr = "<img src='" + filepath + "'/>";

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
        'fileTypeDesc': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
        'fileTypeExts': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
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
                        action: 'sendmessage', FromPrivateToken: ChatStuff.AgentId,
                        usertype: 1, message: escape(tMsg),
                        SendToPrivateToken: ChatStuff.UserId, AllocID: $('#DialogL' + ChatStuff.CsId).attr("CsId"),
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


function setTabT(name, cursel, n) {
    for (i = 1; i <= n; i++) {
        var menu = document.getElementById(name + i);
        var con = document.getElementById("con_" + name + "_" + i);
        menu.className = i == cursel ? "hover" : "";
        con.style.display = i == cursel ? "block" : "none";
    }
    if (name == "one" && n == 3) {
        getEmotionInfo(cursel);
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
    $('#tabLeft > div[CsId]').each(function (i, e) {
        if (!$(e).hasClass('offline')) {
            cslst.push($(e).attr('CsId'));
        }
    });


    cslst = cslst.length == 0 ? "" : cslst.join(',');

    $.ajax({
        url: 'AjaxServers/Handler.ashx',
        type: "post",
        data: { action: "checkstate", csids: cslst, FromPrivateToken: ChatStuff.AgentId },
        timeout: 9000,
        cache: false,
        dataType: "json",
        complete: function (xhr, TS) {
            ////启动计时
            ChatStuff.checkStateTimer = setTimeout(CheckDialogState, ChatStuff.checkStateDuration);

            ////断网时计时判断是否超过指定时间
            if (xhr.status == 0) {
                CheckTimeOut();
            }
            else {
                ChatStuff.timeOutStart = new Date();
            }

        },
        success: function (dataT, textStatusT) {

            if (dataT && dataT.astatus == "0") {
                ////状态设置为离线                
                LeaveNetOverTimeNotice();
            }
            if (dataT && dataT.result == "sendok") {
                $.each(dataT.data, function (n, val) {

                    if (val.state == 0 && !$('#DialogL' + val.csid).hasClass('offline')) {

                        var chatinfo = {
                            CsId: val.csid, //网友ID,        
                            name: $('#DialogL' + val.csid + ' h4').text(), //网友别名        
                            rectime: new Date(),
                            content: "已离线" //消息内容
                        }
                        CreateMyChatNew(chatinfo);

                        $('#DialogL' + val.csid).removeClass('queue').removeClass('online').addClass('offline').append('<span class="close"><a href="#">关闭</a></span>');
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
    var csidT = ChatStuff.CsId
    $.get('AjaxServers/Handler.ashx', { action: "checkorder", csid: csidT }, function (a, b, c) {

        if (a != "") {
            var s = $.parseJSON(a);
            if (s.result == "sendok" && s.data == "1") {
                alert("您已经创建过了工单，不能重复创建.");
                return;
            }
        }

        var urlT = [], curr$ = $('#DialogL' + csidT);

        if (curr$.length == 0) return false;

        urlT.push(ChatStuff.WorkOrderUrl);
        urlT.push("?IsClientOpen=1&SYSType=isIM&ctype=3&CalledNum=");
        urlT.push(escape(curr$.attr("ctp")));
        urlT.push("&CustName=");
        urlT.push(escape(curr$.attr("ctn")));
        urlT.push("&MemberCode=");
        urlT.push(escape(curr$.attr("mc")));
        urlT.push("&MemberName=");
        urlT.push(escape($.trim(curr$.find('h4').text())));
        urlT.push("&CSID=");
        urlT.push(escape(curr$.attr("CsId")));
        urlT.push("&in=" + Math.random());
        //window.open(urlT.join(''));
        $('#aNewOrder').attr('href', urlT.join(''));
        //$('#spanOrder').click();
        document.getElementById('aNewOrder').click();
    });
}