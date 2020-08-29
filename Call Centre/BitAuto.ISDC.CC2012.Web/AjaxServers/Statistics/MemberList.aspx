<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Statistics.MemberList" %>

<script> 
    //...add by qizhiqiang 2012-4-13 判断是统计中的会员查询还是客户核实中的会员查询
    <%if(RequestComeExcel=="1"){%>
      $('#comeExcel').show();
    <%}else{%>
      $('#comeExcel').hide();
    <%}%>
    //...
    BindExportExcel();
    function BindExportExcel() {
        var url = 'MemberContactExoprtList.aspx?Browser=' + GetBrowserName();
        url += '&DMSMemberCode=' + escape('<%=RequestDMSMemberCode %>');
        url += '&DMSMemberName=' + escape('<%=RequestDMSMemberName %>');
        url += '&BrandIDs=' + escape('<%=RequestBrandIDs %>');
        url += '&ProvinceID=' + escape('<%=RequestProvinceID %>');
        url += '&CityID=' + escape('<%=RequestCityID %>');
        url += '&CountyID=' + escape('<%=RequestCountyID %>');
        url += '&AreaTypeIDs=' + escape('<%=RequestAreaTypeIDs %>');
        url += '&MemberCooperateStatus=' + escape('<%=RequestMemberCooperateStatus %>');
        url += '&CooperatedStatusIDs=' + escape('<%=CooperatedStatusIDs %>');
        url += '&BeginMemberCooperatedTime=' + escape('<%=RequestBeginMemberCooperatedTime %>');
        url += '&EndMemberCooperatedTime=' + escape('<%=RequestEndMemberCooperatedTime %>');
        url += '&BeginNoMemberCooperatedTime=' + escape('<%=RequestBeginNoMemberCooperatedTime %>');
        url += '&EndNoMemberCooperatedTime=' + escape('<%=RequestEndNoMemberCooperatedTime %>');
        url += '&MemberTypeIDs=' + '<%=RequestMemberTypeIDs %>';
        url += '&ContactOfficeTypeCode=' + '<%=RequestContactOfficeTypeCode %>';
        url += '&IsReturnMagazine=' + '<%=RequestIsReturnMagazine %>';
        url += '&ExecCycle=' + '<%=RequestExecCycle %>';
        url += '&MemberSyncStatus=' + '<%=RequestMemberSyncStatus %>';
        url += '&selectDeptID=' + '<%=SelectDeptID %>';
        url += '&strDeptS=' + '<%=StrDeptS %>';

        url += '&MemberCreateTimeStart=' + '<%=RequestMemberCreateTimeStart %>';
        url += '&MemberCreateTimeEnd=' + '<%=RequestMemberCreateTimeEnd %>';
        $('#lbtnContactExport').click(function () { window.location = url; });
    }
</script>
<form id="form1" runat="server">
<div class="optionBtn  clearfix">
    <div>
        <%if (right_Contact)
          { %>
        <input name="" type="button" value="联系人导出" id="lbtnContactExport" class="newBtn mr10" />
        <%} %>
        <input name="" type="button" value="客户ID导出" onclick="openUploadExcelInfoAjaxPopupForExportCustID()"
            class="newBtn mr10" />
        <%if (right_Member)
          { %>
        <input name="" type="button" value="合作会员导出" onclick="openUploadExcelInfoAjaxPopup()"
            class="newBtn mr10" />
        <%} %>
        <%if (right_Magazine)
          { %>
        <input name="" type="button" value="杂志回访导入" onclick="openMagazineExcelInfoAjaxPopup()"
            class="newBtn mr10" />
        <%} %>
        <span>查询结果 </span><small><span>总计:
            <%= this.CountOfRecords.ToString() %></span></small>
    </div>
</div>
<div class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableCSTMember">
        <tr>
            <th>
                会员ID
            </th>
            <th>
                会员名称
            </th>
            <th>
                所属客户
            </th>
            <% if (RequestShowDMSMemberPart.ToLower().Equals("yes"))
               {%>
            <th>
                客户ID
            </th>
            <th>
                会员状态
            </th>
            <th>
                销售地址
            </th>
            <%}
               else
               { %>
            <th>
                负责部门
            </th>
            <th style="display: none">
                负责员工
            </th>
            <th>
                会员类型
            </th>
            <th>
                销售类型（当前）
            </th>
            <th>
                执行周期
            </th>
            <%} %>
        </tr>
        <asp:repeater id="Repeater_DMSMember" runat="server">
        <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td align="center" MemberCode='<%#Eval("MemberCode") %>'>
                    <a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=<%#Eval("ID")%>&CustID=<%#Eval("CustID")%>' target="_blank"><%#Eval("MemberCode")%></a>&nbsp;
                </td>                 
                <td align="center">
                 <%#Eval("MemberCode").ToString() != "" ? Eval("Name") : "  <a href='/CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=" + Eval("ID") + "&CustID=" + Eval("CustID") + "' target='_blank'>" + Eval("Name") + "</a>&nbsp;"%> &nbsp;
                </td>
                <td align="center" custid='<%#Eval("CustID")%>'>
                    <a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=<%#Eval("CustID")%>' target="_blank"><%#Eval("CustName")%></a>&nbsp;
                </td>
                 <% if (RequestShowDMSMemberPart.ToLower().Equals("yes"))
                    {%>
                <td align="center">
                    <%#Eval("CustID") %>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("Status").ToString()=="0"?"正常":"删除"%>&nbsp;
                </td>
                <td align="center">
                    <%#Eval("ContactAddress").ToString()%>&nbsp;
                </td>
                <%}
                    else
                    { %>
                <td> 
                  <a href="javascript:void(0)" style="text-decoration:none;color:#333333;cursor:inherit;" title="<%# Eval("titleCustName").ToString()%>"><%#Eval("showCustName").ToString()%>
                  </a> &nbsp;
                </td>
                <td style="display:none">
                </td>
                <td>
                   <%#Eval("MemberLevelName").ToString() %>&nbsp;
                </td>
                <td>
                   <%# GetMemberCooperateStatus(Eval("CooperateStatus"))%>&nbsp;
                </td>
                <td>
                   <%#Eval("MemberRecordTime").ToString()%>&nbsp;
                </td>
                <%} %>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    </table>
    <div class="pages1" style="text-align: right;">
        <uc:AjaxPager ID="AjaxPager_DMSMember" runat="server" />
    </div>
</div>
</form>
