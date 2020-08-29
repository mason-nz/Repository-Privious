<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="MainList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder.MainList"
    Title="无主订单管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../../js/GooCalendar.js"></script>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
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
            GetselReson();
            search();
            //敲回车键执行方法
            enterSearch(search);


            $('#SubBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'SubEndTime\')}', onpicked: function () { document.getElementById("SubEndTime").focus(); } }); });
            $('#SubEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'SubBeginTime\')}' }); });


            $('#CreatBeginTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 00:00:00", maxDate: '#F{$dp.$D(\'CreatEndTime\')}', onpicked: function () { document.getElementById("CreatEndTime").focus(); } }); });
            $('#CreatEndTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", minDate: '#F{$dp.$D(\'CreatBeginTime\')}' }); });

        });
        var CustBaseInfoHelper = (function () {
            var triggerProvince = function () {//选中省份
                $.post("../../AjaxServers/CustBaseInfo/Handler.ashx", {
                    Action: "GetDepartID",
                    AreaID: $("#<%=this.ddlProvince.ClientID %>").val()
                },
                function (data) {
                    if (data) {
                        $("#<%=this.ddlArea.ClientID %>").val(data);
                    }
                })
                BindCity('<%=this.ddlProvince.ClientID %>', '<%=this.ddlCity.ClientID %>');

            }
            return {
                TriggerProvince: triggerProvince
            }
        })();
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('.bit_table').load('../../AjaxServers/TaskManager/NoDealerOrder/MainList.aspx .bit_table > *', pody);
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
            //易湃订单id
            var txtYpOrderID = $.trim($("#txtYpOrderID").val());
            if (Len(txtYpOrderID) > 0) {

                if (!isNum(txtYpOrderID)) {
                    $.jAlert("易湃单号格式不正确！");
                    return false;
                }
            }

            if ($.trim(CreatBeginTime).length > 0) {
                if ($.trim(CreatEndTime).length == 0) {
                    $.jAlert("请输入创建时间的结束时间！", function () {
                        $('#CreatEndTime').focus();
                    });
                    return false;
                }
            }
            if ($.trim(CreatEndTime).length > 0) {
                if ($.trim(CreatBeginTime).length == 0) {
                    $.jAlert("请输入创建时间的开始时间！", function () {
                        $('#CreatBeginTime').focus();
                    });
                    return false;
                }
            }


            if ($.trim(CreatBeginTime).length > 0) {
                if (!($.trim(CreatBeginTime).isDateTime())) {
                    $.jAlert("您输入的创建时间的开始时间格式不正确！", function () {
                        $('#CreatBeginTime').val('');
                        $('#CreatBeginTime').focus();
                    });
                    return false;
                }
            }
            if ($.trim(CreatEndTime).length > 0) {
                if (!($.trim(CreatEndTime).isDateTime())) {
                    $.jAlert("您输入的创建时间的结束时间格式不正确！", function () {
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
                    $.jAlert("您输入的创建日期开始时间必须小于等于创建日期结束时间！");
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
            //任务id
            var txtTaskID = $.trim($("#txtTaskID").val());

            if (Len(txtTaskID) > 0) {

                if (!isNum(txtTaskID)) {
                    $.jAlert("任务ID格式不正确！");
                    return;
                }
            }
            //易湃订单id
            var txtYpOrderID = $.trim($("#txtYpOrderID").val());
            if (Len(txtYpOrderID) > 0) {

                if (!isNum(txtYpOrderID)) {
                    $.jAlert("易湃单号格式不正确！");
                    return;
                }
            }
            //处理人
            var ddlDealPerson = $("select[id$='ddlDealPerson']").val();
            //订单类型
            var OrderType = $("input[name='OrderType']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //任务类型
            var TaskType = $("input[name='TaskType']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');

            //推荐经销售商
            var DealerHave = $("input[name='DealerHave']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //任务状态
            var TaskStatus = $("input[name='TaskStatus']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //创建任务时间范围
            var CreatBeginTime = $.trim($("#CreatBeginTime").val());
            var CreatEndTime = $.trim($("#CreatEndTime").val());

            //提交任务时间范围
            var SubBeginTime = $.trim($("#SubBeginTime").val());
            var SubEndTime = $.trim($("#SubEndTime").val());

            //理由

            var Reson = $("#<%=this.selReason.ClientID %>").val();

            //所属大区
            var ddlArea = $("select[id='<%=this.ddlArea.ClientID %>']").val();

            //所属省份
            var ddlProvince = $("select[id='<%=this.ddlProvince.ClientID %>']").val();
            //所属城市
            var ddlCity = $("select[id='<%=this.ddlCity.ClientID %>']").val();

            $('#hidProvinceID').val(ddlProvince);
            $('#hidCityID').val(ddlCity);

            var pageSize = $("#hidSelectPageSize").val();

            var CustNameSelect = encodeURIComponent(custName);
            var TaskIDSelect = encodeURIComponent(txtTaskID);
            var YpOrderIDSelect = encodeURIComponent(txtYpOrderID);
            var DealPersonSelect = encodeURIComponent(ddlDealPerson);
            var CreatBeginTimeSelect = encodeURIComponent(CreatBeginTime);
            var CreatEndTimeSelect = encodeURIComponent(CreatEndTime);
            var SubBeginTimeSelect = encodeURIComponent(SubBeginTime);
            var SubEndTimeSelect = encodeURIComponent(SubEndTime);
            var OrderTypeSelect = encodeURIComponent(OrderType);
            var DealerHaveSelect = encodeURIComponent(DealerHave);
            var TaskStatusSelect = encodeURIComponent(TaskStatus);
            var ReasonSelect = encodeURIComponent(Reson);
            var ddlArea = encodeURIComponent(ddlArea);
            var ProvinceID = encodeURIComponent(ddlProvince);
            var CityID = encodeURIComponent(ddlCity);
            var TaskType = encodeURIComponent(TaskType);
            var pageSize = encodeURIComponent(pageSize);
            var R = Math.random()


            var str = "CustName=" + CustNameSelect + "&TaskID=" + TaskIDSelect + "&YpOrderID=" + YpOrderIDSelect + "&DealPerson=" + DealPersonSelect + "&CreatBeginTime=" + CreatBeginTimeSelect + "&CreatEndTime=" + CreatEndTimeSelect + "&SubBeginTime=" + SubBeginTimeSelect + "&SubEndTime=" + SubEndTimeSelect + "&OrderType=" + OrderTypeSelect + "&DealerHave=" + DealerHaveSelect + "&TaskStatus=" + TaskStatusSelect + "&Reason=" + ReasonSelect + "&ddlArea=" + ddlArea + "&ProvinceID=" + ProvinceID + "&CityID=" + CityID + "&TaskType=" + TaskType + "&pageSize=" + pageSize + "&R=" + Math.random();
            return str;
        }
        //查询
        function search() {

            //加载查询结果
            if (SearchBool()) {

                LoadingAnimation("ajaxTable");
                $("#ajaxTable").load("../../AjaxServers/TaskManager/NoDealerOrder/MainList.aspx", GetPody());
            }
        }

        function GetselReson() {
            if ($("#Dealerno").attr("checked")) {
                $("#Reasonli").css("display", "");
            }
            else {

                $("#Reasonli").css("display", "none");
                $("#<%=this.selReason.ClientID %>").val("-1");
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

        function OperationLogPop2(taskid) {
            $.openPopupLayer({
                name: "SelectCustUserPopup",
                parameters: { TaskID: taskid },
                url: "/CustInfo/MoreInfo/StopCustTaskOperationLog.aspx",
                afterClose: function (e, data) {

                }
            });
        }
        function OperationLogPop(taskid) {
            $.openPopupLayer({
                name: "OperationLogPop",
                parameters: { TaskID: taskid },
                url: "/CustInfo/MoreInfo/CustAssignUserList.aspx",
                afterClose: function (e, data) {

                }
            });
        }
    </script>
    <script type="text/javascript">
        function openSelectActivityAjaxPopup() {
            $.openPopupLayer({
                name: "selectActivityAjaxPopup",
                //parameters: { ProvinceID: '1', CityID: '21', BrandID: '30', ActivityIDs: 'a83ed195-3668-4bd6-a466-93c1680701b0,71c0a894-08d2-4d73-884e-33147b005c39', r: Math.random() },
                //parameters: { ProvinceID: '21', CityID: '-1', BrandID: '30', ActivityIDs: 'a83ed195-3668-4bd6-a466-93c1680701b0,71c0a894-08d2-4d73-884e-33147b005c39', r: Math.random() },
                parameters: { ProvinceID: '2', CityID: '-1', BrandID: '17', ActivityIDs: 'a83ed195-3668-4bd6-a466-93c1680701b0,71c0a894-08d2-4d73-884e-33147b005c39', r: Math.random() },
                url: "/TemplateManagement/SelectActivity.aspx",
                beforeClose: function (b, cData) {
                    if (b) {
                        alert("ActivityIDs=" + cData.ActivityIDs);
                        alert("ActivityNames=" + cData.ActivityNames);
                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                }
            });
        }

        function test() {
            window.external.WebMakeCall("13581797617");
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
            <li style="width: 342px;">
                <label>
                    提交日期：</label>
                <input type="text" name="SubBeginTime" style="width: 106px" id="SubBeginTime" class="w85" />
                至
                <input type="text" name="SubEndTime" style="width: 106px" id="SubEndTime" class="w85" />
            </li>
            <li>
                <label>
                    处理人：</label>
                <span>
                    <select id="ddlDealPerson" runat="server" class="w195" style="width: 155px">
                    </select></span></li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    易湃单号：</label>
                <input type="text" id="txtYpOrderID" class="w190" style="width: 150px" />
            </li>
            <li style="width: 342px;">
                <label>
                    创建日期：</label>
                <input type="text" name="CreatBeginTime" id="CreatBeginTime" class="w85" style="width: 106px" />
                至
                <input type="text" name="CreatEndTime" id="CreatEndTime" class="w85" style="width: 106px" />
            </li>
            <li>
                <label>
                    地区：</label><select id="ddlProvince" class="w80" style="width: 80px" onchange="javascript:CustBaseInfoHelper.TriggerProvince()"
                        runat="server"></select><select id="ddlCity" class="w80" style="margin-left: 5px;
                            width: 70px" runat="server"></select></li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    任务ID：</label>
                <input type="text" id="txtTaskID" class="w190" style="width: 150px" />
            </li>
            <li style="width: 342px;">
                <label>
                    任务状态：</label>
                <span>
                    <input type="checkbox" name="TaskStatus" id="waitfp" value="1" /><em onclick="emChkIsChoose(this)">待分配</em></span>&nbsp;<span><input
                        type="checkbox" name="TaskStatus" id="waitdeal" value="2" /><em onclick="emChkIsChoose(this)">待处理</em></span>&nbsp;<span><input
                            type="checkbox" name="TaskStatus" id="dealing" value="3" /><em onclick="emChkIsChoose(this)">处理中</em></span>&nbsp;<span><input
                                type="checkbox" name="TaskStatus" id="dealed" value="4" /><em onclick="emChkIsChoose(this)">已处理</em></span>
            </li>
            <li>
                <label>
                    分属大区：</label><span><select id="ddlArea" class="w195" runat="server" style="width: 155px"></select></span></li>
        </ul>
        <ul style="clear: both;">
            <li style="width: 244px">
                <label>
                    订单类型：</label>
                <span>
                    <input type="checkbox" name="OrderType" id="newCar" value="1" /><em onclick="emChkIsChoose(this)">新车</em></span>&nbsp;<span><input
                        type="checkbox" name="OrderType" id="ReplaceCar" value="2" /><em onclick="emChkIsChoose(this)">置换</em></span>&nbsp;<span><input
                            type="checkbox" name="OrderType" id="TestDrive" value="3" /><em onclick="emChkIsChoose(this)">试驾</em></span></li>
            <li style="width: 341px">
                <label>
                    推荐经销商：</label>
                <span>
                    <input type="checkbox" name="DealerHave" value="1" id="Dealeryes" /><em onclick="emChkIsChoose(this)">是</em></span>&nbsp;<span><input
                        type="checkbox" name="DealerHave" id="Dealerno" onclick="GetselReson()" value="0" />否</span>
            </li>
            <li id="Reasonli" style="display: none">
                <label>
                    理由：</label>
                <span>
                    <select id="selReason" runat="server" class="w180" style="width: 155px">
                    </select></span> </li>
        </ul>
        <ul style="clear: both;">
            <li>
                <label>
                    任务类型：</label><span>
                        <input type="checkbox" name="TaskType" id="NoDealercb" value="0" /><em onclick="emChkIsChoose(this)">无主订单</em></span>&nbsp;<span><input
                            type="checkbox" name="TaskType" id="HaveDealercb" value="1" /><em onclick="emChkIsChoose(this)">免费订单</em></span></li>
            <li class="btnsearch">
                <input type="button" value="查 询" id="btnsearch" class="searchBtn bold" onclick="search()"
                    name="" /></li>
            <li class="btnsearch">
                <input type="button" value="测 试" id="Button1" class="searchBtn bold" onclick="OperationLogPop('363')"
                    name="" />
                <a href='/OracleTest.aspx' target="_blank">测试2</a></li>
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
