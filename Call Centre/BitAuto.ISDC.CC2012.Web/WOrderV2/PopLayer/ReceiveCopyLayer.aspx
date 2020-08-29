<%@ Page Title="接收人/抄送人" Language="C#" AutoEventWireup="true" CodeBehind="ReceiveCopyLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.ReceiveCopyLayer" %>

<!--选择人员-->
<div class="pop_new w600 openwindow">
    <div class="title bold">
        <h2>
            选择人员<span><a hidefocus="true" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelReceiveCopyUserAjaxPopup', false);"></a></span>
        </h2>
    </div>
    <div class="search search_ry">
        <ul>
            <li class="name_xm">
                <label style="*padding-top: 3px;">
                    员工姓名：</label><span><input style="padding-bottom: 5px; padding-top: 3px" id="txtQueryEmployeeCnName"
                        name="" type="text" class="w120" /></span> </li>
            <li><span class="cx_btn">
                <input name="" type="button" value="查询" onclick="javascript:SearchEm1();" /></span>
                <span style="position: relative; *top: -4px;"><a onclick="javascript:ClearChoosedItem();"
                    href="javascript:void(0)">清空已选项</a></span> </li>
        </ul>
    </div>
    <div class="clearfix">
    </div>
    <div id="divQueryEmployeeList">
        <table id="tableQueryEmployee" border="0" cellpadding="0" cellspacing="0" class="bq_list">
            <tr>
                <th width="20%">
                    选择
                </th>
                <th width="40%">
                    姓名
                </th>
                <th width="40%">
                    部门名称
                </th>
            </tr>
            <asp:repeater id="RptEm" runat="server">
                <ItemTemplate>
                <tr>
                    <td>
                        <a class="linkBlue" name='<%# Eval("UserCode")%>' onclick="SelectCustToReceiveCopy(this,'<%# Eval("UserID") %>','<%#Eval("trueName")%>');" 
                        id='<%# Eval("UserID") %>'  
                        style=" cursor:pointer;">选择</a>
                    </td>
                    <td>
                        <%#Eval("trueName")%>（<%# Eval("ADName")%>）&nbsp;
                    </td>           
                    <td>
                        <%#Eval("NamePath")%>&nbsp;
                    </td>
                  </tr>                  
                </ItemTemplate>
                </asp:repeater>
        </table>
        <!--分页开始-->
        <div class="pageTurn mr10" style="margin-right: 20px;">
            <p>
                <span class="curren_ry_xz" style="float: left; margin-left: 22px;"><a href="javascript:void(0)"
                    onclick="selectAll()">选择当前人员</a></span>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div>
        <!--分页开始-->
    </div>
    <div class="clearfix">
    </div>
    <div>
        <table id="tableSelectedEmployee" border="0" cellpadding="0" cellspacing="0" class="bq_list"
            style="margin: 20px auto;">
            <tr>
                <th width="20%">
                    选择
                </th>
                <th width="40%">
                    姓名
                </th>
                <th width="40%">
                    部门名称
                </th>
            </tr>
            <%=SelectedUserStr%>
        </table>
    </div>
    <div class="clearfix">
    </div>
    <div class="option_button btn">
        <input name="" type="button" value="确定" onclick="javascript:SubmitSelectedEmployee();" />
    </div>
    <div class="clearfix">
    </div>
