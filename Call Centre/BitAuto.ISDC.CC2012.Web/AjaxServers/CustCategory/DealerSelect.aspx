<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerSelect.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory.DealerSelect" %>

<link href="../../Css/base.css" type="text/css" rel="stylesheet" />
<link href="../../Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式
    var NeedPageLoad = '<%=NeedPageLoad %>';
    //查询操作
    function MemberSearch() {
        var pody = 'requesttype=sd&npd=1&MemberName=' + escape($.trim($('#txtMemberName').val())) + '&page=1' + '&random=' + Math.random();
        LoadingAnimation("divMemberList");
        $('#divMemberList').load('/AjaxServers/CustCategory/DealerSelect.aspx #divMemberList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divMemberList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectMember(membername, MemberCode, membertype, brandnames, brandids, custid) {

        //        if ($.jConfirm('确定选择此客户吗')) {
        //            debugger
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberName', membername);
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberCode', MemberCode);
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberType', membertype);
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('BrandNames', brandnames);
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('Brandids', brandids);
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('CustID', custid);
        $.closePopupLayer('DealerSelectAjaxPopup');
        //}
    }
    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation("divMemberList");
        $('#divMemberList').load('/AjaxServers/CustCategory/DealerSelect.aspx #divMemberList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    $(function () {
        enterSearch(MemberSearch);
        if (NeedPageLoad == "0") {
            MemberSearch();
        }
    });
    function ClearDealer() {
        //给经销商工单初始化客户信息
        if (typeof ClearCustInfo == "function") {
            ClearCustInfo();
        }
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberName', "");
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberCode', "");
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberType', "");
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('BrandNames', "");
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('Brandids', "");
        $('#popupLayer_' + 'DealerSelectAjaxPopup').data('CustID', "");
        $.closePopupLayer('DealerSelectAjaxPopup');
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            经销商查询</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('DealerSelectAjaxPopup');">
        </a></span>
    </div>
    <ul class="clearfix  outTable" style="width: 500px">
        <li class="name1">经销商名称：
            <input type="text" name="txtMemberName" id="txtMemberName" runat="server" class="w190" /></li>
        <li class="btn" style="width: 150px">
            <input name="" type="button" value="查 询" onclick="javascript:MemberSearch();" class="btnSave bold" />&nbsp;&nbsp;<a
                id="ClearDealer" onclick="ClearDealer()" style="cursor: pointer;">清空</a></li>
    </ul>
    <div class="Table2" id="divMemberList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableMemberList">
            <tbody>
                <tr class="bold">
                    <th width="10%">
                        操作
                    </th>
                    <th width="25%">
                        经销商名称
                    </th>
                    <th width="25%">
                        会员ID
                    </th>
                    <th width="20%">
                        经销商类型
                    </th>
                    <th width="20%">
                        品牌
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                                <a href="javascript:SelectMember('<%# Eval("name") %>','<%# Eval("MemberCode") %>','<%# Eval("membertype") %>','<%# Eval("brandnames")%>','<%# Eval("brandids")%>','<%# Eval("CustID")%>');" name='<%# Eval("MemberCode")%>' id='<%# Eval("MemberCode") %>'>选择</a>
                                                </td>
                                        <td class="l">
                                            <a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%# Eval("ID") %>&CustID=<%# Eval("CustID") %>&i=0' target="_blank"><%# Eval("name")%></a>&nbsp;
                                        </td>
                                        <td class="l">
                                            <%# Eval("MemberCode")%>&nbsp;
                                        </td> 
                                        <td class="l">
                                            <%# Eval("membertype").ToString() == "1" ? "4s" : Eval("membertype").ToString() == "2" ? "特许经销商" : "综合店"%>&nbsp;
                                        </td> 
                                        <td cls="1"><%#Eval("brandnames")%>&nbsp;</td>
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
</div>
