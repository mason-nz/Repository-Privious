<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperView.aspx.cs"
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

        ul,li{ list-style-type: none;}               .addzs { margin:20px;}        .addzs ul li { line-height:35px;}        .addzs ul {  background:#fff; padding:5px 0;}        .addzs ul li label { float:left; width:100px; text-align:right;  font-size:12px; font-weight:bold;}        .addzs ul li textarea { width:800px; height:50px;}        .addzs ul li .w700{ width:700px;}        .addzs .title { margin:10px 0 0px 30px;}                .addzs ul li .addst2 a  { margin-right:130px; *margin-top:25px;}        .addzs ul li .addst3  { display:block; clear:none; width:200px; margin-top:4px; *margin-top:-25px;margin-right:0px; margin-right:45px; }        .addzs ul li .addst3 span{ margin-right:0px;}                                                                                                                                      .addzs  input.addbtn { line-height:24px;}        .addzs ul li.xzt { line-height:30px; }        .addzs ul li.xzt input { height:25px; line-height:30px;}        .addzs ul li ul { margin-left:100px; margin-top:0px;}        .addzs ul li .conrect { margin-left:100px;}        .addzs .zsdbtn { margin: 10px; margin-left:150px; *margin-left:100px;}        .addzs ul li.xzt textarea { width:535px; vertical-align:top;}          .addzs ul li.attach { clear:both;}        .addzs ul li.attach  label {width:100px; float:left;clear:none;}        .addzs ul li.attach .attc {width:850px; float:left; clear:none; margin:0; padding:0;}        .addzs ul li.attach .attc li { margin-right:15px; float:left; clear:none; }        .addzs ul li.attach .attc li a {text-overflow:ellipsis; white-space:nowrap;overflow:hidden;  width:330px;}          .addzs ul li .zsContent { float:left; clear:none; width:800px; line-height:25px; margin-top:4px;}        .faq ul,.st{ border-bottom:#CCC 1px dotted;}        .faq ul li.answer { margin-left:35px;}        .czRecord table.RocordL { background:#f6f6f6;}        .czRecord table.RocordL tr td { background:#FFF;}        table.RocordL { padding:0 20px;}          .st        {            display: block;            clear: both;        }        .st ul li        {            line-height: 25px;        }        .st ul li.xzt3, .st ul li.xzt2        {            margin: 8px 0 8px 0;        }        .st ul li.xzt3 ul, .st ul li.xzt2 ul        {            padding: 0;            margin: 5px 0 0 50px;        }        /*add time 2012-12-02*/        .st ul li.xzt3 ul li em, .st ul li.xzt2 ul li em        {            margin-left: 30px;            color: #999;        }        .st li.chooseXx        {            color: #06C;        }        .st ul        {            background: none;        }        .st ul:hover        {            background: #F2FBFF;        }        .sjbt {float:left; clear:none; width:800px; font-weight:bold;}
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
