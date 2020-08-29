<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardFiveLevelView.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardFiveLevelView" %>

<script type="text/javascript">
    $(document).ready(function () {
        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        });
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
                        <table class="standardItemTable" border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server">
                                <ItemTemplate>
                                    <tr class="standardtr">
                                        <td  width="8%" class="bdlnone bdblue" name="firsttd" style=" text-align:right; font-weight:700; padding-right:11px;">
                                            <%#GetFiveLevelStandardName(Eval("SkillLevel").ToString())%>（<%#Eval("Score").ToString().Replace(".0","")+"分"%>）
                                            <input type="hidden" sidname='<%#Eval("QS_SID")%>' name="standardscore<%#Eval("SkillLevel").ToString()%>" value="<%#Eval("Score").ToString()%>" />
                                        </td>
                                        <td width="36%" style='text-align: left;'>
                                            <%#Eval("ScoringStandardName")%>
                                        </td>
                                        <td width="36%" style='text-align: left;'>
                                            <%#Eval("SExplanation")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr class="pingfentr">
                                <td width="12%" class="bdlnone bdblue" style="text-align:right; font-weight:700; padding-right:15px;">
                                    评分
                                </td>
                                <td width="34%" style='text-align: left;'>
                                    <label style="color:Red; font-weight:bold;"><%#GetStandardScoreNameBySID(Eval("QS_SID").ToString()) %></label>
                                </td> 
                                <td width="34%" style='text-align: left;'>  
                                    <label style="color:Red; font-weight:bold;" sidname='<%#Eval("QS_SID")%>' iidname='<%#Eval("QS_IID")%>' 
                                           cidname='<%#Eval("QS_CID")%>' cbtype='<%#Eval("ScoreType")%>'
                                           choosedstandardscore=""  name="choosedstandardmark"><%#GetStandardRemarkBySID(Eval("QS_SID").ToString()) %></label>
                                    <input type="text" name="choosedstandardscore"  value="" style=" border:0px ; width:30px; display:none;"/>
                                </td>
                            </tr>
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
                            <td style="width: 12%;">
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