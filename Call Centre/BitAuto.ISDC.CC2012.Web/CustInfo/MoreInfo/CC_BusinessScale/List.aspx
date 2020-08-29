<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_BusinessScale.List" %>

<%@ Register Src="~/Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<script type="text/javascript">
 function refresh(setFirstPage){
            <%=this.AjaxPager1.RefreshFunctionName %>("", setFirstPage);
        }
      function  OpenAddBussinessScale(action,recId) {
            $.openPopupLayer({
                name: "DisposeBussinessScale",
                width: 550,
                url: "/CustInfo/MoreInfo/CC_BusinessScale/Dispose.aspx",
                parameters: {
                    Action: action,
                    TID: '<%= this.TID %>',
                    RecID: recId
                }
            });
        }
    function delBussinessScale(recid) {
        if (confirm("确定删除此二手车规模吗？")) {
            AjaxPost("/CustInfo/MoreInfo/CC_BusinessScale/Handler.ashx", { Action: 'del', RecID: recid }, null, function (data) {
                if (data == "success") {
                    $.jPopMsgLayer("删除成功！", function () {
                    refresh();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
    }
</script>
<h2>
    <span>二手车规模</span><a href="#this" onclick="javascript:OpenAddBussinessScale('add','')">[新增]</a>
</h2>
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
        <th width="16%">
            操作
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr>
                <td >
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                </td>
                <td ><%#GetBusinessScale(Eval("MonthStock").ToString(),typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthStock))%></td>
                <td><%#GetBusinessScale(Eval("MonthSales").ToString(), typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthSales))%></td>
                <td><%#GetBusinessScale(Eval("MonthTrade").ToString(), typeof(BitAuto.YanFa.Crm2009.Entities.EnumBusinessScaleMonthTrade))%></td>
                <td><a href='javascript:void(0)' onclick='javascript:OpenAddBussinessScale("edit","<%#Eval("RecID") %>")'>编辑</a><a href='javascript:delBussinessScale(<%#Eval("RecID") %>)'>删除</a></td>  
            </tr>               
        </ItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager1" runat="server" PageSize="5" />
</div>
<script type="text/javascript">
    $("#tabScale tr:even").addClass("color_hui");
</script>
