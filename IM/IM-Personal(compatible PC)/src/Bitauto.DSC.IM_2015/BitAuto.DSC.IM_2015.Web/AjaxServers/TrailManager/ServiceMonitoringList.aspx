<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceMonitoringList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.ServiceMonitoringList" %>

<table border="0" cellspacing="0" cellpadding="0" width="100%">
    <tr>
        <th width="15%">
            所属分组
        </th>
        <th width="15%">
            客服
        </th>
        <th width="8%">
            工号
        </th>
        <th width="8%">
            当前状态
        </th>
        <th width="9%">
            最大对话量
        </th>
        <th width="9%">
            当前对话量
        </th>
        <th width="9%">
            当前接待量
        </th>
        <th width="9%">
            饱和度
        </th>
        <th width="9%">
            尚未响应量
        </th>
        <th width="9%">
            尚未响应率
        </th>
    </tr>
    <asp:repeater runat="server" id="rpeList">
                <ItemTemplate>
                    <tr>
                         <td>
                           <%#Eval("InBGIDName")%>&nbsp;
                        </td>
                        <td>
                           <%# BitAuto.DSC.IM_2015.BLL.Util.SubTrueName(Eval("AgentName").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td> 
                          <%#Eval("StatusName")%>&nbsp;
                        </td>
                        <td>  
                        <%#Eval("MaxDialogNum")%>&nbsp;
                        </td>
                        <td> 
                          <%#Eval("CurrentDialogNum")%>&nbsp;
                        </td>
                        <td>
                          <%#Eval("CurrentReceptionDNum")%>&nbsp;
                        </td>
                            <td>
                          <%#GetAvg(Eval("SaturationRate").ToString())%>&nbsp;
                        </td>
                            <td>
                          <%#Eval("NoReceptionDNum")%>&nbsp;
                        </td>
                           <td  class="tdrate"  id='<%#Eval("NoReceptionRate") %>'>
                          <%#GetAvg(Eval("NoReceptionRate").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
</table>
<script language="javascript" type="text/javascript">
    $("td.tdrate").each(function () {
        var rateNum = parseFloat($(this).attr("id"));
        if (rateNum >= 0.75) {
            $(this).parent().addClass("redColor");
        }
    });
</script>
