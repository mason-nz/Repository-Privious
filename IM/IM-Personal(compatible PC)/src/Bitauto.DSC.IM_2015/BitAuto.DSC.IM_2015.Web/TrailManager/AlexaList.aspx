<%@ Page Language="C#" Title="流量统计" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="AlexaList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.AlexaList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Scripts/HighCharts/highcharts.js" type="text/javascript"></script>
    <script src="../Scripts/HighCharts/exporting.js" type="text/javascript"></script>
    <script src="../Scripts/Chart.js" type="text/javascript"></script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li class="w600">
                    <label>
                        访客来源：</label>
                    <select class="w240 w220" id="ddSourceType" runat="server" clientidmode="Static"
                        onchange="SourceTypeChange(this.value)">
                        <option value='-1'>请选择</option>
                    </select>
                    -
                    <select class="w240 w220" id="ddSubSourceType" runat="server" clientidmode="Static"
                        disabled="disabled">
                        <option value='-1'>请选择</option>
                    </select>
                </li>
                <li>
                    <label>
                        日期：</label><input id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" value="查询" onclick="search(null,0)" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div class="cxList cxList_chart">
            <div class="table_bt">
                <div class="time_xz">
                    <a href="javascript:void(0)" class="cur" name="selectType" id="ahours">小时</a> ||
                    <a href="javascript:void(0)" name="selectType" id="aday">日</a> || <a href="javascript:void(0)"
                        name="selectType" id="aweek">周</a> || <a href="javascript:void(0)" name="selectType"
                            id="amonth">月</a></div>
            </div>
            <!--图表开始-->
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                </thead>
                <tr class="bgnone">
                    <td class="chart_dh">
                        <div id="divCharts">
                        </div>
                    </td>
                </tr>
            </table>
            <div id="ajaxMessageInfo">
            </div>
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <style type="text/css">
        .cxList_chart .time_xz a.disabled
        {
            color: #999;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); DimDisabled() } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', onpicked: function () { DimDisabled() } }); });


            var nowDate = GetShortNowDate();
            $('#txtStartTime').val(nowDate);
            $('#txtEndTime').val(nowDate);
            search(0);

            $("a[name='selectType']").each(function (index, item) {
                $(this).bind("click", function () { Change(index) });
            });

        });
        //当前日期
        function GetShortNowDate() {
            var data = new Date();
            var nowYear = data.getFullYear();
            var nowMonth = data.getMonth() + 1;
            var nowDay = data.getDate();
            var nowDate = nowYear + "-" + (nowMonth.toString().length == 1 ? ("0" + nowMonth) : nowMonth) + "-" + (nowDay.toString().length == 1 ? ("0" + nowDay) : nowDay)
            return nowDate;
        }


        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/AlexaList.aspx", podyStr, function () { });
        }

        //查询
        var type = 0;
        function search(selectType, searchType) {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                if (selectType != null) {
                    type = selectType;
                }
                if (type == 1) {
                    $("#aday").addClass("cur");
                }

                if (searchType == 0) {
                    var txtStartTime = $.trim($('#txtStartTime').val());
                    var txtEndTime = $.trim($('#txtEndTime').val());
                    var days = GetDateDiff(txtStartTime, txtEndTime);
                    if (days == 0) {//如果时间段为1天，默认选中“时”统计维度
                        $("a[name='selectType']").each(function () {
                            $(this).removeClass("cur");
                        });
                        $("#ahours").addClass("cur");
                        type = 0;
                    }
                }
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/AlexaList.aspx", podyStr);
                ShowLineChart();
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime == "" || endTime == "") {
                msg += "日期不可以为空<br/>";
            }

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }



            return msg;
        }

        //获取参数
        function _params() {
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var ddSourceType = $.trim($('#ddSourceType').val());
            var ddSubSourceType = $.trim($('#ddSubSourceType').val());
            if (ddSourceType == "100" && ddSubSourceType != -1) {
                ddSourceType = ddSubSourceType;
            }



            var pody = {
                Starttime: txtStartTime,
                EndTime: txtEndTime,
                SourceType: ddSourceType,
                SelectType: type,
                r: Math.random()  //随机数
            }

            return pody;
        }

        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/TrailManager/AlexaList.aspx?export=1&" + podyStr;
        }


        function Change(selectType) {
            $("a[name='selectType']").each(function () {
                $(this).removeClass("cur");
            });
            $("a[name='selectType']").eq(selectType).addClass("cur");
            search(selectType);
        }


        function SourceTypeChange(value) {
            if (value == "100") {
                $("#ddSubSourceType").removeAttr("disabled");
            }
            else {
                $("#ddSubSourceType").attr("disabled", "disabled");
                $("#ddSubSourceType").val("-1");
            }

        }

        function ShowLineChart() {
            AjaxPost("/AjaxServers/TrailManager/AlexaApi.ashx?action=ShowChart", _params(), null, function (result) {
                ShowChart("divCharts", "areaspline", result,type);
            });
        }



        //计算日期天数
        function GetDateDiff(startDate, endDate) {
            var startTime = new Date(Date.parse(startDate.replace(/-/g, "/"))).getTime();
            var endTime = new Date(Date.parse(endDate.replace(/-/g, "/"))).getTime();
            var dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);
            return dates;
        }


        function DimDisabled() {
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var days = GetDateDiff(txtStartTime, txtEndTime);
            if (days > 0 && type == 0) {
                type = 1;
                $("#ahours").addClass("disabled");

                $("#ahours").unbind("click");
            }
            else if (days == 0) {
                $("#ahours").removeClass("disabled");
                $("#ahours").removeClass("cur");
                $("#ahours").bind("click", function () { Change(0) });
            }
        }
    </script>
    <!--内容结束-->
</asp:Content>
