<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact.Add" %>

<script type="text/javascript">
    function SaveData() {
        var msg = "";//弹出框提示信息

        var txtContactName = $.trim($('#txtContactName').val());
        var radioSex = $("input[name='radioSex']:checked").val();
        var txtContactDepartment = $('#txtContactDepartment').val();
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

        //var hid_ContactUserS = $('#hid_ContactUserS').val();
        var Hobby = $('#Hobby').val();
        var Responsible = $('#Responsible').val();
        
        // add lxw 12.12.12
        //负责会员
        var chxyesids = $(":checkbox[id='checkBoxAll'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
        var chxnotids = $(":checkbox[id='checkBoxAll'][checked=false]").map(function () { return $(this).val(); }).get().join(",");
        //负责人选中
        var chxisids = $(":checkbox[id='checkBoxMain'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
        //负责人没选中
        var chxnoids = $(":checkbox[id='checkBoxMain'][checked=false]").map(function () { return $(this).val(); }).get().join(",");

        if ($.trim(txtContactName) == '') {
            //弹出提示信息
            msg +='请填写联系人名称！<br/>';
        }
        if(chxisids!=""){
            if($.trim(txtContactEmali) == ""){
                msg +='车易通负责人选中时，Email必填！<br/>'; 
                $("#txtContactEmali").focus();
            }
        }
        if ($.trim(txtContactEmali) != "") {
            if (!ValidateEmail($.trim(txtContactEmali))) {
                msg +='Email格式不正确！<br/>';  
                $("#txtContactEmali").focus(); 
            }
        }
        if (GetStringRealLength(txtContactName) > 50) {
                msg +='联系人姓名不能超过50个字符！<br/>';   
        }
        if (txtContactDepartment == '-1'&&$("#liDepartment").css("display")!='none') {
                msg +='请选择部门！<br/>';   
        }
        if (txtContactDuty == "") {
                msg +='请输入职务！<br/>';    
        }
        if (txtContactModel == "") {
                msg +='请输入移动电话！<br/>';  
        } 
        if (!isTelOrMobile($.trim($('#txtContactModel').val()))) {
                msg +='移动电话格式不正确！<br/>';  
        }

        /*
        if (txtContactEmali == "") {
        $.jAlert('请输入邮箱！');
        return;
        }
        */
        if (txtOfficeTypeCode == '-1') {
                msg +='请选择职级！<br/>'; 
        }
        if (GetStringRealLength(txtContactDuty) > 100) {
                msg +='联系人职务不能超过100个字符！<br/>'; 
        }
        if (GetStringRealLength(txtContactTele) > 100) {
                msg +='联系人电话不能超过100个字符！<br/>';  
        }
        if (GetStringRealLength(txtContactModel) > 100) {
                msg +='联系人手机不能超过100个字符！<br/>';   
        }
        if (GetStringRealLength(txtContactEmali) > 100) {
                msg +='联系人邮箱不能超过100个字符！<br/>';   
        }
        if (GetStringRealLength(txtContactFax) > 50) {
                msg +='联系人传真不能超过50个字符！<br/>';   
        }
        if (GetStringRealLength(txtContactRemark) > 1000) {
                msg +='联系人备注不能超过1000个字符！<br/>';    
        }

        if (GetStringRealLength(txtAddress) > 500) {
                msg +='地址不能超过500个字符！<br/>';     
        }
        if (GetStringRealLength(txtZipCode) > 6) {
                msg +='邮编不能超过6个字符！<br/>';   
        }
        if (GetStringRealLength(txtMSN) > 50) {
                msg +='QQ/MSN不能超过50个字符！<br/>';   
        }
        if (GetStringRealLength(txtBirthday) > 50) {
                msg +='生日不能超过50个字符！<br/>';  
        }

        if ($.trim(txtBirthday).length > 0) {
            if (!($.trim(txtBirthday).isDate())) {
                msg +='生日格式不正确！<br/>';   
                $('#txtBirthday').val('');
                $('#txtBirthday').focus(); 
            }
        }

        if(msg !=""){
             $.jAlert(msg);
             return;
        }

//        if (hid_ContactUserS == "") {
//            $.jAlert('请选择跟进员工！');
//            return;
//        }

        txtContactDepartment = $("#txtContactDepartment").find("option:selected").text();

        var addurl = "<%=addurl %>";

        var url = "/CustInfo/MoreInfo/CC_Contact/Handler.ashx?callback=?" + addurl;
        $.post(url, {TID:encodeURIComponent("<%=TID %>")
                             ,Action:encodeURIComponent("<%=strAction %>")
                             ,CustType:'<%=CustType %>'
                             ,ContactID:encodeURIComponent("<%=Request["ID"] %>")
                             ,CName:encodeURIComponent(txtContactName.replace(/'/g, ""))
                             ,Sex:encodeURIComponent(radioSex)
                             ,DepartMent:encodeURIComponent(txtContactDepartment)
                             ,OfficeTypeCode:encodeURIComponent(txtOfficeTypeCode)
                             ,Title:encodeURIComponent(txtContactDuty)
                             ,OfficeTel:encodeURIComponent(txtContactTele)
                             ,Phone:encodeURIComponent(txtContactModel)
                             ,Email:encodeURIComponent(txtContactEmali)
                             ,Fax:encodeURIComponent(txtContactFax)
                             ,PID:encodeURIComponent(SelectPID)
                             ,Reamrk:encodeURIComponent(txtContactRemark)
                             ,Address:encodeURIComponent(txtAddress)
                             ,ZipCode:encodeURIComponent(txtZipCode)
                             ,MSN:encodeURIComponent(txtMSN)
                             ,Birthday:encodeURIComponent(txtBirthday)
                            //+ "&hid_ContactUserS=" + escapeStr(hid_ContactUserS)
                             ,Hobby:encodeURIComponent(Hobby)
                             ,Responsible:encodeURIComponent(Responsible)
                             ,YesMemberIDs:encodeURIComponent(chxyesids)
                             ,NotMemberIDs:encodeURIComponent(chxnotids)
                             ,YesMainIDs:encodeURIComponent(chxisids)
                             ,NotMainIDs:encodeURIComponent(chxnoids)
                            },function (jd, textStatus, xhr) {
                if (textStatus != 'success') { $.jAlert('请求错误'); }
                else if (jd.success) {
                    closePopup(true);
                }
                else {
                    $.jAlert('错误: ' + jd.message);
                }
            }, 'json');
    }
    //邮箱验证
    function ValidateEmail(j) {
        var emailReg = /^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return emailReg.test(j);
    }
    //绑定部门
    AjaxPost('/CustInfo/MoreInfo/CC_Contact/Handler.ashx', 'action=BindContactDepartment&TID=<%=TID %>&CustType=<%=this.CustType %>', null, successBindContactDepartment);
    function successBindContactDepartment(data) {
        var dd = $.evalJSON($.evalJSON(data).message);
        if (dd.ID==0) {
            return;
        }
        var selectObject = document.getElementById('txtContactDepartment');
        selectObject.options.length = 0;
        selectObject.options[0] = new Option("请选择部门", "-1");
        $.each(dd, function (i, n) {
        if(n.Name=="error")
        {
            if(n.ID=="1")
            {
                $.jAlert('该伙伴未填写类别信息，不能添加/编辑联系人信息，请先设置伙伴类别！',function () {
                    Close('AddContactInfo');
                    });
            }else if(n.ID=="0")
            {
                $.jAlert('该客户未填写类别信息，不能添加/编辑联系人信息，请先设置客户类别！',function () {
                    Close('AddContactInfo');
                    });
            }
        }else{
            selectObject.options[selectObject.options.length] = new Option(n.Name, n.ID);
            }
        }); 
        jsSelectItemByValue($("#Hidden_dept").val(), document.getElementById('txtContactDepartment'))       
    }
    //加载要编辑的数据
    function fillpage(){
        var url = "/CustInfo/MoreInfo/CC_Contact/Handler.ashx?Action=showcontact";
        var postBody = 'ContactID=<%=Request["ID"] %>'; //携带的参数
        AjaxPost(url, postBody, null, function (data) {
            var EditContactName = $('#txtContactName');
            var EditContactDepartment = $('#txtContactDepartment');
            var EditContactDuty = $('#txtContactDuty');
            var EditContactTele = $('#txtContactTele');
            var EditContactModel = $('#txtContactModel');
            var EditContactEmali = $('#txtContactEmali');
            var EditContactFax = $('#txtContactFax');
            var SelectPID = $('#SelectPID');
            var EditContactRemark = $('#txtContactRemark');
            var txtAddress = $('#txtAddress');
            var txtZipCode = $('#txtZipCode');
            var txtMSN = $('#txtMSN');
            var txtBirthday = $('#txtBirthday');

            //var ContactUserS = $('#ContactUserS');
            //var hid_ContactUserS = $('#hid_ContactUserS');
            var Hobby = $('#Hobby');
            var Responsible = $('#Responsible');

            var jsonData = $.evalJSON(data);
            if (jsonData.success==true) {                 
                var s = $.evalJSON(jsonData.message);
                EditContactName.val(s.CName);
                $("#Hidden_dept").val(s.DepartMent);
                jsSelectItemByValue(s.DepartMent, document.getElementById('txtContactDepartment'))
                $('#txtOfficeTypeCode').val(s.OfficeTypeCode);
                EditContactDuty.val(s.Title);
                EditContactTele.val(s.OfficeTel);
                EditContactModel.val(s.Phone);
                EditContactEmali.val(s.Email);
                EditContactFax.val(s.Fax);
                EditContactRemark.val(s.Remark);
                SelectPID.val(s.PID);

                txtAddress.val(s.Address);
                txtZipCode.val(s.ZipCode);
                txtMSN.val(s.MSN);
                txtBirthday.val(s.Birthday);

                if (s.Sex == 1) {
                    var ra1 = document.getElementById('radioBoy');
                    ra1.checked = true

                }
                else {
                    var ra2 = document.getElementById('radioGirl');
                    ra2.checked = true;
                }
                //ContactUserS.val(s.ContactUserS);
                //hid_ContactUserS.val(s.hid_ContactUserS);
                Hobby.val(s.Hobby);
                Responsible.val(s.Responsible);
               
            }
            else {
                $.jAlert('数据加载失败');
                return;
            }
        });
    }
    function jsSelectItemByValue(objItemText, objSelect) {
        //判断是否存在 
        var isExit = false;
        for (var i = 0; i < objSelect.options.length; i++) {
            if (objSelect.options[i].text == objItemText) {
                objSelect.options[i].selected = true;
                isExit = true;
                break;
            }
        }
    }
    function closePopup(effectiveAction)
    {
        $.closePopupLayer('<%= PopupName%>', effectiveAction);
    }
    function IsShowDepartment()
    {
        if('<%=CustType %>'!='20010')
        {
         $("#liDepartment").css("display","block");
        }
        else
        {
        $("#liDepartment").css("display","none");
        }
    }
    IsShowDepartment();
    //<!-- wangzw -->

</script>
<script type="text/javascript">
    $(document).ready(function () {

        var contactid = '<%=Request["ID"] %>';
        if (contactid > 0) {
            loadMappingData();
        }

    });

    //初始化关联数据
    function loadMappingData() {
        var url = "/CustInfo/MoreInfo/CC_Contact/Handler.ashx?Action=GetMember";
        var pody = {
            ContactID: '<%=Request["ID"] %>',
            r: Math.random()
        };
        AjaxPost(url, pody, null, function (data) {
            var jsonData = $.evalJSON(data);
            var jsonDataMessage = $.evalJSON(jsonData.message);
            $.each(jsonDataMessage, function (i, n) {
                $(":checkbox").each(function () {
                    if (n.MemberID == $(this).val()) {
                        if ($(this).attr("name") == "checkBoxAll") {
                            $(this).attr("checked", true);
                        } else if ($(this).attr("name") == "checkBoxMain" && n.IsMain == 1) {
                            $(this).attr("checked", true);
                            //显示邮箱必填*号
                            $("#spanEmail").show();
                        }
                    }
                });
            });
        });
    }

    //点击 会员 复选框 时，进行验证，如果全部不选中则将Email必填隐藏
    function checkMemberSelect(othis) {
        if (!$(othis).is(":checked")) {
            //隐藏邮箱必填*号
            $("#spanEmail").hide();
            //隐藏车易通负责人的复选框
            $(othis).parents("tr").find("td:eq(3) :checkbox").attr("checked", false);

            $(":checkbox[name='checkBoxMain']").each(function () {
                //如果车易通负责人的复选框有一个选中，则将Email必填项显示
                if ($(this).is(":checked")) {
                    $("#spanEmail").show();
                }
            });
        }
    }

    //点击 车易通负责人 复选框 时，做验证:1、如果勾选，则Email必填；2、查看该会员在其他客户下是否有车易通负责人，如果有则提醒；
    function checkIsMainSelect(othis) {
        if ($(othis).is(":checked")) {
            //判断该会员在其他客户下是否有车易通负责人，如果有则提醒
            var url = "/CustInfo/MoreInfo/CC_Contact/Handler.ashx?Action=GetManageContactInfo";
            var pody = {
                ContactID: '<%=Request["ID"] %>',
                MemberID: $(othis).val(),
                r: Math.random()
            };
            AjaxPost(url, pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                var jsonDataMessage = $.evalJSON(jsonData.message);
                if (jsonDataMessage.CName != '') {
                    $.jConfirm('该会员已有车易通负责人（' + jsonDataMessage.CName + '）,是否替换？', function (result) {
                        if (result) {
                            //显示邮箱必填*号
                            $("#spanEmail").show();
                            //同时选中会员复选框 
                            $(othis).parents("tr").find("td:eq(0) :checkbox").attr("checked", true);
                        }
                        else {
                            $(othis).attr("checked", false);
                        }
                    });
                }
                else {
                    //显示邮箱必填*号
                    $("#spanEmail").show();
                    //同时选中会员复选框 
                    $(othis).parents("tr").find("td:eq(0) :checkbox").attr("checked", true);
                }
            });

        }
        else {
            //判断车易通负责人复选框是否选上，如果有一个选中，则Email必填项显示，如都没选中，则隐藏
            $("#spanEmail").hide();
            $(":checkbox[name='checkBoxMain']").each(function () {
                //如果车易通负责人的复选框有一个选中，则将Email必填项显示
                if ($(this).is(":checked")) {
                    $("#spanEmail").show();
                }
            });
        }
    }

</script>
<form id="addContactInfo">
<!--弹窗1客户联系人信息-->
<div class="pop pb15 openwindow" style="width: 700px">
    <div class="title bold">
        <h2>
            客户联系人信息</h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopup(false);"></a></span>
    </div>
    <br />
    <h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;基本信息
    </h2>
    <ul class="clearfix ">
        <li style="width: 300px">
            <label>
                联系人姓名：</label><input id="txtContactName" name="txtContactName" type="text" class="w125"
                    style="width: 145px;" /><span style="color: #ff0000">*</span></li>
        <li style="width: 300px">
            <label>
                性别：</label><input id="radioBoy" name="radioSex" type="radio" checked="checked" value="1" /><span>先生</span><input
                    id="radioGirl" name="radioSex" type="radio" value="0" /><span>女士</span><span style="color: #ff0000">*</span></li>
        <li id='liDepartment' style="width: 300px">
            <label>
                部门：</label><select id="txtContactDepartment" name="txtContactDepartment" class="k180"
                    style="width: 147px;">
                    <option value="0">请选择部门</option>
                </select><span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                职务：</label><input id="txtContactDuty" name="txtContactDuty" type="text" class="w125"
                    style="width: 145px;" /><span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                职级：</label>
            <select id="txtOfficeTypeCode" name="OfficeTypeCode" class="k180" style="width: 147px;">
                <option value="-1">请选择</option>
                <option value="160001">总裁（股东/董事长/董事/总裁…）</option>
                <option value="160002">高管（高层/总经理/副总经理/店长…）</option>
                <option value="160003">总监（中层/市场总监/销售总监…）</option>
                <option value="160004">经理（基层/部门经理/主管…）</option>
                <option value="160005">专员（员工/市场/销售/财务/客服/公关…）</option>
                <option value="160000">其它</option>
            </select>
            <span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                移动电话：</label><input id="txtContactModel" name="txtContactModel" type="text" class="w125"
                    style="width: 145px;" /><span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                办公电话：</label><input id="txtContactTele" name="txtContactTele" type="text" class="w125"
                    style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                Email：</label><input id="txtContactEmali" name="txtContactEmali" type="text" class="w125"
                    style="width: 145px;" /><span id="spanEmail" style="color: #ff0000; display: none">*</span></li>
        <%--<li>
                    <label>
                        跟进员工：</label><input id="ContactUserS" name="ContactUserS" type="text" class="k160" /><input
                            id="hid_ContactUserS" name="hid_ContactUserS" type="hidden" /><a href="javascript:ContactUserSSelectAjaxPopup();"
                                style="float: left;">
                                <img alt="选择跟进员工" src="../../images/button_001.gif" border="0" /></a><span style="color: #ff0000">*</span>
                </li>--%>
        <li style="width: 300px">
            <label>
                直接上级：</label><select id="SelectPID" name="SelectPID" class="k180" runat="server"
                    style="width: 147px;">
                    <option value="0">请选择直接上级</option>
                </select></li>
        <%if (isShowMember) %>
        <%{ %>
        <li style="width: 100%; height: auto">
            <label>
                负责会员：</label>
            <div class="Table2" style="margin: 0; padding: 0; clear: none">
                <table cellspacing="0" cellpadding="0" border="0" style="margin: 0; width: 480px"
                    class="Table2List" id="tableMemberContactMapping">
                    <tbody>
                        <tr>
                            <th width="10%">
                                选择
                            </th>
                            <th width="20%">
                                会员编号
                            </th>
                            <th width="40%">
                                会员简称
                            </th>
                            <th width="30%">
                                车易通负责人<img src="../../../Images/question.png" title="用户名和密码以及服务开通提醒&#10;将以短信和邮件方式自动发送给&#10;车易通负责人"
                                    style="margin-bottom: -5px;" />
                            </th>
                        </tr>
                        <asp:repeater id="rtpMemberList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <input id='checkBoxAll' name='checkBoxAll' value='<%# Eval("ID") %>'type="checkbox" onclick="checkMemberSelect(this)" style="display :inline;float: none"/>
                                        </td>
                                        <td>
                                            <%# Eval("MemberCode")%>
                                        </td>
                                        <td><%#Eval("Abbr")%></td>
                                        <td>
                                            <input id='checkBoxMain' name='checkBoxMain' value='<%# Eval("ID") %>'type="checkbox" onclick="checkIsMainSelect(this)" style="display :inline;float : none"/>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
                    </tbody>
                </table>
            </div>
        </li>
        <%} %>
    </ul>
    <h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;扩展信息</h2>
    <ul class="clearfix ">
        <li style="width: 300px">
            <label>
                传真：</label><input id="txtContactFax" name="txtContactFax" type="text" class="w125"
                    style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                地址：</label><input id="txtAddress" name="txtAddress" type="text" class="w125" style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                邮编：</label><input id="txtZipCode" name="txtZipCode" type="text" class="w125" style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                出生日期：</label><input id="txtBirthday" name="txtBirthday" type="text" class="w125"
                    style="width: 145px;" />
            <input type="button" onclick="MyCalendar.SetDate(this,document.getElementById('txtBirthday'))"
                class="date_click" id="date_click1" /></li>
        <li style="width: 300px">
            <label>
                QQ/MSN：</label><input id="txtMSN" name="txtMSN" type="text" class="w125" style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                爱好：</label><input id="Hobby" name="Hobby" type="text" class="w125" style="width: 145px;" /></li>
        <li style="width: 300px">
            <label>
                具体负责：</label><input id="Responsible" name="Responsible" type="text" class="w125"
                    style="width: 145px;" /></li>
        <li class="nowidth">
            <label>
                备注：</label><textarea id="txtContactRemark" name="txtContactRemark" rows="5"></textarea>
        </li>
        <%--<%if (Request["action"] == "show")
                  {%>
                <li>
                    <label>
                        创建人：</label><input id="CreateUserID" name="CreateUserID" style="border: 0px;" type="text"
                            class="k200" readonly="readonly" /></li>
                <li>
                    <label>
                        创建日期：</label><input id="CreateTime" name="CreateTime" style="border: 0px;" type="text"
                            class="k200" readonly="readonly" /></li>
                <li>
                    <label>
                        修改人：</label><input id="ModifyUserID" name="ModifyUserID" style="border: 0px;" type="text"
                            class="k200" readonly="readonly" /></li>
                <li>
                    <label>
                        修改日期：</label><input id="ModifyTime" name="ModifyTime" style="border: 0px;" type="text"
                            class="k200" readonly="readonly" /></li>
                <%} %>--%>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" value="保存" class="btnSave bold" onclick="javascript:SaveData()" />
        <input type="button" value="重填" class="btnSave bold" onclick="javascript:resetForm('addContactInfo');" />
        <input type="button" value="退出" class="btnSave bold" onclick="javascript:closePopup(false);" />
    </div>
</div>
</form>
<input id="Hidden_dept" type="hidden" />
<script type="text/javascript"><%=strFillPage %></script>
