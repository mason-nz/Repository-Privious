<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlexaList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.AlexaList" %>

<table border="0" cellspacing="0" cellpadding="0" width="100%" id="tab">
    <thead>
        <th colspan="7" style="background: #F9F9F9">
            <div class="btn btn2 right">
                <input type="button" value="导出" class="save w60 gray"   onclick="ExportData()"/></div>
        </th>
    </thead>
    <tr>
        <th width="20%">
            日期
        </th>
        <th width="20%">
            访客来源
        </th>
        <th width="12%">
            页面访问量
        </th>
        <th width="12%">
            总对话量
        </th>
        <th width="12%">
            队列放弃量
        </th>
        <th width="12%">
            登录访客总量
        </th>
        <th width="12%">
            匿名访客总量
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
                             <%#Eval("SumVisit")%>&nbsp; 
                        </td>
                         <td>
                             <%#Eval("SumConversation")%>&nbsp; 
                        </td>
                           <td>
                               <%#Eval("SumQueueFail")%>&nbsp;
                        </td>
                           <td>
                           <%#Eval("LoginVisit")%> &nbsp;
                        </td>
                           <td>
                           <%#Eval("NoLoginVisit")%> &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    <tr>
        <td colspan="7">
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
