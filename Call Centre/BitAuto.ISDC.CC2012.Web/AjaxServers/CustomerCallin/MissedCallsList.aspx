<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MissedCallsList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustomerCallin.MissedCallsList" %>

<!--未接来电查询 强斐 2016-8-16-->
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th name='Col_主叫号码' style="width: 10%;">
            主叫号码
        </th>
        <th name='Col_开始时间' style="width: 13%;">
            开始时间
        </th>
        <th name='Col_结束时间' style="width: 13%;">
            结束时间
        </th>
        <th name='Col_业务线' style="width: 8%;">
            业务线
        </th>
        <th name='Col_技能组' style="width: *">
            技能组
        </th>
        <th name='Col_处理人' style="width: 7%;">
            处理人
        </th>
        <th name='Col_处理时间' style="width: 13%;">
            处理时间
        </th>
        <th name='Col_处理状态' style="width: 7%;">
            处理状态
        </th>
        <th name='Col_留言' style="width: 4%">
            留言
        </th>
        <th name='Col_操作' style="width: 13%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">             
                <td name='Col_主叫号码'>
                    <span id="tr_<%#Eval("RecID").ToString() %>">
                        <%# BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("CallNO").ToString())%>
                    </span>&nbsp;
                </td> 
                <td name='Col_开始时间'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("StartTime").ToString(),"yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td> 
                <td name='Col_结束时间'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                </td> 
                <td name='Col_业务线'>
                    <%#Eval("HotLineName")%>&nbsp;
                </td> 
                <td name='Col_技能组'>
                    <%#Eval("SkillName")%>&nbsp;
                </td> 
                <td name='Col_处理人'>
                    <span id="span_pp_<%#Eval("RecID").ToString() %>">
                        <%# Eval("ProcessUserName")%>
                    </span>&nbsp;
                </td> 
                <td name='Col_处理时间'>
                    <span id="span_pt_<%#Eval("RecID").ToString() %>">
                        <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ProcessTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>
                    </span>&nbsp;
                </td> 
                <td name='Col_处理状态'>                    
                    <span id="span3_<%#Eval("RecID").ToString() %>" style="display:inline-block">
                        <%# Eval("Status_NM")%>
                    </span>
                    <span id="span4_<%#Eval("RecID").ToString() %>" style="display:none">
                        <select id="select_status_nm_<%#Eval("RecID").ToString() %>" style="width:60px; display:inline-block">
                            <option value="0">待处理</option>
                            <option value="1">处理中</option>
                            <option value="2">已处理</option>
                        </select>
                    </span>
                </td> 
                <td name='Col_留言'>
                    <span style=" margin-top:-5px;">
                        <%# Eval("FileFullName").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("FileFullName").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' style='vertical-align: middle;margin-top:-3px;*margin-top:0px;'/></a>"%>
                    </span>
                     &nbsp;
                </td>
                <td name='Col_操作'>
                    <span id="span_link_<%#Eval("RecID").ToString() %>">
                        <%# GetLinkOper(Eval("Status_NM").ToString(), Eval("RecID").ToString(), Eval("CallNO").ToString(), Eval("WorkOrderDataSourceID").ToString(),
                        Eval("OrderVersion").ToString(), Eval("OrderID").ToString(), Eval("old_Category").ToString(), Eval("old_OrderStatus").ToString(),
                        Eval("new_OrderStatus").ToString(), Eval("new_LastRecid").ToString(), Eval("new_createuserid").ToString())%>
                    </span>&nbsp;
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<input id="input_page" type="hidden" value="<%=Page %>" />
<script type="text/javascript">
    //编辑状态
    function ModifyStatus(recid) {
        SetEditable(true, recid);
    }
    //保存
    function SaveStatus(recid) {
        var selectid = "select_status_nm_" + recid;
        var cur_status = $("#" + selectid).find("option:selected").val();
        $.post("/AjaxServers/CustomerCallin/Handler.ashx", { Action: "SaveStatus", RecID: recid, Status: cur_status, r: Math.random() }, function (data) {
            var jsondata = $.evalJSON(data);
            if (jsondata.success) {
                $.jPopMsgLayer("保存成功", function () {
                    RefreshCustID(recid, jsondata);
                    RefreshStatus(recid, jsondata);
                    RefreshOption(recid, jsondata);
                });
            }
            else {
                $.jAlert("保存失败");
            }
        });
    }
    //刷新客户号
    function RefreshCustID(recid, jsondata) {
        var custid = jsondata.custid;
        var tel = jsondata.tel;
        var span = "tr_" + recid;
        if (custid == "") {
            $("#" + span).html(tel);
        }
        else {
            $("#" + span).html("<a href='/TaskManager/CustInformation.aspx?CustID=" + custid + "' target='_blank'>" + tel + "</a><input type='hidden' name='CustID' value='" + custid + "'/>");
        }
    }
    //刷新状态（处理人，处理时间，处理状态）
    function RefreshStatus(recid, jsondata) {
        var span_pp = "span_pp_" + recid;
        var span_pt = "span_pt_" + recid;
        var span3 = "span3_" + recid;
        var span4 = "span4_" + recid;
        var selectid = "select_status_nm_" + recid;

        var cur_status = $.trim($("#" + selectid).find("option:selected").text());

        $("#" + span_pp).html(jsondata.processuser);
        $("#" + span_pt).html(jsondata.processtime);
        $("#" + span3).html(cur_status);
    }
    //刷新操作按钮
    function RefreshOption(recid, jsondata) {
        //退出编辑状态        
        SetEditable(false, recid);
        //已处理 特殊处理        
        var selectid = "select_status_nm_" + recid;
        var cur_status = $.trim($("#" + selectid).find("option:selected").text());
        if (cur_status == "已处理") {
            var span_ck = "span_ck_" + recid;
            if ($("#" + span_ck)[0]) {
                var orderid = $("#" + span_ck).attr("orderid");
                if (orderid != "" && orderid != undefined) {
                    $("#" + span_ck).html("<a href='/WorkOrder/WorkOrderView.aspx?OrderID=" + orderid + "' target='_blank'>查看</a>&nbsp;&nbsp;");
                }
            }

            var span_cl = "span_cl_" + recid;
            if ($("#" + span_cl)[0]) {
                $("#" + span_cl).html("");
                $("#" + span_cl).css("display", "none");
            }
            var span1 = "span1_" + recid;
            if ($("#" + span1)[0]) {
                $("#" + span1).html("");
                $("#" + span1).css("display", "none");
            }
            var span2 = "span2_" + recid;
            if ($("#" + span2)[0]) {
                $("#" + span2).html("");
                $("#" + span2).css("display", "none");
            }
        }
    }
    //取消
    function CancelStatus(recid) {
        SetEditable(false, recid);
    }
    //设置编辑状态
    function SetEditable(b, recid) {
        var span1 = "span1_" + recid;
        var span2 = "span2_" + recid;
        var span3 = "span3_" + recid;
        var span4 = "span4_" + recid;
        if (b) {
            $("#" + span1).css("display", "none");
            $("#" + span2).css("display", "inline-block");
            $("#" + span3).css("display", "none");
            $("#" + span4).css("display", "inline-block");

            SetSelected(recid);
        }
        else {
            $("#" + span2).css("display", "none");
            $("#" + span1).css("display", "inline-block");
            $("#" + span4).css("display", "none");
            $("#" + span3).css("display", "inline-block");
        }
    }
    //设置选择的状态
    function SetSelected(recid) {
        var span3 = "span3_" + recid;
        var selectid = "select_status_nm_" + recid;
        //设置默认选择的状态
        var text = $.trim($("#" + span3).text());
        $("#" + selectid).find("option").each(function (i, n) {
            if (n.text == text) {
                n.selected = true;
            }
            else {
                n.selected = false;
            }
        });
    }
</script>
