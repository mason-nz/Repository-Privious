//请求进入队列
function CominQuene(LoginID, SourceType) {
    var pody = { action: 'cominquene', FromPrivateToken: escape(LoginID), SourceType: escape(SourceType) };
    AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r1 = JSON.parse(msg);
                 //alert(r1.result);
                 if (r1 != null && r1.result == '0') {//登录成功之后
                     //建立长连接
                     if (isleave == "0") {
                         defaultChannel = null;
                         Connect();
                         isleave = "1";
                     }
                 }
                 else if (r1 != null && r1.result == '1') {
                     //0:成功;1:参数错误，未找到业务线，2：等待队列已满
                     $("#divagentAllocat").html("<div class='an_info'><img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统。参数错误，未找到业务线，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     $("#divagentAllocat").css("display", "");
                     $("#Rmessages").css("height", "85%");
                 }
                 else if (r1 != null && r1.result == '2') {
                     $("#divagentAllocat").html("<div class='an_info'><img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     $("#divagentAllocat").css("display", "");
                     $("#Rmessages").css("height", "85%");
                 }
             });
}
function InitUploadify() {
    var uploadSuccess = true;
    $("#uploadify").uploadify({
        'buttonText': '选择',
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
        'fileTypeDesc': '*.jpg;*.gif;*.png',
        'fileTypeExts': '*.jpg;*.gif;*.png',
        'width': 10,
        'height': 26,
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
                    //var filename = unescape(jsonData.FileName);
                    var filename = jsonData.FileName;
                    var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + filepath;
                    $('#Smessage').html("<a href=\"" + hostT + "\" class=\"upfile\" target=\"_blank\">下载文件：" + filename + "</a>");
                    SendMessage("7", filename, jsonData.ExtendName, jsonData.FileSize, filepath);
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
        $("#radctrlenter").attr("checked", true);
    }
}
function setbigsmall() {
    //    $('#bodyDIV').css('height', (($(window).height() - 20) + 'px'));
    $('#divcontent').css('height', (($(window).height() - 20 - 33 - 20) + 'px'));
    //    //            //指定左边高度
    //   $('#divleft').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
    //    //            //指定右面高度
    //    $('#divright').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
    if (SourceType != "2") {
        $('#con_one_1').css('height', (($(window).height() - 20 - 33 - 20 - 40) + 'px'));
    }
}
//给cookie设置网友所选快捷键，以便下次访问不要再重新设置快捷键
function SetQuick(quickvalue) {
    SetCookie("quickvalue", quickvalue);
}
//页面大小关闭时触发，让页面始终适合页面大小
function ChangeBigSmall() {
    setbigsmall();
}
//刷新关闭执行方法
function onbeforeunload_handler() {
    var sendto = $('#hidto').val();
    var pody = { action: 'userclosechat', FromPrivateToken: escape(privatetaken), SendToPublicToken: escape(sendto) };
    AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r = JSON.parse(msg);
                 if (r != null && r.result == 'sendok') {//登录成功之后
                 }
             });
}


//发送文件消息
function SendFileHandLer(privateToken, alias, message) {
    messageFlicker.clear();
    var rectime = parseLongDate(message.t);
    rectime = getHoursMinute2(rectime);
    //message.c.m = replaceRegUrl(message.c.m);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, message.m);
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_name'>客服" + number + "<span>【" + rectime + "】</span></div><div class='user_con'>" + message.m + "</div></div></div><div class='clearfix'></div>";

    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    messageFlicker.show();
}

//客服发起满意度评价消息
function SatisfactionHandLer(privateToken, alias, message) {
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/xiaoxin.png'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！")
    //"<p class='hs'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！</p>";
}

////发送排队信息消息
function QueueOrderHandLer(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("");
    document.getElementById("divagentAllocat").innerHTML += "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服系统。</div></div>";
    document.getElementById("divagentAllocat").innerHTML += "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_dd.png'>" + message.m + "如您不想继续等待，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>！</div></div>"
    $("#divagentAllocat").css("display", "");
    $("#Rmessages").css("height", "85%");
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
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！");
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_dd.png'>请稍候，正在为您转接指定客服。");
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入会话。");
    var rectime = parseTime(message.cst);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, "您好，易车客服" + number + "号很高兴为您服务！");
    //zs("您好，易车客服" + message.anum + "号很高兴为您服务！");

    messageFlicker.show();
}

