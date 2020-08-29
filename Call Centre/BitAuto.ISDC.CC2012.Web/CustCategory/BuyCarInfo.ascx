<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyCarInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustCategory.BuyCarInfo" %>
<script type="text/javascript" language="javascript">
    //验证
    function DataCheck() {
        var txtRemark = $("#txtNote").val();
        var msg = "";
        if (txtRemark != "") {
            if (GetStringRealLength(txtRemark) >200) {
                msg += "备注超长! <br/>";
            }
        }
        return msg;
    }
    function subBuyCarInfo(CustID, CustType) {
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
        var Type = CustType;
        var CustID = CustID;

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
            CustID: encodeURIComponent(CustID)
        }
        AjaxPostAsync("../AjaxServers/CustCategory/BuyCarInfo.ashx", params, null,
                    function (data) {

                        if (data == 'success') {
                            returnValue = true;
                        }
                        else {
                            $.jAlert(data);
                            returnValue = false;
                        }
                    });
        return returnValue;
    }

    $(document).ready(function () {
        var Remark = '<%=BuyCarInfoModel.Remark%>'
        if (Remark != null && Remark != "") {
            $('#txtNote').val(Remark);
        }

        var UserName = '<%=BuyCarInfoModel.UserName%>'
        if (UserName != null && UserName != "") {
            $('#txtUserName').val(UserName);
            $("#btnRegister").hide(); //当汽车通被注册时，注册 按钮隐藏
        }
        var form = "<%=RequestFrom %>";
        if (form == "CTI") {
            //ADTTool.LogonTime("time", "买车控件加载完成 " + "<%=logmsg %>");
        }
    });


    
</script>
<script type="text/javascript">
    //注册汽车通 add lxw 13.4.9
    function registerCarTong(othis) {
        var telNum = $("[id^='CustBaseInfo_txtTel'][value!='']").length;
        var telPhone = $("[id^='CustBaseInfo_txtTel'][value!='']").map(function () {
            return $(this).val();
        }).get().join(',');

        var msg = "";
        $("[id^='CustBaseInfo_txtTel'][value!='']").each(function () {
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
                $("[id^='CustBaseInfo_txtTel']:eq(0)").focus();
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
        var custid = $("#hdnCustID").val();
        if (custid == "") {
            custid = "<%=CustID %>";
        }
        AjaxPost("/AjaxServers/CustCategory/BuyCarInfo.ashx", { Action: "RegisterCarTong", PhoneNumber: encodeURIComponent(phoneNumber), CustID: encodeURIComponent(custid), r: Math.random() }, null,
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
<ul class="" style="display: none">
    <li>
        <label>
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
            婚姻状态：</label><span>
                <label style="float: none; cursor: pointer">
                    <input type="radio" id="radMarriageYes" name="radMarriage" value="1" />已婚</label></span>&nbsp;&nbsp;&nbsp;&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input type="radio" id="radMarriageNo" name="radMarriage" value="0" />未婚</label></span></li>
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
<ul class="">
    <li style="display: none">
        <label>
            目前驾驶：</label>
        <span>
            <select id="dllMyBrand" name="BrandId" class="w125" style="width: 82px;">
                <option value="0">请选择品牌</option>
            </select>
            <select id="dllMySerial" name="SerialId" class="w125" style="width: 82px;">
                <option value="0">请选择系列</option>
            </select>
            <select id="dllMyName" name="NameId" class="w125" style="width: 83px;" onmouseover="javascript:FixWidth(this);">
                <option value="0">请选择车款</option>
            </select>
        </span></li>
    <li style="display: none">
        <label>
            是否认证车主：</label>
        <span>
            <label style="float: none; cursor: pointer">
                <input type="radio" id="radYes" name="legalize" value="1" />是</label></span>&nbsp;&nbsp;&nbsp;&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input type="radio" id="radNo" name="legalize" value="0" />否</label></span></li>
    <li style="display: none">
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
                type="button" value="注册" id="btnRegister" onclick="registerCarTong(this);" /></span>
    </li>
        <li>
        <label>&nbsp;</label>
        <span>
            <a id="lbtnTaoCheBZCUrl" href='/CustBaseInfo/GoToTaoCheBZC.aspx?menu=2' target='TaoCheBZCUrl'>二手车信息查询</a>
            &nbsp;&nbsp;<a href='http://op.easypass.cn/op/main.aspx' target="_blank"'>黑名单查询</a>
        </span>
    </li>
    <li style="display: none">
        <label>
            车牌号：
        </label>
        <span>
            <input type="text" id="txtCarNumber" class="w250" /></span> </li>
    <li style="width: 940px">
        <label style="width: 120px">
            备注：</label><span>
                <input type="text" id="txtNote" style="width: 706px" class="w250" /></span>
    </li>
</ul>
<%--<div>
    <table>
        <tr>
            <td>
                年龄：
            </td>
            <td>
                <input type="text" id="txtAge" />岁
            </td>
            <td>
                身份证号：
            </td>
            <td>
                <input type="text" id="txtIDCards" />
            </td>
        </tr>
        <tr>
            <td>
                职业：
            </td>
            <td>
                <select id="selVocation">
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
                </select>
            </td>
            <td>
                婚姻状态：
            </td>
            <td>
                <input type="radio" id="radMarriageYes" name="radMarriage" value="1" />已婚<input type="radio"
                    id="radMarriageNo" name="radMarriage" value="0" />未婚
            </td>
        </tr>
        <tr>
            <td>
                个人收入：
            </td>
            <td colspan="3">
                <select id="selInCome">
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
                </select>
            </td>
        </tr>
        <tr>
            <td>
                目前驾驶：
            </td>
            <td>
                <select id="selCarBrandID" onchange="SelectCarBrandChange()">
                    <option value="-2" selected="selected">请选择</option>
                </select>
                <select id="selCarSerialID" onchange="SelectCarSerialChange()">
                    <option value="-2" selected="selected">请选择</option>
                </select>
                <input type="text" id="txtCarName" />
            </td>
            <td>
                是否认证车主：
            </td>
            <td>
                <input type="radio" id="radYes" name="legalize" value="1" />是<input type="radio"
                    id="radNo" name="legalize" value="0" />否
            </td>
        </tr>
        <tr>
            <td>
                驾龄：
            </td>
            <td>
                <input type="text" id="txtDriveAge" />年
            </td>
            <td>
                用户名：
            </td>
            <td>
                <input type="text" id="txtUserName" />
            </td>
        </tr>
        <tr>
            <td>
                车牌号：
            </td>
            <td>
                <input type="text" id="txtCarNumber" />
            </td>
            <td>
                备注：
            </td>
            <td>
                <input type="text" id="txtNote" />
            </td>
        </tr>
    </table>
</div>
--%>