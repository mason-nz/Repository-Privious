<%@ Page Title="呼出报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="CallPhoneReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.CallPhoneReport" %>

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
        $(document).ready(function () {
            getUserGroup();
            BindBeginEndtime();
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d', dateFmt: 'yyyy-MM-dd', maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d', dateFmt: 'yyyy-MM-dd', minDate: '#F{$dp.$D(\'tfBeginTime\')}', maxDate: '%y-%M-%d' }); }); //maxDate: '%y-%M-#{%d-1}'
            //敲回车键执行方法
            enterSearch(search);            
            $("#hidCallStatus").val("2");            
            search();
        });
        //根据不同的点击绑定不同的信息页面
        function loadHtml(n, othis) {
            $('#hidSearchType').val(n);
            $("#aList4,#aList3,#aList2,#aList1").removeClass("redColor");
            $("#aList" + n).addClass("redColor");
            // $(othis).addClass("redColor").siblings().removeClass("redColor");
            search();
        }
        function showDataList() {
            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            if (beginTime == endTime) {
                $("#aList4").show();
                $("#showHour_notuse").hide();
            }
            else {
                $("#aList4").hide();
                $("#showHour_notuse").show();
                if ($("#hidSearchType").val()=='4') {
                    $('#hidSearchType').val(3); //小时不可点击时  默认天
                    $("#aList4,#aList3,#aList2,#aList1").removeClass("redColor");
                    $("#aList3").addClass("redColor");
                }
              
            }
        }

        //加载分组
        function getUserGroup() {
            AjaxPost("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //选择客服
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: $("#<%=selGroup.ClientID %>").val() },
                url: "../../AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                }
            });
        }

        //绑定当天时间
        function BindBeginEndtime() {
            var initDate = getToday();
            $("#tfBeginTime").val(initDate);
            $("#tfEndTime").val(initDate);
        }

        //绑定上周开始结束时间
        function BindBeginEndtime2() {
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
            $("#tfBeginTime").val(strlastMonday);

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
            $("#tfEndTime").val(strlastSunday);
        }

        function Export() {
            // if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            if (beginTime == "" || endTime == "") {
                $.jAlert("统计日期不能为空!");
                return;
            }

            if (beginTime != endTime && endTime == getToday()) {

                $.jAlert("历史数据截止日期必须为当前日期的前一日!");
                $("#tfEndTime").val(getYesterToday());
                //showDataList();
            }

            var pody = _params();

            $("#formExport [name='BeginTime']").val(pody.BeginTime);
            $("#formExport [name='EndTime']").val(pody.EndTime);
            $("#formExport [name='AgentGroup']").val(pody.AgentGroup);
            $("#formExport [name='CallStatus']").val(pody.CallStatus); //呼入还是呼出
            $("#formExport [name='Browser']").val(GetBrowserName());

            $("#formExport [name='QueryType']").val(pody.QueryType);
            $("#formExport [name='AgentNum']").val(pody.AgentNum);
            $("#formExport [name='QueryType']").val(pody.QueryType);
            $("#formExport [name='AgentUserID']").val(pody.AgentUserID);

            $("#formExport").submit();
            //  }
        }

        function getToday() {
            return '<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>';
        }


        function getYesterToday() {
            return '<%=System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")%>';
        }
        function search() {
            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            showDataList();
            if (beginTime == "" || endTime == "") {
                $.jAlert("统计日期不能为空!");
                return;
            }

            if (beginTime != endTime && endTime >= getToday()) {

                $.jAlert("历史数据截止日期为当前日期的前一日!");
                $("#tfEndTime").val(getYesterToday());              
            }
            // if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $("#ajaxTable").load("/AjaxServers/TrafficManage/CallPhoneReportList.aspx", podyStr, function () {
                pody.Action = "CallPhoneReportTotalNew";
                //查询合计数据
                AjaxPost('/AjaxServers/TrafficManage/CallPhoneReport.ashx', pody, null, success);
                function success(data) {
                    var mbi = $.evalJSON(data);
                    $("#OutCallCount").text(mbi.N_Total);
                    $("#TalksCount").text(mbi.N_ETotal);
                    $("#JTRate").text(mbi.p_jietonglv);
                    $("#TalkTime").text(mbi.T_Talk);
                    $("#RingTime").text(mbi.T_Ringing);
                    $("#LoginOnTime").text(mbi.T_SignIn);
                    $("#WorkEfficiency").text(mbi.p_gongshilv);
                    $("#AGTalkTime").text(mbi.t_pjtime);
                    $("#AGRingTime").text(mbi.t_pjring);
                }

                StatAjaxPageTime(monitorPageTime, "/AjaxServers/TrafficManage/CallPhoneReportList.aspx?" + podyStr);
            });
            //}
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/TrafficManage/CallPhoneReportList.aspx", pody, function () {
                var pody1 = _params();
                pody1.Action = "CallPhoneReportTotalNew";
                //查询合计数据                
                AjaxPost('/AjaxServers/TrafficManage/CallPhoneReport.ashx', pody1, null, success);
                function success(data) {
                    var mbi = $.evalJSON(data);
                    $("#OutCallCount").text(mbi.N_Total);
                    $("#TalksCount").text(mbi.N_ETotal);
                    $("#JTRate").text(mbi.p_jietonglv);
                    $("#TalkTime").text(mbi.T_Talk);
                    $("#RingTime").text(mbi.T_Ringing);
                    $("#LoginOnTime").text(mbi.T_SignIn);
                    $("#WorkEfficiency").text(mbi.p_gongshilv);
                    $("#AGTalkTime").text(mbi.t_pjtime);
                    $("#AGRingTime").text(mbi.t_pjring);
                }
            });
        }

        //获取参数
        function _params() {
            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            var agentGroup = encodeURIComponent($.trim($("#<%=selGroup.ClientID %>").val()));

            var callStatus = encodeURIComponent($.trim($("#hidCallStatus").val()));
            //客服
            var agent = $("#hidSelOperId").val();
            //工号
            var ptid = $.trim($("#txtAgentNum").val());

            var pody = {
                BeginTime: beginTime,       //统计日期（前一个）
                EndTime: endTime,           //统计日期（后一个）            
                AgentGroup: agentGroup,     //坐席组
                AgentNum: ptid,             //工号
                AgentUserID: agent,         //客服
                CallStatus: callStatus,     //电话状态（1-呼入；2-呼出；默认1）
                QueryType: $("#hidSearchType").val(),
                Action: "", //查询合计数据
                r: Math.random()            //随机数
            }

            return pody;
        }
   
    </script>
    <form id="form1" runat="server">
    <input type="hidden" id="hidCallStatus" value="1" />
    <input type="hidden" id="hidSearchType" value="3" />
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客服：</label>
                <input type="text" id="txtSelOper" class="w200" readonly="true" onclick="SelectVisitPerson('','txtSelOper','hidSelOperId')" />
                <input type="hidden" id="hidSelOperId" value="-2" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtAgentNum" class="w200" />
            </li>
            <li>
                <label>
                    统计日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" vtype="isDate" vmsg="开始时间格式不正确"
                    class="w120" />-<input type="text" name="EndTime" id="tfEndTime" class="w120" vtype="isDate"
                        vmsg="结束时间格式不正确" />
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" runat="server" class="w200" style="width: 206px">
                </select>
            </li>
            <li class="btnsearch">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <span id="showHour_notuse" class="a_gray" style="display: none; margin-right: -4px;">
            小时</span> <a href="javascript:void(0)" onclick="javascript:loadHtml(4,this)" id="aList4">
                小时</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="javascript:loadHtml(3,this)"
                    id="aList3" class="redColor">日</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)"
                        onclick="javascript:loadHtml(2,this)" id="aList2">周</a>&nbsp;&nbsp;||&nbsp;&nbsp;
        <a href="javascript:void(0)" onclick="javascript:loadHtml(1,this)" id="aList1">月</a>
        <%if (IsExport)
          { %>
        <input name="" type="button" value="导出" onclick="Export()" class="newBtn" style='*margin-top: -30px;' />
        <%} %>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="/AjaxServers/TrafficManage/CallPhoneReportExport.aspx"
    method="post">
    <input type="hidden" id="BeginTime" name="BeginTime" value="" />
    <input type="hidden" id="EndTime" name="EndTime" value="" />
    <input type="hidden" id="AgentGroup" name="AgentGroup" value="" />
    <input type="hidden" id="CallStatus" name="CallStatus" value="" />
    <input type="hidden" id="QueryType" name="QueryType" value="" />
    <input type="hidden" id="AgentNum" name="AgentNum" value="" />
    <input type="hidden" id="AgentUserID" name="AgentUserID" value="" />
    </form>
</asp:Content>
