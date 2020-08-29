<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChatMessageLogList.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.ChatMessageLog.ChatMessageLogList" %>

<table cellpadding="0" cellspacing="0" width="99%" id="tableList">
    <tr onmouseout="this.className='back'">
        <th>
            消息ID
        </th>
        <th>
            分配会话ID
        </th>
        <th>
            发送人ID
        </th>
        <th>
            接收人ID
        </th>
        <th>
            聊天内容
        </th>
        <th>
            消息类型
        </th>
        <th>
            聊天日期
        </th>        
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr onmouseover="this.className='hover'" onmouseout="this.className='back'">                        
                        <td>
                            <%#Eval("MessageID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AllocID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Sender")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Receiver")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Content")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Type")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Status")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateTime")%>&nbsp;
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