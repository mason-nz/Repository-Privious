<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="回访客户列表页" CodeBehind="ReturnVisitCustList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.ReturnVisitCustList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
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
        .alarmtr
        {
            color: Red;
        }
        /*客户分类 编辑分类按钮样式*/
        .cxTab ul li.w180 a
        {
            border: none;
            font-size: 14px;
            background: #fff;
            color: #666;
        }
        .cxTab ul li.w180 a:hover
        {
            background: #6BBBD6;
            color: #FFF;
            text-decoration: none;
        }
        .cxTab ul li a.cur
        {
            background: #6BBBD6;
            color: #FFF;
        }
    </style>
    <script type="text/javascript" src="../Js/Enum/Area2.js"></script>
    <script type="text/javascript">
        var totlalDataCount
        var isclickSearch = false;
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            BindProvince('<%=ddlSearchProvince.ClientID%>'); //绑定省份
            $("[id$=ddlSearchProvince]").change(function () {
                BindCity('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>');
                BindCounty('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>', '<%=ddlSearchCounty.ClientID%>');
            });
            $("[id$=ddlSearchCity]").change(function () {
                BindCounty('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>', '<%=ddlSearchCounty.ClientID%>');
            });
            //全选/全不选
            $("#ckbAllSelect").live("click", function () {
                $(":checkbox[name='chkSelect']").attr("checked", $(this).attr("checked"));
            });

            $("#ckCheckAllTag").live("click", function () {
                $(".kh_classfied :checkbox[name='ckCustTag']").attr("checked", $(this).attr("checked"));
            });
            isclickSearch = true;
            search(0);
            $('#txtApplyStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtApplyEndTime\')}', onpicked: function () { document.getElementById("txtApplyEndTime").focus(); } }); });
            $('#txtApplyEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtApplyStartTime\')}' }); });
        });
        var _tagid = 0;
        function search(tagid) {
            if (tagid == undefined || tagid == "") {
                tagid = 0;
            }
            _tagid = tagid;
            var pagesize = $("#hidSelectPageSize").val();
            var url = '/AjaxServers/ReturnVisit/ReturnVisitCustList.aspx?pagesize=' + pagesize;
            var txtSearchCustName = $.trim($('#' + '<%=txtSearchCustName.ClientID%>').val());
            var hidSearchBrand = $.trim($('#' + '<%=hidSearchBrand.ClientID%>').val());
            var ddlSearchProvince = $.trim($('#' + '<%=ddlSearchProvince.ClientID%>').val());
            var ddlSearchCity = $.trim($('#' + '<%=ddlSearchCity.ClientID%>').val());
            var ddlSearchCounty = $.trim($('#' + '<%=ddlSearchCounty.ClientID%>').val());
            var txtSearchTrueNameID = $.trim($('#txtSearchTrueNameID').val());

            //add lxw 6.8 增加条件：最近日期 
            var txtApplyStartTime = $.trim($('#txtApplyStartTime').val());
            var txtApplyEndTime = $.trim($('#txtApplyEndTime').val());
            var radioContact = -2;
            radioContact = $("input[id='radioNOType']:checked").map(function () { return $(this).val() }).get().join(",");
            if (radioContact == "") {
                radioContact = -2;
            }

            var responseCk = -2;
            responseCk = $("input[id='responseCk']:checked").map(function () { return $(this).val() }).get().join(",");
            if (responseCk == "") {
                responseCk = -2;
            }

            var clientType = $.trim($('#selCustType').val());
            if (clientType == -2) {
                clientType = "";
            }
            var carType = $('input[name="carType"]:checked').map(function () { return $(this).val() }).get().join(",");

            //add by masj 2012-8-21 取集采项目名
            var txtSearchProjectName = $.trim($("input[id$='txtSearchProjectName']").val());

            url += '&CustName=' + escape(txtSearchCustName);
            url += '&Brand=' + escape(hidSearchBrand);
            url += '&Province=' + escape(ddlSearchProvince);
            url += '&City=' + escape(ddlSearchCity);
            url += '&County=' + escape(ddlSearchCounty);
            url += '&SearchTrueNameID=' + escape(txtSearchTrueNameID);

            url += '&StartTime=' + escape(txtApplyStartTime);
            url += '&EndTime=' + escape(txtApplyEndTime);
            url += '&Contact=' + escape(radioContact);
            //add by qizq
            url += '&ClientType=' + encodeURIComponent(clientType);
            url += '&CarType=' + encodeURIComponent(carType);
            //add by masj 2012-8-21 取集采项目名
            url += '&ProjectName=' + encodeURIComponent(txtSearchProjectName);
            url += '&NoResponser=' + escape(responseCk);
            url += '&TagID=' + escape(tagid);
            LoadingAnimation("divContent");

            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
            $('#divContent').load(url + "&r=" + Math.random(), '', function () {
                var nowDate = $("#nowDt").val();
                $("#tableReturnVisitCust .backDt").each(function () {
                    if ($(this).html() != "") {
                        var compVal = getddToDateTime(nowDate, $.trim($(this).text()));
                        switch (compVal) {
                            case 1: $(this).parent().find(" td:eq(1)").removeClass("alarmtr"); break;
                            case 0: $(this).parent().find(" td:eq(1)").addClass("alarmtr"); break;
                            case -1: $(this).parent().find(" td:eq(1)").addClass("alarmtr"); break;
                            default: break;
                        }
                    }
                    else {
                        $(this).parent().find(" td:eq(1)").removeClass("alarmtr");
                    }
                });
                var podys = "Action=getTagStatisticData&" + url.substr('../AjaxServers/ReturnVisit/ReturnVisitCustList.aspx?'.length) + "&r=" + Math.random();
                StatAjaxPageTime(monitorPageTime, url);
            })
        }

        function getddToDateTime(nowDt, compDt) {
            var sdate = nowDt.replace(/\-/g, "\/");
            var edate = compDt.replace(/\-/g, "\/");

            var stime = new Date(sdate).getTime();
            var etime = new Date(edate).getTime();
            if (stime == etime) {
                return 0;
            }
            else if (stime > etime) {
                return -1;
            }
            else {
                return 1;
            }
        }
        function openAjaxBrandSearchAjaxPopup() {
            $.openPopupLayer({
                name: "BrandSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/Base/SelectBrand.aspx?BrandIDs=" + $('#hidSearchBrand').val(),
                beforeClose: function (b, cData) {
                    if (b) {
                        $("input[id$='hidSearchBrand']").val(cData.BrandIDs);
                        //$('#txtSearchBrandIDS').val(cData.BrandNames);
                        $("input[name$='txtSearchBrandIDS']").val(cData.BrandNames);
                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }
        //弹出集采项目名
        function openAjaxProjectNameSearchAjaxPopup() {
            $.openPopupLayer({
                name: "ProjectNameSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/Base/SelectProjectName.aspx",
                beforeClose: function (b, cData) {
                    var ProjectName = $('#popupLayer_' + 'ProjectNameSelectAjaxPopup').data('ProjectName');
                    //$('#txtSearchProjectName').val(ProjectName);
                    $("input[id$='txtSearchProjectName']").val(ProjectName);
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }
        function AddLog() {
            $.openPopupLayer({
                name: "ReturnVisitAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ReturnVisit/AddReturnVisit.aspx",
                beforeClose: function (b, cData) {

                },
                afterClose: function () {

                }
            });
        }

        //批量分配任务
        function AssignCheck() {
            var CustIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return "'" + $(this).val() + "'"; }).get().join(",");
            AssignTaskNew(CustIDs);
        }

        function RealAssignTask(CustIDs) {
            var userid = $("#hidSelectUserid").val();

            //分配任务
            $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "AssignTask", CustIDs: encodeURIComponent(CustIDs), AssignUserID: userid }, function (data) {
                if (data == "success") {
                    $.jPopMsgLayer("分配客户成功", function () {
                        search(0);
                    });
                }
                else {
                    alert(data);
                }
            });
        }

        //单个分配任务
        function AssignTask(CustIDs) {
            if (Len(CustIDs) > 0) {
                $.openPopupLayer({
                    name: "AssignTaskAjaxPopupForSelect",
                    url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                    success: function () {
                        //分配任务，隐藏控件
                        $("#popAClear").hide();
                    },
                    beforeClose: function (e) {
                        if (e) {
                            $("#hidSelectUserid").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                            RealAssignTask(CustIDs);
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(search);
                    }
                });
            }
            else {
                $.jAlert("请至少选择一个要分配的客户！");
            }
        }

        //单个分配任务
        function AssignTaskNewCust(CustIDs) {            
            var userid = $("#hidSelectUserid").val();
            //分配任务
            $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "AssignTaskNew", CustIDs: encodeURIComponent(CustIDs), AssignUserID: userid }, function (data) {
                if (data == "success") {
                    $.jPopMsgLayer("分配客户成功", function () {
                        search(0);
                    });
                }
                else {
                    alert(data);
                }
            });
        }

        //单个分配任务
        function AssignTaskNew(CustIDs) {
            if (Len(CustIDs) > 0) {
                $.openPopupLayer({
                    name: "AssignTaskAjaxPopupForSelect",
                    url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                    success: function () {
                        //分配任务，隐藏控件
                        $("#popAClear").hide();
                    },
                    beforeClose: function (e) {
                        if (e) {
                            $("#hidSelectUserid").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                            var userid = $("#hidSelectUserid").val();
                            //分配任务
                            $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "isYYKF", AssignUserID: userid }, function (data) {
                                if (data == "success") {
                                    //是“运营客服”
                                    IsExistsCust(CustIDs);
                                }
                                else {
                                    //不是“运营客服”
                                    CustIDs = CustIDs.replace(new RegExp("'", "gm"), "");
                                    RealAssignTask(CustIDs);
                                }
                            });
                        }
                    },
                    afterClose: function () {
                        //敲回车键执行方法
                        enterSearch(search);
                    }
                });
            }
            else {
                $.jAlert("请至少选择一个要分配的客户！");
            }
        }

        //
        function IsExistsCust(CustIDs) {
            //分配任务
            var userid = $("#hidSelectUserid").val();
            $.post("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "IsExistsCust", CustIDs: encodeURIComponent(CustIDs), AssignUserID: userid }, function (data) {
                if (data == "existsCust") {
                    $.jConfirm("您所选择的客户已存在负责坐席，您确定更换所选客户的负责坐席吗？", function (r) {

                        if (r) {
                            AssignTaskNewCust(CustIDs);
                        }
                    });
                }
                else {
                    AssignTaskNewCust(CustIDs);
                }
            });
        }
        //回收任务
        function RecedeCrmTask() {
            var CustIDs = $(":checkbox[name='chkSelect'][checked=true]").map(function () { return $(this).val(); }).get().join(",");

            var custObj = $(":checkbox[name='chkSelect'][checked=true]");


            //if (CustIDs.length == 0)
            if (custObj.size() == 0) {
                $.jAlert("请至少选择一个要分配的客户！");
                return;
            }
            else if (custObj.size() == 1) {//一个客户
                //判断是否只有一个负责员工
                var users = "";
                users = $(":checkbox[name='chkSelect'][checked=true]").parent().siblings("[title]").attr("title");
                if (users == undefined) {
                    //客户没有负责员工
                    $.jAlert("该客户还未分配，请重新选择！");
                    return;
                }
                else {
                    var index = users.indexOf(",");
                    if (index == -1) {
                        //只有一个负责员工
                        RecedeCrmOne(CustIDs)
                    }
                    else {
                        openSelectEmployeePopup(CustIDs);
                    }
                }
            }
            else {//多个客户
                openSelectEmployeePopup(CustIDs);
            }
        }

        function openSelectEmployeePopup(CustIDs) {//选择回收负责员工弹出层
            $.openPopupLayer({
                name: "EmployeeSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ReturnVisit/SelectEmployeePop.aspx?CustIDs=" + encodeURIComponent(CustIDs),
                beforeClose: function (e, data) {
                    if (e) {
                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        ///回收一个任务
        function RecedeCrmOne(CustIDs) {

            if (Len(CustIDs) > 0) {

                $.jConfirm("确定要回收所选择的的客户吗？", function (r) {

                    if (r) {
                        //回收任务
                        AjaxPostAsync("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "RecedeTaskOne", CustIDs: encodeURIComponent(CustIDs) }, null, function (data) {
                            if (data == "success") {
                                ClearNextCallTimeOne(CustIDs);
                            }
                            else {
                                alert(data);
                            }
                        });
                    }
                });

            }
            else {
                $.jAlert("请至少选择一个要回收的客户！");
            }
        }

        //清除单个客户下次回访日期
        function ClearNextCallTimeOne(CustIDs) {
            AjaxPostAsync("/AjaxServers/ReturnVisit/AssignTask.ashx", { Action: "ClearNextCallTime", CustIDs: encodeURIComponent(CustIDs) }, null, function (data) {
                if (data == "success") {
                    window.location.reload();
                }
                else {
                    $.jAlert(data, function () {
                        window.location.reload();
                    });
                }
            });
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

        //
        function emChkIsChooseTN(othis) {
            var $checkbox = $(othis).prev();
            if ($checkbox.is(":checked")) {
                $checkbox.removeAttr("checked");
            }
            else {
                $checkbox.attr("checked", "checked");
                //清除文本框内容
                $("[id$='txtSearchTrueName']").val("");
                $("#txtSearchTrueNameID").val("");
            }
        }
        function tnValueRemove(othis) {
            var $checkbox = $(othis);
            if ($checkbox.is(":checked")) {
                //清除文本框内容
                $("[id$='txtSearchTrueName']").val("");
                $("#txtSearchTrueNameID").val("");
            }
        }

        //点击文字，选中单选框
        function emRadioIsChoose(othis) {
            var $radio = $(othis).prev();
            if ($radio.is(":checked")) {
                $radio.removeAttr("checked");
            }
            else {
                $radio.attr("checked", "checked");
            }
        }

        //选择访问人
        function SelectVisitPerson() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='txtSearchTrueName']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#txtSearchTrueNameID").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));

                    $("#responseCk").removeAttr("checked");
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        //批量分配  导入
        function openUploadExcelInfoAjaxPopupForImportID() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "ReturnVisitAssignImport/Main.aspx"
            });
        }  

        function ShowDataByPost1(pody) {
            LoadingAnimation("divList");
            $('#divList').load('/AjaxServers/ReturnVisit/ReturnVisitCustList.aspx #divList > *', pody, LoadDivSuccess);
        }
        //查询之后，回调函数
        function LoadDivSuccess(data) {
            $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
            SetTableStyle('tableReturnVisitCust');
            var nowDate = $("#nowDt").val();
            $("#tableReturnVisitCust .backDt").each(function () {
                if ($(this).html() != "") {
                    var compVal = getddToDateTime(nowDate, $.trim($(this).text()));
                    switch (compVal) {
                        case 1: $(this).parent().find(" td:eq(1)").removeClass("alarmtr"); break;
                        case 0: $(this).parent().find(" td:eq(1)").removeClass("alarmtr").addClass("alarmtr"); break;
                        case -1: $(this).parent().find(" td:eq(1)").removeClass("alarmtr").addClass("alarmtr"); break;
                        default: break;
                    }
                }
                else {
                    $(this).parent().find(" td:eq(1)").removeClass("alarmtr");
                }
            });

        }
        function searchUp() {
            isclickSearch = true;
            _tagid = 0;
            search(0);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="min-width: 1020px; margin: 0px; padding: 0px;">
        <div class="search clearfix">
            <!--主体内容部分star-->
            <ul class="clear" id="ulNormal">
                <li>
                    <label>
                        客户名称：</label><input type="text" maxlength="50" class="w200" name="txtSearchCustName"
                            id="txtSearchCustName" runat="server" />
                </li>
                <li>
                    <label>
                        客户类型：</label>
                    <select id="selCustType" class="w200" style="width: 206px;">
                        <option value="-2" selected="selected">请选择</option>
                        <option value="20001">厂商</option>
                        <option value="20002">集团</option>
                        <option value="20003">4s</option>
                        <option value="20004">特许经销商</option>
                        <option value="20005">综合店</option>
                        <option value="20007">汽车服务商</option>
                        <option value="20011">经纪公司</option>
                        <option value="20010">个人</option>
                        <option value="20012">交易市场</option>
                        <option value="20006">其它</option>
                        <option value="20013">车易达</option>
                        <option value="20014">二手车中心</option>
                    </select>
                </li>
                <li>
                    <label>
                        客户地区：</label>
                    <select id="ddlSearchProvince" style="width: 81px" name="ddlSearchProvince" class="kProvince"
                        runat="server">
                    </select>
                    <select id="ddlSearchCity" onchange="javascript:BindCounty('<%=ddlSearchProvince.ClientID%>','<%=ddlSearchCity.ClientID%>','<%=ddlSearchCounty.ClientID%>');"
                        style="width: 81px" name="ddlSearchCity" runat="server">
                    </select>
                    <select id="ddlSearchCounty" name="ddlSearchCounty" class="kArea" style="width: 81px;"
                        runat="server">
                    </select>
                </li>
            </ul>
            <ul class="clear">
                <li>
                    <label>
                        主营品牌：</label><input type="text" class="w200" name="txtSearchBrandIDS" id="txtSearchBrandIDS"
                            runat="server" readonly="readonly" onclick="openAjaxBrandSearchAjaxPopup()" /><input
                                id="hidSearchBrand" runat="server" name="hidSearchBrand" type="hidden" />
                </li>
                <li style="vertical-align: middle; display: block; width: 296px;">
                    <label>
                        负责坐席：</label>
                    <input type="text" name="txtSearchTrueName" class="w125" style="width: 134px;" maxlength="20"
                        onclick="SelectVisitPerson()" id="txtSearchTrueName" runat="server" />
                    <input type="hidden" id="txtSearchTrueNameID" />
                    <span style="width: 50px; text-align: left;">
                        <input type="checkbox" id="responseCk" onclick="tnValueRemove(this)" value="1" /><em
                            onclick="emChkIsChooseTN(this)">无坐席</em> </span></li>
                <li>
                    <label>
                        经营范围：</label>
                    <span style="width: 75px; text-align: left;">
                        <input id="chkTypeNew" name='carType' type="checkbox" value="1" /><em onclick="emChkIsChoose(this)">新车</em></span>
                    <span style="width: 110px; text-align: left;">
                        <input id="chkTypeNewSnd" type="checkbox" name='carType' value="3" /><em onclick="emChkIsChoose(this)">新车/二手车</em></span>
                    <span style="width: 85px; text-align: left;">
                        <input id="chkTypeSnd" type="checkbox" name='carType' value="2" /><em onclick="emChkIsChoose(this)">二手车</em></span>
                </li>
            </ul>
            <ul class="clear">
                <li>
                    <label>
                        集采项目：</label><input type="text" name="txtSearchProjectName" id="txtSearchProjectName"
                            runat="server" class="w200" onclick="openAjaxProjectNameSearchAjaxPopup()" />
                </li>
                <li style="width: 390px;">
                    <label>
                        联系日期：</label>
                    <input type="text" class="w95" name="txtApplyStartTime" id="txtApplyStartTime" />-<input
                        type="text" class="w95" name="txtApplyEndTime" id="txtApplyEndTime" />
                    <input type="checkbox" value="1" id="radioNOType" style='border: none; width: auto;' /><em
                        onclick="emChkIsChoose(this)">未联系</em> </li>
                <li class="btnsearch">
                    <input name="" type="button" value="查 询" onclick="javascript:searchUp()" />
                </li>
            </ul>
        </div>
        <input type="hidden" id="hidSelectUserid" /><input type="hidden" id="hidSelectPageSize"
            value="" />
        <div class="optionBtn clearfix">
            <%if (right_btn2)
              { %>
            <input type="button" id="btnAdd" value="回 收" class="newBtn" onclick="RecedeCrmTask()" />
            <%} %>
            <%if (right_btn1)
              { %>
            <input type="button" id="btnCategory" value="分 配" class="newBtn" onclick="AssignCheck()" />
            <%} %>
                <%if (right_btn3)
              { %>
            <input type="button" id="Button1" value="批量分配" class="newBtn" onclick="openUploadExcelInfoAjaxPopupForImportID()" />
            <%} %>
        </div>
        <div id="divContent">
        </div>
    </div>
</asp:Content>
