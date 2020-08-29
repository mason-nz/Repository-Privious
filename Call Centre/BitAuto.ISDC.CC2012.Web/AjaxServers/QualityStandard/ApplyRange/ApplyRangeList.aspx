<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyRangeList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ApplyRange.ApplyRangeList" %>

<div class="bit_table" id="bit_table">
    <div class="optionBtn clearfix">
    </div>
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="15%">
                所属分组
            </th>
            <th width="15%">
                所属业务
            </th>
            <th width="30%">
                录音评分表
            </th>
            <th width="30%">
                会话评分表
            </th>
            <th width="10%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"> 
                        <td>
                            <%#Eval("Name")%>&nbsp;
                        </td>                 
                        <td align="center">
                            <%#GetBusinessType(Eval("BusinessType").ToString())%>&nbsp;
                        </td>                
                        <td align="center">                            
                            <%#GetQSRTName(Eval("BGID").ToString(), Eval("BusinessType").ToString(), Eval("QS_RTName").ToString(), "ly", Eval("QS_RTID").ToString())%>
                        </td>
                        <td> 
                            <%#GetQSRTName(Eval("BGID").ToString(), Eval("BusinessType").ToString(), Eval("QS_IM_RTName").ToString(), "hh", Eval("QS_IM_RTID").ToString())%>
                        </td>                        
                        <td align="center" id="option_<%#Eval("BGID") %>">
                           <%#GetOptionLink(Eval("BGID").ToString(), Eval("BusinessType").ToString())%>
                        </td>                        
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        //编辑
        function On_Modify(bgid, btype) {
            var select1 = "select_ly_" + bgid;
            var select2 = "select_hh_" + bgid;
            var span1 = "span_ly_" + bgid;
            var span2 = "span_hh_" + bgid;
            var value1 = $.trim($("#hidden_ly_" + bgid).val());
            var value2 = $.trim($("#hidden_hh_" + bgid).val());
            //显示编辑区域
            $("#" + select1).css("display", "block");
            $("#" + select2).css("display", "block");
            $("#" + span1).css("display", "none");
            $("#" + span2).css("display", "none");
            //select 赋值
            getScoreTableName(select1, select2);
            //默认值
            $("#" + select1).val(value1);
            $("#" + select2).val(value2);
            //显示保存取消按钮
            $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_SaveOne('" + bgid + "','" + btype + "')\">保存</a>&nbsp;" +
                                                            "<a href=\"javascript:void(0);\" onclick=\"On_CancelOne('" + bgid + "','" + btype + "')\">取消</a>");
        }
        //取消
        function On_CancelOne(bgid, btype) {
            var select1 = "select_ly_" + bgid;
            var select2 = "select_hh_" + bgid;
            var span1 = "span_ly_" + bgid;
            var span2 = "span_hh_" + bgid;
            //隐藏编辑区域
            $("#" + select1).css("display", "none");
            $("#" + select2).css("display", "none");
            $("#" + span1).css("display", "block");
            $("#" + span2).css("display", "block");
            //清空选择项
            $("#" + select1).html("");
            $("#" + select2).html("");
            //显示编辑按钮
            $("#option_" + bgid).html("<a href=\"javascript:void(0);\" onclick=\"On_Modify('" + bgid + "','" + btype + "')\">编辑</a>");
        }
        //保存
        function On_SaveOne(bgid, btype) {
            var select1 = "select_ly_" + bgid;
            var select2 = "select_hh_" + bgid;
            var span1 = "span_ly_" + bgid;
            var span2 = "span_hh_" + bgid;
            //获取界面数据
            var select1_val = $.trim($("#" + select1).val());
            var select2_val = $.trim($("#" + select2).val());
            if (select1_val == "") select1_val = "-1";
            if (select2_val == "") select2_val = "-1";
            //获取文本值
            var select1_txt = $("select[id='" + select1 + "'] option[value='" + select1_val + "']").text();
            var select2_txt = $("select[id='" + select2 + "'] option[value='" + select2_val + "']").text();
            if (select1_val == "-1") {
                select1_txt = "无";
            }
            else {
                select1_txt = "<a href=\"/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=" + select1_val + "\" target=\"_blank\">" + select1_txt + "</a>";
            }
            if (select2_val == "-1") {
                select2_txt = "无";
            }
            else {
                select2_txt = "<a href=\"/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=" + select2_val + "\" target=\"_blank\">" + select2_txt + "</a>";
            }
            //保存数据
            $.jConfirm("是否保存此行数据？", function (r) {
                if (r) {
                    //入库
                    param = {
                        Action: "RangeManage",
                        BGID: bgid,
                        QS_RTID: select1_val,
                        QS_IM_RTID: select2_val,
                        r: Math.random()
                    };
                    AjaxPostAsync("/AjaxServers/QualityStandard/ApplyRange/ApplyRangeHandler.ashx", param, null, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "ok") {
                            //更新span的值
                            $("#" + span1).html(select1_txt);
                            $("#" + span2).html(select2_txt);
                            //更新hidden的值
                            $("#hidden_ly_" + bgid).val(select1_val);
                            $("#hidden_hh_" + bgid).val(select2_val);
                            //取消
                            On_CancelOne(bgid, btype);
                        }
                        else {
                            $.jAlert("保存失败：" + jsonData.msg);
                        }
                    });
                }
            });
        }
        //评分表
        function getScoreTableName(id1, id2) {
            AjaxPostAsync("/AjaxServers/CommonHandler.ashx", { Action: "getFieldName", TableName: "QS_RulesTable", IDField: "QS_RTID", ShowField: "Name", TableStatus: "10003", r: Math.random() }, null, function (data) {
                if ($("#" + id1).length > 0) {
                    $("#" + id1).append("<option value='-1'>请选择</option>");
                }
                if ($("#" + id2).length > 0) {
                    $("#" + id2).append("<option value='-1'>请选择</option>");
                }
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        if ($("#" + id1).length > 0) {
                            $("#" + id1).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                        }
                        if ($("#" + id2).length > 0) {
                            $("#" + id2).append("<option value=" + jsonData[i].ID + ">" + jsonData[i].Name + "</option>");
                        }
                    }
                }
            });
        }
    </script>
</div>
