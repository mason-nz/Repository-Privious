<%@ Page Title="人员管理—所有客户" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true" CodeBehind="MainListAllUser.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.MainListAllUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .defselect
        {
            width: 224px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            RegionValueChange();
            LoadingAnimation('bit_table');
            var pody = params();
            $("#ajaxTable").load("../AjaxServers/ZuoxiManage/ZuoxiTableListAll.aspx?pwd=<%=Pwd %>", pody);
        });
        //查询-填充列表容器
        function search(page) {
            LoadingAnimation('bit_table');
            var pody = params();
            if (page != undefined) {
                pody = pody.replace(/&page=[0-9]*/, '') + "&page=" + page;
            }
            $("#ajaxTable").load("../AjaxServers/ZuoxiManage/ZuoxiTableListAll.aspx?pwd=<%=Pwd %>", pody);
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('bit_table');
            //$('#ajaxTable').load('../AjaxServers/ZuoxiManage/ZuoxiTableList.aspx .bit_table > *', pody);
            $('#ajaxTable').load('../AjaxServers/ZuoxiManage/ZuoxiTableListAll.aspx', pody);
        }
        function params() {
            var EmpName = $.trim($("#EmpName").val());
            var agentNum = $.trim($("#agentNum").val());
            var role = "";
            var questionQuality = $("#Erole option:selected").val();
            if (questionQuality == '-1') {
                questionQuality = '';
            }
            else {
                role = "'" + $("#Erole option:selected").val() + "'";
            }
            var regionid = $("#sltArea").val();


            var pody = 'trueName=' + escape(EmpName) + '&agentNum=' + agentNum + '&role='
                        + role + '&bgId=' + $("#sltGroup").val() + '&RegionID=' + regionid + '&page=' + $("#pageHiddenMain").val() + '&random=' + Math.round(Math.random() * 10000);
            return pody;
        }

        function RegionValueChange() {
            var areaId = $("#sltArea").val();
            $.post("/AjaxServers/ZuoxiManage/Handler.ashx", { Action: "getgroupbyregionid", AreaID: areaId }, function (data) {
                $("#sltGroup").empty();
                $("#sltGroup").append("<option value=''>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData, function (i, item) {
                        $("#sltGroup").append("<option value='" + item.BGID + "'>" + item.Name + "</option>");
                    });
                }
            });
        }
    </script>
    <div>
        <div class="search" id="SearchCon">
            <ul class="clearfix">
                <li style="margin-right: 0;">
                    <label>
                        姓名：
                    </label>
                    <input type="text" name="EmpName" id="EmpName" class="w220" />
                </li>
                <li style="margin-right: 0;">
                    <label>
                        工号：
                    </label>
                    <input type="text" name="agentNum" id="agentNum" class="w220" />
                </li>
                <li style="margin-right: 0;">
                    <label>
                        所属区域：
                    </label>
                    <select id="sltArea" class="defselect" onchange="RegionValueChange()">
                        <option value="">请选择</option>
                        <asp:Repeater runat="server" ID="rptArea">
                            <ItemTemplate>
                                <option value="<%#Eval("Value") %>">
                                    <%#Eval("Name")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>          
                <li style="margin-right: 0;">
                    <label>
                        所属分组：
                    </label>
                    <select id="sltGroup" class="defselect" onchange="">
                    </select>
                </li>
                <li style="margin-right: 0;">
                    <label>
                        角色：
                    </label>
                    <select id="Erole" class="defselect">
                        <option value="-1">请选择</option>
                        <asp:Repeater runat="server" ID="Rpt_Role">
                            <ItemTemplate>
                                <option value="<%#Eval("RoleID") %>">
                                    <%#Eval("RoleName") %></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>
                <li class="btnsearch" style="padding: 5px; margin-top: -5px; margin-left: 0px; margin-right: 0px;">
                    <div style="width: 310px;">
                        <input type="button" name="Search" value="查询" onclick="search()" style="float: right" />
                    </div>
                </li>
            </ul>
        </div>
        <div id="ajaxTable">
        </div>
        <input type="hidden" id="pageHiddenMain" />
    </div>
</asp:Content>
