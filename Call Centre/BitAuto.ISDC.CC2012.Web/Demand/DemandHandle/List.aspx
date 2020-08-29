<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" MasterPageFile="~/Controls/Top.Master"
    Inherits="BitAuto.ISDC.CC2012.Web.Demand.DemandHandle.List" Title="需求处理" %>

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
        /*客户列表 编辑分类按钮样式*/
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
    <script type="text/javascript" src="/Js/Enum/Area2.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(Search);
            //初始化日历控件，前面的日期不能大于后面的日期
            $('#txtBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtBeginTime\')}' }); });

            BindProvince('ddlSearchProvince'); //绑定省份
            $("[id$=ddlSearchProvince]").change(function () {
                BindCity('ddlSearchProvince', 'ddlSearchCity');
                BindCounty('ddlSearchProvince', 'ddlSearchCity', 'ddlSearchCounty');
            });
            $("[id$=ddlSearchCity]").change(function () {
                BindCounty('ddlSearchProvince', 'ddlSearchCity', 'ddlSearchCounty');
            });
            Search(0);
        });
        var param = {};
        var status = "0";
        function Search(s) {

            if (s !== undefined && s != "") {
                status = s;
            }
            var memberName = $.trim($('#txtMemberName').val());
            var province = $('#ddlSearchProvince').val();
            var provinceId = province == null ? "-1" : province;
            var city = $('#ddlSearchCity').val();
            var cityId = city == null ? "-1" : city;
            var county = $('#ddlSearchCounty').val();
            var countyId = county == null ? "-1" : county;
            var department = $("[id$='sltArea']").val();
            var agentName = $.trim($("#txtAgentName").val());
            var isOverFlow = $("[name='IsOverflow']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(",");

            if (isOverFlow.indexOf(",") > 0) {
                isOverFlow = "";
            }
            var demandID = $("#txtDemandID").val();
            var memberCode = $("#txtMemberCode").val();
            var beginTime = $("#txtBeginTime").val();
            var endTime = $("#txtEndTime").val();
            param = {
                MemberName: escape(memberName),
                ProvinceID: provinceId,
                CityID: cityId,
                CountyID: countyId,
                Department: department,
                Status: status,
                KeFuName: escape(agentName),
                IsOverflow: isOverFlow,
                DemandID: demandID,
                MemberCode: memberCode,
                BeginTime_Start: beginTime,
                BeginTime_End: endTime
            };
            var paramsStr = "";
            for (var attr in param) {
                paramsStr += "&" + attr + "=" + param[attr];
            }
            LoadingAnimation("divContent");
            $('#divContent').load("/AjaxServers/Demand/DemandHandle/List.aspx?R=" + Math.random(), paramsStr, function () {
                GetStatusNum(param);
            });
        }

        function ExportData() {
            var paramsStr = "";
            for (var attr in param) {
                paramsStr += "&" + attr + "=" + param[attr];
            }
            window.location = "/Demand/DemandHandle/Export.aspx?Browser=" + GetBrowserName() + paramsStr;
        }

        function ShowDataByPost1(pody) {
            LoadingAnimation("divContent");
            $('#divContent').load('/AjaxServers/Demand/DemandHandle/List.aspx?R=' + Math.random(), pody);
        }

        //获取任务状态数量
        function GetStatusNum(pody) {
            pody.Action = "getstatusnum";
            var oUl = document.getElementById("selectli");
            oUl.innerHTML = "";
            var aA = oUl.getElementsByTagName('a');

            AjaxPostAsync("/AjaxServers/Demand/DemandHandle/Handler.ashx", pody, null, function (data) {
                var jsonData = eval("(" + data + ")");
                var html = "", total = 0;
                for (var name in jsonData) {
                    var oLi = createLi(name, jsonData);
                    oUl.appendChild(oLi);
                    total += parseInt(jsonData[name][1]);
                }
                var oLi = createLi("全部", { 全部: ['0', total] });
                oUl.insertBefore(oLi, oUl.children[0]);

            });
            function createLi(name, obj) {
                var oLi = document.createElement("li");
                oLi.style.width = "auto";
                oLi.style.marginRight = "18px";
                oLi.className = "w180";
                var oA = document.createElement("a");
                oA.setAttribute("val", obj[name][0]);
                oA.href = "javascript:;";
                oA.innerHTML = name + "（" + obj[name][1] + "）";
                oA.onclick = function () {
                    for (var k = 0; k < aA.length; k++) { aA[k].className = ""; }
                    Search(this.getAttribute("val"));
                }
                if (obj[name][0] == status) {
                    oA.className = "cur";
                }
                oLi.appendChild(oA);
                return oLi;
            }
        }

        //选择负责客服
        function selectServicePop(afterFn) {

            $.openPopupLayer({
                name: "AlloServicePop",
                url: "/AlloCustomer/AlloServicePop.aspx",
                success: function () {
                    if (afterFn) { $("#popAClear").hide(); }
                },
                beforeClose: function (e) {
                    if (!e) return;

                    var userid = $('#popupLayer_' + 'AlloServicePop').data('userid');
                    if (afterFn) {
                        afterFn(userid);
                    }
                    else {
                        $("#txtAgentName").val($('#popupLayer_' + 'AlloServicePop').data('name'));
                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(Search);
                }
            });
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
                        经销商名称：</label><input type="text" maxlength="50" class="w190" name="txtMemberName"
                            id="txtMemberName" />
                </li>
                <li>
                    <label>
                        经销商地区：</label>
                    <select id="ddlSearchProvince" style="width: 81px" name="ddlSearchProvince" class="kProvince">
                    </select>
                    <select id="ddlSearchCity" style="width: 81px" name="ddlSearchCity" onchange="javascript:BindCounty('ddlSearchProvince','ddlSearchCity','ddlSearchCounty');">
                    </select>
                    <select id="ddlSearchCounty" name="ddlSearchCounty" class="kArea" style="width: 81px;">
                    </select>
                </li>
                <li>
                    <label>
                        所属大区：</label>
                    <select id="sltArea" class="w160" runat="server">
                    </select>
                </li>
            </ul>
            <ul class="clear">
                <li>
                    <label>
                        负责客服：</label><input type="text" maxlength="50" class="w190" id="txtAgentName" onclick="selectServicePop()" />
                </li>
                <li style="width: 341px;">
                    <label>
                        服务开始周期：</label>
                    <input type="text" name="BeginTime" id="txtBeginTime" class="w100" style="width: 111px;" />
                    至
                    <input type="text" name="EndTime" id="txtEndTime" class="w100" style="width: 112px;" />
                </li>
                <li>
                    <label>
                        需求编号：</label><input type="text" class="w160" id="txtDemandID" style="width: 155px;" />
                </li>
            </ul>
            <ul class="clear">
                <li>
                    <label>
                        会员ID：</label><input type="text" class="w190" id="txtMemberCode" />
                </li>
                <li>
                    <label>
                        是否超量：</label><input type="checkbox" value="0" name="IsOverflow" />是<input type="checkbox"
                            name="IsOverflow" value="1" />否</li>
                <li style="width: 426px; margin-right: 0px; text-align: right;" class="btnsearch">
                    <input name="" type="button" value="查 询" onclick="javascript:Search()" />
                </li>
            </ul>
            <ul class="clear">
                <li style="width: 750px; height: 30px; margin: 10px 0 -10px 0;">
                    <div id="divStatus" class="cxTab mt5" style="margin-left: 30px;">
                        <ul id="selectli" class="">
                        </ul>
                        <div class="clearfix">
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div id="divContent">
        </div>
    </div>
</asp:Content>
