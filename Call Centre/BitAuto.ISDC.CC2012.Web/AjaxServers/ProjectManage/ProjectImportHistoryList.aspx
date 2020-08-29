<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectImportHistoryList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage.ProjectImportHistoryList" %>

<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="tableList">
    <tr>
        <th style="width: 18%; height: 28px">
            导入人
        </th>
        <th style="width: 18%; height: 28px">
            导入数量
        </th>
        <th style="width: 18%; height: 28px">
            导入时间
        </th>
        <%--<th width="20%">播放录音 </th>--%>
    </tr>
    <asp:repeater id="repeater_ImportHistory" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                   <%#getOperator(Eval("CreateUserID").ToString())%>               
                </td>
                <td>
                    <%#Eval("ImportNumber").ToString()%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
            </tr>               
        </ItemTemplate>
    </asp:repeater>
</table>
