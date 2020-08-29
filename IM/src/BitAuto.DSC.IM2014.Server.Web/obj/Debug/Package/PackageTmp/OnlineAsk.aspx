<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlineAsk.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.OnlineAsk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/uimg/index091217/js/swfobject.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        function LoadOnlineAsk(divid, type, onlineaskurl, imgurl, width, height, num) {
            try {
                var id = 'bitAd_floatFlashforOnlineask_' + num;
                document.getElementById(divid).innerHTML += '<div id=' + id + '></div>';
                var bitAdfloatFlash = [{ 'url': imgurl, 'type': 'image', 'width': width, 'height': height, link: onlineaskurl}];
                var pro = this;
                var minstr = bitAdFrame.buildHtml(bitAdfloatFlash[0], 0);
                var obj = bitAdFrame.$(id);
                var ie6 = ! -[1, ] && !window.XMLHttpRequest;
                var str = '<div id="' + id + '_min" style="position:fixed;' + (ie6 ? '_position:absolute;' : '') + 'font-size:12px;z-index:999999;' + (type == 'left' ? 'left' : 'right') + ':0;bottom:20px;' + (ie6 ? '_top:expression(eval(document.documentElement.scrollTop+document.documentElement.clientHeight-this.offsetHeight-20));' : '') + 'cursor:pointer;;width:' + bitAdfloatFlash[0].width + 'px;" >';
                str += minstr;
                str += '</div>';
                obj.innerHTML = str;
            } catch (err) { }
        }
        $(document).ready(function () {
            LoadOnlineAsk("div1", 'right', 'UserChat.aspx', '123.gif', 152, 118, 1);
            LoadOnlineAsk("div1", 'left', 'UserChat.aspx', '123.gif', 152, 118, 2);
        });
    </script>
</head>
<body>
    <div id="div1">
    </div>
</body>
</html>
