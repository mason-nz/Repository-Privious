<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddConSentence.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.AddConSentence" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title></title>
<script type="text/javascript">
    function Save2DB() {
        var ltid;
        var conSentenceName;

        ltid = $("#sltLabel").val();

        if (ltid == "-1") {
            $.jAlert("请选择标签！");
            return;
        }

        conSentenceName = $("#textMemo").val();

        if (conSentenceName == "") {
            $.jAlert("请添写常用语！");
            return;
        }
        
        if (conSentenceName.length > 100) {
            $.jAlert("常用语不能超过100个字符！");
            return;
        }

        var csid = $("#hidCSID").val();
        if (csid == "") {
            //同一标签下常用语不能重复
            AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", { Action: "IsRepeatLableCS", LTID: ltid, Name: encodeURIComponent(conSentenceName), r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    //alert("jsonData.CSID:" + jsonData.CSID);
                    $.jAlert(jsonData.msg);
                    search();
                    $.closePopupLayer('AddConSentencePop', true);
                }
                else {
                    $.jAlert(jsonData.msg);
                }

            });
        }
        else {
            //编辑页
            AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", { Action: "IsEditCS", CSID: csid, LTID: ltid, Name: encodeURIComponent(conSentenceName), r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    //alert("jsonData.CSID:" + jsonData.CSID);
                    $.jAlert(jsonData.msg);
                    $.closePopupLayer('AddConSentencePop', true);
                }
                else {
                    $.jAlert(jsonData.msg);
                }

            });
        }        

    }

    $(document).ready(function () {
        var isEdit = 0;
        isEdit = $("#hidIsEditCS").val();

        if (isEdit == 1) {
            var ltid = $("#hidLTID").val();
            var csname = $("#hidCSName").val();
            $("#sltLabel").val(ltid);
            $("#textMemo").val(csname);
        }
    });
</script>
</head>
<!--常用语编辑-->
<div class="popup openwindow">
	<div class="title ft14"><h2>新增常用语</h2><span><a href="#" class="right" onclick="javascript:$.closePopupLayer('AddConSentencePop',false);"><img src="../images/c_btn.png" border="0"/></a></span></div>
    <div class="content">
        <ul>
        	<li><label>标签：</label><span><select id="sltLabel" runat="server" class="w100" style="width:210px;"><option></option></select></span></li>
            <li><label>常用语：</label><span><textarea id="textMemo"></textarea></span></li>
        </ul>
        <div class="clearfix"></div>
        <div class="btn"><input type="button"  value="保存" onclick="Save2DB()" class="save w60"/>&nbsp;&nbsp;&nbsp;&nbsp;<input type="button"  value="关闭" onclick="javascript:$.closePopupLayer('AddConSentencePop',false);" class="cancel w60 gray" /></div>
    </div>
</div>
</html>