<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustBaseInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.CustBaseInfo" %>
<script type="text/javascript">
    $(document).ready(function () {
        //控件是否可以编辑
        CustBaseInfo.UIEdit();
        //控件赋值
        CustBaseInfo.UIValue();
        //控件联动
        CustBaseInfo.UILink();
        //注册按钮点击事件
        CustBaseInfo.Event();
        //插件调用初始化
        CustBaseInfo.InitData();

        //测试按钮
        $("#a_test_top").css("display", "none");
    });

    //个人用户实体类
    var CustBaseInfo = {};
    //客户类型变化事件(调用方注册)
    CustBaseInfo.CustTypeChangedEvent = null;
    //控件是否可以编辑
    CustBaseInfo.UIEdit = function () {
        //电话号码赋值
        $("#inp_phone").val(Common.Params.Phone);
        $("#inp_phone").attr("phoneNum", Common.Params.Phone);
        //第N次来电 呼入显示
        $("#spa_callnum").css("display", Common.Params.CallSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.CallSourceEnum.C01_呼入 %>" ? "" : "none");
        //电话号码是否可以编辑
        Common.SetUIEnableOrNotStyle("inp_phone", Common.Params.IsPhoneCanModify == "True");
        //电话号码是否可以外呼
        CustBaseInfo.CanCall(Common.Params.IsShowCallOutBtn == "True");
        //客户类型是否可以被修改
        $("input[name='rad_custtype']").attr("disabled", Common.Params.IsCustTypeCanModify == "True" ? "" : "disabled");
        //查询经销商是否被限制客户
        $("#inp_dealer").attr("LimitCRMCustID", Common.Params.CRMCustID);
        //性别默认：先生
        $("input[name='rad_sex'][value='1']").attr("checked", "checked");
        //客户类型赋值
        $("input[name='rad_custtype'][value='" + (Common.Params.CustType > 0 ? Common.Params.CustType : 4) + "']").attr("checked", "checked");
        //省市区控件初始化
        DIVSelect.InitProvinceCity("div_ProvinceCityCounty");
        //经销商控件初始化
        DealerSearchControl.InitialEvent("inp_dealer", "imp_dealer_search", "imp_dealer_goto", true, Common.Params.CRMCustID);
        //手机号码限制长度15
        InitInputMaxLengthForDigit("inp_phone", 15);
        //用户姓名限制长度10
        InitInputMaxLength("inp_name", 10);
        //经销商名称限制长度30
        InitInputMaxLength("inp_dealer", 30);
    };
    //电话号码是否可以外呼
    CustBaseInfo.CanCall = function (can) {
        //电话号码是否可以外呼
        $("#CallOut_inp_phone").css("display", can ? "" : "none");
        $("#mid_inp_phone").css("width", can ? "305px" : "270px");
        //电话号码隐藏式，需要设置额外的样式
        if (!can) {
            $("#inp_phone").attr("style", "width:196px; *width:192px;");
        }
    };
    //数据库赋值
    CustBaseInfo.UIValue = function () {
        //电话不用赋值
        //电话不为空，数据库读取赋值，电话为空，后台构造默认值
        //同步加载数据（必须同步加载 强斐 2016-8-8）
        AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx",
            {
                Action: "GetCBInfoByPhone",
                Phone: Common.Params.Phone,
                IsCalcN: Common.Params.CallSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.CallSourceEnum.C01_呼入 %>",
                R: Math.random()
            }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result) {
                    //（优先级：入口方法参数>数据库>Url备选参数>默认）
                    //第N次来电
                    $("#spa_callnum").text("第" + (jsonData.data.N_count + 1) + "次来电");

                    //数据库是否有值
                    var isindb = jsonData.data.CustID != "";
                    $("#hid_CustID").val(jsonData.data.CustID);
                    //姓名： 数据库>Url备选参数
                    $("#inp_name").val(Common.Params.MaxName != "" ? Common.Params.MaxName : (isindb ? jsonData.data.CustName : Common.Params.CBName));
                    //性别： 数据库>Url备选参数>默认
                    var sex = Common.Params.MaxSex > 0 ? Common.Params.MaxSex : (isindb ? jsonData.data.Sex : (Common.Params.CBSex > 0 ? Common.Params.CBSex : 1));
                    $("input[name='rad_sex'][value='" + sex + "']").attr("checked", "checked");
                    //客户类型： 入口方法参数>数据库>默认
                    var custtype = Common.Params.CustType > 0 ? Common.Params.CustType : (isindb ? jsonData.data.CustCategoryID : 4);
                    $("input[name='rad_custtype'][value='" + custtype + "']").attr("checked", "checked");
                    //省市区： 数据库>Url备选参数>默认
                    var province = isindb ? jsonData.data.ProvinceID : (Common.Params.CBProvince > 0 ? Common.Params.CBProvince : -1);
                    if (province == -1) province = jsonData.data.ProvinceID; //都不存在的情况下，根据手机号码解析
                    var city = isindb ? jsonData.data.CityID : (Common.Params.CBCity > 0 ? Common.Params.CBCity : -1);
                    if (city == -1) city = jsonData.data.CityID; //都不存在的情况下，根据手机号码解析
                    var county = isindb ? jsonData.data.CountyID : (Common.Params.CBCounty > 0 ? Common.Params.CBCounty : -1);
                    DIVSelect.SetValByProvinceCity("div_ProvinceCityCounty", province, city, county);
                    //经销商： 数据库>Url备选参数>默认
                    var member = Common.Params.MaxMember != "" ? Common.Params.MaxMember : (isindb ? jsonData.data.MemberCode : Common.Params.CBMember);
                    if (member != "") {
                        DealerSearchControl.SetMemberCode(member);
                    }
                    else {
                        DealerSearchControl.SetMemberName(jsonData.data.MemberName);
                    }
                }
            });
    };
    //控件联动
    CustBaseInfo.UILink = function () {
        //提示联动
        $("#inp_phone").bindTextDefaultMsg("用户号码");
        $("#inp_name").bindTextDefaultMsg("用户姓名");
        $("#inp_dealer").bindTextDefaultMsg("所属经销商");
        //个人经销商联动
        $("input[name='rad_custtype']").change(function () {
            //是否显示经销商控件
            CustBaseInfo.MemberControlIsShow();
            //设置免打扰
            CustBaseInfo.SetMianDaRao();
        });
        //首次主动触发【个人经销商联动】
        CustBaseInfo.MemberControlIsShow();
    };
    //是否显示经销商控件
    CustBaseInfo.MemberControlIsShow = function () {
        var value = $("input[name='rad_custtype']:checked").val();
        if (value == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T01_个人 %>") {
            $("#li_inp_dealer").css("display", "none");
            Common.SetLeftPanelTopBotHeight(115);
        }
        else if (value == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>") {
            $("#li_inp_dealer").css("display", "");
            Common.SetLeftPanelTopBotHeight(154);
        }
    };
    //手机号码联动省市控件
    CustBaseInfo.InpphoneFocusOut = function () {
        //blur 属性：phoneNum
        $("#inp_phone").focusout(function () {
            var phoneNum = $.trim($("#inp_phone").val());
            //焦点离去，号码有值
            if (phoneNum != "" && isTelOrMobile(phoneNum)) {
                //是手机号码并且已经发生变化
                if (phoneNum != $("#inp_phone").attr("phoneNum")) {
                    //重置电话号码
                    $("#inp_phone").attr("phoneNum", phoneNum);
                    AjaxPost("/AjaxServers/CTIHandler.ashx", { Action: "GetAreaID", PhoneNum: phoneNum, r: Math.random() }, null, function (data) {
                        if (data != "") {
                            var jsonData = eval("(" + data + ")");
                            DIVSelect.SetValByProvinceCity("div_ProvinceCityCounty", jsonData.ProvinceID, jsonData.CityID, -1);
                        }
                    });
                }
            }
            else {
                DIVSelect.SetValByProvinceCity("div_ProvinceCityCounty", -1, -1, -1);
                $("#inp_phone").attr("phoneNum", "");
            }
        });
    }
    //注册按钮点击事件
    CustBaseInfo.Event = function () {
        //手机号码联动省市控件
        CustBaseInfo.InpphoneFocusOut();
        //打电话按钮
        $("#CallOut_inp_phone").click(function () {
            HollyPhoneControl.CallOut($.trim($("#inp_phone").val()));
        });

    };
    //插件调用初始化
    CustBaseInfo.InitData = function () {
        //发送短信
        SendMessageControl.Init("工单", "", null, null, Common.Params.CRMCustID, null);
        //注册个人用户信息获取方法
        SendMessageControl.SetInfoFunc(
        //客户类型
        function () { return CustBaseInfo.ReadCustTypeID(); },
        //客户姓名
        function () { return CustBaseInfo.ReadCBName(); },
        //客户性别
        function () { return CustBaseInfo.ReadCBSex(); },
        //经销商id
        function () { return CustBaseInfo.ReadMemberCode(); },
        //经销商名称
        function () { return CustBaseInfo.ReadMemberName(); });
        //注册回调事件
        SendMessageControl.SetSendMessageCompleteEvent(function (id) {
            CustBaseInfo.SaveSmsIDToHidden(id);
        });
        //注册发送短信事件
        $("#Sms_inp_phone").click(function () {
            //清空SetTemplateFunc的设置
            SendMessageControl.SetTemplateFunc(null, null, null);
            //获取当前手机号
            var phone = $.trim($("#inp_phone").val());
            SendMessageControl.BtnSendMessageClick(phone, 2);
        });
        //免打扰
        NoDisturbLayerControl.InitialEvent("inp_phone", "Nod_inp_phone", function () {
            //获取最后一次话务
            var val = $.trim($("#hid_callids").val());
            if (val == "") return "";
            else {
                var array = val.split(",");
                if (array.length >= 1) {
                    return array[array.length - 1];
                }
                else return "";
            }
        });
        //设置免打扰
        CustBaseInfo.SetMianDaRao();
    };

    //取值
    CustBaseInfo.Read = function () {
        var result = true;
        var msg = "";
        var error_ui = null;
        //是否经销商
        var ismember = $.trim($("input[name='rad_custtype']:checked").val()) == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>";
        //读取值
        var data = {
            Phone: $.trim($("#inp_phone").val()), //电话
            CallIds: $.trim($("#hid_callids").val()), //通话ids
            SmsIds: $.trim($("#hid_smsids").val()), //短信ids
            CBName: $.trim($("#inp_name").val()), //姓名
            CBSex: $.trim($("input[name='rad_sex']:checked").val()), //性别
            ProvinceID: DIVSelect.GetVal("div_ProvinceCityCounty", 1), //省
            CityID: DIVSelect.GetVal("div_ProvinceCityCounty", 2), //市
            CountyID: DIVSelect.GetVal("div_ProvinceCityCounty", 3), //区县
            CustTypeID: $.trim($("input[name='rad_custtype']:checked").val()), //客户类别
            MemberCode: ismember ? DealerSearchControl.GetCRMMemberCode("inp_dealer") : "", //经销商id
            MemberName: ismember ? DealerSearchControl.GetCRMMemberName("inp_dealer") : "", //经销商名称
            CRMCustID: ismember ? DealerSearchControl.GetCRMCustID("inp_dealer") : "", //客户id
            CRMCustName: ismember ? DealerSearchControl.GetCRMCustName("inp_dealer") : "" //客户名称
        };
        //非空校验
        if (data.Phone == "") {
            result = false;
            msg = "请填写用户号码！";
            error_ui = $("#inp_phone");
        }
        else if (data.CBName == "") {
            result = false;
            msg = "请填写用户姓名！";
            error_ui = $("#inp_name");
        }
        else if (data.CBSex <= 0) {
            result = false;
            msg = "请选择用户性别！";
            error_ui = $("input[name='rad_sex']");
        }
        else if (data.ProvinceID <= 0) {
            result = false;
            msg = "请选择用户所在省份！";
            error_ui = $("#div_ProvinceCityCounty");
        }
        else if (data.CustTypeID <= 0) {
            result = false;
            msg = "请选择客户类别！";
            error_ui = $("input[name='rad_custtype']");
        }
        else if (ismember && data.MemberName == "") {
            result = false;
            msg = "请填写所属经销商！";
            error_ui = $("#inp_dealer");
        }
        //有效性校验
        else if (!isTelOrMobile(data.Phone)) {
            result = false;
            msg = "请填写正确的用户号码！";
            error_ui = $("#inp_phone");
        }
        //提示
        if (result == false) {
            $.jAlert(msg, function () { error_ui.focus(); });
        }
        //数据
        CustBaseInfo.Data = data;
        //校验结果
        CustBaseInfo.Result = result;
        //失败原因
        CustBaseInfo.Msg = msg;
        return result;
    };
    //获取客户类型
    CustBaseInfo.GetCustTypeID = function () {
        return $.trim($("input[name='rad_custtype']:checked").val());
    };
    //注册客户类型变化事件
    CustBaseInfo.CustTypeChanged = function (change) {
        if (change && typeof change == "function") {
            if (CustBaseInfo.CustTypeChangedEvent == null) {
                //首次注册，需要注册到change中
                $("input[name='rad_custtype']").change(function () {
                    //触发自定义的注册事件
                    if (CustBaseInfo.CustTypeChangedEvent && typeof CustBaseInfo.CustTypeChangedEvent == "function") {
                        CustBaseInfo.CustTypeChangedEvent($.trim($("input[name='rad_custtype']:checked").val()));
                    }
                });
            }
            //赋值到属性中
            CustBaseInfo.CustTypeChangedEvent = change;
        }
    };
    //免打扰按钮是否可用
    CustBaseInfo.SetMianDaRao = function () {
        var can = false;
        //是否有话务
        var hid_callids = $.trim($("#hid_callids").val()) != "";
        //是否经销商
        var ismember = $.trim($("input[name='rad_custtype']:checked").val()) == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>";
        //有话务且个人
        if (hid_callids == true && ismember == false) {
            //启用
            $("#Nod_inp_phone").attr("src", "/Images/workorder/hmd.png");
            $("#Nod_inp_phone").attr("disabled", false);
        }
        else {
            //禁用
            $("#Nod_inp_phone").attr("src", "/Images/workorder/hmd_gray.png");
            $("#Nod_inp_phone").attr("disabled", true);
        }
    };

    //对外接口-获取数据
    CustBaseInfo.ReadCBName = function () { return $.trim($("#inp_name").val()); };
    CustBaseInfo.ReadCBSex = function () { return $.trim($("input[name='rad_sex']:checked").val()); };
    CustBaseInfo.ReadCustTypeID = function () { return $.trim($("input[name='rad_custtype']:checked").val()); };
    CustBaseInfo.ReadMemberCode = function () {
        var ismember = $.trim($("input[name='rad_custtype']:checked").val()) == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>";
        return ismember ? DealerSearchControl.GetCRMMemberCode("inp_dealer") : "";
    };
    CustBaseInfo.ReadMemberName = function () {
        var ismember = $.trim($("input[name='rad_custtype']:checked").val()) == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>";
        return ismember ? DealerSearchControl.GetCRMMemberName("inp_dealer") : "";
    };

    //对外接口-保存话务ID
    CustBaseInfo.SaveCallIDToHidden = function (callid) {
        CustBaseInfo.SaveIDToHidden("#hid_callids", callid);
    };
    CustBaseInfo.SaveSmsIDToHidden = function (smsid) {
        CustBaseInfo.SaveIDToHidden("#hid_smsids", smsid);
    };
    CustBaseInfo.SaveIDToHidden = function (id, dataid) {
        var val = $.trim($(id).val());
        if (val == "") {
            $(id).val(dataid);
        }
        else {
            var newval = "," + val + ",";
            var pos = newval.indexOf("," + dataid + ",");
            if (pos == -1) {
                $(id).val(val + "," + dataid);
            }
        }
    };

    //测试
    CustBaseInfo.Test = function () {
        $("#hid_callids").val(1);
        CustBaseInfo.SetMianDaRao();
    };
