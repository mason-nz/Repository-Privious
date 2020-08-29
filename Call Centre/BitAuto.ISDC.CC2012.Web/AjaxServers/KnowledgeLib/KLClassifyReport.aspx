<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KLClassifyReport.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.KLClassifyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" id="tableList" width="99%">
            <tr class="bold">
                <th>
                    分类
                </th>
                <th>
                    知识点
                </th>
                <th>
                    FAQ
                </th>
                <th>
                    试题
                </th>
                <th>
                    录音
                </th>
                <th>
                    点击量
                </th>
                <th>
                    收藏量
                </th>
                <th>
                    下载量
                </th>
            </tr>
             <%--"pid=" + pid + "&mBeginTime=" + escape($('#mfBeginTime').val()) + "&mEndTime=" + escape($('#mfEndTime').val()) + "&r=" + Math.random();--%>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#ShowUrl(Eval("fid").ToString(), Eval("fName").ToString(), Eval("level").ToString())%>                           
                        </td>
                        <td>
                            <%#Eval("知识点") %>
                        </td>
                        <td>
                            <%#Eval("FAQ") %>
                        </td>
                        <td>
                            <%#Eval("试题") %>
                        </td>
                        <td>
                            <%#Eval("录音") %>
                        </td>
                        <td>
                            <%#Eval("点击量") %>
                        </td>
                        <td>
                            <%#Eval("收藏量") %>
                        </td>
                        <td>
                            <%#Eval("下载量") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--分页-->
        <div class="pageTurn mr10" style="margin-top: 10px;">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
