<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrequencyStatisticsDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.FrequencyStatisticsDetails" %>

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
        <tr class="color_hui" >
            <th width="12%">
                <strong>评分人</strong>
            </th>
            <th width="8%">
                <strong>评分次数</strong>
            </th>
            <th width="8%">
                <strong>60秒</strong>
            </th>
            <th width="8%">
                <strong>61-90秒</strong>
            </th>
            <th width="8%">
                <strong>91-120秒</strong>
            </th>
            <th width="8%">
                <strong>121-150秒</strong>
            </th>
            <th width="8%">
                <strong>151-180秒</strong>
            </th>
            <th width="8%">
                <strong>181-210秒</strong>
            </th>
            <th width="8%">
                <strong>211-240秒</strong>
            </th>
            <th width="8%">
                <strong>241-270秒</strong>
            </th>
            <th width="8%">
                <strong>271-600秒</strong>
            </th>
            <th width="8%">
                <strong>600以上</strong>
            </th>
        </tr>
        </thead>
        <tbody id="AjexTableBody">
        <asp:repeater id="repeaterList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                    <%#GetEmNameByEid(int.Parse(Eval("CreateUserID").ToString()))%>&nbsp;
                    </td>
                    <td align="center">
                    <%# Eval("tCount")%>
                    <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=ScoreBeginTime %>&ScoreEndTime=<%=ScoreEndTime%>&ScoreCreater=<%#Eval("CreateUserID") %>&ScoreStatus=1'><%# Eval("tCount")%></a>--%>
                    &nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col1")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col2")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col3")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col4")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col5")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col6")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col7")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col8")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col9")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("col10")%>&nbsp;
                    </td>
                </tr>
            </ItemTemplate>
         </asp:repeater>
         </tbody>
    </table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
</div>
