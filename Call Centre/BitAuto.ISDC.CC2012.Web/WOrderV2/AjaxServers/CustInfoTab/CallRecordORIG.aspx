<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallRecordORIG.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab.CallRecordORIG" %>

<!--话务记录 强斐 2016-8-9-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost1012(pody) {
            LoadingAnimation("bit_table2");
            $('#bit_table2').load('/WOrderV2/AjaxServers/CustInfoTab/CallRecordORIG.aspx #bit_table2 > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="search_list2" id="bit_table2">
        <table cellpadding="0" cellspacing="0" id="tableList2" width="99%">
            <tr class="bold">
                <th>
                    操作人
                </th>
                <th>
                    工号
                </th>
                <th>
                    记录类型
                </th>
                <th>
                    通话时间
                </th>
                <th>
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
                            <%#Eval("CallStatusName")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CallStartTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetOperator(Eval("AudioURL").ToString(), Eval("BusinessID").ToString(), Eval("BGID").ToString(), Eval("SCID").ToString())%>&nbsp;
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
