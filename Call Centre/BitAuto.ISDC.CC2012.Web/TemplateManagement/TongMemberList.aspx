<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongMemberList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TongMemberList" %>

    

<div id="divMemberList" class="Table2">
    &nbsp;
    <table width="98%" cellspacing="0" cellpadding="0" border="1" id="tableMemberList"
        class="Table2List">
        <tbody>
            <tr class="bold color_hui">
                <th width="7%">
                    操作
                </th>
                <th width="22%">
                    经销商名称
                </th>
                <th width="15%">
                    400电话
                </th>
                <th width="9%">
                    所在城市
                </th>
                <th width="*">
                    地址
                </th>
                <th width="11%">
                    会员编号
                </th>
            </tr>
            <asp:repeater id="rptList" runat="server">
                <ItemTemplate>
            <tr>
                <td class="tdFirst" membercode="<%# Eval("MemberCode") %>">
                    <a href="#" class="linkSelect" >
                        选择</a>&nbsp;
                </td>
                <td class="l">
                    <%# Eval("Name") %>&nbsp;
                </td>
                <td class="l">
                    <div>
                        <%# Get400(Eval("MemberCode").ToString())%>&nbsp;
                        </div>
                </td>
                <td class="1">
                    <%# GetAreaName( Eval("CountyID").ToString()) %>&nbsp;
                </td>
                <td class="l">
                    <%# Eval("ContactAddress")%> &nbsp;
                </td>
                <td class="l">
                    <%# Eval("MemberCode")%>&nbsp;
                </td>
            </tr>
                            </ItemTemplate>
                </asp:repeater>
        </tbody>
    </table>
    <div class="pageTurn mr10">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
<table width="98%" cellspacing="0" cellpadding="0" border="1" class="Table2List mt20"
    id="tableCustBrandSelect">
    <tbody>
        <tr class="bold color_hui" style="background-color: transparent;">
            <th width="7%">
                操作
            </th>
            <th width="14%">
                经销商名称
            </th>
            <th width="15%">
                400电话
            </th>
            <th width="14%">
                所在城市
            </th>
            <th width="20%">
                地址
            </th>
            <th width="11%">
                会员编号
            </th>
        </tr>
        
        <tr style="background-color: transparent;" id='trSelect'>
         
        </tr>
    </tbody>
</table>
