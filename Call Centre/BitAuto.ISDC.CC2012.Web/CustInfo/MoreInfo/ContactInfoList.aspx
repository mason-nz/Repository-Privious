<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactInfoList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.ContactInfoList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<form id="form1" runat="server">
<%--<h2 id="Tab" runat="server">
    <span>联系人列表 </span>
</h2>--%>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="10%">联系人 </th>
        <th width="10%">部门 </th>
        <th width="10%">职务 </th>
        <th width="12%">办公电话 </th>
        <th width="10%">移动电话 </th>
        <th width="12%">Email </th>
        <th width="10%">传真 </th>
        <th width="10%">负责会员</th>
        <th width="10%">出生日期 </th>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td class="l"><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim()=="1" ? "先生":"女士"%>--%>
                <td class="l"><%#Eval("DepartMent").ToString()%></td>
                <td class="l"><%#Eval("Title").ToString()%></td>
                <td><%#Eval("OfficeTel").ToString()%></td>
                <td><%#Eval("Phone").ToString()%></td>
                <td class="l"><%#Eval("Email").ToString()%></td>
                <td><%#Eval("Fax").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>
                <td><%#Eval("birthday").ToString()%></td>               
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td class="l"><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim() == "1" ? "先生" : "女士"%>--%>
                <td class="l"><%#Eval("DepartMent").ToString()%></td>
                <td class="l"><%#Eval("Title").ToString()%></td>
                <td><%#Eval("OfficeTel").ToString()%></td>
                <td><%#Eval("Phone").ToString()%></td>
                <td class="l"><%#Eval("Email").ToString()%></td>
                <td><%#Eval("Fax").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>
                <td><%#Eval("birthday").ToString()%></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float:right">
    <uc1:AjaxPager ID="AjaxPager_Contact" runat="server" PageSize="5"/>
</div>
</form>
