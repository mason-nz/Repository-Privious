<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery.1.11.3.min.js" type="text/javascript"></script>
</head>
<body>
    <form action="http://www.chitunion.com/api/Materiel/DownloadZip" method="post" target="_blank">
        <p>
            <input type="text" name="ChannelIds" value="10,11" />
        </p>
        <p>
            <input type="text" name="MaterielId" value="3" />
        </p>
        <input type="submit" value="Submit" />
    </form>

    <a href="javascript:;" id="postDownloadZip">下载图片post 表单</a>

    <a href="http://www.chitunion.com/api/Template/DownloadZip?ids=1,2,3,4">下载图片zip</a>
    <a href="javascript:;" id="toMiddle">下载图片222</a>
    <iframe id="middle"></iframe>
    <script type="text/javascript">
        $(function () {
            //loadajax();

            //load();

            $("#postDownloadZip").on("click", function () {
                $("form").submit();
            });

            $("#toMiddle").on("click", function () {
                $("#middle").attr('src', 'http://www.chitunion.com/UploadFiles/2017/7/24/14/A.png');
                //load();
            })
        });

        function loadajax(bussinesstype) {
            $.ajax({
                type: "GET",
                url: "/Publish/BackGQuery?pageIndex=1&pageSize=4&bussinesstype=" + bussinesstype,
                data: {},
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    console.log(JSON.stringify(data));
                }
            });
        }

        function queryFront(bussinesstype, orderBy) {
            $.ajax({
                type: "GET",
                url: "/Publish/Query?pageIndex=1&pageSize=4&bussinesstype=" + bussinesstype + "&orderBy=" + orderBy,
                data: {},
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    console.log(JSON.stringify(data));
                }
            });
        }

        function load() {

            //{"list":[{"url":"ddd"}]}
            var datadd = {
                "list": [
                    {
                        "url": "http://www.chitunion.com/UploadFiles/2017/7/24/14/A.png"
                    }
                ]
            }

            $.ajax({
                type: "get",
                url: 'http://www.chitunion.com/api/Template/DownloadZip',
                data: { ids: "1,2,3,4" },
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                dataType: "json",
                success: function (data) {
                    console.log(data)

                },
                error: function (msg) {
                    console.log(msg)
                }
            });
        }
    </script>
</body>
</html>