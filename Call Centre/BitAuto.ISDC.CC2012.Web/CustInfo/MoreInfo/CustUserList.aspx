<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustUserList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CustUserList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<script type="text/javascript" language="javascript">
    var cUMHelper = (function () {
        var goToCUMLogList = function () {
            var url = '/CustInfo/MoreInfo/CustUserLogList.aspx';
            var data = { CustID: '<%=CustID %>', ContentElementId: '<%=this.AjaxPager_CUM.ContentElementId %>' };
            jQuery('#<%=this.AjaxPager_CUM.ContentElementId %>').load(url, data);
        };

        return { goToCUMLogList: goToCUMLogList };
    })();
   
    
</script>
<form id="form1" runat="server">
<h2>
    <span>员工列表</span><a href="#this" onclick="javascript:cUMHelper.goToCUMLogList();">[历史分配记录]</a>
</h2>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableCustUserList">
    <tr>
        <th width="7%">
            员工姓名
        </th>
        <th width="14%">
            部门
        </th>
        <th width="16%">
            负责会员
        </th>
        <th width="12%">
            业务线
        </th>
        <th width="12%">
            分机
        </th>
        <th width="10%">
            移动电话
        </th>
        <th width="14%">
            分配时间
        </th>
        <th width="5%">
            状态
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater_CUM">
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("TrueName").ToString() %>
                </td>
                <td class="l">
                    <%#Eval("DepartName").ToString() %>
                </td>
                <td>
                    <%#GetManageMember(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#GetBussinessName(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#Eval("Officetel").ToString() %>
                </td>
                <td>
                    <%#Eval("Mobile").ToString() %>
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# Eval("Status").ToString().Trim() == "0" 
                        ? "在用" 
                        : (
                        Eval("Status").ToString().Trim() == "1" ? "停用" : "已删除"
                          )
                    %>
                </td>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td>
                    <%#Eval("TrueName").ToString() %>
                </td>
                <td class="l">
                    <%#Eval("DepartName").ToString() %>
                </td>
                <td>
                    <%#GetManageMember(Eval("UserID").ToString())%>
                </td>
                 <td>
                    <%#GetBussinessName(Eval("UserID").ToString())%>
                </td>
                <td>
                    <%#Eval("Officetel").ToString() %>
                </td>
                <td>
                    <%#Eval("Mobile").ToString() %>
                </td>
                <td>
                   <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# Eval("Status").ToString().Trim() == "0" 
                        ? "在用" 
                        : (
                        Eval("Status").ToString().Trim() == "1" ? "停用" : "已删除"
                          )
                    %>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager_CUM" runat="server" PageSize="5" />
</div>
</form>
