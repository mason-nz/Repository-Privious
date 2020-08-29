<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.HistoryList" %>

<table style="width: 94%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="15%">
            操作时间
        </th>
        <th width="8%">
            操作类型
        </th>
        <th width="15%">
            任务状态
        </th>
        <th width="18%">
            操作人
        </th>
        <th width="44%">
            备注
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater_Contact">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                                            </td>
                                            <td>
                                                <%# GetType(Eval("OperationStatus").ToString())%>
                                            </td>
                                            <td>
                                                <%# GetTaskStatus(Eval("TaskStatus").ToString())%>
                                            </td>
                                            <td>
                                                <%# BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(Eval("CreateUserID").ToString()))%>
                                            </td>
                                            <td class="l">
                                                <%#Eval("Remark")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:repeater>
</table>
