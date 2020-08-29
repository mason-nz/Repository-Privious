<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B_ProjectReport.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport.B_ProjectReport" %>

<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th>
                客服
            </th>
            <th>
                工号
            </th>
            <th>
                项目名称
            </th>
            <th>
                任务分配量
            </th>
            <th>
                任务提交量
            </th>
            <th>
                任务接通量
            </th>
            <th>
                任务接通率
            </th>
            <th>
                成功量
            </th>
            <th>
                成功率
            </th>
            <th>
                接通后失败量
            </th>
            <th>
                未接通量
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("Truename")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("agentnum")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("projectname")%>&nbsp;                           
                        </td>                                               
                        <td>
                            <%#Eval("assigncount").ToString() == "" ? 0 : Eval("assigncount")%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("tjcount").ToString() == "" ? 0 : Eval("tjcount")%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("jtcount").ToString() == "" ? 0 : Eval("jtcount")%>&nbsp;
                        </td>
                        <td>
                            <%#producelv(Eval("jtcount").ToString(), Eval("tjcount").ToString())%>&nbsp;
                        </td>                                                
                        <td>
                            <%#Eval("successcount").ToString() == "" ? 0 : Eval("successcount")%>&nbsp;
                        </td>
                        <td>
                            <%#producelv(Eval("successcount").ToString(), Eval("jtcount").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("jtfailcount").ToString() == "" ? 0 : Eval("jtfailcount")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("wjtcount").ToString() == "" ? 0 : Eval("wjtcount")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
