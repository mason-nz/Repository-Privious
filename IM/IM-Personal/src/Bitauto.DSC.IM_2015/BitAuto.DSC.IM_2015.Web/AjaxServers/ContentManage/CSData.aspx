<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSData.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage.CSData" %>

      <script type="text/javascript">
          $(function () {
              //鼠标移入该行和鼠标移除该行的事件
              jQuery("#ajaxTable tr:gt(0)").mouseover(function () {
                  jQuery(this).addClass("trover");
              }).mouseout(function () {
                  jQuery(this).removeClass("trover");
              });
              //鼠标点击事件
//              $("#ajaxTable tr:gt(0)").bind("click", function () {
//                  $("#ajaxTable tr:gt(0)").removeClass("trclick");
//                  $(this).addClass("trclick");
//              });
          });
          //分页操作 
          function ShowDataByPost2(pody) {
              LoadingAnimation("ajaxTable");
              $('#ajaxTable').load("/AjaxServers/ContentManage/CSData.aspx .bit_table > *", pody, function () {
                  //鼠标移入该行和鼠标移除该行的事件
                  jQuery("#ajaxTable tr:gt(0)").mouseover(function () {
                      jQuery(this).addClass("trover");
                  }).mouseout(function () {
                      jQuery(this).removeClass("trover");
                  });
                  //鼠标点击事件
                  $("#ajaxTable tr:gt(0)").bind("click", function () {
                      $("#ajaxTable tr:gt(0)").removeClass("trclick");
                      $(this).addClass("trclick");
                  });
                  $("#ajaxTable tr:eq(1)").click();
              });
          }
</script>
<div class="bit_table">
    <!--列表开始-->
    <div class="faqList">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <th width="17%">
                    访客姓名
                </th>
                <th width="13%">
                    电话
                </th>
                <th width="18%">
                    地区
                </th>
                <th width="20%">
                    最后对话时间
                </th>
                <th width="13%">
                    对话时长
                </th>
                <th width="19%" style=" border-right:0px;">
                    客服
                </th>
                <th style="display: none;">
                    隐藏域
                </th>
            </tr>
            <asp:repeater runat="server" id="Rt_CSData">
                    <ItemTemplate>
                          <tr>
                            <td class="cName" style=" padding-left:10px; padding-right:0px;" title="<%#Eval("VisitorName")%>">
                                <%#Eval("VisitorName") == null ? "" :( Eval("VisitorName").ToString().Length>10 ? Eval("VisitorName").ToString().Substring(0,10) + "…" :Eval("VisitorName"))%>&nbsp;
                            </td>
                            <td>
                                <%#Eval("VisitorPhone")%>&nbsp;
                            </td>
                            <td title="<%#Eval("ProvinceCityName")%>">
                                 <%#Eval("ProvinceCityName") == null ? "" : (Eval("ProvinceCityName").ToString().Length > 8 ? Eval("ProvinceCityName").ToString().Substring(0, 8) + "…" : Eval("ProvinceCityName"))%>&nbsp;
                            </td>
                            <td>
                                <%# Eval("EndTime")%>&nbsp;
                            </td>
                            <td>             
                                <%#Eval("Duration")%>秒
                            </td>
                            <td style=" border-right:0px;" title="<%#Eval("AgentName")%>">
                                <%#Eval("AgentName") == null ? "" : (Eval("AgentName").ToString().Length > 8 ? Eval("AgentName").ToString().Substring(0, 8) + "…" : Eval("AgentName"))%>&nbsp;
                            </td>
                            <td class="hidtdinfo" style=" display:none;">
                            <%#Eval("CSID")%>,<%#Eval("CustID")%>,<%#Eval("VisitID")%>,<%#Eval("OrderID")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                 </asp:repeater>
        </table>
        <div class="pagesnew" style="float: none; margin: 10px; text-align:center;"  id="itPage">
                        <p>
                            <asp:literal runat="server" id="litPagerDown"></asp:literal>
                        </p>
                    </div>
    </div>
    <input type="hidden" value="<%=RecordCount %>" id="hidTotalCount" />
</div>

