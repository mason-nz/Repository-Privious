<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCallManageList.aspx.cs"
    Title="自动外呼管理" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.AutoCallManageList"
    MasterPageFile="~/Controls/Top.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        $(function () {
            getUserGroup();
            selGroupChange();
            search();
            enterSearch(search);
        });
        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selGroup").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }
        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#selGroup").val() != "-1") {
                AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selGroup").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }

        function AddAutoCallProject() {
            $.openPopupLayer({
                name: "AddAutoCallProject",
                parameters: { TypeId: "2" },
                url: "PopPanel/AutoCallConfig.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        search();
                    }
                }
            });
        }

        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/ProjectManage/AutoCallManageList.aspx .bit_table > *", podyStr);
        }


        //获取参数
        function _params() {

            var name = encodeURIComponent($.trim($("#txtName").val()));

            var status = $(":checkbox[name='chkStatus']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var acStatus = $(":checkbox[name='acChkStatus']:checked").map(function () {
                return $(this).val();
            }).get().join(',');


            var group = "";
            if ($("#selGroup").val() != "-1" && $("#selGroup").val() != undefined) {
                group = $("#selGroup").val();
            }
            var category = "";
            if ($("#selCategory").val() != "-1" && $("#selCategory").val() != "") {
                category = $("#selCategory").val();
            }


            var pody = {
                name: name,
                status: status,
                acStatus: acStatus,
                group: group,
                category: category,
                r: Math.random()
            };

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {

            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('../AjaxServers/ProjectManage/AutoCallManageList.aspx?r=' + Math.random() + ' .bit_table > *', pody);
        }


        function editProject(pid, pName, cd, skid) {
            $.openPopupLayer({
                name: "AddAutoCallProject",
                parameters: { TypeId: "2" },
                url: "PopPanel/AutoCallConfig.aspx?pn=" + encodeURI(pName) + "&pid=" + pid + "&skgid=" + skid + "&cdid=" + cd + "&r=" + Math.random(),
                beforeClose: function (e, data) {
                    //window.location.reload();
                    if (e) {
                        search();
                    }
                }
            });
        }


        function startProject(pid, acStatus) {
            if (pid != '') {
                if (acStatus == null) {
                    acStatus = "";
                }
                AjaxPostAsync("/ProjectManage/PopPanel/AutoCallProjectManager.ashx", { Action: "startP", projectids: pid, acstatus: acStatus, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (typeof jsonData == "object") {
                        if (jsonData.result == 0) {
                            $.jPopMsgLayer("项目已开始", function () {
                                search();
                            });
                        } else if (jsonData.result == -1) {
                            alert(jsonData.msg);
                        } else {
                            alert(data);
                        }
                    } else {
                        alert(data);
                    }
                });
            }
        }


        function endProject(pid) {
            if (pid != '') {
                AjaxPostAsync("/ProjectManage/PopPanel/AutoCallProjectManager.ashx", { Action: "endP", projectids: pid, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (typeof jsonData == "object") {
                        if (jsonData.result == 0) {
                            $.jPopMsgLayer("项目已停止", function () {
                                search();
                            });
                        } else if (jsonData.result == -1) {
                            alert(jsonData.msg);
                        } else {
                            alert(data);
                        }
                    } else {
                        alert(data);
                    }
                });
            }
        }


        function HoldProject(pid) {
            if (pid != '') {
                AjaxPostAsync("/ProjectManage/PopPanel/AutoCallProjectManager.ashx", { Action: "pauseP", projectids: pid, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (typeof jsonData == "object") {
                        if (jsonData.result == 0) {
                            $.jPopMsgLayer("项目已暂停", function () {
                                search();
                            });
                        } else if (jsonData.result == -1) {
                            alert(jsonData.msg);
                        } else {
                            alert(data);
                        }
                    } else {
                        alert(data);
                    }
                });
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .liStatus label
        {
            font-weight: normal;
            cursor: pointer;
        }
    </style>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtName" class="w190" />
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" class="w90" style="width: 98px;
                    *width: 100px; cursor: pointer; width: 100px\9">
                </select>
                <select id="selCategory" class="w90" style="cursor: pointer;">
                </select>
            </li>
            <li class="liStatus">
                <label>
                    项目状态：</label>
                <span>
                    <input name="chkStatus" type="checkbox" value="0" /><em onclick='emChkIsChoose(this);'>未开始</em>
                    <input name="chkStatus" type="checkbox" value="1" /><em onclick='emChkIsChoose(this);'>进行中</em>
                    <input name="chkStatus" type="checkbox" value="2" /><em onclick='emChkIsChoose(this);'>已结束</em></span>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label style="font-weight: bolder;">
                    外呼状态：</label>
                <span>
                    <input type="checkbox" id="chkStatusUnfinished" value="0" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>未开始&nbsp;</em>
                    <input type="checkbox" id="chkStatusUnused" value="1" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>进行中&nbsp;</em>
                    <input type="checkbox" id="chkStatusUsed" value="2" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>已暂停</em>
                    <input type="checkbox" id="Checkbox1" value="3" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>已结束</em></span>
            </li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="button" id="btnCategory" value="设置外呼任务" class="newBtn" onclick="AddAutoCallProject()" />
    </div>
    <div id="ajaxTable">
    </div>
</asp:Content>
