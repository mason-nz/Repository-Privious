<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WcfClientManagerList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage.WcfClientManagerList" %>

<div class="bit_table">
    <!--列表开始-->
    坐席列表
    <div class="faqList">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <th width="15%">
                    名称
                </th>
                <th width="15%">
                    坐席标识
                </th>
                <th width="20%">
                    业务线标识
                </th>
                <th width="18%">
                    状态
                </th>
                <th width="18%">
                    最近活动时间
                </th>
                <th width="15%">
                    操作
                </th>
            </tr>
            <asp:repeater runat="server" id="agentlist">
     <ItemTemplate>
          <tr>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).AgentName%>&nbsp;
                </td> 
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).AgentToken%>&nbsp;
                </td>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).BusinessLines%>&nbsp;
                </td>
                <%--<td>
                      <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).Status.ToString()=="1"?"在线":(((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).Status.ToString()=="2"?"离线":"暂离")%>&nbsp;
                </td>--%>
                <td>
                      <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).Status.ToString()%>&nbsp;
                </td>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).LastActiveTime%>&nbsp;
                </td>
                <td>
                    <input type="button" onclick="DeleteAgent('<%#((BitAuto.DSC.IM_2015.MainInterface.ProxyAgentClient)Container.DataItem).AgentID%>')"  value="删除"/>
                </td>
            </tr>
        </ItemTemplate>
        </asp:repeater>
        </table>
    </div>
</div>
<br />
<div class="bit_table">
    <!--列表开始-->
    网友列表
    <div class="faqList">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <th width="15%">
                    实体名称
                </th>
                <th width="15%">
                    实体标识
                </th>
                <th width="20%">
                    业务线标识
                </th>
                <th width="15%">
                    在聊坐席
                </th>
                <th width="18%">
                    最近活动时间
                </th>
            </tr>
            <asp:repeater runat="server" id="wylist">
     <ItemTemplate>
          <tr>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyNetFriend)Container.DataItem).NetFName%>&nbsp;
                </td> 
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyNetFriend)Container.DataItem).Token%>&nbsp;
                </td>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyNetFriend)Container.DataItem).BusinessLines%>&nbsp;
                </td>
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyNetFriend)Container.DataItem).AgentToken%>&nbsp;
                </td> 
                <td>
                    <%#((BitAuto.DSC.IM_2015.MainInterface.ProxyNetFriend)Container.DataItem).LastActiveTime%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
        </asp:repeater>
        </table>
    </div>
</div>
