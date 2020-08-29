<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCScoreTableView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCScoreTableView" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        });
        initPingfen();
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

    function initPingfen() {
        // standardItemTable  standardtr  pingfentr
        $(".standardItemTable").each(function () {
            var pingfentr = $(this).find(" .pingfentr");
            $(pingfentr).find(" li").each(function () {
                $(this).hide();
            });
            $(this).find(" .standardtr").each(function () {
                switch ($.trim($(this).find(" td:eq(0)").text())) {
                    case "优秀":
                        $(pingfentr).find(" li").each(function () {
                            if ($(this).html().indexOf("优秀") > 0) {
                                $(this).show();
                            }
                        });
                        break;
                    case "良好":
                        $(pingfentr).find(" li").each(function () {
                            if ($(this).html().indexOf("良好") > 0) {
                                $(this).show();
                            }
                        });
                        break;
                    case "合格":
                        $(pingfentr).find(" li").each(function () {
                            if ($(this).html().indexOf("合格") > 0) {
                                $(this).show();
                            }
                        });
                        break;
                    case "较差":
                        $(pingfentr).find(" li").each(function () {
                            if ($(this).html().indexOf("较差") > 0) {
                                $(this).show();
                            }
                        });
                        break;
                    case "很差":
                        $(pingfentr).find(" li").each(function () {
                            if ($(this).html().indexOf("很差") > 0) {
                                $(this).show();
                            }
                        });
                        break;
                    default: break;
                }
            });
        });
    }
</script>
<div class="pfb">
    <asp:Repeater ID="rp_Category" runat="server" OnItemDataBound="rp_Category_ItemDataBound">
        <ItemTemplate>
            <div class="lybase fwgf">
                <div class="title">
                    <%#GetNum(Container.ItemIndex + 1,"1")%><%#Eval("Name")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                    <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)" style="*margin-top: -25px;"></a>
                    <asp:Label ID="lblQS_CID" runat="server" Visible="false" Text='<%#Eval("QS_CID")%>'></asp:Label>
                </div>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_Item_ItemDataBound">
                    <ItemTemplate>
                        <p>
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span></p>
                        <asp:Label ID="lblQS_IID" runat="server" Visible="false" Text='<%#Eval("QS_IID")%>'></asp:Label>
                        <table class="standardItemTable" border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server" OnItemDataBound="rp_Standard_ItemDataBound" >
                                <ItemTemplate>  
                                    <tr class="standardtr">
                                        <%if (ScoreType == "3")
                                          { %>
                                        <td width="8%" class="bdlnone bdblue">
                                            <%#GetFiveLevelStandardName(Eval("SkillLevel").ToString())%>
                                        </td>
                                        <td width="36%" style='text-align: left;'>
                                            <%#Eval("ScoringStandardName")%>
                                        </td>
                                        <td width="36%" style='text-align: left;'>
                                            <%#Eval("SExplanation")%>
                                        </td>
                                        <%}
                                          else
                                          { %>
                                        <td width="36%" class="bdlnone zdq" name="firsttd">
                                            <%#GetNum(Container.ItemIndex + 1,"3")%><%#Eval("ScoringStandardName")%>
                                            <span>（<%#Eval("ScoreType").ToString()!="2"?Eval("Score")+"分":(Eval("IsIsDead").ToString()=="1"?"致命":"非致命")%>）</span>
                                            <asp:Label ID="lblQS_SID" runat="server" Visible="false" Text='<%#Eval("QS_SID")%>'></asp:Label>
                                        </td>
                                        <td width="64%" class="qrb">
                                            <table border="0" cellspacing="0" cellpadding="0" width='100% auto'>
                                                <asp:Repeater ID="rp_Marking" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="58%" class="zdq">
                                                                <%#Eval("MarkingItemName")%>
                                                                <span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                                                            </td>
                                                            <td width="12%">
                                                                <label>
                                                                    <input id="cbMarking" name="" type="checkbox" class="dx" runat="server" /></label>
                                                            </td>
                                                            <td width="30%" class="borderR">
                                                                <input type="text" id="MarkingRemark" value="" class="wsr" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                            <%} %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%if (ScoreType == "3")
                                          { %><tr class="pingfentr">
                                              <td width="8%" class="bdlnone bdblue">
                                                  评分
                                              </td>
                                              <td width="36%" style='text-align: left;'>
                                                  <ul class="pfbz_choose">
                                                      <li><a href="javascript:void(0)">优秀</a></li>
                                                      <li><a href="javascript:void(0)">良好</a></li>
                                                      <li><a href="javascript:void(0)">合格</a></li>
                                                      <li><a href="javascript:void(0)">较差</a></li>
                                                      <li><a href="javascript:void(0)">很差</a></li>
                                                  </ul>
                                              </td>
                                              <td width="36%" style='text-align: left;'>
                                                  <input type="text" value="" class="w400" />
                                              </td>
                                          </tr>
                            <%}%>
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
                                    <asp:Repeater ID="rp_Dead" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="bdlnone zdq bd itemTitle">
                                                    <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("DeadItemName")%>
                                                </td>
                                                <td style="width: 8%">
                                                    <label>
                                                        <input id="cbDead" name="" type="checkbox" class="dx" runat="server" /></label>
                                                </td>
                                                <td style="width: 20%">
                                                    <input type="text" id="DeadRemark" value="" class="wsr" runat="server" />
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
                                <div class="pj">
                                    <textarea rows="" cols="" name=""></textarea>
                                </div>
                            </div>
</div> 