<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CSHistoryData.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage.CSHistoryData" %>


<script type="text/javascript">
    $(function () {
        $(".pagesnew input").css("width", "10px");
    })
</script>
    <asp:repeater runat="server" id="Rt_CSHistoryData">
        <ItemTemplate>
            <div class="dh1">
                <div class="title">
                  <%#Eval("newName").ToString()%>
                </div>
                <div class="dhc" style="word-break:break-all;">
                    <%#Eval("Content").ToString()%>
                </div>
            </div>
        </ItemTemplate>
    </asp:repeater>
    <br />
    <!--分页-->
    <div class="pageTurn mr10" style=" width:450px;">
        <div class="pagesnew" style="float: none; text-align:center; margin:0px auto;" id="itPage">
           <p>
            <asp:literal runat="server" id="litPagerDown_History"></asp:literal>
           </p>
        </div>
    </div>
    <input type="hidden" value="<%=RecordCount %>" id="hidHistoryTotalCount" />
