<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagManagementList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.TagManagementList" %>

<div class="bit_table" id="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="20%">
                一级标签
            </th>
            <th width="60%">
                二级标签
            </th>
            <th width="5%">
                状态
            </th>
            <th width="15%">
                操作
            </th>
        </tr>
        <asp:repeater id="rpt" runat="server">
                <ItemTemplate>
                    <tr  > 
                     
                        <td align="center">
                            <%#Eval("TagName").ToString()%>&nbsp;
                        </td>                
                        <td align="center">                            
                            <%#Eval("child").ToString().Replace(",", "，")%>&nbsp;
                        </td>
                        <td>
                           <%#GetZYTitle(Eval("Status").ToString(),"1")%>&nbsp;
                        </td>
                        <td>
                           <a href="javascript:void(0);" onclick="AddTagLayer('2',<%#Eval("RecID").ToString()%>,<%#Eval("BusiTypeID").ToString()%>);">编辑二级标签</a>
                         
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
