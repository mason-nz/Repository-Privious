<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustInfoView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.CustInfoView" %>
<div class="content" style="padding-top: 0px; padding-bottom: 0px;">
    <div class="titles bd ft14">
        用户信息</div>
    <div class="lineS">
    </div>
    <table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
        <tr>
            <th style=" width:150px;">
                姓名：
            </th>
            <td style=" width:330px;">
                 <%=CustName%> 
            </td>
            <th style=" width:150px;">
                性别：
            </th>
            <td style=" width:330px;">
                <%=Sex%>
            </td>
        </tr>
        <tr>
            <th style=" width:150px;">
                电话号码：
            </th>
            <td style=" width:330px;" id="td_CustPhones">
                <%=Tels%>
            </td>
            <th style=" width:150px;">
                地区：
            </th>
            <td style=" width:330px;">
                <%=PlaceStr%>
            </td>
        </tr>
        <tr>
            <th style=" width:150px;">
                客户分类：
            </th>
            <td style=" width:330px;">
                <%=CustCategoryStr%>
            </td>
            <th style=" width:150px;" id="MemberNameTd" runat="server">
            </th>
            <td style=" width:330px;">
                <%=MemberName%>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdTels" value="<%=Tels%>" />
</div>
