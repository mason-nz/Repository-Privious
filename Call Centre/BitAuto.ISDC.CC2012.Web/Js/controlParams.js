///需引用jquery.js和common.js

//验证
function validateMsg(formArray) {
    var returnMsg = "";
    var msg = "";
    for (var i = 0; i < formArray.length; i++) {
        var arry = $("[name=" + formArray[i].name + "]:eq(0)");

        if (typeof (arry.attr("vtype")) == "undefined") {
            continue;
        }

        //如果有验证信息
        var types = arry.attr("vtype").split("|"); //验证类型 格式：isNull|isTel
        var vmsgs = arry.attr("vmsg").split("|"); //提示信息 格式：不为空|号码有误 

        var value = $.trim(arry.val());
        for (var k = 0; k < types.length; k++) {

            var type = types[k];

            var vmsg = vmsgs[k];
            switch (type) {

                case "isNull": if (value == "") {
                        msg += vmsg + "<br/>"; //验证不能为空
                    }
                    break;

                case "Len":
                    var leng = arry.attr("lenstr");
                    if (parseInt(leng) < GetStringRealLength(value)) {
                        msg += vmsg + "<br/>"; //验证长度
                    }
                    break;

                case "notFirstOption":
                    var optionFirstVal = arry.find("option:eq(0)").val();
                    if (arry.find("option").length > 1 && value == optionFirstVal) {
                        msg += vmsg + "<br/>"; //验证下拉列表，不能选择第一个选项
                    }
                    break;

                case "checkNum": if (value != "") {
                        if (!checkNum(value)) {
                            msg += vmsg + "<br/>"; //验证是否为数字
                        }
                    }
                    break;

                case "isNum": if (value != "") {
                        if (!isNum(value)) {
                            msg += vmsg + "<br/>"; //验证数字
                        }
                    }
                    break;

                case "isMoney":
                    if (value != "") {
                        if (!isMoney(value)) {
                            msg += vmsg + "<br/>"; //验证数字(包括整数、浮点数)
                        }
                    }
                    break;

                case "isMoneyAbs":
                    if (value != "") {
                        if (!isMoney(Math.abs(value))) {
                            msg += vmsg + "<br/>"; //验证绝对值（即可以为负数）的数字格式(包括整数、浮点数)
                        }
                    }
                    break;

                case "isFloat": if (value != "") {
                        if (!isFloat(value)) {
                            msg += vmsg + "<br/>"; //验证浮点
                        }
                    }
                    break;

                case "isDate":
                    if (value != "") {
                        if (!value.isDate()) {
                            msg += vmsg + "<br/>"; //验证日期格式
                        }
                    }
                    break;
                case "isDateTime":
                    if (value != "") {
                        if (!value.isDateTime()) {
                            msg += vmsg + "<br/>"; //验证时间格式
                        }
                    }
                    break;

                case "isMobile": if (value != "") {
                        if (!isMobile(value)) {
                            msg += vmsg + "<br/>"; //验证手机格式
                        }
                    }
                    break;

                case "isTel": if (value != "") {
                        if (!isTel(value)) {
                            msg += vmsg + "<br/>"; //验证电话格式
                        }
                    }
                    break;

                case "isTelOrMobile": if (value != "") {
                        //alert("controlParam000001:"+ value);
                        if (!isTelOrMobile(value)) {
                            msg += vmsg + "<br/>"; //验证电话或者手机验证
                        }
                    }
                    break;

                case "isEmail": if (value != "") {
                        if (!isEmail(value)) {
                            msg += vmsg + "<br/>"; //验证邮件
                        }
                    }
                    break;

                case "checkIdcard": if (value != "") {
                        if (!checkIdcard(value)) {
                            msg += vmsg + "<br/>"; //验证身份证
                        }
                    }
                    break;
                //加推荐活动个数限制  
                case "Count": if (value != "") {
                        if (value.split(',').length > 5) {
                            msg += vmsg + "<br/>";
                        }
                    }
            }
        }
    }

    if (msg == "") {
        //如果msg为空，则可以直接获取参数
        var pody = showSearchList.getParams(formArray);
        returnMsg = "{\"result\":\"true\",\"pody\":" + pody + "}";
    }
    else {
        //如果msg不为空，则返回提示信息
        returnMsg = "{\"result\":\"false\",\"msg\":\"" + msg + "\"}";
    }
    return returnMsg;
}


var showSearchList = window.showSearchList = (function () {

    var thisPageListUrl = "",

    //列表查询  
    //listURL：列表页地址；formID：包含条件的外层框ID；showListDivID：列表加载的div
        getList = function (listURL, formID, showListDivID) {
            var arry = $("#" + formID).serializeArray();

            var jsonData = $.evalJSON(validateMsg(arry));

            //如果验证失败，则显示提示信息
            if (jsonData.result == "false") {
                $.jAlert(jsonData.msg, function () {
                    return false;
                });
            }
            else {
                thisPageListUrl = listURL;

                var pody = $("#" + formID).find("[name!='__VIEWSTATE'][name!='__EVENTVALIDATION']").fixedSerialize();

                ShowDataByPost1(pody, showListDivID);
            }
        },

    //获取参数
        getParams = function (formArray) {
            var pody = "";
            var _params = " Action: '', r: " + Math.random() + ", ID: '',";      //定义一个Action和随机数、ID，Action、ID可以给它赋值

            //循环参数名称和值
            for (var i = 1; i < formArray.length; i++) {

                var arry = $("[name=" + formArray[i].name + "]:eq(0)");

                var $Value = getValue(arry);    //控件值

                //以键值对的形式表示
                _params += arry.attr("name") + ":'" + $Value + "',";
            }

            pody = "{" + _params.substring(0, _params.length - 1) + "}";

            return pody;
        },

    //加载列表
        ShowDataByPost1 = function (pody, showListDivID) {
            LoadingAnimation(showListDivID);
            //*添加监控
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $("#" + showListDivID).load(thisPageListUrl, { JsonStr: pody }, function () {
                StatAjaxPageTime(monitorPageTime, thisPageListUrl + "?" + pody);
                //*添加监控
                //如果存在这个formatStandardShortDate方法，执行
                if ($.isFunction(window.formatStandardShortDate)) {
                    formatStandardShortDate();
                }

            });
        },

    //获取某个控件或某种控件类型（checkbox、radio）的值
        getValue = function (othis) {
            var _value = "";
            if (othis.attr("type") == "checkbox" || othis.attr("type") == "radio") {   //如果是checkbox、radio类型控件
                _value = encodeURIComponent($("input[name='" + othis.attr("name") + "']:checked").map(function () {
                    return $(this).val();
                }).get().join(','));
            }
            else {
                _value = encodeURIComponent($.trim(othis.val()));
            }

            return _value;
        };

    return {
        getList: getList,
        getParams: getParams
    }

})();


