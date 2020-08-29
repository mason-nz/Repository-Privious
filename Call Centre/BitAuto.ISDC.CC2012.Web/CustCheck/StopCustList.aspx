<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="StopCustList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.StopCustList"
    Title="客户核实" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script type="text/javascript">
        $(document).ready(function () {
            //search();
            //敲回车键执行方法
            enterSearch(search);
        });

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


            //所属大区
            var ddlArea = $("select[id='<%=this.ddlArea.ClientID %>']").val();

         

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
            var R = Math.random()


            var str = "CustName=" + CustNameSelect + "&TaskID=" + TaskIDSelect + "&YpOrderID=" + YpOrderIDSelect + "&DealPerson=" + DealPersonSelect + "&CreatBeginTime=" + CreatBeginTimeSelect + "&CreatEndTime=" + CreatEndTimeSelect + "&SubBeginTime=" + SubBeginTimeSelect + "&SubEndTime=" + SubEndTimeSelect + "&OrderType=" + OrderTypeSelect + "&DealerHave=" + DealerHaveSelect + "&TaskStatus=" + TaskStatusSelect + "&Reason=" + ReasonSelect + "&ddlArea=" + ddlArea + "&ProvinceID=" + ProvinceID + "&CityID=" + CityID + "&R=" + Math.random();
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

//        //点击文字，选中复选框
//        function emChkIsChoose(othis) {
//            var $checkbox = $(othis).prev();
//            if ($checkbox.is(":checked")) {
//                $checkbox.removeAttr("checked");
//            }
//            else {
//                $checkbox.attr("checked", "checked");
//            }
//        }

    </script>
    
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    客户ID：</label>
                <input type="text" id="txtCustID" class="w85" style="width: 150px" />
            </li>
            <li style="width: 342px;">
                <label>
                    申请时间：</label>
                <input type="text" name="ApplyBeginTime" style="width: 106px" id="ApplyBeginTime" onclick="MyCalendar.SetDate(this,document.getElementById('ApplyBeginTime'))"
                    class="w85" />
                至
                <input type="text" name="ApplyEndTime" style="width: 106px" id="ApplyEndTime" onclick="MyCalendar.SetDate(this,document.getElementById('ApplyEndTime'))"
                    class="w85" />
            </li>
            <li>
                <label>
                    大区：</label>
                <span>
                    <select id="ddlArea" runat="server" class="w195" style="width: 155px">
                    </select></span></li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" class="w190" style="width: 150px" />
            </li>
            <li style="width: 342px;">
                <label>
                    审核时间：</label>
                <input type="text" name="SubmitBeginTime" id="SubmitBeginTime" class="w85" style="width: 106px" onclick="MyCalendar.SetDate(this,document.getElementById('SubmitBeginTime'))"/>
                至
                <input type="text" name="SubmitEndTime" id="SubmitEndTime" class="w85" style="width: 106px" onclick="MyCalendar.SetDate(this,document.getElementById('SubmitEndTime'))"/>
            </li>
            <li>
                <label>
                    操作人：</label><select id="ddlOperator" class="w80" style="width: 80px" 
                        runat="server"></select></li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    申请人：</label>
                <input type="text" id="txtApplyer" class="w190" style="width: 150px" />
            </li>
            <li style="width: 342px;">
                <label>
                    停用时间：</label>
                <input type="text" name="StopBeginTime" id="StopBeginTime" class="w85" style="width: 106px" onclick="MyCalendar.SetDate(this,document.getElementById('StopBeginTime'))"/>
                至
                <input type="text" name="StopEndTime" id="StopEndTime" class="w85" style="width: 106px" onclick="MyCalendar.SetDate(this,document.getElementById('StopEndTime'))"/>
            </li>
            <li>
                <label>
                    申请状态：</label><span><select id="ddlStopStatus" class="w195" runat="server" style="width: 155px"></select></span></li>
        </ul>
        <ul>
        </ul>
        <ul class="clear">
        </ul>
    </div>
    <div class="optionBtn clearfix">
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
