<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InComingCallDetails.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport.InComingCallDetails" %>

<script type="text/javascript">
    $(function () {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        $("#tableReturnVisitCust th").css({ "line-height": "18px" });
        SetTableStyle('tableReturnVisitCust');
    });
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableReturnVisitCust');
    }
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
        <thead>
            <tr class="color_hui">
                <th width="9%">
                    <strong>日期</strong>
                </th>
                <th width="5%">
                    <strong>客服</strong>
                </th>
                <th width="5%">
                    <strong>工号</strong>
                </th>
                <th width="6%">
                    <strong>电话总<br />
                        接通量</strong>
                </th>
                <th width="6%">
                    <strong>总振铃<br />
                        时长</strong>
                </th>
                <th width="6%">
                    <strong>总通话<br />
                        时长</strong>
                </th>
                <th width="6%">
                    <strong>总话后<br />
                        时长</strong>
                </th>
                <th width="6%">
                    <strong>总在线<br />
                        时长</strong>
                </th>
                <th width="6%">
                    <strong>工时利<br />
                        用率</strong>
                </th>
                <th width="6%">
                    <strong>平均振铃<br />
                        时长(秒)</strong>
                </th>
                <th width="6%">
                    <strong>平均通话<br />
                        时长(秒)</strong>
                </th>
                <th width="6%">
                    <strong>平均话后<br />
                        时长(秒)</strong>
                </th>
                <th width="6%">
                    <strong>置忙总<br />
                        时长</strong>
                </th>
                <th width="5%">
                    <strong>置忙<br />
                        次数</strong>
                </th>
                <th width="6%">
                    <strong>平均置忙<br />
                        时长(秒)</strong>
                </th>
                <th width="5%">
                    <strong>电话转<br />
                        出次数</strong>
                </th>
                <th width="5%">
                    <strong>电话转<br />
                        入次数</strong>
                </th>
            </tr>
        </thead>
        <tbody id="AjexTableBody">
            <asp:repeater id="repeaterList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%# Eval("StartTime")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("TrueName")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("AgentNum")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("N_CallIsQuantity")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("T_RingingTime")%>&nbsp;
                    </td>

                    <td align="center">
                        <%# Eval("T_TalkTime")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("T_AfterworkTime")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("T_SetLogin")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("P_WorkTimeUse")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("A_AverageRingTime")%>&nbsp;
                    </td>

                    <td align="center">
                        <%# Eval("A_AverageTalkTime")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("A_AfterworkTime")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("T_SetBuzy")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("N_SetBuzy")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("A_AverageSetBusy")%>&nbsp;
                    </td>

                    <td align="center">
                        <%# Eval("N_TransferOut")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("N_TransferIn")%>&nbsp;
                    </td>
                </tr>
            </ItemTemplate>
         </asp:repeater>
        </tbody>
        <tfoot>
            <tr style="background-color: #f5f5f5; height: 45px;">
                <td style="font-weight: bold;" width="9%">
                    合计(共<%=RecordCount %>项)
                </td>
                <td style="font-weight: bold;" width="5%">
                    --
                </td>
                <td style="font-weight: bold;" width="5%">
                    --
                </td>
                <td style="font-weight: bold;" id="t_N_CallIsQuantity" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_T_RingingTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_T_TalkTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_T_AfterworkTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_T_SetLogin" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_P_WorkTimeUse" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_A_AverageRingTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_A_AverageTalkTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_A_AfterworkTime" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_T_SetBuzy" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_N_SetBuzy" width="5%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_A_AverageSetBusy" width="6%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_N_TransferOut" width="5%">
                    &nbsp;
                </td>
                <td style="font-weight: bold;" id="t_N_TransferIn" width="5%">
                    &nbsp;
                </td>
            </tr>
        </tfoot>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
    <input type="hidden" id="nowDt" runat="server" />
    <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
</div>
