<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQCollection.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.FAQCollection" %>

    <div class="bit_table">
        <!--列表开始-->
        <div class="faqList">
            <asp:Repeater runat="server" ID="Rt_FAQ">
                <ItemTemplate>
                    <ul>
                        <li class="bt"><b>Q：</b> <span>
                            <%#Eval("Question") %></span> </li>
                        <li><b>A：</b> <span>
                            <%#Eval("Ask") %></span> </li>
                        <li class="lb" style="  position:relative;">
                            <span style=" display:inline;">
                                关联知识点：<a href="/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=<%#Eval("KLID") %>" target="_blank"><%#Eval("Title") %></a>
                            </span> 
                            <span style=" display:inline;">点击：<%#Eval("ClickCount")%>次 下载：<%#Eval("DownLoadCount")%>次</span>
                            <span onclick="AddQuestion(<%#Eval("KLID")%>,'1')" style=" position:absolute; display:block; top:0px; right:20px; vertical-align:middle; cursor:pointer;display:inline"><a >提问 </a><img src="../../Images/questionimg.png" /></span>
                            <span  onclick="CancelCollectionClick(<%#Eval("KLFavoritesId")%>,'1')"  style=" position:absolute; top:0px; right:80px; display:block;vertical-align:middle; cursor:pointer; display:inline"><a >取消收藏 </a><img src="../../Images/collectimg.png" /></span>  &nbsp;
        
                        </li>
                    </ul>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <!--列表结束-->
        <br />
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
         <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
    </div>
