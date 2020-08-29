<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true" CodeBehind="LabelManage.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.LabelManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>标签配置管理</title>
<script type="text/javascript">
    function LabelConfigPop(obj) {
        var bgid = $(obj).attr("bgid");
        $.openPopupLayer({
            name: "LabelConfigPop",
            parameters: {BGID:bgid},
            url: "/SeniorManage/LabelConfig.aspx",
            beforeClose: function (e) {
                //window.location.reload();
            }
        });
    }

    function search() {
        
        //LoadingAnimation("mycontent");
        $("#mycontent").load("/AjaxServers/SeniorManage/LabelManageList.aspx", "&r=" + Math.random(), function () {

        });

    }
    //移动
    function MoveUpOrDown(ltid, dir) {
        var pody = {
            Action: 'MoveUpOrDown',
            LTID: ltid,
            Direct: dir,
            r: Math.random
        };
        AjaxPostAsync("/AjaxServers/SeniorManage/LabelManageHandler.ashx", pody, function () { }, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "yes") {
                search2();
                search();
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });

    }

    function search2() {

        //LoadingAnimation("mycontent");
        $("#divConfig").load("/AjaxServers/SeniorManage/LabelConfigList.aspx", { BGID: $("#selGroup").val() }, function () {

        });

    }

    //标签配置管理弹出层 复选框绑定全选设置
    function initConfigPop() {        
        $("#chkAll").bind("click", function () {
            if ($("#chkAll").attr("checked")) {
                //全选
                $(".fzList td input[type='checkbox']").each(function (i) {
                    $(this).attr("checked", 'true');
                });
            }
            else {               
                //取消全选
                $(".fzList td input[type='checkbox']:checked").each(function (i) {
                    $(this).removeAttr("checked");
                });
            }
        });
    }

    function Save2DB() {
        var bgid = $("#selGroup").val();
        var ltids = $(".fzList td input[type='checkbox']:checked").map(function () {
            return $(this).attr("ltid");
        }).get().join(",");

        if (ltids == "") {
            $.jAlert("请至少选择一个标签!");
            return;
        }

        var pody = {
            Action:"Save2DBGroupLabel",
            BGID:bgid,
            LTIDS: ltids,
            r: Math.random
        };

        AjaxPostAsync("/AjaxServers/SeniorManage/LabelManageHandler.ashx", pody, function () { }, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "yes") {
                $.jAlert(jsonData.msg);
                search();
                $.closePopupLayer('LabelConfigPop', true);
            }
            else {
                $.jAlert(jsonData.msg);
            }
        });
    }

    $(document).ready(function () {
        enterSearch(search);
        search();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="content">
    <!--列表开始-->
    <div id="mycontent" class="cxList" style="margin-top:20px;">
    	<%--<table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <th width="30%">所属分组</th>
            <th width="50%">标签</th>
            <th width="15%">操作</th>
          </tr>
          <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td class="cName" name="csName">
                                    <%#Eval("Name") %>&nbsp;
                                </td>
                                <td name="ltName">
                                    <%#Eval("ltNames")%>&nbsp;
                                </td>
                                <td>
                                    <a href="javascript:void(0)" csid="" onclick="EditConSentencePop(this)">修改</a>                                     
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>         
        </table>--%>
    </div>
    <!--列表结束-->
    </div>
</asp:Content>
