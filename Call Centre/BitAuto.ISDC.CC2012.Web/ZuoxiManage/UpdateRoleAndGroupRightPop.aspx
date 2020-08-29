<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateRoleAndGroupRightPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.UpdateRoleAndGroupRightPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<style type="text/css">
    
</style>
<script type="text/javascript">
    $(document).ready(function () {
        BindingData();
        BindGroupDataRight();

        ///绑定各个分组的400外显号码
        Binding400CallNums();
    });
    function BindGroupDataRight() {
        var userId = $("#hidUserIDs").val();
        $.post("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetGroupDataRight", UserID: userId }, function (data) {
            if (Len(data) > 0) {
                var jsonData = $.evalJSON(data);
                $(jsonData).each(function (i, v) {
                    var obj = $("[name='chkGroup'][value='" + v.BGID + "']");
                    $(obj).attr("checked", true);
                    $(obj).parent().parent().next().next().find("[type='radio'][value=" + v.RightType + "]").attr("checked", true);
                });
            }
        });
    }
    function BindingData() {
        $("#pageHiddenMain").val($("#pageHidden").val());

        if ($("#hidIsSinge").val() == "true") { //只有在单个时，绑定


            //绑定数据权限
            if ($("#hidIsDataTyle").val() == "1") {
                $("#rdoMyselt").attr("checked", true);
            }
            else if ($("#hidIsDataTyle").val() == "2") {
                $("#rdoAll").attr("checked", true);
            }

            //绑定角色
            var pody = { userID: $("#hidUserIDs").val() };
            AjaxPost('/AjaxServers/ZuoxiManage/GetAgentInfoByUserId.ashx', pody, null, function (data) {

                if (data == "noPar") {
                    $.jAlert("缺少参数");
                }
                else {

                    var jsonStr = $.evalJSON(data);
                    $(jsonStr).each(function (i, v) {

                        $("#txtAgentNum").val(v.AgentNum);
                        if (v.DataRight == "2") {
                            $("#chkAll").attr("checked", true);
                        }

                        var list = v.RoleIDs.split(",");
                        $(list).each(function (i, v) {
                            $("#roleCk_" + v).attr("checked", true);
                        });
                    });
                }
            });
        }
    }

    //绑定各个分组的400外显号码
    function Binding400CallNums() {



    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            用户权限设置</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('updateUsersRigth',false);">
        </a></span>
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 65px; text-align: right; margin-right: 2px;">
            员工姓名：</span>
        <%=UserName%>
    </div>
    <div class="popT bold pt5" id="divAgentNum" runat="server">
        <span style="display: inline-block; width: 65px; text-align: right;">工号：</span>
        <input type="text" id="txtAgentNum" value='<%=AgentNum%>' class="w125" />
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 65px; text-align: right;">选择角色：</span></div>
    <div style="padding: 10px 20px; padding-top: 0; padding-bottom: 0;">
        <div style="border-left: 56px solid #FFF; width: 330px;">
            <asp:repeater runat="server" id="Rpt_Role">
        <ItemTemplate>
            <span style=" display:inline-block; width:160px; line-height:20px;"><input type="checkbox" id="roleCk_<%#Eval("RoleID")%>" name="CK_Role" value="<%#Eval("RoleID") %>" /><%#Eval("RoleName") %></span>
        </ItemTemplate>
        </asp:repeater>
        </div>
    </div>
    <div class="popT bold pt5" id="divData" runat="server">
        <span style="display: inline-block; width: 65px; text-align: right;">数据权限：</span>
        <input type="checkbox" name="dateType" id="chkAll" value="2" /><label for="rdoAll">全部</label>
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 65px; text-align: right;">选择分组：</span></div>
    <div style="padding: 10px 20px; padding-top: 0; padding-bottom: 0;">
        <div style="border-left: 56px solid #FFF; width: 550px;">
            <table class="group">
                <tr style="font-size: larger;">
                    <td style="width: 200px;">
                        所属分组
                    </td>
                    <td style="width: 200px;">
                        外显400号码
                    </td>
                    <td style="width: 300px">
                        分组数据权限
                    </td>
                </tr>
                <asp:repeater runat="server" id="rptGroup">
        <ItemTemplate>
        <tr class="trGroup">
            <td>
            <span style=" display:inline-block; width:160px; line-height:20px;">
            <input type="checkbox" name="chkGroup" value="<%#Eval("BGID") %>" onclick="ChkGroupOnClick(this)" /><label><%#Eval("Name")%></label>
            </span>
            </td>
            <td>
            <input type="hidden" name="hid400CallNum" value='<%#Eval("BGID") %>' />
            <%#Eval("call400Num")%>
            </td>
            <td>
            <input type="radio" name="rdoGroup<%#Container.ItemIndex %>" value="2" />本组<input name="rdoGroup<%#Container.ItemIndex %>" type="radio" value="1" />本人
           </td>
         </tr>
        </ItemTemplate>
        </asp:repeater>
            </table>
        </div>
    </div>
    <div class="btn" style="margin-top: 5px;">
        <input type="button" onclick="serverRight()" value="保存" class="btnSave bold" />
        <input type="button" onclick="javascript:$.closePopupLayer('updateUsersRigth',false);"
            value="取消" class="btnSave bold" />
    </div>
    <input id="hidUserIDs" type="hidden" value='<%=UserIDs%>' />
    <input id="hidIsSinge" type="hidden" value='<%=IsModfiySingle%>' />
    <input id="hidIsDataTyle" type="hidden" value='<%=DataRightTyle%>' />
    <script type="text/javascript" language="javascript">
        function serverRight() {
            var ck_RoleList = $(":checkbox[name^='CK_Role'][checked=true]");
            var length = ck_RoleList.length;

            var CK_RoleIDs = "";
            for (var i = 0; i < length; i++) {
                CK_RoleIDs += ck_RoleList.eq(i).val() + ",";
            }
            if (length > 0) {
                CK_RoleIDs = CK_RoleIDs.substring(0, CK_RoleIDs.length - 1);
            }
            righttype = 1;
            if ($("#chkAll").attr("checked")) {
                var righttype = 2;
            }
            var msg = VerifyGroupRight();
            if (msg.length > 0) {
                $.jAlert(msg);
            }
            else {


                var UserIDs = $("#hidUserIDs").val();
                var AgentNums = $.trim($("#txtAgentNum").val());
                var pody = { roleIDs: CK_RoleIDs, userIDs: UserIDs, agentnum: AgentNums,
                    single: $("#hidIsSinge").val(), righttype: righttype, GroupRightStr: GetGroupRightStr()
                };
                AjaxPost('/ZuoxiManage/UpdateUsersRight.ashx', pody,null,function (data) {
                    //$.unblockUI();
                    switch (data) {
                        case "succeed": $.jAlert("保存成功", function () {
                            search();
                            $.closePopupLayer('updateUsersRigth', true);
                        }); break;
                        case "noUserPar": $.jAlert("缺少用户ID信息"); break;
                        case "useridFormatErr": $.jAlert("用户ID信息格式不正确"); break;
                        case "rightTypeErr": $.jAlert("数据权限格式不正确"); break;
                        case "repeated": ConfirmserverRight(pody); break;
                        case "modifyErr": $.jAlert("修改角色失败"); break;
                        default: $.jAlert("保存出错！");
                    }
                });
            }
        }
        //add by qizq 2013-7-16 如果该工号被其他员工占用，提示是否替换，
        function ConfirmserverRight() {

            $.jConfirm("该工号已经被分配给其他坐席,要替换吗？",
           function (r) {
               if (r) {

                   var ck_RoleList = $(":checkbox[name^='CK_Role'][checked=true]");
                   var length = ck_RoleList.length;

                   var CK_RoleIDs = "";
                   for (var i = 0; i < length; i++) {
                       CK_RoleIDs += ck_RoleList.eq(i).val() + ",";
                   }
                   if (length > 0) {
                       CK_RoleIDs = CK_RoleIDs.substring(0, CK_RoleIDs.length - 1);
                   }
                   var righttype = 1;
                   if ($("#chkAll").attr("checked")) {
                       righttype = 2;
                   }


                   var UserIDs = $("#hidUserIDs").val();
                   var AgentNums = $.trim($("#txtAgentNum").val());
                   var pody = { roleIDs: CK_RoleIDs, userIDs: UserIDs, agentnum: AgentNums,
                       single: $("#hidIsSinge").val(), righttype: righttype, GroupRightStr: GetGroupRightStr(), IsContinute: '1'
                   };

                   AjaxPost('/ZuoxiManage/UpdateUsersRight.ashx', pody, null, function (data) {
                       //$.unblockUI();
                       switch (data) {
                           case "succeed": $.jPopMsgLayer("保存成功", function () {
                               search();
                               $.closePopupLayer('updateUsersRigth', true);
                           }); break;
                           case "modifyErr": $.jAlert("修改角色失败"); break;
                           default: $.jAlert("保存出错！");
                       }
                   });

               }
           }
              );
        }
        function ChkGroupOnClick(obj) {
            if (!$(obj).attr("checked")) {
                $(obj).parent().parent().next().find("[type='radio']").each(function (i, item) {
                    $(item).attr("checked", false);
                });
            }
        }
        function GetGroupRightStr() {
            var groupRightStr = "";
            $(".trGroup").each(function () {
                var obj = $(this).find("[name='chkGroup']");
                if ($(obj).attr("checked")) {
                    var bgId = $(obj).val();
                    var rightType = $(this).find("[name^='rdoGroup']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(",");
                    if (groupRightStr.length > 0) {
                        groupRightStr += ";" + bgId + "|" + rightType;
                    }
                    else {
                        groupRightStr += bgId + "|" + rightType;
                    }
                }
            });

            return groupRightStr;
        }

        function VerifyGroupRight() {
            var msg = "";
            $(".trGroup").each(function () {
                var obj = $(this).find("[name='chkGroup']");
                if ($(obj).attr("checked")) {
                    var rightType = $(this).find("[name^='rdoGroup']").map(function () {
                        if ($(this).attr("checked")) {
                            return $(this).val();
                        }
                    }).get().join(",");
                    if (rightType.length < 1) {
                        msg += "请为" + $(obj).next().text() + "选择分组权限<br/>";
                    }
                }
            });

            return msg;
        }
    </script>
</div>
