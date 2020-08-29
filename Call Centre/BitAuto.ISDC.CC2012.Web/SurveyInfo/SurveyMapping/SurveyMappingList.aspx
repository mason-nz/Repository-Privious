<%@ Page Title="问卷结果管理" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="SurveyMappingList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyMapping.SurveyMappingList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#<%=selSGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
                $("#<%=selPGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selSGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                        $("#<%=selPGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类(问卷分类)
        function selGroupChange() {
            $("#selSCategory").children().remove();
            $("#selSCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#<%=selSGroup.ClientID %>").val() != "-1") {
                AjaxPostAsync("../../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selSGroup.ClientID %>").val(), TypeId: 1, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selSCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }

        //根据选择的分组绑定对应的分类(项目分类)
        function selProjectGroupChange() {
            $("#selPCategory").children().remove();
            $("#selPCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#<%=selPGroup.ClientID %>").val() != "-1") {
                AjaxPostAsync("../../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selPGroup.ClientID %>").val(), TypeId: 2, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selPCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }


        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../../AjaxServers/SurveyInfo/SurveyMapping/SurveyMappingList.aspx", podyStr);
        }

        //获取参数
        function _params() {
            var sname = encodeURIComponent($.trim($("#txtSName").val()));
            var pname = encodeURIComponent($.trim($("#txtPName").val()));

            var sgroup = "";
            if ($("#<%=selSGroup.ClientID %>").val() != "-1" && $("#<%=selSGroup.ClientID %>").val() != undefined) {
                sgroup = $("#<%=selSGroup.ClientID %>").val();
            }
            var scategory = "";
            if ($("#selSCategory").val() != "-1" && $("#selSCategory").val() != "") {
                scategory = $("#selSCategory").val();
            }

            var pgroup = "";
            if ($("#<%=selPGroup.ClientID %>").val() != "-1" && $("#<%=selPGroup.ClientID %>").val() != undefined) {
                pgroup = $("#<%=selPGroup.ClientID %>").val();
            }
            var pcategory = "";
            if ($("#selPCategory").val() != "-1" && $("#selPCategory").val() != "") {
                pcategory = $("#selPCategory").val();
            }
            var pody = {
                SName: sname,
                PName: pname,
                SBGID: sgroup,
                SSCID: scategory,
                PBGID: pgroup,
                PSCID: pcategory,
                r: Math.random()
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('../../AjaxServers/SurveyInfo/SurveyMapping/SurveyMappingList.aspx', pody);
        }

        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            selProjectGroupChange();
            search();
        });

    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    问卷名称：</label>
                <input type="text" id="txtSName" class="w190" />
            </li>
            <li>
                <label>
                    问卷分类：</label>
                <select id="selSGroup" onchange="javascript:selGroupChange()" runat="server" class="w90"
                    style="width: 98px; *width: 100px; width: 100px\9">
                </select>
                <select id="selSCategory" class="w90">
                </select>
            </li>
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtPName" class="w190" />
            </li>
        </ul>
        <ul>            
            <li>
                <label>
                    项目分类：</label>
                <select id="selPGroup" onchange="javascript:selProjectGroupChange()" runat="server"
                    class="w90" style="width: 98px; *width: 100px; width: 100px\9">
                </select>
                <select id="selPCategory" class="w90">
                </select>
            </li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div id="ajaxTable">
    </div>
</asp:Content>
