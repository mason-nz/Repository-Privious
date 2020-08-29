<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleInfoList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.RoleInfoList" %>

<script type="text/javascript">
    function RefreshRoleInfo()
    {  
     <%=ajaxPager.RefreshFunctionName %>();
    }
    //设置权限
    function SetRole(SysID,RoleID)
    {
        $.openPopupLayer({
                name:"RoleModuleListNew",
                url: "/AjaxServers/SysManager/RoleModuleList.aspx?SysID="+SysID+"&RoleID="+RoleID,
                error: function(dd) {alert(dd.status);} ,
                afterClose: RefreshRoleInfo
            });
    }
    //编辑或添加角色信息
    function EditRoleInfo(RoleID,type)
    {   
		   $.openPopupLayer({
		   	            name: "EditRoleInfo",  
						url: "/AjaxServers/SysManager/EditRoleInfo.aspx?ShowType="+type+"&roleID="+RoleID+"&sysID=<%=SysID %>&page=1",
						error: function(dd) {alert(dd.status);} ,
						afterClose: RefreshRoleInfo
					});
	}
	//删除角色信息
	function DeleRoleInfo()
	{
	     if(validateDel('checkBoxDelAll'))
            {
                var ID = $(':checkbox[name="checkBoxDelAll"]:checked').map(function(){  return $(this).val();}).get().join(","); 
                var url = "/AjaxServers/SysManager/RoleInfoManager.ashx";
                var postBody="dele=yes&ID="+ID;
                AjaxPost(url,postBody,null,function(data){
                    var s = $.evalJSON(data);
                    {
                        if(s.dele == 'yes')
                        {
                            RefreshRoleInfo();
                            alert('删除成功！');
                            return;
                        }
                        else if(s.dele == 'exist')
                        {
                            alert('选择的角色中有已经被用户使用的!')
                        }
                        else
                        {
                            alert('删除失败！');
                            return;
                        }
                    }
                });
            }
	}
    
</script>
<form id="form1" runat="server">
<div class="allcont" style="width: 700px">
    <!--主体内容部分star-->
    <div class="cont">
        <div class="cont_cx">
            <div class="allxxsc">
                <div class="sc">
                    <li id="tabadmenu_11">
                        <input type="button" class="button" value="新增" onclick="EditRoleInfo('','add');" />
                        
                    </li>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="cont_cxjg">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
                <tr class="tr_title">
                    <th align="center" width="10%">
                        选 择
                    </th>
                    <th align="center" width="25%">
                        系统名称
                    </th>
                    <th align="center" width="20%">
                        角色编号
                    </th>
                    <th align="center" width="20%">
                        角色名称
                    </th>
                    <th align="center" width="25%">
                        操 作
                    </th>
                </tr>
                <asp:repeater id="repeater" runat="server">
                            <ItemTemplate>
                                <tr id="tr<%#Eval("RoleID") %>">
                                    <th align="center">
                                        <input id="checkBoxDelAll" name="checkBoxDelAll" type="checkbox" value="<%#Eval("RoleID")%>"/>
                                    </th>
                                    <td class="managment">
                                    <%#Eval("SysName")%>
                                </td>
                                <td class="managment">
                                    <%#Eval("RoleID")%>
                                </td>
                                <td>
                                    <%#Eval("RoleName")%>
                                </td>
                               <td align="center"  >
                                    <a href="javascript:SetRole('<%#Eval("SysID") %>','<%#Eval("RoleID") %>');">设置权限</a>
                                    <%-- <a href="RoleModuleList.aspx?SysID=<%#Eval("SysID")%>&RoleID=<%#Eval("RoleID")%>">设置权限</a>--%>
                                    <a href="javascript:EditRoleInfo('<%#Eval("RoleID")%>','updata');">编辑</a>
                                 
                                   <%-- <a href="">查看角色拥有者</a>--%>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:repeater>
            </table>
            <div class="clear">
            </div>
            <div class="quanxuan">
                <ul>
                    <li><a href="javascript:selectCheckBoxDelAll('checkBoxDelAll',1);">全选</a></li>
                    <li><a href="javascript:selectCheckBoxDelAll('checkBoxDelAll',2);">反选</a></li>
                    <li><a href="javascript:selectCheckBoxDelAll('checkBoxDelAll',3);">取消选择</a></li>
                    <li></li>
                </ul>
            </div>
            <div class="pages">
                <input type="hidden" runat="server" id="hidDelAll" name="hidDelAll" />
                <uc:AjaxPager ID="ajaxPager" runat="server" />
            </div>
            <div class="quanxuan allxxsc">
                <ul>
                    <li>
                        <input type="button" class="button" value="删除" onclick="javascript:DeleRoleInfo();" />
                    </li>
                </ul>
            </div>
        </div>
        <!--底部部分star-->
    </div>
</div>
</form>
