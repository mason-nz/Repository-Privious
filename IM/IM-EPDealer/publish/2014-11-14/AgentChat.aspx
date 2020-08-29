<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentChat.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.AgentChat"
    MasterPageFile="~/Controls/Top.Master" Title="对话管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="css/uploadify.css" rel="stylesheet" />
    <style type="text/css">
        #uploadify
        {
            position: absolute;
            left: 88px;
            top: 334px;
        }
        .Contentbox td
        {
            cursor: pointer;
        }
    </style>
    <script type="text/javascript" language="javascript" src="Scripts/AspNetComet.js"></script>
    <script src="AgentChat.js" type="text/javascript"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Setup/UdCapture.js" charset="utf-8"></script>
    <!--内容开始-->
    <div class="content content_dh">
        <!--对话列表左开始-->
        <div class="left dh_list">
            <!--对话列表-->
            <div class="dh_info">
                <div id="Tab1">
                    <div class="Contentbox Contentbox_dh" id="tabLeft">
                        <!--客户信息-->
                        <div class="clearfix">
                        </div>
                        <!--客户信息-->
                    </div>
                </div>
            </div>
            <!--对话列表-->
        </div>
        <!--对话列表左结束-->
        <!--对话中开始-->
        <div class="left dh_window" id="DialogMain" style="position: relative;">
            <input type="file" id="uploadify" name="uploadify" />
        </div>
        <!--对话中结束-->
        <!--对话右开始-->
        <div class="right dh_yy">
            <!--常用语开始-->
            <div class="yy_info">
                <div id="Tab1">
                    <div class="Menubox">
                        <ul>
                            <li id="one1" onclick="setTab('one',1,2)" class="hover">常用服务语</li>
                            <li id="one2" onclick="setTab('one',2,2)">用户连接信息</li>
                        </ul>
                    </div>
                    <div class="Contentbox">
                        <!--常有服务语-->
                        <div id="con_one_1" class="hover" style="margin-top: 10px;">
                            <ul runat="server" id="ulCM">
                            </ul>
                        </div>
                        <!--常有服务语-->
                        <!--用户连接信息-->
                        <div id="con_one_2" style="display: none;">
                        </div>
                        <!--用户连接信息-->
                    </div>
                </div>
            </div>
            <!--常用语结束-->
        </div>
        <!--对话右开始-->
    </div>
    <!--内容结束-->
    <%--    <button id="btnTest">
        AddNewUser</button>
    <button id="btnAgentLive">
        btnAgentLive</button>
    <input type="text" id="idtest" />
    <button id="ReceiveMsg">
        接收到消息</button>--%>
    <a id="aNewOrder" href="http://www.baidu.com" target="_blank" style="display: none;">
        <span id="spanOrder">estt</span></a>

    <script type="text/javascript">

        function setTab(name, cursel, n) {
            for (var i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
            if (cursel == 2) {
                $('#con_one_2 table').hide();
                $('#DialogR' + ChatStuff.CsId).show();
            }
        };


        $(function () {
            $('.top_open').hover(function () {
                $(this).children("a").addClass('top_on').removeClass('csbg3');
                $(this).children("ul").stop(true, true).show();
            }, function () {
                $(this).children("a").removeClass('top_on').addClass('csbg3');
                $(this).children("ul").hide();
            });
        });


        ChatStuff.AgentId = '<%=this.AgentId %>';
        ChatStuff.ulCM = '<%=this.ulCM.ClientID %>';
        ChatStuff.WorkOrderUrl = '<%=WorkOrderUrl%>';


        ////毕帆表情相关
        var _positionIntervalValue;
        var _togglevalue = true;
        var _isbuttonclick = false;
        function BtnBiaoQing(eve) {
            var $obj = $(".dialogue:visible .fw_bq");
            if (_togglevalue) {
                var $fram = $("#infoIframe")
                $fram.get(0).contentWindow.setEmotionTab('one', 2, 3);
                clearInterval(_positionIntervalValue);
                var pos = findPosition($obj.get(0));
                var yPos = pos.yy - 270;
                var xPos = pos.x - 30 + $obj.width() / 2;
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
            return false;
        };

        function changeImgLayerState() {
            if (_togglevalue) {
                clearInterval(_positionIntervalValue);

                var pos = findPosition($(".dialogue:visible .fw_bq").get(0));
                var yPos = pos.yy - 270;
                var xPos = pos.x - 30 + $(".dialogue:visible .fw_bq").width() / 2;
                $("#infoBox").css({ "left": xPos, "top": yPos }).show("fast");

                _positionIntervalValue = setInterval("reinitIframe()", 800);
                _togglevalue = false;
            }
            else {
                $("#infoBox").hide("fast");
                clearInterval(_positionIntervalValue);
                _togglevalue = true;
            }

        };
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
        };
        function reinitIframe() {
            var pos = findPosition($(".dialogue:visible .fw_bq").get(0));
            var yPos = pos.yy - 270;
            var xPos = pos.x - 30 + $(".dialogue:visible .fw_bq").width() / 2;
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
                } else {

                    _isbuttonclick = false;
                }
            });
        });
    </script>
    <div id="infoBox" style="width: 355px; height: 236px; background-color: #FFFFFF;
        text-align: center; padding: 0px; margin: 0px; position: absolute; display: none;">
        <iframe id="infoIframe" src="EmotionForm.aspx?ReplyBoxID=AgentChat" scrolling="no"
            frameborder="0" width="351"></iframe>
    </div>
</asp:Content>
