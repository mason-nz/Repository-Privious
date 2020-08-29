<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCScoreTableEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCScoreTableEdit" %>
<script type="text/javascript">
</script>
<div class="pfb">
    <asp:Repeater ID="rp_Category" runat="server" OnItemDataBound="rp_Category_ItemDataBound">
        <ItemTemplate>
            <div class="lybase fwgf">
                <div class="title">
                    <%#GetNum(Container.ItemIndex + 1,"1")%><%#Eval("Name")%>（<%#Eval("Score")%>分）
                    <asp:Label ID="lblQS_CID" runat="server" Visible="false" Text='<%#Eval("QS_CID")%>'></asp:Label>
                </div>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_Item_ItemDataBound">
                    <ItemTemplate>
                        <p>
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%>（<%#Eval("Score")%>分）</p>
                        <asp:Label ID="lblQS_IID" runat="server" Visible="false" Text='<%#Eval("QS_IID")%>'></asp:Label>
                        <table border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server" OnItemDataBound="rp_Standard_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td width="36%" class="bdlnone zdq">
                                            <%#GetNum(Container.ItemIndex + 1,"3")%><%#Eval("ScoringStandardName")%>
                                            （<%#Eval("Score")%>分）
                                            <asp:Label ID="lblQS_SID" runat="server" Visible="false" Text='<%#Eval("QS_SID")%>'></asp:Label>
                                        </td>
                                        <td width="64%" class="qrb">
                                            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                                <asp:Repeater ID="rp_Marking" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="58%" class="zdq">
                                                                <%#Eval("MarkingItemName")%>
                                                                （<%#Eval("Score")%>分）
                                                            </td>
                                                            <td width="12%">
                                                                <label>
                                                                    <input name="" type="checkbox" value="" class="dx" /></label>
                                                            </td>
                                                            <td width="30%" class="borderR">
                                                                <input type="text" value="" class="wsr" />
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
            <%=GetNum("1")%>致命项</div>
        <table width="100%" cellspacing="0" cellpadding="0" border="1" style="margin-top: 10px;">
            <tbody>
                <asp:Repeater ID="rp_Dead" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="bdlnone zdq bd">
                                <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("DeadItemName")%>
                            </td>
                            <td width="8%">
                            </td>
                            <td width="4%">
                            </td>
                            <td width="20%">
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div class="lybase fwgf" style="<%=haveQulity%>">
        <div class="title">
            <%=GetNum("2")%>质检评价</div>
        <div class="pj">
            <textarea rows="" cols="" name=""></textarea>
        </div>
    </div>
</div>
