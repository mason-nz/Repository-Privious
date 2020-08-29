<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallRecord.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.CallRecord" %>

<table style="width: 94%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th name='Col_Contact' style="width: 10%">
            联系人
        </th>
        <th name='Col_ANI' style="width: 12%;">
            主叫号码
        </th>
        <th name='Col_PhoneNum' style="width: 12%;">
            被叫号码
        </th>
        <th name='Col_BeginTime' style="width: 20%;">
            通话时间
        </th>
        <th name='Col_Status' style="width: 7%;">
            呼叫类型
        </th>
        <th name='Col_AgentName' style="width: 8%;">
            坐席
        </th>
        <th name='Col_AgentNum' style="width: 8%;">
            工号
        </th>
        <th style="width: 8%;">
            操作
        </th>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
             <tr>   
                <td name='Col_Contact'>
                    <%# Eval("CustName")%>&nbsp;
                </td>               
                <td name='Col_ANI'>
                    <%# Eval("ANI")%>&nbsp;
                </td> 
                <td name='Col_PhoneNum'>
                    <%# Eval("PhoneNum")%>&nbsp;
                </td> 
                <td name='Col_BeginTime'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td> 
                <td name='Col_Status'>
                    <%#BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(Eval("CallStatus").ToString())%>&nbsp;
                </td>                 
                 <td name='Col_AgentName'>
                    <%# Eval("AgentName")%>&nbsp;
                </td>  
                  <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>                                                                  
                <td >                
                 <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;
                </td> 
            </tr>      
        </ItemTemplate>
    </asp:repeater>
</table>
