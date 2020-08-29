<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="GroupOrderList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.GroupOrder.GroupOrderList"
    Title="团购订单管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../../js/GooCalendar.js"></script>
    <script type="text/javascript" src="/js/bit.dropdownlist.js"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //绑定车款信息
            BindMyBrand();

            selReturnVisit_Change();

            var ddlProvinceLoad = $("#hidProvinceID").val();
            var ddlCityLoad = $("#hidCityID").val();
            BindProvince('<%=this.ddlProvince.ClientID %>'); //绑定省份            
            $("select[id$='ddlProvince']").val(ddlProvinceLoad);
            CustBaseInfoHelper.TriggerProvince();
            $("select[id$='ddlCity']").val(ddlCityLoad);
            var CreatBeginTimeload = $.trim($("#CreatBeginTime").val());
            var CreatEndTimeload = $.trim($("#CreatEndTime").val());

            $("#CreatBeginTime").val(CreatBeginTimeload);
            $("#CreatEndTime").val(CreatEndTimeload);

            search();
            //敲回车键执行方法
            enterSearch(search);

            //提交日期
            $('#SubBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'SubEndTime\')}', onpicked: function () { document.getElementById("SubEndTime").focus(); } }); });
            $('#SubEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'SubBeginTime\')}' }); });

            //下单日期
            $('#CreatBeginTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 00:00:00", maxDate: '#F{$dp.$D(\'CreatEndTime\')}', onpicked: function () { document.getElementById("CreatEndTime").focus(); } }); });
            $('#CreatEndTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", minDate: '#F{$dp.$D(\'CreatBeginTime\')}' }); });

        });

        function selReturnVisit_Change() {
            $("select[id$='ddlFailReason']").val("-2");
            if ($("select[id$='ddlIsReturnVisit']").val() == "0") {
                $('#liFailReason').show();
            }
            else {
                $('#liFailReason').hide();
            }
        }

        var CustBaseInfoHelper = (function () {
            var triggerProvince = function () {//选中省份
                
                BindCity('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>');

            }
            return {
                TriggerProvince: triggerProvince
            }
        })();

        //绑定品牌信息
        function BindMyBrand() {

            //目前驾驶的车
            var options4 = {
                container: { master: "dllMyBrand", serial: "dllMySerial", cartype: "dllMyName" },
                include: { serial: "1", cartype: "1" },
                datatype: 0,
                binddefvalue: { master: '', serial: '', cartype: '' }
            };
            new BindSelect(options4).BindList();


        }

        //分页操作
        function ShowDataByPost2(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../../AjaxServers/GroupOrder/GroupOrderList.aspx .bit_table > *', pody + "&r=" + Math.random());
        }

        function SearchBool() {

            //任务id
            var txtTaskID = $.trim($("#txtTaskID").val());
            //创建任务时间范围
            var CreatBeginTime = $.trim($("#CreatBeginTime").val());
            var CreatEndTime = $.trim($("#CreatEndTime").val());

            //提交任务时间范围
            var SubBeginTime = $.trim($("#SubBeginTime").val());
            var SubEndTime = $.trim($("#SubEndTime").val());
            if (Len(txtTaskID) > 0) {

                if (!isNum(txtTaskID)) {
                    $.jAlert("任务ID格式不正确！");
                    return false;
                }
            }
            //团购订单id
            var txtYpOrderID = $.trim($("#txtOrderID").val());
            if (Len(txtOrderID) > 0) {

                if (!isNum(txtYpOrderID)) {
                    $.jAlert("团购订单编号格式不正确！");
                    return false;
                }
            }

            if ($.trim(CreatBeginTime).length > 0) {
                if ($.trim(CreatEndTime).length == 0) {
                    $.jAlert("请输入下单日期的结束时间！", function () {
                        $('#CreatEndTime').focus();
                    });
                    return false;
                }
            }
            if ($.trim(CreatEndTime).length > 0) {
                if ($.trim(CreatBeginTime).length == 0) {
                    $.jAlert("请输入下单日期的开始时间！", function () {
                        $('#CreatBeginTime').focus();
                    });
                    return false;
                }
            }


            if ($.trim(CreatBeginTime).length > 0) {
                if (!($.trim(CreatBeginTime).isDateTime())) {
                    $.jAlert("您输入的下单日期的开始时间格式不正确！", function () {
                        $('#CreatBeginTime').val('');
                        $('#CreatBeginTime').focus();
                    });
                    return false;
                }
            }
            if ($.trim(CreatEndTime).length > 0) {
                if (!($.trim(CreatEndTime).isDateTime())) {
                    $.jAlert("您输入的下单日期的结束时间格式不正确！", function () {
                        $('#CreatEndTime').val('');
                        $('#CreatEndTime').focus();
                    });
                    return false;
                }
            }


            if ($.trim(SubBeginTime).length > 0) {
                if (!($.trim(SubBeginTime).isDate())) {
                    $.jAlert("您输入的提交日期开始时间格式不正确！", function () {
                        $('#SubBeginTime').val('');
                        $('#SubBeginTime').focus();
                    });
                    return false;
                }
            }
            if ($.trim(SubEndTime).length > 0) {
                if (!($.trim(SubEndTime).isDate())) {
                    $.jAlert("您输入的提交日期结束时间格式不正确！", function () {
                        $('#SubEndTime').val('');
                        $('#SubEndTime').focus();
                    });
                    return false;
                }
            }


            var reg = new RegExp("-", "g");
            if ($.trim(CreatBeginTime).length > 0 && $.trim(CreatEndTime).length > 0) {
                if (Date.parse(CreatEndTime.replace(reg, '/')) - Date.parse(CreatBeginTime.replace(reg, '/')) < 0) {
                    $.jAlert("您输入的下单日期开始时间必须小于等于下单日期结束时间！");
                    return false;
                }
            }
            if (Date.parse(SubEndTime.replace(reg, '/')) - Date.parse(SubBeginTime.replace(reg, '/')) < 0) {
                $.jAlert("您输入的提交日期开始时间必须小于等于提交日期结束时间！");
                return false;
            }

            return true;
        }


        function GetPody() {
            //客户名称
            var custName = $.trim($("#txtCustName").val());

            //订单编号
            var txtOrderID = $.trim($("#txtOrderID").val());
            if (Len(txtOrderID) > 0) {

                if (!isNum(txtOrderID)) {
                    $.jAlert("订单编号格式不正确！");
                    return;
                }
            }

            //任务id
            var txtTaskID = $.trim($("#txtTaskID").val());

            if (Len(txtTaskID) > 0) {

                if (!isNum(txtTaskID)) {
                    $.jAlert("任务ID格式不正确！");
                    return;
                }
            }

            //所属省份
            var ddlProvince = $("select[id='<%=this.ddlProvince.ClientID %>']").val();
            //所属城市
            var ddlCity = $("select[id='<%=this.ddlCity.ClientID %>']").val();

            $('#hidProvinceID').val(ddlProvince);
            $('#hidCityID').val(ddlCity);

            //处理人
            var ddlDealPerson = $("select[id$='ddlDealPerson']").val();
            
            
            //下单经销售商
            var txtDealer = $("#txtDealer").val();

            //任务状态
            var TaskStatus = $("select[id$='ddlTaskStatus']").val();

            //处理状态
            var IsReturnVisit = $("select[id$='ddlIsReturnVisit']").val();

            //提交日期时间范围
            var SubBeginTime = $.trim($("#SubBeginTime").val());
            var SubEndTime = $.trim($("#SubEndTime").val());

            //下单日期时间范围
            var CreatBeginTime = $.trim($("#CreatBeginTime").val());
            var CreatEndTime = $.trim($("#CreatEndTime").val());

            //车款
            var CarMasterName = $("#dllMyBrand").val();
            var CarSerialName = $("#dllMySerial").val();
            var CarName = $("#dllMyName").val();

            //失败理由
            var FailReason = $("#<%=this.ddlFailReason.ClientID %>").val();

            if ($('#liFailReason').css("display") == "none") {
                FailReason = "-2";
            }

            var txtCustomerTel = $.trim($("#txtCustomerTel").val());

            var pageSize = $("#hidSelectPageSize").val();

            var CustNameSelect = encodeURIComponent(custName);
            var OrderIDSelect = encodeURIComponent(txtOrderID);
            var TaskIDSelect = encodeURIComponent(txtTaskID);
            var ProvinceID = encodeURIComponent(ddlProvince);
            var CityID = encodeURIComponent(ddlCity);

            var DealPersonSelect = encodeURIComponent(ddlDealPerson);
            var Dealer = encodeURIComponent(txtDealer);
            var TaskStatusSelect = encodeURIComponent(TaskStatus);
            var IsReturnVisitSelect = encodeURIComponent(IsReturnVisit);
            
            var SubBeginTimeSelect = encodeURIComponent(SubBeginTime);
            var SubEndTimeSelect = encodeURIComponent(SubEndTime);
            var CreatBeginTimeSelect = encodeURIComponent(CreatBeginTime);
            var CreatEndTimeSelect = encodeURIComponent(CreatEndTime);

            var CarMasterNameSelect = encodeURIComponent(CarMasterName);
            var CarSerialNameSelect = encodeURIComponent(CarSerialName);
            var CarNameSelect = encodeURIComponent(CarName);

            var FailReasonSelect = encodeURIComponent(FailReason);

            //txtCustomerTel
            var CustomerTel = encodeURIComponent(txtCustomerTel);

            var pageSize = encodeURIComponent(pageSize);
            var R = Math.random()
            var selectTelCount = $('#selectTelCount').val();

            //var str = "CustName=" + CustNameSelect + "&TaskID=" + TaskIDSelect + "&OrderID=" + OrderIDSelect + "&DealPerson=" + DealPersonSelect + "&CreatBeginTime=" + CreatBeginTimeSelect + "&CreatEndTime=" + CreatEndTimeSelect + "&SubBeginTime=" + SubBeginTimeSelect + "&SubEndTime=" + SubEndTimeSelect + "&OrderType=" + OrderTypeSelect + "&DealerHave=" + DealerHaveSelect + "&TaskStatus=" + TaskStatusSelect + "&Reason=" + ReasonSelect + "&ProvinceID=" + ProvinceID + "&CityID=" + CityID + "&TaskType=" + TaskType + "&pageSize=" + pageSize + "&R=" + Math.random();
            var str = "CustName=" + CustNameSelect + "&OrderCode=" + OrderIDSelect + "&TaskID=" + TaskIDSelect + "&ProvinceID=" + ProvinceID + "&CityID=" + CityID + "&AssignUserID=" + DealPersonSelect + "&Dealer=" + Dealer + "&TaskStatus=" + TaskStatusSelect + "&IsReturnVisit=" + IsReturnVisitSelect + "&CreatetimeBegin=" + CreatBeginTimeSelect + "&CreatetimeEnd=" + CreatEndTimeSelect + "&SubmitTimeBegin=" + SubBeginTimeSelect + "&SubmitTimeEnd=" + SubEndTimeSelect + "&CarMasterID=" + CarMasterNameSelect + "&CarSerialID=" + CarSerialNameSelect + "&CarID=" + CarNameSelect + "&FailReason=" + FailReasonSelect + "&CustomerTel=" + CustomerTel + "&pageSize=" + pageSize +"&TelCount=" + selectTelCount + "&R=" + Math.random();
            return str;
        }
        //查询
        function search() {

            //加载查询结果
            if (SearchBool()) {

                LoadingAnimation("ajaxTable");
                $("#ajaxTable").load("../../AjaxServers/GroupOrder/GroupOrderList.aspx", GetPody());
            }
        }
        

        //点击文字，选中复选框
        function emChkIsChoose(othis) {
            var $checkbox = $(othis).prev();
            if ($checkbox.is(":checked")) {
                $checkbox.removeAttr("checked");
            }
            else {
                $checkbox.attr("checked", "checked");
            }
        }

    </script>
    <style type="text/css">
        .pageP
        {
            width: 200px;
            float: left;
            text-align: left;
            padding-left: 20px;
        }
        
        .pageP a.selectA
        {
            color: Red;
        }
        .pageP a
        {
            height: 50px;
        }
        .pageP a:hover
        {
            font-size: 16px;
        }
    </style>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客户姓名：</label>
                <input type="text" id="txtCustName" class="w85" style="width: 150px" />
            </li>
            <li>
                <label>
                    处理人：</label>
                <span>
                    <select id="ddlDealPerson" runat="server" class="w190" style="width: 150px">
                    </select></span></li>
            <li style="width: 362px;">
                <label>
                    提交日期：</label>
                <input type="text" name="SubBeginTime" style="width: 116px" id="SubBeginTime" class="w85" />
                至
                <input type="text" name="SubEndTime" style="width: 116px" id="SubEndTime" class="w85" />
            </li>            
        </ul>
        <ul class="clear">
            <li>
                <label>
                    订单编号：</label>
                <input type="text" id="txtOrderID" class="w190" style="width: 150px" />
            </li>
            <li>
                <label>
                    下单经销商：</label>
                <input type="text" id="txtDealer" class="w190" style="width: 147px" />
            </li>
            <li style="width: 362px;">
                <label>
                    下单日期：</label>
                <input type="text" name="CreatBeginTime" id="CreatBeginTime" class="w85" style="width: 116px" />
                至
                <input type="text" name="CreatEndTime" id="CreatEndTime" class="w85" style="width: 116px" />
            </li>            
        </ul>
        <ul class="clear">
            <li>
                <label>
                    任务ID：</label>
                <input type="text" id="txtTaskID" class="w190" style="width: 150px" />
            </li>
            <li style="width: 242px;">
                <label>
                    任务状态：</label>
                <span>
                    <select id="ddlTaskStatus" runat="server" class="w190" style="width: 150px">
                    </select></span> </li>             

            <li>
                <label>
                    订购车款：</label>
                <span>                    
                    <select id="dllMyBrand" name="BrandId" class="w125" style="width: 84px;">
                    </select>
                    <select id="dllMySerial" name="SerialId" class="w125" style="width: 84px;">
                    </select>
                    <select id="dllMyName" name="NameId" class="w125" style="width: 84px;" >
                    </select>
                </span></li>
        </ul>
        <ul style="clear: both;">
            <li>
                <label>
                    地区：</label><select id="ddlProvince" class="w80" style="width: 80px" onchange="javascript:CustBaseInfoHelper.TriggerProvince()"
                        runat="server"></select><select id="ddlCity" class="w80" style="margin-left: 5px;
                            width: 70px" runat="server"></select></li>   
            <li>
                <label>
                    处理状态：</label>
                <span>
                    <select id="ddlIsReturnVisit" runat="server" onchange="selReturnVisit_Change()" class="w190" style="width: 150px">
                    </select></span> </li>         
            <li id="liFailReason">
                <label>
                    失败理由：</label>
                <span>
                    <select id="ddlFailReason" runat="server" class="w180" style="width: 260px">
                    </select></span> </li>
        </ul>
        <ul style="clear: both;">
            <li>
                <label>
                    客户电话：</label>
                <input type="text" id="txtCustomerTel" class="w190" style="width: 150px" />
            </li>
            <li>
                <label>
                    用户下单数：</label>
                    <span>
                <select id="selectTelCount" style="width:150px;">
                    <option value="-1" selected="selected">请选择</option>
                    <option value="1">1次</option>
                    <option value="n">多次</option>
                </select></span>
            </li>
            <li class="btnsearch">
                <input type="button" value="查 询" id="btnsearch" class="searchBtn bold" onclick="search()"
                    name="" /></li>
        </ul>
        <ul>
        </ul>
        <ul class="clear">
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="hidden" id="hidProvinceID" value="-1" />
        <input type="hidden" id="hidCityID" value="-1" />
        <input type="hidden" id="hidSelectPageSize" value="" />
        <%if (ImportExcelWriteBack) {%>
        <input name="" type="button" value="导入Excel回写" class="newBtn" onclick="ImportExcelWriteBack()" />
        <%} %>
        <%if (RevokeTask)
          {%>
        <input name="" type="button" value="收回任务" onclick="RevokeTask()" class="newBtn" />
        <%}%>
        <%if (AssignTask)
          { %><input name="" type="button" value="分配任务" class="newBtn" onclick="AssignTask()" /><%} %>          
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
</asp:Content>
