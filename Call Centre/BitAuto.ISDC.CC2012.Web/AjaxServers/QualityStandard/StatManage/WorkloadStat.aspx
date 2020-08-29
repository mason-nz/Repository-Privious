<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkloadStat.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.StatManage.WorkloadStat" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th>
            评分人
        </th>
        <th>
            评分次数
        </th>
        <th>
            质检录音总时长
        </th>
        <th>
            被申诉总量
        </th>
        <th>
            被申诉成功量
        </th>
        <th>
            被申诉成功率
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                      
                        <td>
                            <%#Eval("TrueName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("sumCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&ScoreCreater=<%#Eval("CreateUserID") %>'><%#Eval("sumCount")%></a>--%>
                            &nbsp;
                        </td>
                        <td>
                            <%#Eval("TotalTallTime")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AppealCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&ScoreCreater=<%#Eval("CreateUserID") %>&ScoreStatus=0 '><%#Eval("AppealCount")%></a>--%>
                            &nbsp;
                        </td>
                        <td>
                             <%#Eval("AppealSuccessCount")%>&nbsp;
                        </td>
                        <td>
                        <%#Eval("AppealCount").ToString() == "0" ? "0.00%" : (float.Parse(Eval("AppealSuccessCount").ToString())*100 / (float.Parse(Eval("AppealCount").ToString()))).ToString("0.00") + "%" %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
</table>
<br />
<!--分页-->
<%--<div style=" float:left"><a href="javascript:CheckBoxControl(1)">全选</a><a href="javascript:CheckBoxControl(3)" style=" margin-left:10px;">反选</a><a href="javascript:CheckBoxControl(2)" style=" margin-left:10px;">取消选择</a></div>--%>
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
