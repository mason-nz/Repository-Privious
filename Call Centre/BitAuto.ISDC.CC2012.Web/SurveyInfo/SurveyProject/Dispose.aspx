<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispose.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.Dispose" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>调查项目管理</title>
    <link href="../../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../../js/GooCalendar.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        //所属业务组改变时，重新加载分类
        function UserGroupChanged() {
            $("#sltSurveyCategory").find("option").remove();
            var bgId = $("#sltUserGroup").val();
            $.post("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: bgId, IsFilterStop: "1" }, function (data) {
                if (data) {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, item) {
                        var selectStr = "";
                        if ("<%=surveyCategoryStr %>" == item.SCID) {
                            selectStr = "selected=selected";
                        }
                        $("#sltSurveyCategory").append("<option value='" + item.SCID + "' " + selectStr + ">" + item.Name + "</option>");
                    });
                }
            });
        }
        //保存调查项目信息
        function SubmitSurveyProject() {
            var spiId = '<%=SPIID %>';
            var projectName = $.trim($("#txtProjectName").val());
            var bgId = $.trim($("#sltUserGroup").val());
            var scId = $.trim($("#sltSurveyCategory").val());
            var description = $.trim($("#txtDescription").val());
            var businessGroup = $.trim($("#txtBusinessGroup").val());
            var siid = $.trim($("#hdnSIID").val());
            var personIDS = $.trim($("#hdnPersonIDS").val());
            var begintime = $.trim($("#txtBeginTime").val());
            var endtime = $.trim($("#txtEndTime").val());
            var msg = "";
            if (Len(projectName) == 0) {
                msg += "项目名称不能为空！</br>";
            }
            if (!isNum(bgId) || bgId < 0) {
                msg += "请选择所属组！</br>";
            }
            if (!isNum(scId) || scId < 0) {
                msg += "请选择分类！</br>";
            }
            if (Len(description) == 0) {
                msg += "请填写调查说明！</br>";
            }
            if (!isNum(siid) || siid < 0) {
                msg += "请选择问卷！</br>";
            }
            if (Len(personIDS) == 0) {
                msg += "请选择调查对象！</br>";
            }
            if (Len(begintime) == 0) {
                msg += "调查开始时间不能为空！</br>";
            }
            if (Len(endtime) == 0) {
                msg += "调查结束时间不能为空！</br>";
            }

            var pody = {
                Action: "SubmitSurveyProject",
                SPIID: escapeStr(spiId),
                ProjectName: escapeStr(projectName),
                BGIF: escapeStr(bgId),
                SCID: escapeStr(scId),
                Description: escapeStr(description),
                BusinessGroup: escapeStr(businessGroup),
                SIID: escapeStr(siid),
                PersonIDS: escapeStr(personIDS),
                Begintime: escapeStr(begintime),
                Endtime: escapeStr(endtime)
            }
            if (Len(msg) == 0) {
                $.post("/AjaxServers/SurveyInfo/SurveyProject/Handler.ashx", pody, function (data) {
                    if (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.Result == "success") {
//                            $.jAlert("保存成功！", function () {
//                                closePage();
//                            });
                            $.jPopMsgLayer("保存成功！", function () {
                                closePage();
                            });
                        }
                        else {
                            $.jAlert(unescape(jsonData.ErrorMsg));
                        }
                    }
                });
            }
            else {
                $.jAlert(msg);
            }
        }
        //选择参考人员
        function openSelectUserPopup() {
            var userIDS = $("#hdnPersonIDS").val();
            $.openPopupLayer({
                name: "SelectEmployeePopup",
                parameters: { UserIDs: userIDS },
                url: "/ExamOnline/ExamObject/GetEmployeeList.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var names = data.split(";")[0];
                        var userIDS = data.split(";")[1];
                        $("#txtExamPersonNames").val(names);
                        $("#hdnPersonIDS").val(userIDS);
                    }
                }
            }
         );
        }

        //选择试卷
        function openSelectSurveyInfoPopup() {
            $.openPopupLayer({
                name: "SelectSurveyInfo",
                parameters: {},
                url: "SelectSurveyInfo.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var siid = $('#popupLayer_' + 'SelectSurveyInfo').data('SIID');
                        var name = $('#popupLayer_' + 'SelectSurveyInfo').data('Name');
                        $("#hdnSIID").val(siid);
                        $("#txtSurveyName").val(name);
                    }
                }
            }
         );
        }

        $(document).ready(function () {
            UserGroupChanged();
            //InitWdatePicker(3, ["txtBeginTime", "txtEndTime"]);
            $.createGooCalendar("txtBeginTime", property2);
            $.createGooCalendar("txtEndTime", property2);
            $("#txtBeginTime").val('<% =BeginDateTime %>');
            $("#txtEndTime").val('<% =EndDateTime %>');
        });
    </script>
    <script type="text/javascript">
        var property2 = {
            divId: "txtBeginTime", //日历控件最外层DIV的ID
            needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd hh:mm:ss"
            /*设定日期的输出格式,可不填*/
        };
        var property = {
            divId: "demo",
            needTime: true,
            fixid: "fff"
            /*决定了日历的显示方式，默认不填时为点击INPUT后出现，但如果填了此项，日历控件将始终显示在一个id为其值（如"fff"）的DIV中（且此DIV必须存在）*/
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            添加调查项目</div>
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <span class="redColor">*</span>调查名称：</label>
                    <span>
                        <input type="text" value="" id="txtProjectName" class="w260" style="*width: 258px;"
                            runat="server" /><input type="hidden" id="hdnSPIID" runat="server" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>所属分组：</label>
                    <select id="sltUserGroup" runat="server" style="width: 264px; *width: 262px;" onchange="UserGroupChanged()">
                    </select>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>分类：</label>
                    <select id="sltSurveyCategory" runat="server" style="width: 264px; *width: 262px;">
                    </select>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>调查说明：</label>
                    <span>
                        <textarea name="" id="txtDescription" runat="server"></textarea></span>
                </li>
                <li>
                    <label>
                        调查范围：</label>
                    <span>
                        <input type="text" value="" id="txtBusinessGroup" runat="server" style="*width: 258px;"
                            class="w260" />
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>调查问卷：</label>
                    <span>
                        <input type="text" id="txtSurveyName" disabled="disabled" runat="server" class="w550"
                            style="width: 568px;" /></span>&nbsp;
                    <input type="hidden" id="hdnSIID" class="w550" runat="server" />
                    <span class="btnOption">
                        <input name="" type="button" value="选择" onclick="openSelectSurveyInfoPopup()" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>调查对象：</label>
                    <span>
                        <textarea name="" id="txtExamPersonNames" runat="server" disabled="disabled"></textarea></span>&nbsp;
                    <input type="hidden" id="hdnPersonIDS" runat="server" />
                    <span class="btnOption" style="display: inline-block; vertical-align: top;">
                        <input name="" type="button" value="选择" onclick="javascript:openSelectUserPopup();" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>调查时间：</label>
                    <input type="text" id="txtBeginTime" class="w90" style="width: 122px; *width: 121px;" />&nbsp;-<span>
                        <input type="text" id="txtEndTime" class="w90" style="width: 122px; *width: 121px;" /></span>
                </li>
            </ul>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="保 存" onclick="SubmitSurveyProject()" />&nbsp;&nbsp;
            <input type="button" value="取 消" onclick='closePage()' />
        </div>
    </div>
    </form>
</body>
</html>
