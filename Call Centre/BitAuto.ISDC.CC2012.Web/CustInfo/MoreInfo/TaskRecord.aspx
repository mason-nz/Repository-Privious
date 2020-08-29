<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskRecord.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.TaskRecord" %>

<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>

<form id="form1" runat="server"> 
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="10%">
            任务ID
        </th>
        <th width="10%">
            所属项目
        </th>
        <th width="10%">
            操作人
        </th>
        <th width="12%">
            状态
        </th>
        <th width="10%">
            操作时间
        </th>
    </tr>
    <asp:repeater id="repeater_TaskRecord" runat="server">
        <ItemTemplate>
            <tr>
            <td><%#getview(Eval("PTID").ToString(),Eval("CarType").ToString(),Eval("Source").ToString(),Eval("TaskStatus").ToString()) %></td> 
                <td><%#Eval("Name").ToString()%></td>
                <td><%#getUserName(Eval("LastOptUserID").ToString())%></td>
                <td><%#statusName(Eval("TaskStatus").ToString(), Eval("Source").ToString())%></td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOptTime").ToString())%>&nbsp;
                </td>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
            <td><%#getview(Eval("PTID").ToString(),Eval("CarType").ToString(),Eval("Source").ToString(),Eval("TaskStatus").ToString()) %></td> 
                <td><%#Eval("Name").ToString()%></td>
                <td><%#getUserName(Eval("LastOptUserID").ToString())%></td>
                <td><%#statusName(Eval("TaskStatus").ToString(), Eval("Source").ToString())%></td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOptTime").ToString())%>&nbsp;
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float:right">
    <uc1:AjaxPager ID="AjaxPager_TaskRecord" runat="server" PageSize="5" />
</div>
</form>
