<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWOrderInfo.aspx.cs"
    Title="新增工单" Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AddWOrderInfo" %>

<%@ Register Src="UserControl/CustBaseInfo.ascx" TagName="CustBaseInfo" TagPrefix="uc1" %>
<%@ Register Src="UserControl/WOrderInfo.ascx" TagName="WOrderInfo" TagPrefix="uc2" %>
<%@ Register Src="UserControl/DataListInfo.ascx" TagName="DataListInfo" TagPrefix="uc3" %>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增工单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="/Css/workorder/wo-base.css?v=1.4" rel="stylesheet" type="text/css" />
    <link href="/Css/workorder/wo-style.css?v=1.4" rel="stylesheet" type="text/css" />
    <link href="/Css/workorder/wo-uploadify.css?v=1.1" rel="stylesheet" type="text/css" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/wo-jquery.uploadify.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Js/jquery.divselect.1.0.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Js/bit.dropdownlist.js"></script>
    <!-----------------------------CC自定义js-------------------------------------------->
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("UserControl");
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //url参数类
            Common.Params = Common.GetURLParams();
            //通话中，禁用添加和转出按钮
            if (Common.Params.CallSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.CallSourceEnum.C01_呼入 %>") {
                WOrderInfo.SetBtnEnable(false);
            }
            else {
                WOrderInfo.SetBtnEnable(true);
            }
            //初始化电话控件
            HollyPhoneControl.Init("工单", "", null, null, Common.Params.CRMCustID);
            //注册个人用户信息获取方法
            HollyPhoneControl.SetInfoFunc(
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
            //注册接通事件
            HollyPhoneControl.SetEstablishedEvent(function () {
                //设置按钮不可用
                WOrderInfo.SetBtnEnable(false);
                //客户回访设置是否接通
                WOrderInfo.SetIsJieTong("接通");
                //设置同步crm=是
                WOrderInfo.SetSyncCRM(true);
            });
            //注册挂断保存成功事件
            HollyPhoneControl.SetReleaseEvent(function (jsondata) {
                //设置按钮可用
                WOrderInfo.SetBtnEnable(true);
                //客户回访设置是否接通
                WOrderInfo.SetIsJieTong("挂断", jsondata.CallData.IsEstablished == "True");
                //保存话务id
                CustBaseInfo.SaveCallIDToHidden(jsondata.CallData.CallID);
                //设置免打扰
                CustBaseInfo.SetMianDaRao();
                //保存成功//
                $.jPopMsgLayer("保存个人用户信息成功！");
            });
            //注册消息转发事件
            HollyPhoneControl.SetSendMsgToWindowsEvent(function (data) {
                WOrderInfo.AppendContent(data);
            });
            //客户回访进来时，要自动外拨号码(延时触发)
            if (Common.Params.CallSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.CallSourceEnum.C02_呼出 %>") {
                setTimeout(function () { HollyPhoneControl.CallOut(Common.Params.Phone); }, 500);
            }
        });

        //公共方法类
        var Common = {};
        //获取url参数
        Common.GetURLParams = function () {
            var params = {
                CallSource: "<%=(int)RequestInfo.CallSource %>",
                ModuleSource: "<%=(int)RequestInfo.ModuleSource %>",

                Phone: "<%=RequestInfo.Phone %>",
                IsPhoneCanModify: "<%=RequestInfo.IsPhoneCanModify %>",
                IsShowCallOutBtn: "<%=RequestInfo.IsShowCallOutBtn %>",

                CustType: "<%=(int)RequestInfo.CustType %>",
                IsCustTypeCanModify: "<%=RequestInfo.IsCustTypeCanModify %>",
                CRMCustID: "<%=RequestInfo.CRMCustID %>",

                DataSource: "<%=(int)RequestInfo.DataSource %>",
                Category: "<%=(int)RequestInfo.Category %>",
                IsDataSourceCanModify: "<%=RequestInfo.IsDataSourceCanModify %>",
                IsCategoryCanModify: "<%=RequestInfo.IsCategoryCanModify %>",

                BusinessType: "<%=RequestInfo.BusinessType %>",
                BusinessTag: "<%=RequestInfo.BusinessTag %>",
                RelatedID: "<%=RequestInfo.RelatedData %>",

                MaxName: "<%=RequestInfo.MaxName %>",
                MaxSex: "<%=RequestInfo.MaxSex %>",
                MaxMember: "<%=RequestInfo.MaxMember %>",

                CBName: "<%=RequestInfo.CBName %>",
                CBSex: "<%=RequestInfo.CBSex %>",
                CBProvince: "<%=RequestInfo.CBProvince %>",
                CBCity: "<%=RequestInfo.CBCity %>",
                CBCounty: "<%=RequestInfo.CBCounty %>",
                CBMember: "<%=RequestInfo.CBMember %>"
            };

            return params;
        }
        //设置控件的启用和禁用样式
        Common.SetUIEnableOrNotStyle = function (id, enable) {
            //每一个输入框分左中右3个图片，需要替换掉
            var left = "#left_" + id;
            var right = "#right_" + id;
            var mid = "#mid_" + id;

            var qiyong_left = "url(/Images/workorder/borleft2.jpg) no-repeat";
            var qiyong_right = "url(/Images/workorder/borright2.jpg) no-repeat";
            var qiyong_mid = "url(/Images/workorder/borcenter2.jpg) repeat-x";

            var jingyong_left = "url(/Images/workorder/borleft.jpg) no-repeat";
            var jingyong_right = "url(/Images/workorder/borright.jpg) no-repeat";
            var jingyong_mid = "url(/Images/workorder/borcenter.jpg) repeat-x";

            if (enable) {
                $(left).css("background", qiyong_left);
                $(right).css("background", qiyong_right);
                $(mid).css("background", qiyong_mid);

                $("#" + id).attr("readonly", "");
            }
            else {
                $(left).css("background", jingyong_left);
                $(right).css("background", jingyong_right);
                $(mid).css("background", jingyong_mid);

                $("#" + id).attr("readonly", "readonly");
            }
        }
        //调整左侧上下面板的高度
        Common.SetLeftPanelTopBotHeight = function (new_topheight) {
            var init_top_height = 154;
            var init_bot_height = 618;
            $("#div_leftcon_top").css("height", new_topheight + "px");
            $("#div_leftcon_bot").css("height", (init_top_height + init_bot_height - new_topheight) + "px");
        }
        //读取公共数据
        Common.Read = function () {
            var data = {
                CallSource: Common.Params.CallSource, //通话来源
                ModuleSource: Common.Params.ModuleSource, //功能来源
                CRMCustID: Common.Params.CRMCustID, //客户回访的id
                RelatedID: Common.Params.RelatedID //IM的对话id，未接来电的id，客户回访的联系人姓名
            };
            Common.Data = data;
            Common.Result = true;
            return true;
        };

        //添加+转出
        Common.Save = function (oper) {
            //设置按钮不可用
            WOrderInfo.SetBtnEnable(false);
            $.blockUI({ message: "正在保存工单，请等待..." });

            try {
                //读取数据
                if (Common.Read() && CustBaseInfo.Read() && WOrderInfo.Read()) {
                    //传入CRMCustID>用户手选CRMCustID
                    if (Common.Data.CRMCustID == "" && CustBaseInfo.Data.CRMCustID != "") {
                        Common.Data.CRMCustID = CustBaseInfo.Data.CRMCustID;
                    }
                    //工单数据
                    var MainData = {
                        Common: Common.Data,
                        CustBaseInfo: CustBaseInfo.Data,
                        WOrderInfo: WOrderInfo.Data,
                        Oper: oper
                    };
                    //构造数据
                    var pody = {
                        Action: "SaveWOrderInfo",
                        JsonData: escape(JSON.stringify(MainData)), //json数据
                        R: Math.random()
                    };
                    //同步保存
                    AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx", pody, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result) {
                            //$.jPopMsgLayer
                            $.jAlert("工单" + oper + "成功，关闭当前页面！", function () { ClosePageForModule(); });
                        }
                        else {
                            $.jAlert(jsonData.message, function () { });
                        }
                    });
                }
            }
            catch (e) {
                $.jAlert("发生异常：" + e);
            }
            //保存完毕
            $.unblockUI();
            //设置按钮可用
            WOrderInfo.SetBtnEnable(true);
        };
        //关闭后操作
        function ClosePageForModule() {
            if (Common.Params.ModuleSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.ModuleSourceEnum.M04_未接来电 %>") {
                closePageExecOpenerSearch("btnsearch");
            }
            else if (Common.Params.ModuleSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.ModuleSourceEnum.M02_工单 %>") {
                closePageExecOpenerSearch("btnsearch");
            }
            else if (Common.Params.ModuleSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.ModuleSourceEnum.M03_客户回访 %>") {
                closePageReloadOpener();
            }
            else {
                closePage();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="cc">
        <div class="head">
            <%=PageTitle%>
        </div>
        <div class="cont">
            <!--左开始-->
            <div class="leftcon">
                <uc1:CustBaseInfo ID="ucCustBaseInfo" runat="server" />
                <uc2:WOrderInfo ID="ucWOrderInfo" runat="server" />
            </div>
            <!--左结束-->
            <!--右开始-->
            <uc3:DataListInfo ID="ucDataListInfo" runat="server" />
            <!--右结束-->
        </div>
    </div>
    </form>
</body>
</html>
