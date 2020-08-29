<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFAQView.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCFAQView" %>
<!--FAQ开始-->
<%if(IsStart()){ %>
<div class="title bold">
    <a name='faq' ></a>FAQ</div>
<div class="faq">
    <asp:Repeater runat="server" ID="Rt_FAQList">
    <ItemTemplate>
    <ul>
        <li style=" margin-left:70px;">
            <label style="vertical-align: top; width:20px; display:block; float:left;">Q：</label>
            <span style=" display:block; float:left; width:860px;"><%#Eval("Question")%></span> 
            <div style=" clear:both;"></div>
        </li>
        <li style=" margin-left:70px;">
            <label style="vertical-align: top; width:20px; display:block; float:left;">A：</label>
            <span style=" display:block; float:left; width:860px;"><%#Eval("Ask")%></span>
            <div style=" clear:both;"></div>
        </li>
    </ul>
    </ItemTemplate>
    </asp:Repeater>    
</div>
<%} %>
<!--FAQ结束-->