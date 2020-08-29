//一级工单分类
function uc_BindCategory1(id, useScope) {
    $("#" + id + " option").remove();
    AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "GetCategory1", UseScope: useScope, r: Math.random() }, null, function (data) {
        $("#" + id).append("<option value='-1'>请选择</option>");
        var jsonData = $.evalJSON(data);
        if (jsonData != "") {
            for (var i = 0; i < jsonData.length; i++) {
                $("#" + id).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
            }
        }
    });
}

//二级工单分类
function uc_BindCategory2(id, pid) {
    $("#" + id + " option").remove();
    AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "GetCategory2", PID: $("#" + pid).val(), r: Math.random() }, null, function (data) {
        $("#" + id).append("<option value='-1'>请选择</option>");
        var jsonData = $.evalJSON(data);
        if (jsonData != "") {
            for (var i = 0; i < jsonData.length; i++) {
                $("#" + id).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
            }
        }
    });
    $("#selCategoryID option:gt(0)").remove();
}

//三级工单分类
function uc_BindCategory3(id, pid) {

    $("#" + id + " option").remove();
    AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "GetCategory", PID: $("#" + pid).val(), r: Math.random() }, null, function (data) {
        $("#" + id).append("<option value='-1'>请选择</option>");
        var jsonData = $.evalJSON(data);
        if (jsonData != "") {
            for (var i = 0; i < jsonData.length; i++) {
                $("#" + id).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
            }
        }
    });

    var categoryText = $.trim($("#" + pid + " option:selected").html());

    if (categoryText == "新车" || categoryText == "买二手车") {
        $(".ucSecondCar").hide();
        $(".ucNewCar").show();
        $("span[ucType='uc_gzcx']").show(); 
        $("select[ucType='uc_gzcx']").attr("vtype", "notFirstOption"); //关注车型为必填项
    }
    else if (categoryText == "卖二手车") {
        $(".ucNewCar").hide();
        $(".ucSecondCar").show();
        $("span[ucType='uc_gzcx']").hide(); //关注车型的*去掉
        $("select[ucType='uc_gzcx']").removeAttr("vtype"); //关注车型为非必填项
    }
    else {
        $(".ucNewCar").hide();
        $(".ucSecondCar").hide();
    }
}


function uc_BindCategory4()
{
    var category1Val =$("#selCategoryID1").val();
    var category2Val = $("#selCategoryID2").val();
    var category3Val = $("#selCategoryID").val();
    //var category3Text = $("#selCategoryID" + "  option:selected").text();

    //if (category1Val == "1" && (category2Val == "2" || category2Val == "17") && $.trim(category3Text) == "易集客") {
    //68表示"反馈"-"产品"分类下的"易集客",68表示"反馈"-"服务"分类下的"易集客"
    if (category1Val == "1" && ((category2Val == "2" && category3Val == "68") || (category2Val == "17" && category3Val == "69"))) {
        $("#lbRelateDemands").html("<span class=\"redColor\">*</span>关联需求：");
    }
    else {
        $("#lbRelateDemands").html("关联需求：");
    }
}


//工单处理页面修改三级工单分类
function uc_BindCategoryJudgeDemand(id, pid) {

    $("#" + id + " option").remove();
    AjaxPostAsync("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: "GetCategory", PID: $("#" + pid).val(), r: Math.random() }, null, function (data) {
        $("#" + id).append("<option value='-1'>请选择</option>");
        var jsonData = $.evalJSON(data);
        if (jsonData != "") {
            for (var i = 0; i < jsonData.length; i++) {
                $("#" + id).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
            }
        }
    });
    if ($("#hlRelateDemand").text() == "") {
        $("#" + id + " option[value='68']").remove();
        $("#" + id + " option[value='69']").remove();
    }
    
    var categoryText = $.trim($("#" + pid + " option:selected").html());

    if (categoryText == "新车" || categoryText == "买二手车") {
        $(".ucSecondCar").hide();
        $(".ucNewCar").show();
        $("span[ucType='uc_gzcx']").show();
        $("select[ucType='uc_gzcx']").attr("vtype", "notFirstOption"); //关注车型为必填项
    }
    else if (categoryText == "卖二手车") {
        $(".ucNewCar").hide();
        $(".ucSecondCar").show();
        $("span[ucType='uc_gzcx']").hide(); //关注车型的*去掉
        $("select[ucType='uc_gzcx']").removeAttr("vtype"); //关注车型为非必填项
    }
    else {
        $(".ucNewCar").hide();
        $(".ucSecondCar").hide();
    }
}


function changeLabelText() {

    if ($("#selCategoryID").val() == "68" || $("#selCategoryID").val() == "69") {
        $("#lbRelateDemands").html("<span class=\"redColor\">*</span>关联需求：");
    }
    else {
        $("#lbRelateDemands").html("关联需求：");
    }
}