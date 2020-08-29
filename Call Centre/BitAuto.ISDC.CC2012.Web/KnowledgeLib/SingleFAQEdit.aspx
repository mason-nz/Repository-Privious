<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleFAQEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.SingleFAQEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <title>查看FAQ</title>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="/Js/jquery-1.4.4.min.js"></script>
    <script language="javascript" type="text/javascript" src="/Js/common.js"></script>
    <script type="text/javascript" src="/Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindSelectChange() {
            var n = 2;
            var pid = $("#selKCID1").val();
            $.get("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid }, function (data) {
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <input type="hidden" id="inputSaveOrSub" value="save" />
        <div class="taskT" id="divTitle">
            编辑FAQ</div>
        <div class="searchTj">
            <ul>
                <li style="width: auto; margin-left: 55px;"><span style="font-weight: bold;"><b>分类：</b></span>
                    <select id="selKCID1" style="width: 134px; margin-left: 5px;" class="w60" runat="server"
                        onchange="javascript:BindSelectChange()">
                        <option value='-1'>请选择</option>
                    </select>
                    <select id="selKCID2" class="w60" style="width: 134px;">
                        <option value='-1'>请选择</option>
                    </select>
                </li>
            </ul>
        </div>
        <br />
        <br />
        <div class="addzs addzs2" style="clear: both; margin-top: 10px;">
            <div class="title bold">
                <a name="faq"></a>编辑FAQ：<span class="add"><a onclick="FAQ_Add()" href="javascript:void(0);"></a></span>
                <input type="text" style="display: none;" id="FAQ_DelIDs" name="FAQ_DelIDs"></div>
            <ul>
                <li style="height: 65px;">
                    <label style="vertical-align: top">
                        Q、</label>
                    <span>
                        <textarea class="w700" id="FAQ_Q" runat="server" name="FAQ_Q"></textarea>
                    </span></li>
                <li class="answer">
                    <label style="vertical-align: top">
                        A、</label>
                    <span>
                        <textarea class="w700" id="FAQ_A" runat="server" name="FAQ_A"></textarea>
                    </span>
                    <!--隐藏域-->
                    <input type="hidden" value="0" name="hidden_FAQID">
                </li>
            </ul>
            <!--添加试题结束-->
            <div class="btn zsdbtn">
                <img id="imgLoadingPop" src="/Images/blue-loading.gif" style="display: none" />
                <input id="btnSave" type="button" name="" value="编 辑" class="btnSave bold" />&nbsp;&nbsp;
                <input id="btnSubmit" type="button" name="" value="保 存" class="btnCannel bold" />&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <input type="hidden" id="hidIsManager" value="" />
    </form>
</body>
</html>
