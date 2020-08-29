<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectActivity.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.SelectActivity" %>

<script type="text/javascript">
    $(document).ready(function () {
        var ddlProvinceLoad = '<%= RequestProvinceID %>';
        var ddlCityLoad = '<%= RequestCityID %>';
        BindProvince('<%=this.ddlProvince.ClientID %>'); //绑定省份
        //$("select[id$='ddlProvince']").val(ddlProvinceLoad);
        $("select[id$='<%=this.ddlProvince.ClientID %>']").val(ddlProvinceLoad);
        TrigerProvice();
        //$("select[id$='ddlCity']").val(ddlCityLoad);
        $("select[id$='<%=this.ddlCity.ClientID %>']").val(ddlCityLoad);

        SetTableDbClickEvent();
    }

    );

    function TrigerProvice() {
        BindCity('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>');
    }


    //分页操作
    function ShowDataByPost3(pody) {
        pody += "&Paging=true&r="+ Math.random();
        $('#divBrandList').load('/TemplateManagement/SelectActivity.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableBrandList');
        SetTableDbClickEvent();
    }
    //选择操作
    function SelectCustBrand(brandID, name) {
        var activities = $('#tableCustBrandSelect tbody tr');
        if (activities.size() == 6) {
            $.jAlert("最多选择5个活动!");
            return;
        }

        var idsame = $('#tableCustBrandSelect tbody tr').find('a[id="' + brandID + '"]');
        var parentNodes = $("a[id='" + brandID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(1).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过活动名称为：' + brandName + '的这条记录了！');
            return;
        }
        var trhtml = '<tr><td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td class="l">' + parentNodes.eq(1).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(2).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(3).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(4).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(5).html() + '</td></tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
    }
    //删除操作
    function DelSelectCustBrand(brandid) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustBrandSelect');
    }
    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }

    function SaveSelectActivity() {
        var ids = $("#tableCustBrandSelect a[id][href]").map(function () { return $(this).attr('id'); }).get().join(",");
        var values = $("#tableCustBrandSelect a[id][href]").map(function () { return $(this).attr('name'); }).get().join(",");
        
        $.closePopupLayer('selectActivityAjaxPopup', true, { ActivityIDs: ids, ActivityNames: values });
    }

    //查询操作
    function ActivitySearch() {
        //var pody = { IsSearch: 'true', ProvinceID: $('#' + '<%=this.ddlProvince.ClientID %>').val(), CityID: $('#' + '<%=this.ddlCity.ClientID %>').val(), BrandID: '<%=CacheBrandID %>', ActivityIDs: '', page: '1', r: Math.random() };
        var pody = { IsSearch: 'true', ProvinceID: $('#' + '<%=this.ddlProvince.ClientID %>').val(), CityID: $('#' + '<%=this.ddlCity.ClientID %>').val(), ActivityIDs: '', page: '1', r: Math.random() };
        $('#divBrandList').load('/TemplateManagement/SelectActivity.aspx #divBrandList > *', pody, LoadDivSuccess);
    }

</script>
<div class="pop pb15 openwindow">
    <input type="hidden" id="hidProvinceID" value="-1" />
    <input type="hidden" id="hidCityID" value="-1" />
    <div class="title bold">
        <h2>
            选择活动</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('selectActivityAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1">
            <label>
                选择落地地区：</label>
            <select id="ddlProvince" class="w80" style="width: 80px" onchange="TrigerProvice()"
                runat="server">
            </select><select id="ddlCity" class="w80" style="margin-left: 5px; width: 70px" runat="server"></select></li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:ActivitySearch();" /></li>
    </ul>
    <div id="divBrandList" class="Table2">
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableBrandList">
            <tbody>
                <tr>
                    <th width="10%">
                        操作
                    </th>
                    <th width="16%">
                        活动名称
                    </th>
                    <th width="16%">
                        品牌
                    </th>
                    <th width="16%">
                        落地地区
                    </th>
                    <th width="16%">
                        开始时间
                    </th>
                    <th width="16%">
                        结束时间
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                            <a href="javascript:SelectCustBrand('<%# Eval("ActivityGuid") %>','<%# Eval("AvtivityName")%>');" name='<%# Eval("AvtivityName")%>' id='<%# Eval("ActivityGuid") %>'>选择</a>
                                        </td>
                                        <td class="l">
                                            <%--<a href='<%# Eval("url") %>' target="_blank"><%# Eval("AvtivityName")%></a>--%>
                                            <%#getOperator(Eval("url").ToString(), Eval("AvtivityName").ToString())%>&nbsp;
                                        </td>
                                        <td class="l">
                                            <%# Eval("BrandName")%>
                                        </td> 
                                        <td class="l">
                                            <%# Eval("ProvinceName") + " " + Eval("CityName")%>
                                        </td>
                                        <td class="l">
                                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("StartTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                                        </td> 
                                        <td class="l">
                                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                                        </td> 
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown1"></asp:literal>
            </p>
        </div>
    </div>
    <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List mt20"
        id="tableCustBrandSelect">
        <tbody>
            <tr>
                <th width="10%">
                    操作
                </th>
                <th width="16%">
                    活动名称
                </th>
                <th width="16%">
                    品牌
                </th>
                <th width="16%">
                    落地地区
                </th>
                <th width="16%">
                    开始时间
                </th>
                <th width="16%">
                    结束时间
                </th>
                <asp:literal id="literalEditCont" runat="server"></asp:literal>
        </tbody>
    </table>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" class="btnSave bold" value="提交" onclick="javascript:SaveSelectActivity();" />
        <input type="button" class="btnSave bold" value="取消" onclick="javascript:$.closePopupLayer('selectActivityAjaxPopup',false);" />
    </div>
</div>
