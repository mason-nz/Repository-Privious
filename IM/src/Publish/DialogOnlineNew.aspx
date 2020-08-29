<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialogOnlineNew.aspx.cs"
    Inherits="BitAuto.DSC.IM2014.Server.Web.DialogOnlineNew" %>

<%@ Register Src="/Controls/Top.ascx" TagName="TopMaster" TagPrefix="TM" %>
<%@ Register Src="Controls/ControlManage.ascx" TagName="ControlManage" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>在线对话</title>
    <link type="text/css" href="IMCss/css.css" rel="Stylesheet" />
    <link type="text/css" href="IMCss/style.css" rel="Stylesheet" />
    <%--<script src="js/jquery-1.6.4.min.js" language="javascript" type="text/javascript"></script>--%>
    <script src="Scripts/jquery-1.4.1.min.js"
    <script src="js/public.js" language="javascript" type="text/javascript"></script>
    <script src="js/hdm.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        //生成客户信息
        function GenerateCustInfo(custid) {
            $("#con_one_1").empty();
            var html = "";
            html += "<table border='0' cellspacing='0' cellpadding='0' class='cusInfo'>";
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
            html += "<tr>";
            html += "<th>工单记录：</th>";
            html += "<td colspan='3'></td>";
            html += "</tr>";
            html += "</table>";
            html += "<div class='btn' >";
            html += "<div class='right'><input type='button' value='编辑信息'  class='w80 gray'/> <input type='button' value='添加工单'  class='w80 gray'/></div>";
            html += "</div>";
            html += "<div class='clearfix'></div>";

            $("#con_one_1").append(html);

            //缺省显示
            $("#con_one_1").show();
        }

        //生成对话记录
        //参数：会话ID，对话开始时间，对话结束时间
        //type:1坐席结束时间，2网友结束时间
        function GenerateDialog(AllocID, StartTime, EndTime,type) {
            $("#con_one_2").empty();
            var html = "";
            html += "<div class='dialogue'>";            
            html += "<div class='scroll_gd' id='mychat" + AllocID + "'>";
            html += "<p>对话开始于: " + StartTime + "</p>";
            if (EndTime != '' && type == 2) {
                html += "<p>结束:" + EndTime + " </p>";
                html += "<p>访客关闭对话。 </p>";
            }
            else if (EndTime != '' && type == 1) {
                html += "<p>结束:" + EndTime + " </p>";
                html += "<p>坐席关闭对话。 </p>";
            }
            else {
                html += "<p>会话还未结束。 </p>";
            }
            html +=  "</div>";            
            html += "</div>";

            $("#con_one_2").append(html);
            $("#con_one_2").hide();
        }

        function GenerateDialog2(AllocID, StartTime, EndTime) {
            $("#con_one_2").empty();
            var html = "";
            html += "<div class='dialogue'>";
            html += "<p>对话开始于: " + StartTime + "</p>";
            html += "<div class='scroll_gd' id='mychat" + AllocID + "'></div>";
            html += "<p>结束:" + EndTime + " </p>";
            html += "<p>访客关闭对话。 </p>";
            html += "</div>";

            $("#con_one_2").append(html);
            $("#con_one_2").hide();
        }
        //查询聊天记录 生成聊天记录层
        function GenerateChatLayers(AllocID) {
            var parameters = {
                action: 'GetChatMessageLog',
                allocid: AllocID
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    //var r = JSON.parse(msg);
                    var r = $.evalJSON(msg);
                    if (r != null && r.result != '') {
                        //var result = $.evalJSON(r.result);
                        for (var i = 0; i < r.result.length; i++) {
                            var message = r.result[i];
                            CreateMyChat(AllocID, message);
                        }

                        //让滚动条自动向下滚动
                        document.getElementById("mychat" + AllocID).scrollTop = document.getElementById("mychat" + AllocID).scrollHeight;
                    }
                    else {
                        //alert('获取失败');
                        //$.jAlert("没有查询到聊天记录!");
                        //$("#mychat" + AllocID).html("没有查询到聊天记录!");
                        //$("#mychat" + AllocID).css('height','3px');
                        var $find = $("#mychat" + AllocID);
                        var html = "<p>没有查询到聊天记录!</p>";
                        $find.find("p").eq(0).after(html);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("error:" + errorThrown);
                }
            });
        }

        //生成聊天记录html
        function CreateMyChat(AllocID, message) {
            var name = message.name;
            if (message.name.indexOf('-') > 0) {
                name = message.name.split('-')[4];
            }

            //将文本中的url替换成链接
            message.content = replaceRegUrl(message.content);

            var html = "";
            html += "<div class='dh1'>";
            html += "<div class='title'>" + name + " " + message.rectime.split(' ')[1] + "</div>";
            html += "<div class='dhc'>" + message.content + "</div>";
            html += "</div>";

            var $find = $("#mychat" + AllocID);
            if ($find.find(".dh1").length > 0) {
                $(".dh1").last().after(html);
            }
            else {
                $find.find("p").eq(0).after(html);
            }

        }

        function CreateMyChat2(AllocID, message) {
            var name = message.name;
            if (message.name.indexOf('-') > 0) {
                name = message.name.split('-')[4];
            }

            var html = "";
            html += "<div class='dh1'>";
            html += "<div class='title'>" + name + " " + message.rectime.split(' ')[1] + "</div>";
            html += "<div class='dhc'>" + message.content + "</div>";
            html += "</div>";

            $("#mychat" + AllocID).append(html);

        }

        //生成详细信息
        function GenerateDetailInfo(AllocID) {
            var parameters = {
                action: 'GetDetailInfo',
                allocid: AllocID
            };

            $.ajax({
                type: "POST",
                url: "AjaxServers/Handler.ashx",
                data: parameters,
                success: function (msg) {
                    var r = JSON.parse(msg);
                    if (r != null && r.result != '') {
                        var chatinfo = $.evalJSON(r.result);
                        CreateDetailInfo(chatinfo);
                    }
                    else {
                        //alert('没有查询到数据!');
                        $.jAlert("没有查询到数据!");
                    }
                }
            });



        }

        //生成详细信息
        function CreateDetailInfo(chatinfo) {
            $("#con_one_3").empty();
            var html = "";
            html += "<div class='dialogue'>";
            html += "<p>开始时间: " + chatinfo.QueueStartTime + "</p>";
            html += "<p>应答于: " + chatinfo.StartTime + "</p>";
            html += "<p>结束时间: " + chatinfo.EndTime + "</p>";
            html += "<p>对话时长: " + chatinfo.TalkTime + "</p>";
            html += "<p>客服: " + chatinfo.AgentID + "</p>";
            html += "<p>服务评价:</p>";
            html += "<p>访客姓名: </p>";
            html += "<p>访客地区: " + chatinfo.Location + " </p>";
            html += "<p>访客IP: " + chatinfo.LocalIP + " </p>";
            html += "<p>对话类型: 点击咨询图标</p>";
            html += "<p>访客来源:<a href='#'>" + chatinfo.UserReferURL + "</a></p>";
            html += "<p>发起页面:<a href='#'>" + chatinfo.UserReferURL + "</a></p>";
            //样试左右对齐，加的空p
            html += "<p style='height:9px;'></p>";

            $("#con_one_3").append(html);
            $("#con_one_3").hide();

        }

        function OperateDialogLayer(AllocID, starttime, endtime,type) {
            GenerateCustInfo("");
            var allocid = "696";
            GenerateDialog(AllocID, starttime, endtime,type);
            GenerateChatLayers(AllocID);
            GenerateDetailInfo(AllocID);
            setTab('one', 2, 3);
        }

        $(document).ready(function () {
            //OperateDialogLayer();
            LoadingAnimation('mycontent');
            var url = 'AjaxServers/OnlineDialogList.aspx?r=' + Math.random();
            $("#mycontent").load(url, null, function () {
                $($("#DialogListTable tr:eq(1)")).click();
            });

        });
        function ShowDataByPost1(pody) {
            LoadingAnimation('mycontent');
            var url = 'AjaxServers/OnlineDialogList.aspx?r=' + Math.random();
            $("#mycontent").load(url, pody);
        }
        function SelectAlloc(AllocID, starttime, agentendtime, userendtime) {
            var type = 0;
            var endtime = "";
            if (userendtime != "" && userendtime != "9999-12-31 0:00:00" && agentendtime != "" && agentendtime != "9999-12-31 0:00:00") {
                var millsecs = CompareDateTime(agentendtime, userendtime);
                if (millsecs > 0) {
                    type = 1;
                    endtime = agentendtime.split(' ')[1];
                }
                else {
                    endtime = userendtime.split(' ')[1];
                    type = 2;
                }
            }
            else {
                if (userendtime != "" && userendtime != "9999-12-31 0:00:00") {
                    endtime = userendtime.split(' ')[1];
                    type = 2;
                }
                else if (agentendtime != "" && agentendtime != "9999-12-31 0:00:00") {
                    endtime = agentendtime.split(' ')[1];
                    type = 1;
                }
            }
            OperateDialogLayer(AllocID, starttime, endtime,type);
        }

    </script>
