<%@ Page Title="满意度统计报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="SatisfactionReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CallReport.SatisfactionReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#TBeginTime').bind('click focus', function () {
                WdatePicker({ maxDate: '#F{$dp.$D(\'TEndTime\')}', onpicked: function () {
                    document.getElementById("TEndTime").focus();
                }
                });
            });
            $('#TEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'TBeginTime\')}' }); });
            if ('<%=RegionID %>' == '1') {
                $('#liSkillGroup').css("display", "none");
            }

            $('#hdDateType').val("1");
            $("#aShowAll").addClass("redColor");
            SelectListInit();
            BussyTypeChange();

            //敲回车键执行方法
            enterSearch(search);
            $('#TBeginTime').val('<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>');
            $('#TEndTime').val('<%=System.DateTime.Now.ToString("yyyy-MM-dd")%>');
            search();
        });
        function SelectListInit() {
            var str = TelNumManag.GetOptions();
            $("select[id$='selBussyType']").append(str);
        }
        //查询
        function search() {
            var TBeginTime;
            TBeginTime = $("#TBeginTime").val();
            var TEndTime;
            TEndTime = $("#TEndTime").val();
            if (TBeginTime == "" && TEndTime != "") {
                $.jAlert("请选择统计开始时间！");
            }
            else if (TEndTime == "" && TBeginTime != "") {
                $.jAlert("请选择统计结束时间！");
            }
            else if ($.trim(TEndTime) == "" && $.trim(TBeginTime) == "") {
                $.jAlert("请选择统计时间！");
            }
            else if (CheckForSelectCallRecordORIG("TBeginTime", "TEndTime")) {
                var pody = params();
                LoadingAnimation("ajaxTable");
                var selBussyType;
                selBussyType = $("select[id$='selBussyType']").val();
                //业务类型是西安
                  if (TelNumManag.CheckTelNum(selBussyType)) {
                    $("#ajaxTable").load("../AjaxServers/CallReport/SatisfactionListForXA.aspx .bit_table > *", pody, function () {
                     $("#tableList tr:last").css("background-color","#f5f5f5");
                     $("#tableList tr:last td").css("line-height","45px");
                     $("#tableList tr:last td").css("font-weight","bold");
                    });
                }
                else if (selBussyType == "2446" || selBussyType == "2454"|| selBussyType == "2448") {
                    $("#ajaxTable").load("../AjaxServers/CallReport/SatisfactionListForBJ.aspx .bit_table > *", pody, function () {
                     $("#tableList tr:last").css("background-color","#f5f5f5");
                     $("#tableList tr:last td").css("line-height","45px");
                     $("#tableList tr:last td").css("font-weight","bold");
                    });
                }
            }
        }
        //参数
        function params() {
            var agentid;
            agentid = $("#hidSelAgentId").val();
            var agentnum;
            agentnum = $("#txtAgentNum").val();
            var TBeginTime;
            TBeginTime = $("#TBeginTime").val();
            var TEndTime;
            TEndTime = $("#TEndTime").val();
            var selBussyType;
            selBussyType = $("select[id$='selBussyType']").val();
            var selSkillGroup;
            selSkillGroup = $("select[id$='selSkillGroup']").val();
            var hdDateType = $('#hdDateType').val();
            var pody = "AgentID=" + $.trim(agentid) + "&AgentNum=" + $.trim(agentnum) + "&TbeginTime=" + $.trim(TBeginTime) + "&TendTime=" + $.trim(TEndTime) +
"&BusinessType=" + $.trim(selBussyType) + "&SkillGroup=" + $.trim(selSkillGroup) + "&DateType=" + hdDateType + "&r=" + Math.random();
            return pody;
        }
        //选择操作人
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                    $("#txtAgentNum").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('agentnum'));
                },
                afterClose: function () {
                }
            });
        }
        function BussyTypeChange() {
            var PlaceID = '<%=RegionID %>';

            if (PlaceID == '2') {
                $("select[id$='selSkillGroup']").empty();
                var selvalue = $("select[id$='selBussyType']").val();
                var skillgroup=TelNumManag.GetSkillGroup(selvalue);
                if(skillgroup!=null){         
                    $("select[id$='selSkillGroup']").append("<option value='-1'>全部</option>");
                    for(var i=0;i<skillgroup.length;i++) {
                         $("select[id$='selSkillGroup']").append("<option value='"+skillgroup[i].id+"'>"+skillgroup[i].name+"</option>");
                    }       
                }
            }
        }
        //分页操作
        function ShowDataByPost1(pody) {
                        LoadingAnimation("tableList");
                        if ('<%=RegionID %>' == '2') {
                            $('#ajaxTable').load('../AjaxServers/CallReport/SatisfactionListForXA.aspx', pody + "&r=" + Math.random(),function(){
                            $("#tableList tr:last").css("background-color","#f5f5f5");
                            $("#tableList tr:last td").css("line-height","45px");
                            $("#tableList tr:last td").css("font-weight","bold");
                            });
                        }
                        else if ('<%=RegionID %>' == '1') {
                            $('#ajaxTable').load('../AjaxServers/CallReport/SatisfactionListForBJ.aspx', pody + "&r=" + Math.random(),function(){
                            $("#tableList tr:last").css("background-color","#f5f5f5");
                            $("#tableList tr:last td").css("line-height","45px");
                            $("#tableList tr:last td").css("font-weight","bold");
                            });
                        }
        }
        function ShowDate(Datetype) {
            if (Datetype == "Date") {
                $('#hdDateType').val("1");
                $('#aShowUnRead').removeClass("redColor");
                $('#aMarkRead').removeClass("redColor");
                $("#aShowAll").addClass("redColor");
            }
            else if (Datetype == "Week")
            { 
                $('#hdDateType').val("2"); 
                $('#aShowAll').removeClass("redColor");
                $('#aMarkRead').removeClass("redColor");
                $("#aShowUnRead").addClass("redColor");
            }
            else if (Datetype == "Month")
            { 
                $('#hdDateType').val("3");
                $('#aShowAll').removeClass("redColor");
                $('#aShowUnRead').removeClass("redColor");
                $("#aMarkRead").addClass("redColor");
            }
            search();
        }
        function ExportData() {
           if (CheckForSelectCallRecordORIG("TBeginTime", "TEndTime")) {
                var pody = params();
                window.location = "../AjaxServers/CallReport/SatisfactionExport.aspx?" + pody + "&r=" + Math.random() + "&PlaceID="+<%=RegionID%>;
            }
        }
    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客服：</label>
                <input type="text" id="txtSelAgent" class="w200" readonly="true" onclick="SelectVisitPerson('employee','txtSelAgent','hidSelAgentId')" />
                <input type="hidden" id="hidSelAgentId" value="-2" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtAgentNum" class="w200" />
            </li>
            <li>
                <label>
                    统计时间：</label>
                <input type="text" name="TBeginTime" style="width: 106px" id="TBeginTime" vtype="isDate"
                    vmsg="统计时间格式不正确" class="w85" />
                至
                <input type="text" name="TEndTime" style="width: 106px" id="TEndTime" vtype="isDate"
                    vmsg="统计时间格式不正确" class="w85" />
            </li>
        </ul>
        <ul>
            <li style="width: 296px;">
                <label>
                    业务类型：</label>
                <select id="selBussyType" class="w200" onchange="BussyTypeChange()" runat="server"
                    style="width: 206px">
                </select></li>
            <li id="liSkillGroup">
                <label>
                    技能分组：</label>
                <select id="selSkillGroup" class="w200" runat="server" style="width: 206px">
                </select></li>
            <li class="btnsearch" style="float: right;">
                <input style="" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <input type="hidden" id="hdDateType" value="1" />
    <div class="optionBtn clearfix">
        <div>
            <span style="float: left;"><a href="javascript:void(0)" onclick="ShowDate('Date')"
                id="aShowAll">日</a>&nbsp;&nbsp;||&nbsp;&nbsp;<a href="javascript:void(0);" onclick="ShowDate('Week')"
                    id="aShowUnRead">周</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="ShowDate('Month');"
                        id="aMarkRead">月</a></span>
            <%if (DataExportButton)
              {%>
            <input id="btnPutOut" class="newBtn" type="button" onclick="ExportData()" style="*margin-top: 0px auto;"
                value="导出">
            <%}%>
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
