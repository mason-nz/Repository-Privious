<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SatisfactionList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.SatisfactionList" %>

<table border="0" cellspacing="0" cellpadding="0" width="100%" id="tab">
    <tr>
        <th rowspan="2">
            日期
        </th>
        <th rowspan="2">
            客服
        </th>
        <th rowspan="2">
            工号
        </th>
        <th rowspan="2">
            总对话量
        </th>
        <th rowspan="2">
            总参评量
        </th>
        <th rowspan="2">
            参评率
        </th>
        <th colspan="6">
            产品评价
        </th>
        <th colspan="6">
            服务评价
        </th>
    </tr>
    <tr>
        <th>
            非常满意
        </th>
        <th>
            满意
        </th>
        <th>
            一般
        </th>
        <th>
            不满意
        </th>
        <th>
            非常不满意
        </th>
        <th>
            满意率
        </th>
        <th>
            非常满意
        </th>
        <th>
            满意
        </th>
        <th>
            一般
        </th>
        <th>
            不满意
        </th>
        <th>
            非常不满意
        </th>
        <th>
            满意率
        </th>
    </tr>
    <asp:repeater runat="server" id="rpeList">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("DatePeriod").ToString() == "" ? "合计（共" + RecordCount.ToString() + "项）" : Eval("DatePeriod")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.DSC.IM_2015.BLL.Util.SubTrueName(Eval("truename").ToString() == "" ? "--" : Eval("truename").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("agentnum").ToString() == "" ? "--" : Eval("agentnum")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("sumduihua")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("chanping")%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.DSC.IM_2015.BLL.Util.GetLv(Eval("chanping").ToString(),Eval("sumduihua").ToString())%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("profcmy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("promy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("proyb")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("probmy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("profcbmy")%>&nbsp;
                        </td>
                         <td>
                            <%#GetAvg(Eval("profcmy").ToString(), Eval("promy").ToString(), Eval("chanping").ToString())%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("perfcmy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("permy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("peryb")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("perbmy")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("perfcbmy")%>&nbsp;
                        </td>
                         <td>
                            <%#GetAvg(Eval("perfcmy").ToString(), Eval("permy").ToString(), Eval("chanping").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    <tr>
        <td colspan="17">
            <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $("#tab tr").last().prev().addClass("sum");
    });

</script>
