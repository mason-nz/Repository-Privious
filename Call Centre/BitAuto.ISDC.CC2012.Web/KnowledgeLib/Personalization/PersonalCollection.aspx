<%@ Page Title="个人收藏"  Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"

 CodeBehind="PersonalCollection.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.PersonalCollection" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            .aToButton
            {
                background-color:#48A6D2;
                padding:5px 10px;
                color:#FFFFFF;
                font-weight:bold  ;
            }
    </style>
    <script language="javascript" type="text/javascript">
        $(function () {
            search();
        });
        //查询
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#faqBt").addClass("aToButton").css("text-decoration", "none"); ;
            $("#knowledgeBt").addClass("aToButton").css("text-decoration", "none"); ;
            if ($("#hidType").val() == '1') {
                $("#ajaxTable").load("/AjaxServers/KnowledgeLib/FAQCollection.aspx", pody);
                $("#knowledgeBt").removeClass("aToButton");
                $("#faqBt").attr("disabled", "disabled");
                $("#knowledgeBt").removeAttr("disabled");
            }
            else if ($("#hidType").val() == '2') {
                $("#ajaxTable").load("/AjaxServers/KnowledgeLib/KnowledgeCollection.aspx", pody);
                $("#faqBt").removeClass("aToButton");
                $("#faqBt").removeAttr("disabled");
                $("#knowledgeBt").attr("disabled", "disabled");
            }
        }
        //参数
        function params() {
            var pody = "SelType=" + $("#hidType").val() + "&r=" + Math.random();
            return pody;
        }
        //分页操作 
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            if ($("#hidType").val() == '1') {
                $('#ajaxTable').load("/AjaxServers/KnowledgeLib/FAQCollection.aspx", pody);
            } else if ($("#hidType").val() == '2') {
                $('#ajaxTable').load("/AjaxServers/KnowledgeLib/KnowledgeCollection.aspx", pody);
            }
        }
        //点击超链接触发事件
        function aHrefTitleClick(kid) {
            try {
                window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=' + kid));
            }
            catch (e) {
                window.open("/KnowledgeLib/KnowledgeViewForUsers.aspx?kid=" + kid);
            }

        }
        function CancelCollectionClick(KLFavoritesId,cancelType) {
            //alert(KLFavoritesId);
            $.jConfirm("确定取消该收藏吗？", function (r) {
                if (r) {
                    $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "cancelcollect", KLFavoritesId: KLFavoritesId }, function (data) {
                        if (data == "success") {
                            //$.jAlert("取消收藏成功！");
                            $.jPopMsgLayer("取消收藏成功！");
                            if (cancelType == '1') {
                                ShowData('1');
                            }                    
                            else {
                                ShowData('2');
                            }
                        }
                        else {
                            $.jAlert(data);
                        }
                    });
                }
            });
        }
        function AddQuestion(KLID,Type) {
            $.openPopupLayer({
                name: "AddNewQuestionAjaxPopup",
                parameters: {},
                url: "/AjaxServers/KnowledgeLib/AddQuestion.aspx?KLID=" + KLID + "&KLType=" + Type + "&r=" + Math.random()
            });

        }
        function ShowData(theTpe) {
            $("#hidType").val(theTpe);
            search();
        }
    </script>
    <div class="optionBtn  clearfix" style=" height:30px; background-color:#F5F5F5">
         <input type="hidden" id="hidType" value="1"/>
    </div>
    <!--查询结束-->
    <div class="optionBtn  clearfix" style=" height:30px; background-color:#EBF2F6">
        <div>
            <a disabled="disabled" id="faqBt" href="javascript:void(0)" onclick="ShowData('1')">FAQ</a>&nbsp;&nbsp;
            <a disabled="disabled"  id="knowledgeBt" href="javascript:void(0);" onclick="ShowData('2')">知识点</a>
        </div>
    </div>
    <!--列表开始-->
    <div id="ajaxTable">
    </div>
</asp:Content>
