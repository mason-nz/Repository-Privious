<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalHistoryList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ApprovalHistoryList" %>

<div class="title">
    操作记录 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
        style="*margin-top: -25px;"></a>
</div>
<table width="100%" cellspacing="0" cellpadding="0" border="0">
    <tbody>
        <tr>
            <th class="bdlnone">
                操作人
            </th>
            <th>
                动作
            </th>
            <th>
                结果
            </th>
            <th>
                备注
            </th>
            <th>
                操作时间
            </th>
        </tr>
        <asp:repeater id="rptApprovalHistoryList" runat="server">
              <ItemTemplate>
              <tr>
                <td class="bdlnone"><%#Eval("TrueName") %></td>
                <td><%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.QSApprovalType), int.Parse(Eval("ApprovalType").ToString())) %></td>
                <td><%#Eval("ApprovalResult").ToString() == "1" ? "通过" : (Eval("ApprovalResult").ToString()=="2"?"拒绝":"")%></td>
                <td><%#Eval("Remark") %></td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
              </tr>
              </ItemTemplate>
              </asp:repeater>
    </tbody>
</table>
