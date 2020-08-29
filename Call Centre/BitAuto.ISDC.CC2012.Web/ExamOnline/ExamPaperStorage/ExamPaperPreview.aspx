<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperPreview.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage.ExamPaperPreview" %>

<%@ Register Src="~/ExamOnline/UCExamOnline/ExamPaperView.ascx" TagName="ExamPaperView"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>试卷预览</title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="../../Js/common.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            试卷预览
        </div>
        <div class="examBt1">
            <b>
                <%=ExamperName%></b></div>
        <div class="addzs">
            <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
                <ItemTemplate>
                    <div class="title bold examT">
                        <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"-")%><%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.ExamBigQuestionPageinfo)(Container.DataItem)).bigqpageinfo.Name.ToString()%>：<span><%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.ExamBigQuestionPageinfo)(Container.DataItem)).bigqpageinfo.BQDesc%></span></div>
                    <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.ExamBigQuestionPageinfo)(Container.DataItem)).bigqpageinfo.AskCategory%>"></asp:Label>
                    <asp:Label ID="lblBQID" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.ExamBigQuestionPageinfo)(Container.DataItem)).bigqpageinfo.BQID%>"></asp:Label>
                    <asp:Repeater ID="repeaterRadio" OnItemDataBound="repeaterRadio_ItemDataBound" runat="server"
                        Visible="false">
                        <ItemTemplate>
                            <div class="st">
                                <ul>
                                    <li class="xzt">
                                        <label>
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#GetSmallquestionName(((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.QuestinShipPageInfo)(Container.DataItem)).KLQID.ToString())%></span>
                                        <asp:Label ID="lblKLQID" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.QuestinShipPageInfo)(Container.DataItem)).KLQID%>"></asp:Label>
                                        <ul class="clearfix w800">
                                            <asp:Repeater ID="repeatersonradio" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <input name="<%#Eval("KLQID")%>" type="radio" value="<%#Eval("KLAOID")%>" class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></li>
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
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#GetSmallquestionName(((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.QuestinShipPageInfo)(Container.DataItem)).KLQID.ToString())%></span>
                                        <asp:Label ID="lblKLQID" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.QuestinShipPageInfo)(Container.DataItem)).KLQID%>"></asp:Label>
                                        <ul class="clearfix w800">
                                            <asp:Repeater ID="repeatersonCheckbox" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <input name="<%#Eval("KLQID")%>" type="checkbox" value="<%#Eval("KLAOID")%>" class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></li>
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
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#GetSmallquestionName(((BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.QuestinShipPageInfo)(Container.DataItem)).KLQID.ToString())%></span>
                                    </li>
                                    <li class="answer2"><span>
                                        <textarea name="" cols="" rows=""></textarea></span></li>
                                </ul>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="关 闭" onclick="javascript:closePage();">
        </div>
    </div>
</body>
</html>
