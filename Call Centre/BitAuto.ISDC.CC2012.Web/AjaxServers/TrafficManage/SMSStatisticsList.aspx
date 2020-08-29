<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSStatisticsList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.SMSStatisticsList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 15%">
            &nbsp;
        </th>
        <th style="width: 17%;">
            客服
        </th>
        <th style="width: 17%;">
            工号
        </th>
        <th style="width: 17%;">
            短信发送量
        </th>
        <th style="width: 17%;">
            成功量
        </th>
        <th style="width: 17%;">
            失败量
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>&nbsp;</td>       
                <td>
                    <%# Eval("truename")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("agentnum")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("znum")%>&nbsp;
                </td>              
                <td >
                    <%# Eval("successnum")%>&nbsp;
                </td> 
                <td>
                    <%#Eval("failnum")%>&nbsp;
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
