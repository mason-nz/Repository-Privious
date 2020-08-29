//默认值
var defaultData = {
    RecID: '',
    TFCode: '',
    TFDesName: '字段名',
    TFName: '',
    TTCode: '',
    TTypeID: '',
    TFLen: '',
    TFDes: '',
    TFInportIsNull: '1',
    TFIsNull: '1',
    TFSortIndex: '',
    TFCssName: '',
    TFIsExportShow: '',
    TFIsListShow: '',
    TFShowCode: '',
    TFGenSubField: '',
    TFValue: ''
};

var divTwoIdNum = 0;
var returnData = null;

//点击文字，选中复选框
function emChkIsChoose(othis) {
    var $checkbox = $(othis).prev();

    if ($checkbox.attr("type") == "radio") {
        if ($checkbox.attr("disabled") != true) {
            $checkbox.attr("checked", "checked");
        }
    }
    else {
        if ($checkbox.attr("disabled") != true) {
            if ($checkbox.is(":checked")) {
                $checkbox.removeAttr("checked");
            }
            else {
                $checkbox.attr("checked", "checked");
            }
        }
    }
}
function GetHtmlByShowCode(ParaData, callback_func) {
    returnData = $.extend(returnData, defaultData, ParaData); //合并传入的值和默认值，如果传入的没有，则取默认值
    var htmlstr = "<li class='";
    if (returnData.TFShowCode == "100002") {
        returnData.TFCssName = 'hkId hkId2'; //设置为单占一行
    }
    if (returnData.TFShowCode == "100014") {
        returnData.TFDesName = "客户ID";
        returnData.TFSortIndex = 0;
        returnData.TFIsExportShow = "1";
        returnData.TFInportIsNull = "0";
        returnData.TFIsNull = "0";
        returnData.TFDes = "客户ID";
    }
    if (returnData.TFShowCode == "100015") {
        returnData.TFSortIndex = 0;
        returnData.TFIsExportShow = "1";
        returnData.TFInportIsNull = "0";
        returnData.TFIsNull = "0";
    }
    if (returnData.TFShowCode == "100016") {
        returnData.TFDesName = "下单车型";
    }
    if (returnData.TFShowCode == "100017") {
        returnData.TFDesName = "意向车型";
    }
    if (returnData.TFShowCode == "100018") {
        returnData.TFDesName = "出售车型";
    }
    if (returnData.TFShowCode == "100019") {
        returnData.TFDesName = "推荐活动";
        returnData.TFSortIndex = 0;
        returnData.TFIsExportShow = "0";
        returnData.TFInportIsNull = "1";
        returnData.TFLen = "4000";
    }
    if (returnData.TFShowCode == "100020") {
        returnData.TFSortIndex = 0;
        returnData.TFIsExportShow = "0";
        returnData.TFInportIsNull = "1";
        returnData.TFIsNull = "0";
    }

    if (returnData.TFCssName != "") {
        htmlstr += returnData.TFCssName;
    }
    htmlstr += "'>";
    htmlstr += "<label>";

    if (returnData.TFIsNull && returnData.TFIsNull == "0") {//必填
        htmlstr += "<em class='redColor'>*</em>"
    }
    htmlstr += returnData.TFDesName + "：</label>";
    //不同的类型，生成不同的HTML
    switch (returnData.TFShowCode) {
        case '100001': //单行文本
            htmlstr += "<span><input  type='text'  class='w280' id='" + returnData.TFName + "' lenstr='" + returnData.TFLen + "' name='" + returnData.TFName + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|Len' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
            }
            else {
                htmlstr += " vtype='Len' vmsg=\"" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
            }
            htmlstr += "></span>";
            break;
        case '100002': //多行文本
            htmlstr += "<span class='jyfw'><textarea  rows='' cols='' id='" + returnData.TFName + "' lenstr='" + returnData.TFLen + "' name='" + returnData.TFName + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|Len' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
            }
            else {
                htmlstr += " vtype='Len' vmsg=\"" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
            }
            htmlstr += "></textarea></span>";
            break;
        case '100003': //单选
            if (returnData.TFValue == "") {
                returnData.TFValue = '1|选项一;2|选项二';
            }
            htmlstr += "<span class='jyfw'>";
            $(returnData.TFValue.split(';')).each(function (i, v) {
                if (v != "") {
                    if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                        htmlstr += "<label for='" + returnData.TFName + i + "' style='cursor: pointer'><input type='radio' class='dx' vtype='isNull' vmsg='" + returnData.TFDesName + "必选' id='" + returnData.TFName + i + "' name='" + returnData.TFName + "_radioid' value='" + v.split('|')[0] + "'  textstr='" + v.split('|')[1] + "'/>" + v.split('|')[1] + "</label>";
                    } else {
                        htmlstr += "<label for='" + returnData.TFName + i + "'  style='cursor: pointer'><input type='radio' class='dx' id='" + returnData.TFName + i + "' name='" + returnData.TFName + "_radioid' value='" + v.split('|')[0] + "'  textstr='" + v.split('|')[1] + "'/>" + v.split('|')[1] + "</label>";
                    }
                }
            });
            htmlstr += "</span>";
            break;
        case '100004': //复选
            if (returnData.TFValue == "") {
                returnData.TFValue = '1|选项一;2|选项二';
            }
            htmlstr += "<span class='jyfw'>";
            $(returnData.TFValue.split(';')).each(function (i, v) {
                if (v != "") {
                    if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                        htmlstr += "<label for='" + returnData.TFName + i + "' style='float:none;cursor: pointer'><input type='checkbox' id='" + returnData.TFName + i + "' vtype='isNull' vmsg='" + returnData.TFDesName + "必选'  name='" + returnData.TFName + "_checkid'   value='" + v.split('|')[0] + "' textstr='" + v.split('|')[1] + "' />" + v.split('|')[1] + "</label>";
                    }
                    else {
                        htmlstr += "<label for='" + returnData.TFName + i + "' style='float:none;cursor: pointer'><input type='checkbox' id='" + returnData.TFName + i + "'  name='" + returnData.TFName + "_checkid'   value='" + v.split('|')[0] + "' textstr='" + v.split('|')[1] + "' />" + v.split('|')[1] + "</label>";
                    }
                }
            });
            htmlstr += "</span>";
            break;
        case '100005': //下拉
            if (returnData.TFValue == "") {
                returnData.TFValue = '1|选项一;2|选项二';
            }
            htmlstr += "<span><select class='w280'  id='" + returnData.TFName + "_selectid' name='" + returnData.TFName + "_selectid'";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='notFirstOption' vmsg='" + returnData.TFDesName + "项必须选择' ";
            }
            htmlstr += ">";
            htmlstr += "<option value='-1'>请选择</option>";
            $(returnData.TFValue.split(';')).each(function (i, v) {
                if (v != "") {
                    htmlstr += "<option value='" + v.split('|')[0] + "'>" + v.split('|')[1] + "</option>";
                }
            });
            htmlstr += "</select></span>";
            break;
        case '100006': //电话号码
            htmlstr += "<span><input type='text' class='w280' id='" + returnData.TFName + "' name='" + returnData.TFName + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isTelOrMobile' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "电话格式不正确\"";
            }
            else {
                htmlstr += " vtype='isTelOrMobile' vmsg='" + returnData.TFDesName + " 电话格式不正确' ";
            }
            htmlstr += "/></span>";
            break;
        case '100007': //邮箱
            htmlstr += "<span><input type='text' class='w280' id='" + returnData.TFName + "' name='" + returnData.TFName + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isEmail' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 邮箱格式不正确\"";
            }
            else {
                htmlstr += " vtype='isEmail' vmsg='" + returnData.TFDesName + " 邮箱格式不正确' ";
            }
            htmlstr += "/></span>";
            break;
        case '100008': //日期点
            htmlstr += "<span><input type='text' class='w280'  id='" + returnData.TFName + "' name='" + returnData.TFName + "' ";
            htmlstr += "   onclick='MyCalendar.SetDate(this,this)'  ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDate' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 日期格式不正确\"";
            }
            else {
                htmlstr += " vtype='isDate' vmsg='" + returnData.TFDesName + " 日期格式不正确' ";
            }
            htmlstr += "/></span>";
            break;
        case '100009': //日期段
            htmlstr += "<span class='jyfw'>";
            htmlstr += "<input type='text' class='w150'  onclick='MyCalendar.SetDate(this,this)'  id='" + returnData.TFName + "_startdata' name='" + returnData.TFName + "_startdata'  ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDate' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 日期格式不正确\"";
            }
            else {
                htmlstr += " vtype='isDate' vmsg='" + returnData.TFDesName + " 日期格式不正确' ";
            }
            htmlstr += ">-<input type='text'  class='w150'  onclick='MyCalendar.SetDate(this,this)'   id='" + returnData.TFName + "_enddata' name='" + returnData.TFName + "_enddata'  ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDate' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 日期格式不正确\"";
            }
            else {
                htmlstr += " vtype='isDate' vmsg='" + returnData.TFDesName + " 日期格式不正确' ";
            }
            htmlstr += "></span>";
            break;
        case '100010': //时间点
            divTwoIdNum = Number(divTwoIdNum) + 1;
            var id = returnData.TFName;
            if (id == "") {
                id = 'txtTime' + divTwoIdNum;
            }
            htmlstr += "<script type='text/javascript' language='javascript'>$(document).ready(function () {$.createGooCalendar('" + id + "', property2);});</script>";
            htmlstr += "<span><input type='text' class='w280'   id='" + id + "' name='" + id + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDateTime' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 时间格式不正确\"";
            }
            else {
                htmlstr += " vtype='isDateTime' vmsg='" + returnData.TFDesName + " 时间格式不正确' ";
            }
            htmlstr += "/></span>";
            break;
        case '100011': //时间段
            htmlstr += "<script type='text/javascript' language='javascript'>$(document).ready(function () {$.createGooCalendar('" + returnData.TFName + "_starttime', property2);});</script>";
            htmlstr += "<span class='jyfw'>";
            htmlstr += "<input type='text' class='w150'   id='" + returnData.TFName + "_starttime' name='" + returnData.TFName + "_starttime'  ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDateTime' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 时间格式不正确\"";
            } else {
                htmlstr += " vtype='isDateTime' vmsg='" + returnData.TFDesName + " 时间格式不正确' ";
            }
            htmlstr += "/>";
            htmlstr += "-";
            htmlstr += "<script type='text/javascript' language='javascript'>$(document).ready(function () {$.createGooCalendar('" + returnData.TFName + "_endtime', property2);});</script>";
            htmlstr += "<input type='text' class='w150'  id='" + returnData.TFName + "_endtime' name='" + returnData.TFName + "_endtime' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|isDateTime' vmsg=\"" + returnData.TFDesName + " 不能为空|" + returnData.TFDesName + " 时间格式不正确\"";
            } else {
                htmlstr += " vtype='isDateTime' vmsg='" + returnData.TFDesName + " 时间格式不正确' ";
            }
            htmlstr += "></span>";
            break;
        case '100012': //二级省市
            divTwoIdNum = Number(divTwoIdNum) + 1;
            var idstr = returnData.TFName;
            if (idstr == "") {
                idstr = returnData.TFName + "ej" + divTwoIdNum;
            }
            htmlstr += "<script type='text/javascript' language='javascript'>$(document).ready(function () {var oTwo = new Area({ BaseId: '" + idstr + "', type: 2,PClass: 'w90',CityClass:'w100',CountryClass:'w100' });});</script>";
            htmlstr += "<span class='erji'>";
            htmlstr += "<div id='" + idstr + "'></div>";
            htmlstr += "</span>";
            break;
        case '100013': //三级省市县
            divTwoIdNum = Number(divTwoIdNum) + 1;
            var idstr = returnData.TFName;
            if (idstr == "") {
                idstr = returnData.TFName + "sj" + divTwoIdNum;
            }
            htmlstr += "<script type='text/javascript' language='javascript'>$(document).ready(function () {var oTwo" + idstr + " = new Area({ BaseId:  '" + idstr + "', type: 3,PClass: 'w90',CityClass:'w100',CountryClass:'w100'});});</script>";
            htmlstr += "<span class='sanji'>";
            htmlstr += "<div id='" + idstr + "'></div>";
            htmlstr += "</span>";
            break;
        case '100014': //CRM客户ID
            htmlstr += "<span ><input crmcust='yes'  type='text'  class='w280' id='" + returnData.TFName + "_crmcustid_name' lenstr='" + returnData.TFLen + "' name='" + returnData.TFName + "' ";
            htmlstr += "></span>";
            break;
        case '100015': //个人用户 
            //如果是新增           
            if (returnData.TFDesName == "字段名") {
                htmlstr = "<li name='FullName'>";
                htmlstr += "<label><em class='redColor'>*</em>姓名：</label>";
                htmlstr += "<span  class=''>";
                htmlstr += "<input type='text' class='w280' id='' name=''  vtype='isTelOrMobile' vmsg='字段名 电话格式不正确' />";
                htmlstr += "</span>";
                htmlstr += "</li>";
                htmlstr += "<li name='FullSex'>";
                htmlstr += "<label><em class='redColor'>*</em>性别：</label>";
                htmlstr += "<span  class='jyfw'>";
                htmlstr += "<label for='0'  style='cursor: pointer'><input type='radio' class='dx' id='0' name='_radioid' value='1'  textstr='选项一'/>先生</label>";
                htmlstr += "<label for='1'  style='cursor: pointer'><input type='radio' class='dx' id='1' name='_radioid' value='2'  textstr='选项二'/>女士</label></span>";
                htmlstr += "</span>";
                htmlstr += "</li>";
                htmlstr += "<li name='FullTel'>";
                htmlstr += "<label><em class='redColor'>*</em>电话：</label>";
                htmlstr += "<span  class=''>";
                htmlstr += "<input type='text' class='w280' id='' name=''  vtype='isTelOrMobile' vmsg='字段名 电话格式不正确' />";
                htmlstr += "</span>";
                htmlstr += "</li>";
            }
            else {
                if (returnData.TFDesName == "姓名") {
                    htmlstr = "<li name='FullName'>";
                    htmlstr += "<label><em class='redColor'>*</em>姓名：</label>";
                    htmlstr += "<span><input  type='text'  class='w280' id='" + returnData.TFName + "' lenstr='" + returnData.TFLen + "' name='" + returnData.TFName + "' ";
                    if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                        htmlstr += " vtype='isNull|Len' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
                    }
                    else {
                        htmlstr += " vtype='Len' vmsg=\"" + returnData.TFDesName + "长度不能超过" + returnData.TFLen + "个字符\"";
                    }
                    htmlstr += "></span>";
                    htmlstr += "</li>";
                }
                else if (returnData.TFDesName == "性别") {
                    htmlstr = "<li name='FullSex'>";
                    htmlstr += "<label><em class='redColor'>*</em>性别：</label>";
                    htmlstr += "<span class='jyfw'>";
                    $(returnData.TFValue.split(';')).each(function (i, v) {
                        if (v != "") {
                            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                                htmlstr += "<label for='" + returnData.TFName + i + "' style='cursor: pointer'><input type='radio' class='dx' vtype='isNull' vmsg='" + returnData.TFDesName + "必选' id='" + returnData.TFName + i + "' name='" + returnData.TFName + "_radioid' value='" + v.split('|')[0] + "'  textstr='" + v.split('|')[1] + "'/>" + v.split('|')[1] + "</label>";
                            } else {
                                htmlstr += "<label for='" + returnData.TFName + i + "'  style='cursor: pointer'><input type='radio' class='dx' id='" + returnData.TFName + i + "' name='" + returnData.TFName + "_radioid' value='" + v.split('|')[0] + "'  textstr='" + v.split('|')[1] + "'/>" + v.split('|')[1] + "</label>";
                            }
                        }
                    });
                    htmlstr += "</span>";
                    htmlstr += "</li>";
                }
                else if (returnData.TFDesName == "电话") {
                    htmlstr = "<li name='FullTel'>";
                    htmlstr += "<label><em class='redColor'>*</em>电话：</label>";
                    htmlstr += "<span><input type='text' class='w280' id='" + returnData.TFName + "' name='" + returnData.TFName + "' ";
                    if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                        htmlstr += " vtype='isNull|isTelOrMobile' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "电话格式不正确\"";
                    }
                    else {
                        htmlstr += " vtype='isTelOrMobile' vmsg='" + returnData.TFDesName + " 电话格式不正确' ";
                    }
                    htmlstr += "/></span>";
                    htmlstr += "</li>";
                }
            }
            break;
        case '100016': //下单车型
            htmlstr = htmlstr
                                          + "<script> $(function () {"
                                            + "var options1 = {"
                                            + "container: { master: '" + returnData.TFName + "_XDBrand', serial: '" + returnData.TFName + "_XDSerial'},"
                                            + " include: { serial: '1', cartype: '1' },"
                                            + "datatype: 0,"
                                            + "binddefvalue: { master: '0', serial: '0'}"
                                            + "};"
                                            + "  new BindSelect(options1).BindList();});</script>";

            htmlstr += "<span class='erji'>";
            htmlstr += "<select id='" + returnData.TFName + "_XDBrand' class='w100 ListBrandSerial' name='XDBrand'></select>";
            htmlstr += "<select id='" + returnData.TFName + "_XDSerial' class='w90 ListBrandSerial' name='XDSerial'></select>";
            htmlstr += "</span>";
            break;
        case '100017': //意向车型
            htmlstr = htmlstr + "<script> $(function () {"
                                            + "var options2 = {"
                                                 + "container: { master: '" + returnData.TFName + "_YXBrand', serial: '" + returnData.TFName + "_YXSerial'},"
                                            + " include: { serial: '1', cartype: '1' },"
                                            + "datatype: 0,"
                                            + "binddefvalue: { master: '0', serial: '0'}"
                                            + "};"
                                            + "  new BindSelect(options2).BindList();});</script>";

            htmlstr += "<span class='erji'>";
            htmlstr += "<select id='" + returnData.TFName + "_YXBrand' class='w100 ListBrandSerial' name='YXBrand'></select>";
            htmlstr += "<select id='" + returnData.TFName + "_YXSerial' class='w90 ListBrandSerial' name='YXSerial'></select>";
            htmlstr += "</span>";
            break;
        case '100018': //出售车型
            htmlstr = htmlstr + "<script> $(function () {"
                                            + "var options3 = {"
                                            + "container: { master: '" + returnData.TFName + "_CSBrand', serial: '" + returnData.TFName + "_CSSerial'},"
                                            + " include: { serial: '1', cartype: '1' },"
                                            + "datatype: 0,"
                                            + "binddefvalue: { master: '0', serial: '0'}"
                                            + "};"
                                            + "  new BindSelect(options3).BindList();});</script>";

            htmlstr += "<span class='erji'>";
            htmlstr += "<select id='" + returnData.TFName + "_CSBrand' class='w100 ListBrandSerial' name='CSBrand'></select>";
            htmlstr += "<select id='" + returnData.TFName + "_CSSerial' class='w90 ListBrandSerial' name='CSSerial'></select>";
            htmlstr += "</span>";
            break;
        case '100019': //推荐活动
            htmlstr += "<span><input  type='text'  class='w280' id='" + returnData.TFName + "_Activity_Name' lenstr='" + returnData.TFLen + "' name='" + returnData.TFName + "' ";
            if (returnData.TFIsNull && returnData.TFIsNull == "0") {
                htmlstr += " vtype='isNull|Count' vmsg=\"" + returnData.TFDesName + "不能为空|" + returnData.TFDesName + "个数不能超过5个\"";
            }
            else {
                htmlstr += " vtype='Count' vmsg=\"" + returnData.TFDesName + "个数不能超过5个\"";
            }
            htmlstr += "></span><a href='javascript:void(0)' id='" + returnData.TFName + "_selectActivityA' style='cursor: pointer;'>查询</a>";
            htmlstr += "<input type='hidden' id='" + returnData.TFName + "_Activity' />";
            break;
        case '100020': //话务结果
            if (returnData.TFDesName == "是否接通") {
                htmlstr = "<li name='" + returnData.TFName + "' style='height:50px;'>";
                htmlstr += "<label id='" + returnData.TFName + "_label'><em class='redColor'>*</em>是否接通：</label>";
                htmlstr += "<span><select class='w280'  id='" + returnData.TFName + "_selectid' name='" + returnData.TFName + "_select'";
                htmlstr += " onchange='IsEstablish_Change(this);' >";
                htmlstr += "<option value='-1'>请选择</option>";
                $(returnData.TFValue.split(';')).each(function (i, v) {
                    if (v != "") {
                        htmlstr += "<option value='" + v.split('|')[0] + "'>" + v.split('|')[1] + "</option>";
                    }
                });
                htmlstr += "</select></span>";
                htmlstr += "</li>";
            }
            else if (returnData.TFDesName == "未接通原因") {
                htmlstr = "<li name='" + returnData.TFName + "' style='height:50px;'>";
                htmlstr += "<label id='" + returnData.TFName + "_label'><em class='redColor'>*</em>未接通原因：</label>";
                htmlstr += "<span><select class='w280'  id='" + returnData.TFName + "_selectid' name='" + returnData.TFName + "_select'";
                htmlstr += ">";
                htmlstr += "<option value='-1'>请选择</option>";
                $(returnData.TFValue.split(';')).each(function (i, v) {
                    if (v != "") {
                        htmlstr += "<option value='" + v.split('|')[0] + "'>" + v.split('|')[1] + "</option>";
                    }
                });
                htmlstr += "</select></span>";
                htmlstr += "</li>";
            }
            else if (returnData.TFDesName == "是否成功") {
                htmlstr = "<li name='" + returnData.TFName + "' style='height:50px;'>";
                htmlstr += "<label id='" + returnData.TFName + "_label'><em class='redColor'>*</em>是否成功：</label>";
                htmlstr += "<span><select class='w280'  id='" + returnData.TFName + "_selectid' name='" + returnData.TFName + "_select'";
                htmlstr += " onchange='IsSuccess_Change(this);' >";
                htmlstr += "<option value='-1'>请选择</option>";
                $(returnData.TFValue.split(';')).each(function (i, v) {
                    if (v != "") {
                        htmlstr += "<option value='" + v.split('|')[0] + "'>" + v.split('|')[1] + "</option>";
                    }
                });
                htmlstr += "</select></span>";
                htmlstr += "</li>";
            }
            else if (returnData.TFDesName == "失败原因") {
                htmlstr = "<li name='" + returnData.TFName + "' style='height:50px;'>";
                htmlstr += "<label id='" + returnData.TFName + "_label'><em class='redColor'>*</em>失败原因：</label>";
                htmlstr += "<span><select class='w280'  id='" + returnData.TFName + "_selectid' name='" + returnData.TFName + "_select'";
                htmlstr += ">";
                htmlstr += "<option value='-1'>请选择</option>";
                $(returnData.TFValue.split(';')).each(function (i, v) {
                    if (v != "") {
                        htmlstr += "<option value='" + v.split('|')[0] + "'>" + v.split('|')[1] + "</option>";
                    }
                });
                htmlstr += "</select></span>";
                htmlstr += "</li>";
            }
            break;
    }
    if (typeof callback_func == "function") {
        callback_func(returnData, htmlstr);
    }
    return htmlstr;
}
function IsEstablish_Change(select) {
    var a = $(select).val();
    if (a == 1) {
        SetTemplateLiShowOrHid("NotEstablishReason", false);
        SetTemplateLiShowOrHid("IsSuccess", true);
        SetTemplateLiShowOrHid("NotSuccessReason", false);

        //未接通，是否成功不能有【是】
//        $("#IsSuccess_selectid option[value='1']").attr("disabled", "");
//        $("#IsSuccess_selectid option[value='-1']").attr("disabled", "");

        $("#IsSuccess_selectid").attr("disabled", "");
    }
    else if (a == 0) {
        SetTemplateLiShowOrHid("NotEstablishReason", true);
        SetTemplateLiShowOrHid("IsSuccess", true);
        SetTemplateLiShowOrHid("NotSuccessReason", false);

        //未接通，是否成功不能有【是】
//        $("#IsSuccess_selectid option[value='1']").attr("disabled", "disabled");
//        $("#IsSuccess_selectid option[value='-1']").attr("disabled", "disabled");
        $("#IsSuccess_selectid").val("0");
        $("#IsSuccess_selectid").attr("disabled", "disabled");
    }
    else {
        SetTemplateLiShowOrHid("NotEstablishReason", false);
        SetTemplateLiShowOrHid("IsSuccess", false);
        SetTemplateLiShowOrHid("NotSuccessReason", false);
    }
}
function IsSuccess_Change(select) {
    var a = $("#IsEstablish_selectid").val();
    var b = $(select).val();
    //接通 且 失败
    if (a == 1 && b == 0) {
        SetTemplateLiShowOrHid("NotSuccessReason", true);
    }
    else {
        SetTemplateLiShowOrHid("NotSuccessReason", false);
    }
}

