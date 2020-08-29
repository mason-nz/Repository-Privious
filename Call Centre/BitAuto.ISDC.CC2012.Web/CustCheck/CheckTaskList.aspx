<%@ Page Title="客户核实" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="CheckTaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.CheckTaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
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

        $(document).ready(function () {

            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            search();

            $("a[name='aAssign']").live("click", function (e) { e.preventDefault(); AssignTask($(this).attr("taskid")); });
            $("a[name='aReturn']").live("click", function (e) { e.preventDefault(); RecedeCrmOne($(this).attr("taskid")); });
            $("a[name='aStop']").live("click", function (e) { e.preventDefault(); StopTaskOne($(this).attr("taskid")); });

            //全选/全不选
            $("#ckbAllSelect").live("click", function () {
                $(":checkbox[name='chkSelect']").attr("checked", $(this).attr("checked"));
            });

            $("[id$='selStatus']").change(function () {

                changeStatus();
            });



            $("#radioNoCRMBrand").click(function () {
                if ($(this).attr("checked")) {
                    $("#txtCRMBrandNames").attr("disabled", "disabled");
                }
                else {
                    $("#txtCRMBrandNames").removeAttr("disabled");
                }
            });

            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });


            $('#txtLastOperStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtLastOperEndTime\')}', onpicked: function () { document.getElementById("txtLastOperEndTime").focus(); } }); });
            $('#txtLastOperEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtLastOperStartTime\')}' }); });

        });

        //选择状态
        function changeStatus() {
            var statusId = $("[id$='selStatus']").val();
            if (statusId == "180001" || statusId == "180099" || statusId == "180015") {
                $("#ulHidel").show();
            }
            else {
                $("#ulHidel").hide();
            }

            if (statusId == "180001") {
                //处理中d
                $("#divAdditionalStatus").show();
            }
            else {
                $("#divAdditionalStatus").hide();

            }
            if (statusId == "180099") {
                $("#divCheckStatus").show();
            }
            else {
                $("#divCheckStatus").hide();
            }
            //已完成
            if (statusId == "180015") {
                $("#divCheckStatus2").show();
            }
            else {
                $("#divCheckStatus2").hide();
            }
        }


        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
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
            if ($("#<%=selGroup.ClientID %>").val() != "-1") {
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
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CustCheck/CheckTaskList.aspx?v=" + Math.random(), podyStr);
        }



        //分页操作
        function ShowDataByPost2(pody) {

            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('/AjaxServers/CustCheck/CheckTaskList.aspx?v=' + Math.random(), pody);
        }

        //获取参数
        function _params() {

            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));

            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            if ((beginTime != "" && !beginTime.isDate()) || (endTime != "" && !endTime.isDate())) {
                $.jAlert("输入的时间格式不正确");
                return false;
            }

            var name = encodeURIComponent($.trim($("#txtName").val()));
            var custName = encodeURIComponent($.trim($("#txtCustName").val()));

            var status_s = $("#<%=selStatus.ClientID %>").val();

            var operstatus_s = "";
            if ($("#divCheckStatus").is(":visible")) {
                operstatus_s = $("#divCheckStatus :checkbox[name='checkStatus']:checked").map(function () {
                    return $(this).val()
                }).get().join(',');
            }
            else if ($("#divCheckStatus2").is(":visible")) {
                operstatus_s = $("#divCheckStatus2 :checkbox[name='checkStatus']:checked").map(function () {
                    return $(this).val()
                }).get().join(',');
            }


            var additionalStatus = "";

            if ($("#divAdditionalStatus").is(":visible")) {
                additionalStatus = $('input[name="AdditionalStatus"]:checked').map(function () { return $(this).val(); }).get().join(",");
            }

            var group = "";
            if ($("#<%=selGroup.ClientID %>").val() != "-1" && $("#<%=selGroup.ClientID %>").val() != undefined) {
                group = $("#<%=selGroup.ClientID %>").val();
            }
            var category = "";
            if ($("#selCategory").val() != "-1" && $("#selCategory").val() != "") {
                category = $("#selCategory").val();
            }

            var creater = "";
            if ($("#<%=selCreater.ClientID %>").val() != "-1" && $("#<%=selCreater.ClientID %>").val() != "") {
                creater = $("#<%=selCreater.ClientID %>").val();
            }

            var optUserId = "";
            if ($("#<%=selOptUserId.ClientID %>").val() != "-1" && $("#<%=selOptUserId.ClientID %>").val() != "") {
                optUserId = $("#<%=selOptUserId.ClientID %>").val();
            }

            var selUserId = "";
            if ($("#<%=selUserId.ClientID %>").val() != "-1" && $("#<%=selUserId.ClientID %>").val() != "") {
                selUserId = $("#<%=selUserId.ClientID %>").val();
            }
            var pagesize = $("#hidSelectPageSize").val();

            var CRMBrandIDs = $("#txtCRMBrandIDs").val(); //品牌IDs
            var NoCRMBrand = "0"; //空品牌
            if ($("#radioNoCRMBrand").attr("checked")) {
                //选中就是1
                NoCRMBrand = "1";
            }

            var TaskID = $("#txtTaskID").val();

            var CustID = $("#txtCustID").val();
            var custType = $("#<%=sltCustType.ClientID %>").val();

            var lastOperStartTime = $("#txtLastOperStartTime").val();
            var lastOperEndTime = $("#txtLastOperEndTime").val();

            var pody = {
                name: name,
                custName: custName,
                status_s: status_s,
                operstatus_s: operstatus_s,
                AdditionalStatus: additionalStatus,
                group: group,
                category: category,
                creater: creater,
                selUserId: selUserId,
                optUserId: optUserId,
                beginTime: beginTime,
                endTime: endTime,
                pagesize: pagesize,
                CRMBrandIDs: CRMBrandIDs,
                NoCRMBrand: NoCRMBrand,
                TaskID: TaskID,
                CustID: CustID,
                CustType: custType,
                LastOperStartTime: lastOperStartTime,
                LastOperEndTime: lastOperEndTime,
                r: Math.random()
            }

            //为导出赋值
            $("#formExport input[name='name']").val(name);
            $("#formExport input[name='custName']").val(custName);
            $("#formExport input[name='status_s']").val(status_s);
            $("#formExport input[name='operstatus_s']").val(operstatus_s);
            $("#formExport input[name='group']").val(group);
            $("#formExport input[name='category']").val(category);
            $("#formExport input[name='creater']").val(creater);
            $("#formExport input[name='selUserId']").val(selUserId);
            $("#formExport input[name='beginTime']").val(beginTime);
            $("#formExport input[name='endTime']").val(endTime);
            $("#formExport input[name='AdditionalStatus']").val(additionalStatus);
            $("#formExport input[name='optUserId']").val(optUserId);
            $("#formExport input[name='CRMBrandIDs']").val(CRMBrandIDs);
            $("#formExport input[name='NoCRMBrand']").val(NoCRMBrand);
            $("#formExport input[name='TaskID']").val(TaskID);
            $("#formExport input[name='CustID']").val(CustID);

            return pody;
        }
    </script>
    <script type="text/javascript">

        function clearCheck() {
            $('input[name="AdditionalStatus"]:checked').each(function () {
                $(this).attr("checked", false);
            })
        }

        //批量分配任务
        function AssignCheck() {
            var taskIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");

            AssignTask(taskIDs);
        }

        //单个分配任务
        function AssignTask(TaskIDS) {

            if (Len(TaskIDS) > 0) {
                $.openPopupLayer({
                    name: "AssignTaskAjaxPopupForSelect",
                    parameters: { TaskIDS: escape(TaskIDS) },
                    url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                    beforeClose: function (e) {
                        if (e) {
                            $("#hidSelectUserid").val($("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid"));
                            var userid = $("#hidSelectUserid").val();
                            //分配任务
                            $.post("/AjaxServers/CustCheck/AssignTask.ashx", { Action: "AssignTask", TaskIDS: encodeURIComponent(TaskIDS), AssignUserID: userid }, function (data) {
                                if (data == "success") {

                                    $.jAlert("分配任务成功", function () {
                                        search();
                                    });
                                }
                                else {
                                    alert(data);
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
                        $.post("/AjaxServers/CustCheck/AssignTask.ashx", { Action: "RecedeTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                            if (data == "success") {
                                $.jAlert("收回任务成功", function () {
                                    search();
                                });
                            }

                            else {
                                alert(data);
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
                        $.post("/AjaxServers/CustCheck/AssignTask.ashx", { Action: "StopTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                            if (data == "success") {

                                $.jAlert("结束任务成功", function () {
                                    search();
                                });
                            }

                            else {
                                alert(data);
                            }
                        });
                    }
                });

            }
            else {
                $.jAlert("请至少选择一个要结束的任务！");
            }
        }

        //导出任务
        function ExportTask() {

            var taskIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");

            $("#formExport [name='SelectPTIDs']").val(taskIDs);
            $("#formExport [name='Browser']").val(GetBrowserName());
            $("#formExport").submit();

        }
        //选择主营品牌
        function OpenSelectBrandPopup() {
            $.openPopupLayer({
                name: "BrandSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/CustCategory/SelectBrand.aspx?BrandIDs=" + $('#txtCRMBrandIDs').val(),
                beforeClose: function (e, data) {
                    if (e) {
                        var brandids = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandids');
                        var brandnames = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandnames');
                        $('#txtCRMBrandIDs').val(brandids);
                        $('#txtCRMBrandNames').val(brandnames);
                    }
                }
            });
        };
        function OpenAuditTaskPopup() {
            //验证是否选中任务
            var selectSize = $("input[name='chkSelect']:checked").size();
            if (selectSize == 0) {
                $.jAlert("至少选择一条任务！");
                return;
            }
            var selectPTID = "";
            var isRight = true;
            //验证是否选中的任务都是“待审核”状态
            $("input[name='chkSelect']:checked").each(function () {
                var taskStatusStr = $.trim($("td[name='tdTaskStatus']", $(this).parent().parent()).text());
                if (taskStatusStr != "待审核") {
                    $.jAlert("选中任务不符合审核条件！");
                    isRight = false;
                    return false;
                }
                else {
                    if (selectPTID != "") {
                        selectPTID += ",";
                    }
                    selectPTID += $(this).val();
                }
            });
            if (isRight) {
                $.openPopupLayer({
                    name: "AuditTaskPopup",
                    parameters: { PTIDS: selectPTID },
                    url: "/AjaxServers/CustCheck/TaskAuditPopupLayer.aspx",
                    beforeClose: function (e, data) {
                        if (e) {
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    所属项目：</label>
                <input type="text" id="txtName" class="w190" style="width: 188px; *width: 183px;
                    width: 183px\9;" />
            </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreater" runat="server" class="w125" style="width: 191px; *width: 188px;
                    width: 187px\9;">
                </select>
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w90"
                    style="width: 102px; *width: 98px; width: 98px\9">
                </select>
                <select id="selCategory" class="w90">
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" class="w190" style="width: 188px; *width: 183px;
                    width: 183px\9;" />
            </li>
            <li>
                <label>
                    坐席：</label>
                <select id="selUserId" runat="server" class="w125" style="width: 191px; *width: 188px;
                    width: 187px\9;">
                </select>
            </li>
            <li>
                <label>
                    操作时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户ID：</label>
                <input type="text" id="txtCustID" class="w190" style="width: 188px; *width: 183px;
                    width: 183px\9;" />
            </li>
            <li>
                <label>
                    操作人：</label>
                <select id="selOptUserId" runat="server" class="w90" style="width: 191px; *width: 188px;
                    width: 187px\9;">
                </select>
            </li>
            <li>
                <label>
                    最后操作时间：</label>
                <input type="text" name="LastOperStartTime" id="txtLastOperStartTime" class="w85"
                    style="width: 84px; *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="LastOperEndTime" id="txtLastOperEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    任务ID：</label>
                <input type="text" id="txtTaskID" class="w190" style="width: 188px; *width: 183px;
                    width: 183px\9;" />
            </li>
            <li>
                <label>
                    状态：</label>
                <select id="selStatus" runat="server" class="w90" style="width: 191px; *width: 188px;
                    width: 187px\9;">
                    <option value="-1">请选择</option>
                    <option value="180012">未分配</option>
                    <option value="180000">未处理</option>
                    <option value="180001">处理中</option>
                    <option value="180099">待审核</option>
                    <option value="180014">待复核</option>
                    <option value="180015">已完成</option>
                    <option value="180016">已结束</option>
                </select>
            </li>
            <li style="width: 390px;">
                <label>
                    主营品牌：</label><span>
                        <input type="text" class="w190" onclick="javascript:OpenSelectBrandPopup();" readonly="readonly"
                            id="txtCRMBrandNames" name="txtCRMBrandNames" style="width: 192px; *width: 191px;
                            width: 190px\9;">
                        <input type="hidden" name="txtCRMBrandIDs" id="txtCRMBrandIDs">
                    </span>
                <label for="radioNoCRMBrand" style="float: none; cursor: pointer">
                    <input type="checkbox" id="radioNoCRMBrand" value="NoCRMBrand" style='border: none;
                        width: auto;' /><span>空品牌</span>
                </label>
            </li>
        </ul>
        <ul class="clear" id="ulHidel" style="display: none;">
            <li id="" style="margin-left: 400px;">
                <div id="divAdditionalStatus" style="float: left; display: none;">
                    <span for="chkAS_A" style="margin-right: 10px;">
                        <input type="checkbox" id="chkAS_A" name="AdditionalStatus" value="AS_A" checked="true"
                            class="checkbox" />
                        <em onclick="emChkIsChoose(this)">A</em> </span><span for="chkAS_B" style="margin-right: 10px;">
                            <input type="checkbox" id="chkAS_B" name="AdditionalStatus" value="AS_B" checked="true"
                                class="checkbox" />
                            <em onclick="emChkIsChoose(this)">B</em> </span><span for="chkAS_C" style="margin-right: 10px;">
                                <input type="checkbox" id="chkAS_C" name="AdditionalStatus" value="AS_C" checked="true"
                                    class="checkbox" />
                                <em onclick="emChkIsChoose(this)">C</em> </span><span for="chkAS_D" style="margin-right: 10px;">
                                    <input type="checkbox" id="chkAS_D" name="AdditionalStatus" value="AS_D" checked="true"
                                        class="checkbox" />
                                    <em onclick="emChkIsChoose(this)">D</em> </span><span for="chkAS_E" style="margin-right: 10px;">
                                        <input type="checkbox" id="chkAS_E" name="AdditionalStatus" value="AS_E" checked="true"
                                            class="checkbox" />
                                        <em onclick="emChkIsChoose(this)">E</em> </span><span for="chkAS_F" style="margin-right: 10px;">
                                            <input type="checkbox" id="chkAS_F" name="AdditionalStatus" value="AS_F" checked="true"
                                                class="checkbox" />
                                            <em onclick="emChkIsChoose(this)">F</em> </span><span for="chkAS_G" style="margin-right: 10px;">
                                                <input type="checkbox" id="chkAS_G" name="AdditionalStatus" value="AS_G" checked="true"
                                                    class="checkbox" />
                                                <em onclick="emChkIsChoose(this)">G</em> </span><span for="chkAS_H" style="margin-right: 10px;">
                                                    <input type="checkbox" id="chkAS_H" name="AdditionalStatus" value="AS_H" checked="true"
                                                        class="checkbox" />
                                                    <em onclick="emChkIsChoose(this)">H</em> </span>
                    <span for="chkAS_I" style="margin-right: 10px;">
                        <input type="checkbox" id="chkAS_I" name="AdditionalStatus" value="AS_I" checked="true"
                            class="checkbox" />
                        <em onclick="emChkIsChoose(this)">I</em> </span><span for="chkAS_J" style="margin-right: 10px;">
                            <input type="checkbox" id="chkAS_J" name="AdditionalStatus" value="AS_J" checked="true"
                                class="checkbox" />
                            <em onclick="emChkIsChoose(this)">J</em> </span><span for="chkAS_K" style="margin-right: 10px;">
                                <input type="checkbox" id="chkAS_K" name="AdditionalStatus" value="AS_K" checked="true"
                                    class="checkbox" />
                                <em onclick="emChkIsChoose(this)">K</em> </span><span><a href="javascript:clearCheck()">
                                    清空</a></span>
                </div>
                <div id="divCheckStatus" style="display: none;">
                    <span for="chkAS_1" style="margin-right: 10px;">
                        <input type="checkbox" id="chkAS_1" name="checkStatus" value="5" checked="true" class="checkbox" />
                        <em onclick="emChkIsChoose(this)">提交</em> </span><span for="chkAS_2" style="margin-right: 10px;">
                            <input type="checkbox" id="chkAS_2" name="checkStatus" value="4" checked="true" class="checkbox" />
                            <em onclick="emChkIsChoose(this)">删除</em> </span><span for="chkAS_3" style="margin-right: 10px;">
                                <input type="checkbox" id="chkAS_3" name="checkStatus" value="-3" checked="true"
                                    class="checkbox" />
                                <em onclick="emChkIsChoose(this)">停用</em> </span>
                </div>
                <div id="divCheckStatus2" style="display: none;">
                    <span for="chkAS_1" style="margin-right: 10px;">
                        <input type="checkbox" id="Checkbox1" name="checkStatus" value="6" checked="true"
                            class="checkbox" />
                        <em onclick="emChkIsChoose(this)">审核通过</em> </span><span for="chkAS_2" style="margin-right: 10px;">
                            <input type="checkbox" id="Checkbox2" name="checkStatus" value="7" checked="true"
                                class="checkbox" />
                            <em onclick="emChkIsChoose(this)">审核拒绝</em> </span>
                </div>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户类别：</label>
                <select id="sltCustType" runat="server" style="width: 192px; *width: 192px; width: 190px\9">
                </select>
            </li>
            <li class="btnsearch" style="padding-left: 50px;">
                <input name="" type="button" id="btnSearch" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (btnAudit)
          { %>
        <input type="button" id="Button3" value="审核" class="newBtn" onclick="OpenAuditTaskPopup()" />
        <%} %>
        <%if (right_btn4)
          { %>
        <input type="button" id="Button2" value="导出" class="newBtn" onclick="ExportTask()" />
        <%} %>
        <%if (right_btn3)
          { %>
        <input type="button" id="Button1" value="结束" class="newBtn" onclick="Stop()" />
        <%} %>
        <%if (right_btn2)
          { %>
        <input type="button" id="btnAdd" value="回收" class="newBtn" onclick="RecedeCrmTask()" />
        <%} %>
        <%if (right_btn1)
          { %>
        <input type="button" id="btnCategory" value="分配" class="newBtn" onclick="AssignCheck()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <input type="hidden" id="hidSelectUserid" value="" />
    <input type="hidden" id="hidSelectPageSize" value="" />
    <%--导出任务--%>
    <form id="formExport" action="/AjaxServers/CustCheck/ExportTask.aspx">
    <input type="hidden" name="SelectPTIDs" value="" />
    <input type="hidden" id="Browser" name="Browser" value="" />
    <%--条件--%>
    <input type="hidden" name="name" value="" />
    <input type="hidden" name="custName" value="" />
    <input type="hidden" name="status_s" value="" />
    <input type="hidden" name="operstatus_s" value="" />
    <input type="hidden" name="group" value="" />
    <input type="hidden" name="category" value="" />
    <input type="hidden" name="creater" value="" />
    <input type="hidden" name="selUserId" value="" />
    <input type="hidden" name="beginTime" value="" />
    <input type="hidden" name="endTime" value="" />
    <input type="hidden" name="AdditionalStatus" value="" />
    <input type="hidden" name="optUserId" value="" />
    <input type="hidden" name="CRMBrandIDs" value="" />
    <input type="hidden" name="NoCRMBrand" value="" />
    <input type="hidden" name="TaskID" value="" />
    <input type="hidden" name="CustID" value="" />
    </form>
</asp:Content>
