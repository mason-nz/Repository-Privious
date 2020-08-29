<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderCreateUserPoper.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers.WorkOrderCreateUserPoper" %>

<script type="text/javascript">

    //根据真实姓名，查询员工信息
    function SearchByUserName() {
        var trueName = $.trim($('#txtUserName').val()); //请输入姓名或员工编号进行查询
        var pody = 'UserName=' + escape(trueName)
        $('#divUserList').load('/WorkOrder/AjaxServers/WorkOrderCreateUserPoper.aspx #divUserList > *', pody, LoadDivSuccessByEmployee);
    }

    //查询之后，回调函数
    function LoadDivSuccessByEmployee(data) {
        $('#tableQueryEmployee tr:even').addClass('bg'); //设置列表行样式
    }
    function ShowDataByPost3(pody) {
        $('#divUserList').load('/WorkOrder/AjaxServers/WorkOrderCreateUserPoper.aspx #divUserList > *', pody, LoadDivSuccessByEmployee);
    }

    //选择操作
    function SelectUser(userId, userName) {
        $.closePopupLayer('SelectUserPopup', true, { UserID: userId, UserName: userName });
    }
    //清空
    function ClearData() {
        $.closePopupLayer('SelectUserPopup', true, { UserID: "", UserName: "" });
    }
    $(document).ready(function () {
        //敲回车键执行方法
        enterSearch(SearchByUserName);
    });
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div class="pop pb15 openwindow" style="background: #FFF;">
    <div class="title bold">
        <h2>
            选择操作人</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectUserPopup',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('SelectUserPopup',false);">
    </div>
    <!--搜索条件-->
    <div id='divQueryByEmployee'>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 260px;">员工姓名：
                    <input type="text" name="txtUserName" id="txtUserName" class="w190" />
                </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchByUserName();" />
                </li>
            </ul>
        </div>
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
                            工号
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
                        </td>
                        <td>
                           <label><%# Eval("AgentNum")%></label>
                        </td>    
                    </tr>
                    </ItemTemplate>
                    </asp:repeater>
                    </tbody>
                </table>
            </div>
            <div class="it_page" style="text-align: right;">
                <a href="javascript:void(0)" onclick="ClearData()" style="cursor: pointer; float: left;
                    color: #0088CC; padding-left: 10px;">清空已选择项</a>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                &nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <br />
    <div style="clear: both;">
    </div>
</div>
