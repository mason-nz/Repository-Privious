<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOkPanel.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.AddOkPanel" %>

<link href="/css/base.css" type="text/css" rel="stylesheet" />
<link href="/css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">

    $(document).ready(function () {

        var projectId = '<%=returnProjectID %>';
        var status = '<%=status %>';
        var source = '<%=Source %>';

        $("#returnGenTask").live("click", function (e) { e.preventDefault(); GetTask(); });

        //生成任务
        function GetTask() {

            var RecCount = '<%=RecCount %>';
            var AddIDcount = '<%=AddIDcount %>';
            if (AddIDcount == 0) {
                $.jAlert("本项目没有未生成任务的数据！", function () { $.closePopupLayer('AddOkPanel', true); });
                return;
            }
            var msg = "本项目共<span style='color:red;'>" + RecCount + "</span>条数据，确认生成任务？";

            if (status == "1") {
                msg = "共有<span style='color:red;'>" + AddIDcount + "</span>条数据还未生成任务，确认生成任务？";
            }

            $.jConfirm(msg, function (r) {
                if (r) {
                    AjaxPost('/AjaxServers/ProjectManage/GenerateTask.ashx', { projectid: projectId, source: source, Action: 'GenerateTask' },
                function () {
                    $("#divTip").hide();
                    $.blockUI({ message: '正在处理，请等待...' });
                },
                    function (data) {
                        $.unblockUI();
                        data = $.evalJSON(data);
                        if (data.msg == "success") {
                            AssignmentTask(data.data,projectId, function () { $.closePopupLayer('AddOkPanel', false); });
                        }
                        else {
                            $("#divTip").show();
                            $.jAlert(data.msg, function () { $.closePopupLayer('AddOkPanel', false); });
                        }
                    });
                }
            });
        }
    });
    //批量分配任务(两处：1)
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
                    $.closePopupLayer('AddOkPanel', false);  
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

    //测试：批量分配任务
    function TestAssignmentTask() {
        var data = {
            MinOtherTaskID: 'OTH0226364',
            MaxOtherTaskID: 'OTH0246363'
        };
        AssignmentTask(data,377, function () { });
    }

</script>
<div class="pop pb15 openwindow" id="divTip" style="background: #FFF; width: 350px;">
    <div class="title bold">
        <h2>
            操作提示</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AddOkPanel',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('AddOkPanel',false);">
    </div>
    <div id='divAddOkPanel'>
        <div class="infor_li renyuan_cx">
            <br />
            <table style="width: 100%;">
                <tr>
                    <td style="width: 20%">
                        &nbsp;
                    </td>
                    <td style="width: 20%">
                        <img src="/Images/u61_normal.png" />
                    </td>
                    <td style="width: 40%">
                        <span style="font-weight: bold;" id="spanSaveOk">项目已保存成功！</span> <span style="font-weight: bold;
                            display: none;" id="spanGenOk">任务已生成！</span>
                        <br />
                        请选择下一步操作
                    </td>
                    <td style="width: 20%">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <div style="margin-left: auto; margin-right: auto; text-align: center; padding-top: 20px;"
                id="divSaveOk">
                <a id="returnAndEdit" href="/ProjectManage/EditProject.aspx?projectid=<%=returnProjectID %>"
                    style="cursor: pointer;">返回编辑</a> &nbsp;&nbsp;&nbsp; <a id="returnGenTask" href="#"
                        style="cursor: pointer;">生成任务</a>
                <%--&nbsp;&nbsp;&nbsp; <a id="A3" href="#" style="cursor: pointer;" onclick="TestAssignmentTask()">分配任务（测试）</a>--%>
            </div>
            <div style="margin-left: auto; margin-right: auto; text-align: center; padding-top: 20px;
                display: none;" id="divGenOk">
                <a id="A1" href="/ProjectManage/EditProject.aspx" style="cursor: pointer;">添加项目</a>&nbsp;&nbsp;&nbsp;
                <a id="A2" href="/CustCheck/CheckTaskList.aspx" style="cursor: pointer;">分配任务</a>
            </div>
        </div>
    </div>
</div>
