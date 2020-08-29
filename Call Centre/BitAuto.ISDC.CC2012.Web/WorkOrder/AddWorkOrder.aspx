<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWorkOrder.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.AddWorkOrder" %>

<%@ Register Src="UControl/DealerControl.ascx" TagName="DealerControl" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加工单</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("controlParams");
        loadJS("CTITool");
        loadJS("ucCommon");
    </script>
    <%--<script src="/WorkOrder/UControl/ucCommon.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/Js/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        //type=1：添加；type=2：转出
        function submitAdd(type) {
            var jsonValidate = ucDealer_Validate(type);

            if (jsonValidate.result == "false") {
                $.jAlert(jsonValidate.msg, function () {
                    return false;
                });
            }
            else {
                if ($.jConfirm("是否确认提交？", function (r) {

                    if (r) {

                        //开始
                        $.blockUI({ message: '正在处理，请等待...' });

                        var jsonData = ucDealer_Submit(type);

                        $.unblockUI();
                        if (jsonData.result == "false") {
                            $.jAlert(jsonData.msg, function () {
                                return false;
                            });
                        }
                        else {
//                            $.jAlert("操作成功！", function () {
//                                closePageExecOpenerSearch("btnsearch");
//                            });
                            $.jPopMsgLayer("操作成功！", function () { closePageExecOpenerSearch("btnsearch"); });
                        }
                        //结束
                    }
                }));
            }
        }

        $(function () {
            uc_DealerInit();
        });
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            添加工单<span></span></div>
        <div class="baseInfo" style="margin-top: 20px; padding-bottom: 20px;">
            <uc1:DealerControl ID="DealerControl1" runat="server" />
            <!--按钮-->
            <div class="btn" style="margin: 10px; margin-left: 150px; *margin-left: 100px;">
                <input type="button" name="add" value="添 加" class="btnCannel bold" onclick="submitAdd(1)" />&nbsp;&nbsp;
                <input type="button" name="turn" value="转 出" class="btnCannel bold" onclick="submitAdd(2)" />
            </div>
            <!--按钮-->
        </div>
    </div>
    </form>
</body>
</html>
