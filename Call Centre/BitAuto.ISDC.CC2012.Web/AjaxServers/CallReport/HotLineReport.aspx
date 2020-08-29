<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotLineReport.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport.HotLineReport" %>

<script type="text/javascript">
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="table_data">
        <tr class="color_hui">
            <th width="20%">
                <strong>日期</strong>
            </th>
            <th width="10%">
                <strong>业务类型</strong>
            </th>
            <th width="11%">
                <strong>呼入量</strong>
            </th>
            <th width="11%">
                <strong>进入队列总量</strong>
            </th>
            <th width="11%">
                <strong>电话总接通量</strong>
            </th>
            <th width="11%">
                <strong>接通率</strong>
            </th>
            <th width="13%">
                <strong>30秒内服务水平</strong>
            </th>
            <th width="13%">
                <strong>平均等待时长（秒）</strong>
            </th>
        </tr>
        <asp:repeater id="repeaterList" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("begintime")%>&nbsp;
                    </td>
                    <td>
                        <%# Eval("objecttype")%>&nbsp;
                    </td>
                    <td>
                        <%# Eval("n_entered")%>&nbsp;
                    </td>
                    <td>
                        <%--修改转人工量指标 2015-3-4 强斐--%>
                        <%# Eval("n_entered_out")%>&nbsp;
                    </td>
                    <td>
                        <%# Eval("n_answered")%>&nbsp;
                    </td>
                    <td>
                        <%# Eval("pc_n_answered")%>&nbsp;
                    </td>
                     <td>
                        <%# Eval("pc_n_distrib_in_tr")%>&nbsp;
                    </td>
                     <td>
                        <%--队列平均排队时长 qiangfei 2015-3-5--%>
                        <%# Eval("av_t_answered")%>&nbsp;
                    </td>
                </tr>
            </ItemTemplate>
         </asp:repeater>
        <tr style="background-color: #f5f5f5; height: 45px;">
            <td style="font-weight: bold;">
                合计（共<asp:literal runat="server" id="total_count"></asp:literal>项）
            </td>
            <td style="font-weight: bold;">
                <asp:literal runat="server" id="objecttype"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <asp:literal runat="server" id="n_entered"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <%--修改转人工量指标 2015-3-4 强斐--%>
                <asp:literal runat="server" id="n_entered_out"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <asp:literal runat="server" id="n_answered"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <asp:literal runat="server" id="pc_n_answered"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <asp:literal runat="server" id="pc_n_distrib_in_tr"></asp:literal>
            </td>
            <td style="font-weight: bold;">
                <%--队列平均排队时长 qiangfei 2015-3-5--%>
                <asp:literal runat="server" id="av_t_answered"></asp:literal>
            </td>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
    <input type="hidden" id="TotalCount" runat="server" />
</div>
