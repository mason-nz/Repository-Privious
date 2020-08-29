<%@ Page Title="问题解答" Language="C#" AutoEventWireup="true"   MasterPageFile="~/Controls/Top.Master"
CodeBehind="QuestionAnswered.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.QuestionAnswered" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
            search();
        });

        //查询
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/KnowledgeLib/QuestionAnsweredDetails.aspx .bit_table > *", pody, function () {
                if ($("#hidType").val() == "1") {
                    $("#spanRecordCount").html($("#hidRecordCount").val());
                    $("#spanNotAnswer").html($("#hidNotAnswer").val());
                }
            });
        };
        //参数
        function params() {
            var pody = "SelType=" + $("#hidType").val() + "&r=" + Math.random();
            return pody;
        }
       function DeleteClick(QuestionId) {
            $.jConfirm("确定删除该提问信息吗？", function (r) {
                if (r) {
                    $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "deletequestion", QuestionId: QuestionId }, function (data) {
                        if (data == "success") {
                            //$.jAlert("删除提问成功！");
                            $.jPopMsgLayer("删除提问成功！");
                            ShowData('1');
                        }
                        else {
                            $.jAlert(data);
                        }
                    });
                }
            });
        };

        function ShowData(theTpe) {
            $("#hidType").val(theTpe);
            search();
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load("/AjaxServers/KnowledgeLib/QuestionAnsweredDetails.aspx .bit_table > *", pody, function () {
                if ($("#hidType").val() == "1") {
                    $("#spanRecordCount").html($("#hidRecordCount").val());
                    $("#spanNotAnswer").html($("#hidNotAnswer").val());
                }
            });
        }

    </script>
    <div class="optionBtn  clearfix" style=" height:30px; background-color:#F5F5F5">
        <input type="hidden" id="hidType" value="1"/>
    </div>
             <!--查询结束-->
    <div class="optionBtn  clearfix" style=" height:30px; background-color:#EBF2F6">
        <div>
            <a id="faqBt" href="javascript:void(0)" onclick="ShowData('1')">全部（<span id="spanRecordCount"></span>）</a>&nbsp;&nbsp;
            <a  id="knowledgeBt" href="javascript:void(0);" onclick="ShowData('2')">未解决（<span id="spanNotAnswer"></span>）</a>
        </div>
    </div>
    <!--列表开始-->
     <div id="ajaxTable">
    </div>
</asp:Content>
