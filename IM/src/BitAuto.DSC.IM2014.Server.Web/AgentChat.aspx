<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentChat.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.AgentChat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>坐席聊天主页面(<%=this.Request["AgentIMID"] %>)</title>
    <style type="text/css">
        .mydiv
        {
            width: 400px;
            height: 300px;
            border: 1px solid #000000;
            display: none;
            position: absolute;
        }
        .mydiv h5
        {
            height: 30px;
            line-height: 30px;
            font-size: 14px;
            background-color: #316AC5;
            margin-top: 0px;
        }
        .mydiv h5 span
        {
            float: right;
            padding-right: 10px;
        }
        .mydiv p
        {
            padding: 10px;
        }
    </style>
    <script type="text/javascript" language="javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/AspNetComet.js"></script>
    <script type="text/javascript">
   
        var defaultChannel = null;
    
        function Connect()
        {
            if(defaultChannel == null)
            {
                defaultChannel = new AspNetComet("/DefaultChannel.ashx", "<%=this.Request["AgentIMID"] %>", "defaultChannel");
                defaultChannel.addTimeoutHandler(TimeoutHandler);
                defaultChannel.addFailureHandler(FailureHandler);
                defaultChannel.addSuccessHandler(SuccessHandler);
                //分配坐席
                defaultChannel.addAllocAgentForUserHandler(AllocAgentForUserHandler);
                
                //发送长连接请求
                defaultChannel.subscribe();
            }
        }

        //分配坐席
        function AllocAgentForUserHandler(privateToken, alias, message)
        {
           //让聊天窗口显示
           //$("#divTalk").css("display","")
           //把坐席标识放在隐藏域里
           //$("#hidto").val(message.AgentID);
           //提示网友
           //document.getElementById("messages").innerHTML += "坐席"+message.AgentID+"为您服务!<br/>";
       
           //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮
           //聊天层、聊天记录、发送消息、消息内容等元素都以网友ID结尾，前缀一致

           //把坐席标识放在隐藏域里
            $("#hidto").val(message.UserID);
            //把分配标识放在隐藏域
            $("#hidAllocID").val(message.AllocID);

           var UserIMID = message.UserID;
           var UserReferURL = message.UserReferURL;
           var AllocID = message.AllocID;
           var LocalIP = message.LocalIP;
           var Location = message.Location;
           var EnterTime = message.EnterTime;
           var WaitTime = message.WaitTime;
           OperateChatLayer(UserIMID,UserReferURL,AllocID);
        }

        //聊天层显示按钮新消息动画触发器
        var timerid;
        
        //接收消息事件方法
        //从消息发送人属性，message.from取网友ID
        function SuccessHandler(privateToken, alias, message)
        {
            
            //网友ID
            var UserIMID = message.c.f;
            var AllocID = message.AllocID;
            if(!isExistChatLayer(UserIMID) && UserIMID != "System"){
                var UserReferURL = "易车网";
                OperateChatLayer(UserIMID,UserReferURL,AllocID);
            }

            //alert(message.ct);

            //有网友新消息
            if(UserIMID != "System"){
                $("#chatbt"+ UserIMID).css("border", "1px dotted red");
                $("#chatbt"+ UserIMID).css("color", "red");
               //timerinfo = BindColorEvent(UserIMID);
            }

            //收到网友退出消息，清除网友聊天层及按钮
            if(message.n == "MLline"){
                $("#mydiv"+ UserIMID).remove();
                $("#chatbt"+ UserIMID).remove();
            }

            //收到消息内容
            //var msg = UserIMID + ":" + message.c.m + "\n";
            var rectime = message.ct;
            //根据网友ID更新聊天层消息最后接收时间
            //var msg = rectime + " 网友" + ":" + message.c.m + "\n";
            var tUserIMID= UserIMID.substr(UserIMID.length-4,4);
            var msg = "网友(" + tUserIMID + ")" + rectime + " \n " + message.c.m + "\n";
            //聊天层里的消息内容
            var msg2 = $("#messages"+ UserIMID).val();
            //原消息+新消息
            var msg3 = msg2 + msg;
            $("#messages"+ UserIMID).val(msg3);     
        }

        function FailureHandler(privateToken, alias, error)
        {
            //document.getElementById("messages").innerHTML += "error" + error + "<br/>";
            alert(error);
            //自动退出
            Quit();
        }
        function TimeoutHandler(privateToken, alias)
        {
            //alert("timeout");
            //document.getElementById("messages").innerHTML += "timeout<br/>";
        }    

        //消息接收人：recipient
        //消息内容：message
        function SendMessage(UserIMID)
        {
           //消息接收人
           var recipient = UserIMID;

           //会话标识，每分配一次生成新标识
           var AllocID = $("#message"+ UserIMID).attr("AllocID");

           //消息内容
           var message = $("#message"+ UserIMID).val();

            if(recipient == ""){
                alert("消息接收人不能为空!");
                return;
            }
            if(message == ""){
                alert("消息内容不能为空!");
                return;
            }

            //消息发送人
            var username = '<%=this.Request["AgentIMID"] %>';
            var parameters={
                    action:'sendmessage',
                    username:username,
                    SendToPublicToken:recipient,
                    AllocID:escape(AllocID),
                    message:message
                };
            $.ajax({
                    type: "POST",
                    url: "AjaxServers/Handler.ashx",
                    data: parameters,
                    success: function (msg) {
                        if(msg != ""){
                            //发送长连接请求
                            //Connect();
                            var r = JSON.parse(msg);
                            if (r != null && r.result == 'sendok') {//登录成功之后
                                //清空发送消息文本框                         
                                $('#message'+ UserIMID).val('');

                                //当前系统时间                                
                                var dtime = getSysTime();

                                //将发帝消息显示到聊天记录
                                //发送消息内容
                                //var msg = dtime + username + ":" + message + "\n";
                                var msg = "坐席(" + username + ")" + dtime + "\n " + message + "\n";
                                //聊天层里的消息内容
                                var msg2 = $("#messages"+ UserIMID).val();
                                //原消息+新消息
                                var msg3 = msg2 + msg;
                                $("#messages"+ UserIMID).val(msg3);
                            }
                            else
                                alert('发送消息失败：' + r.result);
                        }
                        else{
                            alert("您的网络出现异常，请检查网络后重新发送！");
                            //初始化长连接请求
                            //defaultChannel = null;
                        }
                    }
                });
        
        }
        function onKeyPress(e)
        {
            var keyCode = null;

            if(e.which)
                keyCode = e.which;
            else if(e.keyCode) 
                keyCode = e.keyCode;
            
            if(keyCode == 13) 
            {
                SendMessage();
                return false;
            }
            return true;
        }

        //登录系统
        function Login(){
            var parameters={
                action:'init',
                username:$("#userid").val(),
                message:$('#message').val()
            };

            $.ajax({
                    type: "POST",
                    url: "AjaxServers/Handler.ashx",
                    data: parameters,
                    success: function (msg) {
                        var r = JSON.parse(msg);
                        if (r != null && r.result == 'sendok') {//登录成功之后

                            document.getElementById("messages").innerHTML += $('#message').val() + "<br/>";
                            $('#message').val('');
                        }
                        else
                            alert('登录失败：' + r.result);
                    }
                    });

            Connect();
        }

        //退出系统
        function Quit(){
            var parameters={
                action:'closechat',
                username:'<%=this.Request["AgentIMID"] %>'
            };

            $.ajax({
                    type: "POST",
                    url: "AjaxServers/Handler.ashx",
                    data: parameters,
                    success: function (msg) {
                        var r = JSON.parse(msg);
                        if (r != null && r.result == 'sendok') {//登录成功之后                            
                            alert("退出成功");
                            window.location="AgentDefault.aspx";
                        }
                        else{
                            alert('退出失败：' + r.result);
                            window.location="AgentDefault.aspx";
                            }
                    }
                    });
        }

        //更改坐席状态
        function SetAgentState(){
            //取坐席状态值
            var state = $("#selState").val();
            var parameters={
                action:'SetAgentState',
                username:'<%=this.Request["AgentIMID"] %>',
                AgentState:state
            };

            $.ajax({
                    type: "POST",
                    url: "AjaxServers/Handler.ashx",
                    data: parameters,
                    success: function (msg) {
                        var r = JSON.parse(msg);
                        if (r != null && r.result == 'sendok') {                            
                            alert("更改状态成功!");                            
                        }
                        else{
                            alert('更改状态失败：' + r.result);
                            }
                    }
                    });
        }

        //每分配一个网友，生成一个聊天层，生成一个聊天层显示按钮
        //聊天层、聊天记录、发送消息、消息内容等元素都以网友ID结尾，前缀一致
        //UserIMID:网参ID，UserReferURL:访客来源,AllocID:会话标识
        function GenerateChatLayer(UserIMID,UserReferURL,AllocID){
            var tUserIMID= UserIMID.substr(UserIMID.length-4,4);
            var html = "";
            html += "<div id='mydiv"+ UserIMID +"' class='mydiv'>";
            html += "<h5>";
            html += "聊天层(网友:"+ tUserIMID +",来源:"+ UserReferURL +")<span id='span"+ UserIMID +"'>关闭</span>";
            html += "</h5>";
            html += "<p>";
            html += "<textarea id='messages"+ UserIMID +"' readonly='true' style='width:380px;height:300;' rows='9'></textarea>";
            html += "</p>";
            html += "<p style='padding:0px;'>";
            html += "<input type='text' id='message"+ UserIMID +"' userimid='"+ UserIMID +"' AllocID='"+ AllocID +"' style='width:340px;'/>";
            //html += "<textarea rows='3' style='width:300px;' id='message"+ UserIMID +"' userimid='"+ UserIMID +"' AllocID='"+ AllocID +"'/>";
            html += "<input type='button' id='sendmsg"+ UserIMID +"' value='发送' />";
            html += "</p>";
            html += "</div>";

            $("#form1").append(html);

            //生成网友聊层显示按钮
            html = "<input type='button' id='chatbt"+ UserIMID +"' value='网友"+tUserIMID +"' />";
            $("#pButtons").append(html);

            //绑定显示聊天层方法
            $("#chatbt"+ UserIMID).click(function(){
                ShowChatLayer(UserIMID);

                //解除新消息动画提示
                //ClearColorEvent(timerid,UserIMID);
                $("#chatbt"+ UserIMID).css("border", "");
                $("#chatbt"+ UserIMID).css("color", "");
            });

            //绑定发送消息框回车事件
            $("#message"+ UserIMID).keypress(function(e){
                var keyCode = null;

                if(e.which)
                    keyCode = e.which;
                else if(e.keyCode) 
                    keyCode = e.keyCode;
            
                if(keyCode == 13) 
                {
                    var UserIMID = $(this).attr("userimid");
                    SendMessage(UserIMID);
                }
            });
        }

        //为网友聊层绑定拖拽移动事件
        function BindDragAction(UserIMID){
            $("#mydiv"+ UserIMID).mousedown(function (e)//e鼠标事件  
                {
                    $(this).css("cursor", "move"); //改变鼠标指针的形状  

                    var offset = $(this).offset(); //DIV在页面的位置  
                    var x = e.pageX - offset.left; //获得鼠标指针离DIV元素左边界的距离  
                    var y = e.pageY - offset.top; //获得鼠标指针离DIV元素上边界的距离  
                    $(document).bind("mousemove", function (ev)//绑定鼠标的移动事件，因为光标在DIV元素外面也要有效果，所以要用doucment的事件，而不用DIV元素的事件  
                    {
                        $("#mydiv"+ UserIMID).stop(); //加上这个之后  

                        var _x = ev.pageX - x; //获得X轴方向移动的值  
                        var _y = ev.pageY - y; //获得Y轴方向移动的值  

                        $("#mydiv"+ UserIMID).animate({ left: _x + "px", top: _y + "px" }, 10);
                    });

                });

                $(document).mouseup(function () {
                    $("#mydiv"+ UserIMID).css("cursor", "default");
                    $(this).unbind("mousemove");
                })
        }

        //绑定发送消息方法、聊天层关闭方法
        function BindSendMessage(UserIMID){
           //消息发送按钮           
           var sendmsg = $("#sendmsg"+ UserIMID);
           //sendmsg.bind("click",SendMessage(UserIMID));
           sendmsg.click(function(){
            SendMessage(UserIMID);
           });

           //聊天层关闭方法
           $("#span"+ UserIMID).click(function () {
                $("#mydiv"+ UserIMID).hide();
            });
        }

        //显示消息聊天层
        function ShowChatLayer(UserIMID){
            $("#mydiv"+ UserIMID).show();
        }

        //封装聊天层生成、拖拽、发送消息等操作
        function OperateChatLayer(UserIMID,UserReferURL,AllocID){
            GenerateChatLayer(UserIMID,UserReferURL,AllocID);
            BindDragAction(UserIMID);
            BindSendMessage(UserIMID);
            ShowChatLayer(UserIMID);
        }

        //判断网友聊天层是否存在
        function isExistChatLayer(UserIMID) {
            var obj = $("#mydiv"+UserIMID);
            if(obj.length>0){
                return true;
            }
            else{
                return false;
            }
        }
        
    //获取系统时间
    function getSysTime()
    {
        var now=new Date();
        var year=now.getYear();
        var month=now.getMonth();
        var day=now.getDate();
        var hours=now.getHours();
        var minutes=now.getMinutes();
        var seconds=now.getSeconds();
        return parseInt(year+1900)+"-"+parseInt(month+1)+"-"+day+" "+hours+":"+minutes+":"+seconds+"";        
    }    


        //禁止刷新
        //绑定发送消息框回车事件
        function RefreshDisable(){
        $(document).keypress(function(e){
                var keyCode = null;

                if(e.which)
                    keyCode = e.which;
                else if(e.keyCode) 
                    keyCode = e.keyCode;
            
                if(keyCode == 116) 
                {
                    return false;
                }
            });
        }

        //禁用右键、文本选择功能、刷新   
        $(document).bind("contextmenu",function(){return false;});    
        $(document).bind("selectstart",function(){return false;});    
        $(document).keydown(function(){return key(arguments[0])});     
     
        //按键时提示警告    
       function key(e){    
            var keynum;    
            if(window.event) // IE    
              {    
                keynum = e.keyCode;    
              }    
            else if(e.which) // Netscape/Firefox/Opera    
              {    
                keynum = e.which;    
              }    
            if(keynum == 116){ alert("禁止刷新！");return false;}    
        } 
        
        //当前页关闭事件
        function ColseMe(){
            window.onbeforeunload = function () {
                //if(confirm('你确定要关闭吗？')){
                    Quit();
                //};
            }      
        }
       
       
       function AddBorderColor(UserIMID) {
            //$("#chatbt").css("border", "5px dotted red");
            //$("#chat").css("color", "red");

//            if ($("#chatbt"+ UserIMID).css("borderColor") == "red") {
//                $("#chatbt"+ UserIMID).css("borderColor", "yellow");
//            }
//            else{
//            $("#chatbt"+ UserIMID).css("borderColor", "red");
//            }
            alert("hhh");
        }

        function BindColorEvent(UserIMID) {
            timerid = window.setInterval(AddBorderColor(UserIMID), 500);          
        } 

        function ClearColorEvent(UserIMID){
            window.clearInterval(timerid);
            $("#chatbt"+UserIMID).css("border", "");
        }

        
        
        
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            
            ColseMe();
            $("#chat").click(function () {
                var UserIMID = "1040";
                var UserReferURL = "易车网";
                var AllocID = "001";
                if (!isExistChatLayer(UserIMID)) {
                    OperateChatLayer(UserIMID, UserReferURL, AllocID);
                }
                else {
                    ShowChatLayer(UserIMID);
                }
            });

            //禁止F5刷新
            //RefreshDisable();
        });
        
        
    </script>