//修改serialize()方法，在有checkbox时，将选中的相同name的值合并成一个用逗号隔开的串；如&chkStatus=1&chkStatus=2  变成 &chkStatus=1,2

$.fn.extend({
    "fixedSerialize": function () {
        var $f = $(this);
        var data = $(this).serialize();
        data = data.replace(/\+/g, ""); //将条件的空格替换成空
        var $chks = $(this).find(":checkbox:checked");    //取得选中的checkbox  

        if ($chks.length == 0) {
            return data;
        }
        var nameArr = [];
        var tempStr = "";
        $chks.each(function () {
            var chkName = $(this).attr("name");
            if ($.inArray(chkName, nameArr) == -1 && $f.find(":checkbox[name='" + chkName + "']:checked").length > 1) {
                nameArr.push(chkName);
                tempStr += "&" + chkName + "=" + $f.find(":checkbox[name='" + chkName + "']:checked").map(function () {
                    //删除单个的值
                    data = data.replace("&" + chkName + "=" + $(this).val(), "");
                    return $(this).val();
                }).get().join(',');
            }
        });
        data += tempStr;

        return data;
    }
});

//将对象显示的长时间类型 改成 短时间类型 例：2012-10-9 12:09:120 -> 2012-10-9
(function ($) {
    $.fn.formatTime = function () {

        $(this).each(function () {

            var $this = $(this);
            var val = $.trim($this[$this.is("input,select,textarea") ? "val" : "html"]());
            //如果获取的值既不符合长时间类型也不符合短时间类型 return
            if (!val.isDate() && !val.isDateTime()) {
                return;
            }

            var arry = val.split("-");

            if (arry.length == 3) {
                var dateStr = arry[0] + "-" + arry[1] + "-" + arry[2].substr(0, 2);
                if ($.trim(dateStr) == "1900-1-1") {
                    $this[$this.is("input,select,textarea") ? "val" : "html"]('');
                }
                else {
                    $this[$this.is("input,select,textarea") ? "val" : "html"](arry[0] + "-" + arry[1] + "-" + arry[2].substr(0, 2));
                }
            }
        });
    }
} (jQuery));


//将table中显示的短时间或长时间类型 改成 长时间标准类型 例：2012-7-9 12:09:120 -> 2012-07-09 12:09:120
function formatStandardShortDate() {
    $("table tr").each(function () {

        $(this).find("td").each(function () {

            var $this = $(this);

            var val = $(this).html().replace("&nbsp;", "");

            var arry = val.split("-");

            if (arry.length == 3) {

                if (val.indexOf("1900") > -1) {
                    $this.html('&nbsp;'); //需要加&nbsp;否则在没内容情况下表格会消失
                    return;
                }
                var month = Len(arry[1]) == 1 ? ("0" + arry[1]) : arry[1];

                var day = Len($.trim(arry[2].substr(0, 2))) == 1 ? ("0" + arry[2]) : arry[2];

                var dateStr = arry[0] + "-" + month + "-" + day;

                $this[$this.is("input,select,textarea") ? "val" : "html"](dateStr);
            }
        });

    });
}

//获取每个需要验证的控件信息，传到后台进行验证
function GetAreaValidateMsg(id) {

    var validateMsg = "{ 'ControlInfo': [{ 'Value': '', 'ControlType': '', 'VType': '', 'VMsg': '', 'OptionLen': '', 'FirstOptionVal': '', MaxLen: '' },";

    $.each($("#" + id).children().find("input"), function (i, n) {
        var $this = $(this);

        if (!$this.is(":hidden")) {
            validateMsg += "{ 'Value': '" + $this.val() + "', 'ControlType': '" + $this[0].type + "', 'VType':'" + $this.attr('vtype') + "' , 'VMsg':'" + $this.attr('vmsg') + "' , 'OptionLen': '', 'FirstOptionVal': '', MaxLen: '" + $this.attr('lenstr') + "' },";
        }
    });

    $.each($("#" + id).children().find("select"), function (i, n) {
        var $this = $(this);

        if (!$this.is(":hidden")) {
            validateMsg += "{ 'Value':'" + $("#" + $this.attr('id')).val() + "' , 'ControlType':'" + $this[0].type + "' , 'VType':'" + $this.attr('vtype') + "', 'VMsg': '" + $this.attr('vmsg') + "', 'OptionLen':'" + $this.find('option').length + "', 'FirstOptionVal':'" + $this.find('option:eq(0)').val() + "', MaxLen: '" + $this.attr('lenstr') + "'  },";
        }
    });

    validateMsg = validateMsg.substring(0, validateMsg.length - 1) + "]}";

    return validateMsg;
}