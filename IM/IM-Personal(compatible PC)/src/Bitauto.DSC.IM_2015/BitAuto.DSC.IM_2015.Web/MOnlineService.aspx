<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MOnlineService.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.MOnlineService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>在线客户</title>
    <link href="Mcss/style.css?r=1" rel="stylesheet" type="text/css">
    <link href="css/emotionstyle.css?r=3" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Scripts/Enum/Area2.js"></script>
    <script type="text/javascript" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        loadJS("MAspNetComet");
        loadJS("common");
        loadJS("MOnlineService");
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
            //TuPianYanZhengPopup();
            //让按钮不可用
            Chatdisabled(0);
            //设置聊天窗口随屏幕大小变化 
            setbigsmall();
            //从cookie取快捷键
            //GetQuick();
            //初始化
            StartChat();
            //加载常见问题
            GetServerTime();
            //f_getFreProblemlist('<%=SourceType %>');
            //$('#bq_listSH').click(BQIMGClick);
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
                        //GetQuick();
                        //初始化
                        StartChat();
                        //加载常见问题
                        GetServerTime();
                        //f_getFreProblemlist('<%=SourceType %>');
                        //$('#bq_listSH').click(BQIMGClick);
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
                                $("#divagentAllocat").html("<div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>您好，欢迎使用在线客服系统。</div><div class='paidui'><img width='16' height='16' src='Mimages/status1.png'>客服工作时间为周一至周日早<%=ST.ToString()%>至晚<%=ET.ToString()%>，目前为非工作时间，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>。</div>");
                                //网友访问id
                                visitid = r.visitid;
                                //$("#divagentAllocat").css("display", "");
                                //$("#Rmessages").css("height", "85%");
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
                //                //发送文件
                //                defaultChannel.addSendFileHandLer(SendFileHandLer);
                //发送排队信息消息
                defaultChannel.addQueueSortHandLer(QueueOrderHandLer);

                //转接坐席消息
                defaultChannel.addTransferHandLers(TransferHandLer);

                //发送长连接请求
                defaultChannel.subscribe();
            }
        }

    </script>
</head>
<body onclick="messageFlicker.clear();" onresize="ChangeBigSmall()">
    <!--header-->
    <header><a href="#" onclick="javascript:window.history.go(-1);return false;" class="return">返回</a><span class="logo"><img src="Mimages/offline-im.png" id="imgOnline" width="20" height="20">在线客服</span></header>
    <!--/header-->
    <!--content -->
    <div class="bt_page" id="divcontent">
        <input type="hidden" id="hidto" value="" />
        <input type="hidden" id="isInit" value="0" />
        <input type="hidden" id="hidAllocID" value="" />
        <input type="hidden" id="hidIsJiePing" value="" />
        <div id="divagentAllocat" class="carform carform_dh" style="position: relative; overflow: auto;
            *overflow-x: hidden;">
            <div class="paidui">
                <img src="Mimages/status1.png" width="16" height="16">您好，欢迎使用易车在线客服系统！</div>
        </div>
        <!--底部搜索-->
        <section class="m-search">
                            <section class="m-search">
                            	 <span class="bq-btn-box"><a class="bq-btn-sty" id="bq_listSH" href="#"></a></span>
                                 <div id="txtkeywordbottom" class="input_div"></div>
                                 <input type="submit" id="btnSend" value="" onclick="SendMessage()">
                            </section>
                             <!--满意度-->
                            <div class="manyidu" >
                                <div class="manyidu_left"><a href="#" id="divManyiDu"><img src="Mimages/myd.png" width="40" height="40" border="0"><span>满意度</span></a></div>
                            </div>
                            <!--满意度-->
                
                    </section>
        <!--底部搜索-->
    </div>
</body>
</html>
