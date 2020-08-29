<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatesDetails.aspx.cs"
    Title="状态明细" MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.CallReport.StatesDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .pageP
        {
            width: 200px;
            float: left;
            text-align: left;
            padding-left: 20px;
        }
        
        .pageP a.selectA
        {
            color: Red;
        }
        .pageP a
        {
            height: 50px;
        }
        .pageP a:hover
        {
            font-size: 16px;
        }
    </style>
    <script type="text/javascript">

        $(function () {
            var todayTime = '<%=DtToday %>';
            $("#txtTime").val(todayTime);
            //敲回车键执行方法
            enterSearch(search);
            //            $('#txtTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", maxDate: '#F{$dp.$D(\'txtTime\')}' }); });
            $('#txtTime').bind('click focus', function () { WdatePicker({}); });

            $('#selState').change(function () {
                if ($(this).val() == "4") {
                    $("#selAuxState").val("-1");
                    $("#auxS").show();
                } else {
                    $("#auxS").hide();
                }
            });
        });

        function search() {
            var pagesize = $("#hidSelectPageSize").val();
            var url = '../AjaxServers/CallReport/StatesDetails.aspx?pagesize=' + pagesize;

            var userId = $('#txtVisitPerson').data('val');
            userId = userId == null ? "" : userId;
            var agentNum = $('#txtNum').val();
            var timeT = $('#txtTime').val();
            var state = $('#selState').val();
            var auxState = (state != "4") ? "" : $('#selAuxState').val();

            if (!userId && !agentNum) {
                $.jAlert("客服或工号必须选择至少选择一个！");
                return;
            }
            if (!isNum(agentNum)) {
                $.jAlert("工号必须为数字！");
                return;
            }
            if (timeT == "") {
                $.jAlert("统计时间不能为空！");
                return;
            }
            if (timeT && (!timeT.isDate())) {
                $.jAlert("统计时间格式不正确！");
                return;
            }
            url += '&u=' + escape(userId);
            url += '&n=' + escape(agentNum);
            url += '&t=' + escape(timeT);
            url += '&s=' + escape(state);
            url += '&as=' + escape(auxState);
            url += '&r=' + Math.random();
            LoadingAnimation("divContent");
            $('#divContent').load(url + "&" + Math.random());
        }

        //选择访问人
        function SelectVisitPerson() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $('#txtVisitPerson').data('val', $('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                    $('#txtVisitPerson').val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                }
            });
        }



        function Export() {
            var url = '../AjaxServers/CallReport/StatesDetailsExport.aspx?r' + Math.random();
            var userId = $('#txtVisitPerson').data('val');
            userId = userId == null ? "" : userId;
            var agentNum = $('#txtNum').val();
            var timeT = $('#txtTime').val();
            var state = $('#selState').val();
            var auxState = $('#selAuxState').val();

            if (!userId && !agentNum) {
                $.jAlert("客服或工号必须选择至少选择一个！");
                return;
            }
            if (!isNum(agentNum)) {
                $.jAlert("工号必须为数字！");
                return;
            }
            if (timeT == "") {
                $.jAlert("统计时间不能为空！");
                return;
            }
            if (timeT && (!timeT.isDate())) {
                $.jAlert("统计时间格式不正确！");
                return;
            }
            url += '&u=' + escape(userId);
            url += '&n=' + escape(agentNum);
            url += '&t=' + escape(timeT);
            url += '&s=' + escape(state);
            url += '&as=' + escape(auxState);
            url += '&r=' + Math.random();


            window.location = url;
        }

    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客服：</label>
                <input type="text" maxlength="50" onclick="SelectVisitPerson()" class="w200" id="txtVisitPerson"
                    readonly="readonly" />
                <input type="hidden" id="hidId" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtNum" class="w200" /></li>
            <li>
                <label>
                    统计时间<span style="color: Red">*</span>：</label>
                <input type="text" id="txtTime" class="w200" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客服状态：</label>
                <select id="selState" class="w200" style="width: 206px;">
                    <option value="-1">请选择</option>
                    <option value="9">工作中</option>
                    <option value="5">话后</option>
                    <option value="4">置忙</option>
                    <option value="3">置闲</option>
                    <option value="8">振铃</option>
                </select>
            </li>
            <li id="auxS" style="display: none">
                <label>
                    辅助状态：</label>
                <select id="selAuxState" style="width: 128px;">
                    <option value="-1" selected="selected">请选择</option>
                    <option value="1">小休</option>
                    <option value="2">任务回访</option>
                    <option value="3">业务处理</option>
                    <option value="4">会议</option>
                    <option value="5">培训</option>
                    <option value="6">离席</option>
                </select>
            </li>
            <li class="btnsearch">
                <input name="" type="button" value="查 询" onclick="javascript:search();" />
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
    <div id="divContent">
        <div class="bit_table" id="divList">
            <table cellspacing="0" cellpadding="0" border="0" width="99%" id="tableAgentDetail"
                class="tableList">
                <tbody>
                    <tr class="color_hui" style="background-color: transparent;">
                        <th width="10%">
                            <strong>日期</strong>
                        </th>
                        <th width="8%">
                            <strong>所属分组</strong>
                        </th>
                        <th width="8%">
                            <strong>客服</strong>
                        </th>
                        <th width="8%">
                            <strong>工号</strong>
                        </th>
                        <th width="8%">
                            <strong>状态</strong>
                        </th>
                        <th width="8%">
                            <strong>辅助状态</strong>
                        </th>
                        <th width="12%">
                            <strong>状态开始时间</strong>
                        </th>
                        <th width="12%">
                            <strong>状态结束时间</strong>
                        </th>
                        <th width="10%">
                            <strong>持续时长</strong>
                        </th>
                    </tr>
                </tbody>
            </table>
            <div style="margin-top: 10px;" class="pageTurn mr10">
                <p style="padding: 0pt 10px; float: left; display: none;" class="pageP">
                    每页显示条数 <a v="20" name="apageSize" href="#" class="selectA">20</a>&nbsp;&nbsp; <a
                        v="50" name="apageSize" href="#">50</a>&nbsp;&nbsp; <a v="100" name="apageSize" href="#">
                            100</a>
                </p>
                <p>
                    共 0 项 首页&nbsp;上一页&nbsp;下一页&nbsp;尾页
                </p>
            </div>
            <input type="hidden" id="nowDt" name="nowDt">
        </div>
    </div>
    <input type="hidden" id="hidSelectPageSize" value="20" />
</asp:Content>