//继续排队
function ContinueQueue() {

    $.jConfirm("您确定要继续排队吗？", function (r) {
        if (r) {
            $("#divagentAllocat").html("");
            setTimeout(function () {
                var pody = { action: 'resetagent', FromPrivateToken: escape(privatetaken) };
                AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
                                             function (msg) {
                                                 if (msg != "") {
                                                     var m = JSON.parse(msg);
                                                     if (m != null && m.result == 'Initializeok') {//登录成功之后
                                                         //StartChat();
                                                     }
                                                     else {

                                                     }
                                                 }
                                             });
            }, 1000);
        }
    });
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
        $("#Rmessages").css("height", "100%");
        $("#divagentAllocat").html("");
        $("#divagentAllocat").hide();
        //$("#Rmessages").html("");
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入对话。");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'> 您好，易车网运营服务中心竭诚为您服务，已为您接通专属服务顾问" + message.anum + "，请问有什么可以帮您？</div></div></div><div class='clearfix'></div>";
        //让信息框和发送信息按钮可用
        var rectime = parseTime(message.cst);
        document.getElementById("Rmessages").innerHTML += zts(number, rectime, "您好，易车客服" + number + "号很高兴为您服务！");

    }, 500);


}
function alloc() {

}
//坐席全忙，去留言
function addMessage() {
    $.openPopupLayer({
        name: "AddOnlineMessageAjaxPopup",
        parameters: { VisitID: visitid },
        url: "/OnLineMessageForm.aspx?r=" + Math.random()
    });

}
//坐席全忙
function MAllBussyHandler(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("<img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>");
    $("#divagentAllocat").css("display", "");
    $("#Rmessages").css("height", "85%");
}
//成功接收到聊天信息事件
function SuccessHandler(privateToken, alias, message) {
    if ($("#divagentAllocat").html() != "") {
        $("#divagentAllocat").html("");
        $("#divagentAllocat").css("display", "none");
        $("#Rmessages").css("height", "100%");
    }
    if (message.n == "Be Killed") {
        return false;
    }
    messageFlicker.clear();
    var rectime = getHoursMinute2(message.ct);
    //message.c.m = replaceRegUrl(message.c.m);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, message.c.m);
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_name'>客服" + number + "<span>【" + rectime + "】</span></div><div class='user_con'>" + message.c.m + "</div></div></div><div class='clearfix'></div>";


    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    messageFlicker.show();
}
//错误通知
function FailureHandler(privateToken, alias, error) {
    messageFlicker.clear();
    Chatdisabled(0);
    messageFlicker.show();
    //长连接断开
    isleave = "0";
    $("#divagentAllocat").html("");
    $("#divagentAllocat").hide();
    //长连接错误，但坐席没有离开
    if ($('#hidto').val() != "") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
        // "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
    }
    //长连接错误，但是没有分配坐席
    else if ($('#hidto').val() == "") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>排队中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续排队</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束排队</a>")
    }
    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
}
//坐席离开通知
function AgentLeaveHandler(privateToken, alias, message) {
    $('#hidto').val("");

    messageFlicker.clear();
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";


    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    Chatdisabled(0);
    messageFlicker.show();
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
    //把超链接设置成新窗口打开
    $("#Smessage a").attr('target', '_blank');

    var messagetxt = $('#Smessage').html();
    messagetxt = $.trim(messagetxt);

    //替换html,保留换行，img,a标签
    messagetxt = HtmlReplacehaveImgA(messagetxt);
    messagetxt = $.trim(messagetxt);
    if (sendto == "") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
    }
    else {
        if (MessageType == "6") {
            messagetxt = "网友已做满意度评价！";
        }
        //当通知网友已做满意度评价消息，消息信息为空
        if (messagetxt == "" || messagetxt == "<br>") {
            $.jAlert("消息不能为空！");

        }
        else if (messagetxt == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
            $.jAlert("消息不能为空！");

        }
        else if (Len(messagetxt) > 500) {
            $.jAlert("文本内容太长，请整理后重新发送！");
        }
        else {
            var sendmessage = messagetxt;
            $('#Smessage').html("");
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
                document.getElementById("Rmessages").innerHTML += wts("您", GetDateNow(), sendmessage);
            }

            //"<div class='dh2' id='" + idNow + "'><div class='title title2'>您 <span>：" + GetDateNow() + "</span><em class='embg'></em></div><div class='dhc dhc2'>" + sendmessage + "</div></div>";
            document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
            AjaxPost('AjaxServers/Handler.ashx', pody, null,
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
                         document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";

                     }
                     //网友客户端实体不存在
                     else if (r != null && r.result == 'ClientNotExists') {
                         document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<p class='hs'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></p>";
                     }

                 }
                 else {
                     document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，您的网络出现异常！")
                     //"<p class='hs'>消息: " + sendmessage + " 发送失败，您的网络出现异常！</p>";
                     $.jAlert("您的网络出现异常，请检查网络后重试！");
                 }
                 document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
             });
        }
    }
}
//继续等待，分两种是1是需要初始化网友实体，并且重新建立长连接，0是长连接存在，只是重新排队
function RestAllocAgent(Reset) {
    //$("#divagentNo").html("易车网服务顾问");
    number = "";
    document.getElementById("Rmessages").innerHTML = "";
    var sendto = $('#hidto').val();
    if (isleave == "1") {
        //        var pody = { action: 'userclosechat', FromPrivateToken: escape(privatetaken), SendToPublicToken: escape(sendto) };
        //        AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
        //             function (msg) {
        //                 var r = JSON.parse(msg);
        //                 if (r != null && r.result == 'sendok') {//登录成功之后
        //                     StartChat();
        //                 }
        //             });
        CominQuene(privatetaken, SourceType);
    }
    else {
        StartChat();
    }



    //    if (Reset == '0') {
    //        var pody = { action: 'resetagent', FromPrivateToken: escape(privatetaken) };
    //        AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
    //                         function (msg) {
    //                             if (msg != "") {
    //                                 var r = JSON.parse(msg);
    //                                 if (r != null && r.result == 'Initializeok') {//登录成功之后
    //                                     StartChat();
    //                                 }
    //                                 else {

    //                                     CominQuene(privatetaken, SourceType);
    //                                 }
    //                             }
    //                         });
    //    } else {
    //        StartChat();
    //    }
}

