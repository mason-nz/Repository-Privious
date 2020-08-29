<%@ Page Title="免打扰" Language="C#" AutoEventWireup="true" CodeBehind="NoDisturbLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.NoDisturbLayer" %>
     
<!--新增免打扰-->
<div class="pop_new w600 openwindow">
    <div class="title bold">
        <h2> <%=TitleString %><span><a hidefocus="true" href="javascript:void(0)" onclick="javascript:$.closePopupLayer('UpdateBlackDataAjaxPopup');"></a></span></h2></div>
    <div class="set_edit set_ndr">
        <ul>
            <li class="ndr_tel">
                <label>
                    <span class="redcolor">*</span>电话号码：</label><span><input name="" type="text" class="w150" id="edit_txtPhoneNum" runat="server" /></span></li>
            <li class="ndr_hw">
                <label>话务ID：</label><span><input name="" type="text" class="w150" style=" width:148px;" id="txtCallID" runat="server"/></span></li>
            <li>
                <label>
                    <span class="redcolor">*</span>有效期：</label><span>
                    <label><input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi1" value="3" onclick="checkYouXiaoQi(3)"/>三个月</label>
                    <label><input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi2" value="6" onclick="checkYouXiaoQi(6)"/>半年</label>
                    <label><input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi4" value="12" onclick="checkYouXiaoQi(12)"/>一年</label>
                    <label style=" width:50px;"><input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi5" value="" onclick="checkYouXiaoQi(1)"/>其它</label>
                    <input type="text" value="0" class="w300" style="width: 102px;" id="edit_txtYouXiaoQi" runat="server"/></span></li>
            <li>
                <label>
                    <span class="redcolor">*</span>类型：</label><span>
                    <label><input value="1" name="ckb_calltype" type="checkbox" id="edit_ckb_calltype_1" runat="server"/>呼入</label>
                    <label><input value="2" name="ckb_calltype" type="checkbox" id="edit_ckb_calltype_2" runat="server"/>呼出</label></span></li>
            <li id="liSelNoDisturbReason">
                <label>
                    <span class="redcolor">*</span>原因：</label>
                    <span>
                        <select class="w410" id="selNoDisturbReason" runat="server" ></select>
                    </span></li>
            <li>
                <label>
                    备注：</label><span><textarea  id="textearea_reason" name="" cols="" rows=""></textarea></span></li>
        </ul>
    </div>
    <div class="clearfix">
    </div>
    <div class="option_button btn">
        <input name="" type="button" value="保存" onclick="javascript:SaveEditData();"/>
        <input name="" type="button" value="取消" onclick="javascript:$.closePopupLayer('UpdateBlackDataAjaxPopup');"/>
    </div>
    <div class="clearfix">
    </div>
