<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.DataImport.Main" %>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script type="text/javascript">
    function ClearUlFile() {
    }

    function disableConfirmBtn() { $('#btnConfirm').attr('disabled', 'disabled'); }
    function enableConfirmBtn() { $('#btnConfirm').removeAttr('disabled'); }

    var uploadSuccess = true;
    $(function () {
        ClearUlFile();

        $("#uploadify").uploadify({
            'uploader': '../Js/uploadify.swf?_=' + Math.random(),
            'script': '../DataImport/Handler.ashx',
            'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
            'folder': '../DataImport/UpLoad/',
            'auto': false,
            'multi': false,
            'buttonImg': '../Images/selected.JPG',
            'cancelImg': '../Images/uploadify_cancel.png',
            'width': 78,
            'height': 32,
            'fileDesc': '支持格式:xls.',
            'fileExt': '*.xls;',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onComplete': function (event, queueID, fileObj, response, data) {
                var message = "";

                var jsonData = $.evalJSON(response);
                var Exprotdate = "";
                if (jsonData != '') {
                    $.each(jsonData.success, function (idx, item) {
                        message += item.information + "\n\r";
                    });
                    //                    $.each(jsonData.root, function (idx, item) {
                    //                        message += unescape(item.information) + "\n\r";
                    //                    });
                    $.each(jsonData.ExportData, function (idx, item) {
                        Exprotdate += item.information + "&";
                    });
                    $.jAlert(message);
                    $('#SpanMsg').show().text(message);
                    $('#hidData').val(Exprotdate);
                    if (Exprotdate != "&") {
                        $('#formExport').submit();
                    }
                }
            },
            'onAllComplete': function (event, data) {
                $("#uploadify").uploadifyClearQueue();
                enableConfirmBtn();
            },
            'onQueueFull': function () {
                $.jAlert('您最多只能上传1个文件！');
                return false;
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onError': function (event, queueID, fileObj, errorObj) {
                $.jAlert(errorObj.info);
                enableConfirmBtn();
            }
        });
    });
</script>
<script type="text/javascript" language="javascript">
    function ConfirmBatchImport() {
        var uploadify = $('#uploadify');
        var msg = '';
        var carType = '';
        var queueSize = uploadify.uploadifySettings('queueSize');
        if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }


        disableConfirmBtn();
        $('#SpanMsg').hide();
        uploadify.uploadifySettings('scriptData', {
            Action: 'BatchImport',
            UserID: '<%=SessionUserID%>'
        });
        uploadify.uploadifyUpload();
    }

</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            个人客户</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('UploadUserAjaxPopup');">
        </a></span>
    </div>
    <!--主体内容设计部分-->
    <ul class="clearfix ft14">
        <li><a style="margin-left: 12px; margin-right: 20px;" href="../DataImport/Templet/CustomersImport.xls">
            下载个人客户导入模板</a></li>
        <li style="width: 800px; height: auto;">
            <label style="width: 100px;">
                导入Excel：</label>
            <input type="file" id="uploadify" name="uploadify" runat="server" style="float: left" />
        </li>
    </ul>
    <div class="btn">
        <input type="button" value="提交" class="btnSave bold" id="btnConfirm" onclick="javascript:ConfirmBatchImport();"
            style="position: absolute; left: 240px; top: 98px;" />
    </div>
    <div id="divMsg">
        <span id="SpanMsg" style="color: #ff0000; display: none; width: 700px; margin-left: 10px;">
            正在导入EXCEL数据……</span>
        <div class="cont_cxjg">
        </div>
    </div>
</div>
<!--导出Excel form-->
<form id="formExport" action="/DataImport/Export.aspx" method="post">
<input type="hidden" id="hidData" name="hidData" />
</form>
