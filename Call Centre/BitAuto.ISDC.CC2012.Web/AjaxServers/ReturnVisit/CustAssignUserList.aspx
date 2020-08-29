<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustAssignUserList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.CustAssignUserList" %>

<link href="/Css/base.css" type="text/css" rel="stylesheet" />
<link href="/Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $('#tableList tr:even').addClass('color_hui'); //设置列表行样式

    $(document).ready(function () {
        enterSearch(PersonSearch);
    });

    //查询操作
    function PersonSearch() {
        var pody = "BGID=" + escape($('#<%=ddlBussiGroup.ClientID %>').val()) +
        "&TrueName=" + escape($.trim($('#txtUserName').val())) +
        "&Action=<%=Action %>" +
        "&DisplayGroupID=<%=DisplayGroupID %>" +
        "&Groups=<%=Groups %>" +
        "&random=" + Math.random();
        $('#divPersonList').load('/AjaxServers/ReturnVisit/CustAssignUserList.aspx #divPersonList > *', pody, LoadDivSuccess);
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
    function ShowDataByPost201(pody) {
        $('#divPersonList').load('/AjaxServers/ReturnVisit/CustAssignUserList.aspx #divPersonList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }

    (function () {
        document.getElementById("popAClear").onclick = function () {
            $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid', '');
            $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name', '');
            $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('agentnum', '');
            $.closePopupLayer('AssignTaskAjaxPopupForSelect', true);
        }
    })();
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            <asp:literal runat="server" id="literal_title"></asp:literal>
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignTaskAjaxPopupForSelect',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 230px; float: left;" class="name1">所属分组： <span>
            <select id="ddlBussiGroup" runat="server" class="w100" style="width: 160px;">
            </select>
        </span></li>
        <li style="width: 160px; float: left;">姓名：
            <input type="text" name="txtUserName" id="txtUserName" runat="server" class="w100" /></li>
        <li class="btn" style="width: 160px">
            <input name="" type="button" value="查 询" onclick="javascript:PersonSearch();" class="btnSave bold" />&nbsp;&nbsp;<a
                id='popAClear' style="cursor: pointer">清空已选项</a></li>
    </ul>
    <div class="Table2" id="divPersonList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableList">
            <tbody>
                <tr class="bold">
                    <th width="10%">
                        操作
                    </th>
                    <th width="10%">
                        姓名
                    </th>
                    <th width="5%">
                        工号
                    </th>
                    <th width="30%">
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
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
    </div>
</div>
