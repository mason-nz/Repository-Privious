<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustAudit.ExportList" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>导出Excel</title>
    <meta http-equiv="content-type" content="application/ms-excel; charset=UTF-8" />
    <style type="text/css">
        .cont_cxjg table.cxjg
        {
            border-right: 1px solid #CCCCCC;
            border-top: 1px solid #CCCCCC;
            margin: 0 auto 20px;
        }
        tbody
        {
            display: table-row-group;
            vertical-align: middle;
        }
        table.cxjg tr td.l
        {
            padding: 0 0 0 10px;
            text-align: left;
        }
        .cont_cxjg table.cxjg tr.color_hui
        {
            -moz-background-clip: border;
            -moz-background-inline-policy: continuous;
            -moz-background-origin: padding;
            background: #EBEBEB none repeat scroll 0 0;
        }
        .cont_cxjg table.cxjg tr th, .cont_cxjg table.cxjg tr td
        {
            border-bottom: 1px solid #CCCCCC;
            border-left: 1px solid #CCCCCC;
            color: #333333;
            height: 28px;
            line-height: 28px;
            overflow: hidden;
            text-align: center;
            vnd.ms-excel.numberformat:@
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cont_cxjg" id="tableEmployeeExport" runat="server" style="display: none;">
        <table cellspacing="0" cellpadding="0" border="0" width="auto" class="cxjg">
            <tbody>
                <tr class="color_hui">
                    <td width="200px">
                        ID
                    </td>
                    <td width="200px">
                        名称
                    </td>
                    <td width="200px">
                        变更详情
                    </td>
                    <%--<td width="200px">
                        导出状态
                    </td>--%>
                    <td width="200px">
                        变更类型
                    </td>
                    <td width="200px">
                        处理状态
                    </td>
                    <td width="200px">
                        处理时间
                    </td>
                    <th width="6%">
                        备注
                    </th>
                    <td width="200px">
                        创建日期
                    </td>
                    <td width="200px">
                        创建人
                    </td>
                </tr>
                <asp:Repeater ID="repterExportList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td class="l">
                                <%#(Eval("StatID").ToString() == "0" || Eval("StatID").ToString() == "-2") ? string.Empty : Eval("StatID")%>
                            </td>
                            <td class="l">
                                <%# Eval("StatName")%>
                            </td>
                            <td style="vnd.ms-excel.numberformat: @">
                                <%# Eval("ContrastInfo").ToString().Replace("<", "&lt;").Replace(">", "&gt;")%>
                            </td>
                            <%--<td>
                                <%# GetExportStatusName(Eval("ExportStatus"))%>
                            </td>--%>
                            <td class="l">
                                <%# GetContrastTypeName(Eval("ContrastType"))%>
                            </td>
                            <td class="l">
                                <%# GetDisposeStatusName(Eval("DisposeStatus").ToString())%>
                            </td>
                            <td class="l">
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("DisposeTime").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("Remark")%>
                            </td>
                            <td class="l">
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                            </td>
                            <td class="l">
                                <%# BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(Eval("CreateUserID").ToString()))%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="color_hui">
                            <td class="l">
                               <%-- <%# Eval("StatID")%>--%>
                               <%#(Eval("StatID").ToString() == "0" || Eval("StatID").ToString() == "-2") ? string.Empty : Eval("StatID")%>
                            </td>
                            <td class="l">
                                <%# Eval("StatName")%>
                            </td>
                            <td style="vnd.ms-excel.numberformat: @">
                                <%# Eval("ContrastInfo").ToString().Replace("<", "&lt;").Replace(">", "&gt;")%>
                            </td>
                            <%--<td>
                                <%# GetExportStatusName(Eval("ExportStatus"))%>
                            </td>--%>
                            <td class="l">
                                <%# GetContrastTypeName(Eval("ContrastType"))%>
                            </td>
                            <td class="l">
                                <%# GetDisposeStatusName(Eval("DisposeStatus").ToString())%>
                            </td>
                            <td class="l">
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("DisposeTime").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("Remark")%>
                            </td>
                            <td class="l">
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                            </td>
                            <td class="l">
                                <%# BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(Eval("CreateUserID").ToString()))%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
