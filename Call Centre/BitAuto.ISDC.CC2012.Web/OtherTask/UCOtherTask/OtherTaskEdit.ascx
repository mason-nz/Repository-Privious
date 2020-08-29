<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask.OtherTaskEdit" %>
<script type="text/javascript">
    function GetSurveyCount() {
        var value = $("input[cid='hiddenSurveyCount']").val();
        return parseInt(value);
    }
    function GetSIIDStr() {
        var value = $("input[cid='hiddenSIIDStr']").val();
        return value;
    }

    function LoadOtherTask() {
        //加载自定义表单
        var TTCode = '<%=TTCode %>';
        var RelationID = '<%=RelationID%>';
        if (TTCode == "" || RelationID == "") {
        }
        else {
            //读取表单
            BindFields(TTCode);

            //给字段付值
            BindData(TTCode, RelationID);

            //给推荐活动控件加事件
            BindActivityEvent();

            if (isPersonalInfo()) {
                $("#spanInfo").text("个人基本信息");
            }
        }
        if ($("#hidIsUseCRM").val() == "0") {
            //加载历史记录
            GetCallRecordORGIHistory();
        }
        //延时设置是否接通=是
        if ($("#IsAutoEstablish").val() == "1") {
            SetAutoEstablish();
        }
    }
    //调用推荐活动弹出层
    function OtherTaskSelectActivety() {
        //取省，市，意向品牌或意向车型和已选择推荐活动
        var provinceid = $("select[id$='_Province']").first().val();
        var cityid = $("select[id$='_City']").first().val();
        var YXBrand = $("select[id$='_YXBrand']").first().val();
        var YXSerial = $("select[id$='_YXSerial']").first().val();
        var selectids = $("input[type='hidden'][id$='_Activity']").first().val();
        var opts = {
            pid: provinceid,
            cid: cityid,
            bid: YXBrand,
            carid: YXSerial,
            selectids: selectids
        };
        var popObj = fnSelectActivityPop(opts, function (returnObj) {
            $("input[type='hidden'][id$='_Activity']").first().val(returnObj.ids);
            $("input[id$='_Activity_Name']").first().val(returnObj.values);
        });
        if (popObj.msg != "") {
            $.jAlert(popObj.msg);
        }
    }
    //显示历史记录图标
    function GetCallRecordORGIHistory() {
        //先清空历史记录图标
        $("a[chistory='1']").each(function () {
            $(this).remove();
        });

        var tels = "";
        tels = $("a[ctel]:not([ctel=''])").map(function () {
            var val = $(this).attr("ctel").replace(/\-/g, "");
            $(this).attr("ctel", val);
            return $(this).attr("ctel");
        }).get().join(",");

        if (tels == "") {
            return;
        }

        var msg = "";
        var taskid = '<%=RequestTaskID%>';
        AjaxPost('/AjaxServers/OtherTask/OtherTaskDeal.ashx', { Action: "GetCallRecordORGIHistory", TelePhones: tels, TaskID: '<%=RequestTaskID%>' }, null, function (data) {
            var jsonDatas = $.evalJSON(data);
            $.each(jsonDatas, function (i, jsonData) {
                if (jsonData.result != "1" && jsonData.result != "undefined") {//等于1不显示图标
                    //$("a[ctel]:([ctel='" + jsonData.Tel + "'])").each(function () {
                    $("a[ctel='" + jsonData.Tel + "']").each(function () {
                        if (jsonData.result == "2") {//显示个人用户查看页
                            var custid = jsonData.CustID;

                            if (typeof ($(this).attr("csex")) != "undefined") {
                                msg = "<a chistory='1' href='../TaskManager/CustInformation.aspx?CustID=" + custid + "' target='_blank' class='linkBlue'><img alt='历史记录' src='/images/history.png' border='0' style='vertical-align:middle;float:right;padding-top:5px;' /></a>";

                                $(this).next().before(msg);
                            }
                            else {
                                msg = "<a chistory='1' href='../TaskManager/CustInformation.aspx?CustID=" + custid + "' target='_blank' class='linkBlue'><img style='vertical-align:middle;' alt='历史记录' src='/images/history.png' border='0' /></a>";
                                $(this).next().after(msg);
                            }

                        }
                        else if (jsonData.result == "3") {//显示个人用户列表
                            var custTel = jsonData.Tel;
                            if (typeof ($(this).attr("csex")) != "undefined") {
                                msg = "<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' target='_blank' class='linkBlue'><img alt='历史记录' src='/images/history.png' border='0' style='vertical-align:middle;float:right;padding-top:5px;' /></a>";
                                $(this).next().before(msg);
                            }
                            else {
                                msg = "<a chistory='1' href='../CustBaseInfo/List.aspx?CustTel=" + custTel + "' target='_blank' class='linkBlue'><img alt='历史记录' style='vertical-align:middle;' src='/images/history.png' border='0' /></a>";
                                $(this).next().after(msg);
                            }
                        }
                    });
                }
            });
        });
    }
    //绑定控件数据
    function BindData(TTCode, RelationID) {
        var jsonData = "";
        AjaxPostAsync('/AjaxServers/OtherTask/OtherTaskDeal.ashx', { RelationTableID: escape(TTCode), RelationID: escape(RelationID), Action: escape('GetCustomDataInfo') }, null, function (data) {
            jsonData = $.evalJSON(data);
            var isLoadCustInfo = 0;
            //遍历自定义控件
            //遍历input控件
            $("#divBaseInfo li input,#divBaseInfo li select,#divBaseInfo li textarea").each(function () {
                if ($(this).attr("type") == "checkbox" || $(this).attr("type") == "radio") {
                    var checkName = $(this).attr("name");
                    $('[name=' + checkName + ']').each(
                 function () {
                     var checboxitem = $(this);
                     var checboxitemvalue = checboxitem.val();
                     $.each(jsonData, function (i) {
                         var obj = jsonData[i];
                         for (var key in obj) {
                             if (key == checkName) {
                                 var val = unescape(obj[key]); //属性值
                                 var valArry = val.split(',');
                                 for (var n = 0; n < valArry.length; n++) {
                                     if (valArry[n] == checboxitemvalue) {
                                         checboxitem.attr("checked", true);
                                         break;
                                     }
                                 }
                             }
                         }
                     });
                 });
                }
                else {
                    var id = $(this).attr("id");
                    $.each(jsonData, function (i) {
                        var obj = jsonData[i];
                        for (var key in obj) {
                            if (key.indexOf("_crmcustid_name") > 0 && isLoadCustInfo == 0) {
                                //设置是否使用CRM模板隐域
                                $("#hidIsUseCRM").val("1");
                                $("#divCrmBlock").css("display", "");
                                $("#hdnCrmCustID").val(unescape(obj[key]));
                                $("#divCrmBaseInfo").load("/OtherTask/UCOtherTask/CustInfoView.aspx", { CustID: obj[key], IsShowBtn: "yes" }, function () {
                                });
                                isLoadCustInfo = 1;
                            }
                            if (key == id) {
                                var val = unescape(obj[key]); //属性值
                                if (val != null && val != '') {
                                    $("#" + id).val(val);
                                }
                                //如果是电话要加呼出按钮
                                if ($("#" + id).attr("vtype") == "isTelOrMobile" || $("#" + id).attr("vtype") == "isNull|isTelOrMobile") {
                                    //个人类型的打电话和发短信
                                    var tel = $.trim(val.replace(/\-/g, ""));
                                    $("#" + id).after(
                                    "<a ctel='" + tel + "' href='javascript:void(0);' onclick=\"javascript:CallOutForGR($('#" + key + "').val());\">"
                                    + "<img alt='打电话'  style='cursor: pointer; vertical-align:middle;' src='/images/phone.gif' border='0' /></a>"

                                    + "<a  href='javascript:void(0);' onclick=\"javascript:SendSmSForGR('" + tel + "');\">"
                                    + "<img alt='发送短信' src='/images/sms.png' style='cursor: pointer; vertical-align:middle;' border='0'/></a>"

                                    + "<img id='imgNodisturbTel' alt='添加为免打扰电话' disabled='disabled' style='cursor: pointer; vertical-align:middle;' "
                                    + "src='/Images/nodisturbgray.png' border='0' onclick=\"javascript:NoDisturbTool.openNoDisturbPopup(this,$('#" + key + "').val(),$('#hidCallID').val());\" />");

                                    $("#" + id).parent().children(':input[type="text"]').width(198); //解决样式问题 Modify=Masj,Date=2016-04-06

                                    var istest = "<%=IsTest %>";
                                    if (istest == "False") {
                                        $("#" + id).attr("disabled", "disabled"); //add by wangtonghai  2016/4/25 for 	其他任务页面基本信息电话号码不可编辑
                                    }
                                }

                                if ($("#" + id).attr("vtype") == "isDate" || $("#" + id).attr("vtype") == "isNull|isDate") {
                                    if ($("#" + id).val() == "1900-1-1 0:00:00") {
                                        $("#" + id).val("");
                                    }
                                    if ($("#" + id).val().split(' ').length == 2) {
                                        $("#" + id).val($("#" + id).val().split(' ')[0]);
                                    }
                                }
                                else if ($("#" + id).attr("vtype") == "isDateTime" || $("#" + id).attr("vtype") == "isNull|isDateTime") {
                                    if ($("#" + id).val() == "1900-1-1 0:00:00") {
                                        $("#" + id).val("");
                                    }
                                }

                                if (id.split('_').length == 2) {
                                    try {
                                        if (id.split('_')[1] == "Province") {
                                            if (id.split('_')[0] + "_City") {
                                                BindCity(id, id.split('_')[0] + "_City");
                                            }
                                        }
                                        else if (id.split('_')[1] == "City") {
                                            if (id.split('_')[0] + "_Country") {
                                                BindCounty(id.split('_')[0] + "_Province", id, id.split('_')[0] + "_Country");
                                            }
                                        }

                                    } catch (e) {
                                    }

                                    //品牌、车型
                                    if (id.split('_')[1] == "XDBrand" || id.split('_')[1] == "YXBrand" || id.split('_')[1] == "CSBrand") {

                                        var serialselectID = id.replace('Brand', 'Serial'); //获取车型的下拉列表ID

                                        var options1 = {
                                            container: { master: id, serial: serialselectID },
                                            include: { serial: "1", cartype: "1" },
                                            datatype: 0,
                                            deftext: {
                                                master: { "value": "-1", "text": "请选择" },
                                                serial: { "value": "-1", "text": "请选择" }
                                            },
                                            binddefvalue: { master: unescape(obj[id]), serial: unescape(obj[serialselectID])} //绑定值
                                        };
                                        new BindSelect(options1).BindList();

                                        if (obj.TFIsNull == "0") {
                                            $("#" + id).attr("vtype", "notFirstOption");
                                            $("#" + id).attr("vmsg", "必须选择");
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    });
                }
            });

            //话务结果赋值
            $.each(jsonData, function (i) {
                var obj = jsonData[i];
                for (var key in obj) {
                    var val = unescape(obj[key]); //属性值
                    if (key == "IsEstablish" || key == "NotEstablishReason" || key == "IsSuccess" || key == "NotSuccessReason") {
                        var id = -1;
                        try {
                            id = parseInt(val);
                            if (isNaN(id)) {
                                id = -1;
                            }
                        }
                        catch (e) {
                        }
                        $("#" + key + "_selectid").val(id);
                    }
                }
            });
            InitChange($("#IsEstablish_selectid")[0], $("#IsSuccess_selectid")[0]);

            if (isLoadCustInfo == 1) { //如果有CRM区域，按钮显示在CRM区域
                $.post("/AjaxServers/OtherTask/OtherTaskDeal.ashx", { Action: "IsShowBtnByTTCode", TTCode: TTCode }, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "true") {
                        if (jsonData.IsShowBtn == "true") {
                            $("#crmHrefAddRecord").css("display", "");
                        }
                        if (jsonData.IsShowWorkOrderBtn == "true") {
                            $("#crmhrefAddWorkOrder").css("display", "");
                        }
                        if (jsonData.IsShowSendMsgBtn == "true") {
                            $("#crmhrefSendMsg").css("display", "");
                        }
                        if (jsonData.IsShowQiCheTong == "true") {
                            $("#crmhrefQiCheTong").css("display", "");
                        }
                        if (jsonData.IsShowSubmitOrder == "true") {
                            $("#btnSubmitOrder").css("display", "");
                        }
                    }
                });
            }
            else { //如果没有CRM区域，按钮显示在基本信息区域
                $.post("/AjaxServers/OtherTask/OtherTaskDeal.ashx", { Action: "IsShowBtnByTTCode", TTCode: TTCode }, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "true") {
                        if (jsonData.IsShowWorkOrderBtn == "true") {
                            $("#hrefAddWorkOrder").css("display", "");
                        }
                        if (jsonData.IsShowSendMsgBtn == "true") {
                            $("#hrefSendMsg").css("display", "");
                        }
                        if (jsonData.IsShowQiCheTong == "true") {
                            $("#hrefQiCheTong").css("display", "");
                        }
                    }
                });
            }
        });
    }
    //绑定控件
    function BindFields(ttcode) {
        AjaxPostAsync('/AjaxServers/TemplateManagement/GetFieldList.ashx', { ttcode: ttcode }, null, function (data) {
            var jsonData = $.evalJSON(data);
            $(jsonData).each(function (i, v) {
                GetHtmlByShowCode(this, function (returnData, html) {
                    if (returnData.TFShowCode == "100020") {
                        //模板End-话务结果
                        $("#divCallResult").append(html);
                        $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
                    }
                    else {
                        $("#divBaseInfo").append(html);
                        $("#divBaseInfo li").last().data(returnData);
                        if (returnData.TFShowCode == "100014") {
                            $("#divBaseInfo li").first().attr("code", "100014");
                            $("#divBaseInfo li").first().hide();
                        }
                        else if (returnData.TFShowCode == "100015") {
                            //如果是个人用户
                            $("#divBaseInfo li").last().attr("code", "100015");
                        }
                        else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                            if (returnData.TFIsNull == "0") {
                                var textName = returnData.TFShowCode == "100016" ? "下单车型" : (returnData.TFShowCode == "100017" ? "意向车型" : "出售车型");
                                $("#divBaseInfo li").last().find("select[name$='Brand']").attr("vtype", "notFirstOption");
                                $("#divBaseInfo li").last().find("select[name$='Brand']").attr("vmsg", textName + "必须选择");
                            }
                        }
                    }
                });
            });
        });
    }
    function BindActivityEvent() {
        //如果有推荐活动，给推荐活动按钮加选择事件
        if ($("a[id$='_selectActivityA']").length != 0) {
            $("a[id$='_selectActivityA']").bind('click', OtherTaskSelectActivety);
            //给意向品牌加onchang事件
            if ($("select[id$='_YXBrand']").length != 0) {
                var YXBrandID = $("select[id$='_YXBrand']").first().attr("id");
                //由于意向车型已经有了onchange事件，所以此处要想在chang里执行其他方法要扩展onchange事件AttachEvent是扩展
                AttachEvent(YXBrandID, "change", ClearActivity)
            }
        }
    }
    //清空推荐活动
    function ClearActivity() {
        $("input[type='hidden'][id$='_Activity']").first().val("");
        $("input[id$='_Activity_Name']").first().val("");
    }
    //获取数据
    function GetCustomData() {
        //遍历所有自定义控件
        var arrhave = new Array();
        var body = "";
        $("#divBaseInfo li input,#divBaseInfo li select,#divBaseInfo li textarea").each(function () {
            //控件本身
            var itmes = $(this);
            //控件所对应的li
            var liitmes = itmes.parents('li')[0];
            //li的数据缓存
            var returnData = $(liitmes).data();
            //二级省市
            if (returnData.TFShowCode == "100012" || returnData.TFShowCode == "100013") {
                body += itmes.attr('id') + ":" + escape(itmes.val()) + ",";
                var selText = "";
                if (itmes.find('option:selected').text() != "请选择") {
                    selText = itmes.find('option:selected').text();
                }
                body += itmes.attr('id') + "_name:" + escape(selText) + ",";
            }
            //radio,checkbox
            else if (returnData.TFShowCode == "100003" || returnData.TFShowCode == "100004" || (returnData.TFShowCode == "100015" && returnData.TFDesName == "性别")) {
                //对于radio和checkbox多个控件对应一个name ,一个name只取一次
                if (arrhave.in_array(itmes.attr('name')) == false) {
                    arrhave.push(itmes.attr('name'));
                    var values = $("input[name='" + itmes.attr('name') + "']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(',');
                    body += itmes.attr('name') + ":" + escape(values) + ",";
                    var Texts = $("input[name='" + itmes.attr('name') + "']").map(function () {
                        if ($(this).attr("checked")) {
                            if ($(this).attr("textstr") == "请选择") {
                                return "";
                            }
                            else {
                                return $(this).attr("textstr");
                            }
                        }
                    }).get().join(',');
                    body += itmes.attr('name') + "_name:" + escape(Texts) + ",";
                }

            }
            else if (returnData.TFShowCode == "100005") {
                body += itmes.attr('id') + ":" + escape(itmes.val()) + ",";
                var selText = "";
                if (itmes.find('option:selected').text() != "请选择") {
                    selText = itmes.find('option:selected').text();
                }
                body += itmes.attr('id') + "_name:" + escape(selText) + ",";
            }
            else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                var selVal = itmes.val();
                if (selVal == null) {
                    selVal = -1;
                }
                body += itmes.attr('id') + ":" + selVal + ",";
                var selText = "";

                if (itmes.find('option:selected').text() != "请选择") {
                    var txt = itmes.find('option:selected').text();
                    if ($(itmes).attr("name") == "XDBrand" || $(itmes).attr("name") == "YXBrand" || $(itmes).attr("name") == "CSBrand") {
                        selText = txt.replace(/^[A-Z] /, ''); //去除最前面的大写字母
                    }
                    else {
                        selText = txt;
                    }
                }
                body += itmes.attr('id') + "_Name:" + escape(selText) + ",";
            }
            else {
                body += itmes.attr('id') + ":" + escape(itmes.val()) + ",";
            }
        });
        //获取话务结果
        if ($("#IsEstablish_selectid")[0]) {
            body += "IsEstablish:" + escape($("#IsEstablish_selectid").val()) + ",";
            body += "NotEstablishReason:" + escape($("#NotEstablishReason_selectid").val()) + ",";
            body += "IsSuccess:" + escape($("#IsSuccess_selectid").val()) + ",";
            body += "NotSuccessReason:" + escape($("#NotSuccessReason_selectid").val()) + ",";
        }
        return body;
    }
    //保存
    function SaveOtherTaskInfo() {
        //保存自定义表单
        var yanzArray = new Array();
        var jsonArray = $("#divBaseInfo input:text").serializeArray();

        for (var i = 0; i < jsonArray.length; i++) {
            if (jsonArray[i].value != "") {
                yanzArray.push(jsonArray[i]);
            }
        }
        //验证表单数据
        var returnStr = validateMsg(yanzArray);
        var returnObj = $.evalJSON(returnStr);
        var Message = "";
        if (returnObj.result == "false") {
            Message = returnObj.msg;
        }
        //验证问卷
        for (var i = 0; i < GetSurveyCount(); i++) {
            Message += eval("uCEditSurveyHelper_" + i).CheckDataForSurvey();
        }
        //判断是否使用了个人属性模板
        var CustNametemp = "";
        var Sextemp = "";
        var Telephonetemp = "";
        if (isPersonalInfo()) {
            CustNametemp = $("#divBaseInfo li[name='FullName']").find("span").find("input").val();
            Sextemp = $("#divBaseInfo li[name='FullSex']").find("span").find("input:checked").val();
            Telephonetemp = $("#divBaseInfo li[name='FullTel']").find("span").find("input").val();
        }
        if (Message == "") {
            var Msg = "";
            var podycustom = {
                Action: "SavecustomData",
                Body: GetCustomData(),
                RelationID: '<%=RelationID%>',
                RelationTableID: '<%=TTCode %>',
                TaskID: '<%=RequestTaskID%>',
                CustName: encodeURIComponent(CustNametemp),
                Sex: Sextemp,
                Telephone: Telephonetemp,
                r: Math.random()
            };
            AjaxPostAsync("../../AjaxServers/OtherTask/OtherTaskDeal.ashx", podycustom, null, function (data) {
                if (data == '') {
                    Msg = "success";
                }
                else {
                    Msg += data + "<br/>";
                }
            });
            //保存问卷
            var SIIDStr = GetSIIDStr();
            for (var i = 0; i < GetSurveyCount(); i++) {
                var jsonSurveyAnswer = eval("uCEditSurveyHelper_" + i).GetData();
                var pody = {
                    Action: 'SurveyAnswerSubmit',
                    SPIID: '',
                    SIID: SIIDStr.split(',')[i],
                    ProjectID: '<%=ProJectID%>',
                    PTID: '<%=RequestTaskID%>',
                    JsonSurveyAnswer: encodeURIComponent(jsonSurveyAnswer),
                    IsSub: 0,
                    r: Math.random()
                };
                AjaxPostAsync("../../AjaxServers/CustCheck/TakingAnSurveyHandler.ashx", pody, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.msg == 'success') {
                        Msg = "success";
                    }
                    else {
                        Msg += jsonData.msg + "<br/>";
                    }
                });
            }
            if (Msg != "" && Msg != "success") {
                $.jAlert(Msg);
            }
            else if (Msg == "success") {
                LoadCallRecord();
                LoadTaskLog();
                $.jPopMsgLayer("保存成功！");
            }
        } else {
            $.jAlert(Message);
        }
    }
    //提交
    function SubOtherTaskInfo() {
        var Message = "";
        //获取页面上的值，
        var jsonArray = $("#divBaseInfo input:text,#divBaseInfo select,#divBaseInfo textarea").serializeArray();
        //验证表单数据
        var returnStr = validateMsg(jsonArray);
        var returnObj = $.evalJSON(returnStr);
        //话务结果验证
        if (!CheckChange("IsEstablish")) {
            Message += "是否接通必须选择</br>";
        }
        if (!CheckChange("NotEstablishReason")) {
            Message += "未接通原因必须选择</br>";
        }
        if (!CheckChange("IsSuccess")) {
            Message += "是否成功必须选择</br>";
        }
        if (!CheckChange("NotSuccessReason")) {
            Message += "失败原因必须选择</br>";
        }

        if (returnObj.result == "false") {
            Message = returnObj.msg;
        }
        var arrhave = new Array();
        //验证radio和checkbox的必填
        $("#divBaseInfo input").each(function () {
            if ($(this).attr("type") == "radio" || $(this).attr("type") == "checkbox") {
                if (arrhave.in_array($(this).attr('name')) == false) {
                    arrhave.push($(this).attr('name'));

                    if (typeof ($(this).attr("vtype")) != "undefined") {
                        var values = $("input[name='" + $(this).attr('name') + "']").map(function () {
                            if ($(this).attr("checked")) {
                                return $(this).val();
                            }
                        }).get().join(',');
                        if (values == "") {
                            Message += $(this).attr("vmsg") + "<br/>";
                        }
                    }
                }
            }
        });

        //验证问卷
        for (var i = 0; i < GetSurveyCount(); i++) {
            Message += eval("uCEditSurveyHelper_" + i).CheckDataForSurvey();
        }
        //验证不通过
        if (Message != "") {
            $.jAlert(Message);
        }
        //验证通过
        else {
            var SurveyAnswerAll = "";
            for (var i = 0; i < GetSurveyCount(); i++) {
                SurveyAnswerAll += eval("uCEditSurveyHelper_" + i).CheckEmptyForSurvey();
            }
            if (SurveyAnswerAll != "") {
                $.jConfirm(SurveyAnswerAll + "\r\n确定要提交吗？", function (r) {
                    if (r) {
                        SubendInfo();
                    }
                });
            }
            else {
                SubendInfo();
            }
        }
    }
    function SubendInfo() {
        var SIIDStr = GetSIIDStr();
        //判断是否使用了个人属性模板
        var CustNametemp = "";
        var Sextemp = "";
        var Telephonetemp = "";
        if (isPersonalInfo()) {
            CustNametemp = $("#divBaseInfo li[name='FullName']").find("span").find("input").val();
            Sextemp = $("#divBaseInfo li[name='FullSex']").find("span").find("input:checked").val();
            Telephonetemp = $("#divBaseInfo li[name='FullTel']").find("span").find("input").val();
        }
        //后台报错信息
        var ErrorMes = "";
        //提交自定义表单表单
        var podycustom = {
            Action: 'SubcustomData',
            Body: GetCustomData(),
            RelationID: '<%=RelationID%>',
            RelationTableID: '<%=TTCode %>',
            TaskID: '<%=RequestTaskID%>',
            CustName: encodeURIComponent(CustNametemp),
            Sex: Sextemp,
            Telephone: Telephonetemp,
            r: Math.random()
        };
        AjaxPostAsync("../../AjaxServers/OtherTask/OtherTaskDeal.ashx", podycustom, null, function (data) {
            if (data == '') {
                ErrorMes = "success";
            }
            else {
                ErrorMes = data + "<br/>";
            }
        });
        for (var i = 0; i < GetSurveyCount(); i++) {
            var jsonSurveyAnswer = eval("uCEditSurveyHelper_" + i).GetData();
            var pody = {
                Action: 'SurveyAnswerSubmit',
                SPIID: '',
                SIID: SIIDStr.split(',')[i],
                ProjectID: '<%=ProJectID%>',
                PTID: '<%=RequestTaskID%>',
                JsonSurveyAnswer: encodeURIComponent(jsonSurveyAnswer),
                IsSub: 1,
                r: Math.random()
            };
            AjaxPostAsync("../../AjaxServers/CustCheck/TakingAnSurveyHandler.ashx", pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg == 'success') {
                    ErrorMes = "success";
                }
                else {
                    ErrorMes += jsonData.msg + "<br/>"
                }
            });
        }
        if (ErrorMes != "" && ErrorMes != "success") {
            $.jAlert(ErrorMes);
        }
        else if (ErrorMes == "success") {
            LoadTaskLog();
            $.jPopMsgLayer("提交成功！", function () { closePageExecOpenerSearch(); });
        }
    }
    function LoadCallRecord() {
        var pody = 'TaskID=<%=RequestTaskID %>&r=' + Math.random();
        LoadingAnimation("divCallRecordList");
        $("#divCallRecordList").load("../../AjaxServers/OtherTask/TaskCallRecordList.aspx", pody);

    }
    function LoadTaskLog() {
        var pody = 'TaskID=<%=RequestTaskID %>&r=' + Math.random();
        LoadingAnimation("divTaskLog");
        $("#divTaskLog").load("../../AjaxServers/OtherTask/TaskLogList.aspx", pody);
    }
    //是否加载过历史记录
    var isLoadList = 0;
    function divShowHideEvent(divId, obj) {
        if ($("#" + divId).css("display") == "none") {

            if (isLoadList == 0 && divId == 'infoBlock1') {
                //加载任务处理记录
                LoadTaskLog();
                //加载录音记录
                LoadCallRecord();
                isLoadList = 1;
            }

            $("#" + divId).show("slow");
            $(obj).attr("class", "othertoggle");
        }
        else {
            $("#" + divId).hide("slow");
            $(obj).attr("class", "othertoggle otherhide");
        }
    }
    function divShowHideEventForSurvey(divId, obj) {
        if ($("#" + divId).css("display") == "none") {
            $("#" + divId).show("slow");
            $(obj).attr("class", "othertoggle");
        }
        else {
            $("#" + divId).hide("slow");
            $(obj).attr("class", "othertoggle otherhide");
        }
    }
