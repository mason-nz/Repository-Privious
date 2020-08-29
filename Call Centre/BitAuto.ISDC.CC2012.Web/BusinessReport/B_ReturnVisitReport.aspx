<%@ Page Title="客户回访报表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="B_ReturnVisitReport.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BusinessReport.B_ReturnVisitReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        loadJS("controlParams");
    </script>
    <form id="form1" action="../AjaxServers/BusinessReport/B_ReturnVisitReportForExcel.aspx">
    <div class="searchTj" style="width: 100%;">
        <ul style="width: 98%;">
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
            <li>
                <label>
                    所属分组：</label>
                <select id="ddlBussiGroup" runat="server" class="w180">
                </select>
            </li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    时间：</label>
                <select id="SelYear" runat="server" onchange="yearChange()" class="w100">
                </select><select id="SelMonth" runat="server" class="w100">
                </select>
            </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 290px; margin-left: -12px">
                <input class="cx" name="" id="btnSelect" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
        <input type="hidden" id="hidBrowser" name="Browser" />
        <input type="hidden" id="hidYear" name="Year" />
        <input type="hidden" id="hidMonth" name="Month" />
        <input type="hidden" id="hidBgid" name="BGID" />
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

        //屏蔽2015年1到10月
        function yearChange() {
            var year = $('#<%=SelYear.ClientID %>').val();
            $("#<%=SelMonth.ClientID %>").empty();
            if (year == "2015") {
                $("#<%=SelMonth.ClientID %>").append("<option value='11'>11月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='12'>12月</option>");
            }
            else {
                $("#<%=SelMonth.ClientID %>").append("<option value='1'>1月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='2'>2月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='3'>3月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='4'>4月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='5'>5月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='6'>6月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='7'>7月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='8'>8月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='9'>9月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='10'>10月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='11'>11月</option>");
                $("#<%=SelMonth.ClientID %>").append("<option value='12'>12月</option>");
            }
            if (year == '<%=NowYear %>') {
                $("#<%=SelMonth.ClientID %>").val('<%=NowMonth %>');
            }
        }


        //导出
        function ExportExcel() {

            var bgid = $('#<%=ddlBussiGroup.ClientID %>').val();
            var year = $('#<%=SelYear.ClientID %>').val();
            var month = $('#<%=SelMonth.ClientID %>').val();
            $("#hidBgid").val(bgid);
            $("#hidYear").val(year);
            $("#hidMonth").val(month);
            if (bgid == "") {
                $.jAlert("分组不能为空！");
            }
            else if (year == "" || month == "") {
                $.jAlert("时间不能为空！");
            }
            else {
                $("#form1").submit();
            }
        }
        //坐席弹出层
        function GetEmployeeAgent() {

            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('name', $("#txtAgent").val());
            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('userid', $("#hdnUserID").val());
            $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data('agentnum', $("#txtAgentNum").val());

            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                parameters: { Groups: '<%=returnvisitBg%>' },
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
            $('#ajaxTable').load('/AjaxServers/BusinessReport/B_ReturnVisitReport.aspx', pody, function () {

                var trrows = $("#tableList tr").length;
                if (trrows > 1) {
                    $("#tableList").append('<tr id="trsum" style="background-color: rgb(245, 245, 245); height: 45px;"></tr>');
                    $('#trsum').html('<span style="padding-left: 15px;">正在加载合计...</span>');
                    AjaxPostAsync("/AjaxServers/BusinessReport/B_ReturnVisitReport_Deal.ashx", pody, null, function (data) {
                        $('#trsum').html("");
                        if (data != "") {
                            var jsonData = $.evalJSON(data);
                            if (jsonData != null) {
                                $("#trsum").append('<td style="font-weight: bold;">合计（共' + (trrows - 1) + '项）</td>');
                                $("#trsum").append('<td style="font-weight: bold;">--</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.dyfzmembercount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.hfmembercount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.fglv + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.hfcount + '</td>');
                                $("#trsum").append('<td style="font-weight: bold;">' + jsonData.wjtcount + '</td>');
                            }
                        }
                    });
                }


            });
        }
        //查询
        function search() {
            var bgid = $('#<%=ddlBussiGroup.ClientID %>').val();
            var year = $('#<%=SelYear.ClientID %>').val();
            var month = $('#<%=SelMonth.ClientID %>').val();
            if (bgid == "") {
                $.jAlert("分组不能为空！");
            }
            else if (year == "" || month == "") {
                $.jAlert("时间不能为空！");
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);
                LoadingAnimation("ajaxTable");
                $('#ajaxTable').load("/AjaxServers/BusinessReport/B_ReturnVisitReport.aspx", podyStr, function () {

                    var trrows = $("#tableList tr").length;
                    if (trrows > 1) {
                        $("#tableList").append('<tr id="trsum" style="background-color: rgb(245, 245, 245); height: 45px;"></tr>');
                        $('#trsum').html('<span style="padding-left: 15px;">正在加载合计...</span>');
                        AjaxPostAsync("/AjaxServers/BusinessReport/B_ReturnVisitReport_Deal.ashx", podyStr, null, function (data) {
                            $('#trsum').html("");
                            if (data != "") {
                                var jsonData = $.evalJSON(data);
                                if (jsonData != null) {
                                    $("#trsum").append('<td style="font-weight: bold;">合计（共' + (trrows - 1) + '项）</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">--</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.dyfzmembercount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.hfmembercount + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.fglv + '</td>');
                                    $("#trsum").append('<td style="font-weight: bold;">' + jsonData.hfcount + '</td>');
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

            var bgid = $('#<%=ddlBussiGroup.ClientID %>').val();
            var year = $('#<%=SelYear.ClientID %>').val();
            var month = $('#<%=SelMonth.ClientID %>').val();

            var pody = {
                Action: 'getreturnvisitsum',
                BGID: bgid,       //统计日期（前一个）  暂时没有escape(）  HttpContext.Current.Server.UrlDecode(）
                Year: year,           //统计日期（后一个）            
                AgentNum: txtSearchAgentNum,             //工号
                AgentID: txtSearchTrueNameID,         //AgentID(用户ID)
                Month: month,
                r: Math.random()            //随机数
            }

            return pody;
        }

        $(function () {
            $("#hidBrowser").val(GetBrowserName());
            yearChange();
            enterSearch(search);
            search();
        });

    </script>
    </form>
</asp:Content>
