<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleInfoList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.SysManager.RoleInfoList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title>赤兔联盟平台</title>
    <!--#include file="/base/js.html" -->
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="/js/SysRightCommon.js"></script>
    <script type="text/javascript">
        function openAjaxPopupCommen(SysID) {
            var url = '/AjaxServers/SysManager/RoleInfoList.aspx?page=1&sysID=' + SysID;
            var data = {};
            $('#divContent').load(url + "&" + Math.random());
        }
        //查看角色拥有者
        function openAjaxViewRoleByUser(RoleID) {
            var url = '/AjaxServers/SysManager/ViewRoleByUser.aspx?page=1&roleID=' + RoleID;
            var data = {};
            $('#divContent').load(url + "&" + Math.random());
        }
    </script>
</head>
<body>
    <!--#include file="/base/header.html" -->
    <form id="form1" runat="server">
    <!--中间内容-->
    <div class="order">
        <!--左侧菜单-->
        <!--#include file="/base/Menu.html" -->
        <div class="order_r">
            <div class="cont">
                <div class="cont_left">
                    <asp:TreeView ID="tv" ShowLines="true" runat="server">
                    </asp:TreeView>
                    <input type="hidden" id="hidSysID" runat="server" />
                </div>
                <div class="cont_right">
                    <div id="divContent">
                    </div>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    </form>
    <!--#include file="/base/footer.html" -->
</body>
</html>
