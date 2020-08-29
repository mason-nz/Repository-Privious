<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineDialogList.aspx.cs"
    Inherits="BitAuto.DSC.IM2014.Server.Web.AjaxServers.OnlineDialogList" %>
<table id="DialogListTable" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="20%">
            访客ID
        </th>
        <th width="20%">
            开始时间
        </th>
        <th width="15%">
            对话时长
        </th>
        <th width="15%">
            姓名
        </th>
        <th width="15%">
            地区
        </th>
        <th width="15%">
            客服
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="$($('#DialogListTable tr')).attr('class','');this.className='current';SelectAlloc('<%#Eval("AllocID").ToString()%>','<%# Eval("StartTime").ToString()%>','<%# Eval("AgentEndTime").ToString()%>','<%# Eval("UserEndTime").ToString()%>')">
                                <td>
                                    <%#Eval("UserID").ToString().Substring(Eval("UserID").ToString().LastIndexOf("-")+1,12) %>&nbsp;
                                </td>
                                <td>
                                    <%#Eval("StartTime").ToString()%>&nbsp;
                                </td>
                                <td>
                                    <%#GeTimeLength(Eval("StartTime").ToString(), Eval("AgentEndTime").ToString(), Eval("UserEndTime").ToString())%>&nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <%#Eval("Location")%>&nbsp;
                                </td>
                                <td>
                                    <%#Eval("UserName")%>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
</table>
<!--分页开始-->
<div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
    <asp:literal runat="server" id="litPagerDown"></asp:literal>
</div>
