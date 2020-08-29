<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelatedDemandsInfo.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.RelatedDemandsInfo" %>

<style type="text/css">
    .pop UL LI
    {
        line-height: 0px;
    }
</style>
<script type="text/javascript">
    $(function () {
        $('#tableRelateDemandsList tr:even').addClass('color_hui'); //设置列表行样式
    });

    function SelectDemandName(RelateDemandId) {
        $('#popupLayer_' + 'RelateDemandSelectAjaxPopup').data('RelateDemandId', RelateDemandId);
        $.closePopupLayer('RelateDemandSelectAjaxPopup', true);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divRelateDemandsList').load('/WorkOrder/RelatedDemandsInfo.aspx #divRelateDemandsList > *', pody);
    }
    function Search() {
        var pody = 'CustId=' + escapeStr($('#hidCustId').val())
        + '&phoneNum=' + escapeStr($.trim($("#<%=txtCustomMobileNum.ClientID%>").val()))
        + '&carBrandID=' + escapeStr($("#selCarBrand").val())
        + '&carTypeID=' + escapeStr($("#selCarType").val())
        + '&page=1&r=' + Math.random();
        LoadingAnimation("divRelateDemandsList");
        $('#divRelateDemandsList').load('/WorkOrder/RelatedDemandsInfo.aspx #divRelateDemandsList > *', pody);
    }
    $(function () {
        enterSearch(Search);
    });
    function GetCarTypeByBrand() {
        var objBrandID = $("#<%=this.selCarBrand.ClientID%>").val();

        $("#selCarType").children().remove();
        $("#selCarType").append("<option value='-1'>请选择车型</option>");
        if (objBrandID != "-1") {
            AjaxPostAsync("/WorkOrder/AjaxServers/RelatedDemandInfo.ashx", { Action: "GetCarTypesByBrandID", carBrandID: objBrandID, customID: "<%=CustId%>", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selCarType").append("<option value='" + jsonData[i].SerialIDs + "'>" + jsonData[i].SerialNames + "</option>");
                }
            });
        }
    }

</script>
<div id="jcDiv" class="pop pb15 openwindow" style="width: 760px">
    <div class="title bold">
        <h2>
            选择需求</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('RelateDemandSelectAjaxPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix outTable">
        <li class="name1" style="width: 320px; float: left;">下单车型： <span>
            <select id="selCarBrand" class="w120" onchange="javascript:GetCarTypeByBrand()" runat="server">
            </select>
            <select id="selCarType" class="w120">
                <option value='-1'>请选择车型</option>
            </select>
        </span></li>
        <li style="width: 210px; float: left">手机：
            <input class="w120" type="text" id="txtCustomMobileNum" runat="server"/>&nbsp;<em style=" color:Blue;" onclick="clearPhoneNum()">清空</em>
            <script type="text/javascript">
                function clearPhoneNum() {
                    $("#<%=txtCustomMobileNum.ClientID%>").val("");
                }
            </script>
        </li>
        <li class="btn" style="padding-top: 2px;">
            <input name="" class="btnSave bold" type="button" value="查 询" onclick="Search()" />
        </li>
    </ul>
    <div class="Table2" id="divRelateDemandsList">
        <input type="hidden" id="hidCustId" value='<%=CustId %>' runat="server" />
        <table cellspacing="0" cellpadding="0" border="0" width="98%" class="Table2List"
            id="tableRelateDemandsList" style="margin-top: 15px;">
            <tbody>
                <tr>
                    <th width="7%">
                        操作
                    </th>
                    <th width="15%">
                        需求ID
                    </th>
                    <th width="15%">
                        经销商名称
                    </th>
                    <th width="13%">
                        集客品牌
                    </th>
                    <th width="15%">
                        车型
                    </th>
                    <th width="10%">
                        需求状态
                    </th>
                    <th width="25%">
                        下单信息
                    </th>
                </tr>
                <asp:repeater id="repterFriendCustMappingList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td> 
                                    <a href="javascript:SelectDemandName('<%# Eval("DemandID") %>');" >选择</a>
                                    </td>
                            <td class="l">          
                               <a href='<%# strUrl + "?DemandID=" + Eval("DemandID").ToString() + "&r="+ new Random().Next()%>' target="_blank"> <%# Eval("DemandID")%></a> 
                            </td>
                            <td class="l">
                                <%# Eval("MemberName")%>
                            </td>
                            <td class="l">
                                <%# Eval("BrandNames")%>
                            </td>
                            <td class="l">
                                <%# Eval("SerialNames")%>
                            </td>
                            <td class="l">
                                 <%# GetStatusList(Eval("Status").ToString())%>
  
                            </td>
                            <td class="l" style="padding:0px 5px;">
                                 <%# GetOrderInfo(Eval("DemandID").ToString())%>
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
