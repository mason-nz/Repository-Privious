<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectBrand.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory.SelectBrand" %>

<link href="../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../Css/style.css" type="text/css" rel="stylesheet" />
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
        var CustomBrandIDs = $("#tableCustBrandSelect a[name^='Custom']").map(function () { return $(this).attr('name'); }).get().join(",");
        var group = '<%=Request["group"] %>';
        if (group == 'yes') {
            if (CustomBrandIDs != "") {
                var listids = CustomBrandIDs.split(',');
                var testid = '';
                for (var i = 0; i < listids.length; i++) {
                    if (testid == '') {
                        if (listids[i] != "") {
                            testid = listids[i];
                        }
                    } else {
                        if (listids[i] != "") {
                            if (testid != listids[i]) {
                                $.jAlert('您不能选择属于不同分类的主营品牌');
                                return false;
                            }
                        }
                    }
                }
            }
        }
        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandids', ids);
        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('brandnames', values);
        $('#popupLayer_' + 'BrandSelectAjaxPopup').data('CustomBrandIDs', CustomBrandIDs);
        $.closePopupLayer('BrandSelectAjaxPopup',true);
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
        var pody = 'requesttype=sd&Name=' + escapeStr($('#txtBrandName').val()) + '&page=1' + '&random=' + Math.random();
        $('#divBrandList').load('/AjaxServers/CustCategory/SelectBrand.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divBrandList').load('/AjaxServers/CustCategory/SelectBrand.aspx #divBrandList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableBrandList');
        SetTableDbClickEvent();
        ClearSelectedButtom();
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
        var group = '<%=Request["group"] %>';
        if (group == 'yes') {
            var CustomBrandIDss = $('#tableCustBrandSelect tbody tr').find('a[name="' + parentNodes.eq(4).find('a').attr('name') + '"]');
            var trs = $('#tableCustBrandSelect tbody tr');
            if (trs.size() > 1 && CustomBrandIDss.size() <= 0) {
                $.jAlert('您不能选择属于不同分类的主营品牌：' + brandName + '！');
                return;
            }
        }


        var trhtml = '<tr><td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td class="l">' + parentNodes.eq(1).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(2).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(3).html() + '</td>'
               + '<td  style=" display:none;">' + parentNodes.eq(4).html() + '</td></tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        ClearSelectedButtom();
    }

    //根据选定的品牌，清空选择区域中对应的按钮
    function ClearSelectedButtom() {
        enterSearch(CustBrandSearch);
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
        <h2>主营品牌</h2><span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup',false);"></a></span></div>
    <ul class="clearfix  outTable">
        <li class="name1">名称或分类：
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
                    <th width="25%">
                        编号
                    </th>
                    <th width="25%">
                        名称
                    </th>
                    <th width="20%">
                        品牌分类
                    </th>
                    <th style="display: none;">
                        所属主营品牌组编号
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                                <a href="javascript:SelectCustBrand('<%# Eval("BrandID") %>','<%# Eval("Name")%>');" name='<%# Eval("Name")%>' id='<%# Eval("BrandID") %>'>选择</a>
                                                </td>
                                        <td class="l">
                                            <%# Eval("BrandID")%>&nbsp;
                                        </td>
                                        <td class="l">
                                            <%# Eval("Name")%>&nbsp;
                                        </td> 
                                        <td class="l">
                                            <%# Eval("CustomBrandName")%>&nbsp;
                                        </td> 
                                        <td style=" display:none;" >
                                            <a name="<%# Eval("CustomBrandID")%>"><%# Eval("CustomBrandID")%></a>&nbsp;
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
      
    </div>
      <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List mt20"
            id="tableCustBrandSelect">
            <tbody>
                <tr class="bold">
                    <th width="20%">
                        操作
                    </th>
                    <th width="25%">
                        编号
                    </th>
                    <th width="25%">
                        名称
                    </th>
                    <th width="20%">
                        品牌分类
                    </th>
                    <th style="display: none;">
                        所属主营品牌组编号
                    </th>
                </tr>
                <asp:literal id="literalEditCont" runat="server"></asp:literal>
            </tbody>
        </table>
        <div class="btn" style="margin: 20px 10px 10px 0px;">
            <input type="button" name="" value="提 交" onclick="javascript:SaveSelectBrand();"
                class="btnSave bold">&nbsp;&nbsp;
            <input type="button" name="" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('BrandSelectAjaxPopup',false);">
        </div>
