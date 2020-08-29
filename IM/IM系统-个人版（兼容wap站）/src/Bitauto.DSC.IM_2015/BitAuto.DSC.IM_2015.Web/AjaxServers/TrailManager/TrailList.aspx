<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrailList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.TrailList" %>

<table border="0" cellspacing="0" cellpadding="0" width="100%" id="tab">
    <tr>
        <th>
            日期
        </th>
        <th style="width: 8%">
            客服
        </th>
        <th style="width: 8%">
            工号
        </th>
        <th style="width: 8%">
            在线时长
        </th>
        <th style="width: 8%">
            总对话时长
        </th>
        <th style="width: 8%">
            平均对话时长
        </th>
        <th style="width: 8%">
            首次平均响<br />
            应时长（秒）
        </th>
        <th style="width: 7%">
            总对话量
        </th>
        <th style="width: 7%">
            总接待量
        </th>
        <th style="width: 8%">
            响应率
        </th>
        <th style="width: 8%">
            客服消息<br />
            发送量
        </th>
        <th style="width: 8%">
            访客消息<br />
            发送量
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
                            <%#ConvertDate(Eval("sumonlinetime").ToString())%>&nbsp;
                        </td>
                         <td>
                            <%#ConvertDate(Eval("SumConversationTimeLong").ToString())%>&nbsp;
                        </td>
                         <td>
                           <%#GetAvgConver(Eval("SumConversations").ToString(), Eval("SumConversationTimeLong").ToString())%>&nbsp;
                        </td>
                         <td>
                           <%#GetAvgNum(Eval("SumReception").ToString(), Eval("SumFRTimeLong").ToString())%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("SumConversations")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("SumReception")%>&nbsp;
                        </td>
                         <td>
                           <%#GetAvg(Eval("SumConversations").ToString(), Eval("SumReception").ToString())%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("SumAgentDailog")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("SumNetFriendDailog")%>&nbsp;
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
