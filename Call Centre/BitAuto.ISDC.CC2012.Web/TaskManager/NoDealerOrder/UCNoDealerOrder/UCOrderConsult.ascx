<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCOrderConsult.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.UCOrderConsult" %>
<script type="text/javascript">
    document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
</script>
<script type="text/javascript">
    loadJS("bitdropdownlist");
</script>
<script src="/Js/Enum/Area2.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function () {

        //绑定品牌信息
        BindBrand();

        //绑定省份信息
        BindProvince('<%=selProvince.ClientID %>'); //绑定省份
        triggerProvince();
        $("#<%=selProvince.ClientID %>").val('<%=Province %>');
        triggerProvince();
        $("#<%=selCity.ClientID %>").val('<%=City %>');
        triggerCity();
        $("#<%=selCounty.ClientID %>").val('<%=County %>');

        //前端异步初始化车身颜色   Add=Masj，Date=2013-08-01
        switch ('<%=Source %>') {
            case '1': //新车
            case '3': //试驾
                colorChange('ddlNewName', 'ddlNewCarColor', <%=ddlCarTypeID %>);
                break;
            case '2': //置换
                colorChange('ddlWantName', 'ddlWantColor', <%=ddlCarTypeID %>);
                colorChange('dllOldName', 'dllOldColor', <%=ddlRecCarTypeID %>);
                break;
            default: break;
        }
    });

    //绑定列表颜色
    function colorChange(ddlCarTypeID, ddlColor, carTypeID) {
        $("#UCOrderConsult1_" + ddlColor + " option").remove();
        if ($("#" + ddlCarTypeID).val() != 0) {
                $.post("../../AjaxServers/TaskManager/NoDealerOrder/OrderConsultInfo.ashx", { ActionByCarID: 'getColorTableByID', CarTypeID: (carTypeID != undefined && carTypeID > 0 ? carTypeID : $("#" + ddlCarTypeID).val()) }, function (data) {
                    if (data != "" && data != 'success') {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            var data = jsonData.color.split(',');
                            if (data.length > 0 && ddlColor == 'ddlNewCarColor') {
                                $("#<%=ddlNewCarColor.ClientID %>").append("<option value='-1'>请选择颜色</option>");
                            }
                            for (var i = 0; i < data.length; i++) {
                                $("#UCOrderConsult1_" + ddlColor).append("<option value='" + data[i] + "'>" + data[i] + "</option>");
                            }
                        }
                    }
                    else {

                        if ($("#<%=dllOldColor.ClientID %>").val() != null) {
                            $("#<%=dllOldColor.ClientID %>").val('<%=nowColor %>');
                        }
                        else {
                            //$("#<%=dllOldColor.ClientID %>").append("<option value='<%=nowColor %>'><%=nowColor %></option>");
                        }

                        if ($("#<%=ddlWantColor.ClientID %>").val() != null) {
                            $("#<%=ddlWantColor.ClientID %>").val('<%=relColor %>');
                        }
                        else {
                            //$("#<%=ddlWantColor.ClientID %>").append("<option value='<%=relColor %>'><%=relColor %></option>");
                        }
                    }
                });
        }
    }

    function triggerProvince() {//选中省份
        BindCity('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>');
        BindCounty('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>', '<%=selCounty.ClientID %>');
    }

    function triggerCity() {//选中城市
        BindCounty('<%=selProvince.ClientID %>', '<%=selCity.ClientID %>', '<%=selCounty.ClientID %>');
    }

    //绑定品牌信息
    function BindBrand() {

        //新车
        var options = {
            container: { master: "ddlNewBrand", serial: "ddlNewSerial", cartype: "ddlNewName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '<%=ddlNewBrand %>', serial: '<%=ddlNewSerial %>', cartype: '<%=ddlNewName %>' }
        };
        new BindSelect(options).BindList();

        //要置换的车
        var options2 = {
            container: { master: "ddlWantBrand", serial: "ddlWantSerial", cartype: "ddlWantName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '<%=ddlWantBrand %>', serial: '<%=ddlWantSerial %>', cartype: '<%=ddlWantName %>' }
        };
        new BindSelect(options2).BindList();

        //现有的车
        var options3 = {
            container: { master: "dllOldBrand", serial: "dllOldSerial", cartype: "dllOldName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '<%=dllOldBrand %>', serial: '<%=dllOldSerial %>', cartype: '<%=dllOldName %>' }
        };
        new BindSelect(options3).BindList();
    }


   
   
</script>
<script type="text/javascript">

    function CheckControl() {
        var Source = '<%=Source %>';

        var msg = "";
        if (Source == "1" || Source == "3") {
            if ($("[id$='ddlNewBrand']").val() == "0") {
                msg += "请选择关注车型的品牌<br/>";
            }
            if ($("[id$='ddlNewSerial']").val() == "0") {
                msg += "请选择关注车型的系列<br/>";
            }
            if ($("[id$='ddlNewName']").val() == "0") {
                msg += "请选择关注车型的车型<br/>";
            }

            if ($.trim($("[id$='txtNewCallRecord']").val()) == "") {
                msg += "请填写电话记录<br/>";
            }
        }
        else if (Source == "2") {
            if ($("[id$='ddlWantBrand']").val() == "0") {
                msg += "请选择意向车型的品牌<br/>";
            }
            if ($("[id$='ddlWantSerial']").val() == "0") {
                msg += "请选择意向车型的系列<br/>";
            }
            if ($("[id$='ddlWantName']").val() == "0") {
                msg += "请选择意向车型的车型<br/>";
            }
            //            if ($("[id$='ddlWantColor']").val() == "") {
            //                msg += "请选择意向车的颜色<br/>";
            //            }
            if ($("[id$='dllOldBrand']").val() == "0") {
                msg += "请选择现有车型的品牌<br/>";
            }
            if ($("[id$='dllOldBrand']").val() == "0") {
                msg += "请选择现有车型的系列<br/>";
            }
            if ($("[id$='dllOldName']").val() == "0") {
                msg += "请选择现有车型的车型<br/>";
            }
            //            if ($("[id$='dllOldColor']").val() == "") {
            //                msg += "请选择现有车的颜色<br/>";
            //            }
            if ($("[id$='selProvince']").val() == "-1") {
                msg += "请选择车牌所在地省份<br/>";
            }
            if ($("[id$='selCity']").val() == "-1") {
                msg += "请选择车牌所在地城市<br/>";
            }
            //            if ($("[id$='selCounty']").val() == "-1") {
            //                msg += "请选择车牌所在地区县<br/>";
            //            }
            if ($.trim($("[id$='txtMileage']").val()) == "") {
                msg += "请填写已行驶里程<br/>";
            }
            if (isNaN($.trim($("[id$='txtMileage']").val()))) {
                msg += "已行驶里程应该为数字<br/>";
            }
            //判断公里数在0-100之间
            if (parseInt($.trim($("[id$='txtMileage']").val()), 10) <= 0 || parseInt($.trim($("[id$='txtMileage']").val()), 10) > 100) {
                msg += "已行驶里程应该在0-100之间，且不能等于0<br/>";
            }
            if ($.trim($("[id$='txtProSellPrice']").val()) == "") {
                msg += "请填写预售价格<br/>";
            }
            if (isNaN($.trim($("[id$='txtProSellPrice']").val()))) {
                msg += "预售价格应该为数字<br/>";
            }

            if ($.trim($("[id$='txtReplaceCallRecord']").val()) == "") {
                msg += "请填写电话记录<br/>";
            }
            if ($.trim($("[id$='dllRegDateYear']").val()) == "-1") {
                msg += "请填写首次上牌日期的年份<br/>";
            }
            if ($.trim($("[id$='dllRegDateMonth']").val()) == "-1") {
                msg += "请填写首次上牌日期的月份<br/>";
            }

            //判断预售价格

            var body = {
                Action: "checkprice",
                CarTypeID: escape($("[id$='dllOldName']").val()),
                CarPrice: escape($("[id$='txtProSellPrice']").val())
            };
            AjaxPostAsync("/AjaxServers/TaskManager/NoDealerOrder/Handler.ashx", body, null,
                         function (data) {

                             if (data != "") {
                                 msg += data + "<br/>";

                             }
                         });
        }
        else {
            msg += "没有订单类型信息<br/>";
        }

        return msg;
    }



    function GetNewCarJson() {
        var json = {
            CarMasterID: escape($("[id$='ddlNewBrand']").val()),
            CarSerialID: escape($("[id$='ddlNewSerial']").val()),
            CarTypeID: escape($("[id$='ddlNewName']").val()),
            CarColor: escape($("[id$='ddlNewCarColor']").val()),
            DMSMemberCode: escape($("[id$='hidNewCarDMSmemberCode']").val()),
            DMSMemberName: escape($("[id$='txtNewDealer']").val()),
            CallRecord: escape($("[id$='txtNewCallRecord']").val()),
            UserName: escape($("[id$='txtCustName']").val())

        };
        return json;
    }


    function GetReplaceJson() {
        var json = {
            RepCarMasterID: escape($("[id$='dllOldBrand']").val()),
            RepCarSerialID: escape($("[id$='dllOldSerial']").val()),
            RepCarTypeId: escape($("[id$='dllOldName']").val()),
            ReplacementcCarColor: escape($("[id$='dllOldColor']").val()),

            DMSMemberCode: escape($("[id$='hidWantDMSMemberCode']").val()),
            DMSMemberName: escape($("[id$='txtWantDealer']").val()),

            CarMasterID: escape($("[id$='ddlWantBrand']").val()),
            CarSerialID: escape($("[id$='ddlWantSerial']").val()),
            CarTypeID: escape($("[id$='ddlWantName']").val()),
            CarColor: escape($("[id$='ddlWantColor']").val()),

            ReplacementCarBuyYear: escape($("[id$='dllRegDateYear']").val()),
            ReplacementCarBuyMonth: escape($("[id$='dllRegDateMonth']").val()),
            RepCarProvinceID: escape($("[id$='selProvince']").val()),
            RepCarCityID: escape($("[id$='selCity']").val()),
            RepCarCountyID: escape($("[id$='selCounty']").val()),
            ReplacementCarUsedMiles: escape($("[id$='txtMileage']").val()),
            SellPrice: escape($("[id$='txtProSellPrice']").val()),
            CallRecord: escape($("[id$='txtReplaceCallRecord']").val()),
            UserName: escape($("[id$='txtCustName']").val())
        };
        if (json.CarColor == "null") {
            json.CarColor = "";
        }
        if (json.ReplacementcCarColor == "null") {
            json.ReplacementcCarColor = "";
        }
        return json;
    }
</script>
<script type="text/javascript">

    //发送短信
    function SendSmS() {

        var par = {
            CustName: $("#EditCustBaseInfo1_txtCustName").val(),    //客户姓名
            CustSex: $(":radio[name$='sex']:checked").val(),        //客户性别
            Tel1: $("[id$='txtTel1']").val(),
            Tel2: $("[id$='txtTel2']").val(),
            DMSCode: $("[id$='hidDMSCode']").val(),
            DMSName: $("[id$='hidDMSName']").val(),
            DMSAddress: $("[id$='hidDMSAddress']").val(),
            DMSTel: $("[id$='hidDMSTel']").val(),
            DMSCity: $("[id$='hidDMSCity']").val(),
            DMSLevel: $("[id$='hidDMSLevel']").val()
        };

        $.openPopupLayer({
            name: "SendSMSPopup",
            parameters: par,
            url: "/AjaxServers/TaskManager/NoDealerOrder/SMSSend.aspx",
            beforeClose: function (e) {
            }
        });
    }

</script>
<script type="text/javascript">
    function YpDealSelect() {
        var ProvinceIDDealSelect = $("#hidprovinceid").val();
        var CityIDDealSelect = $("#hidcityid").val();
        var CarIDDealSelect = $("[id$='ddlNewName']").val();
        var CarIDOptionLen = $("#ddlNewName option").length;
        var Source = '<%=Source %>';
        if (Source == "2") {
            CarIDDealSelect = $("[id$='ddlWantName']").val();
            CarIDOptionLen = $("#ddlWantName option").length;
        }


        var SerialID = $("[id$='ddlNewSerial']").val();
        var Source = '<%=Source %>';
        if (Source == "2") {
            SerialID = $("[id$='ddlWantSerial']").val();
        }


        if (parseInt(ProvinceIDDealSelect) <= 0) {
            $.jAlert("请选择省份！");
            return;
        }
        else if (parseInt(CityIDDealSelect) <= 0) {
            $.jAlert("请选择城市！");
            return;
        }
        else if (parseInt(SerialID) <= 0) {
            $.jAlert("请选择车型！");
            return;
        }
        else if (parseInt(CarIDDealSelect) <= 0 && CarIDOptionLen > 1) {
            $.jAlert("请选择车款！");
            return;
        }
        else if (parseInt(CarIDDealSelect) <= 0 && CarIDOptionLen == 1) {
            CarIDDealSelect = SerialID
        }
        $.openPopupLayer({
            name: "YpDealerSelectAjaxPopup",
            parameters: { ProvinceID: escape(ProvinceIDDealSelect), CityID: escape(CityIDDealSelect), CarID: escape(CarIDDealSelect) },
            url: "../../AjaxServers/TaskManager/NoDealerOrder/YpDealerSelect.aspx",
            beforeClose: function () {
                var DMSName = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSName', DMSName);
                var DMSCode = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSCode', DMSCode);
                var DMSLevel = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSLevel', DMSLevel);
                var DMSTel = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSTel', DMSTel);
                var City = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('City', City);
                var Address = $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('Address', Address);
                if (Source == "1" || Source == "3") {

                    $("#<%=this.txtNewDealer.ClientID %>").val(DMSName);
                    $("#<%=this.hidNewCarDMSmemberCode.ClientID %>").val(DMSCode);
                    $("#<%=this.hidDMSCode.ClientID %>").val(DMSCode);
                    $("#<%=this.hidDMSName.ClientID %>").val(DMSName);

                }
                else {
                    $("#<%=this.txtWantDealer.ClientID %>").val(DMSName);
                    $("#<%=this.hidWantDMSMemberCode.ClientID %>").val(DMSCode);
                    $("#<%=this.hidDMSCode.ClientID %>").val(DMSCode);
                    $("#<%=this.hidDMSName.ClientID %>").val(DMSName);
                }
                $("#<%=this.hidDMSLevel.ClientID %>").val(DMSLevel);
                $("#<%=this.hidDMSAddress.ClientID %>").val(Address);
                $("#<%=this.hidDMSTel.ClientID %>").val(DMSTel);
                $("#<%=this.hidDMSCity.ClientID %>").val(City);

            }
        });
    }
</script>
<ul class="clearfix" grouplist="ContactInfo" tagcode="60001" style="display: none;">
    <li>
        <label>
            <span class="redColor">*</span> 关注车型：</label>
        <select id="ddlNewBrand" name="BrandId" class="w125" style="width: 82px;">
        </select>
        <select id="ddlNewSerial" name="SerialId" class="w125" style="width: 82px;">
        </select>
        <select id="ddlNewName" name="NameId" class="w125" style="width: 82px;" onchange="colorChange('ddlNewName','ddlNewCarColor')"
            onmouseover="javascript:FixWidth(this);">
        </select>
    </li>
    <li>
        <label>
            车辆颜色：</label><span>
                <select id="ddlNewCarColor" name="" runat="server" class="w125" style="width: 100px;">
                </select>
            </span></li>
    <%if (DealerId != 0)
      {%>
    <li>
        <label>
            原始经销商：</label><a href="../../../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                target="_blank"><%=DealerName%></a></li>
    <%}%>
    <li>
        <label>
            推荐经销商：</label><span>
                <input id="txtNewDealer" runat="server" type="text" readonly="true" class="w220"
                    value="" contoltype="txt" /></span> <a id="btnNewSelect" onclick="YpDealSelect()"
                        style="cursor: pointer;">查询</a> <a id="A3" onclick="SendSmS()" style="cursor: pointer;">
                            发送短息</a>
        <input type="hidden" runat="server" id="hidNewCarDMSmemberCode" value="" />
    </li>
    <li>
        <label>
            补充说明：</label><span id="labNewsRemark" runat="server"> </span></li>
    <li>
        <label>
            创建时间：</label><span id="labNewCreatTime" runat="server"> </span></li>
    <li style="width: 1000px">
        <label>
            <span class="redColor">*</span> 电话记录：</label>
        <textarea id="txtNewCallRecord" runat="server" rows="5" cols="40" name="CallRecord"
            contoltype="txt" class="fullRow cont_cxjg" style="width: 702px;"></textarea></li>
</ul>
<ul class="clearfix" grouplist="ContactInfo" tagcode="60002" style="display: none;">
    <li>
        <label>
            <span class="redColor">*</span> 意向车型：</label>
        <select id="ddlWantBrand" name="BrandId" class="w125" style="width: 82px;">
        </select>
        <select id="ddlWantSerial" name="SerialId" class="w125" style="width: 82px;">
        </select>
        <select id="ddlWantName" name="NameId" class="w125" style="width: 82px;" onchange="colorChange('ddlWantName','ddlWantColor')"
            onmouseover="javascript:FixWidth(this);">
        </select>
    </li>
    <li>
        <label>
            意向车的颜色：</label><span>
                <select id="ddlWantColor" runat="server" name="" class="w125" style="width: 100px;">
                </select>
            </span></li>
    <%if (DealerId != 0)
      {%>
    <li>
        <label>
            原始经销商：</label><a href="../../../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                target="_blank"><%=DealerName%></a></li>
    <%} %>
    <li>
        <label>
            推荐经销商：</label><span>
                <input id="txtWantDealer" runat="server" type="text" readonly="true" class="w220"
                    value="" contoltype="txt" /></span> <a id="btnWantSelect" onclick="YpDealSelect()"
                        style="cursor: pointer;">查询</a> <a id="A4" onclick="SendSmS()" style="cursor: pointer;">
                            发送短息</a>
        <input type="hidden" runat="server" id="hidWantDMSMemberCode" value="" />
    </li>
    <li>
        <label>
            <span class="redColor">*</span> 现有车型：</label>
        <select id="dllOldBrand" name="BrandId" class="w125" style="width: 82px;">
        </select>
        <select id="dllOldSerial" name="SerialId" class="w125" style="width: 82px;">
        </select>
        <select id="dllOldName" name="NameId" class="w125" style="width: 82px;" onchange="colorChange('dllOldName','dllOldColor')"
            onmouseover="javascript:FixWidth(this);">
        </select>
    </li>
    <li>
        <label>
            现有车的颜色：</label><span>
                <select id="dllOldColor" runat="server" name="" class="w125" style="width: 100px;">
                </select>
            </span></li>
    <li>
        <label>
            <span class="redColor">*</span> 首次上牌日期：</label>
        <select id="dllRegDateYear" runat="server" name="" class="w125" style="width: 125px;">
        </select>
        <select id="dllRegDateMonth" runat="server" name="" class="w125" style="width: 125px;">
        </select>
    </li>
    <li>
        <label>
            <span class="redColor">*</span> 车牌所在地：</label>
        <select id="selProvince" runat="server" name="" class="w125" onchange="javascript:triggerProvince();"
            style="width: 82px;">
        </select>
        <select id="selCity" runat="server" name="" class="w125" onchange="javascript:triggerCity();"
            style="width: 82px;">
        </select>
        <select id="selCounty" runat="server" name="" class="w125" style="width: 82px;">
        </select>
    </li>
    <li>
        <label>
            <span class="redColor">*</span> 已行驶里程：</label>
        <input id="txtMileage" runat="server" type="text" class="w250" value="" contoltype="txt" />万公里
    </li>
    <li>
        <label>
            <span class="redColor">*</span> 预售价格：</label>
        <input id="txtProSellPrice" runat="server" type="text" class="w250" value="" contoltype="txt" />万元
    </li>
    <li>
        <label>
            补充说明：</label><span id="labWantRemark" runat="server"> </span></li>
    <li>
        <label>
            创建时间：</label><span id="labWantCreateTime" runat="server"> </span></li>
    <li style="width: 1000px">
        <label>
            <span class="redColor">*</span>电话记录：</label><span><textarea id="txtReplaceCallRecord"
                runat="server" rows="5" cols="40" name="CallRecord" contoltype="txt" style="width: 702px;"></textarea></span></li>
</ul>
<input type="hidden" id="hidDMSCode" runat="server" value='' />
<input type="hidden" id="hidDMSName" runat="server" value='' />
<input type="hidden" id="hidDMSLevel" runat="server" value='' />
<input type="hidden" id="hidDMSAddress" runat="server" value='' />
<input type="hidden" id="hidDMSTel" runat="server" value='' />
<input type="hidden" id="hidDMSCity" runat="server" value='' />
