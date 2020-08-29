<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrailList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.TrailList" %>

<div class="bit_table">
    <!--列表开始-->
    <div class="faqList">
        <table border="0" cellspacing="0" cellpadding="0" id="tab">
            <thead>
                <th colspan="12" style="background: #F9F9F9">
                    <div class="btn btn2 right">
                        <input type="button" value="导出" onclick="ExportData()" class="save w60 gray" /></div>
                </th>
            </thead>
            <tr>
                <th width="12%">
                    日期
                </th>
                <th width="10%">
                    客服
                </th>
                <th>
                    工号
                </th>
                <th>
                    总对话量
                </th>
                <th>
                    总对话时长
                </th>
                <th>
                    平均对话时长
                </th>
                <th>
                    总在线时长
                </th>
                <th>
                    客服消息发送量
                </th>
                <th>
                    访客消息发送量
                </th>
                <th>
                    对话转出次数
                </th>
                <th>
                    对话转入次数
                </th>
            </tr>
            <asp:repeater runat="server" id="rpeList">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("statDate").ToString()==""?"合计（共"+RecordCount.ToString()+"项）":Convert.ToDateTime(Eval("statDate").ToString()).ToString("yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("UserName").ToString() == "" ? "--" : Eval("UserName")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("AgentNum").ToString() == "" ? "--" : Eval("AgentNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("allCount")%>&nbsp;
                        </td>
                        <td>  
                            <%#ConvertDate(int.Parse(Eval("allConver").ToString() == "" ? "0" : Eval("allConver").ToString()))%>&nbsp;
                        </td>
                        <td>
                         <%#GetAvgConver(Eval("allCount").ToString(), Eval("allConver").ToString())%>&nbsp;
                        </td>
                       <td>
                            <%#ConvertDate(int.Parse(Eval("timelong").ToString() == "" ? "0" : Eval("timelong").ToString()))%>&nbsp;
                        </td>
                        <td> <%#Eval("Ccount").ToString() == "" ? "0" : Eval("Ccount")%>&nbsp;
                        </td>
                        <td> <%#Eval("Pcount").ToString() == "" ? "0" : Eval("Pcount")%>&nbsp;
                        </td>
                        <td> <%#Eval("turmOutCount")%>&nbsp;
                        </td>
                        <td>
                           <%#Eval("turmInCount")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
            <tr>
                <td colspan="12">
                    <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                        <p>
                            <asp:literal runat="server" id="litPagerDown"></asp:literal>
                        </p>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!--列表结束-->
    <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
</div>
<style type="text/css">
    .hzTr
    {
    }
    .hzTr td
    {
        font-weight: bolder;
    }
</style>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $("#tab tr").last().prev().addClass("hzTr");
    });

</script>
