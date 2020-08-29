<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.SMSList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
        <th style="width: 2%">
                &nbsp;
            </th>
            <th style="width: 12%">
                所属分组
            </th>
            <th style="width: 10%">
                模板分类
            </th>
            <th style="width: 15%">
                模板标题
            </th>
            <th style="width: 33%">
                模板内容
            </th>
            <th style="width: 10%">
                创建时间
            </th>
            <th style="width: 8%">
                创建人
            </th>
            <th style="width: 10%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                    <td>&nbsp;</td>
                        <td>
                            <%#Eval("bgname")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("scname")%>&nbsp;
                        </td>
                        <td>
                          <a href='javascript:void(0)' onclick="ViewTemplate(<%#Eval("RecID")%>)"><%#Eval("Title")%></a>&nbsp;
                        </td>
                        <td title="<%#Eval("Content")%>">
                          <%# Eval("Content").ToString().Length > 27 ? Eval("Content").ToString().Substring(0,27)+"...": Eval("Content").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                         </td>
                         <td>
                            <%#getOperator(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                        <%#getOperLink(Eval("RecID").ToString())%>  
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