</head>
<body>
    <div class="crm">
        <div class="head">
            <TM:TopMaster ID="TopMaster" runat="server" />
            <div class="mainmenu_bottom">
                <ul class="" id="mb1">
                    <uc3:ControlManage ID="ControlManageSet1" runat="server" />
                </ul>
                <div class="clearfix">
                </div>
            </div>
        </div>
        <!--头部结束-->
        <!--内容开始-->
        <div class="content content2">
            <div class="cxList online_dh" id="mycontent" style="margin-top: 8px;">
            </div>
            <div class="kh_info">
                <div id="Tab1">
                    <div class="Menubox">
                        <ul>
                            <li id="one1" onclick="setTab('one',1,3)" class="hover">客户信息</li>
                            <li id="one2" onclick="setTab('one',2,3)">对话记录</li>
                            <li id="one3" onclick="setTab('one',3,3)">详细信息</li>
                        </ul>
                    </div>
                    <div class="Contentbox">
                        <!--客户信息-->
                        <div id="con_one_1" class="hover">
                        </div>
                        <!--客户信息-->
                        <!--对话记录-->
                        <div id="con_one_2" style="display: none">
                        </div>
                        <!--对话记录-->
                        <!--详细信息-->
                        <div id="con_one_3" style="display: none">
                        </div>
                        <!--详细信息-->
                    </div>
                </div>
            </div>
            <!--信息展示-->
            <div class="clearfix">
            </div>
        </div>
        <!--列表结束-->
        <!--信息展示-->
        <div class="clearfix">
        </div>
        <!--内容结束-->
        <div class="footer mt16">
            信息系统研发中心 任何建议和意见，请发邮件至：<a href="#">ISDC@bitauto.com</a><br />
            CopyRight © 2000-2013 Bitauto,All Rights Reserved 版权所有 北京易车互联信息技术有限公司
        </div>
    </div>
</body>
</html>
