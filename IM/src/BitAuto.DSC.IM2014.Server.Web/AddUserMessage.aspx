<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUserMessage.aspx.cs"
    Inherits="BitAuto.DSC.IM2014.Server.Web.AddUserMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>在线留言</title>
    <link type="text/css" href="IMCss/css.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="Scripts/common.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        function setbigsmall() {
            //指定全屏高度
            $('#bodyDIV').css('height', (($(window).height() - 20) + 'px'))
            //指定左边高度
            //$('#divleft').css('height', (($(window).height() - 200) + 'px'));
            //指定右面高度
            //$('#divright').css('height', (($(window).height() - 200) + 'px'));
            $('#divcontent').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
            //            //指定左边高度
            $('#divleft').css('height', "101.4%");
            //            //指定右面高度
            $('#divright').css('height', "103.2%");
        }
        function ChangeBigSmall() {
            setbigsmall();
        }

        $(document).ready(function () {
            setbigsmall();
        });


        //处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外  
        function banBackSpace(e) {
            var ev = e || window.event; //获取event对象     
            var obj = ev.target || ev.srcElement; //获取事件源     

            var t = obj.type || obj.getAttribute('type'); //获取事件源类型    

            //获取作为判断条件的事件类型  
            var vReadOnly = obj.getAttribute('readonly');
            var vEnabled = obj.getAttribute('enabled');
            //处理null值情况  
            vReadOnly = (vReadOnly == null) ? false : vReadOnly;
            vEnabled = (vEnabled == null) ? true : vEnabled;

            //当敲Backspace键时，事件源类型为密码或单行、多行文本的，  
            //并且readonly属性为true或enabled属性为false的，则退格键失效  
            var flag1 = (ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea")
                    && (vReadOnly == true || vEnabled != true)) ? true : false;

            //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效  
            var flag2 = (ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea")
                    ? true : false;

            //判断  
            if (flag2) {
                return false;
            }
            if (flag1) {
                return false;
            }
        }

        //禁止后退键 作用于Firefox、Opera  
        document.onkeypress = banBackSpace;
        //禁止后退键  作用于IE、Chrome  
        document.onkeydown = banBackSpace;
        //window.history.forward(1);
        //捕获窗口关闭
        //        window.onbeforeunload = onbeforeunload_handler;
        //        function onbeforeunload_handler() {


        //            var sendto = '<%=this.Request["SendTo"] %>';
        //            var pody = { action: 'userclosechat', username: escape('<%=UserMessageIMID %>'), SendToPublicToken: escape(sendto) };
        //            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
        //             function (msg) {
        //                 var r = JSON.parse(msg);
        //                 if (r != null && r.result == 'sendok') {//登录成功之后
        //                 }
        //             });
        //            window.opener = null; window.open('', '_self'); window.close();



        //            //            var pody = { action: 'closechat', username: escape('<%=UserMessageIMID%>') };
        //            //            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
        //            //             function (msg) {
        //            //                 var r = JSON.parse(msg);
        //            //                 if (r != null && r.result == 'sendok') {//登录成功之后
        //            //                 }
        //            //             });
        //            //            window.opener = null; window.open('', '_self'); window.close();
        //        }
        function AddMessage() {
            var Message = $.trim($("#txtMessage").val());
            var LeaveMessageType = "";
            var phone = $.trim($("#txtPhone").val());
            var emil = $.trim($("#txtemail").val());
            var username = $.trim($("#txtUserName").val());
            $("#emErroremail").css("display", "none");
            $("#emErrorType").css("display", "none");
            $("#emErrorContent").css("display", "none");
            $("#emErrorUserName").css("display", "none");
            $("#emErrorPhone").css("display", "none");

            $("input[name='LeaveMessageType']").each(function () {
                if ($(this).attr("checked")) {
                    LeaveMessageType = $(this).val();
                }
            });
            if (LeaveMessageType == "") {
                $("#emErrorType").css("display", "");
                $("#ErrorType").html("请选择类型！");
            }
            else if (Message == "") {
                $("#emErrorContent").css("display", "");
                $("#ErrorContent").html("请填写内容！");
            }
            else if (GetStringRealLength(Message) > 2000) {
                $("#emErrorContent").css("display", "");
                $("#ErrorContent").html("内容超长！");
            }
            else if (emil == "") {
                $("#emErroremail").css("display", "");
                $("#Erroremail").html("请输入邮箱地址！");
            }
            else if (GetStringRealLength(emil) > 200) {
                $("#emErroremail").css("display", "");
                $("#Erroremail").html("邮箱地址超长！");
            }
            else if (!isEmail(emil)) {
                $("#emErroremail").css("display", "");
                $("#Erroremail").html("邮箱地址格式不正确！");
            }
            else if (GetStringRealLength(phone) > 100) {
                $("#emErrorPhone").css("display", "");
                $("#ErrorPhone").html("手机超长！");
            }
            else if (GetStringRealLength(phone) > 0 && (!isTelOrMobile(phone))) {
                $("#emErrorPhone").css("display", "");
                $("#ErrorPhone").html("手机格式不正确！");
            }
            else if (Len(username) > 200) {
                $("#emErrorUserName").css("display", "");
                $("#ErrorUserName").html("姓名超长！");
            }
            else {
                var UserMessageIMID = '<%=UserMessageIMID %>';
                if (UserMessageIMID == "") {
                    $.jAlert("网友标识不能为空！");
                }
                else {
                    var json = { Action: 'addusermessage',
                        UserMessage: escape(Message),
                        UserMessageIMID: escape(UserMessageIMID),
                        LeaveMessageType: escape(LeaveMessageType),
                        Email: escape(emil),
                        LeavePhone: escape(phone),
                        LeaveUserName: escape(username)
                    };

                    AjaxPost("AjaxServers/UserMessageHandler.ashx", json,
                       null,
                         function (data) {
                             var jsonData = $.evalJSON(data);
                             if (jsonData.success = "yes") {
                                 $.jAlert(jsonData.result, function () {
                                     window.opener = null; window.open('', '_self'); window.close();
                                 });
                             }
                             else {
                                 $.jAlert(jsonData.result);
                             }
                         });
                }
            }
        }
        function CloseWindow() {
            $.jConfirm("您确定关闭页面吗？", function (r) {
                if (r) {
                    //onbeforeunload_handler();
                    window.opener = null; window.open('', '_self'); window.close();
                }

            });
        }
    </script>
</head>
<body onresize="ChangeBigSmall()">
    <form id="form1" runat="server">
    <div class="online_kf" id="bodyDIV">
        <div class="title_kf">
            在线留言<span><a href="#"><img src="images/c_btn.png" border="0" alt="关闭" onclick="CloseWindow()" /></a></span></div>
        <div class="content_kf" id="divcontent">
            <!--左开始-->
            <div class="left_c" id="divleft">
                <div class="answer" style="height: 96%">
                    <p class="hs">
                        请填写您的信息及咨询问题，我们会尽快给您回复</p>
                    <table border="0" cellspacing="0" cellpadding="0" class="msg_lv" width="100%">
                        <tr>
                            <th width="10%">
                                <span class="red">*</span>类型：
                            </th>
                            <td width="80%">
                                <label>
                                    <input name="LeaveMessageType" type="radio" value="1" /><span> 购车咨询</span></label><label><input
                                        name="LeaveMessageType" type="radio" value="2" /><span> 活动咨询</span></label><label><input
                                            name="LeaveMessageType" type="radio" value="3" /><span> 网站建议</span></label><label><input
                                                name="LeaveMessageType" type="radio" value="4" /><span> 其它</span></label><em id="emErrorType"
                                                    style="display: none"><img src="images/tip.png"><span id="ErrorType" class="tsy"></span></em>
                            </td>
                        </tr>
                        <tr>
                            <th style="vertical-align: top;">
                                <span class="red">*</span>内容：
                            </th>
                            <td>
                                <textarea id="txtMessage" cols="" rows="10"></textarea><em id="emErrorContent" style="display: none"><img
                                    src="images/tip.png"><span id="ErrorContent" class="tsy"></span></em>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <span class="red">*</span>邮箱：
                            </th>
                            <td>
                                <input type="text" value="" id="txtemail" class="w180" /><em id="emErroremail" style="display: none"><img
                                    src="images/tip.png"><span id="Erroremail" class="tsy"></span></em>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                姓名：
                            </th>
                            <td>
                                <input type="text" value="" id="txtUserName" class="w180" /><em id="emErrorUserName"
                                    style="display: none"><img src="images/tip.png"><span id="ErrorUserName" class="tsy"></span></em>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                手机：
                            </th>
                            <td>
                                <input type="text" value="" id="txtPhone" class="w180" /><em id="emErrorPhone" style="display: none"><img
                                    src="images/tip.png"><span id="ErrorPhone" class="tsy"></span></em>
                            </td>
                        </tr>
                    </table>
                    <div class="btn submit">
                        <input type="button" value="提交" onclick="AddMessage()" class="w80" />
                    </div>
                </div>
            </div>
            <!--右开始-->
            <div class="right_c" id="divright">
                <div class="person">
                    <div class="pic_t">
                    </div>
                    <div class="title">
                        易车网客服</div>
                </div>
                <div class="question">
                    <div class="title">
                        常见问题</div>
                    <ul>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=1" target="_blank">什么是汽车通？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=2" target="_blank">为什么要激活邮箱？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=3" target="_blank">如何激活邮箱？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=4" target="_blank">如何找回个人密码？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=5" target="_blank">如何完善个人信息？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=6" target="_blank">如何进入会长管理后台？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=7" target="_blank">完善个人信息的重要性</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=8" target="_blank">如何获得易车网车标？</a></li>
                        <li><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=&tab=9" target="_blank">如何联系我们？</a></li>
                        <li class="more"><a href="http://www.bitauto.com/feedback/FAQ.aspx?col=2
" target="_blank">更多&gt;&gt;</a></li>
                    </ul>
                </div>
                <div>
                </div>
            </div>
        </div>
        <div class="clearfix">
        </div>
    </div>
    </form>
</body>
</html>
