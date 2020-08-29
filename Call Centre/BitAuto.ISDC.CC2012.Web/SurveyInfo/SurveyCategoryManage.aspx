<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyCategoryManage.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyCategoryManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            getUserGroup();
        });

        //加载登陆人业务组
        function getUserGroup() {
            $("#<%=ddlGroup.ClientID %>").append("<option value='-1'>请选择</option>");
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=ddlGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //新增一行
        function addNewRow() {
            if ($("[id$='ddlGroup']").val() == "-1") {

                $.jAlert("请选择所属分组");
            }
            else {
                var groupName = $.trim($("#ddlGroup option:selected").text());

                var appendStr = "<tr class='back' onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
                appendStr += "<td>" + groupName + "</td>";
                appendStr += "<td><input type='text'></td>";
                appendStr += "<td></td>";
                appendStr += "<td>";
                appendStr += "<a href='javascript:void(0);' onclick=\"groupSave(2,'-1',this)\" SCID='-1' Name='a_save'>保存</a>&nbsp;";
                appendStr += "<a href='javascript:void(0);' onclick=\"groupCancel('-1',this)\">取消</a>&nbsp;";
                appendStr += "</td>";
                appendStr += "</tr>";
                $("#trList").append(appendStr);
                $("#trList").find("tr:last-child [type='text']").focus();
            }
        }

        //编辑
        function groupEdit(groupName, othis) {
            showEditText(othis);
        }

        //全部保存
        function allGroupSave() {

            //判断是否选择业务组
            var bgid = $("#ddlGroup").val();
            if (bgid == "-1") {
                alert("请选择所属分组");
                return false;
            }

            //判断分类名称是否为空
            var txtIsNull = "";
            var txtEditCount = 0;
            var txtEditText = "";
            $("#trList tr").each(function () {
                var $td0 = $(this).find("td:eq(1) input[type='text']");

                txtEditText += $.trim($td0.val()) + ",";
                if ($td0.is(":visible")) {
                    txtEditCount = 1;
                    if ($.trim($td0.val()) == "") {
                        txtIsNull = "false";
                        $td0.focus();
                    }
                }
            });

            var isRepeatStr = isRepeat(null, txtEditText.substr(1, txtEditText.length - 2));
            if (isRepeatStr == false) {
                alert("分类名称有重复，不能保存");
                return false;
            }

            if (txtIsNull == "false") {
                alert("请输入分类名称");
                return false;
            }
            if (txtEditCount == 0) {
                alert("没有要编辑的信息")
                return false;
            }

            if ($.jConfirm("是否确认全部保存编辑的信息？", function (r) {
                if (r) {
                    $("#trList tr").each(function () {
                        var $td0 = $(this).find("td:eq(1) input[type='text']");

                        if ($td0.is(":visible")) {
                            var $td1 = $(this).find("td:eq(2) a[Name='a_save']");
                            groupSave(1, $td1.attr('SCID'), $td1);
                        }
                    });
                    $.jPopMsgLayer("操作成功", function () {
                        loadList();
                    });
                }
                else
                { loadList(); }
            }));

        }

        //删除
        function groupDelete(scid) {
            if ($.jConfirm("是否确认删除？", function (r) {
                if (r) {
                    var TypeId = '<%=TypeId %>';
                    $.post("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryUpdate", SCID: scid, Status: -1, TypeId: TypeId, r: Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.msg == "操作成功") {
                            $.jPopMsgLayer(jsonData.msg, function () {
                                loadList();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg, function () {
                                loadList();
                            });
                        }
                    });
                }
                else {
                    return false;
                }
            }));
        }

        //保存。如果scid=-1：是新增；scid!=-1：则是编辑；type=1是全部保存，type=2是单个保存
        function groupSave(type, scid, othis) {
            var categoryName = encodeURIComponent($.trim($(othis).parents("tr").find("td:eq(1) [type='text']").val()));

            var txtEditText = "";
            $("#trList tr").each(function () {
                var $td0 = $(this).find("td:eq(1) input[type='text']");
                if (!$td0.is(":visible")) {
                    txtEditText += $.trim($td0.val()) + ",";
                }
            });


            if (categoryName == "") {
                alert("请输入分类名称");
                return false;
            }
            var bgid = $("#ddlGroup").val();
            if (scid == "-1") {

                if (bgid == "-1") {
                    alert("请选择所属分组");
                    return false;
                }
                else {
                    switch (type) {
                        case 2:

                            var isRepeatStr = isRepeat(decodeURIComponent(categoryName), txtEditText.substr(1, txtEditText.length - 2));
                            if (isRepeatStr == false) {
                                alert("分类名称有重复，不能保存");
                                return false;
                            }


                            if ($.jConfirm("是否确认新增该条记录？", function (r) {
                                if (r) {
                                    var msg = add_one(bgid, categoryName);
                                    if (msg == "操作成功") {
                                        $.jPopMsgLayer(msg, function () {
                                            loadList();
                                        });
                                    }
                                    else {
                                        $.jAlert(msg, function () {
                                            loadList();
                                        });
                                    }
                                }
                                else {
                                    groupCancel(scid, othis);
                                    return false;
                                }
                            }));
                            break;
                        case 1: add_one(bgid, categoryName);
                            break;
                    }
                }

            }
            else {

                switch (type) {
                    case 2:

                        var isRepeatStr = isRepeat(decodeURIComponent(categoryName), txtEditText.substr(1, txtEditText.length - 2));
                        if (isRepeatStr == false) {
                            alert("分类名称有重复，不能保存");
                            return false;
                        }

                        if ($.jConfirm("是否确认修改该条记录？", function (r) {
                            if (r) {
                                var msg = update_one(scid, categoryName);
                                if (msg == "操作成功") {
                                    $.jPopMsgLayer(msg, function () {
                                        loadList();
                                    });
                                }
                                else {
                                    $.jAlert(msg, function () {
                                        loadList();
                                    });
                                }
                            }
                            else {
                                groupCancel(scid, othis);
                                return false;
                            }
                        }));
                        break;
                    case 1: update_one(scid, categoryName);
                        break;
                }

            }

            hideEditText(othis);
        }

        //新增
        function add_one(bgid, categoryName) {
            var msg = "";
            AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryInsert", BGID: bgid, Name: categoryName, TypeId: '<%=TypeId %>', r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;
            });
            return msg;
        }

        //修改
        function update_one(scid, categoryName) {
            var msg = "";
            AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryUpdate", SCID: scid, Name: categoryName, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;
            });
            return msg;
        }

        //取消。如果scid=-1：是新增行取消，需要移除新增行；scid!=-1：则是编辑时取消
        function groupCancel(scid, othis) {
            if (scid != "-1") {
                hideEditText(othis);
            }
            else {
                $(othis).parents("tr").remove();
            }
        }

        //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
        function showEditText(othis) {

            var $td = $(othis).parent().parent()
                            .find("td:eq(1)");

            // 1
            $td.find("em").hide().end()
                        .find("input[type='text']").show().focus();

            // 2 
            $(othis).parent().find("a:eq(0)").hide().end()
                .find("a:eq(1)").hide().end()
                .find("a:eq(2)").show().end()
                .find("a:eq(3)").show();
        }

        //1 显示文本，隐藏编辑框；2 显示编辑、删除按钮，隐藏保存、取消按钮
        function hideEditText(othis) {

            var $td = $(othis).parent().parent()
                            .find("td:eq(1)");

            //记录文本，编辑时不会出现文本不一致情况
            var emStr = $td.find("em").html();
            $td.find("input[type='text']").val($.trim(emStr));

            // 1
            $td.find("em").show().end()
                            .find("input[type='text']").hide();

            // 2 
            $(othis).parent().find("a:eq(0)").show().end()
                .find("a:eq(1)").show().end()
                .find("a:eq(2)").hide().end()
                .find("a:eq(3)").hide();
        }

        function changeCategoryStatus(scid, obj) {
            AjaxPostAsync("/AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "UpdateSurveyCategoryStatus", SCID: scid, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg == "success") {
                    var tdStatus = $(obj).parent().parent().find("td:eq(2)");
                    var dtStatusText = $.trim(tdStatus.text());
                    if (dtStatusText == "在用") {
                        tdStatus.text("停用");
                        $(obj).text("启用");
                    }
                    else {
                        tdStatus.text("在用");
                        $(obj).text("停用");
                    }
                }
            });
        }

        //刷新列表
        function loadList() {
            var group = "";
            if ($("#ddlGroup").val() != "-1") {
                group = $("#ddlGroup").val();
            }

            var TypeId = '<%=TypeId %>';

            pody = { popGroup: group, TypeId: TypeId, r: Math.random() };
            $("#divQueryCategoryList").load("/SurveyInfo/SurveyCategoryManage.aspx #divQueryCategoryList > *", pody);
        }

        //判断是否重复(arrayStr-逗号分隔的字符串;compareStr为null时，arrayStr自己比较，不为null时，compareStr在arrayStr是否有重复)
        function isRepeat(compareStr, arrayStr) {
            var msg = "true";

            if ($.trim(arrayStr) != "") {
                var array_str = $.trim(arrayStr).split(',');
                for (var i = 0; i < array_str.length; i++) {
                    if (compareStr == null) {
                        for (var j = i + 1; j < array_str.length; j++) {
                            if (array_str[i] == array_str[j]) {
                                msg = "false";
                                return false;
                            }
                        }
                    }
                    else {
                        if ($.trim(compareStr) == array_str[i]) {
                            msg = "false";
                            return false;
                        }
                    }

                }
            }
            return msg;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 500px">
        <div class="title bold">
            <h2>
                分类管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('addCategory',false);">
            </a></span>
        </div>
        <ul class="clearfix ft14" id="addTemplatePage">
            <li style="width: 259px">
                <label>
                    所属分组：</label>
                <select id="ddlGroup" onchange="loadList()" class="w125" style="width: 128px" runat="server">
                </select>
            </li>
            <li class="btnsearch" style="width: 100px">
                <input type="button" value="新增" onclick="addNewRow()" />&nbsp;&nbsp;&nbsp; </li>
        </ul>
        <div class="Table2" id="divQueryCategoryList" style="margin-left: 20px; width: 464px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryCategory"
                width="98%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold; width: 30%">
                            所属分组
                        </th>
                        <th style="color: Black; font-weight: bold; width: 35%">
                            分类名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            状态
                        </th>
                        <th style="color: Black; font-weight: bold; width: 35%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="repeaterCategoryList" runat="server">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                                    <em>
                                        <%#Eval("GroupName")%></em>
                                </td>
                                <td>
                                    <em>
                                        <%#Eval("Name") %></em>
                                    <input type="text" value="<%#Eval("Name") %>" style="display: none" />&nbsp;
                                </td>
                                <td>
                                    <%#(Eval("Status").ToString()=="0" ||Eval("Status").ToString()=="-3")?"在用":"停用" %>
                                </td>
                                <td>
                                    <%#Eval("Status").ToString() == "-3" ? "" : "<a href=javascript:void(0); onclick=groupEdit('"+Eval("GroupName").ToString()+"',this)>编辑</a>"%>
                                    <%#Eval("Status").ToString() == "-3" ? "" : "<a href=javascript:void(0); onclick=groupDelete("+Eval("SCID").ToString()+")>删除</a>"%>
                                    <a href="javascript:void(0);" onclick="groupSave(2,'<%#Eval("SCID") %>',this)" scid='<%#Eval("SCID") %>'
                                        style="display: none" name='a_save'>保存</a> <a href="javascript:void(0);" onclick="groupCancel('<%#Eval("SCID") %>',this)"
                                            style="display: none">取消</a>
                                    <%#Eval("Status").ToString() == "0" ? "<a href=javascript:void(0); onclick=changeCategoryStatus(" + Eval("SCID").ToString() + ",this)>停用</a>" : ""%>
                                    <%#Eval("Status").ToString() == "1" ? "<a href=javascript:void(0); onclick=changeCategoryStatus(" + Eval("SCID").ToString() + ",this)>启用</a>" : ""%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <input type="button" onclick="allGroupSave();" class="btnChoose" value="全部保存" style="margin-right: 18px" />
                <input type="button" onclick="javascript:$.closePopupLayer('addCategory',false);"
                    class="btnChoose" value="关闭页面" /></li>
        </ul>
    </div>
    </form>
</body>
</html>
