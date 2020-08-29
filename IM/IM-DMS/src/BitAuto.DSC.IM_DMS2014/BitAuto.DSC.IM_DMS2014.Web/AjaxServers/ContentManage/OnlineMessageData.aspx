<%@ Page Title="在线留言数据列表页" Language="C#" AutoEventWireup="true" CodeBehind="OnlineMessageData.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage.OnlineMessageData" %>


<div class="bit_table">
    <!--列表开始-->
    
    <div class="faqList">
        <table border="0" cellspacing="0" cellpadding="0">
            <thead>
            <th colspan="11" style="background:#F9F9F9">
            <div class="btn btn2 right"><input type="button"  value="导出" onclick="ExportData()" class="save w60 gray"/></div>
            </th>
            </thead>
            <tr>
            <th width="14%">经销商名称</th>
            <th width="6%">类型</th>
            <th width="5%">姓名</th>
            <th width="8%">电话</th>
            <th width="10%">时间</th>
            <th width="12%">内容</th>
            <th width="5%">操作人</th>
            <th width="10%">操作时间</th>
            <th width="10%">备注</th>
            <th width="12%">工单</th>
            <th width="8%">状态</th>
            </tr>
            <asp:repeater runat="server" id="Rt_CSData">
                <ItemTemplate>
                      <tr>
                        <td class="cName"><%#Eval("MemberName")%>&nbsp;</td>
                        <td ><%#Eval("MessageType")%>&nbsp;</td>
                        <td><%#Eval("UserName")%>&nbsp;</td>
                        <td><%#Eval("Phone")%>&nbsp;</td>
                        <td ><%#Eval("CreateTime")%>&nbsp;</td>
                        <td class="cName"><a title='<%#Eval("Content")%>' onclick='javascript:OpenContentDetailLayer(<%#Eval("RecID")%>)' href="#"><%#Eval("Content") !=null?(Eval("Content").ToString().Length>8?Eval("Content").ToString().Substring(0,8)+"……":Eval("Content").ToString()):""%>&nbsp;</a></td>
                        <td ><%#Eval("TrueName")%>&nbsp;</td>
                        <td ><%#Eval("LastModifyTime")%>&nbsp;</td>   
                        <td ><%# RemarkMenuContrl(Eval("RecID").ToString(), Eval("hasRemarks").ToString(), Eval("Status").ToString(), Eval("Remarks"))%></td>
                        <td >
                            <%# WorkOrderMenuContrl(Eval("OrderID"),Eval("hasWorkOrder"),Eval("Status"),Eval("Phone"),Eval("UserName"),Eval("MemberCode"),Eval("MemberName"),Eval("RecID")) %>
                        </td>
                        <td><select class="w60" style=" width:70px;"  id="selMessageState111" onchange="changeMessageState(this)" name="<%#Eval("RecID") %>">
                            <%#GetOptions(Eval("Status").ToString())%></select>
                        </td>
                      </tr>
                </ItemTemplate>
             </asp:repeater>
             <tr>
                    <td colspan="11">
                        <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                            <p>
                                 <asp:literal runat="server" id="litPagerDown"></asp:literal>
                            </p>
                        </div>
                    </td>
             </tr>
       </table>
    </div>
    <!--列表结束-->
    <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
</div>

