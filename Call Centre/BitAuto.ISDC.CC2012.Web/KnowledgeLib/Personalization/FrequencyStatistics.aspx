<%@ Page Title="抽查频次统计 " Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="FrequencyStatistics.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.FrequencyStatistics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#txtScoreBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtScoreEndTime\')}', onpicked: function () { document.getElementById("txtScoreEndTime").focus(); } }); });
            $('#txtScoreEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtScoreBeginTime\')}' }); });

            $('#txtScoreBeginTime').val("<%=startTime%>");
            $('#txtScoreEndTime').val("<%=endTime%>");

            getCreater();
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
                $('#ajaxTable').load("/AjaxServers/KnowledgeLib/FrequencyStatisticsDetails.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var beginTime = $.trim($("#txtScoreBeginTime").val());
            var endTime = $.trim($("#txtScoreEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "统计时间格式不正确<br/>";
                    $("#txtScoreBeginTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "统计时间格式不正确<br/>";
                    $("#txtScoreEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "统计时间中结束时间不能大于开始时间<br/>";
                    $("#txtScoreBeginTime").val('');
                    $("#txtScoreEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var selScoreCreater = $.trim($('#selScoreCreater').val());
            var txtScoreBeginTime = $.trim($('#txtScoreBeginTime').val());
            var txtScoreEndTime = $.trim($('#txtScoreEndTime').val());

            if (selScoreCreater == '-1') {
                selScoreCreater = "";
                $('#selScoreCreater option:gt(0)').each(function () {
                    selScoreCreater += "," + $(this).val();
                });
                if (selScoreCreater.length > 0) {
                    selScoreCreater = selScoreCreater.substr(1, selScoreCreater.length - 1);
                }
            }

            var pody = {
                ScoreCreater: selScoreCreater,
                ScoreBeginTime: txtScoreBeginTime,
                ScoreEndTime: txtScoreEndTime,
                r: Math.random()            //随机数
            }

            return pody;
        }
        //导出数据
        function Export() {
            var pody = _params();
            $("#formExport [name='ep_ScoreCreater']").val(pody.ScoreCreater);
            $("#formExport [name='ep_ScoreBeginTime']").val(pody.ScoreBeginTime);
            $("#formExport [name='ep_ScoreEndTime']").val(pody.ScoreEndTime);
            $("#formExport").submit();
        }
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    评分人：</label>
                <select id="selScoreCreater" class="w200" style="width: 205px;">
                </select>
            </li>
            <li style="width: 325px;">
                <label>
                    评分时间：</label>
                <input type="text" class="w95" name="txtScoreBeginTime" id="txtScoreBeginTime" />
                <span>-</span>
                <input type="text" class="w95" name="txtScoreEndTime" id="txtScoreEndTime" />
            </li>
            <li class="btnsearch" style="clear: none; width: 290px;">
                <input name="" type="button" value="查 询" class="cx" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix" style="margin-top: 10px;">
        <div>
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
    <form id="formExport" action="/AjaxServers/KnowledgeLib/FrequencyStatisticsExport.aspx"
    method="post">
    <input type="hidden" id="ep_ScoreCreater" name="ep_ScoreCreater" value="" />
    <input type="hidden" id="ep_ScoreBeginTime" name="ep_ScoreBeginTime" value="" />
    <input type="hidden" id="ep_ScoreEndTime" name="ep_ScoreEndTime" value="" />
    </form>
</asp:Content>
