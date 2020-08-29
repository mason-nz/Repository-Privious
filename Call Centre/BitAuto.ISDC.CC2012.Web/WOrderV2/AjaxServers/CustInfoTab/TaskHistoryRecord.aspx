<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskHistoryRecord.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab.TaskHistoryRecord" %>

<!--业务记录 强斐 2016-8-9-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1011(pody) {
            LoadingAnimation("bit_table1");
            $('#bit_table1').load('/WOrderV2/AjaxServers/CustInfoTab/TaskHistoryRecord.aspx #bit_table1 > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="search_list2" id="bit_table1">
        <table cellpadding="0" cellspacing="0" id="tableList" width="99%">
            <tr class="bold">
                <th style="width: 8%">
                    所属坐席
                </th>
                <th style="width: 8%">
                    工号
                </th>
                <th style="width: 8%">
                    记录类型
                </th>
                <th style="width: 13%">
                    最后操作时间
                </th>
                <th style="width: 13%">
                    最后通话时间
                </th>
                <th style="width: 10%">
                    分类
                </th>
                <th style="width: 24%;">
                    联系记录
                </th>
                <th style="width: 8%;">
                    状态
                </th>
                <th style="width: 8%;">
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("TrueName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TaskSource")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOperTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetCategoryFullName(Eval("BusinessType").ToString(), Eval("BGID").ToString(), Eval("SCID").ToString())%>
                        </td>
                        <td style="word-break: break-all; text-align: left;">
                            <%#Eval("Content")%>&nbsp;
                        </td>
                        <td>
                            <%#GetStatusText(BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(Eval("BusinessType")), Eval("TaskStatus").ToString(), Eval("StopStatus").ToString(), Eval("ApplyType").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetOperator( Eval("AudioURL").ToString(), Eval("TaskID").ToString(), Eval("BusinessType").ToString(), Eval("BGID").ToString(), Eval("SCID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <!--分页-->
        <div class="pageTurn mr10" style="margin-right: 20px;">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
