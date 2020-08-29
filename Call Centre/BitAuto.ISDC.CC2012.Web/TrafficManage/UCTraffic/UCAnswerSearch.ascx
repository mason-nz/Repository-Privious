<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCAnswerSearch.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.UCTraffic.UCAnswerSearch" %>
<script type="text/javascript">
    function search() {
        var msg = judgeIsSuccess();
        if (msg != "") {
            $.jAlert(msg, function () {
                return false;
            });
        }
        else if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $("#ajaxTable").load("/AjaxServers/TrafficManage/AnswerList.aspx", podyStr, function () {
                $("td[name='Col_TaskID'] a[href='']").text("");
                StatAjaxPageTime(monitorPageTime, "/AjaxServers/TrafficManage/AnswerList.aspx?" + podyStr);
            });
        }
    }

    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation("ajaxTable");
        $("#ajaxTable").load("/AjaxServers/TrafficManage/AnswerList.aspx", pody, function () {

            $("td[name='Col_TaskID'] a[href='']").text("");

        });
    }
    //获取参数
    function _params() {
        var name = "";
        var ANI = "";
        if ($("#liANI").is(":visible")) {
            ANI = encodeURIComponent($.trim($("#txtANI").val())); //主叫号码
        }
        var agent = encodeURIComponent($.trim($("#txtAgent").val()));
        var taskID = encodeURIComponent($.trim($("#txtTaskID").val()));
        var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
        var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));
        var agentNum = encodeURIComponent($.trim($("#txtAgentNum").val()));
        var phoneNum = encodeURIComponent($.trim($("#txtPhoneNum").val())); //被叫号码

        var taskCategory = ""; //任务分类

        var spanTime1 = encodeURIComponent($.trim($("#txtSpanTime1").val()));
        var spanTime2 = encodeURIComponent($.trim($("#txtSpanTime2").val()));
        var agentGroup = encodeURIComponent($.trim($("#<%=selAgentGroup.ClientID %>").val()));

        var callStatus = encodeURIComponent($.trim($("#hidCallStatus").val()));
        var selCategory = encodeURIComponent($.trim($("#selCategory").val()));

        var ivrScore = encodeURIComponent($.trim($("#selScore").val()));
        var selSolve = encodeURIComponent($.trim($("#selSolve").val()));

        var selIncomingSource = "";
        var selBusinessType = $.trim($('#selBusinessType').val());

        var callID = "";

        var pody = {
            CallID: callID,               //录音ID
            IncomingSource: selIncomingSource, //呼入来源
            IVRScore: ivrScore,          //IVR满意度
            selSolve: selSolve,  //问题解决
            Name: name,                 //客户姓名
            ANI: ANI,                   //主叫号码
            Agent: agent,               //坐席
            TaskID: taskID,             //任务ID
            BeginTime: beginTime,       //来电日期（前一个）
            EndTime: endTime,           //来电日期（后一个）
            AgentNum: agentNum,         //工号
            PhoneNum: phoneNum,         //被叫号码
            TaskCategory: taskCategory, //任务编号
            SpanTime1: spanTime1,       //通话时长（前一个）
            SpanTime2: spanTime2,       //通话时长（后一个）
            AgentGroup: agentGroup,     //坐席组
            CallStatus: callStatus,     //电话状态（1-呼入；2-呼出；默认1）
            selCategory: selCategory,
            selBusinessType: selBusinessType, //业务线
            r: Math.random()            //随机数
        }

        return pody;
    }

    //验证数据格式
    function judgeIsSuccess() {
        var msg = "";

        var beginTime = $.trim($("#tfBeginTime").val());
        var endTime = $.trim($("#tfEndTime").val());
        var agentNum = $.trim(encodeURIComponent($("#txtAgentNum").val()));
        var spanTime1 = $.trim($("#txtSpanTime1").val());
        var spanTime2 = $.trim($("#txtSpanTime2").val());

        if (beginTime != "") {
            if (!beginTime.isDate()) {
                msg += "来电日期格式不正确<br/>";
                $("#tfBeginTime").val('');
            }
        }
        if (endTime != "") {
            if (!endTime.isDate()) {
                msg += "来电日期格式不正确<br/>";
                $("#tfEndTime").val('');
            }
        }

        if (beginTime != "" && endTime != "") {
            if (endTime < beginTime) {
                msg += "来电日期中后面日期不能大于前面日期<br/>";
                $("#tfBeginTime").val('');
                $("#tfEndTime").val('');
            }
        }

        if (!isNum(spanTime1) || !isNum(spanTime2)) {
            msg += "通话时长格式不正确<br/>";
            $("#txtSpanTime1").val('');
            $("#txtSpanTime2").val('');
        }

        return msg;
    }

    //加载分组
    function getUserGroup() {
        AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
            $("#<%=selAgentGroup.ClientID %>").append("<option value='-2'>请选所属分组</option>");
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                for (var i = 0; i < jsonData.length; i++) {
                    $("#<%=selAgentGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }
            }
        });
    }

    $(document).ready(function () {
        $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
        $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

        SelectListInit();
        getUserGroup();

    });

    function SelectListInit() {
        var str = TelNumManag.GetOptions();
        $("#selBusinessType").append(str);
    }
