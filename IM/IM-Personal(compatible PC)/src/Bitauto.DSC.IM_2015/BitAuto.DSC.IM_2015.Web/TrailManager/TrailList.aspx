<%@ Page Title="客服统计" Language="C#" AutoEventWireup="true" CodeBehind="TrailList.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.TrailList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });


            var nowDate = GetShortNowDate();
            $('#txtStartTime').val(nowDate);
            $('#txtEndTime').val(nowDate);
            search(1);

        });
        //当前日期
        function GetShortNowDate() {
            var data = new Date();
            var nowYear = data.getFullYear();
            var nowMonth = data.getMonth() + 1;
            var nowDay = data.getDate();
            var nowDate = nowYear + "-" + (nowMonth.toString().length == 1 ? ("0" + nowMonth) : nowMonth) + "-" + (nowDay.toString().length == 1 ? ("0" + nowDay) : nowDay)
            return nowDate;
        }


        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/TrailList.aspx", podyStr, function () { });
        }

        //查询
        var type = 1;
        function search(selectType) {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                if (selectType) {
                    type = selectType;
                }
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/TrailList.aspx", podyStr);
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

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime == "" || endTime == "") {
                msg += "日期不可以为空<br/>";
            }

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }



            return msg;
        }

        //获取参数
        function _params() {
            var txtName = encodeURIComponent($.trim($('#txtName').val()));
            var txtCode = encodeURIComponent($.trim($('#txtCode').val()));
            var ddGroup = encodeURIComponent($.trim($('#ddGroup').val()));
            var hidSelAgentId = encodeURIComponent($.trim($('#hidSelAgentId').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));

            var pody = {
                AgentNum: txtCode,
                UserID: hidSelAgentId,
                GroupID: ddGroup,
                Starttime: txtStartTime,
                EndTime: txtEndTime,
                SelectType: type,
                r: Math.random()  //随机数
            }

            return pody;
        }

        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/TrailManager/TrailList.aspx?export=1&" + podyStr;
        }

        function SelectVisitPerson(txtName, hidId) {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                //IsHaveCompetence是否有数据权限，1是有数据权限，0没有
                parameters: { IsHaveCompetence: "1", BGID: escape($('#<%=ddGroup.ClientID %>').val()) },
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


        function Change(selectType) {
            $("a[name='selectType']").each(function () {
                $(this).removeClass("cur");
            });
            $("a[name='selectType']").eq(selectType - 1).addClass("cur");
            search(selectType);
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
                        <input type="hidden" id="hidSelAgentId" value="-2" />
                        客服：</label><input name="" id="txtName" type="text" class="w240" readonly="true" onclick="SelectVisitPerson('txtName','hidSelAgentId')" /></li>
                <li>
                    <label>
                        工号：</label><input name="" type="text" id="txtCode" class="w240" />
                </li>
                <li>
                    <label>
                        日期：</label><input id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li class="btn_cx">
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
        <div class="cxList cxList_chart" style="margin-top: 8px; height: auto;">
            <div class="table_bt">
                <div class="time_xz">
                    <a href="javascript:Change(1)" class="cur" name="selectType">日</a> || <a href="javascript:Change(2)"
                        name="selectType">周</a> || <a href="javascript:Change(3)" name="selectType">月</a><span
                            class="btn right" style="margin-top: 2px; *margin-top: -28px;">
                            <input type="button" value="导出" onclick="ExportData()" class="w60 gray" /></span></div>
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
