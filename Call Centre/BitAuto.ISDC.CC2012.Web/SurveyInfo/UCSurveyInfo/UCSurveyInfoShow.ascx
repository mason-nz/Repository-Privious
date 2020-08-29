<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSurveyInfoShow.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.UCSurveyInfo.UCSurveyInfoShow" %>
<script type="text/javascript" src="../../Js/anchor.1.0.js"></script>
<script type="text/javascript">

    //数组编码格式,给数组对象做扩展
    Array.prototype.S = String.fromCharCode(2);
    Array.prototype.in_array = function (e) {
        var r = new RegExp(this.S + e + this.S);
        return (r.test(this.S + this.join(this.S) + this.S));
    };
    $(function () {
        //为跳题
        $("input[type='radio']").each(function () {
            if ($(this).attr("yes") != undefined && $(this).attr("yes") != "0") {
                $(this).zxxAnchor();
            }
        })
        $("span").each(function () {
            if ($(this).attr("yes") != undefined && $(this).attr("yes") != "0") {
                $(this).zxxAnchor();
            }
        })
    });
    function strlen(s) {
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
    }

    //给个题型数组付值
    function setArrayData(DataArray, i) {
        if (i == 1) {
            var radiostr = $("#hidRadioSQID").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false) {
                        DataArray.push(radiostr.split(',')[j]);
                    }
                }
            }
        }
        if (i == 2) {
            var radiostr = $("#hidCheckBoxSQID").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 3) {
            var radiostr = $("#hidTextSQID").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 4) {
            var radiostr = $("#hidMatrixRadioSQID").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
        if (i == 5) {
            var radiostr = $("#hidMatrixDropSOID").val();
            if (radiostr != "") {
                for (var j = 0; j < radiostr.split(',').length; j++) {
                    if (DataArray.in_array(radiostr.split(',')[j]) == false)
                    { DataArray.push(radiostr.split(',')[j]); }
                }
            }
        }
    }

    function MustRadio(pre_arrRadio) {
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
                emptystr += "第" + RadioNoNumber + "题单选题必答。\n";
            }

        }
        return emptystr;
    }
    //验证多选选不答情况
    function MustCheck(pre_arrRadio) {
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
                emptystr += "第" + RadioNoNumber + "题多选题必答。\n";
            }
        }
        return emptystr;
    }
    //验证文本不答情况
    function MustText(pre_arrRadio) {
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
                emptystr += "第" + RadioNoNumber + "题文本题必答。\n";
            }
        }
        return emptystr;
    }
    //验证矩阵单选不答情况
    function MustMatrixRadio(pre_arrRadio) {
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
                emptystr += "第" + RadioNoNumber + "题矩阵单选题必答。\n";
            }
        }
        return emptystr;
    }
    //验证矩阵下拉选不答情况
    function MustMatrixSelect(pre_arrRadio) {
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
                emptystr += "第" + RadioNoNumber + "题矩阵评分题必答。\n";
            }
        }
        return emptystr;
    }
    //验证选择的选项如果要输入文本，文本必填
    function CheckRadio(pre_arrRadio) {
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
                            emptystr += "第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。\n";

                        }
                    }

                }
            });
        }
        return emptystr;
    }

    //验证选择的选项如果要输入文本，文本必填
    function CheckCheckBoxLong(pre_arrRadio) {
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
                    emptystr += "第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能大于" + maxlong + "项。\n";
                }
            }
            if (minlong != "" && minlong != "0") {
                if (checkednum < parseInt(minlong)) {
                    emptystr += "第" + $('#' + pre_arrRadio[i]).attr("index") + "题多选题，选择项数不能小于" + minlong + "项。\n";
                }
            }
        }
        return emptystr;
    }

    //验证选择的选项如果要输入文本，文本必填
    function CheckCheckType(pre_arrRadio) {
        var emptystr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>';
            $("input[type='checkbox'][name='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).attr("checked")) {
                    if ($("#" + $(this).val()).attr("showflag") == "1") {
                        if ($("#" + $(this).val()).val() == "") {
                            $("#" + $(this).val()).focus();
                            emptystr += "第" + $(this).parents("li[nums]").attr("nums") + "题请在选项后输入文本。\n";
                        }
                    }

                }
            });
        }
        return emptystr;
    }



    //验证文本长度
    function CheckText(pre_arrRadio) {

        var stralter = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';
            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).val() != "") {

                    if ($(this).attr("maxLong") != "" && $(this).attr("minLong") != "") {
                        if (strlen($(this).val()) > parseInt($(this).attr("maxLong"))) {
                            $(this).focus();
                            stralter += "第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能大于最大允许输入长度" + parseInt($(this).attr("maxLong")) + "个字符。\n";
                        }
                        if (strlen($(this).val()) < parseInt($(this).attr("minLong"))) {
                            $(this).focus();
                            stralter += "第" + $(this).parents("li[nums]").attr("nums") + "题，文本输入长度不能小于最小允许输入长度" + parseInt($(this).attr("minLong")) + "个字符。\n";
                        }
                    }
                }
            });
        }
        return stralter;
    }
    //验证矩阵单选选择一个，就都选
    function CheckMatrixRadio(pre_arrRadio) {
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
                    stralter += "第" + num + "为矩阵单选题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。\n";
                }
            }
        }
        return stralter;
    }
    //验证矩阵下拉选选择一个，就都选
    function CheckMatrixDrop(pre_arrRadio) {
        var stralter = "";

        for (var i = 0; i < pre_arrRadio.length; i++) {
            var j = 0;
            var num = "";
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';

            $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").each(function () {
                //判断题型，如果是单选题

                num = $(this).parents("li[nums]").attr("nums");
                if ($(this).val() != "-1") {
                    j = j + 1;

                }
            });

            if (j != 0) {
                var hang = $("select[SQID$='" + pre_arrRadio[i] + "'][typename$='" + asktype + "']").length;
                if (hang > 0 && hang > j) {
                    stralter += "第" + num + "为矩阵评分题，如若作答请全部作答，为了保证统计结果的准确度，请返回填写。\n";
                }
            }
        }
        return stralter;
    }
    //验证单选不答情况
    function emptyRadio(pre_arrRadio) {
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
            if (flag == false) {
                emptystr += "第" + RadioNoNumber + "题单选题没答。\n";
            }

        }
        return emptystr;
    }
    //验证多选选不答情况
    function emptyCheck(pre_arrRadio) {
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
            if (flag == false) {
                emptystr += "第" + RadioNoNumber + "题多选题没答。\n";
            }
        }
        return emptystr;
    }
    //验证文本不答情况
    function emptyText(pre_arrRadio) {
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

            if (flag == false) {
                emptystr += "第" + RadioNoNumber + "题文本题没答。\n";
            }
        }
        return emptystr;
    }
    //验证矩阵单选不答情况
    function emptyMatrixRadio(pre_arrRadio) {
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
            if (flag == false) {
                emptystr += "第" + RadioNoNumber + "题矩阵单选题没答。\n";
            }
        }
        return emptystr;
    }
    //验证矩阵下拉选不答情况
    function emptyMatrixSelect(pre_arrRadio) {
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
            if (flag == false) {
                emptystr += "第" + RadioNoNumber + "题矩阵评分题没答。\n";
            }
        }
        return emptystr;
    }
    //取单选大题数据
    function GetDataRadio(pre_arrRadio) {
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
    }
    //取多选大题数据
    function GetCheckData(pre_arrRadio) {
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
    }

    //取文本
    function GetDataText(pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)%>';

            $("textarea[name='" + pre_arrRadio[i] + "']").each(function () {
                //判断题型，如果是单选题
                //如果文本题不答不保存
                if ($(this).val() != "") {
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent(emptystr) + "','SMCTID':'" + encodeURIComponent(emptystr) + "','SOID':'" + encodeURIComponent(emptystr) + "','AnswerContent':'" + encodeURIComponent($(this).val()) + "'},";
                }
            });
        }
        return dataStr;
    }

    //取矩阵单选答题数据
    function GetDataMatrixRadio(pre_arrRadio) {
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
    }
    //取矩阵下拉选数据
    function GetDataMatrixSelect(pre_arrRadio) {
        var dataStr = "";
        for (var i = 0; i < pre_arrRadio.length; i++) {
            var asktype = '<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)%>';
            $("select[SQID='" + pre_arrRadio[i] + "'][typename='" + asktype + "']").each(function () {
                //判断题型，如果是单选题
                if ($(this).val() != "-1") {
                    var emptystr = "";
                    dataStr += "{'SQID':'" + encodeURIComponent(pre_arrRadio[i]) + "','SMRTID':'" + encodeURIComponent($(this).attr("hang")) + "','SMCTID':'" + encodeURIComponent($(this).attr("lie")) + "','SOID':'" + encodeURIComponent($(this).val()) + "','AnswerContent':'" + encodeURIComponent(emptystr) + "'},";
                }
            });
        }
        return dataStr;
    }

    function CheckDataForSurvey() {
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
        //是否验证通过
        var flagRadio = "";
        //取单选题不答提示
        emptystr += emptyRadio(pre_arrRadio);
        //取多选题不答提示
        emptystr += emptyCheck(pre_arrCheck);
        //取文本题不答提示
        emptystr += emptyText(pre_arrText);
        //取矩阵单选不答提示
        emptystr += emptyMatrixRadio(pre_arrMatrixRadio);
        //取矩阵下拉不答提示
        emptystr += emptyMatrixSelect(pre_arrMatrixDrop);
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

        if (flagRadio != "") {
            alert(flagRadio);
            return false;
        }
        //验证通过
        if (emptystr != "") {
            if (confirm(emptystr + "\r\n您要继续提交吗？")) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }
    }
    //取数据
    function GetData() {
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


    }
    //点击文字，选中复选框
    function emChkIsChoose(othis) {
        var $checkbox = $(othis).prev();
        if ($checkbox.attr("type") == "radio") {
            $checkbox.attr("checked", "checked");
        }
        else {
            if ($checkbox.is(":checked")) {
                $checkbox.removeAttr("checked");
            }
            else {
                $checkbox.attr("checked", "checked");
            }
        }
    }


