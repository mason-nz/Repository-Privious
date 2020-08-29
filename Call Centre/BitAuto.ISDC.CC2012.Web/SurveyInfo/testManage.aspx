<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testManage.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.testManage" %>

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
            var appendStr = "<tr class='back' onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
            appendStr += "<td><input type='text'></td>";
            appendStr += "<td>";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupSave(2,'-1',this)\" SCID='-1' Name='a_save'>保存</a>&nbsp;";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupCancel('-1',this)\">取消</a>&nbsp;";
            appendStr += "</td>";
            appendStr += "</tr>";
            $("#trList").append(appendStr);
            $("#trList").find("tr:last-child [type='text']").focus();
        }

        //点击行的某个按钮时，获取到的改行控件
        function getRowControl(othis) {
            var $td1 = $(othis).parent("tr td:eq(0)");
            var $td2 = $(othis).parent("tr td:eq(1)");

            var $em = $td1.find("em");
            var $nameText = $td1.find("input[type='text']");
            var $nameEdit = $td2.find("a[name='a_edit']");
            var $nameDelete = $td2.find("a[name='a_delete']");
            var $nameSave = $td2.find("a[name='a_save']");
            var $nameCancel = $td2.find("a[name='a_cancel']");

            return {
                Td1: $td1,
                Td2: $td2,
                Em: $em,
                NameText: $nameText,
                NameEdit: $nameEdit,
                NameDelete: $nameDelete,
                NameSave: $nameSave,
                NameCancel: $nameCancel
            }
        }

        //编辑
        function groupEdit(groupName, othis) {
            var $TR = getRowControl(othis);
            $TR.NameEdit.hide();
            $TR.NameDelete.hide();
            $TR.NameSave.show();
            $TR.NameCancel.show();
            $TR.Em.hide();
            $TR.NameText.show();
        }

        //取消
        function groupCancel(groupName, othis) {
            var $TR = getRowControl(othis);
            $TR.NameEdit.show();
            $TR.NameDelete.show();
            $TR.NameSave.hide();
            $TR.NameCancel.hide();
            $TR.Em.show();
            $TR.NameText.hide();
        }

        //保存
        function groupSave(scid, othis) {
            var $TR = getRowControl(othis);
            var categoryName = $.trim($TR.NameText.val());

            if ($.jConfirm("是否确认修改该条记录？", function (r) {
                if (r) {
                    if (!validateName(0, categoryName)) {
                        alert("分类名称不能重复");
                        return false;
                    }
                    var msg = update_one(scid, categoryName);
                    alert(msg);
                    loadList();
                }
            }));
        }

        //验证；scid：0-代表是新增时验证是否有重复；scid存在值，代表是编辑验证是否有重复
        function validateName(scid, categoryName) {
            var result = true;

            //验证

            return result;
        }

        //单条修改
        function update_one(scid, categoryName) {
            var msg = "";
            AjaxPostAsync("../AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "surveyCategoryUpdate", SCID: scid, Name: categoryName, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;
            });
            return msg;
        }

        //刷新列表
        function loadList() {
            var group = "";
            if ($("#ddlGroup").val() != "-1") {
                group = $("#ddlGroup").val();
            }
            pody = { popGroup: group, r: Math.random() };
            $("#divQueryCategoryList").load("SurveyCategoryManage.aspx #divQueryCategoryList > *", pody);
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
                        <th style="color: Black; font-weight: bold">
                            分类名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 38%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="repeaterCategoryList" runat="server">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                                    <em>
                                        <%#Eval("Name") %></em>
                                    <input type="text" value="<%#Eval("Name") %>" style="display: none" />&nbsp;
                                </td>
                                <td>
                                    <a name="a_edit" href="javascript:void(0);" onclick="groupEdit('<%#Eval("GroupName") %>',this)">
                                        编辑</a> <a name="a_delete" href="javascript:void(0);" onclick="groupDelete('<%#Eval("SCID") %>')">
                                            删除</a> <a name="a_save" href="javascript:void(0);" onclick="groupSave(2,'<%#Eval("SCID") %>',this)"
                                                scid='<%#Eval("SCID") %>' style="display: none">保存</a> <a name="a_cancel" href="javascript:void(0);"
                                                    onclick="groupCancel('<%#Eval("SCID") %>',this)" style="display: none">取消</a>
                                    &nbsp;
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
