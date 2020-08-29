<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyCarInfoView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExternalTask.CustCategory.BuyCarInfoView" %>
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
<%--<div>
    <table>
        <tr>
            <td>
                年龄：
            </td>
            <td>
                <asp:Label ID="lblAge" runat="server"></asp:Label>
            </td>
            <td>
                身份证号：
            </td>
            <td>
                <asp:Label ID="lblIDCard" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                职业：
            </td>
            <td>
                <asp:Label ID="lblVocation" runat="server"></asp:Label>
            </td>
            <td>
                婚姻状态：
            </td>
            <td>
                <asp:Label ID="lblMarriage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                个人收入：
            </td>
            <td colspan="3">
                <asp:Label ID="lblInCome" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                目前驾驶：
            </td>
            <td>
                <asp:Label ID="lblCarName" runat="server"></asp:Label>
            </td>
            <td>
                是否认证车主：
            </td>
            <td>
                <asp:Label ID="lbllegalize" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                驾龄：
            </td>
            <td>
                <asp:Label ID="lblDriveAge" runat="server"></asp:Label>年
            </td>
            <td>
                用户名：
            </td>
            <td>
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                车牌号：
            </td>
            <td>
                <asp:Label ID="lblCarNumber" runat="server"></asp:Label>
            </td>
            <td>
                备注：
            </td>
            <td>
                <asp:Label ID="lblNote" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</div>
--%>