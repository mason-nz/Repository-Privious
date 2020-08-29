<%@ Page Language="C#" Title="客服实时统计" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ServiceMonitoringList.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.ServiceMonitoringList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="refresh" content="10" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            search();
        });

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
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/ServiceMonitoringList.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtName = $.trim($("#txtName").val());
            if (txtName.length > 50) {
                msg += "客服字数太多<br/>";
                $("#txtName").val('');
            }
            return msg;
        }

        //获取参数
        function _params() {
            var txtName = encodeURIComponent($.trim($('#txtName').val()));
            var txtCode = encodeURIComponent($.trim($('#txtCode').val()));
            var ddGroup = encodeURIComponent($.trim($('#ddGroup').val()));
            var hidSelAgentId = encodeURIComponent($.trim($('#hidSelAgentId').val()));
            var status = $(":radio[name='radStatus']:checked").eq(0).val();
            var pody = {
                AgentNum: txtCode,
                UserID: hidSelAgentId,
                GroupID: ddGroup,
                Status: status,
                r: Math.random()  //随机数
            }

            return pody;
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
                        所属分组：</label>
                    <select class="w240" id="ddGroup" runat="server" clientidmode="Static">
                        <option value='-1'>请选择</option>
                    </select>
                </li>
                <li>
                    <label>
                        <input type="hidden" id="hidSelAgentId" value="" />
                        客服：</label><input name="" id="txtName" type="text" class="w240" readonly="true" onclick="SelectVisitPerson('txtName','hidSelAgentId')" /></li>
                <li>
                    <label>
                        工号：</label><input name="" type="text" id="txtCode" class="w240" />
                </li>
                <li style="width: 80px;">
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
        <div class="cxList cxList_chart">
            <div class="table_bt">
                <div class="time_xz">
                    <label>
                        <input name="radStatus" type="radio" value="" class="dx" onclick="search()" />全部</label><label><input
                            name="radStatus" type="radio" value="13" class="dx" checked="checked" onclick="search()" />非离线</label>
                    <span class="btn right" style="*margin-top: -25px;">
                        <input type="button" value="刷新" class="w60 gray" onclick="search()" /></span></div>
            </div>
            <div id="ajaxMessageInfo">
            </div>
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
