<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustAssignUserList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.CustAssignUserList" %>

<script type="text/javascript">
    $('#tableList tr:even').addClass('color_hui'); //设置列表行样式

    $(document).ready(function () {
        enterSearch(PersonSearch);
    });

    //查询操作
    function PersonSearch() {
        var pody = "BGID=" + escape($('#<%=ddlBussiGroup.ClientID %>').val()) +
        "&TrueName=" + escape($.trim($('#txtUserName').val())) +
        "&IsHaveCompetence=<%=IsHaveCompetence %>" +
        "&random=" + Math.random();
        $('#divPersonList').load('/AjaxServers/CustAssignUserList.aspx #divPersonList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divPersonList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectPerson(userid, name, agentnum) {
        $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid', userid);
        $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name', name);
        $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('agentnum', agentnum);
        $.closePopupLayer('AssignTaskAjaxPopupForSelect', true);

    }
    //分页操作
    function ShowDataByPost2(pody) {
        $('#divPersonList').load('/AjaxServers/CustAssignUserList.aspx #divPersonList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }

    (function () {
        document.getElementById("popAClear").onclick = function () {
            $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid', '');
            $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name', '');
            $.closePopupLayer('AssignTaskAjaxPopupForSelect', true);
        }
    })();
</script>
<div class="popup">
    <div class="title ft14">
        <asp:literal runat="server" id="literal_title"></asp:literal>
        <a class="right" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignTaskAjaxPopupForSelect',false);">
            <img src="/Images/c_btn.png" border="0" alt="关闭" /></a></div>
    <div class="content" id="divPersonList">
        <div class="search">
            <ul>
                <li class="name1">
                    <label>
                        所属分组：
                    </label>
                    <select id="ddlBussiGroup" runat="server" class="w100">
                    </select></li>
                <li>
                    <label>
                        姓名：
                    </label>
                    <input type="text" name="txtUserName" id="txtUserName" class="w200" /></li>
                <li style="width: 160px;" class="btn">
                    <input name="" type="button" value="查 询" onclick="javascript:PersonSearch();" class="w60" />&nbsp;&nbsp;<a
                        id='popAClear' style="cursor: pointer">清空已选项</a></li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <table cellspacing="0" cellpadding="0" class="fzList">
            <tbody>
                <tr>
                    <th width="15%">
                        操作
                    </th>
                    <th width="25%">
                        姓名
                    </th>
                    <th width="15%">
                        工号
                    </th>
                    <th width="40%">
                        角色
                    </th>
                </tr>
                <asp:repeater id="repterPersonlist" runat="server">
                    <ItemTemplate>
                      <tr>
                        <td> 
                            <a href="javascript:SelectPerson('<%# Eval("UserID") %>','<%# Eval("trueName") %>','<%#Eval("AgentNum")%>')">选择</a>
                        </td>
                        <td class="l">
                            <%#Eval("trueName").ToString()%>&nbsp;
                        </td>                
                        <td class="l">                            
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td class="l"> 
                            <%#Eval("RoleName")%>&nbsp;
                        </td>
                      </tr>
                    </ItemTemplate>
                  </asp:repeater>
            </tbody>
        </table>
        <div class="pagesnew" style="margin: 10px 0 0 0;">
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
        <div class="clearfix">
        </div>
    </div>
</div>
