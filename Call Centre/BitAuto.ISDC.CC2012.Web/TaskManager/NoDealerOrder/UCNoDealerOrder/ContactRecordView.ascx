<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactRecordView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.ContactRecord" %>
<script language="javascript" type="text/javascript">
    $(function () {
        judgeSource();
    });
    //绑定联系记录
    function judgeSource() {
        if ('<%=Source %>' == "1" || '<%=Source %>' == "3") {
            $("#divNewCar").show();
        }
        else if ('<%=Source %>' == "2") {
            $("#divRelpaceCar").show();
        }
    }
</script>
<div id="divNewCar" style="display: none">
    <ul class="clearfix">
        <li>
            <label>
                咨询类型：</label><span>新车</span></li>
        <li>
            <label>
                记录类型：</label><span>呼出</span></li>
        <li>
            <label>
                车型：</label><span><%=CarType%></span></li>
        <li>
            <label>
                颜色：</label><span><%=CarColor%></span></li>
        <%if (DealerId != 0)
          {%>
        <li>
            <label>
                原始经销商：</label><a href="../../../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                    target="_blank"><%=DealerName%></a></li>
        <%}%>
        <li>
            <label>
                推荐经销商：</label><span><%=DMSMemberName%></span></li>
        <li>
            <label>
                补充说明：</label><span><%=OrderRemark%></span></li>
        <li>
            <label>
                创建时间：</label><span><%=CreateTime%></span></li>
    </ul>
    <div class="line">
    </div>
    <ul class="clearfix">
        <li>
            <label>
                来电记录：</label><span><%=CallRecord%></span></li>
    </ul>
    <div class="line">
    </div>
    <ul class="clearfix">
        <li>
            <label>
                历史地区：</label><span><%=PlaceStr%></span></li>
        <li>
            <label>
                历史车型：</label><span><%=OldCarType%></span></li>
    </ul>
</div>
<div id="divRelpaceCar" style="display: none">
    <ul class="clearfix">
        <li>
            <label>
                咨询类型：</label><span>置换</span></li>
        <li>
            <label>
                记录类型：</label><span>呼出</span></li>
        <li>
            <label>
                意向车型：</label><span><%=CarType%></span></li>
        <li>
            <label>
                意向车颜色：</label><span><%=CarColor%></span></li>
        <%if (DealerId != 0)
          {%>
        <li>
            <label>
                原始经销商：</label><a href="../../../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%=MemberID%>&CustID=<%=CustID%>"
                    target="_blank"><%=DealerName%></a></li>
        <%}%>
        <li>
            <label>
                推荐经销商：</label><span><%=DMSMemberName%></span></li>
        <li>
            <label>
                现有车型：</label><span><%=CarTypeNow%></span></li>
        <li>
            <label>
                现有车的颜色：</label><span><%=CarColorNow%></span></li>
        <li>
            <label>
                首次上牌日期：</label><span><%=CarBuyTime%></span></li>
        <li>
            <label>
                车牌所在地：</label><span><%=ReplacementCarLocationID%></span></li>
        <li>
            <label>
                已行驶里程：</label><span><%=ReplacementCarUsedMiles%>万公里</span></li>
        <li>
            <label>
                预售价格：</label><span><%=CarPrice%>万元</span></li>
        <li>
            <label>
                补充说明：</label><span><%=OrderRemark%></span></li>
        <li>
            <label>
                创建时间：</label><span><%=CreateTime%></span></li>
    </ul>
    <div class="line">
    </div>
    <ul class="clearfix">
        <li>
            <label>
                来电记录：</label><span><%=CallRecord%></span></li>
    </ul>
</div>
