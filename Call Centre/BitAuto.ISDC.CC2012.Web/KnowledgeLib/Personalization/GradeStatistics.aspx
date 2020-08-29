<%@ Page Title="部门成绩明细" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="GradeStatistics.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.GradeStatistics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '%y-%M-%d' }); });

            $('#txtStartTime').val("<%=startTime%>");
            $('#txtEndTime').val("<%=endTime%>");
            $("#showhotLine").addClass("redColor");
            search();
        });
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
            
               
                if ($('#SearchType').val()=="hotLine") {
                    var beginTime = $.trim($("#txtStartTime").val());
                    var endTime = $.trim($("#txtEndTime").val());
                    if (beginTime != endTime && endTime == getToday()) {
                        $.jAlert("热线历史数据截止日期必须为当前日期的前一日!");   
                        $("#txtEndTime").val(getYesterToday());
                    }
                }              
              
                    var pody = _params();
                    var podyStr = JsonObjToParStr(pody);

                    LoadingAnimation("ajaxTable");
                    $('#ajaxTable').load("/AjaxServers/KnowledgeLib/GradeStatisticsDetails.aspx", podyStr);

               
            }

        }


        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime == "" || endTime=="") {
                msg += "查询日期不能为空<br/>";
            }
            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "查询日期格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "查询日期格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }
            if (beginTime != "" && endTime != "") {
                if (endTime < beginTime) {
                    msg += "结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var selBusinessType = $.trim($('#<%=ddlBussiGroup.ClientID %>').val())
            if (selBusinessType == '-1') {
                selBusinessType = "";
                $('#<%=ddlBussiGroup.ClientID %> option:gt(0)').each(function () {
                    selBusinessType += "," + $(this).val();
                });
                if (selBusinessType.length > 0) {
                    selBusinessType = selBusinessType.substr(1, selBusinessType.length - 1);
                }
            }

            var txtStartTime = $.trim($('#txtStartTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var searchType = $.trim($('#SearchType').val());
            var pody = {
                BusinessType: selBusinessType,
                StartTime: txtStartTime,       //统计日期（前一个）
                EndTime: txtEndTime,           //统计日期（后一个）            
                Action: "",
                searchType: searchType,
                r: Math.random()            //随机数
            }

            return pody;
        }
        //        //分页操作
        //        function ShowDataByPost1(pody) {
        //            LoadingAnimation("ajaxTable");
        //            $("#ajaxTable").load("/AjaxServers/CallReport/InComingCallDetails.aspx", pody);
        //        }
        //导出数据
        function Export() {
           var msg = judgeIsSuccess();        
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {

                if ($('#SearchType').val() == "hotLine") {
                    var beginTime = $.trim($("#txtStartTime").val());
                    var endTime = $.trim($("#txtEndTime").val());
                    if (beginTime != endTime && endTime == getToday()) {
                        $.jAlert("热线历史数据截止日期必须为当前日期的前一日!");
                        $("#txtEndTime").val(getYesterToday());
                    }
                }                   
                    var pody = _params();
                    $("#formExport [name='ep_BusinessType']").val(pody.BusinessType);
                    $("#formExport [name='ep_StartTime']").val(pody.StartTime);
                    $("#formExport [name='ep_EndTime']").val(pody.EndTime);
                    $("#formExport").submit();
             
            }
        }

        //根据不同的点击绑定不同的信息页面
        function loadHtml(searchType, othis) {
            $('#SearchType').val(searchType);
            if (searchType=="hotLine") {
                $("#timeTypeTxt").text("通话时间：");
            }
            else {
                $("#timeTypeTxt").text("对话时间：");
            }
            $(othis).addClass("redColor").siblings().removeClass("redColor");
            search();
        }
    </script>
    <form>
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    所属分组：</label>
                <select id="ddlBussiGroup" runat="server" class="w200" style="width: 205px;">
                </select>
            </li>
            <li style="width: 325px;">
                <label id="timeTypeTxt">
                    通话时间：</label>
                <input type="text" class="w95" name="txtStartTime" id="txtStartTime" />
                <span>-</span>
                <input type="text" class="w95" name="txtEndTime" id="txtEndTime" />
            </li>
            <li class="btnsearch" style="clear: none; width: 290px;">
                <input name="" type="button" class="cx" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix" style="margin-top: 10px;">
        <div>
            <a href="javascript:void(0)" onclick="javascript:loadHtml('hotLine',this)" id="showhotLine">
                热线统计</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="javascript:loadHtml('onLine',this)"
                    id="showonLine">在线统计</a>
            <%if (IsExport)
              {%>
            <input type="button" id="btnPutOut" value="导出" class="newBtn" onclick="Export()" />
            <%} %>
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="/AjaxServers/KnowledgeLib/GradeStatisticsExport.aspx"
    method="post">
    <input type="hidden" id="ep_StartTime" name="ep_StartTime" value="" />
    <input type="hidden" id="ep_EndTime" name="ep_EndTime" value="" />
    <input type="hidden" id="ep_BusinessType" name="ep_BusinessType" value="" />
    <input type="hidden" id="SearchType" name="SearchType" value="hotLine" />
    </form>
</asp:Content>
