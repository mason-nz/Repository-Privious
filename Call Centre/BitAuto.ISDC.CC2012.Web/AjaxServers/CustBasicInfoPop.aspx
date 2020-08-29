<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustBasicInfoPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustBasicInfoPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<style type="text/css">
    .workpop ul li
    {
        width: 345px;
    }
    .workpop ul li.gdjl
    {
        clear: both;
        width: 800px;
    }
    .workpop ul li.gdjl textarea
    {
        width: 650px;
        height: 90px;
        margin-top: 10px;
    }
    .workpop ul li
    {
        height: 35px;
    }
    .baseInfo ul label
    {
        width: 80px;
    }
    .pop .ul_auto
    {
        position: absolute;
        left: 126px;
        top: 37px;
        width: 200px;
        height: 100px;
        background: #eee;
        border: 1px solid #666;
        z-index: 99999;
        overflow: hidden;
        padding: 2px;
    }
    .pop .ul_auto li
    {
        width: 200px;
        height: 30px;
        font-size: 14px;
        color: #333;
        cursor: pointer;
    }
</style>
<div class="pop pb15 openwindow workpop" style="width: 450px; height: 230px; position: relative;">
    <div class="title bold">
        <h2>
            保存个人用户信息</h2>
        <span><a href="javascript:void(0)" onclick="closepop();"></a></span>
    </div>
    <div class="baseInfo" style="margin: 10px 0;">
        <ul class="" style="height: 125px;">
            <li style="position: relative; z-index: 999;" id="popAutoLi">
                <label style="font-weight: normal">
                    <span class="redColor">*</span>姓名：</label><span><input type="hidden" id="hdnCustID"
                        value="" /><input type="text" id="txtCustName" class="w200" maxlength="32" style="color: #666" /></span>
            </li>
            <li>
                <label style="font-weight: normal">
                    <span class="redColor">*</span>性别：</label><span><input type="radio" name="sex" id="rdoMan"
                        value="1" /><em onclick="emChkIsChoose(this);">先生</em><input type="radio" id="rdoWomen"
                            name="sex" style="margin-left: 50px;" value="2" /><em onclick="emChkIsChoose(this);">女士</em></span></li>
            <li>
                <label style="font-weight: normal">
                    <span class="redColor">*</span>电话：</label><span><input type="text" id="txtTel1" class="w200"
                        style="color: #666;line-height:25px;" /></span></li>
            <li id="popMember" style="display: none; position: relative;">
                <label style="font-weight: normal">
                    经销商名称：</label><span>
                        <select id="popSelMemberName" style="width: 208px; position: absolute; top: 7px;
                            left: 126px;" runat="server">
                        </select>
                    </span></li>
            <!--按钮-->
            <li id="popBtnSave" style="width: 80px; position: absolute; top: 198px; left: 167px;">
                <input type="button" name="" value="保 存" onclick="popSubmit()" class="btnSave bold"
                    style="border: none; background: url('/Css/img/btn.jpg') repeat-x scroll 0 0 transparent;
                    width: 76px; height: 25px; color: white; cursor: pointer;" />
            </li>
            <!--按钮-->
        </ul>
    </div>
</div>
<script type="text/javascript">
    function popValidate() {
        var msg = "";

        var oName = document.getElementById("txtCustName");
        var oSex = document.getElementsByName("sex");
        var oTel = document.getElementById("txtTel1");
        var nameVal = $.trim(oName.value);
        var sexVal = oSex[0].checked ? oSex[0].value : oSex[1].checked ? oSex[1].value : "";
        var telVal = $.trim(oTel.value);

        if (nameVal == "") {
            msg += "姓名不能为空！<br/>";
        }
        if (sexVal == "") {
            msg += "请选择性别！<br/>";
        }
        if (telVal == "") {
            msg += "电话不能为空！<br/>";
        }
        if ("<%=CustType %>" == "") {
            msg += "客户分类不能为空！<br/>";
        }
        return msg;
    }
    function popSubmit() {
        var validateMsg = popValidate();
        if (validateMsg != "") {
            $.jAlert(validateMsg);
            return false;
        }
        var oName = document.getElementById("txtCustName");
        var oSex = document.getElementsByName("sex");
        var oTel = document.getElementById("txtTel1");
        var oMemberName = document.getElementById("<%=popSelMemberName.ClientID %>");
        var nameVal = encodeURIComponent($.trim(oName.value));
        var sexVal = oSex[0].checked ? oSex[0].value : oSex[1].checked ? oSex[1].value : "";
        var telVal = $.trim(oTel.value);

        var index = oMemberName.selectedIndex;
        var memberID = index != -1 ? oMemberName.options[index].value : "";
        var memberName = index != -1 ? ($.trim(oMemberName.options[index].text)) : "";


        var jsonData = new Object();
        jsonData.PageData = new Object();
        jsonData.PageData.CustType = "<%=CustType %>";
        jsonData.PageData.CBSex = sexVal;
        jsonData.PageData.CBName = nameVal;
        jsonData.PageData.CBMemberName = memberName;
        jsonData.PageData.CBMemberCode = memberID;

        AjaxPostAsync("/AjaxServers/CommonCallHandler.ashx", {
            Action: "InsertBasicInfo",
            JsonData: escape(JSON.stringify(jsonData)),
            Phone: telVal,
            r: Math.random()
        }, null, function (returnValue) {
            if (returnValue) {
                var jsonData = $.evalJSON(returnValue);
                if (jsonData.result == true) {
                    $("#popupLayer_" + "OperCustInfoPop").data("popCustID", jsonData.data);
                    $.jPopMsgLayer("操作成功！", function () {
                        $.closePopupLayer('OperCustInfoPop', true);
                    });
                }
                else {
                    $.jAlert("错误");
                }
            }
        });
    }
