<%@ Page Title="项目报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="B_ProjectReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BusinessReport.B_ProjectReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        loadJS("controlParams");
        $(document).ready(function () {
            $('#txtBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtBeginTime\')}' }); });
        });
    </script>
    <form id="form1" action="../AjaxServers/BusinessReport/B_ProjectReportForExcel.aspx">
    <div class="searchTj" style="width: 100%;" id="dataID">
        <ul style="width: 98%;">
            <li>
                <label>
                    项目名称：</label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtProjectName" class="text02" readonly="readonly" />
                    <b onclick="GetProjectInfo()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="hdprojectid" name="ProjectID" value="" />
                    <input type="hidden" id="hdsourcetype" name="BusinessType" value="" />
                    <input type="hidden" id="ProjectTime" name="ProjectTime" value="" />
                </div>
            </li>
            <li>
                <label>
                    客服：</label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtAgent" class="text02" readonly="readonly" />
                    <b onclick="GetEmployeeAgent()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="hdnUserID" name="AgentID" value="" />
                </div>
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtAgentNum" class="w190" name="AgentNum" />
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    时间：</label>
                <input type="text" name="StartTime" id="txtBeginTime" vtype="isDate" vmsg="起始时间格式不正确"
                    class="w95" />-<input type="text" name="EndTime" id="txtEndTime" class="w95" vtype="isDate"
                        vmsg="结束时间格式不正确" />
            </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 290px;">
                <input class="cx" name="" id="btnSelect" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <input type="hidden" id="hidBrowser" name="Browser" />
    </div>
    <div class="optionBtn clearfix">
        <%if (right_Export)
          {%>
        <input name="" type="button" value="导出" onclick="javascript:ExportExcel()" class="newBtn"
            style="*margin-top: 3px;" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
    <script type="text/javascript">


        //导出
        function ExportExcel() {

            if ($("#txtProjectName").val() == "") {
                $.jAlert("项目不能为空！");
            }
            else {
                $("#form1").submit();
            }
        }


        //初始化项目
        function loadProject() {
            AjaxPostAsync("/AjaxServers/BusinessReport/B_ProjectReport_Deal.ashx", { Action: "getLastestProject", r: Math.random() }, null, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != null) {
                        $("#txtProjectName").val(jsonData.projectname);
                        $("#hdprojectid").val(jsonData.projectid);
                        $("#hdsourcetype").val(jsonData.source);
                        $("#ProjectTime").val(jsonData.ProjectTime);
                        search();
                    }
                }
            });
        }




        //项目弹出层
        function GetProjectInfo() {
            $("#dataID").data("name", $("#txtProjectName").val());
            $("#dataID").data("ProjectID", $("#hdprojectid").val());
            $("#dataID").data("Source", $("#hdsourcetype").val());
            $("#dataID").data("ProjectTime", $("#ProjectTime").val());
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/BusinessReport/ProjectSelect.aspx",
                beforeClose: function (e) {
                    var name = $("#dataID").data("name");
                    var projectID = $("#dataID").data("ProjectID");
                    var Source = $("#dataID").data("Source");
                    var projecttime = $("#dataID").data("ProjectTime");
                    if (name == undefined) {
                        name = "";
                        projectID = "";
                        Source = "";
                        projecttime = "";
                    }
                    $("#txtProjectName").val(name);
                    $("#hdprojectid").val(projectID);
                    $("#hdsourcetype").val(Source);
                    $("#ProjectTime").val(projecttime);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }



        //坐席弹出层
        function GetEmployeeAgent() {

            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('name', $("#txtAgent").val());
            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('userid', $("#hdnUserID").val());
            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('agentnum', $("#txtAgentNum").val());

            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {

                    var name = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("name");
                    var userID = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid");
                    var agentnum = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("agentnum");
                    if (name == undefined) {
                        name = "";
                        userID = "";
                        agentnum = "";
                    }
                    $("#txtAgent").val(name);
                    $("#hdnUserID").val(userID);
                    $("#txtAgentNum").val(agentnum);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('/AjaxServers/BusinessReport/B_ProjectReport.aspx', pody, function () {

                var trrows = $("#tableList tr").length;

                if (trrows > 1) {
                    $("#tableList").append('<tr id="trsum" style="background-color: rgb(245, 245, 245); height: 45px;" ></tr>');
                    $('#trsum').html('<span style="padding-left: 15px;">正在加载合计...</span>');
                    AjaxPostAsync("/AjaxServers/BusinessReport/B_ProjectReport_Deal.ashx", pody, null, function (data) {
                        $('#trsum').html("");
                        if (data != "") {
                            var jsonData = $.evalJSON(data);
                            if (jsonData != null) {
                                $("#trsum").append('<td style="font-weight: bold;">合计（共' + (trrows - 1) + '项）</td>');
                                $("#trsum").append('<td style="font-weight: bold;">--</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.projectname + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.assigncount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.tjcount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtcount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtlv + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.successcount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.cglv + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtfailcount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.wjtcount + '</td>');
                            }
                        }
                    });
                }

            });
        }
        //查询
        function search() {
            var begintime = $("#txtBeginTime").val();
            var endtime = $("#txtEndTime").val();
            if ($("#txtProjectName").val() == "") {
                $.jAlert("项目不能为空！");
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);
                LoadingAnimation("ajaxTable");
                $('#ajaxTable').load("/AjaxServers/BusinessReport/B_ProjectReport.aspx", podyStr, function () {

                    var trrows = $("#tableList tr").length;

                    if (trrows > 1) {
                        $("#tableList").append('<tr id="trsum" style="background-color: rgb(245, 245, 245); height: 45px;" ></tr>');
                        $('#trsum').html('<span style="padding-left: 15px;">正在加载合计...</span>');
                        AjaxPostAsync("/AjaxServers/BusinessReport/B_ProjectReport_Deal.ashx", podyStr, null, function (data) {
                            $('#trsum').html("");
                            if (data != "") {
                                var jsonData = $.evalJSON(data);
                                if (jsonData != null) {
                                    $("#trsum").append('<td style="font-weight: bold;">合计（共' + (trrows - 1) + '项）</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">--</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.projectname + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.assigncount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.tjcount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtcount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtlv + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.successcount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.cglv + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.jtfailcount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.wjtcount + '</td>');
                                }
                            }
                        });
                    }

                });
            }
        }
        //获取参数
        function _params() {
            var txtSearchTrueNameID = $.trim($('#hdnUserID').val());
            var txtSearchAgentNum = $.trim($('#txtAgentNum').val());

            var txtStartTime = $.trim($('#txtBeginTime').val());
            var txtEndTime = $.trim($('#txtEndTime').val());
            var selBusinessType = $.trim($('#hdsourcetype').val());
            var txtProjectID = $.trim($('#hdprojectid').val());

            var pody = {
                Action: 'getprojectreprotsum',
                StartTime: txtStartTime,       //统计日期（前一个）  暂时没有escape(）  HttpContext.Current.Server.UrlDecode(）
                EndTime: txtEndTime,           //统计日期（后一个）            
                AgentNum: txtSearchAgentNum,             //工号
                AgentID: txtSearchTrueNameID,         //AgentID(用户ID)
                BusinessType: selBusinessType,
                ProjectID: txtProjectID,
                r: Math.random()            //随机数
            }

            return pody;
        }

        $(function () {
            $("#hidBrowser").val(GetBrowserName());
            //加载项目
            loadProject();
            enterSearch(search);

        });

    </script>
    </form>
</asp:Content>
