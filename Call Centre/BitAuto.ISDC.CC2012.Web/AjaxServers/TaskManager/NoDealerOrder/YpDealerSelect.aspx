<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YpDealerSelect.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder.YpDealerSelect" %>

<%--<link href="../../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../../Css/style.css" type="text/css" rel="stylesheet" />--%>
<script type="text/javascript">

    var NeedPageLoad = '<%=NeedPageLoad %>';
    
    $(document).ready(function () {
        enterSearch(MemberSearch);
        BindProvince('<%=this.ddlProvince.ClientID %>'); //绑定省份
        $("#<%=this.ddlProvince.ClientID %>").val('<%=ProvinceID %>');
        selectDealertriggerProvince();
        $("#<%=this.ddlCity.ClientID %>").val('<%=CityID %>');
        selectDealertriggerCity();
        
        if (NeedPageLoad == "0") {
            MemberSearch();
        }
    });
    function selectDealertriggerProvince() {//选中省份
        BindCity('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>');
        BindCounty('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>', '<%=this.ddlCounty.ClientID %>');
    }
    function selectDealertriggerCity() {//选中城市
        BindCounty('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>', '<%=this.ddlCounty.ClientID %>');
        //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
        if ($('#ddlCounty option').size() == 1)
        { $('#ddlCounty').attr('noCounty', '1'); }
        else
        { $('#ddlCounty').removeAttr('noCounty'); }
    }
    $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式
    //查询操作
    function MemberSearch() {
        //        var pody = 'requesttype=sd&MemberName=' + escape($('#txtMemberName').val()) + '&page=1' + '&random=' + Math.random();
        //        $('#divMemberList').load('/AjaxServers/CustCategory/DealerSelect.aspx #divMemberList > *', pody, LoadDivSuccess);

        var CarID = '<%=CarID %>';
        var provinceid = $("#<%=this.ddlProvince.ClientID %>").val();
        var areaid = $("#<%=this.ddlCity.ClientID %>").val();
        if (areaid == -1) {
            $.jAlert("请选择城市！");
            return;
        }
        var Countyareaid = $("#<%=this.ddlCounty.ClientID %>").val();
        if (Countyareaid != -1) {
            areaid = Countyareaid;
        }
        var levelstr = $("input[name='level']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');

        var pricestr = $("input[name='price']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');

        LoadingAnimation("divall");
        var pody = 'CarID=' + escape(CarID) + '&ProvinceID=' + escape(provinceid) + '&CityID=' + areaid + '&level=' + levelstr + '&npd=1&IsPrice=' + pricestr + '&random=' + Math.random();
        $('#divall').load('../../AjaxServers/TaskManager/NoDealerOrder/YpDealerSelect.aspx #divall > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divMemberList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectMember(DMSName, DMSCode, DMSLevel, DMSTel, City, Address) {

        //        if ($.jConfirm('确定选择此客户吗')) {
        //            debugger
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSName', DMSName);
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSCode', DMSCode);
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSLevel', DMSLevel);
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('DMSTel', DMSTel);
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('City', City);
        //$('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('ProVince', ProVince);
        $('#popupLayer_' + 'YpDealerSelectAjaxPopup').data('Address', Address);
        $.closePopupLayer('YpDealerSelectAjaxPopup');
        //}
    }
    function GetDealerListByLocationId(areaid, provinceid) {
        var CarID = '<%=CarID%>';
        var levelstr = $("input[name='level']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');
        var pody = 'CarID=' + escape(CarID) + '&npd=1&ProvinceID=' + escape(provinceid) + '&CityID=' + areaid + '&level=' + levelstr + '&random=' + Math.random();
        $('#divMemberList').load('../../AjaxServers/TaskManager/NoDealerOrder/YpDealerSelect.aspx #divMemberList > *', pody, LoadDivSuccess);
    }

    function PageForHC(PageIndex) {
        var pody = 'PageIndex=' + escape(PageIndex) + '&npd=1&random=' + Math.random();
        $('#divMemberList').load('../../AjaxServers/TaskManager/NoDealerOrder/YpDealerSelect.aspx #divMemberList > *', pody, LoadDivSuccess);
    }
    //点击文字，选中复选框
    function emChkIsChoose(othis) {
        var $checkbox = $(othis).prev();
        if ($checkbox.is(":checked")) {
            $checkbox.removeAttr("checked");
        }
        else {
            $checkbox.attr("checked", "checked");
        }
    }

</script>
<div class="pop pb15 openwindow" style="width: 650px">
    <div class="title bold">
        <h2>
            选择经销商</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('YpDealerSelectAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable" style="border-bottom: dotted  1px #0099FF; width: 630px">
        <li class="name1" style="width: 400px">
            <label>
                请选择所在省份：</label><select id="ddlProvince" class="w80" onchange="javascript:selectDealertriggerProvince()"
                    runat="server"></select><select id="ddlCity" class="w80" style="margin-left: 5px"
                        runat="server" onchange="javascript:selectDealertriggerCity()"></select><select id="ddlCounty"
                            runat="server" class="w80" style="margin-left: 5px"></select></li>
        <li style="width: 180px; float: left;">
            <label style="width: 80px;">
                经销商级别：</label>
            <input type="checkbox" value="1" name="level" id="rb4S" /><em onclick="emChkIsChoose(this)">4S</em><input
                name="level" id="rbZH" type="checkbox" value="0" /><em onclick="emChkIsChoose(this)">综合</em></li>
        <li style="width: 200px; float: left;">
            <label style="width: 120px;">
                是否有报价：</label>
            <input type="checkbox" value="1" name="price" id="rbhavePrice" /><em onclick="emChkIsChoose(this)">有</em>
            <input type="checkbox" value="0" name="price" id="rbnoPrice" /><em onclick="emChkIsChoose(this)">无</em></li>
        <li class="btn">
            <input name="" type="button" value="查 询" onclick="javascript:MemberSearch();" class="btnSave bold" /></li>
    </ul>
    <div class="line">
    </div>
    <div id="divall">
        <div class="clearfix  outTable">
            <%=strhtml%>
        </div>
        <div class="Table2" id="divMemberList">
            <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
                id="tableMemberList">
                <tbody>
                    <tr class="bold">
                        <th width="7%">
                            操作
                        </th>
                        <th width="14%">
                            经销商名称
                        </th>
                        <th width="9%">
                            级别
                        </th>
                        <th width="15%">
                            电话
                        </th>
                        <th width="14%">
                            所在城市
                        </th>
                        <th width="20%">
                            地址
                        </th>
                        <th width="11%">
                            是否有报价
                        </th>
                        <th width="10%">
                            销售类型
                        </th>
                    </tr>
                    <asp:repeater id="DealerList" runat="server">
                                <ItemTemplate>
                    <tr>
                        <td>
                            <a href="javascript:SelectMember('<%# Eval("vendorName") %>','<%# Eval("DealerId") %>','<%# Eval

("vendorBizMode") %>','<%# Eval("vendorTel")%>','<%#BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(Eval("cityId").ToString())%>','<%#Eval

("vendorSaleAddr")%>');">选择</a>&nbsp;
                        </td>
                        <td class="l">
                            <a href="../../AjaxServers/TaskManager/NoDealerOrder/ReturnMemberDetail.aspx?DealerID=<%# Eval("DealerId") %>" target="_blank">
                                <%# Eval("vendorName")%></a>
                        </td>
                        <td class="l">
                            <%# Eval("vendorBizMode").ToString()=="1"?"4S":"综合"%>&nbsp;
                        </td>
                        <td class="l">
                            <div style="float: left; word-wrap: break-word; word-break: break-all; text-align: left;">
                                <%# Eval("vendorTel").ToString().Trim()%>&nbsp;</div>
        </td>
        <td class="1">
            <%#BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(Eval("cityId").ToString())%>&nbsp;
        </td>
        <td class="l">
            <%# Eval("vendorSaleAddr")%>&nbsp;
        </td>
        <td class="l"><%# Eval("ishavePrice").ToString() == "1" ? "有" : "无"%>&nbsp;</td>
        <td class="l"><%# Eval("MemberSaleType").ToString() == "0" ? "" :(Eval("MemberSaleType").ToString() == "1"?"销售":"试用")%>&nbsp;</td>
        </tr> </ItemTemplate> </asp:repeater>
                </tbody>
            </table>
            <div class="pageTurn mr10">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </div>
    </div>
</div>
