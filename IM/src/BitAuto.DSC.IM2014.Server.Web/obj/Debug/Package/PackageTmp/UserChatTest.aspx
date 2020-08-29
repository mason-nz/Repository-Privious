<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserChatTest.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.UserChatTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <title>在线客服</title>
    <link type="text/css" href="IMCss/css.css" rel="stylesheet" />
    <link href="IMCss/uploadify.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/AspNetComet.js"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitUploadify();
        });
        //设置刷新，关闭页面警告，并执行onbeforeunload_handler()
        SetBeforeunload(true, onbeforeunload_handler);
        //使页面全屏显示
        function InitUploadify() {
            var uploadSuccess = true;
            $("#uploadify").uploadify({
                'buttonText': '选择',
                'auto': false,
                'swf': 'Scripts/uploadify.swf',
                'uploader': '/AjaxServers/FileLoad.ashx?v=' + Math.random(),
                'multi': false,
                'fileSizeLimit': '5MB',
                'queueSizeLimit': 1,
                'method': 'post',
                'removeTimeout': 1,
                'fileTypeDesc': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
                'fileTypeExts': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
                'width': 79,
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

                            $('#Smessage').val("<a href=http://im.sys1.bitauto.com/"+jsonData.FilePath+">"+jsonData.FileName+"</a>");
                            SendMessage();
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
        function setbigsmall() {
            $('#bodyDIV').css('height', (($(window).height() - 20) + 'px'));
            $('#divcontent').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
            //            //指定左边高度
            $('#divleft').css('height', "87%");
            //            //指定右面高度
            $('#divright').css('height', "102.4%");
        }
        //给cookie设置网友所选快捷键，以便下次访问不要再重新设置快捷键
        function SetQuick(quickvalue) {
            SetCookie("quickvalue", quickvalue);
        }
        //页面大小关闭时触发，让页面始终适合页面大小
        function ChangeBigSmall() {
            setbigsmall();
        }
        $(document).ready(function () {
            //bindHotKey();
            setbigsmall();
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
            StartChat();
        });
        //初始化
        function StartChat() {
            //是否初始化过标识
            $("#isInit").val("1");
            var UserReferURL = '<%=UserReferURL%>';
            var username = '<%=username%>';
            var cityname = bit_locationInfo.cityName;
            var Ip = bit_locationInfo.IP;
            var cityID = bit_locationInfo.cityId;
            var usertype = "2";
            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: "action=init&username=" + escape(username) + "&usertype=" + escape(usertype) + "&UserReferURL=" + escape(UserReferURL) + "&UserCityName=" + escape(cityname) + "&UserIP=" + escape(Ip) + "&cityID=" + escape(cityID),
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result == 'loginok') {
                        $("#agentAllocat").html("您好，欢迎使用易车在线客服。稍候会有客服为您服务...");
                        defaultChannel = null;
                        Connect();
                    }
                    else {
                        SetBeforeunload(false, onbeforeunload_handler);
                        alert('登录失败：' + r.result);
                        window.opener = null; window.open('', '_self'); window.close();
                    }

                }
            });

        }
        //刷新关闭执行方法
        function onbeforeunload_handler() {
            var sendto = $('#hidto').val();
            var pody = { action: 'userclosechat', username: escape('<%=username %>'), SendToPublicToken: escape(sendto) };
            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r = JSON.parse(msg);
                 if (r != null && r.result == 'sendok') {//登录成功之后
                 }
             });
        }
        //长链接请求
        var defaultChannel = null;
        function Connect() {
            if (defaultChannel == null) {
                defaultChannel = new AspNetComet("/DefaultChannel.ashx", "<%=username %>", "defaultChannel");
                defaultChannel.addTimeoutHandler(TimeoutHandler);
                defaultChannel.addFailureHandler(FailureHandler);
                defaultChannel.addSuccessHandler(SuccessHandler);
                //分配坐席
                defaultChannel.addAllocAgentForUserHandler(AllocAgentForUserHandler);
                //坐席全忙
                defaultChannel.addMAllBussyHandler(MAllBussyHandler);
                //坐席离开
                defaultChannel.addAgentLeaveHandler(AgentLeaveHandler);
                //排队队列达到上限
                defaultChannel.addMaxQueueHandLer(MaxQueueHandler);
                //发送长连接请求
                defaultChannel.subscribe();
            }
        }
        //队列排队达到上限值
        function MaxQueueHandler(privateToken, alias, message) {
            $("#Smessage").attr("disabled", "disabled");
            $("#btnSend").attr("disabled", "disabled");
            $("#agentAllocat").html("您好，欢迎使用易车在线客服,目前排队等待的队列已达上限，如果您想继续排队，请<a style='cursor:pointer'  onclick='ContinueQueue()'>点击继续排队</a><br/>如果你选择留言，请<a style='cursor:pointer' onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>");
            $("#divagentAllocat").css("display", "");
            $("#Rmessages").css("height", "85%");
        }
        //继续排队
        function ContinueQueue() {

            $.jConfirm("您确定要继续排队吗？", function (r) {
                if (r) {
                    $("#agentAllocat").html("");
                    setTimeout(function () {
                        var pody = { action: 'ResetAgent', username: escape('<%=username %>') };
                        AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
                                             function (msg) {
                                                 if (msg != "") {
                                                     var r = JSON.parse(msg);
                                                     if (r != null && r.result == 'Initializeok') {//登录成功之后
                                                         StartChat();
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
            messageFlicker.clear();
            $("#hidto").val(message.AgentID);
            $("#divagentNo").html("易车网客服 " + message.AgentID);
            //把分配坐席标识
            $("#hidAllocID").val(message.AllocID);
            //清空等待信息
            $("#agentAllocat").html("");
            $("#divagentAllocat").css("display", "none");
            $("#Rmessages").css("height", "100%");
            document.getElementById("Rmessages").innerHTML += "<p class='hs'>您好，欢迎光临易车网~我是您的购车博士" + message.AgentID + "~在接下来的时间里，我将根据您的需求，为您提供车型推荐、车型对比、车型点评、经销商及报价查询等一系列服务，请问您现在最想了解什么呢？</p>";
            //让信息框和发送信息按钮可用
            $("#Smessage").removeAttr("disabled");
            $("#btnSend").removeAttr("disabled");
            messageFlicker.show();
        }
        //坐席全忙，去留言
        function addMessage() {
            $.jConfirm("您确定要留言吗？", function (r) {
                if (r) {
                    var sendto = $('#hidto').val();
                    onbeforeunload_handler();
                    window.location.href = "AddUserMessage.aspx?SendTo=" + sendto + "&UserMessageIMID=<%=username %>";
                }
                else {
                    //取消留言时让退出提示可用，并且退出时调用onbeforeunload_handler方法
                    SetBeforeunload(true, onbeforeunload_handler);
                }
            });
        }
        //坐席全忙
        function MAllBussyHandler(privateToken, alias, message) {
            $("#Smessage").attr("disabled", "disabled");
            $("#btnSend").attr("disabled", "disabled");
            $("#agentAllocat").html("您好，欢迎使用易车在线客服。目前您前面有 " + message.WaitCount + " 个人正在排队等待，请耐心等待...<br/>如果您不想继续等待，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>");
            $("#divagentAllocat").css("display", "");
            $("#Rmessages").css("height", "85%");
        }
        //成功接收到聊天信息事件
        function SuccessHandler(privateToken, alias, message) {
            if ($("#agentAllocat").html() != "") {
                $("#agentAllocat").html("");
                $("#divagentAllocat").css("display", "none");
                $("#Rmessages").css("height", "100%");
            }
            messageFlicker.clear();
            var rectime = getHoursMinute2(message.ct);
            message.c.m = replaceRegUrl(message.c.m);
            document.getElementById("Rmessages").innerHTML += "<div class='dh1'><div class='title'>客服代表" + message.c.f + "  " + rectime + "</div><div class='dhc'>" + message.c.m + "</div></div>";
            document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
            messageFlicker.show();
        }
        //错误通知
        function FailureHandler(privateToken, alias, error) {
            if ($('#hidto').val() != "") {
                if (error == "CometClient does not exist.") {
                    messageFlicker.clear();
                    document.getElementById("Rmessages").innerHTML += "<p class='hs'>对话被中断或已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(1)'>结束对话</a></p>";
                    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
                    $("#btnSend").attr("disabled", "disabled");
                    $("#Smessage").attr("disabled", "disabled");
                    messageFlicker.show();
                }
            }
        }
        //坐席离开通知
        function AgentLeaveHandler(privateToken, alias, message) {
            $('#hidto').val("");
            messageFlicker.clear();
            document.getElementById("Rmessages").innerHTML += "<p class='hs'>对话被中断或已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(1)'>结束对话</a></p>";
            document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
            $("#btnSend").attr("disabled", "disabled");
            $("#Smessage").attr("disabled", "disabled");
            messageFlicker.show();
        }
        //长连接超时通知
        function TimeoutHandler(privateToken, alias) {
        }
        //发送消息
        function SendMessage() {


            //消息接收者
            var sendto = $('#hidto').val();
            //坐席分配标识
            var AllocID = $('#hidAllocID').val();
            //var issuccess=0;
            var messagetxt = $('#Smessage').val();
            //替换特殊字符
            messagetxt = FormatSpecialCharacters(messagetxt);
            if (sendto == "") {
                document.getElementById("Rmessages").innerHTML += "<p class='hs'>对话被中断或已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(1)'>结束对话</a></p>";
            }
            else {
                if ($.trim(messagetxt) == "") {
                    $.jAlert("消息不能为空！");
                }
                else if ($.trim(messagetxt) == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
                    $.jAlert("消息不能为空！");
                }
                else if (Len(messagetxt) > 600) {
                    $.jAlert("文本内容太长，请整理后重新发送！");
                }
                else {
                    sendmessage = messagetxt;
                    $('#Smessage').val('');

                    var pody = { action: 'sendmessage', username: escape('<%=username %>'), message: escape(sendmessage), SendToPublicToken: escape(sendto), AllocID: escape(AllocID) };
                    AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 if (msg != "") {
                     var r = JSON.parse(msg);
                     //消息发送成功
                     if (r != null && r.result != "SendToLeave" && r.result != "ClientNotExists") {//登录成功之后
                         var r2 = $.evalJSON(r.result)
                         if (r2.result == 'sendok') {
                             var dtime = getHoursMinute2(r2.rectime);
                             sendmessage = replaceRegUrl(sendmessage);
                             document.getElementById("Rmessages").innerHTML += "<div class='dh5'><div class='title'>您 " + dtime + "</div><div class='dhc'>" + sendmessage + "</div></div>";
                         }
                     }
                     //坐席离线
                     else if (r != null && r.result == 'SendToLeave') {
                         document.getElementById("Rmessages").innerHTML += "<p class='hs'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(1)'>结束对话</a></p>";
                         document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
                     }
                     //网友客户端实体不存在
                     else if (r != null && r.result == 'ClientNotExists') {
                         document.getElementById("Rmessages").innerHTML += "<p class='hs'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(1)'>结束对话</a></p>";
                         document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
                     }

                 }
                 else {
                     document.getElementById("Rmessages").innerHTML += "<p class='hs'>消息: " + sendmessage + " 发送失败，您的网络出现异常！</p>";
                     $.jAlert("您的网络出现异常，请检查网络后重试！");
                     document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
                 }
                 document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
             });
                }
            }
        }
        //继续等待，分两种是1是需要初始化网友实体，并且重新建立长连接，0是长连接存在，只是重新排队
        function RestAllocAgent(Reset) {
            $("#divagentNo").html("易车网客服");
            document.getElementById("Rmessages").innerHTML = "";
            if (Reset == '0') {
                var pody = { action: 'ResetAgent', username: escape('<%=username %>'), UserReferURL: escape('<%=UserReferURL%>') };
                AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
                         function (msg) {
                             if (msg != "") {
                                 var r = JSON.parse(msg);
                                 if (r != null && r.result == 'Initializeok') {//登录成功之后
                                     StartChat();
                                 }
                                 else {

                                 }
                             }
                         });
            } else {
                StartChat();
            }
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

            if (quickvalue == "1") {
                if (keyCode == 13) {
                    SendMessage();
                    return false;
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
            uploadify.uploadify('upload', '*')
            
        }
    </script>
</head>
<body onclick="messageFlicker.clear();" onresize="ChangeBigSmall()">
    <div id="bodyDIV" class="online_kf">
        <input type="hidden" id="hidto" />
        <input type="hidden" id="isInit" value="0" />
        <input type="hidden" id="hidAllocID" />
        <input type="hidden" id="hidNoAgent" value="0" />
        <div class="title_kf">
            在线客服<span><a href="#"><img src="images/c_btn.png" border="0" alt="关闭" style="cursor: pointer"
                onclick="CloseWindow(0)" /></a></span></div>
        <div class="content_kf" id="divcontent">
            <!--左开始-->
            <div class="left_c" id="divleft">
                <div class="answer">
                    <div id="divagentAllocat" class="fix_gd">
                        <p id="agentAllocat" class="hs">
                        </p>
                    </div>
                    <div class="scorll_gd" style="height: 85%" id="Rmessages">
                    </div>
                </div>
                <div class="ask">
                    <div class="style_1" style="height: 20%">
                    </div>
                    <div class="ask_c" style="height: 80%">
                        <textarea id="Smessage" style="height: 80%; overflow-y: auto;" disabled="disabled"
                            onclick="ClearTitle()" onkeypress="return onKeyPress(event)">请用一句话简要、准确地描述您的问题，如易车惠购车返现问题</textarea></div>
                </div>
                <div class="btn" style="height: 10%">
                    <div class="left gray">
                        发送快捷键：
                        <label onclick="SetQuick('1')">
                            <input name="RadioQuick" id="radenter" type="radio" value="1" /><span> Enter</span></label>&nbsp;<label
                                onclick="SetQuick('2')"><input name="RadioQuick" id="radctrlenter" type="radio" value="2" /><span>
                                    Ctrl + Enter</span></label></div>
                    <div class="right">
                        <input type="button" disabled="disabled" value="发送" id="btnSend" onclick="SendMessage()"
                            class="w80" /></div>
                </div>
            </div>
            <!--右开始-->
            <div class="right_c" id="divright">
                <div class="person">
                    <div class="pic_t">
                    </div>
                    <div class="title" id="divagentNo">
                        易车网客服</div>
                </div>
                <div class="question">
                    <div class="title">
                        常见问题</div>
                    <ul>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=1" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">什么是汽车通？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=2" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">为什么要激活邮箱？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=3" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何激活邮箱？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=4" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何找回个人密码？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=5" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何完善个人信息？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=6" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何进入会长管理后台？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=7" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">完善个人信息的重要性</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=8" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何获得易车网车标？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=9" target="_blank"
                            onclick="SetBeforeunload(false, onbeforeunload_handler);">如何联系我们？</a></li>
                        <li class="more"><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=2
" target="_blank">更多&gt;&gt;</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="clearfix">
        </div>
        <div>
            <input type="file" id="uploadify" name="uploadify" /><input type="button" onclick="shangchuan();" value="确认" />
        </div>
    </div>
</body>
</html>
