<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectEmployees.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExternalTask.TaskManager.SelectEmployees" %>

<script type="text/javascript">
    //根据真实姓名，查询员工信息
    function SearchByCnName() {
        var txtQueryEmployeeCnName = $('#txtQueryEmployeeCnName'); //请输入姓名或员工编号进行查询
        if (txtQueryEmployeeCnName.val() == '')
        { return false; }
        if ($.trim(txtQueryEmployeeCnName.val()) == '请输入姓名或员工编号进行查询')
        { txtQueryEmployeeCnName.val(''); }
        var pody = 'CnName=' + escape(txtQueryEmployeeCnName.val()) +
                   '&page=1&SearchByCnName=yes';
        $('#divQueryEmployeeList').load('/TaskManager/SelectEmployees.aspx #divQueryEmployeeList > *', pody, LoadDivSuccessByEmployee);
    }
    //查询之后，回调函数
    function LoadDivSuccessByEmployee(data) {
        $('#tableQueryEmployee tr:even').addClass('bg'); //设置列表行样式
    }
    function ShowDataByPost3(pody) {
        $('#divQueryEmployeeList').load('SelectEmployees.aspx #divQueryEmployeeList > *', pody, LoadDivSuccessByEmployee);
    }
    function SelectEmployeeAppendToTable(eid, name, number, departname) {
        var htmlStr = "<tr id='tr_" + eid + "'><td><a href='javascript:delSelectedEmployee(" + eid + ")'>删除</a></td><td>" + number + "</td><td><input id='hdn_" + eid + "' type='hidden' value=" + eid + " />" + name + "</td><td>" + departname + "</td></tr>";
        $("#SelectedEmployees").append(htmlStr);
    }
    function delSelectedEmployee(eid) {
        $("#tr_" + eid).remove();
    }
    //提交选择的员工
    function SubmitSelectEmployee() {
        if ($('#divQueryEmployeeList:visible').size() == 1) { //查找员工
            var eid = $('#divQueryEmployeeList table[id="tableQueryEmployee"] tr :radio[checked]').val();

            if (typeof (eid) == 'undefined')
            { eid = ""; }
            //            { $.jAlert('请选择一个员工！'); }
            //            else {
            var radioObj = $('#divQueryEmployeeList table[id="tableQueryEmployee"] :radio[value=' + eid + ']');
            var cnname = $.trim(radioObj.parents().next().html());
            var employeeNumber = $.trim(radioObj.parents().next().next().html());
            var domainAccount = $.trim(radioObj.parents().next().next().next().html());
            var departID = $.trim(radioObj.parents().parents().children().find('td:last').attr('value'));
            $('#popupLayer_' + 'SelectEmployeePopup').data('EID', eid)
                                                         .data('DepartID', departID)
                                                         .data('CnName', cnname)
                                                         .data('EmployeeNumber', employeeNumber)
                                                         .data('DomainAccount', domainAccount);

            $.closePopupLayer('SelectEmployeePopup', true, { EID: eid, DeptID: departID, CnName: cnname, DomainAccount: domainAccount, EmployeeNumber: employeeNumber });
            // }
        }
    }

    //清空
    function ClearEmployee() {
        $("input[name=QueryByCnName]").removeAttr('checked');
    }
</script>
<div class="pop pb15">
    <div class="title pt5 ft16 bold">
        人员查询<span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);" class="right"><img src="../images/clolse.gif" width="18" height="18" /></a></span></div>
    <ul class="clearfix ft14 outTable">
        <li class="name1" style="width:500px">请输入员工姓名或域名：
            <input type="text" class="w125" value="" onfocus="javascript:var name=document.getElementById('txtQueryEmployeeCnName');if(name.value=='请输入姓名或员工编号进行查询')name.value='';"
                name="txtQueryEmployeeCnName" id="txtQueryEmployeeCnName" />
            <input type="button" value="查询" class="searchBtn bold" onclick="javascript:SearchByCnName();" /></li>
         
    </ul>
    <div id="divQueryEmployeeList">
        <div class="bit_table renyuan_t">
            <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="100%" id="tableQueryEmployee">
                <tr class="bold">
                    <td>
                        选择
                    </td>
                    <td>
                        姓名
                    </td>
                    <td>
                        编号
                    </td>
                    <td style="display: none">
                        域账号
                    </td>
                    <td>
                        部门
                    </td>
                </tr>
                <asp:repeater id="repeaterEmployeeList" runat="server">
                    <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                            <td>
                                <input type="radio" value='<%# Eval("EmployeeID") %>' name="QueryByCnName" />
                            </td>
                            <td>
                                <%# Eval("CnName") %>
                            </td>
                            <td>
                                <%# Eval("EmployeeNumber") %>
                            </td>
                            <td style="display:none">
                                <%# Eval("DomainAccount") %>
                            </td>
                            <td value='<%# GetDepartmentFullName(Eval("EmployeeNumber").ToString()) %>'>
                                <%# GetDepartmentFullName(Eval("EmployeeNumber").ToString())%>
                            </td>
                        </tr>
                        </ItemTemplate>
                        </asp:repeater>
            </table>
        </div>
        <div class="it_page" style="float: right;">
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </div>
    </div>
    <ul class="clearfix" style="text-align: center;">
        <li>
            <input type="button" onclick="javascript:SubmitSelectEmployee();" class="searchBtn bold"
                value="确定">
            <input type="button" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);"
                class="searchBtn bold" value="取消"></li>
    </ul>
</div>
