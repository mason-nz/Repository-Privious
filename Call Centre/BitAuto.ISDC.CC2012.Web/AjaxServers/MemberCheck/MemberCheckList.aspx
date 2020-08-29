<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberCheckList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.MemberCheck.MemberCheckList" %>

<script type="text/javascript"> 
    BindExportExcel();
    function Audit(AuditType, tid) {
        var url = "../AjaxServers/CustAudit/CustAuditManager.ashx";
        var TID = '';
        if (tid == 'all') {
            if (!validateEdit('checkBoxDelAll')) {
                return;
            }
            TID = $(':checkbox[name="checkBoxDelAll"][checked]').map(function () { return $(this).val(); }).get().join(",");
        }
        else {
            TID = tid;
        }
        var postBody = "Audit=yes&TID=" + TID + '&AuditType=' + AuditType;

        AjaxPost(url, postBody, null, SuccessPost);
        function SuccessPost(data) {
            var s = $.evalJSON(data);
            var update = unescape(s.Update);
            if (update == 'yes') {
                $.jPopMsgLayer('操作成功！', function () {
                    search($('#hidPageIndex').val()); //刷新当前页
                });
            }
            else {
                var info = "审核成功:" + s.aduitSuccessCount + "个<br/>" +
                           "审核失败:" + (~ ~s.aduitFaileCount + ~ ~s.otherAduitFaileCount) + "个<br/>" +
                           (s.verifyCount == 0 ? "" : "未审核:" + s.verifyCount + "个<br/>") +
                           (s.aduitFaileCount == 0 ? "" : "审核失败详情如下:<br/>" + unescape(s.aduitFaileDesc) + "<br/>") +
                           (s.otherAduitFaileCount == 0 ? "" : "其他审核失败详情如下:<br/>" + unescape(s.otherAduitFaileDesc) + "<br/>") +
                           (s.verifyCount == 0 ? "" : "未审核详情如下:<br/>" + unescape(s.verifyDesc) + "<br/>");
                var msg = update.replace('VerifyLogic,', '');
                if (tid == 'all') { //批量审核
                    msg = info;
                }
                $.jAlert(msg, function () {
                    search($('#hidPageIndex').val()); //刷新当前页
                });
            }
        }
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tablelist tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tablelist');
    }

    //设置每页记录数
    function setPageSize(pageSizeSelect) {
        memberCheckHelper.search({
            Page: '<%= BitAuto.ISDC.CC2012.Web.Util.PagerHelper.GetCurrentPage() %>',
            PageSize: $('#' + pageSizeSelect).val()
        });
    }

    function CheckAll() {
        $('#tablelist td input[type="checkbox"]:not(:disabled)').attr('checked', 'true');
    }

    function Inverse() {
        $('#tablelist td input[type="checkbox"]:not(:disabled)').each(function () {
            $(this).attr("checked", !this.checked);
        });
    }

    function UncheckAll() {
        $('#tablelist td input[type="checkbox"]:not(:disabled)').removeAttr('checked');
    }

    function BindExportExcel() {
        var url = 'MemberCheckExportList.aspx?Browser=' + GetBrowserName();
        url += '&MemberName=' + escape('<%=RequestMemberName %>');
        url += '&MemberAddr=' + escape('<%=RequestMemberAddr %>');
        url += '&CustName=' + escape('<%=RequestCustName %>');
        url += '&CustID=' + escape('<%=RequestCustID %>');
        url += '&ApplyStartTime=' + escape('<%=RequestApplyStartTime %>');
        url += '&ApplyEndTime=' + escape('<%=RequestApplyEndTime %>');
        url += '&ApplyUserName=' + escape('<%=RequestApplyUserName %>');
        url += '&MemberOptStartTime=' + escape('<%=RequestMemberOptStartTime %>');
        url += '&MemberOptEndTime=' + escape('<%=RequestMemberOptEndTime %>');
        url += '&DMSSyncStatus=' + escape('<%=RequestDMSSyncStatus %>');
        url += '&DMSStatus=' + escape('<%=RequestDMSStatus %>');
        url += '&Type=' + escape('<%=RequestType %>');
        $('#lbtnExportExcel').attr('href', url);
    }
