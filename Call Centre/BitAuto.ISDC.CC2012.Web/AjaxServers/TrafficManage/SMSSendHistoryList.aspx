<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendHistoryList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.SMSSendHistoryList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 12%;">
            接收人
        </th>
        <th style="width: 8%;">
            手机
        </th>
        <th style="width: 10%;">
            任务ID
        </th>
        <th style="width: 10%;">
            客户ID
        </th>
        <th style="width: *">
            发送内容
        </th>
        <th style="width: 13%;">
            发送日期
        </th>
        <th style="width: 8%;">
            是否成功
        </th>
        <th style="width: 5%;">
            客服
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">           
                <td >
                    <a href="../../TaskManager/CustInformation.aspx?CustID=<%# Eval("CustID")%>" target="_blank"><%# Eval("custname")%></a>&nbsp;
                </td>         
                 <td >
                    <%# Eval("Phone")%>&nbsp;
                </td>  
                  <td >
                    <a href="../../OtherTask/OtherTaskDealView.aspx?OtherTaskID=<%# Eval("TaskID")%>" target="_blank"><%# Eval("TaskID")%></a>&nbsp;
                </td>  
                <td >
                <%# getCrmUrl(Eval("CRMCustID"),Eval("CustID"))%>                
                </td>  
                <td title="<%#Eval("Content")%>">
                    <%# Eval("Content").ToString().Length > 30 ? Eval("Content").ToString().Substring(0, 30) + "..." : Eval("Content").ToString()%>&nbsp;
                </td>   
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td>              
                <td >
                    <%# Eval("Status").ToString()=="0"?"成功":"失败"%>&nbsp;
                </td> 
                 <td>
                    <%#Eval("TrueName")%>&nbsp;
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
