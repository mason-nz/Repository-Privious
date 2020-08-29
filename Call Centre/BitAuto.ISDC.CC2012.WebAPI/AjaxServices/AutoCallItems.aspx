<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCallItems.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.WebAPI.AjaxServices.AutoCallItems" %>

<div class="bit_table">
    <table width="98%" border="1" cellpadding="0" cellspacing="0" class="tableList" id="tableMemberList">
        <tbody>
            <tr class="bold color_hui" style="height: 28px;">
                <th width="10%" style="height: 28px;">
                    项目ID
                </th>
                <th width="10%" style="height: 28px;">
                    电话号码
                </th>
                <th width="10%" style="height: 28px;">
                    任务ID
                </th>
                <th width="50px;" style="height: 28px;">
                    外呼状态
                </th>
                <th width="65px;">
                    是否回写
                </th>
                <th width="50px;">
                    外呼结果
                </th>
                <th width="20%">
                    外显400号码
                </th>
                <th width="40px;">
                    技能组
                </th>
                <th width="80px;">
                    推送时间
                </th>
                <th width="80px;">
                    回写时间
                </th>
            </tr>
            <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">                        
                        <td>
                            <%#Eval("ProjectID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Phone")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("BusinessID")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("callStatus")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("isReWrite")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("callResult")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("CallNum")%>&nbsp;
                        </td>
                         <td>
                            <%#Eval("SkillID")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("PushTime").ToString())%>&nbsp;
                        </td>
                        <td>
                           <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ReturnTime").ToString())%>&nbsp;
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
