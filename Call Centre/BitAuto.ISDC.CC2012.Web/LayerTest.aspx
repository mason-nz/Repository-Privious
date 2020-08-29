<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LayerTest.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.LayerTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
    <link href="Css/base.css" rel="stylesheet" type="text/css" />
    <script src="Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="CTI/CTITool.js" type="text/javascript"></script>
    <script src="Js/UserControl.js" type="text/javascript"></script>
    <script src="Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript">
        CustBaseInfoPopControl.SetSaveCompleteEvent(function (id) {
            alert(id);
        });
        function testPOPgr() {
            CustBaseInfoPopControl.Open("18629257531", "个人", "", "", "");
        }
        function testPOPjxs() {
            CustBaseInfoPopControl.Open("18629257531", "经销商", "1000002007", "", "", "");
        }
    </script>
</head>
<body>
    <form id="form1">
    <div>
        <input type="button" value="测试弹层" onclick="testPOPgr()" />
        <input type="button" value="测试弹层" onclick="testPOPjxs()" />
    </div>
    </form>
</body>
</html>