</script>
<script type="text/javascript">
    function closepop() {
        try {
            $.closePopupLayer('OperCustInfoPop', false);
        }
        catch (e) { }
    }
</script>
<script type="text/javascript">
    $(function () {
        init();
        var oMember = document.getElementById("popMember");
        if (oMember.style.display == "none") {
            document.getElementById("popBtnSave").style.top = "168px";
        }
    });

    function init() {
        var oName = document.getElementById("txtCustName");
        var oSex = document.getElementsByName("sex");
        var oTel = document.getElementById("txtTel1");

        if ("<%=CustName %>" != "") {
            oName.value = "<%=CustName %>";
        }
        if ("<%=Tel %>" != "") {
            oTel.value = "<%=Tel %>";
        }
        var sex = "<%=Sex %>";
        if (sex != "") {
            sex = sex == 0 ? 2 : sex;
            for (var i = 0; i < oSex.length; i++) {
                if (sex == oSex[i].value) {
                    oSex[i].checked = true;
                    break;
                }
            }
        }
        //显示经销商
        if ("<%=CustType %>" == "<%=(int)BitAuto.ISDC.CC2012.Entities.CustTypeEnum.T02_经销商 %>" && "<%=CRMCustID %>" != "") {
            $("#popMember").show();
        }
    }
</script>
<script type="text/javascript">
    $(function () {
        var oCustName = document.getElementById("txtCustName");
        var oPopAutoLi = document.getElementById("popAutoLi");
        var popCustNameVal = "<%=CustName %>";
        var keyIndex = -1;

        //智能提示
        oCustName.onkeyup = function (ev) {
            var oEvent = ev || event;
            if (oPopAutoLi.getElementsByTagName("ul")[0]) {
                var oUl = oPopAutoLi.getElementsByTagName("ul")[0];
                var aLi = oUl.getElementsByTagName("li");
                for (var k = 0; k < aLi.length; k++) {
                    aLi[k].style.background = "";
                }
                //向下
                if (oEvent.keyCode == 40) {
                    if (keyIndex >= aLi.length - 1) {
                        keyIndex = 0;
                    }
                    else {
                        ++keyIndex;
                    }
                    aLi[keyIndex].style.background = "#ccc";
                    return false;
                }
                //向上
                if (oEvent.keyCode == 38) {
                    if (keyIndex <= 0) {
                        keyIndex = aLi.length - 1;
                    }
                    else {
                        --keyIndex;
                    }
                    aLi[keyIndex].style.background = "#ccc";
                    return false;
                }
                //回车
                if (oEvent.keyCode == 13) {
                    $("#txtCustName").val(aLi[keyIndex].innerHTML);
                    popCustNameVal = $.trim($("#txtCustName").val());
                    oPopAutoLi.removeChild(oPopAutoLi.getElementsByTagName("ul")[0]);
                    keyIndex = -1;
                    return false;
                }

            }
            if (popCustNameVal == $("#txtCustName").val()) {
                return false;
            }
            if (oPopAutoLi.getElementsByTagName("ul")[0]) {
                oPopAutoLi.removeChild(oPopAutoLi.getElementsByTagName("ul")[0]);
            }

            AjaxPostAsync("/AjaxServers/ContactTips.ashx", { keyWord: $.trim($("#txtCustName").val()), Tel: $.trim($("#txtTel1").val()), r: Math.random() }, null, function (data) {
                if (data == "") {
                    return false;
                }
                var jsonData = eval(data);
                var oUl = document.createElement("ul");
                oUl.className = "ul_auto";
                for (var i = 0; i < jsonData.length; i++) {

                    var oLi = document.createElement("li");
                    oLi.innerHTML = jsonData[i].CustName;
                    oLi.onclick = function () {
                        oCustName.value = this.innerHTML;
                        popCustNameVal = this.innerHTML;
                        oPopAutoLi.removeChild(oPopAutoLi.getElementsByTagName("ul")[0]);
                    }
                    oLi.onmouseover = function () {
                        this.style.background = "#ccc";
                    }
                    oLi.onmouseout = function () {
                        this.style.background = "";
                    }
                    oUl.appendChild(oLi);
                }
                oUl.style.height = jsonData.length * 30 + "px";
                oPopAutoLi.appendChild(oUl);
                keyIndex = -1;

            });
            popCustNameVal = $.trim($("#txtCustName").val());
            return false;
        }

        document.onclick = function () {
            try {
                if (oPopAutoLi.getElementsByTagName("ul")[0]) {
                    oPopAutoLi.removeChild(oPopAutoLi.getElementsByTagName("ul")[0]);
                    popCustNameVal = $.trim($("#txtCustName").val());
                    keyIndex = -1;
                }
            }
            catch (e) { }
        }
    });
</script>
