<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyCarInfoView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.BuyCarInfoView" %>
<ul class="clearfix ">
    <li>
        <label>
            年龄：</label><asp:Label ID="lblAge" runat="server"></asp:Label></li>
    <li>
        <label>
            身份证号：</label><asp:Label ID="lblIDCard" runat="server"></asp:Label></li>
    <li>
        <label>
            职业：</label><asp:Label ID="lblVocation" runat="server"></asp:Label></li>
    <li>
        <label>
            婚姻状态：</label>
        <asp:Label ID="lblMarriage" runat="server"></asp:Label>
    <li>
        <label>
            个人收入：</label>
        <asp:Label ID="lblInCome" runat="server"></asp:Label></li></ul>
<div class="line">
</div>
<ul class="clearfix ">
    <li>
        <label>
            目前驾驶：</label><asp:Label ID="lblCarName" runat="server"></asp:Label></li>
    <li>
        <label>
            是否认证车主：</label>
        <asp:Label ID="lbllegalize" runat="server"></asp:Label></li>
    <li>
        <label>
            驾龄：
        </label>
        <asp:Label ID="lblDriveAge" runat="server"></asp:Label>年</li>
    <li>
        <label>
            用户名：
        </label>
        <asp:Label ID="lblUserName" runat="server"></asp:Label>
    </li>
    <li>
        <label>
            车牌号：
        </label>
        <asp:Label ID="lblCarNumber" runat="server"></asp:Label>
    </li>
    <li>
        <label>
            备注：</label>
        <asp:Label ID="lblNote" runat="server"></asp:Label>
    </li>
</ul>
<div class="line">
</div>
<ul class="clearfix">
    <li>
        <label>
            创建人：</label><span>易湃</span></li>
    <li>
        <label>
            创建时间：</label><span><%=CreateTime%></span></li>
    <li>
        <label>
            修改人：</label><span><%=UpdateUser%></span></li>
    <li>
        <label>
            修改时间：</label><span><%=UpdateTime%></span></li>
</ul>
