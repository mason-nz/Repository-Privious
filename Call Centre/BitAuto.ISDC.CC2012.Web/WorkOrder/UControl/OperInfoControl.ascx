<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperInfoControl.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.UControl.OperInfoControl" %>
<script type="text/javascript">
    $(function () {
        $("#uc_divOperInfoControl1").remove();
        //在回复信息中，如果有回复为空的信息需要删除
        if ("<%=ViewType %>" == "1") {
            $("[name='uc_divRevert1']").each(function () {
                var ucStrongContentVal = $.trim($(this).find("strong[name='uc_strongContent']").html());
                if (ucStrongContentVal == "") {
                    $(this).remove();
                }
            });
        }
        $("[name^='uc_divRevert']:eq(0)").css("border-top", "none");
        $("#uc_divOut1").css("display", "inherit");

    });
</script>
<div class="hfq clearfix" style="border-top: none; margin-top: 0;" id="uc_divOut<%=ViewType %>">
    <div class="hf1" style="border-top: none;" id="uc_divOperInfoControl<%=ViewType %>">
        <div class="user">
            <%=userName(modelRevert.CreateUserID.ToString())%>【<%=modelRevert.ReceiverDepartName %>】
            <span class="right">
                <%=modelRevert.CreateTime %>
            </span>
        </div>
        <div class="hfc">
            <%=modelRevert.RevertContent %><%=LinkUrl(modelRevert.AudioURL)%></div>
        <ul class="clearfix">
            <li>
                <label>
                    工单分类：</label><span><%=modelRevert.CategoryName%></span></li>
            <li>
                <label>
                    工单来源：</label><span><%=modelRevert.DataSource%></span></li>
            <li>
                <label>
                    工单状态：</label><span><%=modelRevert.WorkOrderStatus%></span></li>
            <li>
                <label>
                    优先级：</label><span><%=modelRevert.PriorityLevelName%></span></li>
            <li>
                <label>
                    客户名称：</label><span><%=modelRevert.CustName%></span></li>
            <li>
                <label>
                    客户地区：</label><span><%=modelRevert.ProvinceName%>&nbsp;<%=modelRevert.CityName%>&nbsp;<%=modelRevert.CountyName%></span></li>
            <li>
                <label>
                    联系人：</label><span><%=modelRevert.Contact%></span></li>
            <li>
                <label>
                    联系电话：</label><span><%=modelRevert.ContactTel%></span></li>
            <li id="liIsEstablish" visible="false" runat="server">
                <label>
                    是否接通：</label>
                <%=isEstablish%></li>
            <li id="liNotEstablishReason" visible="false" runat="server">
                <label>
                    未接通原因：</label>
                <%=notEstablishReason%></li>
            <li>
                <label>
                    最晚处理日期：</label><span><%=modelRevert.LastProcessDate%>
                    </span></li>
            <li runat="server" id="liDemand">
                <label>
                    关联需求：</label><span>
                        <asp:HyperLink ID="hlDemandDetails" runat="server"></asp:HyperLink>
                    </span></li>
            <li style="width: 800px;">
                <label>
                    标签：</label><span><%=modelRevert.TagName%></span></li>
        </ul>
    </div>
    <asp:Repeater ID="repeaterTableList" runat="server">
        <ItemTemplate>
            <div class="hf1 hf2" name="uc_divRevert<%=ViewType %>">
                <div class="user">
                    <%#Eval("TrueName")%>【<%#Eval("ReceiverDepartName")%>】<span class="right">
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                    </span></div>
                <div class="hfc">
                    <strong name="uc_strongContent" style="font-weight: normal; table-layout: fixed;
                        word-break: break-all; overflow: hidden;">
                        <%#Eval("RevertContent")%><%#LinkUrl(Eval("AudioURL").ToString())%></strong></div>
                <%#GetLi(Eval("CategoryName").ToString(), Eval("WorkOrderStatus").ToString(), Eval("ReceiverName").ToString(), Eval("PriorityLevelName").ToString(), Eval("TagName").ToString(), Eval("IsComplaintType").ToString())%>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
