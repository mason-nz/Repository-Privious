<%@ Page Title="聊天记录" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ChatHistory.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.ContentManage.ChatHistory" %>

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
    </style>
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            BindSelDistrictData();
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });

            $("#ajaxTable tr").live("click", function () {
                var objs = $(this).find(" .hidtdinfo").text();
                if (objs != "") {
                    var objssplits = objs.split(",");
                    for (var i = 0; i < objssplits.length; i++) {
                        if (i == 0) {
                            $("#hidcsid").val(objssplits[i]);
                        }
                        else if (i == 2) {
                            $("#hidvisitid").val(objssplits[i]);
                        }
                        else if (i == 3) {
                            $("#hidorderid").val(objssplits[i]);
                        }
                    }
                    setTab('one', 1, 4);
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
            //1：客户信息;2:对话记录;3:详细信息;4:工单记录
            if (cursel == 1) {
                $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getmemberinfobyvisitid', VisitID: encodeURIComponent($.trim($("#hidvisitid").val())) }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            $.each(jsonData.root, function (idx, item) {
                                $("#tab41_MemberName").text(item.MemberName);
                                var $loginid = $("#tab41_LoginID");
                                $loginid.text(item.MemberCode);
                                $loginid.attr("href", "<%=CrmMemberDetail%>" + "?i=0&MemberCode=" + item.MemberCode);
                                $("#tab41_District").text(item.DistrictName);
                                //$("#tab41_CityGroup").text(item.CityGroupName);
                                $("#tab41_Name").text(item.ContractName);
                                $("#tab41_WorkLevel").text(item.ContractJob);
                                $("#tab41_Address").text(item.Address);
                                $("#tab41_Email").text(item.ContractEmail);
                                $("#tab41_PhoneNum").text(item.ContractPhone);
                            });
                        }
                        else {
                            $("#tab41_MemberName").text("");
                            $("#tab41_LoginID").text("");
                            $("#tab41_District").text("");
                            //$("#tab41_CityGroup").text("");
                            $("#tab41_Name").text("");
                            $("#tab41_WorkLevel").text("");
                            $("#tab41_Address").text("");
                            $("#tab41_Email").text("");
                            $("#tab41_PhoneNum").text("");
                        }
                    }
                    else {
                        $("#tab41_MemberName").text("");
                        $("#tab41_LoginID").text("");
                        $("#tab41_District").text("");
                        //$("#tab41_CityGroup").text("");
                        $("#tab41_Name").text("");
                        $("#tab41_WorkLevel").text("");
                        $("#tab41_Address").text("");
                        $("#tab41_Email").text("");
                        $("#tab41_PhoneNum").text("");
                    }
                });
            }
            else if (cursel == 2) {
                searchHistory();
            }
            else if (cursel == 3) {
                $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getcsrelateinfobycsid', CSID: encodeURIComponent($.trim($("#hidcsid").val())) }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            $.each(jsonData.root, function (idx, item) {
                                $("#tab43_CreateTime").text(item.CreateTime);
                                $("#tab43_AgentStartTime").text(item.AgentStartTime);
                                $("#tab43_EndTime").text(item.EndTime);
                                $("#tab43_Duration").text(item.Duration);
                                $("#tab43_AgentNum").text(item.AgentNum);
                                $("#tab43_PerSatisfaction").text(item.PerSatisfaction);
                                $("#tab43_ProSatisfaction").text(item.ProSatisfaction);
                                $("#tab43_Contents").text(item.Contents);
                                $("#tab43_UserReferTitle").text(item.UserReferTitle);
                            });
                        }
                        else {
                            $("#tab43_CreateTime").text("");
                            $("#tab43_AgentStartTime").text("");
                            $("#tab43_EndTime").text("");
                            $("#tab43_Duration").text("");
                            $("#tab43_AgentNum").text("");
                            $("#tab43_PerSatisfaction").text("");
                            $("#tab43_ProSatisfaction").text("");
                            $("#tab43_Contents").text("");
                            $("#tab43_UserReferTitle").text("");
                        }
                    }
                    else {
                        $("#tab43_CreateTime").text("");
                        $("#tab43_AgentStartTime").text("");
                        $("#tab43_EndTime").text("");
                        $("#tab43_Duration").text("");
                        $("#tab43_AgentNum").text("");
                        $("#tab43_PerSatisfaction").text("");
                        $("#tab43_ProSatisfaction").text("");
                        $("#tab43_Contents").text("");
                        $("#tab43_UserReferTitle").text("");
                    }
                });
            }
            else if (cursel == 4) {
                ; $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getorderinfobyorderid', OrderID: encodeURIComponent($.trim($("#hidorderid").val())) }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            $.each(jsonData.root, function (idx, item) {
                                $("#tab44_OrderID").text(item.OrderID);
                                if (item.OrderID != "") {
                                    if (item.OrderID.length != 17) {
                                        $("#tab44_OrderID").attr({ "href": "<%=WorkOrderViewUrl%>" + "?OrderID=" + item.OrderID, "target": "_blank" });
                                    }
                                    else {
                                        $("#tab44_OrderID").attr({ "href": "<%=WorkOrderViewUrlNew%>" + "?OrderID=" + item.OrderID, "target": "_blank" });
                                    }
                                }
                                $("#tab44_Title").text(item.Title);
                                $("#tab44_CreateTime").text(item.CreateTime);
                                $("#tab44_TrueName").text(item.TrueName);
                                $("#tab44_Status").text(item.Status);
                            });
                        }
                        else {
                            $("#tab44_OrderID").text("");
                            $("#tab44_Title").text("");
                            $("#tab44_CreateTime").text("");
                            $("#tab44_TrueName").text("");
                            $("#tab44_Status").text("");
                        }
                    }
                    else {
                        $("#tab44_OrderID").text("");
                        $("#tab44_Title").text("");
                        $("#tab44_CreateTime").text("");
                        $("#tab44_TrueName").text("");
                        $("#tab44_Status").text("");
                    }
                });
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
                    $("#ajaxTable tr:eq(1)").click();
                });
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtjxsName = $.trim($("#jxsName").val());
            if (txtjxsName.length > 50) {
                msg += "经销商名称字数太多<br/>";
                $("#jxsName").val('');
            }
            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 20) {
                msg += "客服名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "最后对话时间格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "最后对话时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "最后对话时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtJxsName = encodeURIComponent($.trim($('#txtJxsName').val()));
            var selDistict = encodeURIComponent($.trim($('#selDistrict').val()));
            var selCity = '';
            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));
            var txtOrderID = encodeURIComponent($.trim($('#txtOrderID').val()));

            var pody = {
                MemberName: txtJxsName,
                District: selDistict,
                CityGroup: selCity,
                UserName: txtAgentName,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                OrderID: txtOrderID,
                r: Math.random()  //随机数
            }

            return pody;
        }

        //分页操作 
        function ShowDataByPost2(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load("/AjaxServers/ContentManage/CSData.aspx", pody, function () {
                $("#ajaxTable tr:eq(1)").click();
            });
        }

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

        
    </script>
    <input type="hidden" id="hidcsid" />
    <input type="hidden" id="hidvisitid" />
    <input type="hidden" id="hidorderid" />
    <!--内容开始-->
    <div class="content ">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        经销商名称：</label><input name="" id="txtJxsName" type="text" class="w240" /></li>
                <li>
                    <label>
                        所属区域：</label><select class="w240" id="selDistrict" onchange="javascript:BindSelectChange()"><option
                            value='-1'>请选择</option>
                        </select></li>
                <li>
                    <label>
                        所属客服：</label><input name="" type="text" id="txtAgentName" class="w240" />
                </li>
                <li>
                    <label>
                        最后对话时间：</label><input name="txtStartTime" id="txtStartTime" type="text" class="w240"
                            style="width: 108px;" />
                    -
                    <input name="txtEndTime" id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li>
                    <label>
                        工单ID：</label><input name="" id="txtOrderID" type="text" class="w240" /></li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" id="bt_search" onclick="javascript:search()" value="查询" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <!--列表开始-->
        <div class="cxList online_dh" id="ajaxTable" style="margin-top: 8px;">
        </div>
        <!--列表结束-->
        <!--信息展示-->
        <div class="kh_info">
            <div id="Tab1">
                <div class="Menubox">
                    <ul>
                        <li id="one1" onclick="setTab('one',1,4)" class="hover">客户信息</li>
                        <li id="one2" onclick="setTab('one',2,4)">对话记录</li>
                        <li id="one3" onclick="setTab('one',3,4)">详细信息</li>
                        <li id="one4" onclick="setTab('one',4,4)">工单记录</li>
                    </ul>
                </div>
                <div class="Contentbox" style="height: 384px;">
                    <!--客户信息-->
                    <div id="con_one_1" class="hover">
                        <table border="0" cellspacing="0" cellpadding="0" class="cusInfo">
                            <tr>
                                <th width="20%">
                                    经销商名称：
                                </th>
                                <td width="70%" id="tab41_MemberName">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    易湃会员ID：
                                </th>
                                <td>
                                    <a href="#" id="tab41_LoginID" target="_blank">&nbsp;</a>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    所属区域：
                                </th>
                                <td id="tab41_District">
                                    &nbsp;
                                </td>
                            </tr>
                            <%-- <tr>
                                <th>
                                    所属城市群：
                                </th>
                                <td id="tab41_CityGroup">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <th>
                                    客户分类：
                                </th>
                                <td id="tab41_MemberClass">
                                    经销商
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    姓名：
                                </th>
                                <td id="tab41_Name">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    职务：
                                </th>
                                <td id="tab41_WorkLevel">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    电话：
                                </th>
                                <td id="tab41_PhoneNum">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    所在地区：
                                </th>
                                <td id="tab41_Address">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    联系人邮箱：
                                </th>
                                <td id="tab41_Email">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--客户信息-->
                    <!--对话记录-->
                    <div id="con_one_2" style="display: none">
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
                            <div class="" id="ajaxHistoryList" style="height: 300px; overflow: scroll; overflow-x: hidden;">
                            </div>
                        </div>
                    </div>
                    <!--对话记录-->
                    <!--详细信息-->
                    <div id="con_one_3" style="display: none">
                        <table border="0" cellspacing="0" cellpadding="0" class="cusInfo">
                            <tr>
                                <th width="20%">
                                    开始时间：
                                </th>
                                <td width="70%" id="tab43_CreateTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    应答于：
                                </th>
                                <td id="tab43_AgentStartTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    结束时间：
                                </th>
                                <td id="tab43_EndTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对话时长：
                                </th>
                                <td id="tab43_Duration">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    客服：
                                </th>
                                <td id="tab43_AgentNum">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对客服评价：
                                </th>
                                <td id="tab43_PerSatisfaction">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对产品评价：
                                </th>
                                <td id="tab43_ProSatisfaction">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    满意度评价留言：
                                </th>
                                <td id="tab43_Contents">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    对话类型：
                                </th>
                                <td>
                                    点击咨询图标
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    发起页面：
                                </th>
                                <td id="tab43_UserReferTitle">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--详细信息-->
                    <!--工单记录-->
                    <div id="con_one_4" style="display: none">
                        <table border="0" cellspacing="0" cellpadding="0" class="cusInfo">
                            <tr>
                                <th width="20%">
                                    工单ID：
                                </th>
                                <td width="70%">
                                    <a href="#" id="tab44_OrderID">&nbsp;</a>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    工单标题：
                                </th>
                                <td id="tab44_Title">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    提交日期：
                                </th>
                                <td id="tab44_CreateTime">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    提交人：
                                </th>
                                <td id="tab44_TrueName">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    状态：
                                </th>
                                <td id="tab44_Status">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!--工单记录-->
                </div>
            </div>
        </div>
        <!--信息展示-->
        <div class="clearfix">
        </div>
    </div>
</asp:Content>
