<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.CC_Contact.Add" %>

<script type="text/javascript">
  //初始化页面
    $(document).ready(function () {
        //“扩展信息”显示逻辑
        $('#lbtnMore').click(function() {
            $('#divMore').slideToggle("fast");
            var  lbtnMore= $("#lbtnMore");
            if (lbtnMore[0].innerHTML == "扩展信息&gt;&gt;") 
            {
                lbtnMore[0].innerHTML = "收起&lt;&lt;";
            } 
            else
            {
                lbtnMore[0].innerHTML = "扩展信息&gt;&gt;";
            }   
        });

        var contactid = '<%=Request["ID"] %>';
        if (contactid >0) {
            loadMappingData();
        }
        var action = '<%=Request["action"] %>';
        if(action=='show') {
            var a = document.getElementsByTagName("input");  
            for (var i=0;i<a.length;i++)  {  
                  if(a[i].type=="checkbox") {
                      a[i].disabled=true;  
                  }  
            }
        }
    });

    function SaveData() {
        var b = true;
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
        var txtMsn = $.trim($('#txtMSN').val());
        var txtBirthday = $.trim($('#txtBirthday').val());
        var selectPid = $('#SelectPID').val();
        var txtContactRemark = $.trim($('#txtContactRemark').val());
        var hidContactUserS = $('#hid_ContactUserS').val();
        var hobby = $('#Hobby').val();
        var responsible = $('#Responsible').val();
        //负责会员
        var chxyesids = $(":checkbox[id='checkBoxAll'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
        var chxnotids = $(":checkbox[id='checkBoxAll'][checked=false]").map(function () { return $(this).val(); }).get().join(",");
        //负责人选中
        var chxisids = $(":checkbox[id='checkBoxMain'][checked=true]").map(function () { return $(this).val(); }).get().join(",");
        //负责人没选中
        var chxnoids = $(":checkbox[id='checkBoxMain'][checked=false]").map(function () { return $(this).val(); }).get().join(",");

        if ($.trim(txtContactName) == '') {
            //弹出提示信息
            $.jAlert('请填写联系人名称！');
            b = false;
            return;
        }
        
        if (GetStringRealLength(txtContactName) > 50) {
            $.jAlert('联系人姓名不能超过50个字符！');
            b = false;
            return;
        }

        <%if(TypeID!="20010"){ %>
        if (txtContactDepartment == '-1'||txtContactDepartment=='') {
            $.jAlert('请选择部门！');
            b = false;
            return;
        }
        <%} %>
        if (txtContactDuty == "") {
            $.jAlert('请输入职务！');
            b = false;
            return;
        }
        
        if (txtOfficeTypeCode == '-1'||txtOfficeTypeCode=='') {
            $.jAlert('请选择职级');
            b = false;
            return;
        }
        if (txtContactModel == "") {
            $.jAlert('请输入移动电话！');
            b = false;
            return;
        }
        
        if (GetStringRealLength(txtContactDuty) > 100) {
            $.jAlert('联系人职务不能超过100个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtContactTele) > 100) {
            $.jAlert('联系人电话不能超过100个字符！');
            b = false;
            return;
        }
        if (!isTelOrMobile(txtContactModel)) {
            $.jAlert('联系人移动电话格式不正确！');
            b = false;
            return;
        }
        
        if (chxisids!="" && txtContactEmali == "") {
            $.jAlert('请输入邮箱！');
            b = false;
            return;
        }
        if ($.trim(txtContactEmali) != "") {
            if (!ValidateEmail($.trim(txtContactEmali))) {
                $.jAlert('Email格式不正确！');
                b = false;
                return;
            }
        }

        if (GetStringRealLength(txtContactEmali) > 100) {
            $.jAlert('联系人邮箱不能超过100个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtContactFax) > 50) {
            $.jAlert('联系人传真不能超过50个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtContactRemark) > 1000) {
            $.jAlert('联系人备注不能超过1000个字符！');
            b = false;
            return;
        }

        if (GetStringRealLength(txtAddress) > 500) {
            $.jAlert('地址不能超过500个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtZipCode) > 6) {
            $.jAlert('邮编不能超过6个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtMsn) > 50) {
            $.jAlert('QQ/MSN不能超过50个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtBirthday) > 50) {
            $.jAlert('生日不能超过50个字符！');
            b = false;
            return;
        }
        if (GetStringRealLength(txtBirthday) > 0) {
            if (!(txtBirthday.isDate())) {
                $.jAlert("生日格式不正确", function () {
                    $('#txtBirthday').val('');
                    $('#txtBirthday').focus();
                });
                b = false;
                return;
            }
        }
        
        <%if(TypeID!="20010"){ %>
        txtContactDepartment = $("#txtContactDepartment").find("option:selected").text();        
        <%} else {%>
        txtContactDepartment = '无';
        <%} %>
        var addurl = "<%=Addurl %>";

        var url = "../ReturnVisit/CC_Contact/Handler.ashx?"+addurl;
        var postBody = "&CustID=" + escape("<%=CustID %>")
            + "&CName=" + escapeStr(txtContactName.replace(/'/g, ""))
            + "&Sex=" + radioSex
            + "&DepartMent=" + escapeStr(txtContactDepartment)
            + "&OfficeTypeCode=" + escapeStr(txtOfficeTypeCode)
            + "&Title=" + escapeStr(txtContactDuty)
            + "&OfficeTel=" + escapeStr(txtContactTele)
            + "&Phone=" + escapeStr(txtContactModel)
            + "&Email=" + escapeStr(txtContactEmali)
            + "&Fax=" + escapeStr(txtContactFax)
            + "&PID=" + selectPid
            + "&YesMemberIDs="+chxyesids
            + "&NotMemberIDs="+chxnotids
            + "&YesMainIDs="+chxisids
            + "&NotMainIDs="+chxnoids
            + "&Reamrk=" + escapeStr(txtContactRemark)
            + "&Address=" + escapeStr(txtAddress)
            + "&ZipCode=" + escapeStr(txtZipCode)
            + "&MSN=" + escapeStr(txtMsn)
            + "&Birthday=" + escapeStr(txtBirthday)
            + "&hid_ContactUserS=" + escapeStr(hidContactUserS)
            + "&Hobby=" + escapeStr(hobby)
            + "&Responsible=" + escapeStr(responsible);
        if (b==true) {
            $('#btn_ReturnVisit_Add').attr('disabled','disabled');
            AjaxPost(url, postBody, null, SuccessPost);
        }
    }
    
    function SuccessPost(data) {
        $('#btn_ReturnVisit_Add').removeAttr('disabled');
        var s = $.evalJSON(data);
        if (s.Add == 'yes') {
            $.jPopMsgLayer('操作成功！', function () {
            if(s.Edit=="no")
            {
                $.closePopupLayer('AddContactInfo',true);
                }else
                {
                $.closePopupLayer('EditContactInfo',true);
                }
            });
        }
        else {
            $.jAlert('操作失败！');
        }
    }
    
    AjaxPost('../ReturnVisit/CC_Contact/Handler.ashx', 'action=DropDownListPID&CustID=<%=CustID %>', null, success);
    function success(data) {
        var dd = $.evalJSON(data);
        var selectObject = document.getElementById('SelectPID');
        if (selectObject!=null) {
            selectObject.options.length = 0;
            selectObject.options[0] = new Option("请选择直接上级", "-1");
            $.each(dd, function (i, n) {
                selectObject.options[selectObject.options.length] = new Option(n.Name, n.ID);
            });
        }
    }
    //邮箱验证
    function ValidateEmail(j) {
        var emailReg = /^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        return emailReg.test(j);
    }
    //绑定部门
    AjaxPost('../ReturnVisit/CC_Contact/Handler.ashx', 'action=BindContactDepartment&CustID=<%=CustID %>&type=customer', null, successBindContactDepartment);
    function successBindContactDepartment(data) {
        var dd = $.evalJSON(data);
        var selectObject = document.getElementById('txtContactDepartment');
        if(selectObject!=null) {
            selectObject.options.length = 0;
            selectObject.options[0] = new Option("请选择部门", "-1");
            $.each(dd, function (i, n) {
                if(n.Name=="error")
                {
                    if(n.ID=="1")
                    {
                        $.jAlert('该伙伴未填写类别信息，不能添加/编辑联系人信息，请先设置伙伴类别！',function () {
                            $.closePopupLayer('AddContactInfo',false);
                        });
                    }else if(n.ID=="0")
                    {
                        $.jAlert('该客户未填写类别信息，不能添加/编辑联系人信息，请先设置客户类别！',function () {
                            $.closePopupLayer('AddContactInfo',false);
                        });
                    }
                }else{
                    selectObject.options[selectObject.options.length] = new Option(n.Name, n.ID);
                }
            });
            jsSelectItemByValue($("#Hidden_dept").val(), document.getElementById('txtContactDepartment')); 
        }
    }
    //选择跟进员工
    function ContactUserSSelectAjaxPopup() {
        $.openPopupLayer({
            name: "ContactUserSSelectAjaxPopup",
            parameters: {},
            url: "../ReturnVisit/CC_Contact/Handler.ashx?UserIDS=" + $('#hid_ContactUserS').val() + "&CustID=<%=CustID %>",
            beforeClose: function () {
                if ($('#popupLayer_' + 'ContactUserSSelectAjaxPopup').data('hidUserS')) {
                    var hidUserS = $('#popupLayer_' + 'ContactUserSSelectAjaxPopup').data('hidUserS');
                    var txtUserS = $('#popupLayer_' + 'ContactUserSSelectAjaxPopup').data('txtUserS');

                    $('#hid_ContactUserS').val(hidUserS);
                    $('#ContactUserS').val(txtUserS);
                }

            }
        });
    }
    //加载要编辑的数据
    function fillpage(){
         <%if (Request["action"] == "show")
           {%>
                    $("#fldContent input").hide();
                    $("#fldContent select").hide();
                    $("#link_ContactUserS").hide();
                    $("#fldContent span").hide();
                    $("#fldContent input:checkbox").show();

          <%} %>

        var url = "../ReturnVisit/CC_Contact/Handler.ashx?show=yes";
        var postBody = 'ID=<%=Request["ID"] %>'; //携带的参数
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

            var ContactUserS = $('#ContactUserS');
            var hid_ContactUserS = $('#hid_ContactUserS');
            var Hobby = $('#Hobby');
            //添加样式
            Hobby.addClass("k400");
            var Responsible = $('#Responsible');

            var jsonData = $.evalJSON(data);
            if (jsonData.success) {
                var s = $.evalJSON(jsonData.result);
                EditContactName.val(s.CName);
                $("#Hidden_dept").val(s.DepartMent);
                jsSelectItemByValue(s.DepartMent, document.getElementById('txtContactDepartment'));
                $('#txtOfficeTypeCode').val(s.OfficeTypeCode);
                EditContactDuty.val(s.Title);
                EditContactTele.val(s.OfficeTel);
                EditContactModel.val(s.Phone);
                EditContactEmali.val(s.Email);
                if(s.Fax==null)
                {
                    s.Fax="";
                }
                EditContactFax.val(s.Fax);
                EditContactRemark.val(s.Remark);
                SelectPID.val(s.PID);

                txtAddress.val(s.Address);
                txtZipCode.val(s.ZipCode);
                txtMSN.val(s.MSN);
                txtBirthday.val(s.Birthday);

                if (s.Sex == 1) {
                    var ra1 = document.getElementById('radioBoy');
                    ra1.checked = true;

                }
                else {
                    var ra2 = document.getElementById('radioGirl');
                    ra2.checked = true;
                }
                ContactUserS.val(s.ContactUserS);
                hid_ContactUserS.val(s.hid_ContactUserS);
                Hobby.val(s.Hobby);
                Responsible.val(s.Responsible);

                <%if (Request["action"] == "show")
                  {%>

                EditContactName.before(s.CName);
                $("#txtContactName").remove();
                EditContactDepartment.before(s.DepartMent);
                $("#txtContactDepartment").remove();
                $('#txtOfficeTypeCode').before($("#txtOfficeTypeCode").find("option:selected").text());
                $("#txtOfficeTypeCode").remove();
                EditContactDuty.before(s.Title);
                $("#txtContactDuty").remove();
                EditContactTele.before(s.OfficeTel);
                $("#txtContactTele").remove();
                EditContactModel.before(s.Phone);
                $("#txtContactModel").remove();
                EditContactEmali.before(s.Email);
                $("#txtContactEmali").remove();
                EditContactFax.before(s.Fax);
                $("#txtContactFax").remove();
                EditContactRemark.before("<span class='k400' >"+s.Remark+"<span>");
                $("#txtContactRemark").remove();
                SelectPID.before($("#SelectPID").find("option:selected").text().replace("请选择直接上级",""));
                $("#SelectPID").remove();

                txtAddress.before(s.Address);
                $("#txtAddress").remove();
                txtZipCode.before(s.ZipCode);
                $("#txtZipCode").remove();
                txtMSN.before(s.MSN);
                $("#txtMSN").remove();
                txtBirthday.before(s.Birthday);
                $("#txtBirthday").remove();
                
                if (s.Sex == 1) {
                    $('#radioBoy').before("男");
                    $("#radioBoy").remove();
                    $("#radioGirl").remove();

                }
                else {
                    $('#radioBoy').before("女");
                    $("#radioBoy").remove();
                    $("#radioGirl").remove();
                }
                ContactUserS.before(s.ContactUserS);
                $("#ContactUserS").remove();                
                Hobby.before(s.Hobby);
                $("#Hobby").remove();
                Responsible.before(s.Responsible);
                $("#Responsible").remove();


                $("#CreateUserID").before(s.CreateUserTrueName);
                $("#CreateUserID").remove();
                $('#CreateTime').before(s.CreateTimeFormat);
                $("#CreateTime").remove();
                $('#ModifyUserID').before(s.ModifyUserTrueName);
                $("#ModifyUserID").remove();
                $('#ModifyTime').before(s.ModifyTimeFormat);
                $("#ModifyTime").remove();
                $("#condiv ol li").find("span").remove();
                $("#condiv ol li").find("img").remove();
                $("#date_click1").remove();
                <%} %>
            }
            else {
                $.jAlert('数据加载失败');
                return;
            }
        });
    }
    //初始化已经关联的数据
    function loadMappingData() {
        var url = "../ReturnVisit/CC_Contact/Handler.ashx";
        var postBody = "action=getmember" +
            '&ID=<%=Request["ID"]%>' +
            "&random=" + Math.random();
        AjaxPost(url, postBody, null, function (data) {
            var jsonData = $.evalJSON(data);
            $.each(jsonData, function (i, n) {
                $(":checkbox").each(function () {
                    if (n.MemberID == $(this).val()) {
                        if( $(this).attr("name")=="checkBoxAll") {
                            $(this).attr("checked", true);
                        } else if ($(this).attr("name")=="checkBoxMain" && n.IsMain ==1) {
                            $(this).attr("checked", true);
                             //显示邮箱必填*号
                            $("#spanEmail").show();
                        }
                    }
                });
            });
        });
    }

    function jsSelectItemByValue(objItemText, objSelect) {
        //判断是否存在 
        if(objSelect==null) return false;
        var isExit = false;
        for (var i = 0; i < objSelect.options.length; i++) {
            if (objSelect.options[i].text == objItemText) {
                objSelect.options[i].selected = true;
                isExit = true;
                break;
            }
        }
    }
</script>
<script type="text/javascript">
    //会员选择检查
    function checkMemberSelect(e) {
        if (!$(e).attr("checked")) {
            //隐藏邮箱必填*号
            $("#spanEmail").hide();
            $(":checkbox").each(function () {
                if ($(this).attr("name") == "checkBoxMain" && $(e).val() == $(this).val() && $(this).attr("checked")) {
                    $(this).attr("checked", false);
                } else {
                    if ($(this).attr("name") == "checkBoxMain" && $(this).attr("checked")) {
                        //显示邮箱必填*号
                        $("#spanEmail").show();
                    }
                }
            });
        }
    }

    //负责人选中检查
    function checkIsMainSelect(e) {
        if ($(e).attr("checked")) {
            var url = "../ReturnVisit/CC_Contact/Handler.ashx";
            var postBody = "action=ishasmanager" +
            '&MemberID=' + $(e).val() +
            "&random=" + Math.random();
            AjaxPost(url, postBody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.CName != '') {
                    $.jConfirm('该会员已有车易通负责人（' + jsonData.CName + '）,是否替换？', function (result) {
                        if (result) {
                            //显示邮箱必填*号
                            $("#spanEmail").show();
                            //选中时会员也选中
                            $(":checkbox").each(function () {
                                if ($(this).attr("name") == "checkBoxAll" && $(e).val() == $(this).val() && !$(this).attr("checked")) {
                                    $(this).attr("checked", true);
                                }
                            });
                        } else {
                            $(e).attr("checked", false);
                        }
                    });
                } else {
                    //显示邮箱必填*号
                    $("#spanEmail").show();
                    //选中时会员也选中
                    $(":checkbox").each(function () {
                        if ($(this).attr("name") == "checkBoxAll" && $(e).val() == $(this).val() && !$(this).attr("checked")) {
                            $(this).attr("checked", true);
                        }
                    });
                }
            });
        } else {
            //隐藏邮箱必填*号
            $("#spanEmail").hide();
            $(":checkbox").each(function () {
                if ($(this).attr("name") == "checkBoxMain" && $(this).attr("checked")) {
                    //隐藏邮箱必填*号
                    $("#spanEmail").show();
                    return;
                }
            });
        }
    }
</script>
<form id="addContactInfo">
<!--弹窗1联系人信息-->
<div class="pop pb15 openwindow" style="width: 700px">
    <div class="title bold">
        <h2>
            客户联系人信息</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('<%=PopupName%>',false);">
        </a></span>
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
        <li style="display: <%=DepartmenDisplay%>; width: 300px">
            <label>
                部门：</label><select id="txtContactDepartment" name="txtContactDepartment" style="width: 147px;">
                    <option value="0">请选择部门</option>
                </select>
            <span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                职务：</label><input id="txtContactDuty" name="txtContactDuty" type="text" class="w125"
                    style="width: 145px;" /><span style="color: #ff0000">*</span> </li>
        <li style="width: 300px">
            <label>
                职级：</label>
            <select id="txtOfficeTypeCode" name="OfficeTypeCode" style="width: 147px;">
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
        <li style="width: 300px; display: none">
            <label>
                跟进员工：</label><input id="ContactUserS" name="ContactUserS" type="text" class="k180" /><input
                    id="hid_ContactUserS" name="hid_ContactUserS" type="hidden" /><a id="link_ContactUserS"
                        href="javascript:ContactUserSSelectAjaxPopup();" style="float: left;">
                        <img alt="选择跟进员工" src="/images/button_001.gif" border="0" /></a>
        </li>
        <li style="width: 300px">
            <label>
                直接上级：</label><select id="SelectPID" name="SelectPID" style="width: 147px;">
                    <option value="0">请选择直接上级</option>
                </select></li>
        <%if (IsShowMember)
          {%>
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
                                车易通负责人<img src="../../Images/question.png" title="用户名和密码以及服务开通提醒&#10;将以短信和邮件方式自动发送给&#10;车易通负责人"
                                    alt="车易通负责人" style="margin-bottom: -5px;" />
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
        <%}%>
    </ul>
    <div class="cont cont_cx" id="divMore" style="display: none">
        <h2 style="">
            <div>
                扩展信息</div>
        </h2>
        <ol>
            <li style="width: 100%">
                <label>
                    地址：</label><input id="txtAddress" name="txtAddress" type="text" style="width: 480px;" /></li>
            <li>
                <label>
                    传真：</label><input id="txtContactFax" name="txtContactFax" type="text" class="k180" /></li>
            <li>
                <label>
                    邮编：</label><input id="txtZipCode" name="txtZipCode" type="text" class="k180" /></li>
            <li>
                <label>
                    出生日期：</label><input id="txtBirthday" name="txtBirthday" type="text" class="k180" />
                <input type="button" onclick="MyCalendar.SetDate(this,document.getElementById('txtBirthday'))"
                    class="date_click" id="date_click1" /></li>
            <li>
                <label>
                    QQ/MSN：</label><input id="txtMSN" name="txtMSN" type="text" class="k180" /></li>
            <li style="width: 100%">
                <label>
                    爱好：</label><input id="Hobby" name="Hobby" type="text" style="width: 480px;" /></li>
            <li style="width: 100%">
                <label>
                    具体负责：</label><input id="Responsible" name="Responsible" type="text" style="width: 480px;" /></li>
            <li class="nowidth">
                <label>
                    备注：</label><textarea id="txtContactRemark" name="txtContactRemark" rows="5" style="width: 480px;"></textarea>
            </li>
            <%if (Request["action"] == "show")
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
            <%} %>
        </ol>
    </div>
    <%if (Request["action"] != "show")
      {%>
    <div class="btn" style="">
        <input type="button" value="保存" class="button" id="btn_ReturnVisit_Add" onclick="javascript:SaveData();" />
    </div>
    <%} %>
</div>
</form>
<input id="Hidden_dept" type="hidden" />
<script type="text/javascript"><%=StrFillPage %></script>
