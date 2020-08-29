<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordingSharing.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.QualityResultManage.RecordingSharing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script type="text/javascript">
    //验证知识点信息
    function ValidateKnow() {

        var msg = "";

        if ($.trim($("[id$=txtTitle]").val()) == "") {
            msg = "知识点标题不能为空!<br/>";
        }

        if ($.trim($("[id$=txtAbstract]").val()) == "") {
            msg = "知识点摘要不能为空!<br/>";
        }

        if ($.trim($("[id$=txtAbstract]").val()).length > 200 || $.trim($("[id$=txtAbstract]").val()).length < 100) {
            msg = msg + "知识点摘要的长度要大于100字并且小于200字<br/>";
        }

        if (Len($.trim($("[id$=txtTitle]").val())) > 40) {
            msg = msg + "知识点标题不能大于20个汉字或者40个字母!<br/>";
        }        
        if ($("#selKCID2").css("display") != "none" && $("#selKCID2").val() == "-1") {
            msg = msg + "请选择分类<br/>";
        }        
        return msg;
    }

    //获取知识点信息数据
    function GetKnowData() {

        var kcid;
        if ($("#selKCID2").val() != "-1") {
            kcid = $("#selKCID2").val();
        }        
        else {
            kcid = "null";
        }

        var fileUrl = $("#HideFIleUrl").val();

        var knowData = {  
            Action:'RecordingSharingSave',          
            KCID: escape(kcid),
            Title: escape($.trim($("[id$=txtTitle]").val())),
            KAbstract: escape($.trim($("[id$=txtAbstract]").val())),
            FileUrl: escape(fileUrl)
        };

        return knowData;
    }

    function SubmitInfo() {
        var msg = "";

        //验证知识点
        msg = ValidateKnow();
        if (msg != "") {
            $.jAlert(msg);
            return;
        }

        //获取知识点数据
        var pody = GetKnowData();

        AjaxPostAsync('/AjaxServers/KnowledgeLib/KnowledgeSave.ashx', pody,null,
             function (data) {

                 var jsonData = $.evalJSON(data);

                 if (jsonData.result == "success") {
                     //$.jAlert('提交成功!');
                     $.jPopMsgLayer('提交成功!');
                 }
                 else {
                     $.jAlert('提交出错：' + jsonData.errMsg);
                 }

                 $.closePopupLayer('RecordingSharingPopup', false);
             });
       
    }
    
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            加入案例库</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('RecordingSharingPopup',false);">
        </a></span>
    </div>
    <div class="popT bold pt5">
        <ul class="clearfix  outTable">
            <li><label style="margin-right:7px;"><span class="redColor">*</span>标题：</label><span><input id="txtTitle" name="" type="text" class="w260" style="width:249px;" runat="server" /></span></li>
            <li><label><span class="redColor">*</span>分类：</label><span>
                    <select id="selKCID1" class="w125" disabled="disabled">
                        <option value='-1'>录音共享</option>
                    </select></span> <span>
                        <select id="selKCID2" class="w125" runat="server">
                            <option value='-1'>请选择</option>
                        </select></span></li>
            <li>
                <label style="vertical-align: top">
                    摘要：</label><span>
                        <textarea id="txtAbstract" maxlength="200" name="" rows="5" runat="server"></textarea>
                    </span></li>
        </ul>
    </div>
    <div class="btn" style="margin: 20px auto">
        <input type="button" value="提交" onclick="SubmitInfo()" name="" />
        <input type="button" value="取消" onclick="javascript:$.closePopupLayer('RecordingSharingPopup',false);"
            name="" />
    </div>
</div>
