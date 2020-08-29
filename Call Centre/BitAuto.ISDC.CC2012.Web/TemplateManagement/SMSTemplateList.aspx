<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="短信模板管理" CodeBehind="SMSTemplateList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.SMSTemplateList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);
            //加载分组
            getUserGroup();
            //根据选择的分组绑定对应的分类
            selGroupChange();
            //查询
            search();
        });

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
                AjaxPostAsync("../AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#<%=selGroup.ClientID %>").val(), TypeId: "2", r: Math.random() }, null, function (data) {
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
            $("#ajaxTable").load("SMSList.aspx .bit_table > *", podyStr);
        }

        //获取参数
        function _params() {

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
            var smstitle = "";
            smstitle = $.trim($("#txtTitle").val());
            var pody = {
                group: group,
                category: category,
                creater: creater,
                SMSTitle: smstitle,
                r: Math.random()
            }
            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('SMSList.aspx .bit_table > *', pody);
        }
        //删除
        function deleteTemplate(recID) {
            var url = "/AjaxServers/TemplateManagement/SMSTemplateEdit.ashx";
            if ($.jConfirm("是否确认删除该短信模板？", function (r) {
                if (r) {
                    $.blockUI({ message: '正在执行，请等待...' });
                    $.post(url, { Action: "Delete", RecID: recID, r: Math.random() }, function (data) {
                        $.unblockUI();
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "true") {
                            $.jPopMsgLayer("操作成功!", function () {
                                search();
                            });
                        }
                        else {
                            $.jAlert(jsonData.msg);
                        }
                    });
                }
            }));
        }
        function AddTemplate() {
            $.openPopupLayer({
                name: "updateSMSTemplate",
                url: "SMSTemplateEdit.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                    }
                }
            });
        }
        function EditTemplate(RecID) {
            $.openPopupLayer({
                name: "updateSMSTemplate",
                parameters: { RecID: RecID },
                url: "SMSTemplateEdit.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                    }
                }
            });
        }
        function ViewTemplate(RecID) {
            $.openPopupLayer({
                name: "ViewSMSTemplate",
                parameters: { RecID: RecID },
                url: "SMSTemplateView.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    所属分组：</label>
                <select id="selGroup" onchange="javascript:selGroupChange()" runat="server" class="w90"
                    style="width: 128px; *width: 130px; width: 130px\9">
                </select>
                <select id="selCategory" class="w90" style="width: 98px; *width: 100px; width: 100px\9">
                </select>
            </li>
            <li>
                <label>
                    模板标题：</label>
                <input type="text" id="txtTitle" class="w160" />
            </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreater" runat="server" class="w125" style="width: 128px">
                </select>
            </li>
            <li class="btnsearch">
                <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_btnAdd)
          { %>
        <input type="button" id="btnCategory" value="新增模板" class="newBtn" onclick="AddTemplate()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
</asp:Content>
