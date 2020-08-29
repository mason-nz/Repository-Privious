<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Statistics.MemberIDImport.Main" %>

<link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
<script src="/Js/json2.js" type="text/javascript"></script>
<script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function disableConfirmBtn() {
        $('#btnConfirm').attr('disabled', 'disabled');
    }
    function enableConfirmBtn() {
        $('#btnConfirm').removeAttr('disabled');
    }
    var uploadSuccess = true;
    $(document).ready(function () {
        $("#uploadify").uploadify({
            'buttonText': '上 传',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/Statistics/MemberIDImport/Handler.ashx',
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
                    if (jsonObj.success) {
                        $('#TableMsg').hide();
                        $('#hidData').val($.evalJSON(data).message);
                        $('#formExport').submit();
                    }
                    else {
                        document.write(jsonObj.message);
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
        $('#SpanMsg').hide();
        $('#TableMsg').hide();
        uploadify.uploadify('upload', '*')
    }

    function closePopup() {
        $('#uploadify').uploadify('destroy');
        $.closePopupLayer('UploadUserAjaxPopup', false);
    }

</script>
<div class="pop pb15 openwindow" style="height: 215px">
    <div class="title bold">
        <h2>
            <%=TitleStr%>
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopup();" />
        </span>
    </div>
    <!--主体内容设计部分-->
    <ul class="part01">
        <li style="width: 300px"><a style="margin-left: 12px; margin-right: 20px;" href="../Statistics/MemberIDImport/Templet/会员ID号导入模板.xls">
            会员ID导入模板</a></li>
        <li style="width: 460px;">
            <div style="width: 80px; float: left; padding-left: 20px">
                导入Excel：</div>
            <div style="width: 300px; padding-left: 100px; *padding-left: 0px;">
                <input type="file" id="uploadify" name="uploadify" /></div>
            <div style="width: 100px; padding-left: 100px">
                <input type="button" class="btnChoose" value="提交" id="btnConfirm" onclick="javascript:ConfirmBatchImport();" /></div>
        </li>
    </ul>
    <span id="SpanMsg" style="color: #ff0000; display: none; margin-left: 10px;">正在导入EXCEL数据……</span>
    <div class="cont_cxjg">
    </div>
</div>
<!--导出Excel form-->
<form id="formExport" action="../Statistics/MemberIDImport/MemberInfoExport.aspx"
method="post">
<input type="hidden" id="hidData" name="hidData" />
<%--标识是导出会员信息还是车商通信息 (Member:会员信息，CstMember:车商通信息，CustIDYP:易湃客户ID，CustIDCST:车商通客户ID  )--%>
<input type="hidden" id="hidType" name="hidType" value='<%=ExportTyle %>' />
</form>
