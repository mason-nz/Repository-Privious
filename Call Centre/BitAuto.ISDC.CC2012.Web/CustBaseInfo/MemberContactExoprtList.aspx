<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberContactExoprtList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.MemberContactExoprtList" %>

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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableDocs">
            <tr class="color_hui">
                <th width="25%">
                    <strong>客户ID</strong>
                </th>
                <th width="25%">
                    <strong>客户名称</strong>
                </th>
                <th width="10%">
                    <strong>会员ID</strong>
                </th>
                <th width="25%">
                    <strong>会员名称</strong>
                </th>
                <th width="20%">
                    <strong>联系人</strong>
                </th>
                <th width="20%">
                    <strong>职务</strong>
                </th>
                <th width="20%">
                    <strong>职级</strong>
                </th>
                <th width="20%">
                    <strong>会员地址</strong>
                </th>
                <th width="20%">
                    <strong>会员邮编</strong>
                </th>
                <th width="20%">
                    <strong>省份</strong>
                </th>
                <th width="20%">
                    <strong>城市</strong>
                </th>
                <th width="20%">
                    <strong>区县</strong>
                </th>
                <th width="20%">
                    <strong>客户邮编</strong>
                </th>
                <th width="20%">
                    <strong>主营品牌</strong>
                </th>
            </tr>
            <asp:Repeater ID="Repeater_DMSMember" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%#Eval("CustID")%>
                        </td>
                        <td align="center">
                            <%#Eval("CustName").ToString().Replace(" ", "&nbsp;") %>
                        </td>
                        <td align="center">
                            <%#Eval("MemberCode")%>
                        </td>
                        <td align="center">
                            <%#Eval("DMSName").ToString().Replace(" ", "&nbsp;")%>
                        </td>
                        <td align="center">
                            <%#Eval("CName").ToString().Replace(" ", "&nbsp;")%>
                        </td>
                        <td align="center">
                            <%#Eval("Title")%>
                        </td>
                        <td align="center">
                            <%# GetOfficeTypeName(Eval("OfficeTypeCode").ToString())%>
                        </td>
                        <td align="center">
                            <%#Eval("contactaddress")%>
                        </td>
                        <td align="center">
                            <%#Eval("postcode")%>
                        </td>
                        <td align="center">
                            <%#Eval("ProvinceName")%>
                        </td>
                        <td align="center">
                            <%#Eval("CityName")%>
                        </td>
                        <td align="center">
                            <%#Eval("CountyName")%>
                        </td>
                        <td align="center">
                            <%#Eval("CustZipCode")%>
                        </td>
                <td align="center">
                    <%#Eval("BrandNames")%>
                </td> 
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
