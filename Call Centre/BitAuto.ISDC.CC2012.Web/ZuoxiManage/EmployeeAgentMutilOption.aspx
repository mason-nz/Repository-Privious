<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAgentMutilOption.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.EmployeeAgentMutilOption" %>

<link href="../Css/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
        //所属分组
        BindOwnGroup();
        //技能分组
        BindAllSkillGroup();
    });
    //绑定所属分组
    function BindOwnGroup() {
        //通过界面上的区域控件查询
        var AreaID = '<%=AreaID %>';
        var span = $("#span_bg");
        BindGroup(AreaID, span, "radio", "radio_ownbg", "");
    }
    //绑定分组
    function BindGroup(AreaID, span, type, name, user_values) {
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetGroupByAreaID", AreaID: AreaID, r: Math.random() }, null, function (data) {
            span.html("");
            if (data != "") {
                var jsonData = $.evalJSON(data);
                var htmlStr = "";
                //绑定分组
                $(jsonData).each(function (i, v) {
                    htmlStr += "<label style=\"width: 210px; line-height: 30px;\"><input type='" + type + "' id='" + type + "_" + v.BGID + "' name='" + name + "' value='" + v.BGID + "' class='dx' />" + v.Name + "</label>";
                });
                span.html(htmlStr);
            }
            //根据用户数据选择组
            if (user_values != "") {
                $(user_values.split(',')).each(function (i, v) {
                    $("#" + type + '_' + v).attr("checked", true);
                });
            }
        });
    }
    //获取多选值
    function GetMutilSelectValues(key, value) {
        var ids = $("input[" + key + "='" + value + "']").map(function () {
            if ($(this).attr("checked")) {
                return $(this).val();
            }
        }).get().join(',');
        return ids;
    }

    //保存
    function SaveMutilData() {
        var UserIDs = '<%=UserIDs %>';
        var AreaID = '<%=AreaID %>';
        var BusinessType = GetMutilSelectValues("name", "ckb_btype");
        var UserRolesID = $("input[name='rdo_Role']:checked").val();
        var $AtGroup = $("input[name='radio_ownbg']:checked");
        var AtGroupID = $AtGroup.val();
        var AtGroupName = $AtGroup.parent().text();
        var SGIDSAndPrioritys = getskillgroupandpriority();
        var to_skillprioritys = "";
//        $("input:checkbox[name='checkbox_skillgroup']:checked").each(function (i, v) {
//            var thisval = $(this).val();
//            to_skillprioritys += ";" + thisval + "=" + $("#select_skillgrouppriority_" + thisval).val();
//        });
//        if (to_skillprioritys != "") {
//            to_skillprioritys = to_skillprioritys.substr(1);
        //        }
        //外呼技能组
        $("input:radio[name='radio_skillgroup']:checked").each(function (i, v) {
            var thisval = $(this).val();
            to_skillprioritys += ";" + thisval + "=3";
        });
        //外呼外的技能组
        $("input:checkbox[name='checkbox_skillgroup']:checked").each(function (i, v) {
            var thisval = $(this).val();
            to_skillprioritys += ";" + thisval + "=" + $("#select_skillgrouppriority_" + thisval).val();
        });
        if (to_skillprioritys != "") {
            to_skillprioritys = to_skillprioritys.substr(1);
        }

        var param = {
            Action: 'EmployeeAgentMutilOption',
            UserIDs: UserIDs,
            AreaID: AreaID,
            BusinessType: BusinessType,
            UserRolesID: UserRolesID,
            AtGroupID: AtGroupID,
            AtGroupName: AtGroupName,
            SGIDSAndPrioritys: SGIDSAndPrioritys,
            ToHeLiSGIDAndPriority: to_skillprioritys,
            r: Math.random()
        };

        if (BusinessType == undefined || BusinessType == "") {
            $.jAlert("请选择所属业务");
            return;
        }
        if (UserRolesID == undefined || UserRolesID == "") {
            $.jAlert("请选择角色");
            return;
        }
        if (AtGroupID == undefined || AtGroupID == "") {
            $.jAlert("请选择所属分组");
            return;
        }
        if (to_skillprioritys == "") {
            $.jAlert("请选择所属的技能组！");
            return;
        }
        //保存数据
        SaveData(param);
    }
    //保存数据
    function SaveData(param) {
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", param, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "success") {
                $.jPopMsgLayer("保存成功", function () { $.closePopupLayer('EmployeeAgentMutilOption', true); });
            }
            else {
                $.jAlert("保存失败：" + jsonData.msg);
            }
        });
    }


    //绑定所属技能分组
    function BindAllSkillGroup() {
        $("div[name='jinengzudiv']").css("display", "");
        var span = $("#span_SkillGroup");
        BindAllSkillGroupData(span, "checkbox_ownsg");
    }
    //绑定技能分组
    function BindAllSkillGroupData(span, name) {
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "getallhotlineskillgroup", r: Math.random() }, null, function (data) {
            span.html("");
            if (data != "") {
                span.html(data);
                $(".laboverflowvisb .jnz_wh").each(function () {
                    $(this).css("display", "none");
                });

                $(".laboverflowvisb .jnz_xx").each(function () {
                    $(this).css("display", "none");
                });
            }
        });
    }
    function isallselected(obj) {
        var ischecked = true;
        $(obj).find("input:checkbox").each(function () {
            if (!$(this).attr("checked")) {
                ischecked = false;
            }
        }
        );
        return ischecked;
    }
    function hotlinechange(obj) {
        alert($(obj).attr("checked"));
        //var nextdiv = $(obj).next().next();
        var nextdiv = $(obj).parent().next().next();
        //alert(nextdiv.css("display"));
        if (nextdiv.css("display") == "none") {
            nextdiv.css("display", "block");
        }
        else {
            nextdiv.css("display", "none");
        }
        return false;
    }
    function selectallcheckbox(obj) {
        var objchecked = $(obj).attr("checked");

        $(obj).parent().next(0).css("display", "block").find("input:checkbox[name='checkbox_skillgroup']").each(function () {
            $(this).attr("checked", objchecked);
        });
    }
    function getskillgroupandpriority() {
        var sgidandpriority = "";
        //外呼技能组
        $("input:radio[name='radio_skillgroup']:checked").each(function (i, v) {
            var thisval = $(this).val();
            sgidandpriority += thisval + ",3;";
        });
        $("input:checkbox[name='checkbox_skillgroup']:checked").each(function (i, v) {
            var thisval = $(this).val();
            sgidandpriority += thisval + "," + $("#select_skillgrouppriority_" + thisval).val() + ";";
        });
        return sgidandpriority;
    }


    //外呼选择
    function selectfirstradio(obj) {
        var objchecked = $(obj).attr("checked");
        if (!objchecked) {
            $(obj).parent().next(0).css("display", "block").find("input:radio[name='radio_skillgroup']").each(function () {
                $(this).attr("checked", false);
            });
        }
        else {
            $(obj).parent().next(0).css("display", "block").find("input:radio[id='radio_1099']").each(function () {
                $(this).attr("checked", true);
            });
        }
    }
