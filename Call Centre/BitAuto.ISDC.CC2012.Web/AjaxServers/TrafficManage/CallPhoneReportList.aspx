<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallPhoneReportList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.CallPhoneReportList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">        
        <th name='Col_StatisticsTime' style="width: 10%;">
            日期
        </th>
        <th name='Col_AgentName' style="width: 10%;">
            客服
        </th>
        <th name='Col_AgentNum' style="width: 5%;">
            工号
        </th>
        <th name='Col_OutCallCount' style="width:8%;">
            外呼电<br />话总量
        </th>
        <th name='Col_TalksCount' style="width:7%;">
            外呼接<br />通量
        </th>
        <th name='Col_JTRate' style="width:8%;">
            外呼接<br />通率
        </th>
        <th name='Col_TalkTime' style="width: 8%;">
            总通话<br />时长
        </th>
        <th name='Col_RingTime' style="width: 8%;">
            总振铃<br />时长
        </th>  
        <th name='Col_LoginOnTime' style="width: 8%;">
            总在线<br />时长
        </th>  
        <th name='Col_WorkEfficiency' style="width: 8%;">
            工时利<br />用率
        </th>    
        <th name='Col_AGTalkTime' style="width: 8%;">
            平均通话<br />时长(秒)
        </th>
        <th name='Col_AGRingTime' style="width: 8%;">
            平均振铃<br />(秒)
        </th>                
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">       
                <td name='Col_StatisticsTime'>
                    <%# Eval("StartTime")%>&nbsp;
                </td>         
                 <td name='Col_AgentName'>
                    <%# Eval("TrueName")%>&nbsp;
                </td>  
                  <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>  
                <td name='Col_OutCallCount'>
                    <%# Eval("N_Total")%>&nbsp;
                </td>  
                <td name='Col_TalksCount'>
                    <%# Eval("N_ETotal")%>&nbsp;
                </td>   
                <td name='Col_JTRatet'>
                    <%# Eval("p_jietonglv")%>&nbsp;
                </td>              
                <td name='Col_TalkTime'>
                    <%# Eval("T_Talk")%>&nbsp;
                </td> 
                 <td name='Col_RingTime'>
                    <%#Eval("T_Ringing")%>&nbsp;
                </td> 
                <td name='Col_LoginOnTime'>
                    <%# Eval("T_SignIn")%>&nbsp;
                </td>
                <td name='Col_WorkEfficiency'>
                    <%# Eval("p_gongshilv")%>&nbsp;
                </td>
                <td name='Col_AGTalkTime'>
                    <%#Eval("t_pjtime")%>&nbsp;
                </td> 
                <td name='Col_AGRingTime'>
                    <%# Eval("t_pjring")%>&nbsp;
                </td>  
            </tr>
        </ItemTemplate>
    </asp:repeater>
    <tr style="background-color: #f5f5f5; height: 45px;">     
        <td style="font-weight: bold;" style="width: 10%;">
            合计(共<%=RecordCount %>项)
        </td>
        <td style="font-weight: bold;" style="width: 10%;">
            --
        </td>
        <td style="font-weight: bold;" style="width: 5%;">
            --
        </td>
        <td id='OutCallCount' style="font-weight: bold;" style="width:8%;">
           0 
        </td>
        <td id="TalksCount" style="font-weight: bold;" style="width:7%;">
           0 
        </td>
        <td id="JTRate" style="font-weight: bold;" style="width:8%;">
           0.00% 
        </td>
        <td id="TalkTime" style="font-weight: bold;" style="width: 8%;">
           0 
        </td>
        <td id="RingTime" style="font-weight: bold;" style="width: 8%;">
           0 
        </td>  
        <td id="LoginOnTime" style="font-weight: bold;" style="width: 8%;">
            0
        </td>  
        <td id="WorkEfficiency" style="font-weight: bold;" style="width: 8%;">
           0.00% 
        </td>    
        <td id="AGTalkTime" style="font-weight: bold;" style="width: 8%;">
            0.00
        </td>
        <td id="AGRingTime" style="font-weight: bold;" style="width: 8%;">
           0.00 
        </td>                
    </tr>    
</table> 
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
