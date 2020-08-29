<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueueList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.QueueList"
    MasterPageFile="~/Controls/Top.Master" Title="排队中" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">



        function loadData() {
            //LoadingAnimation("divcontent");
            $('#divcontent').load("/AjaxServers/QueueManage/QueueList.aspx?r="+Math.random(), function () {
                setTimeout(loadData, 3000);
            });
        };

        $(function () {
            loadData();
        });
        
    </script>
    <script type="text/javascript">
        //分页
        function ShowDataByPost1(pody) {
            LoadingAnimation("divcontent");
            $("#divcontent").load('/AjaxServers/QueueManage/QueueList.aspx?r=' + Math.random(), pody);
        }
    </script>
    <div class="content">
        <!--列表开始-->
        <div class="cxList" style="margin-top: 20px;" id="divcontent">
            <div class="clearfix">
            </div>
        </div>
</asp:Content>
