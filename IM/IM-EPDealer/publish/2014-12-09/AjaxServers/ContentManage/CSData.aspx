<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSData.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage.CSData" %>

<div class="bit_table">
    <!--列表开始-->
    <div class="faqList">
         <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th width="40%">
                        经销商名称
                    </th>
                    <th width="24%">
                        最后对话时间
                    </th>
                    <th width="12%">
                        对话时长
                    </th>
                    <th width="22%">
                        客服
                    </th>
                    <th style=" display:none;">
                    隐藏域
                    </th>
                </tr>
                <asp:Repeater runat="server" id="Rt_CSData">
                    <ItemTemplate>
                          <tr>
                            <td class="cName">
                                <%#Eval("MemberName")%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("LastClientTime").ToString().Contains("1900") ? "" : Convert.ToDateTime(Eval("LastClientTime").ToString()).ToString("yyyy-MM-dd HH:mm:ss")%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("duration")%>秒
                            </td>
                            <td>
                                <%#Eval("TrueName")%>&nbsp;
                            </td>
                            <td class="hidtdinfo" style=" display:none;">
                            <%#Eval("CSID")%>,<%#Eval("MemberCode")%>,<%#Eval("VisitID")%>,<%#Eval("OrderID")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                 </asp:Repeater>
                 <tr>
                    <td colspan="5">
                        <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                            <p>
                                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                            </p>
                        </div>
                    </td>
                </tr>
         </table>
    </div>

    <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
</div>