</script>
<input type="hidden" id="hidCallStatus" value="1" />
<div class="search clearfix">
    <ul>
        <li id="liANI">
            <label>
                主叫号码：</label>
            <input type="text" id="txtANI" class="w190" />
        </li>
        <li>
            <label>
                被叫号码：</label>
            <input type="text" id="txtPhoneNum" class="w190" />
        </li>
        <li>
            <label>
                通话时长：</label>
            <input type="text" id="txtSpanTime1" class="w85" style="width: 85px;" />
            至
            <input type="text" id="txtSpanTime2" class="w85" style="width: 84px;" />
        </li>
    </ul>
    <ul class="clear">
        <li>
            <label>
                任&nbsp;务&nbsp;ID：</label>
            <input type="text" id="txtTaskID" class="w190" />
        </li>
        <li>
            <label>
                业务线：</label>
            <select id="selBusinessType" class="w190" style="width: 194px;">
                <option value="-1">请选择</option>
            </select>
        </li>
        <li>
            <label>
                来电日期：</label>
            <input type="text" name="BeginTime" value='<%=DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd")%>'
                id="tfBeginTime" class="w85" style="width: 85px;" />
            至
            <input type="text" name="EndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                id="tfEndTime" class="w85" style="width: 84px;" />
        </li>
    </ul>
    <ul class="clear">
        <li>
            <label>
                坐席：</label>
            <input type="text" id="txtAgent" class="w190" />
        </li>
        <li>
            <label>
                工号：</label>
            <input type="text" id="txtAgentNum" class="w190" />
        </li>
        <li>
            <label>
                所属分组：</label>
            <select id="selAgentGroup" class="w125" style="width: 196px;" runat="server">
            </select>
        </li>
    </ul>
    <ul class="clear">
        <li>
            <label>
                问题解决：</label>
            <select id="selSolve" class="w125" style="width: 194px;">
                <option value="-1">请选择</option>
                <option value="0">未评价</option>
                <option value="1">已解决</option>
                <option value="2">未解决</option>
            </select>
        </li>
        <li>
            <label>
                满意度：</label>
            <select id="selScore" class="w125" style="width: 194px;">
                <option value="-1">请选择</option>
                <option value="0">未评价</option>
                <option value="1">满意</option>
                <option value="2">对处理结果不满意</option>
                <option value="3">对客服代表服务不满意</option>
            </select>
        </li>
        <li class="btnsearch" style="float: right; *margin-right: 80px;">
            <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
        </li>
    </ul>
</div>
