<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineService.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.OnlineService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <title>在线客服</title>
    <link type="text/css" href="css/css.css" rel="stylesheet" />
    <link type="text/css" href="css/style.css" rel="stylesheet" />
    <link type="text/css" href="css/uploadify.css" rel="stylesheet" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/AspNetComet.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Setup/UdCapture.js" charset="utf-8"></script>
    <%--    <script type="text/javascript" src="OnlineService.js" charset="utf-8"></script>--%>
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        loadJS("OnlineService");
    </script>
    <script type="text/javascript">
        //网友标识
        var privatetaken = "";
        var visitid = "";
        //工号
        var number = "";
        //是否已做满意度
        var isManyi = false;
        $(document).ready(function () {
            //让按钮不可用
            Chatdisabled(0);
            //初始化上次控件
            InitUploadify();
            //设置聊天窗口随屏幕大小变化 
            setbigsmall();
            //从cookie取快捷键
            GetQuick();
            //初始化
            StartChat();
            //加载常见问题
            f_getFreProblemlist();
        });


        //初始化
        function StartChat() {
            var UserReferURL = '<%=UserReferURL%>';
            var EPTitle = '<%=EPTitle%>';
            var EPKey = '<%=EPKey%>';
            var usertype = "2";
            var EPPostURL = '<%=EPPostURL%>';

            var pody = { action: 'init', EPTitle: EPTitle, FromPrivateToken: escape(privatetaken), UserReferURL: UserReferURL, EPKey: EPKey, EPPostURL: EPPostURL, usertype: escape(usertype) };
            AjaxPost('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 if (msg != "") {
                     var r = JSON.parse(msg);
                     if (r != null && r.result == 'loginok') {
                         //是否初始化过标识
                         $("#isInit").val("1");
                         //网友端标识，通过初始化返回
                         privatetaken = r.loginid;
                         //网友访问id
                         visitid = r.visitid;
                         $("#agentAllocat").html("您好，欢迎使用易车在线客服。稍候会有客服为您服务...");
                         defaultChannel = null;
                         Connect();
                     }
                     else if (r != null && r.result == 'exists') {
                         SetBeforeunload(false, onbeforeunload_handler);
                         alert('您已登录！');
                         window.opener = null; window.open('', '_self'); window.close();
                     }
                     else if (r != null && r.result == 'servicetimeout') {
                         $("#agentAllocat").html("您好，欢迎使用易车在线客服<br/>客服工作时间为周一至周五早<%=ST.ToString()%>至晚<%=ET.ToString()%>，目前为非工作时间，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>");
                         //网友访问id
                         visitid = r.visitid;
                         $("#divagentAllocat").css("display", "");
                         $("#Rmessages").css("height", "85%");
                     }
                     else if (r != null && r.result == 'eploginerror') {
                         SetBeforeunload(false, onbeforeunload_handler);
                         alert('登录验证失败！');
                         window.opener = null; window.open('', '_self'); window.close();
                     }
                 } else {
                     SetBeforeunload(false, onbeforeunload_handler);
                     alert('登录验证失败！');
                     window.opener = null; window.open('', '_self'); window.close();
                 }
             });


            //            $.ajax({
            //                type: "POST",
            //                url: "AjaxServers/Handler.ashx",
            //                data: "action=init&EPTitle=" + EPTitle + "&usertype=" + escape(usertype) + "&UserReferURL=" + UserReferURL + "&EPKey=" + EPKey + "&FromPrivateToken=" + escape(privatetaken) + "&EPPostURL=" + EPPostURL,
            //                success: function (msg) {
            //                    var r = JSON.parse(msg);
            //                    //alert(r.result);
            //                    if (r != null && r.result == 'loginok') {
            //                        //是否初始化过标识
            //                        $("#isInit").val("1");
            //                        //网友端标识，通过初始化返回
            //                        privatetaken = r.loginid;
            //                        //网友访问id
            //                        visitid = r.visitid;
            //                        $("#agentAllocat").html("您好，欢迎使用易车在线客服。稍候会有客服为您服务...");
            //                        defaultChannel = null;
            //                        Connect();
            //                    }
            //                    else {
            //                        SetBeforeunload(false, onbeforeunload_handler);
            //                        alert('登录验证失败！');
            //                        window.opener = null; window.open('', '_self'); window.close();
            //                    }

            //                }
            //            });

        }



        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
