<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerInfo.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.DealerInfo" %>

<script type="text/javascript">
    //省市区县
    var CustBaseInfoHelper = (function () {
        var triggerProvince = function () {
            //selProvince  selCity  selCounty 
            //选中省份
            BindCity('selProvince', 'selCity');
            BindCounty('selProvince', 'selCity', 'selCounty');
            $("#hidprovinceid").val($("#selProvince").val());
            $("#hidcityid").val($("#selProvince").val());
        }
        var triggerCity = function () {
            //选中城市
            BindCounty('selProvince', 'selCity', 'selCounty');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#ddlCounty option').size() == 1) {
                $('#ddlCounty').attr('noCounty', '1');
            }
            else {
                $('#ddlCounty').removeAttr('noCounty');
            }

            $("#hidprovinceid").val($("#selProvince").val());
            $("#hidcityid").val($("#selCity").val());
        }
        return {
            TriggerProvince: triggerProvince,
            TriggerCity: triggerCity
        }
    })();
</script>
<script type="text/javascript">
    //初始化
    $(function () {
        //绑定品牌
        var options = {
            container: { master: "selCarBrandName", serial: "selCarSerialName", cartype: "selCarTypeName" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: '0', serial: '0', cartype: '0' }
        };
        new BindSelect(options).BindList();
        //绑定省份
        BindProvince('selProvince');
    });
    //复制操作
    function CopyDealerInfoToOrder(dealername, dealerid, typename, address) {
        WOrderInfo.AppendContent(
        "【经销商全称】" + dealername
        + "\n【经销商ID】" + dealerid
        + "\n【类别】" + typename
        + "\n【地址】" + address);
    }
    //发送短信
    function SendmailOption(dealerName, dealerAddress, dealerTel) {
        //清空SetTemplateFunc的设置
        SendMessageControl.SetTemplateFunc(
        function () { return dealerName; },
        function () { return dealerAddress; },
        function () { return dealerTel; });
        //获取当前手机号
        var phone = $.trim($("#inp_phone").val());
        SendMessageControl.BtnSendMessageClick(phone, 1);
    }
    //查询操作
    function RightMemberSearch() {
        var dealername = $.trim($("#txtDealerName").val());

        var brandname = $.trim($("#selCarBrandName").val());
        var serialname = $.trim($("#selCarSerialName").val());
        var typename = $.trim($("#selCarTypeName").val());

        var province = $.trim($("#selProvince").val());
        var city = $.trim($("#selCity").val());
        var county = $.trim($("#selCounty").val());

        var pricestr = $("input[name='rbHasPrice']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');

        var pody = 'DealerName=' + encodeURIComponent(dealername)
        + '&BrandId=' + brandname
        + '&SerialId=' + serialname
        + '&TypeId=' + typename
        + '&ProvinceID=' + province
        + '&CityID=' + city
        + '&CountyId=' + county
        + '&PriceIds=' + pricestr
        + '&page=1' + '&random=' + Math.random();

        if (dealername != "") {
            LoadingAnimation("divMemberinfoList", "search_list_bt_loading");
            $('#divMemberinfoList').load('/WOrderV2/AjaxServers/OrderYPTab/DealerInfoList.aspx', pody);
        }
        else if (brandname == 0 && serialname == 0 && typename == 0 && province == -1 && city == -1 && county == -1 && pricestr == "") {
            $('#divMemberinfoList').html("");
            return;
        }
        else {
            if (parseInt(serialname) <= 0 || serialname == null) {
                $.jAlert("请选择车型！", function () { $("#selCarSerialName").focus(); });
                return;
            }
            else if (parseInt(typename) <= 0) {
                $.jAlert("请选择车款！", function () { $("#selCarTypeName").focus(); });
                return;
            }
            else if (parseInt(province) <= 0 || province == null) {
                $.jAlert("请选择地区-省份！", function () { $("#selProvince").focus(); });
                return;
            }
            else if (parseInt(city) <= 0 || city == null) {
                $.jAlert("请选择地区-城市！", function () { $("#selCity").focus(); });
                return;
            }
            else {
                LoadingAnimation("divMemberinfoList", "search_list_bt_loading");
                $('#divMemberinfoList').load('/WOrderV2/AjaxServers/OrderYPTab/DealerInfoList_YP.aspx', pody);
            }
        }
    }
    //回车事件
    function EnterSearch(event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if (e && e.keyCode == 13) {
            // enter 键
            RightMemberSearch();
        }
    }
</script>
<div class="search">
    <ul>
        <li>
            <label>
                经销商名称：</label>
            <span>
                <input id="txtDealerName" name="" type="text" class="w200" onkeydown="EnterSearch()" />
            </span></li>
        <li>
            <label style="width: 70px;">
                车型：</label>
            <span>
                <select id="selCarBrandName" class="w65">
                    <option>请选择</option>
                </select>
                <select id="selCarSerialName" class="w65">
                    <option>请选择</option>
                </select>
                <select id="selCarTypeName" class="w65">
                    <option>请选择</option>
                </select>
            </span></li>
        <li>
            <label style="width: 70px;">
                地区：</label>
            <span>
                <select id="selProvince" class="w65" onchange="javascript:CustBaseInfoHelper.TriggerProvince()">
                    <option value="-1">省/直辖市</option>
                </select>
                <select id="selCity" class="w65" onchange="javascript:CustBaseInfoHelper.TriggerCity()">
                    <option value="-1">市</option>
                </select>
                <select id="selCounty" class="w65">
                    <option value="-1">区</option>
                </select>
            </span></li>
        <li style="clear: both;">
            <label style=" height:27px;">
                是否有报价：</label>
            <spans>
                <label style=" height:27px; text-align:left; width:55px;">
                    <input name="rbHasPrice" id="rbhavePrice" type="checkbox" value="1" /><em style=" font-style:normal;">是</em></label>
                <label style=" height:27px; text-align:left;">
                    <input name="rbHasPrice" id="rbnoPrice" type="checkbox" value="0" /><em style=" font-style:normal;">否</em></label>
            </span></li>
        <li class="button">
            <input name="" type="button" value="查询" style=" *margin-top:5px; margin-left:30px;" onclick="javascript:RightMemberSearch();" />
        </li>
    </ul>
</div>
<div class="clearfix">
    <input type="hidden" id="hidDealerPhone" value="" />
</div>
<div class="search_list_bt" style="position: relative;">
    <table border="1" cellspacing="0" cellpadding="0" class="bt_guding" style="margin: 0px 12px;
        position: absolute; z-index: 9;">
        <tr>
            <th style="width: 222px;">
                经销商全称
            </th>
            <th style="width: 142px;">
                经销商简称
            </th>
            <th style="width: 60px;">
                类别
            </th>
            <th style="width: 120px;">
                电话
            </th>
            <th style="width: 228px;">
                地址
            </th>
            <th style="width: 110px;">
                是否有报价
            </th>
            <th style="width: 90px;">
                会员详情
            </th>
            <th style="width: 90px;">
                操作
            </th>
        </tr>
    </table>
</div>
<div class="search_list search_list_jxs" id="divMemberinfoList">
</div>
