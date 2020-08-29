<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.AjaxList" %>

<script type="text/javascript">
</script>
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th width="13%">
            话务ID
        </th>
        <th width="12%">
            任务ID
        </th>
        <th width="5%">
            工号
        </th>
        <th width="8%">
            主叫
        </th>
        <th width="8%">
            被叫
        </th>
        <th width="7%">
            话务类型
        </th>
        <th width="7%">
            呼叫类别
        </th>
        <th width="9%">
            接通时间
        </th>
        <th width="9%">
            挂断时间
        </th>
        <th width="4%">
            振铃
        </th>
        <th width="4%">
            通话
        </th>
        <th width="4%">
            话后
        </th>
        <th width="5%">
            总时长
        </th>
        <th width="5%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("CallID")%>&nbsp;
                        </td>
                        <td name='Col_TaskID'>
                           <%#GetViewUrl(Eval("TaskID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;                          
                        </td>                        
                        <td>
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td>
                            <%#GetPhoneNum(Eval("CallStatus").ToString(),Eval("PhoneNum").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetANI(Eval("CallStatus").ToString(), Eval("ANI").ToString())%>&nbsp;                        
                        </td>
                        <td>
                            <%#GetCallStatus(Eval("CallStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetOutBoundType(Eval("CallStatus").ToString(),Eval("OutBoundType").ToString())%>&nbsp;
                        </td> 
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EstablishedTime").ToString())%>&nbsp;
                        </td>
                        <td>
                             <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ReleaseTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("RingingSpanTime")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TallTime")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AfterWorkTime")%>&nbsp;
                        </td>
                        <td>
                            <%#GetTotalTime(BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(Eval("TallTime")), BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(Eval("AfterWorkTime")))%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='/Images/callTel.png' /></a>"%>&nbsp;
                        </td>
                    </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
