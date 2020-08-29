<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetEmployeeList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.GetEmployeeList" %>

<script type="text/javascript">

    //根据真实姓名，查询员工信息
    function SearchByCnName() {
        var txtQueryEmployeeCnName = $('#txtQueryEmployeeCnName'); //请输入姓名或员工编号进行查询
        if ($.trim(txtQueryEmployeeCnName.val()) == '请输入姓名或员工编号进行查询')
        { txtQueryEmployeeCnName.val(''); }
        var pody = 'CnName=' + escape(txtQueryEmployeeCnName.val()) +
                   '&page=1&SearchByCnName=yes';
        $('#divQueryEmployeeList').load('../AjaxServers/GetEmployeeList.aspx #divQueryEmployeeList > *', pody, LoadDivSuccessByEmployee);
    }

    //查询之后，回调函数
    function LoadDivSuccessByEmployee(data) {
        $('#tableQueryEmployee tr:even').addClass('bg'); //设置列表行样式
    }
    function ShowDataByPost3(pody) {
        $('#divQueryEmployeeList').load('GetEmployeeList.aspx #divQueryEmployeeList > *', pody, LoadDivSuccessByEmployee);
    }
    function SelectEmployeeAppendToTable(eid, name, number, departname) {
        var htmlStr = "<tr id='tr_" + eid + "'>"
                            + "<td><a href='javascript:delSelectedEmployee(" + eid + ")'>删除</a></td>"
                            + "<td style=' display:none;'>" + number + "</td>"
                            + "<td><input id='hdn_" + eid + "' type='hidden' value=" + eid + " />" + name + "</td>"
                            + "<td>" + departname + "</td></tr>";
        $("#SelectedEmployees").append(htmlStr);
    }
    function delSelectedEmployee(eid) {
        $("#tr_" + eid).remove();
    }
    //提交选择的员工
    function SubmitSelectEmployee() {
        var selectedEm = $('#tableCustBrandSelect tbody tr');
        var length = selectedEm.length;
        var names = "";
        var marks = "";
        //        if (length > 1) {
        for (var i = 1; i < length; i++) {
            names += $.trim(selectedEm.eq(i).find("td").eq(1).text()) + ",";

            marks += $.trim(selectedEm.eq(i).find("td").eq(2).find("label").text()) + ",";
        }
        names = names.substring(0, names.length - 1);
        marks = marks.substring(0, marks.length - 1);
        $("#PersonsGetEmail").html(names);
        $("#PersonsGetEmailIN").val(marks);

        $.closePopupLayer('SelectEmployeePopup', false);
        //        }
        //        else {
        //            $.jAlert('请至少选择一个员工！');
        //        }
    }
    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }
    //删除操作
    function DelSelectCustBrand(brandid) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustBrandSelect');
    }
    //选择操作
    function SelectCustBrand(brandID, name) {
        var idsame = $('#tableCustBrandSelect tbody tr').find('a[id="' + brandID + '"]');
        var parentNodes = $("a[id='" + brandID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(2).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过：' + name + '这条记录了！');
            return;
        }
        var trhtml = '<tr class="back" onmouseout="this.className=\'back\'" onmouseover="this.className=\'hover\'\"><td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td>' + parentNodes.eq(1).html() + '</td>'
               + '<td style=" display:none;">' + parentNodes.eq(2).html() + '</td>'
               + '<td>' + parentNodes.eq(3).html() + '</tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');
    }
    //清空
    function ClearEmployee() {
        $("input[name=QueryByCnName]").removeAttr('checked');
    }
</script>
<div class="pop pb15 openwindow" style="background: #FFF;">
    <div class="title bold">
        <h2>
            模板定制</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);">
    </div>
    <div id='divQueryByEmployee'>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 260px;">员工姓名：
                    <input type="text" value="请输入姓名进行查询" onfocus="javascript:var name=document.getElementById('txtQueryEmployeeCnName');if(name.value=='请输入姓名进行查询')name.value='';"
                        name="txtQueryEmployeeCnName" id="txtQueryEmployeeCnName" class="w190" />
                </li>
                <%--<li style=" width:auto">
                    <label>角色：</label>
                    <select id="Erole">
                    <option value="-1">请选择</option>
                    <asp:Repeater runat="server" ID="Rpt_Role">
                        <ItemTemplate>
                            <option value="<%#Eval("RoleID") %>"><%#Eval("RoleName") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                    </select>
                    </li>--%>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchByCnName();" />
                </li>
            </ul>
        </div>
        <div id="divQueryEmployeeList">
            <div class="Table2">
                <table width="98%" cellspacing="0" cellpadding="0" id="tableQueryEmployee" class="Table2List">
                    <tr>
                        <th style="width: 8%;">
                            选择
                        </th>
                        <th style="width: 35%;">
                            姓名
                        </th>
                        <th style="width: 10%; display: none;">
                            编号
                        </th>
                        <th style="width: 52%;">
                            部门
                        </th>
                    </tr>
                    <tbody>
                        <asp:repeater id="repeaterEmployeeList" runat="server">
                    <ItemTemplate>
                    <tr>
                        <td>
                            <a class="linkBlue" href="javascript:void(0)" onclick="SelectCustBrand('<%# Eval("EmployeeNumber") %>','<%# Eval("CnName")%>');" name='<%# Eval("CnName")%>' id='<%# Eval("EmployeeNumber") %>'>选择</a>
                        </td>
                        <td>
                            <%# Eval("CnName") %>
                        </td>
                        <td style=" display:none;">
                            <span style=" display:none;"><%# Eval("EmployeeNumber") %></span><label><%# Eval("EmployeeID") %></label>
                        </td>                        
                        <td value='<%# GetDepartmentFullName(Eval("EmployeeNumber").ToString()) %>'>
                            <%# Eval("Department.FullPath")%>
                        </td>
                    </tr>
                    </ItemTemplate>
                    </asp:repeater>
                    </tbody>
                </table>
            </div>
            <div class="it_page" style="text-align: right;">
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </div>
        </div>
    </div>
    <br />
    <div style="clear: both;">
    </div>
    <div class="Table2List mt10 mb15" id="divCustBrandSelect" style="display: block;">
        <table cellspacing="0" cellpadding="0" class="Table2List" id="tableCustBrandSelect"
            style="width: 98%;">
            <tbody>
                <tr>
                    <th style="width: 8%;">
                        选择
                    </th>
                    <th style="width: 35%;">
                        姓名
                    </th>
                    <th style="width: 10%; display: none;">
                        编号
                    </th>
                    <th style="width: 52%;">
                        部门
                    </th>
                </tr>
                <%=LoadSelectedEmployees()%>
            </tbody>
        </table>
        <div style="clear: both;">
        </div>
    </div>
    <div class="btn" style="margin: 10px 0 10px 0">
        <input type="button" onclick="javascript:SubmitSelectEmployee();" class="btnSave bold"
            value="确定">
    </div>
</div>
