<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentChat.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AgentChat"
    MasterPageFile="~/Controls/Top.Master" Title="对话管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="css/uploadify.css" rel="stylesheet" />
    <link href="css/emotionstyle.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript">
        loadJS("AspNetComet");
        loadJS("AgentChat");

    </script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Setup/UdCapture.js" charset="utf-8"></script>
    <!--内容开始-->
    <div class="content content_dh" id="MainContentDiv">
        <!--对话列表左开始-->
        <div class="left dh_list">
            <!--对话列表-->
            <div class="dh_info">
                <div id="Tab1">
                    <div class="Contentbox Contentbox_dh" id="tabLeft">
                        <div class="dh_tab" id="tabLeftTotalInfo">
                            <span>全部对话【<em class="bule" id="totalConvsNum">0</em>】</span><span>未读对话【<em class="red"
                                id="totalUnReadConvsNum">0</em>】</span></div>
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
        <div class="right dh_yy" id="rightTabDiv">
            <!--常用语开始-->
            <div class="yy_info">
                <div id="tabRight">
                    <%--<div class="Menubox">
                        <ul>
                            <li id="one1" onclick="setTab('one',1,2)" class="hover">常用服务语</li>
                            <li id="one2" onclick="setTab('one',2,2)">用户连接信息</li>
                        </ul>
                    </div>--%>
                    <div class="Menubox Menubox2">
                        <ul>
                            <li id="one1" onclick="setTab('one',1,4)" class="hover">
                                <img src="images/img1.png" width="24" height="23" style="vertical-align: baseline;
                                    position: relative; top: 8px;" /></li>
                            <li id="one2" onclick="setTab('one',2,4)">
                                <img src="images/img2.png" width="24" height="23" style="vertical-align: baseline;
                                    position: relative; top: 8px;" /></li>
                            <li id="one3" onclick="setTab('one',3,4)">
                                <img src="images/img3.png" width="24" height="23" style="vertical-align: baseline;
                                    position: relative; top: 8px;" /></li>
                            <li id="one4" onclick="setTab('one',4,4)">
                                <img src="images/img4.png" width="24" height="23" style="vertical-align: baseline;
                                    position: relative; top: 8px;" /></li>
                        </ul>
                    </div>
                    <div class="Contentbox">
                        <!--常有服务语-->
                        <div id="con_one_1" class="hover" style="margin-top: 10px;">
                            <ul runat="server" id="ulCM">
                            </ul>
                        </div>
                        <!--客户-->
                        <div id="con_one_2" style="display: none;">
                        </div>
                        <!--客户-->
                        <!--订单-->
                        <div id="con_one_3" style="display: none;">
                            <div class="search_dd">
                                <div class="left coupon-box02">
                                    <input type="text" value="" id="txt_Telephone" class="text02">
                                    <b>
                                        <img src="images/search_dd.png" onclick="searchOrderInfo()" width="17" height="15"
                                            style="position: relative; top: 5px;" /></b></div>
                                <div class="dd_list" id="ajaxOrderInfoList" style=" text-align:center;">
                                </div>
                                <input type="hidden" id="hidkeyid" value="112233" />
                                <script type="text/javascript">
                                    //查询对话记录
                                    function searchOrderInfo() {
                                        var txtTel = $.trim($('#txt_Telephone').val());
                                        var hidkeyid = $.trim($('#hidkeyid').val());
                                        //LoadingAnimation("ajaxOrderInfoList");
                                        $('#rightTabDiv').Mask();
                                        $(".blue-loading").css("width","55%");
                                        $('#ajaxOrderInfoList').html("");
                                        $('#ajaxOrderInfoList').load("/AjaxServers/OrderInfo.aspx?tel=" + txtTel + "&keyid=" + hidkeyid + "&r=" + Math.random(), null, function () {
                                           $('#rightTabDiv').UnMask();
                                        });
                                    }
                                    //分页操作 
                                    function ShowDataByPost5(pody) {
                                        //                                        LoadingAnimation("ajaxOrderInfoList");
                                        $('#rightTabDiv').Mask();
                                        $(".blue-loading").css("width", "55%");
                                        $('#ajaxOrderInfoList').load("/AjaxServers/OrderInfo.aspx?r=" + Math.random(), pody, function () {
                                            $('#rightTabDiv').UnMask();
                                        });
                                    }
                                </script>
                            </div>
                        </div>
                        <!--订单-->
                        <!--知识库-->
                        <div id="con_one_4" style="display: none;">
                            <h4 style="margin: 20px; text-align: center; vert-align: middle;">
                                暂未开放</h4>
                        </div>
                    </div>
                </div>
            </div>
            <!--常用语结束-->
        </div>
        <!--对话右开始-->
    </div>
    <!--内容结束-->
   <%-- <button id="btnTest" onclick="return ttt();">
        AddNewUser</button>
    <button id="btnAgentLive">
        btnAgentLive</button>
    <input type="text" id="idtest" />
    <button id="ReceiveMsg">
        接收到消息</button>
    <a id="aNewOrder" href="http://www.baidu.com" target="_blank" style="display: none;">
        <span id="spanOrder">estt</span></a>--%>
    <script type="text/javascript">

      

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
        ChatStuff.AgentToken ='<%=this.AgentToken%>';
        ChatStuff.ulCM = '<%=this.ulCM.ClientID %>';
        ChatStuff.WorkOrderUrl = '<%=WorkOrderUrl%>';
        ChatStuff.EditCustBaseInfoUrl = '<%=EditCustBaseInfoUrl%>';
        ChatStuff.TimeidleAgent = <%=TimeidleAgent %>+9000;  //坐席断网多久提示离线+jquery ajax默认超时时间
        ChatStuff.IsDuplicateLogin = '<%=IsDuplicateLogin %>';
       
    </script>
</asp:Content>
