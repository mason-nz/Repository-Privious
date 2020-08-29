<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderInfo.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.OrderInfo" %>

<asp:repeater runat="server" id="OrderDataList">
        <ItemTemplate>
            <table border="0" cellspacing="0" cellpadding="0" class="mt10">
                <tr>
                    <th width="38%">
                        订单来源：
                    </th>
                    <td width="60%">
                        <%#Eval("Source")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <th>
                        订单编号：
                    </th>
                    <td>
                        <%#GetLinkString(Eval("Url").ToString(), Eval("OrderID").ToString(), Eval("OrderType").ToString())%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <th>
                        提交时间：
                    </th>
                    <td>
                        <%#Eval("OrderTime")%>&nbsp;
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:repeater>
<br />
<!--分页-->
<div class="pageTurn mr10" style="margin:0px auto; text-align:center;" id="pageTurnmr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<script type="text/javascript">
    $(function () {
        $("#pageTurnmr10 input[type='text']").addClass("inputwidht");
    })
</script>