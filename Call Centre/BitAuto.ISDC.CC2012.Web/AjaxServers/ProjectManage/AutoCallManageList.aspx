<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCallManageList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage.AutoCallManageList" %>

<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" id="tableList" width="98%">
        <tr>
            <th style="width: 15%;">
                外呼项目
            </th>
            <th style="width: 8%;">
                项目状态
            </th>
            <th style="width: 8%;">
                外呼状态
            </th>
            <th style="width: 20%;">
                外显400号码
            </th>
            <th style="width: 14%;">
                技能组
            </th>
            <th style="width: 10%;">
                创建人
            </th>
            <th style="width: 15%;">
                创建时间
            </th>
            <th style="width: *%;">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td did="<%#Eval("ProjectID") %>">
                            <a href='/ProjectManage/ViewProject.aspx?projectid=<%#Eval("ProjectID") %>' target="_blank"><%#Eval("proName") %></a>
                        </td>
                        <td>                            
                            <%#GetStatus( Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#GetACStatus(Eval("ACStatus").ToString())%>&nbsp;                            
                        </td>
                        <td>
                          <%#Eval("CallNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("skName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TrueName")%>&nbsp;
                         </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                          <%#getOperLink(Eval("Status").ToString(), Eval("ACStatus").ToString(), Eval("ProjectID").ToString(), Eval("proName").ToString(), Eval("CDID").ToString(), Eval("SkillID").ToString())%> &nbsp;
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
