<%@ Page Title="常见问题管理" Language="C#" AutoEventWireup="true" CodeBehind="FreProblemList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.FreProblemList" MasterPageFile="~/Controls/Top.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.tmpl.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //查询数据
            Search();
            GetSourceType();

        });
        function SetMoreURL() {
            $.openPopupLayer({
                name: "AddFreProblem",
                url: "/SeniorManage/MoreUrlConfig.aspx?r=" + Math.random(),
                beforeClose: function (e) {
                    //                    if (e) {
                    //                        Search();
                    //                    }
                }
            });
        }
        //查询数据
        function Search() {
            LoadingAnimation("dataList");
            var podyStr = JsonObjToParStr(GetParams());
            $("#dataList").load("/AjaxServers/SeniorManage/FreProblemList.aspx?r=" + Math.random(), podyStr, function () { });
        }

        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("dataList");
            $("#dataList").load("/AjaxServers/SeniorManage/FreProblemList.aspx?r=" + Math.random(), podyStr, function () { });
        }

        //新增数据
        function AddFreProblem() {
            //校验总数
            var pody = {
                Action: 'check',
                r: Math.random
            };
            AjaxPostAsync("/AjaxServers/SeniorManage/FreProblemHandler.ashx", pody, function () { }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "success") {
                    $.openPopupLayer({
                        name: "AddFreProblem",
                        url: "/SeniorManage/AddFreProblem.aspx?r=" + Math.random(),
                        beforeClose: function (e) {
                            if (e) {
                                Search();
                            }
                        }
                    });
                }
                else {
                    if (jsonData.msg == "") {
                        $.jAlert("常见问题数量已达20条上限，无法继续创建！");
                    }
                    else {
                        $.jAlert("校验错误：" + jsonData.msg);
                    }
                }
            });
        }
        //获取参数
        function GetParams() {
            var title = encodeURIComponent($.trim($('#txtTitle').val()));
            var remark = encodeURIComponent($.trim($('#txtRemark').val()));
            var sourceType = "";
            if (!$("#sourceType").attr("checked")) {
                sourceType = $(":checked[name='sourceType']").map(function () { return $(this).val() }).get().join(",");
            }
            var pody = {
                Title: title,
                Remark: remark,
                SourceType: sourceType,
                r: Math.random()  //随机数
            }
            return pody;
        }

        function GetSourceType() {
            AjaxPost("/AjaxServers/SeniorManage/FreProblemHandler.ashx", { Action: 'GetSourceType' }, function () { }, function (data) {
                if (data.result) {

                    $("#ulSearch li").last().after($("#tmp").tmpl(data));
                }
            });
        }

        function CheckAll(name) {
            if ($("#" + name).attr("checked")) {
                $("input[name='" + name + "']").each(function () {
                    $(this).attr("checked", "checked");
                });
            }
            else {
                $("input[name='" + name + "']").each(function () {
                    $(this).removeAttr("checked", "checked");
                });
            }
        }


        function Check(name, obj) {
            var len = $("[name='" + name + "']:checked").length;
            if ($(obj).attr("checked") && len == $("[name='" + name + "']").length) {
                $("#" + name).attr("checked", "checked")
            }
            else {
                $("#" + name).removeAttr("checked", "checked");
            }
        }
    </script>
    <div class="content">
        <!--列表开始-->
        <div class="searchTj">
            <ul id="ulSearch">
                <li>
                    <label>
                        标题：</label><input name="" id="txtTitle" type="text" class="w240" /></li>
                <li>
                    <label>
                        备注：</label><input name="" id="txtRemark" type="text" class="w240" /></li>
                <li>
                    <div class="tjBtn">
                        <input type="button" id="bt_search" onclick="javascript:Search()" value="查询" class="w60" /></div>
                </li>
                <li style="width: 85px; clear: left;">
                    <label>
                        应用业务线：
                    </label>
                </li>
                <li style="width: 85px;"><span>
                    <label>
                        <input type="checkbox" value="" id="sourceType" onclick="CheckAll('sourceType')" /><em>全部</em></label></span>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <div class="cxList" style="margin-top: 20px; margin-bottom: 0px;">
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <th colspan="7" style="background: #F9F9F9">
                        <div class="btn btn2 right">
                            <%if (BitAuto.DSC.IM_2015.BLL.Util.CheckButtonRight("SYS0320MOD400401"))
                              {%>
                            <input id="btn_MoreURL" type="button" value="更多" class="cancel w60 gray" onclick="SetMoreURL()" />&nbsp;<%} %><input
                                id="btn_add" type="button" value="新增" class="cancel w60 gray" onclick="AddFreProblem()" />
                        </div>
                    </th>
                </thead>
            </table>
        </div>
        <div id="dataList" class="cxList" style="margin-top: -1px;">
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <script id="tmp" type="text/x-jquery-tmpl"> 
       {{each json}}
              <li style="width:85px;"><span>
                    <label>
                        <input name="sourceType" type="checkbox" onclick="Check('sourceType',this)"  value="${SourceTypeValue}"/><em>${SourceTypeName}</em></label></span> </li>
       {{/each}}
    </script>
</asp:Content>
