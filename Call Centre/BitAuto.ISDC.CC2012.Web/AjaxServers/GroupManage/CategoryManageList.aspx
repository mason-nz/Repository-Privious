<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryManageList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GroupManage.CategoryManageList" %>

<div class="bit_table" id="bit_table">
    <div class="optionBtn clearfix">
        <%if (AddAuth)
          { %>
        <input type="button" value=" 新增分类 " onclick="On_Modify('')" class="newBtn" style="*margin-top: 3px;" />
        <%} %>
    </div>
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th style="color: Black; font-weight: bold; width: 20%">
                所属分组
            </th>
            <th style="color: Black; font-weight: bold; width: 25%">
                分类名称
            </th>
            <th style="color: Black; font-weight: bold; width: 15%">
                添加时间
            </th>
            <th style="color: Black; font-weight: bold; width: 15%">
                类型
            </th>
            <th style="color: Black; font-weight: bold; width: 10%">
                状态
            </th>
            <th style="color: Black; font-weight: bold; width: 15%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                                    <em>
                                        <%#Eval("GroupName")%></em>
                                </td>
                                <td>
                                    <em>
                                        <%#Eval("Name") %></em>
                                    <input type="text" value="<%#Eval("Name") %>" style="display: none" />&nbsp;
                                </td>
                                <td>
                                    <em>
                                        <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                                    </em>
                                </td>
                                <td>
                                    <em>
                                        <%#(Eval("Status").ToString() == "0" || Eval("Status").ToString() == "1") ? "自定义分组" : (Eval("Status").ToString() == "-3") ? "默认分组" : ""%>
                                </td>
                                <td>
                                    <%#(Eval("Status").ToString()=="0" ||Eval("Status").ToString()=="-3")?"启用":"停用" %>
                                </td>
                                <td>
                                    <%#Eval("Status").ToString() == "-3" ? "" : "<a href=javascript:void(0); onclick=groupEdit('" + Eval("SCID").ToString() + "','" + Eval("Name").ToString().Replace(" ","") + "','" + Eval("BGID").ToString() + "')>编辑</a>"%>
                                    <%#Eval("Status").ToString() == "-3" ? "" : "<a href=javascript:void(0); onclick=groupDelete("+Eval("SCID").ToString()+")>删除</a>"%>
                                    <a href="javascript:void(0);" onclick="groupSave(2,'<%#Eval("SCID") %>',this)" scid='<%#Eval("SCID") %>'
                                        style="display: none" name='a_save'>保存</a> <a href="javascript:void(0);" onclick="groupCancel('<%#Eval("SCID") %>',this)"
                                            style="display: none">取消</a> 
                                             <%#Eval("Status").ToString() == "0" ? "<a href=javascript:void(0); onclick=changeCategoryStatus(" + Eval("SCID").ToString() + ",this)>停用</a>" : ""%>
                                             <%#Eval("Status").ToString() == "1" ? "<a href=javascript:void(0); onclick=changeCategoryStatus(" + Eval("SCID").ToString() + ",this)>启用</a>" : ""%>
                                </td>
                            </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="pageHidden" value='<%=getCurrentPage() %>' />
    <script type="text/javascript">
        //修改
        function On_Modify(bgid) {
            $.openPopupLayer({
                name: "CategoryManageAdd",
                parameters: { BGID: bgid, r: Math.random() },
                url: "CategoryManageAdd.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        search();
                    }
                }
            });
        }

        function groupEdit(scid, name, groupId) {
            $.openPopupLayer({
                name: "CategoryManageEdit",
                parameters: { SCID: scid, BGID: groupId, CategoryName: name, r: Math.random() },
                url: "CategoryManageEdit.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        search();
                    }
                }
            });
        }
        //启用或停用
        function On_ChangeStatus(bgid, status) {
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "ChangeBusinessGroupStatus", BGID: bgid, Status: status, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    if (status == 0) {
                        //启用了
                        $("#span_status_" + bgid).text('启用');
                        $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_Modify('" + bgid + "')\">修改</a>&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','1')\">停用</a>");
                    }
                    else {
                        //停用了
                        $("#span_status_" + bgid).text('停用');
                        $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','0')\">启用</a>");
                    }
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }
    </script>
</div>
