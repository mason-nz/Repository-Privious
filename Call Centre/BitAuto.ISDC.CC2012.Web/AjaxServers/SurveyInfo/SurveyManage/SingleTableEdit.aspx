<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleTableEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage.SingleTableEdit" %>

<script type="text/javascript">


    $(document).ready(function () {
        InitControl();
        $("#btnQuestionSumit").bind("click", QuestionSumit);
    });

    //提交
    function QuestionSumit() {

        if (CheckInfo()) {
            var jsonObj = GetQuestionJsonFormWeb();
            var str = JSON.stringify(jsonObj);

            $('#popupLayer_' + 'QuestionPopup').data('SingleJson', str);
            $.closePopupLayer('QuestionPopup', true);
        }
    }

    function CheckInfo() {
        var msg = "";
        var isTrue = true;
        if ($.trim($("#txtOptionName").val()) == "") {
            isTrue = false;
            msg += "请输入题干内容！<br/>";
        }

        $('input:text[name="txtOptionName"]').each(function () {
            if ($.trim($(this).val()) == "") {
                isTrue = false;
                msg += "选项内容不能为空！<br/>";
                return false;
            }
        });

        $('input:text[name="txtTitleName"]').each(function () {
            if ($.trim($(this).val()) == "") {
                isTrue = false;
                msg += "标题内容不能为空！<br/>";

                return false;
            }
        });
        if (msg != "") {
            $.jAlert(msg);
        }

        return isTrue;
    }


    //从页面上获取信息（Json格式）
    function GetQuestionJsonFormWeb() {

        var optionListObj = new Array();
        var matrixListObj = new Array();

        //选项
        $("#ulOptionList li").each(function (i, v) {
            var isblank = "0";
            var oneOption = {
                ordernum: i,
                score: "0",
                isblank: isblank,
                optionname: escape($(this).find("[name='txtOptionName']").val()),
                sqid: $(this).find("[name='hidSqid']").val(),
                siid: $(this).find("[name='hidSiid']").val(),
                soid: $(this).find("[name='hidSoid']").val(),
                linkid: '0'
            };
            optionListObj.push(oneOption);
        });

        //标题
        $("#ulMatrixList li").each(function (i, v) {
            var isblank = "0";
            var oneMatrixTitle = {
                smtid: $(this).find("[name='hidSmtid']").val(),
                siid: $(this).find("[name='hidSiid']").val(),
                sqid: $(this).find("[name='hidSqid']").val(),
                type: "1",
                titlename: escape($(this).find("[name='txtTitleName']").val())
            };
            matrixListObj.push(oneMatrixTitle);
        });

        var sqid = '<%=SQID %>';
        if (sqid == "") {
            sqid = "-1"; //新增的，暂时为-1
        }
        var showcolumnnum = "1";
        var IsMustAnswer = "0"; //是否必答
        if ($("#ckbBD").attr("checked") == true) {
            IsMustAnswer = "1";
        }
        var QuestionLinkId = $("#hidQuestionLinkId").val();

        //总的问题Json
        var dataObj = {
            mintextlen: "0",
            maxtextlen: "0",
            showcolumnnum: showcolumnnum,
            askcategory: "4",
            ask: escape($("#txtOptionName").val()),
            siid: '<%=SIID %>',
            sqid: sqid,
            option: optionListObj, //选项
            matrix: matrixListObj, //矩阵标题
            ordernum: "0",
            IsMustAnswer: IsMustAnswer,
            IsStatByScore: "0",
            QuestionLinkId: QuestionLinkId//序号
        };

        return dataObj;
    }


    //初始化
    function InitControl() {
        var jsonstr = '<%=JsonStr %>';
        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var action = '<%=action %>';
        if (action == "add") {
            //新增
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项
            AddOption(null); //新加一行选项

            AddMatrix(null); //新加一行矩阵行标题
            AddMatrix(null); //新加一行矩阵行标题
            AddMatrix(null); //新加一行矩阵行标题
            AddMatrix(null); //新加一行矩阵行标题

        }
        else {
            //编辑

            //获取JSon对象
            var jsonObj = $.evalJSON(unescape(jsonstr));

            $("#txtOptionName").val(jsonObj.ask);
            if (jsonObj.IsMustAnswer == "1") { //必填题
                $("#ckbBD").attr("checked", true);
            }

            //生成选项
            $(jsonObj.option).each(function (i, v) {
                var lihtml = " <li>";
                lihtml += "<input type='hidden' name='hidSoid' value='" + v.soid + "' />";
                lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                lihtml += "<span><input name='txtOptionName' type='text' class='w90' value=" + v.optionname + " /></span>";
                lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
                lihtml += " </li>";

                $("#ulOptionList").append(lihtml);
            });

            //序号
            $("#hidQuestionLinkId").val(jsonObj.QuestionLinkId);

            //生成左标题
            $(jsonObj.matrix).each(function (i, v) {

                var lihtml = " <li>";
                lihtml += "<input type='hidden' name='hidSmtid' value='" + v.smtid + "' />";
                lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                lihtml += "<span><input name='txtTitleName' type='text' class='w90' value=" + v.titlename + " /></span>";
                lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrix(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrix(this)'></a></span>";
                lihtml += "</li>";

                $("#ulMatrixList").append(lihtml);
            });

        }

    }

    //修改竖向还是横向
    function changeType() {

        var isShow = $('input:radio[name="rdoOptionCol"]:checked').val();
        if (isShow == "1") {
            $("#txType1").show();
            $("#txType2").hide();
        }
        else if (isShow == "2") {
            $("#txType2").show();
            $("#txType1").hide();
        }

    }

    //新加一个选项，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddOption(obj) {

        optionIDNum = optionIDNum - 1;

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSoid' value='" + optionIDNum + "'  />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<span><input name='txtOptionName' type='text' class='w90' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
        lihtml += " </li>";

        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulOptionList").append(lihtml);
        }
    }

    //新加一个矩阵标题，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddMatrix(obj) {

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSmtid' value='' />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<span><input name='txtTitleName' type='text' class='w90' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrix(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrix(this)'></a></span>";

        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulMatrixList").append(lihtml);
        }
    }


    //删除选项
    function DelOption(obj) {
        if ($("#ulOptionList li").length > 1) {
            if ($($(obj).parent().parent().find("input:text[name='txtOptionName']")[0]).val() != "") {
                $.jConfirm("确定删除选项吗？", function (r) {
                    if (r) {
                        $(obj).parent().parent().remove();
                    }
                });
            }
            else {
                $(obj).parent().parent().remove();

            }
        }
        else {
            $.jAlert("至少要有一个选项");
        }
    }
    //删除矩阵标题
    function DelMatrix(obj) {
        if ($("#ulMatrixList li").length > 1) {
            if ($($(obj).parent().parent().find("input:text[name='txtTitleName']")[0]).val() != "") {
                $.jConfirm("确定删除标题吗？", function (r) {
                    if (r) {
                        $(obj).parent().parent().remove();
                    }
                });
            }
            else {
                $(obj).parent().parent().remove();

            }
        }
        else {
            $.jAlert("至少要有一个标题");
        }
    }



