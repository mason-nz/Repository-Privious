<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalTaskProcess.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.ExternalTaskProcess" %>
    <%@ Register Src="../CustBaseInfo/UCCustBaseInfo/ViewCustBaseInfo.ascx" TagName="ViewCustBaseInfo"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(document).ready(function () {
            $('img#baseinfo').toggle(
		        function () {
		            $(this).attr({ src: "../images/collapsed_yes.gif", title: "收起" }).parent().parent().parent().next().show("normal");
		        },
			    function () {
			        $(this).attr({ src: "../images/collapsed_no.gif", title: "展开" }).parent().parent().parent().next().hide("normal");
			    }
            );
            $('img#historyinfo').toggle(
		        function () {
		            $(this).attr({ src: "../images/collapsed_no.gif", title: "展开" }).parent().parent().parent().next().hide("normal");
		        },
			    function () {
			        $(this).attr({ src: "../images/collapsed_yes.gif", title: "收起" }).parent().parent().parent().next().show("normal");
			    }
            );

            getConsultInfo();

            loadProcessDetail();
        });

        //加载联系记录中咨询类型表信息
        function getConsultInfo() {
            var consultDataID = "<%=RequestConsultDataID %>";
            var pody = "RecID=" + consultDataID;
            switch ("<%=RequestConsultID %>") {
                case "60001": loadConsultHtml("ConsultNewCarView.aspx", pody);
                    break;
                case "60002": loadConsultHtml("ConsultSecondCarView.aspx", pody);
                    break;
                case "60003": loadConsultHtml("ConsultPFeedbackView.aspx", pody);
                    break;
                case "60004": loadConsultHtml("ConsultActivityView.aspx", pody);
                    break;
                case "60005": loadConsultHtml("ConsultPUseCarView.aspx", pody);
                    break;
                case "60006": loadConsultHtml("ConsultPOtherView.aspx", pody);
                    break;
                case "60007":
                case "60008":
                case "60009":
                    var podyType = "";
                    if ("<%=RequestConsultID %>" == "60007") {
                        podyType = "&Type=1";
                    }
                    else if ("<%=RequestConsultID %>" == "60008") {
                        podyType = "&Type=2";
                    }
                    else if ("<%=RequestConsultID %>" == "60009") {
                        podyType = "&Type=3";
                    }
                    loadConsultHtml("ConsultDCoopView.aspx", pody + podyType);
                    break;
            }
        }

        //根据对应页面加载7个表中的一个页面
        function loadConsultHtml(aspx, pody) {
            $("#ConsultHtml").load("../ConsultManager/" + aspx, pody);
        }

        //加载处理记录
        function loadProcessDetail() {
            var pody = 'TaskID=<%=RequestTaskID %>&r=' + Math.random();
            $("#ajaxTable").load("../AjaxServers/TaskManager/TaskProcessDetail.aspx", pody);
        }

        //员工姓名弹出层
        function openSelectEmployeePopup() {
            $.openPopupLayer({
                name: "SelectEmployeePopup",
                parameters: { DepartID: -1, Eid: -1, r: Math.random() },
                popupMethod: 'Post',
                url: "SelectEmployees.aspx",
                beforeClose: function () {
                    var eid = $('#popupLayer_' + 'SelectEmployeePopup').data('EID');

                    if (eid != null) {
                        $("#employeeID").val(eid);
                        $("#domainAccount").val($('#popupLayer_' + 'SelectEmployeePopup').data('DomainAccount'));
                        $("#employeeName").val($('#popupLayer_' + 'SelectEmployeePopup').data('CnName'));
                    }
                }
            });
        }
        //提交
        function btnSubmit() {
            var isComplaint = $("#chkIsComplaint").attr("checked");
            var comment = $("#txtComment").val();
            if (comment == "") {
                $.jAlert("处理意见必填！");
                return false;
            }
            var status = $("[name='rdoProcess']:checked").val();
            if (status == "undefined" || status == "" || status == null) {
                $.jAlert("处理状态必填！");
                return false;
            }
            var toNextSolveUserEID = $("#employeeID").val();
            $.post
            (
                '../AjaxServers/TaskManager/TaskManager.ashx',
                { 'Action': 'CustHistoryLogSubmit', 'IsComplaint': isComplaint, 'CHITaskID': '<%=RequestTaskID %>',
                    'CHLStatus': status, 'CHIRecID': '<%=RequestCHIRecID %>', 'Comment': comment, 'ToNextSolveUserEID': toNextSolveUserEID, 'random': Math.random()
                },
                    function (data) {
                        var jsonData = $.evalJSON(data);
                        alert(jsonData.msg);
                        window.location.reload();
                    }
            )
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrap">
        <div class="baseInfo mt10 mb15 pb15">
            <div class="title pt5 bold ft16">
                基本信息<span><a href="#" class="right"><img src="../images/collapsed_no.gif" width="12"
                    height="10" id="baseinfo" /></a></span>
            </div>
            <div style="display: none">
                <uc1:viewcustbaseinfo id="ViewCustBaseInfo1" runat="server" />
            </div>
            <div class="title pt5 bold ft16 mt18 contact">
                联系记录<span><a href="#" class="right"><img src="../images/collapsed_yes.gif" width="12"
                    height="10" id="historyinfo" /></a></span></div>
            <div id="ConsultHtml">
            </div>
            <div class="line">
            </div>
            <ul class="clearfix ft14" runat="server" id="ulProcess">
                <li>
                    <label>
                        问题性质：</label><span id="QuestionQuality" runat="server" /></li>
                <li>
                    <label style="width: 126px;">
                        最晚处理时间:</label><span id="LastTreatmentTime" runat="server" /></li>
            </ul>
            <div class="deal" id="divDeal" runat="server">
                <label class="bold">
                    填写处理意见<span class="redColor">*</span></label>
                <textarea name="" cols="" rows="" id="txtComment"></textarea>
            </div>
            <ul class="clearfix ft14" runat="server" id="liNextSolveUser">
                <li>
                    <label>
                        选择处理状态<span class="redColor">*</span>：</label>
                    <input type="radio" value="110001" id="rdoProcessSolve" name="rdoProcess" />已解决
                    <input type="radio" value="110003" id="rdoProcessUnresolved" name="rdoProcess" />未解决
                    <input type="radio" value="110002" id="rdoProcessNotSolve" name="rdoProcess" />不解决</li>
                <li>
                    <label style="width: 126px">
                        选择下一级受理人：</label>
                    <input style="width: 120px; padding: 4px 2px 0;" type="text" id="employeeName" value=""
                        class="w125" readonly="readonly" />
                    <input id="employeeID" style="display: none;" />
                    <input id="domainAccount" style="display: none;" />
                    <a href="javascript:void(0);" onclick="javascript:openSelectEmployeePopup();" style="text-decoration: none">
                        <input name="" type="button" value="选择" class="btnChoose bold" /></a> </li>
                <li>
                    <label>
                        是否确定投诉：</label><input type="checkbox" value="true" runat="server" id="chkIsComplaint" />是</li>
                <li><span id="spanBtnSubmit" runat="server">
                    <input name="" type="button" value="提交" class="btnChoose bold" onclick="javascript:btnSubmit()" /></span>
                </li>
            </ul>
            <div class="deal">
                <label class="bold">
                    处理记录</label>
                <div class="tableList mt10" id="ajaxTable">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
