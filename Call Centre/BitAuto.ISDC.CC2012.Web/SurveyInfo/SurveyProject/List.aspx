<%@ Page Language="C#" Title="培训调查管理" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.List" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/jquery.uploadify.v2.1.4.min.js"></script>
    <script type="text/javascript" src="/Js/swfobject.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            getUserGroup();
            //敲回车键执行方法
            enterSearch(search);
            InitWdatePicker(2, ["txtBeginTime", "txtEndTime"]);
            UserGroupChanged();
            search();
        });

        function getUserGroup() {
          
            AjaxPostAsync("../../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#sltUserGroup").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#sltUserGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });


        }


        function GetPody() {
            var projectName = $.trim($("#txtProjectName").val());
            var bgId = $.trim($("[id$='sltUserGroup']").val());
            var scId = $.trim($("#sltSurveyCategory").val());
            var busniessGroup = "";

            var surveyStatus = $("input[class='surveystatus']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            var beginTime = $.trim($("#txtBeginTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            var createUserId = $("[id$='sltCreateUser']").val();
            var pody = {
                ProjectName: escapeStr(projectName),
                BGID: escapeStr(bgId),
                SCID: escapeStr(scId),
                BusniessGroup: escapeStr(busniessGroup),
                SurveyStatus: escapeStr(surveyStatus),
                BeginTime: escapeStr(beginTime),
                EndTime: escapeStr(endTime),
                CreateUserID: createUserId,
                R: Math.random()
            }
            return pody;
        }
        //查询
        function search() {
            //加载查询结果
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/SurveyInfo/SurveyProject/List.aspx", GetPody());
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('/AjaxServers/SurveyInfo/SurveyProject/List.aspx', GetPody());
        }
        function openUploadExcelInfoAjaxPopup() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "../DataImport/Main.aspx"
            });
        }
        //所属业务组改变时，重新加载分类
        function UserGroupChanged() {
            $("[id$='sltSurveyCategory']").find("option").remove();
            var bgId = $("[id$='sltUserGroup']").val();
            //            if (bgId != "-1") {
            //                $("#sltSurveyCategory").show();
            //            }
            //            else {
            //                $("#sltSurveyCategory").hide();
            //            }
            $.post("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: bgId }, function (data) {
                if (data) {
                    $("[id$='sltSurveyCategory']").append("<option value='-1'>请选择分类</option>");
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, item) {
                        $("[id$='sltSurveyCategory']").append("<option value='" + item.SCID + "'>" + item.Name + "</option>");
                    });
                }
            });
        }
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    问卷名称：
                </label>
                <input type="text" id="txtProjectName" class="w200" />
            </li>
            <li>
                <label>
                    分类：
                </label>
                <select id="sltUserGroup"  class="w60" style="width: 101px;" onchange="UserGroupChanged()">
                </select>
                <select id="sltSurveyCategory" class="w60" style="width: 101px;">
                </select>
            </li>
            <li>
                <label>
                    状态：
                </label>
                <input type="checkbox" class="surveystatus" value="0" style="border: none;" /><em
                    onclick="emChkIsChoose(this)">未开始</em>
                <input type="checkbox" class="surveystatus" value="1" style="border: none;" /><em
                    onclick="emChkIsChoose(this)">进行中</em>
                <input type="checkbox" class="surveystatus" value="2" style="border: none;" /><em
                    onclick="emChkIsChoose(this)">已结束</em> </li>
        </ul>
        <ul style="clear: both; width: 90%;">
            <li>
                <label>
                    调查时间：
                </label>
                <input type="text" name="BeginTime" id="txtBeginTime" class="w95" />-<input type="text"
                    name="EndTime" id="txtEndTime" class="w95" />
            </li>
            <li>
                <label>
                    创建人：
                </label>
                <select id="sltCreateUser" runat="server" class="w200" style='width: 206px;'>
                </select>
            </li>
            <li class="btnsearch" style="float: left; clear: none; margin-top: 5px; width: 350px;">
                <input type="button" value="查 询" id="btnSearch" class="cx" style="margin-left: 30px;"
                    onclick="search()" />
            </li>
        </ul>
    </div>
    <div class="bit_table" id="Div1">
        <div class="optionBtn mt10" style="cursor: auto;">
            <div class="new">
                <a target="_blank" style="background: url(/css/img/addProjectObj.gif) scroll 0 0;
                    float: right;" href="Dispose.aspx"></a>
            </div>
        </div>
        <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
        </div>
        <input type="hidden" id="hidFieldsCustomer" value="" />
        <input type="hidden" id="hidIsOkOrCancel" value="0" />
</asp:Content>
