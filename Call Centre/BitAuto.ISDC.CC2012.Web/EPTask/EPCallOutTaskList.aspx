<%@ Page  Language="C#" MasterPageFile="~/Controls/Top.Master" Title="网销通任务" AutoEventWireup="true"
    CodeBehind="EPCallOutTaskList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.EPTask.EPCallOutTaskList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //add by qizq 2014-4-15 根据二级菜单,判断如果是网销通任务,给a标签加onclick事件
            //取所有二级菜单下的li
            $("div[.content] h2").remove();
            $("ul[id='ulSecondMenu'] li").each(function () {
                //如果是网销通菜单
                if ($(this).attr("moduleid") == "SYS024MOD1009") {
//                    var acontrl = $(this).find("a");
//                    $(acontrl).unbind("click").bind("click", function () {
//                        addWXTUrl(acontrl)
//                    });
//                    $(acontrl).attr("target", "iframewxt");
//                    AjaxPostAsync("/AjaxServers/EPEmbedCC.ashx", { YPFanXianURL: '<%=YPFanXianURL %>', GoToEPURL: '<%=TaskURL%>', EPEmbedCC_APPID: '<%=EPEmbedCC_APPID %>' }, null, function (data) {
//                        if (data) {
//                            if (data != "" && data != "Error") {
//                                //var f = document.getElementsByName("iframewxt");
//                                //f.style.display = "inline";
//                                //f.src = data;
//                                window.frames['iframewxt'].document.location.href = decodeURIComponent(data);
//                            }
//                        }
//                    });
                }
            });
        });
//        function addWXTUrl(acontrl) {
//            AjaxPostAsync("/AjaxServers/EPEmbedCC.ashx", { YPFanXianURL: '<%=YPFanXianURL %>', GoToEPURL: '<%=TaskURL%>', EPEmbedCC_APPID: '<%=EPEmbedCC_APPID %>' }, null, function (data) {
//                if (data) {
//                    if (data != "" && data != "Error") {
//                        $(acontrl).attr("href", data);
//                    }
//                }
//            });
//        }
        
    </script>
    <iframe name="iframewxt" src="" frameborder="0" scrolling="no" width="1100px" height="510px">
    </iframe>
</asp:Content>
