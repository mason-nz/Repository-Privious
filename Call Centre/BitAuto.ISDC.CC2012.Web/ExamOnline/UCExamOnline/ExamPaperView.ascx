<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.UCExamOnline.ExamPaperView" %>
<!--试题开始-->
<script type="text/javascript">
    function Getaskandanswer() {
        var radioselect = "";
        var checkselect = "";
        var ExamPaperID = "";
        ExamPaperID = '<%=ExamPaperID%>';

        $("input[type='radio']").each(function () {

            if ($(this).attr("checked")) {
                radioselect += $(this).attr("BQID") + ":" + $(this).attr("name") + ":" + $(this).attr("value") + ",";
            }
        });
        $("input[type='checkbox']").each(function () {

            if ($(this).attr("checked")) {
                checkselect += $(this).attr("BQID") + ":" + $(this).attr("name") + ":" + $(this).attr("value") + ",";
            }
        });
        if (radioselect != "") {
            radioselect = radioselect.substring(0, radioselect.length - 1);
            $("#radioselect").val(radioselect);
        }
        if (checkselect != "") {
            checkselect = checkselect.substring(0, checkselect.length - 1);
            $("#checkselect").val(checkselect);
        }
        $("#ExamPaperID").val(ExamPaperID);
    }

    $(function () {
        $("#ExamPaperID").val('<%=ExamPaperID%>');
    });
</script>
<input type="hidden" id="radioselect" />
<input type="hidden" id="checkselect" />
<input type="hidden" id="ExamPaperID" />
<asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
    <ItemTemplate>
        <div class="title bold examT">
            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"-")%><%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.Name.ToString()%>：<span><%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.BQDesc%></span><span>（共<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.QuestionCount.ToString()%>题，每题<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.EachQuestionScore.ToString()%>分，共<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.QuestionCount * ((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.EachQuestionScore%>分）</span></div>
        <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.AskCategory%>"></asp:Label>
        <asp:Label ID="lblBQID" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.BQID%>"></asp:Label>
        <asp:Repeater ID="repeaterRadio" OnItemDataBound="repeaterRadio_ItemDataBound" runat="server"
            Visible="false">
            <ItemTemplate>
                <div class="st">
                    <ul>
                        <li class="xzt">
                            <label>
                                <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%></span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeatersonradio" runat="server">
                                    <ItemTemplate>
                                        <li>
                                          <label style=" float: none; cursor: pointer; font-weight:normal; width:100%"><input name="<%#Eval("KLQID")%>" type="radio" bqid="<%#Eval("BQID")%>" value="<%#Eval("KLAOID")%>"
                                                <%#GetChecked(Eval("KLQID").ToString(), Eval("BQID").ToString(),Eval("KLAOID").ToString())%>
                                                class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></label> </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </li>
                    </ul>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="repeaterCheckbox" OnItemDataBound="repeaterCheckbox_ItemDataBound"
            runat="server" Visible="false">
            <ItemTemplate>
                <div class="st">
                    <ul>
                        <li class="xzt">
                            <label>
                                <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%></span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeatersonCheckbox" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <label style=" float: none; cursor: pointer; font-weight:normal; width:100%"><input name="<%#Eval("KLQID")%>" type="checkbox" bqid="<%#Eval("BQID")%>" <%#GetChecked(Eval("KLQID").ToString(), Eval("BQID").ToString(),Eval("KLAOID").ToString())%>
                                                value="<%#Eval("KLAOID")%>" class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></label></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </li>
                    </ul>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="repeaterask" Visible="false" runat="server">
            <ItemTemplate>
                <div class="faq">
                    <ul>
                        <li>
                            <label>
                                <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%></span>
                        </li>
                        <li class="answer2"><span>
                            <textarea name="<%#Eval("KLQID")%>" bqid="<%#Eval("BQID")%>" cols="" rows=""><%#Getanswer(Eval("KLQID").ToString(), Eval("BQID").ToString())%></textarea></span></li>
                    </ul>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>
