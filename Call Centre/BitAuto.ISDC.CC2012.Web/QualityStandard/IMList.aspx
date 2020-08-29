<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IMList.aspx.cs" MasterPageFile="~/Controls/Top.Master"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.IMList" Title="对话质检" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='../Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("controlParams");
        loadJS("common");

        $(document).ready(function () {
            //时间配置
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: true, startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: true, startDate: '%y-%M-%d 23:59:59', dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
            $('#txtScoreBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtScoreEndTime\')}', onpicked: function () { document.getElementById("txtScoreEndTime").focus(); } }); });
            $('#txtScoreEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtScoreBeginTime\')}' }); });
            $('#txtAppealBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtAppealEndTime\')}', onpicked: function () { document.getElementById("txtAppealEndTime").focus(); } }); });
            $('#txtAppealEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtAppealBeginTime\')}' }); });
            //对话时间-默认值
            BindBeginEndtime();
            //绑定评分表
            getScoreTableName();
            //绑定评分人
            getCreater();
            //绑定分组
            getUserGroup();

            //界面赋值 (从统计页面传递过来：暂无)
            //loadParams();

            //联动分类
            selGroupChange();
            //联动申诉状态
            QSResultStatusChanged();
            //联动满意度
            QSResultScoreChanged();
            //浏览器定义
            $("#hidBrowser").val(GetBrowserName());

            //回车查询
            enterSearch(search);
            //search();
        });

        //绑定前三月时间
        function BindBeginEndtime() {
            $("#tfBeginTime").val('<%=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") %>');
            $("#tfEndTime").val('<%=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59") %>');
        }
        //评分表
        function getScoreTableName() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getFieldName", TableName: "QS_RulesTable", IDField: "QS_RTID", ShowField: "Name", TableStatus: "10003", r: Math.random() }, null, function (data) {
                $("#selScoreTable").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selScoreTable").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //评分人
        function getCreater() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getCreater", GetCreaterType: "QS_IM", TableName: "QS_IM_Result", ShowField: "CreateUserID", TableStatus: "", r: Math.random() }, null, function (data) {
                $("#selScoreCreater").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selScoreCreater").append("<option value=" + jsonData[i].UserID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selGroup").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //根据选择的分组绑定对应的业务线
        function selGroupChange() {
            //暂时不做
            return;
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择业务线</option>");
            if ($("#selGroup").val() != "-1") {
                AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetBusinessLineByBGID", BGID: $("#selGroup").val(), r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].RecID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }
        //评分状态联动申诉状态
        function QSResultStatusChanged() {
            var QSResultStatus = $.trim($("#QSResultStatus").val());
            if (QSResultStatus == 20006) {
                if ($('#QSResultStatus').parent().parent().find('li:visible').size() < 3) {
                    if ($('#QSResultStatus').parent().parent().find("li div[id='li_QSStateResult']").size() == 0) {
                        $('#QSResultStatus').parent().parent().find('li:last').after('<li>');
                        $('#QSResultStatus').parent().parent().find('li:last').append($("#li_QSStateResult"));
                    }
                }
                $("#li_QSStateResult").css("display", "block");
            }
            else {
                $("#li_QSStateResult").css("display", "none");
            }
        }
        //评价联动满意度
        function QSResultScoreChanged() {
            var QSResultScore = $.trim($("#QSResultScore").val());
            if (QSResultScore == "0") {
                $("#li_PerQSResultScore").css("display", "block");
                $("#li_ProQSResultScore").css("display", "block");
            }
            else {
                $("#li_PerQSResultScore").css("display", "none");
                $("#li_ProQSResultScore").css("display", "none");
            }
        }
        //坐席弹出层
        function GetEmployeeAgent() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    var name = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("name");
                    var userID = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid");
                    if (name == undefined) {
                        name = "";
                        userID = "";
                    }
                    $("#txtAgent").val(name);
                    $("#hdnUserID").val(userID);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        //查询
        function search() {
            showSearchList.getList("/AjaxServers/QualityStandard/IMList.aspx", "form1", "ajaxTable");
        }

        //评分
        function clickScore(tableid, resultid, csid,scoretype) {
            //如果为0，则表示还没有评分表
            if (tableid == 0) {
                $.jAlert("该对话还没有对应的评分表，无法评分！请添加评分表", function () { return false });
                return false;
            }
            else {
                try {
                    if ($.trim(scoretype) == "3") {
                        var key = "/QualityStandard/IMQualityResultManage/QualityResultFiveLevelEdit.aspx";
                        var url = '/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>' +
                                  key + '?QS_RTID=' + tableid + '&QS_RID=' + resultid + '&CSID=' + csid);
                        window.external.MethodScript(url);
                    }
                    else {
                        var key = "/QualityStandard/IMQualityResultManage/QualityResultEdit.aspx";
                        var url = '/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>' +
                                  key + '?QS_RTID=' + tableid + '&QS_RID=' + resultid + '&CSID=' + csid);
                        window.external.MethodScript(url);
                    }
                }
                catch (e) {
                    window.open(key + "?QS_RTID=" + tableid + "&QS_RID=" + resultid + "&CSID=" + csid);
                }
            }
        }
        //导出
        function ExportExcel() {
            if ($("#selScoreTable").val() == "-1") {
                $.jAlert("请先选择评分表再进行导出！");
                return false;
            }
            $("#form1").submit();
        }
    </script>
    <form id="form1" action="Export/IMScoreDetailsExport.aspx">
    <div class="searchTj" style="width: 100%;">
        <%--第一行--%>
        <ul style="width: 98%;">
            <li>
                <label>
                    对话ID：
                </label>
                <input type="text" id="txt_csid" class="w95" name="CSID" vtype="isNum" vmsg="对话ID格式不正确"
                    style="width: 200px;" />
            </li>
            <li>
                <label>
                    对话日期：
                </label>
                <input type="text" name="BeginTime" id="tfBeginTime" vtype="isDateTime" vmsg="对话起始时间格式不正确"
                    class="w95" />-<input type="text" name="EndTime" id="tfEndTime" vtype="isDateTime"
                        vmsg="对话结束时间格式不正确" class="w95" />
            </li>
            <li style="display: none">
                <label>
                    消息次数：
                </label>
                <input type="text" name="BeginCount" id="txt_begincount" vtype="isNum" vmsg="消息次数起始值格式不正确"
                    class="w95" />-<input type="text" name="EndCount" id="txt_endcount" vtype="isNum"
                        vmsg="消息次数结束值格式不正确" class="w95" />
            </li>
            <%--<li>
                <label>
                    成&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;绩：
                </label>
                <span class="fxz">
                    <label>
                        <input type="checkbox" value="1" id="chkQualifiedYes" name="Qualified" /><em onclick="emChkIsChoose(this);">合格</em>
                    </label>
                    <label>
                        <input type="checkbox" value="-1" id="chkQualifiedNot" name="Qualified" /><em onclick="emChkIsChoose(this);">不合格</em>
                    </label>
                </span></li>--%>
                <li>
                <label>
                    评&nbsp;分&nbsp;表：
                </label>
                <select id="selScoreTable" class="w200" style='width: 206px;' name="ScoreTable">
                </select>
            </li>
                
        </ul>
        <%--第二行--%>
        <ul style="width: 98%;">
            
            
            <li>
                <label>
                    坐&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;席：
                </label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtAgent" class="text02" readonly="readonly" />
                    <b onclick="GetEmployeeAgent()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="hdnUserID" name="AgentUserID" value="" />
                </div>
            </li>
            <li>
                <label>
                    评分日期：
                </label>
                <input type="text" name="ScoreBeginTime" id="txtScoreBeginTime" vtype="isDate" vmsg="评分起始时间格式不正确"
                    class="w95" />-<input type="text" name="ScoreEndTime" id="txtScoreEndTime" vtype="isDate"
                        vmsg="评分结束时间格式不正确" class="w95" />
            </li>
            <li>
                <label>
                    评&nbsp;分&nbsp;人：</label>
                <select id="selScoreCreater" class="w200" style='width: 206px;' name="ScoreCreater">
                </select>
            </li>
            
        </ul>
        <%--第三行--%>
        <ul style="width: 98%;">
            <li>
                <label>
                    所属分组：
                </label>
                <select id="selGroup" onchange="javascript:selGroupChange()" class="w60" name="BGID"
                    style="width: 206px;">
                </select>
                <select id="selCategory" class="w60" name="BusinessLine" style="width: 101px; display: none">
                </select>
            </li>
            <li>
                <label>
                    申诉日期：
                </label>
                <input type="text" name="AppealBeginTime" id="txtAppealBeginTime" vtype="isDate"
                    vmsg="申诉起始时间格式不正确" class="w95" />-<input type="text" name="AppealEndTime" id="txtAppealEndTime"
                        class="w95" vtype="isDate" vmsg="申诉结束时间格式不正确" />
            </li>
            
            <li>
                <label>
                    满&nbsp;&nbsp;意&nbsp;度：
                </label>
                <select id="QSResultScore" class="w200" style='width: 206px;' name="QSResultScore"
                    onchange="QSResultScoreChanged();">
                    <option value="-1">请选择</option>
                    <option value="0">已评价</option>
                    <option value="1">未评价</option>
                </select>
            </li>
        </ul>
        <%--第四行--%>
        <ul style="width: 900px">
            <li id="li_PerQSResultScore">
                <label>
                    服务满意：
                </label>
                <select id="PerQSResultScore" class="w200" style='width: 206px;' name="PerQSResultScore">
                    <option value="-1">请选择</option>
                    <option value="5">非常满意</option>
                    <option value="4">满意</option>
                    <option value="3">一般</option>
                    <option value="2">不满意</option>
                    <option value="1">非常不满意</option>
                </select>
            </li>
            <li id="li_ProQSResultScore">
                <label>
                    产品满意：
                </label>
                <select id="ProQSResultScore" class="w200" style='width: 206px;' name="ProQSResultScore">
                    <option value="-1">请选择</option>
                    <option value="5">非常满意</option>
                    <option value="4">满意</option>
                    <option value="3">一般</option>
                    <option value="2">不满意</option>
                    <option value="1">非常不满意</option>
                </select>
            </li>
            
            <li>
                <label>
                    评分状态：
                </label>
                <select id="QSResultStatus" class="w200" style='width: 206px;' name="QSResultStatus"
                    onchange="QSResultStatusChanged();">
                    <option value="-1">请选择</option>
                    <option value="20001">待评分</option>
                    <option value="20002">已提交</option>
                    <option value="20003">待初审</option>
                    <option value="20004">待复审</option>
                    <option value="20005">已评分</option>
                    <option value="20006">已申诉</option>
                </select>
            </li>
            <li>
                <label>
                    消息发送量：
                </label>
                <select id="selSendCount" name="BeginCount" class="w200" style='width: 206px;'>
                    <option value="-1">请选择</option>
                    <option value="1">30及以下</option>
                    <option value="2">31-59</option>
                    <option value="3">60-99</option>
                    <option value="4">100及以上</option>
                </select>
            </li>
        </ul>
        <%--第五行--%>
        <ul style="width: 98%;">
            <li>
                <div id="li_QSStateResult">
                    <label>
                        申诉状态：
                    </label>
                    <span>
                        <label>
                            <input type="checkbox" value="1" id="chkResultSuccess" name="QSStateResult" />
                            <em onclick="emChkIsChoose(this)">成功</em>
                        </label>
                        <label>
                            <input type="checkbox" value="2" id="chkResultFail" name="QSStateResult" />
                            <em onclick="emChkIsChoose(this)">失败</em>
                        </label>
                    </span>
                </div>
                </li>
                <li></li><li class="btnsearch" style="clear: none; margin-top: 5px; width: 290px;">
                <input class="cx" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        
        <input type="hidden" id="hidBrowser" name="Browser" />
    </div>
    <div class="optionBtn clearfix">
        <%if (right_Export)
          {%>
        <input name="" type="button" value="导出" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
    </form>
</asp:Content>
