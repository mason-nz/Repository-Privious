<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.WorkOrderList" %>

<div class="bit_table">
    <input id="input_page" type="text" value="<%=Page %>" style="display: none" />
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="width: 11%">
                工单ID
            </th>
            <th style="width: 10%">
                客户分类
            </th>
            <th style="width: 10%">
                工单类型
            </th>
            <th style="width: 12%">
                业务类型
            </th>
            <th style="width: 20%">
                标签
            </th>
            <th style="width: 10%">
                工单状态
            </th>
            <th style="width: 8%">
                提交人
            </th>
            <th style="width: 12%">
                提交时间
            </th>
            <th style="width: 7%">
                操作
            </th>
        </tr>
        <asp:repeater id="rptWorkOrderList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>                         
                            <a href='/WOrderV2/WorkOrderView.aspx?OrderID=<%#Eval("OrderID") %>' target="_blank">
                            <%# Eval("OrderID")%></a>&nbsp;
                        </td>
                         <td>                     
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.CustTypeEnum), int.Parse(Eval("CustCategoryID").ToString()))%>&nbsp;
                        </td>
                        <td>  
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum), int.Parse(Eval("CategoryID").ToString()))%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("BusiTypeName").ToString()%>&nbsp;                   
                        </td>
                        <td>
                            <%#Eval("TagName").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WorkOrderStatus), int.Parse(Eval("WorkOrderStatus").ToString()))%>&nbsp; 
                        </td>
                        <td>
                            <%#Eval("TrueName").ToString()%>&nbsp;
                        </td>
                        <td>
                             <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                           <%#GetOperStr( Eval("OrderID").ToString(),int.Parse(Eval("WorkOrderStatus").ToString()), int.Parse(Eval("LastUpdateUserID").ToString()), 
                           int.Parse(Eval("CreateUserID").ToString()))%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
