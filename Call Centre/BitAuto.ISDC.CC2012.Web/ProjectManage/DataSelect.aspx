<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataSelect.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.DataSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <link href="../Css/uploadify.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pop ul li label
        {
            width: 70px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            ///根据类型ID,显示隐藏CRM选择和导入
            var ProjectType = '<%=ProjectType %>';
            ChangeRdo(ProjectType);
            getUserGroup(); //绑定业务组
            //将项目编辑页面的业务组ID和分类ID 带过来
            var BGID = '<%=BGID %>';
            if (BGID != "" && BGID != "-1") {
                $("#selGroup").val(BGID);
            }
            selGroupChange();
            var CID = '<%=CID %>';
            if (CID != "" && CID != "-1") {
                $("#selCategory").val(CID);
            }
            selCategoryChange();
            var BGName = '<%=BGName %>';
            var CName = '<%=CName %>';
            $("#selCrmGroup option").text(BGName);
            $("#selCrmCategory option").text(CName);
            //导入模板切换
            $("#selModel").change(function () {
                ChangeTemplate();
            });
            $("#uploadify").uploadify({
                'buttonText': '选择',
                'auto': false,
                'swf': '/Js/uploadify.swf',
                'uploader': '../AjaxServers/ProjectManage/CustDataInput.ashx?v=' + Math.random(),
                'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
                'multi': false,
                'fileSizeLimit': '5MB',
                'queueSizeLimit': 10,
                'uploadLimit': 10,
                'method': 'post',
                'successTimeout': 900,
                'removeTimeout': 1,
                'fileTypeDesc': '支持格式:xls.',
                'fileTypeExts': '*.xls;',
                'width': 79,
                'height': 26,
                'onUploadSuccess': function (file, data, response) {
                    $("#btnImport").removeAttr("disabled");
                    $("#imgBusy").hide();
                    var jsonData = $.evalJSON(data);
                    if (jsonData.success == false) {
                        $.jAlert("导入失败!<br>" + jsonData.message);
                    }
                    else {
                        var count = $(jsonData.message.split(',')).length;
                        $("#hidCrmSelectIDs").val("");
                        $("#hidExportSelectIDs").val(jsonData.message);
                        $("#hidSelectIDsCount").val(count);
                        if (jsonData.result != "") {
                            $.jAlert(jsonData.result);
                        }
                        $.closePopupLayer('SelectDataPopup', true);
                    }
                },
                'onQueueComplete': function (queueData) {
                }
            });
            CrmUploadInfo();
        });

        //切换数据来源的单选按钮
        function ChangeRdo(sourceID) {
            if (sourceID == 1) {
                $("#liExport").hide();
                $("#liCRM").show();
            }
            else if (sourceID == 2) {
                $("#liCRM").hide();
                $("#liExport").show();
            }
        }

        //绑定业务组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupNoRightByLoginUserID", r: Math.random() }, null, function (data) {

                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selModel option[value!='-1']").remove(); //清除模板名称
            ChangeTemplate();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#selGroup").val() != "-1") {
                AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selGroup").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }

        //根据选择的分类绑定对应的模板
        function selCategoryChange() {
            var ProjectType = '<%=ProjectType %>';
            $("#selModel").children().remove();
            $("#selModel").append("<option value='-1'>请选择模板</option>");
            if ($("#selCategory").val() != "-1") {
                AjaxPostAsync("/AjaxServers/TemplateManagement/GetTpageByCID.ashx", { Action: "GetTPage", CID: $("#selCategory").val(), r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {

                        if (ProjectType == 2) {
                            //   其他
                            if (jsonData[i].RecID != '1' && jsonData[i].RecID != '2') {
                                $("#selModel").append("<option value='" + jsonData[i].RecID + "' href='" + jsonData[i].GenTempletPath + "' ttcode='" + jsonData[i].TTCode + "'>" + jsonData[i].TPName + "</option>");
                            }
                        }
                    }
                });
            }
        }

        ///选择模板时，记录TypeID，TTCode, Excel链接地址
        function ChangeTemplate() {
            var cartypeid = $("#selModel").val();
            $("#hidCarType").val(cartypeid);
            if (cartypeid != -1) {
                var selectOption = $("#selModel").find("option:selected");
                var ttcode = $(selectOption).attr("ttcode"); //TTCode
                if (ttcode != "") {
                    $("#hidttcode").val(ttcode);
                    $("#hidTTCode").val(ttcode);
                }
                else {
                    $("#hidttcode").val("");
                    $("#hidTTCode").val(ttcode);
                }

                //生成文件
                var pody = {
                    Action: "SaveExcelTemplate",
                    recid: cartypeid,
                    ttCode: ttcode
                };
                AjaxPost('/AjaxServers/TemplateManagement/GenerateTemplate.ashx', pody, null, function () {
                    var fileUrl = $(selectOption).attr("href"); //模板Excel路径
                    if (fileUrl != "") {
                        $("#linkFiles").show();
                        $("#linkFiles").text($(selectOption).text());
                        $("#linkFiles").attr("href", fileUrl);
                    }
                    else {
                        $("#linkFiles").hide();
                        $("#linkFiles").text("");
                        $("#linkFiles").attr("href", "");
                    }
                });
            }
            else {
                $("#hidttcode").val("");
            }
        }

        function disableConfirmBtn() { $('#btnCrmImput').attr('disabled', 'disabled'); }
        function enableConfirmBtn() { $('#btnCrmImput').removeAttr('disabled'); }

        function CrmUploadInfo() {
            $("#uploadify2").uploadify({
                'buttonText': '选  择',
                'swf': '/Js/uploadify.swf?_=' + Math.random(),
                'uploader': '/ProjectManage/CrmCustImport/Handler.ashx',
                'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
                'auto': false,
                'multi': false,
                'width': 78,
                'successTimeout': 600,
                'height': 25,
                'fileTypeDesc': '支持格式:xls.',
                'fileTypeExts': '*.xls;',
                'queueSizeLimit': 1,
                'scriptAccess': 'always',
                'onQueueComplete': function (event, data) {
                    enableConfirmBtn();
                },
                'onQueueFull': function () {
                    $.jAlert('您最多只能上传1个文件！');
                    return false;
                },
                'onUploadSuccess': function (file, data, response) {
                    $("#imgBusy2").hide();
                    if (response == true) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.success) {
                            var list = new Array(); //定义一数组
                            list = jsonData.message.split(",");
                            $("#hidExportSelectIDs").val(""); //清除导入的
                            $("#hidCrmSelectIDs").val(jsonData.message);
                            $("#hidSelectIDsCount").val(list.length);
                            $.closePopupLayer('SelectDataPopup', true);
                        }
                        else {
                            if (jsonData.message) {
                                try {
                                    var mobj = $.evalJSON(unescape(jsonData.message));
                                    var errormsg = "";
                                    $.each(mobj, function (i, n) {
                                        errormsg = errormsg + n.Infomation + "<br/>";
                                    });
                                    $('#SpanMsg').show().html("导入不成功!<br/>" + errormsg);
                                } catch (e) {
                                    $('#SpanMsg').show().text("导入不成功!" + unescape(jsonData.message));
                                }
                            }
                        }
                    }
                    else {
                        alert(data);
                    }
                },
                'onProgress': function (event, queueID, fileObj, data) { },
                'onUploadError': function (event, queueID, fileObj, errorObj) {
                    enableConfirmBtn();
                    $("#imgBusy2").hide();
                }
            });
        };

    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmBatchImport() {
            var uploadify = $('#uploadify');
            var msg = '';
            var queueSize = uploadify.uploadify('queueLength');
            if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }
            if (queueSize > 1) { $.jAlert('只能导入一个文件'); return; }
            var carTypeid = $("#hidCarType").val();
            if (carTypeid == '-1') {
                $.jAlert('请选择导入模板'); return;
            }
            var ttcode = $("#hidttcode").val();
            var loginID = $("#hidLoginUserID").val();
            var ProjectID = $("#hidProjectID").val();
            //设置传递的数据
            uploadify.uploadify('settings', 'formData', {
                Action: 'BatchImport',
                cartype: carTypeid,
                ttcode: ttcode,
                UserID: loginID,
                ProjectID: ProjectID,
                IsBlacklistCheck: '<%=IsBlacklistCheck %>',
                BlackListCheckType: '<%=BlackListCheckType %>',
                LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>'))
            });
            $("#imgBusy").show();
            $("#btnImport").attr("disabled", "disabled");
            //执行上传
            uploadify.uploadify('upload', '*');
        }

        function CRMCustImport() {
            var uploadify = $('#uploadify2');
            var msg = '';

            var queueSize = uploadify.uploadify('queueLength');
            if (queueSize == 0) { $.jAlert('请选择要导入的文件'); return; }

            disableConfirmBtn();

            //设置传递的数据
            uploadify.uploadify('settings', 'formData', {
                Action: 'BatchImport',
                userid: '<%=LoginUserID %>',
                IsBlacklistCheck: '<%=IsBlacklistCheck %>',
                BlackListCheckType: '<%=BlackListCheckType %>',
                LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>'))
            });
            $("#imgBusy2").show();

            uploadify.uploadify('upload', '*')
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdBGID" />
    <div class="pop pb15 openwindow" style="width: 780px; clear: both; overflow: auto;
        overflow-x: hidden">
        <div class="title bold">
            <h2>
                选择数据</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectDataPopup',false);">
            </a></span>
        </div>
        <ul class="clearfix ft14" id="addTemplatePage">
            <li id="liCRM" style="border: 1px solid #DDDDDD; width: 100%;">
                <ul class="clearfix  outTable">
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            请选择导入模板：</label>
                        <select id="selCrmGroup" disabled="disabled" style="width: 150px;">
                            <option value="1">请选所属分组</option>
                        </select>
                        <select id="selCrmCategory" disabled="disabled" style="width: 150px;">
                            <option value="2">客户回访</option>
                        </select>
                    </li>
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            已选择模板：</label>
                        <a href="/ProjectManage/CrmCustImport/Templet/CRM客户导入模版.xls" id="A1">导入客户ID </a>
                    </li>
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            导入Excel：</label>
                        <div style="width: 300px; padding-left: 160px; *padding-left: 0px;">
                            <input type="file" id="uploadify2" name="uploadify" runat="server" style="float: left" /></div>
                        <ul class="clearfix" style="text-align: center;" id="Ul2">
                            <li>
                                <input type="button" onclick="javascript:CRMCustImport();" class="btnChoose" id="btnCrmImput"
                                    value="导入" style="margin-right: 18px;" />
                                <div>
                                    <span id="imgBusy2" style="display: none;">
                                        <img src="../Images/blue-loading.gif" />正在导入，请稍候...</span> <span id="SpanMsg" style="color: #ff0000;
                                            display: none; width: 100%; margin-left: 10px;">正在导入EXCEL数据……</span>
                                </div>
                        </ul>
                    </li>
                </ul>
            </li>
            <li id="liExport" style="display: none; border: 1px solid #DDDDDD; width: 100%;">
                <ul class="clearfix  outTable">
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            请选择导入模板：</label>
                        <select id="selGroup" disabled="disabled" style="width: 150px;" onchange="javascript:selGroupChange()">
                            <option value="1">请选所属分组</option>
                        </select>
                        <select id="selCategory" disabled="disabled" style="width: 150px;" onchange="javascript:selCategoryChange()">
                            <option value="2">请选择分类</option>
                        </select>
                        <select id="selModel" style="width: 150px;">
                            <option value="-1">请选择模板</option>
                        </select>
                    </li>
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            已选择模板：</label>
                        <a href="" id="linkFiles" style="display: none;">4S客户模板 </a></li>
                    <li style="width: 680px;">
                        <label style="width: 150px;">
                            导入Excel：</label>
                        <div style="width: 300px; padding-left: 160px; *padding-left: 0px;">
                            <input type="file" id="uploadify" name="uploadify" runat="server" style="float: left" /></div>
                        <ul class="clearfix" style="text-align: center;" id="Ul1">
                            <li>
                                <input type="button" onclick="javascript:ConfirmBatchImport();" class="btnChoose"
                                    id="btnImport" value="导入" style="margin-right: 18px" />
                                <div>
                                    <span id="imgBusy" style="display: none;">
                                        <img src="../Images/blue-loading.gif" />正在导入，请稍候...</span></div>
                                <input type="hidden" id="hidLoginUserID" value='<%=LoginUserID %>' />
                                <input type="hidden" id="hidCarType" value="-1" />
                                <input type="hidden" id="hidttcode" value="0" />
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
