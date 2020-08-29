<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SatisfactionListForBJ.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport.SatisfactionListForBJ" %>

<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="line-height: 18px;">
                日期
            </th>
            <th style="line-height: 18px;">
                客服
            </th>
            <th style="line-height: 18px;">
                电话总<br />
                接通量
            </th>
             <th style="line-height: 18px;">
                转IVR数
            </th>
            <th style="line-height: 18px;">
                转接比率
            </th>
            <th style="line-height: 18px;">
                满意度参<br />
                评总数
            </th>
            <th style="line-height: 18px;">
                满意度参<br />
                评率
            </th>
            <th style="line-height: 18px;">
                满意个数
            </th>
            <th style="line-height: 18px;">
                满意度
            </th>
            <th style="line-height: 18px;">
                不满意<br />
                个数
            </th>
            <th style="line-height: 18px;">
                对处理结果<br />
                不满意个数
            </th>
            <th style="line-height: 18px;">
                对服务不<br />
                满意个数
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                   <td><%#Eval("HuiZong").ToString().IndexOf('合') != -1 ? Eval("HuiZong").ToString() : (DateType == "1" ? Eval("mind").ToString().Split(' ')[0] : Eval("mind").ToString().Split(' ')[0] + "至" + Eval("maxd").ToString().Split(' ')[0])%></td>
<%--                      <td><%#Eval("mind").ToString()%></td>--%>
                        <td><%#Eval("TrueName")%></td>
                        <td><%#Eval("总接通量")%></td>
                        <td><%#Eval("转IVR数")%></td>
                        <td><%#GetRound(Eval("转接比率").ToString())%></td>
                        <td><%#Eval("满意度参评总数")%></td>
                        <td><%#GetRound(Eval("满意度参评率").ToString())%></td>
                        <td><%#Eval("满意个数")%></td>
                        <td><%#GetRound(Eval("满意度").ToString())%></td>
                        <td><%#Eval("不满意个数")%></td>
                        <td><%#Eval("对处理结果不满意个数").ToString()%></td>
                        <td><%#Eval("对服务不满意个数").ToString()%></td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
