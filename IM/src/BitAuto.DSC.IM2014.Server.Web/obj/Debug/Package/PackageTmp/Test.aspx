<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">  
    <head>  
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />  
    <script src="js/jquery-1.6.4.min.js" language="javascript" type="text/javascript"></script>
    <script src="js/public.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" language="javascript" src="Scripts/AspNetComet.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/common.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/jquery.hotkeys.js"></script>
    <script type="text/javascript">
        $(document).bind("contextmenu", function () { return false; });
        $(document).bind("selectstart", function () { return false; });
        //$(document).keydown(function () { return key(arguments[0]) }); 
        function SetHotkey() {
            $(document).bind('keydown', 'ctrl+z', function () {
                alert("ok");
            });
        }

        function demo() {
            var is_asked = false;
            window.onbeforeunload = function (ev) {
                var e = ev || window.event;
                window.focus();
                if (!is_asked) {
                    is_asked = true;
                    var showstr = "CUSTOM_MESSAGE";
                    if (e) { //for ie and firefox
                        e.returnValue = showstr;
                    }
                    return showstr; //for safari and chrome
                }
            };


            window.onfocus = function (ev) {
                if (is_asked) {
                    window.location.href = "http://www.google.com";
                }
            } 
        }
        $(document).ready(function () {
            SetHotkey();

            var is_asked = false;
            window.onbeforeunload = function (ev) {
                if (confirm("测试")) {
                    window.location.href = "http://www.google.com";
                }
                else {
                    window.location = "Test.aspx";
                }
                //                var e = ev || window.event;
                //                window.focus();
                //                if (!is_asked) {
                //                    is_asked = true;
                //                    var showstr = "CUSTOM_MESSAGE";
                //                    if (e) { //for ie and firefox
                //                        e.returnValue = showstr;
                //                    }
                //                    window.location.href = "http://www.google.com";
                //                    return showstr; //for safari and chrome
                //                }
            };


            window.onfocus = function (ev) {
                if (is_asked) {
                    //window.location.href = "http://www.google.com";
                    window.location.href = "Test.aspx";
                }
            }

        });
    </script>
        <script type="text/javascript">  
            //<![CDATA[
            $.setupJMPopups({
                screenLockerBackground: "#003366",
                screenLockerOpacity: "0.7"
            });

            function openStaticPopup() {
                $.openPopupLayer({
                    name: "myStaticPopup",
                    width: 600,
                    target: "myHiddenDiv"
                });
            }

            function openStaticPopup() {
                $.openPopupLayer({
                    name: "myStaticPopup2",
                    width: 600,
                    target: "myHiddenDiv2"
                });
            }

            function openAjaxPopup() {
                $.openPopupLayer({
                    name: "mySecondPopup",
                    width: 300,
                    url: "ajax_example.html"
                });
            } 
            //]]>  
        </script>  
        <style type="text/css" media="screen">  
            body {margin:0; font-family:"Trebuchet MS"; line-height:120%; color:#333;}  
            h1 {margin-bottom:50px; font-size:40px; font-weight:normal;}  
            p {margin:0; padding:0 0 30px; font-size:12px;}  
            pre {font-size:12px; font-family:"Consolas","Monaco","Bitstream Vera Sans Mono","Courier New","Courier"; line-height:120%; background:#F4F4F4; padding:10px;}  
            #general {margin:30px;}  
              
            #myHiddenDiv {display:none;}  
            #myHiddenDiv2 {display:none;} 
              
            .popup {background:#FFF; border:1px solid #333; padding:1px;}  
            .popup-header {height:24px; padding:7px; background:url("bgr_popup_header.jpg") repeat-x;}  
            .popup-header h2 {margin:0; padding:0; font-size:18px; float:left;}  
            .popup-header .close-link {float:right; font-size:11px;}  
            .popup-body {padding:10px;height:500px}  
              
            form {margin:0; padding:0;}  
            form * {font-size:12px;}  
            input {margin-bottom:12px;}  
            label {display:block;}    
        </style>  
        <title>jmpopups - example page</title>  
    </head>  
    <body>  
        <div id="general">  
            <p><a href="javascript:;" onclick="openStaticPopup()" title="Static example">Open a popup using a html content from a hidden element.</a></p>  
            <p><a href="javascript:;" onclick="openStaticPopup()" title="Static example">i测试.</a></p>
            </div>  
        <div id="myHiddenDiv">  
            <div class="popup">  
                <div class="popup-header">  
                    <h2>Now an ajax example</h2>  
                    <a href="javascript:;" onclick="$.closePopupLayer('myStaticPopup')" title="Close" class="close-link">Close</a>  
                    <br clear="all" />  
                </div>  
                <div class="popup-body">  
                      
      
                    <p><a href="javascript:;" onclick="openAjaxPopup()" title="Ajax example">Open a popup using ajax</a></p>  
                </div>  
            </div>  
        </div> 
        <div id="myHiddenDiv2">  
            <div class="popup">  
                <div class="popup-header">  
                    <h2>Now an ajax example</h2>  
                    <a href="javascript:;" onclick="$.closePopupLayer('myStaticPopup2')" title="Close" class="close-link">Close</a>  
                    <br clear="all" />  
                </div>  
                <div class="popup-body">  
                      
      
                    <p><a href="javascript:;" onclick="openAjaxPopup()" title="Ajax example">Open a popup using ajax</a></p>  
                </div>  
            </div>  
        </div>          
    </body>  
</html>  
