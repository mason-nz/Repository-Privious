<%@ Page Title="问题解答管理" Language="C#" AutoEventWireup="true" CodeBehind="QuestionManage.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.QuestionManage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#txtCreateBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtCreateEndTime\')}', onpicked: function () { document.getElementById("txtCreateEndTime").focus(); } }); });
            $('#txtCreateEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateBeginTime\')}' }); });
            $('#txtAnswerBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtAnswerEndTime\')}', onpicked: function () { document.getElementById("txtAnswerEndTime").focus(); } }); });
            $('#txtAnswerEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtAnswerBeginTime\')}' }); });
            BindSel1Data();
            getAnsweres();
            search();
        });
        function BindSel1Data() {
            var n = 1;
            var pid = 0;
            $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid, RegionID: '<%=RegionID %>' }, function (data) {
                $("#selKCID" + n).html("");
                $("#selKCID" + n).append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }
        //解答人列表
        function getAnsweres() {
            AjaxPostAsync("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "getAnsweres", r: Math.random() }, null, function (data) {
                $("#selAnswerUser").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selAnswerUser").append("<option value=" + jsonData[i].AnswerUser + ">" + jsonData[i].TrueName + "</option>");
                    }
                }
            });
        };
        //分类change方法
        function BindSel1Change() {
            var n = 2;
            var pid = $("#selKCID1").val();
            $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid, RegionID: '<%=RegionID %>' }, function (data) {
                $("#selKCID" + n).html("");
                $("#selKCID" + n).append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxTable");
                $('#ajaxTable').load("/AjaxServers/KnowledgeLib/QuestionManageDetails.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtTitle = $.trim($("#txtTitle").val());
            var txtCreateUser = $.trim($("#txtCreateUser").val());
            if (txtTitle.length > 50) {
                msg += "标题字数太多<br/>";
                $("#txtTitle").val('');
            }
            if (txtCreateUser.length > 10) {
                msg += "提问人字数太多<br/>";
                $("#txtCreateUser").val('');
            }
            var createBeginTime = $.trim($("#txtCreateBeginTime").val());
            var createEndTime = $.trim($("#txtCreateEndTime").val());
            var answerBeginTime = $.trim($("#txtAnswerBeginTime").val());
            var answerEndTime = $.trim($("#txtAnswerEndTime").val());

            if (createBeginTime != "") {
                if (!createBeginTime.isDate()) {
                    msg += "提问时间格式不正确<br/>";
                    $("#txtCreateBeginTime").val('');
                }
            }
            if (createEndTime != "") {
                if (!createEndTime.isDate()) {
                    msg += "提问时间格式不正确<br/>";
                    $("#txtCreateEndTime").val('');
                }
            }
            if (createBeginTime != "" && createEndTime != "") {
                if (createBeginTime > createEndTime) {
                    msg += "提问时间中结束时间不能大于开始时间<br/>";
                    $("#txtCreateBeginTime").val('');
                    $("#txtCreateEndTime").val('');
                }
            }

            if (answerBeginTime != "") {
                if (!answerBeginTime.isDate()) {
                    msg += "解答时间格式不正确<br/>";
                    $("#txtAnswerBeginTime").val('');
                }
            }
            if (answerEndTime != "") {
                if (!answerEndTime.isDate()) {
                    msg += "解答时间格式不正确<br/>";
                    $("#txtAnswerEndTime").val('');
                }
            }
            if (answerBeginTime != "" && answerEndTime != "") {
                if (answerBeginTime > answerEndTime) {
                    msg += "解答时间中结束时间不能大于开始时间<br/>";
                    $("#txtAnswerBeginTime").val('');
                    $("#txtAnswerEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtTitle = $.trim($("#txtTitle").val());
            var txtCreateUser = $.trim($("#txtCreateUser").val());
            var txtCreateBeginTime = $.trim($('#txtCreateBeginTime').val());
            var txtCreateEndTime = $.trim($('#txtCreateEndTime').val());
            var selKCType = ($('#selKCID2').val() != '-1' ? $('#selKCID2').val() : $('#selKCID1').val());
            var selAnswerUser = $('#selAnswerUser').val();
            var txtAnswerBeginTime = $.trim($('#txtAnswerBeginTime').val());
            var txtAnswerEndTime = $.trim($('#txtAnswerEndTime').val());
            var txtStatus = "";
            $("input[name=Answered]").each(function () {
                if ($(this).attr("checked")) {
                    txtStatus += "," + $(this).val();
                }
            });
            if (txtStatus != "") {
                txtStatus = txtStatus.substr(1, txtStatus.length - 1);
            }

            var pody = {
                Title: txtTitle,
                CreateUserName: txtCreateUser,
                CreateBeginTime: txtCreateBeginTime,
                CreateEndTime: txtCreateEndTime,
                KCType: selKCType,
                AnswerUserId: selAnswerUser,
                AnswerBeginTime: txtAnswerBeginTime,
                AnswerEndTime: txtAnswerEndTime,
                Status: txtStatus,
                RegionID: '<%=RegionID %>',
                r: Math.random()            //随机数
            }

            return pody;
        }
        function DeleteClick(QuestionId) {
            $.jConfirm("确定删除该提问信息吗？", function (r) {
                if (r) {
                    $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "deletequestion", QuestionId: QuestionId }, function (data) {
                        if (data == "success") {

                            //$.jAlert("删除提问成功！");
                            $.jPopMsgLayer("删除提问成功！");
                            search();
                        }
                        else {
                            $.jAlert(data);
                        }
                    });
                }
            });
        };
        //分页操作 
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load("/AjaxServers/KnowledgeLib/QuestionManageDetails.aspx", pody);
        }
        function emChoose(obj) {
            if (event.target == this) {
                if ($(obj).prev().attr("checked")) {
                    $(obj).prev().attr("checked", false);
                }
                else {

                    $(obj).prev().attr("checked", true);
                }
            }
        }
    </script>
    <div class="searchTj" style="width: 1000px;">
        <ul>
            <li>
                <label>
                    标题：</label>
                <input type="text" name="txtTitle" id="txtTitle" class="w200" />
            </li>
            <li>
                <label>
                    提问人：</label>
                <input type="text" name="txtCreateUser" id="txtCreateUser" class="w200" />
            </li>
            <li style="width: 320px;">
                <label>
                    提问时间：</label>
                <input type="text" class="w95" name="txtCreateBeginTime" id="txtCreateBeginTime" />
                <span>-</span>
                <input type="text" class="w95" name="txtCreateEndTime" id="txtCreateEndTime" />
            </li>
        </ul>
        <ul style="margin-top: 0px;">
            <li>
                <label>
                    分类：</label>
                <select id="selKCID1" onchange="javascript:BindSel1Change()" style="width: 101px;"
                    class="w60">
                    <option value='-1'>请选择</option>
                </select>
                <select id="selKCID2" style="width: 101px;" class="w60">
                    <option value='-1'>请选择</option>
                </select>
            </li>
            <li>
                <label>
                    解答人：</label>
                <select id="selAnswerUser" class="w200" style="width: 205px;">
                </select>
            </li>
            <li style="width: 320px;">
                <label>
                    解答时间：</label>
                <input type="text" class="w95" name="txtCreateBeginTime" id="txtAnswerBeginTime" />
                <span>-</span>
                <input type="text" class="w95" name="txtCreateEndTime" id="txtAnswerEndTime" />
            </li>
        </ul>
        <ul style="margin-top: 0px;">
            <li>
                <label>
                    状态：</label>
                <span class="fxz">
                    <label>
                        <input type="checkbox" value="0" id="AnsweredYes" name="Answered" /><em onclick="emChoose(this);">待解答</em>
                    </label>
                    <label>
                        <input type="checkbox" value="1" id="AnsweredNot" name="Answered" /><em onclick="emChoose(this);">已解答</em>
                    </label>
                </span></li>
            <li></li>
            <li class="btnsearch" style="width: 297px; clear: none;">
                <input class="cx" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix">
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