</script>
<!--上半部分基本信息开始-->
<div class="leftcon_up">
    <div class="border_up">
    </div>
    <!--上内容开始-->
    <div class="border_center" id="div_leftcon_top">
        <ul>
            <li id="li_inp_phone"><span>*</span>
                <div class="bor_kun">
                    <div class="borleft" id="left_inp_phone">
                    </div>
                    <div class="borcenter" id="mid_inp_phone">
                        <input name="" type="text" value="" id="inp_phone" />
                        <img src="/Images/workorder/tel.png" border="0" alt="外呼" id="CallOut_inp_phone" />
                        <img src="/Images/workorder/msg.png" border="0" alt="短信" id="Sms_inp_phone" />
                        <img src="/Images/workorder/hmd.png" border="0" alt="免打扰" id="Nod_inp_phone" />
                    </div>
                    <div class="borright" id="right_inp_phone">
                    </div>
                    <span class="call_time" id="spa_callnum">第N次来电</span>
                </div>
            </li>
            <li class="name mt8" id="li_inp_name"><span>*</span>
                <div class="bor_kun">
                    <div class="borleft2" id="left_inp_name">
                    </div>
                    <div class="borcenter2" id="mid_inp_name">
                        <input name="txtName" type="text" id="inp_name" />
                    </div>
                    <div class="borright2" id="right_inp_name">
                    </div>
                </div>
            </li>
            <li class="name mt8 w120" id="li_rad_sex"><span>*</span>
                <label>
                    <input name="rad_sex" type="radio" value="1" /><em>先生</em></label>
                <label>
                    <input name="rad_sex" type="radio" value="2" /><em>女士</em></label>
            </li>
            <li class="name mt8 clear_hh"><span>*</span>
                <div id="div_ProvinceCityCounty">
                </div>
            </li>
            <li class="name mt8 w120" style="*margin-top: 18px;"><span>*</span>
                <label>
                    <input name="rad_custtype" type="radio" value="4" /><em>个人</em></label>
                <label>
                    <input name="rad_custtype" type="radio" value="3" /><em>经销商</em></label>
            </li>
            <li class="name name2 mt8 clear_hh" id="li_inp_dealer"><span>*</span>
                <div class="bor_kun">
                    <div class="borleft2" id="left_inp_dealer">
                    </div>
                    <div class="borcenter2 borcenter3" style="position: relative;" id="mid_inp_dealer">
                        <input id="inp_dealer" type="text" limitcrmcustid="" attrmembercode="" attrcustname=""
                            attrcustid="" />
                        <img src="/Images/workorder/search_jxs.png" border="0" alt="查询" id="imp_dealer_search" />
                        <img src="/Images/workorder/lock.png" border="0" alt="跳转" id="imp_dealer_goto" />
                    </div>
                    <div class="borright2" id="right_inp_dealer">
                    </div>
                </div>
            </li>
        </ul>
    </div>
    <!--上内容结束-->
    <div class="border_bottom">
        <a href="javascript:void(0)" onclick="CustBaseInfo.Test();" id="a_test_top" style="display: none;
            float: right">测试</a>
    </div>
    <!--隐藏域-->
    <input type="hidden" id="hid_CustID" des="个人用户ID（仅供参考）" />
    <input type="hidden" id="hid_callids" des="通话ID(只要在页面打过电话，均记录在此)" value="" />
    <input type="hidden" id="hid_smsids" des="短信ID(只要在页面发过短信，均记录在此)" value="" />
    <!--隐藏域-->
</div>
<!--上半部分基本信息结束-->
