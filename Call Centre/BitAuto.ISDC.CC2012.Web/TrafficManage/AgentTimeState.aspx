<%@ Page Title="实时监控" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="AgentTimeState.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.AgentTimeState" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <script type="text/javascript">
        var preData;
        var timerID;

        function ConvertTime(iSec) {
            try {
                var retval = "";
                var iHour = 0;
                var iHour2 = 0;
                var iMinute = 0;
                var iSecond = 0;

                iHour = parseInt(iSec / 3600);
                iHour2 = parseInt(iSec % 3600);

                iMinute = parseInt(iHour2 / 60);
                iSecond = parseInt(iHour2 % 60);

                if (iSec < 0) {
                    retval = "00:00:00";
                }
                else {
                    var sHour = "", sMinute = "", sSecond = "";
                    if (iHour < 10)
                        sHour = "0" + iHour;
                    else {
                        sHour = iHour;
                    }

                    if (iMinute < 10)
                        sMinute = "0" + iMinute;
                    else
                        sMinute = iMinute;

                    if (iSecond < 10)
                        sSecond = "0" + iSecond;
                    else
                        sSecond = iSecond;

                    retval = sHour + ":" + sMinute + ":" + sSecond;

                }
                return retval;
            } catch (e) {
                alert("error is:" + e);
            }
        }

        function CreateHtmlTR_OLD(n) {
            var d;
            var preAGtime = 0;

            var stmp = "";
            stmp = n.StartTime.charAt(4);
            if (stmp == "-") {
                n.StartTime = n.StartTime.replace(/-/g, "/");
            }
            d = new Date(n.StartTime);
            preAGtime = d.getTime();

            var d2 = new Date();
            var curAGtime = 0;
            curAGtime = d2.getTime();

            var isec = 0;
            var tickcount = 0;


            isec = parseInt((curAGtime - d.getTime()) / 1000);
            tickcount = ConvertTime(isec);
            //alert("aa"+ tickcount);
            //$("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(4).text(tickcount);

            var html = "";
            html = "<tr onmouseout=''this.className='back'' onmouseover=''this.className='hover'' class='back' AgentID=" + n.AgentID + ">" +
                           "<td AgentName=" + n.AgentName + ">" + n.AgentName + "</td>" +
                           "<td AgentNum=" + n.AgentNum + ">" + n.AgentNum + "</td>" +
                           "<td AgentID=" + n.AgentID + ">" + n.AgentID + "</td>" +
                           "<td State=" + n.State + ">" + n.State + "</td>" +
                           "<td StartTime='" + n.StartTime + "'>" + n.StartTime + "</td>" +
            //"<td style='color: #0066ff;' AGTime='" + d.getTime() + "'>00:00:00</td>" +
                           "<td style='color: #0066ff;' AGTime='" + preAGtime + "'>" + tickcount + "</td>" +
                           "</tr>";
            $("#tableList").append(html);
        }

        function CreateHtmlTR(n) {
            var html = "";
            //如果是状态是 工作中或置忙，并且 时长超过设置参数
            if (n.IsRed == "True") {
                html = "<tr onmouseout=''this.className='back'' onmouseover=''this.className='hover'' class='back' AgentID=" + n.AgentID + ">" +
                           "<td style='color:Red;' GroupName=" + n.GroupName + ">" + n.GroupName + "</td>" +
                           "<td style='color:Red;' AgentName=" + n.AgentName + ">" + n.AgentName + "</td>" +
                           "<td style='color:Red;' AgentNum=" + n.AgentNum + ">" + n.AgentNum + "</td>" +
                           "<td style='color:Red;' AgentID=" + n.AgentID + ">" + n.AgentID + "</td>" +
                           "<td style='color:Red;' State=" + n.State + ">" + n.State + "</td>" +
                           "<td style='color:Red;' StartTime='" + n.StartTime + "'>" + n.StartTime + "</td>" +
                           "<td style='color:Red;'>" + n.AGTime + "</td>" +
                           "</tr>";
            }
            else {
                html = "<tr onmouseout=''this.className='back'' onmouseover=''this.className='hover'' class='back' AgentID=" + n.AgentID + ">" +
                           "<td GroupName=" + n.GroupName + ">" + n.GroupName + "</td>" +
                           "<td AgentName=" + n.AgentName + ">" + n.AgentName + "</td>" +
                           "<td AgentNum=" + n.AgentNum + ">" + n.AgentNum + "</td>" +
                           "<td AgentID=" + n.AgentID + ">" + n.AgentID + "</td>" +
                           "<td State=" + n.State + ">" + n.State + "</td>" +
                           "<td StartTime='" + n.StartTime + "'>" + n.StartTime + "</td>" +
                           "<td style='color: #0066ff;'>" + n.AGTime + "</td>" +
                           "</tr>";
            }
            $("#tableList").append(html);
        }

        function LoadData_OLD() {
            AjaxPost('/AjaxServers/TrafficManage/AgentTimeState.ashx', 'showAgentTimeState=yes&random=' + Math.random(), null, success);
            function success(data) {
                var mbi = $.evalJSON(data);
                preData = mbi;
                $.each(mbi, function (i, n) {
                    //n.StartTime = "2013-8-8 8:42:08";
                    CreateHtmlTR(n);
                });
            }
        }

        function LoadData() {
            AjaxPost('/AjaxServers/TrafficManage/AgentTimeState.ashx', _params(), null, success);
            function success(data) {
                var mbi = $.evalJSON(data);
                $.each(mbi, function (i, n) {
                    //生成分机HTML
                    $("#mypager").empty();
                    $("#mypager").append(n.PagerHtml);
                    CreateHtmlTR(n);
                });
            }
        }

        function RefreshData_OLD() {
            AjaxPost('/AjaxServers/TrafficManage/AgentTimeState.ashx', 'showAgentTimeState=yes&random=' + Math.random(), null, success);
            function success(data) {
                var mbi = $.evalJSON(data);
                $.each(mbi, function (i, n) {
                    //首先查看是否有新数据，有则生成新HTML
                    var isFind = false;
                    $.each(preData, function (j, x) {
                        //alert("preAgetnID=" + n.AgentID + ",curAgentID=" + x.AgentID);
                        if (n.AgentID == x.AgentID) {
                            //找到更新
                            isFind = true;
                            //$("#tableList").find("tr:has(th)")
                            $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(3).text(n.State);
                            $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(4).text(n.StartTime);
                            //$("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(5).text(n.AGTime);

                            var d;
                            d = new Date();
                            var isec = 0;
                            var preAGtime = 0;
                            var curAGtime = 0;
                            curAGtime = d.getTime();
                            if (n.State == x.State) {
                                //不做操作                                
                                preAGtime = $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(5).attr("AGTime");
                                isec = parseInt((curAGtime - preAGtime) / 1000);
                                var tickcount = 0;
                                tickcount = ConvertTime(isec);
                                $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(5).text(tickcount);
                            }
                            else {
                                //状态发生变化，初始化秒表初始值
                                $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(5).attr("AGTime", curAGtime);
                                $("#tableList").find("tr[AgentID=" + n.AgentID + "]").eq(0).find("td").eq(5).text("00:00:00");
                            }

                        }
                        else {

                        }

                    });

                    if (isFind == false) {

                        CreateHtmlTR(n);
                    }

                });

                $.each(preData, function (i, n) {
                    var isFind = false;
                    $.each(mbi, function (j, x) {

                        if (n.AgentID == x.AgentID) {
                            isFind = true;
                        }

                    });

                    if (isFind == false) {
                        //说明是已退出的坐席记录，要从界面上删除
                        $("#tableList").find("tr[AgentID=" + n.AgentID + "]").remove();
                    }
                });

                preData = mbi;
            }
        }


        function RefreshData() {
            var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间

            var paramObj = _params();
            AjaxPost('/AjaxServers/TrafficManage/AgentTimeState.ashx', paramObj, null, success);
            function success(data) {
                //先删除老数据 然重新渲染新数据(目的:实现按时长、状态排序)
                $("#tableList").find("tr[agentid]").remove();
                var mbi = $.evalJSON(data);
                $.each(mbi, function (i, n) {
                    //生成分机HTML
                    $("#mypager").empty();
                    $("#mypager").append(n.PagerHtml);
                    //生成HTML
                    CreateHtmlTR(n);

                });

                var strParam = JSON.stringify(paramObj);
                var reg = new RegExp(":", "g");
                var reg1 = new RegExp(",", "g");
                var reg2 = new RegExp("\"", "g");
                strParams = strParam.replace(reg, "=");
                strParams = strParams.replace(reg1, "&");
                strParams = strParams.replace(reg2, "");
                strParams = strParams.substring(1, strParams.length - 1);
                StatAjaxPageTime(monitorPageTime, "/AjaxServers/TrafficManage/AgentTimeState.ashx" + "?" + strParams);
            }
        }


        function StartTimer() {
            timerID = window.setInterval(RefreshData, 3000);
        }

        function StopTimer() {
            window.clearInterval(timerID);
        }

        var timerID2;
        function StartTimer2() {
            timerID2 = window.setInterval(loadAgentStatistics, 3000);
        }

        function StopTimer2() {
            window.clearInterval(timerID2);
        }

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });

        }

        //选择客服
        function SelectVisitPerson(actionName, txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                parameters: { Action: actionName, DisplayGroupID: $("#<%=selGroup.ClientID %>").val() },
                url: "../../AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                    ;
                }
            });
        }

        //查询
        function search() {
            RefreshData();
        }

        //分页操作
        function ShowDataByPost1(p) {
            $("#hidSelectPageIndex").val(p);
            RefreshData();
        }

        //获取参数
        function _params() {
            //工号
            var ptid = $.trim($("#txtAgentNum").val());
            //客服状态
            var status = $("#selStatus").val();
            //所属业务缄
            var group = "";
            if ($("#<%=selGroup.ClientID %>").val() != "-1" && $("#<%=selGroup.ClientID %>").val() != undefined) {
                group = $("#<%=selGroup.ClientID %>").val();
            }
            //客服
            var agent = $("#hidSelOperId").val();
            var pageSize = $("#hidSelectPageSize").val();
            var pageIndex = $("#hidSelectPageIndex").val();

            var pody = {
                AgentNum: ptid,
                AgentState: status,
                BGID: group,
                AgentUserID: agent,
                pageSize: pageSize,
                page: pageIndex,
                LoginUserID: '<%=userid %>',
                random: Math.random(),
                showAgentTimeState: 'yes'
            }

            return pody;
        }

        //根据不同的点击绑定不同的信息页面
        function loadHtml(n, othis) {
            //$('#hidSearchType').val(n);
            $(othis).addClass("redColor").siblings().removeClass("redColor");
            if (n == 2) {
                $("#ajaxTable").hide();
                $("#ajaxTable2").show();
                StopTimer();
                StartTimer2();
            }
            else if (n == 1) {
                $("#ajaxTable").show();
                $("#ajaxTable2").hide();
                StartTimer();
                StopTimer2();
            }
        }

        //实时统计
        function loadAgentStatistics() {
            //LoadingAnimation("ajaxTable");

            var pody = _params();
            $("#ajaxTable2").load("/AjaxServers/TrafficManage/AgentTimeStateList.aspx", pody, function () {

            });
        }

        $(document).ready(function () {
            //设置分页每页记录数
            var pageSize = $("#hidSelectPageSize").val();
            $("a[name='apageSize'][v='" + pageSize + "']").addClass("selectA");

            $("a[name='apageSize']").bind("click", function (e) {
                e.preventDefault();
                $("a[name='apageSize']").removeClass("selectA");
                $(this).addClass("selectA");

                $("#hidSelectPageSize").val($(this).attr("v"));
                RefreshData();
            });
            getUserGroup();
            RefreshData();
            //LoadData();
            StartTimer();
        });

    
    </script>
    <div class="search clearfix">
        <input type="hidden" id="hidSelectPageIndex" value="1" />
        <input type="hidden" id="hidSelectPageSize" value="200" />
        <ul class="clear">
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" runat="server" class="w125" style="width: 129px">
                </select>
            </li>
            <li>
                <label>
                    客服：</label>
                <input type="text" id="txtSelOper" class="w125" readonly="true" onclick="SelectVisitPerson('','txtSelOper','hidSelOperId')" />
                <input type="hidden" id="hidSelOperId" value="-2" />
            </li>
            <li>
                <label>
                    工号：</label>
                <input type="text" id="txtAgentNum" class="w125" />
            </li>
            <li>
                <label>
                    客服状态：</label>
                <select id="selStatus" class="w125" style="width: 129px">
                    <option value="-2">请选择</option>
                    <option value="9">工作中</option>
                    <option value="5">话后</option>
                    <option value="4">置忙</option>
                    <option value="3">置闲</option>
                    <option value="8">振铃</option>
                </select>
            </li>
            <%--<li class="btnsearch" style=" margin-left:145px;">
                <input style="float: right" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>--%>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <a href="javascript:void(0)" onclick="javascript:loadHtml(1,this)" id="aList1" class="redColor">
            实时监控</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="javascript:loadHtml(2,this)"
                id="aList2">实时统计</a>
        <input type="button" class="newBtn" onclick="RefreshData()" value="刷新" name="" />
    </div>
    <div id="ajaxTable" cellpadding="0" cellspacing="0" width="99%" class="bit_table">
        <table cellspacing="0" cellpadding="0" width="99%" id="tableList" class="tableList mt10 mb15">
            <tbody>
                <tr onmouseout="this.className='back'" class="back">
                    <th style="width: 15%;">
                        所属分组
                    </th>
                    <th style="width: 15%;">
                        客服
                    </th>
                    <th style="width: 15%;">
                        工号
                    </th>
                    <th style="width: 15%;">
                        分机号
                    </th>
                    <th style="width: 15%;">
                        当前状态
                    </th>
                    <th style="width: 15%;">
                        开始时间
                    </th>
                    <th style="width: 10%;">
                        当前状态时长
                    </th>
                </tr>
            </tbody>
        </table>
        <!--分页-->
        <div class="pageTurn mr10" style="margin-top: 10px;">
            <%--<p class="pageP">
                每页显示条数 <a href="#" name="apageSize" v='20'>20</a>&nbsp;&nbsp; <a href="#" name="apageSize"
                    v='50'>50</a>&nbsp;&nbsp; <a href="#" name="apageSize" v='100'>100</a>
            </p>--%>
            <p id="mypager">
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    <div id="ajaxTable2" cellpadding="0" cellspacing="0" width="99%" class="bit_table">
    </div>
</asp:Content>
