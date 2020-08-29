<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCallConfig.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.PopPanel.AutoCallConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <link href="../../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    
    
    --%>
    <script type="text/javascript">
        var pid = '<%=PID %>';
        var pn = '<%=PName %>';
        var Skgid = '<%=Skgid %>';
        var Cdid = '<%=Cdid %>';
        var selBusinessType = '<%=selBusinessType.ClientID %>';

        $(function () {
            SelectListInit();
            if (pid != '') {
                $('#autoCallProName').attr("did", pid).val(pn);
            }
            if (Cdid != '') {
                //$('#selBusinessType').val(Cdid);
                $('#' + selBusinessType).val(Cdid);
                // $('#selBusinessType option[mutilid=' + Cdid + ']').attr("selected", "selected");
            }
            if (Skgid != '') {
                $('#selSKG').val(Skgid);
            }

        });
        function SelectListInit() {

        }

        function OpenProjectSelectAutoCall(parameters) {
            if (pid != '') {
                return false;
            }

            $.openPopupLayer({
                name: "OpenProjectSelectAutoCall",
                parameters: { TypeId: "2" },
                url: "/ProjectManage/PopPanel/SelectProjectPop.aspx",
                beforeClose: function (e, data) {
                    if (data && data.dids != null) {
                        $('#autoCallProName').attr("did", data.dids).val(data.dNames);
                    }
                }
            });
        }

        function AddNewAutoCallProjects() {
            var projectids = $('#autoCallProName').attr('did');
            if (projectids == null || projectids == "") {
                alert("外呼项目不能为空...");
                return false;
            }

            var cdid = $('#' + selBusinessType).val(); //$('#selBusinessType option:selected').attr("mutilid");
            if (cdid == null || cdid == "-1") {
                alert("请选择400号码...");
                return false;
            }

            var skillgid = $('#selSKG').val();
            if (skillgid == null || skillgid == "-1") {
                alert("请选择特定技能组...");
                return false;
            }
            if (pn == "") {
                AjaxPostAsync("/ProjectManage/PopPanel/AutoCallProjectManager.ashx", { action: "AddAutoCallProjcts", projectids: projectids, cdid: cdid, skid: skillgid, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (typeof jsonData == "object") {
                        if (jsonData.result == 0) {
                            $.jPopMsgLayer("添加成功", function () {
                                $.closePopupLayer('AddAutoCallProject', true);
                            });
                        } else if (jsonData.result == -1) {
                            alert(jsonData.msg);
                        } else {
                            alert(data);
                        }
                    } else {
                        alert(data);
                    }
                });
            } else {
                AjaxPostAsync("/ProjectManage/PopPanel/AutoCallProjectManager.ashx", { action: "editP", projectids: projectids, cdid: cdid, skid: skillgid, r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (typeof jsonData == "object") {
                        if (jsonData.result == 0) {
                            $.jPopMsgLayer("修改成功", function () {
                                $.closePopupLayer('AddAutoCallProject', true);
                            });
                        } else if (jsonData.result == -1) {
                            alert(jsonData.msg);
                        } else {
                            alert(data);
                        }
                    } else {
                        alert(data);
                    }
                });
            }

        }


        function ModifyProjecty(parameters) {

        }


    </script>
    <style type="text/css">
        .spanSelect select
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <div class="w980">
        <div class="title_12" style="height: 42px; line-height: 40px;">
            设置外呼任务
        </div>
        <div class="content" style="padding: 2px 20px;">
            <div class="titles bd ft14">
                设置外呼任务</div>
            <div class="lineS">
            </div>
            <table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
                <tr>
                    <th width="15%">
                        <span class="redColor">*</span>外呼项目：
                    </th>
                    <td width="30%">
                        <span>
                            <input type="text" value="" class="w180" style="width: 215px;" readonly="readonly"
                                id="autoCallProName" /></span>
                        <input type="button" value="选择" onclick="javascript:OpenProjectSelectAutoCall();" />
                    </td>
                    <th width="15%">
                        <span class="redColor">*</span>显示400号码：
                    </th>
                    <td width="30%">
                        <span class="spanSelect">
                            <asp:Literal runat="server" ID="selBusinessType"></asp:Literal>
                            <%-- <select id="selBusinessType" style="width: 261px; padding: 0 2px; height: 22px; line-height: 22px;
                                border: 1px solid #ccc;">
                                <option value="-1" mutilid="-1">请选择</option>
                            </select>--%>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th>
                        <span class="redColor">*</span>指定技能组：
                    </th>
                    <td>
                        <span class="spanSelect">
                            <asp:Literal runat="server" ID="skg"></asp:Literal>
                        </span>
                    </td>
                    <th>
                    </th>
                    <td>
                        <span>
                            <%--<div class="left coupon-box02">
                            </div>--%>
                        </span>
                    </td>
                </tr>
            </table>
            <div class="btn">
                <input type="button" value="提交" class="subtim" onclick="javascript:AddNewAutoCallProjects();" />
                <input type="button" value="取消" class="btnattach" onclick="javascript:$.closePopupLayer('AddAutoCallProject',false);" />
            </div>
        </div>
    </div>
</body>
</html>
