<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustCategory.DealerInfo" %>
<script type="text/javascript" language="javascript">
    //验证
    function DataCheckDealer() {
        var txtRemark = $("#txtRemark").val();
        var msg = "";
        if (txtRemark != "") {
            if (GetStringRealLength(txtRemark) > 200) {
                msg += "备注超长! <br/>";
            }
        }
        if ($("#txtName").val() == "") {
            msg += "经销商不能为空！<br/>";

        }
        if ($("#txtName").val() != "") {
            if (GetStringRealLength($("#txtName").val()) > 150) {
                msg += "经销商名称超长! <br/>";
            }
        }
        return msg;
    }
    function subDealerInfo(CustID) {
        var returnValue = false;
        var Name = $("#txtName").val();
        var Remark = $("#txtRemark").val();
        var MemberCode = $("#txtMemberID").val();
        var MemberType = $("select[id$='memberType']").val();
        var CityScope = $("input[name='CityScope']:checked").val();

        if (CityScope == undefined) {
            CityScope = "";
        }

        var CarType = $("input[name='CarType']:checked").val();

        if (CarType == undefined) {
            CarType = "";
        }

        var MemberStatus = $("input[name='MemberStatus']:checked").val();

        if (MemberStatus == undefined) {
            MemberStatus = "";
        }

        var CustID = CustID;
        var BrandID = $("#hidBrand").val();
        params = {
            Action: "SubDealerInfo",
            MemberName: escape(Name),
            Remark: escape(Remark),
            MemberCode: encodeURIComponent(MemberCode),
            MemberType: encodeURIComponent(MemberType),
            CityScope: encodeURIComponent(CityScope),
            CarType: encodeURIComponent(CarType),
            MemberStatus: encodeURIComponent(MemberStatus),
            CustID: encodeURIComponent(CustID),
            BrandID: encodeURIComponent(BrandID)
        }
        AjaxPostAsync("../AjaxServers/CustCategory/DealerInfo.ashx", params, null,
                    function (data) {
                        if (data == 'success') {
                            returnValue = true;
                        }
                        else {
                            $.jAlert(data);
                            returnValue = false;
                        }
                    });
        return returnValue;
    }

    $(document).ready(function () {

        //给字段端赋值
        var MemberName = $("#<%=hid_MemberName.ClientID%>").val();
        if (MemberName != "") {
            $('#txtName').val(MemberName);
            getYipaiMember('<%=MemberCode%>');
        }
        var MemberCode = '<%=MemberCode%>';
        if (MemberCode != "") {
            $('#txtMemberID').val(MemberCode);

        }
        var Remark = '<%=Remark%>';
        if (Remark != "") {
            $('#txtRemark').val(Remark);
        }

        var form = "<%=RequestFrom %>";
        if (form == "CTI") {
            //ADTTool.LogonTime("time", "经销商控件加载完成 " + "<%=logmsg %>");
        }
    });
    //选择经销商
    function selectMemberInfo() {
        var MemberName = $.trim($("#txtName").val())
        $.openPopupLayer({
            name: "DealerSelectAjaxPopup",
            parameters: { MemberName: escape(MemberName) },
            url: "../AjaxServers/CustCategory/DealerSelect.aspx",
            beforeClose: function () {
                var MemberName = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberName');
                //var MemberType = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberType');
                //var BrandNames = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('BrandNames');
                //var Brandids = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('Brandids');
                var MemberCode = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('MemberCode');

                var CustID = $('#popupLayer_' + 'DealerSelectAjaxPopup').data('CustID');
                //给经销商工单初始化客户信息
                if (typeof GetCustInfo == "function") {
                    GetCustInfo(CustID);
                }
                $("#txtName").attr("value", MemberName);



                //暂时注释，呼入呼出上线后恢复  By Chybin At 2013-1-5
                //品牌
                // Delete By  Chybin $("#txtBrand").attr("value", BrandNames);// Delete By  Chybin
                //品牌id
                //                $("#hidBrand").attr("value", Brandids);// Delete By  Chybin
                $("#txtMemberID").attr("value", MemberCode);

                //新增后如果经销商有负责人则显示“重置密码”按钮
                if (MemberCode != undefined) {
                    getYipaiMember(MemberCode);
                }
            }
        });
    }
    //根据经销商编号得到邮箱信息
    function getYipaiMember(memberCode) {
        //update 13.7.15 lxw 暂时不用，先全部注释掉！
        //update 13.8.9 lxw 启用
        //update 2013.8.22 masj 启用，调用易湃接口返回账号名称

        $("#aResetPwd").hide();
        $("#hidEmail").val("");
        $("#hidMemberCode").val(memberCode);

        if (memberCode != "") {
            AjaxPost("/AjaxServers/CustCategory/DealerInfo.ashx", { Action: "GetYiPaiMember", MemberCode: memberCode, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "true") {
                    $("#aResetPwd").show();
                    $("#hidEmail").val(jsonData.Email);
                }
            });
        }
    }
    //重置密码弹出层
    function GetResetPwd(memberCode) {
        var email = $("#hidEmail").val();
        var memberCode = $("#hidMemberCode").val();
        $.openPopupLayer({
            name: "ResetPwdPopup",
            parameters: { Email: email, MemberCode: memberCode, r: Math.random() },
            url: "/CustCategory/ResetPwd.aspx",
            beforeClose: function () {
                var newEmail = $("#popupLayer_" + "ResetPwdPopup").data("email");
                if (newEmail != undefined) {
                    $("#hidEmail").val(newEmail);
                }
            }
        });
    }
