<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialogueList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.DialogueList" %>

<table border="0" cellspacing="0" cellpadding="0" width="100%" id="tab">
    <tr>
        <th width="20%">
            日期
        </th>
        <th width="20%">
            访客来源
        </th>
        <th width="15%">
            总对话量
        </th>
        <th width="15%">
            有效对话量
        </th>
        <th width="15%">
            无效对话量
        </th>
        <th width="15%">
            无效对话量占比
        </th>
    </tr>
    <asp:repeater runat="server" id="rpeList">
                <ItemTemplate>
                    <tr>
                         <td>
                             <%#Eval("DatePeriod")%>&nbsp; 
                        </td>
                           <td>
                             <%#Eval("SourceTypeName")%>&nbsp; 
                        </td>
                           <td>
                             <%#Eval("SumConversation")%>&nbsp; 
                        </td>
                         <td>
                             <%#Eval("SumEffective")%>&nbsp; 
                        </td>
                           <td>
                               <%#Eval("SumNoEffective")%>&nbsp;
                        </td>
                           <td>
                           <%#Eval("NoPercent")%> &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    <tr>
        <td colspan="6">
            <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $("#tab tr").last().prev().addClass("sum");
    });

</script>
