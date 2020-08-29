<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectExportFilter.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.ProjectExportFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="/css/base.css" type="text/css" rel="stylesheet" />
<link href="/css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $(document).ready(function () {
        $('#tfBeginTimeExport').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTimeExport\')}', onpicked: function () { document.getElementById("tfEndTimeExport").focus(); } }); });
        $('#tfEndTimeExport').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTimeExport\')}' }); });


        $('#tfBeginTimesub').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTimesub\')}', onpicked: function () { document.getElementById("tfEndTimesub").focus(); } }); });
        $('#tfEndTimesub').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTimesub\')}' }); });

    });
    //1是其他任务，2是厂商集客
    function SubmitFilter() {

        var bussytype = '<%=bussytype %>';
        var taskcreatestart = $("#tfBeginTimeExport").val();
        var taskcreateend = $("#tfEndTimeExport").val();
        var tasksubstart = $("#tfBeginTimesub").val();
        var tasksubend = $("#tfEndTimesub").val();
        var browser = GetBrowserName();
       
        var address = ""; 
        //source=4 其他任务
        if (bussytype == '1') {
            address = '/AjaxServers/ProjectManage/ExportProjectTask.aspx';
        }
        //source=6 厂商集客
        else if (bussytype == '2') {
            address = '/AjaxServers/ProjectManage/ExportProjectForCJK.aspx';
        }
        //source=7 易团购
        else if (bussytype == '3') {
            address = '/AjaxServers/ProjectManage/ExportProjectForYTG.aspx';
        }

        //导出
        if (address != "") {
            $("#formProjectExport").attr("action", address);
            $("#formProjectExport [name='projectid']").val("<%=projectid%>");
            $("#formProjectExport [name='Browser']").val(browser);
            $("#formProjectExport [name='tasksubstart']").val(tasksubstart);
            $("#formProjectExport [name='tasksubend']").val(tasksubend);
            $("#formProjectExport [name='taskcreatestart']").val(taskcreatestart);
            $("#formProjectExport [name='taskcreateend']").val(taskcreateend);
            $("#formProjectExport [name='r']").val(Math.random());
            $("#formProjectExport").submit();
            $.closePopupLayer('ProjectExportFilterPanel', false);
        }
    }
</script>
<form id="formProjectExport" action="" method="post">
<input type="hidden" id="hidden_projectid" name="projectid" value="" />
<input type="hidden" id="hidden_Browser" name="Browser" value="" />
<input type="hidden" id="hidden_tasksubstart" name="tasksubstart" value="" />
<input type="hidden" id="hidden_tasksubend" name="tasksubend" value="" />
<input type="hidden" id="hidden_taskcreatestart" name="taskcreatestart" value="" />
<input type="hidden" id="hidden_taskcreateend" name="taskcreateend" value="" />
<input type="hidden" id="hidden_r" name="r" value="" />
</form>
<div class="pop pb15 openwindow" id="divTip" style="background: #FFF; width: 450px;
    height: 250px">
    <div class="title bold">
        <h2>
            导出</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ProjectExportFilterPanel',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('ProjectExportFilterPanel',false);">
    </div>
    <div id='divAddOkPanel'>
        <div class="infor_li renyuan_cx">
            <div class="addexam clearfix" style="width: 100%">
                <ul class="clear">
                    <li style="width: 100%">
                        <label>
                            任务生成日期：</label>
                        <input type="text" name="tfBeginTimeExport" id="tfBeginTimeExport" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                        至
                        <input type="text" name="tfEndTimeExport" id="tfEndTimeExport" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                    </li>
                    <li style="width: 100%">
                        <label>
                            任务提交日期：</label>
                        <input type="text" name="tfBeginTimesub" id="tfBeginTimesub" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                        至
                        <input type="text" name="tfEndTimesub" id="tfEndTimesub" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                    </li>
                </ul>
            </div>
            <div class="btn" style="margin: 30px auto; width: 100%">
                <input id="btnSave" type="button" onclick="SubmitFilter()" value="确 定" name="">&nbsp;&nbsp;&nbsp;<input
                    id="btncancel" type="button" onclick="javascript:$.closePopupLayer('ProjectExportFilterPanel',false);"
                    value="取 消" name="">
            </div>
        </div>
    </div>
</div>
