<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QualityResultFiveLevelEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.QualityResultManage.QualityResultFiveLevelEdit" %>

<%@ Register Src="~/QualityStandard/UCQualityStandard/UCQualityStandardFiveLevelEdit.ascx"
    TagName="UCQualityStandardFiveLevelEdit" TagPrefix="uc1" %>
<%@ Register Src="../UCQualityStandard/UCCallRecordView.ascx" TagName="UCCallRecordView"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>录音质检评分</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" href="../../Css/base.css" rel="stylesheet" />
    <link type="text/css" href="../../Css/style.css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery-ui.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/anchor.1.0.js"></script>
    <script type="text/javascript">
        function OpenRecordingSharing() {
            $.openPopupLayer({
                name: "RecordingSharingPopup",
                //parameters: { Tel: ''},
                url: "RecordingSharing.aspx",
                beforeClose: function (e) {
                }
            });
        }
         
    </script>
 
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <%=TableName%></div>
        <uc2:UCCallRecordView ID="UCCallRecordView1" runat="server" />
        <uc1:UCQualityStandardFiveLevelEdit ID="QualityStandardEditID" runat="server" />
        <div class="btn" style="margin: 20px auto">
            <input type="button" value="保存" onclick="SaveQualityStandar();" name="" />
            <input type="button" value="提交" onclick="SubQualityStandar();"
                name="" />
            <input type="button" value="录音共享" onclick="OpenRecordingSharing()" name="" />
        </div>
    </div> 
    </form>
</body>
</html>