</script>
<input type="hidden" id="hid_MemberName" runat="server" />
<ul class="">
    <li style="width: 464px;">
        <label>
            <span class="redColor">*</span>经销商名称：</label><span>
                <input type="text" id="txtName" class="w250" /><a id="btnselect" onclick="selectMemberInfo()"
                    style="cursor: pointer;">&nbsp;查询</a> <a id="aResetPwd" onclick="GetResetPwd()" style="cursor: pointer;">
                        重置密码</a>
                <input type="hidden" id="hidEmail" value="" />
                <input type="hidden" id="hidMemberCode" value="" /></span></li>
    <li style="display: none">
        <label>
            城市范围：</label>
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CityScope_10001" value="10001" type='radio' name="CityScope" />111城区</label></span>&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CityScope_10002" value="10002" type='radio' name="CityScope" />111郊区</label></span>&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CityScope_10003" value="10003" type='radio' name="CityScope" />6全城</label></span>&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CityScope_10004" value="10004" type='radio' name="CityScope" />224全城</label></span>
    </li>
    <li style="display: none">
        <label>
            经销商类型：</label><span><select id="memberType" class="w255"><option value="-2" selected="selected">
                请选择</option>
                <option value="20004">4s</option>
                <option value="20005">特许经销商</option>
                <option value="20006">综合店</option>
                <option value="20010">经纪公司</option>
            </select></span>
        <div id="MemberTypeDiv">
        </div>
    </li>
    <li style="display: none">
        <label>
            经营范围：</label><span>
                <label style="float: none; cursor: pointer">
                    <input id="CarType_30001" value="30001" type='radio' name="CarType" />新车</label></span>&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CarType_30002" value="30002" type='radio' name="CarType" />二手车</label></span>&nbsp;
        <span>
            <label style="float: none; cursor: pointer">
                <input id="CarType_30003" value="30003" type='radio' name="CarType" />新车/二手车</label>
        </span></li>
    <li style="display: none">
        <label>
            品牌：</label><span>
                <input type="text" id="txtBrand" class="w190" disabled="disabled" /><input type="hidden"
                    id="hidBrand" /></span>&nbsp;<span><input type="button" id="SelectBrand" class="btnChoose"
                        style="cursor: pointer" value="选择" /></span> </li>
    <li style="display: none">
        <label>
            会员ID：</label><span>
                <input type="text" id="txtMemberID" class="w250" /></span> </li>
    <li style="display: none">
        <label>
            经销商状态：</label>
        <span>
            <input id="MemberStatus_40001" value="40001" type='radio' name="MemberStatus" />会员页</span>&nbsp;<span><input
                id="MemberStatus_40002" value="40002" type='radio' name="MemberStatus" />旺店页</span>&nbsp;<span><input
                    id="MemberStatus_40003" value="40003" type='radio' name="MemberStatus" />待创建
        </span></li>
    <li style="width: 940px">
        <label style="width: 120px">
            备注：</label><span>
                <input type="text" id="txtRemark" class="w250" style="width: 706px" /></span>
    </li>
</ul>