</script>
<input id="hidRadioSQID" type="hidden" value='<%=RadioSQIDStr %>' />
<input id="hidCheckBoxSQID" type="hidden" value='<%=CheckBoxSQIDStr %>' />
<input id="hidTextSQID" type="hidden" value='<%=TextSQIDStr %>' />
<input id="hidMatrixRadioSQID" type="hidden" value='<%=MatrixRadioSQIDStr %>' />
<input id="hidMatrixDropSOID" type="hidden" value='<%=MatrixDropSQIDStr%>' />
<asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
    <ItemTemplate>
        <div class="st" id='<%#Eval("SQID")%>' index='<%#(Container.ItemIndex + 1).ToString()%>'
            must="<%#Eval("IsMustAnswer")%>" maxlong="<%#Eval("MaxTextLen")%>" minlong="<%#Eval("MinTextLen") %>">
            <ul>
                <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>'>
                    <%--<label>
                        <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%>&nbsp;&nbsp;&nbsp;</label><span
                            class="sjbt"><%#Eval("Ask")%></span>--%>
                    <span><span style="float: left;">
                        <label>
                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%></label>&nbsp;&nbsp;&nbsp;</span><span
                                class="sjbt" style="float: left; width: 850px;"><%#Eval("Ask")%></span></span>
                    <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text='<%#Eval("AskCategory")%>'></asp:Label>
                    <asp:Label ID="lblSQID" runat="server" Visible="false" Text='<%#Eval("SQID")%>'></asp:Label>
                </li>
                <li visible="false" runat="server" id="liRadio" nums="<%#(Container.ItemIndex + 1).ToString()%>"
                    class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>'><span>
                        <ul class="clearfix w800">
                            <asp:Repeater ID="repeaterRadio" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input name="<%#Eval("SQID")%>" typename="<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)%>"
                                            type="radio" value="<%#Eval("SOID")%>" class="dt" yes='<%#GetJump(Eval("SOID").ToString())%>'
                                            anchor='<%#GetJump(Eval("SOID").ToString())%>' /><span style="cursor: pointer" onclick="emChkIsChoose(this)"
                                                yes='<%#GetJump(Eval("SOID").ToString())%>' anchor='<%#GetJump(Eval("SOID").ToString())%>'><%#Eval("OptionName")%></span><span><input
                                                    name="<%#Eval("SQID")%>" id="<%#Eval("SOID")%>" type="text" value="" showflag="<%#Eval("IsBlank")%>"
                                                    style="border: none; border-bottom: #CCC 1px solid; background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" />
                                                    &nbsp;<%#GetIndex(GetJump(Eval("SOID").ToString()))%></span></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <span></li>
                <li visible="false" runat="server" id="liCheckBox" nums="<%#(Container.ItemIndex + 1).ToString()%>"
                    class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>'><span>
                        <ul class="clearfix w800">
                            <asp:Repeater ID="repeaterCheckBox" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input name="<%#Eval("SQID")%>" typename="<%=Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)%>"
                                            type="checkbox" value="<%#Eval("SOID")%>" class="dt" /><span onclick="emChkIsChoose(this)"
                                                style="cursor: pointer"><%#Eval("OptionName")%></span><input id="<%#Eval("SOID")%>"
                                                    name="<%#Eval("SQID")%>" type="text" value="" showflag="<%#Eval("IsBlank")%>"
                                                    style="border: none; border-bottom: #CCC 1px solid; background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" />
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <span></li>
                <li visible="false" runat="server" id="liText" nums="<%#(Container.ItemIndex + 1).ToString()%>"
                    class="xzt3"><span>
                        <ul class="clearfix">
                            <li>
                                <textarea name="<%#Eval("SQID")%>" maxlong="<%#Eval("MaxTextLen")%>" minlong="<%#Eval("MinTextLen") %>"
                                    style="width: 810px;"></textarea></li>
                        </ul>
                        <span></li>
                <li class="clearfix" style="width: 910px;" nums="<%#(Container.ItemIndex + 1).ToString()%>"
                    visible="false" runat="server" id="liMatrixRadio">
                    <%#GetTableHtmlForRadio(Eval("SQID").ToString())%>
                </li>
                <li class="clearfix" style="width: 910px;" nums="<%#(Container.ItemIndex + 1).ToString()%>"
                    visible="false" runat="server" id="liMatrixDropDown">
                    <%#GetHtmlForDropDown(Eval("SQID").ToString())%>
                </li>
            </ul>
        </div>
    </ItemTemplate>
</asp:Repeater>
