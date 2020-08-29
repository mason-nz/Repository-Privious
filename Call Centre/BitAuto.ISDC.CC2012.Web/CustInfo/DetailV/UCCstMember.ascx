<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCstMember.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.UCCstMember" %>
<script type="text/javascript" language="javascript"> 
        <%if (SyncStatusValue == "170002")
              { %>
        $(document).ready(
            function () {
                loadCSTAccountInfo<%=CstMemberID() %>();
                search();
                //启用禁用权限
                if('<%=HasRight %>' =='0')
                {
                    $("#linkOpen").css("display","none");
                    $("#linkClose").css("display","none");
                }
            }
        ); 
        <%} %>
        function loadCSTAccountInfo<%=CstMemberID() %>() {
            var where = "";
            var cstMemberID = "<%=CstMemberID() %>";
            where += '&CSTMemberID=' + escape(cstMemberID);
            LoadingAnimation('cstAccountInfo');
            $("#cstAccountInfo<%=CstMemberID() %>").load("../AjaxServers/CSTMember/CSTAccountInfo.aspx?r=" + Math.random(), where, AccountSuccessLoaded);
        }
        function AccountSuccessLoaded() {
        }
        function search() {
            var where = '?Search=yes';
            var cstMemberID = "<%=CstMemberID() %>";
            var useType = $('#useType').val();
            var spendType = $('#spendType').val();

            var txtStartTime = $('#txtStartTime').val();
            var txtEndTime = $('#txtEndTime').val();
            where += '&CstMemberID=' + escape(cstMemberID);
            where += '&useType=' + escape(useType);
            where += '&spendType=' + escape(spendType);
            where += '&txtStartTime=' + escape(txtStartTime);
            where += '&txtEndTime=' + escape(txtEndTime);

            if ($.trim(txtStartTime).length > 0) {
                if (!($.trim(txtStartTime).isDate())) {
                    $.jAlert("开始时间格式不正确", function () {
                        $('#txtStartTime').val('');
                        $('#txtStartTime').focus();
                        return;
                    });
                }
            }
            if ($.trim(txtEndTime).length > 0) {
                if (!($.trim(txtEndTime).isDate())) {
                    $.jAlert("结束时间格式不正确", function () {
                        $('#txtEndTime').val('');
                        $('#txtEndTime').focus();
                        return;
                    });
                }
            }

            LoadingAnimation('divSeekResult');
            $("#divSeekResult").load("../AjaxServers/CSTMember/CSTMemberUCountList.aspx?pagesize=5&r=" + Math.random(), where, SuccessLoaded);
        }
        function SuccessLoaded() {
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
<div class="MemberInfoArea">
    <div class="title" style="clear:both">
        <%= this.ControlName %><a href="javascript:void(0)" onclick="divShowHideEvent('divMemberInfo_<%= this.ID %>',this)"
            class="toggle"></a></div>
    <div id="divMemberInfo_<%= this.ID %>">
        <ul class="clearfix">
            <li>
                <label>
                    会员全称：</label><span><%=clientFullName%><%if (SyncStatusValue == "170002")
                                                            { %>&nbsp;<%=GetStatus(Status)%><%} %></span></li>
            <li>
                <label>
                    开通状态：</label><span>
                        <%=SyncStatus%><%=GetRejectedStr(SyncStatusValue)%></span></li>
            <li>
                <label>
                    会员简称：</label><span><%=clientShortName%></span></li>
            <%if (SyncStatusValue == "170002")
              { %>
            <li>
                <label>
                    会员ID：</label><span><%=CSTMemberID%></span></li>
            <%} %>
            <li>
                <label>
                    会员类型：</label><span><%=clientType%></span></li>
            <li>
                <label>
                    上级公司：</label><span><%=upCompanyName%></span></li>
            <li>
                <label>
                    会员地区：</label><span><%=region%></span></li>
            <li>
                <label>
                    邮编：</label><span><%=postCode%></span></li>
            <li style="height: auto;">
                <label>
                    地址：</label><span><%=detailAddr%></span></li>
            <li>
                <label>
                    联系人姓名：</label><span><%=contact%></span></li>
            <li>
                <label>
                    移动电话：</label><span><%=phone%></span></li>
            <li>
                <label>
                    Email：</label><span><%=email%></span></li>
            <%if (SyncStatusValue == "170002")
              { %>
            <li style="height: 2px; margin-left: 10px; padding-top: 0px; width: 890px; background-image: url(/images/diandian_r.gif);
                background-repeat: repeat-x;"></li>
            <%} %>
            <li>
                <label>
                    总充值车商币：</label><%=UbTotalAmount%></li>
            <li>
                <label>
                    总消费车商币：</label><%=UbTotalExpend%></li>
            <li>
                <label>
                    最后充值时间：</label><%=lastAddUbTime%></li>
            <%--<li>
                <label style="width: 110px;">
                    有效截止日：</label><%=activeTime%></li>--%>
            <li>
                <label>
                    开通时间：</label><%=syncTime%></li>
        </ul>
    </div>
    <%--暂时不要车商通信息 --%>
    <%if (1 == 2) %>
    <%{ %>
    <div class="title">
        车商通产品开通情况<a href="javascript:void(0)" onclick="divShowHideEvent('cheshangtong',this)"
            class="toggle"></a></div>
    <div id="cheshangtong" class="fullRow  cont_cxjg" style="margin: 0 18px;">
        <iframe width="100%" height="260" frameborder="0" scrolling="no" src="http://ma.ucar.cn/webservice/dealerinfo.aspx?tvaid=<%=CstMemberID() %>&key=<%=productOpenKey %>">
        </iframe>
    </div>
    <div class="title" style="display: none">
        车商通账号<a href="javascript:void(0)" onclick="divShowHideEvent('cstAccountInfo<%=CstMemberID() %>',this)"
            class="toggle"></a></div>
    <div id="cstAccountInfo<%=CstMemberID() %>" class="fullRow  cont_cxjg" style="margin: 0 18px;">
    </div>
    <%} %>
</div>
