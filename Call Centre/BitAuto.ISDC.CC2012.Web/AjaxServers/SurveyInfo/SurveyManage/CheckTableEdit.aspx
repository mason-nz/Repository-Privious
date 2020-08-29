<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckTableEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage.CheckTableEdit" %>

<script type="text/javascript">


    $(document).ready(function () {
        InitControl();
        $("#btnQuestionSumit").bind("click", QuestionSumit);

        //显示分数
        $("[name='txtOptionScore']").live("keyup", function (event) {
            if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                $(this).attr("title", "分数值为 " + $(this).val());
                $(this).data('tooltipsy').destroy();
                $(this).tooltipsy();
                $(this).data('tooltipsy').show();
            } else {
                $(this).attr("value", "");
            }
        });


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

        $('input:text[name="txtOptionScore"]').each(function () {
            if ($.trim($(this).val()) == "") {
                isTrue = false;
                 msg += "选项分数不能为空！<br/>";
                return false;
            }
            else {
                if (isNaN($.trim($(this).val()))) {
                    isTrue = false;
                    msg += "选项分数应该为数字！<br/>";
                    return false;
                }
            }
        });

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

            var score = $($(this).find("[name='txtOptionScore']")[0]).val();
            var isblank = "0";
            var oneOption = {
                ordernum: i,
                score: score,
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
        $("#ulMatrixListLeft li").each(function (i, v) {
            var oneMatrixTitle = {
                smtid: $(this).find("[name='hidSmtid']").val(),
                siid: $(this).find("[name='hidSiid']").val(),
                sqid: $(this).find("[name='hidSqid']").val(),
                type: "1",
                titlename: escape($(this).find("[name='txtTitleName']").val())
            };
            matrixListObj.push(oneMatrixTitle);
        });
        $("#ulMatrixListRight li").each(function (i, v) {
            var oneMatrixTitle = {
                smtid: $(this).find("[name='hidSmtid']").val(),
                siid: $(this).find("[name='hidSiid']").val(),
                sqid: $(this).find("[name='hidSqid']").val(),
                type: "2",
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
            askcategory: "5",
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
            AddMatrixLeft(null); //新加一行矩阵行标题
            AddMatrixRight(null); //新加一列矩阵行标题

            AddOption(null); //新加一行选项
            AddMatrixLeft(null); //新加一行矩阵行标题
            AddMatrixRight(null); //新加一列矩阵行标题

            AddOption(null); //新加一行选项
            AddMatrixLeft(null); //新加一行矩阵行标题
            AddMatrixRight(null); //新加一列矩阵行标题

            AddOption(null); //新加一行选项
            AddMatrixLeft(null); //新加一行矩阵行标题
            AddMatrixRight(null); //新加一列矩阵行标题
        }
        else {
            //编辑

            //获取JSon对象
            var jsonObj = $.evalJSON(unescape(jsonstr));

            $("#txtOptionName").val(jsonObj.ask);
            if (jsonObj.IsMustAnswer == "1") { //必填题
                $("#ckbBD").attr("checked", true);
            }
            //序号
            $("#hidQuestionLinkId").val(jsonObj.QuestionLinkId);

            //生成选项
            $(jsonObj.option).each(function (i, v) {
                var lihtml = " <li>";
                lihtml += "<input type='hidden' name='hidSoid' value='" + v.soid + "' />";
                lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                lihtml += "<span><input name='txtOptionName' type='text' class='w90' value=" + v.optionname + " /></span>";
                lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
                lihtml += "<span><input name='txtOptionScore' title='分数值为 " + v.score + "' type='text' class='w50 score' value='" + v.score + "' /></span> </li>";

                $("#ulOptionList").append(lihtml);
            });

            //生成标题
            $(jsonObj.matrix).each(function (i, v) {
                if (v.type == "1") {
                    var lihtml = " <li>";
                    lihtml += "<input type='hidden' name='hidSmtid' value='" + v.smtid + "' />";
                    lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                    lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                    lihtml += "<span><input name='txtTitleName' type='text' class='w90' value=" + v.titlename + " /></span>";
                    lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrixLeft(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrixLeft(this)'></a></span>";
                    lihtml += "</li>";

                    $("#ulMatrixListLeft").append(lihtml);
                }
                else if (v.type == "2") {
                    var lihtml = " <li>";
                    lihtml += "<input type='hidden' name='hidSmtid' value='" + v.smtid + "' />";
                    lihtml += "<input type='hidden' name='hidSiid' value='" + v.siid + "' />";
                    lihtml += "<input type='hidden' name='hidSqid' value='" + v.sqid + "' />";
                    lihtml += "<span><input name='txtTitleName' type='text' class='w90' value=" + v.titlename + " /></span>";
                    lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrixRight(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrixRight(this)'></a></span>";
                    lihtml += "</li>";

                    $("#ulMatrixListRight").append(lihtml);
                }

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

    ///给选项赋值分数
    function SetOptionScore() {

        $("input[name='txtOptionScore']").each(function (i, v) {
            $(this).val(Number(i + 1));
            $(this).attr("title", "分数值是 " + (Number(i + 1)));

            $(this).tooltipsy();

        });
    }

    //新加一个选项，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddOption(obj) {

        //取最大值
        var maxScore = 0;
        $("[name='txtOptionScore']").each(function (i, v) {
            if (!isNaN($(this).val())) {
                if (Number($(this).val()) > maxScore) {
                    maxScore = Number($(this).val());
                }
            }
        });
        maxScore = Number(maxScore) + 1;

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSoid'  value='" + optionIDNum + "' />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<span><input name='txtOptionName' type='text' class='w90' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddOption(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelOption(this)'></a></span>";
        lihtml += "<span><input name='txtOptionScore' type='text' id='" + maxScore + "'  title='' class='w50 score' value='' /></span> </li>";

        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulOptionList").append(lihtml);
        }

        SetOptionScore();
    }

    //新加一个矩阵标题，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddMatrixLeft(obj) {

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSmtid' value='' />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<span><input name='txtTitleName' type='text' class='w90' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrixLeft(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrixLeft(this)'></a></span>";

        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulMatrixListLeft").append(lihtml);
        }
    }
    //新加一个矩阵标题，obj是按钮， 为空是新增个空的，不为空，就在当前后面添加一个
    function AddMatrixRight(obj) {

        var siid = '<%=SIID %>';
        var sqid = '<%=SQID %>';
        var lihtml = " <li>";
        lihtml += "<input type='hidden' name='hidSmtid' value='' />";
        lihtml += "<input type='hidden' name='hidSiid' value='" + siid + "' />";
        lihtml += "<input type='hidden' name='hidSqid' value='" + sqid + "' />";
        lihtml += "<span><input name='txtTitleName' type='text' class='w90' value='' /></span>";
        lihtml += "<span class='add'><a href='javascript:void(0)' onclick='AddMatrixRight(this)'></a></span><span class='delete'><a href='javascript:void(0)' onclick='DelMatrixRight(this)'></a></span>";

        if (obj != null) {
            $(obj).parent().parent().after(lihtml);
        }
        else {
            $("#ulMatrixListRight").append(lihtml);
        }
    }

    //删除选项
    function DelOption(obj) {
        if ($("#ulOptionList li").length > 1) {
            if ($($(obj).parent().parent().find("input:text[name='txtOptionName']")[0]).val() != "") { //判断有没有值，有，就提示，没有就直接删除
                $.jConfirm("确定删除选项吗？", function (r) {
                    if (r) {
                        $(obj).parent().parent().remove();
                    }
                });
            }
            else {
                $(obj).parent().parent().remove();
            }
            SetOptionScore();
        }
        else {
            $.jAlert("至少要有一个选项");
        }
    }
    //删除矩阵标题
    function DelMatrixLeft(obj) {
        if ($("#ulMatrixListLeft li").length > 1) {
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

    //删除矩阵标题
    function DelMatrixRight(obj) {
        if ($("#ulMatrixListRight li").length > 1) {
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
<div class="pop pb15 w700 openwindow" style="width: 800px;">
    <div class="title pt5 ft16 bold">
     <h2 style="cursor: auto;">矩阵评分题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('QuestionPopup');"></a></span></div>
    <ul class="clearfix ft14">
        <li>
            <label>
                题干：</label><span class="tgJr"><input name="" type="text" class="w220" id="txtOptionName" /></span>
                <span><input type="checkbox" value="" id="ckbBD" /><em onclick="emChkIsChoose(this)">必答题</em></span>
                </li>
        <li class="jzSet" style="width: 800px">
            <label>
                设置：</label>
            <div class="jzBt2">
                <b>行标题</b>
                <ul style="padding: 0;" id="ulMatrixListLeft">
                </ul>
            </div>
            <div class="jzBt2">
                <b>列标题</b>
                <ul style="padding: 0;" id="ulMatrixListRight">
                </ul>
            </div>
            <div class="jzBt3">
                <b>选项文字</b>
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
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 550px;">
                        <thead>
                            <tr style="background: #F2F2F2;">
                                <th width="125" style="background: #F2F2F2;">
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
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                            </tr>
                            <tr style="background: #F2F2F2;">
                                <th style="font-weight: bold; background: #F2F2F2;">
                                    重要性
                                </th>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                            </tr>
                            <tr style="background: #ffffff;">
                                <th style="font-weight: bold;">
                                    易用性
                                </th>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                            </tr>
                            <tr style="background: #F2F2F2;">
                                <th style="font-weight: bold; background: #F2F2F2;">
                                    满意度
                                </th>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
                                </td>
                                <td>
                                    <select>
                                        <option value="请选择" selected="">请选择</option>
                                        <option value="5分">5分</option>
                                        <option value="4分">4分</option>
                                        <option value="3分">3分</option>
                                        <option value="2分">2分</option>
                                        <option value="1分">1分</option>
                                    </select>
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
