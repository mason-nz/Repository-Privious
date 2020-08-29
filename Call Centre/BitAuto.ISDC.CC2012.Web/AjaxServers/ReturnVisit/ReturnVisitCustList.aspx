<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnVisitCustList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit.ReturnVisitCustList" %>

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


    function OpenCustTagPop(custID) {
        $.openPopupLayer({
            name: "CustTagPopLayer",
            parameters: { CustID: custID, UserID: '<%=userID%>', r: Math.random() },
            popupMethod: 'post',
            url: "/AjaxServers/ProjectManage/CustTagPop.aspx"
        });
    }

  
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
        <tr class="color_hui">
            <th style="width: 5%">
                <input id="ckbAllSelect" type="checkbox" name="selItem" />
            </th>
            <th width="18%">
                <strong>客户名称</strong>
            </th>
            <th width="14%">
                <strong>主营品牌</strong>
            </th>
            <th width="14%">
                <strong>客户地区</strong>
            </th>
            <th width="6.5%">
                <strong>访问记录</strong>
            </th>
            <th width="6.5%">
                <strong>工单记录</strong>
            </th>
            <th width="9%">
                <strong>最近访问日期</strong>
            </th>
            <th width="9%">
                <strong>下次回访日期</strong>
            </th>
            <th width="10%">
                <strong>负责坐席</strong>
            </th>
            <th width="8%" style="padding-left: 21px;">
                <strong>操作</strong>
            </th>
        </tr>
        <asp:repeater id="repeaterList" runat="server">
                        <ItemTemplate>
                            <tr>
                            <td>                       
                           <input name="chkSelect"  type="checkbox" value='<%#Eval("Custid").ToString()%>'  />
                        </td>
                                <td align="center">
                                    <%# Eval("CustName")%>&nbsp;
                                </td>
                                <td align="center">
                                    <%#getBrandNames(Eval("Custid").ToString())%>&nbsp;
                                </td>
                               <td align="center">
                                    <%# GetAreaName(Eval("ProvinceID").ToString(), Eval("cityID").ToString(), Eval("CountyID").ToString())%>&nbsp;
                                </td>
                                <td align="center">                                
                                    <%# Eval("ReturnVisitCount").ToString() == "0" ? "" : ("<a class='changASty' href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + Eval("Custid").ToString() + "&i=4' target='_blank'>" + Eval("ReturnVisitCount").ToString() + "</a>")%>&nbsp;
                                </td>
                                <td align="center">                                
                                    <%# Eval("WorkOrderCount").ToString() == "0" ? "" : ("<a class='changASty' href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + Eval("Custid").ToString() + "&i=9' target='_blank'>" + Eval("WorkOrderCount").ToString() + "</a>")%>&nbsp;
                                </td>
                                <td align="center">                                    
                                    <%# getLastVisitTime(Eval("LastTime").ToString(), Eval("LastTimeW").ToString())%>&nbsp;                                    
                                </td>                               
                                 <td align="center" class="backDt">                                    
                                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("NextVisitDate").ToString(), "yyyy-MM-dd")%>&nbsp;
                                </td>                                
                                <td align="center" title='<%#Eval("theCustomNames").ToString()%>'>
                                    <%#Eval("theCustomNames").ToString()%>&nbsp;                                    
                                </td>
                                <td align="left">
                                    <a  href="javascript:void(0)" onclick='OpenCustTagPop(<%#Eval("Custid").ToString()%>)'>分类</a>                                   
                                     <%#IsMine(Eval("Custid").ToString(), Eval("theCustomNames").ToString()) == true ?
                                     "<a style='padding-left:6px;' href='CustInfoShow.aspx?custid=" + Eval("Custid") + "' target='_blank'>回访</a>" : ""%>
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
            <input type="hidden" id="totalCountNum" value="<%=RecordCount%>" />
        </p>
    </div>
    <input type="hidden" id="nowDt" runat="server" />
</div>
