<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQuestionView.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCQuestionView" %>

<div class="title bold"><a name='question'></a>试题</div>
<asp:Repeater id="rptQuestion" OnItemDataBound="rptQuestion_ItemDataBind" runat="server">
<ItemTemplate>
<div class="st">
<ul>
<asp:Literal ID="LOption" runat="server"></asp:Literal>
</ul>
</div>
</ItemTemplate>
</asp:Repeater>