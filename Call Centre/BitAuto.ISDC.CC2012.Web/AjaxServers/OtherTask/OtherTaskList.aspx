<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherTaskList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask.OtherTaskList" %>

<script type="text/javascript">
    $(document).ready(function () {
        var pageSize = '<%=PageSize %>';
        $("a[name='apageSize'][v='" + pageSize + "']").addClass("selectA");

        $("a[name='apageSize']").bind("click", function (e) {
            e.preventDefault();
            $("a[name='apageSize']").removeClass("selectA");
            $(this).addClass("selectA");

            $("#hidSelectPageSize").val($(this).attr("v"));
            search();
        });

        //全选/全不选
        $("#ckbAllSelect").live("click", function () {
            $(":checkbox[name='chkSelect']").attr("checked", $(this).attr("checked"));
        });

        var RecordCount = '<%=RecordCount %>';
    });
    
</script>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="width: 5%">
                <input id="ckbAllSelect" type="checkbox" name="selItem" />
            </th>
            <th>
                任务ID
            </th>
            <th>
                客户名称
            </th>
            <th>
                所属项目
            </th>
            <th>
                所属分组
            </th>
            <th>
                状态
            </th>
            <th>
                自动外呼
            </th>
            <th>
                操作人
            </th>
            <th>
                操作时间
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                       
                           <input name="chkSelect"  type="checkbox" value='<%#Eval("PTID") %>'  bgid=<%#Eval("BGID")%> />
                        </td>
                        <td>
                            <a href="/OtherTask/OtherTaskDealView.aspx?OtherTaskID=<%#Eval("PTID")%>" target='_blank'><%#Eval("PTID")%></a>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CustName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("projectName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("GroupName")%>&nbsp;
                        </td> 
                        <td>
                          <%#getStatusName(Eval("TaskStatus").ToString())%>&nbsp;
                        </td>
                         <td>
                          <%#getACStatusName(Eval("ACStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("LastOptUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOptTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                         </td>
                        <td>
                        <%#getOperLink(Eval("TaskStatus").ToString(), Eval("PTID").ToString(), Eval("UserID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p class="pageP">
            每页显示条数 <a href="#" name="apageSize" v='20'>20</a>&nbsp;&nbsp; <a href="#" name="apageSize"
                v='50'>50</a>&nbsp;&nbsp; <a href="#" name="apageSize" v='100'>100</a>
        </p>
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
