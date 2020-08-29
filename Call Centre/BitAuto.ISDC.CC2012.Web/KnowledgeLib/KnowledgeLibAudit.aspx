<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeLibAudit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeLibAudit" %>

<%@ Register Src="UCKnowledgeLib/UCKnowledgeView.ascx" TagName="UCKnowledgeView"
    TagPrefix="uc1" %>
<%@ Register Src="UCKnowledgeLib/UCFAQView.ascx" TagName="UCFAQView"
    TagPrefix="uc1" %>
    <%@ Register Src="UCKnowledgeLib/UCKLOptionLogList.ascx" TagName="UCKLOptionLogList"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>知识点审核</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        //驳回理由弹出层
        function openRejectReasonPopup() {
            $.openPopupLayer({
                name: "RejectReasonPopup",
                parameters: { KLID: '<%=KID %>', r: Math.random(),Action:'auditpage' },
                popupMethod: 'Post',
                url: "RejectReason.aspx",
                success: function () {
                    $('body').prepend('<iframe id="pdfOverIframe" style="left: 200px; top: 200px; width: 1000px; height: 800px; position: absolute; z-index: 1;"></iframe>');
                },
                afterClose: function () {
                    $('#pdfOverIframe').remove();
                }
            });
        }
        function SingleAudit() {
            $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'approvalknowledgelib', 'KLID': '<%=KID %>', 'random': Math.random() }, function (data) {
                $('body').prepend('<iframe id="pdfOverIframe" style="left: 200px; top: 200px; width: 1000px; height: 800px; position: absolute; z-index: 1;"></iframe>');
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
//                    $.jAlert(jsonData.msg, function () {
//                        $('#pdfOverIframe').remove();
//                        window.close();
                    //                    });
                    $.jAlert(jsonData.msg, function () {
                        $('#pdfOverIframe').remove();
                        window.close();
                    });
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980 browse">
        <div class="taskT">
            查看知识点</div>
        <div class="addzs">
            <uc1:UCKnowledgeView ID="UCKnowledgeView1" runat="server" />
            <!--FAQ开始-->
            <uc1:UCFAQView ID="UCFAQView1" runat="server" />
            <!--FAQ结束-->
            <!--试题开始-->
  <asp:PlaceHolder ID="phQuestion" runat="server"></asp:PlaceHolder>
            <!--试题结束-->
            <div class="btn zsdbtn">
             <%if (status == "1")
               { %>
                <input type="button" name="" value="审核通过" class="btnSave bold" onclick="SingleAudit()"/>&nbsp;&nbsp;
                <input type="button" name="" value="驳 回" class="btnSave bold" onclick="openRejectReasonPopup()" />&nbsp;&nbsp;
                <%} %>
            </div>
            <uc2:UCKLOptionLogList ID="UCKLOptionLogList1" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
