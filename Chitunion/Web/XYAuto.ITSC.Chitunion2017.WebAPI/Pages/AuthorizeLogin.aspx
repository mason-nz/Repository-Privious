<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizeLogin.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.WebAPI.Pages.AuthorizeLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>赤兔联盟广告在线交易平台</title>
    <link rel="stylesheet" type="text/css" href="/css/resetNew.css" />
    <link rel="stylesheet" type="text/css" href="/css/layoutNew.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.openPopupLayer({
                name: "popLayerDemo",
                target: "wxDialog_1",
                error: function (dd) { alert(dd.status); }
            });
            $('#lbtnClose').bind('click', function () {
                alert(111);
                window.opener = null;
                window.open('', '_self');
                window.close();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div id="wxDialog_1" style="display: none;">
            <div class="layer" style="width: 600px;">
                <div class="title">
                    温馨提示
        <div class="close">
        </div>
                </div>

                <div class="layer_con3">
                    <div class="familiar">
                        验证不通过，具体原因为：<br />
                        <%=RetvalMsg %>
                    </div>

                    <div class="keep" style="text-align: center; margin-left: 0; margin-top: 0">
                        <%--<a href="javascript:void(0);" id="lbtnClose" class="button" style="width: 140px">关闭</a>--%>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
