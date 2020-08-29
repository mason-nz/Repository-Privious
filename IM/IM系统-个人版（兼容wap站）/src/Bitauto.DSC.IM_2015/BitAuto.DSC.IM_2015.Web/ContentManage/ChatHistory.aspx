<%@ Page Title="聊天记录" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ChatHistory.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.ContentManage.ChatHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #ajaxTable tr
        {
            cursor: pointer;
        }
        .highlight
        {
            background-color: yellow;
        }
        .highlightselectedtr
        {
            background-color: #c4e8f5;
        }
        
        .trover
        {
            background: #f9f9f9;
        }
        .trclick
        {
            background: #c4e8f5;
        }
    </style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/Enum/Area.js" type="text/javascript"></script>
    <script src="../Scripts/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindBeginEndtime() {
            $("#txtStartTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd 00:00:00") %>');
            $("#txtEndTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd 23:59:59") %>');
        }
        $(function () {
            //BindSelDistrictData();
            //            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); }, dateFmt: 'yyyy-MM-dd HH:mm:ss' }); });
            //            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', dateFmt: 'yyyy-MM-dd HH:mm:ss' }); });

            $('#txtStartTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: false, startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: false, startDate: '%y-%M-%d 23:59:59', dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });

            if ($("#txtStartTime").val() == "" && $("#txtEndTime").val() == "") {
                BindBeginEndtime();
            }

            //            BindProvince('ddlSearchVisitorProvince'); //绑定省份
            //            $("[id$=ddlSearchVisitorProvince]").change(function () {
            //                BindCity('ddlSearchVisitorProvince', 'ddlSearchVisitorCity');
            //            });
            BindTag("sltWTag1", 1);
            BindTag("sltWTag2", 2);

            $("#ajaxTable tr").live("click", function () {
                $("#ajaxTable tr:gt(0)").removeClass("trclick");
                $(this).addClass("trclick");

                var objs = $(this).find(" .hidtdinfo").text();
                if (objs != "") {
                    var objssplits = objs.split(",");
                    for (var i = 0; i < objssplits.length; i++) {
                        if (i == 0) {
                            $("#hidcsid").val(objssplits[i]);
                        }
                        if (i == 1) {
                            $("#hidcustid").val(objssplits[i]);
                        }
                        else if (i == 2) {
                            $("#hidvisitid").val(objssplits[i]);
                        }
                        else if (i == 3) {
                            $("#hidorderid").val(objssplits[i]);
                        }
                    }
                    setTab('one', 1, 3);
                }
            });

        });
        //tab切换方法
        function setTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
            //  alert($("#hidvisitid").val());
            //1：对话记录;2:  访客信息;3:服务信息
            if (cursel == 1) {
                searchHistory();
            }
            else if (cursel == 2) {
                $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getmemberinfobyvisitid', VisitID: encodeURIComponent($.trim($("#hidvisitid").val())), CSID: encodeURIComponent($.trim($("#hidcsid").val())) }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            $.each(jsonData.root, function (idx, item) {

                                $("#tab32_VisitorName").text(item.VisitorName);
                                $("#tab32_VisitorType").text(item.VisitorType);
                                $("#tab32_Sex").text(item.sex);
                                $("#tab32_ProvinceCity").text(item.ProvinceCityName);
                                $("#tab32_Phone").text(item.phone);

                                $("#tab32_SourceType").text(item.SourceType);
                                var $tab32_ReferInfo = $("#tab32_ReferInfo");
                                $tab32_ReferInfo.text(item.UserReferTitle);
                                $tab32_ReferInfo.attr("href", item.UserReferURL);
                                $("#tab32_ConversationCreateTime").text(item.ConversationStartTime);
                                $("#tab32_ClientStartTime").text(item.AgentStartTime);
                                $("#tab32_EndTime").text(item.EndTime);

                                $("#tab32_Duration").text(item.Duration);
                                $("#tab32_AgentName").text(item.AgentName + " [ 工号：" + item.AgentNum + " ]");
                                $("#tab32_PerSatisfaction").text(item.PerSatisfaction);
                                $("#tab32_ProSatisfaction").text(item.ProSatisfaction);
                                if (item.SatisfactionContents.length > 30) {
                                    $("#tab32_SatisfactionContents").attr("title", item.SatisfactionContents);
                                    $("#tab32_SatisfactionContents").text(item.SatisfactionContents.substr(0, 30) + "……");
                                } else {
                                    $("#tab32_SatisfactionContents").text(item.SatisfactionContents);
                                }
                            });
                        }
                        else {
                            $("#tab32_VisitorName").text("");
                            $("#tab32_VisitorType").text("");
                            $("#tab32_Sex").text("");
                            $("#tab32_ProvinceCity").text("");
                            $("#tab32_Phone").text("");

                            $("#tab32_SourceType").text("");
                            $("#tab32_ReferInfo").text("");
                            $("#tab32_ConversationCreateTime").text("");
                            $("#tab32_ClientStartTime").text("");
                            $("#tab32_EndTime").text("");

                            $("#tab32_Duration").text("");
                            $("#tab32_AgentName").text("");
                            $("#tab32_PerSatisfaction").text("");
                            $("#tab32_ProSatisfaction").text("");
                            $("#tab32_SatisfactionContents").text("");
                        }
                    }
                    else {
                        $("#tab32_VisitorName").text("");
                        $("#tab32_VisitorType").text("");
                        $("#tab32_Sex").text("");
                        $("#tab32_ProvinceCity").text("");
                        $("#tab32_Phone").text("");

                        $("#tab32_SourceType").text("");
                        $("#tab32_ReferInfo").text("");
                        $("#tab32_ConversationCreateTime").text("");
                        $("#tab32_ClientStartTime").text("");
                        $("#tab32_EndTime").text("");

                        $("#tab32_Duration").text("");
                        $("#tab32_AgentName").text("");
                        $("#tab32_PerSatisfaction").text("");
                        $("#tab32_ProSatisfaction").text("");
                        $("#tab32_SatisfactionContents").text("");
                    }
                });
            }
            else if (cursel == 3) {
                searchServiceInfo();
                //                ; $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getorderinfobyorderid', OrderID: encodeURIComponent($.trim($("#hidorderid").val())) }, function (data) {
                //                    if (data != "") {
                //                        var jsonData = $.evalJSON(data);
                //                        if (jsonData != "") {
                //                            $.each(jsonData.root, function (idx, item) {
                //                                $("#tab44_OrderID").text(item.OrderID);
                //                                if (item.OrderID != "") {
                //                                    $("#tab44_OrderID").attr({ "href": "<%=WorkOrderViewUrl%>" + "?OrderID=" + item.OrderID, "target": "_blank" });
                //                                }
                //                                $("#tab44_Title").text(item.Title);
                //                                $("#tab44_CreateTime").text(item.CreateTime);
                //                                $("#tab44_TrueName").text(item.TrueName);
                //                                $("#tab44_Status").text(item.Status);
                //                            });
                //                        }
                //                        else {
                //                            $("#tab44_OrderID").text("");
                //                            $("#tab44_Title").text("");
                //                            $("#tab44_CreateTime").text("");
                //                            $("#tab44_TrueName").text("");
                //                            $("#tab44_Status").text("");
                //                        }
                //                    }
                //                    else {
                //                        $("#tab44_OrderID").text("");
                //                        $("#tab44_Title").text("");
                //                        $("#tab44_CreateTime").text("");
                //                        $("#tab44_TrueName").text("");
                //                        $("#tab44_Status").text("");
                //                    }
                //                });
            }

        };
        //获取大区数据
        function BindSelDistrictData() {
            $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getdistrictdata', r: Math.random() }, function (data) {
                $("#selDistrict").html("");
                $("#selDistrict").append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selDistrict").append("<option value='" + item.value + "'>" + item.name + "</option>");
                        });

                        $("#ajaxTable tr:eq(0)").click();
                    }
                }
            });
        }
        //根据大区获取城市群数据
        function BindSelectChange() {
            //            var pid = $("#selDistrict").val();
            //            $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getcitydata', District: pid, r: Math.random() }, function (data) {
            //                $("#selCity").html("");
            //                $("#selCity").append("<option value='-1'>请选择</option>");
            //                if (data != "") {
            //                    var jsonData = $.evalJSON(data);
            //                    if (jsonData != "") {
            //                        $.each(jsonData.root, function (idx, item) {
            //                            $("#selCity").append("<option value='" + item.value + "'>" + item.name + "</option>");
            //                        });
            //                    }
            //                }
            //            });
        }
        //根据条件获取会话数据
        $(window).load(function () {
            search();
        });
        //查询
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxTable");
                $('#ajaxTable').load("/AjaxServers/ContentManage/CSData.aspx", podyStr, function () {
                    if ($("#ajaxTable tr").length == 1) {
                        $("#hidcsid").val("-1");
                        $("#hidvisitid").val("-1");
                        $("#hidorderid").val("-1");
                        setTab('one', 1, 3);
                    }
                    else {
                        $("#ajaxTable tr:eq(1)").click();
                    }

                });
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtVisitorName = $.trim($("#txtVisitorName").val());
            if (txtVisitorName.length > 50) {
                msg += "访客姓名字数太多<br/>";
                $("#txtVisitorName").val('');
            }
            var txtPhone = $.trim($("#txtPhone").val());
            if (isNaN(txtPhone)) {
                isTrue = false;
                msg += "电话应该为数字！<br/>";
            }
            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 20) {
                msg += "客服名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var reg = new RegExp(/^[0-9a-zA-Z_]{1,50}$/);
            var txtOrderID = $.trim($("#txtOrderID").val());
            if (txtOrderID != "" && !reg.test(txtOrderID)) {
                isTrue = false;
                msg += "工单ID只能输入数字、字母,且不能超过50个字符！<br/>";
            }

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime == "" || endTime == "") {
                msg += "最后对话时间的开始时间和结束时间都不能为空<br/>";
            }
            else if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "最后对话时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
                else {
                    var newEndMonth = new Date(endTime.replace(/-/g, "/")).getMonth() + 1;
                    var newBeginMonth = new Date(beginTime.replace(/-/g, "/")).getMonth() + 1;
                    if (newEndMonth != newBeginMonth) {
                        msg += "最后对话时间不能跨月";
                    }
                }
            }
            else if (beginTime != "") {
                if (!beginTime.isDateTime()) {
                    msg += "最后对话时间(起始)格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }
            else if (endTime != "") {
                if (!endTime.isDateTime()) {
                    msg += "最后对话时间(终止)格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }
            //            if (beginTime != "") {
            //                if (!beginTime.isDateTime()) {
            //                    msg += "最后对话时间(起始)格式不正确<br/>";
            //                    $("#txtStartTime").val('');
            //                }
            //            }
            //            if (endTime != "") {
            //                if (!endTime.isDateTime()) {
            //                    msg += "最后对话时间(终止)格式不正确<br/>";
            //                    $("#txtEndTime").val('');
            //                }
            //            }
            //            if (beginTime != "" && endTime != "") {
            //                if (beginTime > endTime) {
            //                    msg += "最后对话时间中结束时间不能大于开始时间<br/>";
            //                    $("#txtStartTime").val('');
            //                    $("#txtEndTime").val('');
            //                }
            //            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtVisitorName = encodeURIComponent($.trim($('#txtVisitorName').val()));
            var txtPhone = encodeURIComponent($.trim($('#txtPhone').val()));
            // var selProvinceID = encodeURIComponent($.trim($('#ddlSearchVisitorProvince').val()));
            // var selCityID = encodeURIComponent($.trim($('#ddlSearchVisitorCity').val()));
            var tagbgid = $("#<%=sltBG.ClientID%>").val();
            var tagid = $("#sltWTag2").val();
            if (tagid == "-2") {
                tagid = $("#sltWTag1").val();
            }

            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var txtOrderID = encodeURIComponent($.trim($('#txtOrderID').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));

            var pody = {
                VisitorName: txtVisitorName,
                Phone: txtPhone,
                TagBGID: tagbgid,
                TagID: tagid,

                AgentName: txtAgentName,
                OrderID: txtOrderID,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,

                r: Math.random()  //随机数
            }

            return pody;
        }


        //查询对话记录
        function searchHistory() {

            var pody = _params_History();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxHistoryList");
            $('#ajaxHistoryList').load("/AjaxServers/ContentManage/CSHistoryData.aspx", podyStr);
        }
        function _params_History() {
            var txtCSID = $.trim($('#hidcsid').val());

            var pody = {
                CSID: encodeURIComponent(txtCSID),
                r: Math.random()  //随机数
            }
            return pody;
        }
        //分页操作 
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxHistoryList");
            $('#ajaxHistoryList').load("/AjaxServers/ContentManage/CSHistoryData.aspx", pody, function () { highlight(); });
        }

        //查询服务信息
        function searchServiceInfo() {

            var pody = _params_ServiceInfo();
            var podyStr = JsonObjToParStr(pody);
            LoadingAnimation("ajaxServiceInfo");
            $('#ajaxServiceInfo').load("/CustInfo/ServiceInfo.aspx", podyStr);
        }
        function _params_ServiceInfo() {
            var txtVisitID = $.trim($('#hidvisitid').val());
            var txtCustID = $.trim($('#hidcustid').val());
            var txtoOrderID = $.trim($('#hidorderid').val());

            var pody = {
                VisitID: txtVisitID,
                orderid: txtoOrderID,
                custid: txtCustID,
                r: Math.random()  //随机数
            }
            return pody;
        }

        function highlight() {
            clearSelection(); //先清空一下上次高亮显示的内容；
            var searchText = $.trim($('#txtKeyWord').val()); //获取你输入的关键字；
            if (searchText.length > 0) {
                var regExp = new RegExp(searchText, 'g'); //创建正则表达式，g表示全局的，如果不用g，则查找到第一个就不会继续向下查找了；

                $('#ajaxHistoryList .dhc').each(function ()//遍历文章；
                {
                    var html = $(this).html();
                    var newHtml = html.replace(regExp, "<span class='highlight'>" + searchText + '</span>'); //将找到的关键字替换，加上highlight属性；

                    $(this).html(newHtml); //更新文章；
                });
            }
            //  prekeyword  = searchText;
        }
        function clearSelection() {
            $('#ajaxHistoryList .dhc').each(function ()//遍历
            {
                $(this).find('.highlight').each(function ()//找到所有highlight属性的元素；
                {
                    var thishtml = $(this).html();
                    var ttt = $(this)[0].outerHTML;
                    $(this)[0].outerHTML = thishtml;
                    // $(this).replaceWith(thishtml); //将他们的属性去掉；
                });
            });
        }
        //导出数据
        function exportdata() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {
                var pody = _params();

                $("#formExport [name='ep_VisitorName']").val(pody.VisitorName);
                $("#formExport [name='ep_Phone']").val(pody.Phone);
                $("#formExport [name='ep_TagBGID']").val(pody.TagBGID);
                $("#formExport [name='ep_TagID']").val(pody.TagID);
                $("#formExport [name='ep_AgentName']").val(pody.AgentName);
                $("#formExport [name='ep_OrderID']").val(pody.OrderID);
                $("#formExport [name='ep_QueryStarttime']").val(pody.QueryStarttime);
                $("#formExport [name='ep_QueryEndTime']").val(pody.QueryEndTime);
                $("#formExport").submit();
            }
        }
        //绑定标签
        function BindTag(id, level) {
            $("#" + id + " option").remove();
            var pid = 0;
            var bgid = 0;

            bgid = $("#[id$='sltBG']").val();
            if (level == 1) {

            }
            else if (level == 2) {
                pid = $("#[id$='sltWTag1']").val();
            }

            var para;
            para = {
                Action: "getcontenttag", BGID: bgid, PID: pid, r: Math.random()
            };
            AjaxPostAsync("/AjaxServers/LayerDataHandler.ashx", para, null, function (data) {
                $("#" + id).append("<option value='-2'>请选择</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#" + id + "").append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
            if (level == 1) {
                $("#sltWTag1").trigger("change");
            }
        }

    </script>
    <input type="hidden" id="hidcsid" />
    <input type="hidden" id="hidcustid" />
    <input type="hidden" id="hidvisitid" />
    <input type="hidden" id="hidorderid" />
    <!--内容开始-->
    <div class="content ">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li style="width:230px">
                    <label>
                        访客姓名：</label><input name="" id="txtVisitorName" type="text" class="w240" style="width:120px" /></li>
                <li style="width:230px">
                    <label>
                        电话：</label>
                    <input name="" id="txtPhone" type="text" class="w240" style="width:120px"/></li>
                <li style="width:550px">
                    <label>
                        对话标签：</label>
                    <select id="sltBG" class="w240" onchange="BindTag('sltWTag1',1)" runat="server" style="width: 190px">
                    </select>
                    <select id="sltWTag1" class="w240" onchange="BindTag('sltWTag2',2)" style="width:150px">
                    </select>
                    <select id="sltWTag2" class="w240" style="width: 100px">
                    </select>
                </li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" id="bt_search" onclick="javascript:search()" value="查询" class="w60" /></div>
                </li>
            </ul>
            <ul style="clear: both;">
                <li style="width:230px">
                    <label>
                        所属客服：</label>
                    <input name="" id="txtAgentName" type="text" class="w240" style="width:120px"/>
                </li>
                <li style="width:230px">
                    <label>
                        工单ID：</label><input name="" id="txtOrderID" type="text" class="w240" style="width:120px"/></li>
                <li style="width:550px">
                    <label>
                        最后对话时间：</label>
                    <input name="txtStartTime" id="txtStartTime" type="text" class="w240" style="width: 211px;" />
                    -
                    <input name="txtEndTime" id="txtEndTime" type="text" class="w240" style="width: 211px;" />
                </li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" id="bt_Export" onclick="javascript:exportdata()" value="导出"
                            class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <!--列表开始-->
        <div class="cxList2 online_dh" id="ajaxTable" style="margin-left: 1px; margin-top: 8px;
            width: 695px; height: 421px; border-left: 2px solid #E6E6E6; border-bottom: 1px solid #E6E6E6;
            border-top: 1px solid #E6E6E6">
        </div>
        <!--列表结束-->
        <!--信息展示-->
        <div class="kh_info" style="width: 520px;">
            <div id="Tab1">
                <div class="Menubox">
                    <ul>
                        <li id="one1" onclick="setTab('one',1,3)" class="hover">对话记录</li>
                        <li id="one2" onclick="setTab('one',2,3)">访客信息</li>
                        <li id="one3" onclick="setTab('one',3,3)">服务信息</li>
                    </ul>
                </div>
                <div class="Contentbox" style="height: 365px;">
                    <!--对话记录-->
                    <div id="con_one_1" style="display: none">
                        <div class="dialogue">
                            <ul class="search">
                                <li>
                                    <label>
                                        对话查询：</label><input type="text" id="txtKeyWord" value="" class="w240" /></li>
                                <li class="btn" style="width: 80px;">
                                    <input type="button" value="查询" onclick="highlight()" class="w60" /></li>
                            </ul>
                            <div class="clearfix">
                            </div>
                            <div class="" id="ajaxHistoryList" style="height: 305px; overflow-y: scroll; -ms-overflow-y: scroll;
                                overflow-x: hidden;">
                            </div>
                        </div>
                    </div>
                    <!--对话记录-->
                    <!--访客信息-->
                    <div id="con_one_2" class="hover" style="height: 365px; overflow-y: scroll; -ms-overflow-y: scroll;
                        overflow-x: hidden;">
                        <table border="0" cellspacing="0" cellpadding="0" class="cusInfo">
                            <tr>
                                <th width="25%">
                                    访客姓名：
                                </th>
                                <td width="70%" id="tab32_VisitorName">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    客户分类：
                                </th>
                                <td id="tab32_VisitorType">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    性别：
                                </th>
                                <td id="tab32_Sex">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    地区：
                                </th>
                                <td id="tab32_ProvinceCity">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    电话：
                                </th>
                                <td id="tab32_Phone">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    访客来源：
                                </th>
                                <td id="tab32_SourceType">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    发起页面：
                                </th>
                                <td>
                                    <a href="#" id="tab32_ReferInfo" target="_blank">&nbsp;</a>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对话开始时间：
                                </th>
                                <td id="tab32_ConversationCreateTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    应答时间：
                                </th>
                                <td id="tab32_ClientStartTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    结束时间：
                                </th>
                                <td id="tab32_EndTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对话时长：
                                </th>
                                <td id="tab32_Duration">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    客服：
                                </th>
                                <td id="tab32_AgentName">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对客服评价：
                                </th>
                                <td id="tab32_PerSatisfaction">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对产品评价：
                                </th>
                                <td id="tab32_ProSatisfaction">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    满意度评价留言：
                                </th>
                                <td id="tab32_SatisfactionContents">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--访客信息-->
                    <!--服务信息-->
                    <div id="con_one_3">
                        <div class="dialogue">
                            <div class="" id="ajaxServiceInfo" style="height: 365px; overflow-y: scroll; -ms-overflow-y: scroll;
                                overflow-x: hidden;">
                            </div>
                        </div>
                    </div>
                    <!--服务信息-->
                </div>
            </div>
        </div>
        <!--信息展示-->
        <div class="clearfix">
        </div>
    </div>
    <form id="formExport" action="/AjaxServers/ContentManage/ChatHistoryExport.aspx"
    method="post">
    <input type="hidden" name="ep_VisitorName" value="" />
    <input type="hidden" name="ep_Phone" value="" />
    <input type="hidden" name="ep_TagBGID" value="" />
    <input type="hidden" name="ep_TagID" value="" />
    <input type="hidden" name="ep_AgentName" value="" />
    <input type="hidden" name="ep_OrderID" value="" />
    <input type="hidden" name="ep_QueryStarttime" value="" />
    <input type="hidden" name="ep_QueryEndTime" value="" />
    </form>
</asp:Content>
