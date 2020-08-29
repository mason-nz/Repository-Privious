<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BitAuto.DSC.IM_2015.EPLogin.Test._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button runat="server" Text="测试" onclick="Unnamed1_Click"/>
    </div>
    <asp:Button ID="btnCreateObj" runat="server" Text="创建对象" 
        onclick="btnCreateObj_Click" />
    <asp:Button ID="btnPressure" runat="server" Text="压力测试" 
        onclick="btnPressure_Click" />
    </form>
</body>
</html>
