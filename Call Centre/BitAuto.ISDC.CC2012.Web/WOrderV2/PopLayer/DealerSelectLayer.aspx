<%@ Page Title="经销商查询" Language="C#" AutoEventWireup="true" CodeBehind="DealerSelectLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.DealerSelectLayer" %>

<script type="text/javascript"> 
    var memebernameOld = "<%=MemberName%>";
    
    $('#tableMemberList tr:even').addClass('color_hui'); //设置列表行样式
    //查询操作
    function MemberSearch() {
        var pody = "";
        if (memebernameOld != $.trim($('#<%=txtMemberName.ClientID%>').val())) {
            pody = 'requesttype=sd&nmc=1&MemberName=' + escape($.trim($('#<%=txtMemberName.ClientID%>').val())) + "&MemberCode=&CustId=" + escape("<%=CustId%>") + '&page=1' + '&random=' + Math.random();
        }
        else {
            pody = 'requesttype=sd&nmc=1&MemberName=' + escape($.trim($('#<%=txtMemberName.ClientID%>').val())) + "&MemberCode=" + escape("<%=MemberCode%>") + "&CustId=" + escape("<%=CustId%>") + '&page=1' + '&random=' + Math.random();
        }
        LoadingAnimation("divMemberListTable");
        $('#divMemberListTable').load('/WOrderV2/PopLayer/DealerSelectLayer.aspx #divMemberListTable > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divMemberList tr:even').addClass('color_hui'); //设置列表行样式
        $.resetPopupLayersPosition('DealerSelectPopup');
    }
    //选择操作
    function SelectMember(membername, memberCode, custName, custid) {
        $.closePopupLayer('DealerSelectPopup', true, { "MemberName": membername, "MemberCode": memberCode, "CustName": custName, "CustID": custid });
    }
    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation("divMemberListTable");
        $('#divMemberListTable').load('/WOrderV2/PopLayer/DealerSelectLayer.aspx #divMemberListTable > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    $(function () {
        enterSearch(MemberSearch); 
    });
    function ClearDealer() {
        $.closePopupLayer('DealerSelectPopup', true, { "MemberName": "", "MemberCode": "", "CustName": "", "CustID": "" });
    }
</script>
<!--经销商查询-->
<div class="pop_new w900 openwindow">
    <div class="title bold">
        <h2>
            经销商查询<span><a hidefocus="true" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('DealerSelectPopup');"></a></span></h2>
    </div>
    <div class="set_edit">
        <ul>
            <li class="jxs_title" style="width: 850px; padding: 0;">
                <label style=" *padding-top:3px;">经销商名称：</label>
                <span>
                    <input style="PADDING-BOTTOM: 5px; PADDING-TOP: 3px" type="text" class="w300" name="txtMemberName" id="txtMemberName" runat="server" /></span>
                <span class="cx_btn">
                    <input style=" height:28px;" name="" type="button" value="查询" onclick="javascript:MemberSearch();" />
                    <a href="#" id="ClearDealer" onclick="ClearDealer()">清空已选项</a> </span></li>
        </ul>
    </div>
    <div class="clearfix">
    </div>
    <div class="Table2" id="divMemberListTable" style="min-height: 100px;">
        <table border="0" cellpadding="0" cellspacing="0" class="bq_list jxs_list">
            <tr>
                <th width="6%">
                    操作
                </th>
                <th width="27%">
                    经销商全称
                </th>
                <th width="15%">
                    经销商简称
                </th>
                <th width="12%">
                    会员ID
                </th>
                <th width="10%">
                    经销商类型
                </th>
                <th width="23%">
                    品牌
                </th>
            </tr>
            <asp:repeater id="repterFriendCustMappingList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td> 
                            <a href="javascript:SelectMember('<%# Eval("name") %>','<%# Eval("MemberCode") %>','<%# Eval("CustName")%>','<%# Eval("CustID")%>');" name='<%# Eval("MemberCode")%>' id='<%# Eval("MemberCode") %>'>选择</a>
                        </td>
                        <td class="l" style=" text-align:left;">
                            <a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%# Eval("ID") %>&CustID=<%# Eval("CustID") %>&i=0' target="_blank"><%# Eval("name")%></a>&nbsp;
                        </td>
                        <td class="l">
                            <%# Eval("Abbr")%>&nbsp;
                        </td> 
                        <td class="l">
                            <%# Eval("MemberCode")%>&nbsp;
                        </td> 
                        <td class="l">
                            <%# Eval("membertype").ToString() == "1" ? "4s" : Eval("membertype").ToString() == "2" ? "特许经销商" : "综合店"%>&nbsp;
                        </td> 
                        <td cls="1"><%#Eval("brandnames")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
        </table>
        <div class="pageTurn mr10" style="margin-right: 20px;">
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
    </div>
</div>
<!--经销商查询-->
