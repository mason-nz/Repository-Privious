<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueueList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage.QueueList"
    EnableViewState="false" %>
<div class="bit_table">
    <!--列表开始-->
    <div class="faqList">
<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="15%">
            访客名称
        </th>
        <th width="15%">
            访客来源
        </th>
        <th width="20%">
            发起页面
        </th>
        <th width="18%">
            进入队列时间
        </th>
        <th width="15%">
            等待时长
        </th>
        <th width="17%">
            地区
        </th>
    </tr>
    <asp:repeater runat="server" id="rpt">
     <ItemTemplate>
          <tr>
                <td>
                    <%#Eval("UserName")%>&nbsp;
                </td> 
                <td>
                    <%#Eval("SourceType")%>&nbsp;
                </td>
                <td>
                    <%#Eval("UserReferTitle")%>&nbsp;
                </td>
                <td>
                      <%#Eval("CreatTime")%>&nbsp;
                </td>
                <td>
                    <%#Eval("Seconds")%>&nbsp;
                </td> 
                <td>
                    <%#Eval("ProvinceICity")%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
        </asp:repeater>
    <tr>
        <td colspan="6">
            <div class="pagesnew" style="float: none; margin:10px; text-align:center;" id="Div1">
                <p>
            <asp:literal runat="server" id="litPagerQueueData"></asp:literal>
                </p>
            </div>
        </td>
    </tr>
</table>
    </div>
    <input type="hidden" value="<%=RecordCount %>" id="hidHistoryTotalCount" />
</div>
