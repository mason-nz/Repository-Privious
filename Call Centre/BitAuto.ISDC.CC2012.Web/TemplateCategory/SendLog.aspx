<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendLog.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateCategory.SendLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        日志类型：
        <asp:RadioButton GroupName="SendType" runat="server" ID="RB_EMail" Checked="true" Text="邮件" AutoPostBack="true" OnCheckedChanged="typeChange" />
        <asp:RadioButton GroupName="SendType" runat="server" ID="RB_SM" Text="短信" AutoPostBack="true" OnCheckedChanged="typeChange" />
    </div>
    <table id="DateTable">
        <tr>
            <th>模板名称</th>
            <th>发往地址</th>
            <th>发送时间</th>
            <th>发送内容</th>
            <th>发送人</th>
        </tr>
        <asp:Repeater runat="server" ID="Rpt_LogList">
        <ItemTemplate>
        <tr>
            <td><%#Eval("Title") %></td>
            <td><%#Eval("AllTo") %></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
        
    </table>
    <div>
        <asp:Literal ID="Ltr_page" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
