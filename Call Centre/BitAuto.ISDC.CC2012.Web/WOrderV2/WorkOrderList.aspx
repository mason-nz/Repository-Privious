<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.WorkOrderList" MasterPageFile="~/Controls/Top.Master"
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
        $(function () {
            getUserGroup();
            //敲回车键执行方法
            enterSearch(Search);
            //初始化日历控件，前面的日期不能大于后面的日期
            $('#txtCreateBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtCreateEndTime\')}', onpicked: function () { document.getElementById("txtCreateEndTime").focus(); } }); });
            $('#txtCreateEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateBeginTime\')}' }); });
            GetBusinessType();
            ChangeBusiType();
        });
        var param = {};
        function CheckForSelectDate(beginid, endid) {
            var begintime = $.trim($("#" + beginid).val());
            var endtime = $.trim($("#" + endid).val());

            if (!begintime || begintime == "" || begintime == undefined) {
                $.jAlert("起始日期不能为空", function () { $("#" + beginid).focus(); });
                return false;
            }
            else if (!endtime || endtime == "" || endtime == undefined) {
                $.jAlert("终止日期不能为空", function () { $("#" + endid).focus(); });
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
        //加载业务类型
        function GetBusinessType() {
            var data = { action: "GetSelectData" };
            AjaxPostAsync("/WOrderV2/Handler/BusinessTypeHandler.ashx", data, null, function (data) {
                if (data) {
                    data = JSON.parse(data);
                    if (data.Success == true) {
                        var jsonData = data.Data;
                        if (jsonData != null) {
                            $("#sel_busiType").empty();
                            $("#sel_busiType").append("<option value='-1'>请选择</option>");
                            for (var i = 0; i < jsonData.length; i++) {
                                $("#sel_busiType").append("<option value=" + jsonData[i].Id + ">" + jsonData[i].Name + "</option>");
                            }

                        }

                    } else {
                    }
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
                    enterSearch(Search);
                    ;
                }
            });
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
        //业务线change事件 联动一级标签
        function ChangeBusiType() {
            $("#sltWTag1,#sltWTag2").html("<option value='-1'>请选择</option>");
            var selTypeVal = $.trim($("#sel_busiType").val());
            if (selTypeVal == '-1') {
                $("#sltWTag1,#sltWTag2").attr("disabled", "disabled");
            }
            else {
                $("#sltWTag1,#sltWTag2").removeAttr("disabled");
                var busiTypeId = $.trim($("#sel_busiType").val());
                AjaxPostAsync("Handler/TagHandler.ashx", { Action: "GetData", BusiTypeId: busiTypeId, PID: "0", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.Success) {
                        if (jsonData.Data != "") {
                            var jdata = jsonData.Data;
                            for (var i = 0; i < jdata.length; i++) {
                                $("#sltWTag1").append("<option value=" + jdata[i].RecID + ">" + jdata[i].TagName + "</option>");
                            }
                        }
                    }

                });
            }
        }
        //显示投诉级别
        function ChangeWorkCategory() {
            if ($("#[id$='sltWorkCategory']").val() == '<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉 %>') {
                $("#complaintLevelLI").css("visibility", "visible");
            }
            else {
                $("#complaintLevelLI").css("visibility", "hidden");
                $("input:checkbox[name='chkComplaintLevel']").removeAttr("checked");
            }
        }
        //根据一级标签联动二级标签
        function ChangeWTag() {
            var sltWTag1Val = $.trim($("#sltWTag1").val());
            $("#sltWTag2").html(" <option value='-1'>请选择</option>");
            if (sltWTag1Val != '-1') {

                $("#sltWTag1,#sltWTag2").removeAttr("disabled");
                var pid = $.trim($("#sltWTag1").val());
                AjaxPostAsync("Handler/TagHandler.ashx", { Action: "GetData", PID: pid, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.Success) {
                        if (jsonData != "") {
                            var jdata = jsonData.Data;
                            for (var i = 0; i < jdata.length; i++) {
                                $("#sltWTag2").append("<option value=" + jdata[i].RecID + ">" + jdata[i].TagName + "</option>");
                            }
                        }
                    }
                });
            }
        }
        //查询
        function Search(refresh) {
            //用户联系方式
            var contactTel = $.trim($('#txtContactTel').val());
            if (contactTel != "") {
                if (!isTelOrMobile(contactTel)) {
                    $.jAlert("用户联系方式格式不正确！", function () { $("#txtContactTel").focus(); });
                    return;
                }
            }
            var ComplaintLevelObj = $("input:checkbox[name='chkComplaintLevel']:checked"); //投诉级别

            var ComplaintLevel = "";
            if (ComplaintLevelObj.length > 0) {

                for (var i = 0; i < ComplaintLevelObj.length; i++) {
                    ComplaintLevel += $(ComplaintLevelObj[i]).val() + "-";
                }
                ComplaintLevel = $.trim(ComplaintLevel.substring(0, ComplaintLevel.length - 1));
            }
            if ($("#[id$='sltWorkCategory']").val() != '<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉 %>') {
                if (ComplaintLevel != "") {
                    $.jAlert("工单类型不是投诉时，不能选择投诉级别！");
                    return;
                }
            }
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            var orderId = $.trim($("#txtOrderID").val()); //工单ID
            var custName = $.trim($("#txtCustName").val()); //经销商名字
            var createBeginTime = $.trim($("#txtCreateBeginTime").val()); //提交开始时间
            var createEndTime = $.trim($("#txtCreateEndTime").val()); //提交结束时间
            var createUserId = $.trim($("#hdnCreateUserID").val()); //提交人
            var workOrderStatus = $.trim($("#<%=this.sltWorkOrderStatus.ClientID %>").val()); //工单状态
            var busitype = $.trim($("#sel_busiType").val()); //业务类型
            var isReVisitObj = $("input:checkbox[name='chkIsReturnSelect']:checked");
            var isReVisit = ""; //是否回访
            if (isReVisitObj.length == 1) {
                isReVisit = $.trim($(isReVisitObj[0]).val());
            }
            else if (isReVisitObj.length == 2) {
                isReVisit = "0-1";
            }
            else {
                isReVisit = "";
            }
            var workCategory = $.trim($("#[id$='sltWorkCategory']").val());
            var bgId = $.trim($("#sltGroup").val()); //所属分组
            var bigtagid = $.trim($("#sltWTag1").val()); //标签
            var tagid = $.trim($("#sltWTag2").val()); //标签
            if (bigtagid == "-1") {
                tagid = "-1";
            }
            if (CheckForSelectDate("txtCreateBeginTime", "txtCreateEndTime")) {
                param = {
                    OrderID: escape(orderId),
                    CustName: escape(custName),
                    BeginCreateTime: escape(createBeginTime),
                    EndCreateTime: escape(createEndTime),
                    CreateUserID: escape(createUserId),
                    WorkOrderStatus: escape(workOrderStatus),
                    Phone: escape(contactTel),
                    BGID: bgId,
                    TagID: tagid,
                    BigTagID: bigtagid,
                    CategoryID: escape(workCategory),
                    ReVisitStr: isReVisit,
                    BusiType: busitype,
                    ComplaintLevel: ComplaintLevel,
                    R: Math.random()
                }
                if (refresh == "refresh") {
                    param.page = $.trim($("#input_page").val());
                }
                LoadingAnimation('ajaxTable');
                //console.info(param.ComplaintLevel);
                var strParam = JSON.stringify(param);
                var reg = new RegExp(":", "g");
                var reg1 = new RegExp(",", "g");
                var reg2 = new RegExp("\"", "g");
                var strParams = strParam.replace(reg, "=");
                strParams = strParams.replace(reg1, "&");
                strParams = strParams.replace(reg2, "");
                strParams = strParams.substring(1, strParams.length - 1);
                $("#ajaxTable").load("/WOrderV2/AjaxServers/WorkOrderList.aspx" + "?" + strParams, null, function () {

                    StatAjaxPageTime(monitorPageTime, "/WOrderV2/AjaxServers/WorkOrderList.aspx" + "?" + strParams);
                });
            }
        }
        //刷新
        function Refresh() {
            Search("refresh");
        }
        //新增工单
        function OpenAddWorkOrder() {
            var root = "<%=ExitAddress %>";
            var url = "/WOrderV2/AddWOrderInfo.aspx?<%=Param %>";
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + url);
            }
            catch (e) {
                window.open(url);
            }
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load("/WOrderV2/AjaxServers/WorkOrderList.aspx?r=" + Math.random(), pody, null);
        }
        //导出
        function ExportData() {
            var contactTel = $.trim($('#txtContactTel').val());
            if (contactTel != "") {
                if (!isTelOrMobile(contactTel)) {
                    $.jAlert("用户联系方式格式不正确！", function () { $("#txtContactTel").focus(); });
                    return;
                }
            }

            var ComplaintLevelObj = $("input:checkbox[name='chkComplaintLevel']:checked"); //投诉级别
            var ComplaintLevel = "";
            if (ComplaintLevelObj.length > 0) {
                for (var i = 0; i < ComplaintLevelObj.length; i++) {
                    ComplaintLevel += $(ComplaintLevelObj[i]).val() + ","
                }
                ComplaintLevel = $.trim(ComplaintLevel.substring(0, ComplaintLevel.length - 1));
            }

            if ($("#[id$='sltWorkCategory']").val() != '<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉 %>') {
                if (ComplaintLevel != "") {
                    $.jAlert("工单类型不是投诉时，不能选择投诉级别！");
                    return;
                }
            }
            var paramsStr = "";
            for (var attr in param) {
                paramsStr += "&" + attr + "=" + param[attr];
            }
            $("#formExport").attr("action", "/WOrderV2/ExportWorkOrder.aspx?Browser=" + GetBrowserName() + paramsStr);
            $("#formExport").submit();
        }
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul class="clear" style="width: 98%;">
            <li>
                <label>
                    工单状态：</label><span><select id="sltWorkOrderStatus" class="w200" style='width: 206px;'
                        runat="server"></select></span> </li>
            <li>
                <label>
                    业务类型：</label><span><select id="sel_busiType" class="w200" style='width: 206px;' onchange="ChangeBusiType()"></select></span>
            </li>
            <li>
                <label>
                    标签：</label>
                <select id="sltWTag1" onchange="ChangeWTag()" style="width: 99px" disabled="disabled">
                </select>
                <select id="sltWTag2" style="width: 99px" disabled="disabled">
                </select>
            </li>
        </ul>
        <ul class="clear" style="width: 98%;">
            <li>
                <label>
                    所属分组：</label>
                <select id="sltGroup" class="w200" style='width: 206px;'>
                </select>
            </li>
            <li>
                <label>
                    提交人：</label>
                <input type="text" id="txtCreateUserName" class="w200" onclick="SelectVisitPerson('worderV2submituser','txtCreateUserName','hdnCreateUserID')"
                    readonly="readonly" />
                <input type="hidden" id="hdnCreateUserID" />
            </li>
            <li>
                <label>
                    提交时间：</label>
                <input type="text" name="CreatBeginTime" id="txtCreateBeginTime" value='<%=DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd") %>'
                    class="w95" />-<input type="text" name="CreatEndTime" id="txtCreateEndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                        class="w95" />
            </li>
        </ul>
        <ul name="lastul" class="clear" style="width: 98%;">
            <li>
                <label>
                    经销商名称：</label>
                <input type="text" id="txtCustName" name="CustName" class="w200" />
            </li>
            <li>
                <label>
                    工单ID：</label>
                <input type="text" id="txtOrderID" name="OrderID" class="w200" />
            </li>
            <li>
                <label>
                    用户号码：</label>
                <input type="text" id="txtContactTel" class="w200" />
            </li>
        </ul>
        <ul class="clear" style="width: 98%;">
            <li>
                <label>
                    是否回访：</label>
                <span>
                    <input type="checkbox" name="chkIsReturnSelect" value="1" style="border: 0px" />
                    <em onclick="emChkIsChoose(this)">已回访</em> </span><span>
                        <input type="checkbox" name="chkIsReturnSelect" value="0" style="border: 0px" />
                        <em onclick="emChkIsChoose(this)">未回访</em> </span></li>
            <li>
                <label>
                    工单类型：</label>
                <span>
                    <select id="sltWorkCategory" class="w200" style='width: 206px;' runat="server" onchange="ChangeWorkCategory()">
                    </select></span> </li>
            <li id="complaintLevelLI" style="visibility: hidden">
                <label>
                    投诉级别：</label>
                <span>
                    <input type="checkbox" name="chkComplaintLevel" value="1" style="border: 0px" />
                    <em onclick="emChkIsChoose(this)">A级</em> </span><span>
                        <input type="checkbox" name="chkComplaintLevel" value="2" style="border: 0px" />
                        <em onclick="emChkIsChoose(this)">B级</em> </span><span>
                            <input type="checkbox" name="chkComplaintLevel" value="3" style="border: 0px" />
                            <em onclick="emChkIsChoose(this)">常规</em> </span></li>
            <li class="btnsearch" style="clear: none; width: 120px; margin-top: 10px;">
                <input type="button" value="查 询" id="Button1" class="cx" onclick="Search()" name="" />
                <input type="button" value="刷新" onclick="Refresh()" id="btnsearch" style="display: none" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (canExport)
          { %>
        <input name="" type="button" value="导出" id="lbtnExport" onclick="ExportData();" class="newBtn mr10" />
        <%} %>
        <input name="" type="button" value="新增工单" onclick="OpenAddWorkOrder();" class="newBtn mr10" />
    </div>
    <div class="bit_table" id="ajaxTable" style="min-width: 1050px;">
    </div>

    <form id="formExport" method="post">  
    </form>
</asp:Content>
