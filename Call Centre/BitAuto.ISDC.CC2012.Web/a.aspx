<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="a.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.a" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function kk() {
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "b.aspx", //请求页面
                data: "id=1",
                complete: function () { location.href = "b.aspx" } //跳转页面
            });
        }
    </script>
</head>
<body>
    <a href='javascript:void(0);' onclick='kk()'><font color='#000000'>首页</font></a>
</body>
</html>
