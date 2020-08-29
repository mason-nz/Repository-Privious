<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerControl.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.UControl.DealerControl" %>
<link href="../../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
<script src="../../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
<style type="text/css">
    .pop .ul_auto
    {
        position: absolute;
        left: 126px;
        top: 34px;
        width: 245px;
        height: 100px;
        background: #eee;
        border: 1px solid #666;
        z-index: 99999;
        overflow: hidden;
        padding: 2px;
    }
    .pop .ul_auto li
    {
        width: 245px;
        height: 30px;
        font-size: 14px;
        color: #333;
        cursor: pointer;
    }
    
    #uc_DealerInfo li .coupon-box02
    {
        width: 246px;
    }
    #uc_DealerInfo li .coupon-box02 input
    {
        border: none;
    }
    
    #uc_DealerInfo li .coupon-box02
    {
        position: relative;
        top: 5px;
    }
</style>
<ul class="clearfix " name="uc_DealerInfo" id="uc_DealerInfo">
    <li style="width: 900px;">
        <label>
            工单类型：</label>
        <span>
            <input id="rdoWorkOrderPerson3" type="radio" value="4" onclick="orderSwitch('4')"
                name="OrderCategory3" />
            <em onclick="emChkIsChoose(this);orderSwitch('4');">个人</em> </span>&nbsp;&nbsp;<span>
                <input id="rdoWorkOrderDealer3" type="radio" value="3" name="OrderCategory3" onclick="orderSwitch('3')"
                    checked="checked" />
                <em onclick="emChkIsChoose(this);orderSwitch('3');">经销商</em> </span>
    </li>
    <li>
        <label>
            <span class="redColor">*</span>工单分类：</label><span><select name="CategoryID1" vtype="notFirstOption"
                id="selCategoryID1" onchange="uc_BindCategory2('selCategoryID2','selCategoryID1')"
                vmsg="请选择工单一级分类！" class="w80"><option value='-1'>请选择</option>
            </select></span>&nbsp;&nbsp;<span><select name="CategoryID2" onchange="uc_BindCategory3('selCategoryID','selCategoryID2')"
                class="w80" vtype="notFirstOption" id="selCategoryID2" vmsg="请选择工单二级分类！"><option
                    value='-1'>请选择</option>
            </select></span>&nbsp;&nbsp;<span><select name="CategoryID" class="w80" vtype="notFirstOption"
                id="selCategoryID" vmsg="请选择工单三级分类！" onchange="uc_BindCategory4()"><option value='-1'>
                    请选择</option>
            </select></span></li>
    <li>
        <label>
            <span class="redColor">*</span>工单来源：</label><span><select name="DataSource" class="w250"
                id="selDataSource" style="width: 253px;" vtype="notFirstOption" vmsg="请选择工单来源！"><option
                    value='-1'>请选择</option>
            </select></span></li>
    <li>
        <label>
            <span class="redColor">*</span>客户名称：</label><span><input name="CustName" type="text"
                class="w250" id="txtCustName" vtype="isNull" vmsg="请选择客户！" /></span>&nbsp;<a name="ucDealer_aCustName"
                    id="aCustName" href="javascript:void(0);" onclick="selectCustInfo()">查询</a>
        <input type="hidden" id="hidCustId" name="hidCustId" />
    </li>
    <li>
        <label>
            <span class="redColor">*</span>客户地区：</label><span><select name="Province" class="w80"
                id="selProvince" vtype="notFirstOption" vmsg="请选择客户地区省份！" onchange="uc_triggerProvince()">
            </select>
            </span>&nbsp;&nbsp;<span><select name="City" class="w80" id="selCity" vtype="notFirstOption"
                vmsg="请选择客户地区城市！" onchange="uc_triggerCity()">
            </select>
            </span>&nbsp;&nbsp;<span><select name="County" class="w70" id="selCounty" vtype="notFirstOption"
                vmsg="请选择客户地区区县！">
            </select>
            </span></li>
    <li style="position: relative; z-index: 999;" id="popAutoLi">
        <label>
            <span class="redColor">*</span>联系人：</label><span><input name="Contact" type="text"
                dbfield="Contact" class="w250" vtype="isNull" vmsg="请填写联系人！" id="txtContact" /></span></li>
    <li>
        <label>
            <span class="redColor">*</span>联系电话：</label><span><input name="ContactTel" id="txtContactTel"
                dbfield="ContactTel" type="text" class="w250" vtype="isNull|isTelOrMobile" vmsg="请填写联系电话！|联系电话格式不正确！" /></span></li>
    <li style="display: none" id="lidcSex">
        <label>
            <span class="redColor">*</span>性别：</label><span><input type="radio" name="dcSex"
                id="rdoMan" value="1" /><em onclick="emChkIsChoose(this);">先生</em><input type="radio"
                    id="rdoWomen" name="dcSex" style="margin-left: 50px;" value="2" /><em onclick="emChkIsChoose(this);">女士</em></span></li>
    <li id="dcMember" style="display: none">
        <label>
            经销商名称：</label><span>
                <select id="popSelMemberName" class="w250" style="width: 253px;" runat="server">
                </select></span></li>
    <li>
        <label>
            <span class="redColor">*</span>优先级：</label><span><select name="PriorityLevel" id="selPriorityLevel"
                dbfield="PriorityLevel" class="w250" style="width: 252px;" vtype="notFirstOption"
                vmsg="请选择优先级！"><option>请选择</option>
            </select></span></li>
    <li>
        <label>
            <span class="redColor">*</span>最晚处理日期：</label><span>
                <input name="LastProcessDate" id="txtLastProcessDate" type="text" class="w250" vtype="isNull|isDate"
                    vmsg="请填写最晚处理日期！|最晚处理日期格式不正确！" />
            </span></li>
    <li>
        <label>
            工单类型：</label><span><input name="IsComplaintType" type="checkbox" value="true" class="dx"
                id="chkIsComplaintType" /><em onclick="emChkIsChoose(this);">投拆</em></span></li>
    <li>
        <label id="lbRelateDemands">
            关联需求：
        </label>
        <input type="text" class="w250" name="txtSearchRelateDemands" id="txtSearchRelateDemands"
            runat="server" readonly="readonly" onclick="openAjaxSearchRelateDemandsAjaxPopup()" />
    </li>
    <li>
        <label>
            工单标题：</label><span><input name="Title" type="text" lenstr="30" class="w250" vtype="Len"
                vmsg="工单标题已超过15个字！" id="txtTitle" /></span></li>
    <li>
        <label>
            <span class="redColor">*</span>标签：</label><div class="coupon-box02" style="float: left;
                width: 248px;">
                <input type="text" value="" class="text02" vtype="isNull" vmsg="请选择标签！" id="txtTagInfo"><b><a
                    href="#">选择</a></b></div>
    </li>
    <li class="gdjl">
        <label>
            <span class="redColor">*</span>工单记录：</label><span><textarea id="areaContent" vtype="isNull|Len"
                lenstr="600" vmsg="请填写工单记录！|工单记录已超过300个字！" name="Content"></textarea></span>
    </li>
