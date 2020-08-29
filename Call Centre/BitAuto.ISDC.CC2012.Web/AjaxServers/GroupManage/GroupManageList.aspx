<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupManageList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GroupManage.GroupManageList" %>

<div class="bit_table" id="bit_table">
    <div class="optionBtn clearfix">
        <%if (AddAuth)
          { %>
        <input type="button" value=" 新增分组 " onclick="On_Modify('')" class="newBtn" style="*margin-top: 3px;" />
        <%} %>
    </div>
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="20%">
                分组名称
            </th>
            <th width="8%">
                所属区域
            </th>
            <th width="8%">
                业务分类
            </th>
            <th width="20%">
                负责业务线
            </th>
            <th width="20%">
                外显400号码
            </th>
            <th width="8%">
                员工数量
            </th>
            <th width="8%">
                状态
            </th>
            <th width="8%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"> 
                        <td>
                            <%#Eval("Name")%>&nbsp;
                        </td>                 
                        <td align="center">
                            <%#GetRegionName(Eval("RegionID").ToString())%>&nbsp;
                        </td>                
                        <td align="center">                            
                            <%#GetBusinessType(Eval("BusinessType").ToString())%>&nbsp;
                        </td>
                        <td> 
                            <%#Eval("LineS")%>&nbsp;
                        </td>                        
                        <td>
                           <%#Eval("CallNum")%>&nbsp;
                        </td>
                        <td align="center">
                            <%#Eval("EmployeeNum")%>&nbsp;
                        </td>
                        <td align="center">
                           <span id="span_status_<%#Eval("BGID") %>"><%#GetStatusName(Eval("Status").ToString())%>&nbsp;</span>
                        </td>
                        <td align="center" id="option_<%#Eval("BGID") %>">
                            <%#GetOptionLink(Eval("BGID").ToString(), Eval("Status").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="pageHidden" value='<%=getCurrentPage() %>' />
    <script type="text/javascript">
        //修改
        function On_Modify(bgid) {
            $.openPopupLayer({
                name: "GroupManageAdd",
                parameters: { BGID: bgid, r: Math.random() },
                url: "GroupManageAdd.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        var page = '<%=PageIndex %>';
                        if (bgid == "") {
                            //新增，刷新到第一页
                            page = 1;
                        }
                        search(page);
                    }
                }
            });
        }
        //启用或停用
        function On_ChangeStatus(bgid, status) {
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "ChangeBusinessGroupStatus", BGID: bgid, Status: status, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    if (status == 0) {
                        //启用了
                        $("#span_status_" + bgid).text('在用');
                        $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_Modify('" + bgid + "')\">修改</a>&nbsp;&nbsp;<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','1')\">停用</a>");
                    }
                    else {
                        //停用了
                        $("#span_status_" + bgid).text('停用');
                        $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_ChangeStatus('" + bgid + "','0')\">启用</a>");
                    }
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }
    </script>
</div>
