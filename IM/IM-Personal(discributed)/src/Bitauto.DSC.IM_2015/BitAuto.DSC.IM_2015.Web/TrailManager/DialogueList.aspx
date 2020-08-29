<%@ Page Language="C#" Title="对话统计" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="DialogueList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.DialogueList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });


            var nowDate = GetShortNowDate();
            $('#txtStartTime').val(nowDate);
            $('#txtEndTime').val(nowDate);
            search(1);

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
            $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/DialogueList.aspx", podyStr, function () { });
        }

        //查询
        var type = 1;
        function search(selectType) {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                if (selectType) {
                    type = selectType;
                }
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/DialogueList.aspx", podyStr);
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
            window.location = "/AjaxServers/TrailManager/DialogueList.aspx?export=1&" + podyStr;
        }


        function Change(selectType) {
            $("a[name='selectType']").each(function () {
                $(this).removeClass("cur");
            });
            $("a[name='selectType']").eq(selectType - 1).addClass("cur");
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

    </script>
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
                        <input type="button" value="查询" onclick="search()" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div class="cxList cxList_chart" style="margin-top: 8px; height: auto;">
            <div class="table_bt">
                <div class="time_xz">
                    <a href="javascript:Change(1)" class="cur" name="selectType">日</a> || <a href="javascript:Change(2)"
                        name="selectType">周</a> || <a href="javascript:Change(3)" name="selectType">月</a><span
                            class="btn right" style="margin-top: 2px; *margin-top: -28px;">
                            <input type="button" value="导出" onclick="ExportData()" class="w60 gray" /></span></div>
            </div>
            <div id="ajaxMessageInfo">
            </div>
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
