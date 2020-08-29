<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QiCheTongPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.QiCheTongPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#pop_Phone").val("<%=pop_Phone %>");
        });
        function pop_register() {
            var telPhone = $.trim($("#pop_Phone").val());

            if (telPhone == "") {
                $.jAlert("手机号码不能为空！");
                return false;
            }
            if (!isMobile(telPhone)) {
                $.jAlert("手机号码格式不正确！");
                return false;
            }

            registerByWebService(telPhone);
        }
        //调用web服务接口注册车商通
        function registerByWebService(phoneNumber) {
            AjaxPost("/AjaxServers/CustCategory/BuyCarInfo.ashx", { Action: "RegisterCarTong", PhoneNumber: encodeURIComponent(phoneNumber), TaskID: "<%=TaskID %>", r: Math.random() }, null,
        function (data) {
            var jsonData = eval("(" + data + ")");
            if (jsonData.result == 'yes') {
//                $.jAlert("注册成功", function () {
//                    //$("#txtUserName").val(jsonData.mobile + "/" + jsonData.pwd);
//                    //$("#btnRegister").hide(); //当汽车通注册成功时，注册 按钮隐藏
//                    $.closePopupLayer('QiCheTongPop', false);
                //                });
                $.jPopMsgLayer("注册成功", function () {
                    //$("#txtUserName").val(jsonData.mobile + "/" + jsonData.pwd);
                    //$("#btnRegister").hide(); //当汽车通注册成功时，注册 按钮隐藏
                    $.closePopupLayer('QiCheTongPop', false);
                });
                
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
        }
         
    </script>
    <style type="text/css">
        .pop .bh
        {
            margin: 5px 10px 10px;
        }
        .pop .bh textarea
        {
            width: 575px;
            height: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15" style="width: 400px;">
        <div class="title bold">
            注册汽车通 <a onclick="javascript:$.closePopupLayer('QiCheTongPop',false);" href="javascript:void(0)">
            </a>
        </div>
        <div class="popT bold" style="margin: 20px; margin-left: 60px; text-align: left;">
            <span class="redColor">*</span>电话号码
            <input id="pop_Phone" type="text" /></div>
        <div class="btn" style="width: 400px">
            <input type="button" onclick="javascript:pop_register();" class="btnSave bold" value="注册" />
            <input type="button" onclick="javascript:$.closePopupLayer('QiCheTongPop',false);"
                class="btnCannel bold" value="取消" />
        </div>
    </div>
    </form>
</body>
</html>
