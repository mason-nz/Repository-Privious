<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCKnowledgeEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCKnowledgeEdit" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<script type="text/javascript">

    //验证知识点信息
    function ValidateKnow() {

        var msg = "";

        if ($.trim($("[id$=_txtTitle]").val()) == "") {
            msg = "知识点标题不能为空!<br/>";
        }

        if ($.trim($("[id$=_txtAbstract]").val()) == "") {
            msg = "知识点摘要不能为空!<br/>";
        }

        if ($.trim($("[id$=_txtAbstract]").val()).length > 200 || $.trim($("[id$=_txtAbstract]").val()).length < 100) {
            msg = msg + "知识点摘要的长度要大于100字并且小于200字<br/>";
        }

        if (Len($.trim($("[id$=_txtTitle]").val())) > 40) {
            msg = msg + "知识点标题不能大于20个汉字或者40个字母!<br/>";
        }
        if ($("#selKCID3").css("display") != "none" && $("#selKCID3").val() == "-1") {
            msg = msg + "请选择分类<br/>";
        }

        if ($("#selKCID2").css("display") != "none" && $("#selKCID2").val() == "-1") {
            msg = msg + "请选择分类<br/>";
        }
        if ($("#selKCID1").css("display") != "none" && $("#selKCID1").val() == "-1") {
            msg = msg + "请选择分类<br/>";
        }
        var Editor = FCKeditorAPI.GetInstance('<%=this.FCKeditor1.ClientID %>');
//        var html = Editor.GetXHTML();

//        if (Len($.trim($(html).text())) == 0) {
//            if ($(html).find("img").length == 0) {
//                msg = msg + "知识点内容不能为空<br/>";
//            }
//        }
//        var klid = '<%=KID %>';
        var status = "<%=status %>";
        var isManager = $("#hidIsManager").val();

        if (isManager != "1") { //不是管理员时，判断
            if (klid != "" && $("#inputSaveOrSub").val() == "sub") {
                //编辑时，提交操作，如果不是“未提交"状态，也不是“驳回”，就不能提交”
                if (status != "0" && status != "-1") {
                    msg = msg + "当前状态不能提交";
                }
            }
        }
        return msg;
    }

    //获取知识点信息数据
    function GetKnowData() {

        var kcid;
        if ($("#selKCID3").val() != "-1") {
            kcid = $("#selKCID3").val();
        }
        else if ($("#selKCID2").val() != "-1") {
            kcid = $("#selKCID2").val();
        }
        else if ($("#selKCID1").val() != "-1") {
            kcid = $("#selKCID1").val();
        }
        else {
            kcid = "null";
        }

        var Editor = FCKeditorAPI.GetInstance('<%=this.FCKeditor1.ClientID %>');

        var html = Editor.GetXHTML();

        if (Len($.trim($(html).text())) == 0) {
            if ($(html).find("img").length == 0) {
                html = "";
            }
        }

        var knowData = {
            KLID: escape($.trim($("[id$=_knowID]").val())),
            //分类
            KCID: escape(kcid),
            Title: escape($.trim($("[id$=_txtTitle]").val())),
            //Content: escape($.trim($("[id$=_txtContext]").val()))
            Content: escape(html),
            KAbstract: escape($.trim($("[id$=_txtAbstract]").val()))
        };

        return knowData;
    }



    //获取上传文件信息数据
    function GetFilesData() {
        //        //新上传的
        var fileinfoStr = $("#hidFilesInfo").val();

        //原有的
        $("li[name='spanOldFile']").each(function (i, v) {
            var jsonstr = "{";
            jsonstr = jsonstr + "'result':'succeed',";
            jsonstr = jsonstr + "'RecID':'" + escape($(this).attr("RecID")) + "',";
            jsonstr = jsonstr + "'FilePath':'" + escape($(this).attr("FilePath")) + "',";
            jsonstr = jsonstr + "'FileName':'" + escape($(this).attr("FileName")) + "',";
            jsonstr = jsonstr + "'ExtendName':'" + escape($(this).attr("ExtendName")) + "',";
            jsonstr = jsonstr + "'FileSize':'" + escape($(this).attr("FileSize")) + "'";
            jsonstr = jsonstr + "}";

            fileinfoStr = fileinfoStr + "," + jsonstr;

        });


        fileinfoStr = fileinfoStr.substr(1);
        fileinfoStr = "[" + fileinfoStr + "]";

        var jsonList = $.evalJSON(fileinfoStr);

        return jsonList;


    }

    //获得删除的文件IDs字符串
    function GetDeleteData() {

        var ids = $("#hidDelFileIds").val();
        ids = ids.substr(1);
        return ids;
    }

    //删除文件
    function DelFilesTag(obj) {
        $.jConfirm("确定删除文件吗？", function (r) {
            if (r) {
                var id = $(obj).attr("fileid");
                $("#fileID" + id).remove();
                $("#hidDelFileIds").val($("#hidDelFileIds").val() + "," + id);

                if ($("a[name=btndelFile]").length == 0) {
                    $("[id$='labFiles']").hide();
                }
            }
        });
    }

    function GetNum() {

        var num = "";
        var msg = "";
        num = $("[id$='txtAbstract']").val().length;

        if (Number(num) == 0) {
            return;
        }
        if (Number(num) >= 0 && Number(num) <= 200) {
            $("#spanNum").css("color", "Gray");
            msg = "摘要内容已输入" + num + "个字";
        }
        else {
            $("#spanNum").css("color", "Red");
            msg = "摘要内容已输入" + num + "个字,最多只能输入200个字";
        }
        $("[id$='spanNum']").text(msg);
    }

     
