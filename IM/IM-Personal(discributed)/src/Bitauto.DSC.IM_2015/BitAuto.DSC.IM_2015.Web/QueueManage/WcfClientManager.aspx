<%@ Page Title="对象监控" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="WcfClientManager.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.QueueManage.WcfClientManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">
        function loadData() {
            //LoadingAnimation("divcontent");
            $('#divcontent').load("/AjaxServers/QueueManage/WcfClientManagerList.aspx?r=" + Math.random(), function () {
            });
        };
        function DeleteAgent(agentid) {
            $.jConfirm("您确定要删除该对象吗？", function () {
                var pody = { action: 'deletewcfagent', AgentID: escape(agentid) };
                AjaxPost('../AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 if (msg == "ok") {
                     $.jAlert("删除成功！", function () {
                         loadData();
                     });
                 }

             });
            });

        }
        $(function () {
            loadData();
        });
        
    </script>
    <div class="content">
        <!--列表开始-->
        <div class="cxList" style="margin-top: 20px;" id="divcontent">
            <div class="clearfix">
            </div>
        </div>
</asp:Content>
