<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IMList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.IMList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th width="8%">
            对话ID
        </th>
        <th width="12%">
            工单ID
        </th>
        <th width="8%">
            坐席
        </th>
        <th width="16%">
            对话时间
        </th>
        <%-- <th width="8%">
            消息次数
        </th>--%>
        <th width="16%">
            评分时间
        </th>
        <th width="8%">
            评分人
        </th>
        <th width="8%">
            成绩
        </th>
        <th width="8%">
            状态
        </th>
        <th width="8%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                    <%# GetLink(Eval("CSID").ToString())%>&nbsp;
                </td>
                 <td>
                    <a <%# GetOrderUrl(Eval("OrderID").ToString())%>><%# Eval("OrderID")%></a>&nbsp;
                </td>
                <td>
                    <%# Eval("AgentUserName")%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td>                 
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("Result_CreateTime").ToString())%>&nbsp;
                </td> 
                <td>
                    <%# Eval("Result_CreateUserName")%>&nbsp;
                </td> 
                <td>
                    <%# ViewScore(Eval("QS_RID").ToString(), Eval("ScoreType").ToString(), Eval("Score").ToString(), Eval("IsQualified").ToString(),Eval("Result_Status").ToString())%>               
                </td> 
                <td>
                    <%# GetStatus(Eval("Result_Status").ToString())%>&nbsp;
                </td> 
                <td >                
                    <%#oper(Eval("Result_Status").ToString(), Eval("QS_RTID").ToString(), Eval("QS_RID").ToString(), Eval("CSID").ToString(), Eval("AgentUserID").ToString(), Eval("ScoreType").ToString())%> &nbsp;
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
