<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCustBaseInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.UCNoDealerOrder.EditCustBaseInfo" %>
<script type="text/javascript">
    $(document).ready(function () {
        BindProvince('<%=this.ddlProvince.ClientID %>'); //绑定省份
        //CustBaseInfoHelper.TriggerProvince();

        if ('<%=RequstTaskID %>' != "") {
            $("#<%=this.ddlProvince.ClientID %>").val('<%=ProvinceID %>');
            CustBaseInfoHelper.TriggerProvince();
            $("#<%=this.ddlCity.ClientID %>").val('<%=CityID %>');
            CustBaseInfoHelper.TriggerCity();
            $("#<%=this.ddlCounty.ClientID %>").val('<%=CountyID %>');



        }

    });
    var CustBaseInfoHelper = (function () {
        var triggerProvince = function () {//选中省份
            $.post("/AjaxServers/CustBaseInfo/Handler.ashx", {
                Action: "GetDepartID",
                AreaID: $("#<%=this.ddlProvince.ClientID %>").val()
            },
            function (data) {
                if (data) {
                    $("#<%=this.ddlArea.ClientID %>").val(data);
                }
            })
            BindCity('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>');
            BindCounty('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>', '<%=this.ddlCounty.ClientID %>');
            $("#hidprovinceid").val($("#<%=this.ddlProvince.ClientID %>").val());
            $("#hidcityid").val($("#<%=this.ddlCity.ClientID %>").val());
        },

        triggerCity = function () {//选中城市
            BindCounty('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>', '<%=this.ddlCounty.ClientID %>');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#ddlCounty option').size() == 1)
            { $('#ddlCounty').attr('noCounty', '1'); }
            else
            { $('#ddlCounty').removeAttr('noCounty'); }
            $("#hidprovinceid").val($("#<%=this.ddlProvince.ClientID %>").val());
            $("#hidcityid").val($("#<%=this.ddlCity.ClientID %>").val());
        }
        return {
            TriggerProvince: triggerProvince,
            TriggerCity: triggerCity
        }
    })();
    CustBaseInfoHelper.GetAllDataInPage = function () {
        var TaskID = $("#hdnTaskID").val();
        var custName = $.trim($("#<%=this.txtCustName.ClientID %>").val());
        var province = $("#<%=this.ddlProvince.ClientID %>").val();
        var city = $("#<%=this.ddlCity.ClientID %>").val();
        var county = $("#<%=this.ddlCounty.ClientID %>").val();
        var txtTel1 = $.trim($("#<%=this.txtTel1.ClientID %>").val());
        var txtTel2 = $.trim($("#<%=this.txtTel2.ClientID %>").val());
        var sex = 0;
        $("input[name$='sex']").each(function () {
            if ($(this).attr("checked")) {
                sex = $(this).val();
            }
        });
        var areaId = $("select[id$='ddlArea']").val();


        var email = $.trim($("#<%=this.txtEmail.ClientID %>").val());
        var dataSource = "易湃";
        var custType = $("#hidType").val();
        var address = $.trim($("#<%=this.txtAddress.ClientID %>").val());
        var custInfo = {
            TaskID: TaskID,
            CustName: custName,
            ProvinceID: province,
            CityID: city,
            CountyID: county,
            Sex: sex,
            AreaID: areaId,
            UserPhone: txtTel1,
            UserMobile: txtTel2,
            Email: email,
            CustCategoryID: custType,
            Address: address
        };
        return { CustBaseInfo: custInfo }
    }
    CustBaseInfoHelper.ValidateData = function (custBaseInfo) {
        var msg = "";
        if (custBaseInfo) {
            if (Len(custBaseInfo.CustName) == 0) {
                msg += "姓名不能为空 <br/>";
            }
            else if (Len(custBaseInfo.CustName) > 50) {
                msg += "姓名不能超过50个字符 <br/>";
            }
            if (custBaseInfo.ProvinceID == "-1") {
                msg += "省/直辖市不能为空<br/>"
            }
            if (custBaseInfo.CityID == "-1") {
                if (!isNum(custBaseInfo.CityID) && Len(custBaseInfo.CityID) > 0) {
                    msg += "城市选择不能为空 <br/>";
                }
            }

            if (custBaseInfo.ProvinceID != "-1") {
                if (!isNum(custBaseInfo.ProvinceID) && Len(custBaseInfo.ProvinceID) > 0) {
                    msg += "省/直辖市选择有误 <br/>";
                }
            }
            if (custBaseInfo.CityID != "-1") {
                if (!isNum(custBaseInfo.CityID) && Len(custBaseInfo.CityID) > 0) {
                    msg += "城市选择有误 <br/>";
                }
            }
            if (custBaseInfo.CountyID != "-1") {
                if (!isNum(custBaseInfo.CountyID) && Len(custBaseInfo.CountyID) > 0) {
                    msg += "区/县选择有误 <br/>";
                }
            }

            if (custBaseInfo.Sex == "0") {
                msg += "请选择客户性别 <br/>";
            }
            if (!isNum(custBaseInfo.AreaID) && Len(custBaseInfo.AreaID) > 0) {
                msg += "所属大区选择有误 <br/>";
            }
            if (Len(custBaseInfo.UserPhone) == 0 && Len(custBaseInfo.UserMobile) == 0) {
                msg += "电话至少要有一个不为空 <br/>";
            }
            if (Len(custBaseInfo.UserPhone) > 0) {
                if (!isNum(custBaseInfo.UserPhone)) {
                    msg += "电话1号码不能有符号 <br/>";
                }
                else if (!isTelOrMobile(custBaseInfo.UserPhone)) {
                    msg += "电话1（" + custBaseInfo.UserPhone + "）格式不正确 <br/>";
                }
            }
            if (Len(custBaseInfo.UserMobile) > 0) {
                if (!isNum(custBaseInfo.UserMobile)) {
                    msg += "电话2号码不能有符号 <br/>";
                }
                else if (!isTelOrMobile(custBaseInfo.UserMobile)) {
                    msg += "电话2（" + custBaseInfo.UserMobile + "）格式不正确 <br/>";
                }
            }

            if (Len(custBaseInfo.UserPhone) > 0) {
                if (custBaseInfo.UserPhone == custBaseInfo.UserMobile) {
                    msg += "电话1，电话2不能重复 <br/>";
                }
            }
            if (Len(custBaseInfo.Address) == 0) {
                msg += "客户地址不能为空<br/>";
            }
            if (Len(custBaseInfo.Address) > 200) {
                msg += "客户地址不能超过200个字符 <br/>";
            }
            if (Len(custBaseInfo.Email) > 0 && !isEmail(custBaseInfo.Email)) {
                msg += "客户邮箱格式不正确 <br/>";
            }
            if (Len(custBaseInfo.CustCategoryID) == 0) {
                msg += "请选择客户分类 <br/>";
            }
        }
        return msg;
    }
    CustBaseInfoHelper.encodeParams = function (data) {//为参数编码 info.CustInfo, info.MemberInfoArray
        if (data) {
            for (var k in data.CustBaseInfo) {
                data.CustBaseInfo[k] = encodeURIComponent(data.CustBaseInfo[k]);
            }
        }
        return data;
    };

    function SubmitDataBasicInfo() {
        var returnResult = false;
        var info = CustBaseInfoHelper.GetAllDataInPage();
        var msg = CustBaseInfoHelper.ValidateData(info.CustBaseInfo);
        if (Len(msg) > 0) {
            $.jAlert(msg);
        }
        else {
            var custBaseInfo = CustBaseInfoHelper.encodeParams(info);
            var data = JSON.stringify(custBaseInfo);
            AjaxPostAsync("../../../AjaxServers/TaskManager/NoDealerOrder/Handler.ashx", { Action: "updatecustbaseinfo", CheckedInfoStr: data },
             function () {
                 $("#btnSave").attr("disabled", "disabled");
                 $("#btnSumbit").attr("disabled", "disabled");
                 $("#imgLoadingPop").show();
             },
              function (returnValue) {
                  var jsonData = $.evalJSON(returnValue);
                  if (jsonData.Result == "yes") {
                      returnResult = true;
                  }
                  else {
                      returnResult = false;
                      $.jAlert(jsonData.ErrorMsg);
                  }
                  $("#btnSave").attr("disabled", "");
                  $("#btnSumbit").attr("disabled", "");
                  $("#imgLoadingPop").hide();
              });
        }
        return returnResult;
    }
    function phone() {
        var Tels = "";
        if ($.trim($("#<%=this.txtTel1.ClientID %>").val()) != "") {
            Tels = Tels + $.trim($("#<%=this.txtTel1.ClientID %>").val()) + ",";
        }
        if ($.trim($("#<%=this.txtTel2.ClientID %>").val()) != "") {
            Tels = Tels + $.trim($("#<%=this.txtTel2.ClientID %>").val()) + ",";
        }
        if (Tels != "") {
            Tels = Tels.substring(0, Tels.length - 1);
        }
        return Tels;
    }
