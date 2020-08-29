<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyLogOfCarType.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.ModifyLogOfCarType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        table
        {
            background-color: Green;
        }
        
        td
        {
            background-color: White;
        }
        p
        {
            color:Red;
            font-weight:bold;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>无主订单中，修改过车型的订单记录</p>
   <br />
   <a href="ModifyLogOfCarType.aspx?action=export">导出</a><br /><br />
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    序号
                </td>
                <td>
                    类型
                </td>
                <td>
                    [易湃订单ID]
                </td>
                <td>
                    [订购者姓名]
                </td>
                <td>
                    [订购者手机]
                </td>
                <td>
                    [订购者电话]
                </td>
                <td>
                    [原车款ID]
                </td>
                <td>
                    [原车款名称]
                </td>
                <td>
                    [修改后的车型ID]
                </td>
                <td>
                    [修改后的车型名称]
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rpCarList">
                <ItemTemplate>
                    <td>
                        <%# Container.ItemIndex+1 %>
                    </td>
                    <td>
                        <%#Eval("类型")%>
                    </td>
                    <td>
                        <%#Eval("易湃订单ID")%>
                    </td>
                    <td>
                        <%#Eval("订购者姓名")%>
                    </td>
                    <td>
                        <%#Eval("订购者手机")%>
                    </td>
                    <td>
                        <%#Eval("订购者电话")%>
                    </td>
                    <td>
                        <%#Eval("原车款ID")%>
                    </td>
                    <td>
                        <%#Eval("原车款名称")%>
                    </td>
                    <td>
                        <%#Eval("修改后的车型ID")%>
                    </td>
                    <td>
                        <%#Eval("修改后的车型名称")%>
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
