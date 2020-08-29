<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSurveyInfoEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask.UCSurveyInfoEdit" %>
<script type="text/javascript">
    var uCEditSurveyHelper_<%= this.Num %>=CreateUCEditSurveyHelper();    
    function CreateUCEditSurveyHelper() {
        var obj = (function () {
        var strlen = function (s) {
                var l = 0;
                var a = s.split("");
                for (var i = 0; i < a.length; i++) {
                    if (a[i].charCodeAt(0) < 299) {
                        l++;
                    } else {
                        l += 2;
                    }
                }
                return l;
            },
        //给个题型数组付值
        setArrayData = function (DataArray, i) {
        if (i == 1) {
            var radiostr = $("#hidRadioSQID_<%= this.Num%>").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false) {
                        DataArray.push(radiostr.split(',')[j]);
                    }
                }
            }
        }
        if (i == 2) {
            var radiostr = $("#hidCheckBoxSQID_<%= this.Num%>").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 3) {
            var radiostr = $("#hidTextSQID_<%= this.Num%>").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 4) {
            var radiostr = $("#hidMatrixRadioSQID_<%= this.Num%>").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 5) {
            var radiostr = $("#hidMatrixDropSOID_<%= this.Num%>").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
    },
        MustRadio = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>';
            var flag = false;
            $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                emptystr += "<%=SIIDName%>，第" + RadioNoNumber + "题单选题必答。<br/>";
            }

        }
        return emptystr;
    },
        //验证多选选不答情况
        MustCheck = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                emptystr += "<%=SIIDName%>，第" + RadioNoNumber + "题多选题必答。<br/>";
            }
        }
        return emptystr;
    },
        //验证文本不答情况
        MustText = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';
            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).val() != "") {
                    flag = true;


                }
            });

            if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                emptystr += "<%=SIIDName%>，第" + RadioNoNumber + "题文本题必答。<br/>";
            }
        }
        return emptystr;
    },
        //验证矩阵单选不答情况
        MustMatrixRadio = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)%>';
            $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                emptystr += "<%=SIIDName%>，第" + RadioNoNumber + "题矩阵单选题必答。<br/>";
            }
        }
        return emptystr;
    },
        //验证矩阵下拉选不答情况
        MustMatrixSelect = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';
            $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).val() != "-1") {
                    flag = true;

                }
            });
            if (flag == false && $('#' + pre_arrRadio[i]).attr("must") == "1") {
                emptystr += "<%=SIIDName%>，第" + RadioNoNumber + "题矩阵评分题必答。<br/>";
            }
        }
        return emptystr;
    },
        //验证选择的选项如果要输入文本，文本必填
        CheckRadio = function (pre_arrRadio) {
        var emptystr = "";
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>';
            $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    if ($("#" + $(this).val()).attr("showflag") == "1") {
                        if ($("#" + $(this).val()).val() == "") {
                            $("#" + $(this).val()).focus();
                            emptystr += "<%=SIIDName%>，第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。<br/>";

                        }
                    }

                }
            });
        }
        return emptystr;
    },
        //验证选择的选项如果要输入文本，文本必填
        CheckCheckBoxLong = function (pre_arrRadio) {
        var emptystr = "";

        for (var i = 0; i < pre_arrRadio.length; i++) {
            var checkednum = 0;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    checkednum = checkednum + 1;
                }
            });
            var maxlong = $('#' + pre_arrRadio[i]).attr("maxLong");
            var minlong = $('#' + pre_arrRadio[i]).attr("minLong");
            if (maxlong != "" && maxlong != "0") {
                if (checkednum > parseInt(maxlong)) {
                    emptystr += "<%=SIIDName%>，第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能大于" + maxlong + "项。<br/>";
                }
            }
            if (minlong != "" && minlong != "0") {
                if (checkednum < parseInt(minlong)) {
                    emptystr += "<%=SIIDName%>，第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能小于" + minlong + "项。<br/>";
                }
            }
        }
        return emptystr;
    },
        //验证选择的选项如果要输入文本，文本必填
        CheckCheckType = function (pre_arrRadio) {
        var emptystr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    if ($("#" + $(this).val()).attr("showflag") == "1") {
                        if ($("#" + $(this).val()).val() == "") {
                            $("#" + $(this).val()).focus();
                            emptystr += "<%=SIIDName%>，第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。<br/>";
                        }
                    }

                }
            });
        }
        return emptystr;
    },
        //验证文本长度
        CheckText = function (pre_arrRadio) {
        var stralter = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';
            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).val() != "") {

                    if ($(this).attr("maxLong") != "" && $(this).attr("minLong") != "") {
                        if (strlen($(this).val()) > parseInt($(this).attr("maxLong"))) {
                            $(this).focus();
                            stralter += "<%=SIIDName%>，第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能大于最大允许输入长度" + parseInt($(this).attr("maxLong")) + "个字符。<br/>";
                        }
                        if (strlen($(this).val()) < parseInt($(this).attr("minLong"))) {
                            $(this).focus();
                            stralter += "<%=SIIDName%>，第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能小于最小允许输入长度" + parseInt($(this).attr("minLong")) + "个字符。<br/>";
                        }
                    }
                }
            });
        }
        return stralter;
    },
        //验证矩阵单选选择一个，就都选
        CheckMatrixRadio = function (pre_arrRadio) {
        var stralter = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var j = 0;
            var num = "";
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)%>';
            var liecout = 0;
            $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                liecout = $(this).attr("lie")
                num = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    //选中个数
                    j = j + 1;
                }
            });
            if (j != 0) {
                var hang = $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "'][lie='" + liecout + "']").length;
                if (hang > 0 && hang > j) {
                    stralter += "<%=SIIDName%>，第" + num + "为矩阵单选题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。<br/>";
                }
            }
        }
        return stralter;
    },
        //验证矩阵下拉选选择一个，就都选
        CheckMatrixDrop = function (pre_arrRadio) {
        var stralter = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var j = 0;
            var num = "";
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';

            $("select[SQID$='" + pre_arrRadio[i] + "']").each(function () {
                //判断题型，如果是单选题

                num = $(this).parents("li[nums]").attr("nums");
                if ($(this).val() != "-1") {
                    j = j + 1;

                }
            });

            if (j != 0) {
                var hang = $("select[SQID$='" + pre_arrRadio[i] + "']").length;
                if (hang > 0 && hang > j) {
                    stralter += "<%=SIIDName%>，第" + num + "为矩阵评分题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。<br/>";
                }
            }
        }
        return stralter;
    },
        //验证单选不答情况
        emptyRadio = function (pre_arrRadio, arryNum) {
        var emptystr = 0;
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>';
            var flag = false;
            $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false) {
                arryNum.push(parseInt(RadioNoNumber));
                emptystr += 1;
            }

        }
        return emptystr;
    },
        //验证多选选不答情况
        emptyCheck = function (pre_arrRadio, arryNum) {
        var emptystr = 0;
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false) {
                arryNum.push(parseInt(RadioNoNumber));
                emptystr += 1;
            }
        }
        return emptystr;
    },
        //验证文本不答情况
        emptyText = function (pre_arrRadio, arryNum) {
        var emptystr = 0;
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';
            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).val() != "") {
                    flag = true;


                }
            });

            if (flag == false) {
                arryNum.push(parseInt(RadioNoNumber));
                emptystr += 1;
            }
        }
        return emptystr;
    },
        //验证矩阵单选不答情况
        emptyMatrixRadio = function (pre_arrRadio, arryNum) {
        var emptystr = 0;
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)%>';
            $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    flag = true;

                }
            });
            if (flag == false) {
                arryNum.push(parseInt(RadioNoNumber));
                emptystr += 1;
            }
        }
        return emptystr;
    },
        //验证矩阵下拉选不答情况
        emptyMatrixSelect = function (pre_arrRadio, arryNum) {
        var emptystr = 0;
        var RadioNoNumber = 0;
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var flag = false;
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';
            $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").each(function () {
                RadioNoNumber = $(this).parents("li[nums]").attr("nums");
                //判断题型，如果是单选题
                if ($(this).val() != "-1") {
                    flag = true;

                }
            });
            if (flag == false) {
                arryNum.push(parseInt(RadioNoNumber));
                emptystr += 1;
            }
        }
        return emptystr;
    },
        //取单选大题数据
        GetDataRadio = function (pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>';
            $("input[type='radio'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    var answercontent = "";
                    if ($("#" + $(this).val()).attr("showflag") == "1") {
                        answercontent = $("#" + $(this).val()).val()

                    }
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(answercontent) + "'},";

                }
            });

        }

        return dataStr;
    },
        //取多选大题数据
        GetCheckData = function (pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    var answercontent = "";
                    if ($("#" + $(this).val()).attr("showflag") == "1") {
                        answercontent = $("#" + $(this).val()).val()

                    }
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(answercontent) + "'},";

                }
            });

        }
        return dataStr;
    },
        //取文本
        GetDataText = function (pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';

            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                //如果文本题不答不保存
                if ($(this).val() != "") {
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent(emptystr) + "','AnswerContent':'" + encodeURIComponent($(this).val()) + "'},";
                }
            });
        }
        return dataStr;
    },
        //取矩阵单选答题数据
        GetDataMatrixRadio = function (pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)%>';
            $("input[type='radio'][SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent($(this).attr("name").split('_')[0]) + "','SMCTID':'" + encodeURIComponent($(this).attr("lie")) + "','SOID':'" + encodeURIComponent($(this).attr("lie")) + "','AnswerContent':'" + encodeURIComponent(emptystr) + "'},";
                }
            });
        }
        return dataStr;
    },
        //取矩阵下拉选数据
        GetDataMatrixSelect = function (pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';
            $("select[SQID='" + pre_arrRadio[i] + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).val() != "-1") {
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent($(this).attr("hang")) + "','SMCTID':'" + encodeURIComponent($(this).attr("lie")) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(emptystr) + "'},";
                }
            });
        }
        return dataStr;
    },
        CheckDataForSurvey = function () {
        //声明数组
        //单选题数组,把不同题型的SQID保存到不同的数组里
        var pre_arrRadio = new Array();
        setArrayData(pre_arrRadio, 1);
        var pre_arrCheck = new Array();
        setArrayData(pre_arrCheck, 2);
        var pre_arrText = new Array();
        setArrayData(pre_arrText, 3);
        var pre_arrMatrixRadio = new Array();
        setArrayData(pre_arrMatrixRadio, 4);
        var pre_arrMatrixDrop = new Array();
        setArrayData(pre_arrMatrixDrop, 5);

        //是否验证通过
        var flagRadio = "";
        //是否验证通过
        flagRadio += MustRadio(pre_arrRadio);
        flagRadio += MustCheck(pre_arrCheck);
        flagRadio += MustText(pre_arrText);
        flagRadio += MustMatrixRadio(pre_arrMatrixRadio);
        flagRadio += MustMatrixSelect(pre_arrMatrixDrop);
        flagRadio += CheckRadio(pre_arrRadio);
        flagRadio += CheckCheckBoxLong(pre_arrCheck);
        flagRadio += CheckCheckType(pre_arrCheck);
        flagRadio += CheckText(pre_arrText);
        flagRadio += CheckMatrixRadio(pre_arrMatrixRadio);
        flagRadio += CheckMatrixDrop(pre_arrMatrixDrop);
        return flagRadio;
    },

        CheckEmptyForSurvey = function () {
        //声明数组
        //单选题数组,把不同题型的SQID保存到不同的数组里
        var pre_arrRadio = new Array();
        setArrayData(pre_arrRadio, 1);
        var pre_arrCheck = new Array();
        setArrayData(pre_arrCheck, 2);
        var pre_arrText = new Array();
        setArrayData(pre_arrText, 3);
        var pre_arrMatrixRadio = new Array();
        setArrayData(pre_arrMatrixRadio, 4);
        var pre_arrMatrixDrop = new Array();
        setArrayData(pre_arrMatrixDrop, 5);
        //不答提示
        var emptystr = "";
        var arryNum = new Array();
        //取单选题不答提示
        emptyRadio(pre_arrRadio, arryNum);
        //取多选题不答提示
        emptyCheck(pre_arrCheck, arryNum);
        //取文本题不答提示
        emptyText(pre_arrText, arryNum);
        //取矩阵单选不答提示
        emptyMatrixRadio(pre_arrMatrixRadio, arryNum);
        //取矩阵下拉不答提示
        emptyMatrixSelect(pre_arrMatrixDrop, arryNum);
        $.each(arryNum.sort(function AscSort(x, y) {
            return x == y ? 0 : (x > y ? 1 : -1);
        }), function (i, item) {
            if (i == 0) {
                emptystr += item;
            }
            else {
                emptystr += "," + item;
            }
        });
        if (emptystr != 0) {
            return "<%=SIIDName%>，还有" + emptystr + "题没答。<br/>";
        }
        else {
            return "";
        }
    },
        //取数据
        GetData = function () {
        var pre_arrRadio = new Array();
        setArrayData(pre_arrRadio, 1);
        var pre_arrCheck = new Array();
        setArrayData(pre_arrCheck, 2);
        var pre_arrText = new Array();
        setArrayData(pre_arrText, 3);
        var pre_arrMatrixRadio = new Array();
        setArrayData(pre_arrMatrixRadio, 4);
        var pre_arrMatrixDrop = new Array();
        setArrayData(pre_arrMatrixDrop, 5);
        var msg = "{DataRoot:[";
        var DataRadio = GetDataRadio(pre_arrRadio);
        var DataCheckBox = GetCheckData(pre_arrCheck);
        var DataText = GetDataText(pre_arrText);
        var DataMatrixRadio = GetDataMatrixRadio(pre_arrMatrixRadio);
        var DataMatrixDrop = GetDataMatrixSelect(pre_arrMatrixDrop);
        if (DataRadio != "") {
            msg += DataRadio;
        }
        if (DataCheckBox != "") {
            msg += DataCheckBox;
        }
        if (DataText != "") {
            msg += DataText;
        }
        if (DataMatrixRadio != "") {
            msg += DataMatrixRadio;
        }
        if (DataMatrixDrop != "") {
            msg += DataMatrixDrop;
        }
        if (msg != "{DataRoot:[") {
            msg = msg.substring(0, msg.length - 1);
        }
        msg += "]}";
        //alert(msg);
        return msg;


    };
        return {
                strlen: strlen,
                setArrayData: setArrayData,
                MustRadio: MustRadio,
                MustCheck: MustCheck,
                MustText: MustText,
                MustMatrixRadio: MustMatrixRadio,
                MustMatrixSelect: MustMatrixSelect,
                CheckRadio: CheckRadio,
                CheckCheckBoxLong: CheckCheckBoxLong,
                CheckCheckType: CheckCheckType,
                CheckText: CheckText,
                CheckMatrixRadio: CheckMatrixRadio,
                CheckMatrixDrop: CheckMatrixDrop,
                emptyRadio: emptyRadio,
                emptyCheck: emptyCheck,
                emptyText: emptyText,
                emptyMatrixRadio: emptyMatrixRadio,
                emptyMatrixSelect: emptyMatrixSelect,
                GetDataRadio: GetDataRadio,
                GetCheckData: GetCheckData,
                GetDataText: GetDataText,
                GetDataMatrixRadio: GetDataMatrixRadio,
                GetDataMatrixSelect: GetDataMatrixSelect,
                CheckDataForSurvey: CheckDataForSurvey,
                CheckEmptyForSurvey: CheckEmptyForSurvey,
                GetData: GetData
            };
        })();
        return obj;
    }    
