<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebTest.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.WebTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script>


            var timer = null;
            var oPopup = window.createPopup();
            var CW_Body = oPopup.document.body;
            var CSStext = "margin:1px;color:black; border:2px outset;background-color:buttonface;width:16px;height:14px;font-size:12px;line-height:11px;cursor:hand;";
            var popTop = 50;
            function popmsg(content) {
                var temp = "";
                CW_Body.style.overflow = "hidden";
                CW_Body.style.backgroundColor = "white";
                CW_Body.style.border = "solid black 1px";

                temp += "<table width=100% height=100% cellpadding=0 cellspacing=0 border=0 >";
                temp += "<tr style=';font-size:12px;background:#0099CC;height:20;cursor:default'>";
                temp += "<td style='color:white;padding-left:5px'>消息提示</td>";
                temp += "<td style='color:#ffffff;padding-right:5px;' align=right>";
                temp += "<span id=Close onclick='parent.pophide()' style=\"" + CSStext + "font-family:System;padding-right:2px;\" title='关闭'>x</span>";
                temp += "</td></tr><tr><td colspan=2>";
                temp += "<div id=include style='overflow:scroll;overflow-x:hidden;overflow-y:auto;HEIGHT:100%;padding-left:5px;padding-top:5px;font-size:13px;'>";
                temp += content;
                temp += "</div>";
                temp += "</td></tr></table>";
                CW_Body.innerHTML = temp;
                popshow();

            }

            function popshow() {
                //window.status = popTop;
                //            if (popTop < 180) {
                //                oPopup.show(screen.width - 243, screen.height-100, 241, popTop);
                //            } else if (popTop < 220) {
                //                oPopup.show(screen.width - 243, screen.height-100, 241, 173);
                //            }
                oPopup.show(screen.width - 243, screen.height - 150, 241, 173);
                //popTop += 10;
                //if (popTop <= 220) {
                timer = setTimeout("popshow();", 50);
                //}
                //else {
                //popTop = 1500;
                //}
            }

            function pophide() {
                //            window.status = popTop;
                //            if (popTop > 1720) {
                //                oPopup.hide();
                //                popTop = 50;
                //                return;
                //            } else if (popTop > 1520 && popTop < 1720) {
                //                oPopup.show(screen.width - 243, screen.height, 241, 1720 - popTop);
                //            } else if (popTop > 1500 && popTop < 1520) {
                //                oPopup.show(screen.width - 243, screen.height + (popTop - 1720), 241, 172);
                //            }
                //            popTop += 10;
                //            var mytime = setTimeout("pophide();", 50);
                //clearTimeout(timer);
                oPopup.hide();
            }
            
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" value="测试" onclick="popmsg('你好<br/>中国<br/>世界');"/>
    </div>
    </form>
</body>
</html>
