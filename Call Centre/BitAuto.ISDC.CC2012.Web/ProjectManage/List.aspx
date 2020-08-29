<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="width: 25%;">
                项目名称
            </th>
            <th style="width: 15%;">
                所属分组
            </th>
            <th style="width: 10%;">
                项目状态
            </th>
            <th style="width: 10%;">
                外呼状态
            </th>
            <th style="width: 10%;">
                创建人
            </th>
            <th style="width: 15%;">
                创建时间
            </th>
            <th style="width: 15%;">
                项目操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                          <%#getProjectName(Eval("IsOldData").ToString(), Eval("Name").ToString(), Eval("ProjectID").ToString(), Eval("comCount").ToString(), Eval("SumCount").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("GroupName")%>&nbsp;
                        </td>
                        <td>
                          <%#getStatus(Eval("Status").ToString())%>&nbsp;
                        </td>
                         <td>
                          <%#getACStatus(Eval("ACStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("CreateUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                         </td>                        
                        <td>
                            <%#getOperLink(Eval("Status").ToString(), Eval("ProjectID").ToString(), Eval("Source").ToString(), Eval("NoGenCount").ToString(), Eval("IsAutoCall").ToString(), Eval("ACStatus").ToString(), Eval("notDistributeCount").ToString())%> &nbsp;
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
