<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact.Edit" %>

<script type="text/javascript">
    var editContactHelper = (function () {
        var closePopup = function (effectiveAction) {//关闭本窗口。参数：关闭前是否有[有效改变]
            $.closePopupLayer('<%= PopupName%>', effectiveAction);
        },

        submit = function () {
            var txtContactName = $.trim($('#txtContactName').val());
            var radioSex = $("input[name='radioSex']:checked").val();
            var txtContactDepartment = $.trim($('#txtContactDepartment').val());
            var txtOfficeTypeCode = $('#txtOfficeTypeCode').val();
            var txtContactDuty = $.trim($('#txtContactDuty').val());
            var txtContactTele = $.trim($('#txtContactTele').val());
            var txtContactModel = $.trim($('#txtContactModel').val());
            var txtContactEmali = $.trim($('#txtContactEmali').val());
            var txtContactFax = $.trim($('#txtContactFax').val());

            var txtAddress = $.trim($('#txtAddress').val());
            var txtZipCode = $.trim($('#txtZipCode').val());
            var txtMSN = $.trim($('#txtMSN').val());
            var txtBirthday = $.trim($('#txtBirthday').val());

            var SelectPID = $('#SelectPID').val();
            var txtContactRemark = $.trim($('#txtContactRemark').val());

            if ($.trim(txtContactName) == '') {
                //弹出提示信息
                $.jAlert('请填写客户联系人名称！');
                return;
            }
            if ($.trim(txtContactEmali) != "") {
                if (!isEmail($.trim(txtContactEmali))) {
                    $.jAlert('Email格式不正确！');
                    return;
                }
            }
            if (GetStringRealLength(txtContactName) > 50) {
                $.jAlert('客户联系人姓名不能超过50个字符！');
                return;
            }
            if (GetStringRealLength(txtContactDepartment) > 100) {
                $.jAlert('客户联系人部门不能超过100个字符！');
                return;
            }
            if (txtOfficeTypeCode == '-1') {
                $.jAlert('请选择职级');
                return;
            }
            if (GetStringRealLength(txtContactDuty) > 100) {
                $.jAlert('客户联系人职务不能超过100个字符！');
                return;
            }
            if (GetStringRealLength(txtContactTele) > 100) {
                $.jAlert('客户联系人电话不能超过100个字符！');
                return;
            }
            if (GetStringRealLength(txtContactModel) > 100) {
                $.jAlert('客户联系人手机不能超过100个字符！');
                return;
            }
            if (GetStringRealLength(txtContactEmali) > 100) {
                $.jAlert('客户联系人邮箱不能超过100个字符！');
                return;
            }
            if (GetStringRealLength(txtContactFax) > 50) {
                $.jAlert('客户联系人传真不能超过50个字符！');
                return;
            }
            if (GetStringRealLength(txtContactRemark) > 1000) {
                $.jAlert('客户联系人备注不能超过1000个字符！');
                return;
            }

            if (GetStringRealLength(txtAddress) > 500) {
                $.jAlert('地址不能超过500个字符！');
                return;
            }
            if (GetStringRealLength(txtZipCode) > 6) {
                $.jAlert('邮编不能超过6个字符！');
                return;
            }
            if (GetStringRealLength(txtMSN) > 50) {
                $.jAlert('QQ/MSN不能超过50个字符！');
                return;
            }
            if (GetStringRealLength(txtBirthday) > 50) {
                $.jAlert('生日不能超过50个字符！');
                return;
            }

            if ($.trim(txtBirthday).length > 0) {
                if (!($.trim(txtBirthday).isDate())) {
                    $.jAlert("生日格式不正确", function () {
                        $('#txtBirthday').val('');
                        $('#txtBirthday').focus();
                        return;
                    });
                }
            }

            $.post('/CustInfo/MoreInfo/CC_Contact/Handler.ashx?callback=?', {
                Action: 'EditContact',
                ContactID: encodeURIComponent('<%=ContactID %>'),
                CName: encodeURIComponent(txtContactName),
                Sex: encodeURIComponent(radioSex),
                DepartMent: encodeURIComponent(txtContactDepartment),
                OfficeTypeCode: encodeURIComponent(txtOfficeTypeCode),
                Title: encodeURIComponent(txtContactDuty),
                OfficeTel: encodeURIComponent(txtContactTele),
                Phone: encodeURIComponent(txtContactModel),
                Email: encodeURIComponent(txtContactEmali),
                Fax: encodeURIComponent(txtContactFax),
                PID: encodeURIComponent(SelectPID),
                Reamrk: encodeURIComponent(txtContactRemark),
                Address: encodeURIComponent(txtAddress),
                ZipCode: encodeURIComponent(txtZipCode),
                MSN: encodeURIComponent(txtMSN),
                Birthday: encodeURIComponent(txtBirthday)
            }, function (jd, textStatus, xhr) {
                if (textStatus != 'success') { $.jAlert('请求错误'); }
                else if (jd.success) {
                    closePopup(true);
                }
                else {
                    $.jAlert('错误: ' + jd.message);
                }
            }, 'json');
        };

        return {
            closePopup: closePopup,
            submit: submit
        }
    })();
