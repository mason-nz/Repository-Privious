<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="AgentChatNew.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.AgentChatNew" %>

<%@ Register Src="~/Controls/Top.ascx" TagName="TopMaster" TagPrefix="TM" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>对话管理</title>
    <link type="text/css" href="IMCss/css.css" rel="stylesheet" />
    <link type="text/css" href="IMCss/style.css" rel="stylesheet" />
    <script src="js/jquery-1.6.4.min.js" language="javascript" type="text/javascript"></script>
    <script src="js/public.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="Scripts/AspNetComet.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.hotkeys.js"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">

        var defaultChannel = null;

        function Connect() {
            if (defaultChannel == null) {
                defaultChannel = new AspNetComet("/DefaultChannel.ashx", "<%=this.AgentIMID %>", "defaultChannel");
                defaultChannel.addTimeoutHandler(TimeoutHandler);
                defaultChannel.addFailureHandler(FailureHandler);
                defaultChannel.addSuccessHandler(SuccessHandler);
                //分配坐席
                defaultChannel.addAllocAgentForUserHandler(AllocAgentForUserHandler);
                //网友离开
                defaultChannel.addAgentLeaveHandler(AgentLeaveHandler);
                //发送长连接请求
                defaultChannel.subscribe();
            }
        }

        //分配坐席
        function AllocAgentForUserHandler(privateToken, alias, message) {
            //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮
            //聊天层、聊天记录、发送消息、消息内容等元素都以网友ID结尾，前缀一致

            if (!isExistChatLayer(message.UserID)) {
                var userinfo = {
                    UserIMID: message.UserID,
                    UserReferURL: message.UserReferURL,
                    AllocID: message.AllocID,
                    LocalIP: message.LocalIP,
                    Location: message.Location,
                    EnterTime: message.EnterTime,
                    WaitTime: message.WaitTime,
                    TalkTime: message.TalkTime
                };
                OperateChatLayer(userinfo);
                //NewMessageFlicker();
            }
        }

        //网友离开通知
        function AgentLeaveHandler(privateToken, alias, message) {
            //网友ID
            var UserIMID = message.f;
            $("#mytr" + UserIMID).remove();
            $("#mycusInfo" + UserIMID).remove();
        }

        var gitemp = 0;
        //接收消息事件方法
        //从消息发送人属性，message.from取网友ID
        function SuccessHandler(privateToken, alias, message) {
            if (message.c.f == "System") {
                return;
            }
            //收到网友退出消息，清除网友聊天层及按钮
            if (message.n == "MLline") {
                $("#mytr" + UserIMID).remove();
                $("#mycusInfo" + UserIMID).remove();

                return;
            }

            //网友ID
            var UserIMID = message.c.f;
            var AllocID = message.AllocID;
            if (!isExistChatLayer(UserIMID) && UserIMID != "System") {
                var userinfo = {
                    UserIMID: UserIMID,
                    UserReferURL: message.c.UserReferURL,
                    AllocID: message.c.AllocID,
                    LocalIP: message.c.LocalIP,
                    Location: message.c.Location,
                    EnterTime: message.c.EnterTime,
                    WaitTime: message.c.WaitTime,
                    TalkTime: message.c.TalkTime
                };
                OperateChatLayer(userinfo);
            }

            //有网友新消息，更新临控信息行状态图标
            if (UserIMID != "System") {

                //当前网友聊天层是否显示
                if ($("#mycusInfo" + UserIMID).is(":hidden")) {
                    $("#myimgstatus" + UserIMID).attr("src", "images/status1.png");
                    $("#mytr" + UserIMID).children().css("color", "red");
                }

                //新消息标题闪烁
                //doFlashTitle();
                NewMessageFlicker();
            }

            //将网友显示在最前面
            ChatRowOrderFrist(UserIMID);

            //聊天层关闭、显示方法
//            $("#mytr" + UserIMID).dblclick(function () {
//                ShowChatLayer(UserIMID);
//            });

            //收到消息内容            
            var chatinfo = {
                UserIMID: UserIMID, //网友ID,
                AgentID: '', //坐席ID
                name: '网友', //网友别名
                //rectime:message.ct,//收到消息时间
                //rectime: message.ct.split(' ')[1],
                rectime: getHoursMinute2(message.ct),
                content: message.c.m //消息内容
            }

            //显示聊天记录
            CreateMyChat(chatinfo);

            //测试，收到网友消息后，立即回复
//            gitemp = gitemp + 1;
//            $("#mymsg" + UserIMID).val("您好，这里是自动回复!!"+ UserIMID + "," + gitemp);
//            SendMessage(UserIMID);
        }

        //生成聊天记录html
        function CreateMyChat(chatinfo) {
            var tUserIMID = '';
            if (chatinfo.AgentID == '') {
                tUserIMID = chatinfo.UserIMID.split('-')[4];
                //message.ct.split(' ')[1]
                //tUserIMID = chatinfo.UserIMID.substr(chatinfo.UserIMID.length-4,4);
                //tUserIMID = chatinfo.UserIMID;
            }
            else {
                tUserIMID = chatinfo.AgentID;
            }

            //将文本中的url替换成链接
            chatinfo.content = replaceRegUrl(chatinfo.content);            

            var html = "";
            html += "<div class='dh1'>";
            html += "<div class='title'>" + chatinfo.name + " " + chatinfo.rectime + "</div>";
            //html += "<div class='title'>" + chatinfo.name + tUserIMID + " " + chatinfo.rectime + "</div>";
            html += "<div class='dhc'>" + chatinfo.content + "</div>";
            html += "</div>";

            
            $("#mychat" + chatinfo.UserIMID).append(html);

            //更新聊天记录最后收到消息时间
            $("#myendmsgspan" + chatinfo.UserIMID).text("(发送快捷键：Enter)最后收到消息时间：" + chatinfo.rectime);

            //更新网友临控信息行最收消息时间
            $("#mytdtime" + chatinfo.UserIMID).text(chatinfo.rectime);

            //有新消息时，让滚动条自动向下滚动
            if (isExistChatLayer(chatinfo.UserIMID)) {
                document.getElementById("mychat" + chatinfo.UserIMID).scrollTop = document.getElementById("mychat" + chatinfo.UserIMID).scrollHeight;
                //$("#mychat"+ chatinfo.UserIMID).scrollTop = $("#mychat"+ chatinfo.UserIMID).scrollHeight;
            }
        }

        function FailureHandler(privateToken, alias, error) {
            if (error == "CometClient does not exist.") {
            }
            else {
                alert(error);
            }

            //自动退出
            Quit();
        }
        function TimeoutHandler(privateToken, alias) {
            //alert("timeout");
            //document.getElementById("messages").innerHTML += "timeout<br/>";
        }

        //消息接收人：recipient
        //消息内容：message
        function SendMessage(UserIMID) {
            //消息接收人
            var recipient = UserIMID;

            //会话标识，每分配一次生成新标识
            var AllocID = $("#mytr" + UserIMID).attr("AllocID");

            //消息内容
            var message = $("#mymsg" + UserIMID).val();

            if (recipient == "") {
                alert("消息接收人不能为空!");
                return;
            }
            if (message == "") {
                alert("消息内容不能为空!");
                return;
            }

            //格式化特殊字符
            message = FormatSpecialCharacters(message);
            //清空发送消息文本框                         
            $("#mymsg" + UserIMID).val('');

            //消息发送人
            var username = '<%=this.AgentIMID %>';
            var parameters = {
                action: 'sendmessage',
                username: username,
                SendToPublicToken: recipient,
                AllocID: escape(AllocID),
                message: escape(message)
            };
            $.ajax({
                async: false,
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    if (msg != "") {
                        var r = JSON.parse(msg); //ClientNotExists 
                        if (r != null && r.result == 'SendToLeave') {
                            alert("发送消息失败，网友已离线!");
                            return;
                        }
                        if (r != null && r.result == 'ClientNotExists') {
                            alert("发送消息失败，您已离线!");
                            return;
                        }
                        var r2 = $.evalJSON(r.result)
                        if (r != null && r2.result == 'sendok') {//登录成功之后
                            //清空发送消息文本框                         
                            //$("#mymsg" + UserIMID).val('');

                            //当前系统时间                                
                            //var dtime = getSysTime();
                            var dtime = getHoursMinute2(r2.rectime);
                            //收到消息内容            
                            var chatinfo = {
                                UserIMID: UserIMID, //网友ID
                                AgentID: username, //坐席ID
                                name: '坐席', //别名
                                rectime: dtime, //收到消息时间
                                content: message //消息内容
                            }

                            //显示聊天记录
                            CreateMyChat(chatinfo);
                        }
                        else {
                            // alert('发送消息失败：' + r.result + '/n消息内容：' + message);
                            alert('发送消息失败：');
                        }
                    }
                    else {
                        alert("您的网络出现异常，请检查网络后重新发送！/n消息内容：" + message);

                    }
                }
            });

        }


        //登录系统
        function Login() {
            var parameters = {
                action: 'init',
                username: '<%=this.AgentIMID %>',
                usertype: '1'
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result == 'loginok') {//登录成功之后
                        defaultChannel = null;
                        Connect();
                    }
                    else {
                        $.jAlert('登录失败：' + r.result, function () {
                            SetBeforeunload(false, onbeforeunload_handler);
                            window.location = "Login.aspx";
                        });

                    }

                }
            });
        }

        //退出系统
        function Quit() {
            var parameters = {
                action: 'closechat',
                username: '<%=this.AgentIMID %>'
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result == 'sendok') {//登录成功之后                            
                        //$.jAlert("退出成功");
                        SetBeforeunload(false, onbeforeunload_handler);
                        window.location = "Login.aspx";
                    }
                    else {
                        $.jAlert('退出失败：' + r.result);
                        SetBeforeunload(false, onbeforeunload_handler);
                        window.location = "Login.aspx";
                    }
                }
            });
        }


        function BindAgentStateClick() {
            //缺省：离线
            $("#curAgentState").attr("title", "离线");
            $("#curAgentState").text("离线"); ;
            //生成坐席状态列表Html
            var parameters = {
                action: 'GetAgentState'
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result != '') {
                        var result = $.evalJSON(r.result);
                        for (var i = 0; i < result.length; i++) {
                            var message = result[i];
                            CreateAgentStateHtml(message.name, message.value);
                        }
                        $("#myulstate").children().last().addClass("last");
                        $(".status.right").show();
                    }
                    else {
                        alert('获取坐席状态失败');
                    }
                }
            });

        }

        //生成坐席状态Html
        function CreateAgentStateHtml(name, value) {
            var html = "";
            html += "<li><a class='narxtmin' state='" + value + "'>" + name + "</a></li>";
            $("#myulstate").append(html);

            //绑定Click事件
            $(".narxtmin").click(function () {
                var curstate = $("#curAgentState").text();
                var newstate = $(this).text();

                if (curstate != newstate) {
                    $("#curAgentState").text($(this).text());
                    $("#curAgentState").attr("title", $(this).text());

                    var state = $(this).attr("state");
                    SetAgentState(state);
                }
            });
        }

        //更改坐席状态
        function SetAgentState(state) {
            var parameters = {
                action: 'SetAgentState',
                username: '<%=this.AgentIMID %>',
                AgentState: state
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result == 'sendok') {
                        $.jAlert("更改状态成功!");
                        if (state == 2) {
                            $("tr[id^='mytr']").remove();
                            $("div[id^='mycusInfo']").remove();
                        }
                    }
                    else {
                        $.jAlert('更改状态失败：' + r.result);
                    }
                }
            });
        }

        //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮
        //聊天层、聊天记录、发送消息、消息内容等元素都以网友ID结尾，前缀一致
        //UserIMID:网参ID，UserReferURL:访客来源,AllocID:会话标识
        function GenerateChatLayer(userinfo) {
            //var tUserIMID= userinfo.UserIMID.substr(userinfo.UserIMID.length-4,4);
            var tUserIMID = userinfo.UserIMID.split('-')[4];
            //首先生成在聊友监控信息行
            //字体为红色表明是刚建立会话，图标为等待
            //建立会话，有新消息时图标也为等待
            var html = "";
            html += "<tr id='mytr" + userinfo.UserIMID + "' userimid='" + userinfo.UserIMID + "' AllocID='" + userinfo.AllocID + "' style='cursor:pointer'>";
            html += "<td id='mytdstatus" + userinfo.UserIMID + "'><img id='myimgstatus" + userinfo.UserIMID + "' userimid='" + userinfo.UserIMID + "' src='images/status1.png' width='16' height='16' /></td>";
            html += "<td style='color:;'>" + tUserIMID + "</td>";
            html += "<td style='color:;'>" + userinfo.LocalIP + "</td>";
            html += "<td style='color:;'>" + userinfo.Location + "</td>";
            html += "<td style='color:;'>" + userinfo.EnterTime.split(' ')[1] + "</td>";
            html += "<td style='color:;'>" + userinfo.WaitTime + "</td>";
            html += "<td id='mytdtime" + userinfo.UserIMID + "' style='color:red;'></td>"; //最后消息
            html += "<td class='cName' style='color:;'><a href='javascript:void(0);'>" + userinfo.UserReferURL + "</a></td>";

            //默认按网友分配时间降序排列，如有新消息网友排在最前面
            //新网友显示在最上面
            if (!isExistInAllChatLayers()) {
                $("#mytable").append(html);
            }
            else {
                var $find = $("tr[id^='mytr']").eq(0);
                $find.before(html);
            }

            //生成网友聊层
            html = "";
            html += "<div class='cusInfo' id='mycusInfo" + userinfo.UserIMID + "'>";
            html += "<div class='title'>客户信息<span>来源：<a href='javascript:void(0);'>" + userinfo.UserReferURL + "</a></span></div>";
            html += "<div class='cusInfoC'>";
            html += "<div class='InfoC'>";
            html += "<table border='0' cellspacing='0' cellpadding='0'>";
            html += "<tr>";
            html += "<th width='20%'>姓名：</th>";
            html += "<td width='30%'></td>";
            html += "<th width='20%'>性别：</th>";
            html += "<td width='30%'></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<th>电话：</th>";
            html += "<td></td>";
            html += "<th>QQ：</th>";
            html += "<td></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<th>邮箱：</th>";
            html += "<td></td>";
            html += "<th>客户分类：</th>";
            html += "<td></td>";
            html += "</tr>";
            html += "<tr>";
            html += "<th>地区：</th>";
            html += "<td></td>";
            html += "<th>客户ID：</th>";
            html += "<td></td>";
            html += "</tr>";
            html += "</table>";
            html += "<div class='btn'>";
            //隐藏按钮
            //html += "<div class='right'><input type='button' value='编辑信息'  class='w80 gray'/> <input type='button' value='添加工单'  class='w80 gray'/></div>";
            html += "</div>";
            html += "</div>";

            //对话聊天层
            html += "<div class='dialogue'>";
            html += "<p id='mydialoguep" + userinfo.UserIMID + "'>对话开始于: " + userinfo.TalkTime + "</p>";
            //有新消息时，附加聊天记录层
            //<div class="dh1">
            //<div class="title">张莹莹  09:53:31</div>
            //<div class="dhc">您好，欢迎您访问！请问有什么需要咨询？</div>
            //</div>
            html += "<div class='scroll_gd' id='mychat" + userinfo.UserIMID + "'>";
            html += "</div>";

            //发送消息层
            html += "<div class='editC'>";
            html += "<div class='style_1'></div>";
            html += "<div class='ask_c'><textarea id='mymsg" + userinfo.UserIMID + "' userimid='" + userinfo.UserIMID + "'></textarea></div>"; //发送消息内容
            html += "</div>";
            html += "<div class='endmsg' id='myendmsg" + userinfo.UserIMID + "'><span id='myendmsgspan" + userinfo.UserIMID + "'>(发送快捷键：Enter)最后收到消息时间：</span><span class='right' style='*margin-top:-20px;'><input id='mybtsend" + userinfo.UserIMID + "' type='button' value='发送'  class='w80 gray' /> </span></div>";
            html += "</div>";
            html += "<div class='clearfix'></div>";
            html += "</div>";
            html += "<div class='clearfix'></div>";
            html += "</div>";

            $("#mycontent2").append(html);

            $("#mycusInfo" + userinfo.UserIMID).hide();
        }

        //绑定发送消息方法、聊天层关闭方法
        function BindSendMessage(UserIMID) {
            //消息发送按钮           
            var sendmsg = $("#mybtsend" + UserIMID);
            sendmsg.click(function () {
                SendMessage(UserIMID);
            });

            //绑定发送消息框快捷键设置
            $("#mymsg" + UserIMID).bind("keyup", "return", function () {
                var UserIMID = $(this).attr("userimid");
                SendMessage(UserIMID);
            });

            //聊天层关闭、显示方法
            $("#mytr" + UserIMID).click(function () {
                ShowChatLayer(UserIMID);
            });
        }

        //分配时显示消息聊天层
        function ShowChatLayerAlloc(UserIMID) {
            //首先判断当前页是否有显示的聊天层
            //有，则不显示新聊天层
            //无，则显示新聊天层
            if (!isHasShowChatLayer()) {
                //$("#mycusInfo"+ UserIMID).show();
                ShowChatLayer(UserIMID);
            }
            else {
                $("#mycusInfo" + UserIMID).hide();
            }

        }

        //显示指定消息聊天层
        function ShowChatLayer(UserIMID) {
            //将当前聊天层放在最前面，防止层切换时页面跳动
//            var $find = $("div[id^='mycusInfo']").eq(0);
//            if ($find.attr("id") == "mycusInfo" + UserIMID) {
//            }
//            else {
//                var $find1 = $("#mycusInfo" + UserIMID);
//                var $temp = $find1;
//                $find1.remove();
//                $find.before($temp);
//            }
            $("div[id^='mycusInfo']").each(function (n, i) {
                if (!$(this).is(":hidden")) {
                    $(this).hide();
                }
            });
            $("#mycusInfo" + UserIMID).show();
            $("#myimgstatus" + UserIMID).attr("src", "images/status2.png");
            $("#mytr" + UserIMID).children().css("color", '');
            //滚动条默认在最下边
            document.getElementById("mychat" + UserIMID).scrollTop = document.getElementById("mychat" + UserIMID).scrollHeight;
        }

        function ShowChatLayer2(UserIMID) {
            $("div[id^='mycusInfo']").each(function (n, i) {
                if (!$(this).is(":hidden")) {
                    $(this).hide();
                }
            });
            $("#mycusInfo" + UserIMID).show();
            $("#myimgstatus" + UserIMID).attr("src", "images/status2.png");
            $("#mytr" + UserIMID).children().css("color", '');
            //滚动条默认在最下边
            document.getElementById("mychat" + UserIMID).scrollTop = document.getElementById("mychat" + UserIMID).scrollHeight;
        }

        //判断是否当前页有聊天层显示
        function isHasShowChatLayer() {
            var isHave = false;
            $("div[id^='mycusInfo']").each(function (n, i) {
                if (!$(this).is(":hidden")) {
                    isHave = true;
                    return false;
                }
            });

            return isHave;
        }

        //封装聊天层生成、拖拽、发送消息等操作
        function OperateChatLayer(userinfo) {
            GenerateChatLayer(userinfo);
            BindSendMessage(userinfo.UserIMID);
            ShowChatLayerAlloc(userinfo.UserIMID);
        }

        //根据网友ID判断网友聊天层是否存在
        function isExistChatLayer(UserIMID) {
            var obj = $("#mytr" + UserIMID);
            if (obj.length > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        //当前页是否有聊天层,包括显示、隐藏的都算
        function isExistInAllChatLayers() {
            var $find = $("tr[id^='mytr']");
            if ($find.length > 0) {
                return true;
            }
            return false;
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
            return parseInt(year + 1900) + "-" + parseInt(month + 1) + "-" + day + " " + hours + ":" + minutes + ":" + seconds + "";
        }


        //初始化方法
        function Init() {
            //禁用右键、文本选择功能、刷新   
            $(document).bind("contextmenu", function () { return false; });
            $(document).bind("selectstart", function () { return false; });
            //        $(document).keydown(function(){return key(arguments[0])});

            Login();

            //            var agentid = '<%=this.AgentIMID %>';
            //            $("#myagentid").text(agentid);

            BindAgentStateClick();
            BindMyExit();
            SetBeforeunload(true, onbeforeunload_handler);
        }

        function onbeforeunload_handler() {
            var parameters = {
                action: 'closechat',
                username: '<%=this.AgentIMID %>'
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
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

        function BindMyExit() {
            $("#myexit").click(function () {
                Quit();
            });
        }
        //热键设置
        function SetHotkey() {
            //显示最新消息层
            $(document).bind('keyup', 'ctrl+z', function () {
                ShowLatestChatLayer();
            });
        }

        //显示最新网友消息
        function ShowLatestChatLayer() {
            var UserIMID = "";
            //找新消息
            var $find = $("img[id^='myimgstatus'][src$='status1.png']");

            if ($find.length > 0) {
                UserIMID = $find.eq(0).attr("userimid");
                ShowChatLayer(UserIMID);
            }
        }

        //默认按网友分配时间降序排列
        //有新消息网友排在最前面
        function ChatRowOrderFrist(UserIMID) {
            var $find1 = $("#mytr" + UserIMID);
            var $find2 = $("tr[id^='mytr']");

            if ($find2.length > 1) {
                //判断第一行是否当前网友
                //是则不做操作，不是则将当前网友移到最前面
                if ($find2.eq(0).attr("userimid") == UserIMID) {
                }
                else {
                    var $temp = $find1;
                    $find1.remove();
                    $find2.eq(0).before($temp);

                    //重新绑定聊天层关闭、显示方法
                    $("#mytr" + UserIMID).click(function () {
                        ShowChatLayer(UserIMID);
                    });
                }
            }
        }

        //浏览器标题新消息闪烁
        function NewMessageFlicker() {
            messageFlicker.clear();
            messageFlicker.show();
        }
       
                              
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Init();
            SetHotkey();
        });
        
        
    </script>
</head>
<body onload="" onclick="messageFlicker.clear();">
    <div class="crm">
        <div class="head">
            <!--头部开始-->
            <TM:TopMaster ID="TopMaster" runat="server" />
        </div>
        <!--头部结束-->
        <!--内容开始-->
        <div class="content" id="mycontent">
            <!--列表开始-->
            <div class="cxList" style="margin-top: 8px;">
                <table id="mytable" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="4%">
                            状态
                        </th>
                        <th width="8%">
                            访客ID
                        </th>
                        <th width="8%">
                            IP地址
                        </th>
                        <th width="8%">
                            地理位置
                        </th>
                        <th width="5%">
                            进入
                        </th>
                        <th width="5%">
                            等待
                        </th>
                        <th width="8%">
                            最后消息
                        </th>
                        <th width="26%">
                            最近访问页面
                        </th>
                    </tr>
                    <%--<tr>
                        <td>
                            <img src="images/status1.png" width="16" height="16" />
                        </td>
                        <td>
                            1234567890
                        </td>                        
                        <td>
                            192.168.0.1
                        </td>
                        <td>
                            北京市
                        </td>                        
                        <td>
                            10:30
                        </td>
                        <td>
                            20s
                        </td>
                        <td>
                        </td>                        
                        <td class="cName">
                            <a href="#">http://fanxian.bitauto.com/carsource/nb2977/nc102900/c201/</a>
                        </td>
                    </tr>  
                    <tr>
                        <td>
                            <img src="images/status2.png" width="16" height="16" />
                        </td>
                        <td>
                            1234567890
                        </td>                        
                        <td>
                            192.168.0.1
                        </td>
                        <td>
                            北京市
                        </td>                        
                        <td>
                            10:30
                        </td>
                        <td>
                            20s
                        </td>
                        <td>
                        </td>                        
                        <td class="cName">
                            <a href="#">http://fanxian.bitauto.com/carsource/nb2977/nc102900/c201/</a>
                        </td>
                    </tr>--%>
                </table>
            </div>
            <!--列表结束-->
            <div class="clearfix">
            </div>
            <div id="mycontent2" style="height:400px;"></div>
            <!--客户信息-->
            <%--<div class="cusInfo">
                <div class="title">
                    客户信息<span>来源：<a href="#">http://fanxian.bitauto.com/carsource/nb2977/nc102900/c201/</a></span></div>
                <div class="cusInfoC">
                    <div class="InfoC">
                        <table border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <th width="20%">
                                    姓名：
                                </th>
                                <td width="30%">
                                    李先生
                                </td>
                                <th width="20%">
                                    性别：
                                </th>
                                <td width="30%">
                                    男
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    电话：
                                </th>
                                <td>
                                    13411111111
                                </td>
                                <th>
                                    QQ：
                                </th>
                                <td>
                                    132121312
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    邮箱：
                                </th>
                                <td>
                                    1@1.com
                                </td>
                                <th>
                                    客户分类：
                                </th>
                                <td>
                                    个人
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    地区：
                                </th>
                                <td>
                                    广东省，深圳市
                                </td>
                                <th>
                                    客户ID：
                                </th>
                                <td>
                                    CB00381147
                                </td>
                            </tr>
                        </table>
                        <div class="btn">
                            <div class="right">
                                <input type="button" value="编辑信息" class="w80 gray" />
                                <input type="button" value="添加工单" class="w80 gray" /></div>
                        </div>
                    </div>
                    <div class="dialogue">
                        <p>
                            对话开始于: 09:53</p>
                        <div class="scroll_gd">
                            <div class="dh1">
                                <div class="title">
                                    张莹莹 09:53:31</div>
                                <div class="dhc">
                                    您好，欢迎您访问！请问有什么需要咨询？</div>
                            </div>
                            <div class="dh1">
                                <div class="title">
                                    张莹莹 09:53:31</div>
                                <div class="dhc">
                                    您好，欢迎您访问！请问有什么需要咨询？</div>
                            </div>
                            <div class="dh1">
                                <div class="title">
                                    张莹莹 09:53:31</div>
                                <div class="dhc">
                                    您好，欢迎您访问！请问有什么需要咨询？</div>
                            </div>
                        </div>
                        <div class="editC">
                            <div class="style_1">
                            </div>
                            <div class="ask_c">
                                <textarea name=""></textarea></div>
                        </div>
                        <div class="endmsg">
                            最后收到消息时间：2014-2-21 10:00:09<span class="right" style="*margin-top: -20px;"><input
                                type="button" value="发送" class="w80 gray" />
                            </span>
                        </div>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="clearfix">
                </div>
            </div>--%>
            <!--客户信息-->
        </div>
        <!--内容结束-->
        <div class="footer mt16">
            数据系统研发中心 任何建议和意见，请发邮件至：<a href="javascript:void(0);">ISDC@bitauto.com</a><br />
            CopyRight © 2000-2014 Bitauto,All Rights Reserved 版权所有 北京易车互联信息技术有限公司
        </div>
    </div>
</body>
</html>
