<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCallSearch.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.UCTraffic.UCCallSearch" %>
<script type="text/javascript">
    function search() {
        //---
        var msg = judgeIsSuccess();
        if (msg != "") {
            $.jAlert(msg, function () {
                return false;
            });
        }
        else if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
            var pody = _params_callsearch();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");

            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $("#ajaxTable").load("/AjaxServers/TrafficManage/List.aspx", podyStr, function () {
                $("td[name='Col_TaskID'] a[href='']").text("");
                StatAjaxPageTime(monitorPageTime, "/AjaxServers/TrafficManage/List.aspx?" + podyStr);
            });
        }
    }

    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation("ajaxTable");
        $("#ajaxTable").load("/AjaxServers/TrafficManage/List.aspx", pody, function () {

            $("td[name='Col_TaskID'] a[href='']").text("");
        });
    }

    //获取参数
    function _params_callsearch() {
        var name = encodeURIComponent($.trim($("#txtName").val()));
        var ANI = "";

        var agent = encodeURIComponent($.trim($("#txtAgent").val()));
        var taskID = encodeURIComponent($.trim($("#txtTaskID").val()));
        var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
        var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));
        var agentNum = encodeURIComponent($.trim($("#txtAgentNum").val()));
        var phoneNum = encodeURIComponent($.trim($("#txtPhoneNum").val())); //被叫号码

        var taskCategory = ""; //任务分类
        if ($("#liTaskCategory").is(":visible")) {
            taskCategory = encodeURIComponent($.trim($("#selTaskCategory").val()));
        }
        var spanTime1 = encodeURIComponent($.trim($("#txtSpanTime1").val()));
        var spanTime2 = encodeURIComponent($.trim($("#txtSpanTime2").val()));
        var agentGroup = encodeURIComponent($.trim($("#<%=selAgentGroup.ClientID %>").val()));

        var callStatus = encodeURIComponent($.trim($("#hidCallStatus").val()));
        var selCategory = encodeURIComponent($.trim($("#selCategory").val()));

        var OutTypes = "";
        $("input[name='OutTypes']:checked").each(function (i, n) {
            OutTypes += n.value + ",";
        });
        if (OutTypes.length > 0) {
            OutTypes = OutTypes.substr(0, OutTypes.length - 1);
        }


        var projectId = encodeURIComponent($.trim($("#hidProjectId").val()));
        var isSuccess = "-1";
        if (projectId != "") {
            isSuccess = $.trim($("#SelIsSuccess").val());
        }
        var failReason = "-1";
        if (isSuccess != "-1") {
            failReason = encodeURIComponent($.trim($("#SelFailReason").val()));
        }
        isSuccess = encodeURIComponent(isSuccess);
        
        var pody = {
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
            OutTypes: OutTypes, //呼叫类别
            ProjectId:projectId,
            IsSuccess:isSuccess,
            FailReason:failReason, 
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

    //当业务组选择：易卡售后服务组、198热线组、个人用户服务组 后，任务分类显示，其余业务组不显示
    function agentGroupCategory() {

        $("#selCategory").children().remove();
        $("#selCategory").append("<option value='-1'>请选择分类</option>");
        if ($("[id$='selAgentGroup']").val() != "-1") {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("[id$='selAgentGroup']").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }


    //加载所属分组
    function getUserGroup() {
        AjaxPost("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
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
        getUserGroup();
    });
</script>
<input type="hidden" id="hidCallStatus" value="1" />
<div class="search clearfix">
    <ul>
        <li>
            <label>
                客户姓名：</label>
            <input type="text" id="txtName" class="w190" />
        </li>
        <li>
            <label>
                坐席：</label>
            <input type="text" id="txtAgent" class="w190" /></li>
        <li>
            <label>
                被叫号码：</label>
            <input type="text" id="txtPhoneNum" class="w190" />
        </li>
    </ul>
    <ul class="clear">
        <li>
            <label>
                任&nbsp;务&nbsp;ID：</label>
            <input type="text" id="txtTaskID" class="w190" /></li>
        <li>
            <label>
                工号：</label>
            <input type="text" id="txtAgentNum" class="w190" />
        </li>
        <li>
            <label>
                去电日期：</label>
            <input type="text" name="BeginTime" value='<%=DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") %>'
                id="tfBeginTime" class="w85" style="width: 85px;" />
            至
            <input type="text" name="EndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                id="tfEndTime" class="w85" style="width: 84px;" />
        </li>
    </ul>
    <ul class="clear">
        <li>
            <label>
                所属分组：</label>
            <select id="selAgentGroup" class="w190" runat="server" onchange="agentGroupCategory()"
                style="width: 194px;">
              
            </select>
        </li>
        <li>
            <label>
                分类：</label>
            <select id="selCategory" class="w190" style="width: 194px;">
                <option value="-1">请选择分类</option>
            </select>
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
                项目名称：</label>
            <div class="coupon-box02" style="float: left; width: 188px;">
                <input type="text" id="txtProjectName" class="text02" name="txtProjectName" readonly="readonly"
                    style="border: 0px; border-image: none; width: 145px; padding-top: 0px;
                    margin-top: -2px; margin-left: -2px; font-size:12px;" />
                <b onclick="OpenProjectInfoPop()"><a href="javascript:void(0)">选择</a></b>
                <input type="hidden" id="hidProjectId" name="hidProjectId" />
            </div>
            <script type="text/javascript">
                //项目弹出层
                function OpenProjectInfoPop() {
                    $.openPopupLayer({
                        name: "PopProjectSingleSelect",
                        url: "/ProjectManage/PopPanel/SelectSingleProjectPop.aspx",
                        beforeClose: function (e) {
                            if (e) {
                                //将回传的数据赋值给页面
                                $("#txtProjectName").val($('#popupLayer_' + 'PopProjectSingleSelect').data('projectname'));
                                var projectid = $('#popupLayer_' + 'PopProjectSingleSelect').data('projectid')
                                $("#hidProjectId").val(projectid); 
                                if (projectid != "") {
                                    $("#li_SelIsSuccess").css("display", "block").find("option:[index='0']").attr("selected", true);
                                    $("#li_failReason").css("display", "none").find("option:[index='0']").attr("selected", true);
                                    //加载失败原因   
                                    AjaxPostAsync("/TrafficManage/UCTraffic/TrafficHandler.ashx", { Action: "GetProjectNotSuccessReason", ProjectId: projectid, r: Math.random() }, null, function (data) {
                                        $("#SelFailReason").html("<option value='-1'>请选择</option>");
                                        var jsonData = $.evalJSON(data);
                                        if (jsonData != "") {
                                            if (jsonData.error != "") {
                                                $.jAlert(jsonData.error);
                                            }
                                            else {
                                                for (var i = 0; i < jsonData.reasons.length; i++) {
                                                    $("#SelFailReason").append("<option value=" + jsonData.reasons[i].value + ">" + jsonData.reasons[i].name + "</option>");
                                                }
                                            }
                                        }
                                    });
                                }
                                else {
                                    $("#li_SelIsSuccess").css("display", "none");
                                    $("#li_failReason").css("display", "none");
                                }
                            } else {
                                if ($("#hidProjectId").val() == "") {
                                    $("#li_SelIsSuccess").css("display", "none")
                                    $("#li_failReason").css("display", "none");
                                };
                            }
                            enterSearch(search);
                        }
                    });
                }

                function ChangeReasonLiStatus(obj) {
                    if ($(obj).val() == "0") {
                        $("#li_failReason").css("display", "block");
                    }
                    else {
                        $("#li_failReason").css("display", "none");
                    }
                } 
            </script>
        </li>
        <li id="li_SelIsSuccess" style=" display:none;">
            <label>
                是否成功：</label>
            <select onchange="ChangeReasonLiStatus(this);" id="SelIsSuccess" class="w190" style="width: 194px;">
                <option value="-1" selected="selected">请选择</option>
                <option value="1">是</option>
                <option value="0">否</option>
            </select>
        </li>
        <li id="li_failReason" style="display: none">
            <label>
                失败原因：</label>
            <select id="SelFailReason" class="w190" style="width: 194px;">
                <option value="-1">请选择</option>
            </select>
        </li>
    </ul>
    <ul class="clear">
        <li>
            <%-- 1 页面 2 客户端 3转接 4自动--%>
            <label>
                呼叫类别：
            </label>
            <span>
                <input type="checkbox" value="1" id="cbOutTypes1" name="OutTypes" /><em onclick="emChkIsChoose(this);">页面</em></span>
            <span>
                <input type="checkbox" value="2" id="cbOutTypes2" name="OutTypes" /><em onclick="emChkIsChoose(this);">客户端</em></span>
            <span>
                <input type="checkbox" value="4" id="cbOutTypes3" name="OutTypes" /><em onclick="emChkIsChoose(this);">自动</em></span>
        </li>
        <li class="btnsearch" style="float: right; *margin-right: 80px;">
            <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
        </li>
    </ul>
</div>
