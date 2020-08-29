<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrmMemberLog.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CrmMemberLog" %>

<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
 <tr>
                                    <th width="20%">
                                        操作人
                                    </th>
                                    <th width="12%">
                                        动作
                                    </th>
                                    <th width="20%">
                                        时间
                                    </th>
                                    <th width="38%">
                                        说明
                                    </th>
                                </tr>
                                <asp:Repeater runat="server" ID="repeater_Contact">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(Eval("CreateUserID").ToString()))%>
                                            </td>
                                            <td>
                                                <%# GetSyncStatusDesc(Eval("SyncStatus").ToString())%>
                                            </td>
                                            <td>
                                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                                            </td>
                                            <td class="l">
                                                <%#Eval("Description")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
</table>