</ul>
<%-- 任务来源  0:直接创建 1：呼入创建 2：呼出创建 3：IM创建--%>
<input type="hidden" id="hidTaskSource" value="0" />
<script type="text/javascript">
    function openAjaxSearchRelateDemandsAjaxPopup() {
        if ($("#hidCustId").val == "" || $.trim($("#txtCustName").val()) == "") {
            $.jAlert("请先选择\"客户名称\"!", function () {
                return false;
            });
        } else {

            $.openPopupLayer({
                name: "RelateDemandSelectAjaxPopup",
                parameters: {},
                url: "/WorkOrder/RelatedDemandsInfo.aspx?CustId=" + $('#hidCustId').val() + "&phoneNum=" + $("#CustomRealRelatePhone").val() + "&carBrandID=&carTypeID=&r=" + Math.random(),
                beforeClose: function (b, cData) {
                    var ProjectName = $('#popupLayer_' + 'RelateDemandSelectAjaxPopup').data('RelateDemandId');
                    $("input[id$='txtSearchRelateDemands']").val(ProjectName);
                }
            });
        }
    }
    //插入工单表的信息
    function GetOperData() {

        var operData = "";
        if ($("#selCategoryID option").length > 1) {
            operData += "CategoryID=" + $("#selCategoryID").val() + "&";
        }
        else {
            operData += "CategoryID=" + $("#selCategoryID2").val() + "&";
        }

        operData += "DataSource=" + $("#selDataSource").val() + "&";

        operData += "CustName=" + encodeURIComponent($.trim($("#txtCustName").val())) + "&";

        operData += "ProvinceID=" + $("#selProvince").val() + "&";

        operData += "CityID=" + $("#selCity").val() + "&";

        operData += "CountyID=" + $("#selCounty").val() + "&";

        operData += "Contact=" + encodeURIComponent($.trim($("#txtContact").val())) + "&";

        operData += "ContactTel=" + $.trim($("#txtContactTel").val()) + "&";

        operData += "PriorityLevel=" + $.trim($("#selPriorityLevel").val()) + "&";

        operData += "LastProcessDate=" + $.trim($("#txtLastProcessDate").val()) + "&";

        operData += "TagInfo=" + $.trim($("#hidTagInfo").val()) + "&";

        operData += "IsComplaintType=" + $("#chkIsComplaintType").is(":checked") + "&";

        operData += "Title=" + encodeURIComponent($.trim($("#txtTitle").val())) + "&";

        operData += "Content=" + encodeURIComponent($.trim($("#areaContent").val())) + "&";

        operData += "DemandID=" + encodeURIComponent($.trim($("#txtSearchRelateDemands").val())) + "&";

        var csid = "<%=RequestCSID %>"
        operData += "CSID=" + csid + "&";
        var lyid = "<%=RequestLYID %>"
        operData += "LYID=" + lyid + "&";
        var cartype = "<%=RequestCarType %>"
        operData += "CarType=" + cartype + "&";
        var systype = "<%=RequestSYSType %>"
        operData += "SYSType=" + systype + "&";
        operData = operData.substring(0, operData.length - 1);

        return operData;
    }

    //插入工单回复表的信息
    function GetRevertData() {

        var revertData = "";

        if ($("#selCategoryID option").length > 1) {
            revertData += "CategoryName=" + encodeURIComponent($("#selCategoryID1 option:selected").text() + " " + $("#selCategoryID2 option:selected").text() + " " + $("#selCategoryID option:selected").text()) + "&";
        }
        else {
            revertData += "CategoryName=" + encodeURIComponent($("#selCategoryID1 option:selected").text() + " " + $("#selCategoryID2 option:selected").text()) + "&";
        }

        revertData += "DataSource=" + encodeURIComponent($("#selDataSource option:selected").text()) + "&";

        revertData += "CustName=" + encodeURIComponent($.trim($("#txtCustName").val())) + "&";

        revertData += "ProvinceName=" + encodeURIComponent($("#selProvince option:selected").text()) + "&";

        revertData += "CityName=" + encodeURIComponent($("#selCity option:selected").text()) + "&";

        revertData += "CountyName=" + encodeURIComponent($("#selCounty option:selected").text()) + "&";

        revertData += "Contact=" + encodeURIComponent($.trim($("#txtContact").val())) + "&";

        revertData += "ContactTel=" + encodeURIComponent($.trim($("#txtContactTel").val())) + "&";

        revertData += "PriorityLevelName=" + encodeURIComponent($.trim($("#selPriorityLevel option:selected").text())) + "&";

        revertData += "LastProcessDate=" + $.trim($("#txtLastProcessDate").val()) + "&";

        revertData += "TagName=" + encodeURIComponent($.trim($("#txtTagInfo").val())) + "&";

        revertData += "IsComplaintType=" + encodeURIComponent($("#chkIsComplaintType").is(":checked") ? "是" : "否") + "&";

        revertData += "Title=" + encodeURIComponent($.trim($("#txtTitle").val())) + "&";

        revertData += "RevertContent=" + encodeURIComponent($.trim($("#areaContent").val())) + "&";

        //revertData += "DemandID=" + encodeURIComponent($.trim($("#txtSearchRelateDemands").val())) + "&"; 

        revertData = revertData.substring(0, revertData.length - 1);

        return revertData;
    }

    //插入个人信息表数据(如果显示情况下) add lxw 13.11.25
    function GetCustBasicInfo() {
        var custInfo = "";

        custInfo += "CustName=" + encodeURIComponent($.trim($("#txtContact").val())) + "&";

        custInfo += "Sex=" + $("[name='dcSex']:checked").map(function () {
            return $(this).val();
        }).get().join(',') + "&";

        custInfo += "CustCategoryID=3&";    //3-经销商

        custInfo += "MemberName=" + encodeURIComponent($("#<%=popSelMemberName.ClientID %>").text()) + "&";

        custInfo += "MemberID=" + encodeURIComponent($("#<%=popSelMemberName.ClientID %>").val()) + "&";

        custInfo += "Tel=" + encodeURIComponent($.trim($("#txtContactTel").val())) + "&";

        custInfo = custInfo.substring(0, custInfo.length - 1);

        return custInfo;
    }

    //验证
    function ucDealer_Validate(type) {

        var jsonValidateData = "";

        $("#selPriorityLevel").attr("vtype", "notFirstOption");
        $("#txtLastProcessDate").attr("vtype", "isNull|isDate").attr("vmsg", "请填写最晚处理日期！|最晚处理日期格式不正确！");

        //如果是添加，优先级可以不验证，最晚处理日期只验证格式
        if (type == 1) {
            $("#selPriorityLevel").removeAttr("vtype");
            $("#txtLastProcessDate").attr("vtype", "isDate").attr("vmsg", "最晚处理日期格式不正确！");
        }

        var ulHtml = $("#uc_DealerInfo").html();
        ulHtml = "<form name='uc_DealerInfoForm'>" + ulHtml + "</form>"; //ul

        var serializeData = $(ulHtml).serializeArray();

        jsonValidateData = $.evalJSON(validateMsg(serializeData));

        if ($.trim($("#txtLastProcessDate").val()) != "" && $.trim($("#txtLastProcessDate").val()) < "<%=today %>") {
            jsonValidateData.result = "false";
            jsonValidateData.msg = jsonValidateData.msg == undefined ? "最晚处理日期不能小于当前时间！<br/>" : jsonValidateData.msg + "最晚处理日期不能小于当前时间！<br/>";
        }

        //验证是否选择性别
        if ($("#lidcSex").css("display") != "none") {
            var oSex = document.getElementsByName("dcSex");
            var sex = "";
            for (var i = 0; i < oSex.length; i++) {
                if (oSex[i].checked == true) {
                    sex = oSex[i].value;
                    break;
                }
            }
            if (sex == "") {
                jsonValidateData.result = "false";
                jsonValidateData.msg = jsonValidateData.msg == undefined ? "请选择联系人性别！<br/>" : jsonValidateData.msg + "请选择联系人性别！<br/>";
            }
        }
        var category1Val = $("#selCategoryID1").val();
        var category2Val = $("#selCategoryID2").val();
        var category3Val = $("#selCategoryID").val();
        //var category3Text = $("#selCategoryID" + "  option:selected").text();

        //        if (category1Val == "1" && ((category2Val == "2") || (category2Val == "17")) && $.trim(category3Text) == "易集客") {
        //68表示"反馈"-"产品"分类下的"易集客",68表示"反馈"-"服务"分类下的"易集客"
        if (category1Val == "1" && ((category2Val == "2" && category3Val == "68") || (category2Val == "17" && category3Val == "69"))) {
            if ($("input[id$='txtSearchRelateDemands']").val() == "") {
                jsonValidateData.result = "false";
                jsonValidateData.msg = jsonValidateData.msg == undefined ? "请选择关联需求！<br/>" : jsonValidateData.msg + "请选择关联需求！<br/>";
            }
        }

        return jsonValidateData;
    }

    //前台——如果验证成功，则继续后台验证和插入操作 否则结束
    //type=1：添加；type=2：转出
    function ucDealer_Submit(type) {
        var jsonData = "";
        var callID = "";
        if ($("#hidOrderCallID").val() != undefined) {
            callID = $("#hidOrderCallID").val();
        }

        var callRecID = "";
        if ($("#hidCallID").val() != undefined) {
            callRecID = $("#hidCallID").val();  //录音表主键，关联联系记录用
        }
        var cbInfo = GetCustBasicInfo();
        var isCustBasic = "";
        if ("<%=dcCRMCustID %>" != "") {
            isCustBasic = "true";
        }
        //任务来源
        var hidTaskSource = $("#hidTaskSource").val();

        AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx",
        {
            Action: "dealerSubmit",
            ValidateData: GetAreaValidateMsg("uc_DealerInfo"),
            OperData: GetOperData(),
            RevertData: GetRevertData(),
            TagIDs: $.trim($("#txtTagInfo").attr("did")),
            OperType: type,
            CallID: callID,
            CallRecID: callRecID,
            CustBasicInfo: cbInfo,
            IsCustBasic: isCustBasic,
            DemandID: $("input[id$='txtSearchRelateDemands']").val(),
            TaskSource: hidTaskSource,
            r: Math.random()
        },
        null, function (data) {
            jsonData = $.evalJSON(data);
            if (jsonData.result == "true") {
            }
        });

        return jsonData;
    }

    //查询客户
    function selectCustInfo() {

        var CustName = $.trim($("#txtCustName").val())

        $.openPopupLayer({
            name: "CRMCustAjaxPopup",
            parameters: { CustName: escape(CustName) },
            url: "/WorkOrder/CRMCustPop.aspx",
            beforeClose: function (e) {
                if (e) {
                    var Name = $('#popupLayer_' + 'CRMCustAjaxPopup').data('CustName');
                    var CustID = $('#popupLayer_' + 'CRMCustAjaxPopup').data('CrmCustID');
                    var PID = $('#popupLayer_' + 'CRMCustAjaxPopup').data('PID');
                    var CID = $('#popupLayer_' + 'CRMCustAjaxPopup').data('CID');
                    var TID = $('#popupLayer_' + 'CRMCustAjaxPopup').data('TID');
                    $("#txtCustName").attr("value", Name);
                    $("#hidCustId").attr("value", CustID);
                    uc_DealerToAreaInfo(PID, CID, TID);
                }
            }
        });
    }

    //给省份、城市、区县赋值
    function uc_DealerToAreaInfo(PID, CID, TID) {
        $("#selProvince").val(PID);
        uc_triggerProvince();
        $("#selCity").val(CID);
        uc_triggerCity();
        $("#selCounty").val(TID);
    }

    function uc_triggerProvince() {//选中省份
        BindCity('selProvince', 'selCity');
        BindCounty('selProvince', 'selCity', 'selCounty');
    }

    function uc_triggerCity() {//选中城市
        BindCounty('selProvince', 'selCity', 'selCounty');
    }

    $(function () {

        $('#txtTagInfo').ChooseWorkTag();
        //给经销商工单初始化客户信息，客户地区，用于经销商客户添加工单add by qizq 2013-9-4
        if ($('#txtMemberID').val() != undefined && $('#txtMemberID').val() != "") {
            if (typeof GetCustInfoByMemberCode == "function") {
                GetCustInfoByMemberCode($('#txtMemberID').val());
            }
        }
        InitWdatePicker(1, ["txtLastProcessDate"]);
        $("#txtLastProcessDate").val('<%=days7Before %>');

        //用来判断是否是通过客户端打开此页面
        var IsClientOpen = '<%=RequestIsClientOpen %>';
        if (IsClientOpen == 1) {
            AjaxPost("/AjaxServers/CTIHandler.ashx", { Action: "GetAreaID", PhoneNum: "<%=RequestCalledNum %>" }, null, function (data) {
                //alert("父页面");
                if (data) {
                    var jsonData = $.evalJSON(data);
                    $("[id$='selProvince']").val(jsonData.ProvinceID);
                    uc_triggerProvince();
                    $("[id$='selCity']").val(jsonData.CityID);
                    uc_triggerCity();
                }
            });
        }

        var form = "<%=RequestFrom %>";
        if (form == "CTI") {
            //ADTTool.LogonTime("time", "工单-经销商信息加载完成 " + "<%=logmsg %>");
        }
    });

    function uc_DealerInit() {
        BindProvince("selProvince");

        uc_triggerProvince();

        BindByEnum("selPriorityLevel", "PriorityLevelEnum");

        //优先级默认为：普通
        $("#selPriorityLevel").val("1");

        BindByEnum("selDataSource", "WorkOrderDataSource");

        uc_BindCategory1('selCategoryID1', "1,3");

    }
</script>
