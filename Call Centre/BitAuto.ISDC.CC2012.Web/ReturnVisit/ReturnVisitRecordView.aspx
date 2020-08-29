<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnVisitRecordView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.ReturnVisitRecordView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>访问记录查看</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/style.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/cc_checkStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/adtPopup.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#divCallRecordList').load('/ReturnVisit/CallRecordList.aspx', {
                ContentElementId: 'divCustContacts',
                RVID: '<%= this.RVID %>',
                PageSize: 10
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            回访信息</div>
        <div class="baseInfo clearfix">
            <ul class="clearfix ">
                <li>
                    <label>
                        客户名称：</label><asp:Label ID="lblCustName" Style="width: 300px; float: left; clear: none;"
                            runat="server"></asp:Label></li>
                <li>
                    <label>
                        客户ID：</label><asp:Label ID="lblCustID" runat="server"></asp:Label></li>
                <li>
                    <label>
                        会员名称：</label><asp:Label ID="lblMemberName" runat="server"></asp:Label></li>
                <li>
                    <label>
                        客户联系人：</label>
                    <asp:Label ID="lblLinkMan" runat="server"></asp:Label>
                <li>
                    <label>
                        访问分类：</label>
                    <asp:Label ID="lblVisitType" runat="server"></asp:Label></li>
                <li>
                    <label>
                        所属业务：</label>
                    <asp:Label ID="lblBussinesLine" runat="server"></asp:Label></li>
                <li>
                    <label>
                        访问人：</label>
                    <asp:Label ID="lblVisitPerson" runat="server"></asp:Label></li>
                <li>
                    <label>
                        访问日期：</label>
                    <asp:Label ID="lblVisitDate" runat="server"></asp:Label></li>
                <li style="width: 700px">
                    <label>
                        访问描述：</label>
                    <asp:Label ID="lblRemark" Style="width: 500px; float: left; clear: none;" runat="server"></asp:Label></li>
                <li style="width: 700px">
                    <label>
                        录音记录：</label>
                    <div id="divCallRecordList" class="fullRow cont_cxjg" style="margin-left: 78px;">
                    </div>
                </li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
