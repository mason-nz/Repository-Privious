<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.LeadsTask.AjaxServers.AjaxList" %>

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
    //全选
    (function () {

        var oAllSelect = document.getElementById("ckbAllSelect");
        var aSelect = document.getElementsByName("chkSelect");
        for (var i = 0; i < aSelect.length; i++) {
            aSelect[i].onclick = function () {
                if (this.checked) {
                    var isAllCheck = true;
                    for (var i = 0; i < aSelect.length; i++) {
                        if (!aSelect[i].checked) {
                            isAllCheck = false;
                            break;
                        }
                    }
                    if (isAllCheck) oAllSelect.checked = true;
                }
                else {
                    oAllSelect.checked = false;
                }
            };
        }
        oAllSelect.onclick = function () {
            if (this.checked) {
                for (var i = 0; i < aSelect.length; i++) {
                    aSelect[i].checked = true;
                }
            }
            else {

                for (var i = 0; i < aSelect.length; i++) {
                    aSelect[i].checked = false;
                }

            }
        }
    })();
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
                所属项目
            </th>
            <th>
                姓名
            </th>
            <th>
                电话
            </th>
            <th>
                下单品牌
            </th>
            <th>
                下单车型
            </th>
            <th>
                所属坐席
            </th>
            <th>
                最晚处理日期
            </th>
            <th>
                状态
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>                       
                           <input name="chkSelect"  type="checkbox" value='<%#Eval("TaskID") %>'/>
                        </td>
                        <td>
                            <%#GetView(Eval("source").ToString(), Eval("TaskID").ToString())%>
                        </td>
                        <td>
                            <%#Eval("ProjectName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("UserName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Tel")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("OrderCarMaster")%>&nbsp;
                        </td> 
                        <td>
                          <%#Eval("OrderCarSerial").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AssignName").ToString()%>&nbsp;
                        </td>
                        <td>
                            <%#lastTime(Eval("LastDealTime").ToString())%>&nbsp;
                         </td>
                        <td>
                            <%#getStatus(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                       <%#getOperLink(Eval("Status").ToString(), Eval("TaskID").ToString(),Eval("AssignUserID").ToString(),Eval("source").ToString())%>&nbsp;
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
