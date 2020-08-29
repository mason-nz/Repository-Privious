<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSingleFAQ.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.AddSingleFAQ" %>

<%@ Register Src="UCKnowledgeLib/UCFAQList.ascx" TagName="UCFAQList" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>添加FAQ</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javacript"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindSelectChange() {
            var n = 2;
            var pid = $("#selKCID1").val();
            $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid,regionid:<%=RegionID %>  }, function (data) {
                $("#selKCID" + n).html("");
                $("#selKCID" + n).append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }

        function SubmitInfo() {
            //验证知识点、FAQ、试题
            var msg = "";


            //FAQ验证
            msg = CheckFAQ();
            if (msg != "") {
                $.jAlert(msg);
                return;
            }

            //保存
            if (SaveInfo()) {
                //$.jAlert("保存成功！", function () {

//                    if ($("#inputSaveOrSub").val() != "sub") {
//                        if ($("#hidKid").val() != "") {
//                            window.location = "KnowledgeEdit.aspx?kid=" + $("#hidKid").val();
//                        }
//                    }
//                    else {
                        //closePage();
//                    }
                //});

               $.jPopMsgLayer("保存成功！", function () {
               closePage(); 
               });
            }

        }

        //保存信息
        function SaveInfo() {

            var action = $("#inputSaveOrSub").val(); //（save:保存  sub:提交）

            var isSuccess = false;
            var SelQuestionType2 = $('#selKCID2').val();
            if (SelQuestionType2 == '-1') {
                $.jAlert("请选择FAQ二级分类");
                return;
            }



            //获取FAQ数据
            var faqData = GetAllFAQ();


            //            //获取试题数据
            //            var questionAllData = uCEditQuestionHelper.GetAllPageData();
            //            var qaData = questionAllData.Questions;
            //            var deleteQuestionIds = questionAllData.DeleteQuestionIDs;
            //            var deleteOptionIDs = questionAllData.DeleteOptionIDs;

            //            var htmlstr = knowData.Content;
            //            knowData.Content = "";

            var knowlibData = {
//                Knowledgeinfo: null,
//                fileinfo: null,
//                DeleteFilesIDs: deletFilesData,
                faqinfo: faqData,
                DeleteFAQIDs: $("#FAQ_DelIDs").val()
//                KLQuestions: questionAllData.Questions,
//                DeleteQuestionIDs: deleteQuestionIds,
//                DeleteOptionIDs: deleteOptionIDs
            };
            //保存到数据库
            var pody = { CheckedInfoStr: escape(JSON.stringify(knowlibData).returnRegExp()),KCID:escape(SelQuestionType2), action: escape(action), kid: <%=CommonSingleKLID %>, option: escape('Save'), isManager: escape($("#hidIsManager").val()),singleInfo:"singleFAQ" };
            AjaxPostAsync('/AjaxServers/KnowledgeLib/KnowledgeSave.ashx', pody,
            function () {
                $("#btnSave").attr("disabled", "disabled");
                $("#btnSubmit").attr("disabled", "disabled");
                $("#imgLoadingPop").css("display", "");
            }
            ,
             function (data) {

                 var jsonData = $.evalJSON(data);

                 if (jsonData.result == "success") {
                     //$("#hidKid").val(jsonData.kid);
                     isSuccess = true;
                 }
                 else {
                     $.jAlert(jsonData.kid);
                     isSuccess = false;
                 }
                 $("#btnSave").attr("disabled", "");
                 $("#btnSubmit").attr("disabled", "");
                 $("#imgLoadingPop").css("display", "none");
             });

            return isSuccess;
        }

        $(function () {
            //保存事件
            $("#btnSave").click(function () {
                $("#inputSaveOrSub").val("save");
                SubmitInfo();
            });

            //提交事件
            $("#btnSubmit").click(function () {
                $("#inputSaveOrSub").val("sub");
                SubmitInfo();
            });
            //是否是管理员
            var isManager = '<%=IsManager %>';
            // var isManager = '1';
            $("#hidIsManager").val(isManager);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <input type="hidden" id="inputSaveOrSub" value="save" />
        <div class="taskT" id="divTitle">
            添加FAQ</div>
        <div class="searchTj">
            <ul>
                <li style="width: auto; margin-left: 55px;"><span style="font-weight: bold;"><b>分类：</b></span>
                    <select id="selKCID1" style="width: 134px; margin-left: 5px;" class="w60" runat="server"
                        onchange="javascript:BindSelectChange()">
                        <option value='-1'>请选择</option>
                    </select>
                    <select id="selKCID2" class="w60" style="width: 134px;">
                        <option value='-1'>请选择</option>
                    </select>&nbsp;<span class="redColor">*</span>
                    <%--<select id="selKCID3" class="w60">
                    <option value='-1'>请选择</option>
                </select>--%>
                </li>
            </ul>
        </div>
        <br />
        <div class="addzs addzs2" style="clear: both; margin-top: 10px;">
            <UC:UCFAQList ID="UCFAQList" runat="server" />
            <!--添加试题结束-->
            <div class="btn zsdbtn">
                <img id="imgLoadingPop" src="/Images/blue-loading.gif" style="display: none" />
                <input id="btnSave" type="button" name="" value="保 存" class="btnSave bold" />&nbsp;&nbsp;
                <input id="btnSubmit" type="button" name="" value="提 交" class="btnCannel bold" />&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <input type="hidden" id="hidIsManager" value="" />
    </form>
</body>
</html>
