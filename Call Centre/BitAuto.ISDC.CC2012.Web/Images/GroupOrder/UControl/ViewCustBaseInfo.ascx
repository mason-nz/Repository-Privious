<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewCustBaseInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupOrder.UControl.ViewCustBaseInfo" %>
<ul class="clearfix">
    <li>
        <label>
            姓名：</label><span><%=CustName%></span></li>
    <li>
        <label>
            性别：</label><span><%=Sex%></span></li>
    <li>
        <label>
            电话：</label><span><%=Tels%></span></li>
    <li>
        <label>
            地区：</label><span><%=PlaceStr%></span></li>
    <li>
        <label>
            客户分类：</label><span>个人</span></li>
    <li>
        <label>
            分属大区：</label><span><%=AreaStr%></span></li>    
    <li>
        <label>
            汽车通：
        </label>
        <asp:Label ID="lblUserName" runat="server"></asp:Label>
    </li>
    
</ul>
<%--<div class="line">
</div>--%>
