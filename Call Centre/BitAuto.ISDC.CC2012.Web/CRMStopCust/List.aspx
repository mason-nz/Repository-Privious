<%@ Page Title="客户核实" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CRMStopCust.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        loadJS("controlParams");
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#SubmitBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'SubmitEndTime\')}', onpicked: function () { document.getElementById("SubmitEndTime").focus(); } }); });
            $('#SubmitEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'SubmitBeginTime\')}' }); });
        });
    </script>
    <form id="form1">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客户ID：</label>
                <input type="text" id="txtCustID" class="w200" name="CustID" />
            </li>
            <li>
                <label>
                    大区：</label>
                <span>
                    <select id="sltArea" class="w200" style='width: 206px;' name="AreaName">
                        <option value="-1">请选择</option>
                        <asp:Repeater ID="rp_AreaOptions" runat="server">
                            <ItemTemplate>
                                <option value="<%#Eval("DepartName")%>">
                                    <%#Eval("DepartName")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </span></li>
            <li>
                <label>
                    审核时间：</label>
                <input type="text" name="SubmitBeginTime" id="SubmitBeginTime" class="w95" vtype="isDate"
                    vmsg="审核时间格式不正确" />-<input type="text" name="SubmitEndTime" id="SubmitEndTime" class="w95"
                        vtype="isDate" vmsg="审核时间格式不正确" />
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" class="w200" name="CustName" />
            </li>
            <li>
                <label>
                    坐席：</label>
                <select id="ddlOperator" class="w200" style='width: 206px;' name="OperID">
                </select></li>
            <li>
                <label>
                    申请人：</label>
                <input type="text" id="txtApplyer" class="w200" name="ApplerName" />
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    核实类型：</label>
                <span>
                    <select id="selApplyType" class="w200" style='width: 206px;' onchange="selApplyTypeChange()"
                        name="ApplyType">
                        <option value="1">停用</option>
                        <option value="2">启用</option>
                    </select></span> </li>
            <li>
                <label>
                    申请原因：</label>
                <span>
                    <select id="selReason" class="w200" style='width: 206px;' name="Reason">
                    </select></span> </li>
            <li>
                <label>
                    申请说明：</label>
                <span>
                    <select id="selRemark" class="w200" style='width: 206px;' name="Remark">
                    </select></span> </li>
        </ul>
        <ul>
            <li>
                <label>
                    任务状态：
                </label>
                <span>
                    <select id="ddlTaskStatus" class="w200" style='width: 206px;' name="TaskStatus">
                        <option value="-1">请选择</option>
                        <asp:Repeater ID="ddlTaskStatusRepeater" runat="server">
                            <ItemTemplate>
                                <option value="<%#Eval("value") %>">
                                    <%#Eval("name") %>
                                </option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </span></li>
            <li>
                <label>
                    客户状态：</label>
                <span>
                    <select id="ddlStopStatus" class="w200" style='width: 206px;' name="StopStatus">
                        <option value="-1">请选择</option>
                        <asp:Repeater ID="ddlStopStatusRepeater" runat="server">
                            <ItemTemplate>
                                <option value="<%#Eval("value") %>">
                                    <%#Eval("name") %>
                                </option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </span></li>
            <li>
                <label>
                    经营范围：</label>
                <span style="margin-top: 2px;">
                    <input type="checkbox" value="1,3" id="chkNewCar" name="CarType" /><em onclick="emChkIsChoose(this);">新车</em></span>
                <span>
                    <input type="checkbox" value="2" id="chkSecondCar" name="CarType" /><em onclick="emChkIsChoose(this);">二手车</em>
                </span></li>
            <li class="btnsearch">
                <input style="float: right; margin-top: 5px;" name="" id="btnSearch" type="button"
                    value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <input type="hidden" id="hidBrowser" name="Browser" />
    </div>
    <div class="optionBtn clearfix">
        <%if (right_RevokeTask)
          {%>
        <input name="" type="button" value="收 回" onclick="RecedeCrmTask()" class="newBtn" />
        <%}%>
        <%if (right_AssignTask)
          { %><input name="" type="button" value="分 配" class="newBtn" onclick="AssignCheck()" /><%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <input type="hidden" id="hidSelectUserid" value="" />
    <script type="text/javascript">

        //批量分配任务
        function AssignCheck() {
            var taskIDs = $(":checkbox[name='chkStop']:checked").map(function () {
                return $(this).val();
            }).get().join(",");

            AssignTask(taskIDs);
        }

        //单个分配任务
        function AssignTask(TaskIDS) {
            if (TaskIDS != undefined && TaskIDS != "") {
                $.openPopupLayer({
                    name: "AssignTaskAjaxPopupForSelect",
                    url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                    success: function () {
                        //分配任务，隐藏控件
                        $("#popAClear").hide();
                    },
                    beforeClose: function (e) {
                        if (e) {
                            $("#hidSelectUserid").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                            var userid = $("#hidSelectUserid").val();
                            //分配任务
                            $.post("OperHandler.ashx", { Action: "AssignTask", TaskIDS: encodeURIComponent(TaskIDS), AssignUserID: userid }, function (data) {
                                var jsonData = eval("(" + data + ")");
                                if (jsonData.result == "true") {
                                    $.jPopMsgLayer("分配任务成功", function () {
                                        search();
                                    });
                                }
                                else {
                                    $.jAlert(jsonData.error);
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

            var taskIDs = $(":checkbox[name='chkStop'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
            RecedeCrmOne(taskIDs);
        }

        ///回收一个任务
        function RecedeCrmOne(TaskIDS) {

            if (Len(TaskIDS) > 0) {

                $.jConfirm("确定要收回所选择的的任务吗？", function (r) {

                    if (r) {
                        //回收任务
                        $.post("OperHandler.ashx", { Action: "RecedeTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                            var jsonData = eval("(" + data + ")");
                            if (jsonData.result == "true") {
                                $.jPopMsgLayer("收回任务成功", function () {
                                    search();
                                });
                            }
                            else {
                                $.jAlert(jsonData.error);
                            }
                        });
                    }
                });

            }
            else {
                $.jAlert("请至少选择一个要收回的任务！");
            }
        }


        //操作人
        function getOper() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getCreater", TableName: "OrderCRMStopCustTask", ShowField: "AssignUserID", TableStatus: "0", r: Math.random() }, null, function (data) {
                $("#ddlOperator").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#ddlOperator").append("<option value=" + jsonData[i].UserID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //查询
        function search() {
            showSearchList.getList("/CRMStopCust/AjaxList.aspx", "form1", "ajaxTable");
        }
        //绑定申请原因，申请说明
        function GetApplyReasonAndRemark() {
            $("#selReason").empty();
            $("#selRemark").empty();
            var ApplyType = $("#selApplyType").val();
            $("#selReason").append("<option value='-1'>请选择</option>")
            $.post("OperCustRelationTaskHandler.ashx", { Action: "GetApplyReason", ApplyType: ApplyType }, function (data) {
                if (data) {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, n) {
                        $("#selReason").append("<option value='" + n.value + "'>" + n.name + "</option>");
                    });
                }
            });
            $("#selRemark").append("<option value='-1'>请选择</option>")
            $.post("OperCustRelationTaskHandler.ashx", { Action: "GetApplyRemark", ApplyType: ApplyType }, function (data) {
                if (data) {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, n) {
                        $("#selRemark").append("<option value='" + n.value + "'>" + n.name + "</option>");
                    });
                }
            });
        }
        //申请类型改变加载申请原因，申请说明
        function selApplyTypeChange() {
            GetApplyReasonAndRemark();
        }

        $(function () {
            $("#hidBrowser").val(GetBrowserName());
            getOper();
            //加载申请原因，申请说明
            GetApplyReasonAndRemark();
            //敲回车键执行方法
            enterSearch(search);
        }); 
    </script>
    </form>
</asp:Content>
