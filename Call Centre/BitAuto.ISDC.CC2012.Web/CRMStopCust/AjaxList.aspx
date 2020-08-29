<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.AjaxList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th width="2%">
            <input type="checkbox" id="chkAll" onclick="checkAll()" />
        </th>
        <th width="6%">
            客户ID
        </th>
        <th width="20%">
            客户名称
        </th>
        <th width="7%">
            申请人
        </th>
        <th width="7%">
            坐席
        </th>
        <th width="8%">
            大区
        </th>
        <th width="15%">
            申请时间
        </th>
        <th width="15%">
            审核时间
        </th>
        <th width="7%">
            任务状态
        </th>
        <th width="7%">
            客户状态
        </th>
        <th width="6%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                    <input type="checkbox" name="chkStop" value="<%#Eval("TaskID") %>" />
                </td>
                <td>
                    <%# Eval("CustID")%>&nbsp;
                </td> 
                <td>
                   <%# Eval("CustName")%>&nbsp;
                </td>
                <td>
                    <%# Eval("ApplyerName")%>&nbsp;
                </td>
                <td>
                    <%# Eval("AssignName")%>&nbsp;
                </td>
                <td>
                    <%# Eval("AreaName")%>&nbsp;
                </td>  
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ApplyTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td>  
                <td>
                    <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("AuditTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td>                  
                <td>
                    <%# GetTaskStatusName(Eval("TaskStatus").ToString())%>&nbsp;
                </td> 
                <td>
                    <%# GetStopStatusName(Eval("StopStatus").ToString(),Eval("ApplyType").ToString())%>&nbsp;
                </td> 
                <td>
                    <%#GetOperLink(Eval("StopStatus").ToString(), Eval("TaskID").ToString(), Eval("TaskStatus").ToString(), Eval("AssignUserID").ToString())%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
<script type="text/javascript">
    function checkAll() {
        if ($("#chkAll").is(":checked")) {
            $(":checkbox[name='chkStop']").attr("checked", true);
        } else {
            $(":checkbox[name='chkStop']").attr("checked", false);
        }
    }
</script>
