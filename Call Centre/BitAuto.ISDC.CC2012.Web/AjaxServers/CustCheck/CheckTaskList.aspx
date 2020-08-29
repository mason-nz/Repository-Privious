<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckTaskList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck.CheckTaskList" %>

<script type="text/javascript">

    $(document).ready(function () {

        var pageSize = '<%=pageSize %>';
        $("a[name='apageSize'][v='" + pageSize + "']").addClass("selectA");

        $("a[name='apageSize']").die("click").live("click", function (e) {
            e.preventDefault();
            $("a[name='apageSize']").removeClass("selectA");
            $(this).addClass("selectA");

            $("#hidSelectPageSize").val($(this).attr("v"));
            search();
        });

        var RecordCount = '<%=RecordCount %>';
        if (RecordCount < 20) {
            $(".pageP").hide();
        }
    });
    
</script>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="100%" id="tableList">
        <tr>
            <th style="width: 3%">
                <input id="ckbAllSelect" type="checkbox" name="selItem" />
            </th>
            <th style="width: 10%">
                任务ID
            </th>
            <th>
                客户名称
            </th>
            <th style="width: 15%">
                所属项目
            </th>
            <th style="width: 7%">
                状态
            </th>
            <th style="width: 10%">
                坐席
            </th>
            <th style="width: 10%">
                操作人
            </th>
            <th style="width: 10%">
                操作时间
            </th>
            <th style="width: 10%">
                最后操作时间
            </th>
            <th style="width: 6%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                       
                           <input name="chkSelect"  type="checkbox" value='<%#Eval("PTID") %>'  />
                        </td>
                        <td>
                          <%#getview(Eval("PTID").ToString(),Eval("CarType").ToString(),Eval("Source").ToString(),Eval("TaskStatus").ToString()) %>
                            &nbsp;
                        </td>
                        <td>
                        <%#GetCustUrl(Eval("CustName").ToString(), Eval("CRMCustID").ToString(), Eval("Source").ToString(), Eval("TaskStatus").ToString())%>  &nbsp;
                        </td>
                        <td>
                            <%#Eval("ProjectName")%>
                        </td>
                        
                        <td name="tdTaskStatus">
                          <%#getStatus(Eval("TaskStatus").ToString(),Eval("AdditionalStatus").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("UserID").ToString())%>&nbsp;
                            
                        </td>
                         <td>
                            <%#getOperator(Eval("LastOptUserID").ToString())%>&nbsp;
                            
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOptTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                         </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("LastOperTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                         </td>                        
                        <td>
                            <%# GetCheckUrl(Eval("TaskStatus").ToString(), Eval("AdditionalStatus").ToString(), Eval("PTID").ToString(), Eval("CarType").ToString(), Eval("Source").ToString(), Eval("UserID").ToString())%> &nbsp;
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
