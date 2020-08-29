<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCarSerial.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Base.SelectCarSerial" %>

<script type="text/javascript">
    $('#tableSerialBrandList tr:even').addClass('color_hui'); //设置列表行样式
    $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式

    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableSerialBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }
    function SaveSelectSerialBrand() {
        var ids = $("#tableSerialBrandSelect a[id][href]").map(function () { return $(this).attr('id'); }).get().join(",");
        var values = $("#tableSerialBrandSelect a[id][href]").map(function () { return $(this).attr('name'); }).get().join(",");

        $.closePopupLayer('SerialBrandSelectAjaxPopup', true, { BrandIDs: ids, BrandNames: values });
    }

    SetTableDbClickEvent();


    //删除操作
    function DelSelectSerialBrand(brandid) {
        $('#tableSerialBrandSelect tr:even').removeClass('color_hui');
        $('#tableSerialBrandSelect tr').removeAttr('style');
        $("#tableSerialBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableSerialBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableSerialBrandSelect');
    }
    //查询操作
    function SerialBrandSearch() {
        var pody = 'requesttype=sd&BrandID=' + escapeStr($('#ddlBrandList').val()) + '&Name=' + escapeStr($('#txtBrandName').val()) + '&page=1';
        $('#divSerialBrandList').load('/AjaxServers/Base/SelectCarSerial.aspx #divSerialBrandList > *', pody, LoadDivSuccess);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divSerialBrandList').load('/AjaxServers/Base/SelectCarSerial.aspx #divSerialBrandList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divSerialBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('divSerialBrandList');
        SetTableDbClickEvent();
    }
    //选择操作
    function SelectSerialBrand(brandID, name) {
        var idsame = $('#tableSerialBrandSelect tbody tr').find('a[id="' + brandID + '"]');
        var parentNodes = $("a[id='" + brandID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(3).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过品牌名称为：' + brandName + '的这条记录了！');
            return;
        }
        var trhtml = '<tr><td><a href="javascript:DelSelectSerialBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td class="l">' + parentNodes.eq(1).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(2).html() + '</td>'
               + '<td class="l">' + parentNodes.eq(3).html() + '</td></tr>';
        $('#tableSerialBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableSerialBrandSelect');
        $('#tableSerialBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
    }
</script>
<script type="text/javascript">
    $(function () {
        enterSearch(SerialBrandSearch);
        $('#txtBrandName').keypress(function (k) {
            if (k.keyCode == 13) { SerialBrandSearch(); }
        });
    });
</script>
<div class="pop pb15 openwindow" style="width:700px">
    <div class="title bold">
        <h2>
            主营品牌</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SerialBrandSelectAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1" style="width:255px">
            <label>
                主营品牌：</label><select name="ddlBrandList" class="w190" id="ddlBrandList" runat="server"></select></li>
        <li style="width: 290px; float: left;">
            <label>
                品牌名称：</label><input type="text" name="txtSerialBrandName" id="txtBrandName" runat="server" /></li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:SerialBrandSearch();" /></li>
    </ul>
    <div id="divSerialBrandList" class="Table2">
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableSerialBrandList">
            <tbody>
                <tr>
                    <th width="20%">
                        操作
                    </th>
                    <th width="25%">
                        主营品牌名称
                    </th>
                    <th width="25%">
                        品牌编号
                    </th>
                    <th width="30%">
                        品牌名称
                    </th>
                </tr>
                <asp:repeater id="repterSerialMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> <a href="javascript:SelectSerialBrand('<%# Eval("SerialID") %>' ,'<%# Eval("Name")%>');"  name='<%# Eval("Name")%>' id='<%# Eval("SerialID") %>'>选择</a>
                                                </td>
                                        <td class="l">
                                            <%# Eval("BrandName")%>
                                        </td> <td class="l">
                                            <%# Eval("SerialID")%>
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
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
    </div>
    <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List mt20"
        id="tableSerialBrandSelect">
        <tbody>
            <tr>
                <th width="20%">
                    操作
                </th>
                <th width="25%">
                    主营品牌名称
                </th>
                <th width="25%">
                    品牌编号
                </th>
                <th width="30%">
                    品牌名称
                </th>
                <asp:literal id="literalEditCont" runat="server"></asp:literal>
        </tbody>
    </table>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" class="btnSave bold" value="提交" onclick="javascript:SaveSelectSerialBrand();" />
        <input type="button" class="btnSave bold" value="取消" onclick="javascript:$.closePopupLayer('SerialBrandSelectAjaxPopup',false);" />
    </div>
</div>
