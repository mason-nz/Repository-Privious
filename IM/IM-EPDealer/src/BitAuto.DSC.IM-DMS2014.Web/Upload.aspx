<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="BitAuto.DSC.IM2014.Server.Web.Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link type="text/css" href="IMCss/css.css" rel="stylesheet" />
    <link href="IMCss/uploadify.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
        
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/AspNetComet.js"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="Scripts/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitUploadify();
        });
        function InitUploadify() {
            var uploadSuccess = true;
            $("#uploadify").uploadify({
                'buttonText': '选择',
                'auto': false,
                'swf': 'Scripts/uploadify.swf',
                'uploader': 'AjaxServers/FileLoad.ashx?v=' + Math.random(),
                'multi': true,
                'fileSizeLimit': '5MB',
                'queueSizeLimit': 1,
                'uploadLimit': 1,
                'method': 'post',
                'removeTimeout': 1,
                'fileTypeDesc': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
                'fileTypeExts': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
                'width': 79,
                'height': 26,
                'onUploadSuccess': function (file, data, response) {
                    if (response == false) {
                        uploadSuccess = false;
                        $.jAlert("上传失败!");
                    }
                    else {
                        //    alert('The file ' + file.name + ' was successfully uploaded with a response of ' + response + ':' + data);
                        var jsonData = $.evalJSON(data);

                        if (jsonData.result == "noFiles") {
                            uploadSuccess = false;
                            $.jAlert("请选择文件!");
                        }
                        else if (jsonData.result == "failure") {
                            uploadSuccess = false;
                            $.jAlert("上传文件出错!");
                        }
                        else if (jsonData.result != "succeed") {
                            uploadSuccess = false;
                        }
                        else {
                            //上传成功
                            uploadSuccess = true;
                        }
                    }
                },
                'onQueueComplete': function (queueData) {
                    // alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
                    if (uploadSuccess) { //上传都成功
                    }
                }
            });
        }
        function ConfirmBatchImport() {
            alert(1);
            var uploadify = $('#uploadify');
            uploadify.uploadify('upload', '*')
            //$('#uploadify').uploadify('upload', '*');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul>
            <li>
                <label>
                    上传：</label>
                <span>每次最多上传10份文档，每份文档不超过5M; 支持类型 doc,docx,ppt,pptx,xls,xlsx,pps,pdf,txt</span>
            </li>
            <li style="padding-left: 100px;">
                <input type="file" id="uploadify" name="uploadify" /><input type="button" value="确定" onclick="javascript:ConfirmBatchImport();" />
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
