<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyCarInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.BuyCarInfo" %>
<script type="text/javascript" language="javascript">
    ///品牌和车型
    function selectBrandAndSerialInfo(txtBoxobj) {
        var name = $.trim($("#" + txtBoxobj).val())

        $.openPopupLayer({
            name: "BrandSelectAjaxPopup",
            parameters: { name: escape(name) },
            url: "../../../AjaxServers/CustCategory/SelectBrandAndSerial.aspx",
            beforeClose: function (e, data) {

                if ($('#popupLayer_' + 'BrandSelectAjaxPopup').data('isSelected') == "1") {
                    //如果是选择后关闭
                    var retName = $('#popupLayer_' + 'BrandSelectAjaxPopup').data('serialname');
                    $("#" + txtBoxobj).val(retName);
                }
            }
        });
    }
    //验证
    function DataCheck() {
        var txtagevalue = $("#txtAge").val();

        if (txtagevalue != "") {
            if (txtagevalue.length >= 4) {
                $("#txtAge").focus();
                $.jAlert('年龄超长！');
                return false;
            }
        }
        if ($("#txtIDCards").val() != "") {

            if (!isNum($("#txtIDCards").val())) {
                $.jAlert('身份证号码输入错误！');
                return false;
            }
            if ($("#txtIDCards").val().length != 15 && $("#txtIDCards").val().length != 18) {
                $("#txtIDCards").focus();
                $.jAlert('身份证号码输入错误！');
                return false;
            }
        }
        if ($("#txtDriveAge").val() != "") {
            if ($("#txtDriveAge").val().length >= 4) {
                $("#txtDriveAge").focus();
                $.jAlert('驾龄超长！');
                return false;
            }
        }
        if ($("#txtCarNumber").val() != "") {
            if ($("#txtCarNumber").val().length > 7) {
                $("#txtCarNumber").focus();
                $.jAlert('车牌号码超长！');
                return false;
            }
        }

        if ($("select[id$='dllMyBrand']").val() == null || $("#dllMySerial").val() == null || $("#dllMyName").val() == null || $("select[id$='dllMyBrand']").val() == undefined || $("#dllMySerial").val() == undefined || $("#dllMyName").val() == undefined) {
            $.jAlert("汽车品牌信息未加载完成，请稍候再试。");
            return false;
        }

         if ($("#dllMyBrand").val() != "0" && ($("#dllMySerial").val() == "0" || $("#dllMyName").val() == "0")) {
            $("#dllMyBrand").focus();
            $.jAlert('请选择目前驾驶品牌！');
            return false;
        }

        return true;
    }
    //只允许输入数字
    function NumberCheck(event, object) {
        if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {

        } else {
            object.attr("value", "");
        }
    }

    function subBuyCarInfo() {
        var returnValue = false;
        var age = $("#txtAge").val();
        var IDCards = $("#txtIDCards").val();
        var Vocation = $("select[id$='selVocation']").val();
        var InCome = $("select[id$='selInCome']").val();
        var Marriage = $("input[name='radMarriage']:checked").val();
        if (Marriage == undefined) {
            Marriage = "";
        }
        var CarBrandID = $("select[id$='dllMyBrand']").val();
        var CarSerialID = $("select[id$='dllMySerial']").val();
        var CarTypeID = $("select[id$='dllMyName']").val();
        var CarName = $("#dllMyName").find("option:selected").text();
        var legalize = $("input[name='legalize']:checked").val();
        if (legalize == undefined) {
            legalize = "";
        }

        var DriveAge = $("#txtDriveAge").val();
        var UserName = $("#txtUserName").val();
        var CarNumber = $("#txtCarNumber").val();
        var Remark = $("#txtNote").val();
        var Type = 0;
        $("input[name$='custType']").each(function () {
            if ($(this).attr("checked")) {
                Type = $(this).val();
            }
        });

        var TaskID = '<%=TaskID%>';
        //如果验证通过再进行保存
        if (DataCheck()) {
            params = {
                Action: "SubBuyCarInfoInfo",
                Age: encodeURIComponent(age),
                IDCard: encodeURIComponent(IDCards),
                Vocation: encodeURIComponent(Vocation),
                InCome: encodeURIComponent(InCome),
                Marriage: encodeURIComponent(Marriage),
                CarBrandID: encodeURIComponent(CarBrandID),
                CarSerialID: encodeURIComponent(CarSerialID),
                CarTypeID: encodeURIComponent(CarTypeID),
                CarName: escape(CarName),
                IsAttestation: encodeURIComponent(legalize),
                DriveAge: encodeURIComponent(DriveAge),
                UserName: escape(UserName),
                CarNo: escape(CarNumber),
                Remark: escape(Remark),
                Type: encodeURIComponent(Type),
                TaskID: encodeURIComponent(TaskID)
            }
            AjaxPostAsync("../../../AjaxServers/TaskManager/NoDealerOrder/BuyCarInfo.ashx", params,
                    function () {
                        $("#btnSave").attr("disabled", "disabled");
                        $("#btnSumbit").attr("disabled", "disabled");
                        $("#imgLoadingPop").show();
                    },
                    function (data) {

                        if (data == 'success') {
                            returnValue = true;
                        }
                        else {
                            $.jAlert(data);
                            returnValue = false;
                        }
                        $("#btnSave").attr("disabled", "");
                        $("#btnSumbit").attr("disabled", "");
                        $("#imgLoadingPop").hide();
                    });
        }
        return returnValue;
    }

    $(document).ready(function () {
        //绑定目前驾驶品牌
        BindMyBrand();
        $("#txtAge").keyup(function (event) {
            if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
            } else {
                $('#txtAge').attr("value", "");
            }
        });
        //        $("#txtIDCards").keyup(function (event) {
        //            if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
        //            } else {
        //                $('#txtIDCards').attr("value", "");
        //            }
        //        });
        $("#txtDriveAge").keyup(function (event) {
            if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
            } else {
                $('#txtDriveAge').attr("value", "");
            }
        });



        var Remark = '<%=BuyCarInfoModel.Remark%>'
        if (Remark != null && Remark != "") {
            $('#txtNote').val(Remark);
        }
        var Age = '<%=BuyCarInfoModel.Age%>'
        if (Age != null && Age != "" && Age != "-2") {
            $('#txtAge').val(Age);
        }
        var IDCard = '<%=BuyCarInfoModel.IDCard%>'
        if (IDCard != null && IDCard != "") {
            $('#txtIDCards').val(IDCard);
        }
        var Vocation = '<%=BuyCarInfoModel.Vocation%>'
        if (Vocation != null && Vocation != "") {
            $('#selVocation').val(Vocation);
        }

        var Marriage = '<%=BuyCarInfoModel.Marriage%>'
        if (Marriage != null && Marriage != "") {
            if (Marriage == "1")
            { $('#radMarriageYes').attr("checked", true) }
            if (Marriage == "0")
            { $('#radMarriageNo').attr("checked", true) }
        }


        var Income = '<%=BuyCarInfoModel.Income%>'
        if (Income != null && Income != "") {
            $('#selInCome').val(Income);
        }

        var IsAttestation = '<%=BuyCarInfoModel.IsAttestation%>'
        if (IsAttestation != null && IsAttestation != "") {
            if (IsAttestation == "1")
            { $('#radYes').attr("checked", true) }
            if (IsAttestation == "0")
            { $('#radNo').attr("checked", true) }
        }

        //        var CarName = '<%=BuyCarInfoModel.CarName%>'
        //        if (CarName != null && CarName != "") {
        //            $('#txtCarName').val(CarName);
        //        }

        var DriveAge = '<%=BuyCarInfoModel.DriveAge%>'
        if (DriveAge != null && DriveAge != "" && DriveAge != "-2") {
            $('#txtDriveAge').val(DriveAge);
        }
        var UserName = '<%=BuyCarInfoModel.UserName%>'
        if (UserName != null && UserName != "") {
            $('#txtUserName').val(UserName);
            $("#btnRegister").hide(); //当汽车通被注册时，注册 按钮隐藏
        }
        var CarNo = '<%=BuyCarInfoModel.CarNo%>'
        if (CarNo != null && CarNo != "") {
            $('#txtCarNumber').val(CarNo);
        }
    });

    //绑定品牌信息
    function BindMyBrand() {

        //目前驾驶的车
        var options4 = {
            container: { master: "dllMyBrand", serial: "dllMySerial", cartype: "dllMyName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '<%=CarBrandID%>', serial: '<%=CarSerialID%>', cartype: '<%=CarTypeID%>' }
        };
        new BindSelect(options4).BindList();


    }
</script>

<script type="text/javascript">
    //注册汽车通 add lxw 13.4.12
    function registerCarTong(othis) {
        var telNum = $("[id^='EditCustBaseInfo1_txtTel'][value!='']").length;
        var telPhone = $("[id^='EditCustBaseInfo1_txtTel'][value!='']").map(function () {
            return $(this).val();
        }).get().join(',');

        var msg = "";
        $("[id^='EditCustBaseInfo1_txtTel'][value!='']").each(function () {
            if (!isTelOrMobile($(this).val())) {
                msg += "手机号码 " + $(this).val() + " 格式不正确<br/>";
            }
        });

        if (msg != "") {
            $.jAlert(msg);
            return false;
        }

        if (telNum == 0) {
            $.jAlert("电话号码不能为空", function () {
                $("[id^='EditCustBaseInfo1_txtTel']:eq(0)").focus();
            });
            return false;
        }

        if (telNum == 1) {
            registerByWebService(telPhone);
        }
        else if (telNum == 2) {
            openPhonePopup(othis, telPhone);
        }
    }
    //调用web服务接口注册车商通
    function registerByWebService(phoneNumber) {
        $.blockUI({ message: "正在注册，请等待..." });
        AjaxPost("/AjaxServers/CustCategory/BuyCarInfo.ashx", { Action: "RegisterCarTong", PhoneNumber: encodeURIComponent(phoneNumber), TaskID: encodeURIComponent("<%=TaskID %>"), YPOrderID: encodeURIComponent("<%=YPOrderID %>"), r: Math.random() }, null,
        function (data) {
            $.unblockUI();
            var jsonData = eval("(" + data + ")");
            if (jsonData.result == 'yes') {
//                $.jAlert("注册成功", function () {
//                    $("#txtUserName").val(jsonData.mobile + "/" + jsonData.pwd);
//                    $("#btnRegister").hide(); //当汽车通注册成功时，注册 按钮隐藏
                //                });
                $.jPopMsgLayer("注册成功", function () {
                    $("#txtUserName").val(jsonData.mobile + "/" + jsonData.pwd);
                    $("#btnRegister").hide(); //当汽车通注册成功时，注册 按钮隐藏
                });
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
    }

    //加载电话号码层
    function openPhonePopup(othis, phoneNumText) {//若有多个呼出号码，则弹出层 
        if (window.ADTToolPopupContent) {
            window.ADTToolPopupContent.remove();
            $(document).unbind('mousemove', f1);
            window.ADTToolPopupContent = null;
        }

        var co = $(othis); if (co.length == 0) { return; }

        var content = $('<div class="open_tell" style="position:absolute;"></div>');
        var ul = $('<ul class="list"></ul>');
        var array = phoneNumText.split(',');
        var i;
        for (i = 0; i < array.length; i++) {
            var v = array[i]; //alert(v);
            var ulObj = $('<li>').append($('<span>').html(v));
            var aObj = $("<a onclick='registerByWebService(" + v + ")' href='javascript:void(0);'>注册</a>");
            ul.append(ulObj.append(aObj));
        }
        content.append(ul).append('<em></em>').appendTo($('body'));
        height = 25 * (i + 1) + 29;
        ttop = (co.offset().top - height) + 100;
        tleft = co.offset().left - 21;
        content.css({ left: tleft, top: ttop });
        window.ADTToolPopupContent = content;
        $(document).bind('mousemove', f1);
    }
    var ttop = 0;
    var tleft = 0;
    var theight = 0;
    f1 = function (e) {
        if (e.pageX < tleft || e.pageX > tleft + 180 || e.pageY < ttop || e.pageY > ttop + theight + 80) {
            $(document).unbind('mousemove', f1);
            window.ADTToolPopupContent.remove();
        }
    }
</script>
<div class="line">
</div>
<ul class="clearfix ">
    <li>
        <label>
            <input type="hidden" id="hidType" value='<%=Type%>' />
            年龄：</label><span><input type="text" id="txtAge" class="w50" />&nbsp;岁</span></li>
    <li>
        <label>
            身份证号：</label><span><input type="text" id="txtIDCards" class="w250" /></span></li>
    <li>
        <label>
            职业：</label><span><select id="selVocation" class="w255">
                <option value="-2" selected="selected">请选择</option>
                <option value="130001">一般职业</option>
                <option value="130002">农牧业</option>
                <option value="130003">渔业</option>
                <option value="130004">木材森林业</option>
                <option value="130005">矿业采掘业</option>
                <option value="130006">交通运输业</option>
                <option value="130007">餐饮旅游业</option>
                <option value="130008">建筑工程</option>
                <option value="130009">制造加工维修业</option>
                <option value="130010">出版广告业</option>
                <option value="130011">医药卫生保健</option>
                <option value="130012">娱乐业</option>
                <option value="130013">文教机构</option>
                <option value="130014">宗教机构</option>
                <option value="130015">邮政通信电力自来水</option>
                <option value="130016">零售批发业</option>
                <option value="130017">金融保险证券</option>
                <option value="130018">家庭管理</option>
                <option value="130019">公检法等执法检查机关</option>
                <option value="130020">军人</option>
                <option value="130021">IT业（软硬件开发制作）</option>
                <option value="130022">职业运动</option>
                <option value="130023">无业人员</option>
                <option value="130024">其他</option>
            </select></span></li>
    <li>
        <label>
            婚姻状态：</label><span><input type="radio" id="radMarriageYes" name="radMarriage" value="1" />已婚</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><input
                type="radio" id="radMarriageNo" name="radMarriage" value="0" />未婚</span></li>
    <li>
        <label>
            个人收入：</label><span><select id="selInCome" class="w255">
                <option value="-2" selected="selected">请选择</option>
                <option value="140001">1000元/月以下</option>
                <option value="140002">1000-2000元/月</option>
                <option value="140003">2001-4000元/月</option>
                <option value="140004">4001-6000元/月</option>
                <option value="140005">6001-8000元/月</option>
                <option value="140006">8001-10000元/月</option>
                <option value="140007">10001-15000元/月</option>
                <option value="140008">15001-25000元/月</option>
                <option value="140009">25000元/月以上</option>
                <option value="140010">保密</option>
            </select></span></li><li></li>
</ul>
<div class="line">
</div>
<ul class="clearfix ">
    <li>
        <label>
            目前驾驶：</label>
        <span>
            <select id="dllMyBrand" name="BrandId" class="w125" style="width: 82px;">
            </select>
            <select id="dllMySerial" name="SerialId" class="w125" style="width: 82px;">
            </select>
            <select id="dllMyName" name="NameId" class="w125" style="width: 82px;"  onmouseover="javascript:FixWidth(this);">
            </select>
        </span></li>
    <li>
        <label>
            是否认证车主：</label>
        <span>
            <input type="radio" id="radYes" name="legalize" value="1" />是</span>&nbsp;&nbsp;&nbsp;&nbsp;<span><input
                type="radio" id="radNo" name="legalize" value="0" />否</span></li>
    <li>
        <label>
            驾龄：
        </label>
        <span>
            <input type="text" id="txtDriveAge" class="w50" />&nbsp;年</span></li>
    <li>
        <label>
            汽车通：
        </label>
        <span>
            <input type="text" id="txtUserName" class="w250" readonly="readonly" />&nbsp;<input
                type="button" value="注册" id="btnRegister" onclick="registerCarTong(this);" /></span> </li>
    <li>
        <label>
            车牌号：
        </label>
        <span>
            <input type="text" id="txtCarNumber" class="w250" /></span> </li>
    <li>
        <label>
            备注：</label><span>
                <input type="text" id="txtNote" class="w250" /></span> </li>
</ul>
