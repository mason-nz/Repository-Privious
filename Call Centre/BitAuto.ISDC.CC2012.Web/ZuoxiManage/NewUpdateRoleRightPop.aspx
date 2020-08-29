<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUpdateRoleRightPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.NewUpdateRoleRightPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script>

    $(function () {

        var AreaID = '<%=AreaID %>';
        $("[name='rdo_AreaID'][value='" + AreaID + "']").attr("checked", true);

        $("[name='rdo_AreaID']").change(function () {
            BindGroup();
        });

        BindGroup();

        //绑定用户的角色
        var UserRolesIDs = '<%=UserRolesIDs %>';
        if (UserRolesIDs != "" && UserRolesIDs.split(',')[0] != "") {
            $("#role_" + UserRolesIDs.split(',')[0]).attr("checked", true);
        }

    });

    //保存
    function serverRight() {
        var UserID = $("#hidUserID").val();
        var AgentNum = $("#txtAgentNum").val();
        var UserRolesID = $("input[name='rdo_Role']:checked").val();
        var AtGroupID = $("input[name='AtGroup']:checked").val();
        var AreaID = $('input:radio[name="rdo_AreaID"]:checked').val();        
     
        if (AgentNum == '') {
            $.jAlert("请添写工号");
            return;
        }
       
        if (AreaID == undefined) {
            $.jAlert("请选择所属区域");
            return;
        }
        if (UserRolesID == undefined) {
            $.jAlert("请选择角色");
            return;
        }
        if (AtGroupID == undefined) {
            $.jAlert("请选择所属分组");
            return;
        }

        var Pre = AgentNum.substring(0, 1);
        if (AreaID == 1 && Pre !=0) {
            $.jAlert("北京区的工号应该以0开头");
            return;
        }
        if (AreaID == 2 && Pre != 1) {
            $.jAlert("西安区的工号应该以1开头");
            return;
        }
      

        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetAgentInfoByAgeNum", AgentNum: AgentNum, r: Math.random() }, null, function (data) {
            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData.UserID != UserID) {//其他人有占用当前工号的

                    $.jConfirm("该工号已经被分配给其他坐席,要替换吗？", function (r) {
                        if (r) {
                            SaveInfo();
                        }
                    });
                }
                else {
                    SaveInfo();
                }
            }
            else {
                SaveInfo();
            }
        });
    }

    function SaveInfo() {
        var UserID = $("#hidUserID").val();
        var AgentNum = $("#txtAgentNum").val();
        var UserRolesID = $("input[name='rdo_Role']:checked").val();
        var AtGroupID = $("input[name='AtGroup']:checked").val();
        var ManagerGroupIDs = $("input[name='ManageGroup']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');
        var AreaID = $('input:radio[name="rdo_AreaID"]:checked').val();        


        var pody = {
            UserID: escape(UserID),
            AgentNum: escape(AgentNum),
            UserRolesID: escape(UserRolesID),
            AtGroupID: escape(AtGroupID),
            ManagerGroupIDs: escape(ManagerGroupIDs),
            AreaID: escape(AreaID)
        };

        $.ajax({
            type: "POST",
            url: '/AjaxServers/ZuoxiManage/NewUpdateHandler.ashx',
            dataType: "json",
            data: pody,
            beforeSend: function () { $("#btnSave").attr("disabled", "disabled"); },
            success: function (data) {
                $("#btnSave").attr("disabled", "");

                if (data.Success) {
                    $.jAlert("保存成功", function () {
                        $.closePopupLayer('NewupdateUsersRigth', true);
                    });
                }
                else {
                    $.jAlert(data.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $("#btnSave").attr("disabled", "");
            }
        });
    }


    function BindGroup() {

        var AreaID = $('input:radio[name="rdo_AreaID"]:checked').val();
       
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetGroupByAreaID", AreaID: AreaID, r: Math.random() }, null, function (data) {
            $("#spanGroup").html("");
            $("#spanGroup2").html("");
            if (data != "") {
               
                var jsonData = $.evalJSON(data);
                var htmlStr = "";

                //绑定所属分组
                $(jsonData).each(function (i, v) {
                    htmlStr += "<label><input   type='radio' id='atGroup_" + v.BGID + "' name='AtGroup' value='" + v.BGID + "' class='dx' />" + v.Name + "</label>";
                });
                $("#spanGroup").html(htmlStr);

                //绑定管辖分组
                htmlStr = "";
                $(jsonData).each(function (i, v) {
                    htmlStr += "<label><input   type='checkbox'  id='ManageGroup_" + v.BGID + "' name='ManageGroup' value='" + v.BGID + "' class='dx' />" + v.Name + "</label>";
                });
                $("#spanGroup2").html(htmlStr);
            }

            //绑定所属分组
            var AtGroupID = '<%=AtGroupID %>';
            if (AtGroupID != "") {
                $("#atGroup_" + AtGroupID).attr("checked", true);
            }

            //绑定管辖分组
            var ManagerGroupIDs = '<%=ManagerGroupIDs %>';
            if (ManagerGroupIDs != "") {
                $(ManagerGroupIDs.split(',')).each(function (i, v) {
                    $("#ManageGroup_" + v).attr("checked", true);
                });
            }

        });
    }
</script>
<div class="pop pb15 popuser openwindow">
    <div class="title bold">
        <h2>
            用户权限设置</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('NewupdateUsersRigth',false);">
        </a></span>
    </div>
    <div class="moveC clearfix" style="margin-top: 15px;">
        <ul class="clearfix">
            <li  >
                <label>
                    <span class="redColor">*</span>姓名：</label><span><%=UserName%></span></li>
          
             <li style=" width:235px;" >
                <label>
                    <span class="redColor">*</span>工号：</label><span class="gh"><input type="text" id="txtAgentNum"
                        value='<%=AgentNum%>' class="w125" /></span></li>
             <li style=" width:350px;">
                <label >
                    <span class="redColor">*</span>所属区域：</label><span class="w400" style=" width:200px;">
                    <label  style=" width:80px;">
                   <input type="radio" class="dx" value="1" name="rdo_AreaID">北京
                    </label>
                    <label  style=" width:80px;">
                   <input type="radio" class="dx" value="2" name="rdo_AreaID">西安
                    </label>
                    </span></li>
            <li>
                <label>
                    <span class="redColor">*</span>角色：</label>
                <span class="w400">
                    <asp:repeater runat="server" id="Rpt_Role">
               <ItemTemplate>
                  <label>
                   <input id="role_<%#Eval("RoleID")%>" name="rdo_Role" value="<%#Eval("RoleID") %>" type="radio"  class="dx" /><%#Eval("RoleName") %>
                    </label>
                    </ItemTemplate>
             </asp:repeater>
                </span></li>
        </ul>
    </div>
    <div class="line">
    </div>
    <div class="moveC clearfix">
        <ul class="clearfix">
            <li style=" width:590px;">
                <label>
                    <span class="redColor">*</span>所属分组：</label>
                <span id="spanGroup" class="w400" style=" width:480px;">
                  
                </span></li>
        </ul>
    </div>
    <div class="line">
    </div>
    <div class="moveC clearfix">
        <ul class="clearfix">
           <li style=" width:590px;">
                <label>
                    管辖分组：</label>
                <span id="spanGroup2" class="w400 fxk"  style=" width:480px;">
                    
                </span></li>
        </ul>
    </div>
    <div class="btn mt20">
        <input name="" id="btnSave" type="button" onclick="serverRight()" value="保 存" class="btnSave bold" />
        <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('NewupdateUsersRigth',false);" /></div>
    <input id="hidUserID" type="hidden" value='<%=UserID%>' />
</div>
