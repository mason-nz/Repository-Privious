<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskProcessDetail.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.TaskProcessDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList">
            <tr>
                <th style="width: 10%;">
                    操作人
                </th>
                <th style="width: 15%;">
                    部门
                </th>
                <th style="width: 10%;">
                    动作
                </th>
                <th style="width: 10%;">
                    处理状态
                </th>
                <th style="width: 15%; color: Black; font-weight: bold">
                    操作时间
                </th>
                <th>
                    处理意见
                </th>
                <th>
                    播放录音
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" style="font-size: 12px" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#getEmployName(Eval("SolveUserEID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getDepartName(Eval("SolveUserEID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getActionName(Eval("Action").ToString(),Eval("ToNextSolveUserEID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("SolveTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <div class="deal" title="<%#Eval("Comment") %>">
                                <%#Eval("Comment") %>&nbsp;</div>
                        </td>
                        <td>
                        <%#ShowCallRecord(Eval("Action").ToString(), Eval("TaskID").ToString(), Eval("CallRecordID")==null?"":Eval("CallRecordID").ToString(), Eval("AudioURL")==null?"":Eval("AudioURL").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
