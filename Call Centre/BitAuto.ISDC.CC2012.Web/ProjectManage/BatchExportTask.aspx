<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchExportTask.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.BatchExportTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-size: 10pt;
        }
        li
        {
            margin-top: 5px;
        }
    </style>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script type="text/javascript">


        function BatchExport() {

            var list = $("#TextprojectIDs").val().split(",");

            $(list).each(function (i, v) {

                var projectid = v;

                AjaxPostAsync('/AjaxServers/ProjectManage/BatchExportTask.ashx', { projectid: projectid }, null, function (data) {

                    var jsonData = $.evalJSON(data);

                    var htmlLi = "";
                    if (jsonData.ErrInfo == "") {
                        htmlLi += " <li>导出成功:【ID:" + jsonData.ProjectID + ",项目名称：" + jsonData.ProjectName + "】</li>";
                    }
                    else {
                        htmlLi += " <li style='color:red;'>导出失败:【ID:" + jsonData.ProjectID + ",项目名称：" + jsonData.ProjectName + "】【错误：" + jsonData.ErrInfo + "】</li>";
                    }

                    $("#ulInfo").append(htmlLi);
                });

            });
        }
    </script>
</head>
<body>
    <textarea id="TextprojectIDs">
</textarea>
    <input type="button" value="批量导出" onclick="BatchExport()" />
    <ul id="ulInfo">
    </ul>
</body>
</html>
