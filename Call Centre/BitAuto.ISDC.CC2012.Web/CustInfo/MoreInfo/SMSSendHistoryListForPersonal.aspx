<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSSendHistoryListForPersonal.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.SMSSendHistoryListForPersonal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        //分页操作
        function ShowDataByPost40(pody) {
            LoadingAnimation("bit_table4");
            $('.bit_table4').load('/CustInfo/MoreInfo/SMSSendHistoryListForPersonal.aspx .bit_table4 > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table4" id="bit_table4">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableList40">
            <tr>
                <th style="width: 10%;">
                    接收人
                </th>
                <th style="width: 10%;">
                    手机
                </th>
                <th style="width: 10%;">
                    任务ID
                </th>
                <th style="width: 10%;">
                    客户ID
                </th>
                <th style="width: 26%;">
                    发送内容
                </th>
                <th style="width: 15%;">
                    发送日期
                </th>
                <th style="width: 8%;">
                    是否成功
                </th>
                <th style="width: 8%;">
                    客服
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <a href="../../TaskManager/CustInformation.aspx?CustID=<%# Eval("CustID")%>" target="_blank">
                                <%# Eval("custname")%></a>&nbsp;
                        </td>
                        <td title="<%#Eval("Phone")%>">
                            <%# Eval("Phone")%>&nbsp;
                        </td>
                        <td>
                            <a href="../../OtherTask/OtherTaskDealView.aspx?OtherTaskID=<%# Eval("TaskID")%>"
                                target="_blank">
                                <%# Eval("TaskID")%></a>&nbsp;
                        </td>
                        <td>
                            <%# getCrmUrl(Eval("CRMCustID"),Eval("CustID"))%>
                        </td>
                        <td title="<%#Eval("Content")%>">
                            <%# Eval("Content").ToString().Length > 20 ? Eval("Content").ToString().Substring(0,20)+"...": Eval("Content").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("Status").ToString()=="0"?"成功":"失败"%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TrueName")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="color_hui">
                        <td>
                            <a href="../../TaskManager/CustInformation.aspx?CustID=<%# Eval("CustID")%>" target="_blank">
                                <%# Eval("custname")%></a>&nbsp;
                        </td>
                        <td>
                            <%# Eval("Phone")%>&nbsp;
                        </td>
                        <td>
                            <a href="../../OtherTask/OtherTaskDealView.aspx?OtherTaskID=<%# Eval("TaskID")%>"
                                target="_blank">
                                <%# Eval("TaskID")%></a>&nbsp;
                        </td>
                        <td>
                            <%# getCrmUrl(Eval("CRMCustID"),Eval("CustID"))%>
                        </td>
                        <td title="<%#Eval("Content")%>">
                            <%# Eval("Content").ToString().Length > 20? Eval("Content").ToString().Substring(0,20)+"...": Eval("Content").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("Status").ToString()=="0"?"成功":"失败"%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TrueName")%>&nbsp;
                        </td>
                    </tr>
                </AlternatingItemTemplate>
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
