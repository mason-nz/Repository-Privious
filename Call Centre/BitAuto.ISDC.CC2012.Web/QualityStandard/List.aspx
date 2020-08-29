<%@ Page Title="录音质检" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityScoring.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='../Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        $(document).ready(function () {
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d 23:59:59', dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
            $('#txtScoreBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtScoreEndTime\')}', onpicked: function () { document.getElementById("txtScoreEndTime").focus(); } }); });
            $('#txtScoreEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtScoreBeginTime\')}' }); });

            var isXiAn = false;
            isXiAn = '<%= isXiAn %>';
            if (isXiAn == 'True') {
                $("#xaScore").show();
            }
            else {
                $("#bjScore").show();
            }
        });

        //绑定前三月时间
        function BindBeginEndtime() {
            $("#tfBeginTime").val('<%=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") %>');
            $("#tfEndTime").val('<%=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") %>');
        }
        //评分状态联动申诉状态
        function QSResultStatusChanged() {
            var QSResultStatus = $.trim($("#QSResultStatus").val());
            if (QSResultStatus == 20006) {
                $("#liResultStatus6").css("display", "block");
            }
            else {
                $("#liResultStatus6").css("display", "none");
            }
        }
    </script>
    <script type="text/javascript">
        //点击已申诉 显示 申诉成功或失败的复选框
        function showResultStatus() {
            if ($("#QSResultStatus6").is(":checked")) {
                $("#liResultStatus6").show();
            }
            else {
                $("#chkResultSuccess").attr("checked", false);
                $("#chkResultFail").attr("checked", false);
                $("#liResultStatus6").hide();
            }
        }

        //导出
        function ExportExcel() {
            if ($("#selScoreTable").val() == "-1") {
                $.jAlert("请先选择评分表再进行导出！");
                return false;
            }
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
                $("#form1").submit();
            }
        }

        //判断是否有参数值带过来，如果有则附上（主要是统计页面带过来）
        function loadParams() {
            $("#txtAgent").val("<%=RequestAgent %>");
            if ($.trim($("#txtAgent").val()) != "") {
                $("#hdnUserID").val("<%=RequestUserID %>");
            }
            $("#txtScoreBeginTime").val('<%=DateToPara(RequestScoreBeginTime)%>');
            $("#txtScoreEndTime").val('<%=DateToPara(RequestScoreEndTime)%>');
            $("#tfBeginTime").val('<%=DateToPara(RequestRecordBeginTime)%>');
            $("#tfEndTime").val('<%=DateToPara(RequestRecordEndTime)%>');

            if ($("#tfBeginTime").val() == "" && $("#tfEndTime").val() == "") {
                BindBeginEndtime();
            }

            $("#selScoreCreater").val("<%=RequestScoreCreater %>");
            $("#selScoreTable").val("<%=RequestScoreTable %>");

            var stateStatus = "<%=RequestStateResult %>";
            if (stateStatus != "") {
                switch (stateStatus) {
                    case "1":
                        $("#liResultStatus6").show();
                        //$("#QSResultStatus6").attr("checked", true);
                        $("#chkResultSuccess").attr("checked", true);
                        break;
                    case "2":
                        $("#liResultStatus6").show();
                        //$("#QSResultStatus6").attr("checked", true);
                        $("#chkResultFail").attr("checked", true);
                        break;
                }
            }

            //评分表状态：如果是0，则代表查看申诉；需要加上的状态是待初审、待复审、已申诉
            //            var scoreStatus = "<%=RequestScoreStatus %>";
            //            switch (scoreStatus) {
            //                case "0": 
            //                    $("#QSResultStatus3").attr("checked", true);
            //                    $("#QSResultStatus4").attr("checked", true);
            //                    $("#QSResultStatus6").attr("checked", true);
            //                    break;
            //                case "1":
            //                    $("#QSResultStatus2").attr("checked", true);
            //                    $("#QSResultStatus3").attr("checked", true);
            //                    $("#QSResultStatus4").attr("checked", true);
            //                    $("#QSResultStatus5").attr("checked", true);
            //                    $("#QSResultStatus6").attr("checked", true);
            //                    break;
            //                default: "";
            //                    break;
            //            }

            $("#txtAgent").val("<%=RequestAgent %>");

            //成绩是否合格：1合格，其它都是不合格
            var IsQualified = '<%=RequestIsQualified %>';
            if (IsQualified == "1") {
                $("#chkQualifiedYes").attr("checked", true);
            }

            $("#selGroup").val("<%=RequestGroup%>");
        }

        //评分表
        function getScoreTableName() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getFieldName", TableName: "QS_RulesTable", IDField: "QS_RTID", ShowField: "Name", TableStatus: "10003", r: Math.random() }, null, function (data) {
                $("#selScoreTable").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selScoreTable").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //评分人
        function getCreater() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getCreater", GetCreaterType: "QS", TableName: "QS_Result", ShowField: "CreateUserID", TableStatus: "", r: Math.random() }, null, function (data) {
                $("#selScoreCreater").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selScoreCreater").append("<option value=" + jsonData[i].UserID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selGroup").append("<option value='-1'>请选业务分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        function GetCallAgentBGID() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selCallAgentBGID").append("<option value='-1'>请选坐席分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCallAgentBGID").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#selGroup").val() != "-1") {
                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selGroup").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }

        //点击“评分”触发
        function clickScore(RTID, QS_RID, CallID, tableEndName, scoreType) {
            //如果为0，则表示还没有评分表
            if (RTID == 0) {
                $.jAlert("该录音记录还没有对应的评分表，无法评分！请添加评分表", function () { return false });
                return false;
            }
            else {
                try {
                    var url = "";
                    if (scoreType == "3") {
                        url = '/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/QualityStandard/QualityResultManage/QualityResultFiveLevelEdit.aspx?QS_RTID=' + RTID + '&QS_RID=' + QS_RID + '&CallID=' + CallID + '&tableEndName=' + tableEndName);
                    }
                    else {
                        url = '/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/QualityStandard/QualityResultManage/QualityResultEdit.aspx?QS_RTID=' + RTID + '&QS_RID=' + QS_RID + '&CallID=' + CallID + '&tableEndName=' + tableEndName);
                    }
                    window.external.MethodScript(url);
                }
                catch (e) {
                    if (scoreType == "3") {
                        window.open("/QualityStandard/QualityResultManage/QualityResultFiveLevelEdit.aspx?QS_RTID=" + RTID + "&QS_RID=" + QS_RID + "&CallID=" + CallID + '&tableEndName=' + tableEndName);
                    }
                    else {
                        window.open("/QualityStandard/QualityResultManage/QualityResultEdit.aspx?QS_RTID=" + RTID + "&QS_RID=" + QS_RID + "&CallID=" + CallID + '&tableEndName=' + tableEndName);
                    }
                }
            }
        }

        //坐席弹出层
        function GetEmployeeAgent() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {

                    var name = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("name");
                    var userID = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid");
                    if (name == undefined) {
                        name = "";
                        userID = "";
                    }
                    $("#txtAgent").val(name);
                    $("#hdnUserID").val(userID);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        //查询
        function search() {
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
                showSearchList.getList("/AjaxServers/QualityStandard/List.aspx", "form1", "ajaxTable");
            }
        }

        $(function () {
            getUserGroup();
            GetCallAgentBGID();
            $("#hidBrowser").val(GetBrowserName());

            getScoreTableName();
            getCreater();
            loadParams();
            showResultStatus();
            //敲回车键执行方法
            enterSearch(search);
            selGroupChange();
            //search();
        });
    </script>
    <form id="form1" action="Export/ScoreDetailsExport.aspx">
    <div class="searchTj" style="width: 100%;">
        <ul style="width: 98%;">
            <li>
                <label>
                    主叫号码：</label>
                <input type="text" id="txtPhoneNum" class="w200" name="PhoneNum" vtype="isNum" vmsg="主叫号码格式不正确" />
            </li>
            <li>
                <label>
                    通话日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" vtype="isDateTime" vmsg="通话起始时间格式不正确"
                    class="w95" />-<input type="text" name="EndTime" id="tfEndTime" class="w95" vtype="isDateTime"
                        vmsg="通话结束时间格式不正确" />
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" class="w60" name="BGID"
                    style="width: 101px;">
                </select>
                <select id="selCategory" class="w60" name="SCID" style="width: 101px;">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li style="clear: both;">
                <label>
                    被叫号码：</label>
                <input type="text" id="txtANI" class="w200" name="ANI" vtype="isNum" vmsg="被叫号码格式不正确" />
            </li>
            <li>
                <label>
                    通话时长：</label>
                <input type="text" id="txtSpanTime1" class="w95" name="SpanTime1" vtype="isNum" vmsg="通话最小时长格式不正确" />-<input
                    type="text" id="txtSpanTime2" class="w95" name="SpanTime2" vtype="isNum" vmsg="通话最大时长格式不正确" />
            </li>
            <li>
                <label>
                    所属分组：</label>
                <select id="selCallAgentBGID" class="w200" style="width: 206px" name="CallAgentBGID">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li style="clear: both;">
                <label>
                    任&nbsp;务&nbsp;ID：</label>
                <input type="text" id="txtTaskID" class="w200" name="BusinessID" />
            </li>
            <li>
                <label>
                    评分日期：</label>
                <input type="text" name="ScoreBeginTime" id="txtScoreBeginTime" vtype="isDate" vmsg="评分起始时间格式不正确"
                    class="w95" />-<input type="text" name="ScoreEndTime" id="txtScoreEndTime" class="w95"
                        vtype="isDate" vmsg="评分结束时间格式不正确" />
            </li>
            <li>
                <label>
                    评&nbsp;分&nbsp;表：</label>
                <select id="selScoreTable" class="w200" style='width: 206px;' name="ScoreTable">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li style="clear: both;">
                <label>
                    话&nbsp;务&nbsp;ID：</label>
                <input type="text" id="txtCallRecID" class="w200" name="CallID" vtype="isNum" vmsg="话务ID格式不正确" />
            </li>
            <li>
                <label>
                    坐席：</label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtAgent" class="text02" readonly="readonly" />
                    <b onclick="GetEmployeeAgent()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="hdnUserID" name="CreateUserID" value="" />
                </div>
            </li>
            <li class="w350">
                <label>
                    评&nbsp;分&nbsp;人：</label>
                <select id="selScoreCreater" class="w200" style='width: 206px;' name="ScoreCreater">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    问题解决：</label>
                <select id="selSolve" class="w200" style='width: 206px;' name='SelSolve'>
                    <option value="-1">请选择</option>
                    <option value="0">未评价</option>
                    <option value="1">已解决</option>
                    <option value="2">未解决</option>
                </select>
            </li>
            <li>
                <label>
                    满意度：</label>
                <select id="selScore" class="w200" style='width: 206px;' name='IVRScore'>
                    <option value="-1">请选择</option>
                    <option value="0">未评价</option>
                    <option value="1">满意</option>
                    <option value="2">对处理结果不满意</option>
                    <option value="3">对客服代表服务不满意</option>
                </select>
            </li>
            <li>
                <label>
                    录音类型：</label>
                <span>
                    <label>
                        <input type="checkbox" value="1" id="chkCallIn" name="CallType" /><em>呼入</em></label>
                    <label>
                        <input type="checkbox" value="2" id="chkCallOut" name="CallType" /><em>呼出</em></label></span>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    评分状态：</label>
                <select id="QSResultStatus" class="w200" style='width: 206px;' name="QSResultStatus"
                    onchange="QSResultStatusChanged();">
                    <option value="-1">请选择</option>
                    <option value="20001">待评分</option>
                    <option value="20002">已提交</option>
                    <option value="20003">待初审</option>
                    <option value="20004">待复审</option>
                    <option value="20005">已评分</option>
                    <option value="20006">已申诉</option>
                </select>
            </li>
            <li><span style="display: none; margin-left: 20px;" id="liResultStatus6">
                <label>
                    申诉状态：
                </label>
                <span>
                    <label>
                        <input type="checkbox" value="1" id="chkResultSuccess" name="QSStateResult" />
                        <em onclick="emChkIsChoose(this)">成功</em>
                    </label>
                    <label>
                        <input type="checkbox" value="2" id="chkResultFail" name="QSStateResult" />
                        <em onclick="emChkIsChoose(this)">失败</em>
                    </label>
                </span></span></li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 290px;">
                <input class="cx" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <input type="hidden" id="hidBrowser" name="Browser" />
    </div>
    <div class="optionBtn clearfix">
        <%if (right_Export)
          {%>
        <input name="" type="button" value="导出" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
    </form>
</asp:Content>
