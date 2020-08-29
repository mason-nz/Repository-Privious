<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataAdd.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.DataAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="../Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <link href="../Css/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#uploadify").uploadify({
                'buttonText': '选择',
                'auto': false,
                'swf': '/Js/uploadify.swf',
                'uploader': '../AjaxServers/ProjectManage/CustDataInput.ashx?v=' + Math.random(),
                'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
                'multi': false,
                'fileSizeLimit': '5MB',
                'queueSizeLimit': 10,
                'uploadLimit': 10,
                'method': 'post',
                'successTimeout': 900,
                'removeTimeout': 1,
                'fileTypeDesc': '支持格式:xls.',
                'fileTypeExts': '*.xls;',
                'width': 79,
                'height': 26,
                'onUploadSuccess': function (file, data, response) {
                    $("#btnImport").removeAttr("disabled");
                    $("#imgBusy").hide();
                    var jsonData = $.evalJSON(data);
                    if (jsonData.success == false) {
                        $('#SpanMsg').show().text("导入不成功!" + unescape(jsonData.message));
                    }
                    else {
                        var count = 0;
                        var array = $(unescape(jsonData.message).split(','));
                        for (var i = 0; i < array.length; i++) {
                            if (array[i] != undefined && array[i] != "" && array[i] != null) {
                                count++;
                            }
                        }
                        if ("<%=AddType%>" == "1") {
                            $("#hidExportSelectIDs").val("");
                            $("#hidCrmAddIDs").val(jsonData.message);
                        }
                        else {
                            $("#hidCrmSelectIDs").val("");
                            $("#hidExportAddIDs").val(jsonData.message);
                        }

                        $("#hidSelectIDsCount").val(Number($("#hidSelectIDsCount").val()) + Number(count));

                        if (jsonData.result != "") {
                            $.jAlert(jsonData.result);
                        }
                        $.closePopupLayer('AddDataPopup', true);
                    }
                },
                'onQueueComplete': function (queueData) {
                }
            });
            //生成文件
            var pody = {
                Action: "SaveExcelTemplate",
                recid: "<%=RecID %>",
                ttCode: "<%=TTCode %>"
            };
            AjaxPost('/AjaxServers/TemplateManagement/GenerateTemplate.ashx', pody, null, function () { });
        });
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmBatchImport() {
            var uploadify = $('#uploadify');
            var msg = '';
            var queueSize = uploadify.uploadify('queueLength');
            if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }
            if (queueSize > 1) { $.jAlert('只能导入一个文件'); return; }

            var ttcode = '<%=TTCode %>';
            var loginID = '<%=LoginUserID %>';
            var ProjectID = $("#hidProjectID").val();

            //设置传递的数据
            uploadify.uploadify('settings', 'formData', {
                Action: 'BatchImport',
                ttcode: ttcode,
                UserID: loginID,
                ProjectID: ProjectID,
                AddType: '<%=AddType%>',
                IsBlacklistCheck: '<%=IsBlacklistCheck %>',
                BlackListCheckType: '<%=BlackListCheckType %>',
                LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>'))
            });
            $("#imgBusy").show();
            $("#btnImport").attr("disabled", "disabled");
            //执行上传
            uploadify.uploadify('upload', '*');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 530px">
        <div class="title bold">
            <h2>
                添加数据</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AddDataPopup',false);">
            </a></span>
        </div>
        <ul class="clearfix ft14" id="addTemplatePage">
            <li>
                <label>
                    模板：</label>
                <a target="_blank" href='<%=ExcelFilePath %>'>下载模板&nbsp;&nbsp;(<%=ExcelFileName%>)</a>
            </li>
            <li>
                <label>
                    导入Excel：</label>
                <div style="width: 300px; padding-left: 130px; *padding-left: 0px;">
                    <input type="file" id="uploadify" name="uploadify" runat="server" style="float: left" /></div>
            </li>
            <li style="padding-left: 130px;">
                <input type="button" onclick="javascript:ConfirmBatchImport();" class="btnChoose"
                    id="Button1" value="导入" style="margin-right: 18px;" />
                <div>
                    <span id="imgBusy" style="display: none;">
                        <img src="../Images/blue-loading.gif" />正在导入，请稍候...</span>
                </div>
            </li>
            <li>
                <div id="SpanMsg" style="color: #ff0000; display: none; width: 454px; margin-left: 10px;
                    word-break: breal-all; white-space: normal; overflow-y: hidden; overflow-x: scroll">
                </div>
            </li>
        </ul>
    </div>
    <input type="hidden" id="hidLoginUserID" value='<%=LoginUserID %>' />
    </form>
</body>
</html>
