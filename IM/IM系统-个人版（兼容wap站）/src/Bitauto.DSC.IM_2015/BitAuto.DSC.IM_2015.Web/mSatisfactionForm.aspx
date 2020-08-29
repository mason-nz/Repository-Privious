<%@ Page Title="客户满意度" Language="C#" AutoEventWireup="true" CodeBehind="mSatisfactionForm.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.mSatisfactionForm" %>

<script type="text/javascript">

    $(function () {
        BindEvent("rdpersatisfaction");
        BindEvent("rdprosatisfaction");


        //修改
        //$("#rdpersatisfaction").attr("score", "0");
        //$("#rdprosatisfaction").attr("score", "0");

        //ShowPoint("rdpersatisfaction", parseInt($("#rdpersatisfaction").attr("score")) - 1);
        //ShowPoint("rdprosatisfaction", parseInt($("#rdprosatisfaction").attr("score")) - 1);

    })

    function BindEvent(id) {

        $("#" + id + " em").each(function () {

            $(this).hover(function () {
                ShowPoint(id, $(this).index());
            },
                function () {
                    ShowPoint(id);
                });

            $(this).click(function () {
                $("#" + id).attr("score", $(this).index() + 1);
            });
        });

    }

    function ShowPoint(id, index) {
        var score = 0;
        if (index == null || index == undefined) {
            score = parseInt($("#" + id).attr("score") - 1);
        }
        else {
            score = index;
        }

        var ems = $("#" + id + " em");
        for (i = 0; i < ems.length; i++) {
            if (i <= score) {
                $(ems[i]).attr("class", "xx-top");
            }
            else {
                $(ems[i]).attr("class", "");
            }
        }
    }

    function saveSatisfaction() {
        var rdpersatisfaction = $("#rdpersatisfaction").attr("score")
        var rdprosatisfaction = $("#rdprosatisfaction").attr("score")
        var satisfactiondetail = $("#trsatisfactiondetail").val();

        var msg = "";
        if (rdpersatisfaction == 0) {
            msg += "请选择对客服人员服务的满意度<br/>";
        }
        if (rdprosatisfaction == 0) {
            msg += "请选择对易车产品的满意度<br/>";
        }
        if (satisfactiondetail.length > 500) {
            msg += "填写内容不能超过500字<br/>";
        }
        if (msg != "") {
            alert(msg);
        }
        else {

            $.post("/AjaxServers/LayerDataHandlerBefore.ashx", { Action: 'addsatisfaction', CSID: encodeURIComponent('<%=CSID%>'), PerSatisfaction: encodeURIComponent(rdpersatisfaction), ProSatisfaction: encodeURIComponent(rdprosatisfaction), DSatisfaction: encodeURIComponent(satisfactiondetail), r: Math.random() }, function (data) {
                if (data == "success") {
                    alert("提交成功！");
                    $.closePopupLayer('AddSatisfactionAjaxPopup', true);
                }
                else if (data == "您的评价我们已经记录，谢谢您对我们的支持！") {
                    alert(data);
                    $.closePopupLayer('AddSatisfactionAjaxPopup', true);
                }
                else {
                    alert(data);
                }
            });
        }
    }
    function closeLay() {
        $.closePopupLayer('AddSatisfactionAjaxPopup');
    }


</script>
<div class="layer layer2">
    <div class="close" onclick="javascript:$.closePopupLayer('AddSatisfactionAjaxPopup',false,{Tels:''});">
        &nbsp;</div>
    <div class="pinjia_title">
        请对此服务进行评价</div>
    <div class="pinjia_cont">
        <div class="pinjia_cont_title">
            您对当前客服人员的服务是否满意？</div>
        <div class="start-box">
            <span class="start-sty" id="rdpersatisfaction" score="0"><em></em><em></em><em></em>
                <em></em><em></em></span>
        </div>
        <div class="pinjia_cont_title">
            您对易车的产品是否满意？</div>
        <div class="start-box">
            <span class="start-sty" id="rdprosatisfaction" score="0"><em></em><em></em><em></em>
                <em></em><em></em></span>
        </div>
        <div class="pinjia_cont_title2">
            请您输入不满意的原因？</div>
        <div class="textarea">
            <textarea name="" cols="" rows="" id="trsatisfactiondetail"></textarea></div>
    </div>
    <div class="btn_box">
        <a href="javascript:void(0)" class="btn" onclick="saveSatisfaction()">提交</a></div>
</div>
