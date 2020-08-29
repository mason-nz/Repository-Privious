<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BigQuestion.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage.BigQuestion" %>

<%--<script type="text/javascript" src="../../Js/jquery-1.4.4.min.js"></script>--%>
<script type="text/javascript">
    $(document).ready(function () {

        $("#btnAddBigQuestion").click(AddBigQ);

        if ($("#hidBigQName").val() != "") {
            BindControl();
        }

    });
    //添加大题
    function AddBigQ() {
        
        var msg = "";
        msg = checkPar();
        if (msg != "") {
            $.jAlert(msg);
            return;
        }
        else {
            //赋值
            $("#hidBigQName").val($.trim($("#txtBigName").val()));
            $("#hidBigQDesc").val($.trim($("#txtBigDes").val()));
            $("#hidBigQType").val($('input:radio[name="askcategory"]:checked').val());
            $("#hidBigQScore").val($.trim($("#txtScore").val()));
            $("#hidBigQTotolCount").val($.trim($("#txtQuestionCount").val()));
            $("#hidBigQRealTotolCount").val($.trim($("#txtRealTotolCount").val()));
            $("#hidBigQBQID").val($.trim($("#txtBigQBQID").val()));

            $("#hidIsOk").val("1");
            $.closePopupLayer('DisposeOnePoper', false);
        }
    }

    ///根据隐藏域的值绑定控件
    function BindControl() {
        $("#txtBigName").val($.trim($("#hidBigQName").val()));
        $("#txtBigDes").val($.trim($("#hidBigQDesc").val()));
        $("#txtScore").val($.trim($("#hidBigQScore").val()));
        $("#txtQuestionCount").val($.trim($("#hidBigQTotolCount").val()));
        $("#txtRealTotolCount").val($.trim($("#hidBigQRealTotolCount").val()));
        $("#txtBigQBQID").val($.trim($("#hidBigQBQID").val()));
    
        $($('[name="askcategory"][value="' + $("#hidBigQType").val() + '"]')).attr("checked", true);
    }
</script>
<script type="text/javascript">

    function checkPar() {
        var msg = "";
        if ($.trim($("#txtBigName").val()) == "") {
            msg = msg + "大题名称不能为空!<br/>";
        }
        if ($.trim($("#txtBigDes").val()) == "") {
            msg = msg + "大题说明不能为空!<br/>";
        }
        if ($.trim($("#txtScore").val()) == "") {
            msg = msg + "每题分值不能为空!<br/>";
        }
        if ($.trim($("#txtQuestionCount").val()) == "") {
            msg = msg + "试题总量不能为空<br/>";
        }

        var isShow = $('input:radio[name="askcategory"]:checked').val();
        if (isShow == undefined) {
            msg = msg + "请选择题型!<br/>";
        }

        if (isNaN($.trim($("#txtScore").val()))) {
            msg = msg + "每题分值应该为数字!<br/>";
        }
        if (isNaN($.trim($("#txtQuestionCount").val()))) {
            msg = msg + "试题总量应该为数字!<br/>";
        }
        var totol = $.trim($("#txtScore").val()) * $.trim($("#txtQuestionCount").val());
        if (totol > $("#hidBigQMaxScore").val()) {
            msg = msg + "总分值不能超过“" + $("#hidBigQMaxScore").val() + "”,但现在为“" + totol + "”";
        }
        return msg;
    }

</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            添加大题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('DisposeOnePoper',false);">
        </a></span>
    </div>
    <div class="w980" style="width: 99%;border: none;">
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <span class="redColor">*</span>大题名称：</label>
                    <span>
                        <input type="text" id="txtBigName" value="" class="w260" />
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>大题说明：</label><span>
                            <textarea name="" id="txtBigDes" cols="1" rows="2" style="width: 260px;"></textarea>
                        </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>题型：</label><span>
                            <input name="askcategory" type="radio" value="1" /><em>单选题</em>&nbsp;
                            <input name="askcategory" type="radio" value="2" /><em>复选题</em>&nbsp;
                            <input name="askcategory" type="radio" value="4" /><em>判断题</em>&nbsp;
                            <input name="askcategory" type="radio" value="3" /><em>主观题</em>&nbsp; </span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>每题分值：</label><span>
                            <input type="text" id="txtScore" runat="server" value="" class="w90" /></span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>试题总量：</label><span>
                            <input type="text" id="txtQuestionCount" runat="server" value="" class="w90" /></span></li>
                <li style="display: none">
                <input type="text" id="txtRealTotolCount"  value="0" class="w90" />
 <input type="text" id="txtBigQBQID"  value="" class="w90" />
                </li>
            </ul>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" id="btnAddBigQuestion" name="" value="确定添加">&nbsp;&nbsp;
            <input type="button" name="" value="取 消" onclick="javascript:$.closePopupLayer('DisposeOnePoper',false);">
        </div>
    </div>
</div>
