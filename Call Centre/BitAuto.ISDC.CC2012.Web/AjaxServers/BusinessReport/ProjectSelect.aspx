<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectSelect.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport.ProjectSelect" %>

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
        "&ProjectName=" + escape($('#<%=txtTProjectName.ClientID %>').val()) +
        "&random=" + Math.random();
        $('#divProjectList').load('/AjaxServers/BusinessReport/ProjectSelect.aspx #divProjectList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divProjectList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectProject(ProjectID, name, Source,projecttime) {
        $('#dataID').data('ProjectID', ProjectID);
        $('#dataID').data('name', name);
        $('#dataID').data('Source', Source);
        $('#dataID').data('ProjectTime', projecttime);
        $.closePopupLayer('AssignTaskAjaxPopupForSelect', true);
    }
    //分页操作
    function ShowDataByPost2(pody) {
        $('#divProjectList').load('/AjaxServers/BusinessReport/ProjectSelect.aspx #divProjectList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }

    
</script>
<div class="pop pb15 openwindow" style="width: 700px">
    <div class="title bold">
        <h2>
            <asp:literal runat="server" id="literal_title"></asp:literal>
        </h2>
        <span><a style="top: 38px" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignTaskAjaxPopupForSelect',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 180px; float: left;" class="name1">所属分组： <span>
            <select id="ddlBussiGroup" runat="server" class="w100">
            </select></span></li>
        <li style="width: 250px; float: left;">项目名称：
            <input type="text" name="txtTProjectName" id="txtTProjectName" runat="server" class="w180" /></li>
        <li class="btn" style="width: 160px">
            <input name="" type="button" value="查 询" onclick="javascript:PersonSearch();" class="btnSave bold" /></li>
    </ul>
    <div class="Table2" id="divProjectList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableList">
            <tbody>
                <tr class="bold">
                    <th width="5%">
                        操作
                    </th>
                    <th width="20%">
                        项目名称
                    </th>
                    <th width="18%">
                        所属分组
                    </th>
                    <th width="10%">
                        创建时间
                    </th>
                </tr>
                <asp:repeater id="repterProjectlist" runat="server">
                    <ItemTemplate>
                      <tr>
                        <td> 
                            <a href="javascript:SelectProject('<%# Eval("ProjectID") %>','<%# Eval("Name") %>','<%#Eval("Source")%>','<%#Eval("createtime")%>')">选择</a>
                        </td>
                        <td class="l">
                            <%#Eval("Name").ToString()%>&nbsp;
                        </td>                
                        <td class="l">                            
                            <%#Eval("bgname")%>&nbsp;
                        </td>
                        <td class="l"> 
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString())%>&nbsp;
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
