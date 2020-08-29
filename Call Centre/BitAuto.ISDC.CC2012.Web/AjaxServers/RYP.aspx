<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RYP.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.RYP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var blNeedRedirect = '<%=NeedRedirect %>'
        window.onload = function () {
            if (blNeedRedirect=='1') {
                document.getElementById('form2').submit();
            }
        };
    </script>
</head>
<body>
    <form id="form1">
    </form>
    <form id="form2" action="http://dealer.easypass.cn/WeakSuperLogin.aspx" method="post"
    style="display: none;" target="_self">
    <input type="hidden" id="wkopuserid" name="wkopuserid" value="2784" runat="server" />
    <input type="hidden" id="token" name="token" runat="server" />
    </form>
    <%--<button id="btnTest" onclick="javascript:btnCick();">
        cesi
    </button>--%>
    <br/>
    <br/>
    <asp:Label runat="server" Style="font-size: 24px; color: red; margin: 20px;" ID="lbError">调用易湃接口错误或者账号不存在.</asp:Label>
</body>
</html>
