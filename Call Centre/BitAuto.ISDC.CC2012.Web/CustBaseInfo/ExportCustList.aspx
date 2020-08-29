<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportCustList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.ExportList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
            width: 97.85%;
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
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cont_cxjg">
        <table  style=" width:100px;" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableDocs">
            <tr class="color_hui"  style=" width:100px;">
                <th>
                    <strong>客户ID</strong>
                </th>
            </tr>
            <asp:Repeater ID="rptCust" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%#Eval("CustID")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
