<%@ Page Title="标签管理" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="TagManagement.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.TagManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">

        var BodyParam = null;
        $(document).ready(function () {

            GetBusinessType();

            BindData();

            //敲回车键执行方法
            enterSearch(search);

        });

        function search() {

            BindData();
        }

        function BindData() {
            LoadingAnimation('ajaxTable');
            //状态
            var status = "";
            if ($('#ckAvalibile0').is(':checked')) {
                status += $('#ckAvalibile0').attr("value") + ",";
            }
            if ($('#ckAvalibile1').is(':checked')) {
                status += $('#ckAvalibile1').attr("value") + ",";
            }
            if (status.length > 0) {
                status = status.substring(0, status.length - 1);
            }

            var busitypeid = $("#sel_busiType").val();

            var param = "busitypeid=" + busitypeid + "&status=" + status + "&r=" + Math.random();
            $("#ajaxTable").load("/WOrderV2/AjaxServers/TagManagementList.aspx", param, function () { });

            BodyParam = param;
        }

        function AddBusinessType() {

            $.openPopupLayer({
                name: "BusinessTypeLayer",
                parameters: { r: Math.random() },
                url: "/WOrderV2/PopLayer/BusinessTypeLayer.aspx",
                beforeClose: function (e, data) {
                    GetBusinessType();
                }
            });
        }

        function AddTagLayer(level, pid, busitypeid) {
            $.openPopupLayer({
                name: "TagLayer",
                parameters: { level: level, pid: pid, busitypeid: busitypeid, r: Math.random() },
                url: "/WOrderV2/PopLayer/TagLayer.aspx?r=" + Math.random(),
                beforeClose: function (e, data) {
                    ShowDataByPost1(BodyParam);
                }
            });
        }

        function GetBusinessType() {
            var data = { action: "GetSelectData", r: Math.random() };
            AjaxPostAsync("/WOrderV2/Handler/BusinessTypeHandler.ashx", data, null, function (data) {
                if (data) {
                    data = JSON.parse(data);
                    if (data.Success == true) {
                        var jsonData = data.Data;
                        if (jsonData != null) {
                            $("#sel_busiType").empty();
                            //  $("#sel_busiType").append("<option value='-1'>请选择</option>");
                            var strselect = "";
                            for (var i = 0; i < jsonData.length; i++) {
                                strselect = "";
                                if (jsonData[i].Selected && jsonData[i].Selected == true) {
                                    strselect = "selected='selected'";
                                }
                                $("#sel_busiType").append("<option value=" + jsonData[i].Id + " " + strselect + ">" + jsonData[i].Name + "</option>");
                            }

                        }

                    } else {
                        // $.jAlert(data.Message);
                    }
                }

            });


        }

        function ShowDataByPost1(body) {
            LoadingAnimation("ajaxTable");
            BodyParam = body;
            $("#ajaxTable").load("/WOrderV2/AjaxServers/TagManagementList.aspx", body, function () { });
        }
     
    </script>
    <div class="search">
        <ul>
            <li>
                <label>
                    业务类型：</label><span>
                        <select id="sel_busiType" class="w180">
                        </select></span></li>
            <li>
                <label>
                    状态：</label>
                <span>
                    <input type="checkbox" name="chkStatus" id="ckAvalibile0" value="1" checked="checked" /><em
                        onclick="emChkIsChoose(this);">在用</em>
                    <input type="checkbox" name="chkStatus" id="ckAvalibile1" value="0" /><em onclick="emChkIsChoose(this);">停用</em>
                </span></li>
            <li class="btnsearch" style="padding: 5px; margin-top: -5px; margin-left: 0px; margin-right: 0px;">
                <div style="width: 310px;">
                    <input type="button" name="Search" value="查询" onclick="search()" style="float: right">
                </div>
            </li>
        </ul>
    </div>
    <div class="clearfix">
    </div>
    <div class="optionBtn clearfix">
        <%if (IsRightTagEdit)
          { %>
        <input type="button" id="btnAdd" value="新增一级标签" class="newBtn" onclick="AddTagLayer('1','0')" />
        <%} %>
        <%if (IsRigthBusiTypeEdit)
          { %>
        <input type="button" id="btnCategory" value="新增业务类型" class="newBtn" onclick="AddBusinessType()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <script>

      
    </script>
</asp:Content>
