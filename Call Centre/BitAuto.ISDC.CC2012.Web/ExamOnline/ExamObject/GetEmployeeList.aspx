<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetEmployeeList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamObject.GetEmployeeList" %>

<script type="text/javascript">
    $(document).ready(function () {
        enterSearch(SearchEm1);
    });
    //查询员工信息
    function SearchEm1() {
        LoadingAnimation('divQueryEmployeeList');
        var txtQueryEmployeeCnName = $('#txtQueryEmployeeCnName'); //请输入姓名进行查询
        var EmName = "";
        if ($.trim(txtQueryEmployeeCnName.val()) == '请输入姓名进行查询') {
            txtQueryEmployeeCnName.val('');
            EmName = "";
        }
        else {
            EmName = escape(txtQueryEmployeeCnName.val());
        }
        var pody = 'EmName=' + EmName + '&page=1&GroupID=' + escape($("#EGroup").val()) + "&redom=" + Math.random();
        $('#divQueryEmployeeList').load('/ExamOnline/ExamObject/GetEmployeeList.aspx #divQueryEmployeeList > *', pody);
    }

    function SelectEmployeeAppendToTable(eid, name, number, departname) {
        var htmlStr = "<tr id='tr_" + eid + "'>"
                            + "<td><a href='javascript:delSelectedEmployee(" + eid + ")'>删除</a></td>"
                            + "<td>" + number + "</td>"
                            + "<td><input id='hdn_" + eid + "' type='hidden' value=" + eid + " />" + name + "</td>"
                            + "<td>" + departname + "</td></tr>";
        $("#SelectedEmployees").append(htmlStr);
    }

    //移除所选
    function delSelectedEmployee(eid) {
        $("#tr_" + eid).remove();
    }

    //提交选择的员工
    function SubmitSelectEmployee() {
        var selectedEm = $('#tableCustBrandSelect tbody tr');
        var length = selectedEm.length;
        var names = "";
        var marks = "";
        if (length > 1) {
            for (var i = 1; i < length; i++) {
                names += $.trim(selectedEm.eq(i).find("td").eq(1).text()) + ",";
                marks += $.trim(selectedEm.eq(i).find("td").eq(2).find("span").text()) + ",";
            }
            names = names.substring(0, names.length - 1);
            marks = marks.substring(0, marks.length - 1);

            $("#PersonsGetEmail").html(names);
            $("#PersonsGetEmailIN").val(marks);

            $.closePopupLayer('SelectEmployeePopup', true, names + ";" + marks);
        }
        else {
            $.closePopupLayer('SelectEmployeePopup', true, ";");
        }
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
            $.jAlert('您已经添加过：' + name + '了！');
            return;
        }
        var trhtml = '<tr class="back" onmouseout="this.className=\'back\'" onmouseover="this.className=\'hover\'\"><td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\''
         + name + '\' id=\'' + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td>' + parentNodes.eq(1).html() + '</td>'
               + '<td>' + parentNodes.eq(2).html() + '</td>'
               + '<td>' + parentNodes.eq(3).html() + '</td>'
               + '<td style="padding-right:10px;">' + parentNodes.eq(4).html() + '</td></tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');
    }
    //清空
    function ClearEmployee() {
        $("input[name=QueryByCnName]").removeAttr('checked');
    }

    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation('divQueryEmployeeList');
        $('#divQueryEmployeeList').load('/ExamOnline/ExamObject/GetEmployeeList.aspx #divQueryEmployeeList > *', pody);
    }

    function selectAll() {
        var Persions = $("#tableQueryEmployee tr");
        var Persions_length = Persions.length;
        for (var i = 1; i < Persions_length; i++) {
            var PersionID = Persions.eq(i).find("td").eq(0).find("a").attr("id");
            var PersionRol = $.trim(Persions.eq(i).find("td").eq(1).html());
            SelectCustBrand(PersionID, PersionRol);
        }
    }
</script>
<div class="pop pb15 openwindow" style="background: #FFF;">
    <div class="title bold">
        <h2>
            选择人员</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('SelectEmployeePopup',false);">
    </div>
    <!--搜索条件-->
    <div id='divQueryByEmployee'>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 180px;">员工姓名：
                    <input type="text" value="请输入姓名进行查询" onfocus="javascript:var name=document.getElementById('txtQueryEmployeeCnName');if(name.value=='请输入姓名进行查询')name.value='';"
                        name="txtQueryEmployeeCnName" id="txtQueryEmployeeCnName" class="w190" style="width: 110px;" />
                </li>
                <li style="margin-right: 0; width: 200px;">所属分组： <span>
                    <select id="EGroup" style='width: 120px;'>
                        <option value="-1">请选择</option>
                        <asp:repeater runat="server" id="Rpt_Group">
                            <ItemTemplate>
                                <option value="<%#Eval("BGID") %>"><%#Eval("Name") %></option>
                            </ItemTemplate>
                        </asp:repeater>
                    </select>
                </span></li>
                <li class="btn" style="margin-top: 5px;">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchEm1();" />
                </li>
            </ul>
        </div>
        <!--备选列表-->
        <div id="divQueryEmployeeList">
            <div class="Table2" id="EmList" style="clear: both">
                <table width="100%" cellspacing="0" cellpadding="0" id="tableQueryEmployee" class="tableList mt10 mb15">
                    <tr>
                        <th style="width: 10%;">
                            选择
                        </th>
                        <th style="width: 15%;">
                            姓名
                        </th>
                        <th style="width: 15%;">
                            工号
                        </th>
                        <th style="width: 30%;">
                            角色
                        </th>
                        <th style="width: 30%;">
                            所属分组
                        </th>
                    </tr>
                    <tbody>
                        <asp:repeater id="RptEm" runat="server">
                <ItemTemplate>
                <tr>
                    <td>
                        <a class="linkBlue" onclick="SelectCustBrand('<%# Eval("UserID") %>','<%#Eval("trueName").ToString()%>');" 
                        name='<%# getRolesByUserID(Eval("UserID").ToString())%>' 
                        id='<%# Eval("UserID") %>'  
                        style=" cursor:pointer;">选择</a>
                    </td>
                    <td>
                        <%#Eval("trueName").ToString()%>
                    </td>
                    <td>
                        <span style=" display:none;"><%# Eval("UserID")%></span> <label><%# Eval("AgentNum")%></label>&nbsp;
                    </td>   
                    <td>
                        <%# getRolesByUserID(Eval("UserID").ToString())%>&nbsp;
                    </td>                
                    <td>
                        <%#Eval("BGName").ToString()%>&nbsp;
                    </td>
                  </tr>                  
                </ItemTemplate>
                </asp:repeater>
                    </tbody>
                </table>
            </div>
            <div class="it_page" style="text-align: right;">
                <a href="javascript:void(0)" onclick="selectAll()" style="cursor: pointer; float: left;
                    color: #0088CC; padding-left: 10px;">选择当前人员</a>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                &nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <br />
    <div style="clear: both;">
    </div>
    <!--已选人员列表-->
    <div class="Table2" id="divCustBrandSelect" style="display: block;">
        <table cellspacing="0" cellpadding="0" border="0" class="tableList mt10 mb15" id="tableCustBrandSelect"
            style="margin-left: 0px; width: 100%;">
            <tbody>
                <tr>
                    <th style="width: 10%;">
                        选择
                    </th>
                    <th style="width: 15%;">
                        姓名
                    </th>
                    <th style="width: 15%;">
                        工号
                    </th>
                    <th style="width: 30%;">
                        角色
                    </th>
                    <th style="width: 30%;">
                        所属分组
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
