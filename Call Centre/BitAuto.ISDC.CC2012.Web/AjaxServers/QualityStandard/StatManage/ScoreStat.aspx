<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreStat.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.StatManage.ScoreStat" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 15%">
            坐席
        </th>
        <th style="width: 25%">
            评分表
        </th>
        <th style="width: 15%">
            质检次数
        </th>
        <th style="width: 15%">
            合格量
        </th>
        <th style="width: 15%">
            合格率
        </th>
         <th style="width: 15%">
            平均分
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                             <%#Eval("TrueName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Name")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("sumCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&RecordBeginTime=<%=RequestRecordBeginTime %>&RecordEndTime=<%=RequestRecordEndTime %>&UserID=<%#Eval("CreateUserId") %>&Agent=<%#HttpUtility.UrlEncode(Eval("TrueName").ToString()) %>&ScoreTable=<%#Eval("QS_RTID") %>&ScoreStatus=1'><%#Eval("sumCount")%></a> --%>
                           &nbsp;
                        </td>
                        <td>
                       
                             <%#Eval("ScoreType").ToString() != "2" ? "--" :Eval("statCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&RecordBeginTime=<%=RequestRecordBeginTime %>&RecordEndTime=<%=RequestRecordEndTime %>&UserID=<%#Eval("CreateUserId") %>&Agent=<%#HttpUtility.UrlEncode(Eval("TrueName").ToString()) %>&ScoreTable=<%#Eval("QS_RTID") %>&ScoreStatus=1&IsQualified=1'><%#Eval("statCount")%></a> --%>
                           &nbsp;
                        </td>                       
                        <td>
                        <%#Eval("ScoreType").ToString() != "2" ? "--" :
                          (Eval("statCount") == null || string.IsNullOrEmpty(Eval("statCount").ToString())
                          || Eval("sumCount") == null || string.IsNullOrEmpty(Eval("sumCount").ToString()) || Eval("sumCount").ToString() == "0" ? "0.00" :
                        (float.Parse(Eval("statCount").ToString())*100 / float.Parse(Eval("sumCount").ToString())).ToString("0.00"))+"%"%>
                        </td>
                         <td>
                        <%#Eval("ScoreType").ToString() == "2" ? "--" :
                     (Eval("totScore") == null || string.IsNullOrEmpty(Eval("totScore").ToString())
                          || Eval("totScore") == null || string.IsNullOrEmpty(Eval("sumCount").ToString())                          
                          || Eval("sumCount").ToString() == "0" ? "0.00" :                                                                                  (float.Parse(Eval("totScore").ToString()) / float.Parse(Eval("sumCount").ToString())).ToString("0.00"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
</table>
<br />
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
</div>
