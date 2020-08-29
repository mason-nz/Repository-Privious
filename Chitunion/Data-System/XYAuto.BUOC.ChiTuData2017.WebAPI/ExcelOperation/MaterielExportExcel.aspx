<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterielExportExcel.aspx.cs" Inherits="XYAuto.BUOC.ChiTuData2017.WebAPI.ExcelOperation.MaterielExportExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <script src="../Content/layer/jquery.1.11.3.min.js"></script>
    <script src="../Content/layer/layer.js"></script>
    <script>

        function MessageBox(mes) {
            layer.msg(mes, { 'time': 1000 }, function () {
                history.go(-1);
            });
        }
    </script>
   
</head>
<body>
    <form id="form1" runat="server">
        <div>
           
        </div>
    </form>
</body>
</html>
