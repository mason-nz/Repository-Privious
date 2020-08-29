<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChatMessageLog.aspx.cs"
    Inherits="BitAuto.DSC.IM2014.Server.Web.ChatMessageLog.ChatMessageLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>聊天记录查询</title>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindBeginEndtime();
            $('#txtBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtBeginTime\')}' }); });
            enterSearch(search);

            search();
        });

        //页面敲回车键，执行的方法，funName-方法名
        //注：如果列表页面有弹出层也需要使用该方法，则需要在列表页面调用完弹出层方法后的回调函数重新绑定该方法，否则列表页因为document的keydown事件被解除而不可用
        enterSearch = function (funName) {
            $(document).unbind("keydown");
            $(document).keydown(function (event) {
                if (event.keyCode == 13) {
                    funName();
                }
            });
        }

        function BindBeginEndtime() {

            var today = new Date();
            var days = 0; //当天至上周一，相差天数
            days = today.getDay() + 6;

            var lastMonday_milliseconds = 0; //当天至上周一，相差毫秒数
            lastMonday_milliseconds = today.getTime() - 1000 * 60 * 60 * 24 * days;
            var lastMonday = new Date();
            lastMonday.setTime(lastMonday_milliseconds);

            var strYear = lastMonday.getFullYear();
            var strDay = lastMonday.getDate();
            var strMonth = lastMonday.getMonth() + 1;
            if (strMonth < 10) {
                strMonth = "0" + strMonth;
            }
            if (strDay < 10) {
                strDay = "0" + strDay;
            }
            var strlastMonday = strYear + "-" + strMonth + "-" + strDay;
            $("#txtBeginTime").val(strlastMonday);

            var lastSunday_milliseconds = 0; //当天至上周日，相差毫秒数
            lastSunday_milliseconds = today.getTime() - 1000 * 60 * 60 * 24 * (days - 6);
            var lastSunday = new Date();
            lastSunday.setTime(lastSunday_milliseconds);

            strYear = lastSunday.getFullYear();
            strDay = lastSunday.getDate();
            strMonth = lastSunday.getMonth() + 1;
            if (strMonth < 10) {
                strMonth = "0" + strMonth;
            }
            if (strDay < 10) {
                strDay = "0" + strDay;
            }
            var strlastSunday = strYear + "-" + strMonth + "-" + strDay;
            $("#txtEndTime").val(strlastSunday);
        }

        function search() {
            var parameters = {
                BeginTime: escape($("#txtBeginTime").val()),
                EndTime:escape($("#txtEndTime").val()),
                Sender: escape($("#txtSenderID").val()),
                Receiver: escape($("#txtReceiverID").val())                
            };

            $("#ajaxTable").load("/ChatMessageLog/ChatMessageLogList.aspx", parameters, function () {
                //alert("ok");
            });

        }

        //分页操作
        function ShowDataByPost1(pody) {
            $("#ajaxTable").load("/ChatMessageLog/ChatMessageLogList.aspx", pody, function () {

            });
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            聊记录查询</h2>
        <div>
            <ul>
                <li>
                    <label>
                        聊天日期：</label>
                    <input id="txtBeginTime" name="BeginTime" />
                    至
                    <input id="txtEndTime" name="EndTime" />
                </li>
                <li>
                    <label>
                        发送人ID：</label>
                    <input id="txtSenderID" />
                </li>
                <li>
                    <label>
                        接收人ID：</label>
                    <input id="txtReceiverID" />
                </li>
                <li>
                    <input type="button" onclick="javascript:search()" value="查 询" name="" style="float: left;" />
                </li>
            </ul>
        </div>
        <div id="ajaxTable">
        </div>
    </div>
    </form>
</body>
</html>
