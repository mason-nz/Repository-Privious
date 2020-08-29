<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportFilter.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyMapping.ExportFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="/css/base.css" type="text/css" rel="stylesheet" />
<link href="/css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $(document).ready(function () {
        $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
        $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });
    });
    function SubmitFilter() {

        var tasksubstart = $("#tfBeginTime").val();
        var tasksubend = $("#tfEndTime").val();
        var Browser = GetBrowserName();
        var urlstr = 'ExportExcel.aspx?SIID=<%=SIID%>&ProjectID=<%=ProjectID%>&typeID=<%=TypeID%>&tasksubstart=' + tasksubstart + '&tasksubend=' + tasksubend + '&Browser=' + Browser;
        try {
            var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/SurveyInfo/SurveyMapping/' + urlstr));
            $.closePopupLayer('ExportFilterPanel', false);
        }
        catch (e) {
            window.open(urlstr);
            $.closePopupLayer('ExportFilterPanel', false);
        }
    }



</script>
<div class="pop pb15 openwindow" id="divTip" style="background: #FFF; width: 450px;
    height: 200px">
    <div class="title bold">
        <h2>
            导出</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ExportFilterPanel',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('ExportFilterPanel',false);">
    </div>
    <div id='divAddOkPanel'>
        <div class="infor_li renyuan_cx">
            <div class="addexam clearfix" style="width: 100%">
                <ul class="clear">
                    <li style="width: 100%">
                        <label style="margin-left: -5px">
                            提交日期：</label>
                        <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                        至
                        <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 84px;
                            *width: 83px; width: 83px\9;" />
                    </li>
                </ul>
            </div>
            <div class="btn" style="margin: 30px auto; width: 100%">
                <input id="btnSave" type="button" onclick="SubmitFilter()" value="确 定" name="">&nbsp;&nbsp;&nbsp;<input
                    id="btncancel" type="button" onclick="javascript:$.closePopupLayer('ExportFilterPanel',false);"
                    value="取 消" name="">
            </div>
        </div>
    </div>
</div>
