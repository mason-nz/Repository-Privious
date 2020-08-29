<%@ Page Title="去电记录" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="CallPhone.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.CallPhone" %>

<%@ Register Src="UCTraffic/UCCallSearch.ascx" TagName="UCCallSearch" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            $("#hidCallStatus").val("2");

            //search();

        });

        function Export() {
            if (CheckForSelectCallRecordORIG("tfBeginTime", "tfEndTime")) {
                var pody = _params_callsearch();

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
                $("#formExport [name='selCategory']").val(pody.selCategory);
                $("#formExport [name='OutTypes']").val(pody.OutTypes);

                $("#formExport [name='ProjectId']").val(pody.ProjectId);
                $("#formExport [name='IsSuccess']").val(pody.IsSuccess);
                $("#formExport [name='FailReason']").val(pody.FailReason); 
                $("#formExport").submit();
            }
        }
    </script>
    <form id="form1" runat="server">
    <uc1:UCCallSearch ID="UCCallSearch1" runat="server" />
    <div class="optionBtn clearfix">
        <%if (ExportButton)
          { %><input name="" type="button" value="导出" onclick="Export()" class="newBtn" /><%} %>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    </form>
    <form id="formExport" action="/AjaxServers/TrafficManage/CallPhoneExport.aspx" method="post">
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
    <input type="hidden" id="selCategory" name="selCategory" value="" />
    <input type="hidden" id="hidOutTypes" name="OutTypes" value="" />

    <input type="hidden" id="ProjectId"  name="ProjectId" value="" />
    <input type="hidden" id="IsSuccess"  name="IsSuccess" value="" />
    <input type="hidden" id="FailReason" name="FailReason" value="" />
    </form>
</asp:Content>
