<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleInfoList.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.ModuleInfoList" %>
<script>
 function RefreshModule()
{  //alert(<%=ajaxPager.RefreshFunctionName %>);
<%=ajaxPager.RefreshFunctionName %>();
}
 function RefreshModuleSearch()
{ 
var txtSeachModuleName = escape( $('#txtSeachModuleName').val());
 
 <%=ajaxPager.RefreshFunctionName %>("&ModuleName="+txtSeachModuleName);
}
function openAjaxPopupModule(ModuleID,type,pid)
{   
	   $.openPopupLayer({
	   	            name: "ModuleAjaxPopup",  
					url: "../AjaxServers/SysManager/EditModuleInfo.aspx?ShowType="+type+"&SysID=<%=RequestSysID %>&ModuleID="+ModuleID+"&Pid="+pid+"&page=1",
					error: function(dd) {alert(dd.status);} ,
					afterClose: RefreshModule 
				});
}
 
function Delete()
 {
    if(validateEdit('checkBoxDelAll'))
    {
        var ID = $(':checkbox[name="checkBoxDelAll"]:checked').map(function(){  return $(this).val();}).get().join(","); 
        var url = "../AjaxServers/SysManager/ModuleInfoManager.ashx";
        var postBody="dele=yes&ID="+ID;
        AjaxPost(url,postBody,null,SuccessPostDel);
    }
}

function SuccessPostDel(data)
{
    var jsonData=$.evalJSON(data);
    var str=$.trim(jsonData.del);
    if(str=='yes')
    { 
        alert('操作成功!');
        RefreshModule();
    } 
    else if(str=='exist')
    {
        alert('选择的模块，有用户在使用，不能删除。');
    } 
    else if(str=='no')
    {
        alert('操作失败!');
    }
}

 
</script>
<form id="form1" runat="server">
    <div class="allcont" style="width: 700px">
        <!--主体内容部分star-->
        <div class="cont">
            <div class="cont_cx">
            <div class="allxxsc">
                    <div class="xx" style="width:300px">
                        <label >
                           
                        </label>
                        <span> 模块名称：</span><input name="txtSeachModuleName" id="txtSeachModuleName" type="text" runat="server" />
                    </div>
                    <div class="sc">
                        <li>
                            <input type="button" value="查询" class="button" onclick="javascript:RefreshModuleSearch();" />
                        </li>
                    </div>
                </div>
                <div class="allxxsc">
                    <div class="xx">
                    </div>
                    <div class="sc">
                        <li id="tabadmenu_11">
                            <input type="button" class="button" value="新增" onclick="openAjaxPopupModule('','add','<%=RequestModuleID %>');" /></li>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="cont_cxjg">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
                    <tr>
                        <th align="center" width="10%">
                            选择</th>
                        <th align="center" width="20%">
                            模块名称</th>
                        <th align="center" width="20%">
                            模块编号</th>
                        <th align="center" width="20%">
                            模块描述</th>
                        <th align="center" width="20%">
                            创建时间</th>
                        <th align="center" width="10%">
                            操作</th>
                    </tr>
                    <asp:repeater id="repeater" runat="server">
                            <ItemTemplate>
                                <tr id="tr<%#Eval("ModuleID") %>">
                                    <th align="center">
                                        <input id="checkBoxDelAll" name="checkBoxDelAll" type="checkbox" value="<%#Eval("ModuleID")%>"
                                             />
                                    </th>
                                    <td class="managment">
                                    <%#Eval("moduleName")%>
                                </td>
                                <td>
                                    <%#Eval("moduleid")%>
                                </td>
                                <td>
                                    <%#Eval("intro")%>
                                </td>
                                <td>
                                    <%#Eval("createtime")%>
                                </td>
                                    <td align="center"  >
                                        <a href="javascript:openAjaxPopupModule('<%#Eval("ModuleID")%>','edit','<%#Eval("PID")%>');">编辑</a> 
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
                            <input type="button" class="button" value="删除" onclick="Delete();" />
                        </li>
                    </ul>
                </div>
            </div>
            <!--底部部分star-->
        </div>
    </div>
</form>
