<%@ Page Language="C#" Title="客服状态明细" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="AgentStatusList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.AgentStatusList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/AgentStatusList.aspx", podyStr, function () { });
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
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/AgentStatusList.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtName = $.trim($("#txtName").val());
            if (txtName.length == 0) {
                msg += "客服不可以为空<br/>";
            }

            if (txtName.length > 50) {
                msg += "客服字数太多<br/>";
                $("#txtName").val('');
            }

            var beginTime = $.trim($("#txtStartTime").val());

            if (beginTime == "") {
                msg += "日期不可以为空<br/>";
            }
            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "日期格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }
            return msg;
        }

        //获取参数
        function _params() {
            var txtName = encodeURIComponent($.trim($('#txtName').val()));
            var txtCode = encodeURIComponent($.trim($('#txtCode').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var ddStatus = encodeURIComponent($.trim($('#ddStatus').val()));
            var hidSelAgentId = encodeURIComponent($.trim($('#hidSelAgentId').val()));
            var pody = {
                UserName: txtName,
                Code: txtCode,
                Starttime: txtStartTime,
                UserID: hidSelAgentId,
                Status: ddStatus,
                r: Math.random()  //随机数
            }

            return pody;
        }
        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/TrailManager/AgentStatusList.aspx?export=1&" + podyStr;
        }

        // 客服弹层
        function SelectVisitPerson(txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                //IsHaveCompetence是否有数据权限，1是有数据权限，0没有
                parameters: { IsHaveCompetence: "11" },
                url: "/AjaxServers/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='" + txtName + "']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#" + hidId).val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                    $("#txtCode").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('agentnum'));
                },
                afterClose: function () {
                }
            });
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        <input type="hidden" id="hidSelAgentId" value="-2" />
                        客服：</label><input name="" id="txtName" type="text" class="w240" readonly="true" onclick="SelectVisitPerson('txtName','hidSelAgentId')" /></li>
                <li>
                    <label>
                        工号：</label><input name="" type="text" id="txtCode" class="w240" />
                </li>
                <li>
                    <label>
                        时间：</label><input name="" id="txtStartTime" type="text" class="w240" onfocus="WdatePicker({isShowClear:true,readOnly:true,dateFmt:'yyyy-MM-dd'})" />
                </li>
                <li>
                    <label>
                        客服状态：</label>
                    <select class="w240" id="ddStatus">
                        <option value='-2'>请选择</option>
                        <option value='1'>在线</option>
                        <option value='3'>暂离</option>
                        <option value='2'>离线</option>
                    </select>
                </li>
                <li  class="btn_cx">
                    <div class="tjBtn">
                        <input type="button" value="查询" onclick="search()" class="w60" /></div>
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
