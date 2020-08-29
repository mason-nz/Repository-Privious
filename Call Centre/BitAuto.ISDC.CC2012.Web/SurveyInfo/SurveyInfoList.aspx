<%@ Page Language="C#" Title="调查问卷管理" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="SurveyInfoList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyInfoList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            $('#tfBeginTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfBeginTime\')}' }); });

            //敲回车键执行方法
            enterSearch(search);

            getUserGroup();
            selGroupChange();
            chkStatusChange();
            search();
        });

        //状态条件选择“已使用”，是否可用的选项显示，否则不显示;
        function chkStatusChange() {
            $("#IsAvailable").hide();
            $(":checkbox[name='chkIsAvailable']").attr("checked", false);
            if ($("#chkStatusUsed").attr("checked")) {
                $("#IsAvailable").show();
            }
        }

        //加载登陆人业务组
        function getUserGroup() {
            AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
                $("#<%=selGroup.ClientID %>").append("<option value='-1'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=selGroup.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }

        //根据选择的分组绑定对应的分类
        function selGroupChange() {
            $("#selCategory").children().remove();
            $("#selCategory").append("<option value='-1'>请选择分类</option>");
            if ($("#<%=selGroup.ClientID %>").val() != "-1") {
                AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selGroup.ClientID %>").val(), r: Math.random() }, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selCategory").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                    }
                });
            }
        }


        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("../AjaxServers/SurveyInfo/SurveyInfoList.aspx?r=" + Math.random() + " .bit_table > *", podyStr);
        }

        //获取参数
        function _params() {

            var beginTime = encodeURIComponent($.trim($("#tfBeginTime").val()));

            var endTime = encodeURIComponent($.trim($("#tfEndTime").val()));

            if ((beginTime != "" && !beginTime.isDate()) || (endTime != "" && !endTime.isDate())) {
                $.jAlert("输入的时间格式不正确");
                return false;
            }

            var name = encodeURIComponent($.trim($("#txtName").val()));

            var status = $(":checkbox[name='chkStatus']:checked").map(function () {
                return $(this).val()
            }).get().join(',');

            var isAvailable = $(":checkbox[name='chkIsAvailable']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var group = "";
            if ($("#<%=selGroup.ClientID %>").val() != "-1" && $("#<%=selGroup.ClientID %>").val() != undefined) {
                group = $("#<%=selGroup.ClientID %>").val();
            }
            var category = "";
            if ($("#selCategory").val() != "-1" && $("#selCategory").val() != "") {
                category = $("#selCategory").val();
            }

            var creater = "";
            if ($("#<%=selCreater.ClientID %>").val() != "-1" && $("#<%=selCreater.ClientID %>").val() != "") {
                creater = $("#<%=selCreater.ClientID %>").val();
            }

            var pody = {
                name: name,
                status: status,
                isAvailable: isAvailable,
                group: group,
                category: category,
                creater: creater,
                beginTime: beginTime,
                endTime: endTime
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('../AjaxServers/SurveyInfo/SurveyInfoList.aspx?r=' + Math.random() + ' .bit_table > *', pody);
        }

        //弹出层-分类管理
        function OpenCategory() {
            $.openPopupLayer({
                name: "addCategory",
                parameters: {},
                url: "SurveyCategoryManage.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            });
        }

        //列表-删除、停用、启用
        function ajaxPager_update(siid, status, isAvailable, alertMsg) {

            if (status == null) {
                status = "";
            }

            if (isAvailable == null) {
                isAvailable = "";
            }

            if ($.jConfirm(alertMsg, function (r) {
                if (r) {
                    $.post("../AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "SurveyInfoUpdate", SIID: siid, Status: status, IsAvailable: isAvailable, r: Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.msg = "操作成功") {
                            $.jPopMsgLayer(jsonData.msg, function () {
                                search();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg, function () {
                                search();
                            });
                        }
                    });
                }
            }));

        }


        //生成新问卷
        function ajaxPager_createNewQuestionPaper(siid) {
            if ($.jConfirm("是否确认将问卷生成新问卷？", function (r) {
                if (r) {
                    $.post("../AjaxServers/SurveyInfo/SurveyInfoListHandle.ashx", { Action: "SurveyCreateNewQuestionPaper", SIID: siid, r: Math.random() }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.msg == "success") {
                            $.jPopMsgLayer("操作成功");
                            search();
                        }
                        else {
                            alert(jsonData.msg);
                        }

                    });
                }
            }));
        }

        //添加按钮链接
        function add_SurveyInfo() {

            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/SurveyInfo/SurveyInfoEdit.aspx');
            }
            catch (e) {
                window.open("/SurveyInfo/SurveyInfoEdit.aspx");
            }
        }
         
    </script>
    <div class="search clearfix">
        <ul>
            <li>
                <label>
                    问卷名称：</label>
                <input type="text" id="txtName" class="w190" />
            </li>
            <li>
                <label>
                    创建日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
                至
                <input type="text" name="EndTime" id="tfEndTime" class="w85" style="width: 84px;
                    *width: 83px; width: 83px\9;" />
            </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreater" runat="server" class="w200" style="width: 206px">
                </select>
            </li>
        </ul>
        <ul>
            <li style="width: 284px;">
                <label>
                    分类：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w90"
                    style="width: 98px; *width: 100px; width: 100px\9">
                </select>
                <select id="selCategory" class="w90">
                </select>
            </li>
            <li>
                <label>
                    状态：</label>
                <span>
                    <input type="checkbox" id="chkStatusUnfinished" value="0" name="chkStatus" /><em
                        onclick='emChkIsChoose(this);'>未完成&nbsp;</em></span> <span>
                            <input type="checkbox" id="chkStatusUnused" value="1" name="chkStatus" /><em onclick='emChkIsChoose(this);'>未使用&nbsp;</em></span>
                <span>
                    <input type="checkbox" id="chkStatusUsed" value="2" name="chkStatus" onclick="chkStatusChange()" /><em
                        onclick='emChkIsChoose(this);chkStatusChange();'>已使用</em></span> </li>
            <li id="IsAvailable">
                <label style="width: 86px; *width: 86px; width: 91px\9;">
                    是否可用：</label>
                <span>
                    <input type="checkbox" id="chkIsAvailableYes" value="1" name="chkIsAvailable" /><em
                        onclick='emChkIsChoose(this)'>启用</em></span> <span>
                            <input type="checkbox" id="chkIsAvailableNo" value="0" name="chkIsAvailable" /><em
                                onclick='emChkIsChoose(this)'>停用</em></span> </li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_btnCategory)
          { %>
        <input type="button" id="btnCategory" value="分类管理" class="newBtn" onclick="OpenCategory()" />
        <%} %>
        <%if (right_btnAdd)
          { %>
        <input type="button" id="btnAdd" value="添加" class="newBtn" onclick="add_SurveyInfo()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
</asp:Content>
