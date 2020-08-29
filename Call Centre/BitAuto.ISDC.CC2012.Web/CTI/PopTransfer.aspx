<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopTransfer.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CTI.PopTransfer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>呼入弹屏页面</title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            window.location.href = "<%=gotourl %>";
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; text-align: center; margin-top: 150px;">
        <img src="../Images/blue-loading.gif" style="vertical-align: middle; margin-right: 5px" />
        <span>查询客户信息中，请稍后....</span>
    </div>
    </form>
</body>
</html>
