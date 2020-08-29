<%@ Page Title="外呼技能组初始化页" Language="C#" AutoEventWireup="true" CodeBehind="InitialSkill.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.InitialSkill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="初始化外呼技能" 
            onclick="Button1_Click" />
            <br /><br />

        <asp:Label ID="lb_msg" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