</script>
<div class="pop pb15 w700 openwindow">
    <div class="title pt5 ft16 bold">
       <h2 style="cursor: auto;">矩阵单选题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('QuestionPopup');"></a></span></div>
    <ul class="clearfix ft14">
        <li>
            <label>
                题干：</label><span class="tgJr"><input name="" type="text" class="w220" id="txtOptionName" /></span>
                <span><input type="checkbox" value="" id="ckbBD" /><em onclick="emChkIsChoose(this)">必答题</em></span>
                </li>
        <li class="jzSet">
            <label>
                设置：</label>
            <div class="jzBt">
                <b>左行标题</b>
                <ul style="padding: 0;" id="ulMatrixList">
                </ul>
            </div>
            <div class="jzBt">
                <b>选项</b>
                <ul style="padding: 0;" id="ulOptionList">
                </ul>
            </div>
        </li>
        <li class="ckTx">
            <label>
                参考样式：</label>
            <span>请问您每天使用数字销售助手接待多少批次请问您每天使用数字销售助手接待多少批次的顾客的顾客？</span>
            <ul class="txType">
                <li>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr style="background: #F2F2F2;">
                                <th width="115" style="background: #F2F2F2;">
                                    &nbsp;
                                </th>
                                <td width="168" style="font-weight: bold;">
                                    很不满意
                                </td>
                                <td width="167" style="font-weight: bold;">
                                    不满意
                                </td>
                                <td width="155" style="font-weight: bold;">
                                    一般
                                </td>
                                <td width="130" style="font-weight: bold;">
                                    满意
                                </td>
                                <td width="147" style="font-weight: bold;">
                                    很满意
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr style="background: #ffffff;">
                                <th style="font-weight: bold;">
                                    使用频次
                                </th>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                            </tr>
                            <tr style="background: #F2F2F2;">
                                <th style="font-weight: bold; background: #F2F2F2;">
                                    重要性
                                </th>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                            </tr>
                            <tr style="background: #ffffff;">
                                <th style="font-weight: bold;">
                                    易用性
                                </th>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                            </tr>
                            <tr style="background: #F2F2F2;">
                                <th style="font-weight: bold; background: #F2F2F2;">
                                    满意度
                                </th>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                                <td>
                                    <input type="radio">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </li>
            </ul>
        </li>
    </ul>
    <div class="btn">
        <input type='hidden' id='hidQuestionLinkId' value='' />
        <input name="" type="button" value="提 交" class="btnSave bold" id="btnQuestionSumit" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('QuestionPopup');" /></div>
</div>
