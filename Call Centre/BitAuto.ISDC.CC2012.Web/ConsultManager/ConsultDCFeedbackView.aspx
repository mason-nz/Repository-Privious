<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultDCFeedbackView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ConsultManager.ConsultDCFeedbackView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>经销商反馈</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul class="clearfix">
            <li>
                <label>
                    咨询类型：</label>
                <span id="ConsultID" runat="server" /></li>
            <li>
                <label>
                    记录类型：</label>
                <span id="RecordType" runat="server" /></li>
            <li>
                <label>
                    问题类别：</label>
                <span id="QuestionType" runat="server" /></li>
            <li>
                <label>
                    来电渠道：</label>
                <span id="CallerCategory" runat="server" /></li>
            <li id="liQuestionContentID" runat="server" visible="false">
                <label>
                    问题内容：</label>
                <span id="QuestionContent" runat="server" /></li>
        </ul>
        <div class="line">
        </div>
        <ul class="clearfix">
            <li style="width: 700px;">
                <label>
                    来电记录：</label>
                <span id="CallRecord" runat="server" class="exceed" style="width: 550px;" />
            </li>
            <li>
            <label>
                录音记录：</label> <%=ShowCallRecord()%></li>
        </ul>
    </div>
    </form>
</body>
</html>
