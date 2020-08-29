<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CooperationProjectList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CooperationProjectList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<%--<h2>
    <span>合作项列表</span>
</h2>--%>
<%--<div class="daochu">--%>
<%-- <select name="" class="k120" id="selectOrderField" onchange="javascript:RefreshCooperationPro();">
        <option value="BeginTime">按开通时间排序</option>
        <option value="ProjectType" selected="selected">按产品名称排序</option>
    </select>--%>
<%--</div>--%>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="15%">
            类型
        </th>
        <th width="25%">
            合作名称
        </th>
        <th width="10%">
            销售类型
        </th>
        <th width="27%">
            执行周期
        </th>
        <th width="13%">
            创建时间
        </th>
        <th width="10%">
            备注
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr> 
                <td><%# Eval("type")%></td> 
                <td><%# Eval("name")%></td> 
                <td><%# Eval("saletype")%></td> 
                <td><%# Eval("period")%></td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString(), "yyyy-MM-dd")%>&nbsp;
                </td>
                <td><%# Eval("note")%></td>  
             </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td><%# Eval("type")%></td> 
                <td><%# Eval("name")%></td> 
                <td><%# Eval("saletype")%></td> 
                <td><%# Eval("period")%></td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString(), "yyyy-MM-dd")%>&nbsp;
                </td>
                <td><%# Eval("note")%></td> 
             </tr>   
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPagerCooperationProjects" runat="server" PageSize="5" />
</div>
