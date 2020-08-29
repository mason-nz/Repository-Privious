<%@ Page Title="日志查看" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="UserActionLog.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.ContentManage.UserActionLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });
            $('#txtStartTime').val(setDealTime());
            $('#txtEndTime').val(setDealTime());
            search();
        });

        function setDealTime() {
            var oDate = new Date();
            function addZero(n) {
                return n < 10 ? '0' + n : n;
            }
            return [oDate.getFullYear(), addZero(oDate.getMonth() + 1), addZero(oDate.getDate())].join('-');
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
                $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/UserActionLogList.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";

            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 50) {
                msg += "操作人名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "操作时间格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "操作时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "操作时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtJxsName = encodeURIComponent($.trim($('#txtJxsName').val()));
            var selLogInType = encodeURIComponent($.trim($('#selLogInType').val()));
            var selOperUserType = encodeURIComponent($.trim($('#selOperUserType').val()));
            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var selMessageState = encodeURIComponent($.trim($('#selMessageState').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));


            var pody = {
                LogInfo: txtJxsName,
                selLogInType: selLogInType,
                OperUserType: selOperUserType,
                OperUserName: txtAgentName,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                r: Math.random()  //随机数
            }

            return pody;
        }

        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/UserActionLogList.aspx", pody, null);
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        日志内容：</label><input name="" id="txtJxsName" type="text" class="w240" /></li>
                <li>
                    <label>
                        日志类型：</label><select class="w240" id="selLogInType"><option value='-1'>请选择</option>
                            <option value='1'>坐席断网超时</option>
                            <option value='9'>网友断网超时</option>
                            <option value='2'>网友不发消息超时</option>
                            <option value='4'>客户登录</option>
                            <option value='5'>客户退出</option>
                            <option value='6'>坐席登录</option>
                            <option value='10'>坐席修改状态</option>
                            <option value='8'>给坐席新增聊天客户</option>
                            <option value='7'>坐席退出</option>
                            <option value='3'>系统异常记录</option>
                        </select></li>
                <li>
                    <label>
                        记录人类型：</label><select class="w240" id="selOperUserType"><option value='-1'>请选择</option>
                            <option value='1'>坐席</option>
                            <option value='2'>网友</option>
                            <option value='3'>系统</option>
                        </select></li>
                <li>
                    <label>
                        操作人：</label><input name="" type="text" id="txtAgentName" class="w240" />
                </li>
                <li>
                    <label>
                        操作时间：</label><input name="" id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input name="" id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
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
        <div id="ajaxMessageInfo" class="cxList" style="margin-top: 8px; height: auto;">
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