//            if (name == "one" && n == 3) {
//                getEmotionInfo(cursel);
//            }
        }
        function getEmotionInfo(ecategory) {

            if ($("#con_one_" + ecategory + " table tr").length <= 0) {

                $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getemotioninfobyecategory', ECategory: ecategory }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);

                        if (jsonData != "") {
                            var temphtml = "<tr>";
                            $.each(jsonData.root, function (idx, item) {
                                temphtml += '<td><img class="emotionImgs" title="' + item.EText + '" src="' + item.EUrl + '" width="24" height="14"/></td>';
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
        }
        //设置刷新，关闭页面警告，并执行onbeforeunload_handler()
        SetBeforeunload(true, onbeforeunload_handler);
        //长链接请求
        var defaultChannel = null;
        function Connect() {
            if (defaultChannel == null) {
                defaultChannel = new AspNetComet("/DefaultChannel.ashx", privatetaken, "defaultChannel");
                defaultChannel.addTimeoutHandler(TimeoutHandler);
                defaultChannel.addFailureHandler(FailureHandler);
                defaultChannel.addSuccessHandler(SuccessHandler);
                //分配坐席
                defaultChannel.addAllocAgentForUserHandler(AllocAgentForUserHandler);
                //坐席全忙
                defaultChannel.addMAllBussyHandler(MAllBussyHandler);
                //坐席离开
                defaultChannel.addAgentLeaveHandler(AgentLeaveHandler);
                //客户发起满意度评价消息
                defaultChannel.addSatisfactionHandLer(SatisfactionHandLer);
                //发送文件
                defaultChannel.addSendFileHandLer(SendFileHandLer);
                //发送排队信息消息
                defaultChannel.addQueueSortHandLer(QueueOrderHandLer);
                //发送长连接请求
                defaultChannel.subscribe();
            }
        }
        
    </script>

    <script type="text/javascript">

        var _positionIntervalValue;
        var _togglevalue = true;
        var _isbuttonclick = false;
        $(function () {
            $("#bq_listSH").click(function () {
                if (_togglevalue) {
                    document.getElementById('infoIframe').contentWindow.setEmotionTab('one', 2, 3);
                    clearInterval(_positionIntervalValue);
                    var pos = findPosition($(this).get(0));
                    var yPos = pos.yy - 270;
                    var xPos = pos.x - 30 + $(this).width() / 2;
                    $("#infoBox").css({ "left": xPos, "top": yPos }).show("fast");

                    _positionIntervalValue = setInterval("reinitIframe()", 800);
                    _togglevalue = false;
                }
                else {
                    $("#infoBox").hide("fast");
                    clearInterval(_positionIntervalValue);
                    _togglevalue = true;
                }
                _isbuttonclick = true;
            });
        });
        function changeImgLayerState() {
            if (_togglevalue) {
                clearInterval(_positionIntervalValue);

                var pos = findPosition($("#bq_listSH").get(0));
                var yPos = pos.yy - 270;
                var xPos = pos.x - 30 + $("#bq_listSH").width() / 2;
                $("#infoBox").css({ "left": xPos, "top": yPos }).show("fast");

                _positionIntervalValue = setInterval("reinitIframe()", 800);
                _togglevalue = false;
            }
            else {
                $("#infoBox").hide("fast");
                clearInterval(_positionIntervalValue);
                _togglevalue = true;
            }

        }
        function findPosition(oElement) {
            var x2 = 0;
            var y2 = 0;
            var width = oElement.offsetWidth;
            var height = oElement.offsetHeight;
            if (typeof (oElement.offsetParent) != 'undefined') {
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                }
                x2 = posX + width;
                y2 = posY + height;
                return { x: posX, y: posY, xx: x2, yy: y2 };
            } else {
                x2 = oElement.x + width;
                y2 = oElement.y + height;
                return { x: oElement.x, y: oElement.y, xx: x2, yy: y2 };
            }
        }
        function reinitIframe() {
            var pos = findPosition($("#bq_listSH").get(0));
            var yPos = pos.yy - 270;
            var xPos = pos.x - 30 + $("#bq_listSH").width() / 2;
            $("#infoBox").css({ "left": xPos, "top": yPos });

            var iframe = document.getElementById("infoIframe");
            try {
                var bHeight = iframe.contentWindow.document.body.scrollHeight;
                var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
                var height = Math.max(bHeight, dHeight);
                iframe.height = height;

                
            } 
            catch (ex) { }
        }
        $(function () {
            $(document).click(function () {
                if (!_isbuttonclick) {
                    $("#infoBox").hide("fast");
                    clearInterval(_positionIntervalValue);
                    _togglevalue = true;
                }
                else {

                    _isbuttonclick = false;
                }
            });
        });
    </script>
</head>
<body onclick="messageFlicker.clear();" onresize="ChangeBigSmall()">
    <div id="bodyDIV" class="online_kf">
        <input type="hidden" id="hidto" value="" />
        <input type="hidden" id="isInit" value="0" />
        <input type="hidden" id="hidAllocID" value="" />
        <input type="hidden" id="hidIsJiePing" value="" />
        <div class="title_kf">
            在线客服<span><a href="#"><img src="images/c_btn.png" border="0" alt="关闭" style="cursor: pointer"
                onclick="CloseWindow(0)" /></a></span></div>
        <div id="divcontent" class="content_kf">
            <!--左开始-->
            <div class="left_c" id="divleft">
                <div class="answer">
                    <div id="divagentAllocat" class="fix_gd">
                        <p id="agentAllocat" class="hs">
                        </p>
                    </div>
                    <div class="scorll_gd scorll_gd2" style="height: 85%" id="Rmessages">
                    </div>
                </div>
                <div class="ask">
                    <div class="style_1">
                        <span class="kind_detail"><a href="#" id="bq_listSH" title="表情" class="fw_bq"></a><a
                            href="#" id="HistroyList" title="历史记录" class="fw_jl"></a><a href="#" id="fileUpload"
                                title="上传文件" class="fw_sc">
                                <input type="file" id="uploadify" name="uploadify" /></a><a href="#" id="btnCapture"
                                    title="截屏" class="fw_jp"></a><a href="#" id="EMYiChe" title="满意度" class="fw_my"></a><a
                                        id="btnReload" href="javascript:f_loadPlugin()" class="btn" style="display: none">
                                        正在进行插件安装，安装成功后请点击这里...</a></span>
                    </div>
                    <div class="ask_c" style="height: 80%">
                        <div class="ask_t" style="height: 70%" contenteditable="true" id="Smessage" onkeypress="return onKeyPress(event)">
                        </div>
                    </div>
                </div>
                <div id="fileQueue" style="z-index: 9999; position: absolute; top: 40%; right: 40%;">
                </div>
                <div class="btn">
                    <div class="left gray">
                        发送快捷键：<label onclick="SetQuick('1')">
                            <input name="RadioQuick" id="radenter" type="radio" value="1" /><span> Enter</span></label>&nbsp;<label
                                onclick="SetQuick('2')"><input name="RadioQuick" id="radctrlenter" type="radio" value="2" /><span>
                                    Ctrl + Enter</span></label></div>
                    <div id="logInfo" style="display: none">
                    </div>
                    <div class="right">
                        <input type="button" value="结束对话" class="w80 endBtn" onclick="CloseWindow(0)" />
                        <input type="button" value="发送" id="btnSend" onclick="SendMessage()" class="w80" /></div>
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
                    <div id="divquestion">
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix">
        </div>
    </div>
    <!--表情弹出-->
    <div id="infoBox" style="width: 355px; height: 236px; background-color: #FFFFFF;
        text-align: center; padding: 0px; margin: 0px; position: absolute; display: none;">
        <iframe id="infoIframe" src="EmotionForm.aspx" scrolling="no" frameborder="0" width="351">
        </iframe>
    </div>
    <!--表情弹出-->
</body>
</html>
