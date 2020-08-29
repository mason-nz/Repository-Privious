<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplyRangeList.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.ApplyRange.ApplyRangeList"
    Title="应用范围" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            pody = "random=" + Math.round(Math.random() * 10000);
            LoadingAnimation('bit_table');
            $("#ajaxTable").load("/AjaxServers/QualityStandard/ApplyRange/ApplyRangeList.aspx", pody);
        });
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('bit_table');
            $("#ajaxTable").load("/AjaxServers/QualityStandard/ApplyRange/ApplyRangeList.aspx", pody);
        }
    </script>
    <div>
        <div id="ajaxTable">
        </div>
    </div>
</asp:Content>
