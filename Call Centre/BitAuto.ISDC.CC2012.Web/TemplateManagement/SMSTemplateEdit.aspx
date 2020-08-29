<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSTemplateEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.SMSTemplateEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑短信模板</title>
    <script type="text/javascript">

        $(function () {
            var bgid = '<%=BGID %>';
            var scid = '<%=SCID %>';
            getUserGroup1();
            if (bgid != '') {
                $("#selGroup1").val(bgid);
            }
            selGroupChange1();
            if (scid != '') {
                $("#selCategory1").val(scid);
            }

        });


        //加载登陆人业务组
        function getUserGroup1() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#selGroup1").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup1").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange1() {
            $("#selCategory1").children().remove();
            $("#selCategory1").append("<option value='-1'>请选择分类</option>");
            if ($("#selGroup1").val() != "-1") {
                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selGroup1").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory1").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }
        function Submit() {
            var bgid = $("#selGroup1").val();
            var scid = $("#selCategory1").val();
            var smstitle = $("#txtTemplateTitle").val();
            var smscontent = $("#txtContent").val();

            if (bgid == "" || bgid == "-1") {
                $.jAlert("请选择分组！");
            }
            else if (scid == "" || scid == "-1") {
                $.jAlert("请选择分类！");
            }
            else if (smstitle == "") {
                $.jAlert("请输入标题！");
            }
            else if (smscontent == "") {
                $.jAlert("请输入内容！");
            }
            else if (Len(smstitle) > 100) {
                $.jAlert("标题超长！");
            }
            else if (Len(smscontent) > 500) {
                $.jAlert("内容超长！");
            }
            else {
                var url = "/AjaxServers/TemplateManagement/SMSTemplateEdit.ashx";
                $.blockUI({ message: '正在执行，请等待...' });
                var recid = '<%=RequestRecID%>';
                var param;
                if (recid != "") {
                    param = { Action: "Edit", RecID: recid, BGID: bgid, SCID: scid, SMSTitle: escape(smstitle), Content: escape(smscontent), r: Math.random() };
                }
                else {
                    param = { Action: "ADD", BGID: bgid, SCID: scid, SMSTitle: escape(smstitle), Content: escape(smscontent), r: Math.random() };
                }
                $.post(url, param, function (data) {
                    $.unblockUI();
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "true") {
                        $.jPopMsgLayer("操作成功!", function () {
                            $.closePopupLayer('updateSMSTemplate', false);
                            search();
                        });
                    }
                    else {
                        $.jAlert(jsonData.msg);
                    }
                });
            }
        }
    </script>
</head>
<body>
    <div class="pop pb15 editbq">
        <div class="title bold">
            编辑模板<span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('updateSMSTemplate',false);">
            </a></span>
        </div>
        <ul id="tagEditUL" class="clearfix ">
            <li>
                <label>
                    <span class="redColor">*</span>所属分组：</label><select id="selGroup1" class="w125" onchange="javascript:selGroupChange1()">
                    </select>&nbsp;&nbsp;<select id='selCategory1' class="w165">
                    </select></li>
            <li>
                <label>
                    <span class="redColor">*</span>模板标题：</label><input type="text" style="width: 294px;" class="w280" value="<%=smstitle%>"
                        id="txtTemplateTitle"></li>
            <li>
                <label>
                    <span class="redColor">*</span>模板内容：</label><textarea style="width:284px" cols="" rows="5" name="" id="txtContent"><%=smscontent%></textarea></li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" onclick="Submit()" class="btnSave bold" value="保 存" name="" />
            <input type="button" class="btnCannel bold" onclick="javascript:$.closePopupLayer('updateSMSTemplate',false);"
                value="取 消" name="" /></div>
    </div>
</body>
</html>
