<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSurveyInfoView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask.UCSurveyInfoView" %>
<div style="clear: both;" class="mbInfo">
    问卷调查-<%=SIIDName%><a href="javascript:void(0)" onclick="divShowHideEventForSurvey('divSurvey_<%= this.Num%>',this)"
        class="othertoggle"></a>
</div>
<div id="divSurvey_<%= this.Num%>" style="<%=GetClass%>">
    <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
        <ItemTemplate>
            <div class="st" id='<%#Eval("SQID")%>' index='<%#(Container.ItemIndex + 1).ToString()%>'
                style='border-bottom-width: 0px;'>
                <ul style="padding-left: 50px;">
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>'>
                        <%--<label>
                        <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%>&nbsp;&nbsp;&nbsp;</label><span><span style="float:left;"><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%></span><span class="sjbt" style="float:left; width:850px;"><%#Eval("Ask")%></span></span>--%>
                        <span><span style="float: left;">
                            <label>
                                &nbsp;&nbsp;&nbsp;&nbsp;<%#ConvertStrForNeed((Container.ItemIndex + 1).ToString())%></label>&nbsp;&nbsp;&nbsp;</span><span
                                    class="sjbt" style="float: left; width: 800px;"><%#Eval("Ask")%></span></span>
                        <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text='<%#Eval("AskCategory")%>'></asp:Label>
                        <asp:Label ID="lblSQID" runat="server" Visible="false" Text='<%#Eval("SQID")%>'></asp:Label>
                    </li>
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>' visible="false"
                        runat="server" id="ulRadio"><span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeaterRadio" runat="server">
                                    <ItemTemplate>
                                        <li <%# GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"class='chooseXx'":""%>>
                                            <input disabled="disabled" name="<%#Eval("SQID")%>" type="radio" <%#GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"checked='checked'":""%>
                                                value="<%#Eval("SOID")%>" class="dt" /><span><%#Eval("OptionName")%><input type="text"
                                                    value="<%#AnswerContent(Eval("SQID").ToString(),Eval("SOID").ToString())%>" style="border: none;
                                                    border-bottom: #CCC 1px solid; background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" /></span>
                                            <span><span>
                                                <%#GetIndex(GetJump(Eval("SOID").ToString()))%></span> </span></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <span></li>
                    <li class='<%#GetShowColumnStyle(Eval("ShowColumnNum").ToString())%>' visible="false"
                        runat="server" id="ulCheckBox"><span>
                            <ul class="clearfix w800">
                                <asp:Repeater ID="repeaterCheckBox" runat="server">
                                    <ItemTemplate>
                                        <li <%# GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"class='chooseXx'":""%>>
                                            <input disabled="disabled" name="<%#Eval("SQID")%>" type="checkbox" <%#GetChecked(Eval("SQID").ToString(),Eval("SOID").ToString(),"","")=="checked"?"checked='checked'":""%>
                                                value="<%#Eval("SOID")%>" class="dt" /><span><%#Eval("OptionName")%><input type="text"
                                                    value="<%#AnswerContent(Eval("SQID").ToString(),Eval("SOID").ToString())%>" style="border: none;
                                                    border-bottom: #CCC 1px solid; background: none; display: <%#IsBank(Eval("IsBlank").ToString())%>" /></span>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                            <span></li>
                    <li class='xzt3' visible="false" runat="server" id="ulText"><span>
                        <ul class="clearfix">
                            <li class="chooseXx">
                                <textarea disabled="disabled" name="<%#Eval("SQID")%>" style="width: 810px;"><%#GetChecked(Eval("SQID").ToString(),"","","")%></textarea></li>
                        </ul>
                    </span></li>
                    <li class="clearfix" style="width: 910px;" visible="false" runat="server" id="liMatrixRadio">
                        <%#GetTableHtmlForRadio(Eval("SQID").ToString())%>
                    </li>
                    <li class="clearfix" style="width: 910px;" visible="false" runat="server" id="liMatrixDropDown">
                        <%#GetHtmlForDropDown(Eval("SQID").ToString())%>
                    </li>
                </ul>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
