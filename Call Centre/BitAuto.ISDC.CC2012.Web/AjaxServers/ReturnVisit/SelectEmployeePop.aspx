<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectEmployeePop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.SelectEmployeePop" %>

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

        var idList = new Array();

        $("#tableCustBrandSelect a[userid][href]").each(function (i) {

            var id = {
                CustID: encodeURIComponent($(this).attr('custid')),
                UserID: encodeURIComponent($(this).attr('userid'))
            };
            idList.push(id);
        });

        RecedeCrmOnePop(idList);
    }

    SetTableDbClickEvent();


    function RecedeCrmOnePop(IDs) {
        if (IDs.length > 0) {
            $.jConfirm("确定要回收所选择的的员工吗？", function (r) {
                if (r) {
                    //回收任务
                    var str = JSON.stringify(IDs);
                    $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "RecedeTask", IDs: encodeURIComponent(str) }, function (data) {
                        if (data == "success") {
                            ClearNextCallTime();
                        }
                        else {
                            alert(data);
                        }
                    });
                }
            });

        }
        else {
            $.jAlert("请至少选择一个要回收的员工！");
        }
    }

    //清除客户下次回访日期
    function ClearNextCallTime() {
        //获取要清空的客户ID
        var ids = "";
        ids = $("#tableCustBrandSelect a[userid][href]").map(function () { return $(this).attr('custid'); }).get().join(",");

        $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "ClearNextCallTime", CustIDs: encodeURIComponent(ids) }, function (data) {
            if (data == "success") {
                search();
                $.closePopupLayer('EmployeeSelectAjaxPopup', false);
            }
            else {
                alert(data);
                search();
                $.closePopupLayer('EmployeeSelectAjaxPopup', false);
            }
        });
    }

    //删除操作
    function DelSelectCustBrand(custID, userID) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[custid='" + custID + "'][userid='" + userID + "'][href]").parent().parent().remove();
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
        //$('#divBrandList').load('/AjaxServers/Base/SelectBrand.aspx #divBrandList > *', pody, LoadDivSuccess);
        $('#divBrandList').load('/AjaxServers/ReturnVisit/SelectEmployeePop.aspx #divBrandList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divBrandList tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableBrandList');
        SetTableDbClickEvent();
    }
    //选择操作
    function SelectCustBrand(custID, userID, name) {
        var idsame = $('#tableCustBrandSelect tbody tr').find('a[custid="' + custID + '"][userid="' + userID + '"]');
        var parentNodes = $("a[custid='" + custID + "'][userid='" + userID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(1).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过负责员工姓名为：' + brandName + '的这条记录了！');
            return;
        }


        var trhtml = '<tr><td><a href="javascript:DelSelectCustBrand(\'' + custID + '\',\'' + userID + '\');"  name=\'' + name + '\' custid=\'' + custID + '\' userid=\'' + userID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td class="l">' + parentNodes.eq(1).html() + '</td></tr>'
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
            回收负责员工</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('EmployeeSelectAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <%--<li class="name1">
            <label>
                品牌名称：</label>
            <input type="text" class="k200" name="txtBrandName" id="txtBrandName" runat="server" /></li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:CustBrandSearch();" /></li>--%>
    </ul>
    <div id="divBrandList" class="Table2">
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableBrandList">
            <tbody>
                <tr>
                    <th width="20%">
                        操作
                    </th>
                    <th width="80%">
                        负责员工
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                            <a href="javascript:SelectCustBrand('<%# Eval("custid") %>','<%# Eval("userid") %>','<%# Eval("TrueName")%>');" name='<%# Eval("TrueName")%>' custid='<%# Eval("custid")%>' userid='<%# Eval("userid") %>'>选择</a>
                                        </td>                                        
                                        <td class="l">
                                            <%# Eval("TrueName")%>
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
                <th width="80%">
                    负责员工
                </th>
                <asp:literal id="literalEditCont" runat="server"></asp:literal>
        </tbody>
    </table>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" class="btnSave bold" value="提交" onclick="javascript:SaveSelectBrand();" />
        <input type="button" class="btnSave bold" value="取消" onclick="javascript:$.closePopupLayer('EmployeeSelectAjaxPopup',false);" />
    </div>
</div>
