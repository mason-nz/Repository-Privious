<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRoleInfo.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.EditRoleInfo" %>

<script type="text/javascript">
    //提交编辑的数据
    function EditRoleInfoPostPaly(RoleID) {
        var EditRoleName = $('#txtEditRoleName').val();
        //var dllEditSysID = $('#dllEditSysID').val();
        var EditIntro = $('#txtEditIntro').val();
        //var ddlRoleType = $('#ddlRoleType').val();
        if ($.trim(EditRoleName) == '') {
            alert('请填写角色名称！');
            return;
        }
        //        if(dllEditSysID == '-1')
        //        {
        //            alert('请选择所属系统！');
        //            return;
        //        }
        var url = "/AjaxServers/SysManager/RoleInfoManager.ashx";
        var postBody = "<%=ShowType %>=yes&RoleID=" + RoleID
                                    + "&RoleName=" + escape(EditRoleName)
                                    + "&SysID=<%=SysID %>"
                                    //+ "&RoleType=" + ddlRoleType
                                    + "&Intro=" + escape(EditIntro)
        AjaxPost(url, postBody, null, function (data) {
            var s = $.evalJSON(data);
            if ('<%=ShowType %>' == 'updata') {
                if (s.updata == 'yes') {
                    Close('EditRoleInfo', true);
                    alert('编辑成功！');
                    return;
                }
                else {
                    alert('编辑失败！');
                    return;
                }
            }
            if ('<%=ShowType %>' == 'add') {
                if (s.add == 'yes') {
                    Close('EditRoleInfo', true);
                    alert('添加成功！');
                    return;
                }
                else {
                    alert('添加失败！');
                    return;
                }
            }
        });
    }
</script>

<form id="form1" runat="server">
<div>
    <div class="openwindow">
        <div class="close">
            <a href="javascript:Close('EditRoleInfo');">关闭</a></div>
        <h2>
            <span>角色<asp:literal runat="server" id="litTitle"></asp:literal></span></h2>
        <fieldset>
            <ol>
                <li class="nowidth">
                    <label>
                        角色名称：</label>
                    <input type="hidden" id="hiddenRoleID" />
                    <input type="text" name="txtEditRoleName" runat="server" onblur="this.value=this.value.replace(/[']+/img,'');"
                        id="txtEditRoleName" maxlength="40" /><font color="red">*</font></li>
                <%--<li class="nowidth">
                    <label>
                        角色类型：</label>
                    <select id="ddlRoleType" runat="server">
                        <option value="0">业务角色</option>
                        <option value="1">系统角色</option>
                    </select>
                </li>--%>
                <li class="nowidth">
                    <label>
                        角色描述：</label>
                    <asp:textbox runat="server" style="width: 400px;" maxlength="400" textmode="MultiLine" width="200px" rows="6"
                        id="txtEditIntro"></asp:textbox>
                </li>
            </ol>
        </fieldset>
        <fieldset class="submits">
            <input type="button" value="保存" onclick="javascript:EditRoleInfoPostPaly('<%=RoleID %>');"
                class="button" />
            <input type="button" onclick="Close('EditRoleInfo');" value="退出" class="button" />
        </fieldset>
    </div>
</div>
</form>
