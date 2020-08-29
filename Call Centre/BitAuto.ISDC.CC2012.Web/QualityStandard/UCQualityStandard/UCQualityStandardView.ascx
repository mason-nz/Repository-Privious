<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardView" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        })
    });
    function divShowHideEvent(divId, obj) {
        if ($(obj).hasClass("hide")) {
            //隐藏的
            $(obj).parent().siblings().show();
            $(obj).attr("class", "toggle2");
        }
        else {
            $(obj).parent().siblings().hide();
            $(obj).attr("class", "toggle2 hide");
        }
    } 
</script>
<div class="pfb">
    <asp:Repeater ID="rp_Category" runat="server" OnItemDataBound="rp_Category_ItemDataBound">
        <ItemTemplate>
            <div class="lybase fwgf">
                <div class="title">
                    <%#GetNum(Container.ItemIndex + 1,"1")%><%#Eval("Name")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                    <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
                        style="*margin-top: -25px;"></a>
                    <asp:Label ID="lblQS_CID" runat="server" Visible="false" Text='<%#Eval("QS_CID")%>'></asp:Label>
                </div>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_Item_ItemDataBound">
                    <ItemTemplate>
                        <p>
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span></p>
                        <asp:Label ID="lblQS_IID" runat="server" Visible="false" Text='<%#Eval("QS_IID")%>'></asp:Label>
                        <table border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server" OnItemDataBound="rp_Standard_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td width="36%" class="bdlnone zdq" name="firsttd">
                                            <%#GetNum(Container.ItemIndex + 1,"3")%><%#Eval("ScoringStandardName")%>
                                            <span>（<%#Eval("ScoreType").ToString()=="1"?Eval("Score")+"分":(Eval("IsIsDead").ToString()=="1"?"致命":"非致命")%>）</span>
                                            <asp:Label ID="lblQS_SID" runat="server" Visible="false" Text='<%#Eval("QS_SID")%>'></asp:Label>
                                        </td>
                                        <td width="64%" class="qrb">
                                            <table style="border: 0; cellspacing: 0;cellpadding:0; width: 100%">
                                                <asp:Repeater ID="rp_Marking" runat="server" OnItemDataBound="rp_Marking_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="width: 58%" class="zdq">
                                                                <%#Eval("MarkingItemName")%>
                                                                <span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                                                                <asp:Label ID="lblMarkID" Visible="false" Text='<%#Eval("QS_MID")%>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblStandarID" Visible="false" Text='<%#Eval("QS_SID")%>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblScoreType" runat="server" Visible="false" Text='<%#Eval("ScoreType")%>'></asp:Label>
                                                            </td>
                                                            <td class="<%#IsExistResultDetail(Eval("QS_MID").ToString(),"1")%>" style="width: 12%">
                                                                <label>
                                                                    <asp:Label ID="lblMarkingScore" Visible="false" Text='<%#Eval("Score")%>' runat="server"></asp:Label>
                                                                </label>
                                                            </td>
                                                            <td class="<%#IsExistResultDetail(Eval("QS_MID").ToString(),"1")%>" style="width: 12%;<%=StatsSuccess%>">
                                                                <label>
                                                                    <asp:Label ID="lblMarkingScore_End" Visible="false" Text='<%#Eval("Score")%>' runat="server"></asp:Label>
                                                                </label>
                                                            </td>
                                                            <td class="<%#IsExistResultDetail(Eval("QS_MID").ToString(),"1")%>" style="word-wrap: break-word; word-break: break-all; width: 18%;border-right:0px;">
                                                                <asp:Label ID="lblMarkingRemark" Visible="false" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="lybase fwgf" style="<%=haveDead%>">
        <div class="title">
            <%=GetNum("1")%>致命项 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)"
                href="javascript:void(0)" style="*margin-top: -25px;"></a>
        </div>
        <table width="100%" cellspacing="0" cellpadding="0" border="1" style="margin-top: 10px;">
            <tbody>
                <asp:Repeater ID="rp_Dead" runat="server" OnItemDataBound="rp_Dead_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="bdlnone zdq bd" style="width: 58%">
                                <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("DeadItemName")%>
                                <asp:Label ID="lblDeadID" Visible="false" Text='<%#Eval("QS_DAID")%>' runat="server"></asp:Label>
                            </td>
                            <td style="width: 12%">
                                <asp:Label ID="lblDeadInfo" Visible="false" Text="致命" runat="server"></asp:Label>
                            </td>
                            <td style="width: 12%; <%=StatsSuccess%>">
                                <asp:Label ID="lblDeadInfo_End" Visible="false" Text="" runat="server"></asp:Label>
                            </td>
                            <td style="width: 18%; word-wrap: break-word; word-break: break-all;">
                                <asp:Label ID="lblDeadRemark" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div class="lybase fwgf" style="<%=haveQulity%>">
        <div class="title">
            <%=GetNum("2")%>质检评价 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)"
                href="javascript:void(0)" style="*margin-top: -25px;"></a>
        </div>
        <div class="pj" style="word-wrap: break-word; word-break: break-all;">
            <asp:Label ID="txtQualityInfo" runat="server"></asp:Label>
        </div>
    </div>
</div>
