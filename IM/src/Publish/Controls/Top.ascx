<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top.ascx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.Controls.Top" %>
<link type="text/css" href="/IMCss/css.css" rel="stylesheet" />
<link type="text/css" href="/IMCss/style.css" rel="stylesheet" />
<script src="/js/public.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var AgentUserName = '<%=AgentUserName %>';
        $("#myagentid").text(AgentUserName);
    });
    //只能打开一个对话管理页面逻辑
    function openwindow(url, isTalk) {
        if (isTalk == "SYS032MOD0001") {
            var pody = { action: 'IsExists', username: escape('<%=AgentIMID %>') };
            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r = JSON.parse(msg);
                 if (r != null && r.result == 'NoExists') {//登录成功之后
                     window.open(url);
                 }
                 else {
                     $.jAlert("对话管理已存在！");
                 }
             });
        }
        else {
            window.open(url);
        }
    }
    function QuiteWindow() {
        window.location.href = "login.aspx";
    }
</script>
<!--头部开始-->
<!--header开始-->
<div class="header">
    <div class="logo left">
        <img src="images/logo.png" alt="CRM系统logo" /></div>
    <!--菜单开始-->
    <div class="menu mt16 ft14">
        <ul>
            <asp:Literal ID="Lit_menus" runat="server"></asp:Literal>
        </ul>
    </div>
    <!--菜单结束-->
    <div class="function right">
        <ul>
            <li><a href="#" class="write">帮助</a></li>
            <li><a href="#" class="write">关于</a></li>
            <li><a class="write" id="myexit" onclick="QuiteWindow()">退出</a></li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="user_zt right">
            <div class="user left">
                <img src="images/user.png" width="12" height="14" />
                <span id="myagentid"></span>&nbsp;&nbsp;
            </div>
            <div class="status right" style="display: none">
                <ul>
                    <li>
                        <ul class="menuUl">
                            <li class="position"><span class="top_open nav_class" onmouseover="this.className='nav_class_hover'"
                                onmouseout="this.className='nav_class'"><a id="curAgentState" class="csbg" title="在线">
                                    在线</a>
                                <ul class="top_cont nextT" id="myulstate">
                                </ul>
                            </span></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>
<div style="clear: both;">
</div>
<!--header结束-->
<div class="clearfix">
</div>
<!--子菜单开始-->
<!--子菜单结束-->
<!--头部结束-->
