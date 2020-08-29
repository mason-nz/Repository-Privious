<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact.List" %>
<form id="form1" runat="server">
<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="10%">联系人 </th>
        <th width="10%">职务 </th>
        <th width="20%">办公电话 </th>
        <th width="20%">移动电话 </th>
        <th width="20%">Email </th>
        <th width="10%">负责会员 </th>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td class><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim()=="1" ? "先生":"女士"%>--%>        
                <td class><%#Eval("Title").ToString()%></td>
                <td class="l">
                    <%#Eval("OfficeTel").ToString()%>                  
                </td>
                <td class="l">
                    <%#Eval("Phone").ToString()%>                    
                </td>
                <td ><%#Eval("Email").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td class><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim() == "1" ? "先生" : "女士"%>--%>
                <td class><%#Eval("Title").ToString()%></td>
                <td class="l"><%#Eval("OfficeTel").ToString()%></td>
                <td class="l">
                    <%#Eval("Phone").ToString()%>
                </td>
                <td ><%#Eval("Email").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>           
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float:right">
    <uc:AjaxPager ID="AjaxPager_Contact" runat="server" />
</div>
</form>