</script>

<form>
<div class="openwindow">
    <div class="close">
        <a href="#this" onclick="javascript:editContactHelper.closePopup(false);">关闭</a></div>
    <h2>
        <span>客户联系人信息</span></h2>
    <fieldset>
        <ol>
            <li>
                <label>联系人姓名：</label>
                <input id="txtContactName" name="txtContactName" type="text" class="k120" runat="server" />
                <span style="color: #ff0000">*</span>
            </li>
            <li>
                <label>性别：</label>
                <input id="radioBoy" name="radioSex" type="radio" value="1" runat="server" />
                <span>先生</span>
                <input id="radioGirl" name="radioSex" type="radio" value="0" runat="server" />
                <span>女士</span>
                <span style="color: #ff0000">*</span>
            </li>
            <li>
                <label>部门：</label>
                <input id="txtContactDepartment" name="txtContactDepartment" type="text" class="k200"
                    runat="server" />
            </li>
            <li>
                <label>职级：</label>
                <select id="txtOfficeTypeCode" name="OfficeTypeCode" style="width: 183px;" runat="server">
                    <option value="-1">请选择</option>
                    <option value="160001">总裁（股东/董事长/董事/总裁…）</option>
                    <option value="160002">高管（高层/总经理/副总经理/店长…）</option>
                    <option value="160003">总监（中层/市场总监/销售总监…）</option>
                    <option value="160004">经理（基层/部门经理/主管…）</option>
                    <option value="160005">专员（员工/市场/销售/财务/客服/公关…）</option>
                    <option value="160000">其它</option>
                </select>
                <span style="color: #ff0000">*</span>
            </li>
            <li>
                <label>职务：</label>
                <input id="txtContactDuty" name="txtContactDuty" type="text" class="k200" runat="server" />
            </li>
            <li>
                <label>办公电话：</label><input id="txtContactTele" name="txtContactTele" type="text"
                    class="k200" runat="server" /></li>
            <li>
                <label>移动电话：</label><input id="txtContactModel" name="txtContactModel" type="text"
                    class="k200" runat="server" /></li>
            <li>
                <label>Email：</label><input id="txtContactEmali" name="txtContactEmali" type="text"
                    class="k200" runat="server" /></li>
            <li>
                <label>传真：</label><input id="txtContactFax" name="txtContactFax" type="text" class="k200"
                    runat="server" /></li>
            <li>
                <label>地址：</label><input id="txtAddress" name="txtAddress" type="text" class="k200"
                    runat="server" /></li>
            <li>
                <label>邮编：</label><input id="txtZipCode" name="txtZipCode" type="text" class="k200"
                    runat="server" /></li>
            <li>
                <label>QQ/MSN：</label><input id="txtMSN" name="txtMSN" type="text" class="k200" runat="server" /></li>
            <li>
                <label>出生日期：</label>
                <input id="txtBirthday" name="txtBirthday" type="text" class="k180" runat="server" />
                <input type="button" onclick="MyCalendar.SetDate(this,document.getElementById('txtBirthday'))"
                    class="date_click" id="date_click1" /></li>
            <li class="nowidth">
                <label>直接上级：</label>
                <select id="SelectPID" name="SelectPID" class="k200" runat="server"></select>
            </li>
            <li class="nowidth">
                <label>备注：</label>
                <textarea id="txtContactRemark" name="txtContactRemark" rows="5" class="k400" runat="server"></textarea>
            </li>
        </ol>
    </fieldset>
    <fieldset class="submits">
        <input type="button" value="保存" class="button" onclick="javascript:editContactHelper.submit();" />
        <input type="button" value="退出" class="button" onclick="javascript:editContactHelper.closePopup(false);" />
    </fieldset>
</div>
</form>
