<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkReportStat.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.WorkReport.WorkReportStat" %>

<div class="work_bgcon">
    <table border="1" cellpadding="0" cellspacing="0" width="100%" class="work_sum">
        <tr>
            <th rowspan="2">
                提交人
            </th>
            <th colspan="3">
                <%=MonthFirst %>月
            </th>
            <th colspan="3">
                <%=MonthCenter %>月
            </th>
            <th colspan="3">
                <%=MonthLast %>月
            </th>
        </tr>
        <tr>
            <th>
                日报
            </th>
            <th>
                周报
            </th>
            <th>
                月报
            </th>
            <th>
                日报
            </th>
            <th>
                周报
            </th>
            <th>
                月报
            </th>
            <th>
                日报
            </th>
            <th>
                周报
            </th>
            <th>
                月报
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("提交人") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num1") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num2") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num3") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num4") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num5") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num6") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num7") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num8") %>&nbsp;
                    </td>
                    <td>
                        <%#Eval("num9") %>&nbsp;
                    </td>
                </tr>
            </ItemTemplate>
        </asp:repeater>
    </table>
</div>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<input id="input_page" type="hidden" value="<%=Page %>" />
