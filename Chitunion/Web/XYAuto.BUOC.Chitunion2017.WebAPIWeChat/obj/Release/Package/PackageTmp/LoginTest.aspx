<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginTest.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.LoginTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        手机号：<asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
        <br />
        <asp:RadioButton ID="radiobtnGGZ" runat="server" GroupName="Category" Text="广告主" />
        <asp:RadioButton ID="radiobtnMTZ" runat="server" Checked="True" GroupName="Category" Text="媒体主" />
        <br />
        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="登陆" style="height: 21px" />
        <asp:Button ID="btnExit" runat="server" OnClick="btnExit_Click" Text="退出" />
    </form>
</body>
</html>
