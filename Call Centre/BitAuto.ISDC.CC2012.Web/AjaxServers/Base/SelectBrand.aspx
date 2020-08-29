<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectBrand.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Base.SelectBrand" %>

<script type="text/javascript">

    $('#tableBrandList tr:even').addClass('color_hui'); //设置列表行样式
    $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式

    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }
    function SaveSelectBrand() {
        var ids = $("#tableCustBrandSelect a[id][href]").map(function () { return $(this).attr('id'); }).get().join(",");
        var values = $("#tableCustBrandSelect a[id][href]").map(function () { return $(this).attr('name'); }).get().join(",");
        //    
        //        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandids', ids);
        //        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandnames', values);
        $.closePopupLayer('BrandSelectAjaxPopup', true, { BrandIDs: ids, BrandNames: values });
    }

    SetTableDbClickEvent();


    //删除操作
    function DelSelectCustBrand(brandid) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustBrandSelect');
    }
    //查询操作
    function CustBrandSearch() {
        var pody = 'requesttype=sd&name=' + escapeStr($('#txtBrandName').val()) + '&page=1&r=' + Math.random();
        $('#divBrandList').load('/AjaxServers/Base/SelectBrand.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //分页操作
    function ShowDataByPost3(pody) {
        $('#divBrandList').load('/AjaxServers/Base/SelectBrand.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableBrandList');
        SetTableDbClickEvent();
    }
    //选择操作
    function SelectCustBrand(brandID, name) {
        var idsame = $('#tableCustBrandSelect tbody tr').find('a[id="' + brandID + '"]');
        var parentNodes = $("a[id='" + brandID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(2).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过品牌名称为：' + brandName + '的这条记录了！');
            return;
        }
        var trhtml = '<tr><td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td class="l">' + parentNodes.eq(1).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(2).html() + '</td></tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
    }
</script>
<script type="text/javascript">
    $(function () {
        enterSearch(CustBrandSearch);
    });
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            主营品牌</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1">
            <label>
                品牌名称：</label>
            <input type="text" class="k200" name="txtBrandName" id="txtBrandName" runat="server" /></li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:CustBrandSearch();" /></li>
    </ul>
    <div id="divBrandList" class="Table2">
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableBrandList">
            <tbody>
                <tr>
                    <th width="20%">
                        操作
                    </th>
                    <th width="25%">
                        主营品牌编号
                    </th>
                    <th width="45%">
                        主营品牌名称
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                            <a href="javascript:SelectCustBrand('<%# Eval("BrandID") %>','<%# Eval("Name")%>');" name='<%# Eval("Name")%>' id='<%# Eval("BrandID") %>'>选择</a>
                                        </td>
                                        <td class="l">
                                            <%# Eval("BrandID")%>
                                        </td>
                                        <td class="l">
                                            <%# Eval("Name")%>
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
                <th width="20%">
                    操作
                </th>
                <th width="25%">
                    主营品牌编号
                </th>
                <th width="45%">
                    主营品牌名称
                </th>
                <asp:literal id="literalEditCont" runat="server"></asp:literal>
        </tbody>
    </table>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" class="btnSave bold" value="提交" onclick="javascript:SaveSelectBrand();" />
        <input type="button" class="btnSave bold" value="取消" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup',false);" />
    </div>
</div>
