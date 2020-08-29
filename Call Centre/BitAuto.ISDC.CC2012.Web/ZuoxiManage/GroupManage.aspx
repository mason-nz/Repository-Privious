<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupManage.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.GroupManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var OptionStr = "";
        var OptionCallDisplayStr = "";
        $(document).ready(function () {
            OptionStr = GetCurrentUserAreaReturnOptionStr();
            OptionCallDisplayStr = GetCallDisplayOptionStr();
        });

        function SearchGroup() {
            var regionId = $("#sltRegion").val();
            var status = $("[name='chkStatus']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(",");
            var index = status.indexOf(",");
            if (index > 0) {
                status = "";
            }

            var pody = 'RegionID=' + regionId + '&Status=' + status;
            $('#divList').load('../ZuoxiManage/GroupManage.aspx #divList > *', pody, function () { });
        }
        //新增一行
        function addNewRow() {
            var appendStr = "<tr class='back' onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
            appendStr += "<td><em></em><input type='text'style='width:80%;'  class='w120' ><input type='hidden'  /></td>";
            appendStr += "<td></td><td><em></em><select style='width:80%'>" + OptionStr + "</select></td>";
            appendStr += "<td><em></em><select style='width:80%'>" + OptionCallDisplayStr + "</select></td>";
            appendStr += "<td><em></em></td>";
            appendStr += "<td>";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupSave(this)\"   class='operateafter'>保存</a> ";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupCancel(this)\"  class='operateafter'>取消</a> ";
            appendStr += "<a href='javascript:void(0);' onclick=\"showEditText(this)\"   class='operatebefore' style='display:none'>修改</a> ";
            appendStr += "<a href='javascript:void(0);' onclick=\"changeStatus(this)\" class='operatebefore' style='display:none'>停用</a>";
            appendStr += "</td>";
            appendStr += "</tr>";
            $("#trList").append(appendStr);
            $("#trList").find("tr:last-child [type='text']").focus();
        }

        function GetCurrentUserAreaReturnOptionStr() {
            var optionStr = "";

            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "getcurrentuserarea" }, null, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, item) {
                        optionStr += "<option value='" + item.Value + "'>" + item.Name + "</option>";
                    });
                }
            });

            return optionStr;
        }
        function GetCallDisplayOptionStr() {
            var optionStr = "";

            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "getcalldisplay" }, null, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, item) {
                        optionStr += "<option value='" + item.CDID + "'>" + item.CallNum + "</option>";
                    });
                }
            });

            return optionStr;
        }
        //编辑
        function groupEdit(groupName, othis) {
            showEditText(othis);
        }


        //保存
        function groupSave(obj) {
            var groupName = $.trim($(obj).parents("tr").find("td:eq(0) [type='text']").val());
            var cdid = $.trim($(obj).parents("tr").find("td:eq(3)").find("select").val());
            var bgid = $.trim($(obj).parents("tr").find("td:eq(0) [type='hidden']").val());
            var areaid = $.trim($(obj).parents("tr").find("td:eq(2)").find("select").val());

            var txtEditText = "";
            $("#trList tr").each(function () {
                var $td0 = $(this).find("td:eq(0) input[type='text']");
                if (!$td0.is(":visible")) {
                    txtEditText += $.trim($td0.val()) + ",";
                }
            });

            if (groupName == "") {
                $.jAlert("请输入分组名称");
                return false;
            }
            else if (Len(groupName) > 32) {
                $.jAlert("分组名称不能超过32个字符！");
                return false;
            }

            var isRepeatStr = isRepeat(groupName, txtEditText.substr(1, txtEditText.length - 2));
            if (isRepeatStr == false) {
                $.jAlert("分组名称有重复，不能保存");
                return false;
            }
            if (areaid == "") {
                $.jAlert("请选择区域！");
                return false;
            }
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "DisposeBusinessGroup", BGID: bgid, Name: encodeURIComponent(groupName), CDID: encodeURIComponent(cdid), RegionID: areaid, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    var currentTR = $(obj).parent().parent();
                    currentTR.find("td:eq(0)").find("[type='hidden']").val(jsonData.BGID);
                    currentTR.find("td:eq(0)").find("em").text(groupName);
                    currentTR.find("td:eq(3)").find("em").text($(obj).parents("tr").find("td:eq(3)").find("select").find("option:selected").text());
                    currentTR.find("td:eq(2)").find("em").text($(obj).parents("tr").find("td:eq(2)").find("select").find("option:selected").text());
                    hideEditText(obj);
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }

        function changeStatus(obj) {
            var currentTR = $(obj).parent().parent();
            var bgid = currentTR.find("td:eq(0)").find("[type='hidden']").val();
            var statusText = $.trim(currentTR.find("td:eq(4)").find("em").text());
            var status = 0;
            if (statusText == "在用") {
                status = 1;
            }
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "ChangeBusinessGroupStatus", BGID: bgid, Status: status, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    if (status == 1) {
                        currentTR.find("td:eq(4)").find("em").text("停用");
                        $(obj).text("启用");
                    }
                    else {
                        currentTR.find("td:eq(4)").find("em").text("在用");
                        $(obj).text("停用");
                    }
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }

        //取消
        function groupCancel(obj) {
            var bgid = $(obj).parent().parent().find("td:eq(0)").find("[type='hidden']").val();
            hideEditText(obj);
            if (bgid == "") {
                $(obj).parents("tr").remove();
            }

        }

        //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
        function showEditText(obj) {
            var currentTR = $(obj).parent().parent();
            //显示文本框
            currentTR.find("td:eq(0)").find("em").css("display", "none");
            currentTR.find("td:eq(0)").find("[type='text']").css("display", "");

            //显示选择框
            var sltObj = currentTR.find("td:eq(2)").find("select").css("display", "").val($.trim(currentTR.find("td:eq(2)").find("em").attr("value"))); ;
            currentTR.find("td:eq(2)").find("em").css("display", "none");

            //显示选择框
            currentTR.find("td:eq(3)").find("select").css("display", "").val($.trim(currentTR.find("td:eq(3)").find("em").attr("value"))); ;
            currentTR.find("td:eq(3)").find("em").css("display", "none");

            //控制操作
            currentTR.find("td:eq(5)").find(".operateafter").css("display", "");
            currentTR.find("td:eq(5)").find(".operatebefore").css("display", "none");
        }

        //
        function hideEditText(obj) {
            var currentTR = $(obj).parent().parent();

            //隐藏文本框
            var textObj = currentTR.find("td:eq(0)").find("[type='text']");
            currentTR.find("td:eq(0)").find("em").css("display", "");
            textObj.css("display", "none"); ;

            //隐藏选择框
            var sltObj = currentTR.find("td:eq(2)").find("select");
            currentTR.find("td:eq(2)").find("em").css("display", "");
            sltObj.css("display", "none"); ;

            //隐藏选择框
            var sltObj2 = currentTR.find("td:eq(3)").find("select");
            currentTR.find("td:eq(3)").find("em").css("display", "");
            sltObj2.css("display", "none");

            //显示状态
            var emObj = currentTR.find("td:eq(4)").find("em");
            if (emObj.text() == "") {
                currentTR.find("td:eq(4)").find("em").text("在用");
            }

            //控制操作
            currentTR.find("td:eq(5)").find(".operateafter").css("display", "none");
            currentTR.find("td:eq(5)").find(".operatebefore").css("display", "");
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

        function alertInfo() {
            $.jAlert("此业务组目前正在使用，无法进行停用操作！");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 600px">
        <div class="title bold">
            <h2>
                CC呼叫中心分组管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ModfiyBusinessGroup',false);">
            </a></span>
        </div>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 260px;">
                    <label>
                        所属区域：
                    </label>
                    <select id="sltRegion" class="defselect" style="width: 100px;">
                        <option value="-1">请选择</option>
                        <asp:Repeater runat="server" ID="rptRegion">
                            <ItemTemplate>
                                <option value="<%#Eval("Value") %>">
                                    <%#Eval("Name")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>
                <li style="width: 160px;">
                    <label style="width: 50px;">
                        状态：
                    </label>
                    <input type="checkbox" name="chkStatus" value="0" />在用
                    <input type="checkbox" name="chkStatus" value="1" />停用 </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchGroup();" />
                </li>
            </ul>
        </div>
        <div class="Table2" id="divList" style="margin: 10px 0px 0px 20px; width: 550px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryGroup"
                width="98%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold; width: 30%">
                            分组名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            员工数量
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            所属区域
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            外显400号码
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            状态
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rptGroup" runat="server" OnItemDataBound="rptSelectBind">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                                    <em>
                                        <%#Eval("Name")%></em>
                                    <input type="text" class="w120" value='<%#Eval("Name") %>' style="display: none;
                                        width: 80%;" />&nbsp;
                                    <input type="hidden" value='<%#Eval("BGID") %>' />
                                </td>
                                <td>
                                    <%# GetUserCountByGroup(Eval("BGID").ToString())%>
                                </td>
                                <td>
                                    <em value="<%#Eval("RegionID")%>">
                                        <%#Eval("RegionID").ToString()=="1"?"北京":"西安"%></em>
                                    <select style="display: none; width: 80%;">
                                        <asp:Repeater ID="rptSelectArea" runat="server">
                                            <ItemTemplate>
                                                <option value="<%#Eval("Value")%>">
                                                    <%#Eval("Name")%></option>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </td>
                                <td>
                                    <em value="<%#Eval("CDID")%>">
                                        <%#Eval("CallNum")%></em>
                                    <select style="display: none; width: 80%;">
                                        <asp:Repeater ID="rptSelect" runat="server">
                                            <ItemTemplate>
                                                <option value="<%#Eval("CDID")%>">
                                                    <%#Eval("CallNum")%></option>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                    <%--<option value="1" <%#Eval("CDID").ToString()=="1"?"selected=true":""%>>4000716719</option>
                                        <option value="2" <%#Eval("CDID").ToString()=="2"?"selected=true":""%>>4000168168</option>--%>
                                </td>
                                <td>
                                    <em>
                                        <%#Eval("Status").ToString()=="0"?"在用":"停用"%></em>
                                </td>
                                <td>
                                    <a href="javascript:void(0);" onclick="groupSave(this)" style="display: none" class='operateafter'>
                                        保存</a> <a href="javascript:void(0);" onclick="groupCancel(this)" style="display: none"
                                            class='operateafter'>取消</a> <a href="javascript:void(0)" onclick="showEditText(this)"
                                                class='operatebefore'>修改</a>
                                    <%#Eval("Status").ToString() == "0" ? "<a href='javascript:void(0)' onclick='changeStatus(this)' class='operatebefore'>停用</a>" : "<a href='javascript:void(0)'  onclick='changeStatus(this)' class='operatebefore'>启用</a>"%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <div class="btn" style="width: auto">
                    <input type="button" value="新增" onclick="addNewRow()" class="btnSave bold" />&nbsp;&nbsp;&nbsp;
                    <input type="button" class="btnSave bold" onclick="javascript:$.closePopupLayer('ModfiyBusinessGroup',false);"
                        class="btnChoose" value="关闭页面" />
                </div>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
