//请求进入队列
function CominQuene(LoginID, SourceType) {
    LeaveMessage = "0";
    var pody = { action: 'cominquene', FromPrivateToken: escape(LoginID), SourceType: escape(SourceType) };
    AjaxPostAsync(ONS.HdlUrl, pody, null,
             function (msg) {
                 var r1 = JSON.parse(msg);
                 //alert(r1.result);
                 if (r1 != null && r1.result == '0') {//登录成功之后
                     defaultChannel = null;
                     Connect();
                 }
                 else if (r1 != null && r1.result == '1') {
                     //0:成功;1:参数错误，未找到业务线，2：等待队列已满
                     $("#divagentAllocat").html("<div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>您好，欢迎使用在线客服系统。参数错误，未找到业务线，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     //                     $("#divagentAllocat").css("display", "");
                     //                     $("#Rmessages").css("height", "85%");
                 }
                 else if (r1 != null && r1.result == '2') {
                     $("#divagentAllocat").html("<div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>您好，欢迎使用在线客服系统。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     //$("#divagentAllocat").css("display", "");
                     //$("#Rmessages").css("height", "85%");
                 }
             });
}

function GetQuick() {
    var quickvalue = GetCookie("quickvalue");
    //快捷键从cookie读取
    if (quickvalue != null && quickvalue != "") {
        if (quickvalue == "1") {
            $("#radenter").attr("checked", true);
        }
        else {
            $("#radctrlenter").attr("checked", true);
        }
    }
    else {
        $("#radenter").attr("checked", true);
    }
}
function setbigsmall() {
    //    $('#bodyDIV').css('height', (($(window).height() - 20) + 'px'));
    //$('#divcontent').css('height', (($(window).height() - 20 - 33 - 20) + 'px'));
    $('#divagentAllocat').css('height', (($(window).height() - 45 - 150 - 60 - 10) + 'px'));
    document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;
}
//页面大小关闭时触发，让页面始终适合页面大小
function ChangeBigSmall() {
    setbigsmall();
}
//刷新关闭执行方法
function onbeforeunload_handler() {
    var sendto = $('#hidto').val();
    var pody = { action: 'userclosechat', FromPrivateToken: escape(privatetaken), SendToPublicToken: escape(sendto) };
    AjaxPostAsync(ONS.HdlUrl, pody, null,
             function (msg) {
                 var r = JSON.parse(msg);
                 if (r != null && r.result == 'sendok') {//登录成功之后
                 }
             });
}




//客服发起满意度评价消息
function SatisfactionHandLer(privateToken, alias, message) {
    messageFlicker.clear();
    messageFlicker.show();
    document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/xiaoxin.png'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！")
    //"<p class='hs'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！</p>";
    document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;
}

////发送排队信息消息
function QueueOrderHandLer(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("");

    document.getElementById("divagentAllocat").innerHTML += "<div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>您好，欢迎使用易车在线客服系统。</div>";
    document.getElementById("divagentAllocat").innerHTML += "<div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>" + message.m + "如您不想继续等待，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>！</div>";
    //$("#divagentAllocat").css("display", "");
    //$("#Rmessages").css("height", "85%");
}

////转接消息
function TransferHandLer(privateToken, alias, message) {
    Chatdisabled(1);
    messageFlicker.clear();
    //坐席标识
    $('#hidto').val(message.a);
    number = message.anum;
    //坐席分配标识
    $('#hidAllocID').val(message.cs);
    isManyi = false;
    var AllocID = $('#hidAllocID').val();
    document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！");
    document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/an_dd.png'>请稍候，正在为您转接指定客服。");
    document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入会话。");
    var rectime = parseTime(message.cst);
    document.getElementById("divagentAllocat").innerHTML += zs("您好，易车客服" + number + "号很高兴为您服务！");
    //zs("您好，易车客服" + message.anum + "号很高兴为您服务！");

    messageFlicker.show();
}


//分配坐席
function AllocAgentForUserHandler(privateToken, alias, message) {
    //把坐席标识放在隐藏域里
    setTimeout(function () {

        messageFlicker.clear();
        messageFlicker.show();
        Chatdisabled(1);
        $("#hidto").val(message.a);
        number = message.anum;
        //$("#divagentNo").html("易车网服务顾问 " + message.anum);
        //把分配坐席标识
        $("#hidAllocID").val(message.cs);
        GetHMessageByAgent(privateToken, message.a);
        //分配后就是可以重新做满意度评价
        isManyi = false;
        //$("#Rmessages").css("height", "100%");
        $("#divagentAllocat").html("");
        //$("#divagentAllocat").hide();
        //$("#Rmessages").html("");
        document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入对话。");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'> 您好，易车网运营服务中心竭诚为您服务，已为您接通专属服务顾问" + message.anum + "，请问有什么可以帮您？</div></div></div><div class='clearfix'></div>";
        //让信息框和发送信息按钮可用
        var rectime = parseTime(message.cst);
        document.getElementById("divagentAllocat").innerHTML += "<div class='duihua_time'>" + GetDateNowForLong() + "</div>";
        document.getElementById("divagentAllocat").innerHTML += zs("您好，易车客服" + number + "号很高兴为您服务！");

    }, 800);


}
function alloc() {

}
//坐席全忙，去留言
function addMessage() {
    $.openPopupLayer({
        name: "AddOnlineMessageAjaxPopup",
        parameters: { VisitID: visitid },
        url: "/mOnLineMessageForm.aspx?r=" + Math.random()
    });

}
//坐席全忙
function MAllBussyHandler(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("<div class='paidui'><img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
    //$("#divagentAllocat").css("display", "");
    //$("#Rmessages").css("height", "85%");
}
//成功接收到聊天信息事件
function SuccessHandler(privateToken, alias, message) {
    //    if ($("#divagentAllocat").html() != "") {
    //        $("#divagentAllocat").html("");
    //        $("#divagentAllocat").css("display", "none");
    //        $("#Rmessages").css("height", "100%");
    //    }
    if (message.n == "Be Killed") {
        return false;
    }
    messageFlicker.clear();
    var rectime = getHoursMinute2(message.ct);
    //message.c.m = replaceRegUrl(message.c.m);
    document.getElementById("divagentAllocat").innerHTML += zs(message.c.m);
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_name'>客服" + number + "<span>【" + rectime + "】</span></div><div class='user_con'>" + message.c.m + "</div></div></div><div class='clearfix'></div>";


    document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;
    messageFlicker.show();
}
//错误通知
function FailureHandler(privateToken, alias, error) {
    messageFlicker.clear();
    Chatdisabled(0);
    messageFlicker.show();
    //    $("#divagentAllocat").html("");
    //    $("#divagentAllocat").hide();
    //如果已收到离线消息，则不再提示
    if (LeaveMessage == "0") {
        //长连接错误，但坐席没有离开
        if ($('#hidto').val() != "") {
            document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>");
            // "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
        }
        //长连接错误，但是没有分配坐席
        else if ($('#hidto').val() == "") {
            document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>排队中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续排队</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束排队</a>");
        }
    }
    document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;
}
//坐席离开通知
function AgentLeaveHandler(privateToken, alias, message) {
    $('#hidto').val("");

    if (LeaveMessage == "0") {
        LeaveMessage = "1";
        messageFlicker.clear();
        document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>");
        document.getElementById('divagentAllocat').scrollTop = document.getElementById('Rmessages').scrollHeight;
        Chatdisabled(0);
        messageFlicker.show();
    }
}
//长连接超时通知
function TimeoutHandler(privateToken, alias, message) {
    GetServerTime(new Date(parseFloat(message.ct)));
}


//发送消息，sendtype=7是文件,不传是文本
function SendMessage(MessageType, FileName, FileType, FileSize, FilePath) {
    //消息接收者
    var sendto = $('#hidto').val();
    //坐席分配标识
    var AllocID = $('#hidAllocID').val();
    //var issuccess=0;
    if ($("#txtkeywordbottom a").length == 0 && $("#txtkeywordbottom img").length == 0) {
        var str = UpdateHttp($('#txtkeywordbottom').html());
        $('#txtkeywordbottom').html(str);
    }
    //把超链接设置成新窗口打开
    $("#txtkeywordbottom a").attr('target', '_blank');

    var messagetxt = $('#txtkeywordbottom').html();
    messagetxt = $.trim(messagetxt);

    //替换html,保留换行，img,a标签
    messagetxt = HtmlReplacehaveImgA(messagetxt);
    messagetxt = $.trim(messagetxt);
    if (sendto == "") {
        document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
    }
    else {
        if (MessageType == "6") {
            messagetxt = "网友已做满意度评价！";
        }
        //当通知网友已做满意度评价消息，消息信息为空
        if (messagetxt == "" || messagetxt == "<br>") {
            alert("消息不能为空！");

        }
        else if (messagetxt == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
            alert("消息不能为空！");

        }
        else if (Len(messagetxt) > 500) {
            alert("文本内容太长，请整理后重新发送！");
        }
        else {
            var sendmessage = messagetxt;
            $('#txtkeywordbottom').html("");
            //发送文件
            var pody;
            if (MessageType == "7") {
                pody = { action: 'sendmessage', FromPrivateToken: escape(privatetaken), usertype: escape('2'), message: escape(sendmessage), SendToPrivateToken: escape(sendto), AllocID: escape(AllocID), MessageType: escape(MessageType), FileName: escape(FileName), FileType: escape(FileType), FileSize: escape(FileSize), FilePath: escape(FilePath) };
            }
            else {
                pody = { action: 'sendmessage', FromPrivateToken: escape(privatetaken), usertype: escape('2'), message: escape(sendmessage), SendToPrivateToken: escape(sendto), AllocID: escape(AllocID), MessageType: escape(MessageType) };
            }
            //var idNow = Date.now();
            var idNow = (new Date()).valueOf();
            if (MessageType != "6") {
                document.getElementById("divagentAllocat").innerHTML += wts(sendmessage, "", "");
            }

            //"<div class='dh2' id='" + idNow + "'><div class='title title2'>您 <span>：" + GetDateNow() + "</span><em class='embg'></em></div><div class='dhc dhc2'>" + sendmessage + "</div></div>";
            document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;
            AjaxPost(ONS.HdlUrl, pody, null,
             function (msg) {
                 if (msg != "") {
                     var r = JSON.parse(msg);
                     //消息发送成功
                     if (r != null && r.result != "SendToLeave" && r.result != "ClientNotExists") {//登录成功之后
                         var r2 = $.evalJSON(r.result);
                         if (r2.result == 'sendok') {
                             //var dtime = getHoursMinute2(r2.rectime);
                             //sendmessage = replaceRegUrl(sendmessage);
                             // document.getElementById("Rmessages").innerHTML += "<div class='dh2'><div class='title title2'>您 <span>：" + dtime + "</span><em class='embg'></em></div><div class='dhc dhc2'>" + sendmessage + "</div></div>";                             
                             $('#' + idNow + ' .title2 em').removeClass('embg').html("");
                         }
                         //else {
                         //    $('#' + idNow + ' .title2 em').removeClass('embg').html("发送消息失败...");
                         //}
                     }
                     //坐席离线
                     else if (r != null && r.result == 'SendToLeave') {
                         document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";

                     }
                     //网友客户端实体不存在
                     else if (r != null && r.result == 'ClientNotExists') {
                         document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<p class='hs'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></p>";
                     }

                 }
                 else {
                     document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，您的网络出现异常！")
                     //"<p class='hs'>消息: " + sendmessage + " 发送失败，您的网络出现异常！</p>";
                     alert("您的网络出现异常，请检查网络后重试！");
                 }
                 document.getElementById('divagentAllocat').scrollTop = document.getElementById('divagentAllocat').scrollHeight;

                 var IEbrowser = checkBrowser().split(":")[0];
                 if (IEbrowser == "Safari") {
                     EditDivFocus();
                 }
             });
        }
    }
}
//继续等待，分两种是1是需要初始化网友实体，并且重新建立长连接，0是长连接存在，只是重新排队
function RestAllocAgent(Reset) {
    number = "";
    document.getElementById("divagentAllocat").innerHTML = "";
    var sendto = $('#hidto').val();
    StartChat();
}



function EditDivFocus() {
    var s = window.getSelection(),
        r = document.createRange(),
        p = document.getElementById('Smessage');

    p.innerHTML = '\u00a0';
    r.selectNodeContents(p);
    s.removeAllRanges();
    s.addRange(r);
    document.execCommand('delete', false, null);
}

//文本框提示语
function ClearTitle() {
    if ($("#Smessage").html() == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
        $("#Smessage").html("");
    }
}
//窗口关闭按钮事件
function CloseWindow(ConfirmValue) {

    //    if (ConfirmValue == 0) {
    ////        $.jConfirm("您确定要结束对话么？", function (r) {
    ////            if (r) {
    //                SetBeforeunload(false, onbeforeunload_handler);
    //                onbeforeunload_handler();
    //                window.opener = null; window.open('', '_self'); window.close();
    ////            }

    ////        });
    //    } else {
    SetBeforeunload(false, onbeforeunload_handler);
    onbeforeunload_handler();
    window.opener = null; window.open('', '_self'); window.close();
    //    }
}


//根据客户ID与坐席ID，取近六条聊天记录
function GetHMessageByAgent(privatetaken, agentid) {

}


//取历史聊天记录
function HistroyMore() {
    var userid = $("#hidto").val();
    window.open("ConversationHistory.aspx?LoginID=" + escape(loginid) + "&r=" + Math.random());
}
//满意度弹层
function addSatisfaction() {
    if (isManyi) {
        alert("您已对客服进行过评价，请勿重复提交~");
    }
    else {
        var csid = $("#hidAllocID").val();
        $.openPopupLayer({
            name: "AddSatisfactionAjaxPopup",
            parameters: { CSID: csid },
            url: "/mSatisfactionForm.aspx?r=" + Math.random(),
            beforeClose: function (n) {
                if (n == true) {
                    isManyi = true;
                    //$("#EMYiChe").unbind("click");
                    //发送满意度消息给客服
                    SendMessage("6", "", "", "", "");

                    document.getElementById("divagentAllocat").innerHTML += js("<img width='16' height='16' src='images/taiyang.png'>非常感谢您对我的服务做出评价，祝您生活愉快。^_^");
                }

            }
        });
    }
}
//设置按钮是否可用
function Chatdisabled(type) {
    if (type == 0) {
        $("#imgOnline").attr("src", "Mimages/offline-im.png");
        $("#bq_listSH").unbind('click', BQIMGClick);
        $("#divManyiDu").unbind('click', addSatisfaction);
        $("#txtkeywordbottom").attr("contenteditable", false);
        $("#btnSend").attr("disabled", "disabled");
    }
    else {
        $("#imgOnline").attr("src", "Mimages/online-im.png");
        $("#bq_listSH").bind('click', BQIMGClick);
        $("#divManyiDu").bind('click', addSatisfaction);
        $("#btnSend").removeAttr("disabled");
        $("#txtkeywordbottom").attr("contenteditable", true);
    }
}


function parseTime(strDate) {
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
function GetDateNow() {

    var myDate = GetServerTime();

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

///针对类型///Date(-2209017600000+0800)/
function parseLongDate(strDate) {
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

function GetDateNowForLong() {
    var myDate = GetServerTime();
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
    return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s;

}

function zts(z, t, s) {

    return "<div class='dh_user1' style='padding: 0px 10px;margin: 3px 0px;'><table  border='0'><tr><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td><td><div class='user_name'><span>客服" + z + "</span>" + t + "</div></td></tr><tr><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + s + "</div></div></td></tr></table></div><div class='clearfix'></div>";
}
function zs(msg) {
    return "<div class='dh_user1' style='padding: 0px 10px;'><table  border='0'><tr><td class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + msg + "</div></div></td></tr></table></div><div class='clearfix'></div>";
    //return "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>" + msg + "</div></div></div><div class='clearfix'></div>";
}
function wts(w, t, s) {
    return "<div class='dh_user1 dh_user2' style='padding: 0px 10px; float:right; margin:0;'><table  border='0'><tr><td><div class='im-con'><div class='user_con'>" + w + "<div class='arrow2'></div></div></div></td><td><div class='user_img user_img2'><img src='images/user-im.png' /></div></td> </tr></table></div><div class='clear'></div>"
}
function js(msg) {
    return "<div class='paidui' >" + msg + "</div><div class='clearfix'></div>";
}


function BQIMGClick(eve) {
    eve.stopPropagation();
    eve.preventDefault();
    if (BQPop == null) {
        $('.m-search').BQPop({ left: 10, bottom: 150, isAgent: false, isWap: true });
        BQPop = $('#BQMain');
    }
    BQPop.toggle();
    return false;
}
//表情按钮：
function BQClick(url, e) {

    $('#txtkeywordbottom').append('<img src="' + url + '" />').focus();
    BQPop.hide();
    $("#txtkeywordbottom").focus();
    moveEnd("txtkeywordbottom");
}