<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonalControl.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.UControl.PersonalControl" %>
<ul class="clearfix " name="uc_PersonalInfo" id="uc_PersonalInfo">
    <li style="width: 900px;">
        <label>
            工单类型：</label>
        <span>
            <input id="rdoWorkOrderPerson4" type="radio" value="4" checked="checked" onclick="orderSwitch('4')"
                name="OrderCategory4" />
            <em onclick="emChkIsChoose(this);orderSwitch('4');">个人</em> </span>&nbsp;&nbsp;<span>
                <input id="rdoWorkOrderDealer4" type="radio" value="3" name="OrderCategory4" onclick="orderSwitch('3')" />
                <em onclick="emChkIsChoose(this);orderSwitch('3');">经销商</em> </span>
    </li>
    <li>
        <label>
            <span class="redColor">*</span>工单分类：</label><span><select name="p_CategoryID1" vtype="notFirstOption"
                id="p_selCategoryID1" onchange="uc_BindCategory2('p_selCategoryID2','p_selCategoryID1')"
                vmsg="请选择工单一级分类！" class="w80"><option value='-1'>请选择</option>
            </select></span>&nbsp;<span><select name="p_CategoryID2" onchange="uc_BindCategory3('p_selCategoryID','p_selCategoryID2')"
                class="w80" vtype="notFirstOption" id="p_selCategoryID2" vmsg="请选择工单二级分类！"><option
                    value='-1'>请选择</option>
            </select></span>&nbsp;<span><select name="p_CategoryID" class="w70" vtype="notFirstOption"
                id="p_selCategoryID" vmsg="请选择工单三级分类！"><option value='-1'>请选择</option>
            </select></span></li>
    <li name='uc_PersonalLi'>
        <label>
            <span class="redColor">*</span>工单来源：</label><span><select name="p_DataSource" class="w250"
                id="p_selDataSource" style="width: 253px;" vtype="notFirstOption" vmsg="请选择工单来源！"><option>
                    请选择</option>
            </select></span></li>
    <li class="ucNewCar ucSecondCar">
        <label>
            <span class="redColor" uctype="uc_gzcx">*</span>关注车型：</label><span><select name="AttentionCarBrandName"
                class="w80 ucNewCar ucSecondCar" id="selAttentionCarBrandName" vtype="notFirstOption"
                uctype="uc_gzcx" vmsg="请选择关注车型品牌！">
            </select>
            </span><span>
                <select name="AttentionCarSerialName" class="w80 ucNewCar ucSecondCar" uctype="uc_gzcx"
                    id="selAttentionCarSerialName" vtype="notFirstOption" vmsg="请选择关注车型系列！">
                </select>
            </span><span>
                <select name="AttentionCarTypeName" class="w70 ucNewCar ucSecondCar" id="selAttentionCarTypeName"
                    uctype="uc_gzcx" vtype="notFirstOption" vmsg="请选择关注车型车款！">
                </select>
            </span></li>
    <li class="ucSecondCar" style="display: none;">
        <label>
            出售车型：</label><span><select name="SaleCarBrandName" class="w80 ucSecondCar" id="selSaleCarBrandName">
            </select>
            </span>&nbsp;&nbsp;<span><select name="SaleCarSerialName" class="w80 ucSecondCar"
                id="selSaleCarSerialName">
            </select>
            </span>&nbsp;&nbsp;<span><select name="SaleCarTypeName" class="w70 ucSecondCar" id="selSaleCarTypeName">
            </select>
            </span></li>
    <li class="ucNewCar" name='uc_PersonalLi' style="width: 470px">
        <label>
            <span class="redColor">*</span>推荐经销商：</label><span>
                <input id="txtDealerName" type="text" class="w250 ucNewCar" value="" readonly="true"
                    vtype="isNull" vmsg="请选择推荐经销商！" contoltype="txt" name="SelectDealerName" />
                <input type="hidden" id="hidDMSName" value='' /><input type="hidden" id="hidSelectDealerID"
                    name="SelectDealerID" class="ucNewCar" />&nbsp;<a id="btnNewSelect" onclick="YpDealSelect()"
                        style="cursor: pointer;">查询</a>&nbsp;&nbsp;<a id="A3" onclick="SendSmS()" style="cursor: pointer;">发送短信</a></span></li>
    <li class="ucNewCar">
        <label>
            <span class="redColor">*</span>接受回访：</label><span>
                <input id="rdoIsReturnVisitYes" type="radio" name="IsReturnVisit" checked="checked"
                    class="ucNewCar" value="1" /><em onclick="emChkIsChoose(this);">接受</em></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>
                        <input id="rdoIsReturnVisitNo" type="radio" name="IsReturnVisit" value="0" class="ucNewCar" /><em
                            onclick="emChkIsChoose(this);">不接受</em> </span></li>
    <li class="ucNewCar" name='uc_PersonalLi'>
        <label>
            推荐活动：</label><span>
                <input name="NominateActivity" type="text" id="txtNominateActivity" class="w250 ucNewCar"
                    readonly="readonly" />&nbsp;&nbsp;<a id="aSelActivity" onclick="popSelActivity()"
                        style="cursor: pointer;">查询</a>
                <input type="hidden" id="popHidActivityIDs" value='' />
            </span></li>
    <li>
        <label>
            工单标题：</label><span><input name="p_Title" type="text" lenstr="30" class="w250" vtype="Len"
                vmsg="工单标题已超过15个字！" id="p_txtTitle" /></span></li>
    <li>
        <%--<label>
            标签：</label><span><input name="TagInfo" type="text" onclick="selectTagPersonal()"
                id="txtTagInfoPersonal" class="w250" /><input type="hidden" id="hidTagIDPersonal"
                    value="" /></span>--%>
        <label>
            <span class="redColor">*</span>标签：</label><div class="coupon-box02" style="float: left;
                width: 246px;">
                <input type="text" style="border: none;" class="text02" vtype="isNull" vmsg="请选择标签！"
                    id="txtTagInfoPersonal"><b><a href="#">选择</a></b></div>
    </li>
    <li class="gdjl">
        <label>
            <span class="redColor">*</span>工单记录：</label><span><textarea id="p_areaContent" vtype="isNull|Len"
                lenstr="600" vmsg="请填写工单记录！|工单记录已超过300个字！" name="p_Content"></textarea></span></li>
