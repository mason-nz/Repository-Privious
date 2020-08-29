<%@ Page Language="C#" Title="质检成绩统计" AutoEventWireup="true" CodeBehind="ScoreStat.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.StatManage.ScoreStat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        var pody = "";
        function search() {
            var userId = $("#hdnUserID").val();
            var groupId = $("[id$='sltGroup']").val();   
            var recordBeginTime = $("#txtRecordBeginTime").val();
            var recordEndTime = $("#txtRecordEndTime").val();

            pody = "UserID=" + userId + "&GroupID=" + groupId + "&RecordBeginTime=" + recordBeginTime + "&RecordEndTime=" + recordEndTime;
            LoadingAnimation('ajaxTable');

            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $("#ajaxTable").load("/AjaxServers/QualityStandard/StatManage/ScoreStat.aspx",
                { UserID: userId, GroupID: groupId, RecordBeginTime: recordBeginTime, RecordEndTime: recordEndTime },
                function () {
                    StatAjaxPageTime(monitorPageTime, "/AjaxServers/QualityStandard/StatManage/ScoreStat.aspx?" + pody);

                });
        }
        function Export() {
            var userId = $("#hdnUserID").val();
            var groupId = $("[id$='sltGroup']").val();        
            var recordBeginTime = $("#txtRecordBeginTime").val();
            var recordEndTime = $("#txtRecordEndTime").val();

            $("#formExport [name='UserID']").val(userId);
            $("#formExport [name='GroupID']").val(groupId);         
            $("#formExport [name='RecordBeginTime']").val(recordBeginTime);
            $("#formExport [name='RecordEndTime']").val(recordEndTime);
            $("#formExport").submit();
        }
        //坐席弹出层
        function GetEmployeeAgent() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    var name = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("name");
                    var userID = $("#popupLayer_" + "AssignTaskAjaxPopupForSelect").data("userid");
                    if (name == undefined) {
                        name = "";
                        userID = "";
                    }
                    $("#txtEmployee").val(name);
                    $("#hdnUserID").val(userID);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }
        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#sltGroup").append("<option value='-1'>请选择业务组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#sltGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        $(function () {          
            $('#txtRecordBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtRecordEndTime\')}', onpicked: function () { document.getElementById("txtRecordEndTime").focus(); }, dateFmt: 'yyyy-MM-dd' }); });
            $('#txtRecordEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtRecordBeginTime\')}', dateFmt: 'yyyy-MM-dd' }); });

            $("#txtRecordBeginTime").val('<%=DateTime.Today.AddMonths(-3).ToString("yyyy-MM-dd") %>');
            $("#txtRecordEndTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd") %>');

            getUserGroup();
            //敲回车键执行方法
            enterSearch(search);
            search();
        });
    </script>
    <form>
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    坐席：</label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtEmployee" class="text02" name="ANI" readonly="readonly" />
                    <b onclick="GetEmployeeAgent()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="hdnUserID" name="UserID" />
                </div>
            </li>
            <li>
                <label>
                    所属分组：</label>
                <select id="sltGroup" class="w200" name="GroupID" style="width: 205px;">
                </select>
            </li>
           
            <li>
                <label>
                    通话时间：</label>
                <input type="text" id="txtRecordBeginTime" class="w95" name="RecordBeginTime" />-<input
                    type="text" id="txtRecordEndTime" class="w95" name="RecordEndTime" />
            </li>
            <li class="btnsearch" style="clear: none; width: 100px;">
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
    <form id="formExport" action="ExportScoreStat.aspx" method="post">
    <input type="hidden" id="hidden1" name="UserID" value="" />
    <input type="hidden" id="hidden2" name="GroupID" value="" />
    <input type="hidden" id="hidden5" name="RecordBeginTime" value="" />
    <input type="hidden" id="hidden6" name="RecordEndTime" value="" />
    </form>
</asp:Content>
