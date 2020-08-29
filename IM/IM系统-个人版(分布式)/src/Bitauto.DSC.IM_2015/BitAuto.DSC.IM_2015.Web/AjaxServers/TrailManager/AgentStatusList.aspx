<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentStatusList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager.AgentStatusList" %>

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
                <th width="16%">
                    日期
                </th>
                <th width="20%">
                    所属分组
                </th>
                <th width="10%">
                    客服
                </th>
                <th width="10%">
                    工号
                </th>
                <th width="10%">
                    状态
                </th>
                <th width="12%">
                    状态开始时间
                </th>
                <th width="12%">
                    状态结束时间
                </th>
                <th width="12%">
                    状态持续时长
                </th>
            </tr>
            <asp:repeater runat="server" id="rpeList">
                <ItemTemplate>
                    <tr>
                        <td>
                           <%#Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd")%>&nbsp;
                        </td>
                         <td>
                           <%#Eval("BGName")%>&nbsp;
                        </td>
                        <td>
                           <%# BitAuto.DSC.IM_2015.BLL.Util.SubTrueName(Eval("TrueName").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td> 
                          <%#GetStatusText(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>  
                         <%#Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd HH:mm:ss")%>&nbsp;
                        </td>
                        <td> 
                         <%#Eval("EndTime").ToString()==""?"":Convert.ToDateTime(Eval("EndTime")).ToString("yyyy-MM-dd HH:mm:ss")%>&nbsp;
                        </td>
                        <td>
                           <%#GetTimeLongText(Eval("TimeLong").ToString())%>&nbsp;
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
