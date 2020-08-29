<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderViewPersonal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.WorkOrderViewPersonal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工单查看</title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../CTI/CTITool.js" type="text/javascript"></script>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            var CategoryID = '<%=CategoryID %>';
            if (CategoryID == '34' || CategoryID == '35') {
                $('#liOutCarCategory').css("display", "none");
            }
            else if (CategoryID == '36') {
                $('#liAcceptVisit').css("display", "none");
                $('#liRecommend').css("display", "none");
                $('#lblSelectDealerName').css("display", "none");
                $('#liSelectDealerName').css("display", "none");
            }
            else {
                $('#lblSelectDealerName').css("display", "none");
                $('#liCarCategory').css("display", "none");
                $('#liOutCarCategory').css("display", "none");
                $('#liAcceptVisit').css("display", "none");
                $('#liRecommend').css("display", "none");
                $('#liSelectDealerName').css("display", "none");
            } 
            //控制文本超长后换行
            $("#lblWorkOrderRecord").attr("style", "width:810px; float:left; clear:none;");
            $("#lblRecommend").attr("style", "width:345px; float:left; clear:none;");
        });
 
    </script>
</head>
<body>
    <div class="w980 gdcl">
        <div class="taskT">
            工单查看</div>
        <div class="content">
            <div class="gd_title">
                <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><span>（来源：<asp:Label
                    ID="lblDataSource" runat="server" Text=""></asp:Label>）</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><%=CustID%></span>
            &nbsp;&nbsp;&nbsp;&nbsp;<span><%=CHUrl%></span>
            </div>
            <div class="Contentbox8">
                <div id="con_one_1" class="hover">
                    <ul class="clearfix Info2" style="width: 940px">
                        <li>
                            <label>
                                工单分类：</label><asp:Label ID="lblCategory" runat="server" Text="" /></li>
                        <li id="liCarCategory">
                            <label>
                                关注车型：</label><asp:Label ID="lblCarCategory" runat="server" Text="" /></li>
                        <li id="liSelectDealerName">
                            <label>
                                推荐经销商：</label><asp:Label ID="lblSelectDealerName" runat="server" Text="" /></li>
                        <li id="liOutCarCategory">
                            <label>
                                出售车型：</label><asp:Label ID="lblOutCarCategory" runat="server" Text="" /></li>
                        <li id="liAcceptVisit">
                            <label>
                                接受回访：</label><asp:Label ID="lblAcceptVisit" runat="server" Text="" /></li>
                        <li id="liRecommend">
                            <label>
                                推荐活动：</label><asp:Label ID="lblRecommend" runat="server" Text="" /></li>
                        <li>
                            <label>
                                工单标签：</label><asp:Label ID="lblTag" runat="server" Text="" /></li>
                        <li>
                            <label>
                                添加人：</label><asp:Label ID="lblAddPerson" runat="server" Text="" /></li>
                        <li>
                            <label>
                                添加时间：</label><asp:Label ID="lblAddTime" runat="server" Text="" /></li>
                        <li style="width: 940px">
                            <label>
                                工单记录：</label><asp:Label ID="lblWorkOrderRecord" runat="server" Text="" /></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
