<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustBrandLicenseList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CustBrandLicenseList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg" id="tableALInfoList">
    <tr>
        <th width="25%">
            授权品牌
        </th>
        <th width="18%">
            有效期
        </th>
        <th width="8%">
            上传人
        </th>
        <th width="14%">
            上传时间
        </th>
        <th width="19%">
            附件
        </th>
    </tr>
    <asp:repeater runat="server" id="repeater">
        <ItemTemplate>
            <tr>   
                <td>
                    <%# Eval("Brand").ToString() %>
                </td>                
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                </td>
                 <td>
                    <%# getUserName(Eval("createUserId").ToString())%>
                </td>                
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
                <td >
                    <a href='http://crm.sys.bitauto.com/<%#GetFile(Eval("ID").ToString()) %>' target="_blank">查看</a><% %>
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
