<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSTMemberInfo.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CSTMember.CSTMemberInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车商通会员查看页</title>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/cc_list.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.free.ajaxTabPanel.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <script type="text/javascript">
        document.domain = "<%=SysUrl %>";
    </script>
    <script type="text/javascript" language="javascript">

        <%if (SyncStatusValue == "170002")
              { %>
        $(document).ready(
            function () {
                loadCSTAccountInfo();
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
        function loadCSTAccountInfo() {
            var where = "";
            var cstMemberID = "<%=CstMemberID() %>";
            where += '&CSTMemberID=' + escape(cstMemberID);
            LoadingAnimation('cstAccountInfo');
            $("#cstAccountInfo").load("../AjaxServers/CSTMember/CSTAccountInfo.aspx?r=" + Math.random(), where, AccountSuccessLoaded);
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
    <script type="text/javascript">
        function showMoreMember(showtype) {

            if ($('#moreinfo')) {
                if (showtype) {
                    $('#moreinfo').show();
                }
                else {
                    $('#moreinfo').hide();
                }
            }

        }
    </script>
    <script type="text/javascript">
        function loadTabList(custId, n, menu) {
            switch (n) {
                case 1:
                    $("#con_two_1").load("/CustInfo/MoreInfo/ContactInfoList.aspx", { CustID: custId, ContentElementId: 'con_two_1' }, function () { mao(menu); });
                    break;
                case 2:
                    $("#con_two_2").load("/CustInfo/MoreInfo/CooperationProjectList.aspx", { CustID: custId, ContentElementId: 'con_two_2' }, function () { mao(menu); });
                    break;
                case 3:
                    $("#con_two_3").load("/CustInfo/MoreInfo/CustUserList.aspx", { CustID: custId, ContentElementId: 'con_two_3' }, function () { mao(menu); });
                    break;
                case 4:
                    $("#con_two_4").load("/CustInfo/MoreInfo/ReturnVisitList.aspx", { CustID: custId, ContentElementId: 'con_two_4' }, function () { mao(menu); });
                    break;
                case 5:
                    $("#con_two_5").load("/CustInfo/MoreInfo/BusinessLicenseList.aspx", { CustID: custId, ContentElementId: 'con_two_5' }, function () { mao(menu); });
                    break;
                case 6:
                    $("#con_two_6").load("/CustInfo/MoreInfo/SecondCarList.aspx", { CustID: custId, ContentElementId: 'con_two_6' }, function () { mao(menu); });
                    break;
                case 7:
                    $("#con_two_7").load("/CustInfo/MoreInfo/TaskRecord.aspx", { CustID: custId, ContentElementId: 'con_two_7' }, function () { mao(menu); });
                    break;
                case 8:
                    $("#con_two_8").load("/CustInfo/MoreInfo/CustBrandLicenseList.aspx", { CustID: custId, ContentElementId: 'con_two_8' }, function () { mao(menu); });
                case 9:
                    $("#con_two_9").load("/WorkOrder/WorkOrderRecordTag.aspx", { CustID: custId, ContentElementId: 'con_two_9' }, function () { mao(menu); });
                    break;
            }
        }

        function mao(menu) {
            var m = "";
            if (menu != "" && menu != undefined) {
                m = "#m";
                window.location.hash = m;
            }
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.s_ul').hide(); //初始ul隐藏（建议直接写style）
            $('.select_box span').hover(function () {
                $(this).parent().find('ul.s_ul').show();
                //li的hover效果
                $(this).parent().hover(function () {
                }, function () {
                    $(this).parent().find("ul.s_ul").hide();
                });
            }, function () {
            });
            $('ul.s_ul li').click(function () {
                $(this).parents('li').find('span').html($(this).html());
                $(this).parents('li').find('ul').hide();
            });
            loadTabList('<%= this.CustID %>', 1);
        });
    </script>
    <script type="text/javascript">
        /*第一种形式 第二种形式 更换显示样式*/
        function setTab(cursel) {
            $(".menuConbox2 [id^='con_two']").css("display", "none");
            $(".menuConbox2 .cont_cxjg").css("display", "none");
            $(".hd2 ul li[id^='two']").attr("class", "");
            $("#two" + cursel).attr("class", "hover");
            $("#con_two_" + cursel).css("display", "block").parent().css("display", "block");
            loadTabList('<%= this.CustID %>', cursel, 1);
        }

        $(function () {
            if ("<%=tabI %>" != "") {
                $(".hd ul li[id^='one']").attr("class", "");
                $("#one<%=tabI %>").attr("class", "hover");
            }
        });
        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
        function deleteMember() {
            $.jConfirm("您确认要删除会员吗？", function (data) {
                if (data) {
                    $.post("/CustInfo/Handler.ashx", { Action: 'deletecstmember', MemberID: '<%=CSTRecID %>' }, function (jd, textStatus, xhr) {
                        var jdData = $.evalJSON(jd);
                        if (jdData.success) {
                            $.jAlert("删除成功", function () {
                                window.location = $("#hrefCustInfoHref").attr("href");
                            });
                        }
                    });
                }
            });
        }
    </script>
    <style type="text/css">
        .baseInfo ul li
        {
            width: 365px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            车商通会员信息 <span></span>
        </div>
        <div class="baseInfo">
            <div class="hd">
                <ul>
                    <li id="one100" class=""><a id="hrefCustInfoHref" href="/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%=CustID %>"
                        style="text-decoration: none; color: #555;">基本信息</a></li>
                    <asp:Literal ID="lbCSTMember" runat="server"></asp:Literal>
                </ul>
            </div>
            <asp:Literal ID="literalMemberNameList" runat="server"></asp:Literal>
            <div class="menuConbox">
                <div class="title" style="margin: 0">
                    车商通会员信息<a href="javascript:void(0)" onclick="divShowHideEvent('con_one_101',this)"
                        class="toggle"></a>
                    <%if (IsCanEdit)
                      { %>
                    <a id="A1" href="javascript:void(0)" onclick="deleteMember()" style="float: right;
                        margin-right: 20px;">删除</a>
                    <%} %>
                    <%if (IsCanEdit)
                      { %>
                    <a id="hrefEditCst" href="http://<%=CrmUrl %>/CustomInfo/MemberMain.aspx?CustID=<%=CustID %>&SourceUrl=cc&Name=CstEdit&MemberID=<%=CSTRecID%>"
                        style="float: right; margin-right: 20px;" target="_blank">编辑</a>
                    <%} %>
                </div>
                <div id="con_one_100" class="">
                </div>
                <div id="con_one_101" class="">
                    <ul class="clearfix">
                        <li>
                            <label>
                                会员全称：</label><span><%=clientFullName%><%if (SyncStatusValue == "170002")
                                                                        { %>&nbsp;<%=GetStatus(Status)%><%} %></span></li>
                        <li>
                            <label>
                                开通状态：</label>
                            <span style="display: block">
                                <%=SyncStatus%><%=GetRejectedStr(SyncStatusValue)%></span></li>
                        <li>
                            <label>
                                会员简称：</label><span style="width: 185px;"><%=clientShortName%></span></li>
                        <%--<li>
                        <label style="width: 110px;">
                            会员编码：</label><span style="width: 185px;"><%=clientCode%></span></li>--%>
                        <%if (SyncStatusValue == "170002")
                          { %>
                        <li>
                            <label>
                                会员ID：</label><span style="width: 185px;"><%=CSTMemberID%></span></li>
                        <%} %>
                        <li>
                            <label>
                                会员类型：</label><span style="width: 185px;"><%=clientType%></span></li>
                        <li>
                            <label>
                                上级公司：</label><span style="width: 185px;"><%=upCompanyName%></span></li>
                        <li>
                            <label>
                                会员地区：</label><span style="width: 185px;"><%=region%></span></li>
                        <li>
                            <label>
                                邮编：</label><span style="width: 185px;"><%=postCode%></span></li>
                        <li style="height: auto;">
                            <label>
                                地址：</label><span style="width: 185px; word-wrap: break-word; word-break: normal;"><%=detailAddr%></span></li>
                        <li>
                            <label>
                                联系人姓名：</label><span style="width: 185px;"><%=contact%></span></li>
                        <li>
                            <label>
                                移动电话：</label><span style="width: 185px;"><%=phone%></span></li>
                        <li>
                            <label>
                                Email：</label><span style="width: 185px;"><%=email%></span></li>
                        <%if (SyncStatusValue == "170002")
                          { %>
                        <li style="height: 2px; margin-left: 10px; padding-top: 0px; width: 890px; background-image: url(/images/diandian_r.gif);
                            background-repeat: repeat-x;"></li>
                        <%} %>
                        <%if (SyncStatusValue == "170002")
                          { %>
                        <li>
                            <label>
                                总充值车商币：</label><%=UbTotalAmount%></li>
                        <li>
                            <label>
                                总消费车商币：</label><%=UbTotalExpend%></li>
                        <li>
                            <label>
                                最后充值时间：</label><%=lastAddUbTime%></li>
                        <li>
                            <label>
                                开通时间：</label><%=syncTime%></li>
                        <%} %>
                    </ul>
                </div>
            </div>
            <%--暂时不要车商通信息，1==2 --%>
            <%if (SyncStatusValue == "170002" && 1 == 2)
              { %>
            <div class="title contact">
                车商通产品开通情况<a class="toggle" onclick="divShowHideEvent('cheshangtong',this)" href="javascript:void(0)"></a>
            </div>
            <div id="cheshangtong" class="fullRow  cont_cxjg" style="margin: 0 18px;">
                <iframe width="100%" height="260" frameborder="0" scrolling="no" src="http://ma.ucar.cn/webservice/dealerinfo.aspx?tvaid=<%=CstMemberID() %>&key=<%=productOpenKey %>">
                </iframe>
            </div>
            <div class="title contact">
                车商通账号<a class="toggle" onclick="divShowHideEvent('cstAccountInfo',this)" href="javascript:void(0)"></a>
            </div>
            <div id="cstAccountInfo" style="margin: 0 18px;">
            </div>
            <%}
              else
              { %>
            <br />
            <br />
            <%} %>
            <a name="m"></a>
            <div class="hd2">
                <ul>
                    <%= this.ListOfContact == true ? "<li id='two1' onclick=\"setTab(1)\" class='hover'>客户联系人</li>" : ""%>
                    <%= this.ListOfCooperationProjects == true ? "<li id='two2' onclick=\"setTab(2)\" class=''>合作项</li>" : ""%>
                    <%= this.ListOfCustUser == true ? "<li id='two3' onclick=\"setTab(3)\" class=''>负责员工</li>" : ""%>
                    <%= this.ListOfReturnVisit == true ? "<li id='two4' onclick=\"setTab(4)\" class=''>访问记录</li>" : ""%>
                    <%= this.ListOfBusinessLicense == true ? "<li id='two5' onclick=\"setTab(5)\" class=''>年检记录</li>" : ""%>
                    <%= this.ListOfBusinessBrandLicense == true ? "<li id='two8' onclick='setTab(8)' class=''>品牌授权书</li>":""%>
                    <%= this.ListOfBusinessScaleInfo == true ? "<li id='two6' onclick=\"setTab(6)\" class=''>二手车规模</li>" : ""%>
                    <%= this.ListOfTaskRecord == true ? "<li id='two7' onclick=\"setTab(7)\" class=''>任务记录</li>" : ""%>
                    <%= this.ListOfWorkOrder == true ? "<li id='two9' onclick=\"setTab(9)\" class=''>工单记录</li>" : ""%>
                </ul>
                <%--<asp:Literal ID="literalMemberNameList" runat="server"></asp:Literal> --%>
            </div>
            <div class="menuConbox menuConbox2">
                <!--内容1-->
                <%= this.ListOfContact == true ? "<div class='cont_cxjg'><div id='con_two_1' class='hover'></div></div>" : ""%>
                <!--内容1-->
                <!--内容2-->
                <%= this.ListOfCooperationProjects == true ? "<div class='cont_cxjg'><div id='con_two_2'  style='display: none;'></div></div>" : ""%>
                <!--内容2-->
                <!--内容3-->
                <%= this.ListOfCustUser == true ? "<div class='cont_cxjg'><div id='con_two_3'  style='display: none;'></div></div>" : ""%>
                <!--内容3-->
                <!--内容4-->
                <%= this.ListOfReturnVisit == true ? "<div class='cont_cxjg'><div id='con_two_4'  style='display: none;'></div></div>" : ""%>
                <!--内容4-->
                <!--内容5-->
                <%= this.ListOfBusinessLicense == true ? "<div class='cont_cxjg'><div id='con_two_5'  style='display: none;'></div></div>" : ""%>
                <!--内容5-->
                <!--内容5-->
                <%= this.ListOfBusinessBrandLicense == true ? "<div class='cont_cxjg'> <div id='con_two_8' style='display: none;'> </div> </div>": "" %>
                <!--内容5-->
                <!--内容6-->
                <%= this.ListOfBusinessScaleInfo == true ? "<div class='cont_cxjg'><div id='con_two_6'  style='display: none;'></div></div>" : ""%>
                <!--内容6-->
                <!--内容7-->
                <%= this.ListOfTaskRecord == true ? "<div class='cont_cxjg'><div id='con_two_7'  style='display: none;'></div></div>" : ""%>
                <!--内容7-->
                <%= this.ListOfWorkOrder == true ? "<div class='cont_cxjg'><div id='con_two_9'  style='display: none;'></div></div>" : ""%>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
