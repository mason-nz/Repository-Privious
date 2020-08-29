<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignTask.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.AssignTask" %>

<link href="../../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../../Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $('#tableList tr:even').addClass('color_hui'); //设置列表行样式

    //查询操作
    function PersonSearch() {
        var pody = 'TrueName=' + escape($('#txtUserName').val()) + '&random=' + Math.random();
        //$('#divPersonList').load('../../AjaxServers/TaskManager/NoDealerOrder/AssignTask.aspx #divPersonList > *', pody, LoadDivSuccess);
        $('#divPersonList').load('/AjaxServers/GroupOrder/AssignTask.aspx #divPersonList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divPersonList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectPerson(userid) {
        var TaskIDS = '<%=TaskIDS%>';        

        $.post("/AjaxServers/GroupOrder/AssignTask.ashx", { Action: "AssignTask", TaskIDS: encodeURIComponent(TaskIDS), AssignUserID: userid }, function (data) {            
            var jsonData = $.evalJSON(data);
            if (jsonData.Result == "yes") {
                alert("分配任务成功！");
                $.closePopupLayer('AssignTaskAjaxPopup',true);

            }
            else {
                alert(jsonData.ErrorMsg);

                $.closePopupLayer('AssignTaskAjaxPopup');
            }
        });
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divPersonList').load('/AjaxServers/GroupOrder/AssignTask.aspx #divPersonList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            选择坐席</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignTaskAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 180px; float: left;" class="name1">所属分组： <span>
            <select id="ddlBussiGroup" runat="server" class="w100">
            </select></span>
        <li style="width: 180px; float: left;">姓名：
            <input type="text" name="txtUserName" id="txtUserName" runat="server" class="w100" /></li>
        <li class="btn">
            <input name="" type="button" value="查 询" onclick="javascript:PersonSearch();" class="btnSave bold" /></li>
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
                            <a href="javascript:SelectPerson('<%# Eval("UserID") %>')">选择</a>
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
