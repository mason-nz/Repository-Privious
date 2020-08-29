<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupOrderList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.GroupOrderList" %>

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
    });

    function chkAllChecked() {
        if ($("#chkAll").attr("checked")) {
            $("input[name='chkSelect']").each(function () {
                if ($(this).attr("disabled") == false) {
                    $(this).attr("checked", true)
                }
            });
        }
        else {
            $("input[name='chkSelect']").each(function () {
                $(this).attr("checked", false)
            });
        }
    }
    //回收任务
    function RevokeTask() {
        var TaskIDS = $("input[name='chkSelect']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');
        if (Len(TaskIDS) > 0) {
            $.jConfirm("确定收回选择的任务吗？", function (r) {
                if (r) {
                    $.post("/AjaxServers/GroupOrder/AssignTask.ashx", { Action: "RevokeTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.Result == "yes") {
                            $.jPopMsgLayer("收回成功！");
                            search();
                        }
                        else {
                            alert(jsonData.ErrorMsg);
                            search();
                        }
                    });
                }
            });
        }
        else {
            $.jAlert("请至少选择一个回收的任务！");
        }
    }

    function AssignTask() {
        var TaskIDS = $("input[name='chkSelect']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');
        if (Len(TaskIDS) > 0) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopup",
                parameters: { TaskIDS: escape(TaskIDS) },
                url: "/AjaxServers/GroupOrder/AssignTask.aspx",
                beforeClose: function (n) {
                if(n==true){
                    search();
                    }

                }
            });
        }
        else {
            $.jAlert("请至少选择一个要分配的任务！");
        }

    }
    function AssignTaskOne(TaskID) {
        $.openPopupLayer({
            name: "AssignTaskAjaxPopup",
            parameters: { TaskIDS: escape(TaskID) },
            url: "/AjaxServers/GroupOrder/AssignTask.aspx",
            beforeClose: function () {
                search();
            }
        });

    }

    function ImportExcelWriteBack() {
        $.openPopupLayer({
            name: "UploadUserAjaxPopup",
            parameters: {},
            url: "/AjaxServers/GroupOrder/ImportExcelWriteBack/Main.aspx?CurrentUserId="+ <%=CurrentUserId %>
        });
    }

</script>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th>
                <input type="checkbox" id="chkAll" onclick="chkAllChecked()" />
            </th>
            <th>
                任务ID
            </th>
            <th>
                客户姓名
            </th>            
            <th>
                订单编号
            </th>
            <th>
                任务状态
            </th>
            <%--<th>
                处理状态
            </th>--%>
            <th>
                处理人
            </th>                        
            <th>
                下单时间
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <input name="chkSelect"  type="checkbox" value='<%#Eval("TaskID") %>' <%#Convert.ToInt32(Eval("TaskStatus").ToString())==4?"disabled":""%> />
                        </td>
                        <td>
                            <a href="../../../GroupOrder/GroupOrderView.aspx?TaskID=<%#Eval("TaskID") %>" target="_blank"><%#Eval("TaskID") %></a>&nbsp;
                        </td>
                        <td>
                           <%--<%#Eval("CustomerName")%>
                            &nbsp;--%>
                            <%#getCustomerLink(Eval("CustID").ToString(), Eval("CustomerName").ToString(), Eval("TaskStatus").ToString())%>&nbsp;                           
                        </td>                                               
                        <td>
                            <%#Eval("OrderCode")%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.GroupTaskStatus), Convert.ToInt32(Eval("TaskStatus").ToString()))%>&nbsp;
                        </td>
                        <%--<td>
                            <%#Eval("IsReturnVisit").ToString() == "-2" ? "" : BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.IsReturnVisit), Convert.ToInt32(Eval("IsReturnVisit").ToString()))%>&nbsp;
                        </td>--%>
                        <td>
                            <%#Eval("AssignUserID") == DBNull.Value ? "" : BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(Eval("AssignUserID").ToString()))%>&nbsp;
                        </td>                                                
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("OrderCreateTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("TaskID").ToString(), Eval("TaskStatus").ToString(), Eval("AssignUserID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
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
