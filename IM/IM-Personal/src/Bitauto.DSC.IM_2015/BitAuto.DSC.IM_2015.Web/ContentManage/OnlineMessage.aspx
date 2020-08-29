<%@ Page Title="在线留言" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="OnlineMessage.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.ContentManage.OnlineMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });
        });

        $(function () {
            search();
        })

        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx", podyStr, function () { });
        }

        //查询
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtName = $.trim($("#txtName").val());
            if (txtName.length > 50) {
                msg += "访客名称字数太多<br/>";
                $("#txtJxsName").val('');
            }
            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 20) {
                msg += "操作人名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "留言时间格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "留言时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "留言时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtName = encodeURIComponent($.trim($('#txtName').val()));
            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var selMessageState = encodeURIComponent($.trim($('#selMessageState').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));
            var typeId = $.trim($('#ddTypeId').val());
            var sourceType = $.trim($('#<%=ddlSourceType.ClientID%>').val());

            var pody = {
                UsertName: txtName,
                LastModifyUserName: txtAgentName,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                MessageState: selMessageState,
                TypeId: typeId,
                SourceType: sourceType,
                r: Math.random()  //随机数
            }

            return pody;
        }


        function changeMessageState(obj) {
            var selValue = $(obj).val();
            var firstValue = $(obj).find(" option").eq(0).val();
            if (firstValue != selValue) {
                $.jConfirm("您确认要执行此操作吗？", function (isOk) {
                    if (isOk) {

                        var selRecID = $(obj).attr("name");
                        $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'addremarkinfo', RemarkRecID: encodeURIComponent(selRecID), MessageStatus: encodeURIComponent(selValue), r: Math.random() }, function (data) {
                            if (data == "success") {
                                $.jAlert("状态修改成功！");

                                reloadpagedata();     //--------------------------------------------------------------------------
                            }
                            else {
                                $.jAlert("操作出现异常，请稍后再试！");
                            }
                        });
                    }
                    else {
                        $(obj).find(" option").removeAttr("selected").eq(0).attr("selected", "selected");
                    }
                });
            }

        }
        function OpenAddRemarkLayer(RecID, CType) {

            $.openPopupLayer({
                name: "AddRemarkLayerAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ContentManage/AddRemarkForm.aspx?RecID=" + RecID + "&CType=" + CType + "&r=" + Math.random()
            });

        };

        function OpenContentDetailLayer(RecID) {
            $.openPopupLayer({
                name: "ContentDetailLayerAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ContentManage/ContentsDetailForm.aspx?RecID=" + RecID + "&r=" + Math.random()
            });
        }
        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/ContentManage/OnlineMessageExport.aspx?" + podyStr;
        }
    </script>
    <script type="text/javascript">
        function reloadpagedata() {
            var page = $("#ajaxMessageInfo #itPage a[class='active']").text();
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx?page=" + page + "&", podyStr);
            }
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        访客名称：</label><input name="" id="txtName" type="text" class="w240" /></li>
                <li>
                    <label>
                        访客来源：</label>
                    <select class="w240" id="ddlSourceType" runat="server">
                    </select>
                </li>
                <li>
                    <label>
                        时间：</label><input name="" id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input name="" id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" value="查询" onclick="search()" class="w60" /></div>
                </li>
                <li>
                    <label>
                        操作人：</label><input name="" type="text" id="txtAgentName" class="w240" />
                </li>
                <li>
                    <label>
                        咨询类型：</label>
                    <select class="w240" id="ddTypeId">
                        <option value='-1'>请选择</option>
                        <option value='1'>购车咨询</option>
                        <option value='2'>卖车咨询</option>
                        <option value='3'>活动咨询</option>
                        <option value='4'>网站建设</option>
                        <option value='5'>其他</option>
                    </select>
                </li>
                <li>
                    <label>
                        状态：</label>
                    <select class="w240" id="selMessageState">
                        <option value='-1'>请选择</option>
                        <option value='1'>新建</option>
                        <option value='2'>处理中</option>
                        <option value='3'>已完成</option>
                    </select>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div id="ajaxMessageInfo" class="cxList" style="margin-top: 8px; height: auto;">
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
