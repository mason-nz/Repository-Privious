<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupOrderTaskOperLogList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.GroupOrderTaskOperLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("divHandlerHistory");
            $('.bit_table').load('../AjaxServers/GroupOrder/GroupOrderTaskOperLogList.aspx .bit_table > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" style="margin-bottom: 40px;">
            <tr>
                <th style="width: 10%;">
                    操作人
                </th>
                <th style="width: 10%;">
                    动作
                </th>
                <th style="width: 5%;">
                    状态
                </th>
                <%--<th style="width: 10%;">
                    备注
                </th>--%>
                <th style="width: 10%; color: Black; font-weight: bold;">
                    操作时间
                </th>
                <th style="width: 10%;">
                    播放录音
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" style="font-size: 12px" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#getEmployName(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getActionName(Eval("OperationStatus").ToString(), Eval("Remark").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("TaskStatus").ToString())%>&nbsp;
                        </td>                        
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("createtime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../../Images/callTel.png' /></a>"%>&nbsp;
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
