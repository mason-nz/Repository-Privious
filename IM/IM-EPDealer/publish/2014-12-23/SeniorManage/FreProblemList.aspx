<%@ Page Title="常见问题管理" Language="C#" AutoEventWireup="true" CodeBehind="FreProblemList.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.FreProblemList" MasterPageFile="~/Controls/Top.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //查询数据
            Search();
        });
        //查询数据
        function Search() {
            LoadingAnimation("dataList");
            $("#dataList").load("/AjaxServers/SeniorManage/FreProblemList.aspx?r=" + Math.random());
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
    </script>
    <div class="content">
        <!--列表开始-->
        <div class="cxList" style="margin-top: 20px; margin-bottom: 0px;">
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <th colspan="7" style="background: #F9F9F9">
                        <div class="btn btn2 right">
                            <input id="btn_add" type="button" value="新增" class="cancel w60 gray" onclick="AddFreProblem()" />
                        </div>
                    </th>
                </thead>
            </table>
        </div>
        <div id="dataList" class="cxList" style="margin-top: -1px;">
            <table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th width="35%" class="cName">
                        标题
                    </th>
                    <th width="45%" class="cName">
                        链接地址
                    </th>
                    <th width="15%">
                        操作
                    </th>
                </tr>
            </table>
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
</asp:Content>
