<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.CrmCustImport.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
<script src="/Js/json2.js" type="text/javascript"></script>
<script type="text/javascript">

    function disableConfirmBtn() { $('#btnConfirm').attr('disabled', 'disabled'); }
    function enableConfirmBtn() { $('#btnConfirm').removeAttr('disabled'); }

    var uploadSuccess = true;
    $(document).ready(function () {
        $("#uploadify2").uploadify({
            'buttonText': '上 传',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/ProjectManage/CrmCustImport/Handler.ashx',
            'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
            'auto': false,
            'multi': false,
            'width': 78,
            'successTimeout': 600,
            'height': 25,
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
                    if (jsonData.success) {
                        var list = new Array(); //定义一数组
                        list = jsonData.message.split(",");
                       
                        $("#hidExportSelectIDs").val(""); //清除导入的
                        $("#hidCrmSelectIDs").val(jsonData.message);
                        $("#hidSelectIDsCount").val(list.length);

                        $.closePopupLayer('ImportCrmAjaxPopup', true);
                        $.closePopupLayer('SelectDataPopup', true);
                    }
                    else {
                        if (jsonData.message) {
                            try {
                                var mobj = $.evalJSON(unescape(jsonData.message));
                                var errormsg = "";
                                $.each(mobj, function (i, n) {
                                    errormsg = errormsg + n.Infomation + "<br/>";
                                });
                                $('#SpanMsg').show().html("导入不成功!<br/>" + errormsg);
                            } catch (e) {
                                $('#SpanMsg').show().text("导入不成功!" + unescape(jsonData.message));
                            }
                        }
                    }
                }
                else {

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
    function CRMCustImport() {
        var uploadify = $('#uploadify2');
        var msg = '';

        var queueSize = uploadify.uploadify('queueLength');
        if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }

        disableConfirmBtn();
        $('#SpanMsg').hide();
        $('#TableMsg').hide();

        $("#SpanMsg").css("display", "block");

        //设置传递的数据
        uploadify.uploadify('settings', 'formData', {
            Action: 'BatchImport',
            userid: '<%=UserID %>',
            LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>'))
        });

        uploadify.uploadify('upload', '*')
    }
</script>
<div class="pop pb15 openwindow" style="height: 315px; width: 470px;">
    <div class="title bold">
        <h2>
            CRM客户数据导入
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ImportCrmAjaxPopup',false);" />
        </span>
    </div>
    <!--主体内容设计部分-->
    <ul class="part01">
        <li><a style="margin-left: 12px; margin-right: 20px;" href="/ProjectManage/CrmCustImport/Templet/CRM客户导入模版.xls">
            下载Excel导入模板</a></li>
        <li>
            <label>
                导入Excel：</label>
            <div style="width: 300px; padding-left: 85px; *padding-left: 0px; margin-top: 5px;">
                <input type="file" id="uploadify2" name="uploadify" runat="server" /></div>
        </li>
        <li class="btnsearch">
            <label>
                &nbsp;</label>
            <input type="button" class="button" value="提交" id="btnConfirm" onclick="javascript:CRMCustImport();" />
        </li>
    </ul>
    <div class="cont_cxjg">
        <div id="divMsg">
            <span id="SpanMsg" style="color: #ff0000; display: none; width: 100%; margin-left: 10px;">
                正在导入EXCEL数据……</span>
        </div>
    </div>
</div>
