<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnVisitRecordList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.ReturnVisitRecordList" %>

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

    $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
    SetTableStyle('tableReturnVisitCust');
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divList').load('/AjaxServers/ReturnVisit/ReturnVisitRecordList.aspx #divList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableReturnVisitCust');
    }    
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
        <tr class="color_hui">
            <th width="10%">
                <strong>回访ID</strong>
            </th>
            <th width="19%">
                <strong>客户名称</strong>
            </th>
            <th width="10%">
                <strong>客户联系人</strong>
            </th>
            <th width="10%">
                <strong>联系电话</strong>
            </th>
            <th width="15%">
                <strong>访问时间</strong>
            </th>
            <th width="10%">
                <strong>访问分类</strong>
            </th>
            <th width="12%">
                <strong>访问人</strong>
            </th>
            <th width="9%">
                <strong>操作</strong>
            </th>
        </tr>
        <asp:repeater runat="server" id="rplist">
         <ItemTemplate>
                            <tr>
                           <td align="center">
                                    <%#Eval("RID")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# Eval("CustName")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%# Eval("CName")%>&nbsp;
                                </td>
                               <td align="center">
                                    <%#Eval("phonenum")%>&nbsp;
                                </td>                               
                                <td align="center">
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("begintime").ToString())%>&nbsp;
                                </td>
                                <td align="center">
                                    <%#Eval("DictName").ToString()%>
                                </td>
                                <td align="center">
                                    <%#Eval("truename")%>&nbsp;
                                </td>
                                <td align="center">
                                <a onclick='void(0)' target='_blank' class="linkBlue" href='/ReturnVisit/ReturnVisitRecordView.aspx?RVID=<%# Eval("RID").ToString()%>'>查看</a>
                                    &nbsp;
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
            <asp:literal runat="server" id="litPagerDown1"></asp:literal>
        </p>
    </div>
</div>
