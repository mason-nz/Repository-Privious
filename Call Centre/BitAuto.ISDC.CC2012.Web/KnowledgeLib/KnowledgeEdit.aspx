<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeEdit" %>

<%@ Register Src="UCKnowledgeLib/UCKnowledgeEditPDF.ascx" TagName="UCKnowledgeEdit"
    TagPrefix="uc1" %>
<%@ Register Src="UCKnowledgeLib/UCFAQList.ascx" TagName="UCFAQList" TagPrefix="UC" %>
<%@ Register Src="UCKnowledgeLib/UCKLOptionLogList.ascx" TagName="UCKLOptionLogList"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>添加知识点</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <link href="/css/uploadify.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript" src="/Js/json2.js"></script>
    <script src="/Js/jquery.uploadify.v3.2.min.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            //绑定分类
            //selKCIDChange(0);
            BindSelect(1, 0);
            SelectCategory();
            $("#selKCID1").change(function () {
                $("#selKCID3").hide();
                BindSelect(2, $("#selKCID1").val());
            });
            $("#selKCID2").change(function () {
                $("#selKCID3").hide();
                BindSelect(3, $("#selKCID2").val());
            });

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

            //删除文件
            $("a[name=btndelFile]").click(function (e) { e.preventDefault(); DelFilesTag(this); });

            //初始化上传
            InitUploadify();

            //删除的文件
            $("#hidDelFileIds").val("");

            //标题
            var kid = '<%=KID %>';

            if (kid == "") {
                //新增
                $("[id$='labFiles']").hide();

                $("#divTitle").text("添加知识点");
            }
            else {
                ///编辑
                $("[id$='labFiles']").show(); //附件显示

                $("#divTitle").text("编辑知识点");
            }

            //是否是管理员
            var isManager = '<%=IsManager %>';
            // var isManager = '1';
            $("#hidIsManager").val(isManager);

            GetNum();
            $("[id$='txtAbstract']").keyup(function (e) { GetNum(); });
        });

    </script>
    <script type="text/javascript">
        function InitUploadify() {
            var uploadSuccess = true;
            $("#hidFilesInfo").val("");

            $("#uploadify").uploadify({
                'buttonText': '选择',
                'auto': false,
                'swf': '/Js/uploadify.swf',
                'uploader': '/AjaxServers/KnowledgeLib/KnowledgeFiles.ashx?v=' + Math.random(),
                'formData': { LoginCookiesContent: escapeStr(GetCookie('<%=BitAuto.ISDC.CC2012.BLL.Util.SysLoginCookieName %>')) },
                'multi': true,
                'fileSizeLimit': '5MB',
                'queueSizeLimit': 1,
                'uploadLimit': 1,
                'method': 'post',
                'removeTimeout': 1,
                'fileTypeDesc': '*.pdf;*.mp3;*.wav',
                'fileTypeExts': '*.pdf;*.mp3;*.wav',
                'width': 79,
                'height': 26,
                'onUploadSuccess': function (file, data, response) {

                    if (response == false) {
                        uploadSuccess = false;
                        $.jAlert("上传失败!");
                    }
                    else {
                        //    alert('The file ' + file.name + ' was successfully uploaded with a response of ' + response + ':' + data);
                        var jsonData = $.evalJSON(data);
                        if (jsonData != null && jsonData.length > 0) {
                            jsonData = jsonData[0];
                        }
                        if (jsonData.result == "noFiles") {
                            uploadSuccess = false;
                            $.jAlert("请选择文件!");
                        }
                        else if (jsonData.result == "failure") {
                            uploadSuccess = false;
                            $.jAlert("上传文件出错!");
                        }
                        else if (jsonData.result != "succeed") {
                            uploadSuccess = false;
                        }
                        else {
                            //上传成功

                            var str = $("#hidFilesInfo").val() + "," + data;

                            $("#hidFilesInfo").val(str);

                            uploadSuccess = true;
                        }
                    }
                },
                'onQueueComplete': function (queueData) {
                    // alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');

                    if (uploadSuccess) { //上传都成功

                        //alert("整体提交");

                        if (SaveInfo()) {
//                            $.jAlert("保存成功！", function () {


//                                //$('#uploadify').uploadify('cancel', '*');

//                                if ($("#inputSaveOrSub").val() != "sub") {
//                                    if ($("#hidKid").val() != "") {
//                                        window.location = "KnowledgeEdit.aspx?kid=" + $("#hidKid").val();
//                                    }
//                                }
//                                else {
//                                    closePage();
//                                }
                            //                            });
                            $.jPopMsgLayer("保存成功！", function () {


                                //$('#uploadify').uploadify('cancel', '*');

                                if ($("#inputSaveOrSub").val() != "sub") {
                                    if ($("#hidKid").val() != "") {
                                        window.location = "KnowledgeEdit.aspx?kid=" + $("#hidKid").val();
                                    }
                                }
                                else {
                                    closePage();
                                }
                            });
                        }

                    }


                }
            });
        }

     
    </script>
    <script type="text/javascript">

        function SubmitInfo() {
            //验证知识点、FAQ、试题
            var msg = "";

         
            //验证知识点
            msg = ValidateKnow();
            if (msg != "") {
                $.jAlert(msg);
                return;
            }

            //FAQ验证
            msg = CheckFAQ();
            if (msg != "") {
                $.jAlert(msg);
                return;
            }

            //验证试题
            var questionAllData = uCEditQuestionHelper.GetAllPageData();
            msg = uCEditQuestionHelper.ValidateData(questionAllData.Questions);
            if (msg != "") {
                $.jAlert(msg);
                return;
            }


            //            var uploadify = $("#uploadify");
            //            var queueSize = $("div.uploadify-queue-item").length;
            var queueSize = $('#uploadify').uploadify('queueLength');


            if (queueSize > 0) {

                //上传并保存
                $("#hidFilesInfo").val("");
                $('#uploadify').uploadify('upload', '*');
            }
            else {
                //保存
                if (SaveInfo()) {
//                    $.jAlert("保存成功！", function () {

//                        if ($("#inputSaveOrSub").val() != "sub") {
//                            if ($("#hidKid").val() != "") {
//                                window.location = "KnowledgeEdit.aspx?kid=" + $("#hidKid").val();
//                            }
//                        }
//                        else {
//                            closePage();
//                        }
                    //                    });
                    $.jPopMsgLayer("保存成功！", function () {
                        if ($("#inputSaveOrSub").val() != "sub") {
                            if ($("#hidKid").val() != "") {
                                window.location = "KnowledgeEdit.aspx?kid=" + $("#hidKid").val();
                            }
                        }
                        else {
                            closePage();
                        }
                    });
                }
            }
        }

        //保存信息
        function SaveInfo() {


            var action = $("#inputSaveOrSub").val(); //（save:保存  sub:提交）

            //获取文件数据
            var fileData = GetFilesData();

//            if (fileData == null || fileData.length <= 0) {
//                $.jAlert("请上传附件后提交！");
//                return false;
//            }
            var isSuccess = false;
            //获取知识点数据
            var knowData = GetKnowData();
            if (knowData.KCID == null || knowData.KCID == -1) {
                $.jAlert("只可以选择二级分类");
                return false;
            }

           

            //获取删除的文件IDs
            var deletFilesData = GetDeleteData();
            //获取FAQ数据
            var faqData = GetAllFAQ();
            var deleteFaqData = $("#FAQ_DelIDs").val();

            //获取试题数据
            var questionAllData = uCEditQuestionHelper.GetAllPageData();
            var qaData = questionAllData.Questions;
            var deleteQuestionIds = questionAllData.DeleteQuestionIDs;
            var deleteOptionIDs = questionAllData.DeleteOptionIDs;

            var htmlstr = knowData.Content;
            knowData.Content = "";

            var knowlibData = {
                Knowledgeinfo: knowData,
                fileinfo: fileData,
                DeleteFilesIDs: deletFilesData,
                faqinfo: faqData,
                DeleteFAQIDs: deleteFaqData,
                KLQuestions: questionAllData.Questions,
                DeleteQuestionIDs: deleteQuestionIds,
                DeleteOptionIDs: deleteOptionIDs
            };
            //保存到数据库
            var pody = { CheckedInfoStr: escape(JSON.stringify(knowlibData).returnRegExp()), HtmlStr: htmlstr, action: escape(action), kid: escape('<%=KID %>'), option: escape('Save'), isManager: escape($("#hidIsManager").val()) };
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
                     $("#hidKid").val(jsonData.kid);
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

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="inputSaveOrSub" value="save" />
    <input type="hidden" id="hidKid" value="" />
    <div class="w980">
        <div class="taskT" id="divTitle">
            添加知识点</div>
        <div class="addzs addzs2">
            <uc1:UCKnowledgeEdit ID="UCKnowledgeEdit1" runat="server" />
            <UC:UCFAQList ID="UCFAQList" runat="server" />
            <div id="divAddQuestion" class="title bold">
                <a name='question'></a>添加试题：
            </div>
            <!--添加试题开始-->
            <asp:PlaceHolder ID="phQuestion" runat="server"></asp:PlaceHolder>
            <div class="title bold">
                
                    <input name="" style="margin-left: 70px;" type="button" value="添加单选题" onclick="javascript:uCEditQuestionHelper.AddQuestion('radio');"
                        class="newBtn2 addbtn" /></span>&nbsp; <span>
                            <input name="" type="button" value="添加复选题" class="newBtn2 addbtn" onclick="javascript:uCEditQuestionHelper.AddQuestion('checkbox');" /></span>&nbsp;<span><input
                                name="" type="button" value="添加主观题" onclick="javascript:uCEditQuestionHelper.AddQuestion('text');"
                                class="newBtn2 addbtn" /></span>&nbsp; <span>
                                    <input name="" type="button" value="添加判断题" onclick="javascript:uCEditQuestionHelper.AddQuestion('select');"
                                        class="newBtn2 addbtn" /></span>
            </div>
            <!--添加试题结束-->
            <div class="btn zsdbtn">
                <img id="imgLoadingPop" src="/Images/blue-loading.gif" style="display: none" />
                <input id="btnSave" type="button" name="" value="保 存" class="btnSave bold" />&nbsp;&nbsp;
                <input id="btnSubmit" type="button" name="" value="提 交" class="btnCannel bold" />&nbsp;&nbsp;
            </div>
            <uc2:UCKLOptionLogList ID="UCKLOptionLogList1" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
