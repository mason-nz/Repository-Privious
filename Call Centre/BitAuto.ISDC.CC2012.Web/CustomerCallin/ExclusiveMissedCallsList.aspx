<%@ Page Title="专属客服未接来电" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ExclusiveMissedCallsList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustomerCallin.ExclusiveMissedCallsList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        loadJS("common");
    </script>
    <script type="text/javascript">
        //load方法
        $(document).ready(function () {
            getUserGroup();
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
            //敲回车键执行方法
            enterSearch(search);
            search();
        });

        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "来电日期的起始日期格式不正确<br/>";
                    $("#tfBeginTime").val('');
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "来电日期的终止日期格式不正确<br/>";
                    $("#tfEndTime").val('');
                }
            }
            if (beginTime != "" && endTime != "") {
                if (endTime < beginTime) {
                    msg += "来电日期的终止日期不能大于起始日期<br/>";
                    $("#tfBeginTime").val('');
                    $("#tfEndTime").val('');
                }
            }

            return msg;
        }

        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#selGroup").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //选择操作人
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: $("#selGroup").val() },
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                    ;
                }
            });
        }

        //选择访问人
        //        function SelectVisitPerson() {
        //            $.openPopupLayer({
        //                name: "AssignTaskAjaxPopupForSelect",
        //                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
        //                beforeClose: function (e) {
        //                    $("#txtZuoxi").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
        //                    $("#txtSearchTrueNameID").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));

        //                    $("#txtAgentNum").val("");
        //                },
        //                afterClose: function () {
        //                    //敲回车键执行方法
        //                    enterSearch(search);
        //                }
        //            });
        //        }

        //获取参数
        function _params(refresh) {
            //主叫号码
            var ANI = encodeURIComponent($.trim($("#txtANI").val()));
            //来电日期
            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            //工号
            var AgentNum = encodeURIComponent($.trim($("#txtAgentNum").val()));

            //客服
            var AgentID = encodeURIComponent($.trim($("#txtSearchTrueNameID").val()));


            //是否专属客服
            var AgentGroup = encodeURIComponent($.trim($("#selGroup").val())); ;

            var pody = {
                ANI: ANI, //主叫号码
                BeginTime: beginTime, //来电日期（前一个）
                EndTime: endTime, //来电日期（后一个）
                AgentID: AgentID,
                AgentNum: AgentNum,
                AgentGroup: AgentGroup, //组
                r: Math.random()//随机数
            }
            if (refresh == "refresh") {
                pody.page = $("#input_page").val();
            }
            return pody;
        }
        //查询
        function search(refresh) {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            //获取查询条件
            var pody = _params(refresh);
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CustomerCallin/ExclusiveMissedCallsList.aspx", podyStr, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CustomerCallin/ExclusiveMissedCallsList.aspx?r=" + Math.random(), pody, null);
        }
        //导出
        function Export() {
            var pody = _params();
            $("#formExport [name='ANI']").val(pody.ANI);
            $("#formExport [name='BeginTime']").val(pody.BeginTime);
            $("#formExport [name='EndTime']").val(pody.EndTime);
            $("#formExport [name='AgentID']").val(pody.AgentID);
            $("#formExport [name='AgentNum']").val(pody.AgentNum);
            $("#formExport [name='AgentGroup']").val(pody.AgentGroup);
            $("#formExport [name='r']").val(pody.r);
            $("#formExport").submit();
        }          
    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客服：</label>
                <input type="text" id="txtZuoxi" class="w190" onclick="SelectVisitPerson('employee','txtZuoxi','txtSearchTrueNameID')"
                    readonly="readonly" />
                <input type="hidden" id="txtSearchTrueNameID" />
            </li>
            <li>
                <label>
                    工号：
                </label>
                <input type="text" id="txtAgentNum" class="w190" />
            </li>
            <li>
                <label>
                    所属分组：
                </label>
                <select id="selGroup" class="w190">
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li id="liANI">
                <label>
                    主叫号码：</label>
                <input type="text" id="txtANI" class="w190" />
            </li>
            <li>
                <label>
                    来电日期：</label>
                <input type="text" name="BeginTime" value='<%=DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") %>'
                    id="tfBeginTime" class="w85" style="width: 85px;" />
                至
                <input type="text" name="EndTime" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>'
                    id="tfEndTime" class="w85" style="width: 84px;" />
            </li>
            <li class="btnsearch" style="width: 80px">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
                <input type="button" value="刷新" onclick="javascript:search('refresh')" id="btnsearch"
                    style="display: none" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (ExportButton)
          { %><input name="" type="button" value="导出" onclick="Export()" class="newBtn" /><%} %>
    </div>
    <div class="bit_table" id="ajaxTable">
    </div>
    <form id="formExport" action="/AjaxServers/CustomerCallin/ExclusiveMissedCallsListExport.aspx"
    method="post">
    <input type="hidden" id="hidden_ANI" name="ANI" value="" />
    <input type="hidden" id="hidden_BeginTime" name="BeginTime" value="" />
    <input type="hidden" id="hidden_EndTime" name="EndTime" value="" />
    <input type="hidden" id="hidden_AgentID" name="AgentID" value="" />
    <input type="hidden" id="hidden_AgentNum" name="AgentNum" value="" />
    <input type="hidden" id="hidden_AgentGroup" name="AgentGroup" value="" />
    <input type="hidden" id="hidden_r" name="r" value="" />
    </form>
</asp:Content>
