<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignmentTaskNew.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.AssignmentTaskNew" %>

<script type="text/javascript">
    //全局变量
    //选择坐席
    var AgentChooseArray = new Array();

    //初始化
    $(document).ready(function () {
        DefineCheckBox();
        //初始化选择坐席
        AgentChooseArray = new Array();
        //默认选择所有坐席
        SelectAll();
        //默认分配任务
        AssignTask();
        if ($.browser.msie && 8 > $.browser.version) {
            $("#Div_assignmentTask").css("border", "0px;");
        }
        enterSearch(SearchData);
    });
    //定义选择控件动作
    function DefineCheckBox() {
        var ckb_all = document.getElementById("ckb_all");
        var ckb_row = document.getElementsByName("ckb_row");
        for (var i = 0; i < ckb_row.length; i++) {
            ckb_row[i].onclick = function () {
                if (this.checked) {
                    var isAllCheck = true;
                    for (var i = 0; i < ckb_row.length; i++) {
                        if (!ckb_row[i].checked) {
                            isAllCheck = false;
                            break;
                        }
                    }
                    if (isAllCheck)
                        ckb_all.checked = true;
                }
                else {
                    ckb_all.checked = false;
                }
                OnChecked(this);
                AssignTask();
            };
        }
        ckb_all.onclick = function () {
            for (var i = 0; i < ckb_row.length; i++) {
                ckb_row[i].checked = this.checked;
                OnChecked(ckb_row[i]);
            }
            AssignTask();
        }
    }
    //选择所有坐席
    function SelectAll() {
        var ckb_all = document.getElementById("ckb_all");
        var ckb_row = document.getElementsByName("ckb_row");
        ckb_all.checked = true;
        for (var i = 0; i < ckb_row.length; i++) {
            ckb_row[i].checked = true;
            OnChecked(ckb_row[i]);
        }
        AssignTask();
    }
    //选择状态变化时
    function OnChecked(ckb_row) {
        var array = ckb_row.value.split(',');
        if (ckb_row.checked) {
            AddAgent(array[0], array[1], array[2], array[3]);
        }
        else {
            DelAgent(array[0]);
        }
        //设置行状态
        SetRowStatus(ckb_row.checked, array[0]);
        //触发自动分配
        //AssignTask();
    }
    //设置行状态
    function SetRowStatus(checked, userid) {
        //选择项中选择对勾去掉之后，文本框中分配的数量清0，状态还原为修改状态。
        if (!checked) {
            $("#row_status_" + userid).val("xg");
            $("#txt_" + userid).css("display", "none");
            $("#txt_" + userid).val("");
        }
        else {
            $("#txt_" + userid).css("display", "block");
        }
        //xg：修改状态  其他：保持状态
        var row_status = $("#row_status_" + userid).val();
        if (row_status == "xg") {
            if (checked) {
                $("#txt_" + userid).attr("disabled", false);

                $("#a_bc_" + userid).css("display", "block");
                $("#s_bc_" + userid).css("display", "none");
            }
            else {
                $("#a_bc_" + userid).css("display", "none");
                $("#s_bc_" + userid).css("display", "block");
            }

            $("#a_xg_" + userid).css("display", "none");
            $("#s_xg_" + userid).css("display", "none");
        }
        else {
            $("#a_bc_" + userid).css("display", "none");
            $("#s_bc_" + userid).css("display", "none");

            if (checked) {
                $("#txt_" + userid).attr("disabled", true);

                $("#a_xg_" + userid).css("display", "block");
                $("#s_xg_" + userid).css("display", "none");
            }
            else {
                $("#a_xg_" + userid).css("display", "none");
                $("#s_xg_" + userid).css("display", "block");
            }
        }
    }
    //行状态变更
    function ChangeStatus(s, userid) {
        $("#row_status_" + userid).val(s);
        SetRowStatus(true, userid);
    }
    //分配任务
    function AssignTask() {
        var total_task = $("#total_task").text();
        if (total_task <= 0) {
            $.jAlert("任务数为0，无法分配任务，窗口关闭！", function () {
                $.closePopupLayer('AssignmentTaskNew', false);
                return;
            });
            return;
        }
        var lock_task = CalcAgentLock();
        if (total_task - lock_task < 0) {
            //无可分配的任务
            RefreshNum();
            return;
        }
        var assign_agent = AgentChooseArray.length - CalcAgentLockCount();
        if (assign_agent <= 0) {
            //无可分配的人员
            RefreshNum();
            return;
        }
        //排序
        SortAgent();
        //整除部分
        var p = Math.floor((total_task - lock_task) / assign_agent);
        //余数部分
        var y = (total_task - lock_task) % assign_agent;
        //分配任务
        var j = 0;
        for (var i = 0; i < AgentChooseArray.length; i++) {
            if (!AgentChooseArray[i].IsLock()) {
                AgentChooseArray[i].CurrentTask = p;
                if (j < y) {
                    AgentChooseArray[i].CurrentTask += 1;
                }
                j++;
                //呈现列表
                $("#txt_" + AgentChooseArray[i].userid).val(AgentChooseArray[i].CurrentTask);
            }
        }
        //刷新数量
        RefreshNum();
    }
    //更新界面3数量
    function RefreshNum() {
        var total_task = $("#total_task").text();
        var hasass_task = CalcAgentTotal();
        var noass_task = total_task - hasass_task;
        //$("#assign_task").text(hasass_task);
        $("#remain_task").text(noass_task);
    }
    //输入框只能输入数字
    function InputDigital(txt) {
        var total_task = $("#total_task").text();
        var lock_task = CalcAgentLock();
        var rem_task = total_task - lock_task;
        var value = $.trim($(txt).val());
        if (value == "") {
            value = 0;
            $(txt).val("0");
        }
        var int_vlaue = parseInt(value);
        if (isNaN(int_vlaue)) {
            //$.jAlert("输入数量格式不正确，请输入范围（" + 0 + "~" + rem_task + "）的整数！", function () { $(txt).focus(); });
            int_vlaue = 0;
        }
        else if (int_vlaue < 0 || int_vlaue > rem_task) {
            //$.jAlert("输入数量超出界限，请输入范围（" + 0 + "~" + rem_task + "）的整数！", function () { $(txt).focus(); });
            if (int_vlaue < 0)
                int_vlaue = 0;
            else
                int_vlaue = rem_task;
        }
        //赋正确格式的值
        if (int_vlaue != $(txt).val()) {
            $(txt).val(int_vlaue);
        }
        //获取当前对象
        var userid = $(txt).attr("name");
        var agent = GetAgent(userid);
        if (agent) {
            agent.lock = true;
            agent.CurrentTask = int_vlaue;
            AssignTask();
            agent.lock = false;
        }
    }

    ///////////////////////////////////////查询////////////////////////////////////////////
    //只能输入数字
    function LimitDigital(txt) {
        var value = $.trim($(txt).val());
        if ($(txt).val() != value) {
            $(txt).val(value);
        }
        if (value == "")
            return;

        if (/^\d+$/.test(value)) {
            if (value.length > 8) {
                value = value.substr(0, 8)
                $(txt).val(value);
            }
            return;
        }
        else {
            if (value.length > 0) {
                value = value.substr(0, value.length - 1)
                $(txt).val(value);
                LimitDigital(txt)
            }
        }
    }
    //长度限制
    function LimitLength(txt) {
        var value = $.trim($(txt).val());
        if ($(txt).val() != value) {
            $(txt).val(value);
        }
        if (value == "")
            return;
        if (value.length > 20) {
            value = value.substr(0, 20)
            $(txt).val(value);
        }
    }
    //查询
    function SearchData() {
        var name = $.trim($("#txt_name").val());
        var anum = $.trim($("#txt_anum").val());
        var data = GetSearchData(name, anum);
        if (data.length == 0) {
            return;
        }
        //位置切换
        ResetTableRow(data);
        DefineCheckBox();
    }
    //符合条件的数据
    function GetSearchData(name, anum) {
        var data = new Array();
        //获取所有的坐席数据
        var ckb_row = document.getElementsByName("ckb_row");
        for (var i = 0; i < ckb_row.length; i++) {
            var array = ckb_row[i].value.split(',');
            //array[0], array[1], array[2] 工号, array[3] 名称
            if (
            //名称条件
                (name == "" || (name != "" && array[3].indexOf(name) >= 0))
            &&
            //工号条件
                (anum == "" || (anum != "" && array[2].indexOf(anum) >= 0))
            ) {
                data.push(CreateAgent(array[0], array[1], array[2], array[3]));
            }
        }
        if (data.length > 0) {
            SortSearchData(data);
        }
        return data;
    }
    //排序
    function SortSearchData(data) {
        data.sort(function (a, b) {
            if (parseInt(a.tasknum) != parseInt(b.tasknum)) {
                //任务总数小的优先
                if (parseInt(b.tasknum) < parseInt(a.tasknum)) {
                    return 1;
                }
                else return -1;
            }
            else {
                //员工号小的优先
                if (b.agentnum < a.agentnum) {
                    return 1;
                }
                else {
                    return -1;
                }
            }
        });
    }
    //充值表格位置
    function ResetTableRow(data) {
        var table = document.getElementById("table_list");
        var tr_head = document.getElementById("tr_head");
        for (var i = data.length - 1; i >= 0; i--) {
            var userid = data[i].userid;
            var tr = document.getElementById("tr_" + userid);
            //记录checkbox和input的值和状态
            var a1 = $("#row_status_" + userid).val();
            var a2 = $("#ckb_" + userid)[0].checked;
            var a3 = $("#txt_" + userid).val();
            var a4 = $("#txt_" + userid).attr("disabled");
            //插入行
            var newtr = $(table.insertRow(1)).html(tr.innerHTML);
            //newtr.innerHTML = tr.innerHTML;
            //newtr.outerHTML = tr.outerHTML;
            //恢复记录
            $("#row_status_" + userid).val(a1);
            $("#ckb_" + userid)[0].checked = a2;
            $("#txt_" + userid).val(a3);
            $("#txt_" + userid).attr("disabled", a4);
            //删除行
            tr = document.getElementById("tr_" + userid);
            table.deleteRow(tr.rowIndex);
            //赋值id
            $(newtr).attr("id", "tr_" + userid);
        }
    }
    //回车查询
    function EnterPressToSerach() {
        var e = window.event;
        if (e.keyCode == 13) {
            SearchData();
        }
    }
    ///////////////////////////////////////查询////////////////////////////////////////////

    ///////////////////////////////////////数组操作////////////////////////////////////////////
    //新建一个坐席对象
    function CreateAgent(userid, tasknum, agentnum, name) {
        var agent = {
            userid: userid,
            tasknum: tasknum,
            agentnum: agentnum,
            name: name,
            //当前分配任务数
            CurrentTask: 0,
            //临时锁定
            lock: false,
            IsLock: function () {
                //此坐席为刚刚输入域，锁定
                if (this.lock) {
                    return true;
                }
                //检查坐席状态
                var row_status = $("#row_status_" + userid).val();
                if (row_status == "xg") {
                    return false;
                }
                else {
                    return true;
                }
            }
        };
        return agent;
    }
    //获取项目
    function GetAgent(userid) {
        for (var i = 0; i < AgentChooseArray.length; i++) {
            if (AgentChooseArray[i].userid == userid) {
                return AgentChooseArray[i];
            }
        }
        return null;
    }
    //获取项目idx
    function GetAgentIdx(userid) {
        for (var i = 0; i < AgentChooseArray.length; i++) {
            if (AgentChooseArray[i].userid == userid) {
                return i;
            }
        }
        return -1;
    }
    //给数组添加新项目
    function AddAgent(userid, tasknum, agentnum, name) {
        var agent = GetAgent(userid);
        if (agent == null) {
            AgentChooseArray.push(CreateAgent(userid, tasknum, agentnum, name));
        }
    }
    //移除项目
    function DelAgent(userid) {
        var idx = GetAgentIdx(userid);
        if (idx >= 0) {
            AgentChooseArray.splice(idx, 1);
        }
    }
    //按照任务数和工号排序
    function SortAgent() {
        AgentChooseArray.sort(function (a, b) {
            if (parseInt(a.tasknum) != parseInt(b.tasknum)) {
                //任务总数小的优先
                if (parseInt(b.tasknum) < parseInt(a.tasknum)) {
                    return 1;
                }
                else return -1;
            }
            else {
                //员工号小的优先
                if (b.agentnum < a.agentnum) {
                    return 1;
                }
                else {
                    return -1;
                }
            }
        });
    }
    //计算已分配数
    function CalcAgentTotal() {
        var total = 0;
        for (var i = 0; i < AgentChooseArray.length; i++) {
            total += AgentChooseArray[i].CurrentTask;
        }
        return total;
    }
    //计算锁定任务数
    function CalcAgentLock() {
        var total = 0;
        for (var i = 0; i < AgentChooseArray.length; i++) {
            if (AgentChooseArray[i].IsLock()) {
                total += AgentChooseArray[i].CurrentTask;
            }
        }
        return total;
    }
    //计算锁定人数
    function CalcAgentLockCount() {
        var total = 0;
        for (var i = 0; i < AgentChooseArray.length; i++) {
            if (AgentChooseArray[i].IsLock()) {
                total += 1;
            }
        }
        return total;
    }
    //获取分配数的字符串
    function GetAgentString() {
        var str = "";
        for (var i = 0; i < AgentChooseArray.length; i++) {
            str += AgentChooseArray[i].userid + ":" + AgentChooseArray[i].CurrentTask + ",";
        }
        str = str.substr(0, str.length - 1);
        return str;
    }
    ///////////////////////////////////////数组操作////////////////////////////////////////////
    //保存
    function SaveData() {
        if (AgentChooseArray.length == 0) {
            $.jAlert("请至少选择一个坐席进行任务分配！", function () { });
            return;
        }
        var task = CalcAgentTotal();
        if (task == 0) {
            $.jAlert("分配任务总数为0，请重新填写分配数！", function () { });
            return;
        }
        var para = {
            Action: "AssignEmployeeNew",
            ProjectID: '<%=ProjectID%>',
            Data: GetAgentString(),
            r: Math.random()
        };
        AjaxPost("/AjaxServers/ProjectManage/GenerateTask.ashx", para,
            function () {
                $("#Div_assignmentTask").hide();
                $.blockUI({ message: '正在分配中，请等待...' });
            },
            function (data) {
                $.unblockUI();
                data = $.evalJSON(data);
                if (data.msg == "success") {
                    $.jPopMsgLayer("任务分配成功！", function () { $.closePopupLayer('AssignmentTaskNew', true); });
                }
                else {
                    $("#Div_assignmentTask").show();
                    $.jAlert(data.msg);
                }
            }
        );
    }
    //关闭
    function CloseWin() {
        $.closePopupLayer('AssignmentTaskNew', false);
    }
