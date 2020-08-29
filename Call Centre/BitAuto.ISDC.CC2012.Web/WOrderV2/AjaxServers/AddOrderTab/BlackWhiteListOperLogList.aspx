<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlackWhiteListOperLogList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.AddOrderTab.BlackWhiteListOperLogList" %>

<!--黑白名单日志 强斐 2016-8-10-->
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function ShowDataByPost1015(pody) {
            LoadingAnimation("bit_table5", "search_list_bt_loading");
            $('#bit_table5').load('/WOrderV2/AjaxServers/AddOrderTab/BlackWhiteListOperLogList.aspx #bit_table5 > *', pody, function () { });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="clearfix">
    </div>
    <div class="search_list_bt" style="position: relative;">
        <table border="1" cellspacing="0" cellpadding="0" class="bt_guding" style="position: absolute;">
            <tr class="back" onmouseout="this.className='back'">
                <th style="width: 25%;">
                    电话
                </th>
                <th style="width: 25%;">
                    操作人
                </th>
                <th style="width: 35%;">
                    操作时间
                </th>
                <th style="width: 15%;">
                    操作信息
                </th>
            </tr>
        </table>
    </div>
    <div class="search_list" id="bit_table5">
        <table cellpadding="0" cellspacing="0" width="99%" id="tableList51">
            <tr class="back" onmouseout="this.className='back'">
                <th style="width: 25%;">
                    电话
                </th>
                <th style="width: 25%;">
                    操作人
                </th>
                <th style="width: 35%;">
                    操作时间
                </th>
                <th style="width: 15%;">
                    操作信息
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%# Eval("PhoneNum")%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("TrueName")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("OperTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetOperTypesName(Eval("OperType").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <%--        <br />
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
        --%>
    </div>
    </form>
</body>
</html>
