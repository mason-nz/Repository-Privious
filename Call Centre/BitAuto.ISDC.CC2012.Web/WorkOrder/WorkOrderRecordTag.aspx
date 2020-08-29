<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderRecordTag.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.WorkOrderRecordTag" %>

<!--客户回访-工单记录 强斐 2016-8-17-->
<%@ Register Src="/Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="18%">
            工单ID
        </th>
        <th>
            工单记录
        </th>
        <th width="6%">
            状态
        </th>
        <th width="11%">
            创建人
        </th>
        <th width="15%">
            创建日期
        </th>
    </tr>
    <asp:repeater id="repeater_Record" runat="server">
        <ItemTemplate>
            <tr> 
                <td style="text-align:center"> 
                    <%#GetURL(Eval("OrderID").ToString())%>&nbsp;
                </td> 
                <td style="text-align:left">
                    <a title='<%#Eval("Content").ToString()%>' style=' text-decoration:none; color:#333' href="javascript:void(0)">
                        <%#GetContent(Eval("Content").ToString())%>
                    </a>&nbsp;
                </td> 
                <td>
                    <%#StatusName(Eval("WorkOrderStatus").ToString())%>&nbsp;
                </td>
                <td>
                    <%#Eval("TrueName").ToString()%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td style="text-align:center"> 
                    <%#GetURL(Eval("OrderID").ToString())%>&nbsp;
                </td> 
                <td style="text-align:left">
                    <a title='<%#Eval("Content").ToString()%>' style=' text-decoration:none; color:#333' href="javascript:void(0)">
                        <%#GetContent(Eval("Content").ToString())%>
                    </a>&nbsp;
                </td> 
                <td>
                    <%#StatusName(Eval("WorkOrderStatus").ToString())%>&nbsp;
                </td>
                <td>
                    <%#Eval("TrueName").ToString()%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager_Record" runat="server" PageSize="5" />
</div>
