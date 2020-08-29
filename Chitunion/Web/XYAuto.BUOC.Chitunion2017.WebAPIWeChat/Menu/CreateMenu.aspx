<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateMenu.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.Menu.CreateMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtMenu" runat="server" Height="23px" Width="472px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnMenu" runat="server" Text="创建菜单" OnClick="btnMenu_Click" />
            <asp:Button ID="btnMenu0" runat="server" Text="清除菜单" OnClick="btnMenu0_Click" />
            <br />
            <br />
            <asp:Literal ID="litlMenu" runat="server" Visible="false"></asp:Literal>
            <br />
            <br />
            <br />
            <br />            
        </div>


    </form>
</body>
</html>
