<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderInfo" %>
<script type="text/javascript">
    //工单记录限制长度
    var OrderContentMaxLen = 1000;

    $(document).ready(function () {
        //下拉框选项数据库初始化
        WOrderInfo.InitData();
        //控件是否可以编辑
        WOrderInfo.UIEdit();
        //控件联动
        WOrderInfo.UILink();
        //事件
        WOrderInfo.Event();

        //测试按钮
        $("#a_test_bot").css("display", "none");
    });

    //个人用户实体类
    var WOrderInfo = {};
    //下拉框选项数据库初始化
    WOrderInfo.InitData = function () {
        AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx",
            {
                Action: "GetSelectOption",
                R: Math.random()
            }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result) {
                    //工单来源
                    DIVSelect.Init("div_datasource", jsonData.data.sourcedt, "请选择工单来源");
                    //工单类型
                    DIVSelect.Init("div_category", jsonData.data.categorydt, "请选择工单类型", function () { WOrderInfo.CustTypeCategoryChanged(); });
                    //业务类型
                    DIVSelect.Init("div_businesstype", jsonData.data.businessdt, "请选择业务类型", function () { WOrderInfo.BusinessTypeChanged(); });
                    //访问分类
                    DIVSelect.Init("div_visittype", jsonData.data.visitdt, "请选择访问分类");
                    //是否接通
                    DIVSelect.Init("div_isjietong", jsonData.data.isjietongdt, "请选择是否接通", function () { WOrderInfo.IsJieTongChanged(); });
                    //未接通原因
                    DIVSelect.Init("div_nojtreason", jsonData.data.nojietongdt, "请选择未接通原因");
                    //投诉级别
                    DIVSelect.Init("div_complaintlevel", jsonData.data.complaintdt, "请选择投诉级别");
                }
            });
        //初始化标签控件
        WOrderInfo.TagControl = new TagSelectControl({ tag: "inp_businesstag" });
        //上传控件初始化
        UploadifyControl.Init("uploadDiv", "上传附件", "uploadpos", "<%=(int)BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath.WorkOrder %>");
    };
    //控件是否可以编辑
    WOrderInfo.UIEdit = function () {
        //设置业务线来源
        DIVSelect.SetVal("div_datasource", Common.Params.DataSource);
        //设置工单分类
        DIVSelect.SetVal("div_category", Common.Params.Category);
        //设置业务线来源是否可以编辑
        DIVSelect.SetEnabledOrDisabled("div_datasource", Common.Params.IsDataSourceCanModify == "True");
        //设置工单分类是否可以编辑
        DIVSelect.SetEnabledOrDisabled("div_category", Common.Params.IsCategoryCanModify == "True");
        //设置业务类型
        DIVSelect.SetVal("div_businesstype", Common.Params.BusinessType);
        //设置业务标签
        WOrderInfo.TagControl.SetBusiTypeId(Common.Params.BusinessType);
        WOrderInfo.TagControl.SetTagNameById(Common.Params.BusinessTag);
        //手机号码限制长度15
        InitInputMaxLengthForDigit("inp_contacttel", 15);
        //用户姓名限制长度10
        InitInputMaxLength("inp_contactname", 10);
        //工单内容限制长度1000
        InitInputMaxLength("text_content", OrderContentMaxLen);
    };
    //控件联动
    WOrderInfo.UILink = function () {
        //文本框绑定提示
        $("#inp_contactname").bindTextDefaultMsg("联系人姓名");
        $("#inp_contacttel").bindTextDefaultMsg("联系人电话");
        $("#inp_businesstag").bindTextDefaultMsg("请选择标签");
        $("#text_content").bindTextDefaultMsg("工单记录");

        //注册客户类型改变事件+注册工单类型改变事件
        CustBaseInfo.CustTypeChanged(function (custypeid) {
            //触发【客户类型，工单类型改变事件】
            WOrderInfo.CustTypeCategoryChanged();
        });
        //注册添加联系人事件
        $("#a_addcontact").click(function () {
            var oper = $("#a_addcontact").attr("oper");
            if (oper == "add") {
                WOrderInfo.SetContactControlShow(true);
                $("#a_addcontact").attr("oper", "del");
            }
            else {
                WOrderInfo.SetContactControlShow(false);
                $("#a_addcontact").attr("oper", "add");
            }
        });
        //注册是否同步访问记录事件
        $("#cbx_syscrm").change(function () {
            var value = $("#cbx_syscrm").attr("checked");
            $("#sp_visittype").text(value ? "*" : "　");
        });

        //首次主动触发【客户类型+工单类型改变事件】
        WOrderInfo.CustTypeCategoryChanged();
        //首次主动触发【业务类型改变事件】
        WOrderInfo.BusinessTypeChanged();
        //首次主动触发【是否接通改变事件】
        WOrderInfo.IsJieTongChanged();
        //首次主动触发【添加联系人事件】
        WOrderInfo.SetContactControlShow(false);
        //首次主动触发【是否同步访问记录事件】
        $("#cbx_syscrm").trigger("change");
    };

    //客户类型+工单类型改变事件
    WOrderInfo.CustTypeCategoryChanged = function () {
        //客户类型
        var custypeid = CustBaseInfo.GetCustTypeID();
        //工单类型
        var categoryid = DIVSelect.GetVal("div_category", 1);

        //默认设置
        //业务线必填
        $("#sp_businesstype").text("*");
        //标签必填
        $("#sp_businesstag").text("*");
        //隐藏访问分类
        $("#li_visittype").css("display", "none");
        //隐藏是否接通
        $("#li_isjietong").css("display", "none");
        //隐藏投诉级别
        $("#li_complaintlevell").css("display", "none");
        //隐藏是否同步
        $("#div_cbx_syscrm").css("display", "none");
        //回访类型
        if (categoryid == "<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W04_回访 %>") {
            //经销商类型
            if (custypeid == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>") {
                //业务线选填
                $("#sp_businesstype").text("　");
                //标签选填
                $("#sp_businesstag").text("　");
                //显示访问分类
                $("#li_visittype").css("display", "");
                //显示是否接通
                $("#li_isjietong").css("display", "");
                //显示是否同步
                $("#div_cbx_syscrm").css("display", "");
                //选择是否同步
                WOrderInfo.SetSyncCRM(true);
            }
        }
        //投诉
        else if (categoryid == "<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉 %>") {
            //显示投诉级别
            $("#li_complaintlevell").css("display", "");
        }
        //触发WOrderInfo.IsJieTongChanged事件 - 设置 未接通原因是否显示
        WOrderInfo.IsJieTongChanged();
    };
    //业务类型改变事件
    WOrderInfo.BusinessTypeChanged = function () {
        //业务类型
        var businesstype = DIVSelect.GetVal("div_businesstype", 1);
        //设置标签控件的业务类型
        WOrderInfo.TagControl.SetBusiTypeId(businesstype);
        //设置标签控件的鼠标手势
        if (businesstype == -1) {
            $("#inp_businesstag").css("cursor", "default");
            $("#mid_inp_businesstag label.u-label").css("cursor", "default");
        }
        else {
            $("#inp_businesstag").css("cursor", "pointer");
            $("#mid_inp_businesstag label.u-label").css("cursor", "pointer");
        }
    };
    //是否接通改变事件
    WOrderInfo.IsJieTongChanged = function () {
        //是否接通
        var isjietong = DIVSelect.GetVal("div_isjietong", 1);
        //是否显示
        var show = $("#li_isjietong").css("display") != "none";
        if (isjietong == 0 && show) {
            //否+是否接通显示
            $("#li_nojtreason").css("display", "");
        }
        else {
            //是+请选择
            $("#li_nojtreason").css("display", "none");
        }

        //设置添加联系人特殊样式
        WOrderInfo.SetAddContactCss();
    };
    //客户回访设置是否接通
    WOrderInfo.SetIsJieTong = function (event, IsEstablished) {
        //是否显示
        var show = $("#li_isjietong").css("display") != "none";
        if (show) {
            var isjietong = true;
            if (event == "接通") {
                //设置是否接通=true
                DIVSelect.SetVal("div_isjietong", 1);
                //触发是否接通变化事件
                WOrderInfo.IsJieTongChanged();
                //是否接通不可编辑
                DIVSelect.Disabled("div_isjietong");
            }
            else if (event == "挂断") {
                //是否接通
                var old_isjt = DIVSelect.GetVal("div_isjietong", 1);
                var IsEstablished_value = IsEstablished ? 1 : 0;
                if (old_isjt != IsEstablished_value) {
                    DIVSelect.SetVal("div_isjietong", IsEstablished_value);
                    WOrderInfo.IsJieTongChanged();
                }
            }
        }
    }
    //设置联系人的显示和隐藏
    WOrderInfo.SetContactControlShow = function (show) {
        if (show) {
            $("#a_addcontact").text("删除联系人");
            $("#li_inp_contactname").css("display", "");
            $("#li_inp_contacttel").css("display", "");
        }
        else {
            $("#a_addcontact").text("添加联系人");
            $("#li_inp_contactname").css("display", "none");
            $("#li_inp_contacttel").css("display", "none");
        }
    }
    //设置添加联系人特殊样式
    WOrderInfo.SetAddContactCss = function () {
        //li_a_addcontact
        var ul = $("#WOrderInfo_UL");
        var lis = ul.find("li");
        var count = 0;
        for (var i = 0; i < lis.length; i++) {
            var li = $(lis[i]);
            if (li.attr("id") == "li_a_addcontact") {
                break;
            }
            else {
                if (li.css("display") != "none") {
                    count++;
                }
            }
        }
        if (count % 2 == 0) {
            //独占一行
            $("#li_a_addcontact").css("width", "350px");
            $("#li_a_addcontact").css("margin-left", "8px");
        }
        else {
            //非独占一行
            $("#li_a_addcontact").css("width", "150px");
            $("#li_a_addcontact").css("margin-left", "20px");
        }
    }
    //设置同步CRM值
    WOrderInfo.SetSyncCRM = function (yesno) {
        $("#cbx_syscrm").attr("checked", yesno);
        //主动触发【是否同步访问记录事件】
        $("#cbx_syscrm").trigger("change");
    };

    //取值
    WOrderInfo.Read = function () {
        //客户类型
        var custypeid = CustBaseInfo.GetCustTypeID();
        //工单类型
        var categoryid = DIVSelect.GetVal("div_category", 1);
        //判断条件
        var ishuifang = categoryid == "<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W04_回访 %>";
        var istousu = categoryid == "<%=(int)BitAuto.ISDC.CC2012.Entities.WOrderCategoryEnum.W03_投诉 %>";
        var isjxs = custypeid == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>";
        var addcontact = $("#a_addcontact").attr("oper") == "del";

        //读取值
        var result = true;
        var msg = "";
        var error_ui = null;
        var data = {
            DataSource: $.trim(DIVSelect.GetVal("div_datasource", 1)), //工单来源
            Category: $.trim(DIVSelect.GetVal("div_category", 1)), //工单分类
            BusinessType: $.trim(DIVSelect.GetVal("div_businesstype", 1)), //业务类型
            BusinessTag: $.trim(WOrderInfo.TagControl.GetTagId()), //标签
            VisitType: ishuifang && isjxs ? $.trim(DIVSelect.GetVal("div_visittype", 1)) : "-1", //访问分类
            IsJieTong: ishuifang && isjxs ? $.trim(DIVSelect.GetVal("div_isjietong", 1)) : "-1", //是否接通
            NoJtReason: ishuifang && isjxs && $.trim(DIVSelect.GetVal("div_isjietong", 1)) == 0 ? $.trim(DIVSelect.GetVal("div_nojtreason", 1)) : "-1", //未接通原因
            ComplaintLevel: istousu ? $.trim(DIVSelect.GetVal("div_complaintlevel", 1)) : "-1", //投诉级别
            ContactName: addcontact ? $.trim($("#inp_contactname").val()) : "", //联系人
            ContactTel: addcontact ? $.trim($("#inp_contacttel").val()) : "", //联系电话
            Content: $.trim($("#text_content").val()), //工单记录
            IsSysCRM: ishuifang && isjxs ? $("#cbx_syscrm").attr("checked") : false, //是否同步crm
            Attachment: UploadifyControl.GetUploadifyArr(), //附件集合
            CustTypeID: custypeid //冗余（cs校验用）
        };

        //工单记录长度校验
        if (data.Content.length > OrderContentMaxLen) {
            result = false;
            msg = "工单记录长度不能超过" + OrderContentMaxLen + "！";
            error_ui = $("#text_content");
        }
        //非空校验
        if (data.DataSource < 0) {
            result = false;
            msg = "请选择工单来源！";
            error_ui = $("#div_datasource");
        }
        else if (data.Category < 0) {
            result = false;
            msg = "请选择工单类型！";
            error_ui = $("#div_category");
        }
        else if (!(ishuifang && isjxs) && data.BusinessType < 0) {
            //除了回访经销商外，必填
            result = false;
            msg = "请选择业务类型！";
            error_ui = $("#div_businesstype");
        }
        else if (!(ishuifang && isjxs) && (data.BusinessTag < 0 || data.BusinessTag == "")) {
            //除了回访经销商外，必填
            result = false;
            msg = "请选择标签！";
            error_ui = $("#inp_businesstag");
        }
        else if ((ishuifang && isjxs) && data.IsSysCRM && data.VisitType < 0) {
            //回访经销商且同步时，必填
            result = false;
            msg = "请选择访问分类！";
            error_ui = $("#div_visittype");
        }
        else if ((ishuifang && isjxs) && data.IsJieTong < 0) {
            //回访经销商，必填
            result = false;
            msg = "请选择是否接通！";
            error_ui = $("#div_isjietong");
        }
        else if ((ishuifang && isjxs) && data.IsJieTong == 0 && data.NoJtReason < 0) {
            //回访经销商且未接通，必填
            result = false;
            msg = "请选择未接通原因！";
            error_ui = $("#div_nojtreason");
        }
        else if (istousu && data.ComplaintLevel < 0) {
            //投诉时，必填
            result = false;
            msg = "请选择投诉级别！";
            error_ui = $("#div_complaintlevel");
        }
        else if (addcontact && data.ContactName == "") {
            //有联系人时，必填
            result = false;
            msg = "请输入联系人姓名！";
            error_ui = $("#inp_contactname");
        }
        else if (addcontact && data.ContactTel == "") {
            //有联系人时，必填
            result = false;
            msg = "请输入联系人电话！";
            error_ui = $("#inp_contacttel");
        }
        else if (data.Content == "") {
            result = false;
            msg = "请输入工单记录！";
            error_ui = $("#text_content");
        }
        //有效性校验
        else if (addcontact && !isTelOrMobile(data.ContactTel)) {
            result = false;
            msg = "请填写正确的联系人电话号码！";
            error_ui = $("#inp_contacttel");
        }
        else if (ishuifang && isjxs && Common.Params.CRMCustID == "" && CustBaseInfo.Data.CRMCustID == "" && data.IsSysCRM) {
            //同步访问记录，必须存在CRM客户ID
            result = false;
            msg = "所属经销商没有所属客户，不能同步访问记录！";
            error_ui = $("#cbx_syscrm");
        }
        //提示
        if (result == false) {
            $.jAlert(msg, function () { error_ui.focus(); });
        }
        //数据
        WOrderInfo.Data = data;
        //校验结果
        WOrderInfo.Result = result;
        //失败原因
        WOrderInfo.Msg = msg;
        return result;
    };
    //事件
    WOrderInfo.Event = function () {
        $("#btn_add").click(function () {
            if (WOrderInfo.IsBtnEnable("btn_add")) {
                if (Common.Save && typeof Common.Save == 'function') {
                    Common.Save("添加");
                }
            }
        });
        $("#btn_turn").click(function () {
            if (WOrderInfo.IsBtnEnable("btn_turn")) {
                if (Common.Save && typeof Common.Save == 'function') {
                    Common.Save("转出");
                }
            }
        });
    };
    //测试
    WOrderInfo.Test = function () {

    };

    //设置按钮是否可用
    WOrderInfo.SetBtnEnable = function (enable) {
        $("#btn_turn").removeClass("btn_gary").removeClass("btn_blue");
        $("#btn_add").removeClass("btn_gary").removeClass("btn_blue");
        if (enable) {
            $("#btn_add").addClass("btn_blue").attr("switch", "on");
            $("#btn_turn").addClass("btn_blue").attr("switch", "on");
            $("#btn_add").attr("disabled", false);
            $("#btn_turn").attr("disabled", false);
        }
        else {
            $("#btn_add").addClass("btn_gary").attr("switch", "off");
            $("#btn_turn").addClass("btn_gary").attr("switch", "off");
            $("#btn_add").attr("disabled", true);
            $("#btn_turn").attr("disabled", true);
        }
    };
    //按钮是否可用
    WOrderInfo.IsBtnEnable = function (id) {
        return $("#" + id).attr("switch") == "on";
    };
    //对外接口-追加工单记录
    WOrderInfo.AppendContent = function (text) {
        var content = $.trim($("#text_content").val());
        if (content != "") {
            content = content + "\n";
        }
        content = content + text;
        if (content.length > OrderContentMaxLen) {
            content = content.substr(0, OrderContentMaxLen);
        }
        $("#text_content").val(content);
    };
