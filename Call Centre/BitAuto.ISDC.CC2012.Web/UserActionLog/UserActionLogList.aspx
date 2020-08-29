<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActionLogList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.UserActionLog.UserActionLogList" %>

<div class="bit_table">
   <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="100%" id="tableList" style="table-layout:fixed;">
        <colgroup>
            <col style="width: 10%;" />
            <col style="width: 10%;" />
            <col style="width: 60%;" />
            <col style="width: 20%;" />
        </colgroup>
          <tr class="back" onmouseout="this.className='back'">
            <th>
                用户
            </th>
            <th>
                IP
            </th>
            <th>
                操作描述
            </th>
            <th>
                操作时间
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                    <ItemTemplate>
                       <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                            <td>
                           
                              <%#Eval("TrueName").ToString()%>
                            </td>
                            <td>
                                <%#Eval("IP")%>
                            </td>
                            <td style="text-align:left;word-break : break-all;";>
                               <%# Eval("Loginfo")%> 
                            </td>
                            <td>
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
    </table>
    <br />
    <!--分页-->
    <div class="pages1" style="text-align: right;">
                <uc:AjaxPager ID="AjaxPager_Custs" runat="server" ContentElementId="ajaxTable" />
        </div>
</div>
