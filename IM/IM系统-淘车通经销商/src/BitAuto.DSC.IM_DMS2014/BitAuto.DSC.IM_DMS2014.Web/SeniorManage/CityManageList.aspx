<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CityManageList.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.CityManageList" MasterPageFile="~/Controls/Top.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>城市管理</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //自动查询
        $(document).ready(function () {
            SearchData();
        });
        //获取城市群
        function onDistrictChanged(sel) {
//            var sel_citygroup = document.getElementById("sel_citygroup");
//            sel_citygroup.options.length = 0;
//            sel_citygroup.options.add(new Option("请选择", "-1"));

//            if (sel.value != -1) {
//                var pody = {
//                    Action: 'citygroup',
//                    DistrictID: sel.value,
//                    r: Math.random
//                };
//                AjaxPostAsync("/AjaxServers/SeniorManage/CityManageHandler.ashx", pody, function () { }, function (data) {
//                    var jsonData = $.evalJSON(data);
//                    if (jsonData.result == "success") {
//                        $.each(jsonData.data, function (i, n) {
//                            sel_citygroup.options.add(new Option(n.CityGroupName, n.CityGroup));
//                        });
//                    }
//                    else {
//                        $.jAlert("查询城市群失败：" + jsonData.msg);
//                    }
//                });
//            }
        }
        //获取坐席
        function ShowMutilAgentChoose() {
            $.openPopupLayer({
                name: "ShowMutilAgentChoose",
                parameters: {
                    select: "radio", //multiple  //单选
                    citygroups: "",
                    chooseusers: "",
                    definetitle: encodeURIComponent("选择客服"),
                    r: Math.random()
                },
                url: "/Common/MutilAgentChoose.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        $("#txt_user_name").val(data.name);
                        $("#txt_user_id").val(data.userid);
                    }
                }
            });
        }
        //无坐席
        function ClearAgent(ckb) {
            $("#txt_user_name").val("");
            $("#txt_user_id").val("");
            if (ckb.checked) {
                $("#txt_user_name").attr("disabled", "true");
            }
            else {
                $("#txt_user_name").removeAttr("disabled");
            }
        }
        //查询
        function SearchData() {
            LoadingAnimation("dataList");
            var pody = {
                DistrictID: $("#sel_district").val(),
                
                UserID: $("#txt_user_id").val(),
                IsHave: $("#ckb_user")[0].checked,
                r: Math.random()
            };
            var podyStr = JsonObjToParStr(pody);
            $("#dataList").load("/AjaxServers/SeniorManage/CityManageList.aspx?r=" + Math.random(), podyStr);
        }
        //分页操作
        function ShowDataByPost10(podyStr) {
            LoadingAnimation("dataList");
            $("#dataList").load("/AjaxServers/SeniorManage/CityManageList.aspx?r=" + Math.random(), podyStr);
        }
        //分配
        function Assignation() {
            var citygroup_list_id = GetSelectCityGroup("分配");
            if (citygroup_list_id.length == 0) {
                return;
            }
            MutilSelectAgent("", "分配客服", function (data) {
                AccessAgent("assign", citygroup_list_id, data.userids);
            });
        }
        //回收
        function Recover() {
            var citygroup_list_id = GetSelectCityGroup("回收");
            if (citygroup_list_id.length == 0) {
                return;
            }
            MutilSelectAgent(citygroup_list_id, "回收客服", function (data) {
                AccessAgent("recover", citygroup_list_id, data.userids);
            });
        }
        //处理客服
        function AccessAgent(action, citygroups, userids) {
            var pody = {
                Action: action,
                CityGroups: citygroups,
                UserIds: userids,
                r: Math.random
            };
            AjaxPostAsync("/AjaxServers/SeniorManage/CityManageHandler.ashx", pody, function () { }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "success") {
                    SearchData();
                }
                else {
                    $.jAlert("处理客服失败：" + jsonData.msg);
                }
            });
        }
        //记录选择的城市群
        function GetSelectCityGroup(info) {
            var citygroup_list_id = "";
            var ckb_row = document.getElementsByName("ckb_row");
            for (var i = 0; i < ckb_row.length; i++) {
                if (ckb_row[i].checked) {
                    citygroup_list_id += ckb_row[i].value + ",";
                }
            }
            if (citygroup_list_id.length > 1) {
                citygroup_list_id = citygroup_list_id.substr(0, citygroup_list_id.length - 1);
            }
            if (citygroup_list_id.length == 0) {
                $.jAlert("请至少选择一个城市群进行" + info);
            }
            return citygroup_list_id;
        }
        //多选坐席
        function MutilSelectAgent(citygroup_list_id, title, callback) {
            $.openPopupLayer({
                name: "ShowMutilAgentChoose",
                parameters: {
                    select: "multiple", //radio //多选
                    citygroups: citygroup_list_id, //城市群下的坐席
                    chooseusers: "", //已选择的坐席（暂不实行 强斐 2014-11-3）
                    definetitle: encodeURIComponent(title),
                    r: Math.random()
                },
                url: "/Common/MutilAgentChoose.aspx",
                beforeClose: function (e, data) {
                    if (e && callback != null) {
                        callback(data);
                    }
                }
            });
        }
        //校验小时：分钟格式
        function IsHourAndMin(time) {
            var re = new RegExp("^([1]{0,1}[0-9]:[0-5]{1}[0-9]{1})$|^(2[0-3]:[0-5]{1}[0-9]{1})$");
            if (re.test(time)) {
                return true;
            } else {
                return false;
            }
        }
        //校验时间大小
        function CompareTime(st, et) {
            var time1 = SplitTime(st);
            var time2 = SplitTime(et);
            if (time1 == null) {
                $.jAlert("开始时间 [分钟:秒] 输入不正确", function () { $("#txt_time_st").focus(); });
                return false;
            }
            else if (time2 == null) {
                $.jAlert("结束时间 [分钟:秒] 输入不正确", function () { $("#txt_time_et").focus(); });
                return false;
            }
            else {
                if (time1.hour > time2.hour) {
                    $.jAlert("开始时间不能大于结束时间", function () { $("#txt_time_et").focus(); });
                    return false;
                }
                else if (time1.hour == time2.hour && time1.min > time2.min) {
                    $.jAlert("开始时间不能大于结束时间", function () { $("#txt_time_et").focus(); });
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        //分割小时和分钟
        function SplitTime(time) {
            var hour = time.split(':')[0];
            var min = time.split(':')[1];
            hour = parseInt(hour);
            min = parseInt(min);
            if (isNaN(hour) || isNaN(min)) {
                return null;
            }
            else {
                return { hour: hour, min: min }
            }
        }
        //输入校验
        function CheckTime(txt) {

        }
        //保存时间
        function SaveTime() {
            var st = $.trim($("#txt_time_st").val());
            var et = $.trim($("#txt_time_et").val());
            if (CompareTime(st, et)) {
                var pody = {
                    Action: "savetime",
                    StartTime: st,
                    EndTime: et,
                    r: Math.random
                };
                AjaxPostAsync("/AjaxServers/SeniorManage/CityManageHandler.ashx", pody, function () { }, function (data) {
                    var jsonData = $.evalJSON(data);
                    if (jsonData.result == "success") {
                        $.jAlert("保存成功");
                    }
                    else {
                        $.jAlert("保存失败：" + jsonData.msg);
                    }
                });
            }
        }
    </script>
    <!--查询开始-->
    <div class="searchTj">
        <ul>
            <li>
                <label>
                    所属大区：
                </label>
                <select class="w240 w200" id="sel_district" onchange="onDistrictChanged(this)">
                    <option value="-1">请选择</option>
                    <asp:Repeater ID="rpt_district" runat="server">
                        <ItemTemplate>
                            <option value="<%#Eval("District") %>">
                                <%#Eval("DistrictName")%>
                            </option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </li>
<%--            <li>
                <label>
                    城市群：
                </label>
                <select class="w240 w200" id="sel_citygroup">
                    <option value="-1">请选择</option>
                </select>
            </li>--%>
            <li class="w380">
                <label>
                    所属客服：
                </label>
                <input type="text" class="w240 w200" id="txt_user_name" readonly="true" onclick="ShowMutilAgentChoose()" />
                <input type="hidden" id="txt_user_id" value="" />
                <span>
                    <label>
                        <input type="checkbox" value="" id="ckb_user" onchange="ClearAgent(this)" />
                        <em>无客服</em>
                    </label>
                </span></li>
            <li style="width: 60px;">
                <div class="tjBtn">
                    <input type="button" value="查询" class="w60" onclick="SearchData()" /></div>
            </li>
        </ul>
        <div class="clearfix">
        </div>
    </div>
    <!--查询结束-->
    <div class="dc">
    </div>
    <!--列表开始-->
    <div class="cxList online_dh" style="margin-top: 8px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <thead>
                <th colspan="7" style="background: #F9F9F9">
                    <div class="btn btn2 right">
                        <input type="button" value="分配" class="save w60 gray" onclick="Assignation()" />
                        <input type="button" value="回收" class="cancel w60 gray" onclick="Recover()" />
                    </div>
                </th>
            </thead>
        </table>
        <div id="dataList" class="cxList online_dh" style="margin-top: -1px; margin-bottom: 0px;">
            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th width="10%">
                        <input name="" type="checkbox" value="" />
                    </th>
                    <th width="30%">
                        所属大区
                    </th>
                    <th width="30%">
                        城市群
                    </th>
                    <th width="25%">
                        所属客服
                    </th>
                </tr>
            </table>
        </div>
    </div>
    <!--列表结束-->
    <!--信息展示-->
    <div class="kh_info">
        <div id="Tab1">
            <div class="Menubox">
                <ul>
                    <li id="one1" class="hover" style="margin-top: -1px; padding-top: 1px;">服务时间</li>
                </ul>
            </div>
            <div class="Contentbox Contentbox_time" style="min-height: 410px;">
                <!--客户信息-->
                <div id="con_one_1" class="hover">
                    <div class="fw_time">
                        <label>
                            服务时间设定：
                        </label>
                        <span>
                            <input type="text" value="<%=st.ToString() %>" id="txt_time_st" last_value="<%=st.ToString() %>"
                                onfocus="CheckTime(this)" onblur="CheckTime(this)" />
                            至
                            <input type="text" value="<%=et.ToString() %>" id="txt_time_et" last_value="<%=et.ToString() %>"
                                onfocus="CheckTime(this)" onblur="CheckTime(this)" />
                        </span><span class="btn">
                            <input type="button" value="保存" class="w60 save" onclick="SaveTime()" /></span>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <!--客户信息-->
            </div>
        </div>
    </div>
    <!--信息展示-->
    <div class="clearfix">
    </div>
    </div>
    <div class="clearfix">
    </div>
</asp:Content>