</script>
<div class="pop pb15 popuser openwindow" style="width: 770px;">
    <style>
    input[type="checkbox"]
    {
        margin-top:7px;
        *margin-top:0px;
    }
    input[type="radio"]
    {
         margin-top:8px;
        *margin-top:0px;
    }
</style>
    <div class="title bold">
        <h2>
            批量修改用户数据权限
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('EmployeeAgentMutilOption',false);">
        </a></span>
    </div>
    <div class="contentbox" style="width: auto; border: 0px; background: #FFFFFF; padding-top: 0px;
        padding-bottom: 0px; margin-top: 0px; *margin-top: 0px; margin-bottom: 0px; *margin-bottom: 0px;">
        <div id="page1" class="moveC clearfix" style="margin-top: 5px; width: 720px;">
            <ul class="clearfix" style="padding-bottom: 10px;">
                <li style="width: 770px;">
                    <label>
                        姓名：
                    </label>
                    <span class="w400" style="width: 650px;">
                        <%=UserNames%>
                    </span></li>
                <li style="width: 600px;">
                    <label>
                        <span class="redColor">*</span> 所属业务：
                    </label>
                    <span class="w400" style="width: 400px;">
                        <label style="width: 150px;">
                            <input type="checkbox" class="dx" value="1" name="ckb_btype" id="ckb_businesstype_1" />热线
                        </label>
                        <label style="width: 150px;">
                            <input type="checkbox" class="dx" value="2" name="ckb_btype" id="ckb_businesstype_2" />在线
                        </label>
                    </span></li>
            </ul>
            <div class="line" style="width: 720px;">
            </div>
            <div class="moveC clearfix" style="padding-bottom: 10px; width: 720px;">
                <ul class="clearfix">
                    <li style="width: 760px;">
                        <label>
                            <span class="redColor">*</span>角色：
                        </label>
                        <span id="span_role" class="w400" style="width: 650px;">
                            <asp:repeater runat="server" id="Rpt_Role">
                            <ItemTemplate>
                                <label style="width: 210px; line-height: 30px;">
                                    <input id="role_<%#Eval("RoleID")%>" name="rdo_Role" value="<%#Eval("RoleID") %>" type="radio"  class="dx" cc_role_id="<%#Eval("CC_RoleID") %>" im_role_id="<%#Eval("IM_RoleID") %>"/>
                                    <%#Eval("RoleName") %>
                                </label>
                            </ItemTemplate>
                        </asp:repeater>
                        </span></li>
                </ul>
            </div>
            <div class="line" style="width: 720px;">
            </div>
            <div class="moveC clearfix" style="padding-bottom: 10px; width: 720px;">
                <ul class="clearfix">
                    <li style="width: 760px;">
                        <label>
                            <span class="redColor">*</span>所属分组：
                        </label>
                        <span id="span_bg" class="w400" style="width: 650px;"></span></li>
                </ul>
            </div>
            <script type="text/javascript">
                function visibleToggle(obj) {
                    //alert($(obj).parent().prev().css("background"));
                    var detaildiv = $(obj).parent().next(0);
                    if (detaildiv.css("display") == "none") {
                        detaildiv.css("display", "block");
                        detaildiv.next(0).addClass("line").css({ "width": "610px" });
                        $(obj).html("收起<span style=\" position:absolute;top:0px;float:right; width:15px; height:20px; background:url(../Images/up1.png) right -5px no-repeat;\"></span>");

                        var objpolayer = $("#popupLayerScreenLocker");
                        objpolayer.css("height", (Number(objpolayer.css("height").split("px")[0]) + 100) + "px");
                    }
                    else {
                        detaildiv.css("display", "none");
                        detaildiv.next(0).removeClass("line").css({ "width": "610px" });
                        $(obj).html("展开<span style=\" position:absolute;top:0px;float:right; width:15px; height:20px; background:url(../Images/down1.png) right 8px no-repeat;\"></span>");

                        var objpolayer = $("#popupLayerScreenLocker");
                        objpolayer.css("height", (Number(objpolayer.css("height").split("px")[0]) - 100) + "px");
                    }
                    return false;
                }

                function isallcheckcheckboxchecked(obj) {
                    if (!$(obj).attr("checked")) {
                        $(obj).parent().parent().parent().prev(0).find("input:checkbox").attr("checked", false);
                    }
                    else {
                        var isallchecked = true;
                        var $allcheckboxparent = $(obj).parent().parent().parent();
                        $allcheckboxparent.find("input:checkbox").each(function () {
                            if (!$(this).attr("checked")) {
                                isallchecked = false;
                            }
                        });
                        if (isallchecked) {
                            $allcheckboxparent.prev(0).find("input:checkbox").attr("checked", true);
                        }
                        else {
                            $allcheckboxparent.prev(0).find("input:checkbox").attr("checked", false);
                        }
                    }
                }
           
            </script>
            <!--所属技能组-->
            <div name="jinengzudiv" class="line" style="width: 720px; display: none;">
            </div>
            <div name="jinengzudiv" class="moveC clearfix" style="width: 720px; display: none;">
                <ul class="clearfix">
                    <li style="width: 760px;">
                        <label>
                            <span class="redColor">*</span>所属技能组：</label>
                        <span class="w600" id="span_SkillGroup"></span></li>
                </ul>
            </div>
            <div class="btn mt20">
                <input id="btnSave_Page1" type="button" onclick="SaveMutilData();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关 闭" class="btnCannel bold" onclick="javascript:$.closePopupLayer('EmployeeAgentMutilOption',false);" />
            </div>
        </div>
    </div>
</div>
