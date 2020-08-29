<%@ Page Language="C#" Title="今日概况" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ToadySummary.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.ToadySummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="refresh" content="30" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Scripts/HighCharts/highcharts.js" type="text/javascript"></script>
    <script src="../Scripts/HighCharts/exporting.js" type="text/javascript"></script>
    <script src="../Scripts/Chart.js" type="text/javascript"></script>
    <!--内容开始-->
    <div class="content">
        <!--列表开始-->
        <div class="cxList cxList_chart">
            <!--图表开始-->
            <div class="table_bt">
                <div class="title">
                    ● 客服&访客实时在线情况</div>
            </div>
            <div class="clearfix">
            </div>
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                </thead>
                <tr class="bgnone">
                    <td class="chart_dh" style="width: 33.33%">
                        <div id="divCharts">
                        </div>
                    </td>
                    <td class="chart_dh" style="width: 33.33%">
                        <div id="divCharts1">
                        </div>
                    </td>
                    <td class="chart_dh" style="width: 33.33%">
                        <div id="divCharts3">
                        </div>
                    </td>
                </tr>
            </table>
            <!--图表结束-->
            <!--列表1开始-->
            <table border="0" cellspacing="0" cellpadding="0" width="100%" id="tab">
                <tr>
                    <th>
                        所属分组
                    </th>
                    <th width="18%">
                        分组客服数
                    </th>
                    <th width="12%">
                        登录客服数
                    </th>
                    <th width="11%">
                        在线客服数
                    </th>
                    <th width="11%">
                        暂离客服数
                    </th>
                    <th width="11%">
                        离线客服数
                    </th>
                    <th width="11%">
                        接待客服数
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rpeList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#GetInBGIDName(Eval("InBGIDName").ToString(),Eval("InBGID").ToString())%>&nbsp;
                            </td>
                            <td>
                                &nbsp;<%#Eval("AgentIDCount")%>
                            </td>
                            <td>
                                &nbsp;<%#Eval("AgentLoginCount")%>
                            </td>
                            <td>
                                <%#Eval("AgentOnlineCount")%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("AgentBussyCount")%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("AgentLeaveCount")%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("AgentReceptionCount")%>
                                &nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <!--列表1结束-->
        </div>
        <!--列表结束-->
        <!--列表开始-->
        <div class="cxList cxList_chart" style="margin-top: 20px;">
            <div class="table_bt">
                <div class="title">
                    ● 今日业务线情况汇总<span style="color: #00A3D3">(数据每小时刷新)</span></div>
            </div>
            <div class="clearfix">
            </div>
            <!--列表1开始-->
            <table border="0" cellspacing="0" cellpadding="0" width="100%" id="tabbussiness">
                <tr>
                    <th width="20%">
                        业务线名称
                    </th>
                    <th width="10%">
                        页面访问量
                    </th>
                    <th width="10%">
                        总对话量
                    </th>
                    <th width="10%">
                        连通率
                    </th>
                    <th width="10%">
                        接待量
                    </th>
                    <th width="10%">
                        响应率
                    </th>
                    <th width="10%">
                        队列放弃量
                    </th>
                    <th width="10%">
                        放弃率
                    </th>
                    <th width="10%">
                        留言量
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rptToadyTotal">
                    <ItemTemplate>
                        <tr id='source<%#Eval("SourceType")%>' class="lenght">
                            <td class="name_ywx">
                                <%#GetSourceTypeName(Eval("SourceType").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("SumVisit")%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("SumConversation")%>&nbsp;
                            </td>
                            <td>
                                <%#GetAvg(Eval("SumVisit").ToString(), Eval("SumConversation").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("SumReception")%>&nbsp;
                            </td>
                            <td>
                                <%#GetAvg(Eval("SumConversation").ToString(), Eval("SumReception").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("SumQueueFail")%>&nbsp;
                            </td>
                            <td>
                                <%#GetAvg(Eval("SumVisit").ToString(), Eval("SumQueueFail").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("LeaveMessage")%>&nbsp;
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <table id="childdiv" style="display: none">
            <asp:Repeater runat="server" ID="rptChildTotal">
                <ItemTemplate>
                    <tr name="trChild">
                        <td colspan="11" style="padding: 0; border-left: none;">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="child_table">
                                <tr class="lenght">
                                    <td width="20%" class="name">
                                        <%#GetSourceTypeName(Eval("SourceType").ToString())%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#Eval("SumVisit")%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#Eval("SumConversation")%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#GetAvg(Eval("SumVisit").ToString(), Eval("SumConversation").ToString())%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#Eval("SumReception")%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#GetAvg(Eval("SumConversation").ToString(), Eval("SumReception").ToString())%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#Eval("SumQueueFail")%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#GetAvg(Eval("SumVisit").ToString(), Eval("SumQueueFail").ToString())%>&nbsp;
                                    </td>
                                    <td width="10%">
                                        <%#Eval("LeaveMessage")%>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--列表1结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            ShowLineChart();
            $("#tab tr").last().addClass("sum");
            $("#tabbussiness tr").last().addClass("sum");
            var sourceYiChe = $("#source100").find("td").eq(0);
            sourceYiChe.addClass("zhankai");
            var sourceName = sourceYiChe.html();
            var str = "<a href='javaScript:showChild()' id='showMore' name='-1'>+</a>";
            sourceYiChe.html(sourceName + str);

        });

        function ShowLineChart() {
            AjaxPost("/AjaxServers/TrailManager/SummaryApi.ashx?action=ShowChart", null, null, function (result) {
                ShowChart("divCharts", "pie", result[0]);
                ShowChart("divCharts1", "pie", result[1]);
                ShowChart("divCharts3", "pie", result[2], 1);
            });
        }
        function showChild() {

            var str = $("#showMore").attr("name");
            if (str == -1) {//默认
                $("#source100").after($("#childdiv tbody").html());
                $("#showMore").attr("name", 2)
                $("#showMore").html("-");
            }
            else if (str == 1) {//隐藏
                $("tr[name='trChild']").each(function () {
                    $(this).show();
                });
                $("#showMore").attr("name", 2)
                $("#showMore").html("-");
            }
            else if (str == 2) {//展开
                $("tr[name='trChild']").each(function () {
                    $(this).hide();
                });

                $("#showMore").attr("name", 1)
                $("#showMore").html("+");
            }
            var trLenght = $("tr:visible.lenght").length;
            var totalCount = 0;
            if (trLenght > 1) {
                totalCount = trLenght - 3;
            }
            $("#tabbussiness tr").last().find("td").eq(0).html("合计(共" + (totalCount) + "项）");
        }

    </script>
</asp:Content>
