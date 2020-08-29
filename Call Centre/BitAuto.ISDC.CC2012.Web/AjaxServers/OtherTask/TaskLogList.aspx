<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskLogList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask.TaskLogList" %>

<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="20%">
            操作时间
        </th>
        <th width="20%">
            操作类型
        </th>
        <th width="20%">
            任务状态
        </th>
        <th width="20%">
            操作人
        </th>
        <th width="20%">
            备注
        </th>
        <%--<th width="20%">播放录音 </th>--%>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.EnumProjectTaskOperationStatus), int.Parse(Eval("OperationStatus").ToString()))%>
                </td>
                 <td>
                    <%# BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.OtheTaskStatus), int.Parse(Eval("TaskStatus").ToString()))%>
                </td>
                <td>
                    <%#Eval("TrueName")%>                  
                </td>
                <td class="l">
                    <%#Eval("Description").ToString()%>                    
                </td>
                <%--<td>
                    <%#Eval("TID").ToString()%>
                </td> --%>              
            </tr>               
        </ItemTemplate>
    </asp:repeater>
</table>
