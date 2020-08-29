<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDelCustRelation.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.AddDelCustRelation" %>

<script type="text/javascript" language="javascript">
    var addDelCustRelationHelper = (function () {
        var triggerProvince = function () {//选中省份
            BindCity('<%= this.selAddDelCustRelationProvince.ClientID %>', '<%= this.selAddDelCustRelationCity.ClientID %>');
            BindCounty('<%= this.selAddDelCustRelationProvince.ClientID %>', '<%= this.selAddDelCustRelationCity.ClientID %>', '<%= this.selAddDelCustRelationCounty.ClientID %>');
        },

        triggerCity = function () {//选中城市
            BindCounty('<%= this.selAddDelCustRelationProvince.ClientID %>', '<%= this.selAddDelCustRelationCity.ClientID %>', '<%= this.selAddDelCustRelationCounty.ClientID %>');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#<%= this.selAddDelCustRelationCounty.ClientID %> option').size() == 1)
            { $('#<%= this.selAddDelCustRelationCounty.ClientID %>').attr('noCounty', '1'); }
            else
            { $('#<%= this.selAddDelCustRelationCounty.ClientID %>').removeAttr('noCounty'); }
        },
        init = function () {
            //绑定区域
            BindProvince('<%= this.selAddDelCustRelationProvince.ClientID %>'); //绑定省份
            $('#<%= this.selAddDelCustRelationProvince.ClientID %>').val('<%= this.InitialProvinceID %>');
            triggerProvince();
            $('#<%= this.selAddDelCustRelationCity.ClientID %>').val('<%= this.InitialCityID %>');
            triggerCity();
            $('#<%= this.selAddDelCustRelationCounty.ClientID %>').val('<%= this.InitialCountyID %>');

        },
        openSelectCustomerPopup = function () {//选择交易市场 
            $.openPopupLayer({
                name: 'SelectCustomerPopup',
                parameters: { type: 3 },
                url: "/AjaxServers/Base/SelectCustomer.aspx?page=1",
                beforeClose: function (e, data) {
                    if (e) {
                        $('#<%= this.txtAddDelTradeMarketName.ClientID %>').val(data.CustName);
                        $('#<%= this.txtAddDelTradeMarketID.ClientID %>').val(data.CustID);
                    }
                }
            });
        },
        search = function () {
            var txtAddDelCustName = $.trim($('#txtAddDelCustName').val());
            var province = $.trim($('#selAddDelCustRelationProvince').val());
            var city = $.trim($('#selAddDelCustRelationCity').val());
            var county = $.trim($('#selAddDelCustRelationCounty').val());
            var txtAddDelTradeMarketID = $.trim($('#txtAddDelTradeMarketID').val());
            var txtAddDelCustContactName = $.trim($('#txtAddDelCustContactName').val());
            var txtAddDelAddress = $.trim($('#txtAddDelAddress').val());

            if (('<%=CarType %>' == '1' || '<%=CarType %>' == '3') &&
                txtAddDelCustName == '') {
                $.jAlert("客户名称关键字不能为空");
                return;
            }
            if (GetStringRealLength(txtAddDelCustName) > 100) {
                $.jAlert("客户名称关键字长度不能超过100个字符");
                return;
            }
            if (('<%=CarType %>' == '1' || '<%=CarType %>' == '3') &&
                 ~ ~province <= 0) {
                $.jAlert("查重客户地区必须选择一个省份");
                return;
            }
            if (GetStringRealLength(txtAddDelAddress) > 100) {
                $.jAlert("客户注册地址长度不能超过100个字符");
                return;
            }
            if ('<%=CarType %>' == '2' &&
                txtAddDelCustName == '' && txtAddDelAddress == '' &&
                 ~ ~province <= 0 && txtAddDelCustContactName == '') {
                $.jAlert("客户经营类型为二手车时，条件必须选择一个");
                return;
            }
            var pody = { Action: 'Search', r: Math.random() };
            pody.CustName = encodeURIComponent(txtAddDelCustName);
            pody.ProvinceID = province;
            pody.CityID = city;
            pody.CountyID = county;
            pody.ExceptCustID = '<%=ExceptCustID %>';
            pody.Address = txtAddDelAddress;
            pody.TradeMarketID = txtAddDelTradeMarketID;
            pody.TID = '<%=TID %>';
            pody.CustContactName = encodeURIComponent(txtAddDelCustContactName);
            LoadingAnimation('divAddDelCustRelationList');
            $('#divAddDelCustRelationList').load('/CustInfo/EditVWithCalling/AddDelCustRelation.aspx #divAddDelCustRelationList > *', pody);
        },
        //删除查询的客户
        delSearchCust = function (custid) {
            $('#divAddDelCustRelationList tr td :checkbox[value="' + custid + '"]').parent().parent().remove();
        },
        //批量删除查询的客户
        batchDelCust = function () {
            $('#divAddDelCustRelationList tr td :checkbox[checked]').parent().parent().remove();
        };

        return {
            triggerProvince: triggerProvince,
            triggerCity: triggerCity,
            init: init,
            search: search,
            delSearchCust: delSearchCust,
            batchDelCust: batchDelCust,
            openSelectCustomerPopup: openSelectCustomerPopup
        }
    })();
    $(function () {
        addDelCustRelationHelper.init();
    });

    function selectCheckBoxAll(objName, showType) {
        var delAllObj = document.getElementsByName(objName);
        if (showType == 1) {
            //全选
            $("#pages").disabled = true;
            for (var i = 0; i < delAllObj.length; i++) {
                if (delAllObj[i].disabled != true) {
                    delAllObj[i].checked = true;
                    delAllObj[i].disabled = true;
                }
            }
        }
        else if (showType == 2) {
            //反选
            for (var i = 0; i < delAllObj.length; i++) {
                if (delAllObj[i].checked) {
                    {
                        delAllObj[i].checked = false;
                        delAllObj[i].disabled = false;
                    }
                }
                else {
                    if (delAllObj[i].disabled == false) {
                        delAllObj[i].checked = true;
                    }
                }
            }
        }
        else if (showType == 3) {
            //取消
            for (var i = 0; i < delAllObj.length; i++) {
                if (delAllObj[i].checked == true) {
                    delAllObj[i].checked = false;
                    delAllObj[i].disabled = false;
                }
            }
        }
        else if (showType == 4) {
            for (var i = 0; i < delAllObj.length; i++) {
                if (delAllObj[i].disabled == false) {
                    delAllObj[i].checked = true;
                }
                else if (delAllObj[i].disabled == true && delAllObj[i].checked == true) {
                    delAllObj[i].disabled = false;
                }
            }
        }
    }    
