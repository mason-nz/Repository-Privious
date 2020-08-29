<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentDefault.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.AgentDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户名:
        <asp:TextBox runat="server" ID="username"></asp:TextBox><br />
        <asp:Button ID="Button1" type="button" Text="login" runat="server" OnClick="Login_Click">
        </asp:Button>
        <p style="color: Red;">
            <asp:Literal runat="server" ID="errorMessage" EnableViewState="false">
            </asp:Literal>
        </p>            
    </div>
    </form>
</body>
</html>
