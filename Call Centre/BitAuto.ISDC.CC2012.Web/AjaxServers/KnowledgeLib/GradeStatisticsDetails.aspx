<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GradeStatisticsDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.GradeStatisticsDetails" %>

<script type="text/javascript">
    $(function () {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        $("#tableReturnVisitCust th").css({ "line-height": "18px" });
        SetTableStyle('tableReturnVisitCust');
        var type = '<%=searchType %>';
        if (type == 'hotLine') {
            $("#onLineTable").hide();
            $("#tableReturnVisitCust").show();
        }
        else if (type == 'onLine') {
            $("#tableReturnVisitCust").hide();
            $("#onLineTable").show();
        }
    });

    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableReturnVisitCust');
    }
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust" >
    <thead>
        <tr class="color_hui" >
            <th width="20%">
                <strong>所属分组</strong>
            </th>
                <th width="10%">
                <strong>客服人数</strong>
            </th>
            <th width="10%">
                <strong>总通话量</strong>
            </th>
                <th width="10%">
                <strong>总通话时长</strong>
            </th>
            <th width="10%">
                <strong>抽检量</strong>
            </th>
            <th width="10%">
                <strong>抽检率</strong>
            </th>
            <th width="10%">
           
                <strong>合格量</strong>
            </th>
            <th width="10%">
                <strong>合格率</strong>
            </th>
                <th width="10%">
                <strong>平均分</strong>
            </th>
        </tr>
        </thead>
        <tbody id="AjexTableBody">
        <asp:repeater id="repeaterList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%# Eval("Name")%>&nbsp;
                    </td>
                       <td align="center">
                        <%# Eval("t_AgentCount")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("N_ETotal")%>
                        <%--<a href='/TrafficManage/CallRecord.aspx?BeginTime=<%=StartTime %>&EndTime=<%=EndTime%>&SelGroup=<%#Eval("BGID") %>'><%# Eval("t_Talk")%></a>--%>
                        &nbsp;
                    </td>
                         <td align="center">
                        <%# Eval("t_TallTime")%>&nbsp;
                    </td>
                    <td align="center">
                    <%# Eval("t_QS")%>
                    <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=StartTime %>&ScoreEndTime=<%=EndTime%>&SelGroup=<%#Eval("BGID") %>'><%# Eval("t_QS")%></a>--%>
                    &nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("p_Qs")%>&nbsp;
                    </td>
                    <td align="center">
                    <%# Eval("t_Qualified")%>
                    <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=StartTime %>&ScoreEndTime=<%=EndTime%>&SelGroup=<%#Eval("BGID") %>&ScoreStatus=1&IsQualified=1'><%# Eval("t_Qualified")%></a>--%>
                    &nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("p_Qualified")%>&nbsp;
                    </td>
                       <td align="center">
                        <%# Eval("t_AVG")%>&nbsp;
                    </td>

                </tr>
            </ItemTemplate>
         </asp:repeater>
         </tbody>
    </table>

        <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="onLineTable">
    <thead>
        <tr class="color_hui" >
            <th width="19%">
                <strong>所属分组</strong>
            </th>
                <th width="9%">
                <strong>客服人数</strong>
            </th>
            <th width="9%">
                <strong>总接待量</strong>
            </th>
                <th width="9%">
                <strong>客服消息发送量</strong>
            </th>
            <th width="9%">
                <strong>访客消息发送量</strong>
            </th>
            <th width="9%">
                <strong>抽检量</strong>
            </th>
            <th width="9%">
                <strong>抽检率</strong>
            </th>
            <th width="9%">
                <strong>合格量</strong>
            </th>
                <th width="9%">
                <strong>合格率</strong>
            </th>
             </th>
                <th width="10%">
                <strong>平均分</strong>
            </th>
        </tr>
        </thead>
        <tbody id="Tbody1">
        <asp:repeater id="repeaterListHotLine" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%# Eval("Name")%>&nbsp;
                    </td>
                       <td align="center">
                        <%# Eval("t_AgentNum")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("t_DailogCount")%>
                        &nbsp;
                    </td>
                         <td align="center">
                        <%# Eval("t_AgentDailog")%>&nbsp;
                    </td>
                    <td align="center">
                    <%# Eval("t_NetFriendDailog")%>
                    &nbsp;
                    </td>
                      <td align="center">
                        <%# Eval("t_Qs")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("p_Qs")%>&nbsp;
                    </td>
                    <td align="center">
                    <%# Eval("t_Qualified")%>
                    &nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("p_Qualified")%>&nbsp;
                    </td>
                       <td align="center">
                        <%# Eval("t_AVG")%>&nbsp;
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