</head>
<body onload="Connect()">
    <form id="form1" runat="server">
    <input type="hidden" id="hidto" />
    <input type="hidden" id="hidUserReferURL" />
    <input type="hidden" id="hidAllocID" />
    <div id="divTalk" style="display: ''">
        <%--<p>
            <input type="hidden" id="hidto" />
            <div id="messages" style="width: 500px; height: 300px; border: 1px solid silver;
                overflow: auto">
            </div>
            <input type="text" id="message" onkeypress="return onKeyPress(event)" />&nbsp;
            <input type="button" onclick="SendMessage()" value="Send Message" />
        </p>--%>
        <p id="pButtons">
            <input type="button" onclick="Quit()" value="退出" />
            <label>状态：</label>
            <select id="selState" runat="server" onchange="SetAgentState()">
                <option value="-1">请选择</option>
            </select>
            <a href="ChatMessageLog/ChatMessageLog.aspx" target="_blank">查看聊天记录</a>
            <%--<input type="button" id="chat" value="聊天层" style="border:1px dotted red;color:red"/>--%>
        </p>
    </div>
    <%--<div id="mydiv0" name="InitializationDiv" class="mydiv">
        <h5>
            弹出层标题<span id="span">关闭</span></h5>
        <p>
            <textarea id="messages" readonly="true" style="width:380px;height:300;" rows="9"></textarea> </p>
            <p style="padding:0px;"><input type="text" id="message" onkeypress="return onKeyPress(event)" /><input type="button" onclick="SendMessage('hello')" value="发送" /></p>
    </div>
    <div id="mydiv2" class="mydiv">
        <h5>
            弹出层标题<span id="span2">关闭</span></h5>
        <p>
            <textarea id="messages2" readonly="true" style="width:380px;height:300;" rows="9"></textarea> </p>
            <p style="padding:0px;"><input type="text" id="message2" onkeypress="return onKeyPress(event)" /><input type="button" onclick="SendMessage('hello2')" value="发送" /></p>
    </div>--%>
    </form>
</body>
</html>
