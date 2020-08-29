<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="数据模板管理" CodeBehind="TemplateManagementList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TemplateManagementList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            search();
        });

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#<%=selGroup.ClientID %>").val() != "-1") {
                AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selGroup.ClientID %>").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }


        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("List.aspx .bit_table > *", podyStr);
        }

        //获取参数
        function _params() {

            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));

            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            if ((beginTime != "" && !beginTime.isDate()) || (endTime != "" && !endTime.isDate())) {
                $.jAlert("输入的时间格式不正确");
                return false;
            }

            var name = encodeURIComponent($.trim($("#txtName").val()));

            var status = $(":checkbox[name='chkStatus']:checked").map(function () {
                return $(this).val()
            }).get().join(',');


            var group = "";
            if ($("#<%=selGroup.ClientID %>").val() != "-1" && $("#<%=selGroup.ClientID %>").val() != undefined) {
                group = $("#<%=selGroup.ClientID %>").val();
            }
            var category = "";
            if ($("#selCategory").val() != "-1" && $("#selCategory").val() != "") {
                category = $("#selCategory").val();
            }

            var creater = "";
            if ($("#<%=selCreater.ClientID %>").val() != "-1" && $("#<%=selCreater.ClientID %>").val() != "") {
                creater = $("#<%=selCreater.ClientID %>").val();
            }

            var pody = {
                name: name,
                status: status,
                group: group,
                category: category,
                creater: creater,
                beginTime: beginTime,
                endTime: endTime,
                r: Math.random()
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('List.aspx .bit_table > *', pody);
        }

        //弹出层-分类管理
        function OpenCategory() {
            $.openPopupLayer({
                name: "addCategory",
                parameters: {},
                url: "../SurveyInfo/SurveyCategoryManage.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }

        //生成模板 add lxw 13.3.23
        function GenerateTemplate(recid, ttCode) {
            //调用创建表的方法，创建成功，返回true，进行保存excel操作

            //创建物理表

            var pody = {
                Action: "Generate",
                recid: recid,
                ttCode: ttCode
            };

            if ($.jConfirm("生成模板后不可再编辑，确认生成？", function (r) {
                if (r) {
                    AjaxPost('/AjaxServers/TemplateManagement/GenerateTemplate.ashx', pody, function () {
                        $.blockUI({ message: '正在执行，请等待...' });
                    },
                function (data) {
                    $.unblockUI();
                    if (data == "") {
                        $.jPopMsgLayer("生成成功！", function () {
                            search();
                        });
                    }
                    else {
                        $.jAlert("生成失败！" + data);
                    }
                });
                }
            }));
        }

        //删除
        function deleteTemplate(recID) {
            var url = "/AjaxServers/TemplateManagement/TemplateEdit.ashx";
            if ($.jConfirm("是否确认删除该模板？", function (r) {
                if (r) {
                    $.blockUI({ message: '正在执行，请等待...' });
                    $.post(url, { Action: "Delete", RecID: recID, r: Math.random() }, function (data) {
                        $.unblockUI();
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            $.jPopMsgLayer("操作成功!", function () {
                                search();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg);
                        }
                    });
                }
            }));
        }

        //弹出层-分类管理
        function OpenCategory() {
            $.openPopupLayer({
                name: "addCategory",
                parameters: { TypeId: 2 },
                url: "/SurveyInfo/SurveyCategoryManage.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }

        function DownLoadExcel(url, recid, ttcode, a) {
            //生成文件
            var pody = {
                Action: "SaveExcelTemplate",
                recid: recid,
                ttCode: ttcode
            };
            AjaxPostAsync('/AjaxServers/TemplateManagement/GenerateTemplate.ashx', pody, null, function () {
            });
            a.href = url;
        }
    </script>
    <script type="text/javascript">
        //add lxw 13.12.16 增加 启用、停用 功能

        //启用
        function enableTemplate(recID) {
            operTemplate(recID, "Enable");
        }
        //停用
        function disableTemplate(recID) {
            operTemplate(recID, "Disable");
        }

        function operTemplate(recID, action) {
            var url = "/AjaxServers/TemplateManagement/TemplateEdit.ashx";
            var desc = "";
            switch (action) {
                case "Enable": desc = "是否确认启用该模板？";
                    break;
                case "Disable": desc = "是否确认停用该模板？";
                    break;
            }
            if ($.jConfirm(desc, function (r) {
                if (r) {
                    $.blockUI({ message: '正在执行，请等待...' });
                    $.post(url, { Action: action, RecID: recID, r: Math.random() }, function (data) {
                        $.unblockUI();
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            $.jPopMsgLayer("操作成功!", function () {
                                search();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg);
                        }
                    });
                }
            }));
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    模板名称：</label>
                <input type="text" id="txtName" class="w200" />
            </li>
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w200"
                    style="width: 98px; *width: 100px; width: 100px\9">
                </select>
                <select id="selCategory" class="w90">
                </select>
            </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreater" runat="server" class="w200" style="width: 206px">
                </select>
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    创建时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w95" />-<input type="text"
                    name="EndTime" id="tfEndTime" class="w95" />
            </li>
            <li>
                <label>
                    状态：</label>
                <span>
                    <input type="checkbox" id="chkStatusUnfinished" value="0" name="chkStatus" /><em
                        onclick='emChkIsChoose(this);'>未完成&nbsp;</em></span> <span>
                            <input type="checkbox" id="chkStatusUnused" value="1" name="chkStatus" /><em onclick='emChkIsChoose(this);'>已完成&nbsp;</em></span>
                <span>
                    <input type="checkbox" id="chkStatusUsed" value="2" name="chkStatus" /><em onclick='emChkIsChoose(this);'>已使用</em></span>
            </li>
            <li class="btnsearch" style="width: 150px;">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_btnAdd)
          { %>
        <input type="button" id="btnCategory" value="新增模板" class="newBtn" onclick="AddTemplate()" />
        <%} %>
        <%if (right_btnCategory)
          { %>
        <input type="button" id="btnAdd" value="新增模板分类" class="newBtn" onclick="OpenCategory()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <script type="text/javascript">

        function AddTemplate() {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/TemplateManagement/TemplateEdit.aspx');
            }
            catch (e) {
                window.open("/TemplateManagement/TemplateEdit.aspx");
            }
        }
    </script>
</asp:Content>
