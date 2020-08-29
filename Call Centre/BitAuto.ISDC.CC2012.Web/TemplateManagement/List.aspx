<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th>
                模板名称
            </th>
            <th>
                所属分组
            </th>
            <th>
                分类
            </th>
            <th>
                状态
            </th>
            <th>
                是否可用
            </th>
            <th>
                创建人
            </th>
            <th>
                创建时间
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <a href='/TemplateManagement/TemplateView.aspx?ttcode=<%#Eval("TTCode")%>' target="_blank"><%#Eval("TPName")%></a>&nbsp;
                        </td>
                        <td>
                            <%#Eval("GroupName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CategoryName")%>&nbsp;
                        </td>
                        <td>
                          <%#getStatusName(Eval("Status").ToString(), Eval("TTIsData").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("IsUsedName")%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                         </td>
                        <td>
                        <%#getOperLink(Eval("Status").ToString(), Eval("RecID").ToString(), Eval("TPName").ToString(), Eval("TTCode").ToString(), Eval("GenTempletPath").ToString(), Eval("TTIsData").ToString(), Eval("IsUsed").ToString())%>                          
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
