<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentTimeStateList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.AgentTimeStateList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList2">
    <tr class="back" onmouseout="this.className='back'">        
        <th style="width: 15%;">
                        所属分组
                    </th>
                    <th style="width: 15%;">
                        在线人数
                    </th>
                    <th style="width: 15%;">
                        置闲
                    </th>
                    <th style="width: 15%;">
                        置忙
                    </th>
                    <th style="width: 15%;">
                        振铃
                    </th>
                    <th style="width: 15%;">
                        工作中
                    </th>
                    <th style="width: 10%;">
                        话后
                    </th>                
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">       
                <td>
                    <%# Eval("Name")%>&nbsp;
                </td>         
                 <td>
                    <%# Eval("totalCount")%>&nbsp;
                </td>  
                  <td>
                    <%# Eval("ReadyCount")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("NotReadyCount")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("RingCount")%>&nbsp;
                </td>   
                <td>
                    <%# Eval("WorkingCount")%>&nbsp;
                </td>              
                <td>
                    <%# Eval("AfterWorkCount")%>&nbsp;
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
