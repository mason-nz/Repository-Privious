<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMCustPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.CRMCustPop" %>
 
<script type="text/javascript">
    $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式
    var NeedPageLoad = '<%=NeedPageLoad %>';
    //查询操作
    function MemberSearch() {

        var pody = 'CustName=' + escape($.trim($('#txtCustName1').val())) + '&npd=1&page=1' + '&random=' + Math.random();
        LoadingAnimation("divMemberList");
        $('#divMemberList').load('/WorkOrder/CRMCustPop.aspx #divMemberList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divMemberList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectMember(CustName, CRMCustID, pid, cid, tid) {

        $('#popupLayer_' + 'CRMCustAjaxPopup').data('CustName', CustName);
        $('#popupLayer_' + 'CRMCustAjaxPopup').data('CrmCustID', CRMCustID);
        $('#popupLayer_' + 'CRMCustAjaxPopup').data('PID', pid);
        $('#popupLayer_' + 'CRMCustAjaxPopup').data('CID', cid);
        $('#popupLayer_' + 'CRMCustAjaxPopup').data('TID', tid);
        $.closePopupLayer('CRMCustAjaxPopup',true);

    }
    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation("divMemberList");
        $('#divMemberList').load('/WorkOrder/CRMCustPop.aspx #divMemberList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    $(function () {
        $("#txtCustName1").val("<%=CustName %>");
        enterSearch(MemberSearch);
        if (NeedPageLoad == "0") {
            MemberSearch();
        }
    });

    //清空已选项
    function popClearCustInfo() {
        SelectMember("", "", "-1", "-1", "-1");
    }

</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            客户查询</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('CRMCustAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1">客户名称：
            <input type="text" name="txtCustName1" id="txtCustName1" class="w190" /></li>
        <li class="btn" style="width:150px">
            <input name="" type="button" value="查 询" onclick="javascript:MemberSearch();" class="btnSave bold" />&nbsp;&nbsp;&nbsp;<a id="a1" href="javascript:void(0);"
                            onclick="popClearCustInfo()">清空已选项</a></li>
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
                        客户名称
                    </th>
                    <th width="25%">
                        客户ID
                    </th>
                    <th width="20%">
                        客户类型
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td> 
                                                <a href="javascript:SelectMember('<%# Eval("custName") %>','<%# Eval("CustID") %>','<%# Eval("ProvinceID") %>','<%# Eval("CityID") %>','<%# Eval("CountyID") %>')">选择</a>
                                                </td>
                                        <td class="l">
                                            <a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%# Eval("CustID") %> ' target="_blank"><%# Eval("custName")%></a>&nbsp;
                                        </td>
                                        <td class="l">
                                            <%# Eval("CustID")%>&nbsp;
                                        </td> 
                                        <td class="l">
                                            <%# GetTypeName(Eval("TypeID").ToString())%>&nbsp;
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
</div>