</script>
<script type="text/javascript">
    var property2 = {
        divId: "calen1", //日历控件最外层DIV的ID
        needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
        yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
        //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
        //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
        format: "yyyy-MM-dd hh:mm:ss"
        /*设定日期的输出格式,可不填*/
    };
</script>
<script type="text/javascript">
    $(document).ready(function () {
        setTimeout(LoadControl, 100);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(){ 
             //预先创建调查问卷实体类
             var num=0;
             for(num=0;num<100;num++)
             {
                 if($("#hidNum_"+num).val()==num)
                 {
                    eval("uCEditSurveyHelper_"+num+"=CreateUCEditSurveyHelper_New(num,$(\"#hidSIIDName_\"+num).val());");
                 }
                 else
                 {
                    break;
                 }
             }
        });
    });
    function LoadControl() {
        <%=MyClientScript %>;
    }

    //调查问卷实体类
    function CreateUCEditSurveyHelper_New(num,siidname)
    {
        return CreateUCEditSurveyHelper_Paras(num,siidname,'1','2','3','4','5');
    }
    function CreateUCEditSurveyHelper_Paras(num, siidname, radiot, checkboxt, textt, matrixradiot, matrixdropdownt) {
        var obj = (function () {
            var strlen = function (s) {
                var l = 0;
                var a = s.split("");
                for (var i = 0; i < a.length; i++) {
                    if (a[i].charCodeAt(0) < 299) {
                        l++;
                    } else {
                        l += 2;
                    }
                }
                return l;
            },
            //给个题型数组付值
            setArrayData = function (DataArray, i) {
                if (i == 1) {
                    var radiostr = $("#hidRadioSQID_" + num).val();
                    if (radiostr != "") {
                        for (var j = 0; j < radiostr.split(',').length; j++) {
                            if (DataArray.in_array(radiostr.split(',')[j]) == false) {
                                DataArray.push(radiostr.split(',')[j]);
                            }
                        }
                    }
                }
                if (i == 2) {
                    var radiostr = $("#hidCheckBoxSQID_" + num).val();
                    if (radiostr != "") {
                        for (var j = 0; j < radiostr.split(',').length; j++) {
                            if (DataArray.in_array(radiostr.split(',')[j]) == false)
                            { DataArray.push(radiostr.split(',')[j]); }
                        }
                    }
                }
                if (i == 3) {
                    var radiostr = $("#hidTextSQID_" + num).val();
                    if (radiostr != "") {
                        for (var j = 0; j < radiostr.split(',').length; j++) {
                            if (DataArray.in_array(radiostr.split(',')[j]) == false)
                            { DataArray.push(radiostr.split(',')[j]); }
                        }
                    }
                }
                if (i == 4) {
                    var radiostr = $("#hidMatrixRadioSQID_" + num).val();
                    if (radiostr != "") {
                        for (var j = 0; j < radiostr.split(',').length; j++) {
                            if (DataArray.in_array(radiostr.split(',')[j]) == false)
                            { DataArray.push(radiostr.split(',')[j]); }
                        }
                    }
                }
                if (i == 5) {
                    var radiostr = $("#hidMatrixDropSOID_" + num).val();
                    if (radiostr != "") {
                        for (var j = 0; j < radiostr.split(',').length; j++) {
                            if (DataArray.in_array(radiostr.split(',')[j]) == false)
                            { DataArray.push(radiostr.split(',')[j]); }
                        }
                    }
                }
            },
            MustRadio = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = radiot;
                    var flag = false;
                    $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                        emptystr += siidname + "，第" + RadioNoNumber + "题单选题必答。<br/>";
                    }

                }
                return emptystr;
            },
            //验证多选选不答情况
            MustCheck = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = checkboxt;
                    $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                        emptystr += siidname + "，第" + RadioNoNumber + "题多选题必答。<br/>";
                    }
                }
                return emptystr;
            },
            //验证文本不答情况
            MustText = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = textt;
                    $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).val() != "") {
                            flag = true;


                        }
                    });

                    if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                        emptystr += siidname + "，第" + RadioNoNumber + "题文本题必答。<br/>";
                    }
                }
                return emptystr;
            },
            //验证矩阵单选不答情况
            MustMatrixRadio = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = matrixradiot;
                    $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                        emptystr += siidname + "，第" + RadioNoNumber + "题矩阵单选题必答。<br/>";
                    }
                }
                return emptystr;
            },
            //验证矩阵下拉选不答情况
            MustMatrixSelect = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = matrixdropdownt;
                    $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).val() != "-1") {
                            flag = true;

                        }
                    });
                    if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                        emptystr += siidname + "，第" + RadioNoNumber + "题矩阵评分题必答。<br/>";
                    }
                }
                return emptystr;
            },
            //验证选择的选项如果要输入文本，文本必填
            CheckRadio = function (pre_arrRadio) {
                var emptystr = "";
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = radiot;
                    $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            if ($("#" + $(this).val()).attr("showflag") == "1") {
                                if ($("#" + $(this).val()).val() == "") {
                                    $("#" + $(this).val()).focus();
                                    emptystr += siidname + "，第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。<br/>";

                                }
                            }

                        }
                    });
                }
                return emptystr;
            },
            //验证选择的选项如果要输入文本，文本必填
            CheckCheckBoxLong = function (pre_arrRadio) {
                var emptystr = "";

                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var checkednum = 0;
                    var asktype = checkboxt;
                    $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            checkednum = checkednum + 1;
                        }
                    });
                    var maxlong = $('#' + pre_arrRadio[i]).attr("maxLong");
                    var minlong = $('#' + pre_arrRadio[i]).attr("minLong");
                    if (maxlong != "" && maxlong != "0") {
                        if (checkednum > parseInt(maxlong)) {
                            emptystr += siidname + "，第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能大于" + maxlong + "项。<br/>";
                        }
                    }
                    if (minlong != "" && minlong != "0") {
                        if (checkednum < parseInt(minlong)) {
                            emptystr += siidname + "，第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能小于" + minlong + "项。<br/>";
                        }
                    }
                }
                return emptystr;
            },
            //验证选择的选项如果要输入文本，文本必填
            CheckCheckType = function (pre_arrRadio) {
                var emptystr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = checkboxt;
                    $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            if ($("#" + $(this).val()).attr("showflag") == "1") {
                                if ($("#" + $(this).val()).val() == "") {
                                    $("#" + $(this).val()).focus();
                                    emptystr += siidname + "，第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。<br/>";
                                }
                            }

                        }
                    });
                }
                return emptystr;
            },
            //验证文本长度
            CheckText = function (pre_arrRadio) {
                var stralter = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = textt;
                    $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).val() != "") {

                            if ($(this).attr("maxLong") != "" && $(this).attr("minLong") != "") {
                                if (strlen($(this).val()) > parseInt($(this).attr("maxLong"))) {
                                    $(this).focus();
                                    stralter += siidname + "，第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能大于最大允许输入长度" + parseInt($(this).attr("maxLong")) + "个字符。<br/>";
                                }
                                if (strlen($(this).val()) < parseInt($(this).attr("minLong"))) {
                                    $(this).focus();
                                    stralter += siidname + "，第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能小于最小允许输入长度" + parseInt($(this).attr("minLong")) + "个字符。<br/>";
                                }
                            }
                        }
                    });
                }
                return stralter;
            },
            //验证矩阵单选选择一个，就都选
            CheckMatrixRadio = function (pre_arrRadio) {
                var stralter = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var j = 0;
                    var num = "";
                    var asktype = matrixradiot;
                    var liecout = 0;
                    $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        liecout = $(this).attr("lie")
                        num = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            //选中个数
                            j = j + 1;
                        }
                    });
                    if (j != 0) {
                        var hang = $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "'][lie='" + liecout + "']").length;
                        if (hang > 0 && hang > j) {
                            stralter += siidname + "，第" + num + "为矩阵单选题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。<br/>";
                        }
                    }
                }
                return stralter;
            },
            //验证矩阵下拉选选择一个，就都选
            CheckMatrixDrop = function (pre_arrRadio) {
                var stralter = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var j = 0;
                    var num = "";
                    var asktype = matrixdropdownt;

                    $("select[SQID$='" + pre_arrRadio[i] + "']").each(function () {
                        //判断题型，如果是单选题

                        num = $(this).parents("li[nums]").attr("nums");
                        if ($(this).val() != "-1") {
                            j = j + 1;

                        }
                    });

                    if (j != 0) {
                        var hang = $("select[SQID$='" + pre_arrRadio[i] + "']").length;
                        if (hang > 0 && hang > j) {
                            stralter += siidname + "，第" + num + "为矩阵评分题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。<br/>";
                        }
                    }
                }
                return stralter;
            },
            //验证单选不答情况
            emptyRadio = function (pre_arrRadio, arryNum) {
                var emptystr = 0;
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = radiot;
                    var flag = false;
                    $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false) {
                        arryNum.push(parseInt(RadioNoNumber));
                        emptystr += 1;
                    }

                }
                return emptystr;
            },
            //验证多选选不答情况
            emptyCheck = function (pre_arrRadio, arryNum) {
                var emptystr = 0;
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = checkboxt;
                    $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false) {
                        arryNum.push(parseInt(RadioNoNumber));
                        emptystr += 1;
                    }
                }
                return emptystr;
            },
            //验证文本不答情况
            emptyText = function (pre_arrRadio, arryNum) {
                var emptystr = 0;
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = textt;
                    $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).val() != "") {
                            flag = true;


                        }
                    });

                    if (flag == false) {
                        arryNum.push(parseInt(RadioNoNumber));
                        emptystr += 1;
                    }
                }
                return emptystr;
            },
            //验证矩阵单选不答情况
            emptyMatrixRadio = function (pre_arrRadio, arryNum) {
                var emptystr = 0;
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = matrixradiot;
                    $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            flag = true;

                        }
                    });
                    if (flag == false) {
                        arryNum.push(parseInt(RadioNoNumber));
                        emptystr += 1;
                    }
                }
                return emptystr;
            },
            //验证矩阵下拉选不答情况
            emptyMatrixSelect = function (pre_arrRadio, arryNum) {
                var emptystr = 0;
                var RadioNoNumber = 0;
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var flag = false;
                    var asktype = matrixdropdownt;
                    $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").each(function () {
                        RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                        //判断题型，如果是单选题
                        if ($(this).val() != "-1") {
                            flag = true;

                        }
                    });
                    if (flag == false) {
                        arryNum.push(parseInt(RadioNoNumber));
                        emptystr += 1;
                    }
                }
                return emptystr;
            },
            //取单选大题数据
            GetDataRadio = function (pre_arrRadio) {
                var dataStr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = radiot;
                    $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            var answercontent = "";
                            if ($("#" + $(this).val()).attr("showflag") == "1") {
                                answercontent = $("#" + $(this).val()).val()

                            }
                            var emptystr = "";
                            dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(answercontent) + "'},";

                        }
                    });

                }

                return dataStr;
            },
            //取多选大题数据
            GetCheckData = function (pre_arrRadio) {
                var dataStr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = checkboxt;
                    $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            var answercontent = "";
                            if ($("#" + $(this).val()).attr("showflag") == "1") {
                                answercontent = $("#" + $(this).val()).val()

                            }
                            var emptystr = "";
                            dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(answercontent) + "'},";

                        }
                    });

                }
                return dataStr;
            },
            //取文本
            GetDataText = function (pre_arrRadio) {
                var dataStr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = textt;

                    $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                        //如果文本题不答不保存
                        if ($(this).val() != "") {
                            var emptystr = "";
                            dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent(emptystr) + "','AnswerContent':'" + encodeURIComponent($(this).val()) + "'},";
                        }
                    });
                }
                return dataStr;
            },
            //取矩阵单选答题数据
            GetDataMatrixRadio = function (pre_arrRadio) {
                var dataStr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = matrixradiot;
                    $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).attr("checked")) {
                            var emptystr = "";
                            dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent($(this).attr("name").split('_')[0]) + "','SMCTID':'" + encodeURIComponent($(this).attr("lie")) + "','SOID':'" + encodeURIComponent($(this).attr("lie")) + "','AnswerContent':'" + encodeURIComponent(emptystr) + "'},";
                        }
                    });
                }
                return dataStr;
            },
            //取矩阵下拉选数据
            GetDataMatrixSelect = function (pre_arrRadio) {
                var dataStr = "";
                for (var i = 0; i < pre_arrRadio.length; i++) {
                    var asktype = matrixdropdownt;
                    $("select[SQID='" + pre_arrRadio[i] + "']").each(function () {
                        //判断题型，如果是单选题
                        if ($(this).val() != "-1") {
                            var emptystr = "";
                            dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent($(this).attr("hang")) + "','SMCTID':'" + encodeURIComponent($(this).attr("lie")) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(emptystr) + "'},";
                        }
                    });
                }
                return dataStr;
            },
            CheckDataForSurvey = function () {
                //声明数组
                //单选题数组,把不同题型的SQID保存到不同的数组里
                var pre_arrRadio = new Array();
                setArrayData(pre_arrRadio, 1);
                var pre_arrCheck = new Array();
                setArrayData(pre_arrCheck, 2);
                var pre_arrText = new Array();
                setArrayData(pre_arrText, 3);
                var pre_arrMatrixRadio = new Array();
                setArrayData(pre_arrMatrixRadio, 4);
                var pre_arrMatrixDrop = new Array();
                setArrayData(pre_arrMatrixDrop, 5);

                //是否验证通过
                var flagRadio = "";
                //是否验证通过
                flagRadio += MustRadio(pre_arrRadio);
                flagRadio += MustCheck(pre_arrCheck);
                flagRadio += MustText(pre_arrText);
                flagRadio += MustMatrixRadio(pre_arrMatrixRadio);
                flagRadio += MustMatrixSelect(pre_arrMatrixDrop);
                flagRadio += CheckRadio(pre_arrRadio);
                flagRadio += CheckCheckBoxLong(pre_arrCheck);
                flagRadio += CheckCheckType(pre_arrCheck);
                flagRadio += CheckText(pre_arrText);
                flagRadio += CheckMatrixRadio(pre_arrMatrixRadio);
                flagRadio += CheckMatrixDrop(pre_arrMatrixDrop);
                return flagRadio;
            },

            CheckEmptyForSurvey = function () {
                //声明数组
                //单选题数组,把不同题型的SQID保存到不同的数组里
                var pre_arrRadio = new Array();
                setArrayData(pre_arrRadio, 1);
                var pre_arrCheck = new Array();
                setArrayData(pre_arrCheck, 2);
                var pre_arrText = new Array();
                setArrayData(pre_arrText, 3);
                var pre_arrMatrixRadio = new Array();
                setArrayData(pre_arrMatrixRadio, 4);
                var pre_arrMatrixDrop = new Array();
                setArrayData(pre_arrMatrixDrop, 5);
                //不答提示
                var emptystr = "";
                var arryNum = new Array();
                //取单选题不答提示
                emptyRadio(pre_arrRadio, arryNum);
                //取多选题不答提示
                emptyCheck(pre_arrCheck, arryNum);
                //取文本题不答提示
                emptyText(pre_arrText, arryNum);
                //取矩阵单选不答提示
                emptyMatrixRadio(pre_arrMatrixRadio, arryNum);
                //取矩阵下拉不答提示
                emptyMatrixSelect(pre_arrMatrixDrop, arryNum);
                $.each(arryNum.sort(function AscSort(x, y) {
                    return x == y ? 0 : (x > y ? 1 : -1);
                }), function (i, item) {
                    if (i == 0) {
                        emptystr += item;
                    }
                    else {
                        emptystr += "," + item;
                    }
                });
                if (emptystr != 0) {
                    return siidname + "，还有" + emptystr + "题没答。<br/>";
                }
                else {
                    return "";
                }
            },
            //取数据
            GetData = function () {
                var pre_arrRadio = new Array();
                setArrayData(pre_arrRadio, 1);
                var pre_arrCheck = new Array();
                setArrayData(pre_arrCheck, 2);
                var pre_arrText = new Array();
                setArrayData(pre_arrText, 3);
                var pre_arrMatrixRadio = new Array();
                setArrayData(pre_arrMatrixRadio, 4);
                var pre_arrMatrixDrop = new Array();
                setArrayData(pre_arrMatrixDrop, 5);
                var msg = "{DataRoot:[";
                var DataRadio = GetDataRadio(pre_arrRadio);
                var DataCheckBox = GetCheckData(pre_arrCheck);
                var DataText = GetDataText(pre_arrText);
                var DataMatrixRadio = GetDataMatrixRadio(pre_arrMatrixRadio);
                var DataMatrixDrop = GetDataMatrixSelect(pre_arrMatrixDrop);
                if (DataRadio != "") {
                    msg += DataRadio;
                }
                if (DataCheckBox != "") {
                    msg += DataCheckBox;
                }
                if (DataText != "") {
                    msg += DataText;
                }
                if (DataMatrixRadio != "") {
                    msg += DataMatrixRadio;
                }
                if (DataMatrixDrop != "") {
                    msg += DataMatrixDrop;
                }
                if (msg != "{DataRoot:[") {
                    msg = msg.substring(0, msg.length - 1);
                }
                msg += "]}";
                return msg;
            };
            return {
                strlen: strlen,
                setArrayData: setArrayData,
                MustRadio: MustRadio,
                MustCheck: MustCheck,
                MustText: MustText,
                MustMatrixRadio: MustMatrixRadio,
                MustMatrixSelect: MustMatrixSelect,
                CheckRadio: CheckRadio,
                CheckCheckBoxLong: CheckCheckBoxLong,
                CheckCheckType: CheckCheckType,
                CheckText: CheckText,
                CheckMatrixRadio: CheckMatrixRadio,
                CheckMatrixDrop: CheckMatrixDrop,
                emptyRadio: emptyRadio,
                emptyCheck: emptyCheck,
                emptyText: emptyText,
                emptyMatrixRadio: emptyMatrixRadio,
                emptyMatrixSelect: emptyMatrixSelect,
                GetDataRadio: GetDataRadio,
                GetCheckData: GetCheckData,
                GetDataText: GetDataText,
                GetDataMatrixRadio: GetDataMatrixRadio,
                GetDataMatrixSelect: GetDataMatrixSelect,
                CheckDataForSurvey: CheckDataForSurvey,
                CheckEmptyForSurvey: CheckEmptyForSurvey,
                GetData: GetData
            };
        })();
        return obj;
    }
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="taskT">
    <%=TPName%></div>
