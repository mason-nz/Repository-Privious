<%@ Page Title="免打扰" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="BlackList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BlackWhite.BlackList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("controlParams");
    </script>
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    电话号码：</label>
                <input type="text" id="txtPhoneNum" class="w190" />
            </li>
            <li>
                <label>
                    有效期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    对应业务：</label>
                <select id="selBusinessType" class="w190" style="width: 194px;">
                    <option value="-1" mutilid="-1">请选择</option>
                </select>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    添加人：</label>
                <input type="text" id="txtSelAddUser" class="w190" readonly="true" onclick="SelectVisitPerson('addblackdatauser','txtSelAddUser','hidSelAddUser')" />
                <input type="hidden" id="hidSelAddUser" value="-1" />
            </li>
            <li>
                <label>
                    添加时间：</label>
                <input type="text" id="txtAddTime1" class="w85" style="width: 84px; *width: 83px;
                    width: 83px\9;" name="AddTime1" />
                至
                <input type="text" id="txtAddTime2" class="w85" style="width: 84px; *width: 83px;
                    width: 83px\9;" name="AddTime2" />
            </li>
            <li style="width: 286px; *width: 287px; margin-right: 0px;">
                <label>
                    类型：</label>
                <span style="display: block; width: 50px; float: left; margin-left: 0px; *margin-left: 0px;
                    padding-left: 0px;">
                    <input type="checkbox" value="1" id="chkCallIn" name="CallTypes" style="*margin-left: -3px;
                        margin-left: 0px; padding-left: 0px;" /><em onclick="emChkIsChoose(this);">呼入</em></span>
                <span style="display: block; width: 50px; float: left;">
                    <input type="checkbox" value="2" id="chkCallOut" name="CallTypes" /><em onclick="emChkIsChoose(this);">呼出</em></span>
                <span style="display: block; width: 90px; float: left;">
                    <input type="checkbox" value="3" id="chkCallInOut" name="CallTypes" /><em onclick="emChkIsChoose(this);">呼入及呼出</em></span>
            </li>
            <li class="btnsearch" style="width: 80px;">
                <input style="float: right;" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_export)
          { %>
        <input name="" type="button" value="导入" onclick="javascript:openUploadExcelInfoPopup()"
            class="newBtn" style="*margin-top: 3px;" />
        <%} %>
        <%if (right_add)
          { %>
        <input name="" type="button" value="新增" onclick="javascript:OpenBWEditLayer('')"
            class="newBtn" style="*margin-top: 3px;" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <script type="text/javascript">

        //导入
        function openUploadExcelInfoPopup() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: { Action: 'batchimport', Type: 0 },
                url: "/BlackWhite/AjaxService/ImportExcelData.aspx",
                beforeClose: function (e) {
                    if (e) {
                        search();
                    }
                }
            });
        }
        function SelectListInit() {
            var str = TelNumManag.GetOptions();
            $("#selBusinessType").append(str);
        }
        //选择操作人
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: "-1" },
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                    ;
                }
            });
        }

        $(function () {
            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

            $('#txtAddTime1').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtAddTime2\')}', onpicked: function () { document.getElementById("txtAddTime2").focus(); } }); });
            $('#txtAddTime2').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtAddTime1\')}' }); });

            SelectListInit();
            //敲回车键执行方法
            enterSearch(search);

            search();
        });

        function search(page) {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }

            var pody = _params();
            if (page != undefined) {
                pody = pody.replace(/&page=[0-9]*/, '') + "&page=" + page;
            }
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/BlackWhite/AjaxService/BlackDataList.aspx", podyStr, transBusinessType);

        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/BlackWhite/AjaxService/BlackDataList.aspx", pody, transBusinessType);
        }
        function transBusinessType() {
            $(".td_cdidstocdidsname").each(function () {
                var cdidnames = "";
                var cdid = $.trim($(this).text());
                if (cdid > 0) {
                    $.each(TelNumManag.TelNumList, function (index, data) {
                        if ((data.MutilID & cdid) == data.MutilID) {
                            cdidnames += "," + data.Name;
                        }
                    });
                }
                $(this).html(cdidnames.length > 1 ? cdidnames.substr(1) : "&nbsp;");
            });
            $(".nodistubstatus").each(function () {
                var thisObj = $(this);
                if ($.trim(thisObj.text()) == "已过期") {
                    thisObj.css("color", "red");
                }
            });
        }
        //获取参数
        function _params() {
            var phoneNum = $.trim($("#txtPhoneNum").val());
            var effectivebeginTime = $.trim($("#tfBeginTime").val());
            var effectiveendTime = $.trim($("#tfEndTime").val());
            var callTypes = $("input:checkbox[name='CallTypes']:checked").map(function () {
                return $(this).val();
            }).get().join(",");
            if (callTypes == "1,2,3") {
                callTypes = "";
            }
            var userid = $.trim($("#hidSelAddUser").val());
            var addbeginTime = $.trim($("#txtAddTime1").val());
            var addendTime = $.trim($("#txtAddTime2").val());

            var businesstypes = $.trim($('#selBusinessType ').find("option:selected").attr("mutilid"));

            var pody = {
                PhoneNumber: encodeURIComponent(phoneNum),
                EffectiveBeginTime: encodeURIComponent(effectivebeginTime),
                EffectiveEndTime: encodeURIComponent(effectiveendTime),
                CallTypes: encodeURIComponent(callTypes),
                UserId: encodeURIComponent(userid),
                AddBeginTime: encodeURIComponent(addbeginTime),
                AddEndTime: encodeURIComponent(addendTime),
                BusinessTypes: encodeURIComponent(businesstypes),
                r: Math.random()            //随机数
            }
            return pody;
        }

        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var phoneNum = $.trim($("#txtPhoneNum").val());
            var effectivebeginTime = $.trim($("#tfBeginTime").val());
            var effectiveendTime = $.trim($("#tfEndTime").val());
            var addbeginTime = $.trim($("#txtAddTime1").val());
            var addendTime = $.trim($("#txtAddTime2").val());

            if (!isNum(phoneNum)) {
                msg += "电话号码格式不正确<br/>";
                $("#txtPhoneNum").val('');
            }
            if (effectivebeginTime != "") {
                if (!effectivebeginTime.isDate()) {
                    msg += "有效期格式不正确<br/>";
                    $("#tfBeginTime").val('');
                }
            }
            if (effectiveendTime != "") {
                if (!effectiveendTime.isDate()) {
                    msg += "有效期格式不正确<br/>";
                    $("#tfEndTime").val('');
                }
            }
            if (effectivebeginTime != "" && effectiveendTime != "") {
                if (effectiveendTime < effectivebeginTime) {
                    msg += "有效期中后面日期不能大于前面日期<br/>";
                    $("#tfBeginTime").val('');
                    $("#tfEndTime").val('');
                }
            }

            if (addbeginTime != "") {
                if (!addbeginTime.isDate()) {
                    msg += "添加日期格式不正确<br/>";
                    $("#txtAddTime1").val('');
                }
            }
            if (addendTime != "") {
                if (!addendTime.isDate()) {
                    msg += "添加日期格式不正确<br/>";
                    $("#txtAddTime2").val('');
                }
            }
            if (addbeginTime != "" && addendTime != "") {
                if (addendTime < addbeginTime) {
                    msg += "添加日期中后面日期不能大于前面日期<br/>";
                    $("#txtAddTime1").val('');
                    $("#txtAddTime2").val('');
                }
            }
            return msg;
        }
        //新增数据
        function AddBlackData() {
            $.openPopupLayer({
                name: "AddBlackDataAjaxPopup",
                parameters: {},
                url: "/BlackWhite/AddBlackData.aspx?r=" + Math.random()
            });

        }


        ///删除指定数据
        function DeleteData(recid) {
            $.jConfirm("确定要删除该条数据吗？", function (r) {
                if (r) {
                    AjaxPostAsync("/BlackWhite/BlackWhiteHandler.ashx", { Action: "changeblackwhitestatus", RecId: recid, r: Math.random() }, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "yes") {
                            search(); //根据查询条件重新查询数据
                        }
                        else {
                            $.jAlert(jsonData.msg);
                        }
                    });
                }
            });
        }

        function PlayAudio(callID) {
            //根据CALLID查看是否有录音
            var para;
            para = {
                Action: "getaudiourlbycallid", CallID: callID
            };
            AjaxPostAsync("/BlackWhite/BlackWhiteHandler.ashx", para, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    ADTTool.PlayRecord(jsonData.msg);
                    //                    $.openPopupLayer({
                    //                        name: 'PlayRecordLayer',
                    //                        url: "/CTI/PlayRecordList.aspx",
                    //                        parameters: { 'RecordURL': jsonData.msg,"OrderID":"-1" }
                    //                      , popupMethod: 'Post'
                    //                    });
                }
                else {
                    $.jAlert("没有找到当前话务ID对应的录音!");
                    return;
                }
            });
        }
    </script>
    </form>
</asp:Content>
