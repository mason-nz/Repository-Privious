<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers.WorkOrderList" %>

<div class="optionBtn clearfix">
    <%if (canExport)
      { %>
    <input name="" type="button" value="导出" id="lbtnExport" onclick="ExportData()" class="newBtn mr10" />
    <%} %>
    <span>查询结果</span><small><span>总计:
        <%= totalCount%></span></small>&nbsp;&nbsp;&nbsp;&nbsp;<span><a onclick="BindOrderCreateTime()"
            id="aCreateTime" style="cursor: pointer" href="javascript:void(0)">排序：按创建时间</a></span>
</div>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="padding-left: 20px;">
                工单ID
            </th>
            <th>
                客户名称
            </th>
            <th>
                标签
            </th>
            <th>
                处理人
            </th>
            <th>
                状态
            </th>
            <th>
                最晚处理时间
            </th>
            <th>
                操作人
            </th>
            <th>
                提交日期
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="rptWorkOrderList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                           <div style="float:left;clear:none; width:165px;"> <span style="float:left;clear:none; width:20px;"><%#Eval("PriorityLevel").ToString() == "2" ? "<img src='/images/jinji.png' style='position:relative; top:6px;'>" : ""%></span>  
                                  <a href='/WorkOrder/WorkOrderView.aspx?OrderID=<%#Eval("OrderID") %>' style="float:right;clear:none; width:140px; margin-left:5px;" target="_blank"><%# Eval("OrderID")%></a>
                                  </div>
                        </td>
                         <td>
                        <%#Eval("CustName")%>&nbsp;
                        </td>
                        <td>
                       <%--<%#GetTitleStyle(Eval("Title").ToString(), Eval("LastProcessDate").ToString(), Eval("WorkOrderStatus").ToString())%>&nbsp;--%>
                       <%#GetTitleStyle(Eval("TagName").ToString(), Eval("LastProcessDate").ToString(), Eval("WorkOrderStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                        <%#Eval("ReceiverName")%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WorkOrderStatus),int.Parse(Eval("WorkOrderStatus").ToString())) %>&nbsp;
                        </td>
                        <td>
                           <%#Eval("LastProcessDate") %>&nbsp;
                        </td>
                        <td>
                            <%#GetUserName(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                              <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td>
                           <%#GetOperStr(int.Parse(Eval("WorkOrderStatus").ToString()), int.Parse(Eval("ReceiverID").ToString()), int.Parse(Eval("CreateUserID").ToString()), Eval("OrderID").ToString())%>&nbsp;
                           <a href='javascript:void(0);' OrderID="<%#Eval("OrderID")%>" onclick='PlayRecordList(this);' title='播放录音' ><img src='../../Images/callTel.png' /></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
    <div class="pageTurn mr10">
        <p>
            <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
        </p>
    </div>
</div>
