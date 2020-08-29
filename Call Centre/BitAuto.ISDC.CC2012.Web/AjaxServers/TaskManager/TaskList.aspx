<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.TaskList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
            <tr>
                <th>
                    任务ID
                </th>
                <th>
                    客户姓名
                </th>
                <th>
                    当前受理人
                </th>
                <th>
                    处理状态
                </th>
                <th>
                    最晚处理时间
                </th>
                <th>
                    创建时间
                </th>
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("TaskID") %>&nbsp;
                        </td>
                        <td>
                            <a target='_blank' href='../../TaskManager/CustInformation.aspx?CustID=<%#Eval("CustID")%>'
                                class="linkBlue">
                                <%#Eval("CustName")%>&nbsp; </a>
                        </td>
                        <td>
                            <%#getEmployNameByTaskID(Eval("TaskID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("ProcessStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastTreatmentTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator( Eval("TaskID").ToString(),Eval("ProcessStatus").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
