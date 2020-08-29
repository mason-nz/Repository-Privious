<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExclusiveMissedCallsList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustomerCallin.ExclusiveMissedCallsList" %>

<!--专属客服未接来电查询-->
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th name='Col_主叫号码' style="width: 15%;">
            主叫号码
        </th>
        <th name='Col_开始时间' style="width: 20%;">
            开始时间
        </th>
        <th name='Col_挂断时间' style="width: 20%;">
            挂断时间
        </th>
        <th name='Col_所属分组' style="width: 15%;">
            所属分组
        </th>
        <th name='Col_工号' style="width: 15%">
            工号
        </th>
        <th name='Col_客服' style="width: 15%;">
            客服
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">             
                <td name='Col_主叫号码'>
                    <span id="tr_<%#Eval("RecID").ToString() %>">
                        <%# BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("CallNO").ToString())%>
                    </span>&nbsp;
                </td> 
                <td name='Col_开始时间'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("StartTime").ToString(),"yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td> 
                <td name='Col_挂断时间'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td> 
                <td name='Col_所属分组'>
                    <%#Eval("BusinessGroupName")%>&nbsp;
                </td> 
                <td name='Col_工号'>
                    <%#Eval("ExclusiveAgentNum")%>&nbsp;
                </td> 
                <td name='Col_客服'>                   
                        <%# Eval("ExclusiveUserName")%>&nbsp;
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
<input id="input_page" type="hidden" value="<%=Page %>" />
