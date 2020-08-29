<%@ Page Title="话务总表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="CallRecord.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.CallRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("controlParams");        
    </script>
    <script type="text/javascript">
        function CheckedOut() {
            if ($("#chkCallOut")[0].checked) {
                $('#label_OutTypes').css('display', 'inline-block');
                $('#span_OutTypes1').css('display', 'inline-block');
                $('#span_OutTypes2').css('display', 'inline-block');
                $('#span_OutTypes3').css('display', 'inline-block');
            }
            else {
                $('#label_OutTypes').css('display', 'none');
                $('#span_OutTypes1').css('display', 'none');
                $('#span_OutTypes2').css('display', 'none');
                $('#span_OutTypes3').css('display', 'none');

                $("#cbOutTypes1")[0].checked = false;
                $("#cbOutTypes2")[0].checked = false;
                $("#cbOutTypes3")[0].checked = false;
            };
        }
    </script>
    <form id="form1" method="post">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    话&nbsp;务&nbsp;ID：</label>
                <input type="text" id="txtCallID" class="w190" name="CallID" vtype="isNum|Len" vmsg="CallID格式不正确|CallID长度应该小于18位"
                    lenstr="18" />
            </li>
            <li>
                <label>
                    任&nbsp;务&nbsp;ID：
                </label>
                <input type="text" id="txtTaskID" name="TaskID" class="w190" />
            </li>
            <li>
                <label>
                    通话日期：
                </label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 120px;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 120px;" />
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    主叫号码：
                </label>
                <input type="text" id="txtPhoneNum" class="w190" name="PhoneNum" vtype="isNum" vmsg="主叫号码格式不正确" />
            </li>
            <li>
                <label>
                    被叫号码：
                </label>
                <input type="text" id="txtANI" class="w190" name="ANI" vtype="isNum" vmsg="被叫号码格式不正确" />
            </li>
            <li>
                <label>
                    通话时长：
                </label>
                <input type="text" id="txtSpanTime1" class="w85" style="width: 120px;" name="SpanTime1"
                    vtype="isNum" vmsg="通话最小时长格式不正确" />
                至
                <input type="text" id="txtSpanTime2" class="w85" style="width: 120px;" name="SpanTime2"
                    vtype="isNum" vmsg="通话最大时长格式不正确" />
            </li>
            <%--<li>
                <label>
                    挂断类型：
                </label>
                <span>
                    <input type="checkbox" value="1" id="chkCustomerRelease" name="CallRelease" /><em
                        onclick="emChkIsChoose(this);">客户</em></span> <span>
                            <input type="checkbox" value="2" id="chkAgentRelease" name="CallRelease" /><em onclick="emChkIsChoose(this);">坐席</em>
                        </span></li>--%>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    工&nbsp;号&nbsp;：
                </label>
                <input type="text" id="txtAgentNum" name="AgentNum" class="w190" />
            </li>
            <li style="WIDTH: 300px; MARGIN-RIGHT: 20px">
                <label>
                    话务类型：
                </label>
                <span>
                    <input type="checkbox" value="1" id="chkCallIn" name="CallTypes" /><em onclick="emChkIsChoose(this);">呼入</em>
                </span>
                <span>
                    <input type="checkbox" value="2" id="chkCallOut" name="CallTypes" onclick="CheckedOut();" /><em onclick="emChkIsChoose(this);CheckedOut();">呼出</em> 
                </span>
                <span>
                    <input type="checkbox" value="3" id="Checkbox1" name="CallTypes" /><em onclick="emChkIsChoose(this);">转接</em>
                </span>
                <span>
                    <input type="checkbox" value="4" id="Checkbox2" name="CallTypes" /><em onclick="emChkIsChoose(this);">接管</em>
                </span>
           </li>
            <li style="width: 285px; margin-right: 0px;">
                <%-- 1 页面 2 客户端 3转接 4自动--%>
                <label id="label_OutTypes" style="display: none">
                    呼叫类别：
                </label>
                <span id="span_OutTypes1" style="display: none">
                    <input type="checkbox" value="1" id="cbOutTypes1" name="OutTypes" /><em onclick="emChkIsChoose(this);">页面</em></span>
                <span id="span_OutTypes2" style="display: none">
                    <input type="checkbox" value="2" id="cbOutTypes2" name="OutTypes" /><em onclick="emChkIsChoose(this);">客户端</em></span>
                <span id="span_OutTypes3" style="display: none">
                    <input type="checkbox" value="4" id="cbOutTypes3" name="OutTypes" /><em onclick="emChkIsChoose(this);">自动</em></span>
            </li>
            <%-- <li>
                <label>
                    话务总时长：</label>
                <input type="text" id="txtTallTime1" class="w85" style="width: 84px; *width: 83px;
                    width: 83px\9;" name="TallTime1" vtype="isNum" vmsg="最小话务总时长格式不正确" />
                至
                <input type="text" id="txtTallTime2" class="w85" style="width: 84px; *width: 83px;
                    width: 83px\9;" name="TallTime2" vtype="isNum" vmsg="最小话务总时长格式不正确" />
            </li>--%>
            <li class="btnsearch" style="margin-left: 0px; margin-right: 0px; float: right">
                <input style="float: left;" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_export)
          { %>
        <input name="" type="button" value="导出" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%} %>
    </div>
    <input type="hidden" name="BusinessGroup" value="<%=RequestGroup%>" />
    <div id="ajaxTable">
    </div>
    <script type="text/javascript">

        //导出
        function ExportExcel() {
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
                var jsonStr = $("#form1").find("[name!='__VIEWSTATE'][name!='__EVENTVALIDATION']").fixedSerialize();
                form1.action = "CallRecordExport.aspx?JsonStr=" + encodeURIComponent(jsonStr) + "&tfBeginTime=" + $.trim($("#tfBeginTime").val()) + "&tfEndTime="+$.trim($("#tfEndTime").val());
                $("#form1").submit();
            }
        }

        function search() {
            var msg = "";
            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
//            if (beginTime != "") {
//                if (!beginTime.isDate()) {
//                    msg += "通话日期格式不正确<br/>";
//                    $("#tfBeginTime").val('');
//                }
//            }
//            if (endTime != "") {
//                if (!endTime.isDate()) {
//                    msg += "通话日期格式不正确<br/>";
//                    $("#tfEndTime").val('');
//                }
//            }

//            if (beginTime != "" && endTime != "") {
//                if (endTime < beginTime) {
//                    msg += "通话日期中后面日期不能大于前面日期<br/>";
//                    $("#tfBeginTime").val('');
//                    $("#tfEndTime").val('');
//                }
//            }

//            if (msg != "") {
//                $.jAlert(msg, function () {
//                    return false;
//                });
//            }
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
              
                showSearchList.getList("/TrafficManage/AjaxList.aspx?tfBeginTime=" + encodeURIComponent($("#tfBeginTime").val())+"&tfEndTime="+ encodeURIComponent($("#tfEndTime").val()), "form1", "ajaxTable");
            }
        }

        //默认通话日期三个月内的
        function defaultTime() {
            $("#tfBeginTime").val("<%=beginTime %>");
            $("#tfEndTime").val("<%=endTime %>");
        }

        $(function () {
            defaultTime();
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
            //敲回车键执行方法
            enterSearch(search);

            //search();
        });

    </script>
    </form>
</asp:Content>
