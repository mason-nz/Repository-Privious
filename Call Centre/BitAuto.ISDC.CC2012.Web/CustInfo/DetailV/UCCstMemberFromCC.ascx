<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCstMemberFromCC.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.UCCstMemberFromCC" %>
<script type="text/javascript" language="javascript"> 
        //弹出客户名称变更历史
    function openAjaxCstMemberNameHistory(memberId) {
        $.openPopupLayer({
            name: "CstMemberFullNameHistoryShowAjaxPopup",
            parameters: { CstMemberID: memberId },
            url: "/AjaxServers/CSTMember/CstMemberFullNameHistory.aspx"
        });
    }
        <%if (SyncStatusValue == "170002")
              { %>
        $(document).ready(
            function () {
                loadCSTAccountInfo<%=CstMemberID() %>();
            }
        ); 
        <%} %>
        function loadCSTAccountInfo<%=CstMemberID() %>() {
            var where = "";
            var cstMemberID = "<%=CstMemberID() %>";
            where += '&CSTMemberID=' + escape(cstMemberID);
            LoadingAnimation('cstAccountInfo_<%=CstMemberID() %>');
            $("#cstAccountInfo_<%=CstMemberID() %>").load("../../AjaxServers/CSTMember/CSTAccountInfo.aspx?r=" + Math.random(), where, AccountSuccessLoaded);
        }
        function AccountSuccessLoaded() {
        }
       
        function triggerMemberArea(contentId) {//隐藏、显示区域
            var content = $('#' + contentId);
            if (content.length == 0) { return; }
            if (content.is(':visible')) {
                content.hide('fast');
                currentMemberArea = null;
            }
            else {
                $('#divMembers .MemberInfoArea:visible').hide('fast');
                content.show('fast');
                currentMemberArea = content;
            }
        }
</script>
<style type="text/css">
    .huiyuanbox
    {
        margin: -27px 0 0 100px;
        _margin: -27px 0 0 50px;
        position: relative;
        float: left;
        width: 850px;
    }
    .huiyuanbox ul.list
    {
        height: 27px;
        overflow: hidden;
        float: left;
        padding: 0;
        float: left;
        margin: 0;
        width: 780px;
    }
    .huiyuanbox ul.list li
    {
        display: inline;
        float: left;
        width: auto;
        padding: 0 5px;
        height: 27px;
        line-height: 27px;
        background: url("/images/menu_er_bg.gif") no-repeat scroll right -6px;
    }
    .huiyuanbox ul.list li a
    {
        display: block;
        float: left;
        line-height: 27px;
        padding: 0 4px;
    }
    .huiyuanbox ul.list li.current
    {
        border-left: 1px solid #d6d6d6;
        border-top: 1px solid #d6d6d6;
        border-right: 1px solid #d6d6d6;
        border-bottom: 1px solid #fff;
        background: #FBF7F7;
    }
    .gegnduo
    {
        float: right;
    }
    .gegnduo a.gd
    {
        width: 40px;
        position: absolute;
        right: 15px;
        top: 5px;
        z-index: 1;
        padding: 0 5px;
        height: 22px;
        line-height: 22px;
    }
    .gegnduo a:hover.gd
    {
        border: 1px solid #ccc;
        border-bottom: 1px solid #fff;
        background: #fff;
        width: 38px;
        height: 22px;
        line-height: 22px;
    }
    .gegnduo ul.more
    {
        clear: both;
        float: right;
        margin: 0 15px 0 0;
        padding: 5px;
        border: 1px solid #ccc;
        position: absolute;
        top: 28px;
        right: 0;
        background: #fff;
        width: 100px;
    }
    .gegnduo ul.more li
    {
        width: auto;
        margin: 0;
        padding: 0;
        float: none;
    }
