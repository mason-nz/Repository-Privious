<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComSenManage.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.ComSenManage"
    MasterPageFile="~/Controls/Top.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>常用语管理</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            enterSearch(search);
            search();
        });

        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            //LoadingAnimation("mycontent");
            $("#mycontent").load("/AjaxServers/SeniorManage/ComSenManageList.aspx", podyStr, function () {

            });

        }

        //分页操作
        function ShowDataByPost1(pody) {
            //LoadingAnimation("tableList");
            $('#mycontent').load('/AjaxServers/SeniorManage/ComSenManageList.aspx', pody + "&r=" + Math.random());
        }

        //获取参数
        function _params() {

            var csName = encodeURIComponent($.trim($("#csName").val()));

            var ltName = encodeURIComponent($.trim($("#ltName").val()));
            
            var pageSize = $("#hidSelectPageSize").val();

            var pody = {
                CSName: csName,
                LTName: ltName,                
                pageSize: pageSize,                
                r: Math.random()
            }

            return pody;
        }

        function DeleteConSentence(obj) {
            var csid = $(obj).attr("csid");
            $.jConfirm("确定删除此常用语吗？", function (r) {
                if (r) {
                    var pody = {
                        Action: 'DeleteConSentence',
                        CSID: csid,
                        r: Math.random
                    };
                    AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", pody, function () { }, function (data) {
                        var jsonData = $.evalJSON(data);
                        if (jsonData.result == "yes") {
                            //Search();
                            window.location.reload();
                        }
                        else {
                            $.jAlert("删除失败：" + jsonData.msg);
                        }
                    });
                }
            });


        }
        

        //移动
        function MoveUpOrDown(csid, dir) {
            var csName = encodeURIComponent($.trim($("#csName").val()));

            var ltName = encodeURIComponent($.trim($("#ltName").val()));
            var pody = {
                Action: 'MoveUpOrDown',
                CSID: csid,
                Direct: dir,
                CSName: csName,
                LTName: ltName,
                r: Math.random()
            };
            AjaxPostAsync("/AjaxServers/SeniorManage/ComSenManageHandler.ashx", pody, function () { }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    search();                   
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
            
        }

        function AddConSentencePop() {
            $("#hidIsEditCS").val("");
            $("#hidLTID").val("");
            $("#hidCSID").val("");
            $("#hidCSName").val("");

            $.openPopupLayer({
                name: "AddConSentencePop",
                parameters: {},
                url: "/SeniorManage/AddConSentence.aspx",
                beforeClose: function (e) {
                    //window.location.reload();
                }

            });
        }

        function EditConSentencePop(obj) {
            $("#hidIsEditCS").val(1);
            $("#hidLTID").val($(obj).attr("ltid"));
            $("#hidCSID").val($(obj).attr("csid"));            
            var currentTR = $(obj).parent().parent();
            $("#hidCSName").val($.trim(currentTR.find("td:eq(0)").text()));
            
            $.openPopupLayer({
                name: "AddConSentencePop",
                parameters: {},
                url: "/SeniorManage/AddConSentence.aspx",
                beforeClose: function (e) {
                    window.location.reload();
                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                }

            });
        }

        function LabelEditPop() {
            $.openPopupLayer({
                name: "LabelEditPop",
                parameters: {},
                url: "/SeniorManage/LabelEdit.aspx",
                beforeClose: function (e) {

                },
                afterClose: function () {
                    //敲回车键执行方法
                    //enterSearch(search);
                }

            });
        }
    </script>
    <input type="hidden" id="hidSelectPageSize" value="20" />
    <input type="hidden" id="hidIsEditCS" value="" />
    <input type="hidden" id="hidLTID" value="" />
    <input type="hidden" id="hidCSID" value="" />
    <input type="hidden" id="hidCSName" value="" />
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        常用语：</label><input id="csName" name="" type="text" class="w240" /></li>
                <li>
                    <label>
                        标签：</label><input id="ltName" name="" type="text" class="w240" /></li>
                <li>
                    <div class="tjBtn">
                        <input type="button" onclick="javascript:search()" value="查询" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div class="cxList" style="margin-top: 8px; height: auto;" id="mycontent">           
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
</asp:Content>
