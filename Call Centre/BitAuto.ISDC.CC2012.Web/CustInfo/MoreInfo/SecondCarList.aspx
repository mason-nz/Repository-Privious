<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondCarList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.SecondCarList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<table width="100%" id="tabScale" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="20%">
            时间
        </th>
        <th width="20%">
            月库存数量
        </th>
        <th width="20%">
            月置换数量
        </th>  
        <th width="20%">
            月交易数量
        </th>  
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr>
                <td >
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                </td>
                <td ><%#GetBusinessScale(Eval("MonthStock").ToString())%></td>
                <td><%#GetBusinessScale(Eval("MonthSales").ToString())%></td>  
                <td><%#GetBusinessScale(Eval("MonthTrade").ToString())%></td>
                                  
            </tr>               
        </ItemTemplate>
    </asp:repeater>
</table> 
<div class="pages" style="float:right">
    <uc1:AjaxPager ID="AjaxPager1" runat="server" PageSize="5" />
</div>
<script type="text/javascript">
    $("#tabScale tr:even").addClass("color_hui");
</script>