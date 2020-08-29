<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUsersRight.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.UpdateUsersRight" %>

<script type="text/javascript">
    $(document).ready(BindingData);

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
                        if (v.DataRight == "1") {
                            $("#rdoMyselt").attr("checked", true);
                        }
                        else if (v.DataRight == "2") {
                            $("#rdoAll").attr("checked", true);
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

    function emChkIsChooseTN(othis) {
        var $checkbox = $(othis).prev();
        if ($checkbox.is(":checked")) {
            $checkbox.removeAttr("checked");
        }
        else {
            $checkbox.attr("checked", "checked");
        }
    }
</script>
<div class="pop pb15 openwindow" style="width: 770px;">
    <div class="title bold">
        <h2>
            批量修改用户权限
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('updateUsersRigth',false);">
        </a></span>
    </div>

    <div class="popT bold pt5" style="clear: both;">
        <span style="display: inline-block; width: 85px; text-align: right; margin-right: 2px;
            float: left;">员工姓名：</span> <span style="display: block; padding-right: 10px; float: left;
                width: 480px;">
                <%=UserName%></span>
    </div>
    <div class="popT bold pt5" id="divAgentNum" runat="server" style="clear: both; padding-top: 8px;">
        <span style="display: inline-block; width: 85px; text-align: right;">工号：</span>
        <input type="text" id="txtAgentNum" value='<%=AgentNum%>' class="w125" />
    </div>
    <div class="popT bold pt5" style="clear: both; padding-top: 5px;">
        <span style="display: inline-block; width: 85px; text-align: right; margin-right: 2px;
            float: left;">选择角色：</span>
        <div style="display: block; padding-right: 10px; float: left; width: 480px;">
            <div style="border-left: 0px solid #FFF; width: 480px;">
                <asp:repeater runat="server" id="Rpt_Role">
                <ItemTemplate>
                    <span style=" display:inline-block; width:140px; line-height:20px; cursor:pointer;"><input type="radio" id="roleCk_<%#Eval("RoleID")%>" name="CK_Role" value="<%#Eval("RoleID") %>" /><em onclick="emChkIsChooseTN(this)"><%#Eval("RoleName") %></em></span>
                </ItemTemplate>
            </asp:repeater>
            </div>
        </div>
    </div>
    <div class="popT bold pt5" style="clear: both; padding-top: 5px;">
        <div class="line" style="background: url(../Css/img/line.jpg) repeat-x; height: 1px;
            width: 100%; margin: 0px auto; padding-bottom: 5px;">
        </div>
        <span style="display: inline-block; width: 85px; text-align: right; margin-right: 2px;
            float: left;">选择所属分组：</span>
        <div style="display: block; padding-right: 10px; float: left; width: 480px;">
            <div style="border-left: 0px solid #FFF; width: 480px;">
                <asp:repeater runat="server" id="Rpt_BusinessGroup">
                <ItemTemplate>
                    <span style=" display:inline-block; width:140px; line-height:20px; cursor:pointer;"><input type="radio" id="Radio1" name="CK_BGID" value="<%#Eval("BGID") %>" /><em onclick="emChkIsChooseTN(this)"><%#Eval("Name")%></em></span>
                </ItemTemplate>
            </asp:repeater>
            </div>
        </div>
    </div>
    <div class="popT bold pt5" id="divData" runat="server" style="margin-top: 5px; clear: both;">
        <span style="display: inline-block; width: 85px; text-align: right;">数据权限：</span>
        <input type="radio" name="dateType" id="rdoMyselt" value="1" checked="checked" /><label
            for="rdoMyselt">本人</label>
        <input type="radio" name="dateType" id="rdoAll" value="2" /><label for="rdoAll">全部</label>
    </div>
    <div class="btn" style="padding-top: 15px; clear: both;">
        <input type="button" onclick="serverRight()" value="保存" class="btnSave bold" />
        <input type="button" onclick="javascript:$.closePopupLayer('updateUsersRigth',false);"
            value="取消" class="btnSave bold" />
    </div>
    <input id="hidUserIDs" type="hidden" value='<%=UserIDs%>' />
    <input id="hidIsSinge" type="hidden" value='<%=IsModfiySingle%>' />
    <input id="hidIsDataTyle" type="hidden" value='<%=DataRightTyle%>' />
    <script type="text/javascript" language="javascript">
        function serverRight() {
            var ck_RoleList = $(":radio[name^='CK_Role'][checked=true]");
            var length = ck_RoleList.length;

            var CK_RoleIDs = "";
            for (var i = 0; i < length; i++) {
                CK_RoleIDs += ck_RoleList.eq(i).val() + ",";
            }
            if (length > 0) {
                CK_RoleIDs = CK_RoleIDs.substring(0, CK_RoleIDs.length - 1);
            }

            var ck_BGID = $(":radio[name^='CK_BGID'][checked=true]");
            var length_BGID = ck_BGID.length;
            var CK_BGIDs = "";
            for (var i = 0; i < length_BGID; i++) {
                CK_BGIDs += ck_BGID.eq(i).val() + ",";
            }
            if (length_BGID > 0) {
                CK_BGIDs = CK_BGIDs.substring(0, CK_BGIDs.length - 1);
            }

            var righttype = $('input:radio[name="dateType"]:checked').val(); ;
            var pody = { roleIDs: CK_RoleIDs, bgids: CK_BGIDs, userIDs: $("#hidUserIDs").val(),regionId:<%=ArealID%>, agentnum: $("#txtAgentNum").val(), single: $("#hidIsSinge").val(), righttype: righttype };
            AjaxPost('/ZuoxiManage/UpdateUsersRight.ashx', pody, null, function (data) {

                switch (data) {

                    case "succeed": $.jPopMsgLayer("保存成功", function () {
                        search();
                        $.closePopupLayer('updateUsersRigth', true);
                    }); break;
                    case "noUserPar": $.jAlert("缺少用户ID信息"); break;
                    case "useridFormatErr": $.jAlert("用户ID信息格式不正确"); break;
                    case "rightTypeErr": $.jAlert("数据权限格式不正确"); break;
                    case "repeated": $.jAlert("此工号已经被分配给其他坐席"); break;
                    case "modifyErr": $.jAlert("修改角色失败"); break;
                    default: $.jAlert("保存出错！");
                }
            });

        }
    </script>
</div>