</script>
<script type="text/javascript">

    function SelectCategory() {
        var kcid = '<%=KCID %>';
        var level = '<%=Level %>';
        var Level1ID = '<%=Level1ID %>';
        var Level2ID = '<%=Level2ID %>';
        var Level3ID = '<%=Level3ID %>';

        if (level == "1") {

            $("option[value='" + Level1ID + "']").attr("selected", "selected");
        }
        else if (level == "2") {

            $("option[value='" + Level1ID + "']").attr("selected", "selected");
            BindSelect(2, Level1ID);
            $("option[value='" + Level2ID + "']").attr("selected", "selected");
        }
        else if (level == "3") {

            $("#selKCID3").show();
            $("option[value='" + Level1ID + "']").attr("selected", "selected");
            BindSelect(2, Level1ID);
            $("option[value='" + Level2ID + "']").attr("selected", "selected");
            BindSelect(3, Level2ID);
            $("option[value='" + Level3ID + "']").attr("selected", "selected");
        }
    }

    // n 要绑定控件所在级别   pid 上一个ID
    function BindSelect(n, pid) {

        AjaxPostAsync("/AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'BindKnowledgeCategory', Level: n, KCID: pid,regionid:<%=RegionID %> }, null, function (data) {

            $("#selKCID" + n).html("");
            $("#selKCID" + n).append("<option value='-1'>请选择</option>");

            if (data != "") {
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {

                    $.each(jsonData.root, function (idx, item) {
                        $("#selKCID" + n).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                    });
                    if (n == 3) {
                        $("#selKCID3").show();
                    }
                    else {
                        $("#selKCID3").hide();
                    }
                }
            }
        });
    }
</script>
<input type="hidden" id="knowID" value="" runat="server" />
<input type="hidden" id="hidFilesInfo" value="" />
<input type="hidden" id="hidDelFileIds" value="" />
<input type="hidden" id="hidIsManager" value="" />
<ul>
    <li>
        <label>
            标题：</label><span><input id="txtTitle" name="" type="text" class="w260" runat="server" /></span>&nbsp;<span
                class="redColor">*</span></li>
    <li>
        <label>
            所属分类：</label><span>
                <select id="selKCID1" class="w125">
                    <option value='-1'>请选择</option>
                </select></span> <span>
                    <select id="selKCID2" class="w125">
                        <option value='-1'>请选择</option>
                    </select></span> <span>
                        <select id="selKCID3" class="w125">
                            <option value='-1'>请选择</option>
                        </select>
                    </span>&nbsp;<span class="redColor">*</span> </li>
    <li>
        <label style="vertical-align: top">
            摘要：</label><span>
                <textarea id="txtAbstract" maxlength="200" name="" rows="5" runat="server"></textarea>
            </span></li>
    <li class="attach">
        <label>
            &nbsp; &nbsp;</label>&nbsp;<span id="spanNum" style="color: Gray;"></span>
    </li>
    <li>
        <label style="vertical-align: top">
            内容：</label><span>
                <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Width="800" Height="400" AutoDetectLanguage="false">
                </FCKeditorV2:FCKeditor>
            </span></li>
    <li class="attach">
        <label id="labFiles" runat="server">
            附件：</label>
        <ul class="attc">
            <asp:Repeater runat="server" ID="rpfileList">
                <ItemTemplate>
                    <li id='fileID<%#Eval("RecID") %>' name="spanOldFile" recid='<%#Eval("RecID") %>'
                        filepath='<%#Eval("FilePath") %>' filename='<%#Eval("Filename") %>' extendname='<%#Eval("ExtendName") %>'
                        filesize='<%#Eval("FileSize") %>'><a href='<%#Eval("FilePath").ToString().Replace(@"\","/") %>'
                            target="_blank">
                            <%#Eval("Filename")%><%#Eval("ExtendName")%></a> &nbsp;<a href="#" name="btndelFile"
                                fileid='<%#Eval("RecID") %>'><img src="/Images/clolse.gif" width="10" height="10" /></a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </li>
    <li>
        <label>
            上传：</label>
        <span>每次最多上传10份文档，每份文档不超过5M; 支持类型 doc,docx,ppt,pptx,xls,xlsx,pps,pdf,txt</span>
    </li>
    <li style="padding-left: 100px;">
        <input type="file" id="uploadify" name="uploadify" />
    </li>
</ul>
