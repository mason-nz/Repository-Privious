<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessLicensePicView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.BusinessLicensePicView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="repeaterFileDocList" runat="server">
            <ItemTemplate>
                <img src="<%=CRMSiteURL %><%# Eval("FilePath").ToString() %>" style="width: 800px;" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>

