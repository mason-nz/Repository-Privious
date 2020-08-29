<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueueList.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.QueueManage.QueueList"
    EnableViewState="false" %>

<form id="formQueueList" runat="server">
<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="24%">
            经销商名称
        </th>
        <th width="13%">
            地理位置
        </th>
        <th width="8%">
            所属区域
        </th>
        <th width="8%">
            连接时间
        </th>
        <th width="10%">
            上次最后消息时间
        </th>
        <th width="10%">
            上次访问时间
        </th>
        <th width="7%">
            访问次数
        </th>
        <th width="20%">
            最近访问页面
        </th>
    </tr>
    <asp:repeater runat="server" id="rpt">
     <ItemTemplate>
          <tr>
             
                     <td class="cName">
                            <%#Eval("MemberName")%>&nbsp;
                        </td> 
                        <td>
                            <%#Eval("Address")%>&nbsp;
                        </td> 
                        <td>
                            <%#Eval("CityGroupName")%>
                        </td>
                        <td>
                            <%#GetConnetTime(Convert.ToDateTime(Eval("ConverSTime")))%>
                        </td>
                        <td>
                            <%#Eval("LastMessageTime").ToString() == "1900-1-1 0:00:00" ? "" : Eval("LastMessageTime").ToString()%>
                        </td>
                        <td>
                            <%#Eval("LastConBeginTime").ToString() == "1900-1-1 0:00:00" ? "" : Eval("LastConBeginTime").ToString()%>
                        </td> 
                         <td> <%#Eval("Distribution")%></td>
                     <td class="cName"><%#Eval("UserReferTitle")%></td>
                    </tr>
                </ItemTemplate>
        </asp:repeater>
</table>
</form>
