<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSingleProjectPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage.SelectSingleProjectPop" %>

<div class="bit_table">
    <table width="98%" border="1" cellpadding="0" cellspacing="0" class="tableList" id="tableMemberList">
        <tbody>
            <tr class="bold color_hui" style="height: 28px;">
                <th width="20%" style="height: 28px;">
                    操作
                </th>
                <th width="28%" style="height: 28px;">
                    项目名称
                </th>
                <th width="24%" style="height: 28px;">
                    所属分组
                </th>
                <th width="28%" style="height: 28px;">
                    分类
                </th>
            </tr>
            
              <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <a href="javascript:PopAjaxSelectProject('<%# Eval("Name") %>','<%# Eval("ProjectID") %>')">选择</a>
                        </td>
                        <td>
                            <%#Eval("Name")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("GroupName")%>&nbsp;
                        </td>
                        <td>
                           <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>                      
                    </tr>
                </ItemTemplate>
            </asp:repeater>
        </tbody>
    </table>
    <div class="pageTurn mr10">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
