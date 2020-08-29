<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskList.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Title="其他任务" Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.OtherTaskList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
        });
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul style="width: 98%;">
            <li>
                <label>
                    任务ID：</label>
                <input type="text" id="txtPTID" class="w200" />
            </li>
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w200"
                    style="width: 206px">
                </select>
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selCategory" class="w200" style="width: 206px">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtProjectName" class="w200" />
            </li>
            <li>
                <label>
                    操作人：</label>
                <input type="text" id="txtSelOper" class="w200" readonly="readonly" onclick="SelectVisitPerson('operuser','txtSelOper','hidSelOperId')" />
                <input type="hidden" id="hidSelOperId" value="-1" />
            </li>
            <li>
                <label>
                    创建人：</label>
                <input type="text" id="txtSelCreater" class="w200" readonly="readonly" onclick="SelectVisitPerson('createuser','txtSelCreater','hidSelCreaterId')" />
                <input type="hidden" id="hidSelCreaterId" value="-1" />
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    操作时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" value='<%=DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") %>'
                    class="w95" />-<input type="text" name="EndTime" id="tfEndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                        class="w96" />
            </li>
            <li>
                <label>
                    状&nbsp;&nbsp;&nbsp;态：</label>
                <select id="selStatus" class="w200" style="width: 206px">
                    <option value="-1">请选择</option>
                    <option value="1">未分配</option>
                    <option value="2">未处理</option>
                    <option value="3">处理中</option>
                    <option value="4">已处理</option>
                    <option value="5">已结束</option>
                </select>
            </li>
            <li>
                <label>
                    所属坐席：</label>
                <input type="text" id="txtSelAgent" class="w200" readonly="readonly" onclick="SelectVisitPerson('employee','txtSelAgent','hidSelAgentId')" />
                <input type="hidden" id="hidSelAgentId" value="-2" />
            </li>
            <li class="btnsearch" style="clear: none; width: 120px;">
                <input class="cx" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_gameover)
          { %>
        <input type="button" id="btnOver" value="结束" class="newBtn" onclick="Stop()" />
        <%} %>
        <%if (right_withdraw)
          { %>
        <input type="button" id="btnWithDraw" value="收回" class="newBtn" onclick="RecedeCrmTask()" />
        <%} %>
        <%if (right_allocation)
          { %>
        <input type="button" id="btnAllocation" value="分配" class="newBtn" onclick="AssignCheck()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <input type="hidden" id="hidSelectUserid" value="" />
    <input type="hidden" id="hidSelectPageSize" value="" />
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#<%=selGroup.ClientID %>").val() != "-2") {
                AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selGroup.ClientID %>").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }

        //查询
        function search() {
            if (CheckForSelectDate("tfBeginTime", "tfEndTime")) {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxTable");
                //*添加监控
                var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
                $("#ajaxTable").load("/AjaxServers/OtherTask/OtherTaskList.aspx", podyStr, function () {
                    StatAjaxPageTime(monitorPageTime, "/AjaxServers/OtherTask/OtherTaskList.aspx?" + podyStr);
                });
            }
        }
        function CheckForSelectDate(beginid, endid) {
            var begintime = $.trim($("#" + beginid).val());
            var endtime = $.trim($("#" + endid).val());

            if (!begintime || begintime == "" || begintime == undefined) {
                $.jAlert("起始操作时间不能为空", function () { $("#" + beginid).focus(); });
                return false;
            }
            else if (!endtime || endtime == "" || endtime == undefined) {
                $.jAlert("终止操作时间不能为空", function () { $("#" + endid).focus(); });
                return false;
            }
            else {
                var newEndTime = new Date(endtime).getTime();
                var newBeginTime = new Date(begintime).getTime();
                var numDays = (newEndTime - newBeginTime) / 24 / 60 / 60 / 1000;
                if (numDays > 90) {
                    $.jAlert("操作时间最大跨度不能超过90天", function () { $("#" + beginid).focus(); });
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        //获取参数
        function _params() {

            var ptid = encodeURIComponent($.trim($("#txtPTID").val()));

            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));

            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            if ((beginTime != "" && !beginTime.isDate()) || (endTime != "" && !endTime.isDate())) {
                $.jAlert("输入的时间格式不正确");
                return false;
            }

            var name = encodeURIComponent($.trim($("#txtProjectName").val()));

            var status = $("#selStatus").val();

            var group = "";
            if ($("#<%=selGroup.ClientID %>").val() != "-1" && $("#<%=selGroup.ClientID %>").val() != undefined) {
                group = $("#<%=selGroup.ClientID %>").val();
            }
            var category = "";
            if ($("#selCategory").val() != "-1" && $("#selCategory").val()) {
                category = $("#selCategory").val();
            }

            var creater = "";
            var createrVal = $("#hidSelCreaterId").val();
            if (createrVal != "-1" && createrVal != "") {
                creater = createrVal;
            }

            var oper = "";
            var operVal = $("#hidSelOperId").val();
            if (operVal != "-1" && operVal != "") {
                oper = operVal;
            }

            var agent = "";
            agent = $("#hidSelAgentId").val();
            var custID = "";
            var custName = "";

            var pageSize = $("#hidSelectPageSize").val();

            var pody = {
                PTID: ptid,
                projectName: name,
                status: status,
                group: group,
                category: category,
                creater: creater,
                oper: oper,
                beginTime: beginTime,
                endTime: endTime,
                Agent: agent,
                pageSize: pageSize,
                CustID: custID,
                CustName: custName,
                r: Math.random()
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('/AjaxServers/OtherTask/OtherTaskList.aspx', pody + "&r=" + Math.random());
        }

        //点击“处理”触发
        function ProcessClick(PTID) {
            try {
                var url = '/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/OtherTask/OtherTaskDeal.aspx?OtherTaskID=' + PTID + '&r=' + Math.random());
                window.external.MethodScript(url);
            }
            catch (e) {
                window.open("/OtherTask/OtherTaskDeal.aspx?OtherTaskID=" + PTID + '&r=' + Math.random(), "OtherTaskDealWindow");
            }
        }
    </script>
    <script type="text/javascript">
        //批量分配任务
        function AssignCheck() {
            var taskIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
            AssignTask(taskIDs);
        }

        //单个分配任务
        function AssignTask(TaskIDS) {
            if (Len(TaskIDS) > 0) {
                var array_BGIDs = new Array();
                var bgids = $(":checkbox[name='chkSelect'][checked=true]").map(function () {
                    array_BGIDs[array_BGIDs.length] = $(this).attr("bgid");
                });
                //去重
                for (var i = 0; i < array_BGIDs.length; i++) {
                    for (var k = i + 1; k < array_BGIDs.length; k++) {
                        if (array_BGIDs[i] === array_BGIDs[k]) {
                            array_BGIDs.splice(i, 1);
                            i--;
                        }
                    }
                }
                $.openPopupLayer({
                    name: "AssignTaskAjaxPopupForSelect",
                    parameters: { TaskIDS: escape(TaskIDS), BGIDs: array_BGIDs.join(',') },
                    url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                    success: function () {
                        //分配任务，隐藏控件
                        $("#popAClear").hide();
                    },
                    beforeClose: function (e) {
                        if (e) {
                            $("#hidSelectUserid").val($("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid"));
                            var userid = $("#hidSelectUserid").val();
                            //分配任务
                            $.post("/AjaxServers/OtherTask/AssignTask.ashx", { Action: "AssignTask", TaskIDS: encodeURIComponent(TaskIDS), AssignUserID: userid }, function (data) {
                                if (data == "success") {
                                    $.jPopMsgLayer("分配任务成功", function () {
                                        search();
                                    });
                                }
                                else {
                                    $.jAlert(data);
                                }
                            });
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(search);
                    }

                });
            }
            else {
                $.jAlert("请至少选择一个要分配的任务！");
            }
        }
        //回收任务
        function RecedeCrmTask() {

            var taskIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
            RecedeCrmOne(taskIDs);
        }
        ///回收一个任务
        function RecedeCrmOne(TaskIDS) {
            if (Len(TaskIDS) > 0) {
                $.jConfirm("确定要回收所选择的的任务吗？", function (r) {
                    if (r) {
                        //回收任务
                        $.post("/AjaxServers/OtherTask/AssignTask.ashx", { Action: "RecedeTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                            if (data == "success") {
                                //                                $.jAlert("收回任务成功", function () {
                                //                                    search();
                                //                                });
                                $.jPopMsgLayer("收回任务成功", function () {
                                    search();
                                });

                            }
                            else {
                                $.jAlert(data);
                            }
                        });
                    }
                });
            }
            else {
                $.jAlert("请至少选择一个要回收的任务！");
            }
        }
        function Stop() {
            var taskIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
            StopTaskOne(taskIDs);
        }
        ///结束任务
        function StopTaskOne(TaskIDS) {
            if (Len(TaskIDS) > 0) {
                $.jConfirm("确定要结束所选择的的任务吗？", function (r) {
                    if (r) {
                        //结束任务
                        $.post("/AjaxServers/OtherTask/AssignTask.ashx", { Action: "StopTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                            if (data == "success") {
                                //                                $.jAlert("结束任务成功", function () {
                                //                                    search();
                                //                                });
                                $.jPopMsgLayer("结束任务成功", function () {
                                    search();
                                });

                            }
                            else {
                                $.jAlert(data);
                            }
                        });
                    }
                });
            }
            else {
                $.jAlert("请至少选择一个要结束的任务！");
            }
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            $('#selStatus').val('2'); //访问页面时，设置默认条件"状态"
            search();
        }); 

    </script>
    <style type="text/css">
        .pageP
        {
            width: 200px;
            float: left;
            text-align: left;
            padding-left: 20px;
        }
        
        .pageP a.selectA
        {
            color: Red;
        }
        .pageP a
        {
            height: 50px;
        }
        .pageP a:hover
        {
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">
        //选择操作人
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: $("#<%=selGroup.ClientID %>").val() },
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                    ;
                }
            });
        }
       
    </script>
</asp:Content>
