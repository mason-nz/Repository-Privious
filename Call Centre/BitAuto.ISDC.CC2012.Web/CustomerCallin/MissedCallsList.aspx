<%@ Page Title="未接来电" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="MissedCallsList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustomerCallin.MissedCallsList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        loadJS("common");
    </script>
    <script type="text/javascript">
        //load方法
        $(document).ready(function () {
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

            $('#prBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'prEndTime\')}', onpicked: function () { document.getElementById("prEndTime").focus(); } }); });
            $('#prEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'prBeginTime\')}' }); });

            //业务线
            SelectListInit();

            //敲回车键执行方法
            enterSearch(search);
            search();
        });
        //加载业务线
        function SelectListInit() {
            var str = TelNumManag.GetOptions();
            $("#selBusinessType").append(str);
        }
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

            var beginTime = $.trim($("#prBeginTime").val());
            var endTime = $.trim($("#prEndTime").val());
            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "处理日期的起始日期格式不正确<br/>";
                    $("#prBeginTime").val('');
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "处理日期的终止日期格式不正确<br/>";
                    $("#prEndTime").val('');
                }
            }
            if (beginTime != "" && endTime != "") {
                if (endTime < beginTime) {
                    msg += "处理日期的终止日期不能大于起始日期<br/>";
                    $("#prBeginTime").val('');
                    $("#prEndTime").val('');
                }
            }

            return msg;
        }
        //获取参数
        function _params(refresh) {
            //主叫号码
            var ANI = encodeURIComponent($.trim($("#txtANI").val()));
            //来电日期
            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));
            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));
            //业务线
            var selBusinessType = $.trim($('#selBusinessType').val());
            //处理人
            var agent = encodeURIComponent($.trim($("#txtAgent").val()));
            //处理日期
            var process_beginTime = encodeURIComponent($.trim($("#prBeginTime").val()));
            var process_endTime = encodeURIComponent($.trim($("#prEndTime").val()));
            //处理状态
            var prStatus = "";
            $("input[name='prStatus']:checked").each(function (i, n) {
                prStatus += n.value + ",";
            });
            if (prStatus.substr(prStatus.length - 1, 1) == ',') {
                prStatus = prStatus.substr(0, prStatus.length - 1);
            }
            //是否有技能组
            var HasSkill = "";
            $("input[name='HasSkill']:checked").each(function (i, n) {
                HasSkill += n.value + ",";
            });
            if (HasSkill.substr(HasSkill.length - 1, 1) == ',') {
                HasSkill = HasSkill.substr(0, HasSkill.length - 1);
            }
            //是否有留言
            var Hasaudio = "";
            $("input[name='Hasaudio']:checked").each(function (i, n) {
                Hasaudio += n.value + ",";
            });
            if (Hasaudio.substr(Hasaudio.length - 1, 1) == ',') {
                Hasaudio = Hasaudio.substr(0, Hasaudio.length - 1);
            }

            //是否专属客服
            var isExclusive = "";
            if ($("input[name='isExclusive']:checked").length == 1) {
                isExclusive = $("input[name='isExclusive']:checked").val();
            }
            var pody = {
                ANI: ANI, //主叫号码
                BeginTime: beginTime, //来电日期（前一个）
                EndTime: endTime, //来电日期（后一个）
                selBusinessType: selBusinessType, //业务线
                Agent: agent, //处理人
                PRBeginTime: process_beginTime, //处理日期（前一个）
                PREndTime: process_endTime, //处理日期（后一个）
                prStatus: prStatus, //处理状态  
                HasSkill: HasSkill, //是否有技能组
                Hasaudio: Hasaudio, //是否有留言
                isExclusive: isExclusive, //是否专属客服
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
            $("#ajaxTable").load("/AjaxServers/CustomerCallin/MissedCallsList.aspx", podyStr, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/CustomerCallin/MissedCallsList.aspx?r=" + Math.random(), pody, null);
        }
        //导出
        function Export() {
            var pody = _params();
            $("#formExport [name='ANI']").val(pody.ANI);
            $("#formExport [name='BeginTime']").val(pody.BeginTime);
            $("#formExport [name='EndTime']").val(pody.EndTime);
            $("#formExport [name='selBusinessType']").val(pody.selBusinessType);
            $("#formExport [name='Agent']").val(pody.Agent);
            $("#formExport [name='PRBeginTime']").val(pody.PRBeginTime);
            $("#formExport [name='PREndTime']").val(pody.PREndTime);
            $("#formExport [name='prStatus']").val(pody.prStatus);
            $("#formExport [name='HasSkill']").val(pody.HasSkill);
            $("#formExport [name='Hasaudio']").val(pody.Hasaudio);
            $("#formExport [name='r']").val(pody.r);
            $("#formExport").submit();
        }          
    </script>
    <div class="search clearfix">
        <ul>
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
            <li>
                <label>
                    业务线：</label>
                <select id="selBusinessType" class="w190" style="width: 194px;">
                    <option value="-1">请选择</option>
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    处理人：</label>
                <input type="text" id="txtAgent" class="w190" />
            </li>
            <li>
                <label>
                    处理日期：</label>
                <input type="text" name="BeginTime" value='' id="prBeginTime" class="w85" style="width: 85px;" />
                至
                <input type="text" name="EndTime" value='' id="prEndTime" class="w85" style="width: 84px;" />
            </li>
            <li>
                <label>
                    技能组：
                </label>
                <span>
                    <input type="checkbox" name="HasSkill" value="1" /><em onclick="emChkIsChoose(this);">有</em></span>
                <span>
                    <input type="checkbox" name="HasSkill" value="0" /><em onclick="emChkIsChoose(this);">无</em></span>
            </li>
        </ul>
        <ul class="clear">
            <li style="width: 284px;">
                <label>
                    处理状态：
                </label>
                <span>
                    <input type="checkbox" name="prStatus" value="0" /><em onclick="emChkIsChoose(this);">待处理</em></span>
                <span>
                    <input type="checkbox" name="prStatus" value="1" /><em onclick="emChkIsChoose(this);">处理中</em></span>
                <span>
                    <input type="checkbox" name="prStatus" value="2" /><em onclick="emChkIsChoose(this);">已处理</em></span>
            </li>
            <li style="width: 284px;">
                <label>
                    留言：
                </label>
                <span>
                    <input type="checkbox" name="Hasaudio" value="1" /><em onclick="emChkIsChoose(this);">有</em></span>
                <span>
                    <input type="checkbox" name="Hasaudio" value="0" /><em onclick="emChkIsChoose(this);">无</em></span>
            </li>
               <li style="width: 284px;margin-right:0px">
                <label>
                    专属客服：
                </label>
                <span>
                    <input type="checkbox" name="isExclusive" value="1" /><em onclick="emChkIsChoose(this);">是</em></span>
                <span>
                    <input type="checkbox" name="isExclusive" value="0" checked="checked" /><em onclick="emChkIsChoose(this);">否</em></span>
            </li>
            <li class="btnsearch"  style="width:80px">
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
    <form id="formExport" action="/AjaxServers/CustomerCallin/MissedCallsListExport.aspx"
    method="post">
    <input type="hidden" id="hidden_ANI" name="ANI" value="" />
    <input type="hidden" id="hidden_BeginTime" name="BeginTime" value="" />
    <input type="hidden" id="hidden_EndTime" name="EndTime" value="" />
    <input type="hidden" id="hidden_selBusinessType" name="selBusinessType" value="" />
    <input type="hidden" id="hidden_Agent" name="Agent" value="" />
    <input type="hidden" id="hidden_PRBeginTime" name="PRBeginTime" value="" />
    <input type="hidden" id="hidden_PREndTime" name="PREndTime" value="" />
    <input type="hidden" id="hidden_prStatus" name="prStatus" value="" />
    <input type="hidden" id="hidden_HasSkill" name="HasSkill" value="" />
    <input type="hidden" id="hidden_Hasaudio" name="Hasaudio" value="" />
    <input type="hidden" id="hidden_r" name="r" value="" />
    </form>
</asp:Content>