<input id="hdnCrmCustID" type="hidden" />
<div class="baseInfo clearfix baseInfo5" id="divCrmBlock" style="width: 1000px; display: none;">
    <div class="mbInfo" style="clear: both;">
        Crm客户基本信息
    </div>
    <div class="zdset clearfix" style="margin-bottom: 0px;">
        <div id="divCrmBaseInfo">
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div class="editTemplate readTemplate" style="padding-bottom: 0px">
    <div class="mbInfo" style="clear: both;">
        <span id="spanInfo">基本信息</span>（<%=RequestTaskID%>）
    </div>
    <div class="zdset clearfix" style="margin-bottom: 0px;">
        <ul id="divBaseInfo">
        </ul>
        <div class="clear">
        </div>
        <ul id="divCallResult" class="clear" style="border-top: #999 1px dotted; height: 100px;">
        </ul>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<%--问卷调查--%>
<div class="addzs">
    <div id="divSurvey">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate>
                <asp:Button ID="Button_Async" runat="server" Text="Button" OnClick="Button_Async_Click"
                    Style="display: none" />
                <asp:PlaceHolder ID="PlaceHolderSurvey" runat="server"></asp:PlaceHolder>
                <input id="hiddenSurveyCount" type="hidden" runat="server" value="" cid="hiddenSurveyCount" />
                <input id="hiddenSIIDStr" type="hidden" runat="server" value="" cid="hiddenSIIDStr" />
            </contenttemplate>
        </asp:UpdatePanel>
    </div>
</div>
<%--历史记录--%>
<div class="baseInfo">
    <div class="cont_cx khxx CustInfoArea">
        <div class="mbInfo" style="clear: both;">
            记录历史<a class="othertoggle" onclick="divShowHideEvent('infoBlock1',this)" href="javascript:void(0)"></a>
        </div>
        <div id="infoBlock1" style="display: none;">
            <ul class="infoBlock firstPart">
                <li class="singleRow">
                    <div style="">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;操作记录</div>
                    <div id="divTaskLog" class="fullRow cont_cxjg" style="margin-left: 78px;">
                    </div>
                </li>
                <li class="singleRow">
                    <div style="">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;通话记录</div>
                    <div id="divCallRecordList" class="fullRow cont_cxjg" style="margin-left: 78px;">
                    </div>
                </li>
            </ul>
        </div>
    </div>
</div>
<input type="hidden" id="hidCallID" />