</style>
<div class="cont_cx khxx ">
    <div class="title" style="clear: both;">
        <%= this.ControlName %><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a></div>
    <div id="divMemberInfo_<%= this.ID %>" class="MemberInfoArea">
        <ul class="part01">
            <h3 style="background-color: #e1e1e1; margin: 5px 10px 5px 5px; padding: 5px;">
                车商通会员信息
            </h3>
            <li style="width: 590px; height: auto">
                <label style="width: 110px;">
                    会员全称：</label><span style="width: 300px;"><%=clientFullName%></span>
                <%if (FullNameHavHistory)
                  { %><a href="javascript:void(0)" onclick="openAjaxCstMemberNameHistory('<%=this.Member.ID %>')">曾用名</a>
                <%} %></li>
            <li style="height: auto; display: none">
                <label style="width: 110px;">
                    开通状态：</label>
                <span style="width: 185px; display: block">
                    <%=SyncStatus%><%=GetRejectedStr(SyncStatusValue)%></span></li>
        </ul>
        <ul>
            <li>
                <label style="width: 110px;">
                    会员简称：</label><span style="width: 185px;"><%=clientShortName%></span></li>
            <%--<li>
                        <label style="width: 110px;">
                            会员编码：</label><span style="width: 185px;"><%=clientCode%></span></li>--%>
            <%if (SyncStatusValue == "170002")
              { %>
            <li>
                <label style="width: 110px;">
                    会员ID：</label><span style="width: 185px;"><%=CSTMemberID%></span></li>
            <%} %>
            <li>
                <label style="width: 110px;">
                    会员类型：</label><span style="width: 185px;"><%=clientType%></span></li>
            <%if (vendorClass == "2")
              { %>
            <li style="height: auto;">
                <label style="width: 110px;">
                    主营品牌：</label><span style="width: 185px;"><%=BrandIdsName%></span></li>
            <%} %>
            <li>
                <label style="width: 110px;">
                    上级公司：</label><span style="width: 185px;"><%=upCompanyName%></span></li>
            <li>
                <label style="width: 110px;">
                    会员地区：</label><span style="width: 185px;"><%=region%></span></li>
            <li>
                <label style="width: 110px;">
                    邮编：</label><span style="width: 185px;"><%=postCode%></span></li>
            <li style="height: auto;">
                <label style="width: 110px;">
                    地址：</label><span style="width: 185px; word-wrap: break-word; word-break: normal;"><%=detailAddr%></span></li>
            <%-- <li style="height: auto;">
                        <label style="width: 110px;">
                            交通信息：</label><span style="width: 185px;word-wrap: break-word; word-break: normal;"><%=TrafficInfo%></span> </li>--%>
            <li style="height: 2px; margin-left: 10px; padding-top: 0px; width: 890px; background-image: url(/images/diandian_r.gif);
                background-repeat: repeat-x;"></li>
        </ul>
        <ul>
            <li>
                <label style="width: 110px;">
                    联系人姓名：</label><span style="width: 185px;"><%=contact%></span></li>
            <%--<li>
                        <label style="width: 110px;">
                            部门：</label><span style="width: 185px;"><%=depart%></span></li>
                    <li>
                        <label style="width: 110px;">
                            职务：</label><span style="width: 185px;"><%=position%></span></li>--%>
            <li>
                <label style="width: 110px;">
                    移动电话：</label><span style="width: 185px;"><%=phone%></span></li>
            <li>
                <label style="width: 110px;">
                    Email：</label><span style="width: 185px;"><%=email%></span></li>
            <%if (SyncStatusValue == "170002")
              { %>
            <li style="height: 2px; margin-left: 10px; padding-top: 0px; width: 890px; background-image: url(/images/diandian_r.gif);
                background-repeat: repeat-x;"></li>
            <%} %>
        </ul>
        <%if (SyncStatusValue == "170002")
          { %>
        <div class="spliter">
        </div>
        <ul>
            <li>
                <label style="width: 110px;">
                    车商币余额：</label><%=UCount%></li>
            <li>
                <label style="width: 110px;">
                    累计充值车商币：</label><%=UbTotalAmount%></li>
            <li>
                <label style="width: 110px;">
                    累计消费车商币：</label><%=UbTotalExpend%></li>
            <li>
                <label style="width: 110px;">
                    最后充值时间：</label><%=lastAddUbTime%></li>
            <li>
                <label style="width: 110px;">
                    有效截止日：</label><%=activeTime%></li>
            <li>
                <label style="width: 110px;">
                    开通时间：</label><%=syncTime%></li>
        </ul>
        <div class="spliter">
        </div>
        <%--暂时不要车商通信息 --%>
        <%if (1 == 2) %>
        <%{ %>
        <div class="khinfo">
            <h3 style="background-color: #e1e1e1; margin: 5px 0 5px 0; padding: 5px;">
                车商通产品开通情况</h3>
            <div>
                <iframe width="100%" height="260" frameborder="0" scrolling="no" src="http://ma.ucar.cn/webservice/dealerinfo.aspx?tvaid=<%=CstMemberID() %>&key=<%=productOpenKey %>">
                </iframe>
            </div>
        </div>
        <div class="khinfo" style="background-image: none">
            <h3 style="background-color: #e1e1e1; margin: 5px 0 5px 0; padding: 5px;">
                车商通账号</h3>
            <div id="cstAccountInfo_<%=CstMemberID() %>">
            </div>
        </div>
        <%} %>
        <%} %>
    </div>
</div>