//绑定发送消息快捷键
function onKeyPress(e) {
    var keyCode = null;
    if (e.which)
        keyCode = e.which;
    else if (e.keyCode)
        keyCode = e.keyCode;
    var quickvalue = "";
    $("input[name='RadioQuick']").each(function () {
        if ($(this).attr("checked")) {
            quickvalue = $(this).val();
        }
    });
    var IEbrowser = checkBrowser().split(":")[0];
    if (quickvalue == "1") {
        if (IEbrowser == "Firefox") {
            if (e.ctrlKey == false && keyCode == 13) {
                SendMessage();
                return false;
            }
            else if (e.ctrlKey && keyCode == 13) {
                document.getElementById("Smessage").innerHTML += "<br>";
                moveEnd("Smessage");
                return false;
            }
        }
        else {
            if (event.ctrlKey == false && keyCode == 13) {
                SendMessage();
                return false;
            }
            else if (event.ctrlKey && keyCode == 10) {
                if (IEbrowser == "Chrome") {
                    document.getElementById("Smessage").innerHTML += "<div><br /></div>";
                    moveEnd("Smessage");
                    return false;
                }
                else {
                    document.getElementById("Smessage").innerHTML += "<br>";
                    moveEnd("Smessage");
                    return false;
                }
            }
        }
    }
    else if (quickvalue == "2") {
        //如果是火狐浏览器
        if (navigator.userAgent.indexOf('Firefox') >= 0) {
            if (e.ctrlKey && keyCode == 13) {
                SendMessage();
                return false;
            }
        }
        else {
            if (event.ctrlKey && keyCode == 10) {
                SendMessage();
                return false;
            }
        }
    }

    return true;
}
//文本框提示语
function ClearTitle() {
    if ($("#Smessage").html() == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
        $("#Smessage").html("");
    }
}
//窗口关闭按钮事件
function CloseWindow(ConfirmValue) {
    if (ConfirmValue == 0) {
        $.jConfirm("您确定要结束对话么？", function (r) {
            if (r) {
                SetBeforeunload(false, onbeforeunload_handler);
                onbeforeunload_handler();
                window.opener = null; window.open('', '_self'); window.close();
            }

        });
    } else {
        SetBeforeunload(false, onbeforeunload_handler);
        onbeforeunload_handler();
        window.opener = null; window.open('', '_self'); window.close();
    }
}
function shangchuan() {
    var uploadify = $('#uploadify');
    uploadify.uploadify('upload', '*');

}