</script>
<div style="clear: both;" class="mbInfo">
    问卷调查-<%=SIIDName%>
    <a href="javascript:void(0)" onclick="divShowHideEventForSurvey('divSurvey_<%= this.Num%>',this)"
        class="<%=GetStyle %>"></a>
</div>
<div id="divSurvey_<%= this.Num%>" style="<%=GetClass%>">
    <input id="hidRadioSQID_<%= this.Num%>" type="hidden" value='<%=RadioSQIDStr %>' />
    <input id="hidCheckBoxSQID_<%= this.Num%>" type="hidden" value='<%=CheckBoxSQIDStr %>' />
    <input id="hidTextSQID_<%= this.Num%>" type="hidden" value='<%=TextSQIDStr %>' />
    <input id="hidMatrixRadioSQID_<%= this.Num%>" type="hidden" value='<%=MatrixRadioSQIDStr %>' />
    <input id="hidMatrixDropSOID_<%= this.Num%>" type="hidden" value='<%=MatrixDropSQIDStr%>' />
    <input id="hidNum_<%= this.Num%>" type="hidden" value='<%=this.Num%>' />
    <input id="hidSIIDName_<%= this.Num%>" type="hidden" value='<%=this.SIIDName%>' />
    <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
        <ItemTemplate>
            <div class="st" id='<%#Eval("SQID")%>' index='<%#(Container.ItemIndex + 1).ToString()%>'
                must="<%#Eval("IsMustAnswer")%>" maxlong="<%#Eval("MaxTextLen")%>" minlong="<%#Eval("MinTextLen") %>"
                style='border-bottom-width: 0px;'>
                <ul style="padding-left: 50px;">
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>'><span><span
                        style="float: left;">
                        <label>
                            &nbsp;&nbsp;&nbsp;&nbsp;<%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%></label>&nbsp;&nbsp;&nbsp;</span><span
                                class="sjbt" style="float: left; width: 800px;"><%#Eval("Ask")%></span></span>
                        <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text='<%#Eval("AskCategory")%>'></asp:Label>
                        <asp:Label ID="lblSQID" runat="server" Visible="false" Text='<%#Eval("SQID")%>'></asp:Label>
                    </li>
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>' visible="false"
                        runat="server" id="ulRadio" nums="<%#(Container.ItemIndex + 1).ToString()%>"><span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeaterRadio" runat="server">
                                    <ItemTemplate>
                                        <li <%# GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"class='chooseXx'":""%>>
                                            <input name="<%#Eval("SQID")%>" typename="<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>"
                                                type="radio" yes='<%#GetJump(Eval("SOID").ToString())%>' anchor='<%#GetJump(Eval("SOID").ToString())%>'
                                                <%#GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"checked='checked'":""%>
                                                value="<%#Eval("SOID")%>" class="dt" /><span style="cursor: pointer" onclick="emChkIsChoose(this)"
                                                    yes='<%#GetJump(Eval("SOID").ToString())%>' anchor='<%#GetJump(Eval("SOID").ToString())%>'><%#Eval("OptionName")%></span><span><input
                                                        type="text" name="<%#Eval("SQID")%>" id="<%#Eval("SOID")%>" type="text" showflag="<%#Eval("IsBlank")%>"
                                                        value="<%#AnswerContent(Eval("SQID").ToString(),Eval("SOID").ToString())%>" style="border: none;
                                                        border-bottom: #CCC 1px solid; background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" /></span>
                                            <span><span>
                                                <%#GetIndex(GetJump(Eval("SOID").ToString()))%></span> </span></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <span></li>
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>' visible="false"
                        runat="server" id="ulCheckBox" nums="<%#(Container.ItemIndex + 1).ToString()%>">
                        <span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeaterCheckBox" runat="server">
                                    <ItemTemplate>
                                        <li <%# GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"class='chooseXx'":""%>>
                                            <input name="<%#Eval("SQID")%>" typename="<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>"
                                                type="checkbox" <%#GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"checked='checked'":""%>
                                                value="<%#Eval("SOID")%>" class="dt" /><span onclick="emChkIsChoose(this)" style="cursor: pointer"><%#Eval("OptionName")%></span><span><input
                                                    type="text" id="<%#Eval("SOID")%>" name="<%#Eval("SQID")%>" value="<%#AnswerContent(Eval("SQID").ToString(),Eval("SOID").ToString())%>"
                                                    showflag="<%#Eval("IsBlank")%>" style="border: none; border-bottom: #CCC 1px solid;
                                                    background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" /></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <span></li>
                    <li class='xzt3' visible="false" runat="server" id="ulText" nums="<%#(Container.ItemIndex + 1).ToString()%>">
                        <span>
                            <ul class="clearfix">
                                <li class="chooseXx">
                                    <textarea name="<%#Eval("SQID")%>" style="width: 810px;" maxlong="<%#Eval("MaxTextLen")%>"
                                        minlong="<%#Eval("MinTextLen") %>"><%#GetChecked(Eval("SQID").ToString(),"","","")%></textarea></li>
                            </ul>
                        </span></li>
                    <li class="clearfix" style="width: 910px;" visible="false" runat="server" id="liMatrixRadio"
                        nums="<%#(Container.ItemIndex + 1).ToString()%>">
                        <%#GetTableHtmlForRadio(Eval("SQID").ToString())%>
                    </li>
                    <li class="clearfix" style="width: 910px;" visible="false" runat="server" id="liMatrixDropDown"
                        nums="<%#(Container.ItemIndex + 1).ToString()%>">
                        <%#GetHtmlForDropDown(Eval("SQID").ToString())%>
                    </li>
                </ul>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
