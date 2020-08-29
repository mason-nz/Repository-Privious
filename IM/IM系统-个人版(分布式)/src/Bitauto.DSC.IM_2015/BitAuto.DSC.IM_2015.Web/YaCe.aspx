<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YaCe.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.YaCe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="Setup/UdCapture.js" charset="utf-8"></script>
    <%--    <script type="text/javascript" src="OnlineService.js" charset="utf-8"></script>--%>
    <script type="text/javascript" src="Scripts/Enum/Area2.js"></script>
    <script type="text/javascript" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        loadJS("AspNetComet");
        loadJS("common");
    </script>
    <script type="text/javascript">
        function YaChe() {
            var shu = $("#idshu").val();
            for (var i=0; i < parseInt(shu); i++) {
                yace();
            }
        }

        function yace() {
            var pody = { action: 'init', EPTitle: '', FromPrivateToken: escape(''),
                UserReferURL: '', LoginID: escape(''), SourceType: '2', EPPostURL:
    '', usertype: escape('2'), CityID: escape('201'),
                ProvinceID: escape('2')
            };

            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null, function (msg) {

                if (msg != "") {
                    var r = JSON.parse(msg);
                    if (r != null && r.result == 'loginok') {
                        document.getElementById("divlist").innerHTML += r.loginid + "初始化对象成功！<br/>"
                        //请求进入队列
                        CominQuene(r.loginid, '2')
                        //
                    }
                }

            });
        }
        function CominQuene(LoginID, SourceType) {
            var pody = { action: 'cominquene', FromPrivateToken: escape(LoginID), SourceType: escape(SourceType) };
            AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r1 = JSON.parse(msg);
                 //alert(r1.result);
                 if (r1 != null && r1.result == '0') {//登录成功之后
                     document.getElementById("divlist").innerHTML += LoginID + "进入排队队列成功！<br/>"
                 }
                 else if (r1 != null && r1.result == '1') {

                 }
                 else if (r1 != null && r1.result == '2') {
                     //目前没有空闲客服
                 }
             });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        请输入压测对象数
        <input type="text" id="idshu" />
        <input type="button" value="压测" onclick="YaChe()" />
    </div>
    <div id="divlist" style="width: 400px; height: auto; border: 1px solid #00F">
    </div>
    </form>
</body>
</html>
