<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="AppealStat.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.StatManage.AppealStat" %>

 <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
            <tr class="back" onmouseout="this.className='back'">
                <%--<th style=" width:20%">
                    所属分组
                </th>--%>
                <th style=" width:20%">
                    坐席
                </th>
                <th style=" width:20%">
                    申诉总量
                </th>
                <th style=" width:20%">
                    申诉成功量
                </th>
                <th style=" width:20%">
                    申诉成功率
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <%--<td>--%>
                            <%--<%#GetGroupName(Eval("CreateUserID").ToString()) %>--%>
                            <%--<%#Eval("BGName")%>&nbsp;--%>
                        <%--</td>--%>
                        <td>
                             <%#Eval("TrueName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("sumCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&RecordBeginTime=<%=RequestRecordBeginTime %>&RecordEndTime=<%=RequestRecordEndTime %>&UserID=<%#Eval("CreateUserID") %>&Agent=<%#HttpUtility.UrlEncode(Eval("TrueName").ToString()) %>&ScoreStatus=0'><%#Eval("sumCount")%></a>--%>
                            &nbsp;
                        </td>
                        <td>
                            <%#Eval("AppealCount")%>
                            <%--<a href='/QualityStandard/List.aspx?ScoreBeginTime=<%=RequestBeginTime %>&ScoreEndTime=<%=RequestEndTime %>&RecordBeginTime=<%=RequestRecordBeginTime %>&RecordEndTime=<%=RequestRecordEndTime %>&UserID=<%#Eval("CreateUserID") %>&Agent=<%#HttpUtility.UrlEncode(Eval("TrueName").ToString()) %>&ScoreStatus=0&StateResult=1 '></a>--%>
                            &nbsp;
                        </td>
                        <td>
                            <%#Eval("sumCount").ToString() == "0" ? "0" : (float.Parse(Eval("AppealCount").ToString())*100 / (float.Parse(Eval("sumCount").ToString()))).ToString("0.00") + "%" %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <!--分页-->
        <%--<div style=" float:left"><a href="javascript:CheckBoxControl(1)">全选</a><a href="javascript:CheckBoxControl(3)" style=" margin-left:10px;">反选</a><a href="javascript:CheckBoxControl(2)" style=" margin-left:10px;">取消选择</a></div>--%>
        <div class="pages1" style="text-align: right;">
                <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" />
        </div>