<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleModuleList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.RoleModuleList" %>


<script type="text/javascript">
    function clickE(SysID, RoleID) {
        var url = "/AjaxServers/SysManager/RoleModuleList.aspx?SysID=" + SysID + "&RoleID=" + RoleID;
        var data = {};
        $('#treeviewRole').load(url + "&" + Math.random());
    }
    //保存设置的角色
    function SaveRole(IsClose) {
        if (IsClose) {
            var idArray = new Array();
            jQuery('#tvModule :checkbox:checked').each(function (i) {
                var str = jQuery(this).find('+ a').attr('href');
                //只设置1、2、3级菜单
                if (str.indexOf("#1") != -1 | str.indexOf("#2") != -1 | str.indexOf("#3") != -1 | str.indexOf("#4") != -1) {
                    idArray.push(str.substring(str.indexOf("#") + 2));
                }
            });
            var ids = idArray.join(':');
            //console.log(ids);
            //alert(ids);
            var url = "/AjaxServers/SysManager/RoleInfoManager.ashx";
            var postBody = "SetRole=yes&sysID=<%=SysID %>&roleID=<%=RoleId %>&moduleID=" + ids;
            AjaxPost(url, postBody, null, function (data) {
                var s = $.evalJSON(data);
                if (s.setRole == 'yes') {
                    //设置成功，关闭。
                    Close('RoleModuleListNew');
                    alert('设置角色权限成功！');
                    return;
                }
                else {
                    alert('设置角色权限失败,请联系管理员！');
                    return;
                }
            });
        }
    }
    $(document).ready(function () {
        //获取click事件
        $(".aa :checkbox").click(function () {
            var check = $(this).parents("table").next("div").find("input");
            //console.log($(this)) ;
            var checkedStatus = $(this).is(":checked");
            check.each(function () {
                $(this).attr('checked', checkedStatus);
            });
            var str = $(this).next("a").attr("href");
            //只有是3级菜单的时候才进行上级checkbox选中
            if (checkedStatus == true && str.indexOf("#3") != -1) {
                var check = $(this).closest("div").prev("table").find("input");
                check.each(function () {
                    $(this).attr('checked', true);
                });
            }
        });
    });
</script>

<div id="treeviewRole">
    <form id="Form1" runat="server">
        <div class="openwindow">
            <!--主体内容部分star class="cont"-->
            <div>
                <div class="close">
                    <a href="javascript:Close('RoleModuleListNew');">关闭</a></div>
                <div class="cont_cxjg">
                    <div class="roler_set">
                        <ul>
                            <asp:repeater runat="server" id="repeaterRole" onitemdatabound="repeaterRole_ItemDataBound">
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litRole"></asp:Literal></ItemTemplate>
                        </asp:repeater>
                        </ul>
                        <ul class="aa">
                            <asp:treeview id="tvModule" runat="server" expanddepth="1" showcheckboxes="All">
                        </asp:treeview>
                        </ul>
                        <%--<br style="clear: left" />--%>
                    </div>
                </div>
                <!--底部部分star-->
                <%--<asp:button runat="server" cssclass="in_td in_menu" id="btnSaveRoleModule" text="Save"
                    onclick="btnSaveRoleModule_ServerClick" />--%>
                <div style="text-align: center; height: 30px" class="submits">
                    <input id="btnSave" type="button" onclick="javascript:SaveRole(true);" class="button"
                        value="保存" />
                    <input type="button" onclick="Close('RoleModuleListNew');" value="退出" class="button" />
                </div>
            </div>
        </div>
    </form>
</div>