</script>
<script type="text/javascript">
    $(function () { 
        var pageSize = '<%=PageSize %>'; 

        $("a[name='apageSize'][v='" + pageSize + "']").addClass("selectA");

        $("a[name='apageSize']").die("click").live("click", function (e) {
            e.preventDefault();
            $("a[name='apageSize']").removeClass("selectA");
            $(this).addClass("selectA");

            $("#hidSelectPageSize").val($(this).attr("v"));
            memberCheckHelper.search();
        });

        var RecordCount = '<%=RecordCount %>';
    });
</script>
<div class="optionBtn  clearfix" style="width: 97%">
    <div>
        <span>查询结果</span><small id="sml" runat="server"></small>
    </div>
</div>
<div class="bit_table" id="divList">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList"
        id="tablelist">
        <tr>
            <th width="9%">
                客户ID
            </th>
            <th width="13%">
                客户名称
            </th>
            <th width="13%">
                会员名称
            </th>
            <th width="10%">
                会员简称
            </th>
            <th width="8%">
                申请人
            </th>
            <th width="9%">
                申请时间
            </th>
            <th width="7%">
                申请状态
            </th>
            <th width="9%">
                审批时间
            </th>
            <th width="10%">
                驳回理由
            </th>
            <th width="6%">
                CRM会员状态
            </th>
            <th width="6%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterList" runat="server">
        <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                            <%# Eval("OriginalCustID")%>&nbsp;
                                </td>
                                <td>
                                       <%# Eval("OriginalCustID").ToString().Trim() != "" ? "<a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID="+Eval("OriginalCustID").ToString()+"' target='_blank'>"+Eval("CC_CustName").ToString()+"</a>" : Eval("CC_CustName").ToString()%>&nbsp;
                                </td>
                                <td>
                                     <%# Eval("CC_MemberName")%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("CC_MemberAbbr")%>&nbsp;
                                </td>
                                <td>
                                        <%# Eval("ApplyUserTrueName")%>&nbsp;
                                </td>
                                <td>
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ApplyTime").ToString())%>&nbsp;
                                </td> 
                                <td>
                                    <%# Eval("SyncStatus").ToString().Trim()!=""?BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptText(int.Parse(Eval("SyncStatus").ToString())):""%>&nbsp;
                                </td>     
                                <td>
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("SyncTime").ToString())%>&nbsp;
                                </td>
                                <td>
                                    <%# Eval("SyncDesc")%>&nbsp;
                                </td>
                                <td>
                               <%#  getStatus(Eval("Status").ToString())%>&nbsp;
                                </td>
                                <td>
                                <%# GetUrl(Eval("SyncStatus").ToString().Trim(), Eval("MemberID").ToString(), Eval("PTID").ToString(), Eval("Status").ToString())%>&nbsp;
                                    <%--<%# Eval("SyncStatus").ToString().Trim()==Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateSuccessful)||
                                        Eval("SyncStatus").ToString().Trim()==Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.CreateUnsuccessful)||
                                        Eval("SyncStatus").ToString().Trim() == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.ApplyFor) ? "<a href='/CustCheck/EditMemberDetail.aspx?TID=" + Eval("TID").ToString() + "&MemberID=" + Eval("ID").ToString() + "' target='_self'>查看</a>" :
                                                                                                                                                                                                                                                "<a href='/CustCheck/EditMemberDetail.aspx?TID=" + Eval("TID").ToString() + "&MemberID=" + Eval("ID").ToString() + "' target='_self'>编辑</a>"%>
--%>                                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    </table>
    <br />
    <div class="pages" style="text-align: right;">
        <p class="pageP">
            每页显示条数 <a href="#" name="apageSize" v='20'>20</a>&nbsp;&nbsp; <a href="#" name="apageSize"
                v='50'>50</a>&nbsp;&nbsp; <a href="#" name="apageSize" v='100'>100</a>
        </p>
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
        <%--<p>
            显示<span><select id="selectDown" name="selectDown" runat="server" onchange="javascript:setPageSize('selectDown');">
                <option value="10">10</option>
                <option value="20">20</option>
                <option value="30">30</option>
                <option value="50">50</option>
                <option value="100">100</option>
            </select></span>条
            <uc:AjaxPager ID="AjaxPager_Member" runat="server" />
        </p>--%>
    </div>
</div>
