<%@ Page Title="项目列表" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ProjectList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.ProjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <link href="../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        //Source:1：数据清洗-Excel；2：数据清洗-Crm；3：客户回访；4：其他任务 5：易集客 6：厂家集客 7：易团购
        $(document).ready(function () {

            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            search();

            //删除
            $("a[name='a_delete']").live("click", function (e) {
                e.preventDefault();
                var aOjb = this;
                $.jConfirm("是否要删除此项目？", function (r) {
                    if (r) {
                        var id = $(aOjb).attr("projectid");

                        AjaxPost('/AjaxServers/ProjectManage/ModifyProjectStatus.ashx', { projectid: id, status: '-1' }, null, function (data) {
                            if (data == "success") {
                                $.jPopMsgLayer("删除成功", function () {
                                    window.location = window.location;
                                });
                            }
                            else {

                                $.jAlert(data);
                            }
                        });
                    }
                });
            });

            //结束项目
            $("a[name='aStopProject']").live("click", function (e) {
                e.preventDefault();
                var aOjb = this;
                $.jConfirm("是否要结束此项目？", function (r) {
                    if (r) {
                        var id = $(aOjb).attr("projectid");

                        AjaxPost('/AjaxServers/ProjectManage/ModifyProjectStatus.ashx', { projectid: id, status: '2' },

                        function () {
                            $.blockUI({ message: '正在处理，请等待...' });
                        },
                        function (data) {
                            $.unblockUI();
                            if (data == "success") {
                                $.jPopMsgLayer("结束操作成功", function () {
                                    window.location = window.location;
                                });
                            }
                            else {

                                $.jAlert(data);
                            }
                        });
                    }
                });
            });

            $('#txtName').autocomplete('/AjaxServers/ProjectManage/ProjectHandler.ashx', {
                minChars: 1,
                //width: showWidth,
                scrollHeight: 300,
                autoFill: false,
                delay: 100,
                matchContains: true,
                extraParams: { Action: 'GetProejctNameByAutoComplete', ProjectName: function () { return $("#txtName").val(); }, r: Math.random() },
                parse: function (data) {
                    if (data != "") {
                        return $.map(eval(data), function (row) {
                            return {
                                data: row,
                                value: row.ProjectID,    //此处无需把全部列列出来，只是两个关键列
                                result: data.ProjectName
                            }
                        });
                    }
                    else {
                        $("#txtName").removeAttr('ProjectID');
                    }
                },
                formatItem: function (data, i, n, value) {
                    return data.ProjectName;
                },
                formatResult: function (data, value) {
                    return data.ProjectID;
                }
            }).result(function (event, data, formatted) {
                if (data && data != "") {
                    $(this).val(data.ProjectName);
                    $(this).attr('ProjectID', data.ProjectID);
                }
            });


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
        //项目导出bussytype=1是其他任务，2是厂商集客
        function ExportData(projectid, bussytype) {
            var projectidstr = escape(projectid);
            var bussytype = escape(bussytype);
            $.openPopupLayer({
                name: "ProjectExportFilterPanel",
                parameters: { projectid: projectidstr, bussytype: bussytype },
                url: "ProjectExportFilter.aspx",
                afterClose: function (e, data) {
                }
            });
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
            if (name == '' || ($('#txtName').attr('ProjectID') == null)) { $("#txtName").removeAttr('ProjectID'); }
            var projectID = '';
            if ($('#txtName').attr('ProjectID')) {
                projectID = $('#txtName').attr('ProjectID');
            }
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

            var ACStatus = "";
            if ($("#li_ACStatus").css("display") == "none") {
                ACStatus = "";
            }
            else {
                ACStatus = $(":checkbox[name='ACStatus']:checked").map(function () {
                    return $(this).val()
                }).get().join(',');
            }

            var ISAutoCall = $(":checkbox[name='ISAutoCall']:checked").map(function () {
                return $(this).val()
            }).get().join(',');

            if (category == null) {
                category = "";
            }
            var pody = {
                name: name,
                projectID: projectID,
                statuss: status,
                group: group,
                category: category,
                creater: creater,
                beginTime: beginTime,
                endTime: endTime,
                ACStatus: ACStatus,
                ISAutoCall: ISAutoCall,
                r: Math.random()
            }

            return pody;
        }
        //分页操作
        function ShowDataByPost1(pody) {

            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('List.aspx?r=' + Math.random() + ' .bit_table > *', pody);
        }
        //弹出层-分类管理
        function OpenCategory() {
            $.openPopupLayer({
                name: "addCategory",
                parameters: { TypeId: "2" },
                url: "../SurveyInfo/SurveyCategoryManage.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }
        //生成任务
        function newTask(projectId, source) {
            if ($.jConfirm("是否确认将生成新任务吗？", function (r) {
                if (r) {
                    AjaxPost('/AjaxServers/ProjectManage/GenerateTask.ashx', { projectid: projectId, Source: source, Action: 'GenerateTask' },
                    function () {
                        $.blockUI({ message: '正在处理，请等待...' });
                    },
                     function (data) {
                         $.unblockUI();
                         data = $.evalJSON(data);
                         if (data.msg == "success") {
                             AssignmentTask(data.data, projectId, function () { search(); });
                         }
                         else {
                             $.jAlert(data.msg);
                         }
                     });
                }
            }));
        }
        //批量分配任务(两处：2)
        function AssignmentTask(data, projectId, callback) {
            if (data && data.MinOtherTaskID && data.MaxOtherTaskID && data.open == 'true') {
                $.openPopupLayer({
                    name: "AssignmentTaskNew",
                    parameters: {
                        MinOtherTaskID: data.MinOtherTaskID,
                        MaxOtherTaskID: data.MaxOtherTaskID,
                        ProjectID: projectId,
                        r: Math.random()
                    },
                    url: "AssignmentTaskNew.aspx",
                    beforeClose: function (e, data) {
                        if (callback != null) {
                            callback();
                        }
                    }
                });
            }
            else {
                callback();
            }
        }
        //分配任务点击事件
        function DistributeTask(projectid) {
            $.openPopupLayer({
                name: "AssignmentTaskNew",
                parameters: {
                    ProjectID: projectid,
                    r: Math.random()
                },
                url: "AssignmentTaskNew.aspx",
                beforeClose: function (e) {
                    if (e) {
                        search();
                    }
                    enterSearch(search);
                }
            });
        }

        //添加按钮链接
        function AddProject() {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/ProjectManage/EditProject.aspx');
            }
            catch (e) {
                window.open("/ProjectManage/EditProject.aspx");
            }
        }
        //设置自动外呼状态是否显示
        function SetAutoShow() {
            var c = $("#Checkbox1")[0].checked;
            if (c) {
                $("#li_ACStatus").css("display", "");
            }
            else {
                $("#li_ACStatus").css("display", "none");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtName" class="w190" />
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w90"
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
            <li style="width: 284px;">
                <label>
                    创建时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 83px;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 83px;" />
            </li>
            <li>
                <label>
                    项目状态：</label>
                <span>
                    <input type="checkbox" id="chkStatusUnfinished" value="0" name="chkStatus" /><em
                        onclick='emChkIsChoose(this);'>未开始&nbsp;</em></span> <span>
                            <input type="checkbox" id="chkStatusUnused" value="1" name="chkStatus" /><em onclick='emChkIsChoose(this);'>进行中&nbsp;</em></span>
                <span>
                    <input type="checkbox" id="chkStatusUsed" value="2" name="chkStatus" /><em onclick='emChkIsChoose(this);'>已结束</em></span>
            </li>
            <li>
                <label>
                    自动外呼：</label>
                <span>
                    <input type="checkbox" id="Checkbox1" value="1" name="ISAutoCall" onclick='SetAutoShow();' /><em
                        onclick='emChkIsChoose(this);SetAutoShow();'>是&nbsp;</em> </span><span>
                            <input type="checkbox" id="Checkbox2" value="0" name="ISAutoCall" /><em onclick='emChkIsChoose(this);'>否&nbsp;</em>
                        </span></li>
        </ul>
        <ul>
            <li id="li_ACStatus" style="display: none">
                <label>
                    外呼状态：</label>
                <span>
                    <input type="checkbox" id="Checkbox3" value="0" name="ACStatus" /><em onclick='emChkIsChoose(this);'>未开始&nbsp;</em>
                </span><span>
                    <input type="checkbox" id="Checkbox4" value="1" name="ACStatus" /><em onclick='emChkIsChoose(this);'>进行中&nbsp;</em>
                </span><span>
                    <input type="checkbox" id="Checkbox5" value="2" name="ACStatus" /><em onclick='emChkIsChoose(this);'>暂停中&nbsp;</em>
                </span><span>
                    <input type="checkbox" id="Checkbox6" value="3" name="ACStatus" /><em onclick='emChkIsChoose(this);'>已结束&nbsp;</em>
                </span></li>
            <li class="btnsearch" style="float: right;">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()"
                    id="btnsearch_id" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_btnAdd)
          { %>
        <input type="button" id="btnCategory" value="新增项目" class="newBtn" onclick="AddProject()" />
        <%} %>
        <%if (right_btnCategory)
          { %>
        <input type="button" id="btnAdd" value="新增项目分类" class="newBtn" onclick="OpenCategory()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <form id="formExport" action="/AjaxServers/ProjectManage/ExportProjectTask.aspx">
    <input type="hidden" name="projectid" value="" />
    <input type="hidden" id="Browser" name="Browser" value="" />
    </form>
</asp:Content>
