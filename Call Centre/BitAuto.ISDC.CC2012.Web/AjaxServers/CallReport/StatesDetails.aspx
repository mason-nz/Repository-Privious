<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatesDetails.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport.StatesDetails" %>

<script type="text/javascript">
    $(document).ready(function () {

        var pageSize = '<%=PageSize %>';
        $("a[name='apageSize'][v='" + pageSize + "']").addClass("selectA");

        $("a[name='apageSize']").bind("click", function (e) {
            e.preventDefault();
            $("a[name='apageSize']").removeClass("selectA");
            $(this).addClass("selectA");

            $("#hidSelectPageSize").val($(this).attr("v"));
            search();
        });

        var RecordCount = '<%=RecordCount %>';
        if (RecordCount < 20) {
            $(".pageP").hide();
        }
    });

    $('#tableAgentDetail tr:even').addClass('color_hui'); //设置列表行样式
    SetTableStyle('tableAgentDetail');


    function ShowDataByPost1(pody) {
        //AjaxServers/CallReport/StatesDetails.aspx?pagesize=
        //        $('#divList').load('/AjaxServers/ReturnVisit/ReturnVisitRecordList.aspx #divList > *', pody, LoadDivSuccess);
        $('#divList').load('/AjaxServers/CallReport/StatesDetails.aspx', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableAgentDetail tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableAgentDetail');
    }

  
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableAgentDetail">
        <tr class="color_hui">
            <th width="10%">
                <strong>日期</strong>
            </th>
            <th width="8%">
                <strong>所属分组</strong>
            </th>
            <th width="8%">
                <strong>客服</strong>
            </th>
            <th width="8%">
                <strong>工号</strong>
            </th>
            <th width="8%">
                <strong>状态</strong>
            </th>
            <th width="8%">
                <strong>辅助状态</strong>
            </th>
            <th width="12%">
                <strong>状态开始时间</strong>
            </th>
            <th width="12%">
                <strong>状态结束时间</strong>
            </th>
            <th width="10%">
                <strong>持续时长</strong>
            </th>
        </tr>
        <asp:repeater id="repeaterList" runat="server">
                        <ItemTemplate>
                            <tr>
                            
                                <td align="center">
                                    <%# Eval("rq")%>&nbsp;
                                </td>
                                <td align="center">
                                  <%#Eval("usedGroup")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%#Eval("TrueName")%>&nbsp;
                                </td>
                               <td align="center">
                                    <%#Eval("AgentNum")%>&nbsp;
                                </td>
                               
                                <td align="center">                                
                                     <%#Eval("State")%>&nbsp;
                                </td>
                                <td align="center">                                
                                     <%#Eval("auxState")%>&nbsp;
                                </td>
                                <td align="center">                                    
                                      <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("startTime").ToString())%>&nbsp;
                                </td>                               
                                 <td align="center" class="backDt">                                    
                                      <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("endTime").ToString())%>&nbsp;
                                </td>
                               
                                <td align="center">
                                      <%#Eval("dur")%>&nbsp;
                                </td>
                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p class="pageP" style="padding: 0 10px; float: left;">
            每页显示条数 <a href="#" name="apageSize" v='20'>20</a>&nbsp;&nbsp; <a href="#" name="apageSize"
                v='50'>50</a>&nbsp;&nbsp; <a href="#" name="apageSize" v='100'>100</a>
        </p>
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
    <input type="hidden" id="nowDt" runat="server" />
</div>
