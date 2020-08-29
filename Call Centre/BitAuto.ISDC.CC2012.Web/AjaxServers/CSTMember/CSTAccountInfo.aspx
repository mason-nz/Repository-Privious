<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSTAccountInfo.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CSTMember.CSTAccountInfo" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse">
    <tr>
        <td class="cstText" style="width: 12%">
            <strong>状态</strong>
        </td>
        <td class="cstText" style="width: 14%">
            <strong>登录名</strong>
        </td>
        <td class="cstText" style="width: 10%">
            <strong>登录次数</strong>
        </td>
        <td class="cstText" style="width: 20%">
            <strong>最后登录时间</strong>
        </td>
        <td class="cstText" style="width: 12%">
            <strong>真实姓名</strong>
        </td>
        <td class="cstText" style="width: 12%">
            <strong>手机</strong>
        </td>
        <td class="cstText" style="width: 20%">
            <strong>座机</strong>
        </td>
    </tr>
    <asp:repeater id="repterCSTAcountList" runat="server">
        <ItemTemplate>
            <tr>
                <td class="cstValue" style="width:14%"><%# GetCstAccountStatusString(Convert.ToInt32(Eval("Status").ToString()))%></td>
                <td class="cstValue" style="width:12%"><%# Eval("UserName")%></td>
                <td class="cstValue" style="width:10%"><%# Eval("LoginCount")%></td>
                <td class="cstValue" style="width:20%"><%# Eval("LastLoginTime")%></td>
                <td class="cstValue" style="width:12%"><%# Eval("FullName")%></td>
                <td class="cstValue" style="width:12%"><%# Eval("Mobile")%> </td>
                <td class="cstValue" style="width:20%"><%# Eval("Tel")%></td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>