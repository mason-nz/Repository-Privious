<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StopCustApplyList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.StopCustApplyList" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th style="width: 15%;">
            申请类型
        </th>
        <th style="width: 15%;">
            原因
        </th>
        <th style="width: 15%;">
            说明
        </th>
        <th style="width: 15%;">
            申请时间
        </th>
        <th style="width: 15%;">
            状态
        </th>
        <th style="width: 15%;">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr>   
                <td >
                    <%# Eval("ApplyType").ToString()=="1"?"客户停用":"客户启用"%>&nbsp;
                </td>         
                 <td >
                    <%# BindApplyReason(Eval("ApplyReason").ToString())%>&nbsp;
                </td>  
                  <td >
                    <%# BindStopRemark(Eval("ApplyRemark").ToString())%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ApplyTime").ToString())%>&nbsp;
                </td>   
                <td>
                    <%# BindStopStatus(Eval("StopStatus").ToString(), Eval("ApplyType").ToString())%>&nbsp;
                </td>              
                <td >
                    <a href="javascript:void(0)" onclick="OperationLogPop('+ <%#Eval("TaskID")%> +')">审批记录</a>&nbsp;
                </td>                 
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
             <td >
                    <%# Eval("ApplyType").ToString()=="1"?"客户停用":"客户启用"%>&nbsp;
                </td>         
                 <td >
                    <%# BindApplyReason(Eval("ApplyReason").ToString())%>&nbsp;
                </td>  
                  <td >
                    <%# BindStopRemark(Eval("ApplyRemark").ToString())%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ApplyTime").ToString())%>&nbsp;
                </td>   
                <td>
                    <%# BindStopStatus(Eval("StopStatus").ToString(), Eval("ApplyType").ToString())%>&nbsp;
                </td>              
                <td >
                    <a href="javascript:void(0)" onclick="OperationLogPop('+ <%#Eval("TaskID")%> +')">审批记录</a>&nbsp;
                </td>                
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager_ApplyRecord" runat="server" PageSize="20" />
</div>
