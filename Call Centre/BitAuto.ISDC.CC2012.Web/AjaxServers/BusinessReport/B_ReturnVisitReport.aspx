<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="B_ReturnVisitReport.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport.B_ReturnVisitReport" %>

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
                当月负责会员数
            </th>
            <th>
                当月已回访会员数
            </th>
            <th>
                覆盖率
            </th>
            <th>
                回访记录数
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
                            <%#Eval("dyfzmembercount").ToString() == "" ? 0 : Eval("dyfzmembercount")%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("hfmembercount").ToString() == "" ? 0 : Eval("hfmembercount")%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("fglv").ToString() == "" ? "-" : Eval("fglv")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("hfcount").ToString() == "" ? 0 : Eval("hfcount")%>&nbsp;
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
