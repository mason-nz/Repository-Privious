<%@ Page Title="任务列表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="TaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.TaskList1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            //填充空格，以使checkbox对齐，按照3个字对齐，少一个字填充3个&nbsp;
            $("em").each(function () {
                var em_val = $.trim($(this).html());
                var add_nbsp = "";
                for (var i = em_val.length; i < 3; i++) {
                    add_nbsp += "&nbsp;&nbsp;&nbsp;";
                }
                $(this).html(em_val + add_nbsp);
            });

            chkTask();

            selChangeType();

            search();

            //敲回车键执行方法
            enterSearch(search);
        });
        //咨询类别的问题类别 联动
        function selChangeType() {
            //联动时 清除 问题类别 复选框和单选框值
            $("[name='chkQuestionType']").each(function () {
                $(this).attr("checked", false);
            });
            var _value = $("#selConsult").val();
            var name = "";
            if (_value != "-1") {
                switch (_value) {
                    case "60001": name = "";
                        break;
                    case "60002": name = "SecondCar";
                        break;
                    case "60003": name = "PFeedback";
                        break;
                    case "60004": name = "Activity";
                        break;
                    case "60005": name = "PUseCar";
                        break;
                    case "60006": name = "";
                        break;
                    case "60007": name = "DCoop1";
                        break;
                    case "60008": name = "DCoop2";
                        break;
                    case "60009": name = "";
                        break;
                }
            }

            if (name != "" && name != undefined) {
                $("li[id^='li_QuestionType']").css("display", "none");
                $("#li_QuestionType" + name).css("display", "block");
            }
            else {
                $("li[id^='li_QuestionType']").css("display", "none");
            }
        }

        //处理状态 联动
        function chkTask() {
            var taskStatusNow = $(":checkbox[id='chkTaskStatusNow']").attr("checked");
            if (taskStatusNow == true) {
                $(":checkbox[name='chkProcess']").removeAttr("checked");
                $("li[id='liProcess']").css("display", "block");
            }
            else {
                $("li[id='liProcess']").css("display", "none");
            }
        }

        //查询
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/TaskManager/TaskList.aspx .bit_table", pody);
        }

        function params() {
            var taskID = $.trim($("#txtTaskID").val());
            var custName = $.trim($("#txtCustName").val());

            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    $.jAlert("日期格式不正确！");
                    return false;
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    $.jAlert("日期格式不正确！");
                    return false;
                }
            }
            var consultID = $("#selConsult").val() == "-1" ? "" : $("#selConsult").val();
            var questionType = "";
            var typeValue = $("[name='chkQuestionType']:checked").map(function () {
                return $(this).val();
            }).get().join(',');
            questionType = typeValue;

            var questionQuality = $(":checkbox[name='chkQuestionQuality']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');

            var isComplaint = $(":checkbox[name='chkIsComplaint']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');

            var isForwarding = $(":checkbox[name='chkIsForwarding']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var taskStatus = $(":checkbox[name='chkTaskStatus']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');

            var status = "";
            if ($("#liProcess")[0].style.display == "block") {
                status = $("#liProcess :checkbox[name='chkProcess']").map(function () {
                    if ($(this).attr("checked")) {
                        return $(this).val();
                    }
                }).get().join(',');
            }
            var pody = 'TaskID=' + taskID + '&CustName=' + escape(custName) + '&BeginTime=' + beginTime + '&EndTime=' + endTime +
                        '&ConsultID=' + consultID + '&QuestionType=' + questionType + '&QuestionQuality=' + questionQuality +
                        '&IsComplaint=' + isComplaint + '&IsForwarding=' + isForwarding + '&ProcessStatus=' + taskStatus +
                        '&Status=' + status + '&r=' + Math.random();


            $("#formExport [name='TaskID']").val(escapeStr(taskID));
            $("#formExport [name='CustName']").val(escapeStr(custName));
            $("#formExport [name='BeginTime']").val(escapeStr(beginTime));
            $("#formExport [name='EndTime']").val(escapeStr(endTime));
            $("#formExport [name='ConsultID']").val(escapeStr(consultID));
            $("#formExport [name='QuestionType']").val(escapeStr(questionType));
            $("#formExport [name='QuestionQuality']").val(escapeStr(questionQuality));
            $("#formExport [name='IsComplaint']").val(escapeStr(isComplaint));
            $("#formExport [name='IsForwarding']").val(escapeStr(isForwarding));
            $("#formExport [name='ProcessStatus']").val(escapeStr(taskStatus));
            $("#formExport [name='Status']").val(escapeStr(status));

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../AjaxServers/TaskManager/TaskList.aspx .bit_table > *', pody);
        }

        function ExportExcel() {

            $("#hidIsOkOrCancel").val("0");

            $.openPopupLayer({
                name: "DisposeSetPoper",
                parameters: {},
                url: "TaskFields.aspx",
                afterClose: function (e, data) {

                    if ($("#hidIsOkOrCancel").val() == "1") { //是否是点击了确定后关闭的
                        var par = params();
                        var ids = $("#hidFieldsTask").val();
                        $("#formExport [name='field']").val(escapeStr(ids));
                        $("#formExport").submit();
                    }
                }
            });
        }
         
    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    任&nbsp;务&nbsp;ID：</label>
                <input type="text" id="txtTaskID" class="w190" />
            </li>
            <li>
                <label>
                    任务状态：</label>
                <span>
                    <input type="checkbox" value="150001" id="chkTaskStatusWait" name="chkTaskStatus"
                        onclick="chkTask(0)" checked="checked" /><em onclick="emChkIsChoose(this);chkTask(0)">待处理</em></span>
                <span>
                    <input type="checkbox" value="150002" id="chkTaskStatusNow" name="chkTaskStatus"
                        onclick="chkTask(1)" checked="checked" /><em onclick="emChkIsChoose(this);chkTask(1)">处理中</em></span>
                <span>
                    <input type="checkbox" value="150003" id="chkTaskStatusOver" name="chkTaskStatus"
                        onclick="chkTask(0)" /><em onclick="emChkIsChoose(this);chkTask(0)">已处理</em></span>
            </li>
            <li style="display: none;" id="liProcess">
                <label>
                    处理状态：</label>
                <span>
                    <input type="checkbox" value="110001" id="chkProcessSolve" name="chkProcess" /><em
                        onclick="emChkIsChoose(this)">已解决</em></span> <span>
                            <input type="checkbox" value="110003" id="chkProcessUnresolved" name="chkProcess" /><em
                                onclick="emChkIsChoose(this)">未解决</em></span> <span>
                                    <input type="checkbox" value="110002" id="chkProcessNotSolve" name="chkProcess" /><em
                                        onclick="emChkIsChoose(this)">不解决</em></span> </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户姓名：</label>
                <input type="text" id="txtCustName" class="w190" />
            </li>
            <li style="width: 278px; *width: 277px; width: 275px\9">
                <label>
                    问题性质：</label>
                <span>
                    <input type="checkbox" id="chkQuestionQualityUrgent" value="160004" name="chkQuestionQuality" /><em
                        onclick="emChkIsChoose(this)">紧急</em></span> <span>
                            <input type="checkbox" id="chkQuestionQualityCommon" value="160005" name="chkQuestionQuality" /><em
                                onclick="emChkIsChoose(this)">普通</em></span> </li>
            <li>
                <label>
                    是否转发：</label>
                <span>
                    <input type="checkbox" value="1" id="chkIsForwardingYes" name="chkIsForwarding" /><em
                        onclick="emChkIsChoose(this)">是</em></span> <span>
                            <input type="checkbox" value="0" id="chkIsForwardingNo" name="chkIsForwarding" /><em
                                onclick="emChkIsChoose(this)">否</em></span> </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    创建日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" onclick="MyCalendar.SetDate(this,document.getElementById('tfBeginTime'))"
                    class="w85" style="width: 84px; *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" onclick="MyCalendar.SetDate(this,document.getElementById('tfEndTime'))"
                    style="width: 84px; *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    是否投诉：</label>
                <span>
                    <input type="checkbox" value="1" id="chkIsComplaintYes" name="chkIsComplaint" /><em
                        onclick="emChkIsChoose(this)">是</em></span> <span>
                            <input type="checkbox" value="0" id="chkIsComplaintNo" name="chkIsComplaint" /><em
                                onclick="emChkIsChoose(this)">否</em></span> </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    咨询类别：</label>
                <select id="selConsult" onchange="javascript:selChangeType()" class="w125" style="width: 192px;
                    *width: 194px; width: 194px\9">
                    <option value="-1">请选择</option>
                    <option value="60001">新车</option>
                    <option value="60002">二手车</option>
                    <option value="60003">个人反馈</option>
                    <%--  <option value="60004">活动</option>--%>
                    <option value="60006">个人其他</option>
                    <option value="60005">个人用车</option>
                    <option value="60007">经销商合作</option>
                    <option value="60008">经销商反馈</option>
                    <option value="60009">经销商其他</option>
                </select>
            </li>
            <li id="li_QuestionTypePFeedback" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" value="80001" id="QuestionTypePFeedback1" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">论坛</em></span> <span>
                            <input type="checkbox" value="80002" id="QuestionTypePFeedback2" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">编辑</em></span> <span>
                                    <input type="checkbox" value="80003" id="QuestionTypePFeedback3" name="chkQuestionType" /><em
                                        onclick="emChkIsChoose(this)">经销商</em></span> <span>
                                            <input type="checkbox" value="80004" id="QuestionTypePFeedback4" name="chkQuestionType" /><em
                                                onclick="emChkIsChoose(this)">产品</em></span> <span>
                                                    <input type="checkbox" value="80005" id="QuestionTypePFeedback5" name="chkQuestionType" /><em
                                                        onclick="emChkIsChoose(this)">活动</em></span>
                <span>
                    <input type="checkbox" value="80006" id="QuestionTypePFeedback6" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">呼叫中心</em></span> </li>
            <li id="li_QuestionTypeActivity" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" id="QuestionTypeActivity1" value="1" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">活动前</em></span> <span>
                            <input type="checkbox" id="QuestionTypeActivity2" value="1" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">活动后</em></span> </li>
            <li id="li_QuestionTypeSecondCar" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" id="QuestionTypeSecondCar1" value="70001" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">买车</em></span> <span>
                            <input type="checkbox" id="QuestionTypeSecondCar2" value="70002" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">卖车</em></span> <span>
                                    <input type="checkbox" id="QuestionTypeSecondCar3" value="70003" name="chkQuestionType" /><em
                                        onclick="emChkIsChoose(this)">删除</em></span> </li>
            <li id="li_QuestionTypePUseCar" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" value="90001" id="txtQuestionTypePUseCar1" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">信贷</em></span> <span>
                            <input type="checkbox" value="90002" id="txtQuestionTypePUseCar2" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">保险</em></span> <span>
                                    <input type="checkbox" value="90003" id="txtQuestionTypePUseCar3" name="chkQuestionType" /><em
                                        onclick="emChkIsChoose(this)">养护维修</em></span> <span>
                                            <input type="checkbox" value="90004" id="txtQuestionTypePUseCar4" name="chkQuestionType" /><em
                                                onclick="emChkIsChoose(this)">自驾游</em></span> <span>
                                                    <input type="checkbox" value="90005" id="txtQuestionTypePUseCar5" name="chkQuestionType" /><em
                                                        onclick="emChkIsChoose(this)">其他</em></span>
            </li>
            <li id="li_QuestionTypeDCoop1" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" value="100001" id="QuestionTypeDCoop1" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">新车</em></span> <span>
                            <input type="checkbox" value="100002" id="QuestionTypeDCoop2" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">二手车</em></span> <span>
                                    <input type="checkbox" value="100003" id="QuestionTypeDCoop3" name="chkQuestionType" /><em
                                        onclick="emChkIsChoose(this)">汽车用品周边</em></span><span>
                                            <input type="checkbox" value="100006" id="QuestionTypeDCoop6" name="chkQuestionType" /><em
                                                onclick="emChkIsChoose(this)">DSA</em></span></li>
            <li id="li_QuestionTypeDCoop2" class="name_QuestionType" style="display: none">
                <label>
                    问题类别：</label>
                <span>
                    <input type="checkbox" value="100004" id="QuestionTypeDCoop4" name="chkQuestionType" /><em
                        onclick="emChkIsChoose(this)">咨询</em></span> <span>
                            <input type="checkbox" value="100005" id="QuestionTypeDCoop5" name="chkQuestionType" /><em
                                onclick="emChkIsChoose(this)">投诉</em></span> </li>
            <li class="btnsearch">
                <input style="float: right;" name="" id="btnsearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (DataExportButton)
          {%>
        <input name="" type="button" value="导出任务信息" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
    <input type="hidden" id="hidFieldsTask" value="" />
    <input type="hidden" id="hidIsOkOrCancel" value="0" />
    <form id="formExport" action="TastExcelExport.aspx">
    <input type="hidden" name='field' value="" />
    <input type="hidden" name='TaskID' value="" />
    <input type="hidden" name='CustName' value="" />
    <input type="hidden" name='BeginTime' value="" />
    <input type="hidden" name='EndTime' value="" />
    <input type="hidden" name='ConsultID' value="" />
    <input type="hidden" name='QuestionType' value="" />
    <input type="hidden" name='QuestionQuality' value="" />
    <input type="hidden" name='IsComplaint' value="" />
    <input type="hidden" name='IsForwarding' value="" />
    <input type="hidden" name='ProcessStatus' value="" />
    <input type="hidden" name='Status' value="" />
    </form>
</asp:Content>
