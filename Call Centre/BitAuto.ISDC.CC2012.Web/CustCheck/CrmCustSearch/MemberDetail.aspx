<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberDetail.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustSearch.MemberDetail" %>

<%@ Register Src="../../CustInfo/DetailH/UCMemberDetail.ascx" TagName="UCMemberDetail"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRM会员详细信息</title>
    <link href="../../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/cc_list.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script type="text/javascript">
        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            易湃会员信息 <span></span>
        </div>
        <div class="baseInfo">
        <uc1:UCMemberDetail ID="UCMemberDetail1" runat="server" />
        </div>
    </div>
    </form>
    <script type="text/javascript" src="../../Js/GMapTool.js"></script>
    <script type="text/javascript">
//        $(function () {
//            $.getScript("http://maps.google.com/maps/api/js?sensor=false&callback=ucMemberDetailHelper.showMap");
//        });
    </script>
</body>
</html>
