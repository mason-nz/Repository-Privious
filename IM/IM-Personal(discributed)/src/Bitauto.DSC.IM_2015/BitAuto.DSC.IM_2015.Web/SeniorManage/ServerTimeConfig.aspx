<%@ Page Language="C#" Title="服务时间" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="ServerTimeConfig.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.ServerTimeConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function allsave() {
            var flag = true;
            var ServerTimeList = new Array();
            $("#trList tr:gt(0)").each(function () {
                var sourcetype = $(this).find("td:eq(0)").attr("sourcetype");
                var ST = $(this).find("td:eq(1) input[type='text'][name='ST']").val();
                var ET = $(this).find("td:eq(1) input[type='text'][name='ET']").val();
                var sourcetypename = $(this).find("td:eq(0)").text();
                if (CompareTime(ST, ET, sourcetypename, $(this))) {
                    var servertimeModel = {
                        SourceTypeName: escape(sourcetypename),
                        SourceType: escape(sourcetype),
                        ST: escape(ST),
                        ET: escape(ET)
                    };
                    ServerTimeList.push(servertimeModel);
                }
                else {
                    flag = false;
                    return false;
                }
            });
            if (flag) {
                var ServerTimeStr = JSON.stringify(ServerTimeList);
                var pody = {
                    ServerTimeStr: escape(ServerTimeStr),
                    Action: escape('savetime')
                };
                AjaxPostAsync("/AjaxServers/SeniorManage/BussinessConfigDeal.ashx", pody, function () { }, function (data) {
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
        //校验时间大小
        function CompareTime(st, et, sourctypename, control) {
            if (st.indexOf(':') < 0) {
                $.jAlert(sourctypename + "开始时间 [分钟:秒] 输入不正确", function () { $(control).find("td:eq(1) input[type='text'][name='ST']").focus(); });
                return false;
            }
            if (et.indexOf(':') < 0) {
                $.jAlert(sourctypename + "结束时间 [分钟:秒] 输入不正确", function () { $(control).find("td:eq(1) input[type='text'][name='ET']").focus(); });
                return false;
            }
            var time1 = SplitTime(st);
            var time2 = SplitTime(et);
            if (time1 == null) {
                $.jAlert(sourctypename + "开始时间 [分钟:秒] 输入不正确", function () { $(control).find("td:eq(1) input[type='text'][name='ST']").focus(); });
                return false;
            }
            else if (time2 == null) {
                $.jAlert(sourctypename + "结束时间 [分钟:秒] 输入不正确", function () { $(control).find("td:eq(1) input[type='text'][name='ET']").focus(); });
                return false;
            }
            else {
                if (time1.hour > time2.hour) {
                    $.jAlert(sourctypename + "开始时间不能大于结束时间", function () { $(control).find("td:eq(1) input[type='text'][name='ST']").focus(); });
                    return false;
                }
                else if (time1.hour == time2.hour && time1.min > time2.min) {
                    $.jAlert(sourctypename + "开始时间不能大于结束时间", function () { $(control).find("td:eq(1) input[type='text'][name='ST']").focus(); });
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

            if (isNaN(hour) || isNaN(min)) {
                return null;
            }
            else {
                hour = parseInt(hour);
                min = parseInt(min);
                return { hour: hour, min: min }
            }
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div id="ajaxMessageInfo" class="cxList" style="margin-top: 8px; height: auto;">
            <table border="0" cellspacing="0" cellpadding="0">
                <tbody id="trList">
                    <tr>
                        <th width="25%">
                            业务线
                        </th>
                        <th width="50%">
                            服务时间
                        </th>
                    </tr>
                    <asp:Repeater ID="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor: pointer" class="" onclick="">
                                <td sourcetype="<%#((BitAuto.DSC.IM_2015.BLL.TimeModelClss)Container.DataItem).SourceType%>">
                                    <%#((BitAuto.DSC.IM_2015.BLL.TimeModelClss)Container.DataItem).SourceTypeName%>
                                </td>
                                <td>
                                    <input type="text" name="ST" value='<%#((BitAuto.DSC.IM_2015.BLL.TimeModelClss)Container.DataItem).ST%>' />至
                                    <input type="text" name="ET" value='<%#((BitAuto.DSC.IM_2015.BLL.TimeModelClss)Container.DataItem).ET%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div class="btn submit">
            <input type="button" value="提交" onclick="allsave()" class="w80" />
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
