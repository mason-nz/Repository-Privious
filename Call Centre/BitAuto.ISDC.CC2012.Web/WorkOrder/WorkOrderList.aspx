<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.WorkOrderList" MasterPageFile="~/Controls/Top.Master"
    Title="工单记录" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <style type="text/css">
        .search ul li
        {
            width: 280px;
        }
        .changesearchbt
        {
            margin-top: -20px;
            margin-left: -60px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            ChangeWorkCategory();
            getUserGroup();
            //敲回车键执行方法
            enterSearch(search);
            //初始化日历控件，前面的日期不能大于后面的日期
            $('#txtCreateBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtCreateEndTime\')}', onpicked: function () { document.getElementById("txtCreateEndTime").focus(); } }); });
            $('#txtCreateEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateBeginTime\')}' }); });


            BindCategory("sltChildCategory", 2);
            BindCategory("sltChildCategory2", 3);
            BindWorkOrderTag("sltWTag1", 1);
            BindWorkOrderTag("sltWTag2", 2);
            //search();
        });

        function PlayRecordList(obj) {
            var orderid = $(obj).attr("OrderID");
            //根据工单ID查看是否有录音
            var para;
            para = {
                Action: "GetWorkOrderRecordUrl", OrderID: orderid
            };
            AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", para, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "true") {
                    $.openPopupLayer({
                        name: 'PlayRecordLayer',
                        url: "/CTI/PlayRecordList.aspx",
                        parameters: { 'RecordURL': jsonData.RecordURL, 'OrderID': orderid },
                        popupMethod: 'Post'
                    });
                }
                else {
                    $.jAlert("当前工单无录音!");
                    return;
                }
            });

        }

        //绑定标签
        function BindWorkOrderTag(id, level) {
            $("#" + id + " option").remove();
            var pid = 0;
            var bgid = 0;

            bgid = $("#[id$='sltBG']").val();
            if (level == 1) {

            }
            else if (level == 2) {
                pid = $("#[id$='sltWTag1']").val();
            }

            var para;
            para = {
                Action: "GetWorkOrderTag", BGID: bgid, PID: pid, r: Math.random()
            };
            AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", para, null, function (data) {
                $("#" + id).append("<option value='-2'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#" + id + "").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
            if (level == 1) {
                $("#sltWTag1").trigger("change");
            }
        }

        //绑定二级工单分类
        function BindCategory(id, level) {
            $("#" + id + " option").remove();
            var pid = 0;
            if (level == 2) {
                pid = $("#[id$='sltParentCategory']").val();
            }
            else if (level == 3) {
                pid = $("#[id$='sltChildCategory']").val();
            }
            AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "GetCategory", PID: pid, r: Math.random() }, null, function (data) {
                $("#" + id).append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#" + id + "").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
            if (level == 2) {
                $("#sltChildCategory").trigger("change");
            }
        }

        var param = {};

        function search() {
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间

            var orderId = $.trim($("#txtOrderID").val());
            var custName = $.trim($("#txtCustName").val());
            var createBeginTime = $.trim($("#txtCreateBeginTime").val());
            var createEndTime = $.trim($("#txtCreateEndTime").val());
            var createUserId = $.trim($("#hdnCreateUserID").val());
            var workOrderStatus = $.trim($("#<%=this.sltWorkOrderStatus.ClientID %>").val());
            var categoryId = $.trim($("#sltChildCategory2").val());
            if (categoryId == "-1" || categoryId == undefined) {
                categoryId = $("#sltChildCategory").val();
            }
            if (categoryId == "-1" || categoryId == undefined) {
                categoryId = $("#<%=this.sltParentCategory.ClientID %>").val();
            }
            var priorityLevel = $.trim($("#<%=this.sltPriorityLevel.ClientID %>").val());
            var isReCheck = "";
            var workCategory = $("#sltWorkCategory").val();
            var areaId = $("[id$='sltArea']").val();
            var contactTel = $('#txtContactTel').val();
            var bgId = $("#sltGroup").val();

            var tagbgid = $("#ctl00_ContentPlaceHolder1_sltBG").val();
            var tagid = $("#sltWTag2").val();
            if (tagid == "-2") {
                tagid = $("#sltWTag1").val();

            }

            if (CheckForSelectDate("txtCreateBeginTime", "txtCreateEndTime")) {
                param = {
                    OrderID: escape(orderId),
                    CustName: escape(custName),
                    BeginCreateTime: escape(createBeginTime),
                    EndCreateTime: escape(createEndTime),
                    CreateUserID: escape(createUserId),
                    WorkOrderStatus: escape(workOrderStatus),
                    CategoryID: escape(categoryId),
                    PriorityLevel: escape(priorityLevel),
                    IsReCheck: escape(isReCheck),
                    WorkCategory: escape(workCategory),
                    AreaID: areaId,
                    ContactTel: escape(contactTel),
                    BGID: bgId,
                    TagBGID: tagbgid,
                    TagID: tagid,
                    OrderCreateTime: $("#hidOrderCreateTime").val(),
                    R: Math.random()
                }
                LoadingAnimation('ajaxTable');
                $("#ajaxTable").load("/WorkOrder/AjaxServers/WorkOrderList.aspx", param, function () {
                    var strParam = JSON.stringify(param);
                    var reg = new RegExp(":", "g");
                    var reg1 = new RegExp(",", "g");
                    var reg2 = new RegExp("\"", "g");
                    strParams = strParam.replace(reg, "=");
                    strParams = strParams.replace(reg1, "&");
                    strParams = strParams.replace(reg2, "");
                    strParams = strParams.substring(1, strParams.length - 1);
                    StatAjaxPageTime(monitorPageTime, "/WorkOrder/AjaxServers/WorkOrderList.aspx" + "?" + strParams);
                });
            }
        }
        function CheckForSelectDate(beginid, endid) {
            var begintime = $.trim($("#" + beginid).val());
            var endtime = $.trim($("#" + endid).val());

            if (!begintime || begintime == "" || begintime == undefined) {
                $.jAlert("起始提交日期不能为空", function () { $("#" + beginid).focus(); });
                return false;
            }
            else if (!endtime || endtime == "" || endtime == undefined) {
                $.jAlert("终止提交日期不能为空", function () { $("#" + endid).focus(); });
                return false;
            }
            else {
                var newEndTime = new Date(endtime).getTime();
                var newBeginTime = new Date(begintime).getTime();
                var numDays = (newEndTime - newBeginTime) / 24 / 60 / 60 / 1000;
                if (numDays > 90) {
                    $.jAlert("提交日期最大跨度不能超过90天", function () { $("#" + beginid).focus(); });
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        function SelectCreateUser(valueObj, NameObj) {
            $.openPopupLayer({
                name: "SelectUserPopup",
                parameters: {},
                url: "/WorkOrder/AjaxServers/WorkOrderCreateUserPoper.aspx",
                afterClose: function (e, data) {
                    if (e) {
                        $("#txtCreateUserName").val(data.UserName);
                        $("#hdnCreateUserID").val(data.UserID);
                    }
                    enterSearch(search);
                }
            });
        }

        //选择操作人
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName },
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                    ;
                }
            });
        }

        function ExportData() {
            var paramsStr = "";
            for (var attr in param) {
                paramsStr += "&" + attr + "=" + param[attr];
            }
            window.location = "/WorkOrder/ExportWorkOrder.aspx?Browser=" + GetBrowserName() + paramsStr;
        }
        //工单类型改变
        function ChangeWorkCategory() {
            var categoryId = $("#sltWorkCategory").val();
            if (categoryId == "1") {
                InitLastUlControl();
                $("ul[name='lastul'] li").slice(2, 6).hide();
                $(".btnsearch").css({ "margin-top": "-25px", "margin-left": "600px" })
            }
            else {
                $("ul[name='lastul'] li").slice(2, 6).show();
                $(".btnsearch").css({ "margin-top": "10px", "margin-left": "0px" })
            }
        }
        function InitLastUlControl() {
            $("#txtCustName").val("");
            $("select[id$='sltPriorityLevel']").val(-1);
            $("select[id$='sltArea']").val(-1);
            $('#txtContactTel').val("");
        }


        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#sltGroup").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#sltGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        function BindOrderCreateTime() {
            $("#hidOrderCreateTime").val("1");
            search();
            //            $("#hidOrderCreateTime").val("");
        }
        function search2() {
            //先清空按创建时间排序条件
            $("#hidOrderCreateTime").val("");
            search();
        }
    </script>
    <div class="searchTj" style="width: 100%;">
        <input type="hidden" id="hidOrderCreateTime" value="" />
        <ul class="clear" style="width: 98%;">
            <li>
                <label>
                    工单类型：</label>
                <span>
                    <select id="sltWorkCategory" class="w200" style='width: 206px;' onchange="ChangeWorkCategory()">
                        <option value="2">经销商</option>
                        <option value="1">个人</option>
                    </select></span> </li>
            <li>
                <label>
                    工单ID：</label>
                <input type="text" id="txtOrderID" name="OrderID" class="w200" />
            </li>
            <li>
                <label>
                    提交日期：</label>
                <input type="text" name="CreatBeginTime" id="txtCreateBeginTime" value='<%=DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd") %>'
                    class="w95" />-<input type="text" name="CreatEndTime" id="txtCreateEndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                        class="w95" />
            </li>
        </ul>
        <ul class="clear" style="width: 98%;">
            <li>
                <label>
                    提交人：</label>
                <input type="text" id="txtCreateUserName" class="w200" onclick="SelectVisitPerson('workordersubmituser','txtCreateUserName','hdnCreateUserID')"
                    readonly="readonly" />
                <input type="hidden" id="hdnCreateUserID" />
            </li>
            <li>
                <label>
                    工单状态：</label><span><select id="sltWorkOrderStatus" class="w200" style='width: 206px;'
                        runat="server"></select></span></li>
            <li>
                <label>
                    工单分类：</label>
                <select id="sltParentCategory" class="kProvince" onchange="BindCategory('sltChildCategory',2)"
                    runat="server" style="width: 66px">
                </select>
                <select id="sltChildCategory" onchange="BindCategory('sltChildCategory2',3)" style="width: 66px">
                </select>
                <select id="sltChildCategory2" class="kArea" style="width: 66px">
                </select>
            </li>
        </ul>
        <ul name="lastul" class="clear" style="width: 98%;">
            <li>
                <label>
                    所属分组：</label>
                <select id="sltGroup" class="w200" style='width: 206px;'>
                </select>
            </li>
            <li>
                <label>
                    标签：</label>
                <select id="sltBG" onchange="BindWorkOrderTag('sltWTag1',1)" runat="server" style="width: 66px">
                </select>
                <select id="sltWTag1" onchange="BindWorkOrderTag('sltWTag2',2)" style="width: 66px">
                </select>
                <select id="sltWTag2" style="width: 66px">
                </select>
            </li>
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" class="w200" />
            </li>
        </ul>
        <ul name="lastul" class="clear" style="width: 98%;">
            <li>
                <label>
                    优先级：</label>
                <span>
                    <select id="sltPriorityLevel" class="w200" style='width: 206px;' runat="server">
                    </select></span> </li>
            <li>
                <label>
                    大区：</label>
                <span>
                    <select id="sltArea" runat="server" class="w200" style='width: 206px;'>
                    </select></span> </li>
            <li>
                <label>
                    联系电话：</label>
                <input type="text" id="txtContactTel" class="w200" />
            </li>
            <li class="btnsearch" style="clear: none; width: 120px; margin-top: 10px;">
                <input type="button" value="查 询" id="Button1" class="cx" onclick="search2()" name="" /></li>
        </ul>
    </div>
    <div class="bit_table" id="ajaxTable" style="min-width: 1050px;">
        <div class="optionBtn clearfix">
            <%if (canExport)
              { %>
            <input name="" type="button" value="导出" id="lbtnExport" onclick="" class="newBtn mr10" />
            <%} %>
            <span>查询结果</span><small><span>总计: 0</span></small>&nbsp;&nbsp;&nbsp;&nbsp;<span><a
                onclick="" id="aCreateTime" style="cursor: pointer" href="javascript:void(0)">排序：按创建时间</a></span>
        </div>
    </div>
</asp:Content>
