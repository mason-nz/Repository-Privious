<%@ Page Title="热线数据报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="HotLineReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CallReport.HotLineReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .a_red
        {
            color: #C00;
        }
        .a_gray
        {
            color: #666;
            cursor: default;
        }
    </style>
    <script type="text/javascript">
        //初始化方法
        $(document).ready(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });
            var s = GetToday();
            $('#txtStartTime').val(s);
            $('#txtEndTime').val(s);
            InitselBusinessType();
            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        //初始化选型
        function InitselBusinessType() {
            var str = TelNumManag.GetOptions_HotLine();
            $("#selBusinessType").html(str);
            //Holly的ID是Genesys的id+100
            //数据库和js存储的都是Genesys的id
            //强斐 2015-12-14
            for (var i = 0; i < $("#selBusinessType")[0].options.length; i++) {
                var option = $("#selBusinessType")[0].options[i];
                var value = parseInt(option.value);
                option.value = value + 100;
            }
        }
        //获取当前日期
        function GetToday() {
            var d = new Date();
            var vYear = d.getFullYear();
            var vMon = d.getMonth() + 1;
            var vDay = d.getDate();
            s = vYear + '-' + (vMon < 10 ? "0" + vMon : vMon) + '-' + (vDay < 10 ? "0" + vDay : vDay);
            return s;
        }
        //获取参数
        function GetParam() {
            var selBusinessType = $.trim($('#selBusinessType').val());
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var ShowTime = $.trim($("#ShowTime").val());

            if (txtStartTime != txtEndTime && ShowTime == "hour") {
                $("#ShowTime").val("day");
            }

            return "BusinessType=" + selBusinessType + "&StartTime=" + txtStartTime + "&EndTime=" + txtEndTime;
        }
        //查询按钮
        function search() {
            var url = GetParam();
            $('#SearchWhere').val(url);
            GetData(url);
        }
        //统计方式
        function ShowPart(type) {
            $("#ShowTime").val(type);
            var url = $.trim($('#SearchWhere').val());
            if (url != "") {
                GetData(url);
            }
        }
        //获取数据
        function GetData(url) {
            var ShowTime = $.trim($("#ShowTime").val());
            url += "&ShowTime=" + ShowTime + "&r=" + Math.random();
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('/AjaxServers/CallReport/HotLineReport.aspx?' + url, '', AfterSearch);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CallReport/HotLineReport.aspx", pody, AfterSearch);
        }
        //查询完成之后
        function AfterSearch() {
            var ShowTime = $.trim($("#ShowTime").val());
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var TotalCount = parseInt($.trim($('#TotalCount').val()));
            $("#showHour").removeClass("a_red");
            $("#showDay").removeClass("a_red");
            $("#showWeek").removeClass("a_red");
            $("#showMonth").removeClass("a_red");
            if (isNaN(TotalCount)) {
                //都不可用
                UseOrNotUse("showHour", false);
                UseOrNotUse("showDay", false);
                UseOrNotUse("showWeek", false);
                UseOrNotUse("showMonth", false);
            }
            else {
                if (txtStartTime == txtEndTime) {
                    //小时可用
                    UseOrNotUse("showHour", true);
                }
                else {
                    //小时不可用
                    UseOrNotUse("showHour", false);
                }
                //其他可用
                UseOrNotUse("showDay", true);
                UseOrNotUse("showWeek", true);
                UseOrNotUse("showMonth", true);
            }
            if (ShowTime == "hour") {
                $("#showHour").removeClass("a_red").addClass("a_red");
            }
            else if (ShowTime == "day") {
                $("#showDay").removeClass("a_red").addClass("a_red");
            }
            else if (ShowTime == "week") {
                $("#showWeek").removeClass("a_red").addClass("a_red");
            }
            else if (ShowTime == "month") {
                $("#showMonth").removeClass("a_red").addClass("a_red");
            }
        }
        //可用或不可用
        function UseOrNotUse(type, isuse) {
            if (isuse) {
                $('#' + type).css("display", "inline-block");
                $('#' + type + '_notuse').css("display", "none");
            }
            else {
                $('#' + type).css("display", "none");
                $('#' + type + '_notuse').css("display", "inline-block");
            }
        }
        //导出数据
        function ExportData() {
            var selBusinessType = $.trim($('#selBusinessType').val());
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());

            $("#formExport [name='BusinessType']").val(selBusinessType);
            $("#formExport [name='StartTime']").val(txtStartTime);
            $("#formExport [name='EndTime']").val(txtEndTime);

            $("#formExport").submit();
        }
    </script>
    <form id="form1" runat="server">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    业务类型：</label>
                <select id="selBusinessType" style="width: 128px">
                </select>
            </li>
            <li>
                <label>
                    统计日期：</label>
                <input type="text" class="w85" name="txtStartTime" id="txtStartTime" />
                <span style="padding-right: 3px;">至</span>
                <input type="text" class="w85" name="txtEndTime" id="txtEndTime" />
            </li>
            <li class="btnsearch" style="margin-top: 2px;">
                <input name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix">
        <div>
            <span style="float: left;"><span id="showHour_notuse" class="a_gray" style="display: none;
                margin-right: -4px;">小时</span> <a href="javascript:void(0)" onclick="ShowPart('hour')"
                    id="showHour">小时</a>&nbsp;&nbsp;||&nbsp;&nbsp; <span id="showDay_notuse" class="a_gray"
                        style="display: none; margin-right: -4px;">日</span> <a href="javascript:void(0)"
                            onclick="ShowPart('day')" id="showDay">日</a>&nbsp;&nbsp;||&nbsp;&nbsp;
                <span id="showWeek_notuse" class="a_gray" style="display: none; margin-right: -4px;">
                    周</span> <a href="javascript:void(0)" onclick="ShowPart('week')" id="showWeek">周</a>&nbsp;&nbsp;||&nbsp;&nbsp;
                <span id="showMonth_notuse" class="a_gray" style="display: none; margin-right: -4px;">
                    月</span> <a href="javascript:void(0)" onclick="ShowPart('month')" id="showMonth">月</a>
            </span>
            <%if (DataExportButton)
              {%>
            <input type="button" id="btnPutOut" value="导出" class="newBtn" style="*margin-top: 0px auto;"
                onclick="ExportData()" />
            <%} %>
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
    <!--列表结束-->
    </form>
    <form id="formExport" action="/AjaxServers/CallReport/HotLineReportExport.aspx" method="post">
    <input type="hidden" id="BusinessType" name="BusinessType" value="" />
    <input type="hidden" id="StartTime" name="StartTime" value="" />
    <input type="hidden" id="EndTime" name="EndTime" value="" />
    <input type="hidden" id="ShowTime" name="ShowTime" value="day" />
    <input type="hidden" id="SearchWhere" name="SearchWhere" value="" />
    </form>
</asp:Content>
