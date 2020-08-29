<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCustUserPoper.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers.SelectCustUserPoper" %>

<script type="text/javascript">
    if ('<%=CrmCustID %>' == '') {
        //根据真实姓名，查询员工信息
        function SearchByUserName() {
            var trueName = $.trim($('#txtUserName').val()); //请输入姓名或员工编号进行查询
            var pody = 'UserName=' + escape(trueName)
            $('#divUserList').load('/WorkOrder/AjaxServers/SelectCustUserPoper.aspx #divUserList > *', pody, LoadDivSuccessByEmployee);
        }

        //查询之后，回调函数
        function LoadDivSuccessByEmployee(data) {
            $('#tableQueryEmployee tr:even').addClass('bg'); //设置列表行样式
        }
        function ShowDataByPost3(pody) {
            $('#divUserList').load('/WorkOrder/AjaxServers/SelectCustUserPoper.aspx #divUserList > *', pody, LoadDivSuccessByEmployee);
        }

        //清空
    }
    //选择操作
    function SelectUser(userId, userName) {
        $.closePopupLayer('SelectCustUserPopup', true, { UserID: userId, UserName: userName });
    }
    $(document).ready(function () {
        //敲回车键执行方法
        if ('<%=CrmCustID %>' == '') {
            enterSearch(SearchByUserName);
        }
    });
</script>
<div class="pop pb15 openwindow" style="background: #FFF;">
    <div class="title bold">
        <h2>
            选择负责人</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectCustUserPopup',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('SelectCustUserPopup',false);">
    </div>
    <!--搜索条件-->
    <div id='divQueryByEmployee'>
        <%if (string.IsNullOrEmpty(CrmCustID))
          { %>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 260px;">员工姓名：
                    <input type="text" name="txtUserName" id="txtUserName" class="w190" />
                </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchByUserName();" />&nbsp;
                </li>
            </ul>
        </div>
        <%} %>
        <!--备选列表-->
        <div id="divUserList">
            <div class="Table2" id="EmList">
                <table width="100%" cellspacing="0" cellpadding="0" id="tableQueryEmployee" class="tableList mt10 mb15">
                    <tr>
                        <th style="width: 8%;">
                            选择
                        </th>
                        <th style="width: 10%;">
                            姓名
                        </th>
                        <th style="width: 10%;">
                            部门名称
                        </th>
                    </tr>
                    <tbody>
                        <asp:repeater id="rptUser" runat="server">
                    <ItemTemplate>
                    <tr>
                        <td>
                            <a class="linkBlue" href="javascript:void(0)" onclick="SelectUser('<%# Eval("UserID") %>','<%# Eval("TrueName")%>');" name='<%# Eval("TrueName")%>' id='<%# Eval("UserID") %>'>选择</a>
                        </td>
                        <td>
                            <%# Eval("TrueName") %>
                            <%# string.IsNullOrEmpty(Eval("BGName").ToString()) ?  string.Empty:"【"+ Eval("BGName")+"】"%>
                        </td>
                        <td>
                           <label><%# Eval("DepartName")%></label>
                        </td>    
                    </tr>
                    </ItemTemplate>
                    </asp:repeater>
                    </tbody>
                </table>
            </div>
            <div class="it_page" style="text-align: right;">
             <a href="javascript:void(0)" onclick="SelectUser('','')" style="cursor: pointer; float: left;
                    color: #0088CC; padding-left: 10px;">清空选择项</a>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                &nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <br />
    <div style="clear: both;">
    </div>
</div>
