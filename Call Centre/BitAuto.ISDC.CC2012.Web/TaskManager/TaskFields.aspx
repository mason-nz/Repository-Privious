<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskFields.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.TaskFields" %>

<link href="../Css/style.css" rel="stylesheet" type="text/css" />
<link href="../Css/base.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {

        $.get("TaskFields.xml", LoadCheckFromXML);

        $(".allSelect").click(function () {

            if ($(this).attr("checked") == true) {
                $("#ckDiv_" + $(this).attr("typename"), $("#divExportSet")).find("input[type=checkbox]").attr("checked", true);
            }
            else {
                $("#ckDiv_" + $(this).attr("typename"), $("#divExportSet")).find("input[type=checkbox]").attr("checked", false);
            }
        });

        //确定

        $("#btnOk").click(ReturnFidldStr);

        //清空选中
        $("#btnClear").click(function () {
            $("input[type='checkbox']", $("#divExportSet")).attr("checked", false);
        });

    });

    function LoadCheckFromXML(conext) {

        //生成【基本信息】的复选框
        loadCheckBox(conext, "baseInfo");

        //生成【客户扩展信息-个人】的复选框
        loadCheckBox(conext, "BuyCarInfo");

        //生成【客户扩展信息-经销商】的复选框
        loadCheckBox(conext, "DealerInfo");

        //生成【联系记录-新车】的复选框
        loadCheckBox(conext, "NewCarInfo");

        //生成【联系记录-二手车】的复选框
        loadCheckBox(conext, "SecondCarInfo");

        //生成【系统属性】的复选框
        loadCheckBox(conext, "SystemInfo");

        //CheckAll
        $(":checkbox[class=ckBox]", $("#divExportSet")).click(function () { CheckAll($(this), $(this).attr("typeName")); });

        //根据Cookie选中复选框

        CookieSet();
    }

    function loadCheckBox(conext, key) {
        var tableName = $(conext).find("Fields[id='" + key + "']").attr("Table");

        var html = "<ul class='clearfix ft14 output'>";
        $(conext).find("Fields[id='" + key + "'] item").each(function () {
            html = html + "<li><input type=checkbox id='ck_" + tableName + "_" + $(this).attr("name") +
                             "' fieldName='" + $(this).attr("name") + "' table='" + tableName + "' class='ckBox'  typeName='" + key + "' />" +
                              "<label  style='float:none;'  for='ck_" + tableName + "_" + $(this).attr("name") + "'>" + $(this).attr("des") + "</label>" + "</li>";
        });
        html = html + " </ul>";
        $("#ckDiv_" + key).html(html);
    }

    function ReturnFidldStr() {

        var fieldStr = "";
        $("input[class=ckBox]", $("#divExportSet")).each(function () {
            if ($(this).attr("checked") == true) {
                fieldStr = fieldStr + $(this).attr("table") + "." + $(this).attr("fieldName") + ",";
            }
        });
        fieldStr = fieldStr.substr(0, fieldStr.length - 1);

        if (fieldStr == "") {
            $.jAlert("请至少选择一个要导出的字段");
            return;
        }
        $("#hidFieldsTask").val(fieldStr);
        $("#hidFieldsTask2").val(fieldStr);

        //设置cookie
        var selectIds = "";
        $("input[type=checkbox]", $("#divExportSet")).each(function () {
            if ($(this).attr("checked") == true) {
                selectIds = selectIds + $(this).attr("id") + ",";
            }
        });
        selectIds = selectIds.substr(0, selectIds.length - 1);
        SetCookie("TaskFields", selectIds);
        $("#hidIsOkOrCancel").val("1");
        $.closePopupLayer("DisposeSetPoper");
    }

    function CookieSet() {
        var c = GetCookie("TaskFields");
        if (c == null) {
            return;
        }

        var list = c.split(",");

        $(list).each(function (i, v) {
            $("#" + v).attr("checked", true);
        });
    }

    function CheckAll(o, key) {
        var len = $(":checkbox[class=ckBox][checked=false][typeName=" + key + "]").length;
        if (len == 0) {
            $(":checkbox[class=allSelect][typename=" + key + "]").attr("checked", true)
        }
        else {
            $(":checkbox[class=allSelect][typename=" + key + "]").attr("checked", false)
        }
    }
</script>
<div class="pop pb15 openwindow" style="width: 800px;" id="divExportSet">
    <div class="title pt5 ft16 bold">
        <h2>
            选择导出字段</h2>
        <span><a href="javascript:void(0);" onclick="javascript:$.closePopupLayer('DisposeSetPoper');"
            class="right" ></a></span>
    </div>
    <div class="popT bold pt5">
        客户基本信息 <span>
            <input name="" id="ck_baseInfoall" typename="baseInfo" class="allSelect" type="checkbox"
                value="" />
            全选</span></div>
    <div id="ckDiv_baseInfo">
    </div>
    <div class="popT bold pt5">
        客户扩展信息-个人 <span>
            <input name="" id="ck_BuyCarInfoall" typename="BuyCarInfo" class="allSelect" type="checkbox"
                value="" />
            全选</span></div>
    <div id="ckDiv_BuyCarInfo">
    </div>
    <div class="popT bold pt5">
        客户扩展信息-经销商 <span>
            <input name="" id="ck_DealerInfoall" typename="DealerInfo" class="allSelect" type="checkbox"
                value="" />
            全选</span></div>
    <div id="ckDiv_DealerInfo">
    </div>
    <div class="popT bold pt5">
        联系记录信息-新车 <span>
            <input name="" id="ck_NewCarInfoall" typename="NewCarInfo" class="allSelect" type="checkbox"
                value="" />
            全选</span></div>
    <div id="ckDiv_NewCarInfo">
    </div>
    <div class="popT bold pt5">
        联系记录信息-二手车<span>
            <input name="" id="ck_SecondCarInfoall" typename="SecondCarInfo" class="allSelect"
                type="checkbox" value="" />
            全选</span></div>
    <div id="ckDiv_SecondCarInfo">
    </div>
    <div class="popT bold pt5">
        系统属性<span>
            <input name="" id="ck_SystemInfoall" typename="SystemInfo" class="allSelect" type="checkbox"
                value="" />
            全选</span></div>
    <div id="ckDiv_SystemInfo">
    </div>
    <div class="btn">
        <input name="" id="btnOk" type="button" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" onclick="javascript:$.closePopupLayer('DisposeSetPoper');"
            class="btnCannel bold" />
        <input name="" id="btnClear" type="button" value="清 空" class="btnSave bold" /></div>
</div>
