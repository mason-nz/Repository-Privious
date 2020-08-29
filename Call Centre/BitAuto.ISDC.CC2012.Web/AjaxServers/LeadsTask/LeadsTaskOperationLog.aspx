<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadsTaskOperationLog.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.LeadsTask.LeadsTaskOperationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("divHandlerHistory");
            $('.bit_table').load('../AjaxServers/LeadsTask/LeadsTaskOperationLog.aspx .bit_table > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" style="width: 100%; margin-bottom: 40px;">
            <tr>
                <th style="width: 10%; color: Black; font-weight: bold;">
                    操作时间
                </th>
                <th style="width: 10%;">
                    操作类型
                </th>
                <th style="width: 10%;">
                    任务状态
                </th>
                <th style="width: 10%;">
                    操作人
                </th>
                <th style="width: 10%;">
                    备注
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" style="font-size: 12px" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getActionName(Eval("OperationStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("TaskStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Convert.ToInt32(Eval("OperationStatus")) == (Int32)BitAuto.ISDC.CC2012.Entities.Leads_OperationStatus.Gen || Convert.ToInt32(Eval("OperationStatus")) == (Int32)BitAuto.ISDC.CC2012.Entities.Leads_OperationStatus.End ? "—" : getEmployName(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Remark").ToString()%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="clearfloat" style="clear: both; height: 0; font-size: 1px; line-height: 0px;">
        </div>
    </div>
    </form>
</body>
</html>
