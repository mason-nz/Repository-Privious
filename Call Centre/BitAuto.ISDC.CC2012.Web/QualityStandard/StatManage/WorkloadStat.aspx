<%@ Page Language="C#" Title="质检工作量统计 " AutoEventWireup="true" CodeBehind="WorkloadStat.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.StatManage.WorkloadStat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        var pody = "";
        function search() {
            var createUseId = $("#selScoreCreater").val();
            var beginTime = $("#txtBeginTime").val();
            var endTime = $("#txtEndTime").val();
            var recordBeginTime = $("#txtRecordBeginTime").val();
            var recordEndTime = $("#txtRecordEndTime").val();

            pody = "CreateUserID=" + createUseId + "&BeginTime=" + beginTime + "&EndTime=" + endTime + "&RecordBeginTime=" + recordBeginTime + "&RecordEndTime=" + recordEndTime;
            LoadingAnimation('ajaxTable');
            $("#ajaxTable").load("/AjaxServers/QualityStandard/StatManage/WorkloadStat.aspx",
                  { CreateUserID: createUseId, BeginTime: beginTime, EndTime: endTime, RecordBeginTime: recordBeginTime, RecordEndTime: recordEndTime }
                  , function () {
                  });
        }
        function Export() {
            var createUseId = $("#selScoreCreater").val();
            var beginTime = $("#txtBeginTime").val();
            var endTime = $("#txtEndTime").val();
            var recordBeginTime = $("#txtRecordBeginTime").val();
            var recordEndTime = $("#txtRecordEndTime").val();

            $("#formExport [name='CreateUserID']").val(createUseId);
            $("#formExport [name='BeginTime']").val(beginTime);
            $("#formExport [name='EndTime']").val(endTime);
            $("#formExport [name='RecordBeginTime']").val(recordBeginTime);
            $("#formExport [name='RecordEndTime']").val(recordEndTime);
            $("#formExport").submit();
        }
        $(function () {
            $('#txtBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtBeginTime\')}' }); });

            $('#txtRecordBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtRecordEndTime\')}', onpicked: function () { document.getElementById("txtRecordEndTime").focus(); }, dateFmt: 'yyyy-MM-dd HH:mm:ss' }); });
            $('#txtRecordEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtRecordBeginTime\')}', dateFmt: 'yyyy-MM-dd HH:mm:ss' }); });

            $("#txtBeginTime").val('<%=DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd") %>');
            $("#txtEndTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd") %>');
            getCreater();

            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        //评分人
        function getCreater() {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getCreater", GetCreaterType: "QS", TableName: "QS_Result", ShowField: "CreateUserID", TableStatus: "", r: Math.random() }, null, function (data) {
                $("#selScoreCreater").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selScoreCreater").append("<option value=" + jsonData[i].UserID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
    </script>
    <form>
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    评&nbsp;分&nbsp;人：</label>
                <select id="selScoreCreater" class="w200" style='width: 206px;' name="ScoreCreater">
                </select>
            </li>
            <li>
                <label>
                    评分时间：</label>
                <input type="text" id="txtBeginTime" class="w95" name="BeginTime" />-<input type="text"
                    id="txtEndTime" class="w95" name="EndTime" />
            </li>
            <li>
                <label>
                    通话时间：</label>
                <input type="text" id="txtRecordBeginTime" class="w95" name="RecordBeginTime" />-<input
                    type="text" id="txtRecordEndTime" class="w95" name="RecordEndTime" />
            </li>
            <li class="btnsearch" style="clear: none; width: 130px;">
                <input class="cx" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="button" value="导出" class="newBtn mr10" onclick="Export()" />
    </div>
    <div id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="ExportWorkloadStat.aspx" method="post">
    <input type="hidden" id="hidden1" name="CreateUserID" value="" />
    <input type="hidden" id="hidden3" name="BeginTime" value="" />
    <input type="hidden" id="hidden4" name="EndTime" value="" />
    <input type="hidden" id="hidden5" name="RecordBeginTime" value="" />
    <input type="hidden" id="hidden6" name="RecordEndTime" value="" />
    </form>
</asp:Content>