</div>
<!--新增免打扰-->
    <script type="text/javascript">
        //获取多选值
        function GetMutilSelectValues(key, value) {
            var totalNum = 0;
            var phoneNum = $.trim($("#edit_txtPhoneNum").val());
            $("input[" + key + "='" + value + "']").each(function () {
                if ($(this).attr("checked")) {
                    totalNum = totalNum + parseInt($(this).val());
                }
            });
            return totalNum;
        }
        function GetMutilSelectValues2(key, value) {
            var ids = $("input[" + key + "='" + value + "']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            return ids;
        }
        //保存按钮
        function SaveEditData() { 
            var callid = $.trim($("#txtCallID").val());
            var nodisturbreason = $("#selNoDisturbReason").val();

            var phoneNum = $.trim($("#edit_txtPhoneNum").val());
            var callType = 0
            if ($("#edit_ckb_calltype_1").attr("checked")) {
                callType += parseInt($("#edit_ckb_calltype_1").val())
            }
            if ($("#edit_ckb_calltype_2").attr("checked")) {
                callType += parseInt($("#edit_ckb_calltype_2").val());
            }
            var youXiaoQi = $.trim($('input:radio[name="rd_youxiaoqi"]:checked').val());
            if (youXiaoQi == 0) {
                youXiaoQi = $.trim($("#edit_txtYouXiaoQi").val());
            }


            var businessType = GetMutilSelectValues("name", "cb_BussinessTyoe");
            var reason = $.trim($("#textearea_reason").val());

            if (callid != '' && !isNum(callid)) {
                $.jAlert("话务ID格式不正确");
                $("#txtCallID").val('');
                return;
            }

            //非空校验
            if (phoneNum == '') {
                $.jAlert("请填写电话号码", function () { $("#edit_txtPhoneNum").focus(); });
                return;
            }
            if (!isNum(phoneNum)) {
                $.jAlert("电话号码只能由数字组成", function () { $("#edit_txtPhoneNum").focus(); });
                return;
            }
            if (callType == 0) {
                $.jAlert("请选择类型");
                return;
            }
            if (youXiaoQi == undefined || youXiaoQi == "") {
                $.jAlert("请选择有效期");
                return;
            }

            if ($("#edit_ckb_calltype_1").attr("checked")) {
                if (businessType == undefined || businessType == "") {
                    $.jAlert("请选择对应业务");
                    return;
                }
            }
            if ($("#edit_ckb_calltype_2").attr("checked")) {
                if (nodisturbreason <= 0) {
                    $.jAlert("请选择原因");
                    return;
                }
            }
            if (!$("#edit_ckb_calltype_1").attr("checked")) {
                businessType = 0;
            }
            if (!$("#edit_ckb_calltype_2").attr("checked")) {
                nodisturbreason = 0;
            }

            //名称重复校验
            var cdid2 = GetMutilSelectValues2("name", "cb_BussinessTyoe");

            var pody = {
                Action: '<%=strAction%>',
                RecId: '<%=strRecID%>',
                Type: '0',
                PhoneNum: phoneNum,
                CallType: callType,
                ExpiryDate: youXiaoQi,
                CDIDS: businessType,
                CDIDS2: cdid2,
                Reason: reason,
                CallID: callid,
                NoDisturbReason: nodisturbreason,
                r: Math.random()
            }

            AjaxPostAsync("/BlackWhite/BlackWhiteHandler.ashx", pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    $.closePopupLayer('UpdateBlackDataAjaxPopup', true);
                    if ("<%=ResponseFrom%>" != "plugin") {
                        $.jPopMsgLayer("操作成功！", function () {
                            search();
                        }); //根据查询条件重新查询数据
                    }
                    else {
                        $.jPopMsgLayer("操作成功！");
                    }
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }
        //补零
        function popAddZero(value) {
            return value < 10 ? "0" + value : value;
        }

        $(function () {
            $("#textearea_reason").hover(function () {
                $(this).css("background", "none repeat scroll 0 0 #FFC");
            }, function () {
                $(this).css("background", "");
            });
            if ('<%=ResponseFrom%>' == 'plugin') {
                $("#edit_txtPhoneNum").attr("disabled", true);
                $("#txtCallID").attr("disabled", true);
            }
            addSelectListInit();
            var nowDate = new Date();
            var now = nowDate.getFullYear() + "-" + popAddZero(nowDate.getMonth() + 1) + "-" + popAddZero(nowDate.getDate());
            $('#edit_txtYouXiaoQi').bind('click focus', function () { WdatePicker({ minDate: now }); });

            var cdid = '<%=cdids%>';
            if (cdid != "0") {
                $("#addSelBusinessType input:checkbox[name='cb_BussinessTyoe']").each(function () {
                    if ((parseInt($(this).val()) & parseInt(cdid)) == parseInt($(this).val())) {
                        $(this).attr("checked", true);
                    }
                });
            }
            $("#textearea_reason").val('<%=strReason%>');

            if ($("#edit_txtYouXiaoQi").val() != "") {
                $("#edit_txtYouXiaoQi").attr("disabled", false);
                $("#edit_rd_youxiaoqi5").attr("checked", true);
            }
            //呼入类型，业务线选项显示
            $("#edit_ckb_calltype_1").change(function () {
                if ($("#edit_ckb_calltype_1").attr("checked")) {
                    $("#liSelBusinessType").css("display", "");
                    if ($("#edit_ckb_calltype_2").attr("checked")) {
                        $("#liSelNoDisturbReason").css("display", "");
                    }
                    else {
                        $("#liSelNoDisturbReason").css("display", "none");
                    }
                }
                else {
                    $("#liSelBusinessType").css("display", "none");
                }
            });
            //呼出类型，原因选项显示
            $("#edit_ckb_calltype_2").change(function () {
                if ($("#edit_ckb_calltype_2").attr("checked")) {
                    $("#liSelNoDisturbReason").css("display", "");
                    if ($("#edit_ckb_calltype_1").attr("checked")) {
                        $("#liSelBusinessType").css("display", "");
                    }
                    else {
                        $("#liSelBusinessType").css("display", "none");
                    }
                }
                else {
                    $("#liSelNoDisturbReason").css("display", "none");
                }
            });

            $("#edit_ckb_calltype_1").change();
            $("#edit_ckb_calltype_2").change();

            if ('<%=strAction%>' == 'AddNoDisturbData') {
                $("#edit_rd_youxiaoqi1").attr("checked", true);
            }
        });
        function addSelectListInit() {
            var str = TelNumManag.GetCheckboxes();
            $("#addSelBusinessType").append(str).find("span").css("width", "120px");
        }
        function checkYouXiaoQi(num) {
            if (num == 1) {
                $("#edit_txtYouXiaoQi").attr("disabled", false);
                $("#edit_rd_youxiaoqi5").attr("checked", true);
            }
            else {
                var now = '<%=DateNow%>'
                $("#edit_txtYouXiaoQi").val(addMoth(now, num));
                $("#edit_txtYouXiaoQi").attr("disabled", true);
                $("#edit_rd_youxiaoqi" + (num / 3)).attr("checked", true);
            }
        }
        function addMoth(d, m) {
            var ds = d.split('-'), _d = ds[2] - 0;
            var nextM = new Date(ds[0], ds[1] - 1 + m + 1, 0);
            var max = nextM.getDate();
            var d = new Date(ds[0], ds[1] - 1 + m, _d > max ? max : _d);
            // return d.toLocaleDateString().match(/\d+/g).join('-')
            return d.getFullYear() + "-" + popAddZero(d.getMonth() + 1) + "-" + popAddZero(d.getDate());
        }
 
    </script>