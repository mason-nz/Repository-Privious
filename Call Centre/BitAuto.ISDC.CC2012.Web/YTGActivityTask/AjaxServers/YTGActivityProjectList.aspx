<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YTGActivityProjectList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers.YTGActivityProjectList"
    EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="width: 18%;">
                项目名称
            </th>
            <th style="width: 10%;">
                创建时间
            </th>
            <th style="width: 30%;">
                关联活动主题
            </th>
            <th style="width: 15%;">
                报名时间
            </th>
            <th style="width: 15%;">
                活动时间
            </th>
            <th style="width: 5%;">
                状态
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                          <%#Eval("projectName") %>
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ActivityName")%>&nbsp;
                        </td>
                        <td>
                          <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("SignTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("Activetime").ToString())%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("status")%>&nbsp;
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
