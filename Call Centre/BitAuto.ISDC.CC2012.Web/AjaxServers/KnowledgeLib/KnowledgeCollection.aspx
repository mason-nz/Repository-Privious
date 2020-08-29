<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeCollection.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.KnowledgeCollection" %>

    <div class="bit_table" id="divList">
        <asp:Repeater ID="repeaterTableList" runat="server">
            <ItemTemplate>
                <div class="zskList">
                    <div class="bt">
                        <b id="bBold<%#Eval("KLID") %>">
                            <a onclick="aHrefTitleClick(<%#Eval("KLID") %>)" style="cursor: pointer;" title="<%#Eval("Title") %>" target='_blank' id="aHrefTitle">
                            <%# getTitle(Eval("Title").ToString()) %></a>
                        </b>
                        <em>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>
                        </em>
                        <span onclick="CancelCollectionClick(<%#Eval("KLFavoritesId") %>,'2')" style=" vertical-align:middle;display:block; float:right;cursor:pointer;">
                                <a >取消收藏</a><img src="../../Images/collectimg.png" />  
                        </span>
                        <span class="right">点击：<%#Eval("NewClickCount")%>次 下载：<%#Eval("NewDownLoadCount")%>次</span> 
                       
                        <span class="right">所属分类：<%# getCategory(Eval("KCID").ToString())%></span>
                        
                    </div>
                    <p>
                        <%# getContent(Eval("Abstract") == null ? "" : Eval("Abstract").ToString())%>
                        <a onclick="aHrefTitleClick(<%#Eval("KLID") %>)" target="_blank" style="cursor: pointer;">
                            查阅全文
                        </a>
                    </p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
         <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
    </div>

