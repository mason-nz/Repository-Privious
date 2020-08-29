<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.WebForm1" %>

<%@ Register Src="UCOtherTask/WebUserControl1.ascx" TagName="WebUserControl1" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            aa();
        }); 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc2:WebUserControl1 ID="WebUserControl11" runat="server" />
    </div>
    <input type="button" value=" 测试 " onclick="javascript:aa();" />
    </form>
</body>
</html>
