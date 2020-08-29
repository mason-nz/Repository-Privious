<%@ Page Title="短信统计" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true" CodeBehind="SMSStatistics.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.SMSStatistics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //绑定分组
            getUserGroup();
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
            //给时间赋值
            BindBeginEndtime();
            //敲回车键执行方法
            enterSearch(search);
            search();
        });

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //选择客服
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: $("#<%=selGroup.ClientID %>").val() },
                url: "../../AjaxServers/ReturnVisit/CustAssignUserList.aspx",
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

        //绑定当天时间
        function BindBeginEndtime() {
            var today = new Date();
            var strYear = today.getFullYear();
            var strDay = today.getDate();
            var strMonth = today.getMonth() + 1;
            if (strMonth < 10) {
                strMonth = "0" + strMonth;
            }
            if (strDay < 10) {
                strDay = "0" + strDay;
            }
            var strlastMonday = strYear + "-" + strMonth + "-" + strDay;
            $("#tfBeginTime").val(strlastMonday);
            $("#tfEndTime").val(strlastMonday);
        }



        function Export() {
            var pody = _params();

            $("#formExport [name='BeginTime']").val(pody.BeginTime);
            $("#formExport [name='EndTime']").val(pody.EndTime);
            $("#formExport [name='AgentGroup']").val(pody.AgentGroup);
            $("#formExport [name='Browser']").val(GetBrowserName());
            $("#formExport [name='AgentUserID']").val(pody.AgentUserID);
            $("#formExport [name='AgentNum']").val(pody.AgentNum);
            $("#formExport").submit();
        }

        function search() {

            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }

            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/TrafficManage/SMSStatisticsList.aspx", podyStr, function () {
            });

        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/TrafficManage/SMSStatisticsList.aspx", pody, function () {
            });
        }

        //获取参数
        function _params() {
            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));
            var agentGroup = encodeURIComponent($.trim($("#<%=selGroup.ClientID %>").val()));
            //客服
            var agent = $("#hidSelOperId").val();
            //工号
            var AgentNum = $.trim($("#txtAgentNum").val());
            var pody = {
                BeginTime: beginTime,       //统计日期（前一个）
                EndTime: endTime,           //统计日期（后一个）            
                AgentGroup: agentGroup,     //坐席组
                AgentNum: AgentNum,         //工号
                AgentUserID: agent,         //客服
                r: Math.random()            //随机数
            }
            return pody;
        }

        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";

            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "统计日期格式不正确<br/>";
                    $("#tfBeginTime").val('');
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "统计日期格式不正确<br/>";
                    $("#tfEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (endTime < beginTime) {
                    msg += "统计日期中后面日期不能大于前面日期<br/>";
                    $("#tfBeginTime").val('');
                    $("#tfEndTime").val('');
                }
            }

            return msg;
        }
    </script>
    <form id="form1" runat="server">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" runat="server" class="w125" style="width: 198px;*width:196px;width: 195px\9;">
                </select>
            </li>
            <li>
                <label>
                    客服：</label>
                <input type="text" id="txtSelOper" class="w125" style="width: 191px" readonly="true"
                    onclick="SelectVisitPerson('','txtSelOper','hidSelOperId')" />
                <input type="hidden" id="hidSelOperId" value="-2" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtAgentNum" class="w125" style="width: 190px" />
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    统计时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li class="btnsearch">
                <input style="float: left;" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (IsExport)
          { %>
        <input name="" type="button" value="导出" onclick="Export()" class="newBtn" />
        <%} %>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="/TrafficManage/SMSStatisticsExport.aspx" method="post">
    <input type="hidden" id="BeginTime" name="BeginTime" value="" />
    <input type="hidden" id="EndTime" name="EndTime" value="" />
    <input type="hidden" id="AgentGroup" name="AgentGroup" value="" />
    <input type="hidden" id="AgentNum" name="AgentNum" value="" />
    <input type="hidden" id="AgentUserID" name="AgentUserID" value="" />
    <input type="hidden" id="Browser" name="Browser" value="" />
    </form>
</asp:Content>
