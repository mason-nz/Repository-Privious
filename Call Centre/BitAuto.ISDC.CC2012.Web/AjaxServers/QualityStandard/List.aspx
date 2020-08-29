<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.List" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th width="6%">
            任务ID
        </th>
        <th width="6%">
            话务ID
        </th>
        <th width="7%">
            坐席
        </th>
        <th width="9%">
            主叫号码
        </th>
        <th width="9%">
            被叫号码
        </th>
        <th width="15%">
            通话时间
        </th>
        <th width="8%">
            通话时长
        </th>
        <th width="13%">
            评分时间
        </th>
        <th width="7%">
            评分人
        </th>
        <th width="7%">
            成绩
        </th>
        <th width="7%">
            状态
        </th>
        <th width="6%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">               
                <td>
                   <%#GetTaskViewUrl(Eval("BusinessID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;
                </td>   
                <td>
                    <%# Eval("CallID")%>&nbsp;
                </td>              
                <td>
                    <%# Eval("SeatName")%>&nbsp;
                </td>  
                <td>
                    <%#Eval("CallStatus").ToString() == "1" ? BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("PhoneNum").ToString()) : Eval("PhoneNum").ToString()%>&nbsp;
                </td>  
                <td>
                    <%#Eval("CallStatus").ToString() == "1" ? Eval("ANI").ToString() : BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("ANI").ToString())%>&nbsp;
                </td>  
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EstablishedTime").ToString())%>&nbsp;
                </td>  
                <td>
                    <%# Eval("TallTime")%>&nbsp;
                </td> 
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ScoreTime").ToString())%>&nbsp;
                </td> 
                <td>
                    <%# Eval("ScoreName")%>&nbsp;
                </td> 
                <td>
                 <a href='<%#GetViewUrl(Eval("QS_RID").ToString(), Eval("ScoreType").ToString())%>' target="_blank">
                 <%# GetScoreToView(Eval("ScoreType").ToString(), Eval("Score").ToString(), Eval("IsQualified").ToString(),Eval("ScoreStatusName").ToString())%></a>   &nbsp;
                </td>
                <td>
                    <%# Eval("ScoreStatusName")%>&nbsp;
                </td> 
                <td > 
                    <%#oper(Eval("ScoreStatus").ToString(), Eval("RTID").ToString(), Eval("QS_RID").ToString(), Eval("CallID").ToString(), Eval("CreateUserID").ToString(), Eval("ScoreType").ToString())%> &nbsp;
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
