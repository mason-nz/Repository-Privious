<%@ Page Title="知识点管理" Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="KnowledgeLibList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeLibList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            InitWdatePicker(2, ["tfBeginTime", "tfEndTime"]);
            chkProperty();
            selKCIDChange(0);
            loadHtml(1);
        });

        //查询 根据不同的点击绑定不同的信息页面
        function search() {
            var pody = params();
            LoadingAnimation("ajaxTable");
            var type = $("#hidSearchType").val();
            switch (Number(type)) {
                case 1: $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KnowledgeList.aspx", pody);
                    break;
                case 2: $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KLFAQList.aspx", pody);
                    break;
                case 3: $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KLQuestionList.aspx", pody);
                    break;
                case 4: $("#ajaxTable").load("../AjaxServers/KnowledgeLib/KnowledgeLibCount.aspx", pody);
                    break;
            }

        }
        //参数
        function params() {
            var title = encodeURI($.trim($("#txtTitle").val()));
            var kcid;
            if ($("#selKCID2").val() != "-1") {
                kcid = $("#selKCID2").val();
            }
            else if ($("#selKCID1").val() != "-1") {
                kcid = $("#selKCID1").val();
            }
            else {
                kcid = "";
            }
            var createUser = "";
            if ($("#<%=selCreateUser.ClientID %>").val() != "-1" && $("#<%=selCreateUser.ClientID %>").val() != undefined) {
                createUser = $("#<%=selCreateUser.ClientID %>").val();
            }
            var mUser = "";
            if ($("#<%=selModifier.ClientID %>").val() != "-1" && $("#<%=selModifier.ClientID %>").val() != undefined) {
                mUser = $("#<%=selModifier.ClientID %>").val();
            }

            var beginTime = $.trim($("#tfBeginTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            var property = $(":checkbox[name='chkProperty']:checked").map(function () {
                return $(this).val();
            }).get().join(',');
            var category = ""; 
            var pody = "Title=" + title + "&KCID=" + kcid + "&CreateUserID=" + createUser + "&ModifyUserID=" + mUser + "&BeginTime=" + beginTime
            + "&EndTime=" + endTime + "&Property=" + property + "&Category=" + category + "&r=" + Math.random();
            return pody;
        }
        //绑定二级列表
        function selKCIDChange(n) {
            var pid = $("#selKCID" + n).val();
            if (pid == undefined) {
                pid = 0;
            }
            if (n <= 2) {
                $("#selKCID" + (n + 1)).children().remove();
                $("#selKCID" + (n + 1)).append("<option value='-1'>请选择</option>");
            }
            var level = n + 1;
            $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'bindknowledgecategoryexceptdel', Level: level, KCID: pid,RegionID:<%=RegionID %> }, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "" && n <= 2) {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selKCID" + (n + 1)).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });

                    }
                }
            });
        }
        //试题
        function chkProperty() {
            var isQuestion = $(":checkbox[id='chkIsQuestion']").attr("checked");
            if (isQuestion == true) {
                $(":checkbox[name='chkCategory']").removeAttr("checked");
            }
            else {
            }
        }
        //根据不同的点击绑定不同的信息页面
        function loadHtml(n, othis) {
            $("select[id^='selKCID']").removeAttr("disabled");
            $(":checkbox[name='chkProperty']").removeAttr("disabled");
            $(":checkbox[name='chkQuestionStatus']").attr("checked", false);
            $('#hidSearchType').val(n);
            $(othis).addClass("redColor").siblings().removeClass("redColor");
            search();
        }
        //审核通过（批量）
        function LibByApproval(klid) {
            var KLIDs = klid;
            if (KLIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            if ($.jConfirm("确认将所选知识点进行审核通过操作？", function (r) {
                if (r) {
                    $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'ApprovalKnowledgeLib', 'KLID': KLIDs, 'random': Math.random() }, function (data) {
                        operSuccess(data);
                    });
                }
            }));
        }
        //停用（批量、单个）
        function LibDisable(klid) {
            var KLIDs = klid;
            if (KLIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            if ($.jConfirm("确认将所选知识点进行停用操作？", function (r) {
                if (r) {
                    $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'DisableKnowledgeLib', 'KLID': KLIDs, 'random': Math.random() }, function (data) {
                        operSuccess(data);
                    });
                }
            }));
        }
        //删除（批量、单个）
        function LibDelete(klid) {
            var KLIDs = klid;
            if (KLIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            if ($.jConfirm("确认将所选知识点进行删除操作？", function (r) {
                if (r) {
                    $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'DeleteKnowledgeLib', 'KLID': KLIDs, 'random': Math.random() }, function (data) {
                        operSuccess(data);
                    });
                }
            }));
        }
        //获取需要审批通过、驳回、停用、删除操作的KLID字符串
        function getKLIDs() {
            var KLIDs = $(":checkbox[name='chkOper']:checked").map(function () {
                return $(this).val();
            }).get().join(',');
            return KLIDs;
        }
        //操作成功提示
        function operSuccess(data) {
            var jsonData = $.evalJSON(data);
            
            if (jsonData != "") {
            if(jsonData.msg.indexOf("成功") != -1){
            $.jPopMsgLayer(jsonData.msg, function () {
                    window.location.reload();
                });
            }
            else
            {
            $.jAlert(jsonData.msg, function () {
                    window.location.reload();
                });
            }
                
            }
            else {
                $.jAlert("操作失败");
                return false;
            }
        }
        //驳回理由弹出层
        function openRejectReasonPopup(klid) {
            var KLIDs = klid;
            if (KLIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            if ($.jConfirm("确认将所选知识点进行驳回操作？", function (r) {
                if (r) {
                    $.openPopupLayer({
                        name: "RejectReasonPopup",
                        parameters: { KLID: KLIDs, r: Math.random() },
                        popupMethod: 'Post',
                        url: "RejectReason.aspx",
                        beforeClose: function () {

                        }
                    });
                }
            }));
        }
        //移动弹出层
        function openMovePopup() {
            var KLIDs = getKLIDs();
            if (KLIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            $.openPopupLayer({
                name: "KnowledgeCategorySelectAjaxPopup",
                parameters: { KLID: KLIDs, r: Math.random() },
                popupMethod: 'Post',
                url: "KnowledgeCategorySelect.aspx",
                beforeClose: function () {

                }
            });
        }

        //停用试题（批量）
        function QuestionDisable(klqid) {
            var KLQIDs = klqid;
            if (KLQIDs == "") {
                $.jAlert("必须选择至少一个才能进行操作！");
                return false;
            }
            if ($.jConfirm("确认将所选试题进行停用操作？", function (r) {
                if (r) {
                    $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { 'Action': 'QuestionDisableKnowledgeLib', 'KLQID': KLQIDs, 'random': Math.random() }, function (data) {
                        operSuccess(data);
                    });
                }
            }));
        }

        //新增知识点
        function OpenKnowledgeEdit(type) {
            try {
                var urlT = "";
                switch (type) {
                    case 1:
                        urlT = "KnowledgeEdit.aspx";
                        break;
                    case 2:
                        urlT = "AddSingleFAQ.aspx";
                        break;
                    case 3:
                        urlT = "AddSingleQuestion.aspx";
                        break;
                    default:
                        urlT = "KnowledgeEdit.aspx";
                        break;
                }
                window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/KnowledgeLib/' + urlT);
            }
            catch (e) {
                window.open(urlT);
            }
        }

        //弹出层-分类管理
        function OpenCategory() {
            $.openPopupLayer({
                name: "addCategory",
                parameters: { TypeId: "2" },
                url: "KnowledgeCategoryManage.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }

        function changeLayerPosition() {
            var winHeight = $(window).height();
            var divHeight = $("#popupLayer_addCategory").height();
            //$.jAlert("winHeight=" + winHeight + "     " + divHeight);
            var divNewPY = (winHeight - divHeight) / 2;
            if (divNewPY < 0) {
                divNewPY = 50;
            }
            // $("#popupLayer_addCategory").css({ "position": "absolute", "top": divNewPY + "px", "left": "400px" });
            $("#popupLayer_addCategory").css({ "top": divNewPY + "px" });
        }
    </script>
    <script type="text/javascript">
        var lastpid = 0;
        var lastlevel = 1;
        //绑定分类列表和网格数据
        function laySelKCIDChange(n) {
            var pid = $("#layerSelKCID" + n).val(); //获取选中的列表值
            var level = n + 1;

            if (pid == undefined) {
                pid = 0;
            }
            $.post("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: 'BindKnowledgeCategory', Level: level, KCID: pid,RegionID:<%=RegionID %>, r: Math.random() }, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#layerSelKCID" + (n + 1)).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });

            if (pid == -1) {
                pid = $("#layerSelKCID" + (n - 1)).val();
                if (pid == undefined) {
                    pid = 0;
                }
                level = n;
            }
            lastlevel = level;
            lastpid = pid;
            laySearchData();
        }

        //绑定查询到的网格数据
        function laySearchData() {
            var nameVal = $.trim($("#layKnowledgekeywords").val());
            if (nameVal == "请输入关键字查询") {
                nameVal = "";
            }
            $.get("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: 'bindchildrencategoryinfo', Level: lastlevel, KCID: lastpid, name: nameVal,RegionID:<%=RegionID %>,r: Math.random() }, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        var htmlText = "";
                        var sort_info_new = "";
                        var sort_info_old = "";
                        $.each(jsonData.root, function (idx, item) {
                            htmlText += "<tr><td>" + item.parentName + "&nbsp;</td><td><em>" + item.name + "</em><input type='text' value='" + item.name + "' style='display: none;width:92%' />&nbsp;</td>";
                            if (item.status == 0) {
                                htmlText += "<td>在用</td><td>";
                            }
                            else if (item.status == 1) {
                                htmlText += "<td>停用</td><td>";
                            }
                            else if (item.status == -3) {
                                htmlText += "<td>系统</td><td>";
                            }
                            else {
                                htmlText += "<td>&nbsp;</td><td>";
                            }
                            if (item.status ==0 || item.status ==1) {
                                htmlText += "<a href=javascript:void(0); onclick='groupEdit(" + item.kcid + ",this)'>编辑</a> "
                               + "<a href=javascript:void(0); onclick='groupDelete(" + item.kcid + ")'>删除</a> "
                               + "<a href='javascript:void(0);' onclick='groupSave(2," + item.kcid + ",this)' scid='" + item.kcid
                               + "' style='display: none' name='a_save'>保存</a> <a href='javascript:void(0);' onclick='groupCancel("
                               + item.kcid + ",this)' style='display: none'>取消</a> ";
                            }
                            if (item.status == 0) {
                                htmlText += "<a href=javascript:void(0); onclick='changeCategoryStatus(" + item.kcid + ",this)'>停用</a> ";
                            }
                            if (item.status == 1) {
                                htmlText += "<a href=javascript:void(0); onclick='changeCategoryStatus(" + item.kcid + ",this)'>启用</a> ";
                            }
                            htmlText += "</td>";
                            //顺序列
                            htmlText += "<td>";
                            if (idx == 0) {
                                htmlText += "<span style='color: #666;'>上移</span> ";
                            }
                            else {
                                htmlText += "<a href=javascript:void(0); onclick='SortNumUpOrDown(" + (idx + 1) + ",\"up\")'>上移</a> ";
                            }
                            if (idx + 1 == jsonData.root.length) {
                                htmlText += "<span style='color: #666;'>下移</span> ";
                            }
                            else {
                                htmlText += "<a href=javascript:void(0); onclick='SortNumUpOrDown(" + (idx + 1) + ",\"down\")'>下移</a> ";
                            }
                            htmlText += "</td>";
                            htmlText += "</tr>";

                            //新的顺序
                            sort_info_new += (idx + 1) + ":" + item.kcid + ",";
                            //旧的顺序
                            sort_info_old += item.kcid + ":" + item.sortnum + ",";
                        });
                        $("#trList tr:gt(0)").remove();
                        $("#trList").append(htmlText);
                        $("#inpsortnum").val(sort_info_new + "@" + sort_info_old);

                        changeLayerPosition();
                    }
                    else {
                        $("#trList tr:gt(0)").remove();
                    }
                }
                else {
                    $("#trList tr:gt(0)").remove();
                }
            });
        }
        //编辑
        function groupEdit(groupName, othis) {
            showEditText(othis);
        }
        //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
        function showEditText(othis) {
            var $td = $(othis).parent().parent()
                            .find("td:eq(1)");
            // 1
            $td.find("em").hide().end()
                        .find("input[type='text']").show().focus();
            // 2 
            $(othis).parent().find("a:eq(0)").hide().end()
                .find("a:eq(1)").hide().end()
                .find("a:eq(2)").show().end()
                .find("a:eq(3)").show();
        }
        //取消。如果scid=-1：是新增行取消，需要移除新增行；scid!=-1：则是编辑时取消
        function groupCancel(scid, othis) {
            if (scid != "-1") {
                hideEditText(othis);
            }
            else {
                $(othis).parents("tr").remove();
            }
        }
        //1 显示文本，隐藏编辑框；2 显示编辑、删除按钮，隐藏保存、取消按钮
        function hideEditText(othis) {
            var $td = $(othis).parent().parent()
                            .find("td:eq(1)");
            //记录文本，编辑时不会出现文本不一致情况
            var emStr = $td.find("em").html();
            $td.find("input[type='text']").val($.trim(emStr));

            // 1
            $td.find("em").show().end()
                            .find("input[type='text']").hide();

            // 2 
            $(othis).parent().find("a:eq(0)").show().end()
                .find("a:eq(1)").show().end()
                .find("a:eq(2)").hide().end()
                .find("a:eq(3)").hide();
        }

        //保存。如果scid=-1：是新增；scid!=-1：则是编辑；type=1是全部保存，type=2是单个保存
        function groupSave(type, scid, othis) {
            var $othis = $(othis).parents("tr").find("td:eq(1) [type='text']");
            var categoryName = "";
            if ($othis.is(":visible")) {
                categoryName = $.trim($othis.val());
            }

            var txtEditText = "";
            $("#trList tr").each(function () {
                var $td0 = $(this).find("td:eq(1) input[type='text']");
                if (!$td0.is(":visible")) {
                    txtEditText += $.trim($td0.val()) + ",";
                }
            });

            if (categoryName == "") {
                $.jAlert("请输入分类名称");
                return false;
            }

            var isRepeatStr = isRepeat(categoryName, txtEditText.substr(1, txtEditText.length - 2));
            if (isRepeatStr == false) {
                $.jAlert("分类名称有重复，不能保存");
                return false;
            }

            if ($.jConfirm("是否确认修改该条记录？", function (r) {
                if (r) {
                    var msg = update_one(scid, encodeURIComponent(categoryName));
                    $.jAlert(msg);
                    laySearchData();
                    refrashList();
                }
                else {
                    groupCancel(scid, othis);
                    return false;
                }
            }));

            hideEditText(othis);
        }


        function isRepeat(compareStr, arrayStr) {
            var msg = "true";

            if ($.trim(arrayStr) != "") {
                var array_str = $.trim(arrayStr).split(',');
                for (var i = 0; i < array_str.length; i++) {
                    if (compareStr == null) {
                        for (var j = i + 1; j < array_str.length; j++) {
                            if (array_str[i] == array_str[j]) {
                                msg = "false";
                                return false;
                            }
                        }
                    }
                    else {
                        if ($.trim(compareStr) == array_str[i]) {
                            msg = "false";
                            return false;
                        }
                    }

                }
            }
            return msg;
        }

        //修改
        function update_one(scid, categoryName) {
            var msg = "";
            AjaxPostAsync("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: "knowledgecategoryupdate", KCID: scid, name: categoryName,RegionID:<%=RegionID %>, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;
            });
            return msg;
        }

        //新增一行
        function layAddNewRow() {
            var groupName = $.trim($("#ddlGroup option:selected").text());

            var appendStr = "<tr class='back' onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
            appendStr += "<td></td>";
            appendStr += "<td><input type='text' style='width: 92%; margin-left: -3px;'></td>";
            appendStr += "<td></td>";
            appendStr += "<td>";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupSave2()\" SCID='-1' Name='a_save'>保存</a>&nbsp;";
            appendStr += "<a href='javascript:void(0);' onclick=\"groupCancel('-1',this)\">取消</a>&nbsp;";
            appendStr += "</td>";
            appendStr += "<td>";
            appendStr += "</td>";
            appendStr += "</tr>";
            $("#trList").append(appendStr);
            $("#trList").find("tr:last-child [type='text']").focus();
        }

        //保存。如果scid=-1：是新增；scid!=-1：则是编辑；type=1是全部保存，type=2是单个保存
        function groupSave2() {
            var categoryName = $.trim($("#trList").find("tr:last-child [type='text']").val());

            var txtEditText = "";
            $("#trList tr").each(function () {
                var $td0 = $(this).find("td:eq(1) input[type='text']");
                if (!$td0.is(":visible")) {
                    txtEditText += $.trim($td0.val()) + ",";
                }
            });

            if (categoryName == "") {
                $.jAlert("请输入分类名称");
                return false;
            }

            var isRepeatStr = isRepeat(categoryName, txtEditText.substr(1, txtEditText.length - 2));
            if (isRepeatStr == false) {
                $.jAlert("分类名称有重复，不能保存");
                return false;
            }

            if ($.jConfirm("是否确认添加该条记录？", function (r) {
                if (r) {
                    var msg = add_one(encodeURIComponent(categoryName));
                    $.jAlert(msg);
                    laySearchData();
                    refrashList();
                }
                else {
                    groupCancel(scid, othis);
                    return false;
                }
            }));
        }
        //新增
        function add_one(categoryName) {
            var msg = "";
            AjaxPostAsync("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: "KnowledgeCategoryInsert", KCID: lastpid, name: categoryName, Level: lastlevel,RegionID:<%=RegionID %>, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                msg = jsonData.msg;
            });
            return msg;
        }

        //删除
        function groupDelete(kcid) {
            if ($.jConfirm("是否确认删除(此操作同时会删除所有子分类)？", function (r) {
                if (r) {
                    $.post("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: "deleteknowledgecategoryandchildren", KCID: kcid, r: Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        $.jAlert(jsonData.msg, function () {
                            laySearchData();
                            refrashList();
                        });
                    });
                }
                else {
                    return false;
                }
            }));
        }

        //操作后更新列表数据
        function refrashList() {
            $.post("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: 'BindKnowledgeCategory', Level: lastlevel, KCID: lastpid,RegionID:<%=RegionID %>, r: Math.random() }, function (data) {
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $("#layerSelKCID" + (lastlevel)).children().remove();
                        $("#layerSelKCID" + (lastlevel)).append("<option value='-1'>请选择</option>");
                        $.each(jsonData.root, function (idx, item) {
                            $("#layerSelKCID" + (lastlevel)).append("<option value='" + item.kcid + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }

        function changeCategoryStatus(kcid, obj) {
            AjaxPostAsync("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx", { Action: "knowledgecategorystatuschange", KCID: kcid, r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.msg == "success") {
                    var tdStatus = $(obj).parent().parent().find("td:eq(2)");
                    var dtStatusText = $.trim(tdStatus.text());
                    if (dtStatusText == "在用") {
                        tdStatus.text("停用");
                        $(obj).text("启用");
                    }
                    else {
                        tdStatus.text("在用");
                        $(obj).text("停用");
                    }
                }
            });
        }
    </script>
    <input type="hidden" id="hidSearchType" value="1" />
    <div class="searchTj">
        <ul>
            <li>
                <label>
                    标题：</label>
                <input type="text" id="txtTitle" class="w200" />
            </li>
            <li>
                <label>
                    分类：</label>
                <select id="selKCID1" style="width: 101px;" class="w60" onchange="selKCIDChange(1)">
                    <option value='-1'>请选择</option>
                </select>
                <select id="selKCID2" class="w60" style="width: 100px;" onchange="selKCIDChange(2)">
                    <option value='-1'>请选择</option>
                </select>
            </li>
            <li style="width: 296px;">
                <label>
                    创建日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w95" />
                <span>-</span>
                <input type="text" name="EndTime" id="tfEndTime" class="w95" />
            </li>
        </ul>
        <ul>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreateUser" class="w200" runat="server" style="width: 206px;">
                </select>
            </li>
            <li>
                <label>
                    修改人：</label>
                <select id="selModifier" class="w200" runat="server" style="width: 206px;">
                </select>
            </li>
            <li>
                <label>
                    属性：</label>
                <span class="fxz">
                    <label>
                        <input type="checkbox" id="chkIsFAQ" value="2" name="chkProperty" onclick="chkProperty()" />
                        <em onclick="chkProperty();">有FAQ</em>
                    </label>
                    <label>
                        <input type="checkbox" id="chkIsQuestion" value="3" name="chkProperty" onclick="chkProperty()" />
                        <em onclick="chkProperty();">有试题</em>
                    </label>
                </span></li>
            <li class="btnsearch" style="width: 150px; clear: none; margin-top: 0px;">
                <input class="cx" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix" style="position: relative;">
        <a href="javascript:void(0)" onclick="javascript:loadHtml(1,this)" id="aList1" class="redColor">
            知识点</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="javascript:loadHtml(2,this)"
                id="aList2">FAQ</a>&nbsp;&nbsp;||&nbsp;&nbsp; <a href="javascript:void(0)" onclick="javascript:loadHtml(3,this)"
                    id="aList3">试题</a>&nbsp;&nbsp;
        <%if (right_btnCategory)
          { %>
        <input type="button" id="btnAdd" value="新增分类" class="newBtn" style="float: none;
            position: absolute; right: 120px;top:4px;" onclick="OpenCategory()" />
        <%} %>
        <%if (DataAddButton)
          {%>
        <input name="" type="button" style="float: none; position: absolute; right: 10px;top:4px;"
            value="新增知识点" onclick="javascript:OpenKnowledgeEdit(1)" class="newBtn" />
        <%}%>
    </div>
    <div id="ajaxTable">
    </div>
</asp:Content>
