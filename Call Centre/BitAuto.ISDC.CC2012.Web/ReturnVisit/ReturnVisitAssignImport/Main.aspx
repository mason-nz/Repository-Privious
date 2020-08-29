<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.ReturnVisitAssignImport.Main" %>

<link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
<script src="/Js/json2.js" type="text/javascript"></script>
<script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function disableConfirmBtn() {
        $('#SpanMsg').show();
        $('#btnConfirm').attr('disabled', 'disabled');
        $('#uploadify').uploadify('disable', true)
        //setTimeout(function () { $('#uploadify').uploadify('disable', true); }, 500);

    }
    function enableConfirmBtn() {
        $('#SpanMsg').hide();
        $('#btnConfirm').removeAttr('disabled');
        $('#uploadify').uploadify('disable', false);
        //setTimeout(function () { $('#uploadify').uploadify('disable', false); }, 500);

    }
    function updateReturn(filename, filePath, updateflag) {
        disableConfirmBtn();      
        AjaxPost("ReturnVisitAssignImport/Handler.ashx", {
            Action: "updateReturnVisit",
            fileName: filename,
            filePath: filePath,
            updateflag: updateflag,
            LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')),
            r: Math.random()
        }, null, function (returnValue) {
            var jsonObj = $.evalJSON(returnValue);
            if (jsonObj.success) {
                var jsonData = $.evalJSON(returnValue);
                if (jsonObj.success) {
                    if (updateflag != "delete") {

                        $.jPopMsgLayer("分配客户成功", function () {
                            search(0);
                        });

                    }

                }
                else {
                    $.jAlert("分配失败！");
                }

            }
            enableConfirmBtn();
            closePopup();

        });
    }

    function closePopup() {
        $('#uploadify').uploadify('destroy');
        $.closePopupLayer('UploadUserAjaxPopup', false);
    }


    var uploadSuccess = true;
    $(document).ready(function () {
        $("#uploadify").uploadify({
            'buttonText': '上 传',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': 'ReturnVisitAssignImport/Handler.ashx',
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

                    var jsonObj = $.evalJSON(data);
                    if (jsonObj.success) {//不是运营客服 直接导入  或者是运营客服，但是不存在负责座席

                        $.jPopMsgLayer(jsonObj.result, function () {
                            closePopup();
                            search(0);
                        });
                    }

                    else {
                        if (jsonObj.result == "custidexists") {
                            var jsonFile = $.evalJSON(jsonObj.message);
                            $.jConfirm("您所选择的客户已存在负责坐席，您确定更换所选客户的负责坐席吗？", function (r) {

                                if (r) {
                                    updateReturn(jsonFile.fileName, jsonFile.filePath, "update");
                                }
                                else {
                                    updateReturn(jsonFile.fileName, jsonFile.filePath, "delete");
                                }
                            });

                        }

                        else if (jsonObj.result == "onlyupload") {
                            var jsonFile = $.evalJSON(jsonObj.message);
                               updateReturn(jsonFile.fileName, jsonFile.filePath, "onlyupload");

                        }

                        else if (jsonObj.result == "editupload") {
                            var jsonFile = $.evalJSON(jsonObj.message);                          
                                    updateReturn(jsonFile.fileName, jsonFile.filePath, "editupload");
                               
                          }
                        
                        else {                          
                            $.jAlert(jsonObj.result);
                            closePopup();

                        }

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

        uploadify.uploadify('upload', '*')

    }

</script>
<style>
    .uploadify-button.disabled
    {
        background-color: #fff;
    }
</style>
<div class="pop pb15 openwindow" style="height: 235px">
    <div class="title bold">
        <h2>
            批量分配
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopup();" /></span>
    </div>
    <!--主体内容设计部分-->
    <ul class="part01">
        <li style="width: 300px"><a style="margin-left: 12px; margin-right: 20px;" href="../ReturnVisit/ReturnVisitAssignImport/Templet/客户回访批量分配导入模板.xls">
            下载Excel模板</a></li>
        <li style="width: 460px;">
            <div style="width: 80px; float: left; padding-left: 20px">
                导入数据：</div>
            <div style="width: 300px; padding-left: 100px; *padding-left: 0px;">
                <input type="file" id="uploadify" name="uploadify" /></div>
            <div style="width: 100px; padding-left: 100px">
                <input type="button" class="btnChoose" value="提交" id="btnConfirm" onclick="javascript:ConfirmBatchImport();" /></div>
            <span id="SpanMsg" style="display: none; padding-left: 100px">
                <img src="../../Images/blue-loading.gif" />正在分配，请稍候...</span> </li>
    </ul>
    <div class="cont_cxjg">
    </div>
</div>
