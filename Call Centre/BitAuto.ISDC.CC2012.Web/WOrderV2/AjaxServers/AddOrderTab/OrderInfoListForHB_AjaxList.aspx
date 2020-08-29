<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderInfoListForHB_AjaxList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.AddOrderTab.OrderInfoListForHB_AjaxList" %>

<table cellpadding="0" cellspacing="0" id="tableList3" width="99%">
    <tr class="bold">
        <th style="width: 8%;">
            姓名
        </th>
        <th style="width: 12%;">
            下单号码
        </th>
        <th style="width: 10%;">
            下单品牌
        </th>
        <th style="width: 10%;">
            下单车型
        </th>
        <th style="width: 18%;">
            下单时间
        </th>
        <th style="width: 8%;">
            来源
        </th>
        <th style="width: 10%;">
            订单ID
        </th>
        <th style="">
            备注
        </th>
        <th style="width: 8%;">
            操作
        </th>
    </tr>
    <asp:repeater runat="server" id="OrderDataList">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                   <%#Eval("UserName")%>&nbsp;
                </td>
                <td>
                   <%#Eval("UserPhone")%>&nbsp;
                </td>
                <td>
                   <%#Eval("Brand")%>&nbsp;
                </td>
                <td>
                    <%#Eval("Serial")%>&nbsp;
                </td>
                <td>
                   <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("OrderTime").ToString())%>&nbsp;
                </td>
                <td>
                   <%#Eval("Source")%>&nbsp;
                </td>
                <td>
                <%#GetLinkString(Eval("Url").ToString(), Eval("OrderID").ToString(), Eval("OrderType").ToString())%>&nbsp;
                </td>
                <td  style=" text-align:left;word-break: break-all;">
                   <%#Eval("Remark")%>&nbsp;
                </td> 
                <td>
                 <a href="javascript:void(0)" data=" " onclick="javascript:WOrderInfo.AppendContent('【姓名】<%#Eval("UserName")%>\n【下单号吗】<%#Eval("UserPhone")%>\n【下单品牌】<%#Eval("Brand")%>\n【下单车型】<%#Eval("Serial")%>\n【下单时间】<%#Eval("OrderTime").ToString()%>\n【来源】<%#Eval("Source")%>\n【订单ID】<%#Eval("Remark")%>\n【备注】<%#Eval("OrderID")%>');" >
                 <img  src="/Images/workorder/copy.png" border="0" title="复制"></a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<%--<br />
<!--分页-->
<div class="pageTurn mr10" style="margin-right: 20px;">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>--%>
