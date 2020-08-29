﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage.ExamPaperView" %>

<%@ Register Src="~/ExamOnline/UCExamOnline/ExamPaperView.ascx" TagName="ExamPaperView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="htmlMain" runat="server">
<head runat="server" id="bodyHead">
    <title>试卷查看</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tbtnPDF = '#btnPDF';
        var epid = '<%=RequestEPID %>';
        $(function () {

            $(tbtnPDF).click(function () {                
                var href = "ExamPaperPDF.aspx?epid=" + epid;
                window.location.href = href;
     
            });
           // setTimeout(function () { $('#btnPDF,#btnPDF2').show(); }, 1000);

            $("#btnPDF2").click(function () {
                var href = "ExamPaperPDF.aspx?paper=1&epid=" + epid;
                window.location.href = href;
            });
        });
    </script>
    <style type="text/css">
        
         .examBt1 { text-align:center; font-size:24px; margin:20px 0 10px 0;}
         .examT { line-height:40px;}
        .examT span { font-weight:normal;}

        ul,li{ list-style-type: none;}       
        .faq .answer2 textarea { margin-left:30px;height: 62px;width: 800px; }
    </style>
</head>
<body runat="server" id="bodyMain">    
    <button title="导出页面PDF" id="btnPDF" style="display: none;">
        导出页面PDF
    </button>
    <button title="导出字纸试卷" id="btnPDF2" style="display: none;">
        导出字纸试卷
    </button>
    <div class="w980">
        <%-- <button id="btnPdfT">
            Import PDF</button>--%>
        <div class="taskT">
            试卷查看</div>
        <div class="examBt1">
            <b style="font-family: 宋体; font-size: 20px" iname="1">
                <%=ExamperName%></b></div>
        <div class="addzs">
            <uc1:ExamPaperView ID="ExamPaperView1" runat="server" />
        </div>
    </div>
</body>
</html>