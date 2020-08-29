<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BitAuto.DSC.IM_2015.EPLogin.Test.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function aa() {
            $("input[name='PageTitle']").val(escape('百度首页'));
            document.getElementById("form2").submit();
        }

        function bb() {
            $("input[name='PageTitle']").val(escape('商城'));
            $("input[name='ShowPageTitle']").val(escape('业务线展示页面title'));
            document.getElementById("form1").submit();
        }
        function cc() {
            var a = "23233";
            var b = "3";
            var urlstr = "http://im.sys1.bitauto.com/ImGetLogin.aspx?LoginID=" + a + "&SourceType=b&logcallback=?";
            $.getJSON(urlstr, null, null);

        }
    </script>
</head>
<body>
    <form id="form2" action="http://im.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle" name="PageTitle" value='百度首页' />
    <input type="hidden" id="hidUrl" name="SourceUrl" value="http://attend.oa.bitauto.com/admin/AttendLogTable.aspx" />
    <input type="hidden" id="Hidden1" name="SourceType" value="2" />
    </form>
    <form id="form1" action="http://im.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="Hidden2" name="PageTitle" value='商城' />
    <input type="hidden" id="Hidden3" name="SourceUrl" value="http://attend.oa.bitauto.com/admin/AttendLogTable.aspx" />
    <input type="hidden" id="Hidden4" name="SourceType" value="3" />
    <input type="hidden" name="ShowPageTitle" value='业务线展示页面title' />
    <input type="hidden" name="ShowPageUrl" value="http://attend.oa.bitauto.com/admin/AttendLogTable.aspx" />
    </form>
    <input type="button" value="登录" onclick="aa()" />
    <input type="button" value="商城登录" onclick="bb()" />
    <input type="button" value="模拟商城登录" onclick="cc()" />
    <div id="aa">
    </div>
</body>
</html>
