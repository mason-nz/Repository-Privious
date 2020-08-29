<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabelManageList.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage.LabelManageList" %>

<table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <th width="30%">所属分组</th>
            <th width="50%">标签</th>
            <th width="15%">操作</th>
          </tr>
          <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td class="cName" name="csName">
                                    <%#Eval("Name") %>&nbsp;
                                </td>
                                <td class="cName" name="ltName">
                                    <%#Eval("ltNames")%>&nbsp;
                                </td>
                                <td>
                                    <a href="javascript:void(0)" bgid="<%#Eval("BGID") %>" onclick="LabelConfigPop(this)">修改</a>                                     
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>         
        </table>
