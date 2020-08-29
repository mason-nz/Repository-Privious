<%@ Page Title="呼入报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="InComingCallDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CallReport.InComingCallDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .a_red
        {
            color: #C00;
        }
        .a_gray
        {
            color: #666;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '%y-%M-%d' }); });

            SelectListInit();
           var  initDate = getToday();
           $('#txtStartTime').val(initDate);
           $('#txtEndTime').val(initDate);
            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        function SelectListInit() {
            var str = TelNumManag.GetOptions();
            $("#selBusinessType").append(str);
        }
        function getToday() {
            return '<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>';
        }


        function getYesterToday() {
            return '<%=System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")%>';
        }
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {
                var beginTime = $.trim($("#txtStartTime").val());
                var endTime = $.trim($("#txtEndTime").val());

                if (beginTime != endTime && endTime >= getToday()) {

                    $.jAlert("历史数据截止日期必须为当前日期的前一日!");
                    $("#txtEndTime").val(getYesterToday());
                }
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxTable");
                $('#ajaxTable').load("/AjaxServers/CallReport/InComingCallDetails.aspx", podyStr, function () {
                    pody.Action = "InComingCallTotal";
                    //查询合计数据                        
                    AjaxPost('/AjaxServers/CallReport/InComingCallDetails.ashx', pody, null, success);
                    function success(data) {

                        var mbi = $.evalJSON(data);
                        $("#hidSumData").val(data);
                        $("#t_N_CallIsQuantity").text(mbi.N_CallIsQuantity);
                        $("#t_T_RingingTime").text(mbi.T_RingingTime);
                        $("#t_T_TalkTime").text(mbi.T_TalkTime);
                        $("#t_T_AfterworkTime").text(mbi.T_AfterworkTime);
                        $("#t_T_SetLogin").text(mbi.T_SetLogin);
                        $("#t_P_WorkTimeUse").text(mbi.P_WorkTimeUse);
                        $("#t_A_AverageRingTime").text(mbi.A_AverageRingTime);
                        $("#t_A_AverageTalkTime").text(mbi.A_AverageTalkTime);
                        $("#t_A_AfterworkTime").text(mbi.A_AfterworkTime);
                        $("#t_T_SetBuzy").text(mbi.T_SetBuzy);
                        $("#t_N_SetBuzy").text(mbi.N_SetBuzy);
                        $("#t_A_AverageSetBusy").text(mbi.A_AverageSetBusy);
                        $("#t_N_TransferOut").text(mbi.N_TransferOut);
                        $("#t_N_TransferIn").text(mbi.N_TransferIn);

                        AfterSearch();
                    }
                })
            }
        }

        //查询完成之后
        function AfterSearch() {
            //都不可用
            $("#showDay").removeClass("a_red");
            $("#showWeek").removeClass("a_red");
            $("#showMonth").removeClass("a_red");

            var ShowTime = $.trim($("#hidSearchType").val());
            if (ShowTime == "3") {
                $("#showDay").addClass("a_red");
            }
            else if (ShowTime == "2") {
                $("#showWeek").addClass("a_red");
            }
            else if (ShowTime == "1") {
                $("#showMonth").addClass("a_red");
            }

        }

        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";

            var agentNum = $.trim($('#txtSearchAgentNum').val());
            if (agentNum != "") {
                if (isNaN(agentNum)) {
                    msg += "工号应该为数字！<br/>";
                    $("#txtSearchAgentNum").val('');
                }
            }

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "统计日期格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }
            else {
                msg += "统计日期必须有开始日期<br/>";
                $("#txtStartTime").val('');
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "统计日期格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }
            else {
                msg += "统计日期必须有结束日期<br/>";
                $("#txtEndTime").val('');
            }

            if (beginTime != "" && endTime != "") {
                if (endTime < beginTime) {
                    msg += "统计日期中结束日期不能大于开始日期<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }
            if ($("#selBusinessType option").length <= 0) {
                msg += "请选择业务类型<br/>";
            }
            return msg;
        }

        //获取参数
        function _params() {
            var txtSearchTrueNameID = $.trim($('#txtSearchTrueNameID').val());
            var txtSearchAgentNum = $.trim($('#txtSearchAgentNum').val());
            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var selBusinessType = $.trim($('#selBusinessType').val());

            var pody = {
                StartTime: txtStartTime,       //统计日期（前一个）  暂时没有escape(）  HttpContext.Current.Server.UrlDecode(）
                EndTime: txtEndTime,           //统计日期（后一个）            
                AgentNum: txtSearchAgentNum,             //工号
                AgentID: txtSearchTrueNameID,         //AgentID(用户ID)
                QueryType: $("#hidSearchType").val(),
                BusinessType: selBusinessType,
                QueryArea: "3",
                Action: "", //查询合计数据
                r: Math.random()            //随机数
            }

            return pody;
        }



        //选择客服
        function SelectVisitPerson() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='txtSearchTrueName']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#txtSearchTrueNameID").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    // enterSearch(search);
                }
            });
        }

        //根据不同的点击绑定不同的信息页面
        function loadHtml(n, othis) {
            $('#hidSearchType').val(n);
            $(othis).addClass("redColor").siblings().removeClass("redColor");
            search();
        }

        //导出数据
        function Export() {

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime == "" || endTime == "") {
                $.jAlert("统计日期不能为空!");
                return;
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != endTime && endTime >= getToday()) {

                $.jAlert("历史数据截止日期必须为当前日期的前一日!");
                $("#txtEndTime").val(getYesterToday());
            }
            var pody = _params();
            $("#formExport [name='ep_StartTime']").val(pody.StartTime);
            $("#formExport [name='ep_EndTime']").val(pody.EndTime);
            $("#formExport [name='ep_AgentNum']").val(pody.AgentNum);
            $("#formExport [name='ep_AgentID']").val(pody.AgentID);
            $("#formExport [name='ep_QueryType']").val(pody.QueryType);
            $("#formExport [name='ep_BusinessType']").val(pody.BusinessType);
            $("#formExport [name='ep_QueryArea']").val(pody.QueryArea);
            $("#formExport").submit();

        }


        //分页操作
        function ShowDataByPost1(pody) {

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CallReport/InComingCallDetails.aspx", pody, function () {

                var mbi = $.evalJSON($("#hidSumData").val());
                $("#t_N_CallIsQuantity").text(mbi.N_CallIsQuantity);
                $("#t_T_RingingTime").text(mbi.T_RingingTime);
                $("#t_T_TalkTime").text(mbi.T_TalkTime);
                $("#t_T_AfterworkTime").text(mbi.T_AfterworkTime);
                $("#t_T_SetLogin").text(mbi.T_SetLogin);
                $("#t_P_WorkTimeUse").text(mbi.P_WorkTimeUse);
                $("#t_A_AverageRingTime").text(mbi.A_AverageRingTime);
                $("#t_A_AverageTalkTime").text(mbi.A_AverageTalkTime);
                $("#t_A_AfterworkTime").text(mbi.A_AfterworkTime);
                $("#t_T_SetBuzy").text(mbi.T_SetBuzy);
                $("#t_N_SetBuzy").text(mbi.N_SetBuzy);
                $("#t_A_AverageSetBusy").text(mbi.A_AverageSetBusy);
                $("#t_N_TransferIn").text(mbi.N_TransferIn);
                $("#t_N_TransferOut").text(mbi.N_TransferOut);

                AfterSearch();
            });
        }

    </script>
    <form id="form1" runat="server">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客服：</label>
                <input type="text" name="txtSearchTrueName" class="w200" maxlength="20" onclick="SelectVisitPerson()"
                    readonly="readonly" id="txtSearchTrueName" />
                <input type="hidden" id="txtSearchTrueNameID" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" name="agentNum" id="txtSearchAgentNum" class="w200" />
            </li>
            <li>
                <label>
                    统计日期：</label>
                <input type="text" class="w120" name="txtStartTime" id="txtStartTime" />-<input type="text"
                    class="w120" name="txtEndTime" id="txtEndTime" />
            </li>
            <li>
                <label>
                    业务类型：</label>
                <select id="selBusinessType" class="w200" style="width: 206px">
                </select>
            </li>
            <li class="btnsearch">
                <input name="" style="float: right;" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix">
        <div>
            <a href="javascript:void(0)" onclick="javascript:loadHtml(3,this)" id="showDay">日</a>&nbsp;&nbsp;||&nbsp;&nbsp;
            <a href="javascript:void(0)" onclick="javascript:loadHtml(2,this)" id="showWeek">周</a>&nbsp;&nbsp;||&nbsp;&nbsp;
            <a href="javascript:void(0)" onclick="javascript:loadHtml(1,this)" id="showMonth">月</a>
            <%if (IsExport)
              {%>
            <input type="button" id="btnPutOut" value="导出" class="newBtn" style="*margin-top: -30px;"
                onclick="Export()" />
            <%} %>
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
    <!--列表结束-->
    <input type="hidden" id="hidSearchType" value="3" />
    <input type="hidden" id="hidSumData" />
    </form>
    <form id="formExport" action="/AjaxServers/CallReport/InComingCallDetailsExport.aspx"
    method="post">
    <input type="hidden" id="ep_StartTime" name="ep_StartTime" value="" />
    <input type="hidden" id="ep_EndTime" name="ep_EndTime" value="" />
    <input type="hidden" id="ep_AgentNum" name="ep_AgentNum" value="" />
    <input type="hidden" id="ep_AgentID" name="ep_AgentID" value="" />
    <input type="hidden" id="ep_QueryType" name="ep_QueryType" value="" />
    <input type="hidden" id="ep_BusinessType" name="ep_BusinessType" value="" />
    <input type="hidden" id="ep_QueryArea" name="ep_QueryArea" value="" />
    </form>
</asp:Content>
