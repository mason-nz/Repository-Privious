<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientLogRequireListNoLogin.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ClientLogRequire.ClientLogRequireListNoLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户端日志查询-研发</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="/Js/Enum/ShowEnum.js"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtBeginTime\')}', maxDate: '<%=DateTime.Today.ToString("yyyy-MM-dd") %>' }); });
            $("#txtBeginTime").val('<%=DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd") %>');
            $("#txtEndTime").val('<%=DateTime.Today.ToString("yyyy-MM-dd") %>');

            enterSearch(search);
            search();

            $("#EmpName").val("请输入姓名，分机号或工号");
            $("#EmpName").css("color", "#999");
        });
        //获取查询参数
        function parmas(page) {
            //取客服姓名
            var name = $.trim($("#EmpName").val());
            if (name == txttip) {
                name = "";
            }
            name = escape(name);
            //取时间
            var date_st = escape($.trim($("#txtBeginTime").val()));
            var date_et = escape($.trim($("#txtEndTime").val()));
            //取其他
            var Online = CheckboxValues("Online");
            var Vendor = CheckboxValues("Vendor");
            var NoLogin = "<%=NoLogin %>";

            if (date_st == "") {
                $.jAlert("查询起始日期不允许为空", function () { document.getElementById("txtBeginTime").focus(); });
                return "error";
            }
            if (date_et == "") {
                $.jAlert("查询终止日期不允许为空", function () { document.getElementById("txtEndTime").focus(); });
                return "error";
            }
            if (page == null) {
                page = 1;
            }
            return "date_st=" + date_st +
            "&date_et=" + date_et +
            "&name=" + name +
            "&online=" + Online +
            "&vendor=" + Vendor +
            "&nologin=" + NoLogin +
            "&page=" + page +
            "&r=" + Math.random();
        }
        //查询
        function search(page) {
            var pody = parmas(page);
            if (pody == "error") {
                return;
            }
            LoadingAnimation('bit_table');
            $('#ajaxTable').load('/ClientLogRequire/Ajax/AjaxList.aspx', pody);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/ClientLogRequire/Ajax/AjaxList.aspx?r=" + Math.random(), pody, null);
        }
        //多选框
        function CheckboxValues(name) {
            var Hasaudio = "";
            $("input[name='" + name + "']:checked").each(function (i, n) {
                Hasaudio += n.value + ",";
            });
            if (Hasaudio.substr(Hasaudio.length - 1, 1) == ',') {
                Hasaudio = Hasaudio.substr(0, Hasaudio.length - 1);
            }
            return Hasaudio;
        }
        //文本框提示
        var txttip = "请输入姓名，分机号或工号";
        function EmpNameOnFocus() {
            var value = $.trim($("#EmpName").val());
            if (value == txttip) {
                $("#EmpName").val("");
                $("#EmpName").css("color", "#666");
            }
        }
        function EmpNameOnBlur() {
            var value = $.trim($("#EmpName").val());
            if (value == "") {
                $("#EmpName").val(txttip);
                $("#EmpName").css("color", "#999");
            }
        }        
    </script>
    <div class="left" id="content" style="margin: 10px;">
        <div class="rC left">
            <div class="content">
                <h2>
                </h2>
                <div class="search" id="SearchCon">
                    <ul>
                        <li>
                            <label>
                                查询日期：
                            </label>
                            <input type="text" id="txtBeginTime" class="w95" name="BeginTime" />
                            -
                            <input type="text" id="txtEndTime" class="w95" name="txtEndTime" />
                        </li>
                        <li>
                            <label>
                                客服 ：
                            </label>
                            <input type="text" name="EmpName" id="EmpName" class="w190" onfocus="EmpNameOnFocus();"
                                onblur="EmpNameOnBlur();" />
                        </li>
                        <li>
                            <label>
                                厂家版本：
                            </label>
                            <span>
                                <input type="radio" name="Vendor" value="0" /><em onclick="emChkIsChoose(this);">Genesys</em></span>
                            <span>
                                <input type="radio" name="Vendor" value="1" checked="checked" /><em onclick="emChkIsChoose(this);">Holly</em></span>
                        </li>
                    </ul>
                    <ul>
                        <li>
                            <label>
                                在线状态：
                            </label>
                            <span>
                                <input type="checkbox" name="Online" value="0" /><em onclick="emChkIsChoose(this);">在线</em></span>
                            <span>
                                <input type="checkbox" name="Online" value="1" /><em onclick="emChkIsChoose(this);">离线</em></span>
                        </li>
                        <li style="width:600px;"></li>
                        <li class="btnsearch">
                            <div style="float: right">
                                <input type="button" name="Search" value="查询" onclick="search()" style="margin-left: 30px;" />
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="optionBtn clearfix" style="clear:both">
                    <input name="" type="button" value="批量请求" onclick="RequireLogMutil();" class="newBtn" />
                </div>
                <div id="ajaxTable">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
