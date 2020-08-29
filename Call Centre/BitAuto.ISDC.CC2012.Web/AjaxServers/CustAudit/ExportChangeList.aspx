<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportChangeList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustAudit.ExportChangeList" %>

<script>
    $('#tableExportChanged tr:even').addClass('color_hui'); //设置列表行样式
    SetTableStyle('tableExportChanged');
    //导出Excel信息
    function ExportExcel() {
        //        var txtCreateBeginTime = $.trim($('#txtCreateBeginTime').val());
        //        var txtCreateEndTime = $.trim($('#txtCreateEndTime').val());
        //        var txtCustIDORMemberID = $.trim($('#txtCustIDORMemberID').val());
        //        var txtCustNameORMemberName = $.trim($('#txtCustNameORMemberName').val());
        //var ddlExportStatus = $.trim($('#ddlExportStatus').val());
        var ddlContrastType = $.trim($('#ddlContrastType').val());
        var para = 'Export=yes&CreateBeginTime=' + encodeURIComponent('<%=RequestCreateBeginTime %>') +
                   '&CreateEndTime=' + encodeURIComponent('<%=RequestCreateEndTime %>') +
                   '&CustIDORMemberID=' + encodeURIComponent('<%=RequestCustIDORMemberID %>') +
                   '&CustNameORMemberName=' + encodeURIComponent('<%=RequestCustNameORMemberName %>') +
                   '&ContrastType=<%=RequestContrastType %>' +
                   '&DisposeStatus=<%=RequestDisposeStatus %>' +
                   '&SeatTrueName=<%=RequestSeatTrueName %>' +

                   '&MemberProvince=<%=RequestMemberProvinceID %>' +
                   '&MemberCity=<%=RequestMemberCityID %>' +
                   '&MemberCounty=<%=RequestMemberCountyID %>' +

                   '&TaskBatch=<%=RequestTaskBatch %>' +
                   '&Browser=' + GetBrowserName();
        window.location = '/CustAudit/ExportList.aspx?' + para;
    }
    
</script>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableExportChanged">
        <tr>
            <%--<th width="3%">
        </th>--%>
            <th width="8%">
                ID
            </th>
            <th width="13%">
                名称
            </th>
            <th width="*">
                变更详情
            </th>
            <th width="15%">
                变更类型
            </th>
            <th width="6%">
                处理状态
            </th>
            <th width="10%">
                处理时间
            </th>
            <th width="10%">
                备注
            </th>
            <th width="10%">
                创建时间
            </th>
        </tr>
        <asp:repeater id="Repeater_ExportChanged" runat="server">
        <ItemTemplate>
            <tr>  
                <%--<td align="center" >
                    <input id='checkBoxAll' name='checkBoxAll' type="checkbox" value='<%#Eval("RecID") %>' />
                </td> --%>                
                <td align="center">
                    <%#(Eval("StatID").ToString() == "0" || Eval("StatID").ToString() == "-2") ? string.Empty : Eval("StatID")%>&nbsp;
                </td>                
                <td align="center">
                    <%#Eval("StatName") %>&nbsp;
                </td>
                <td align="left" style="text-align:left;">
                    <label title='<%#Eval("ContrastInfo")%>' >
                   <%# BitAuto.Utils.StringHelper.SubString(Eval("ContrastInfo").ToString().Replace("<", "&lt;").Replace(">", "&gt;"), 75, true)%></label>&nbsp;
                </td>
                <td>
                    <%# GetContrastTypeName(Eval("ContrastType"))%>&nbsp;
                </td>
                <td>
                 <%# GetDisposeStatusName(Eval("DisposeStatus").ToString())%>&nbsp;
                </td>
                 <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("DisposeTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# Eval("Remark")%>&nbsp;
                </td>
               <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
               </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    </table>
    <div class="clear">
    </div>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
