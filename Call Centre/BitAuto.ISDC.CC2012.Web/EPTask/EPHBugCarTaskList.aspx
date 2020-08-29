<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="惠买车" CodeBehind="EPHBugCarTaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.EPTask.EPHBugCarTaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //add by qizq 2014-4-15 根据二级菜单,判断如果是网销通任务,给a标签加onclick事件
            //取所有二级菜单下的li
            $("div[.content] h2").remove();
            addWXTUrl('<%=YPFanXianURL%>', '<%=TaskURL%>', '<%=EPEmbedCC_APPID %>');
//            AjaxPostAsync("/AjaxServers/EPEmbedCC.ashx", { YPFanXianURL: '<%=YPFanXianURL%>', GoToEPURL: '<%=TaskURL%>', EPEmbedCC_APPID: '<%=EPEmbedCC_APPID %>' }, null, function (data) {
//                if (data) {
//                    if (data != "" && data != "Error") {
//                        try {
//                            var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
//                        }
//                        catch (e) {
//                            window.open(decodeURIComponent(data));
//                        }
//                    }
//                }
            //});
        });
        //        function addWXTUrl(acontrl) {
        //            AjaxPostAsync("/AjaxServers/EPEmbedCC.ashx", { YPFanXianURL: '<%=YPFanXianURL%>', GoToEPURL: '<%=TaskURL%>', EPEmbedCC_APPID: '<%=EPEmbedCC_APPID %>' }, null, function (data) {
        //                if (data) {
        //                    if (data != "" && data != "Error") {
        //                        //alert(data);
        //                        //$(acontrl).attr("href", data);
        //                        window.location = data;
        //                        //window.open(data);
        //                        //                                //window.frames['iframewxt'].document.location.href = data;
        //                        //                                try {
        //                        //                                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
        //                        //                                }
        //                        //                                catch (e) {
        //                        //                                    window.open(data);
        //                        //                                }
        //                    }
        //                }
        //            });
        //        }
        
    </script>
    <iframe name="iframewxt" src="" frameborder="0" scrolling="no" width="1100px" height="510px">
    </iframe>
</asp:Content>
