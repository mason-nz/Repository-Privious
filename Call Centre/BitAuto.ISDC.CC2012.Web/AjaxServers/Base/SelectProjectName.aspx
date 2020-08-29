<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectProjectName.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Base.SelectProjectName" %>

<script type="text/javascript">
    $('#tableBrandList tr:even').addClass('color_hui'); //设置列表行样式
    function SelectProjectName(ProjectName) {
        $('#popupLayer_' + 'ProjectNameSelectAjaxPopup').data('ProjectName', ProjectName);
        $.closePopupLayer('ProjectNameSelectAjaxPopup', true);
    }
    function Search() {
        var pody = 'ProjectName=' + escapeStr($('#txtBrandName').val()) + '&page=1&r=' + Math.random();
        LoadingAnimation("divBrandList");
        $('#divBrandList').load('/AjaxServers/Base/SelectProjectName.aspx #divBrandList > *', pody);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divBrandList').load('/AjaxServers/Base/SelectProjectName.aspx #divBrandList > *', pody);
    }
    $(function () {
        enterSearch(Search);
    }); 
</script>
<div id="jcDiv" class="pop pb15 openwindow" style="width: 660px">
    <div class="title bold">
        <h2>
            <em id="spanTitle" runat="server" style="color: #FFFFFF;">集采项目信息</em></h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ProjectNameSelectAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1">
            <label>
                集采项目名：</label>
            <input type="text" class="k200" name="txtBrandName" maxlength="100" id="txtBrandName"
                runat="server" /></li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:Search();" /></li>
    </ul>
    <div class="Table2" id="divBrandList">
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableBrandList">
            <tbody>
                <tr>
                    <th width="20%">
                        操作
                    </th>
                    <th width="70%">
                        集采项目名
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                                <a href="javascript:SelectProjectName('<%# Eval("projectname") %>');" name='<%# Eval("projectname")%>' id='<%# Eval("projectname") %>'>选择</a>
                                                </td>
                                        <td class="l">
                                            <%# Eval("projectname")%>
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
