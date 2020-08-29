<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerInfoView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustCategory.DealerInfoView" %>
<ul>
    <li>
        <label>
            经销商名称：</label>
        <asp:Label ID="lblMemberName" runat="server" Text=""></asp:Label></li>
    <li style="display: none">
        <label>
            城市范围：</label>
        <asp:Label ID="lblCityScope" runat="server" Text=""></asp:Label>
    </li>
    <li style="display: none">
        <label>
            经销商类型：</label>
        <asp:Label ID="lblMemberType" runat="server" Text=""></asp:Label>
    </li>
    <li style="display: none">
        <label>
            经营范围：</label>
        <asp:Label ID="lblCarType" runat="server" Text=""></asp:Label>
    </li>
    <li style="display: none">
        <label>
            品牌：</label>
        <asp:Label ID="lblBrand" runat="server" Text=""></asp:Label>
    </li>
    <li style="display: none">
        <label>
            会员ID：</label>
        <asp:Label ID="lblMemberID" runat="server" Text=""></asp:Label>
    </li>
    <li style="display: none">
        <label>
            经销商状态：</label>
        <asp:Label ID="lblMemberStatus" runat="server" Text=""></asp:Label>
    </li>
    <li style="width: 940px">
        <label style="width: 120px">
            备注：</label>
        <asp:Label ID="lblRemark" style="width: 706px" runat="server" Text=""></asp:Label>
    </li>
</ul>
<%--<div>
    <table>
        <tr>
            <td>
                经销商名称：
            </td>
            <td>
                <asp:Label ID="lblMemberName" runat="server" Text=""></asp:Label>
            </td>
            <td>
                城市范围：
            </td>
            <td>
                <asp:Label ID="lblCityScope" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                经销商类型：
            </td>
            <td>
                <asp:Label ID="lblMemberType" runat="server" Text=""></asp:Label>
            </td>
            <td>
                经营范围：
            </td>
            <td>
                <asp:Label ID="lblCarType" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                品牌：
            </td>
            <td>
                <asp:Label ID="lblBrand" runat="server" Text=""></asp:Label>
            </td>
            <td>
                会员ID：
            </td>
            <td>
                <asp:Label ID="lblMemberID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                经销商状态：
            </td>
            <td>
                <asp:Label ID="lblMemberStatus" runat="server" Text=""></asp:Label>
            </td>
            <td>
                备注：
            </td>
            <td>
                <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</div>
--%>