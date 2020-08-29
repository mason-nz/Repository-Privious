<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabelConfigList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage.LabelConfigList" %>
<script type="text/javascript">
    initConfigPop();   
    </script>
<table cellspacing="0" cellpadding="0"  class="fzList">
          <tr>
            <th width="10%"><input id="chkAll" type="checkbox" /></th>
            <th width="50%">标签</th>
            <th width="30%">操作</th>
          </tr>
          <asp:repeater id="repeaterConfig" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td>
                                <%#Eval("isBelong").ToString() == "0" ? "<input name='' type='checkbox' checked value='' ltid='" + Eval("LTID").ToString() + "' />" : "<input name='' type='checkbox' value='' ltid='" + Eval("LTID").ToString() + "' />"%>
                                </td>
                                <td class="cName"><%#Eval("Name") %></td>
                                <td>                                    
                                    <%#Eval("LTID").ToString().Trim() != MinLTID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("LTID").ToString() + "','up')\">上移</a>" : "<span style=\"color:#666;\">上移</span>"%>
                                    <%#Eval("LTID").ToString().Trim() != MaxLTID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("LTID").ToString() + "','down')\">下移</a>" : "<span style=\"color:#666;\">下移</span>"%>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
        </table>
