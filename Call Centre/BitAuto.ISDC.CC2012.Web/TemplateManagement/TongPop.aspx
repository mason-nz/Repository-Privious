<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TongPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TongPop" %>

<script>
    var clock;
    var loopcount = 0;

    $(function () {
        //省市
        BindProvince('ddlProvincePop');
        var ProvinceID = '<%=ProvinceID %>';
        $("#ddlProvincePop").val(ProvinceID);
        TriggerProvince('ddlProvincePop', 'ddlCityPop');
        var CityID = '<%=CityID %>';
        $("#ddlCityPop").val(CityID);

        //
        BindProvince('ddlProvinceSearch');
        var ProvinceID = '<%=ProvinceID %>';
        $("#ddlProvinceSearch").val(ProvinceID);
        TriggerProvince('ddlProvinceSearch', 'ddlCitySearch');
        var CityID = '<%=CityID %>';
        $("#ddlCitySearch").val(CityID);

        //
        var options = {
            container: { master: "ddlWantBrand", serial: "ddlWantSerial", cartype: "ddlWantName" },
            include: { serial: "1", cartype: "1" },
            datatype: 4,
            binddefvalue: { master: '<%=MasterBrandID%>', serial: '<%=SerialID%>'}
        };
        new BindSelect(options).BindList();

        //循环检测，如果绑定完了，就查询
      //  clock = setInterval(run, 500);

    });

    function run() {
        loopcount++;
        //检测次数超过了一定次数，就不检测了
        if (loopcount > 100) {
            clearInterval(clock);
        }
        if (Number($("#ddlWantName").val()) > 0 &&
              (Number($("#ddlProvinceSearch").val()) > 0 || Number($("#ddlCitySearch").val()) > 0)) {
            clearInterval(clock);
            Search();
        }
    }

    function TriggerProvince(ProvinceID, CityID) {//选中省份
        //选择省份,分属大区跟着变化
        BindCity(ProvinceID, CityID);
    }

    //查询经销商
    function Search() {
        var msg = CheckData();
        if (msg != "") {
            $.jAlert(msg);
            return;
        }
        var pody = GetData('1');
        LoadingAnimation("divall");

        $("#divall").load('/TemplateManagement/TongMemberList.aspx', pody);
    }

    function CheckData() {
        var msg = "";
        var locationid = "";
        var carid = "";

        if ($("#ddlCitySearch").val() != "-1") {
            locationid = $("#ddlCitySearch").val();
        }
        else if ($("#ddlProvinceSearch").val() != "-1") {
            locationid = $("#ddlProvinceSearch").val();
        }
        else {
            msg += "请选择所在省份<br/>";
        }

        if ($("#ddlWantName").val() != "0") {
            carid = $("#ddlWantName").val();
        }
        else {
            msg += "请选择车款<br/>";
        }
        return msg;
    }
    function GetData(PageIndex) {
        var locationid = "";
        var carid = "";

        if ($("#ddlCitySearch").val() != "-1") {
            locationid = $("#ddlCitySearch").val();
        }
        else if ($("#ddlProvinceSearch").val() != "-1") {
            locationid = $("#ddlProvinceSearch").val();
        }
        carid = $("#ddlWantName").val();

        var pody = "locationid=" + escape(locationid) + "&carid=" + escape(carid) + "&PageIndex=" + escape(PageIndex) + "&random=" + Math.random();
        return pody;
    }

    function SubmitData() {

        var membercode = $("#tableCustBrandSelect td.tdFirst").attr("membercode");
        if (membercode == undefined) {
            $.jAlert("请选择经销商");
            return;
        }

        var Mobile = $.trim($("#txtMobilePop").val());
        if (Mobile == "") {
            $.jAlert("当前手机号码不能为空");
            return;
        }

        if (!isMobile(Mobile)) {
            $.jAlert("当前手机号码格式不正确！");
            return false;
        }

        var UserName = $.trim($("#txtUserNamePop").val());
        if (UserName == "") {
            $.jAlert("用户姓名不能为空");
            return;
        }

        if (UserName.length > 6) {
            $.jAlert("用户姓名不能超过6个字");
            return;
        }

        var ProvinceID = $.trim($("#ddlProvincePop").val());
        if (ProvinceID == "-1") {
            $.jAlert("请选择用户所在地区的省份");
            return;
        }

        var CarTypeID = $("#ddlWantName").val();
        if (CarTypeID == "0") {
            $.jAlert("请选择意向车型");
            return;
        }

        var CityID = $.trim($("#ddlCityPop").val());
        if (CityID == "-1") {
            $.jAlert("请选择用户所在地区的城市");
            return;
        }
        var userid = $("#hidDMSUserid").val();

        $("#btnWXT").attr("disabled", "disabled");
        $("#imgwait").show();

        if (userid == "") {
            //查询汽车通用户名

            $.ajax({
                type: "POST",
                url: "/AjaxServers/TemplateManagement/TongHandler.ashx",
                data: { Action: 'GetQiCheTongUser', mobile: Mobile },
                dataType: 'json',
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.Success == true) {
                        var userid = Number(data.Data);
                        if (userid > 0) {
                            $("#hidDMSUserid").val(userid);
                        }
                        else if (userid == -1) {
                            //未注册汽车通，要注册汽车通
                            registerByWebService(Mobile);
                        }
                        else {
                            //查询汽车通用户出错
                            $.jAlert("查询汽车通用户信息出错");
                        }
                    }
                    else {
                        $.jAlert("查询汽车通出错:" + data.Message);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $.jAlert('错误: ' + XMLHttpRequest.responseText);
                }
            });
        }
        userid = $("#hidDMSUserid").val();

        if (userid == "" || Number(userid) <= 0) {
            $("#btnWXT").attr("disabled", "");
            $("#imgwait").hide();
            return;
        }

        var pody = {
            Action: 'SumbitWXT',
            TaskID: '<%=TaskID %>',
            Mobile: escape(Mobile),
            UserName: escape(UserName),
            ProvinceID: escape(ProvinceID),
            CityID: escape(CityID),
            UserID: escape(userid),
            CarTypeID: escape(CarTypeID),
            MemberCode: escape(membercode)
        };

        $.ajax({
            type: "POST",
            url: "/AjaxServers/TemplateManagement/TongHandler.ashx",
            data: pody,
            dataType: 'json',
            beforeSend: function () {

            },
            success: function (data) {
                $("#btnWXT").attr("disabled", "");
                $("#imgwait").hide();

                if (data.Success == true) {
                    if (data.Data.code == "1") {
                        $.jAlert("提交成功", function () {

                            $('#popupLayer_' + 'TongPop').data('Mobile', Mobile);
                            $('#popupLayer_' + 'TongPop').data('UserName', UserName);
                            $('#popupLayer_' + 'TongPop').data('ProvinceID', ProvinceID);
                            $('#popupLayer_' + 'TongPop').data('CityID', CityID);

                            $('#popupLayer_' + 'TongPop').data('BrandID', $("#ddlWantBrand").val());
                            $('#popupLayer_' + 'TongPop').data('SerialID', $("#ddlWantSerial").val());
                            $('#popupLayer_' + 'TongPop').data('CarTypeID', CarTypeID);

                            $.closePopupLayer('TongPop', true);
                        });
                    }
                    else {
                        $.jAlert("提交出错！" + data.Data.message);
                    }
                }
                else {
                    $.jAlert("提交出错!" + data.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $("#btnWXT").attr("disabled", "");
                $("#imgwait").hide();

                $.jAlert('错误: ' + XMLHttpRequest.responseText);
            }
        });
    }

    function PageForHC(PageIndex) {
        var msg = CheckData();
        if (msg != "") {
            $.jAlert(msg);
            return;
        }
        var pody = GetData(PageIndex);
        pody += "&ispage=1";
        LoadingAnimation("divMemberList");

        $('#divMemberList').load('/TemplateManagement/TongMemberList.aspx #divMemberList', pody, LoadDivSuccess);
    }

    function LoadDivSuccess(data) {

        $('#divall tr:even').addClass('color_hui'); //设置列表行样式
    }

    //注册汽车通
    function pop_register() {
        var telPhone = $.trim($("#txtMobilePop").val());

        if (telPhone == "") {
            $.jAlert("手机号码不能为空！");
            return false;
        }
        if (!isMobile(telPhone)) {
            $.jAlert("手机号码格式不正确！");
            return false;
        }

        registerByWebService(telPhone);
    }

    //调用web服务接口注册车商通
    function registerByWebService(phoneNumber) {
        AjaxPostAsync("/AjaxServers/CustCategory/BuyCarInfo.ashx", { Action: "RegisterCarTong", PhoneNumber: encodeURIComponent(phoneNumber), TaskID: "<%=TaskID %>", r: Math.random() },
        function () {
        },
        function (data) {
            var jsonData = eval("(" + data + ")");
            if (jsonData.result == 'yes') {
                $("#spanInfoPop").html("注册成功! &nbsp;" + jsonData.userid + "/" + jsonData.pwd);
                $("#hidDMSUserid").val(jsonData.userid);
            }
            else {
                $("#spanInfoPop").html(jsonData.msg);
            }
        });
    }

</script>
<script>

    $(function () {
        $(".linkSelect").live('click', function (e) {
            e.preventDefault();
            SelectItem(this);
        });
    });

    function SelectItem(o) {
        var tr = $(o).parent().parent();
        $("#trSelect").html($(tr).html());
        $("#trSelect td.tdFirst").html("<img title='删除' src='/Images/close.png'>");
        $("#trSelect td.tdFirst img").click(function () {
            $("#trSelect").html("");
        });
    }
</script>
<div>
    <a style="position: absolute; left: -9999px; top: -1px;" class="jmp-link-at-top"
        href="#">&nbsp;</a>
    <div style="width: 650px" class="pop pb15 openwindow">
        <div class="title bold">
            <h2 style="cursor: auto;">
                选择经销商，并提交网销通订单</h2>
            <span><a onclick="javascript:$.closePopupLayer('TongPop',false);" href="javascript:void(0)">
            </a></span>
        </div>
        <ul style="border-bottom: dotted  1px #0099FF; width: 630px" class="clearfix  outTable">
            <li style="width: 600px; float: left;">
                <label style="width: 120px;">
                    <span class="redColor">*</span>手机号码：</label>
                <input type="text" value="<%=Mobile %>" id="txtMobilePop" /><input type="button"
                    name="" value="注册汽车通" onclick="javascript:pop_register();" class="btnSave" />
                <input type="hidden" id="hidDMSUserid" value='' />
                &nbsp;&nbsp;<span id="spanInfoPop"></span> </li>
            <li style="width: 300px; float: left; border-bottom: dotted  1px #0099FF;">
                <label style="width: 120px;">
                    <span class="redColor">*</span>用户姓名：</label>
                <input type="text" value="<%=UserName %>" id="txtUserNamePop" /></li><li class="name1"
                    style="width: 330px; float: left; border-bottom: dotted  1px #0099FF;">
                    <label>
                        <span class="redColor">*</span>用户地区：</label>
                    <select name="ddlProvince" id="ddlProvincePop" class="w80" onchange="javascript:TriggerProvince('ddlProvincePop', 'ddlCityPop');">
                    </select>
                    <select name="ddlCity" id="ddlCityPop" class="w80" style="margin-left: 5px">
                        <option value="-1">城市</option>
                    </select></li>
            <li style="width: 500px;">
                <label>
                    <span class="redColor">*</span>意向车型：</label>
                <select id="ddlWantBrand" name="BrandId" class="w125" style="width: 82px;">
                </select>
                <select id="ddlWantSerial" name="SerialId" class="w125" style="width: 82px;">
                </select>
                <select id="ddlWantName" name="NameId" class="w125" style="width: 180px;">
                </select>
            </li>
            <li style="width: 400px" class="name1">
                <label>
                   <span class="redColor">*</span>目标城市：</label>
                <select name="ddlProvinceSearch" id="ddlProvinceSearch" class="w80" onchange="javascript:TriggerProvince('ddlProvinceSearch', 'ddlCitySearch')">
                </select>
                <select name="ddlCitySearch" id="ddlCitySearch" class="w80" style="margin-left: 5px">
                    <option value="-1">城市</option>
                </select>
            </li>
            <li class="btn">
                <input type="button" class="btnSave bold" onclick="javascript:Search();" value="查 询"
                    name=""></li>
        </ul>
        <div class="line">
        </div>
        <div id="divall">
        </div>
        <ul>
            <li class="btn" style="width: 580px;">
            <img id="imgwait" style=" display:none;" src="/Images/blue-loading.gif" />
                <input type="button" id="btnWXT"  name="" value="提交网销通" onclick="javascript:SubmitData();" class="btnSave bold">&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" name="" value="取消" onclick="javascript:$.closePopupLayer('TongPop',false);"
                    class="btnSave bold">
            </li>
        </ul>
        <br>
    </div>
    <a style="position: absolute; left: -9999px; bottom: -1px;" class="jmp-link-at-bottom"
        href="#">&nbsp;</a>
    <input style="position: absolute; left: -9999px; top: -1px;" class="jmp-link-at-bottom">
</div>
