<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>
        UserID:<input id="txtUserID" type="text" runat="server" /><br/>
        <asp:Button ID="Button1" runat="server" Text="生成Cookies内容" OnClick="Button1_Click" />
        <asp:Literal ID="litCookieText" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
