<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustDetail.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustSearch.CustDetail" %>

<%@ Register Src="../../CustInfo/DetailH/UCCustDetail.ascx" TagName="UCCustDetail"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户信息</title>
    <link href="../../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/cc_list.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
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
        function OperationLogPop2(taskid) {
            $.openPopupLayer({
                name: "OperationLogPop",
                parameters: { TaskID: taskid },
                url: "/CustInfo/MoreInfo/StopCustTaskOperationLog.aspx",
                afterClose: function (e, data) {

                }
            });
        }
        function OperationLogPop(taskid) {
            $.openPopupLayer({
                name: "OperationLogPop",
                parameters: { TaskID: taskid },
                url: "/CustInfo/MoreInfo/StopCustOperationLog.aspx",
                afterClose: function (e, data) {

                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">客户信息
            <span></span>
        </div>
        <div class="baseInfo">
            <uc1:UCCustDetail ID="UCCustDetail1" runat="server" />
        </div>
    </div> 
    </form>
</body>
</html>