</script>
<ul class="clearfix ">
    <li>
        <label>
            <span class="redColor">*</span>姓名：</label><span><input type="hidden" id="hdnTaskID"
                value="<%=RequstTaskID %>" /><input type="text" id="txtCustName" class="w250" runat="server" /></span></li>
    <li>
        <input type="hidden" id="hidprovinceid" />
        <input type="hidden" id="hidcityid" />
        <label>
            <span class="redColor">*</span>地区：</label><select id="ddlProvince" class="w80" onchange="javascript:CustBaseInfoHelper.TriggerProvince()"
                runat="server"></select><select id="ddlCity" class="w80" style="margin-left: 8px"
                    runat="server" onchange="javascript:CustBaseInfoHelper.TriggerCity()"></select><select
                        id="ddlCounty" runat="server" class="w80" style="margin-left: 8px"></select></li>
    <li>
        <label>
            <span class="redColor">*</span>性别：</label><span><input type="radio" name="sex" id="rdoMan"
                runat="server" value="1" />先生<input type="radio" id="rdoWomen" runat="server" name="sex"
                    style="margin-left: 50px;" value="2" />女士</span></li>
    <li>
        <label>
            分属大区：</label><span><select id="ddlArea" runat="server" class="w255"></select></span></li>
    <li>
        <label>
            <span class="redColor">*</span>电话：</label><span><input type="text" tag="custTel"
                id="txtTel1" class="w120" runat="server" /><input type="text" style="margin-left: 8px;"
                    class="w120" name="custTel" tag="custTel" runat="server" id="txtTel2" /></span><img
                        alt="打电话" style="cursor: pointer" src="../../../Images/phone.gif" border="0"
                        onclick="功能废弃" /></li>
    <li>
        <label>
            <span class="redColor">*</span>地址：</label><span><input type="text" id="txtAddress"
                class="w250" runat="server" /></span></li>
    <li>
        <label>
            邮箱：</label><span><input type="text" id="txtEmail" class="w250" runat="server" /></span></li>
    <li>
        <label>
            数据来源：</label><span>易湃</span></li>
    <li>
        <label>
            <span class="redColor">*</span>客户分类：</label><span><input type="radio" id="rdoHavCar"
                value="1" name="custType" runat="server" />已购车<input type="radio" name="custType"
                    style="margin-left: 30px;" value="2" id="rdoNoCar" runat="server" />未购车</span></li>
</ul>