//根据客户ID与坐席ID，取近六条聊天记录
function GetHMessageByAgent(privatetaken, agentid) {
    //    AjaxPostAsync("AjaxServers/Handler.ashx", {
    //        action: escape("getfristhistroy"),
    //        FromPrivateToken: escape(privatetaken),
    //        SendToPrivateToken: escape(agentid)
    //    }, null, function (data) {
    //        if (data != "") {
    //            var jsonData = JSON.parse(data);
    //            //不成功提示错误，成功把录音主键保存在隐藏域里面

    //            document.getElementById("Rmessages").innerHTML += "<p class='his_more'> ---查看更多，请点击 <a href=\"ConversationHistory.aspx?UserID=" + agentid + "&LoginID=" + privatetaken + "\" target=\"_blank\">历史记录</a>---</p>";
    //            document.getElementById("Rmessages").innerHTML += "<div class='fix_gd'>";
    //            for (var i = 0; i < jsonData.length; i++) {
    //                if (jsonData[i].Sender == "1") {
    //                    document.getElementById("Rmessages").innerHTML += zts(jsonData[i].AgentNum, jsonData[i].CreateTime, jsonData[i].Content);
    //                    //"<div class='dh1'><div class='title'>客服" + jsonData[i].AgentNum + "说（" + jsonData[i].CreateTime + "）：</div><div class='dhc'>" +
    //                    //jsonData[i].Content + "</div></div>";
    //                }
    //                else {
    //                    document.getElementById("Rmessages").innerHTML += wts("您", jsonData[i].CreateTime, jsonData[i].Content);
    //                    //"<div class='dh1 dh2'><div class='title'>您说（" + jsonData[i].CreateTime + "）：</div><div class='dhc'>" +
    //                    //jsonData[i].Content + "</div></div>";
    //                }

    //            }
    //        }
    //        var nowdate = new Date();
    //        var datestr = nowdate.getFullYear() + "年" + (nowdate.getMonth() + 1) + "月" + nowdate.getDate() + "日";
    //        document.getElementById("Rmessages").innerHTML += "</div><p class='his_more'>---" + datestr + "---</p>"
    //    });
}
function jieping() {
    //scpMgr.Capture();
    f_capture();
}
//事件-传输完毕
function ScreenCapture_Complete(obj) {
    obj.Progress.text("100%");
    obj.Message.text("上传完成");
    obj.State = obj.StateType.Complete;
    obj.CloseInfPanel(); //隐藏信息层
    //显示截取的屏幕图片

    var imgstr = "<img src='" + obj.ATL.Response + "'/>";

    $("#Smessage").html(oldText + imgstr);
    //设置为截屏
    $("#hidIsJiePing").val(imgstr);
}
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
        var oldText = $.trim($("#Smessage").html());
        $("#Smessage").html(oldText + imgstr);
        //设置为截屏
        $("#hidIsJiePing").val(imgstr);
    }
}
//加载常用问题
function f_getFreProblemlist(SourceType) {
    LoadingAnimation('divquestion');
    $('#divquestion').load('SeniorManage/FreProblemForClient.aspx?SourceType=' + SourceType + '&r=' + Math.random(), null);

}
//取历史聊天记录
function HistroyMore() {
    var userid = $("#hidto").val();
    window.open("ConversationHistory.aspx?LoginID=" + escape(loginid) + "&r=" + Math.random());
}
//满意度弹层
function addSatisfaction() {
    if (isManyi) {
        $.jAlert("您已对客服进行过评价，请勿重复提交~");
    }
    else {
        var csid = $("#hidAllocID").val();
        $.openPopupLayer({
            name: "AddSatisfactionAjaxPopup",
            parameters: { CSID: csid },
            url: "/SatisfactionForm.aspx?r=" + Math.random(),
            beforeClose: function (n) {
                if (n == true) {
                    isManyi = true;
                    //$("#EMYiChe").unbind("click");
                    //发送满意度消息给客服
                    SendMessage("6", "", "", "", "");

                    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/taiyang.png'>非常感谢您对我的服务做出评价，祝您生活愉快。^_^");
                }

            }
        });
    }
}
//设置按钮是否可用
function Chatdisabled(type) {
    if (type == 0) {
        $("#bq_listSH").css("display", "none");
        $("#HistroyList").css("display", "none");
        $("#btnCapture").css("display", "none");
        $("#EMYiChe").css("display", "none");
        $("#fileUpload").css("display", "none");


        $("#btnSend").attr("disabled", "disabled");
        //$("#Smessage").attr("contenteditable", false);
    }
    else {


        $("#bq_listSH").css("display", "");
        $("#HistroyList").css("display", "");
        $("#btnCapture").css("display", "");
        $("#EMYiChe").css("display", "");
        $("#fileUpload").css("display", "");

        $("#btnSend").removeAttr("disabled");
        $("#Smessage").attr("contenteditable", true);
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

function zts(z, t, s) {
    return "<div class='dh_user1' style='padding: 0px 10px;margin: 3px 0px;'><table  border='0'><tr><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td><td><div class='user_name'><span>客服" + z + "</span>" + t + "</div></td></tr><tr><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + s + "</div></div></td></tr></table></div><div class='clearfix'></div>";
}
function zs(msg) {
    return "<div class='dh_user1' style='padding: 0px 10px;margin: 3px 0px;'><table  border='0'><tr><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td></tr><tr><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + msg + "</div></div></td></tr></table></div><div class='clearfix'></div>";
    //return "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>" + msg + "</div></div></div><div class='clearfix'></div>";
}
function wts(w, t, s) {

    return "<div class='dh_user1 dh_user2' style='padding: 0px 10px;margin: -1px 0px; float:right;'><table  border='0'><tr><td><div class='user_name'><span>" + w + "</span>" + t + "</div></td><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='images/user-im.png' /></div></td> </tr><tr><td><div class='im-con'><div class='user_con' >" + s + "<div class='arrow2'></div></div></div></td></tr></table></div><div class='clearfix'></div>"

    //return "<div class='dh_user1 dh_user2' style='padding: 0px 10px; float:right;'><div class='user_img' style='float:right;'><img width='70' height='70' src='images/user-im.png'></div><div class='user_content' style='float:right;'><div class='user_name'>" + w + "<span>【" + t + "】</span></div><div class='user_con'>" + s + "</div><div class='arrow2' style='float:right;'></div></div></div><div class='clearfix'></div>";
}
function js(msg) {
    return "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'>" + msg + "</div></div><div class='clearfix'></div>";
}


function BQIMGClick(eve) {
    eve.stopPropagation();
    eve.preventDefault();
    if (BQPop == null) {
        $('#divAnswer').BQPop({ left: 10, bottom: 8, isAgent: false });
        BQPop = $('#BQMain');
    }
    BQPop.toggle();
    return false;
}
//表情按钮：
function BQClick(url, e) {

    $('#Smessage').append('<img src="' + url + '" />').focus();
    BQPop.hide();
    $("#Smessage").focus();
    moveEnd("Smessage");
}