</script>
<!--下半部分基本信息开始-->
<div class="leftcon_up mt5">
    <div class="border_up">
    </div>
    <!--下半部分内容开始-->
    <div class="border_center border_center2" id="div_leftcon_bot">
        <ul id="WOrderInfo_UL">
            <li class="name" id="li_datasource"><span class="spnw" id="sp_datasource">*</span>
                <div id="div_datasource">
                </div>
            </li>
            <li class="name ml15" id="li_category"><span class="spnw" id="sp_category">*</span>
                <div id="div_category">
                </div>
            </li>
            <li class="name mt8" id="li_businesstype"><span class="spnw" id="sp_businesstype">*</span>
                <div id="div_businesstype">
                </div>
            </li>
            <li class="name mt8 ml15" id="li_businesstag"><span class="spnw" id="sp_businesstag">
                *</span>
                <div class="bor_kun">
                    <div class="borleft2" id="left_inp_businesstag">
                    </div>
                    <div class="borcenter2 cursorhand" id="mid_inp_businesstag">
                        <input type="text" class="colorblack" id="inp_businesstag" readonly="readonly" />
                    </div>
                    <div class="borright2" id="right_inp_businesstag">
                    </div>
                </div>
            </li>
            <li class="name mt8" id="li_visittype"><span class="spnw" id="sp_visittype">*</span>
                <div id="div_visittype">
                </div>
            </li>
            <li class="name mt8 ml15" id="li_isjietong"><span class="spnw" id="sp_isjietong">*</span>
                <div id="div_isjietong">
                </div>
            </li>
            <li class="name mt8" id="li_nojtreason"><span class="spnw" id="sp_nojtreason">*</span>
                <div id="div_nojtreason">
                </div>
            </li>
            <li class="name mt8" id="li_complaintlevell"><span class="spnw" id="sp_complaintlevel">
                *</span>
                <div id="div_complaintlevel">
                </div>
            </li>
            <li class="name mt8 addlxrheight" id="li_a_addcontact">
                <div class="add_lxr">
                    <a href="javascript:void(0)" id="a_addcontact" oper="add">添加联系人</a></div>
            </li>
            <li class="name mt8 clear_hh" id="li_inp_contactname"><span class="spnw" id="sp_inp_contactname">
                *</span>
                <div class="bor_kun">
                    <div class="borleft2" id="left_inp_contactname">
                    </div>
                    <div class="borcenter2" id="mid_inp_contactname">
                        <input type="text" class="colorblack" id="inp_contactname" />
                    </div>
                    <div class="borright2" id="right_inp_contactname">
                    </div>
                </div>
            </li>
            <li class="name mt8 ml15" id="li_inp_contacttel"><span class="spnw" id="sp_inp_contacttel">
                *</span>
                <div class="bor_kun">
                    <div class="borleft2" id="left_inp_contacttel">
                    </div>
                    <div class="borcenter2" id="mid_inp_contacttel">
                        <input type="text" class="colorblack" id="inp_contacttel" />
                    </div>
                    <div class="borright2" id="right_inp_contacttel">
                    </div>
                </div>
            </li>
            <li class="name mt8 clear_hh"><span>*</span>
                <div class="bor_kun bor_kun4">
                    <div class="textarea_top">
                    </div>
                    <div class="textarea_center">
                        <textarea cols="" rows="" class="colorblack" id="text_content"></textarea>
                    </div>
                    <div class="textarea_bottom">
                    </div>
                </div>
            </li>
            <li class="clear_hh pt5">
                <div class="kj_option kj_option2 right" id="div_cbx_syscrm">
                    <label>
                        <input name="" type="checkbox" value="" id="cbx_syscrm" />
                        <em>同步至访问记录</em></label></div>
                <div class="kj_option left">
                    <div class="uploadify-otherbtn" style="display: none">
                        <a href="javascript:void(0)" id="a_addinput">快捷输入</a>
                    </div>
                    <div class="uploadify-othersplit" style="display: none">
                        |</div>
                    <div id="uploadDiv">
                    </div>
                </div>
                <div id="div_FileContent" class="uploadify-filecontent">
                </div>
            </li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn right">
            <input name="" type="button" value="转出" class="btn_gary" id="btn_turn" />
            <input name="" type="button" value="添加" class="btn_blue" id="btn_add" />
        </div>
    </div>
    <!--下半部分内容结束-->
    <div class="border_bottom">
        <a href="javascript:void(0)" onclick="WOrderInfo.Test();" id="a_test_bot" style="display: none;
            float: right; clear: both">测试</a>
    </div>
</div>
<!--下半部分基本信息结束-->
