<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder.MainList" %>

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
                    $.post("../../AjaxServers/TaskManager/NoDealerOrder/Handler.ashx", { Action: "RevokeTask", TaskIDS: encodeURIComponent(TaskIDS) }, function (data) {
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
                url: "../../AjaxServers/TaskManager/NoDealerOrder/AssignTask.aspx",
                beforeClose: function () {
                    search();

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
            url: "../../AjaxServers/TaskManager/NoDealerOrder/AssignTask.aspx",
            beforeClose: function () {
                search();
            }
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
                任务类别
            </th>
            <th>
                订单类型
            </th>
            <th>
                易湃单号
            </th>
            <th>
                处理人
            </th>
            <th>
                处理状态
            </th>
            <th>
                推荐经销商
            </th>
            <th>
                创建时间
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
                            <a href="../../../TaskManager/NoDealerOrder/NoDealerOrderView.aspx?TaskID=<%#Eval("TaskID") %>" target="_blank"><%#Eval("TaskID") %></a>&nbsp;
                        </td>
                        <td>
                           <%#Eval("UserName")%>
                            &nbsp;
                        </td>
                        <td>
                           <%#(Eval("DealerID").ToString() == "0" || Eval("DealerID").ToString() == "") ? "无主订单" : "免费订单"%>
                            &nbsp;
                        </td>
                        <td>
                            <%#Eval("Source") .ToString()=="1"?"新车":(Eval("Source") .ToString()=="2"?"置换":"试驾")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("YPOrderID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AssignUserID") == DBNull.Value ? "" : BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(Eval("AssignUserID").ToString()))%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.TaskStatus), Convert.ToInt32(Eval("TaskStatus").ToString()))%>&nbsp;
                        </td>
                        <td>
                        <%#Eval("IsSelectDMSMember") == DBNull.Value? "" : Eval("IsSelectDMSMember").ToString() == "True" ? "是" : "否"%> &nbsp;
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
