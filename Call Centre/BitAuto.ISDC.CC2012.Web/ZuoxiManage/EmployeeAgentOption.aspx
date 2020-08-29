<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAgentOption.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.EmployeeAgentOption" %>

<link href="../Css/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
        //所属分组
        BindOwnGroup();
        //技能分组
        BindOwnSkillGroup();
        //角色选择
        ChangeBusinessTypeForRole();
        //区域切换
        $("[name='rdo_AreaID']").change(function () {
            BindOwnGroup();
        });
        //业务切换
        $("input[cname='ckb_btype']").change(function () {
            ChangeBusinessTypeForRole();
        });
        //全选按钮设置
        $("#cbk_all_beijing").change(function () {
            $("input[name='ckb_managebg_beijing']").attr("checked", $("#cbk_all_beijing").attr("checked"));
        });
        $("#cbk_all_xian").change(function () {
            $("input[name='ckb_managebg_xian']").attr("checked", $("#cbk_all_xian").attr("checked"));
        });
        SetPage(1);
    });
    //绑定所属分组
    function BindOwnGroup() {
        //通过界面上的区域控件查询
        var AreaID = $('input:radio[name="rdo_AreaID"]:checked').val();
        if (AreaID == "") {
            return;
        }
        var span = $("#span_bg");
        var user_values = '<%=AtGroupID %>';
        BindGroup(AreaID, span, "radio", "radio_ownbg", user_values);
    }
    //绑定管辖分组
    function BindManageGroup() {
        //通过用户id获取区域信息查询
        var UserID = '<%=UserID %>';
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetAreaInfoAndManageBgs", UserID: UserID, r: Math.random() }, null, function (data) {
            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData.areaid != null) {
                    var span_BiJing = $("#span_managebg_beijing");
                    var span_XiAn = $("#span_managebg_xian");
                    if (jsonData.areaid == "1") {
                        $("#span_area").text("北京");
                    }
                    else if (jsonData.areaid == "2") {
                        $("#span_area").text("西安");
                    }
                    //北京
                    BindGroup("1", span_BiJing, "checkbox", "ckb_managebg_beijing", jsonData.managebgs, function () {
                        //设置全选
                        SetAllSelectChecked("beijing");
                        //设置每一个选项
                        // var checkboxnum = $("input[name='ckb_managebg_beijing']").length;
                        // var checkedboxnum = $("input[name='ckb_managebg_beijing']:checked").length;
                        // alert(checkboxnum + ";" + checkedboxnum);
                        $("input[name='ckb_managebg_beijing']").change(function () {
                            SetAllSelectChecked("beijing");
                        });
                    });
                    //西安
                    BindGroup("2", span_XiAn, "checkbox", "ckb_managebg_xian", jsonData.managebgs, function () {
                        //设置全选
                        SetAllSelectChecked("xian");
                        //设置每一个选项
                        $("input[name='ckb_managebg_xian']").change(function () {
                            SetAllSelectChecked("xian");
                        });
                    });
                }
            }
        });
    }
    //设置全选状态
    function SetAllSelectChecked(areaType) {
        var areaname = "";
        if (areaType == "beijing") {
            areaname = "_beijing";
        }
        else if (areaType == "xian") {
            areaname = "_xian";
        }
        if ($("input[name='ckb_managebg" + areaname + "']").length == $("input[name='ckb_managebg" + areaname + "']:checked").length && $("input[name='ckb_managebg" + areaname + "']").length != 0) {
            $("#cbk_all" + areaname).attr("checked", true);
        }
        else {
            $("#cbk_all" + areaname).attr("checked", false);
        }
    }
    //绑定分组
    function BindGroup(AreaID, span, type, name, user_values, callback) {
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
            //完成后回调
            if (callback != null) {
                callback();
            }
        });
    }
    //切换业务时，同步显示对应系统中的角色
    function ChangeBusinessTypeForRole() {
        ChooseRadioChecked("rdo_Role", null, null);
        var ckb_BusinessType = GetMutilSelectValues("cname", "ckb_btype");
        if (ckb_BusinessType == "") {
            return;
        }
        else {
            var UserID = '<%=UserID %>';
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetRoleIDForCCAndIMByUserid", UserID: UserID, BusinessType: ckb_BusinessType, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.key != "" && jsonData.value != "") {
                    ChooseRadioChecked("rdo_Role", jsonData.key, jsonData.value);
                }
            });
        }
    }
    //设置Radio控件的值
    function ChooseRadioChecked(name, key, value) {
        //清空选择项
        $("input[name='" + name + "']").attr("checked", false);
        //设置选择值
        if (key != null && value != null) {
            $("input[" + key + "='" + value + "']").attr("checked", true);
        }
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
    //设置页签
    function SetPage(page) {
        if (page == 1) {
            $("#page1").css("display", "block");
            $("#page2").css("display", "none");
            $("#link1").css("color", "#0088cc");
            $("#link2").css("color", "#666666");

            $("#img1").attr("src", "/Images/set_hr.png");
            $("#img2").attr("src", "/Images/power.png");
        }
        else if (page == 2) {
            $("#page1").css("display", "none");
            $("#page2").css("display", "block");
            $("#link1").css("color", "#666666");
            $("#link2").css("color", "#0088cc");

            $("#img1").attr("src", "/Images/set.png");
            $("#img2").attr("src", "/Images/power_hr.png");

            //切换页签时触发
            BindManageGroup();
        }
    }
    //绑定所属技能分组
    function BindOwnSkillGroup() {
        $("div[name='jinengzudiv']").css("display", "");
        var span = $("#span_SkillGroup");
        var user_values = '<%=OwnSkillGroupIDS %>';
        BindSkillGroup(span, "checkbox_ownsg", user_values);
    }
    //绑定技能分组
    //    function BindSkillGroup(AreaID, span, name, user_values, callback) {
    //        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "GetSkillGroupByAreaID", AreaID: AreaID, r: Math.random() }, null, function (data) {
    //            span.html("");
    //            if (data != "") {
    //                var jsonData = $.evalJSON(data);
    //                var htmlStr = "<ul>";
    //                //绑定热线组
    //                var thecdid = "";
    //                var hasnum = 0;
    //                var isfirst = 0;
    //                $(jsonData).each(function (i, v) {
    //                    if (v.CDID != thecdid) {
    //                        thecdid = v.CDID;
    //                        if (isfirst > 0) {
    //                            htmlStr += "<br/><div class=\"line\" style=\"width:625px;clear:both;\"></div>";
    //                        }
    //                        if (hasnum != 0) {
    //                            htmlStr += "</div></li>";
    //                        }
    //                        htmlStr += "<li><label  style=\"width: 210px; line-height: 30px;\"><input onclick='hotlinechange(this)'  type='checkbox' id='checkbox_hotline_"
    //                        + v.CDID + "' name='checkbox_hotline' value='" + v.CDID + "'  />" + v.CallNum + "</label><br/>";
    //                        htmlStr += "<div style='width:650px;display:none'><div class=\"line\" style=\"width:625px;\"></div>";
    //                        htmlStr += "<label style=\"width: 210px; line-height: 30px;\"><i><input type='checkbox' id='checkbox_" + v.SGID + "' name='" + name + "' value='" + v.SGID + "'  />" + v.Name + "</i></label>";

    //                        isfirst++;
    //                    }
    //                    else {
    //                        hasnum = 0;
    //                        htmlStr += "<label style=\"width: 210px; line-height: 30px;\"><i><input type='checkbox' id='checkbox_" + v.SGID + "' name='" + name + "' value='" + v.SGID + "'  />" + v.Name + "</i></label>";
    //                    }
    //                });
    //                htmlStr += "<br/><div class=\"line\" style=\"width:625px;clear:both;\"></div>";
    //                htmlStr += "</div></li>";
    //                htmlStr += "</ul>";
    //                span.html(htmlStr);
    //            }
    //            //根据用户数据选择组
    //            if (user_values != "") {
    //                $(user_values.split(',')).each(function (i, v) {
    //                    $("#checkbox_" + v).attr("checked", true);
    //                });
    //            }
    //            //完成后回调
    //            if (callback != null) {
    //                callback();
    //            }
    //        });
    //    }
    //绑定技能分组
    function BindSkillGroup(span, name, user_values, callback) {
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "getallhotlineskillgroup", r: Math.random() }, null, function (data) {
            span.html("");
            if (data != "") {

                span.html(data);
            }
            //根据用户数据选择组
            if (user_values != "") {
                var sgidandpriority = new Array();
                $(user_values.split(',')).each(function (i, v) {
                    sgidandpriority = v.split(';');
                    if ($("#checkbox_" + sgidandpriority[0]).length > 0) {
                        $("#checkbox_" + sgidandpriority[0]).attr("checked", true);
                        $("#checkbox_" + sgidandpriority[0]).parent().next(0).val(sgidandpriority[1]);
                    }
                    else if ($("#radio_" + sgidandpriority[0]).length > 0) {
                        $("#radio_" + sgidandpriority[0]).attr("checked", true);
                    }
                    //                    if (isallselected($("#checkbox_" + sgidandpriority[0]).parent().parent().parent())) {
                    //                        $("#checkbox_" + sgidandpriority[0]).parent().parent().parent().prev(0).find("input:checkbox").attr("checked", true);
                    //                    }
                });


            }

            $(".laboverflowvisb .jnz_wh").each(function () {
                var detaildiv = $(this);
                var haschecked = false;
                var hasnotchecked = false;
                detaildiv.find("input:radio").each(function () {
                    if ($(this).attr("checked")) {
                        haschecked = true;
                        detaildiv.prev(0).find("input:checkbox").attr("checked", true);
                    }
                    else {
                        hasnotchecked = true;
                    }
                });

                if (haschecked) {
                    if (!hasnotchecked) {
                        $(this).prev(0).find("input:checkbox").attr("checked", true);
                    }
                    $(this).css("display", "block");
                }
                else {
                    $(this).css("display", "none");
                }
            });

            $(".laboverflowvisb .jnz_xx").each(function () {
                var detaildiv = $(this);
                var haschecked = false;
                var hasnotchecked = false;
                detaildiv.find("input:checkbox").each(function () {
                    if ($(this).attr("checked")) {
                        haschecked = true;
                    }
                    else {
                        hasnotchecked = true;
                    }
                });

                if (haschecked) {
                    if (!hasnotchecked) {
                        $(this).prev(0).find("input:checkbox").attr("checked", true);
                    }
                    $(this).css("display", "block");
                }
                else {
                    $(this).css("display", "none");
                }
            });

            //完成后回调
            if (callback != null) {
                callback();
            }
            //  loadjscssfile("/Css/style.css", "css");
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

    function loadjscssfile(filename, filetype) {
        alert(filetype);
        if (filetype == "js") {
            var fileref = document.createElement('script');
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", filename);
        } else if (filetype == "css") {

            var fileref = document.createElement('link');
            fileref.setAttribute("rel", "stylesheet");
            fileref.setAttribute("type", "text/css");
            fileref.setAttribute("href", filename);
        }
        if (typeof fileref != "undefined") {
            document.getElementsByTagName("head")[0].appendChild(fileref);
        }

    }
</script>
<script type="text/javascript">
    //除外呼热线之外的选择
    function selectallcheckbox(obj) {
        var objchecked = $(obj).attr("checked");
        $(obj).parent().next(0).css("display", "block").find("input:checkbox[name='checkbox_skillgroup']").each(function () {
            $(this).attr("checked", objchecked);
        });
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
    function getskillgroupandpriority() {
        var sgidandpriority = "";
        //$("input:radio_skillgroup")
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

    var ispage1Saved = false;
    var ispage2Saved = false;

    //保存页面1
    function SavePage1() {
        //给合力接口回传数据
        var to_username = "<%=UserName %>";
        var to_skillprioritys = "";
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
        var UserID = '<%=UserID %>';
        var AgentNum = $.trim($("#txtAgentNum").text());
        var AreaID = $('input:radio[name="rdo_AreaID"]:checked').val();
        var BusinessType = GetMutilSelectValues("cname", "ckb_btype");
        var UserRolesID = $("input[name='rdo_Role']:checked").val();
        var $AtGroup = $("input[name='radio_ownbg']:checked");
        var AtGroupID = $AtGroup.val();
        var AtGroupName = $AtGroup.parent().text();
        var SGIDSAndPrioritys = getskillgroupandpriority();
        var param = {
            Action: 'EmployeeAgentOptionForPageOne',
            UserID: UserID,
            AgentNum: AgentNum,
            AreaID: AreaID,
            BusinessType: BusinessType,
            UserRolesID: UserRolesID,
            AtGroupID: AtGroupID,
            SGIDSAndPrioritys: SGIDSAndPrioritys,
            UserName: to_username,
            ToHeLiSGIDAndPriority: to_skillprioritys,
            AtGroupName: AtGroupName,
            r: Math.random()
        };

        //非空校验
        if (AgentNum == '') {
            $.jAlert("请添写工号", function () { $("#txtAgentNum").focus(); });
            return;
        }
        if (AreaID == undefined || AreaID == "") {
            $.jAlert("请选择所属区域");
            return;
        }
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
//        //工号规则校验
//        var Pre = AgentNum.substring(0, 1);
//        if (AreaID == 1 && Pre != 0) {
//            $.jAlert("北京区的工号应该以0开头");
//            return;
//        }
//        if (AreaID == 2 && Pre == 0) {
//            $.jAlert("西安区的工号应该以非0开头");
//            return;
//        }

        if (to_skillprioritys == "") {
            $.jAlert("请选择所属的技能组！");
            return;
        }
        //保存数据
        SaveData(param, 1);

    }
    //保存页面2
    function SavePage2() {
        var managebg_beijing = GetMutilSelectValues("name", "ckb_managebg_beijing");
        var managebg_xian = GetMutilSelectValues("name", "ckb_managebg_xian");
        var UserID = '<%=UserID %>';
        var param = {
            Action: 'EmployeeAgentOptionForPageTwo',
            UserID: UserID,
            ManageBG_BeiJing: managebg_beijing,
            ManageBG_XiAn: managebg_xian,
            r: Math.random()
        };
        SaveData(param, 2);
    }
    //保存数据
    function SaveData(param, page) {
        AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", param, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "success") {
                $.jPopMsgLayer("保存成功", function () {
                    if (page == 1) {
                        ispage1Saved = true;
                    }
                    else {
                        ispage2Saved = true;
                    }
                });
            }
            else {
                $.jAlert("保存失败：" + jsonData.msg);
            }
        });
    }
    //关闭页面
    function ClosePage() {
        var pageid = "EmployeeAgentOption";
        var refresh = ispage1Saved || ispage2Saved;
        $.closePopupLayer(pageid, refresh);
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
            用户权限设置</h2>
        <span><a href="javascript:void(0)" onclick="ClosePage();"></a></span>
    </div>
    <div class="optionBtn clearfix">
        <img id="img1" style="vertical-align: middle; margin-left: 20px; margin-right: 5px;" /><a
            href="javascript:void(0);" onclick="SetPage(1);" id="link1">基本设置</a>
        <img id="img2" style="vertical-align: middle; margin-left: 40px; margin-right: 5px;" /><a
            href="javascript:void(0);" onclick="SetPage(2);" id="link2">数据权限设置</a>
    </div>
    <div class="contentbox" style="width: auto; border: 0px; background: #FFFFFF; padding-top: 0px;
        padding-bottom: 0px; margin-top: 0px; *margin-top: 0px; margin-bottom: 0px; *margin-bottom: 0px;">
        <div id="page1" class="moveC clearfix" style="margin-top: 5px;">
            <ul class="clearfix" style="padding-bottom: 10px;">
                <li>
                    <label>
                        姓名：
                    </label>
                    <span>
                        <%=UserName %></span></li>
                <li style="width: 600px;">
                    <label>
                        工号：
                    </label>
                    <span id="txtAgentNum" runat="server">
                    </span></li>
                <li style="width: 600px;">
                    <label>
                        <span class="redColor">*</span> 所属区域：
                    </label>
                    <span class="w400" style="width: 400px;">
                        <label style="width: 150px;">
                            <input type="radio" class="dx" value="1" name="rdo_AreaID" runat="server" id="rdo_areaid_1" />北京
                        </label>
                        <label style="width: 150px;">
                            <input type="radio" class="dx" value="2" name="rdo_AreaID" runat="server" id="rdo_areaid_2" />西安
                        </label>
                    </span></li>
                <li style="width: 600px;">
                    <label>
                        <span class="redColor">*</span> 所属业务：
                    </label>
                    <span class="w400" style="width: 400px;">
                        <label style="width: 150px;">
                            <input type="checkbox" class="dx" value="1" cname="ckb_btype" id="ckb_businesstype_1"
                                runat="server" />热线
                        </label>
                        <label style="width: 150px;">
                            <input type="checkbox" class="dx" value="2" cname="ckb_btype" id="ckb_businesstype_2"
                                runat="server" />在线
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
            <!--<div class="line" style="width: 720px;">
        </div>
        <div class="moveC clearfix" style="padding-bottom: 10px;">
            <ul class="clearfix">
                <li style="width: 760px;">
                    <label>
                        <span class="redColor">*</span>所属技能组：
                    </label>
                    <span id="span_SkillGroup" class="w400" style="width: 650px;"></span>
                </li>
            </ul>
        </div>
        -->
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
                <input id="btnSave_Page1" type="button" onclick="SavePage1();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关 闭" class="btnCannel bold" onclick="ClosePage();" />
            </div>
        </div>
        <div id="page2" class="moveC clearfix" style="margin-top: 5px;">
            <ul class="clearfix" style="padding-bottom: 10px;">
                <li>
                    <label>
                        姓名：
                    </label>
                    <span>
                        <%=UserName %></span></li>
                <li>
                    <label>
                        区域：
                    </label>
                    <span id="span_area"></span></li>
            </ul>
            <div class="line" style="width: 720px;">
            </div>
            <div class="moveC clearfix" style="width: 720px; padding-bottom: 10px;">
                <ul class="clearfix">
                    <li style="width: 760px;">
                        <label>
                            管辖分组：
                        </label>
                        <span class="w400" style="width: 650px;">
                            <label style="width: 650px; color: Red;">
                                <input id="cbk_all_beijing" type="checkbox" class="dx" />
                                北京全选
                            </label>
                            <span id="span_managebg_beijing"></span>
                            <label style="width: 650px; color: Red;">
                                <input id="cbk_all_xian" type="checkbox" class="dx" />
                                西安全选
                            </label>
                            <span id="span_managebg_xian"></span></span></li>
                </ul>
            </div>
            <div class="btn mt20">
                <input id="btnSave_Page2" type="button" onclick="SavePage2();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关 闭" class="btnCannel bold" onclick="ClosePage();" />
            </div>
        </div>
    </div>
</div>