function SetTemplateLiShowOrHid(li_name, b) {
    SetTemplateLiShowOrHid_NOC(li_name, b);
    $("#" + li_name + "_selectid").val("-1");
}
function SetTemplateLiShowOrHid_NOC(li_name, b) {
    if (b) {
        $("#" + li_name + "_label").show();
        $("#" + li_name + "_selectid").show();
    }
    else {
        $("#" + li_name + "_label").hide();
        $("#" + li_name + "_selectid").hide();
    }
}

function InitChange(select_e, select_s) {
    var a = $(select_e).val();
    var b = $(select_s).val();

    if (a == 1) {
        SetTemplateLiShowOrHid_NOC("NotEstablishReason", false);
        SetTemplateLiShowOrHid_NOC("IsSuccess", true);
        SetTemplateLiShowOrHid_NOC("NotSuccessReason", false);

        //未接通，是否成功不能有【是】
//        $("#IsSuccess_selectid option[value='1']").attr("disabled", "");
//        $("#IsSuccess_selectid option[value='-1']").attr("disabled", "");

        $("#IsSuccess_selectid").attr("disabled", "");
    }
    else if (a == 0) {
        SetTemplateLiShowOrHid_NOC("NotEstablishReason", true);
        SetTemplateLiShowOrHid_NOC("IsSuccess", true);
        SetTemplateLiShowOrHid_NOC("NotSuccessReason", false);

        //未接通，是否成功不能有【是】
//        $("#IsSuccess_selectid option[value='1']").attr("disabled", "disabled");
//        $("#IsSuccess_selectid option[value='-1']").attr("disabled", "disabled");


        $("#IsSuccess_selectid").val("0");
        $("#IsSuccess_selectid").attr("disabled", "disabled");
    }
    else {
        SetTemplateLiShowOrHid_NOC("NotEstablishReason", false);
        SetTemplateLiShowOrHid_NOC("IsSuccess", false);
        SetTemplateLiShowOrHid_NOC("NotSuccessReason", false);
    }

    //接通 且 失败
    if (a == 1 && b == 0) {
        SetTemplateLiShowOrHid_NOC("NotSuccessReason", true);
    }
    else {
        SetTemplateLiShowOrHid_NOC("NotSuccessReason", false);
    }
}

function CheckChange(name) {
    var select = $("#" + name + "_selectid");
    if (select.css("display") != "none") {
        if (select.val() == "-1") {
            return false;
        }
    }
    return true;
}

//设置当是否接通选择"是"的时候,未接通原因为-1,是否接通不可用
function SetIsEstablishTrue(select_e, select_s) {
    $(select_e).val(1);
    $(select_e).attr("disabled", "disabled");  //当接通时，选择"是"的时候，不可用
    InitChange(select_e, select_s);
    $("#NotEstablishReason_selectid").val(-1);
}