</div>
<!--选择人员-->
<script type="text/javascript">
    //分页操作
    function ShowDataByPost100(pody) {
        LoadingAnimation("divQueryEmployeeList");
        $('#divQueryEmployeeList').load('/WOrderV2/PopLayer/ReceiveCopyLayer.aspx #divQueryEmployeeList > *', pody, function () {
            formatDepartName();
        });
    }
    function formatDepartName() {
        var persons = $("#tableQueryEmployee tr");
        var personLength = persons.length;
        if (personLength > 1) {
            for (var i = 1; i < personLength; i++) {
                var personObj = persons.eq(i).find("td");
                var departName = $.trim(personObj.eq(2).text());
                var fistIndex = departName.indexOf("-");
                if (fistIndex > 0) {
                    personObj.eq(2).text(departName.substring(0, fistIndex));
                }
            }
        }
        //已选择人员的部门名称格式化
        persons = $("#tableSelectedEmployee tr");
        personLength = persons.length;
        if (personLength > 1) {
            for (var i = 1; i < personLength; i++) {
                var personObj = persons.eq(i).find("td");
                var departName = $.trim(personObj.eq(2).text());
                var fistIndex = departName.indexOf("-");
                if (fistIndex > 0) {
                    personObj.eq(2).text(departName.substring(0, fistIndex));
                }
            }
        }
    }
    $(document).ready(function () {
        enterSearch(SearchEm1);
        formatDepartName();
    });
    //查询员工信息
    function SearchEm1() {
        LoadingAnimation('divQueryEmployeeList');
        var txtQueryEmployeeCnName = $('#txtQueryEmployeeCnName');
        var EmName = "";
        if ($.trim(txtQueryEmployeeCnName.val()) == '请输入姓名进行查询') {
            txtQueryEmployeeCnName.val('');
            EmName = "";
        }
        else {
            EmName = escape(txtQueryEmployeeCnName.val());
        }
        var pody = 'EmName=' + EmName + '&page=1&redom=' + Math.random();
        $('#divQueryEmployeeList').load('/WOrderV2/PopLayer/ReceiveCopyLayer.aspx #divQueryEmployeeList > *', pody, function () {
            formatDepartName();
        });
    }
    //选择操作
    function SelectCustToReceiveCopy(thisObj, userId, name) {
        var selectedEm = $('#tableSelectedEmployee tr');
        var selectedCount = selectedEm.length;
        var limitCount = parseInt("<%=LimitSelectCount%>") == "NaN" ? 0 : parseInt("<%=LimitSelectCount%>");

        if (limitCount > 0 && selectedCount > limitCount) {
            $.jAlert('本次人员选择限定最多选择' + limitCount + '个！');
            return;
        }

        var idsame = $('#tableSelectedEmployee tr').find('a[id="' + userId + '"]');
        if (idsame.size() > 0) {
            //$.jAlert('您已经添加过：' + name + '了！');
            return;
        }
        //td列表
        var parentNodes = $(thisObj).parent().parent().children().clone();
        var trhtml = '<tr><td><a href="javascript:DelSelectCustBrand(\'' + userId + '\');" employeenum=' + parentNodes.eq(0).find("a").attr("name") + ' name="'
            + name + '" id="' + userId + '"><img src="/Images/close.png" title="删除"/></a></td>'
            + '<td>' + parentNodes.eq(1).html() + '</td>' //姓名
            + '<td>' + parentNodes.eq(2).html() + '</td></tr>'; //部门
        $('#tableSelectedEmployee tr:last').after(trhtml);
    }
    //删除操作
    function DelSelectCustBrand(userId) {
        $("#tableSelectedEmployee a[id='" + userId + "'][href]").parent().parent().remove();
    }
    //选择当前页所有人员
    function selectAll() {
        var person = $("#tableQueryEmployee tr");
        var personLength = person.length;
        if (personLength > 1) {
            for (var i = 1; i < personLength; i++) {
                var personObj = person.eq(i).find("td");
                var personId = $.trim(personObj.eq(0).find("a").attr("id"));
                var personName = $.trim(personObj.eq(1).text());
                SelectCustToReceiveCopy(personObj.eq(0).find("a").get(0), personId, personName);
            }
        }
    }
    //提交选择的员工
    function SubmitSelectedEmployee() {
        var selectedEm = $('#tableSelectedEmployee tr');
        var length = selectedEm.length;

        if (length > 1) {
            var names = "";
            var ids = "";
            var userIDuserCodeJson = "";
            for (var i = 1; i < length; i++) {
                var name = $.trim(selectedEm.eq(i).find("td").eq(1).text());
                name = name.split('（')[0];
                names += name + ",";
                ids += $.trim(selectedEm.eq(i).find("a").attr("id")) + ",";
                userIDuserCodeJson += ",{'UserID':'" + $.trim(selectedEm.eq(i).find("a").attr("id"))
                                    + "','UserNum':'" + $.trim(selectedEm.eq(i).find("a").attr("employeenum"))
                                    + "','UserName':'" + name + "'}"; ;
            }
            if (userIDuserCodeJson.length > 0) {
                userIDuserCodeJson = "[" + userIDuserCodeJson.substring(1) + "]";
            }
            else {
                userIDuserCodeJson = "";
            }
            names = names.substring(0, names.length - 1);
            ids = ids.substring(0, ids.length - 1);
            $.closePopupLayer('SelReceiveCopyUserAjaxPopup', true, { Names: names, Ids: ids, UserIDuserCodeJson: userIDuserCodeJson });
        }
        else {
            $.closePopupLayer('SelReceiveCopyUserAjaxPopup', true, { Names: "", Ids: "", UserIDuserCodeJson: "" });
        }
    }
    function ClearChoosedItem() {
        $.closePopupLayer('SelReceiveCopyUserAjaxPopup', true, { Names: "", Ids: "", UserIDuserCodeJson: "" });
    }
</script>
