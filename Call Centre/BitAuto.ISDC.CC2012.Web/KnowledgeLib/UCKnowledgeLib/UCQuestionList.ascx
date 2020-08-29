<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQuestionList.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCQuestionList" %>
<div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" id="tableList" width="99%">
            <tr class="bold">
                <th>
                    试题
                </th>
                <th>
                    题型
                </th>
                <th>
                    分类
                </th>
                <th>
                    创建人
                </th>
                <th>
                    创建日期
                </th>
                <th>
                    状态
                </th> 
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <div class="deal" title="<%#Eval("Ask")%>"><%#Eval("Ask")%></div>
                        </td>
                        <td>
                            <%#Eval("AskCategory")%>&nbsp;
                        </td> 
                        <td>
                            <%#getCategory(Eval("KCID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getCreateUserName(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateTime").ToString() == "1900-1-1 0:00:00" ? "" : Eval("CreateTime")%>&nbsp;
                        </td>
                        <td>
                            <%#getStatusName(Eval("libStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator()%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>