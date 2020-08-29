<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subordinate.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.WorkReport.Subordinate" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th name='报告主题' style="width: 24%;">
            报告主题
        </th>
        <th name='提交人' style="width: 12%;">
            提交人
        </th>
        <th name='提交时间' style="width: 15%;">
            提交时间
        </th>
        <th name='审阅状态' style="width: 15%;">
            审阅状态
        </th>
        <th name='回复数' style="width: 11%;">
            回复数
        </th>
        <th name='最后回复' style="width: 23%;">
            最后回复
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr>
                <td class="cName aorou">
                    <%# GetTitle(Container.DataItem as BitAuto.YanFa.Crm2009.Entities.WorkReport) %>&nbsp;
                </td>
                <td>
                     <span class="aorou"><%# Eval("CreateUserName") %></span>&nbsp;
                </td>
                <td>
                    <span class="aorou">
                        <%# ((BitAuto.YanFa.Crm2009.Entities.WorkReport) Container.DataItem).Status == BitAuto.YanFa.Crm2009.Entities.WorkReportStatus.Submitted ?
                            DateTimeToString(((BitAuto.YanFa.Crm2009.Entities.WorkReport)Container.DataItem).PostTime) : 
                            string.Empty %></span>
                        &nbsp;
                </td>
                <td>
                    <%# GetStatus(Container.DataItem as BitAuto.YanFa.Crm2009.Entities.WorkReport)%>&nbsp;
                </td>
                <td>
                    <%# Eval("ReplyNum") %>&nbsp;
                </td>
                <td>
                    <%# Eval("LastReplyUserName") %>&nbsp;&nbsp;
                    <span class="aorou"><%# DateTimeToString(((BitAuto.YanFa.Crm2009.Entities.WorkReport)Container.DataItem).LastReplyTime)%></span>
                    &nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<input id="input_page" type="hidden" value="<%=Page %>" />