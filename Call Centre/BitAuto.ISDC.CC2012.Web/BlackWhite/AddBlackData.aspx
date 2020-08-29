<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBlackData.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BlackWhite.AddBlackData" %>

<div class="pop pb15 popuser openwindow" >
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
            //   txtPhoneNum    ckb_calltype   cb_youxiaoqi   addSelBusinessType   textearea_reason
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

            //非空校验
            if (phoneNum == '') {
                $.jAlert("请填写电话号码", function () { $("#edit_txtPhoneNum").focus(); });
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
            if (businessType == undefined || businessType == "") {
                $.jAlert("请选择对应业务");
                return;
            }
            if (reason == '') {
                $.jAlert("请填写原因", function () { $("#textearea_reason").focus(); });
                return;
            }
            //名称重复校验
            var cdid2 = GetMutilSelectValues2("name", "cb_BussinessTyoe");
            
            var pody = {
                Action: '<%=strAction%>',
                RecId: '<%=RecId%>',
                Type: '0',
                PhoneNum: phoneNum,
                CallType: callType,
                ExpiryDate: youXiaoQi,
                CDIDS: businessType,
                CDIDS2: cdid2,
                Reason: reason,
                r: Math.random()
            }

            AjaxPostAsync("/BlackWhite/BlackWhiteHandler.ashx", pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    $.closePopupLayer('UpdateBlackDataAjaxPopup', false);
                    search(); //根据查询条件重新查询数据
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
        });
        function addSelectListInit() {
            var str = TelNumManag.GetCheckboxes();
            $("#addSelBusinessType").append(str);
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
    <div class="title bold">
        <h2>
            <%=TitleString %>
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('UpdateBlackDataAjaxPopup',false);">
        </a></span>
    </div>
    <div id="page1" class="moveC clearfix" style="margin-top: 5px;">
        <ul class="clearfix" style="padding-bottom: 10px;">
            <li >
                <label>
                    <span class="redColor">*</span>电话号码：
                </label>
                <span class="gh">
                    <input type="text" id="edit_txtPhoneNum" value='' class="w125" style="width: 200px;"
                        runat="server" />
                </span></li>
            <li >
                <label>
                    <span class="redColor">*</span>类型：
                </label>
                <span class="w400">
                    <label  style="width:143.5px;">
                        <input type="checkbox" class="dx" value="1" name="ckb_calltype" id="edit_ckb_calltype_1"
                            runat="server" />呼入
                    </label>
                    <label style="width: 150px;">
                        <input type="checkbox" class="dx" value="2" name="ckb_calltype" id="edit_ckb_calltype_2"
                            runat="server" />呼出
                    </label>
                </span></li>
            <li >
                <label>
                    <span class="redColor">*</span>有效期：
                </label>
                 <span style='width:150px;display:block;float:left;'>
                    <input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi1" onclick="checkYouXiaoQi(3)" value="3" /><em onclick="checkYouXiaoQi(3)">三个月</em></span>
                <span style='width:150px;display:block;float:left;'>
                    <input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi2" onclick="checkYouXiaoQi(6)" value="6" /><em onclick="checkYouXiaoQi(6)">半年</em></span>
                <span style='width:150px;display:block;float:left;'>
                    <input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi4" onclick="checkYouXiaoQi(12)" value="12" /><em onclick="checkYouXiaoQi(12)">一年</em></span> 
                <span style='width:50px;display:block;float:left;'>
                    <input name="rd_youxiaoqi" type="radio" id="edit_rd_youxiaoqi5" onclick="checkYouXiaoQi(1)" value="0" /><em onclick="checkYouXiaoQi(1)">其他</em> </span><span style='width:100px;display:block;float:left; margin-top:5px;'>
                                    <input disabled="" class="w250" id="edit_txtYouXiaoQi" style="width: 106px;" type="text" runat="server" /></span> </li>
            <li>
                <label>
                    <span class="redColor">*</span>对应业务：</label>
                <span class="w400" style=" width:650px;" id="addSelBusinessType"></span></li>
            <li >
                <label>
                    <span class="redColor">*</span> 原因：
                </label>
                <span class="w400" style=" width:650px;">
                    <textarea id="textearea_reason" cols=""  rows="5" class="w400" style=" width:600px;"></textarea>
                </span></li>
        </ul>
        <div class="btn mt20" >
            <input id="btnSave_Page1" type="button" onclick="SaveEditData();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('UpdateBlackDataAjaxPopup',false);" />
        </div>
    </div>
</div>