</ul>
<input type="hidden" id="hidDMSLevel" value='' />
<input type="hidden" id="hidDMSAddress" value='' />
<input type="hidden" id="hidDMSTel" value='' />
<input type="hidden" id="hidDMSCity" value='' />
<input type="hidden" id="popHidProvinceID" value='CustBaseInfo_ddlProvince' />
<input type="hidden" id="popHidCityID" value='CustBaseInfo_ddlCity' />
<input type="hidden" id="hidTuJianJingXiaoShangMs" value='' />
<script type="text/javascript">
    document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
</script>
<script type="text/javascript">
    loadJS("bitdropdownlist");
</script>
<script src="http://img1.bitauto.com/bt/Price/js/autodata/brand.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {

        $('#txtTagInfoPersonal').ChooseWorkTag();
        AttachEvent("selAttentionCarBrandName", "change", popCarChange);

        $("#txtTagInfoPersonal").val("<%=RequestTagName%>");
        $("#hidTagIDPersonal").val("<%=RequestTagID%>");

        var form = "<%=RequestFrom %>";
        if (form == "CTI") {
            //ADTTool.LogonTime("time", "工单-个人信息加载完成 （无）");
        }
    });

    function popCarChange() {
        $("#txtNominateActivity").val("");
        $("#popHidActivityIDs").val("");
    }

    //省份、城市ID默认为CustBaseInfo_ddlProvince、CustBaseInfo_ddlCity
    //如果从新增和编辑页面进来可以不需修改，如果从弹出层进来，需要指定省份城市ID
    function popSelActivity() {

        if ($("#popHidProvinceID").val() == "") {
            $.jAlert("省份城市参数有误，无法选择推荐活动！");
            return false;
        }
        var opts = {
            pid: $("#" + $("#popHidProvinceID").val()).val(),
            cid: $("#" + $("#popHidCityID").val()).val(),
            bid: $("#selAttentionCarBrandName").val(),
            carid: $("#selAttentionCarSerialName").val(),
            selectids: $("#popHidActivityIDs").val()
        };
        var popObj = fnSelectActivityPop(opts, function (returnObj) {
            $("#popHidActivityIDs").val(returnObj.ids);
            $("#txtNominateActivity").val(returnObj.values);
        });
        if (popObj.msg != "") {
            $.jAlert(popObj.msg);
        }
    }

    //绑定品牌信息
    function BindBrand() {

        //二手关注的车
        var options2 = {
            container: { master: "selAttentionCarBrandName", serial: "selAttentionCarSerialName", cartype: "selAttentionCarTypeName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '0', serial: '0', cartype: '0' }
        };
        new BindSelect(options2).BindList();

        //二手要出售的车
        var options3 = {
            container: { master: "selSaleCarBrandName", serial: "selSaleCarSerialName", cartype: "selSaleCarTypeName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '0', serial: '0', cartype: '0' }
        };
        new BindSelect(options3).BindList();


    }

    //发送短信-添加个人类型的工单
    function SendSmS() {
        var DMSCode = $("[id$='hidSelectDealerID']").val();
        if (DMSCode == "") {
            $.jAlert("请选择经销商！");
            return;
        }
        var custName = $.trim($("[id$='txtCustName']").val());

        var par = {
            CustName: encodeURIComponent(custName),    //客户姓名
            DMSCode: encodeURIComponent(DMSCode)//经销商ID
        };

        $.openPopupLayer({
            name: "SendSMSPopup",
            parameters: par,
            url: "/AjaxServers/SendSMSPoper.aspx",
            beforeClose: function (e, data) {
                $("#hidTuJianJingXiaoShangMs").val(data.Tels);
            }
        });
    }

    function YpDealSelect() {
        var ProvinceIDDealSelect = $("#" + $("#popHidProvinceID").val()).val();
        var CityIDDealSelect = $("#" + $("#popHidCityID").val()).val();
        var CarIDDealSelect = $("[id$='selAttentionCarTypeName']").val();
        var CarIDOptionLen = $("#selAttentionCarTypeName option").length;
        var SerialID = $("[id$='selAttentionCarSerialName']").val();
        if (parseInt(ProvinceIDDealSelect) <= 0 || ProvinceIDDealSelect == null) {
            $.jAlert("请选择地区-省份！");
            return;
        }
        else if (parseInt(CityIDDealSelect) <= 0 || CityIDDealSelect == null) {
            $.jAlert("请选择地区-城市！");
            return;
        }
        else if (parseInt(SerialID) <= 0 || SerialID == null) {
            $.jAlert("请选择车型！");
            return;
        }
        else if (parseInt(CarIDDealSelect) <= 0 && CarIDOptionLen > 1) {
            $.jAlert("请选择车款！");
            return;
        }
        else if (parseInt(CarIDDealSelect) <= 0 && CarIDOptionLen == 1) {
            CarIDDealSelect = SerialID
        }
        $.openPopupLayer({
            name: "YpDealerSelectAjaxPopup",
            parameters: { ProvinceID: escape(ProvinceIDDealSelect), CityID: escape(CityIDDealSelect), CarID: escape(CarIDDealSelect) },
            url: "../../AjaxServers/TaskManager/NoDealerOrder/YpDealerSelect.aspx",
            beforeClose: function () {
                var DMSName = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSName', DMSName);
                var DMSCode = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSCode', DMSCode);
                var DMSLevel = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSLevel', DMSLevel);
                var DMSTel = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSTel', DMSTel);
                var City = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('City', City);
                var Address = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('Address', Address);

                $("#hidDMSName").val(DMSName);
                $("#hidSelectDealerID").val(DMSCode);

                //add by qizq 2013-8-13,为发短信拼接经销商信息 
                $("#txtDealerName").val(DMSName);
                $("#hidDMSLevel").val(DMSLevel);
                $("#hidDMSAddress").val(Address);
                $("#hidDMSTel").val(DMSTel);
                $("#hidDMSCity").val(City);
                //

            }
        });
    }    
</script>
<script type="text/javascript">
    function p_GetOperData() {

        var operData = "";
        if ($("#p_selCategoryID option").length > 1) {
            operData += "CategoryID=" + $("#p_selCategoryID").val() + "&";
        }
        else {
            operData += "CategoryID=" + $("#p_selCategoryID2").val() + "&";
        }
        operData += "DataSource=" + $("#p_selDataSource").val() + "&";
        operData += "Title=" + encodeURIComponent($.trim($("#p_txtTitle").val())) + "&";
        operData += "Content=" + encodeURIComponent($.trim($("#p_areaContent").val())) + "&";
        operData += "TagInfo=" + $.trim($("#hidTagInfo").val()) + "&";

        if (!$("li[class='ucNewCar ucSecondCar']").is(":hidden") && $("#selAttentionCarBrandName").val() != "0") {
            operData += "AttentionCarBrandName=" + encodeURIComponent($("#selAttentionCarBrandName option:selected").text()) + "&";
            operData += "AttentionCarBrandID=" + $("#selAttentionCarBrandName").val() + "&";

            operData += "AttentionCarSerialName=" + encodeURIComponent($("#selAttentionCarSerialName option:selected").text()) + "&";
            operData += "AttentionCarSerialID=" + $("#selAttentionCarSerialName").val() + "&";

            operData += "AttentionCarTypeName=" + encodeURIComponent($("#selAttentionCarTypeName option:selected").text()) + "&";
            operData += "AttentionCarTypeID=" + $("#selAttentionCarTypeName").val() + "&";
        }

        if (!$("li[class='ucNewCar']").is(":hidden")) {
            operData += "SelectDealerName=" + encodeURIComponent($.trim($("#txtDealerName").val())) + "&";
            operData += "SelectDealerID=" + $("#hidSelectDealerID").val() + "&";

            operData += "IsReturnVisit=" + $("[name='IsReturnVisit']:checked").val() + "&";
            //一下语句之前是注释的，导致导出工单“推荐活动”字段一直为空，所以取消了注释。 add by yangyh 
            operData += "NominateActivity=" + encodeURIComponent($.trim($("#txtNominateActivity").val())) + "&";
        }

        if (!$("li[class='ucSecondCar']").is(":hidden") && $("#selSaleCarBrandName").val() != "0") {
            operData += "SaleCarBrandName=" + encodeURIComponent($("#selSaleCarBrandName option:selected").text()) + "&";
            operData += "SaleCarBrandID=" + $("#selSaleCarBrandName").val() + "&";

            operData += "SaleCarSerialName=" + encodeURIComponent($("#selSaleCarSerialName option:selected").text()) + "&";
            operData += "SaleCarSerialID=" + $("#selSaleCarSerialName").val() + "&";

            operData += "SaleCarTypeName=" + encodeURIComponent($("#selSaleCarTypeName option:selected").text()) + "&";
            operData += "SaleCarTypeID=" + $("#selSaleCarTypeName").val() + "&";
        }

        var csid = "<%=RequestCSID %>"
        operData += "CSID=" + csid + "&";
        var lyid = "<%=RequestLYID %>"
        operData += "LYID=" + lyid + "&";
        var cartype = "<%=RequestCarType %>"
        operData += "CarType=" + cartype + "&";
        var systype = "<%=RequestSYSType %>"
        operData += "SYSType=" + systype + "&";

        return operData;
    }

    function p_GetRevertData() {

        var revertData = "";
        if ($("#p_selCategoryID option").length > 1) {
            revertData += "CategoryName=" + encodeURIComponent($("#p_selCategoryID option:selected").text()) + "&";
        }
        else {
            revertData += "CategoryID=" + encodeURIComponent($("#p_selCategoryID2 option:selected").text()) + "&";
        }
        revertData += "DataSource=" + encodeURIComponent($("#p_selDataSource option:selected").text()) + "&";
        revertData += "Title=" + encodeURIComponent($.trim($("#p_txtTitle").val())) + "&";
        revertData += "RevertContent=" + encodeURIComponent($.trim($("#p_areaContent").val())) + "&";

        if (!$("li[class='ucNewCar ucSecondCar']").is(":hidden")) {
            revertData += "AttentionCarBrandName=" + encodeURIComponent($("#selAttentionCarBrandName option:selected").text()) + "&";

            revertData += "AttentionCarSerialName=" + encodeURIComponent($("#selAttentionCarSerialName option:selected").text()) + "&";

            revertData += "AttentionCarTypeName=" + encodeURIComponent($("#selAttentionCarTypeName option:selected").text()) + "&";
        }

        if (!$("li[class='ucNewCar']").is(":hidden")) {
            revertData += "SelectDealerName=" + encodeURIComponent($.trim($("#txtDealerName").val())) + "&";
            revertData += "SelectDealerID=" + $("#hidSelectDealerID").val() + "&";

            revertData += "IsReturnVisit=" + encodeURIComponent($("[name='IsReturnVisit']:checked").val() == "1" ? "是" : "否") + "&";
            //revertData += "NominateActivity=" + encodeURIComponent($.trim($("#txtNominateActivity").val())) + "&";
        }

        if (!$("li[class='ucSecondCar']").is(":hidden")) {
            revertData += "SaleCarBrandName=" + encodeURIComponent($("#selSaleCarBrandName option:selected").text()) + "&";

            revertData += "SaleCarSerialName=" + encodeURIComponent($("#selSaleCarSerialName option:selected").text()) + "&";

            revertData += "SaleCarTypeName=" + encodeURIComponent($("#selSaleCarTypeName option:selected").text()) + "&";
        }

        return revertData;
    }
</script>
<script type="text/javascript">
    //验证
    function ucPersonal_Validate() {

        var jsonValidateData = "";

        var ulHtml = "";
        //html()方法只会返回满足条件的第一个元素html代码

        var liHtml = "";
        $("#uc_PersonalInfo li").each(function (i, c) {
            if (!$(this).is(":hidden")) {
                liHtml += $(this).html();
            }
        });

        ulHtml = "<form name='uc_PersonalInfoForm'>" + liHtml + "</form>"; //ul

        jsonValidateData = $.evalJSON(validateMsg($(ulHtml).serializeArray()));

        return jsonValidateData;
    }

    //前台——如果验证成功，则继续 否则结束
    function ucPersonal_Submit() {

        var jsonData = "";

        var callID = "";
        if ($("#hidOrderCallID").val() != undefined) {
            callID = $("#hidOrderCallID").val();
        }

        AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "personalSubmit", ValidateData: GetAreaValidateMsg("uc_PersonalInfo"), OperData: p_GetOperData(), RevertData: p_GetRevertData(), TagIDs: $.trim($("#txtTagInfoPersonal").attr('did')), CallID: callID, ActivityIDs: encodeURIComponent($.trim($("#popHidActivityIDs").val())), r: Math.random() }, null, function (data) {
            jsonData = $.evalJSON(data);

            //后台——如果验证成功，继续，否则结束
            if (jsonData.result == "true") {

            }
        });

        return jsonData;
    }

    $(function () {

    });


    function uc_PersonalInit() {

        BindByEnum("p_selDataSource", "WorkOrderDataSource");
        BindBrand();

        uc_BindCategory1('p_selCategoryID1', "1,2");

        $("li[name='uc_PersonalLi']").css("width", "470px");
    }
</script>