</script>
<div class="spliter">
</div>
<ul style="" id="ulSameCustName" class="infoBlock firstPart">
    <li>
        <label style="width: 120px;">
            客户名称关键字：</label>
        <input type="text" id="txtAddDelCustName" class="w190" name="txtAddDelCustName" value='<%=CustName %>' />
        <em>*</em> </li>
    <li>
        <label>
            客户地区：</label>
        <select id="selAddDelCustRelationProvince" style="width: 63px;" name="Province" class="area"
            onchange="javascript:addDelCustRelationHelper.triggerProvince();" runat="server">
            <option value="-1">省/直辖市</option>
        </select>
        <select id="selAddDelCustRelationCity" name="City" style="width: 63px;" class="area"
            onchange="javascript:addDelCustRelationHelper.triggerCity();" runat="server">
            <option value="-1">城市</option>
        </select>
        <select id="selAddDelCustRelationCounty" name="County" style="width: 63px;" class="lastArea"
            runat="server">
            <option value="-1">区/县</option>
        </select>
        <em>*</em><input type="button" value="查重" id="btnAddDelCustRelationSearch" onclick="javascript:addDelCustRelationHelper.search();"
            style="width: 50px; float: right;" />
    </li>
    <% if (CarType != 1)
       {%>
    <li style="display: none;">
        <label style="width: 120px">
            所属交易市场：</label>
        <input id="txtAddDelTradeMarketName" class="w190" name="txtAddDelTradeMarketName"
            readonly="readonly" runat="server" />
        <input id="txtAddDelTradeMarketID" name="txtAddDelTradeMarketID" style="display: none;"
            runat="server" />
        <span><a href="javascript:addDelCustRelationHelper.openSelectCustomerPopup();">
            <img border="0" src="/images/button_001.gif" /></a> </span></li>
    <li>
        <label style="width: 120px;">
            联系人姓名：</label>
        <input type="text" id="txtAddDelCustContactName" class="w190" name="txtAddDelCustContactName" />
    </li>
    <li>
        <label>
            注册地址：</label>
        <input type="text" id="txtAddDelAddress" class="w190" name="txtAddDelAddress" />
    </li>
    <%} %>
    <li class="singleRow">
        <label style="width: 120px;">
            客户名称重复列表：</label>
        <div class="fullRow  cont_cxjg" style="margin-left: 78px;" id="divAddDelCustRelationList">
            <table cellspacing="0" cellpadding="0" border="0" style="width: 100%" class="cxjg">
                <tbody>
                    <tr>
                        <th width="5%">
                        </th>
                        <th width="10%">
                            客户ID
                        </th>
                        <th width="25%">
                            客户名称
                        </th>
                        <th width="20%">
                            客户简称
                        </th>
                        <th width="10%">
                            经营范围
                        </th>
                        <th width="10%">
                            客户状态
                        </th>
                        <th width="13%">
                            客户锁定状态
                        </th>
                        <th width="7%">
                            操作
                        </th>
                    </tr>
                    <asp:repeater id="repterCustInfo" runat="server" onitemdatabound="repterCustInfo_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="l"><input id="ckxAddDelCustRelationAll" name="ckxAddDelCustRelationAll" value='<%#Eval("CustID")%>' type="checkbox" style="width: 15px;"  /></td>
                <td><%#Eval("CustID")%></td>
                <td class="l"><a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%#Eval("CustID")%>' target="_blank"><%#Eval("CustName")%></a></td>
                <td class="l"><%#Eval("AbbrName")%></td>
                <td>
                   <asp:Literal id="litCustCarType" runat="server"></asp:Literal>
                </td>
                <td>
                   <asp:Literal id="litCustStatus" runat="server"></asp:Literal>
                </td>
                <td>
                <asp:Literal id="litCustLockStatus" runat="server"></asp:Literal>
                </td>
                <td><a href='javascript:addDelCustRelationHelper.delSearchCust("<%#Eval("CustID")%>");'>删除</a></td>
            </tr>               
        </ItemTemplate>
    </asp:repeater>
                </tbody>
            </table>
        </div>
        <input id="Button1" type="button" onclick="javascript:selectCheckBoxAll('ckxAddDelCustRelationAll',4);"
            value="全选" style="width: 50px;" />
        <input id="Button2" type="button" onclick="javascript:selectCheckBoxAll('ckxAddDelCustRelationAll',2);"
            value="反选" style="width: 50px;" />
        <input id="Button3" type="button" onclick="javascript:addDelCustRelationHelper.batchDelCust();"
            value="批量删除" style="width: 60px;" />
    </li>
</ul>
<div class="spliter">
</div>
