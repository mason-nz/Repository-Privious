<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustInfoView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask.CustInfoView" %>

<!--其他任务专用控件 2016-8-3 强斐-->
<script type="text/javascript">
    function loadContacts() {
        //显示客户联系人
        AjaxPostAsync('/ReturnVisit/CC_Contact/ListWithEdit.aspx?TaskType=' + escape("其他任务"), {
            ContentElementId: 'ContactInfoContent',
            CustID: '<%= CustID %>',
            NoTab: 0,
            PageSize: 5
        }, null, function (html) {
            $('#ContactInfoContent').append(html);
        });
    }
    $(document).ready(function () {
        loadContacts();
    });
</script>
<div class="cont_cx khxx CustInfoArea">
    <div id="divCustBasicInfo">
        <ul class="infoBlock firstPart">
            <li>
                <label style="width: 115px">
                    客户ID：</label>
                <a href="/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%=CustID %>" target="_blank">
                    <%=CustID %></a> </li>
            <li>
                <label style="width: 115px">
                    客户名称：</label>
                <span id="spanCustName" name="CustName" style="float: left;" runat="server" />
                <div id="spanStatus" runat="server" style="float: left; margin: 8px 3px; display: none;">
                    <img alt="停用" src="/Images/xt_1.gif" id="imgStatus" /></div>
                <div id="spanLock" runat="server" style="float: left; margin: 8px 3px; display: none;">
                    <img alt="锁定" src="/Images/lock.gif" id="imgLock" /></div>
            </li>
            <li>
                <label style="width: 115px">
                    客户类别：</label>
                <span id="spanCustType" name="CustType" style="float: none;" runat="server" />
            </li>
            <li>
                <label style="width: 115px">
                    主营品牌：</label>
                <span id="spanBrandName" name="BrandName" style="float: none;" runat="server" />
            </li>
            <li>
                <label style="width: 115px">
                    客户地区：</label>
                <span id="spanArea" name="CustIndustry" style="float: none;" runat="server" />
            </li>
            <li>
                <label style="width: 115px">
                    注册地址：</label>
                <span id="spanAddress" name="Address" style="float: none;" runat="server" />
            </li>
            <li>
                <label style="width: 115px">
                    联系人：</label>
                <span id="spanContactName" name="ContactName" style="float: none;" runat="server" />
            </li>
            <li>
                <label style="width: 115px">
                    联系电话：</label>
                <span id="spanOfficeTel" name="OfficeTel" style="float: none;" runat="server" />
                <span style="float: none;"><a ctel="<%=OfficeTel %>" href="javascript:void(0);" onclick="CallOutForCRM('<%=OfficeTel %>','<%=FirstMemberCode %>','<%=FirstMemberName %>','<%= CustID %>','<%=ContactName %>','-1');">
                    <img alt="打电话" style="vertical-align: middle;" src="/images/phone.gif" border="0" /></a>
                </span></li>
            <li>
                <label style="width: 115px">
                    邮政编码：</label>
                <span id="spanZipcode" name="Zipcode" style="float: none;" runat="server" />
            </li>
        </ul>
        <asp:repeater id="rptMember" runat="server">
        <ItemTemplate>
        <div class="spliter">
        </div>
        <ul class="infoBlock firstPart ">
            <li>
                <label style="width: 115px">
                    会员ID：</label>
                <span style="float: none;" ><a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%#Eval("ID") %>&CustID=<%= CustID %>' target="_blank">
                <%#Eval("MemberCode")%></a></span></li>
            <li>
                <label style="width: 115px">
                    会员名称：</label>
                <span style="float: none;" ><%#Eval("Name")%></span>
            </li>
            <li>
                <label style="width: 115px">
                    会员类型：</label>
                <span style="float: none;" ><%#GetMemberTypeStr(Eval("MemberType").ToString())%></span>
            </li>
            <li>
                <label style="width: 115px">
                    主营品牌：</label>
                <span style="float: none;" ><%#Eval("BrandNames")%></span>
                </li>
            <li>
                <label style="width: 115px">
                    会员地区：</label>
                <span style="float: none;" ><%#GetAreaStr(Eval("ProvinceID").ToString(), Eval("CityID").ToString(), Eval("CountyID").ToString())%></span>
                </li>
            <li>
                <label style="width: 115px">
                    会员电话：</label>
                    <span id="spanMemberTel" style="float: none;" ><%#Eval("Phone")%></span><span style="float: none;" >
                    <a ctel="<%#Eval("Phone")%>" href="javascript:void(0);"
                    onclick="javascript:CallOutForCRM($('#spanMemberTel').text(),'<%#Eval("MemberCode")%>','<%#Eval("Name")%>','<%= CustID %>');">
                    <img alt="打电话" src="/images/phone.gif" border="0" /></a></span></li>
            <li>
                <label style="width: 115px">
                    邮政编码：</label>
                <span style="float: none;" ><%#Eval("Postcode")%></span></li>
                <li>
                <label style="width: 115px">
                    销售地址：</label>
                <span style="float: none;" ><%#Eval("ContactAddress")%></span></li>
        </ul>
        </ItemTemplate>
         </asp:repeater>
        <div class="spliter">
        </div>
        <ul class="infoBlock">
            <li class="singleRow">
                <label style="width: 115px;">
                    客户联系人：</label>
                <%if (IsShowBtn == "yes")
                  { %>
                <a style="float: right; margin-right: 120px; cursor: Pointer;" onclick="javascript:if(contactInfoList_CC_Helper){contactInfoList_CC_Helper.addNewContactInfo(<%=CustType %>);}">
                    添加联系人</a>
                <%} %>
                <div id="ContactInfoContent" class="fullRow  cont_cxjg" style="margin-left: 78px;
                    float: left;">
                </div>
            </li>
        </ul>
    </div>
</div>
