<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportExcelData.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService.ImportExcelData" %>

<link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
<script src="/Js/json2.js" type="text/javascript"></script>
<script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function ClearUlFile() {
    }

    function disableConfirmBtn() { $('#btnConfirm').attr('disabled', 'disabled'); }
    function enableConfirmBtn() { $('#btnConfirm').removeAttr('disabled'); }

    var uploadSuccess = true;
    $(document).ready(function () {
        //debugger;
        ClearUlFile();
        $("#uploadify").uploadify({
            'buttonText': '上 传',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/BlackWhite/AjaxService/HandlerImport.ashx?Type=<%=Type%>&BusinessTypeJSON=' + encodeURIComponent(JSON.stringify(TelNumManag.GetNameAndMultyidArr())),
            'auto': false,
            'multi': false,
            'width': 78,
            'height': 32,
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
            'fileTypeDesc': '支持格式:xls.',
            'fileTypeExts': '*.xls;',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {
                enableConfirmBtn();

            },
            'onQueueFull': function () {
                $.jAlert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var jsonData = $.evalJSON(data);
                    $('#SpanMsg').hide();

                    $.jAlert(jsonData.msg);

                    if (jsonData.msg == "保存成功" || jsonData.msg.indexOf("部分操作失败") != -1) {
                        $.closePopupLayer('UploadUserAjaxPopup', true);
                    }

                }
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                enableConfirmBtn();
            }
        });
    });
</script>
<script type="text/javascript" language="javascript">
    function ConfirmBatchImport() {
        //debugger
        var uploadify = $('#uploadify');
        var msg = '';
        var carType = '';
        var queueSize = uploadify.uploadify('queueLength');
        if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }


        disableConfirmBtn();
        //$('#SpanMsg').show();        
        uploadify.uploadify('upload', '*')
    }

</script>
<div class="pop pb15 openwindow" style="height: 215px; width:480px;">
    <div class="title bold">
        <h2>
            导入Excel回写文件
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('UploadUserAjaxPopup',false);" />
        </span>
    </div>
    <!--主体内容设计部分-->
    <ul class="part01">
        <li style="width: 300px"><a style="margin-left: 12px; margin-right: 20px;" href="<%=strTempUrl%>">
            下载Excel导入模板</a></li>
        <li style="width: 460px;">
        <div style="width:80px;float:left; padding-left: 20px">导入Excel：</div>
            <div style="width:300px; padding-left: 100px;*padding-left: 0px;"><input type="file" id="uploadify"
            name="uploadify" /></div>
            <div style="width:100px;padding-left: 100px"><input type="button" class="btnChoose" value="提交" id="btnConfirm" onclick="javascript:ConfirmBatchImport();" /></div>
        </li>
    </ul>
    <span id="SpanMsg" style="color: #ff0000; display: none; margin-left: 10px;">正在导入EXCEL数据……</span>
    <div class="cont_cxjg">
    </div>
</div>
<!--导出Excel form-->
<form id="formExport" action="/AjaxServers/GroupOrder/ImportExcelWriteBack/ExcelHelper.aspx" method="post">
<input type="hidden" id="hidData" name="hidData" />
</form>
