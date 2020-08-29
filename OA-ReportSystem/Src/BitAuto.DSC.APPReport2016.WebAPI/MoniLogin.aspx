<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoniLogin.aspx.cs" Inherits="BitAuto.DSC.APPReport2016.WebAPI.MoniLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模拟登陆</title>
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    请输入域账号：
        <input id="txtAdName" type="text" runat="server" />
        <asp:Button ID="btnLogin" runat="server" Text="模拟登陆" onclick="btnLogin_Click" />
        <br />
        <span id='spanError' runat="server"></span>
    </div>
    </form>
</body>
</html>
