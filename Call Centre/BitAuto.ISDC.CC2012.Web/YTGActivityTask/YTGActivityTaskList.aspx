<%@ Page Title="邀约处理" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="YTGActivityTaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.YTGActivityTaskList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        /*客户列表 编辑分类按钮样式*/
        .cxTab ul li.w180 a
        {
            border: none;
            font-size: 14px;
            background: #fff;
            color: #666;
            text-decoration: none;
        }
        #processedli ul li a
        {
            border: none;
            font-size: 12px;
            background: #e6e6e6;
            color: #666;
            text-decoration: none;
        }
        .cxTab ul li.w180 a:hover, .cxTab ul li.w180 a.cur, #processedli ul li a.cur
        {
            background: #6BBBD6;
            color: #FFF;
        }
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
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtProjectName" style="width: 186px;" />
            </li>
            <li>
                <label>
                    任务ID：</label>
                <input type="text" id="txtTaskID" style="width: 186px;" />
            </li>
            <li>
                <label>
                    所属坐席：</label>
                <input type="text" id="txtAgentNum" style="width: 186px;" readonly="true" onclick="SelectVisitPerson()" />
                <input type="hidden" id="txtAssignID" />
            </li>
        </ul>
        <ul class="clear">
            <%-- <li>
                <label>
                    创建日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 83px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 83px;
                    *width: 83px; width: 83px\9;" />
            </li>--%>
            <li>
                <label>
                    创建日期：</label>
                <input type="text" name="TaskBeginTime" id="tBeginTime" class="w85" style="width: 83px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="TaskEndTime" id="tEndTime" class="w85" style="width: 83px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li class="btnsearch" style="width: 96px; *width: 130px; text-align: left; margin-top: 1px;">
                <input style="float: right" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 900px; height: 30px; margin: 10px 0 -10px 0;">
                <div id="divStatus" class="cxTab mt5" style="margin-left: 30px;">
                    <ul id="selectli" class="">
                    </ul>
                    <div class="clearfix">
                    </div>
                </div>
            </li>
            <li style="width: 900px; height: 30px; margin: 10px 0 0px 0; display: none;" id="processedli">
            </li>
        </ul>
    </div>
    <input type="hidden" id="hidStatus" value='' />
    <input type="hidden" id="hidProcessedStatus" value='' />
    <input type="hidden" id="hidSelectPageSize" value="" />
    <div class="optionBtn clearfix">
        <%if (right_withdraw)
          { %>
        <input type="button" id="btnWithDraw" value="收回" class="newBtn" onclick="operRecede()" />
        <%} %>
        <%if (right_allocation)
          { %>
        <input type="button" id="btnAllocation" value="分配" class="newBtn" onclick="operAssign()" />
        <%} %>
    </div>
    <div id="ajaxTable" class="bit_table" width="99%" cellspacing="0" cellpadding="0">
    </div>
    <script type="text/javascript">
        var loginUser = '<%=UserID %>';
        $(function () {
            //初始化时间
            InitWdatePicker(2, ["tBeginTime", "tEndTime"]);
            $("#tEndTime").val(setDealTime());


            //敲回车键执行方法
            enterSearch(search);
            search();
        });

        function setDealTime() {
            var oDate = new Date();
            function addZero(n) {
                return n < 10 ? '0' + n : n;
            }
            return [oDate.getFullYear(), addZero(oDate.getMonth() + 1), addZero(oDate.getDate())].join('-');
        }

        //获取任务状态数量
        function getStatusNum(pody) {
            pody.Action = "getStatusNum";
            pody.lg = loginUser;
            var oUl = document.getElementById("selectli");
            oUl.innerHTML = "";
            var aA = oUl.getElementsByTagName('a');

            var aLi_P = document.getElementById('processedli');
            aLi_P.innerHTML = '';
            var aA_P = aLi_P.getElementsByTagName('a');
            var jsonProcessed = {};

            AjaxPostAsync("/YTGActivityTask/AjaxServers/Handler.ashx", pody, null, function (data) {
                var jsonData = eval("(" + data + ")");
                jsonProcessed = {};
                var html = "", total = 0;
                for (var name in jsonData) {
                    if (name == '成功' || name == '失败') {
                        jsonProcessed[name] = [jsonData[name][0], jsonData[name][1]];
                        continue;
                    }
                    var oLi = createLi(name, jsonData);
                    oUl.appendChild(oLi);
                    total += parseInt(jsonData[name][1]);
                }
                var oLi = createLi("全部", { 全部: ['undefined', total] });
                oUl.insertBefore(oLi, oUl.children[0]);
                createProcessed(oUl.children[4], jsonProcessed);
            });

            function createLi(name, obj) {
                var oLi = document.createElement("li");
                oLi.style.marginRight = "18px";
                oLi.className = "w180";
                var oA = document.createElement("a");
                oA.setAttribute("val", obj[name][0]);
                oA.href = "javascript:;";
                oA.innerHTML = name + "（" + obj[name][1] + "）";
                oA.onclick = function () {
                    //只有在选中已处理时，才显示 成功、失败的信息
                    oA.getAttribute('val') == '4' ? aLi_P.style.display = 'block' : aLi_P.style.display = 'none';

                    for (var k = 0; k < aA.length; k++) { aA[k].className = ""; }
                    for (var k = 0; k < aA_P.length; k++) { aA_P[k].className = ""; }
                    $("#hidStatus").val(this.getAttribute("val"));
                    $("#hidProcessedStatus").val('');
                    search(1, this.getAttribute("val"));
                }
                oLi.appendChild(oA);
                oLi.style.width = 'auto';
                return oLi;
            }

        }

        //增加状态为“已处理”的成功和失败的数量提示
        function createProcessed(obj, jsonProcessed) {
            var oLi = document.getElementById('processedli');

            var div = document.createElement('div');
            div.style.cssText = 'border: 10px dashed transparent; border-bottom-style: solid;border-bottom-color: #e6e6e6; margin: -20px 0px 0px ' + (obj.offsetLeft + 10) + 'px;padding:0;padding:0;width:0;height:0';
            oLi.appendChild(div);

            var ul = document.createElement('ul');
            ul.style.cssText = 'height:28px; width:250px; font-size:12px; background:#e6e6e6; margin-left:' + (obj.offsetLeft - 10) + 'px;';
            oLi.appendChild(ul);
            var aA_P = ul.getElementsByTagName('a');

            var oUl = document.getElementById("selectli");
            var aA = oUl.getElementsByTagName('a');

            for (var name in jsonProcessed) {
                var li = document.createElement('li');
                var value = jsonProcessed[name];
                var oA = document.createElement("a");
                oA.setAttribute("val", value[0]);
                oA.href = "javascript:;";
                oA.innerHTML = name + "（" + value[1] + "）";
                oA.id = 'aProcess' + value[0];
                oA.onclick = function () {
                    for (var k = 0; k < aA.length; k++) { aA[k].className = ""; }
                    for (var k = 0; k < aA_P.length; k++) { aA_P[k].className = ""; }
                    this.className = 'cur';
                    $("#hidStatus").val('');
                    $("#hidProcessedStatus").val(this.getAttribute("val"));
                    if ($("#hidProcessedStatus").val() == "1" || $("#hidProcessedStatus").val() == "0") {
                        $("#hidStatus").val("4");
                    }
                    search(2, this.getAttribute("val"));
                }

                li.appendChild(oA);
                var css = ' width:auto; margin-right:8px;height:28px;line-height: 28px;margin-left:16px;';
                li.style.cssText = css;
                ul.appendChild(li);
            }

        }

        //查询
        function search(type, status) {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/YTGActivityTask/AjaxServers/YTGActivityTaskList.aspx", podyStr);
            if (!status) getStatusNum(pody);

            var pStatus = parseInt($('#hidProcessedStatus').val());
            var oLi = document.getElementById("aProcess" + pStatus);
            if (oLi) {
                oLi.className = 'cur';
            } else {
                var aLi = document.getElementById("selectli").getElementsByTagName('a');
                var idx = parseInt($("#hidStatus").val());
                if (!isNaN(idx) && idx) {
                    aLi[idx].className = 'cur';
                } else {
                    if (aLi[0]) { aLi[0].className = 'cur'; }
                }
            }

        }
        //获取参数
        function _params() {

            var assignID = encodeURIComponent($.trim($("#txtAssignID").val()));
            var pName = encodeURIComponent($.trim($("#txtProjectName").val()));
            var status = encodeURIComponent($.trim($("#hidStatus").val()));
            var isSuccess = encodeURIComponent($.trim($("#hidProcessedStatus").val()));
            //            var beginDealTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
            //            var endDealTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            var TaskCBeginTime = encodeURIComponent($.trim($("#tBeginTime").val()));
            var TaskCEndTime = encodeURIComponent($.trim($("#tEndTime").val()));
            var TaskID = encodeURIComponent($.trim($("#txtTaskID").val()));
            var pageSize = $("#hidSelectPageSize").val();

            var pody = {
                AssignID: assignID,
                ProjectName: pName,
                Status: status,
                IsSuccess: isSuccess,
                //                BeginDealTime: beginDealTime,
                //                EndDealTime: endDealTime,
                TaskCBeginTime: TaskCBeginTime,
                TaskCEndTime: TaskCEndTime,
                TaskID: TaskID,
                lg: loginUser,
                pageSize: pageSize,
                r: Math.random()
            }
            if (status && status != 'undefined') pody.Status = status;

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('/YTGActivityTask/AjaxServers/YTGActivityTaskList.aspx', pody + "&r=" + Math.random());
        }

    </script>
    <script type="text/javascript">

        //选择坐席名称
        function SelectVisitPerson(afterFn) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                success: function () {
                    //分配任务，隐藏控件
                    if (afterFn) { $("#popAClear").hide(); }
                },
                beforeClose: function (e) {
                    if (!e) return;
                    var userid = $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid');
                    if (afterFn) {
                        afterFn(userid);
                    }
                    else {
                        $("[id$='txtAgentNum']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                        $("#txtAssignID").val(userid);
                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        //分配操作
        function operAssign(taskIDs) {
            var _taskIDs = taskIDs ? taskIDs : getTaskIDs();

            if (!_taskIDs) {
                $.jAlert("请选择任务！");
                return;
            }


            AjaxPostAsync("/YTGActivityTask/AjaxServers/Handler.ashx", { Action: "taskIsAssign", lg: loginUser, TaskIDS: _taskIDs, r: Math.random() }, null, function (data) {
                var jsonData = eval("(" + data + ")");
                if (jsonData.result == "noaccess") {
                    $.jAlert(jsonData.msg);
                    return;
                }
                (jsonData.result == "true") && oper();

                if (jsonData.result == "false") {

                    $.jConfirm("任务已存在分配人，是否确定分配？", function (r) {
                        if (!r) return;
                        oper();
                    });

                }
            });


            function oper() {

                SelectVisitPerson(function (userid) {
                    AjaxPostAsync("/YTGActivityTask/AjaxServers/Handler.ashx", { Action: "assignTask", lg: loginUser, TaskIDS: _taskIDs, UserID: userid, r: Math.random() }, null, function (data) {
                        var jsonData = eval("(" + data + ")");
                        if (jsonData.result == "true") {
                            //                            $.jAlert("分配任务成功", function () {
                            //                                search();
                            //                            });
                            $.jPopMsgLayer("分配任务成功", function () {
                                search();
                            });

                        } else {
                            $.jAlert(jsonData.msg);
                        }
                    });
                });

            }

        }


        //收回操作
        function operRecede(taskIDs) {
            var _taskIDs = taskIDs ? taskIDs : getTaskIDs();

            if (!_taskIDs) {
                $.jAlert("请选择任务！");
                return;
            }
            $.jConfirm("是否确定收回任务？", function (r) {
                if (!r) return;

                $.post("/YTGActivityTask/AjaxServers/Handler.ashx", { Action: "recedeTask", lg: loginUser, TaskIDS: _taskIDs, r: Math.random() }, function (data) {
                    var jsonData = eval("(" + data + ")");
                    if (jsonData.result == "true") {
                        //                        $.jAlert("收回任务成功", function () {
                        //                            search();
                        //                        });
                        $.jPopMsgLayer("收回任务成功", function () {
                            search();
                        });

                    }
                    else {
                        $.jAlert(jsonData.msg);
                    }
                });

            });

        }

        //获取TaskIDs
        function getTaskIDs() {
            var taskIDs = "";
            var aSelect = document.getElementsByName("chkSelect");
            for (var i = 0; i < aSelect.length; i++) {
                if (aSelect[i].checked) {
                    taskIDs += aSelect[i].value + ',';
                }
            }
            taskIDs ? taskIDs = taskIDs.substring(0, taskIDs.length - 1) : "";
            return taskIDs;
        }

    </script>
</asp:Content>
