<%@ Page Language="C#" MasterPageFile="~/Controls/Top.Master" Title="标签管理" AutoEventWireup="true"
    CodeBehind="TagManagement.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TagManagement" %>

<%@ Register Src="~/TemplateManagement/UCTag/TagLayout.ascx" TagName="tagSelect"
    TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery-ui.js" type="text/javascript"></script>
    <script src="../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
    <link href="../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ModifyTagGroup(obj) {
            var this$ = $(obj);
            var bgid = this$.closest('.bq1').attr('did');
            $.openPopupLayer({
                name: "EditWorkTag2",
                parameters: { "bgid": this$.closest('.bq1').attr('did'), "c": "0" },
                url: "../AjaxServers/TemplateManagement/TagManagementPop.aspx",                
                beforeClose: function (e, data) {
                    //window.location.reload();
                    if (e) {
                    }
                }
            });
            return false;
        }

       
    </script>
    <div class="rC left">
        <div class="content">
            <br />
            <div id="divMain" class="bqc bqc2">
                <uc3:tagSelect ID="tagSelect1" runat="server" GetAll="True" />
            </div>
        </div>
    </div>
</asp:Content>
