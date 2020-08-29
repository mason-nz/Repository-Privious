<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineService.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.OnlineService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <title>在线客服</title>
    <link type="text/css" href="css/css.css" rel="stylesheet" />
    <link type="text/css" href="css/uploadify.css" rel="stylesheet" />
    <link href="css/emotionstyle.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/OnLineServiceCom.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Setup/UdCapture.js" charset="utf-8"></script>
    <script type="text/javascript" src="Scripts/Enum/Area2.js"></script>
    <script type="text/javascript" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        loadJS("AspNetComet");
        loadJS("common");
        loadJS("OnlineService");
        //        loadJS("OnlineServiceCom");
    </script>
    <script type="text/javascript">

        var ONS = {
            HdlUrl: "/post.action",
            Poll: "/poll"
        };
        //网友标识
        var privatetaken = "";
        var visitid = "";
        var BQPop = null;
        //工号
        var number = "";
        //是否已做满意度
        var isManyi = false;
        //业务线log
        var logoURL = '<%=logoURL%>';
        //业务线头像
        var AgentHeadURL = '<%=AgentHeadURL %>';
        //网友头像
        var UserHeadURL = '<%=UserHeadURL %>';
        var SourceType = '<%=SourceType %>';
        var loginid = "";
        //是否离开
        var isleave = "0";
        var LeaveMessage = "0";
        var ischeck = "0";
        var guidstr = '<%=GuidStr%>';
        $(document).ready(function () {
            TuPianYanZhengPopup();
        });

        function TuPianYanZhengPopup() {
            $.openPopupLayer({
                name: "TuPianYanZhengPopup",
                url: "PictureCheck.aspx?GuidStr=" + guidstr + "&r=" + Math.random(),
                beforeClose: function () {
                    if (ischeck == "1") {
                        //让按钮不可用
                        Chatdisabled(0);
                        //设置聊天窗口随屏幕大小变化 
                        setbigsmall();
                        //从cookie取快捷键
                        GetQuick();
                        //初始化
                        StartChat();
                        //加载常见问题
                        GetServerTime();
                        f_getFreProblemlist('<%=SourceType %>');
                        $('#bq_listSH').click(BQIMGClick);
                    }
                }
            });
        }

        //初始化
        function StartChat() {
            LeaveMessage = "0";
            var UserReferURL = '<%=UserReferURL%>';
            var EPTitle = '<%=EPTitle%>';
            var LoginID = '<%=LoginID%>';
            var usertype = "2";
            var EPPostURL = '<%=EPPostURL%>';

            if (SourceType == "") {
                alert('登录验证失败！');
                SetBeforeunload(false, onbeforeunload_handler);
                window.opener = null; window.open('', '_self'); window.close();
            }
            else {
                //                var islog = "0";
                //                if (islog == "1") {
                //                    alert('您已登录！');
                //                    SetBeforeunload(false, onbeforeunload_handler);
                //                    window.opener = null; window.open('', '_self'); window.close();
                //                }
                if ('<%=CheckDomain()%>' == "0") {
                    alert('验证失败！');
                    SetBeforeunload(false, onbeforeunload_handler);
                    window.opener = null; window.open('', '_self'); window.close();
                }

                var ProvinceID = GetProvinceIDByCityID(parseInt(bit_locationInfo.cityId));
                var pody = { action: 'init', EPTitle: EPTitle, FromPrivateToken: escape(privatetaken), UserReferURL: UserReferURL, LoginID: escape(LoginID), SourceType: SourceType, EPPostURL: EPPostURL, usertype: escape(usertype), CityID: escape(bit_locationInfo.cityId), ProvinceID: escape(ProvinceID), WYGUID: escape(guidstr) };
                AjaxPost('AjaxServers/Handler.ashx', pody, null,
                    function (msg) {
                        if (msg != "") {
                            var r = JSON.parse(msg);
                            if (r != null && r.result == 'loginok') {
                                //是否初始化过标识
                                $("#isInit").val("1");
                                //网友内存中的标识，通过初始化返回
                                privatetaken = r.loginid;
                                //网友访问id
                                visitid = r.visitid;
                                //网友标识
                                loginid = r.Cookieid;

                                //请求进入队列
                                CominQuene(privatetaken, SourceType);
                                //
                            }
                            else if (r != null && r.result == 'exists') {
                                SetBeforeunload(false, onbeforeunload_handler);
                                alert('您已登录！');
                                window.opener = null; window.open('', '_self'); window.close();
                            }
                            else if (r != null && r.result == 'moreperson') {
                                SetBeforeunload(false, onbeforeunload_handler);
                                alert('目前没有空闲坐席，请稍后重试！');
                                window.opener = null; window.open('', '_self'); window.close();
                            }
                            else if (r != null && r.result == 'servicetimeout') {
                                $("#divagentAllocat").html("<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_js.png'>您好，欢迎使用在线客服系统。</div></div><div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_js.png'>客服工作时间为周一至周日早<%=ST.ToString()%>至晚<%=ET.ToString()%>，目前为非工作时间，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>。</div></div>");
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
            }


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
            if (cursel == 2) {
                f_getFreProblemlist('<%=SourceType %>');
            }
        }
        function getEmotionInfo(ecategory) {

            if ($("#con_one_" + ecategory + " table tr").length <= 0) {

                $.get("/AjaxServers/LayerDataHandlerBefore.ashx", { Action: 'getemotioninfobyecategory', ECategory: ecategory }, function (data) {
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
                defaultChannel = new AspNetComet("/poll", privatetaken, "defaultChannel");
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

                //转接坐席消息
                defaultChannel.addTransferHandLers(TransferHandLer);

                //发送长连接请求
                defaultChannel.subscribe();
            }
        }

    </script>
    <script type="text/javascript">

     
    </script>
</head>
<body style="overflow: hidden;" onclick="messageFlicker.clear();" onresize="ChangeBigSmall()">
    <div id="bodyDIV" style="position: absolute; top: 0; bottom: 0px; width: 100%; *overflow-y: hidden;
        *width: 100%; *height: 98%; *position: absolute; *top: 0; *bottom: 0px; height: 100%;">
        <input type="hidden" id="hidto" value="" />
        <input type="hidden" id="isInit" value="0" />
        <input type="hidden" id="hidAllocID" value="" />
        <input type="hidden" id="hidIsJiePing" value="" />
        <div id="win" style="position: absolute; top: 0; height: 100%; width: 100%">
            <!-- 【建议】设置最小高度和最小宽度 -->
            <div style="width: 100%; margin-top: 0px; margin-bottom: 0px; left: 0px;">
                <div class="title_kf_online">
                    <img src="<%=logoURL %>" style="position: relative; top: -2px; *top: -5px; margin-left: 0px;" />&nbsp;
                    |&nbsp; 在线客服<span style="float: right"><a href="#"><img src="images/close2.png" border="0"
                        alt="关闭" style="cursor: pointer;" onclick="CloseWindow(0)" /></a></span></div>
                <div id="divcontent" class="content_kf content_kf_online">
                    <div style="width: auto; overflow: hidden; height: 100%">
                        <div style="position: relative; background-color: #e4e4e4; right: 25px; margin-left: 4px;
                            height: 100%">
                            <!--左开始-->
                            <div class="left_c" id="divleft" style="height: 100%; overflow: hidden;">
                                <div class="answer" id="divAnswer" style="position: relative; overflow: auto; *overflow-x: hidden">
                                    <div id="divagentAllocat" style="">
                                        <div class='an_tip' style='margin-bottom: 10px;'>
                                            <div class="an_info">
                                                <img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服。
                                            </div>
                                        </div>
                                    </div>
                                    <div class="scorll_gd scorll_gd2" style="overflow-x: hidden; height: 85%; position: relative;"
                                        id="Rmessages">
                                    </div>
                                </div>
                                <div class="ask">
                                    <div class="style_1">
                                        <span class="kind_detail"><a href="#" id="bq_listSH" title="表情" class="fw_bq" style="display: none">
                                        </a><a href="#" id="HistroyList" onclick="HistroyMore()" title="历史记录" style="display: none"
                                            class="fw_jl"></a><a href="#" id="btnCapture" title="截屏" class="fw_jp" onclick="jieping()"
                                                style="display: none"></a><a href="#" id="EMYiChe" onclick="addSatisfaction()" title="满意度"
                                                    class="fw_my" style="display: none"></a><a id="btnReload" href="javascript:f_loadPlugin()"
                                                        class="btn" style="display: none">正在进行插件安装，安装成功后请点击这里...</a></span>
                                    </div>
                                    <div class="ask_c" style="height: 80%">
                                        <div class="ask_t" style="height: 70%; word-wrap: break-word; word-break: break-all;"
                                            contenteditable="true" id="Smessage" onkeypress="return onKeyPress(event)">
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
                                        <input type="button" value="发送" id="btnSend" onclick="SendMessage()" class="w80" />
                                    </div>
                                </div>
                            </div>
                            <!--右开始-->
                            <div class="right_c" id="divright">
                                <div id="Tab1">
                                    <div class="Menubox">
                                        <ul>
                                            <%if (!string.IsNullOrEmpty(ShowPageTitle))
                                              { %>
                                            <li id="one1" onclick="setTab('one',1,2)" class="hover">
                                                <%= ShowPageTitle%></li>
                                            <%} %>
                                            <li id="one2" onclick="setTab('one',2,2)">常见问题</li>
                                        </ul>
                                    </div>
                                    <div class="Contentbox" style="padding: 0">
                                        <!--业务系统-->
                                        <%if (!string.IsNullOrEmpty(ShowPageUrl))
                                          {%>
                                        <div id="con_one_1" class="hover">
                                            <iframe src="<%=ShowPageUrl%>" style="border: 0; width: 100%; height: 100%"></iframe>
                                        </div>
                                        <%} %>
                                        <!--业务系统-->
                                        <!--常见问题-->
                                        <div id="con_one_2" <% if(!string.IsNullOrEmpty(ShowPageUrl)){%>style="display:none;"
                                            <%}else{%>style="display:block;" <%}%>>
                                            <div id="divquestion" style="height: auto; line-height: 20px; overflow: auto; _padding-top: -50px;"
                                                class="question">
                                            </div>
                                        </div>
                                        <!--常见问题-->
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
