<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CstMemberFullNameHistory.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CSTMember.CstMemberFullNameHistory" %>
<div class="openwindow">
    <div class="close">
        <a onclick="javascript:$.closePopupLayer('CstMemberFullNameHistoryShowAjaxPopup');">关闭</a></div>
    <h2>
        <span>曾用名列表</span></h2>
    <form id="formShowCustomerNameHistoryAjaxPopup" style="background-color:White">
    
        <fieldset id="fdCustID">
            <span id="spanCustID" runat="server" style=" font-size:14px; font-weight:bold;"></span>
        </fieldset>
        
        <fieldset class="tb">
            <div class="cont_cxjg">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableNameHistory">
                    <tr class="color_hui">
                        <td width="50%">
                            <strong>会员曾用名称</strong>
                        </td>
                        <td width="13%">
                            <strong>变更人</strong>
                        </td>
                        <td width="37%">
                            <strong>变更时间</strong>
                        </td>
                    </tr>
                    <asp:Repeater ID="repterCustNameHistoryList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="text-align:left">
                                    <%# Eval("FullName")%>
                                </td>
                                <td class="l" style="text-align:center;">
                                   <%#Eval("TrueName") %>
                                </td>
                                <td>
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </fieldset>
    </form>
</div>
<script type="text/javascript">
    $('#tableNameHistory tr:even').addClass('color_hui'); //设置列表行样式
    $('#tableNameHistory tr:even').addClass('color_hui'); //设置列表行样式
</script>