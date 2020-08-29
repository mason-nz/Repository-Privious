<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default1.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Default1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="Js/jquery-ui.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="Js/TemplateFiledData.js" type="text/javascript"></script>
    <script src="Js/Enum/Area.js" type="text/javascript"></script>
    <script src="Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="Js/Enum/ProvinceCityCountry.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/GooCalendar.js"></script>
    <script src="Js/controlParams.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/GooCalendar.css" />
    <link href="../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#divBaseInfo").sortable({
                connectWith: ".clearfix",
                cursor: "move"
            });


            GetHtmlByShowCode({ TFShowCode: '100001' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100002' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100003' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100004' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100005' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100006' }, function (returnData, html) {
                 $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100007' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100008' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100009' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100010' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100011' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100012' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });
            GetHtmlByShowCode({ TFShowCode: '100013' }, function (returnData, html) {
                $("#divBaseInfo").append(html);
                $("#divBaseInfo li").last().data(returnData);
            });

        });

        $(".clearfix li").live("mouseover", function () { $(this).addClass("showBorder"); });
        $(".clearfix li").live("mouseout", function () { $(this).removeClass("showBorder"); });


        function save() {

            //获取页面上的值
            var jsonArray = $(".baseInfo input").serializeArray();

            //验证表单数据
            var returnStr = validateMsg(jsonArray);
            var returnObj = $.evalJSON(returnStr);

            if (returnObj.result == "false") {
                alert(returnObj.msg);
                return false;
            }
        }

    </script>
    <script type="text/javascript">
        var property2 = {
            divId: "calen1", //日历控件最外层DIV的ID
            needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd hh:mm:ss"
            /*设定日期的输出格式,可不填*/
        };
        
    </script>
     
</head>
<body>
    <div class="w980">
        <div class="taskT">
            客户信息<input type="hidden" id="hdnCallRecordID"><input type="hidden" value="" id="hdnBeginRingTime"><span></span></div>
        <div class="baseInfo">
            <div class="title ft16">
                基本信息<span id="spanAddCust"><a style="display: none;" id="hrefAddCust" href="&amp;CalledNum=&amp;BeginRingTime=&amp;DataSource=">[新增客户]</a></span><a
                    class="toggle hide" onclick="divShowHideEvent('divBaseInfo',this)" href="javascript:void(0)"></a></div>
            <div>
                <ul id="divBaseInfo" class="clearfix ">
                    <li style="width: 800px;">
                        <label>
                            姓名：</label><span><input type="hidden" value="CB00084018" id="hdnCustID"><input type="text"
                                value="cyb联系记录1" class="w190" id="CustBaseInfo_txtCustName" name="CustBaseInfo$txtCustName"><span
                                    class="redColor">*</span></span></li>
                    <li>
                        <label>
                            地区：</label><select onchange="javascript:CustBaseInfoHelper.TriggerProvince()" class="w80"
                                id="CustBaseInfo_ddlProvince" name="CustBaseInfo$ddlProvince">
                                <option value="-1">省/直辖市</option>
                                <option value="1">安徽省</option>
                                <option value="820000">澳门特别行政区</option>
                                <option value="2">北京</option>
                                <option value="31">重庆</option>
                                <option value="3">福建省</option>
                            </select><select onchange="javascript:CustBaseInfoHelper.TriggerCity()" style="margin-left: 5px"
                                class="w80" id="CustBaseInfo_ddlCity" name="CustBaseInfo$ddlCity">
                                <option value="-1">城市</option>
                                <option value="102">安庆市</option>
                                <option value="103">蚌埠市</option>
                                <option value="125">亳州市</option>
                                <option value="104">巢湖市</option>
                            </select><select style="margin-left: 5px; " class="w80" id="CustBaseInfo_ddlCounty"
                                name="CustBaseInfo$ddlCounty" >
                                <option value="-1">区/县</option>
                                <option value="340823">枞阳县</option>
                            </select></li>
                    <li>
                        <label>
                            性别：</label><span><input type="radio" checked="checked" id="CustBaseInfo_rdoMan" name="CustBaseInfo$sex"
                                value="1">男<input type="radio" style="margin-left: 50px;" id="CustBaseInfo_rdoWomen"
                                    name="CustBaseInfo$sex" value="2">女<span class="redColor">*</span></span></li>
                    <li>
                        <label>
                            分属大区：</label><span><select class="w195" id="CustBaseInfo_ddlArea" name="CustBaseInfo$ddlArea">
                                <option value="0">请选择</option>
                                <option value="170001">北京大区</option>
                                <option value="170002">北方大区</option>
                                <option value="170003">南方大区</option>
                                <option value="170004" selected="selected">华东大区</option>
                                <option value="170005">西部大区</option>
                            </select></span></li>
                    <li>
                        <label>
                            电话：</label><span><input type="text" value="15010658596" class="w90" tag="custTel"
                                id="CustBaseInfo_txtTel1" name="CustBaseInfo$txtTel1"><input type="text" tag="custTel"
                                    class="w90" style="margin-left: 6px;" id="CustBaseInfo_txtTel2" name="CustBaseInfo$txtTel2"><span
                                        class="redColor">*</span></span><img border="0" onclick="javascript:ADTTool.openCallOutPopup(this,phone(),'','9');"
                                            src="../../../Images/phone.gif" style="cursor: pointer" alt="打电话" id="imgTel"></li>
                    <li>
                        <label>
                            地址：</label><span><input type="text" class="w190" id="CustBaseInfo_txtAddress" name="CustBaseInfo$txtAddress"></span></li>
                    <li>
                        <label>
                            邮箱：</label><span><input type="text" class="w190" id="CustBaseInfo_txtEmail" name="CustBaseInfo$txtEmail"></span></li>
                    <li>
                        <label>
                            数据来源：</label><span><select class="w195" id="CustBaseInfo_ddlDataSource" name="CustBaseInfo$ddlDataSource">
                                <option value="0">请选择</option>
                                <option value="180001">168</option>
                                <option value="180006">719</option>
                                <option value="180002">在线</option>
                                <option value="180003">汽车通</option>
                                <option value="180004">车易通</option>
                                <option value="180005">易湃</option>
                            </select></span></li>
                    <li>
                        <label>
                            客户分类：</label><span><input type="radio" checked="checked" onclick="CustTypeChangedEvent()"
                                id="CustBaseInfo_rdoHavCar" name="CustBaseInfo$custType" value="1">已购车<input type="radio"
                                    onclick="CustTypeChangedEvent()" style="margin-left: 30px;" id="CustBaseInfo_rdoNoCar"
                                    name="CustBaseInfo$custType" value="2">未购车<input type="radio" style="margin-left: 30px;"
                                        onclick="CustTypeChangedEvent()" id="CustBaseInfo_rdoDistributor" name="CustBaseInfo$custType"
                                        value="3">经销商<span class="redColor">*</span><input type="hidden" value="1" id="CustBaseInfo_hdnCustCategoryID"
                                            name="CustBaseInfo$hdnCustCategoryID"></span></li>
                    <li>
                        <label>
                            sdf</label>
                        <span>
                            <input type="text" style="width: 84px; *width: 83px; width: 83px\9;" class="w85"
                                onclick="MyCalendar.SetDate(this,this)" id="tfBeginTime" name="BeginTime"></span>
                    </li>
                </ul>
                <div style="margin: 10px; margin-left: 150px; *margin-left: 100px;" class="btn">
                    <input type="button" class="btnSave bold" onclick="save()" value="保 存" name="">&nbsp;&nbsp;
                </div>
            </div>
        </div>
    </div>
</body>
</html>
