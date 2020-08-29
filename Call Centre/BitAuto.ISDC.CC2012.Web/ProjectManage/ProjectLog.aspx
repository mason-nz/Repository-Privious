<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectLog.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.ProjectLog" %>

<table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs czr_list">
    <tr>
        <th width="15%">
            操作人
        </th>
        <th width="20%">
            操作类型
        </th>
        <th width="45%">
            备注
        </th>
        <th width="20%">
            操作时间
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr>
            <td>
                <%#Eval("TrueName").ToString().Trim() == "" ? "系统" : Eval("TrueName").ToString()%>&nbsp;
            </td>
            <td>
                <%#Eval("OperName")%>&nbsp;
            </td>
            <td class="l_dq">
                <div class="bzh">
                    <%#Eval("Remark")%>&nbsp;
                </div>
            </td>
            <td>
                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
            </td>
        </tr>
        </ItemTemplate>
     </asp:repeater>
</table>
<div class="pageTurn mr10" style="margin-top: 10px;">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