</script>
<div class="popup openwindow" id="Div_assignmentTask">
    <div class="title ft14">
        <h2>
            任务分配
        </h2>
        <span class="right"><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignmentTaskNew',false);">
        </a></span>
    </div>
    <div class="content">
        <div class="search">
            <ul>
                <li>
                    <label>
                        姓名：
                    </label>
                    <input id="txt_name" type="text" value="" class="w120" onkeyup="LimitLength(this)"
                        onafterpaste="LimitLength(this)"/>
                </li>
                <li>
                    <label>
                        工号：
                    </label>
                    <input id="txt_anum" type="text" value="" class="w120" onkeyup="LimitDigital(this)"
                        onafterpaste="LimitDigital(this)"  />
                </li>
                <li style="width: 160px;" class="btn">
                    <input type="button" value="查询" class="w60" onclick="SearchData();" />
                   
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <div class="line">
        </div>
        <div class="count_sum">
            <ul>
                <%--<li>
                    <label>
                        任务总数：</label><span id="total_task"><%=NotDistrictTaskCount%></span></li>
                <li>
                    <label>
                        分配数量：</label><span id="assign_task">0</span></li>
                <li class="right">
                    <label>
                        剩余数量：</label><span id="remain_task">0</span></li>--%>
                <li style=" width:130px;">
                    <label>
                        任务总数：</label><span id="Span1"><%=TaskCount%></span></li>
                <li style=" width:153px;">
                    <label>
                        已分配任务数：</label><span id="Span2"><%=TaskCount - NotDistrictTaskCount%></span></li>
                <li style=" width:153px;">
                    <label>
                        待分配任务数：</label><span id="total_task"><%=NotDistrictTaskCount%></span></li>
                <li class="right" style=" width:103px; text-align:left;">
                    <label>  
                        剩余数量：</label><span id="remain_task">0</span></li>
            </ul>
        </div>
        <div class="sum_list" style=" height:300px;">
            <table cellspacing="0" cellpadding="0" id="table_list">
                <tr id="tr_head">
                    <th width="8%" title='<%=rowCount%>'>
                        <input id="ckb_all" type="checkbox" value="" />
                    </th>
                    <th width="25%">
                        客服名称
                    </th>
                    <th width="15%">
                        工号
                    </th>
                    <th width="20%">
                        角色
                    </th>
                    <th width="10%">
                        任务数量
                    </th>
                    <th width="12%">
                        操作
                    </th>
                </tr>
                <asp:repeater id="AgentList" runat="server">
                    <ItemTemplate>
                        <tr id="tr_<%#Eval("UserID") %>">
                            <td>
                                <input id="ckb_<%#Eval("UserID") %>" name="ckb_row" type="checkbox" value="<%#Eval("UserID") %>,<%#Eval("TaskCount")%>,<%#Eval("AgentNum")%>,<%#Eval("TrueName")%>"/>
                            </td>
                            <td>
                                <%#Eval("TrueName")%>
                            </td>
                            <td>
                                <%#Eval("AgentNum")%>
                            </td>
                            <td>
                                <%#Eval("RoleName")%>
                            </td>
                            <td class="task_sum">
                                <div style="width: 95px; height:25px; padding:2px 0px;">
                                    <input id="txt_<%#Eval("UserID") %>" type="text" value="" style="width: 80px;" onkeyup="InputDigital(this)" onafterpaste="InputDigital(this)" name="<%#Eval("UserID") %>"/>
                                </div>
                            </td>
                            <td>
                                <a href="javascript:void(0);" id="a_bc_<%#Eval("UserID") %>" onclick="ChangeStatus('bc','<%#Eval("UserID") %>')" style="display:block;">保持</a>
                                <span id="s_bc_<%#Eval("UserID") %>" style="color:#ccc;display:none;">保持</span>
                                <a href="javascript:void(0);" id="a_xg_<%#Eval("UserID") %>" onclick="ChangeStatus('xg','<%#Eval("UserID") %>')" style="display:none;">修改</a>
                                <span id="s_xg_<%#Eval("UserID") %>" style="color:#ccc;display:none;">修改</span>
                                <input type="hidden" id="row_status_<%#Eval("UserID") %>" value="xg" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </table>
        </div>
        <div class="btn">
            <input type="button" name="" value="保 存" class="btnSave bold" onclick="SaveData()"/>&nbsp;&nbsp;
            <input type="button" name="" value="关 闭" class="btnCannel bold" onclick="CloseWin()"/>
        </div>
    </div>
</div>