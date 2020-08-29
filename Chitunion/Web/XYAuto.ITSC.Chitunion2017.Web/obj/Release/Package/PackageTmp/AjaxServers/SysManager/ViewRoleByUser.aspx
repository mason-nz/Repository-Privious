<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewRoleByUser.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.ViewRoleByUser" %>

<script>
 function RefreshUserInfo()
{  <%=ajaxPager.RefreshFunctionName %>();
}
function Delete(roleid,userid,recid)
 {
        var url = "/AjaxServers/SysManager/UserInfoList.aspx";
        var postBody="delUserRole=yes&recid="+recid+"&userid="+userid+"&roleid="+roleid;
        AjaxPost(url,postBody,null,function(data){
        var jsonData = $.evalJSON(data);
        if(jsonData.delUserRole=='yes')
        {
            alert('删除成功');
            RefreshUserInfo();
        }
        });
   
}
</script>

<form id="form1" runat="server">
<div class="allcont" style="width: 700px">
    <!--主体内容部分star-->
    <div class="cont">
        <div class="cont_cx">
            <div class="allxxsc">
                <div class="sc">
                    <li id="tabadmenu_11"></li>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="cont_cxjg">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
                <tr class="tr_title">
                    <td width="15%">
                        角色编号
                    </td>
                    <td width="15%">
                        角色名称
                    </td>
                    <td width="10%">
                        角色拥有者
                    </td>
                    <td width="10%">
                        状态
                    </td>
                    <td width="25%">
                        创建时间
                    </td>
                    <%--<td>
                        删除
                    </td>--%>
                </tr>
                <asp:repeater id="repeater" runat="server">
                            <ItemTemplate>
                                <tr id="tr<%#Eval("RoleID") %>">                          
                                <td class="managment">
                                    <%#Eval("RoleID")%>
                                </td>
                                <td>
                                    <%#Eval("RoleName")%>
                                </td>
                                <td>
                                    <%#Eval("truename") %>
                                </td> <td>
                                    <%#GetStatus(Eval("userstatus"))%>
                                </td>
                                <td>
                                     <%#Eval("CreateTime") %>
                                </td>
                                <%--<td class="managment">
                                    <a href="javascript:Delete('<%#Eval("RoleID") %>',<%#Eval("userid") %>,<%#Eval("recid") %>)">删除</a>
                                </td>--%></tr>
                            </ItemTemplate>
                        </asp:repeater>
            </table>
            <div class="clear">
            </div>
            <div class="pages">
                <uc:AjaxPager ID="ajaxPager" runat="server" />
            </div>
            <div class="quanxuan allxxsc">
                <ul>
                    <li></li>
                </ul>
            </div>
        </div>
        <!--底部部分star-->
    </div>
</div>
</form>
