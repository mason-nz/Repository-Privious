<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax.AjaxList" %>

<div class="bit_table" id="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="2%">
                <input type="checkbox" id="cb_all" onclick="CheckALL()" />
            </th>
            <th width="8%">
                日期
            </th>
            <th width="6%">
                客服
            </th>
            <th width="5%">
                工号
            </th>
            <th width="15%">
                所属分组
            </th>
            <th width="5%">
                区域
            </th>
            <th width="6%">
                在线状态
            </th>
            <th width="6%">
                厂家版本
            </th>
            <th width="6%">
                分机号
            </th>
            <th width="6%">
                日志状态
            </th>
            <th width="6%">
                请求人
            </th>
            <th width="13%">
                请求时间
            </th>
            <th width="6%">
                响应结果
            </th>
            <th width="*">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"> 
                        <td>
                            <input type="checkbox" name="cb_one" onclick="CheckOne();" data-values='<%#Eval("log_date").ToString() %>,<%#Eval("UserID").ToString() %>,<%#Eval("VendorID").ToString() %>'/>
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToDateTime(Eval("log_date")).ToString("yyyy-MM-dd")%>&nbsp;
                        </td>             
                        <td>
                            <%#Eval("AgentName")%>&nbsp;
                        </td>                 
                        <td>
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>                
                       <td>
                            <%#Eval("BGName")%>&nbsp;        
                        </td>
                       <td>
                            <%#Eval("RegionName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("On_line")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("VendorName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ExtensionNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Status").ToString() == "0" ? "--" : (Eval("Status").ToString() == "1" ? "请求中" : "已响应")%>&nbsp;
                        </td>    
                        <td>
                           <%#Eval("RequireName")%>&nbsp;
                        </td>     
                        <td>
                            <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("RequireDateTime").ToString(), "yyyy-MM-dd HH:mm:ss")%>&nbsp;
                        </td>
                         <td>
                            <span title="<%#Eval("ResponseRemark").ToString() %>">
                            <%#Eval("ResponseSuccess").ToString() == "0" ? "--" : (Eval("ResponseSuccess").ToString() == "1" ? "成功" : "失败")%>&nbsp;
                            </span>
                        </td>     
                        <td>
                            <%#GetOper(Eval("log_date").ToString(), 
                            Eval("UserID").ToString(), 
                            Eval("VendorID").ToString(), 
                            Eval("Status").ToString(), 
                            Eval("ResponseSuccess").ToString(),
                            Eval("FilePath").ToString(),
                            Eval("AgentName").ToString(), 
                            Eval("VendorName").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
    </div>
</div>
<script type="text/javascript">
    //请求日志
    function RequireLog(logdate, userid, vendorid) {
        var body = {
            Action: "RequireLog",
            LogDate: logdate,
            AgentID: userid,
            VendorID: vendorid,
            NoLogin: "<%=NoLogin %>",
            r: Math.random()
        };
        $.post("/ClientLogRequire/Ajax/LogRequireHandler.ashx", body, function (data) {
            var jsondata = $.evalJSON(data);
            if (jsondata.success) {
                $.jPopMsgLayer("请求成功", function () {
                    search('<%=PageIndex %>');
                });
            }
            else {
                $.jAlert("请求失败");
            }
        });
    }
    //全选
    function CheckALL() {
        $("input[name='cb_one']").each(function (i, n) {
            n.checked = $("#cb_all")[0].checked;
        });
    }
    //单个选择
    function CheckOne() {
        var a = $("input[name='cb_one']").length;
        var b = $("input[name='cb_one']:checked").length;
        $("#cb_all")[0].checked = a == b;
    }
    //批量请求
    function RequireLogMutil() {
        var b = $("input[name='cb_one']:checked").length;
        if (b == 0) {
            $.jAlert("请至少选择一项！");
            return;
        }
        //获取数据
        var datas = "";
        $("input[name='cb_one']:checked").each(function (i, n) {
            datas += $(n).attr("data-values") + ";";
        });
        //请求数据
        var body = {
            Action: "RequireLogMutil",
            Datas: datas,
            NoLogin: "<%=NoLogin %>",
            r: Math.random()
        };
        $.post("/ClientLogRequire/Ajax/LogRequireHandler.ashx", body, function (data) {
            var jsondata = $.evalJSON(data);
            if (jsondata.success) {
                $.jPopMsgLayer(jsondata.success_msg, function () {
                    search('<%=PageIndex %>');
                });
            }
            else {
                if (jsondata.success_msg != "") {
                    $.jPopMsgLayer(jsondata.success_msg, null);
                }
                else {
                    $.jAlert("请求失败");
                }
            }
        });
    }
    //下载文件
    function DownLoadFile(a, filepath, agentname, vendorname) {
        //请求数据
        var body = {
            Action: "DownLoadFile",
            filepath: filepath,
            agentname: agentname,
            vendorname: vendorname,
            NoLogin: "<%=NoLogin %>",
            r: Math.random()
        };
        AjaxPostAsync("/ClientLogRequire/Ajax/LogRequireHandler.ashx", body, null, function (data) {
            var jsondata = $.evalJSON(data);
            if (jsondata.success) {
                a.href = filepath;
                return true;
            }
            else {
                a.href = "javascript:void(0)";
                if (jsondata.msg != "") {
                    $.jPopMsgLayer(jsondata.msg, null);
                }
                else {
                    $.jAlert("下载失败");
                }
                return false;
            }
        });
    }
</script>
