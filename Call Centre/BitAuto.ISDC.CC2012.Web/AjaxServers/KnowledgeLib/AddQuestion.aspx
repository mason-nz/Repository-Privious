<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuestion.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.AddQuestion" %>

<script type="text/javascript">
    var pdfOverIframeStr = '<iframe id="pdfOverIframe" style="left: 200px; top: 200px; width: 1000px; height: 800px; position: absolute; z-index: 1;"></iframe>';
    // n 要绑定控件所在级别   pid 上一个ID
    //        $(function(){
    //           // BindSelect(1, 0);
    //            $("#selKCID1").bind("change", function () {
    //                BindSelect(2, $("#selKCID1").val());
    //            });
    //        });

    //保存
    function serverRight() {
        $('body').prepend(pdfOverIframeStr);
        var QuestionTitle = $.trim($("#txtQuestionTitle").val());
        var SelQuestionType2 = $('#selKCID2').val();
        var Content = $.trim($("#txtContent").val());


        if (QuestionTitle == '') {
            $.jAlert("请添加问题标题");
            return;
        }
        if (QuestionTitle.length > 50) {
            $.jAlert("问题标题的长度要小于50字");
            return;
        }
        if (SelQuestionType2 == '-1') {
            $.jAlert("请选择问题分类");
            return;
        }
        if (Content.length > 4000) {
            $.jAlert("问题内容的长度要小于4000字");
            return;
        }
        if (Content.length == 0) {
            $.jAlert("请添加问题内容");
            return;
        }

        $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "AddQuestion", KLID: $("#hidKLID").val(), QuestionTitle: QuestionTitle, QuestionCid: SelQuestionType2, QuestionType: $("#hidKLType").val(), QuestionDetails: Content, r: Math.random() }, function (data) {
            if (data == "success") {
                //                $.jAlert("提问成功！", function () {
                //                    $('#pdfOverIframe').remove();
                //                });
                $.jPopMsgLayer("提问成功！", function () {
                    $('#pdfOverIframe').remove();
                });
                $.closePopupLayer('AddNewQuestionAjaxPopup');
            }
            else {
                $.jAlert(data, function () {
                    $('#pdfOverIframe').remove();
                });
            }
        });
    }

    function BindSelectChange() {
        var n = 2;
        var pid = $("#selKCID1").val();
        $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid, RegionID: '<%=RegionID %>' }, function (data) {
            $("#selKCID" + n).html("");
            $("#selKCID" + n).append("<option value='-1'>请选择</option>");
            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    $.each(jsonData.root, function (idx, item) {
                        $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                    });
                }
            }
        });
    }
    function BindSelectChange2() {
        var n = 2;
        var pid = $("#selKCID1").val();
        var level = "2";
        if ("<%=KLPLevel%>" != "-1") {
            level = "<%=KLPLevel%>";
        }
        $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: level, KCID: pid, RegionID: '<%=RegionID %>' }, function (data) {
            $("#selKCID" + n).html("");
            $("#selKCID" + n).append("<option value='-1'>请选择</option>");
            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    $.each(jsonData.root, function (idx, item) {
                        $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                    });
                }
                if ("<%=KCid%>" != "-1") {
                    $("#selKCID2").val('<%=KCid%>');
                }
            }
        });
    }
    $(function () {
        if ("<%=KCPid%>" == "-1") {
            $.jAlert("数据异常：知识分类不存在！", function () {
                $.closePopupLayer('AddNewQuestionAjaxPopup');
            });
        }
        else {
            $("#selKCID1").val('<%=KCPid%>');
            BindSelectChange2();
        }
    });
</script>
<div class="pop pb15 openwindow" style="width: 390px;">
    <div class="title bold">
        <h2>
            提问</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AddNewQuestionAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable" style="padding-top: 10px;">
        <li style="width: 370px; float: left;"><span class="redColor">*</span><b>问题标题：</b><span>
            <input type="text" name="txtQuestionTitle" id="txtQuestionTitle" runat="server" class="w180"
                style="width: 285px;" />
            <input type="hidden" id="hidKLID" value="<%=KLID%>" />
            <input type="hidden" id="hidKLType" value="<%=KLType%>" />
        </span></li>
        <li style="width: 370px; float: left; *margin-top: 5px;"><span style="display: block;
            line-height: 36px; vertical-align: middle;"><span class="redColor">*</span><b>问题分类：</b>
            <select id="selKCID1" onchange="javascript:BindSelectChange()" runat="server" class="w130"
                style="width: 143px; vertical-align: middle;">
            </select>
            <select id="selKCID2" class="w130" style="width: 143px; vertical-align: middle;">
                <option value='-1'>请选择</option>
            </select>
        </span></li>
        <li style="width: 370px; float: left; *margin-top: 4px; padding-top: 4px;"><span
            style="vertical-align: top"><span class="redColor">*</span><b>问题内容：</b></span> <span>
                <textarea id="txtContent" maxlength="200" name="" rows="5" runat="server" style="width: 277px;"></textarea>
            </span></li>
    </ul>
    <div class="btn mt10" style="width: 370px;">
        <input name="" id="btnSave" type="button" onclick="serverRight()" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('AddNewQuestionAjaxPopup',false);" />
    </div>
</div>
