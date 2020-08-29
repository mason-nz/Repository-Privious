<%@ Page Title="已接来电" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="AnswerPhone.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.AnswerPhone" %>

<%@ Register Src="UCTraffic/UCAnswerSearch.ascx" TagName="UCAnswerSearch" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            $("#hidCallStatus").val("1");
            search();
        });
        function Export() {
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
                var pody = _params();
                $("#formExport [name='Name']").val(pody.Name);
                $("#formExport [name='ANI']").val(pody.ANI);
                $("#formExport [name='Agent']").val(pody.Agent);
                $("#formExport [name='TaskID']").val(pody.TaskID);
                $("#formExport [name='BeginTime']").val(pody.BeginTime);
                $("#formExport [name='EndTime']").val(pody.EndTime);
                $("#formExport [name='AgentNum']").val(pody.AgentNum);
                $("#formExport [name='PhoneNum']").val(pody.PhoneNum);
                $("#formExport [name='TaskCategory']").val(pody.TaskCategory);
                $("#formExport [name='SpanTime1']").val(pody.SpanTime1);
                $("#formExport [name='SpanTime2']").val(pody.SpanTime2);
                $("#formExport [name='AgentGroup']").val(pody.AgentGroup);
                $("#formExport [name='CallStatus']").val(pody.CallStatus); //呼入还是呼出
                $("#formExport [name='Browser']").val(GetBrowserName());
                $("#formExport [name='IncomingSource']").val(pody.IncomingSource);
                $("#formExport [name='IVRScore']").val(pody.IVRScore);
                $("#formExport [name='selBusinessType']").val(pody.selBusinessType); //业务线
                $("#formExport").submit();
            }
        }        
    </script>
    <form id="form1" runat="server">
    <uc1:UCAnswerSearch ID="UCAnswerSearch1" runat="server" />
    <div class="optionBtn clearfix">
        <%if (ExportButton)
          { %><input name="" type="button" value="导出" onclick="Export()" class="newBtn" /><%} %>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="/AjaxServers/TrafficManage/AnswerPhoneExport.aspx"
    method="post">
    <input type="hidden" id="Name" name="Name" value="" />
    <input type="hidden" id="ANI" name="ANI" value="" />
    <input type="hidden" id="Agent" name="Agent" value="" />
    <input type="hidden" id="TaskID" name="TaskID" value="" />
    <input type="hidden" id="BeginTime" name="BeginTime" value="" />
    <input type="hidden" id="EndTime" name="EndTime" value="" />
    <input type="hidden" id="AgentNum" name="AgentNum" value="" />
    <input type="hidden" id="PhoneNum" name="PhoneNum" value="" />
    <input type="hidden" id="TaskCategory" name="TaskCategory" value="" />
    <input type="hidden" id="SpanTime1" name="SpanTime1" value="" />
    <input type="hidden" id="SpanTime2" name="SpanTime2" value="" />
    <input type="hidden" id="AgentGroup" name="AgentGroup" value="" />
    <input type="hidden" id="CallStatus" name="CallStatus" value="" />
    <input type="hidden" id="Browser" name="Browser" value="" />
    <input type="hidden" id="IncomingSource" name="IncomingSource" value="" />
    <input type="hidden" id="IVRScore" name="IVRScore" value="" />
    <input type="hidden" id="selBusinessType" name="selBusinessType" value="" />
    </form>
</asp:Content>
