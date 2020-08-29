<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectBrandAndSerial.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory.SelectBrandAndSerial" %>

<link href="../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $(function () {
        enterSearch(CustBrandSearch);
    });
    $('#tableBrandList tr:even').addClass('color_hui'); //设置列表行样式
    $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式

    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }
    
    SetTableDbClickEvent();
    ClearSelectedButtom();

    //删除操作
    function DelSelectCustBrand(brandid) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableBrandList tbody tr td a[id="' + brandid + '"]').show();
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustBrandSelect');
    }
    //查询操作
    function CustBrandSearch() {
        var pody = 'name=' + escapeStr($('#txtBrandName').val()) + '&page=1' + '&random=' + Math.random();
        $('#divBrandList').load('/AjaxServers/CustCategory/SelectBrandAndSerial.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divBrandList').load('/AjaxServers/CustCategory/SelectBrandAndSerial.aspx #divBrandList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableBrandList');
        SetTableDbClickEvent();
        ClearSelectedButtom();
    }
    //选择操作
    function SelectCustBrand(bname, sname) {

        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('isSelected', "1"); //1、是选择后关闭
        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('serialname', sname);
        $.closePopupLayer('BrandSelectAjaxPopup');
    }

    //根据选定的品牌，清空选择区域中对应的按钮
    function ClearSelectedButtom() {
        $.each($('#tableCustBrandSelect tbody tr td a[id]'), function (i, n) {
            var obj = $('#tableBrandList tbody tr td a[id="' + $(n).attr('id') + '"]');
            if (obj.size() > 0) {
                obj.hide();
            }
        });
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            选择品牌和车型</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1">输入品牌或车型：
            <input type="text" name="txtBrandName" id="txtBrandName" runat="server" class="w190" /></li>
        <li class="btn">
            <input name="" type="button" value="查 询" onclick="javascript:CustBrandSearch();"
                class="btnSave bold" /></li>
    </ul>
    <div id="divBrandList" class="Table2">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableBrandList">
            <tbody>
                <tr class="bold">
                    <th width="20%">
                        操作
                    </th>
                    <th width="15%">
                        车型编号
                    </th>
                    <th width="25%">
                        品牌名称
                    </th>
                    <th width="40%">
                        车型名称
                    </th>
                    <th style="display: none;">
                        所属主营品牌组编号
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                                <a href="javascript:SelectCustBrand('<%# Eval("bname") %>','<%# Eval("sname")%>');" name='<%# Eval("sname")%>' id='<%# Eval("serialid") %>'>选择</a>
                                                </td>
                                        <td class="l">
                                            <%# Eval("serialid")%>&nbsp;
                                        </td>
                                        <td class="l">
                                            <%# Eval("bname")%>&nbsp;
                                        </td> 
                                        <td class="l">
                                            <%# Eval("sname")%>&nbsp;
                                        </td> 
                                        <td style=" display:none;" >
                                          
                                        </td> 
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
        
        <div class="btn" style="margin: 20px 10px 10px 0px;">
            
            <input type="button" name="" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup');">
        </div>
    </div>
