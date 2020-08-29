<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCEditSurvey.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.UCEditSurvey" %>
<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript">
    function chkEmpty(optObj) {
        var obj = $(":radio[id^='ckeQOpt_" + optObj + "']");
        obj.removeAttr('checked');
        var objDescrip = $(":text[id^='qust_" + optObj + "']");
        objDescrip.val("");
    }
</script>
<div class="cont_cx khxx">
    <div class="SurveyInfoArea">
        <h3 ondblclick="javascript:$('#infoSurvey').toggle('fast');">
            <span id="spanSurvey">会员问卷调查</span>
        </h3>
        <div class="telcent_qa">
            <asp:PlaceHolder ID="PlaceHistorySurvey" runat="server"></asp:PlaceHolder>
            <ul id="infoSurvey">
                <asp:Repeater ID="repeaterQ" runat="server" OnItemDataBound="repeaterQ_ItemDataBound">
                    <ItemTemplate>
                        <li class='qustlist'>
                            <h4>
                                <%# Container.ItemIndex+1 %>、<%# Eval("Question")%><input type="hidden" id='hdn_<%#Eval("QustID") %>'
                                    value="<%#Eval("QustID") %>" name="hdn_<%#Eval("QustID") %>_<%# Container.ItemIndex+1 %>" />
                                <input type="hidden" id="hdnID_<%=OriginalDMSMemberID %>" value="<%=OriginalDMSMemberID %>" />
                                <input type="hidden" id="hdnCount_<%=AnswerCount %>" value="<%=AnswerCount %>" />
                            </h4>
                            <p>
                                <asp:Repeater ID="repeaterOpt" runat="server">
                                    <ItemTemplate>
                                        <label>
                                            <input type="radio" id="ckeQOpt_<%# Eval("QustID")%>_<%# Eval("QustOptID")%>" name="<%# Eval("QustID")%>_<%=MemberID%>"
                                                value="<%# Eval("QustOptID")%>" <%# IsCheck(Eval("QustID").ToString(),Eval("QustOptID").ToString()) %> />
                                            <lable for='ckeQOpt_<%# Eval("QustID")%>_<%# Eval("QustOptID")%>'><%# Eval("Option")%></lable>
                                        </label>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <label>
                                    <a href='javascript:chkEmpty("<%# Eval("QustID")%>")'>清空</a></label>
                            </p>
                            <p>
                                <label class="bz">
                                    备注：</label>
                                <input type="text" id="qust_<%#Eval("QustID") %>" value="<%# GetQustDescript(Eval("QustID").ToString()) %>"
                                    class="fullRow" />
                            </p>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</div>
