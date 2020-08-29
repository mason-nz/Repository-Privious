<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabelEdit.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.LabelEdit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<script type="text/javascript">
    //新增一行
    function addNewRow() {
        var appendStr = "<tr>"
        appendStr += "<td class='cName'><em></em><input type='text'style='width:80%;'  class='w120' ><input type='hidden'  /></td>";
        appendStr += "<td><em></em></td>";        
        appendStr += "<td>";
        appendStr += "<a href='javascript:void(0);' onclick=\"LabelSave(this)\"   class='operateafter'>保存</a> ";
        appendStr += "<a href='javascript:void(0);' onclick=\"LabelCancel(this)\"  class='operateafter'>取消</a> ";
        appendStr += "<a href='javascript:void(0);' onclick=\"showEditText(this)\"   class='operatebefore' style='display:none'>修改</a> ";
        appendStr += "<a href='javascript:void(0);' onclick=\"changeStatus(this)\" class='operatebefore' style='display:none'>停用</a>";
        appendStr += "</td>";
        appendStr += "</tr>";
        $("#trList").append(appendStr);
        $("#trList").find("tr:last-child [type='text']").focus();
    }

    //保存
    function LabelSave(obj) {
        var labelName = $.trim($(obj).parents("tr").find("td:eq(0) [type='text']").val());
        var ltid = $.trim($(obj).parents("tr").find("td:eq(0) [type='hidden']").val());

        var txtEditText = "";
        $("#trList tr").each(function () {
            var $td0 = $(this).find("td:eq(0) input[type='text']");
            if (!$td0.is(":visible")) {
                txtEditText += $.trim($td0.val()) + ",";
            }
        });

        if (labelName == "") {
            $.jAlert("请输入标签名称");
            return false;
        }
        else if (Len(labelName) > 50) {
            $.jAlert("标签名称不能超过50个字符！");
            return false;
        }

        if (txtEditText.length>2) {
            var isRepeatStr = isRepeat(labelName, txtEditText.substr(1, txtEditText.length - 2));
            if (isRepeatStr == false) {
                $.jAlert("标签名称有重复，不能保存");
                return false;
            }
        }
        

        AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", { Action: "LabelTableEdit", LTID: ltid, Name: encodeURIComponent(labelName), r: Math.random() }, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "yes") {
                var currentTR = $(obj).parent().parent();
                currentTR.find("td:eq(0)").find("[type='hidden']").val(jsonData.LTID);
                currentTR.find("td:eq(0)").find("em").text(labelName);
                
                hideEditText(obj);
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
    }

    //取消
    function LabelCancel(obj) {
        var ltid = $(obj).parent().parent().find("td:eq(0)").find("[type='hidden']").val();
        hideEditText(obj);
        if (ltid == "") {
            $(obj).parents("tr").remove();
        }

    }

    //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
    function showEditText(obj) {
        var currentTR = $(obj).parent().parent();
        //显示文本框
        currentTR.find("td:eq(0)").find("em").css("display", "none");
        currentTR.find("td:eq(0)").find("[type='text']").css("display", "");
        
        //控制操作
        currentTR.find("td:eq(2)").find(".operateafter").css("display", "");
        currentTR.find("td:eq(2)").find(".operatebefore").css("display", "none");
    }

    function changeStatus(obj) {
        var currentTR = $(obj).parent().parent();
        var ltid = currentTR.find("td:eq(0)").find("[type='hidden']").val();
        var statusText = $.trim(currentTR.find("td:eq(1)").find("em").text());
        var status = 0;
        if (statusText == "在用") {
            status = 1;
        }
        AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", { Action: "ChangLabelTableStatus", LTID: ltid, Status: status, r: Math.random() }, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "yes") {
                if (status == 1) {
                    currentTR.find("td:eq(1)").find("em").text("停用");
                    $(obj).text("启用");
                }
                else {
                    currentTR.find("td:eq(1)").find("em").text("在用");
                    $(obj).text("停用");
                }
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
    }

    function hideEditText(obj) {
        var currentTR = $(obj).parent().parent();

        //隐藏文本框
        var textObj = currentTR.find("td:eq(0)").find("[type='text']");
        currentTR.find("td:eq(0)").find("em").css("display", "");
        textObj.css("display", "none"); ;        

        //显示状态
        var emObj = currentTR.find("td:eq(1)").find("em");
        if (emObj.text() == "") {
            currentTR.find("td:eq(1)").find("em").text("在用");
        }

        //控制操作
        currentTR.find("td:eq(2)").find(".operateafter").css("display", "none");
        currentTR.find("td:eq(2)").find(".operatebefore").css("display", "");
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
        $.jAlert("此标签目前正在使用，无法进行停用操作！");
    }
</script>
</head>
<form id="form1" runat="server">
<div class="popup openwindow">
    <div class="title ft14">
        <h2>
            标签管理                     
        </h2>
        <span><a href="#" class="right" onclick="javascript:$.closePopupLayer('LabelEditPop',false);">
            <img src="../images/c_btn.png" border="0" /></a></span></div>
    <div class="content">
        <table id="trList" cellspacing="0" cellpadding="0" class="fzList">
            <tr>
                <th width="50%">
                    标签
                </th>
                <th width="15%">
                    状态
                </th>
                <th width="30%">
                    操作
                </th>
            </tr>            
            <asp:repeater id="rptLabel" runat="server">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td class="cName">
                                    <em>
                                        <%#Eval("Name")%></em>
                                    <input type="text" class="w120" value='<%#Eval("Name") %>' style="display: none;
                                        width: 80%;" />&nbsp;
                                    <input type="hidden" value='<%#Eval("LTID") %>' />
                                </td>
                                <td>
                                    <em>
                                        <%#Eval("Status").ToString()=="0"?"在用":"停用"%></em>
                                </td>                                
                                <td>
                                    <a href="javascript:void(0);" onclick="LabelSave(this)" style="display: none" class='operateafter'>
                                        保存</a> <a href="javascript:void(0);" onclick="LabelCancel(this)" style="display: none"
                                            class='operateafter'>取消</a> <a href="javascript:void(0)" onclick="showEditText(this)"
                                                class='operatebefore'>修改</a>
                                    <%--<%#Eval("Status").ToString() == "0" ? "<a href='javascript:void(0)' " + (CanStop(Eval("BGID")) > 0 ? "onclick='alertInfo()' " : " onclick='changeStatus(this)' ") + " class='operatebefore'>停用</a>" : "<a href='javascript:void(0)'  onclick='changeStatus(this)' class='operatebefore'>启用</a>"%>--%>
                                    <%#Eval("Status").ToString() == "0" ? "<a href='javascript:void(0)' onclick='changeStatus(this)' class='operatebefore'>停用</a>" : "<a href='javascript:void(0)'  onclick='changeStatus(this)' class='operatebefore'>启用</a>"%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>            
        </table>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" value="新增" onclick="addNewRow()" class="save w60" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                type="button" value="关闭" onclick="javascript:$.closePopupLayer('LabelEditPop',false);" class="cancel w60 gray" /></div>
    </div>
</div>
</form>
</html>