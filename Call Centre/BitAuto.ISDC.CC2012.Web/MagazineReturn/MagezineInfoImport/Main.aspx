<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.MagazineReturn.MagezineInfoImport.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

    $(document).ready(function () {
        $("#uploadify").uploadify({
            'buttonText': '上 传',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '../MagazineReturn/MagezineInfoImport/Handler.ashx',
            'auto': false,
            'multi': false,
            'width': 78,
            'height': 25,
            'successTimeout': 99999,
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
                    var jsonData = $.evalJSON(data); //debugger
                    if (jsonData.success) {
                        $('#SpanMsg').show().text(jsonData.message);
                    }
                    else {
                        var mobj = $.evalJSON(unescape(jsonData.message));
                        $('#SpanMsg').show().text("导入不成功!" + mobj[0].Infomation);
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
        var uploadify = $('#uploadify');
        var msg = '';

        var queueSize = uploadify.uploadify('queueLength');
        if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }

        var cooperationName = $.trim($("#txtCooperationName").val());
        var execCyle = $.trim($("#txtExecCycle").val());
        if (cooperationName.length <= 0) {
            $.jAlert('期数不可为空！'); return;
        }
        else if (!isNum(cooperationName)) {
            $.jAlert('期数必须是数字！'); return;
        }
        if (execCyle.length <= 0) {
            $.jAlert('执行周期不可为空！'); return;
        }

        disableConfirmBtn();
        $('#SpanMsg').hide();
        $('#TableMsg').hide();

        $("#SpanMsg").css("display", "block");

        //设置传递的数据
        uploadify.uploadify('settings', 'formData', {
            Action: 'BatchImport',
            CooperationName: encodeURIComponent(cooperationName),
            ExecCycle: encodeURIComponent(execCyle),
            userid: '<%=UserID %>',
            LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>'))
        });

        uploadify.uploadify('upload', '*')
    }


    function closePopup() {
        $('#uploadify').uploadify('destroy');
        $.closePopupLayer('UploadUserAjaxPopup', false);
    }
</script>
<div class="pop pb15 openwindow" style="height: 315px; width: 470px;">
    <div class="title bold">
        <h2>
            杂志回访数据导入
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopup();" />
        </span>
    </div>
    <!--主体内容设计部分-->
    <ul class="part01">
        <li><a style="margin-left: 12px; margin-right: 20px;" href="../MagazineReturn/MagezineInfoImport/Templet/杂志数据导入模版.xls">
            下载Excel导入模板</a></li>
        <li>
            <label>
                期数：</label><input type="text" id="txtCooperationName" style="width: 60px; float: left;" /><p
                    style="color: Red;">
                    *必须是数字</p>
        </li>
        <li>
            <label>
                执行周期：</label>
            <input type="text" name="txtExecCycle" maxlength="10" id="txtExecCycle" runat="server" /></li>
        <li>
            <label>
                导入Excel：</label>
            <div style="width: 300px; padding-left: 125px; *padding-left: 0px; margin-top: 5px;">
                <input type="file" id="uploadify" name="uploadify" runat="server" /></div>
        </li>
        <li class="btnsearch">
            <label>
                &nbsp;</label>
            <input type="button" class="button" value="提交" id="btnConfirm" onclick="javascript:ConfirmBatchImport();" />
        </li>
    </ul>
    <div class="cont_cxjg">
        <div id="divMsg">
            <span id="SpanMsg" style="color: #ff0000; display: none; width: 100%; margin-left: 10px;">
                正在导入EXCEL数据……</span>
        </div>
    </div>
</div>
