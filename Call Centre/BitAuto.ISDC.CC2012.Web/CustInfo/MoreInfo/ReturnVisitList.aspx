<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnVisitList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.ReturnVisitList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<form id="form1" runat="server">
<%--<h2>
    <span>访问信息列表 </span>
</h2>--%>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="10%">访问类别 </th>
        <th width="10%">访问方式 </th>
        <th width="15%">访问日期 </th>
        <th width="23%">访问描述 </th>
        <th width="6%">访问员 </th>
        <th width="10%">访问部门 </th>
    </tr>
    <asp:repeater runat="server" id="repeater_RVL">
        <ItemTemplate>
            <tr> 
                <td>
                    <%#getUserClassStr( Eval("userclass").ToString())%>记录
                </td>
                <td><%#getTypeStr(Eval("RVType").ToString())%></td> 
                <td><%#DateDiff(Eval("begintime").ToString(),Eval("endtime").ToString())%></td>
                <td><a href="javascript:void(0)" title="<%#Eval("Remark").ToString()%>" style="text-decoration:none; color:#333; cursor:inherit;"><%#Eval("showRemark").ToString()%></a></td>
                <td><%#Eval("truename").ToString()%></td>
                <td><%#Eval("departname").ToString()%></td>          
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
            <td  >
                <%#getUserClassStr( Eval("userclass").ToString())%>记录
                </td>
                <td><%#getTypeStr(Eval("RVType").ToString())%></td> 
                <td><%#DateDiff(Eval("begintime").ToString(),Eval("endtime").ToString())%></td>
                <td><a href="javascript:void(0)" title="<%#Eval("Remark").ToString()%>" style="text-decoration:none; color:#333; cursor:inherit;"><%#Eval("showRemark").ToString()%></a></td>
                <td><%#Eval("truename").ToString()%></td>
                <td><%#Eval("departname").ToString()%></td>            
            </tr>      
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float:right">
    <uc1:AjaxPager ID="AjaxPager_RVL" runat="server" PageSize="5" />
</div>
</form>
