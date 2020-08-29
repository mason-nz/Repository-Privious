<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask.OtherTaskView" %>
<script type="text/javascript">

    //试卷个数
    var SurveyCount = '<%= SurveyCount %>';
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
            if (isPersonalInfo()) {
                $("#spanInfo").text("个人基本信息");
            }
        }
    }

    function isPersonalInfo() {
        //判断是否使用了 个人用户属性模板
        if ($("#divBaseInfo li[name='FullName']").length == 0 || $("#divBaseInfo li[name='FullSex']").length == 0 || $("#divBaseInfo li[name='FullTel']").length == 0) {
            //未使用
            return false;
        }

        return true;
    }

    //绑定控件数据
    function BindData(TTCode, RelationID) {
        var jsonData = "";
        AjaxPostAsync('/AjaxServers/OtherTask/OtherTaskDeal.ashx', { RelationTableID: escape(TTCode), RelationID: escape(RelationID), Action: escape('GetCustomDataInfo') }, null, function (data) {
            jsonData = $.evalJSON(data);

            var isLoadCustInfo = 0;
            //遍历自定义控件
            //遍历input控件
            $("#divBaseInfo li input,#divBaseInfo li select,#divBaseInfo li textarea,#divBaseInfo li span").each(function () {
                $(this).attr("disabled", true);
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
                 }
                 );
                }
                else {
                    //如果是查看页面，让所有控件不可编辑
                    //$(this).attr("readonly",true);
                    //
                    var id = $(this).attr("id");
                    $.each(jsonData, function (i) {
                        var obj = jsonData[i];
                        for (var key in obj) {
                            if (key.indexOf("_crmcustid_name") > 0 && isLoadCustInfo == 0) {
                                $("#divCrmBlock").css("display", "");
                                $("#hdnCrmCustID").val(obj[key]);
                                $("#divCrmBaseInfo").load("/OtherTask/UCOtherTask/CustInfoView.aspx", { CustID: obj[key] }, function () {
                                    $("td a,img[id!='imgStatus'][id!='imgLock']").hide();
                                });
                                isLoadCustInfo = 1;
                            }

                            if (id == "span_Activity") {

                                //对推荐活动调用接口取推荐活动
                                if (key.substring(key.length - 9, key.length) == "_Activity") {
                                    if (unescape(obj[key]).length > 0) {
                                        var ActivityGUIDStr = unescape(obj[key]);
                                        AjaxPostAsync('/AjaxServers/OtherTask/OtherTaskDeal.ashx', { ActivityGuidStr: escape(ActivityGUIDStr), Action: escape('GetActivityInfo') }, null, function (ActivityData) {
                                            jsonActivityData = $.evalJSON(ActivityData);

                                            $.each(jsonActivityData, function (j) {
                                                if (jsonActivityData[j].URL != "") {
                                                    $("#span_Activity").append("<a target='_blank' href='" + unescape(jsonActivityData[j].URL) + "'>" + unescape(jsonActivityData[j].Name) + "</a>&nbsp;&nbsp;");
                                                }
                                                else {
                                                    $("#span_Activity").append("<span style='float:none;'>" + unescape(jsonActivityData[j].Name) + "</span>&nbsp;&nbsp;");
                                                }
                                            });


                                        });
                                    }
                                }


                            }

                            if (key == id) {
                                var val = unescape(obj[key]); //属性值
                                $("#" + id).val(val);
                                if ($("#" + id).attr("vtype") == "isDate") {
                                    if ($("#" + id).val() == "1900-1-1 0:00:00") {
                                        $("#" + id).val("");
                                    }
                                    if ($("#" + id).val().split(' ').length == 2) {
                                        $("#" + id).val($("#" + id).val().split(' ')[0]);
                                    }
                                }
                                else if ($("#" + id).attr("vtype") == "isDateTime") {
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
                                }
                                if (id.split('_').length == 3) {
                                    //品牌、车型
                                    if ((id.split('_')[1] == "XDBrand" || id.split('_')[1] == "YXBrand" || id.split('_')[1] == "CSBrand") && id.split('_')[2] == "Name") {
                                        var serialselectID = id.replace('Brand', 'Serial'); //获取车型的下拉列表ID

                                        var span = $("#" + id).parent();

                                        //品牌
                                        if ($.trim(unescape(obj[id])) != "") {
                                            $(span).append('<select  class="w100 ListBrandSerial" disabled=""><option value="-1">' + unescape(obj[id]) + '</option></select>');
                                        }
                                        else {
                                            $(span).append('<select  class="w100 ListBrandSerial" disabled=""><option value="-1">请选择</option></select>');
                                        }

                                        //车型
                                        if ($.trim(unescape(obj[serialselectID])) != "") {
                                            $(span).append('<select  class="w100 ListBrandSerial" disabled=""><option value="-1">' + unescape(obj[serialselectID]) + '</option></select>');
                                        }
                                        else {
                                            $(span).append('<select  class="w100 ListBrandSerial" disabled=""><option value="-1">请选择</option></select>');
                                        }
                                        $(span).css("margin-top", "15px");

                                        $("#" + id).remove();
                                        $("#" + serialselectID).remove();
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
                        $("#" + returnData.TFName + "_selectid").attr("disabled", "disabled");
                        $("#divCallResult li[name='" + returnData.TFName + "']").data(returnData);
                    }
                    else {
                        //如果是推荐活动，要自己加html
                        if (returnData.TFShowCode == "100019") {
                            //首先加一个控件span,等加载值时调用根据id取推荐活动，根据活动是否有链接决定span里面内容是链接还是汉字
                            if (returnData.TFCssName == "hkId") {
                                $("#divBaseInfo").append("<li class='" + returnData.TFCssName + "'><label>推荐活动：</label><span id='span_Activity' style='margin-top: 0px;float:left; clear:none;width:800px;'></span></li>");
                            }
                            else {
                                $("#divBaseInfo").append("<li class='" + returnData.TFCssName + "'><label>推荐活动：</label><span id='span_Activity' style='margin-top: 10px;width:345px;line-height:30px;'></span></li>");
                            }
                            $("#divBaseInfo li").last().data(returnData);
                        }
                        else {
                            $("#divBaseInfo").append(html);
                            $("#divBaseInfo li").last().data(returnData);
                            if (returnData.TFShowCode == "100014") {
                                $("#divBaseInfo li").first().hide();
                            }
                            else if (returnData.TFShowCode == "100015") {
                                //如果是个人用户

                                $("#divBaseInfo li").last().attr("code", "100015");
                            }
                            else if (returnData.TFShowCode == "100016" || returnData.TFShowCode == "100017" || returnData.TFShowCode == "100018") {
                                $("#divBaseInfo li").last().find("select.ListBrandSerial").each(function (i, v) {
                                    //$(v).parent().after("<span id='" + $(v).attr("id") + "_Name" + "' style='margin-right:10px;' ><span>");
                                    //$(v).remove();
                                    $(v).attr("id", $(v).attr("id") + "_Name");
                                });
                                $("#divBaseInfo li").last().find("span").css("margin-top", "0px");
                            }
                        }
                    }
                });
            });
        });
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
            $(obj).attr({ src: '../images/zhankai.gif', title: '展开' });
            $("#" + divId).show("slow");
            $(obj).attr("class", "othertoggle");
        }
        else {
            $(obj).attr({ src: '/images/shouqi.gif', title: '收起' });
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
<div class="taskT">
    <%=TPName%></div>
<div class="baseInfo clearfix" id="divCrmBlock" style="width: 1000px; display: none;">
    <input id="hdnCrmCustID" type="hidden" />
    <div class="mbInfo" style="clear: both;">
        Crm客户基本信息<a class="othertoggle" onclick="divShowHideEvent('divCrmBaseInfo',this)"
            href="javascript:void(0)"></a>
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
    <div class="mbInfo">
        <span id="spanInfo">基本信息</span>（<%=RequestTaskID%>）<a class="othertoggle" onclick="divShowHideEvent('divBaseInfo',this)"
            href="javascript:void(0)"></a>
    </div>
    <div class="zdset clearfix" style="margin-bottom: 0px;">
        <ul id="divBaseInfo">
        </ul>
        <div class="clear">
        </div>
        <ul id="divCallResult" class="clear" style="border-top: #999 1px dotted; height:100px;">
        </ul>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div class="addzs">
    <%--<div class="mbInfo">
        问卷调查<a class="othertoggle" onclick="divShowHideEvent('divSurvey',this)" href="javascript:void(0)"></a>
    </div>--%>
    <div id="divSurvey">
        <asp:PlaceHolder ID="PlaceHolderSurvey" runat="server"></asp:PlaceHolder>
    </div>
</div>
<div class="cont_cx khxx CustInfoArea">
    <div class="mbInfo">
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
