<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Iframe_Test.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Iframe_Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        #txtCITMsg
        {
            width: 800px;
            height: 150px;
        }
    </style>

    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css?v=20160407" type="text/css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="/Js/Enum/ShowEnum.js"></script>
<%--    <script type="text/javascript">
        loadJS("CTITool");
    </script>--%>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
        });
        //通知浏览器
        var ADTTool = (function () {

            var MethodScript = function (msg) {
                var pos = msg.indexOf('?');
                if (pos > 0) {
                    msg += "&WebPageID=" + window.name;
                }
                else {
                    msg += "?WebPageID=" + window.name;
                }
                return window.external.MethodScript(msg);
            };

            return {
                MethodScript: MethodScript
            }
        })();

        function CallOut(){
            try {
                ADTTool.MethodScript('/CallControl/MakeCall?targetdn=' + $('#txtMobile').val() );
            }
            catch (e) {
                alert('通话功能不可用！');
            }
        }

        //响应事件
        function MethodScript(message) {
            var date=new Date();
            var content = $('#txtCITMsg').val();
            $('#txtCITMsg').val(content+message + '\r\n' +'==========='+ date + '======' + '\r\n');
             
            if ($('#ckxSendSubPage').attr('checked')) {
                var obj = document.getElementById("iframeSubPage"); //获取对象
//                if ((obj.src + '').indexOf('UserEvent=')>0)
                //                {obj.src = obj.src; }
                alert(obj.contentWindow.location.href);
                if ((obj.contentWindow.location.href + '').indexOf('?') > 0) {
                    if ((obj.contentWindow.location.href + '').indexOf('UserEvent=') < 0) {

                        alert(obj.contentWindow.location.href + '&' + message);
                        obj.contentWindow.location.href = obj.contentWindow.location.href + '&' + message; 
                    }
                }
                else {
                    obj.contentWindow.location.href = obj.contentWindow.location.href + '?' + message;
                }
            }
        }

        function ClearText(){
            $('#txtCITMsg').val('');
        }

        function GetIFrameURL() {
            var obj = document.getElementById("iframeSubPage"); //获取对象
            alert(obj.contentWindow.location.href);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    父页面
    <br/>
    CTI消息：
    <textarea id="txtCITMsg" name='txtCITMsg' rows='10' ></textarea>
    <br/>
    电话号码：<input type="text" id="txtMobile" value="13146611863" />
    <input type="button" id="btnCallOut" value="外呼" onclick="CallOut();" />
    <input type="button" id="Button1" value="清空" onclick="javascript:ClearText();" />
    <label for="ckxSendSubPage">
    <input type="checkbox" id="ckxSendSubPage"/> 发送消息到子页面中</label>
    <input type="button" id="Button2" value="获取IframeURL" onclick="GetIFrameURL();" />
    <br/>
    <iframe id="iframeSubPage" src="http://ncc.sys1.bitauto.com" width="1200px" height="800px" scrolling="yes" ></iframe>
    </div>
    </form>
</body>
</html>
